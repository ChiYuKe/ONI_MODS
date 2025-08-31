using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.TransportRouteConfig.TransportTrack
{
    // 运输轨道容量
    [HarmonyPatch(typeof(SolidConduitDispenser), "ConduitUpdate")]
    public class TransportTrackPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = new List<CodeInstruction>(instructions);
            for (int i = 0; i < list.Count; i++)
            {
                bool flag = list[i].opcode == OpCodes.Ldc_R4 && (float)list[i].operand == 20f;
                if (flag)
                {
                    list[i].operand = SingletonOptions<ConfigurationItem>.Instance.TransportTrackMaxCapacity;
                    
                }
            }
            return list.AsEnumerable<CodeInstruction>();
        }

    }
}
