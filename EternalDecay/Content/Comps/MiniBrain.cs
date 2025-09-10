
using System.Collections;
using EternalDecay.Content.Core;
using Klei.AI;
using TemplateClasses;
using UnityEngine;

namespace EternalDecay.Content.Comps
{
    public class KMinionBrain: KMonoBehaviour
    {

        protected override void OnSpawn()
        {
            base.OnSpawn();
            KEffects.ApplyBuff(this.gameObject, KEffects.ETERNALDECAY_BAOZHIQI);
            this.Subscribe((int)GameHashes.EffectRemoved, effectremoved); // buff移除时触发



        }

        private void effectremoved(object obj) 
        {
            Util.KDestroyGameObject(gameObject);
            GameUtil.KInstantiate(Assets.GetPrefab("KMinionBrainBadFx"), transform.position, Grid.SceneLayer.FXFront, null, 0).SetActive(true);

            GameObject prefab = Assets.GetPrefab(new Tag("KmodMiniBrainBad"));
            GameObject newMinion = GameUtil.KInstantiate(prefab, transform.position + new Vector3(0f, 1f, 0f), Grid.SceneLayer.Ore, null, 0);
            newMinion.SetActive(true);


        }
    }
}


