using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

namespace DebuffRoulette
{
    public class EnergyDispersionTableConifg : IBuildingConfig
    {

        public override BuildingDef CreateBuildingDef()
        {
            string text = "EnergyDispersionTableConifg";
            int num = 1;
            int num2 = 2;
            string text2 = "KmodEnergyDispersionTable_kanim";
            int num3 = 10;
            float num4 = 30f;
            float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
            string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
            float num5 = 800f;
            BuildLocationRule buildLocationRule = BuildLocationRule.OnFloor;
            EffectorValues none = NOISE_POLLUTION.NONE;
            BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(text, num, num2, text2, num3, num4, tier, raw_MINERALS, num5, buildLocationRule, BUILDINGS.DECOR.BONUS.TIER0, none, 0.2f);
            buildingDef.DefaultAnimState = "pedestal";
            buildingDef.Floodable = false;
            buildingDef.Overheatable = false;
            buildingDef.ViewMode = OverlayModes.Decor.ID;
            buildingDef.AudioCategory = "Glass";
            buildingDef.AudioSize = "small";
            return buildingDef;
        }

        public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
        {
            go.AddOrGet<Storage>().SetDefaultStoredItemModifiers(new List<Storage.StoredItemModifier>(new Storage.StoredItemModifier[]
            {
            Storage.StoredItemModifier.Seal,
            Storage.StoredItemModifier.Preserve
            }));
            Prioritizable.AddRef(go);

            SingleEntityReceptacle singleEntityReceptacle = go.AddOrGet<SingleEntityReceptacle>();
            singleEntityReceptacle.AddDepositTag("KModEnergyDispersionTableConifg");
            singleEntityReceptacle.occupyingObjectRelativePosition = new Vector3(0f, 1.2f, -1f);


            RangeVisualizer rangeVisualizer = go.AddOrGet<RangeVisualizer>();
            //rangeVisualizer.OriginOffset = new Vector2I(0, 0);
            //rangeVisualizer.RangeMin.x = -10;
            //rangeVisualizer.RangeMin.y = -10;
            //rangeVisualizer.RangeMax.x = 10;
            //rangeVisualizer.RangeMax.y = 10;
            //rangeVisualizer.BlockingTileVisible = true;





            go.AddOrGet<DecorProvider>();
            go.AddOrGet<EnergyDispersionTable>();
            // go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
        }

        public override void DoPostConfigureComplete(GameObject go)
        {
        }

        public const string ID = "EnergyDispersionTableConifg";
    }
}