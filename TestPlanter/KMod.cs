using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using KMod;
using TUNING;

namespace TestPlanter
{
    public class KMod
    {
        public class Patch : UserMod2
        {
            public override void OnLoad(Harmony harmony)
            {
                base.OnLoad(harmony);
                Debug.Log("[TestPlanter] MOD加载成功");
            }
        }




        [HarmonyPatch(typeof(EntityConfigManager), "LoadGeneratedEntities")]
        public class New_Plant_Database_Description
        {
            public static void Prefix()
            {
                KModStringUtils.Add_New_PlantStrings("TestPlanter", "测试植物", "描述", "扩展描述");
                KModStringUtils.Add_New_PlantSeedStrings("TestPlanterSeed", "测试种子", "");
                KModStringUtils.Add_New_FoodStrings("TestFood", "测试食物" , "扩展描述", null);

                // 添加作物类型
                CROPS.CROP_TYPES.Add(new Crop.CropVal("TestFood", 300f, 5, true));
            }
        }















    }
}
