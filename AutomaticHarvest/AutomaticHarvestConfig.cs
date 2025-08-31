using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace AutomaticHarvest
{
    public class AutomaticHarvestConfig : IBuildingConfig
    {
       
        public override BuildingDef CreateBuildingDef()
        {
            string text = "AutomaticHarvestConfig";
            int num = 1;
            int num2 = 1;
            string text2 = "thermalblock_kanim";
            int num3 = 30;
            float num4 = 120f;
            float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER5;
            string[] any_BUILDABLE = MATERIALS.ANY_BUILDABLE;
            float num5 = 1600f;
            BuildLocationRule buildLocationRule = BuildLocationRule.NotInTiles;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(text, num, num2, text2, num3, num4, tier, any_BUILDABLE, num5, buildLocationRule, DECOR.NONE, none, 0.2f);
            buildingDef.Floodable = false;
            buildingDef.Entombable = false;
            buildingDef.Overheatable = false;
            buildingDef.AudioCategory = "Metal";
            buildingDef.BaseTimeUntilRepair = -1f;
            // buildingDef.ViewMode = OverlayModes.Temperature.ID;

            buildingDef.OutputConduitType = ConduitType.Solid;
            buildingDef.UtilityOutputOffset = new CellOffset(0, 0);
            buildingDef.DefaultAnimState = "off";
            buildingDef.ObjectLayer = ObjectLayer.Backwall;
            buildingDef.SceneLayer = Grid.SceneLayer.Backwall;
            buildingDef.ReplacementLayer = ObjectLayer.ReplacementBackwall;
            buildingDef.ReplacementCandidateLayers = new List<ObjectLayer>
        {
            ObjectLayer.FoundationTile,
            ObjectLayer.Backwall
        };
            buildingDef.ReplacementTags = new List<Tag>
        {
            GameTags.FloorTiles,
            GameTags.Backwall
        };
            return buildingDef;
        }


        public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
        {
           AddVisualizer(go, true);
        } 


        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {    
            BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);

            GeneratedBuildings.MakeBuildingAlwaysOperational(go);
           

            AddVisualizer(go, false);


            go.AddComponent<AutoPlantHarvester>();
            //List<Tag> list = new List<Tag>();
            //list.AddRange(STORAGEFILTERS.STORAGE_LOCKERS_STANDARD);
            //list.AddRange(STORAGEFILTERS.FOOD);


            Storage storage = go.AddOrGet<Storage>();
            //storage.capacityKg = 100000f;
            //storage.showInUI = true;
            //storage.showDescriptor = true;
            //storage.storageFilters = STORAGEFILTERS.STORAGE_LOCKERS_STANDARD;
            //storage.allowItemRemoval = false;
            //storage.onlyTransferFromLowerPriority = true;
           
            //storage.showCapacityAsMainStatus = true;

            //go.AddOrGet<SolidConduitInbox>();
            //go.AddOrGet<SolidConduitDispenser>();


        }

       
        public override void DoPostConfigureComplete(GameObject go)
        {
            //AddVisualizer(go, false);
            //go.AddComponent<AutoPlantHarvester>();


            KPrefabID component = go.GetComponent<KPrefabID>();
            component.AddTag(GameTags.Backwall, false);



            //component.prefabSpawnFn += delegate (GameObject game_object)
            //{
            //    HandleVector<int>.Handle handle = GameComps.StructureTemperatures.GetHandle(game_object);
            //    StructureTemperaturePayload payload = GameComps.StructureTemperatures.GetPayload(handle);
            //    int num = Grid.PosToCell(game_object);
            //    payload.OverrideExtents(new Extents(num, AutomaticHarvestConfig.overrideOffsets, Extents.BoundsCheckCoords));
            //    GameComps.StructureTemperatures.SetPayload(handle, ref payload);
            //};
        }


  



        private static void AddVisualizer(GameObject prefab, bool movable)
        {
            RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
            rangeVisualizer.OriginOffset = new Vector2I(0, 0);
            rangeVisualizer.RangeMin.x = -8;
            rangeVisualizer.RangeMin.y = -3;
            rangeVisualizer.RangeMax.x = 8;
            rangeVisualizer.RangeMax.y = 3;
            rangeVisualizer.BlockingTileVisible = true;
        }

        public const string ID = "AutomaticHarvestConfig";

        //private static readonly CellOffset[] overrideOffsets = new CellOffset[]
        //{
        //new CellOffset(-1, -1),
        //new CellOffset(1, -1),
        //new CellOffset(-1, 1),
        //new CellOffset(1, 1)
        //};
    }
}