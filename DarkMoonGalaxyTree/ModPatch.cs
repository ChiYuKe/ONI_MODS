using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CykUtils;
using HarmonyLib;
using KMod;
using PeterHan.PLib.Database;
using STRINGS;
using TUNING;

namespace DarkMoonGalaxy
{
    public class Patch : UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);
            Debug.Log("Mod - DarkMoonGalaxyTree - 已加载并初始化。");

            PCodexManager codexManager = new PCodexManager();
            codexManager.RegisterPlants();


        }

        [HarmonyPatch(typeof(Localization), "Initialize")]
        private class Translate_Initialize_Patch
        {
            public static void Postfix()
            {
                Loc.Translate(typeof(STRINGS), true);
            }
        }


        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch("LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
            public static void Prefix()
            {
                StringUtils.AddPlantStrings(DarkMoonGalaxyConfig.Id, STRINGS.SPECIES.DARKMOONGALAXY.NAME, STRINGS.SPECIES.DARKMOONGALAXY.DESC, STRINGS.SPECIES.DARKMOONGALAXY.DOMESTICATEDDESC);
                StringUtils.AddPlantSeedStrings(DarkMoonGalaxyConfig.SeedId, STRINGS.SPECIES.DARKMOONGALAXYSEED.NAME, STRINGS.SPECIES.DARKMOONGALAXYSEED.DESC);
                StringUtils.AddFoodStrings(DarkMoonGalaxyFruitConfig.Id, STRINGS.SPECIES.DARKMOONGALAXYFRUIT.NAME, STRINGS.SPECIES.DARKMOONGALAXYFRUIT.DESC, null);
                StringUtils.AddFoodStrings(DarkMoonGalaxyFruitProductConfig.Id, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.NAME, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.DESC, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.DOMESTICATEDDESC);
                CROPS.CROP_TYPES.Add(new Crop.CropVal(DarkMoonGalaxyFruitConfig.Id, 300f, 5, true));


            }
        }

    }











    public class RecipeUtils
    {
        // Token: 0x06000029 RID: 41 RVA: 0x00002A4C File Offset: 0x00000C4C
        public static ComplexRecipe AddComplexRecipe(ComplexRecipe.RecipeElement[] input, ComplexRecipe.RecipeElement[] output, string fabricatorId, float productionTime, string recipeDescription, ComplexRecipe.RecipeNameDisplay nameDisplayType, int sortOrder, string requiredTech = null)
        {
            return new ComplexRecipe(ComplexRecipeManager.MakeRecipeID(fabricatorId, input, output), input, output)
            {
                time = productionTime,
                description = recipeDescription,
                nameDisplay = nameDisplayType,
                fabricators = new List<Tag> { fabricatorId },
                sortOrder = sortOrder,
                requiredTech = requiredTech
            };
        }
    }








    public static class StringUtils
    {
        public static void AddPlantStrings(string plantId, string name, string description, string domesticatedDescription)
        {
            Strings.Add(new string[]
            {
                "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".NAME",
                UI.FormatAsLink(name, plantId)
            });
            Strings.Add(new string[]
            {
                "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".DESC",
                description
            });
            Strings.Add(new string[]
            {
                "STRINGS.CREATURES.SPECIES." + plantId.ToUpperInvariant() + ".DOMESTICATEDDESC",
                domesticatedDescription
            });
        }

        public static void AddPlantSeedStrings(string plantId, string name, string description)
        {
            Strings.Add(new string[]
            {
                "STRINGS.CREATURES.SPECIES.SEEDS." + plantId.ToUpperInvariant() + ".NAME",
                UI.FormatAsLink(name, plantId)
            });
            Strings.Add(new string[]
            {
                "STRINGS.CREATURES.SPECIES.SEEDS." + plantId.ToUpperInvariant() + ".DESC",
                description
            });
        }

        public static void AddFoodStrings(string foodId, string name, string description, string recipeDescription = null)
        {
            Strings.Add(new string[]
            {
                "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".NAME",
                UI.FormatAsLink(name, foodId)
            });
            Strings.Add(new string[]
            {
                "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".DESC",
                description
            });
            bool flag = recipeDescription != null;
            bool flag2 = flag;
            if (flag2)
            {
                Strings.Add(new string[]
                {
                    "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".RECIPEDESC",
                    recipeDescription
                });
            }
        }
    }




}
