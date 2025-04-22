using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.MineralDeoxidizer
{
    //电解器
    [HarmonyPatch(typeof(MineralDeoxidizerConfig), "ConfigureBuildingTemplate")]
    internal class MineralDeoxidizerPatch
    {
        private static void Postfix(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<ElementConverter>().outputElements = new ElementConverter.OutputElement[]
            {
                new ElementConverter.OutputElement(SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerEmissionValues, SimHashes.Oxygen, SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerOutputTemperature + 273.15f, false, false, 0f, 1f, 1f, byte.MaxValue, 0, true)
            };
            Prioritizable.AddRef(go);
        }
    }

    [HarmonyPatch(typeof(MineralDeoxidizerConfig), "CreateBuildingDef")]
    public class MineralDeoxidizerPatch_
    {
        
        public static void Postfix(ref BuildingDef __result)
        {
            __result.EnergyConsumptionWhenActive = SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerPowerConsumption;
            __result.Floodable = SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerFloodable;
            __result.Overheatable = SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerOverheatable;
            bool HeatGeneration = SingletonOptions<ConfigurationItem>.Instance.MineralDeoxidizerHeatGeneration;
            __result.ExhaustKilowattsWhenActive = (HeatGeneration ? 0.5f : 0f);
            __result.SelfHeatKilowattsWhenActive = (HeatGeneration ? 1f : 0f);
        }
    }
}
