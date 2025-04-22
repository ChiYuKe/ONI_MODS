using Klei.AI;
using KModTool;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace MinionAge.Core
{
    public static class MinionDataTransfer
    {
        // 生成新的大脑对象
        public static void GenerateNewObject(GameObject oldMinion, Vector3 position)
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
                Debug.LogError("无法实例化大脑对象.");
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
                int traitsAdded = 0;

                foreach (var trait in oldTraits.TraitList)
                {
                    if (traitsAdded >= TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.TRAITSMAXAMOUNT)
                    {
                        Debug.LogWarning($"{newMinion.name} 已经继承了12条特质，无法继承更多特质。");
                        break;
                    }

                    if (trait.Id != "MinionBaseTrait" && !newTraits.HasTrait(trait))
                    {
                        newTraits.Add(trait);
                        traitsAdded++;
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
                int skillsAdded = 0;

                foreach (var kvp in oldResume.MasteryBySkillID)
                {
                    if (skillsAdded >= TUNINGS.TIMERMANAGER.RANDOMDEBUFFTIMERMANAGER.TRANSFER.SKILLMAXAMOUNT)
                    {
                        Debug.Log($"{newMinion.name} 已经继承了12项技能，无法继承更多技能。");
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

        // 计算技能点所需的经验值
        private static float CalculateExperienceForSkills(int skillsAdded)
        {
            return Mathf.Pow((skillsAdded / (float)SKILLS.TARGET_SKILLS_EARNED), SKILLS.EXPERIENCE_LEVEL_POWER) * SKILLS.TARGET_SKILLS_CYCLE * 600f;
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
                            Debug.LogWarning($"{newMinion.name} 的属性 {attributeId} 已达到99级，无法进一步提升。");
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

        // 设置新大脑的名字
        private static void SetNewMinionName(GameObject oldMinion, GameObject newMinion)
        {
            var oldName = oldMinion.GetComponent<KSelectable>().GetName();
            var newNameable = newMinion.AddOrGet<UserNameable>();
            newNameable.SetName(oldName + STRINGS.MISC.NEWMINIONNAME.NAME);
        }
    }
}