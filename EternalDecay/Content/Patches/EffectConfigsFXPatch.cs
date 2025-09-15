using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace EternalDecay.Content.Patches
{
    public class EffectConfigsFXPatch
    {

        [HarmonyPatch(typeof(EffectConfigs), "CreatePrefabs")]
        public static class EffectConfigs_CreatePrefabs_Patch
        {
            public static void Postfix(ref List<GameObject> __result)
            {
                GameObject newEffect = EntityTemplates.CreateEntity(
                    "KMinionBrainBadFx",  // ID
                    "KMinionBrainBadFx",  // name
                    false              // 是否可选中
                );

                KBatchedAnimController anim = newEffect.AddOrGet<KBatchedAnimController>();
                anim.materialType = KAnimBatchGroup.MaterialType.Simple;
                anim.animScale = 0.005f;
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
}
