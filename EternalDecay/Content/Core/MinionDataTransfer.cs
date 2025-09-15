using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalDecay.Content.Configs;
using Klei.AI;
using TUNING;
using UnityEngine;

namespace EternalDecay.Content.Core
{
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

        }

        // 计算技能点所需的经验值
        private static float CalculateExperienceForSkills(int skillsAdded)
        {
            return Mathf.Pow((skillsAdded / (float)SKILLS.TARGET_SKILLS_EARNED), SKILLS.EXPERIENCE_LEVEL_POWER) * SKILLS.TARGET_SKILLS_CYCLE * 600f;
        }

        // 转移属性
        public static void TransferAttributes(GameObject oldMinion, GameObject newMinion)
        {

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
