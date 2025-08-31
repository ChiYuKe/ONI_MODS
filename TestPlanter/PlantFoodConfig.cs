using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STRINGS;
using UnityEngine;

namespace TestPlanter
{
    public class PlantFoodConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            return EntityTemplates.ExtendEntityToFood(
                    EntityTemplates.CreateLooseEntity(
                        PlantFoodConfig.ID,
                        UI.FormatAsLink("测试食物", "TESTFOOD"), 
                        global::STRINGS.ITEMS.FOOD.PRICKLEFRUIT.DESC, 
                        1f,
                        false,
                        Assets.GetAnim("TestFood_kanim"),
                        "object", 
                        Grid.SceneLayer.Front, 
                        EntityTemplates.CollisionShape.RECTANGLE,
                        0.8f, 
                        0.4f,
                        true,
                        0,
                        SimHashes.Creature,
                        null),
                    new EdiblesManager.FoodInfo(PlantFoodConfig.ID, 1000f*5000f, 6, 255.15f, 277.15f, 4800f, true, null, null));



        }

        public void OnPrefabInit(GameObject inst)
        {
            //KBatchedAnimController animController = inst.GetComponent<KBatchedAnimController>();
            //if (animController != null)
            //{
            //    animController.animScale = 0.0004f;
            //}
        }

       
        public void OnSpawn(GameObject inst)
        {
            inst.Subscribe(-10536414, OnEatCompleteDelegate);
        }



        private static void OnEatComplete(Edible edible)
        {
            if (edible != null)
            {
                int num = 0;
                float unitsConsumed = edible.unitsConsumed;
                int num2 = Mathf.FloorToInt(unitsConsumed);
                float num3 = unitsConsumed % 1f;
                if (global::UnityEngine.Random.value < num3)
                {
                    num2++;
                }
                for (int i = 0; i < num2; i++)
                {
                    if (global::UnityEngine.Random.value < PrickleFruitConfig.SEEDS_PER_FRUIT_CHANCE)
                    {
                        num++;
                    }
                }
                if (num > 0)
                {
                    Vector3 vector = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
                    vector = Grid.CellToPosCCC(Grid.PosToCell(vector), Grid.SceneLayer.Ore);
                    GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("TestPlanterSeed")), vector, Grid.SceneLayer.Ore, null, 0);
                    PrimaryElement component = edible.GetComponent<PrimaryElement>();
                    PrimaryElement component2 = gameObject.GetComponent<PrimaryElement>();
                    component2.Temperature = component.Temperature;
                    component2.Units = (float)num;
                    gameObject.SetActive(true);
                }
            }
        }

        public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

        public static string ID = "TestFood";

        private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate (Edible component, object data)
        {
            OnEatComplete(component);
        });

        public string[] GetDlcIds()
        {
            return null;
        }



    }
}
