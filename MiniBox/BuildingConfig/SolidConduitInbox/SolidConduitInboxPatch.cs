using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.SolidConduitInbox
{
    // 运输装载器
    [HarmonyPatch(typeof(SolidConduitInboxConfig), "DoPostConfigureComplete")]
    public class SolidConduitInbox
    {
       
        public static void Postfix(SolidConduitInboxConfig __instance, ref GameObject go)
        {
            float storage_ = SingletonOptions<ConfigurationItem>.Instance.SolidConduitInboxCapacityKg;
            Storage storage = go.AddOrGet<Storage>();
            storage.capacityKg = storage_;
        }
    }
}
