using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinionAge
{
    public class STRINGS
    {

        public class ITEMS
        {
            public class INDUSTRIAL_PRODUCTS 
            {
                public class MINIONBRAIN 
                {
                    public static LocString NAME = "复制人大脑";
                    public static LocString DESC = "这里存放着它生前的全部知识遗产";
                    
                }
                public class MINIONBRAIN2
                {
                    public static LocString NAME = "人造大脑";
                    public static LocString DESC = "人造的终究只是人造的，比不上活体掉落的";

                }
            }
        }
        public class BUILDINGS
        {
            public class ENERGYDISPERSIONTABLECONIFG 
            {
                public static LocString NAME = "散能台";
                public static LocString DESC = "什么？你这走的是科技路线？";
                public static LocString EFFECT = "散发其中的神秘能量";
            }








        }

        public class MISC
        {
            public class TAGS
            {
                public static LocString MINIONBRAIN = "复制人大脑";
            }
            public class NEWMINIONNAME
            {
                public static LocString NAME = "的大脑";
            }
            public class NOTIFICATIONS
            {

                public class INHERITANCE_FAILED
                {
                    public static string NAME = "继承失败";
                    public static string DESCRIPTION = "遭到排斥现象，继承失败";
                }

                public class DEBUFFROULETTE
                {
                    public static LocString NAME = "被给予Debuff";
                }
                public class CTCLEROULETTE
                {
                    public static LocString NAME = "自然增长";
                }
                public class DEATHROULETTE 
                {
                    public static LocString NAME = "进入衰老期";
                }
                public class NOINHERITED
                {
                    public static LocString REPEATED_INHERITANCE = "重复传承";
                    public static LocString REPEATED_INHERITANCE_DESC = "这位复制人已经接受过传承了，他们的脑容量不允许他们再次接受一次传承";

                    public static LocString DEATH = "他已经死去了";
                    public static LocString DEATH_DESC = "这位复制人已经死亡，死者无法接受传承";

                }
                public class YESINHERITED
                {
                    public static LocString NAME = "完成传承";
                }
                public class EXCEEDMAX 
                {
                    public static LocString NAME = "达到最大可继承值大小";
                }
            }
        }
        public class UI 
        {
            public class DETAILTABS 
            {

            }
            public static string FormatAsHotkey(string text)
            {
                return "<b><color=#F44A4A>" + text + "</b></color>";
            }

            public static string FormatAsBold(string text)
            {
                return "<b>" + text + "</b>";
            }
        }

        public class DUPLICANTS
        {
            public class MODIFIERS 
            {
                public class SHUAILAO
                {
                    public static LocString NAME = UI.FormatAsHotkey("衰老");
                    public static LocString TOOLTIP = "人老难免有不中用的时候";
                }

                public class HEATWANDERER
                {
                    public static LocString NAME = UI.FormatAsHotkey("散热者");
                    public static LocString TOOLTIP = $"{UI.FormatAsBold("继承失败所造成的负面效果:")} \n\n  周期性的使自身降温，周围物体升温 \n 降温自身温度不会低于15 ℃，升温周围物体不会高于50 ℃";
                }
                public class COOLWANDERER
                {
                    public static LocString NAME = UI.FormatAsHotkey("积热者");
                    public static LocString TOOLTIP = $"{UI.FormatAsBold("继承失败所造成的负面效果:")} \n\n 周期性的使自身升温，周围物体降温 \n 降温周围物体温度不会低于0 ℃，升温自身不会高于55";
                }
                public class SCORCHINGMETALSHARER
                {
                    public static LocString NAME = UI.FormatAsHotkey("炽热金属分享者");
                    public static LocString TOOLTIP = $"{UI.FormatAsBold("食用特殊食物所造成的负面效果:")} \n\n 复制觉得有好东西要分享，至少这些东西比原神。。。烧  \n\n 周期性的向周围抛下200℃高温金属碎片 ";
                    public static LocString DESCRIPTION = "复制人觉得这个食物感觉比. . . .原神. . .烧 ，而感到开心的丢高温金属";
                }

                public class IMMUNEREJECTION
                {
                    public static LocString NAME = UI.FormatAsHotkey("免疫排斥");
                    public static LocString TOOLTIP = $"{UI.FormatAsBold("继承失败所造成的负面效果:")} \n\n 降低免疫力";
                }

                public class LUMINESCENCEKING
                {
                    public static LocString NAME = UI.FormatAsHotkey("‘帝皇’");
                    public static LocString TOOLTIP = $"你也能散发强大灵能吗？";
                }



                public class ENERGYBOOST
                {
                    public static LocString NAME = "能量充沛";
                    public static LocString TOOLTIP = "复制人感到精力充沛，运动和学习能力大幅提升！";
                }
                public class HEATRESISTANCE
                {
                    public static LocString NAME = "炎热抗性";
                    public static LocString TOOLTIP = "复制人对炎热环境的适应能力增强，体温下降速度减缓。";
                }
                public class COLDRESISTANCE
                {
                    public static LocString NAME = "寒冷抗性";
                    public static LocString TOOLTIP = "复制人对寒冷环境的适应能力增强，体温下降速度减缓。";
                }

                public class EFFICIENTWORK
                {
                    public static LocString NAME = "高效工作";
                    public static LocString TOOLTIP = "复制人工作更加专注，建造和机械操作效率显著提升。";
                }
                public class HAPPYMOOD
                {
                    public static LocString NAME = "快乐心情";
                    public static LocString TOOLTIP = "复制人心情愉悦，对生活质量和环境装饰的期望提高。";
                    public static LocString DESCRIPTION = "复制人觉得这个食物感觉不如. . . .原神. . .烧 ，而感到开心";
                }
                public class QUICKRECOVERY
                {
                    public static LocString NAME = "快速恢复";
                    public static LocString TOOLTIP = "复制人的血量回复增强，疾病治愈速度加快。";
                }
                public class RADIATIONRESISTANCE
                {
                    public static LocString NAME = "辐射抵抗";
                    public static LocString TOOLTIP = "复制人对辐射的抵抗力增强，辐射恢复速度加快。";
                }
                public class IRONWILL
                {
                    public static LocString NAME = "钢铁意志";
                    public static LocString TOOLTIP = "复制人的心理承受能力增强，压力减少速度加快，免疫系统提升。";
                }



            }
            public class DEATHS
            {
                public static LocString NAME = "老死";
                public static LocString DESCRIPTION = "{Target} 固有一死，或重于泰山，或轻如鸿毛";

            }
        }
        public class CONFIGURATIONITEM
        {
            public static LocString MINIONAGETHRESHOLD = "复制人年龄";
            public static LocString AGE80PERCENTTHRESHOLD = "衰老年龄";
            public static LocString INHERITANCESUCCESSPROBABILITY = "继承大脑的成功率";
            public static LocString INHERITANCESUCCESSPROBABILITYDESC = "值越小继承失败概率越大，1为不会失败";
            public static LocString AGE80PERCENTTHRESHOLDDESC = $"具体是根据{UI.FormatAsHotkey("复制人年龄")} x {UI.FormatAsHotkey("当前值")}来确定衰老年龄的时间";


        }

    }

    public class MISSING
    {
        public class STRINGS
        {
            public class CREATURES
            {
                public class ATTRIBUTES
                {
                    public class MINIAGEDELTA
                    {
                        public static LocString NAME = "复制人年龄";
                        public static LocString DESC = "复制人我啊........ \n\n 到点就彻底死了捏";
                    }

                }
            }
        }
    }




}
