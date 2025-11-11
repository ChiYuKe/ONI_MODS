using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.LiquidReservoir
{
    // 液库
    [HarmonyPatch(typeof(LiquidReservoirConfig), "ConfigureBuildingTemplate")]
    public class LiquidReservoir_Storage_Config
    {
      
        public static void Postfix(LiquidReservoirConfig __instance, ref GameObject go)
        {
            Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
            storage.capacityKg = SingletonOptions<ConfigurationItem>.Instance.LiquidReservoirCapacityKg;
            storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            storage.allowUIItemRemoval = true;
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.capacityKG = storage.capacityKg;
        }
    }

    [HarmonyPatch(typeof(LiquidReservoirConfig), "CreateBuildingDef")]
    public class LiquidReservoirPatch
    {

        public static void Postfix(LiquidReservoirConfig __instance, ref BuildingDef __result)
        {
            __result.PermittedRotations = PermittedRotations.R360;
            __result.BuildLocationRule = BuildLocationRule.Anywhere;
            __result.ContinuouslyCheckFoundation = SingletonOptions<ConfigurationItem>.Instance.LiquidReservoirFoundation;
            __result.Overheatable = SingletonOptions<ConfigurationItem>.Instance.LiquidReservoirOverheatable;
        }
    }
}
