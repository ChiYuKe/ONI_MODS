using System.Collections.Generic;
using UnityEngine;

namespace OxygenConsumingPlant
{

    public class OxygenTreeFruit_GConfig : IEntityConfig
    {

        public GameObject CreatePrefab()
        {
            return EntityTemplates.ExtendEntityToFood(
                EntityTemplates.CreateLooseEntity(
                    OxygenTreeFruit_GConfig.ID
                    , STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_G.NAME, 
                    STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_G.DESC,
                    1f,
                    false,
                    Assets.GetAnim("KModOxygenTreeFruit_G_kanim"),
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
                    OxygenTreeFruit_GConfig.ID,
                    "",
                    1600000f,
                    0,
                    255.15f,
                    277.15f, 
                    4800f,
                    true)
                // 将注册好的buff效果添加到当前对象身上
                .AddEffects(new List<string> { "wajue" }, DlcManager.AVAILABLE_EXPANSION1_ONLY));
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
            bool flag = edible != null;
            if (flag)
            {
                Vector3 vector11 = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
                string text = "edible.transform坐标数据";
                Vector3 vector = vector11;
                Debug.Log(text + vector.ToString());
                GameObject gameObject = edible.gameObject;
                Notifier notifier = gameObject.AddOrGet<Notifier>();
                Notification notification = new Notification("进食坐标", NotificationType.Bad, delegate (List<Notification> notificationList, object data)
                {
                    string text2 = "这是坐标:";
                    Vector3 vector3 = vector11;
                    return text2 + vector3.ToString() + notificationList.ReduceMessages(false);
                }, "/t• " + gameObject.GetProperName(), true, 0f, null, null, null, true, false, false);
                notifier.Add(notification, "");
                int num = 0;
                float unitsConsumed = edible.unitsConsumed;
                Debug.Log("食物的消耗量：" + unitsConsumed.ToString());
                int num2 = Mathf.FloorToInt(unitsConsumed);
                float num3 = unitsConsumed % 1f;
                float num4 = 3f;
                bool flag2 = Random.value < num3;
                if (flag2)
                {
                    num2++;
                }
                for (int i = 0; i < num2; i++)
                {
                    bool flag3 = Random.value < SEEDS_PER_FRUIT_CHANCE;
                    if (flag3)
                    {
                        num++;
                    }
                }
                bool flag4 = num4 > 0f;
                if (flag4)
                {
                    Vector3 vector2 = edible.transform.GetPosition() + new Vector3(0f, 0.05f, 0f);
                    vector2 = Grid.CellToPosCCC(Grid.PosToCell(vector2), Grid.SceneLayer.Ore);
                    GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), edible.transform.GetPosition(), Grid.SceneLayer.FXFront, null, 0).SetActive(true);
                    GameObject gameObject2 = GameUtil.KInstantiate(Assets.GetPrefab(new Tag("iron")), vector2, Grid.SceneLayer.Ore, null, 0);
                    PrimaryElement component = edible.GetComponent<PrimaryElement>();
                    PrimaryElement component2 = gameObject2.GetComponent<PrimaryElement>();
                    component2.Temperature = component.Temperature;
                    component2.Units = num4;
                    gameObject2.SetActive(true);
                }
            }
        }

        // Token: 0x0400000F RID: 15
        public static string ID = "KModOxygenTreeFruit_G";

        // Token: 0x04000010 RID: 16
        public static float SEEDS_PER_FRUIT_CHANCE = 0.05f;

        // Token: 0x04000011 RID: 17
        private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate (Edible component, object data)
        {
            OxygenTreeFruit_GConfig.OnEatComplete(component);
        });
    }
}