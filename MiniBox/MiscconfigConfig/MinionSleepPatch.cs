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
    //复制人睡觉
    [HarmonyPatch(typeof(ElementSplitterComponents), "CanFirstAbsorbSecond")]
    public class MinionSleepPatch
    {
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            foreach (CodeInstruction codeInstruction in instr.ToList<CodeInstruction>())
            {
                bool flag = codeInstruction.opcode == OpCodes.Ldc_R4 && (double)((float)codeInstruction.operand) == -0.11666667f;
                if (flag)
                {
                    codeInstruction.operand = SingletonOptions<ConfigurationItem>.Instance.NoWantSleepPatch ? 0f : -0.11666667f;
                }
                yield return codeInstruction;
            }
            yield break;
        }

    }
}
