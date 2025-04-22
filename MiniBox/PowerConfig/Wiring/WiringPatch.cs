using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.PowerConfig.Wiring
{
    [HarmonyPatch(typeof(Wire), "GetMaxWattageAsFloat")]
    internal class WiringPatch
    {
        public static void Postfix(Wire __instance, ref float __result, Wire.WattageRating rating)
        {
            switch (rating)
            {
                case Wire.WattageRating.Max500:
                    __result = 500f;
                    break;
                case Wire.WattageRating.Max1000:
                    __result = SingletonOptions<ConfigurationItem>.Instance.Wires * 1000f; 
                    break;
                case Wire.WattageRating.Max2000:
                    __result = SingletonOptions<ConfigurationItem>.Instance.Conductors * 1000f;
                    break;
                case Wire.WattageRating.Max20000:
                    __result = SingletonOptions<ConfigurationItem>.Instance.HighLoadWires * 1000f;
                    break;
                case Wire.WattageRating.Max50000:
                    __result = SingletonOptions<ConfigurationItem>.Instance.HighLoadConductors * 1000f;
                    break;
                default:
                    __result = 0f;
                    break;
            }
        }
    }
}
