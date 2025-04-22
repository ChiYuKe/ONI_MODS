using System.Collections.Generic;
using UnityEngine;

namespace OxygenConsumingPlant
{

    public class OxygenTreeFruit_RConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            return EntityTemplates.ExtendEntityToFood(
                EntityTemplates.CreateLooseEntity(
                    OxygenTreeFruit_RConfig.ID
                    , STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_R.NAME, 
                    STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_R.DESC,
                    1f,
                    false,
                    Assets.GetAnim("KModOxygenTreeFruit_R_kanim"),
                    "object",
                    Grid.SceneLayer.Front,
                    EntityTemplates.CollisionShape.RECTANGLE,
                    0.8f,
                    0.4f,
                    true,
                    0,
                    SimHashes.Creature
                    , null
                    ),
                new EdiblesManager.FoodInfo(
                    OxygenTreeFruit_RConfig.ID,
                    "",
                    1600000f,
                    0,
                    255.15f,
                    277.15f, 
                    9600f,
                    true));
        }

        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        public void OnPrefabInit(GameObject inst)
        {
        }

        public void OnSpawn(GameObject inst)
        {
            inst.Subscribe(-10536414,OnEatCompleteDelegate);
        }

        private static void OnEatComplete(Edible edible)
        {

        }

        // Token: 0x0400000F RID: 15
        public static string ID = "KModOxygenTreeFruit_R";

        // Token: 0x04000010 RID: 16
        public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

        // Token: 0x04000011 RID: 17
        private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate (Edible component, object data)
        {
            OxygenTreeFruit_RConfig.OnEatComplete(component);
        });
    }
}