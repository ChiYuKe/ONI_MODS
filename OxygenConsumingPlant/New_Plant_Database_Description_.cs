using HarmonyLib;
using KModTool;
using System.Reflection;
using TUNING;
using UnityEngine;

namespace OxygenConsumingPlant
{
    public class New_Plant_Database_Description_ : ISim4000ms
    {
        public void Sim4000ms(float dt)
        {
        }

        public class ModifierSetPatch
        {
            [HarmonyPatch(typeof(ModifierSet), "Initialize")]
            public static class ModifierSet_Initialize_Patch
            {
                public static void Postfix(ModifierSet __instance)
                {
                    // 在 ModifierSet 初始化后注册自定义效果
                    Buff.Register(__instance);
                }
            }
        }






        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class New_Plant_Database_Description
        {
            public static void Prefix()
            {
                KModStringUtils.Add_New_PlantStrings("KModOxygenTree", STRINGS.CREATURES.SPECIES.KMODOXYGENTREE.NAME, "描述", "扩展描述");
                KModStringUtils.Add_New_PlantSeedStrings("KModOxygenTree", STRINGS.CREATURES.SPECIES.SEEDS.KMODOXYGENTREESEED.NAME, "");
                KModStringUtils.Add_New_FoodStrings("KModOxygenTreeFruit_G", STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_G.NAME, "扩展描述", null);
                KModStringUtils.Add_New_FoodStrings("KModOxygenTreeFruit_R", STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_R.NAME, "扩展描述", null);
                KModStringUtils.Add_New_CustomEffectBuilder_Strings("wajue", STRINGS.DUPLICANTS.MODIFIERS.WAJUE.NAME, STRINGS.DUPLICANTS.MODIFIERS.WAJUE.DESC);
                CROPS.CROP_TYPES.Add(new Crop.CropVal("KModOxygenTreeFruit_G", 300f, 5, true));
                CROPS.CROP_TYPES.Add(new Crop.CropVal("KModOxygenTreeFruit_R", 300f, 5, true));

            }
        }

        [HarmonyPatch(typeof(RangeVisualizerEffect), "OnPostRender")]
        public class RangeVisualizerEffectPatch
        {
            public static void Prefix(RangeVisualizerEffect __instance)
            {
                FieldInfo field = typeof(RangeVisualizerEffect).GetField("material", BindingFlags.Instance | BindingFlags.NonPublic);
                Material material = (Material)field.GetValue(__instance);
                __instance.highlightColor = new Color(1f,0.1f,0f,1f);

                bool flag = material != null;
                if (flag)
                {
                    material.SetColor("_HighlightColor", __instance.highlightColor);
                }
            }
        }
    }
}