using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static MiniBox.Config;

namespace MiniBox.MiscconfigConfig
{
    //挖掘掉落倍率
    [HarmonyPatch(typeof(WorldDamage), "OnDigComplete")]
    public class DiggingPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            foreach (CodeInstruction codeInstruction in instr.ToList<CodeInstruction>())
            {
                bool flag = codeInstruction.opcode == OpCodes.Ldc_R4 && (double)((float)codeInstruction.operand) == 0.5;
                if (flag)
                {
                    codeInstruction.operand = SingletonOptions<ConfigurationItem>.Instance.DiggingDropMultiplier;
                }
                yield return codeInstruction;
            }
            yield break;
        }

    }
}
