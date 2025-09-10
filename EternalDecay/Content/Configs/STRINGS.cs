using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EternalDecay.Content.Configs
{
    internal class STRINGS
    {

        public class ITEMS 
        {
            public class INDUSTRIAL_PRODUCTS 
            {
                public class MINIONBRAIN 
                {
                    public static LocString NAME = "罐中脑";
                    public static LocString DESC = "复制人最后的火光了";

                }
                public class MINIONBRAINBAD
                {
                    public static LocString NAME = "泄露的罐中脑";
                    public static LocString DESC = "虽然不能用了但还能吃";

                }
            }
        }


        public class DUPLICANTS 
        {
            public class MODIFIERS 
            {
                public class ETERNALDECAY_SHUAILAO 
                {
                    public static LocString NAME = "衰老";
                    public static LocString TOOLTIP = "衰老带来的虚弱感，极大地影响了复制人的各项机能";
                }
                public class ETERNALDECAY_BAOZHIQI
                {
                    public static LocString NAME = "稳定期";
                    public static LocString TOOLTIP = "稳定期为 0 时，罐中脑会直接损坏";
                }
            }

            public class AGEATTRIBUTE
            {
                public static LocString NAME = "年龄: {0} / {1} 周期";
                public static LocString TOOLTIP = "当前年龄为 {0}，上限 {1}";
            }



            public class STATS
            {
                public class AGEATTRIBUTE
                {
                    public static LocString NAME = "年龄";
                    public static LocString TOOLTIP = "人终究难逃一死";
                }
            }



        }

        public class MISC 
        {
            public class NOTIFICATIONS
            {
                public class AGEATTRIBUTE
                {
                    public static LocString NAME = "自然衰老";
                    public static LocString TOOLTIP = "{0} 已经活到了生命的尽头。";
                }
            }
            
            public class NEWMINIONNAME
            {
                public static LocString NAME = " - 罐中脑";
            }



            public class TAGS
            {
                public class MINIONBRAIN
                {
                    public static LocString NAME = "复制人大脑";
                  
                }
            }



        }


        public class DEATHS
        {
            public class AGING
            {
                public static LocString NAME = "衰老死亡";
                public static LocString DESCRIPTION = "这幅身躯终究是支撑不住，还是死亡了，但留下来宝贵传承。";
            }
        }
    }
}
