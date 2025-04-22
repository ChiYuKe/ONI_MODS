using HarmonyLib;
using UnityEngine;

namespace MinionAge_DLC
{
    internal class Patch
    {
        [HarmonyPatch(typeof(BaseMinionConfig))]
        public static class AddMinionAmountsPatch
        {

            // 给复制人添加组件确保可以应用效果
            [HarmonyPostfix]
            [HarmonyPatch("BaseMinion")]
            public static void Postfix_BaseMinion(GameObject __result)
            {
                __result.AddComponent<SpecializationTrigger>();

            }
        }
    }
}
