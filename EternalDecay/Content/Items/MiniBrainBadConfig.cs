using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klei.AI;
using UnityEngine;

namespace EternalDecay.Content.Items
{
    public class MiniBrainBadConfig : IEntityConfig
    {
        public string[] GetDlcIds()
        {
            return null;
        }

        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(
                "KmodMiniBrainBad",
                Configs.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAINBAD.NAME,
                Configs.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAINBAD.DESC,
                5f,
                true,
                Assets.GetAnim("KmodMiniBrainBad_kanim"),
                "object",
                Grid.SceneLayer.Front,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.6f,
                0.6f,
                true,
                0,
                SimHashes.Creature,
                new List<Tag> { GameTags.IndustrialIngredient }

            );
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
