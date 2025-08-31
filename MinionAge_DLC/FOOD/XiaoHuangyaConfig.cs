using Klei.AI;
using KModTool;
using MinionAge;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MinionAge_DLC
{
    // UBConfig 类实现了 IEntityConfig 接口，用于配置一个新的实体（在这里是一个食物）
    public class XiaoHuangyaConfig : IEntityConfig
    {
        public static readonly SimHashes TestElement = (SimHashes)Hash.SDBMLower("TestElement");
        // CreatePrefab 方法用于创建实体的预制件（Prefab）
        public GameObject CreatePrefab()
        {
            // 使用 EntityTemplates.CreateLooseEntity 创建一个松散的实体
            GameObject gameObject = EntityTemplates.CreateLooseEntity(ID, UI.FormatAsLink(STRINGS.KFOOD.XIAOHUANGYA.NAME, "XiaoHuangya"), STRINGS.KFOOD.XIAOHUANGYA.DESC, 1f,
                false, // 是否可移动
                Assets.GetAnim("XiaoHuangya_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE,
                0.6f, // 实体的宽度
                0.4f, // 实体的高度
                true, 0, SimHashes.Creature, null
            );



            KBatchedAnimController animController = gameObject.GetComponent<KBatchedAnimController>();
            if (animController != null)
            {
                animController.animScale = 0.003f;
            }






            // 判断有没有订阅mod本体
            bool ModDlc = Main.Patch.IsModLoaded("MinionAge.Dev");
            Element element = ElementLoader.FindElementByHash(TestElement);
            if (ModDlc)
            {
                // 创建食物的属性信息
                EdiblesManager.FoodInfo foodInfo = new EdiblesManager.FoodInfo(
                    ID, // 食物的ID
                    15000 * 1000f, // 每单位食物的卡路里值。
                    3, // 食物的质量等级。
                    255.15f, // 如果食物存储在这个温度范围内，可以延长保质期。
                    277.15f, // 如果食物暴露在这个温度以上，会加速腐烂。
                    4800f, // 食物的腐烂时间
                    false // 是否会腐烂
                ).AddEffects(
                    new List<string> { "ScorchingMetalSharer" }, // 食物效果列表
                    DlcManager.EXPANSION1 // 这些效果仅在第一个DLC中可用
                );


                // 将实体扩展为食物
                GameObject gameObject2 = EntityTemplates.ExtendEntityToFood(gameObject, foodInfo);


                // 添加一个复杂的配方，用于在烹饪站中制作这个食物
                this.Recipe = KRecipeUtils.AddComplexRecipe(
                    new ComplexRecipe.RecipeElement[]
                    {
                         new ComplexRecipe.RecipeElement("RawEgg", 5f),
                         new ComplexRecipe.RecipeElement("TestElement", 5f)
                    },
                    new ComplexRecipe.RecipeElement[]
                    {
                        new ComplexRecipe.RecipeElement(ID, 1f)
                    },
                         "CookingStation",
                    100f,
                    STRINGS.KFOOD.XIAOHUANGYA.NAME,
                    ComplexRecipe.RecipeNameDisplay.Result,
                    120,
                    null
                );

                return gameObject2;
            }
            else
            {
                // 创建食物的属性信息
                EdiblesManager.FoodInfo foodInfo = new EdiblesManager.FoodInfo(
                    ID, // 食物的ID
                    15000 * 1000f, // 每单位食物的卡路里值。
                    3, // 食物的质量等级。
                    255.15f, // 如果食物存储在这个温度范围内，可以延长保质期。
                    277.15f, // 如果食物暴露在这个温度以上，会加速腐烂。
                    4800f, // 食物的腐烂时间
                    false // 是否会腐烂
                ).AddEffects(
                    new List<string> { "ScorchingMetalSharer" }, // 食物效果列表
                    DlcManager.EXPANSION1 // 这些效果仅在第一个DLC中可用
                );
                // 将实体扩展为食物
                GameObject gameObject2 = EntityTemplates.ExtendEntityToFood(gameObject, foodInfo);

                // 添加一个复杂的配方，用于在烹饪站中制作这个食物
                this.Recipe = KRecipeUtils.AddComplexRecipe(
                    new ComplexRecipe.RecipeElement[]
                    {
                        new ComplexRecipe.RecipeElement("BasicPlantFood", 5f),
                        new ComplexRecipe.RecipeElement("RawEgg", 5f)
                    },
                    new ComplexRecipe.RecipeElement[]
                    {
                    new ComplexRecipe.RecipeElement(ID, 1f)
                    },
                         "CookingStation",
                    100f,
                    STRINGS.KFOOD.XIAOHUANGYA.NAME,
                    ComplexRecipe.RecipeNameDisplay.Result,
                    120,
                    null
                );
                return gameObject2;
            }
        }






        public void OnPrefabInit(GameObject inst) { }

        public void OnSpawn(GameObject inst)
        {
            inst.Subscribe(-10536414, OnEatCompleteDelegate);
        }


        public static void OnEatComplete(Edible edible)
        {

            // 获取正在进食的复制人
            WorkerBase worker = edible.worker;
            if (worker != null)
            {

                KPrefabID kPrefabID = worker.GetComponent<KPrefabID>();
                Debug.Log("正在进食的对象是: " + kPrefabID.name);

                AudioUtil.PlaySound(ModAssets.Sounds.WW, CameraController.Instance.GetVerticallyScaledPosition(worker.gameObject.transform.GetPosition()), 1f);
              

                // 获取当前年龄
                float currentAge = MinionDataSaver.GetCurrentAgeInSeconds(worker.gameObject);

                // 判断年龄是否足够减去 700f
                if (currentAge > 900f)
                {
                    // 更新复制人年龄
                    MinionDataSaver.UpdateMinionAge(worker.gameObject, currentAge - 700f);
                    Debug.Log($"更新年龄：当前年龄 = {currentAge} 秒，减去 700 秒后为 {currentAge - 700f} 秒");
                }


                // 移除掉“散热者”和“积热者”
                RemoveEffect(worker.gameObject, "HeatWanderer");
                RemoveEffect(worker.gameObject, "CoolWanderer");
            }
        }


        public static void RemoveEffect(GameObject gameObject, string effect_id)
        {
            // 获取 Effects 组件
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            // 如果没有找到 Effects 组件，返回
            if (effectsComponent == null)
            {
                return;
            }

            // 使用 effect_id 移除效果
            effectsComponent.Remove(effect_id);
        }


        private static readonly EventSystem.IntraObjectHandler<Edible> OnEatCompleteDelegate = new EventSystem.IntraObjectHandler<Edible>(delegate (Edible component, object data)
        {
            XiaoHuangyaConfig.OnEatComplete(component);
        });




        // GetDlcIds 方法返回一个字符串数组，表示这个实体在所有DLC版本中都可用

        public string[] GetDlcIds()
        {
            return new string[2] { "", "EXPANSION1_ID" };
        }

        // 常量字段，定义实体的ID
        public const string ID = "XiaoHuangya";

        // Recipe 字段保存了之前定义的复杂配方
        public ComplexRecipe Recipe;
    }
}
