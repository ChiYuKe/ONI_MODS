using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.PowerTransformerSmall
{
    // 小变压器
    [HarmonyPatch(typeof(PowerTransformerSmallConfig), "CreateBuildingDef")]
    public class PowerTransformerSmallPatch
    {
        
        public static void Postfix(ref BuildingDef __result)
        {
            bool HeatGeneration = SingletonOptions<ConfigurationItem>.Instance.PowerTransformerSmallHeatGeneration;
            __result.ExhaustKilowattsWhenActive = (HeatGeneration ? 0.25f : 0f);
            __result.SelfHeatKilowattsWhenActive = (HeatGeneration ? 1f : 0f);
        }
    }
}
