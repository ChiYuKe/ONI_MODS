using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalDecay.Content.Comps;
using EternalDecay.Content.Configs;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace EternalDecay.Content.Core
{
    // 生成一个大脑并将复制人数据传入大脑
    internal class MinionDataTransfer
    {
        public static void GenerateNewObject(GameObject oldMinion, Vector3 position)
        {
            GameObject prefab = Assets.GetPrefab(new Tag("KmodMiniBrain"));
            if (prefab == null)
            {
                Debug.LogError("未找到 KmodMiniBrainCore 预制件.");
                return;
            }

            GameObject newMinion = GameUtil.KInstantiate(prefab, position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
            if (newMinion == null)
            {
                Debug.LogError("无法实例化大脑对象.");
                return;
            }

            newMinion.SetActive(true);

          
            TransferAttributesAndSkills(oldMinion, newMinion);
            SetNewMinionName(oldMinion, newMinion);
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
            var oldTraits = oldMinion?.GetComponent<Traits>();
            var newTraits = newMinion?.GetComponent<Traits>();

            if (oldTraits == null || newTraits == null) return;

            int traitsAdded = 0;
            var maxTraits = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.TRAITSMAXAMOUNT;

            foreach (var trait in oldTraits.TraitList.Where(t => t.Id != "MinionBaseTrait" && !newTraits.HasTrait(t)))
            {
                if (traitsAdded >= maxTraits)
                {
                    Debug.LogWarning($"{newMinion.name} 已经继承了 {maxTraits} 条特质，无法继承更多特质。");
                    break;
                }

                newTraits.Add(trait);
                traitsAdded++;
            }
        }


        // 转移技能
        public static void TransferSkills(GameObject oldMinion, GameObject newMinion)
        {
            var oldResume = oldMinion.GetComponent<MinionResume>();
            var newResume = newMinion.GetComponent<MinionBrainResume>();

            if (oldResume != null && newResume != null)
            {
                int skillsAdded = 0;

                foreach (var kvp in oldResume.MasteryBySkillID)
                {
                    if (skillsAdded >= TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.SKILLMAXAMOUNT)
                    {
                        break;
                    }

                    if (kvp.Value && !newResume.MasteryBySkillID.ContainsKey(kvp.Key))
                    {
                        newResume.MasteryBySkillID.Add(kvp.Key, true);
                        skillsAdded++;
                    }
                }

                if (newResume is MinionBrainResume newMinionResume && oldResume is MinionResume oldMinionResume)
                {
                    float experienceForSkills = CalculateExperienceForSkills(skillsAdded);
                    newMinionResume.TotalExperienceGained += oldMinionResume.TotalExperienceGained + experienceForSkills;
                }
            }

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

                        if (newLevel > TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL)
                        {
                            newLevel = TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.ATTRIBUTEMAXLEVEL;
                         
                        }

                        newAttributes.SetLevel(attributeId, newLevel);
                        newAttributes.SetExperience(attributeId, newExperience);
                    }
                    else
                    {
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


        // 计算技能点所需的经验值
        private static float CalculateExperienceForSkills(int skillsAdded)
        {
            return Mathf.Pow((skillsAdded / (float)SKILLS.TARGET_SKILLS_EARNED), SKILLS.EXPERIENCE_LEVEL_POWER) * SKILLS.TARGET_SKILLS_CYCLE * 600f;
        }

        // 设置新大脑的名字
        private static void SetNewMinionName(GameObject oldMinion, GameObject newMinion)
        {
            var oldName = oldMinion.GetComponent<KSelectable>().GetName();
            var newNameable = newMinion.AddOrGet<UserNameable>();
            newNameable.SetName(oldName + Configs.STRINGS.MISC.NEWMINIONNAME.NAME);
        }

    }
}
