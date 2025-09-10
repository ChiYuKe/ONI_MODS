using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EternalDecay.Content.Comps;
using EternalDecay.Content.Patches;
using Klei.AI;
using UnityEngine;

namespace EternalDecay.Content.Items
{
    public class MinionBrainConfig : IEntityConfig
    {
        public string[] GetDlcIds()
        {
            return null;
        }

        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(
                "KmodMiniBrain",
                Configs.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN.NAME,
                Configs.STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN.DESC,
                5f,
                true,
                Assets.GetAnim("KmodMiniBrain_kanim"),
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

            gameObject.AddTag(GameTags.Dead);
            gameObject.AddOrGet<KSelectable>();
            
            gameObject.AddOrGet<KMinionBrain>();

            gameObject.AddOrGet<Accepttheinheritance>();





            gameObject.AddOrGet<Modifiers>();
            gameObject.AddOrGet<Traits>();
            gameObject.AddOrGet<UserNameable>();
            gameObject.AddOrGet<Effects>();
            // gameObject.AddOrGet<MinionBrainResume>();
            gameObject.AddOrGet<AttributeLevels>();
            gameObject.AddOrGet<AttributeConverters>();
            gameObject.AddOrGet<KOwnable>();

            KPrefabID component = gameObject.GetComponent<KPrefabID>();
            component.AddTag(TagManager.Create("KModEnergyDispersionTableConifg"), false);
            component.AddTag(new Tag("KmodMiniBrain"));


            return gameObject;
        }

        public void OnPrefabInit(GameObject inst)
        {
            var attributes = inst.GetAttributes();
            attributes.Add(Db.Get().Attributes.SpaceNavigation);
            attributes.Add(Db.Get().Attributes.Construction);
            attributes.Add(Db.Get().Attributes.Digging);
            attributes.Add(Db.Get().Attributes.Machinery);
            attributes.Add(Db.Get().Attributes.Athletics);
            attributes.Add(Db.Get().Attributes.Learning);
            attributes.Add(Db.Get().Attributes.Cooking);
            attributes.Add(Db.Get().Attributes.Caring);
            attributes.Add(Db.Get().Attributes.Strength);
            attributes.Add(Db.Get().Attributes.Art);
            attributes.Add(Db.Get().Attributes.Botanist);
            attributes.Add(Db.Get().Attributes.Ranching);

            inst.GetComponent<KOwnable>().slotID = AssignableSlotsPatch.KMinionBrain.Id;


            //KBatchedAnimController animController = inst.GetComponent<KBatchedAnimController>();
            //if (animController != null)
            //{
            //    animController.animScale = 0.0010f;
            //}





            //ColorfulPulsatingLight2D light = inst.AddOrGet<ColorfulPulsatingLight2D>();
            //light.MinIntensity = 10000;
            //light.MaxIntensity = 20000;
            //light.MinRadius = 5;
            //light.MaxRadius = 10;
            //light.PulseSpeed = 6f;


        }

        public void OnSpawn(GameObject inst)
        {


        }






        public const string ID = "KmodMiniBrain";
        public static readonly Tag tag = TagManager.Create("KmodMiniBrain");
        public const float MASS = 1f;
    }
}
