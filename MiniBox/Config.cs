using Microsoft.SqlServer.Server;
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
            //挖掘掉落倍率
            [Option("STRINGS.CONFIGURATIONITEM.MISCCONFIG.DIGGINGDROPRATE.TITLE", "STRINGS.CONFIGURATIONITEM.MISCCONFIG.DIGGINGDROPRATE.TOOLTIP", ConfigStrings.misccconfig.category, Format = "F1")]
            [Limit(0.1f, 100f)]
            [JsonProperty]
            public float DiggingDropRate { get; set; } = 0.5f;

            //复制人睡觉
            [Option("STRINGS.CONFIGURATIONITEM.MISCCONFIG.MINIONSLEEP.TITLE", "STRINGS.CONFIGURATIONITEM.MISCCONFIG.MINIONSLEEP.TOOLTIP", ConfigStrings.misccconfig.category, Format = "F1")]
            [JsonProperty]
            public bool NoWantSleepPatch { get; set; } = false;

            // 擦水
            [Option("STRINGS.CONFIGURATIONITEM.MISCCONFIG.WATERING.TITLE", "STRINGS.CONFIGURATIONITEM.MISCCONFIG.WATERING.TOOLTIP", ConfigStrings.misccconfig.category, Format = "F1")]
            [JsonProperty]
            public bool WateringPatch { get; set; } = false;

            //超级太空服
            [Option("STRINGS.CONFIGURATIONITEM.MISCCONFIG.SUPERSPACESUIT.TITLE", "STRINGS.CONFIGURATIONITEM.MISCCONFIG.SUPERSPACESUIT.TOOLTIP", ConfigStrings.misccconfig.category, Format = "F1")]
            [JsonProperty]
            public bool SuperSpaceSuitPatch { get; set; } = false;





            //轨道容量
            [Option("STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.TITLE", "STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.TOOLTIP", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float TransportTrackMaxCapacity { get; set; } = 20f;

            //运输存放器&运输装载器
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.SOLIDCONDUITOUTBOX.CAPACITY", "", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float SolidConduitOutboxCapacityKg { get; set; } = 2000f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.SOLIDCONDUITINBOX.CAPACITY", "", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(20.0, 10000.0)]
            [JsonProperty]
            public float SolidConduitInboxCapacityKg { get; set; } = 2000f;

            //储物箱
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.STORAGELOCKER.CAPACITY", "kg", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(0, 10000000)]
            [JsonProperty]
            public float StorageLockerCapacityKg { get; set; } = 20000f;


            //气库
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.GASRESERVOIR.CAPACITY", "kg", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float GasReservoirCapacityKg { get; set; } = 20000f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.GASRESERVOIR.FOUNDATION", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool GasReservoirFoundation { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.GASRESERVOIR.OVERHEATABLE", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool GasReservoirOverheatable { get; set; } = true;




            //液库
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.LIQUIDRESERVOIR.CAPACITY", "kg", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float LiquidReservoirCapacityKg { get; set; } = 20000f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.LIQUIDRESERVOIR.FOUNDATION", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool LiquidReservoirFoundation { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.LIQUIDRESERVOIR.OVERHEATABLE", "", ConfigStrings.builddings.category, Format = "F0")]
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
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.POWERTRANSFORMERSMALL.HEATGENERATION", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool PowerTransformerSmallHeatGeneration { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.POWERTRANSFORMER.HEATGENERATION", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool PowerTransformerHeatGeneration { get; set; } = true;











            //电解器制氧
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.TITLE", "kg", ConfigStrings.builddings.category, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float MineralDeoxidizerEmissionValues { get; set; } = 0.5f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.OUTPUTTEMPERATURE", "摄氏度", ConfigStrings.builddings.category, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float MineralDeoxidizerOutputTemperature { get; set; } = 30f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.ENERGYCONSUMPTIONWHENACTIVE", "W", ConfigStrings.builddings.category, Format = "F0")]
            [Limit(0, 500)]
            [JsonProperty]
            public float MineralDeoxidizerPowerConsumption { get; set; } = 120f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.FLOODABLE", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerFloodable { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.OVERHEATABLE", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerOverheatable { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.MINERALDEOXIDIZER.HEATGENERATION", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool MineralDeoxidizerHeatGeneration { get; set; } = true;




            //冰箱
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.ENERGYCONSUMPTIONWHENACTIVE", "W", ConfigStrings.builddings.category, Format = "F0")]
            [Limit(0, 500)]
            [JsonProperty]
            public float RefrigeratorEmissionValues { get; set; } = 120f;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.FLOODABLE", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool RefrigeratorFloodable { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.OVERHEATABLE", "", ConfigStrings.builddings.category, Format = "F0")]
            [JsonProperty]
            public bool RefrigeratorOverheatable { get; set; } = true;
            [Option("STRINGS.CONFIGURATIONITEM.BUILDDINGS.REFRIGERATOR.CAPACITY", "Kg", ConfigStrings.builddings.capacity, Format = "F0")]
            [Limit(0, 1000000)]
            [JsonProperty]
            public float RefrigeratorCapacityKg { get; set; } = 100f;





            // 植物配置项
            // === 米虱木 (BasicPlantFood) 600/1===
            [Option(ConfigStrings.Plant.BasicPlantFoodTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1800)]
            [JsonProperty]
            public float BasicPlantFood_Growth_Time { get; set; } = 1800f;
            [Option(ConfigStrings.Plant.BasicPlantFoodTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int BasicPlantFood_Harvest_Quantity { get; set; } = 1;

            // === 毛刺花 (PrickleFruit) ===
            [Option(ConfigStrings.Plant.PrickleFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 3600)]
            [JsonProperty]
            public float PrickleFruit_Growth_Time { get; set; } = 3600f;
            [Option(ConfigStrings.Plant.PrickleFruitTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int PrickleFruit_Harvest_Quantity { get; set; } = 1;

            // === 沼浆笼 (SwampFruit) ===
            [Option(ConfigStrings.Plant.SwampFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 3960)]
            [JsonProperty]
            public float SwampFruit_Growth_Time { get; set; } = 3960f;
            [Option(ConfigStrings.Plant.SwampFruitTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int SwampFruit_Harvest_Quantity { get; set; } = 1;

            // === 夜幕菇 (Mushroom) ===
            [Option(ConfigStrings.Plant.MushroomTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 4500)]
            [JsonProperty]
            public float Mushroom_Growth_Time { get; set; } = 4500f;
            [Option(ConfigStrings.Plant.MushroomTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int Mushroom_Harvest_Quantity { get; set; } = 1;

            // === 冰霜小麦 (ColdWheatSeed) ===
            [Option(ConfigStrings.Plant.ColdWheatSeedTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 10800)]
            [JsonProperty]
            public float ColdWheatSeed_Growth_Time { get; set; } = 10800f;
            [Option(ConfigStrings.Plant.ColdWheatSeedTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int ColdWheatSeed_Harvest_Quantity { get; set; } = 18;

            // === 火椒藤 (SpiceNut) ===
            [Option(ConfigStrings.Plant.SpiceNutTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 4800)]
            [JsonProperty]
            public float SpiceNut_Growth_Time { get; set; } = 4800f;
            [Option(ConfigStrings.Plant.SpiceNutTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int SpiceNut_Harvest_Quantity { get; set; } = 4;

            // === 顶针芦苇 (BasicFabric) ===
            [Option(ConfigStrings.Plant.BasicFabricTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1200)]
            [JsonProperty]
            public float BasicFabric_Growth_Time { get; set; } = 1200f;
            [Option(ConfigStrings.Plant.BasicFabricTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int BasicFabric_Harvest_Quantity { get; set; } = 1;

            // === 芳香百合 (SwampLilyFlower) ===
            [Option(ConfigStrings.Plant.SwampLilyFlowerTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 7200)]
            [JsonProperty]
            public float SwampLilyFlower_Growth_Time { get; set; } = 7200f;
            [Option(ConfigStrings.Plant.SwampLilyFlowerTitle, "", ConfigStrings.Plant.NumProducedCategory)]
            [Limit(0, 10000)]
            [JsonProperty]
            public int SwampLilyFlower_Harvest_Quantity { get; set; } = 2;

            // === 木材 (WoodLog) ===
            [Option(ConfigStrings.Plant.WoodLogTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 2700)]
            [JsonProperty]
            public float WoodLog_Growth_Time { get; set; } = 2700f;
            [Option(ConfigStrings.Plant.WoodLogTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WoodLog_Harvest_Quantity { get; set; } = 300;

            // === 蜜露 (SugarWater) ===
            [Option(ConfigStrings.Plant.SugarWaterTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 150)]
            [JsonProperty]
            public float SugarWater_Growth_Time { get; set; } = 150f; 

            [Option(ConfigStrings.Plant.SugarWaterTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SugarWater_Harvest_Quantity { get; set; } = 20;

            // === 糖心树木材 (SpaceTreeBranch) ===
            [Option(ConfigStrings.Plant.SpaceTreeBranchTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 2700)]
            [JsonProperty]
            public float SpaceTreeBranch_Growth_Time { get; set; } = 2700f;

            [Option(ConfigStrings.Plant.SpaceTreeBranchTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float SpaceTreeBranch_Harvest_Quantity { get; set; } = 1;

            // === 硬皮浆果 (HardSkinBerry) ===
            [Option(ConfigStrings.Plant.HardSkinBerryTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1800)]
            [JsonProperty]
            public float HardSkinBerry_Growth_Time { get; set; } = 1800f;

            [Option(ConfigStrings.Plant.HardSkinBerryTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float HardSkinBerry_Harvest_Quantity { get; set; } = 1;

            // === 胡萝卜 (Carrot) ===
            [Option(ConfigStrings.Plant.CarrotTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 5400)]
            [JsonProperty]
            public float Carrot_Growth_Time { get; set; } = 5400f;

            [Option(ConfigStrings.Plant.CarrotTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Carrot_Harvest_Quantity { get; set; } = 1;


            // === 藤蔓果实 (VineFruit) ===
            [Option(ConfigStrings.Plant.OxyRockTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1200)]
            [JsonProperty]
            public float OxyRock_Growth_Time { get; set; } = 1200f;

            [Option(ConfigStrings.Plant.OxyRockTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float OxyRock_Harvest_Quantity { get; set; } =  2 * Mathf.RoundToInt(17.76f);

            // === 生菜 (Lettuce) ===
            [Option(ConfigStrings.Plant.LettuceTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 7200)]
            [JsonProperty]
            public float Lettuce_Growth_Time { get; set; } = 7200f;

            [Option(ConfigStrings.Plant.LettuceTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Lettuce_Harvest_Quantity { get; set; } = 12;

            // === 海带 (Kelp) ===
            [Option(ConfigStrings.Plant.BeanPlantSeedTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 12600)]
            [JsonProperty]
            public float BeanPlantSeed_Growth_Time { get; set; } = 12600f;
            [Option(ConfigStrings.Plant.BeanPlantSeedTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float BeanPlantSeed_Harvest_Quantity { get; set; } = 12;


            [Option(ConfigStrings.Plant.PlantMeatTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 18000)]
            [JsonProperty]
            public float PlantMeat_Growth_Time { get; set; } = 18000f;
            [Option(ConfigStrings.Plant.PlantMeatTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float PlantMeat_Harvest_Quantity { get; set; } = 10;


            // === 贫瘠虫果 ===
            [Option(ConfigStrings.Plant.WormBasicFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 2400)]
            [JsonProperty]
            public float WormBasicFruit_Growth_Time { get; set; } = 2400f;
            [Option(ConfigStrings.Plant.WormBasicFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormBasicFruit_Harvest_Quantity { get; set; } = 1;

            // === 虫果 ===
            [Option(ConfigStrings.Plant.WormSuperFruitTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 4800)]
            [JsonProperty]
            public float WormSuperFruit_Growth_Time { get; set; } = 4800f;
            [Option(ConfigStrings.Plant.WormSuperFruitTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float WormSuperFruit_Harvest_Quantity { get; set; } = 8;



            // === 露珠藤 ===
            [Option(ConfigStrings.Plant.DewDripTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1200)]
            [JsonProperty]
            public float DewDrip_Growth_Time { get; set; } = 1200f;
            [Option(ConfigStrings.Plant.DewDripTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float DewDrip_Harvest_Quantity { get; set; } = 1;

            // === 巨蕨 ===
            [Option(ConfigStrings.Plant.FernFoodTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 5400)]
            [JsonProperty]
            public float FernFood_Growth_Time { get; set; } = 5400f;
            [Option(ConfigStrings.Plant.FernFoodTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float FernFood_Harvest_Quantity { get; set; } = 36;

            // === 沙盐藤 ===
            [Option(ConfigStrings.Plant.SaltTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 3600)]
            [JsonProperty]
            public float Salt_Growth_Time { get; set; } = 3600f;
            [Option(ConfigStrings.Plant.WaterTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Salt_Harvest_Quantity { get; set; } = 65;

            // === 仙水掌 ===
            [Option(ConfigStrings.Plant.WaterTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 6000)]
            [JsonProperty]
            public float Water_Growth_Time { get; set; } = 6000f;
            [Option(ConfigStrings.Plant.WaterTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Water_Harvest_Quantity { get; set; } = 350;

            // === 露饵花 ===
            [Option(ConfigStrings.Plant.AmberTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 7200)]
            [JsonProperty]
            public float Amber_Growth_Time { get; set; } = 7200f;
            [Option(ConfigStrings.Plant.AmberTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float Amber_Harvest_Quantity { get; set; } = 264;

            // === 汗甜玉米 800/1===
            [Option(ConfigStrings.Plant.GardenFoodPlantFoodTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 1800)]
            [JsonProperty]
            public float GardenFoodPlantFood_Growth_Time { get; set; } = 1800f;
            [Option(ConfigStrings.Plant.GardenFoodPlantFoodTitle, "", ConfigStrings.Plant.NumProducedCategory, Format = "F0")]
            [Limit(0, 100000)]
            [JsonProperty]
            public float GardenFoodPlantFood_Harvest_Quantity { get; set; } = 1;

            // === 拟芽 ===
            [Option(ConfigStrings.Plant.ButterflyTitle, "", ConfigStrings.Plant.CropDurationCategory, Format = "F1")]
            [Limit(0, 3000)]
            [JsonProperty]
            public float Butterfly_Growth_Time { get; set; } = 3000f;

        }

        public static class ConfigStrings
        {
            public static class builddings 
            {
                public const string capacity = "STRINGS.CONFIGURATIONITEM.TRANSPORTROUTECONFIG.TRANSPORTTRACK.CATEGORY";
                public const string category = "STRINGS.CONFIGURATIONITEM.BUILDDINGS.CATEGORY";
            }
            public static class misccconfig
            {
                public const string category = "STRINGS.CONFIGURATIONITEM.MISCCONFIG.CATEGORY";
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
                public const string WoodLogTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WOODLOG";
                public const string SugarWaterTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SUGARWATER";
                public const string SpaceTreeBranchTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SPACETREEBRANCH";
                public const string HardSkinBerryTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.HARDSKINBERRY";
                public const string CarrotTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.CARROT";
                public const string VineFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.VINEFRUIT";
                public const string OxyRockTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.OXYROCK";
                public const string LettuceTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.LETTUCE";
                public const string KelpTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.KELP";
                public const string BeanPlantSeedTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.BEANPLANTSEED";
                public const string OxyfernSeedTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.OXYFERNSEED";
                public const string PlantMeatTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.PLANTMEAT";
                public const string WormBasicFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WORMSBASICFRUIT";
                public const string WormSuperFruitTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WORMSUPERFRUIT";
                public const string DewDripTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.DEWDRIP";
                public const string FernFoodTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.FERNFOOD";
                public const string SaltTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.SALT";
                public const string WaterTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.WATER";
                public const string AmberTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.AMBER";
                public const string GardenFoodPlantFoodTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.GARDENFOODPLANTFOOD";
                public const string ButterflyTitle = "STRINGS.CONFIGURATIONITEM.PLANTCONFIG.PLANT.BUTTERFLY";
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
