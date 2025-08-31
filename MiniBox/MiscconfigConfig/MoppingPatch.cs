using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.MiscconfigConfig
{
    // 擦水
    [HarmonyPatch(typeof(MopTool), "OnPrefabInit")]
    public class MoppingPatch
    {
        
        public static void Postfix()
        {
            MopTool.maxMopAmt = (SingletonOptions<ConfigurationItem>.Instance.WateringPatch ? 900000000f : 150f);
        }
    }
}
