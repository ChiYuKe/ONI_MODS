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
    ////碎片最大堆叠
    //[HarmonyPatch(typeof(ElementSplitterComponents), "CanFirstAbsorbSecond")]
    //public class StackPatch
    //{
    //    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
    //    {
    //        foreach (CodeInstruction codeInstruction in instr.ToList<CodeInstruction>())
    //        {
    //            bool flag = codeInstruction.opcode == OpCodes.Ldc_R4 && (double)((float)codeInstruction.operand) == 25000f;
    //            if (flag)
    //            {
    //                codeInstruction.operand = SingletonOptions<ConfigurationItem>.Instance.maximumstackmass;
    //            }
    //            yield return codeInstruction;
    //        }
    //        yield break;
    //    }

    //}
}
