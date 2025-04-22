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
            ElementUtil.RegisterElementStrings("KTestElement",  "测试元素", "这是测试元素的描述");
        }

        private static void Postfix()
        {
            Test_Element.RegisterTestElemenSubstance();
        }
       
    }
}
