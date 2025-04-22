using STRINGS;

namespace OxygenConsumingPlant
{
    public class STRINGS
    {
        public class CREATURES
        {
            public class SPECIES
            {
                public class KMODOXYGENTREE
                {
                    public static LocString NAME = "测试植物";

                    public static LocString DESC = "吸收周围植物温度或者给周围植物升温" + "\n成熟后生成" + STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_G.NAME + ".";

                    public static LocString DOMESTICATEDDESC = "This plant produces edible " + UI.FormatAsLink("Bristle Berries", UI.StripLinkFormatting(STRINGS.ITEMS.FOOD.KMODOXYGENTREEFRUIT_G.NAME)) + ".";
                }

                public class SEEDS
                {
                    public class KMODOXYGENTREESEED
                    {
                        public static LocString NAME = UI.FormatAsLink("测试种子", "KMODOXYGENTREE");

                        public static LocString DESC = string.Concat(new string[]
                        {
                            "The ",
                            UI.FormatAsLink("Seed", "PLANTS"),
                            " of a ",
                            UI.FormatAsLink("Bristle Blossom", "KMODOXYGENTREE"),
                            ".\n\nDigging up Buried Objects may uncover a Blossom Seed."
                        });
                    }
                }
            }
        }

        public class ITEMS
        {
            public class FOOD
            {
                public class KMODOXYGENTREEFRUIT_G
                {
                    public static LocString NAME = UI.FormatAsLink("测试原生果", "KMODOXYGENTREEFRUIT_G");

                    public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";

                    public static LocString RECIPEDESC = "测试RECIPEDESC";
                }
                public class KMODOXYGENTREEFRUIT_R
                {
                    public static LocString NAME = UI.FormatAsLink("测试红果", "KMODOXYGENTREEFRUIT_R");

                    public static LocString DESC = "A sweet, mostly pleasant-tasting fruit covered in prickly barbs.";

                    public static LocString RECIPEDESC = "测试RECIPEDESC";
                }
            }
        }

        public class DUPLICANTS
        {
            public class MODIFIERS
            {
                public class WAJUE
                {

                    public static LocString NAME = "矿工体质";

                    public static LocString DESC = "现在是一个优秀矿工时间！！";
                }
            }
        }
    }
}