﻿
using System.Collections.Generic;
using System.Linq;
using CykUtils;
using EternalDecay.Content.Comps;
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
            [HarmonyPriority(Priority.First)]
            public static class ModifierSet_Initialize_Patch
            {
                public static void Postfix(ModifierSet __instance)
                {

                    Content.Core.KEffects.RegisterAll(__instance);
                }
            }




            //[HarmonyPatch(typeof(Db), "Initialize")]
            //public class WaterCoolerPatch
            //{
            //    public static void Postfix(Db __instance)
            //    {
            //        // 新增饮料选项
            //        var newBeverages = new List<Tuple<Tag, string>>(WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS)
            //        {
            //            new Tuple<Tag, string>(
            //                SimHashes.Algae.CreateTag(),
            //                KEffects.ETERNALDECAY_LUMINESCENCEKING 
            //            )
            //        };

            //        WaterCoolerConfig.BEVERAGE_CHOICE_OPTIONS = newBeverages.ToArray();
            //    }
            //}

        }
    }
}
