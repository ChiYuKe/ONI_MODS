using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.PowerTransformer
{
   
    // 大变压器
    [HarmonyPatch(typeof(PowerTransformerConfig), "CreateBuildingDef")]
    public class PowerTransformerPatch
    {
       
        public static void Postfix(ref BuildingDef __result)
        {
            bool HeatGeneration = SingletonOptions<ConfigurationItem>.Instance.PowerTransformerHeatGeneration;
            __result.ExhaustKilowattsWhenActive = (HeatGeneration ? 0.25f : 0f);
            __result.SelfHeatKilowattsWhenActive = (HeatGeneration ? 1f : 0f);
            
        }
    }
}
