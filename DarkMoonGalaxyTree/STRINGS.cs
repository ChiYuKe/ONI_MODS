using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI = STRINGS.UI;


namespace DarkMoonGalaxy
{
    public class STRINGS
    {

        public class SPECIES
        {
            public class DARKMOONGALAXY
            {
                public static LocString NAME = UI.FormatAsLink("食碳草", DarkMoonGalaxyConfig.Id);
                public static LocString DESC = "一种以二氧化碳为食的植物";
                public static LocString DOMESTICATEDDESC = $"一种喜爱生活在二氧化碳环境下的" + UI.FormatAsLink("植物", "PLANTS") + "，它会长出可食用的" + UI.FormatAsLink(DARKMOONGALAXYFRUIT.NAME, DarkMoonGalaxyFruitConfig.Id.ToUpperInvariant()) + "，并释放可以呼吸的氧气。";
            }
            public class DARKMOONGALAXYSEED
            {
                public static LocString NAME = UI.FormatAsLink("食碳草种子", DarkMoonGalaxyConfig.Id);
                public static LocString DESC = "食碳草的种子可以种植";
                public static LocString DOMESTICATEDDESC = $"一种喜爱生活在二氧化碳环境下的" + UI.FormatAsLink("植物", "PLANTS") + "，它会长出可食用的" + DARKMOONGALAXYFRUIT.NAME + "，并释放可以呼吸的氧气。";
            }
            public class DARKMOONGALAXYFRUIT
            {
                public static LocString NAME = UI.FormatAsLink("暗月星河果", DarkMoonGalaxyFruitConfig.Id.ToUpperInvariant());
                public static LocString DESC = "一种富含营养的果实，可以为殖民者提供大量的食物能量。";
            }
            public class DARKMOONGALAXYFRUITPRODUCT
            {
                public static LocString NAME = UI.FormatAsLink("星河饭", DarkMoonGalaxyFruitConfig.Id.ToUpperInvariant());
                public static LocString DESC = "由。" + DARKMOONGALAXYFRUIT.NAME + "加工而成的食物";
                public static LocString DOMESTICATEDDESC = $"由" + DARKMOONGALAXYFRUIT.NAME + "加工而成的美味食物，富含营养，可以为殖民者提供大量的食物能量。";
            }
        }
    }




   




}
