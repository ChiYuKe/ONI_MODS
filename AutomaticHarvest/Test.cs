using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using static STRINGS.MISC.STATUSITEMS;

namespace Test_Mod
{
    [HarmonyPatch(typeof(Electrobank), nameof(Electrobank.Damage))]
    public static class Electrobank_Damage_Patch
    {
       
        static FieldInfo currentHealthField = AccessTools.Field(typeof(Electrobank), "currentHealth");

        public static void Postfix(Electrobank __instance)
        {
            float currentHealth = (float)currentHealthField.GetValue(__instance);
            if (currentHealth <= 0f)
            {
                int centerCell = Grid.PosToCell(__instance.transform.position);

                // GameUtil.KInstantiate(Assets.GetPrefab(EffectConfigs.PlantDeathId), __instance.transform.position, Grid.SceneLayer.FXFront, null, 0).SetActive(true);
                GameUtil.KInstantiate(Assets.GetPrefab("MyCustomEffect"), __instance.transform.position, Grid.SceneLayer.FXFront, null, 0).SetActive(true);
                ExplosionUtil.Explode(cell:centerCell, power: 1f, explosionSpeedRange: new Vector2(3f, 6f), targetTiles: MakeCircleShape());

            }
        }


        //球形雷
        public static List<CellOffset> MakeCircleShape(int radius = 5)
        {
            var offsets = new List<CellOffset>();
            int r2 = radius * radius;
            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
                {
                    if (x * x + y * y <= r2)
                        offsets.Add(new CellOffset(x, y));
                }
            }
            return offsets;
        }

        // 十字雷
        public static List<CellOffset> MakeCrossShape(int armLength = 20)
        {
            var offsets = new List<CellOffset> { new CellOffset(0, 0) };
            for (int i = 1; i <= armLength; i++)
            {
                offsets.Add(new CellOffset(i, 0));   
                offsets.Add(new CellOffset(-i, 0));  
                offsets.Add(new CellOffset(0, i));   
                offsets.Add(new CellOffset(0, -i));  
            }
            return offsets;
        }

    }





    [HarmonyPatch(typeof(EffectConfigs), "CreatePrefabs")]
    public static class EffectConfigs_CreatePrefabs_Patch
    {
        public static void Postfix(ref List<GameObject> __result)
        {
            GameObject newEffect = EntityTemplates.CreateEntity(
                "MyCustomEffect",  // ID
                "MyCustomEffect",  // name
                false              // 是否可选中
            );

            KBatchedAnimController anim = newEffect.AddOrGet<KBatchedAnimController>();
            anim.materialType = KAnimBatchGroup.MaterialType.Simple;
            anim.animScale = 0.05f;
            anim.initialAnim = "loop";  // 动画初始状态
            anim.initialMode = KAnim.PlayMode.Once;
            anim.destroyOnAnimComplete = true;

         
            anim.AnimFiles = new KAnimFile[]
            {
            Assets.GetAnim("Mywhirlpool_fx_kanim")
            };

            //添加声音
            //newEffect.AddOrGet<LoopingSounds>();
            __result.Add(newEffect);
        }
    }





}
