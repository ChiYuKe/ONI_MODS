using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static MiniBox.Config;

namespace MiniBox.BuildingConfig.StorageLocker
{
    //储物箱
    internal class StorageLockerPatch
    {
        [HarmonyPatch(typeof(StorageLockerConfig), "DoPostConfigureComplete")]
        public class StorageBox_Storage_Config
        {
            
            private static void Postfix(GameObject go)
            {
                go.AddOrGet<Storage>().capacityKg = SingletonOptions<ConfigurationItem>.Instance.StorageLockerCapacityKg;
            }
        }
    }
}
