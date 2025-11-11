using System;
using HarmonyLib;
using PeterHan.PLib.Options;

namespace WireAnywhere
{
    // 补丁类，修改 WireHighWattage 建筑的默认设置
    [HarmonyPatch(typeof(WireHighWattageConfig), "CreateBuildingDef")]
    public class WireHighWattagePatch
    {
        // 在创建建筑定义后修改其参数
        public static void Postfix(BuildingDef __result)
        {
            // 设置建筑物的位置规则为Anywhere
            __result.BuildLocationRule = BuildLocationRule.Anywhere;

            // 禁用对基础的持续检查
            __result.ContinuouslyCheckFoundation = false;

            // 设置建筑物所在图层
            __result.ObjectLayer = ObjectLayer.Wire;

            // 设置装饰基准和装饰半径
            __result.BaseDecor = SingletonOptions<Config>.Instance.WireHighWattage_BaseDecor;
            __result.BaseDecorRadius = SingletonOptions<Config>.Instance.WireHighWattage_BaseDecorRadius;
        }
    }
}
