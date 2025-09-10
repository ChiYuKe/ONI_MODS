
using CykUtils;
using EternalDecay.Content.Core;
using HarmonyLib;
using KMod;
using UnityEngine;

namespace EternalDecay
{
    public class KModPatch
    {
        public class Patch : UserMod2 
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                LogUtil.Log("MOD加载成功");

                EternalDecayInitializer.Initialize();

            }


            // 本地化补丁，初始化时注册并加载翻译字符串。//
            [HarmonyPatch(typeof(Localization), "Initialize")]
           
            private class Translate_Initialize_Patch
            {
                public static void Postfix()
                {
                    Loc.Translate(typeof(Content.Configs.STRINGS),true);
                }
            }


            // 在 ModifierSet 初始化时注册所有自定义效果。//
            [HarmonyPatch(typeof(ModifierSet), "Initialize")]
            public static class ModifierSet_Initialize_Patch
            {
                public static void Postfix(ModifierSet __instance)
                {

                    Content.Core.KEffects.RegisterAll(__instance);
                }
            }
















        }
    }
}
