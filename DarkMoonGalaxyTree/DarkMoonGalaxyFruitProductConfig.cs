using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STRINGS;
using UnityEngine;

namespace DarkMoonGalaxy
{
    public class DarkMoonGalaxyFruitProductConfig : IEntityConfig
    {
        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(DarkMoonGalaxyFruitProductConfig.Id, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.NAME, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.DESC, 1f, false, Assets.GetAnim("DarkMoonGalaxyFruitProduct_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.8f, 0.7f, true, 0, SimHashes.Creature, null);
            EdiblesManager.FoodInfo foodInfo = new EdiblesManager.FoodInfo(DarkMoonGalaxyFruitProductConfig.Id, 1000f * 50000f, 6, 255.15f, 277.15f, 12000f, true);
            GameObject gameObject2 = EntityTemplates.ExtendEntityToFood(gameObject, foodInfo);
            this.Recipe = RecipeUtils.AddComplexRecipe(new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(DarkMoonGalaxyFruitConfig.Id, 2f)
            },
            new ComplexRecipe.RecipeElement[]
            {
                new ComplexRecipe.RecipeElement(DarkMoonGalaxyFruitProductConfig.Id, 1f)
            },
            "GourmetCookingStation", 100f, STRINGS.SPECIES.DARKMOONGALAXYFRUITPRODUCT.DOMESTICATEDDESC, ComplexRecipe.RecipeNameDisplay.Result, 120, null);
            return gameObject2;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
        }

        public string[] GetDlcIds()
        {
            return null;
        }

        public const string Id = "DarkMoonGalaxyFruitProductConfig";

        public ComplexRecipe Recipe;
    }


}
