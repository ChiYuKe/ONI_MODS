using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.Refrigerator
{
    //冰箱
    [HarmonyPatch(typeof(RefrigeratorConfig), "CreateBuildingDef")]
    internal class RefrigeratorPatch
    {
        private static void Postfix(ref BuildingDef __result)
        {
            __result.EnergyConsumptionWhenActive = SingletonOptions<ConfigurationItem>.Instance.RefrigeratorEmissionValues;
            __result.Floodable = SingletonOptions<ConfigurationItem>.Instance.RefrigeratorFloodable;
            __result.Overheatable = SingletonOptions<ConfigurationItem>.Instance.RefrigeratorOverheatable;

        }
    }

    [HarmonyPatch(typeof(RefrigeratorConfig), "DoPostConfigureComplete")]
    internal class RefrigeratorPatch_
    {
        private static void Postfix(GameObject go)
        {
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = SingletonOptions<ConfigurationItem>.Instance.RefrigeratorCapacityKg;
        }
    }
}
