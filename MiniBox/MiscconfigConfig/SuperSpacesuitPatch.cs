using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;
using TUNING;

namespace MiniBox.MiscconfigConfig
{
    [HarmonyPatch(typeof(Db), "Initialize")]
    internal class TheInstanceheats
    {
      
        private static void Prefix(Db __instance)
        {
            bool super_atmosuit = SingletonOptions<ConfigurationItem>.Instance.super_atmosuit;
            EQUIPMENT.SUITS.ATMOSUIT_DIGGING = (super_atmosuit ? 200 : 10);
            EQUIPMENT.SUITS.ATMOSUIT_INSULATION = (super_atmosuit ? 3000 : 50);
            EQUIPMENT.SUITS.ATMOSUIT_SCALDING = (super_atmosuit ? 6000 : 1000);
            EQUIPMENT.SUITS.ATMOSUIT_ATHLETICS = (super_atmosuit ? 100 : (-6));
            EQUIPMENT.SUITS.ATMOSUIT_THERMAL_CONDUCTIVITY_BARRIER = (super_atmosuit ? 10f : 0.2f);
        }
    }
}
