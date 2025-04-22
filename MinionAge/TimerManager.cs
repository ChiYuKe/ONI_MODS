using Database;
using Epic.OnlineServices.Logging;
using Klei.AI;
using KModTool;
using STRINGS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TUNING;
using UnityEngine;
using static STRINGS.UI.UISIDESCREENS.AUTOPLUMBERSIDESCREEN.BUTTONS;

namespace DebuffRoulette
{
    public static class RandomDebuffTimerManager
    {

        // 定时器配置
        private static readonly float TimerInterval = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TIMERINTERVAL; 
        private static float nextExecutionTime = 0f; // 下次执行时间
        private static float lastUpdateTime = 0f; // 上次更新的时间

        // 复制人对象相关
        private static HashSet<GameObject> cachedMinionGameObjects = new HashSet<GameObject>(); // 缓存的复制人对象集合
        private static HashSet<GameObject> deadMinions = new HashSet<GameObject>(); // 已死亡的复制人集合
        private static int cachedMinionCount = 0; // 缓存的复制人数量

        // 复制人年龄相关
        public static float MinionAgeThreshold = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.MINIONAGETHRESHOLD; // 复制人年龄阈值（单位：周期）
        private static float AgeThreshold = MinionAgeThreshold * 600f; // 年龄阈值（秒）
        private static float Age80PercentThreshold = AgeThreshold * TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.AGE80PERCENTTHRESHOLD; // 年龄80%阈值
        public static float DebuffTimeThreshold = AgeThreshold - Age80PercentThreshold; // 衰老效果阈值

        // 初始化定时器
        public static void InitializeTimer()
        {
            nextExecutionTime = Time.time + TimerInterval;
            lastUpdateTime = Time.time;
        }

        // 每帧调用更新逻辑
        public static void Update()
        {
            // 获取并更新缓存的复制人对象
            List<GameObject> currentMinions = KModMinionUtils.GetAllMinionGameObjects();

            int count = currentMinions.Count;

            // 如果缓存的复制人数量发生变化，则更新缓存
            if (count != cachedMinionCount)
            {
                UpdateMinionCache(currentMinions);
            }
           

            // 每秒执行一次更新逻辑
            if (Time.time - lastUpdateTime >= 1f)
            {
                ProcessMinionObjects();
                lastUpdateTime = Time.time;
            }
            // 定时执行任务
            if (Time.time >= nextExecutionTime)
            {
                Synchronous();
                // ExecutePeriodicTask();
                nextExecutionTime = Time.time + TimerInterval;
            }
        }
       
        // 更新复制人对象缓存
        private static void UpdateMinionCache(List<GameObject> currentMinions)
        {
            // 创建新集合，并移除死亡或无效对象
            var validMinions = currentMinions
                .Where(obj => obj != null && !deadMinions.Contains(obj))
                .ToHashSet();

            // 更新缓存
            cachedMinionGameObjects.RemoveWhere(obj => !validMinions.Contains(obj));
            cachedMinionGameObjects.UnionWith(validMinions);
            cachedMinionCount = cachedMinionGameObjects.Count;
           
        }

       
        // 处理缓存中的复制人对象
        private static void ProcessMinionObjects()
        {
            foreach (GameObject minion in cachedMinionGameObjects.ToList())
            {
                if (minion == null || deadMinions.Contains(minion)) continue;
             
                
                var ageInstance = Db.Get().Amounts.Age.Lookup(minion);
                if (ageInstance == null) continue;

                
                float currentAgeInSeconds = ageInstance.value * 600;
             
                //死亡
                if (currentAgeInSeconds >= AgeThreshold)
                {
                    HandleDeath(minion);
                    Ownable.objectTimeRecords.RemoveAll(record => record.GameObject == minion);
                    CameraController.Instance.CameraGoTo(minion.transform.position, 1f, false);
                }
                // 衰老
                if (currentAgeInSeconds >= Age80PercentThreshold)
                {
                    // 切换为疲倦 (Uncomfortable)Tired
                    KModMiscellaneous.SetExpression(minion, Db.Get().Expressions.Tired);
                    KModDeBuff.ApplyDebuff(minion);
                }
            }
        }

        // 处理复制人对象的死亡
        private static void HandleDeath(GameObject minion)
        {
            var deathMonitor = minion.GetSMI<DeathMonitor.Instance>();
            if (deathMonitor != null)
            {
                minion.AddOrGet<KPrefabID>().AddTag(new Tag("KModNoMourning"),true);
                var customDeathConfig = AddNewDeathPatch.customDeath;
                deathMonitor.Kill(customDeathConfig);

                // 更新缓存和状态
                cachedMinionGameObjects.Remove(minion);
                deadMinions.Add(minion);

                // 延迟执行操作，以确保在处理完死亡逻辑后更新复制人缓存和移除无效的游戏对象
                KModDelayedActionExecutor.Instance.ExecuteAfterDelay(0.1f, () =>
                {                
                    UpdateMinionCache(KModMinionUtils.GetAllMinionGameObjects());
                    RemoveInvalidGameObjects();
                });
                // 延迟 2 秒后执行以下操作，以确保在处理完死亡逻辑后生成新的对象
                KModDelayedActionExecutor.Instance.ExecuteAfterDelay(2f, () =>
                {
                    if (minion != null)
                    {
                        GenerateNewObject(minion, minion.transform.position);
                    }
                });
            }
            else
            {
                Debug.LogWarning("DeathMonitor.Instance 为空，无法执行死亡操作");
            }
        }

        // 生成新的复制人对象
        private static void GenerateNewObject(GameObject oldMinion, Vector3 position)
        {
            GameObject prefab = Assets.GetPrefab(new Tag("KmodMiniBrainCore"));
            if (prefab == null)
            {
                Debug.LogError("未找到 KmodMiniBrainCore 预制件.");
                return;
            }

            GameObject newMinion = GameUtil.KInstantiate(prefab, position, Grid.SceneLayer.Ore, null, 0);
            if (newMinion == null)
            {
                Debug.LogError("无法实例化新的复制人对象.");
                return;
            }

            newMinion.SetActive(true);

            KModDelayedActionExecutor.Instance.ExecuteAfterDelay(0.1f, () =>
            {
                TransferAttributesAndSkills(oldMinion, newMinion);
                SetNewMinionName(oldMinion, newMinion);
            });
        }



        // 转移旧对象的特质、技能和属性到新对象
        private static void TransferAttributesAndSkills(GameObject oldMinion, GameObject newMinion)
        {
            TransferTraits(oldMinion, newMinion);
            TransferSkills(oldMinion, newMinion);
            TransferAttributes(oldMinion, newMinion);
        }

        // 转移特质
        public static void TransferTraits(GameObject oldMinion, GameObject newMinion)
        {
            var oldTraits = oldMinion.GetComponent<Traits>();
            var newTraits = newMinion.GetComponent<Traits>();

            if (oldTraits != null && newTraits != null)
            {
                int traitsAdded = 0;  // 记录已经添加的特质数量

                foreach (var trait in oldTraits.TraitList)
                {
                    // 检查是否已经添加了12条特质
                    if (traitsAdded >= TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.TRAITSMAXAMOUNT)
                    {
                        Debug.LogWarning($"{newMinion.name} 已经继承了12条特质，无法继承更多特质。");
                        break;
                    }

                    // 过滤掉 MinionBaseTrait 并确保 newMinion 不已有该特质
                    if (trait.Id != "MinionBaseTrait" && !newTraits.HasTrait(trait))
                    {
                        // 添加特质
                        newTraits.Add(trait);
                        traitsAdded++;  // 更新已添加的特质数量
                        
                    }
                }
            }
        }


        // 转移技能
        public static void TransferSkills(GameObject oldMinion, GameObject newMinion)
        {
            var oldResume = oldMinion.GetComponent<MinionResume>();
            var newResume = newMinion.GetComponent<MinionBrainResume>();

            if (oldResume != null && newResume != null)
            {
                int skillsAdded = 0;  // 记录已经添加的技能数量

                foreach (var kvp in oldResume.MasteryBySkillID)
                {
                    // 检查是否已经添加了17项技能
                    if (skillsAdded >= TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.SKILLMAXAMOUNT)
                    {
                        Debug.Log($"{newMinion.name} 已经继承了12项技能，无法继承更多技能。");
                        break;
                    }

                    // 如果技能尚未被继承，则添加该技能
                    if (kvp.Value && !newResume.MasteryBySkillID.ContainsKey(kvp.Key))
                    {
                        newResume.MasteryBySkillID.Add(kvp.Key, true);
                        skillsAdded++;  // 更新已添加的技能数量

                    }
                }

                // 转移经验值
                if (newResume is MinionBrainResume newMinionResume && oldResume is MinionResume oldMinionResume)
                {
                    // 计算当前技能点所需的经验值
                    float experienceForSkills = CalculateExperienceForSkills(skillsAdded);

                    newMinionResume.TotalExperienceGained += oldMinionResume.TotalExperienceGained + experienceForSkills;
                   
                }
            }
        }
        // 计算技能点所需的经验值的函数
        private static float CalculateExperienceForSkills(int skillsAdded)
        {
            float experience = 0;
            // 反向计算
            experience = Mathf.Pow((skillsAdded / (float)SKILLS.TARGET_SKILLS_EARNED), SKILLS.EXPERIENCE_LEVEL_POWER) * SKILLS.TARGET_SKILLS_CYCLE * 600f;
            return experience;
        }


        // 转移属性
        public static void TransferAttributes(GameObject oldMinion, GameObject newMinion)
        {
            var oldAttributes = oldMinion.GetComponent<AttributeLevels>();
            var newAttributes = newMinion.GetComponent<AttributeLevels>();

            if (oldAttributes != null && newAttributes != null)
            {
                foreach (var oldAttribute in oldAttributes)
                {
                    string attributeId = oldAttribute.attribute.Attribute.Id;
                    int oldLevel = oldAttribute.GetLevel();
                    float oldExperience = oldAttribute.experience;

                    var newAttribute = newAttributes.GetAttributeLevel(attributeId);
                    if (newAttribute != null)
                    {
                        int newLevel = newAttribute.GetLevel() + oldLevel;
                        float newExperience = newAttribute.experience + oldExperience;

                     
                        // 如果新等级超过99，则将其限制在99
                        if (newLevel > TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL)
                        {
                            newLevel = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL;
                            Debug.LogWarning($"{newMinion.name} 的属性 {attributeId} 已达到99级，无法进一步提升。");
                        }

                        newAttributes.SetLevel(attributeId, newLevel);
                        newAttributes.SetExperience(attributeId, newExperience);
                    }
                    else
                    {
                        // 如果新属性不存在，则直接设置旧的等级和经验
                        int newLevel = oldLevel > TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL ? TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL : oldLevel;
                        newAttributes.SetLevel(attributeId, newLevel);
                        newAttributes.SetExperience(attributeId, oldExperience);
                    }

                   
                }
            }
            else
            {
                Debug.LogWarning("新对象上未找到 AttributeLevels 组件.");
            }
        }


        // 设置新大脑的名字
        private static void SetNewMinionName(GameObject oldMinion, GameObject newMinion)
        {
            var oldName = oldMinion.GetComponent<KSelectable>().GetName();
            var newNameable = newMinion.AddOrGet<UserNameable>();
            newNameable.SetName(oldName + STRINGS.MISC.NEWMINIONNAME.NAME);
        }

        // 执行定时任务
        private static void ExecutePeriodicTask()
        {
            KModDeBuff.ApplyRandomDebuff(cachedMinionGameObjects);
        }
        private static void Synchronous() 
        {
            foreach (GameObject minion in cachedMinionGameObjects.ToList())
            {
                MinionDataSaver.RemoveNonMatchingData(minion);
                var ageInstance = Db.Get().Amounts.Age.Lookup(minion);
                if (ageInstance == null) continue;

                float GBcurrentAgeInSeconds = ageInstance.value * 600;
              
                // 判断当前年龄与保存的年龄误差是否达到目标值
                if (MinionDataSaver.IsAgeDifferenceExceedingThreshold(minion, GBcurrentAgeInSeconds, 600f))
                {
                    //同步文件中的年龄
                    ageInstance.SetValue(MinionDataSaver.GetAgeByInstance(minion));
                    float GBcurrentAgeInSeconds_ = ageInstance.value * 600;
                    MinionDataSaver.SaveMinionData(minion, GBcurrentAgeInSeconds_);
                    Debug.Log($"当前有误差，进行同步  {GBcurrentAgeInSeconds_}");
                }
                else 
                {
                    MinionDataSaver.SaveMinionData(minion, GBcurrentAgeInSeconds);
                }
                
            }

        }

        // 移除无效的 GameObject
        private static void RemoveInvalidGameObjects()
        {
            cachedMinionGameObjects.RemoveWhere(obj => obj == null);
            if (cachedMinionGameObjects.Count < cachedMinionCount)    
            {
                Debug.LogError("发现无效的 GameObject 在缓存中");
                cachedMinionCount = cachedMinionGameObjects.Count;
            }
        }
    }
}
