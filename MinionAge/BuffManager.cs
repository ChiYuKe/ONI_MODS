using Database;
using Klei.AI;
using KModTool;
using STRINGS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static STRINGS.DUPLICANTS.ATTRIBUTES;

namespace MinionAge
{
   
    internal class KModDeBuff
    {
       
        public static void Register(ModifierSet parent)
        {
            Database.Attributes attributes = Db.Get().Attributes;
            Database.Amounts amounts = Db.Get().Amounts;
            Database.Emotes emotes = Db.Get().Emotes;
            Database.Expressions expressions = Db.Get().Expressions;

            new KModEffectConfigurator("shuailao", 0f, true)
               .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.SHUAILAO.NAME)
               .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.SHUAILAO.TOOLTIP)
               .AddAttributeModifier(attributes.Athletics.Id, -6f, false, false, true)// 运动
               .AddAttributeModifier(attributes.Strength.Id, -5f, false, false, true)//力量
               .AddAttributeModifier(attributes.Digging.Id, -5f, false, false, true)// 挖掘
               .AddAttributeModifier(attributes.Immunity.Id, -2f, false, false, true)// 免疫系统
               .ApplyTo(parent);



            // 免疫排斥
            new KModEffectConfigurator("Immunerejection", 600f, true)
              .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.IMMUNEREJECTION.NAME)
              .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.IMMUNEREJECTION.TOOLTIP)
              .AddAttributeModifier(attributes.Immunity.Id, -5f, false, false, true)// 免疫系统
              .ApplyTo(parent);


            new KModEffectConfigurator("HeatWanderer", 300f, true)
              .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.HEATWANDERER.NAME)
              .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.HEATWANDERER.TOOLTIP)
              .ApplyTo(parent);


            new KModEffectConfigurator("CoolWanderer", 300f, true)
               .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.COOLWANDERER.NAME)
               .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.COOLWANDERER.TOOLTIP)
               .ApplyTo(parent);


            new KModEffectConfigurator("ScorchingMetalSharer", 300f, true)
             .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.SCORCHINGMETALSHARER.NAME)
             .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.SCORCHINGMETALSHARER.TOOLTIP)
             .ApplyTo(parent);



            // 帝皇
            new KModEffectConfigurator("LuminescenceKing", 600f, false)
              .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.LUMINESCENCEKING.NAME)
              .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.LUMINESCENCEKING.TOOLTIP)
              .AddAttributeModifier(attributes.Luminescence.Id, 5000f, false, false, true)// 发光能力 5000勒克斯
              .ApplyTo(parent);



            // 能量充沛 (EnergyBoost)
            new KModEffectConfigurator("EnergyBoost", 600f, false)
             .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.ENERGYBOOST.NAME)
             .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.ENERGYBOOST.TOOLTIP)
             .AddAttributeModifier(attributes.Athletics.Id, 10f, false, false, true) // 运动能力 +10
             .AddAttributeModifier(attributes.Learning.Id, 8f, false, false, true)  // 学习能力 +8
              .AddAttributeModifier(amounts.Stamina.deltaAttribute.Id, 5f/600f, false, false, true) // 体力 +1  = 1%每周期
             .ApplyTo(parent);


            //炎热抗性 (HeatResistance)
            new KModEffectConfigurator("HeatResistance", 1200f, false)
             .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.HEATRESISTANCE.NAME)
             .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.HEATRESISTANCE.TOOLTIP)
             .AddAttributeModifier(attributes.ThermalConductivityBarrier.Id, 1f, false, false, true) // 热传导屏障 +1
             .AddAttributeModifier(attributes.ScaldingThreshold.Id, 40f, false, false, true)         // 烫伤阈值 +10
             .ApplyTo(parent);


            //寒冷抗性 (HeatResistance)
            new KModEffectConfigurator("ColdResistance", 1200f, false)
             .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.COLDRESISTANCE.NAME)
             .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.COLDRESISTANCE.TOOLTIP)
             .AddAttributeModifier(attributes.ThermalConductivityBarrier.Id, 1f, false, false, true) // 热传导屏障 +1
             .AddAttributeModifier(attributes.ScoldingThreshold.Id, -20f, false, false, true)// 冻伤阈值 
             .ApplyTo(parent);


            // 高效工作 (EfficientWork)
            new KModEffectConfigurator("EfficientWork", 900f, false)
            .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.EFFICIENTWORK.NAME)
            .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.EFFICIENTWORK.TOOLTIP)
            .AddAttributeModifier(attributes.Construction.Id, 12f, false, false, true) // 建造能力 +12
            .AddAttributeModifier(attributes.Machinery.Id, 10f, false, false, true)    // 机械操作能力 +10
            .AddAttributeModifier(attributes.CarryAmount.Id, 2000f, false, false, true)    // 携带量  2000  = 2000kg
            .ApplyTo(parent);


            // 快乐心情 (HappyMood)
            new KModEffectConfigurator("HappyMood", 1800f, false)
            .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.HAPPYMOOD.NAME)
            .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.HAPPYMOOD.TOOLTIP)
            .AddAttributeModifier(attributes.QualityOfLife.Id, 15f, false, false, true)      // 士气 +15
            .AddAttributeModifier(amounts.Stress.deltaAttribute.Id, -10f / 600f, false, false, true) // 压力每秒减少压力每秒减少 1 = 1%
            .AddAttributeModifier(attributes.DecorExpectation.Id, 20f, false, false, true)  // 装饰期望 +20
            .ApplyTo(parent);


            // 快速恢复 (QuickRecovery)
            new KModEffectConfigurator("QuickRecovery", 1500f, false)
            .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.QUICKRECOVERY.NAME)
            .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.QUICKRECOVERY.TOOLTIP)
            .AddAttributeModifier(attributes.DiseaseCureSpeed.Id, 1.5f, false, false, true) // 疾病治愈速度 +1.5
            .AddAttributeModifier(amounts.HitPoints.deltaAttribute.Id, 60f / 600f, false, false, true) // 生命 +1 = 补充60
            .ApplyTo(parent);


            // 辐射抵抗 (RadiationResistance)
            new KModEffectConfigurator("RadiationResistance", 1200f, false)
            .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.RADIATIONRESISTANCE.NAME)
            .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.RADIATIONRESISTANCE.TOOLTIP)
            .AddAttributeModifier(attributes.RadiationResistance.Id, 15f, false, false, true) // 辐射抵抗力 +15
            .AddAttributeModifier(attributes.RadiationRecovery.Id, -0.1f, false, false, true)     // 辐射吸收 0.1 = 150
            .ApplyTo(parent);


            // 钢铁意志 (IronWill)
            new KModEffectConfigurator("IronWill", 1800f, false)
            .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.IRONWILL.NAME)
            .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.IRONWILL.TOOLTIP)
            .AddAttributeModifier(attributes.Immunity.Id, 15f, false, false, true) // 免疫系统 +15
            .AddAttributeModifier(amounts.Stamina.deltaAttribute.Id, 10f/600f, false, false, true) // 体力 +10  = 2%每周期
            .AddAttributeModifier(amounts.Stress.deltaAttribute.Id, -15f/600f, false, false, true) // 压力每秒减少 1 = 1%
            .ApplyTo(parent);


        }


        // 添加负面效果
        public static void ApplyDebuff(GameObject gameObject, string BuffName)
        {
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            effectsComponent.GetTimeLimitedEffects();
            if (effectsComponent != null && !effectsComponent.HasEffect(BuffName)) // 如果没有当前debuff
            {
                effectsComponent.Add(BuffName, true); // 添加效果
               
            }
        }



        public static float GetEffectRemainingTime(GameObject gameObject, string effect_id)
        {
            // 获取 Effects 组件
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            // 如果没有找到 Effects 组件，返回 -1
            if (effectsComponent == null)
            {
                return -1f;
            }

            // 获取目标效果实例
            EffectInstance targetEffect = effectsComponent.Get(effect_id);

            // 如果没有找到效果实例，返回 -1
            if (targetEffect == null)
            {
                return -1f;
            }
           
            return targetEffect.timeRemaining;
        }


        public static void RemoveEffect(GameObject gameObject, string effect_id)
        {
            // 获取 Effects 组件
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            // 如果没有找到 Effects 组件，返回
            if (effectsComponent == null)
            {
                return;
            }

            // 使用 effect_id 移除效果
            effectsComponent.Remove(effect_id);
        }
























        public static void ApplyRandomDebuff(HashSet<GameObject> cachedMinionGameObjects)
        {
            List<string> debuffTypes = new List<string> { "debuff1", "debuff2", "debuff3", "debuff4" };
            int minionCount = cachedMinionGameObjects.Count;
            int numToSelect = 3;

            if (minionCount >= numToSelect)
            {
                // 打乱小人列表，随机选择前 numToSelect 个小人
                List<GameObject> selectedMinions = cachedMinionGameObjects.OrderBy(x => UnityEngine.Random.value).Take(numToSelect).ToList();

                foreach (GameObject gameObject in selectedMinions)
                {
                    if (gameObject == null) continue;

                    Klei.AI.Effects effectsComponent = gameObject.GetComponent<Klei.AI.Effects>();
                    if (effectsComponent == null) continue;

                    // 随机整一个 Debuff给小人
                    string randomDebuff = debuffTypes[UnityEngine.Random.Range(0, debuffTypes.Count)];

                    if (!effectsComponent.HasEffect(randomDebuff))
                    {
                        effectsComponent.Add(randomDebuff, true);
                        NotifyDebuffApplied1(gameObject); // 通知玩家谁被添加了DeBuff

                    }
                }
            }
            else
            {
                Debug.Log("复制人数不足3人，不添加 Debuff。");
            }
        }


        private static void NotifyDebuffApplied1(GameObject gameObject)
        {
            Notifier notifier = gameObject.AddOrGet<Notifier>();
            Notification notification = new Notification(
                STRINGS.MISC.NOTIFICATIONS.DEBUFFROULETTE.NAME,
                NotificationType.BadMinor,
                (notificationList, data) => notificationList.ReduceMessages(false),
                "/t• " + gameObject.GetProperName(), true, 0f, null, null, null, true, false, false
            );
            notifier.Add(notification, "");
        }


        // 通知衰老效果应用
        public static void NotifyDeathApplied(GameObject gameObject)
        {
            Notifier notifier = gameObject.AddOrGet<Notifier>();
            Notification notification = new Notification(
                STRINGS.MISC.NOTIFICATIONS.DEATHROULETTE.NAME, // 通知标题
                NotificationType.BadMinor, // 通知类型
                (notificationList, data) => notificationList.ReduceMessages(false), // 通知处理函数
                "/t• " + gameObject.GetProperName(), // 通知内容
                true, 0f, null, null, null, true, false, false
            );
            notifier.Add(notification, ""); // 添加通知
        }

    }
}