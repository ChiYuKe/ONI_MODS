using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace MinionAge
{
    public class MinionBrainCore2Config : IEntityConfig
    {
        public string[] GetDlcIds()
        {
            return null;
        }

        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(
                "KmodMiniBrainCore2",
                STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN2.NAME,
                STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN2.DESC,
                50f,
                true,
                Assets.GetAnim("KmodMiniBrainCore_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.8f,
                0.6f,
                true,
                0,
                SimHashes.Creature,
                new List<Tag> { GameTags.IndustrialIngredient }
            );



            
            KPrefabID component = gameObject.AddOrGet<KPrefabID>();
            component.AddTag(new Tag("KmodMiniBrainCore2"));
            component.AddTag(TagManager.Create("KModEnergyDispersionTableConifg"), false);
           


            return gameObject;
        }

        public void OnPrefabInit(GameObject inst)
        {

        }

        public void OnSpawn(GameObject inst)
        {
           

        }
     

    }
}
