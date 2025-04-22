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
        private static void Postfix()
        {
            // 作物类型和对应的配置项
            var cropConfigs = new Dictionary<int, (string cropName, string growthTimeConfig, string harvestQuantityConfig)>
            {
                { 0, ("BasicPlantFood", "BasicPlantFood_Growth_Time", "BasicPlantFood_Harvest_Quantity") }, // 米虱木
                { 1, ("PrickleFruit", "PrickleFruit_Growth_Time", "PrickleFruit_Harvest_Quantity") }, // 毛刺花
                { 2, ("SwampFruit", "SwampFruit_Growth_Time", "SwampFruit_Harvest_Quantity") }, // 沼浆笼
                { 3, ("Mushroom", "Mushroom_Growth_Time", "Mushroom_Harvest_Quantity") }, // 夜幕菇
                { 4, ("ColdWheatSeed", "ColdWheatSeed_Growth_Time", "ColdWheatSeed_Harvest_Quantity") }, // 冰霜小麦
                { 5, ("SpiceNut", "SpiceNut_Growth_Time", "SpiceNut_Harvest_Quantity") }, // 火椒藤
                { 6, ("BasicFabric", "BasicFabric_Growth_Time", "BasicFabric_Harvest_Quantity") }, // 顶针芦苇
                { 7, ("SwampLilyFlower", "SwampLilyFlower_Growth_Time", "SwampLilyFlower_Harvest_Quantity") }, // 芳香百合
                { 8, ("GasGrassHarvested", "GasGrassHarvested_Growth_Time", "GasGrassHarvested_Harvest_Quantity") }, // 释气草
                { 9, ("WoodLog", "WoodLog_Growth_Time", "WoodLog_Harvest_Quantity") }, // 乔木树
                { 10, (SimHashes.WoodLog.ToString(), "WoodLog_Growth_Time", "WoodLog_Harvest_Quantity") },
                { 11, (SimHashes.SugarWater.ToString(), "SugarWater_Growth_Time", "SugarWater_Harvest_Quantity") }, // 糖心树
                { 12, ("SpaceTreeBranch", "SpaceTreeBranch_Growth_Time", "SpaceTreeBranch_Harvest_Quantity") }, // 糖心树枝
                { 13, ("HardSkinBerry", "HardSkinBerry_Growth_Time", "HardSkinBerry_Harvest_Quantity") }, // 刺壳果灌木
                { 14, ("Carrot", "Carrot_Growth_Time", "Carrot_Harvest_Quantity") }, // 羽葉果薯
                { 15, (SimHashes.OxyRock.ToString(), "OxyRock_Growth_Time", "OxyRock_Harvest_Quantity") }, // 气囊芦荟
                { 16, ("Lettuce", "Lettuce_Growth_Time", "Lettuce_Harvest_Quantity") }, // 水草
                { 17, ("BeanPlantSeed", "BeanPlantSeed_Growth_Time", "BeanPlantSeed_Harvest_Quantity") }, // 小吃芽
                { 18, ("PlantMeat", "PlantMeat_Growth_Time", "PlantMeat_Harvest_Quantity") }, // 土星动物捕草
                { 19, ("WormBasicFruit", "WormBasicFruit_Growth_Time", "WormBasicFruit_Harvest_Quantity") }, // 贫瘠虫果
                { 20, ("WormSuperFruit", "WormSuperFruit_Growth_Time", "WormSuperFruit_Harvest_Quantity") } // 虫果
            };

            // 遍历字典，更新CROPS.CROP_TYPES
            foreach (var cropConfig in cropConfigs)
            {
                string cropName = cropConfig.Value.cropName;
                string growthTimeConfig = cropConfig.Value.growthTimeConfig;
                string harvestQuantityConfig = cropConfig.Value.harvestQuantityConfig;

                // 获取配置项的值，并安全地进行类型转换
                float growthTime = GetConfigValue<float>(growthTimeConfig);
                int harvestQuantity = GetConfigValue<int>(harvestQuantityConfig);

                // 更新作物列表
                CROPS.CROP_TYPES[cropConfig.Key] = new Crop.CropVal(cropName, growthTime, harvestQuantity, true);
            }
        }

        // 泛型方法：获取配置项的值并进行类型转换
        private static T GetConfigValue<T>(string configKey)
        {
            var value = SingletonOptions<ConfigurationItem>.Instance.GetType()
                            .GetProperty(configKey)
                            .GetValue(SingletonOptions<ConfigurationItem>.Instance, null);

            if (value == null)
            {
                // 如果值为 null，返回默认值
                return default(T);
            }

            try
            {
                // 尝试转换为指定类型
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (InvalidCastException)
            {
                // 捕获类型转换异常，返回默认值
                return default(T);
            }
        }
    }
}
