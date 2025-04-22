using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBox
{
    internal class STRINGS
    {
        public class CONFIGURATIONITEM 
        {
            public class TRANSPORTROUTECONFIG
            {
                public class TRANSPORTTRACK
                {
                    public static LocString TITLE = "运输轨道容量";
                    public static LocString TOOLTIP = "轨道最大的运输容量";
                    public static LocString CATEGORY = "容量配置";

                }
            }
            public class BUILDDINGS
            {
                public static LocString CATEGORY = "建筑配置";



                public class GASRESERVOIR 
                {
                    public static LocString CAPACITY = "储气库容量";
                    public static LocString OVERHEATABLE = "储气库会过热";
                    public static LocString FOUNDATION = "储气库不需要地基建造";

                }
               
                public class LIQUIDRESERVOIR
                {
                    public static LocString CAPACITY = "储液库容量";
                    public static LocString OVERHEATABLE = "储液库会过热";
                    public static LocString FOUNDATION = "储液库不需要地基建造";

                }

                public class MINERALDEOXIDIZER
                {
                    public static LocString TITLE = "氧气扩散器气体排放量";
                    public static LocString ENERGYCONSUMPTIONWHENACTIVE = "氧气扩散器电力消耗";
                    public static LocString FLOODABLE = "氧气扩散器会被淹没";
                    public static LocString OVERHEATABLE = "氧气扩散器会过热";
                    public static LocString HEATGENERATION = "氧气扩散器会发热";
                    public static LocString OUTPUTTEMPERATURE = "氧气扩散器排放气体的温度";


                }
                public class POWERTRANSFORMERSMALL 
                {
                    public static LocString HEATGENERATION = "小型变压器会发热";

                }
                public class POWERTRANSFORMER
                {
                    public static LocString HEATGENERATION = "大型变压器会发热";

                }
                public class REFRIGERATOR 
                {
                    public static LocString ENERGYCONSUMPTIONWHENACTIVE = "冰箱电力消耗";
                    public static LocString FLOODABLE = "冰箱会被淹没";
                    public static LocString OVERHEATABLE = "冰箱会过热";
                    public static LocString CAPACITY = "冰箱容量";


                }
                public class SOLIDCONDUITOUTBOX
                {
                    public static LocString CAPACITY = "运输存放器容量";
                }
                public class SOLIDCONDUITINBOX
                {
                    public static LocString CAPACITY = "运输装载器容量";
                }
                public class STORAGELOCKER
                {
                    public static LocString CAPACITY = "存储箱容量";
                }
              
            }
            public class POWERCONFIG 
            {
                public class WIRING
                {
                    public static LocString CATEGORY = "电路配置";
                    public static LocString WIRES = "电线负载";
                    public static LocString CONDUCTORS = "导线负载";
                    public static LocString HIGHLOADWIRES = "高负荷电线负载";
                    public static LocString HIGHLOADCONDUCTORS = "高负荷导线负载";

                }
            }
            public class PLANTCONFIG
            {
                public class PLANT
                {
                    public static LocString CROPDURATION = "生长时间";
                    public static LocString NUMPRODUCED = "收获数量";

                    public static LocString BASICPLANTFOOD = "米虱木";
                    public static LocString PRICKLEFRUIT = "毛刺花";
                    public static LocString SWAMPFRUIT = "沼浆笼";
                    public static LocString MUSHROOM = "夜幕菇";
                    public static LocString COLDWHEATSEED = "冰霜小麦";
                    public static LocString SPICENUT = "火椒藤";
                    public static LocString BASICFABRIC = "顶针芦苇";
                    public static LocString SWAMPLILYFLOWER = "芳香百合";
                    public static LocString GASGRASSHARVESTED = "释气草";
                    public static LocString WOODLOG = "乔木树";
                    public static LocString SUGARWATER = "糖心树";
                    public static LocString SPACETREEBRANCH = "糖心树枝";
                    public static LocString HARDSKINBERRY = "刺壳果灌木";
                    public static LocString CARROT = "羽葉果薯";
                    public static LocString OXYROCK = "气囊芦荟";
                    public static LocString LETTUCE = "水草";
                    public static LocString BEANPLANTSEED = "小吃芽";
                    public static LocString PLANTMEAT = "土星动物捕草";
                    public static LocString WORMSBASICFRUIT = "贫瘠虫果";
                    public static LocString WORMSUPERFRUIT = "虫果";
                }
            }


        }
       
    }
}
