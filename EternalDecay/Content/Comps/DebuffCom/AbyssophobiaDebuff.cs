using CykUtils;
using EternalDecay.Content.Configs;
using EternalDecay.Content.Core;
using Klei.AI;
using UnityEngine;

namespace EternalDecay.Content.Comps.DebuffCom
{
    public class AbyssophobiaDebuff
    {
        // 场景中建筑的 ID（优先使用第一个 ID）
        private static readonly string buildingID = "HeadquartersComplete"; //打印舱
        private static readonly string buildingID2 = "ExobaseHeadquartersComplete"; //迷你基地打印舱

        private static GameObject building;  // 场景中的某个建筑对象

        // debuff 的基础值、增幅值和阈值
        public float debuffBaseValue = 1f;
        public float debuffMultiplier = 1f;

        // 设定每个 Y 坐标差异区间与对应的 debuff 等级
        private static readonly (float threshold, int level)[] debuffThresholds = new (float, int)[]
        {
            (5f, 0),    
            (25f, 1),   
            (45f, 2),   
            (65f, 3),   
            (85f, 4),   
            (105f, 5),  
        };

        private static string[] abyssophobiaEffects = new string[]
        {
            "EternalDecay_Abyssophobia_0",
            "EternalDecay_Abyssophobia_1",
            "EternalDecay_Abyssophobia_2",
            "EternalDecay_Abyssophobia_3",
            "EternalDecay_Abyssophobia_4",
            "EternalDecay_Abyssophobia_5",
        };


        private int debuffLevel = 0;  // 当前的 debuff 级别



        public static void TriggerScan(GameObject minion) 
        {
            // 确保 minion 对象存在
            if (minion == null)
            {
                Debug.LogError("Minion 对象不能为空！");
                return;
            }

            // 获取 minion 的 KPrefabID 组件
            var prefabID = minion.GetComponent<KPrefabID>();
            if (prefabID == null || !prefabID.HasTag(KGameTags.CreatureAbyssophobia)) 
            {
                return;
            }

            
            InitializeBuilding();
            if (building == null) return;

            float targetY = minion.transform.position.y;
            float buildingY = building.transform.position.y;

            // 计算 Y 坐标差值（不取绝对值）
            float yDifference = buildingY - targetY;



            // 通过差异找到对应的 debuff 级别
            int debuffLevel = GetDebuffLevel(yDifference);




            if (prefabID.HasTag(GameTags.Corpse))
            {
                KEffects.RemoveBuff(minion, abyssophobiaEffects[debuffLevel]);
                return; 
            }

  
            // 应用 debuff
            ApplyAbyssophobiaDebuff(minion,debuffLevel);
        }


       
        private static void InitializeBuilding()
        {
            building = GetBuildingByID(buildingID) ?? GetBuildingByID(buildingID2);
            //if (building == null)
            //{
                
            //}
        }



        // 通过建筑 ID 查找建筑对象
        private static GameObject GetBuildingByID(string buildingID)
        {
            return GameObject.Find(buildingID);  
        }

        // 计算 debuff 级别
        private static int GetDebuffLevel(float yDifference)
        {
            // 遍历预设的阈值和级别
            foreach (var (threshold, level) in debuffThresholds)
            {
                if (yDifference <= threshold)
                {
                    return level;
                }
            }

            // 如果差异大于最大阈值，返回最高级别 5
            return 5;
        }

        // 应用 Abyssophobia debuff



        private static int currentLevel = -1;
        private static bool hasNotified = false; // 记录是否已经通知过

        private static void ApplyAbyssophobiaDebuff(GameObject minion, int newLevel)
        {
            if (minion == null) return;
            if (newLevel == currentLevel) return; // 等级没变，不更新

            var effects = minion.GetComponent<Effects>();
            if (effects == null) return;

            // 移除旧 Buff
            if (currentLevel >= 0 && currentLevel < abyssophobiaEffects.Length)
            {
                string oldBuff = abyssophobiaEffects[currentLevel];
                if (effects.HasEffect(oldBuff))
                    effects.Remove(oldBuff);
            }

            // 添加新 Buff
            if (newLevel >= 0 && newLevel < abyssophobiaEffects.Length)
            {
                string newBuff = abyssophobiaEffects[newLevel];
                if (!effects.HasEffect(newBuff))
                    effects.Add(newBuff, true);
            }

            // ✅ 通知逻辑：仅首次触发执行一次
            if (!hasNotified)
            {
                NotifyAbyssophobia(minion);
                hasNotified = true;
            }

            currentLevel = newLevel;
           // LogUtil.Log($"[{gameObject.name}] 深渊恐惧症 → 更新到 {newLevel} ({abyssophobiaEffects[newLevel]})");
        }



        private static void NotifyAbyssophobia(GameObject gameObject)
        {
            Notifier notifier = gameObject.AddOrGet<Notifier>();
            Notification notification = new Notification(
                Configs.STRINGS.MISC.NOTIFICATIONS.DEBUFFINFO.IMMUNERESPONSE.NAME, // 通知标题
                NotificationType.Bad, // 通知类型
                (notificationList, data) => Configs.STRINGS.MISC.NOTIFICATIONS.DEBUFFINFO.TOOLTIP + notificationList.ReduceMessages(false), // 通知处理函数
                "/t•" + gameObject.GetProperName(), // 通知内容

                true, 0f, null, null, null, true, false, false
            );
            notifier.Add(notification, ""); // 添加通知
        }



    }
}
