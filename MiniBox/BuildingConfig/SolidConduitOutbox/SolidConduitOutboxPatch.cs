using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.SolidConduitOutbox
{
    // 运输存放器
    [HarmonyPatch(typeof(SolidConduitOutboxConfig), "ConfigureBuildingTemplate")]
    public class SolidConduitOutboxPatch
    {
        private static void Postfix(SolidConduitOutboxConfig __instance, ref GameObject go)
        {
            float storage_ = SingletonOptions<ConfigurationItem>.Instance.SolidConduitOutboxCapacityKg;
            Storage storage = BuildingTemplates.CreateDefaultStorage(go, false);
            storage.capacityKg = storage_;
            go.AddOrGet<SolidConduitConsumer>().capacityKG = storage_;
            
        }
    }
}
