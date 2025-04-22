
using HarmonyLib;
using STRINGS;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MoreFood
{
    public class More_foodPatch
    {
      
        [HarmonyPatch(typeof(EntityConfigManager))]
        [HarmonyPatch("LoadGeneratedEntities")]
        public class EntityConfigManager_LoadGeneratedEntities_Patch
        {
           
            public static void Prefix()
            {
               
                // AddFoodStrings("XiaoHuangya", STRINGS.KFOOD.XIAOHUANGYA.NAME, STRINGS.KFOOD.XIAOHUANGYA.DESC, STRINGS.KFOOD.XIAOHUANGYA.RECIPEDESC);
              
            }
        }


        public static void AddFoodStrings(string foodId, string name, string description, string recipeDescription = null)
        {
            Strings.Add(new string[]
            {
                "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".NAME",
                UI.FormatAsLink(name, foodId)
            });
            Strings.Add(new string[]
            {
                "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".DESC",
                description
            });
            bool flag = recipeDescription != null;
            if (flag)
            {
                Strings.Add(new string[]
                {
                    "STRINGS.ITEMS.FOOD." + foodId.ToUpperInvariant() + ".RECIPEDESC",
                    recipeDescription
                });
            }
        }
    }
}
