using Klei.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TUNING;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MinionAge.Core
{
    internal class MinionDataRandomTransfer
    {
        // 随机转移特质
        public static void RandomTransferTraits(GameObject oldMinion, GameObject newMinion)
        {
            var oldTraits = oldMinion.GetComponent<Traits>();
            var newTraits = newMinion.GetComponent<Traits>();

            if (oldTraits == null || newTraits == null)
            {
                Debug.LogError("oldMinion 或 newMinion 缺少 Traits 组件，无法进行特质转移。");
                return;
            }

            if (oldTraits != null && newTraits != null)
            {
                // 筛选出可以转移的特质
                var transferableTraits = oldTraits.TraitList
                    .Where(trait => trait.Id != "MinionBaseTrait" && !newTraits.HasTrait(trait))
                    .ToList();

                // 从筛选出的特质中找到符合 GOODTRAITS 的特质
                var goodTransferableTraits = transferableTraits
                    .Where(trait => DUPLICANTSTATS.GOODTRAITS.Any(goodTrait => goodTrait.id == trait.Id))
                    .ToList();

                // 从筛选出的特质中找到符合 BADTRAITS 的特质
                var badTransferableTraits = transferableTraits
                    .Where(trait => DUPLICANTSTATS.BADTRAITS.Any(badTrait => badTrait.id == trait.Id))
                    .ToList();

                // 随机选择一个好的特质
                if (goodTransferableTraits.Count > 0)
                {
                    var randomGoodTrait = goodTransferableTraits[UnityEngine.Random.Range(0, goodTransferableTraits.Count)];
                    newTraits.Add(randomGoodTrait);
                  
                }

                // 随机选择一个坏的特质
                if (badTransferableTraits.Count > 0)
                {
                    var randomBadTrait = badTransferableTraits[UnityEngine.Random.Range(0, badTransferableTraits.Count)];
                    newTraits.Add(randomBadTrait);
                   
                }

                // 从剩余的可转移特质中随机选择额外的特质
                int traitsToAdd = Mathf.Min(UnityEngine.Random.Range(1, 3), transferableTraits.Count);
                var selectedTraits = transferableTraits
                    .Where(trait => !newTraits.HasTrait(trait)) // 排除已添加的特质
                    .OrderBy(x => UnityEngine.Random.value)
                    .Take(traitsToAdd)
                    .ToList();

                foreach (var trait in selectedTraits)
                {
                    newTraits.Add(trait);
                }
            }
        }

        public static void RandomTransferSkills(GameObject oldMinion, GameObject newMinion)
        {
            var oldResume = oldMinion.GetComponent<MinionBrainResume>();
            var newResume = newMinion.GetComponent<MinionResume>();

            if (oldResume != null && newResume != null)
            {
                // 获取 oldMinion 的所有技能并打乱顺序
                var skills = oldResume.MasteryBySkillID.Where(kvp => kvp.Value).Select(kvp => kvp.Key).ToList();
                skills.Shuffle();  // 打乱列表顺序

                // 随机生成要继承的技能数量，范围为 4 到 skills.Count 或 12 之间
                int skillsToInherit = Mathf.Clamp(Random.Range(4, 12), 0, skills.Count);

                // 确保不会越界
                for (int i = 0; i < skillsToInherit; i++)
                {
                    if (i < skills.Count && !newResume.MasteryBySkillID.ContainsKey(skills[i]))
                    {
                        newResume.MasteryBySkillID.Add(skills[i], true);
                    }
                }

                // 转移经验值
                if (newResume is MinionResume newMinionResume && oldResume is MinionBrainResume oldMinionResume)
                {
                    // 使用反射获取新 Minion 的 totalExperienceGained 字段
                    FieldInfo newExperienceField = typeof(MinionResume).GetField("totalExperienceGained", BindingFlags.NonPublic | BindingFlags.Instance);

                    if (newExperienceField != null && oldResume != null)
                    {
                        // 获取新 Minion 的当前经验值
                        float newExperience = (float)newExperienceField.GetValue(newMinionResume);

                        // 将旧 Minion 的经验值添加到新 Minion 的经验值中
                        newExperience += CalculateExperienceForSkills(skillsToInherit);

                        // 更新新 Minion 的经验值
                        newExperienceField.SetValue(newMinionResume, newExperience);
                    }
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

        public static void RoundTransferAttributes(GameObject oldMinion, GameObject newMinion)
        {
            var oldAttributes = oldMinion.GetComponent<AttributeLevels>();
            var newAttributes = newMinion.GetComponent<AttributeLevels>();

            if (oldAttributes != null && newAttributes != null)
            {
                // 手动创建属性列表
                List<AttributeLevel> allOldAttributes = new List<AttributeLevel>();
                foreach (var attribute in oldAttributes)
                {
                    allOldAttributes.Add(attribute);
                }
                var positiveAttributes = allOldAttributes.Where(attr => attr.GetLevel() > 0).ToList();
                var selectedPositiveAttributes = positiveAttributes.OrderBy(x => Random.value).Take(Random.Range(2, 4)).ToList();
                foreach (var attribute in selectedPositiveAttributes)
                {
                    TransferAttribute(attribute, newAttributes);
                }
            }
            else
            {
                Debug.LogWarning("新对象上未找到 AttributeLevels 组件.");
            }
        }

        // 用于传递属性的方法
        private static void TransferAttribute(AttributeLevel oldAttribute, AttributeLevels newAttributes)
        {
            string attributeId = oldAttribute.attribute.Attribute.Id;
            string attributename = oldAttribute.attribute.Attribute.Name;
            int oldLevel = oldAttribute.GetLevel();
            float oldExperience = oldAttribute.experience;

            var newAttribute = newAttributes.GetAttributeLevel(attributeId);
            if (newAttribute != null)
            {
                int newLevel = newAttribute.GetLevel() + oldLevel;
                float newExperience = newAttribute.experience + oldExperience;

                if (newLevel > 50)
                {
                  
                    newLevel = 50;  
                }

                newAttributes.SetLevel(attributeId, newLevel);
                newAttributes.SetExperience(attributeId, newExperience);
            
            }
            else
            {
                // 如果新属性不存在，则直接设置旧的等级和经验
                int newLevel = oldLevel > 50 ? 50 : oldLevel;
                newAttributes.SetLevel(attributeId, newLevel);
                newAttributes.SetExperience(attributeId, oldExperience);
            }

        }



      
    }
}
