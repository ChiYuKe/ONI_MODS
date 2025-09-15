using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using KMod;

namespace TAccessories
{
    public class KModPatch
    {
        public class Patch : UserMod2
        {
            // MOD加载时调用
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                LogUtil.Log("MOD加载成功");
            }

            // 初始化ModifierSet时调用
            [HarmonyPatch(typeof(ModifierSet), "Initialize")]
            public class ModifierSet_Initialize_Patch
            {
                public static void Postfix(ModifierSet __instance)
                {
                    KEffects.Register(__instance);
                }
            }


            // 初始化数据库时调用
            [HarmonyPatch(typeof(Db), "Initialize")]
            public class Db_Initialize_Patch
            {
                public static void Postfix(Db __instance)
                {
                    KDb.Init(__instance);

                }
            }

            [HarmonyPatch(typeof(Localization))]
            [HarmonyPatch("Initialize")]
            private class Translate
            {
                public static void Postfix()
                {
                    Loc.Translate(typeof(STRINGS), true);
                }
            }



        }
    }
}
