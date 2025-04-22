using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using KModTool;

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
                    ModUtil.AddBuildingToPlanScreen("Base", AutomaticHarvestConfig.ID, "Tiles");
                    Db.Get().Techs.Get("HighTempForging").unlockedItemIDs.Add(AutomaticHarvestConfig.ID);
                    KModStringUtils.Add_New_BuildStrings(AutomaticHarvestConfig.ID, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.NAME, STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.DESC,STRINGS.BUILDINGS.AUTOMATICHARVESTCONFIG.EFFECT);
                }
            }
        }












    }
}

