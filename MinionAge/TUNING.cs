using PeterHan.PLib.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MinionAge.Main;

namespace MinionAge
{
    internal class TUNINGS
    {
        public class TIMERMANAGER
        {
            public class RANDOMDEBUFFTIMERMANAGER
            {
                // json同步时间
                public static float TIMERINTERVAL = 500f;
                // 复制人生命周期
                public static float MINIONAGETHRESHOLD = SingletonOptions<ConfigurationItem>.Instance.minionagethreshold;


                // 继承大脑属性失败的概率
                public static float INHERITANCESUCCESSPROBABILITY = SingletonOptions<ConfigurationItem>.Instance.inheritanceSuccessProbability;

                // 衰老开始的阈值 到达 MINIONAGETHRESHOLD 的 AGE80PERCENTTHRESHOLD 开始衰老
                public static float AGE80PERCENTTHRESHOLD = SingletonOptions<ConfigurationItem>.Instance.age80percentthreshold;

                public class TRANSFER
                {
                    // 大脑最大可继承的特质数量
                    public static int TRAITSMAXAMOUNT = 12;
                    // 大脑最大可继承的就技能数量
                    public static int SKILLMAXAMOUNT = 17;

                    // 大脑最大可继承的属性等级
                    public static int ATTRIBUTEMAXLEVEL = 50;

                }

            }



          
        }
        public static readonly Tag HeatWanderer = "HeatWanderer"; //持有此tag的复制人自身会持续降温到15度，周围对象会持续升温50度
        public static readonly Tag NoMourning = "NoMourning"; // 持有此tag的复制人死亡时不会触发死亡哀悼的debuff
        public static readonly Tag Assigned = "Assigned"; // 持有此tag的复制人无法分配大脑
        public static readonly Tag DieOfOldAge = "DieOfOldAge"; // 标记为老死





    }
}
