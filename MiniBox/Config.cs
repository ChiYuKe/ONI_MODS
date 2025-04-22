using Newtonsoft.Json;
using PeterHan.PLib.Options;
using UnityEngine;


namespace MiniBox
{
    internal class Config
    {
        [JsonObject(MemberSerialization.OptIn)]
        [ConfigFile("MiniBox.json", true, true)]
        [RestartRequired]
        public class ConfigurationItem : SingletonOptions<ConfigurationItem>
        {
            //轨道容量
            [Option(ConfigStrings.TransportTrack.TransportTrackTitle, ConfigStrings.TransportTrack.TransportTrackTooltip, ConfigStrings.TransportTrack.TransportTrackCategory, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float TransportTrackMaxCapacity { get; set; } = 2000f;

            //运输存放器&运输装载器
            [Option(ConfigStrings.TransportTrack.TransportTrackTitle, ConfigStrings.TransportTrack.TransportTrackTooltip, ConfigStrings.TransportTrack.TransportTrackCategory, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float SolidConduitOutboxCapacityKg { get; set; } = 2000f;
            [Option(ConfigStrings.TransportTrack.TransportTrackTitle, ConfigStrings.TransportTrack.TransportTrackTooltip, ConfigStrings.TransportTrack.TransportTrackCategory, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float SolidConduitInboxCapacityKg { get; set; } = 2000f;



            //储物箱
            [Option(ConfigStrings.buildings.storagelocker.capacitytitle, "kg", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 10000000)]
            [JsonProperty]
            public float StorageLockerCapacityKg { get; set; } = 20000f;


            //气库
            [Option(ConfigStrings.buildings.storagelocker.capacitytitle, "kg", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 10000000)]
            [JsonProperty]
            public float GasReservoirCapacityKg { get; set; } = 20000f;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool GasReservoirFoundation { get; set; } = false;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool GasReservoirOverheatable { get; set; } = true;




            //液库
            [Option(ConfigStrings.buildings.storagelocker.capacitytitle, "kg", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 10000000)]
            [JsonProperty]
            public float LiquidReservoirCapacityKg { get; set; } = 20000f;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool LiquidReservoirFoundation { get; set; } = false;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool LiquidReservoirOverheatable { get; set; } = true;







            // 电线配置项
            [Option(ConfigStrings.Wiring.WiresTitle, "KW", ConfigStrings.Wiring.WiringCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float Wires { get; set; } = 1f;
            [Option(ConfigStrings.Wiring.ConductorsTitle, "KW", ConfigStrings.Wiring.WiringCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float Conductors { get; set; } = 2f;
            [Option(ConfigStrings.Wiring.HighLoadWiresTitle, "KW", ConfigStrings.Wiring.WiringCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float HighLoadWires { get; set; } = 20f;
            [Option(ConfigStrings.Wiring.HighLoadConductorsTitle, "KW", ConfigStrings.Wiring.WiringCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float HighLoadConductors { get; set; } = 50f;



            //小变压器&大变压器
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool PowerTransformerSmallHeatGeneration { get; set; } = true;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool PowerTransformerHeatGeneration { get; set; } = true;











            //藻类制氧
            [Option(ConfigStrings.buildings.mineraldeoxidizer.title, "kg", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float MineralDeoxidizerEmissionValues { get; set; } = 0.5f;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.outputtemperaturetitle, "摄氏度", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float MineralDeoxidizerOutputTemperature { get; set; } = 30f;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.energyconsumptionwhenactivetitle, "W", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 500)]
            [JsonProperty]
            public float MineralDeoxidizerPowerConsumption { get; set; } = 120f;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.floodabletitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerFloodable { get; set; } = true;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.overheatabletitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerOverheatable { get; set; } = true;
            [Option(ConfigStrings.buildings.mineraldeoxidizer.heatgenerationtitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerHeatGeneration { get; set; } = true;




            //冰箱
            [Option(ConfigStrings.buildings.refrigerator.energyconsumptionwhenactivetitle, "W", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 500)]
            [JsonProperty]
            public float RefrigeratorEmissionValues { get; set; } = 120f;
            [Option(ConfigStrings.buildings.refrigerator.floodabletitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool RefrigeratorFloodable { get; set; } = true;
            [Option(ConfigStrings.buildings.refrigerator.overheatabletitle, "", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [JsonProperty]
            public bool RefrigeratorOverheatable { get; set; } = true;
            [Option(ConfigStrings.buildings.refrigerator.capacitytitle, "Kg", ConfigStrings.buildings.buildingsCategory, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float RefrigeratorCapacityKg { get; set; } = 100f;





            // 植物配置项
            [Option(ConfigStrings.Plant.BasicPlantFoodTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BasicPlantFood_Growth_Time { get; set; } = 1800f;
            [Option(ConfigStrings.Plant.BasicPlantFoodTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BasicPlantFood_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.PrickleFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float PrickleFruit_Growth_Time { get; set; } = 3600f;
            [Option(ConfigStrings.Plant.PrickleFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float PrickleFruit_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.SwampFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SwampFruit_Growth_Time { get; set; } = 3960f;
            [Option(ConfigStrings.Plant.SwampFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SwampFruit_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.MushroomTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Mushroom_Growth_Time { get; set; } = 4500f;
            [Option(ConfigStrings.Plant.MushroomTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Mushroom_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.ColdWheatSeedTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float ColdWheatSeed_Growth_Time { get; set; } = 10800f;
            [Option(ConfigStrings.Plant.ColdWheatSeedTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float ColdWheatSeed_Harvest_Quantity { get; set; } = 18;

            [Option(ConfigStrings.Plant.SpiceNutTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SpiceNut_Growth_Time { get; set; } = 4800f;
            [Option(ConfigStrings.Plant.SpiceNutTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SpiceNut_Harvest_Quantity { get; set; } = 4;

            [Option(ConfigStrings.Plant.BasicFabricTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BasicFabric_Growth_Time { get; set; } = 1200f;
            [Option(ConfigStrings.Plant.BasicFabricTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BasicFabric_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.SwampLilyFlowerTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SwampLilyFlower_Growth_Time { get; set; } = 7200f;
            [Option(ConfigStrings.Plant.SwampLilyFlowerTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SwampLilyFlower_Harvest_Quantity { get; set; } = 2;

            [Option(ConfigStrings.Plant.GasGrassHarvestedTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float GasGrassHarvested_Growth_Time { get; set; } = 2400f;
            [Option(ConfigStrings.Plant.GasGrassHarvestedTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float GasGrassHarvested_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.WoodLogTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WoodLog_Growth_Time { get; set; } = 2700f;
            [Option(ConfigStrings.Plant.WoodLogTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WoodLog_Harvest_Quantity { get; set; } = 300;

            [Option(ConfigStrings.Plant.SugarWaterTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SugarWater_Growth_Time { get; set; } = 150f; 

            [Option(ConfigStrings.Plant.SugarWaterTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SugarWater_Harvest_Quantity { get; set; } = 20;

            [Option(ConfigStrings.Plant.SpaceTreeBranchTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SpaceTreeBranch_Growth_Time { get; set; } = 2700f;

            [Option(ConfigStrings.Plant.SpaceTreeBranchTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SpaceTreeBranch_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.HardSkinBerryTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float HardSkinBerry_Growth_Time { get; set; } = 1800f;

            [Option(ConfigStrings.Plant.HardSkinBerryTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float HardSkinBerry_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.CarrotTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Carrot_Growth_Time { get; set; } = 5400f;

            [Option(ConfigStrings.Plant.CarrotTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Carrot_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.OxyRockTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float OxyRock_Growth_Time { get; set; } = 1200f;

            [Option(ConfigStrings.Plant.OxyRockTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float OxyRock_Harvest_Quantity { get; set; } =  2 * Mathf.RoundToInt(17.76f);

            [Option(ConfigStrings.Plant.LettuceTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Lettuce_Growth_Time { get; set; } = 7200f;

            [Option(ConfigStrings.Plant.LettuceTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Lettuce_Harvest_Quantity { get; set; } = 12;

            [Option(ConfigStrings.Plant.BeanPlantSeedTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BeanPlantSeed_Growth_Time { get; set; } = 12600f;

            [Option(ConfigStrings.Plant.BeanPlantSeedTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BeanPlantSeed_Harvest_Quantity { get; set; } = 12;

            [Option(ConfigStrings.Plant.PlantMeatTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float PlantMeat_Growth_Time { get; set; } = 18000f;

            [Option(ConfigStrings.Plant.PlantMeatTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float PlantMeat_Harvest_Quantity { get; set; } = 10;

            [Option(ConfigStrings.Plant.WormBasicFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormBasicFruit_Growth_Time { get; set; } = 2400f;

            [Option(ConfigStrings.Plant.WormBasicFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormBasicFruit_Harvest_Quantity { get; set; } = 1;

            [Option(ConfigStrings.Plant.WormSuperFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormSuperFruit_Growth_Time { get; set; } = 4800f;

            [Option(ConfigStrings.Plant.WormSuperFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormSuperFruit_Harvest_Quantity { get; set; } = 8;

        }

        // 统一管理字符串的静态类
        public static class ConfigStrings
        {

            public static class buildings
            {
                public const string buildingsCategory = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.CATEGORY";

                public static class storagelocker
                {
                    public const string capacitytitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.STORAGELOCKER.CAPACITY";
                }
                public static class mineraldeoxidizer
                {
                    public const string title = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.TITLE";
                    public const string energyconsumptionwhenactivetitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.ENERGYCONSUMPTIONWHENACTIVE";
                    public const string floodabletitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.FLOODABLE";
                    public const string overheatabletitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.OVERHEATABLE";
                    public const string heatgenerationtitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.HEATGENERATION";
                    public const string outputtemperaturetitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.OUTPUTTEMPERATURE";
                }
                public static class refrigerator
                {
                    public const string energyconsumptionwhenactivetitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.ENERGYCONSUMPTIONWHENACTIVE";
                    public const string floodabletitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.FLOODABLE";
                    public const string overheatabletitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.OVERHEATABLE";
                    public const string capacitytitle = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.CAPACITY";
                }



            }

                public static class Plant
            {
                public const string CropDurationCategory = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.CROPDURATION";
                public const string NumProducedCategory = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.NUMPRODUCED";

                public const string BasicPlantFoodTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.BASICPLANTFOOD";
                public const string PrickleFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.PRICKLEFRUIT";
                public const string SwampFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SWAMPFRUIT";
                public const string MushroomTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.MUSHROOM";
                public const string ColdWheatSeedTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.COLDWHEATSEED";
                public const string SpiceNutTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SPICENUT";
                public const string BasicFabricTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.BASICFABRIC";
                public const string SwampLilyFlowerTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SWAMPLILYFLOWER";
                public const string GasGrassHarvestedTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.GASGRASSHARVESTED";
                public const string WoodLogTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WOODLOG";
                public const string SugarWaterTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SUGARWATER";
                public const string SpaceTreeBranchTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SPACETREEBRANCH";
                public const string HardSkinBerryTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.HARDSKINBERRY";
                public const string CarrotTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.CARROT";
                public const string OxyRockTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.OXYROCK";
                public const string LettuceTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.LETTUCE";
                public const string BeanPlantSeedTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.BEANPLANTSEED";
                public const string PlantMeatTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.PLANTMEAT";
                public const string WormBasicFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WORMSBASICFRUIT";
                public const string WormSuperFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WORMSUPERFRUIT";

            }
            public static class TransportTrack
            {
                public const string TransportTrackTitle = "STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.TITLE";
                public const string TransportTrackTooltip = "STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.TOOLTIP";
                public const string TransportTrackCategory = "STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.CATEGORY";

            }
            public static class Wiring
            {

                public const string WiresTitle = "STRINGS.CONFIGURATIONITEM.POWERCONFIG.WIRING.WIRES";
                public const string ConductorsTitle = "STRINGS.CONFIGURATIONITEM.POWERCONFIG.WIRING.CONDUCTORS";
                public const string HighLoadWiresTitle = "STRINGS.CONFIGURATIONITEM.POWERCONFIG.WIRING.HIGHLOADWIRES";
                public const string HighLoadConductorsTitle = "STRINGS.CONFIGURATIONITEM.POWERCONFIG.WIRING.HIGHLOADCONDUCTORS";
                public const string WiringCategory = "STRINGS.CONFIGURATIONITEM.POWERCONFIG.WIRING.CATEGORY";

            }
        }
    }
}
