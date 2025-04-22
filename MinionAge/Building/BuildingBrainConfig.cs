

using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace DebuffRoulette
{
    public class GeneShufflerConfig : IEntityConfig
    {
        // Token: 0x06000A0D RID: 2573 RVA: 0x0003A786 File Offset: 0x00038986
        public string[] GetDlcIds()
        {
            return DlcManager.AVAILABLE_ALL_VERSIONS;
        }

        // Token: 0x06000A0E RID: 2574 RVA: 0x0003A790 File Offset: 0x00038990
        public GameObject CreatePrefab()
        {
            string text = "KModBuildingBrain";
            string text2 = "测试建筑";
            string text3 = "";
            float num = 2000f;
            EffectorValues tier = global::TUNING.BUILDINGS.DECOR.BONUS.TIER0;
            EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER0;
            GameObject gameObject = EntityTemplates.CreatePlacedEntity(text, text2, text3, num, Assets.GetAnim("geneshuffler_kanim"), "off", Grid.SceneLayer.Building, 4, 3, tier, tier2, SimHashes.Creature, new List<Tag> { GameTags.Gravitas }, 293f);
            gameObject.AddTag(GameTags.NotRoomAssignable);
            PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
            component.SetElement(SimHashes.Unobtanium, true);
            component.Temperature = 294.15f;
            gameObject.AddOrGet<Operational>();
            gameObject.AddOrGet<Notifier>(); 
            gameObject.AddOrGet<BuildingBrain>();
            // LoreBearerUtil.AddLoreTo(gameObject, new LoreBearerAction(LoreBearerUtil.NerualVacillator));
            gameObject.AddOrGet<LoopingSounds>();
            gameObject.AddOrGet<Ownable>();

            gameObject.AddOrGet<Prioritizable>();
            gameObject.AddOrGet<Demolishable>();
            Storage storage = gameObject.AddOrGet<Storage>();
            storage.dropOnLoad = true;
            ManualDeliveryKG manualDeliveryKG = gameObject.AddOrGet<ManualDeliveryKG>();
            manualDeliveryKG.SetStorage(storage);
            manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
            manualDeliveryKG.RequestedItemTag = new Tag("KmodMiniBrainCore");
            manualDeliveryKG.refillMass = 1f;
            manualDeliveryKG.MinimumMass = 1f;
            manualDeliveryKG.capacity = 1f;
            KBatchedAnimController kbatchedAnimController = gameObject.AddOrGet<KBatchedAnimController>();
            kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
            kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;
            return gameObject;
        }

        public void OnPrefabInit(GameObject inst)
        {
            inst.GetComponent<BuildingBrain>().workLayer = Grid.SceneLayer.Building;
            inst.GetComponent<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;
            inst.GetComponent<OccupyArea>().objectLayers = new ObjectLayer[] { ObjectLayer.Building };
            inst.GetComponent<Deconstructable>();
        }

        public void OnSpawn(GameObject inst)
        {
        }
    }
}





















































//using System;
//using System.Collections.Generic;
//using TUNING;
//using UnityEngine;
//using static ResearchTypes;


//namespace DebuffRoulette
//{
//    public class BuildingBrainConfig : IBuildingConfig
//    {

//        public override BuildingDef CreateBuildingDef()
//        {
//            string id = "KModBuildingBrain";
//            int width = 4;
//            int height = 3;
//            string anim = "geneshuffler_kanim";
//            int hitpoints = 100;
//            float construction_time = 30f;
//            float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
//            string[] all_METALS = MATERIALS.ALL_METALS;
//            float num5 = 800f;
//            BuildLocationRule buildLocationRule = BuildLocationRule.OnFloor;
//            EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER3;

//            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, num5, buildLocationRule, BUILDINGS.DECOR.PENALTY.TIER1, tier2, 0.2f);

//            return buildingDef;
//        }


//        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
//        {

//            go.AddOrGet<BuildingBrain>();
//            Storage storage = go.AddOrGet<Storage>();
//            storage.dropOnLoad = true;
//            ManualDeliveryKG manualDeliveryKG = go.AddOrGet<ManualDeliveryKG>();
//            manualDeliveryKG.SetStorage(storage);
//            manualDeliveryKG.choreTypeIDHash = Db.Get().ChoreTypes.MachineFetch.IdHash;
//            manualDeliveryKG.RequestedItemTag = new Tag("KmodMiniBrainCore");
//            manualDeliveryKG.refillMass = 1f;
//            manualDeliveryKG.MinimumMass = 1f;
//            manualDeliveryKG.capacity = 1f;
//            KBatchedAnimController kbatchedAnimController = go.AddOrGet<KBatchedAnimController>();
//            kbatchedAnimController.sceneLayer = Grid.SceneLayer.BuildingBack;
//            kbatchedAnimController.fgLayer = Grid.SceneLayer.BuildingFront;


//            go.AddOrGet<Ownable>().slotID = Db.Get().AssignableSlots.GeneShuffler.Id;

//            Prioritizable.AddRef(go);
//        }


//        public override void DoPostConfigureComplete(GameObject go)
//        {

//        }

//        // Token: 0x04000152 RID: 338
//        public const string ID = "KModBuildingBrain";

//        // Token: 0x04000153 RID: 339
//        public const float WATER2OXYGEN_RATIO = 0.888f;

//        // Token: 0x04000154 RID: 340
//        public const float OXYGEN_TEMPERATURE = 343.15f;
//    }
//}