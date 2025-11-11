using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.GasReservoir
{
    // 气库
    [HarmonyPatch(typeof(GasReservoirConfig), "ConfigureBuildingTemplate")]
    public class GasReservoirPatch
    {
        
        public static void Postfix(GasReservoirConfig __instance, ref GameObject go)
        {
            Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
            storage.capacityKg = SingletonOptions<ConfigurationItem>.Instance.GasReservoirCapacityKg;
            storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
            storage.allowUIItemRemoval = true;
            ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
            conduitConsumer.capacityKG = storage.capacityKg;
        }
    }
    [HarmonyPatch(typeof(GasReservoirConfig), "CreateBuildingDef")]
    public class GasReservoir_Attributes_Config
    {

        public static void Postfix(GasReservoirConfig __instance, ref BuildingDef __result)
        {
            __result.PermittedRotations = PermittedRotations.R360;
            __result.BuildLocationRule = BuildLocationRule.Anywhere;
            __result.ContinuouslyCheckFoundation = SingletonOptions<ConfigurationItem>.Instance.GasReservoirFoundation;
            __result.Overheatable = SingletonOptions<ConfigurationItem>.Instance.GasReservoirOverheatable;
        }


            
        
    }
}
