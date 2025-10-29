using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;

namespace AutomaticHarvest
{
    public class KModPatch
    {


        public static class Buildpatch
        {
            [HarmonyPatch(typeof(GeneratedBuildings), "LoadGeneratedBuildings")]
            public static class ThermalBlock_F_1LoadGeneratedBuildings_Patch
            {

                public static void Prefix()
                {
                    ModUtil.AddBuildingToPlanScreen("Base", AutomaticHarvestConfig.ID, "Tiles");  // 添加到建筑菜单 ，"Base"是基础菜单，"Tiles"是子菜单 具体可定位到 TUNING.BUILDING.PLANORDER 查看其结构
                    Db.Get().Techs.Get("HighTempForging").unlockedItemIDs.Add(AutomaticHarvestConfig.ID); // 使其可研究
                    KModStringUtils.Add_New_BuildStrings(AutomaticHarvestConfig.ID, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.NAME, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.DESC,STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.EFFECT);
                }
            }
        }












    }
}

