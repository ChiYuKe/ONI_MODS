using HarmonyLib;
using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using TUNING;
using static MiniBox.Config;

namespace MiniBox.PlantConfig.Plant
{
    [HarmonyPatch(typeof(Assets), "SubstanceListHookup")]
    public class PlantPatch
    {
        /// <summary>
        /// 遍历配置的作物列表，根据配置项更新游戏中 CROPS.CROP_TYPES 的内容。
        /// </summary>
        public static void Postfix()
        {
            
            var cropConfigs = new List<(string cropName, string growthKey, string harvestKey)>
            {
                ("BasicPlantFood", "BasicPlantFood_Growth_Time", "BasicPlantFood_Harvest_Quantity"),
                ("PrickleFruit", "PrickleFruit_Growth_Time", "PrickleFruit_Harvest_Quantity"),
                ("SwampFruit", "SwampFruit_Growth_Time", "SwampFruit_Harvest_Quantity"),
                ("Mushroom", "Mushroom_Growth_Time", "Mushroom_Harvest_Quantity"),
                ("ColdWheatSeed", "ColdWheatSeed_Growth_Time", "ColdWheatSeed_Harvest_Quantity"),
                ("SpiceNut", "SpiceNut_Growth_Time", "SpiceNut_Harvest_Quantity"),
                ("BasicFabric", "BasicFabric_Growth_Time", "BasicFabric_Harvest_Quantity"),
                ("SwampLilyFlower", "SwampLilyFlower_Growth_Time", "SwampLilyFlower_Harvest_Quantity"),
                ("GasGrassHarvested", "GasGrassHarvested_Growth_Time", "GasGrassHarvested_Harvest_Quantity"),
                ("WoodLog", "WoodLog_Growth_Time", "WoodLog_Harvest_Quantity"),
                (SimHashes.SugarWater.ToString(), "SugarWater_Growth_Time", "SugarWater_Harvest_Quantity"),
                ("SpaceTreeBranch", "SpaceTreeBranch_Growth_Time", "SpaceTreeBranch_Harvest_Quantity"),
                ("HardSkinBerry", "HardSkinBerry_Growth_Time", "HardSkinBerry_Harvest_Quantity"),
                ("Carrot", "Carrot_Growth_Time", "Carrot_Harvest_Quantity"),
                ("VineFruit", "VineFruit_Growth_Time", "VineFruit_Harvest_Quantity"),
                (SimHashes.OxyRock.ToString(), "OxyRock_Growth_Time", "OxyRock_Harvest_Quantity"),
                ("Lettuce", "Lettuce_Growth_Time", "Lettuce_Harvest_Quantity"),
                ("Kelp", "Kelp_Growth_Time", "Kelp_Harvest_Quantity"),
                ("BeanPlantSeed", "BeanPlantSeed_Growth_Time", "BeanPlantSeed_Harvest_Quantity"),
                ("OxyfernSeed", "OxyfernSeed_Growth_Time", "OxyfernSeed_Harvest_Quantity"),
                ("PlantMeat", "PlantMeat_Growth_Time", "PlantMeat_Harvest_Quantity"),
                ("WormBasicFruit", "WormBasicFruit_Growth_Time", "WormBasicFruit_Harvest_Quantity"),
                ("WormSuperFruit", "WormSuperFruit_Growth_Time", "WormSuperFruit_Harvest_Quantity"),
                ("DewDrip", "DewDrip_Growth_Time", "DewDrip_Harvest_Quantity"),
                ("FernFood", "FernFood_Growth_Time", "FernFood_Harvest_Quantity"),
                (SimHashes.Salt.ToString(), "Salt_Growth_Time", "Salt_Harvest_Quantity"),// 沙盐藤
                (SimHashes.Water.ToString(), "Water_Growth_Time", "Water_Harvest_Quantity"),// 仙水掌
                (SimHashes.Amber.ToString(), "Amber_Growth_Time", "Amber_Harvest_Quantity"),//露饵花
                ("GardenFoodPlantFood", "GardenFoodPlantFood_Growth_Time", "GardenFoodPlantFood_Harvest_Quantity"),//汗甜玉米
                ("Butterfly", "Butterfly_Growth_Time", "Butterfly_Harvest_Quantity") //拟芽
            };

            // 遍历所有作物配置
            for (int i = 0; i < cropConfigs.Count; i++)
            {
                var (cropName, growthKey, harvestKey) = cropConfigs[i];

                float growthTime = GetConfigValue<float>(growthKey);
                int harvestQuantity = GetConfigValue<int>(harvestKey);

                // 如果配置项无效（没配置的植物还有为0或者一些负数），尝试从原数据中读取
                if ((growthTime <= 0 || harvestQuantity <= 0) && i < CROPS.CROP_TYPES.Count)
                {
                    var original = CROPS.CROP_TYPES[i];
                    if (growthTime <= 0) growthTime = original.cropDuration;
                    if (harvestQuantity <= 0) harvestQuantity = original.numProduced;
                }

                var cropVal = new Crop.CropVal(cropName, growthTime, harvestQuantity, true);

                if (i < CROPS.CROP_TYPES.Count)
                    CROPS.CROP_TYPES[i] = cropVal;
                else
                    CROPS.CROP_TYPES.Add(cropVal);
            }
        }

        private static readonly ConfigurationItem config = SingletonOptions<ConfigurationItem>.Instance;

        /// <summary>
        /// 从配置项单例中反射获取指定键对应的配置值。
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="configKey">配置属性名</param>
        /// <returns>读取到的配置值，若失败则返回类型默认值</returns>
        private static T GetConfigValue<T>(string configKey)
        {
            // 通过反射获取属性信息
            var prop = config.GetType().GetProperty(configKey);
            if (prop == null) return default;

            var value = prop.GetValue(config, null);
            if (value == null) return default;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default;
            }
        }
    }
}
