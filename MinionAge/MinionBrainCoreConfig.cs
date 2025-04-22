using Klei.AI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

namespace MinionAge
{
    public class MinionBrainCoreConfig : IEntityConfig
    {
        public string[] GetDlcIds()
        {
            return null;
        }

        public GameObject CreatePrefab()
        {
            GameObject gameObject = EntityTemplates.CreateLooseEntity(
                "KmodMiniBrainCore",
                STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN.NAME,
                STRINGS.ITEMS.INDUSTRIAL_PRODUCTS.MINIONBRAIN.DESC,
                5f,
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

            gameObject.AddTag(GameTags.Dead);
            gameObject.AddOrGet<KSelectable>();
            gameObject.AddOrGet<Modifiers>();
            gameObject.AddOrGet<Traits>();
            gameObject.AddOrGet<UserNameable>();
            gameObject.AddOrGet<Effects>();
            gameObject.AddOrGet<MinionBrainResume>();
            gameObject.AddOrGet<AttributeLevels>();
            gameObject.AddOrGet<AttributeConverters>();
            gameObject.AddOrGet<MinionBrainCounter>();
            gameObject.AddOrGet<Core.Ownable>();

            // gameObject.AddOrGet<MinionBrainCoreTimer>();
            
            KPrefabID component = gameObject.GetComponent<KPrefabID>();
            component.AddTag(TagManager.Create("KModEnergyDispersionTableConifg"), false);
            component.AddTag(new Tag("KmodMiniBrainCore"));


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

            inst.GetComponent<Core.Ownable>().slotID = MinionBrainPatch.KMinionBrain.Id;




           
            


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
      





        public static string DESC = "生前的全部遗产";
        public static string NAME = "复制人大脑";

        public const string ID = "KmodMiniBrainCore";
        public static readonly Tag tag = TagManager.Create("KmodMiniBrainCore");
        public const float MASS = 1f;
    }
}
