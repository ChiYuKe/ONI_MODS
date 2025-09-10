using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Database;
using HarmonyLib;

namespace EternalDecay.Content.Patches
{
    public class ChoreTypesPatch
    {

        [HarmonyPatch(typeof(ChoreTypes))]
        public static class AddNewChorePatch
        {
            public static ChoreType Accepttheinheritance;

            [HarmonyPostfix]
            [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(ResourceSet) })]
            public static void Postfix(ChoreTypes __instance)
            {
                if (__instance == null) return;

                MethodInfo addMethod = typeof(ChoreTypes).GetMethod("Add", BindingFlags.NonPublic | BindingFlags.Instance);
                if (addMethod == null) return;

                object[] parameters = new object[]
                {
                "Accepttheinheritance",      
                new string[0],              
                "",                      
                new string[0],             
                "接受 罐中脑",
                "顷刻炼化 罐中脑",            
                "这个复制人正在接受 罐中脑 的传承！！", 
                false,                      
                -1,                          
                null                       
                };

               
                Accepttheinheritance = (ChoreType)addMethod.Invoke(__instance, parameters);

                Accepttheinheritance.interruptPriority = 100000;
            }
        }
    }
}
