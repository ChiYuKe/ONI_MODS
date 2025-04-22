using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestElement
{
    [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
    internal class Assets_SubstanceListHookup
    {
        private static void Prefix()
        {
            ElementUtil.RegisterElementStrings("TestElement", MinionAge_DLC.STRINGS.ELEMENTS.TESTELEMENT.NAME, MinionAge_DLC.STRINGS.ELEMENTS.TESTELEMENT.DESC);
        }

        private static void Postfix()
        {
            Test_Element.RegisterTestElemenSubstance();
        }
       
    }
}
