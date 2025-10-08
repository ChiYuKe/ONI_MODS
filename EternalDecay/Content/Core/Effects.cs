using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klei.AI;
using CykUtils;
using UnityEngine;
using static Database.MonumentPartResource;

namespace EternalDecay.Content.Core
{
    internal static class KEffects
    {
        public static void RegisterAll(ModifierSet modifierSet)
        {
            if (modifierSet == null) return;
            AgingFatigue(modifierSet);
            Baozhiqi(modifierSet);
            Dihuang(modifierSet);

            Abyssophobia_0(modifierSet);
            Abyssophobia_1(modifierSet);
            Abyssophobia_2(modifierSet);
            Abyssophobia_3(modifierSet);
            Abyssophobia_4(modifierSet);
            Abyssophobia_5(modifierSet);

        }

        private static string GetEffectID(string id) => $"EternalDecay_{id}";

        public static void ApplyBuff(GameObject gameObject, string BuffName)
        {
            Effects effectsComponent = gameObject.GetComponent<Effects>();

            effectsComponent.GetTimeLimitedEffects();
            if (effectsComponent != null && !effectsComponent.HasEffect(BuffName))
            {
                effectsComponent.Add(BuffName, true);

            }
        }

        public static void RemoveBuff(GameObject gameObject, string effect_id)
        {
            Effects effectsComponent = gameObject.GetComponent<Effects>();
            if (effectsComponent == null)
            {
                return;
            }
            effectsComponent.Remove(effect_id);
        }




        // 统一缓存数据库引用，避免重复调用 Db.Get()
        private static readonly Database.Attributes Attributes = Db.Get().Attributes;
        private static readonly Database.Amounts Amounts = Db.Get().Amounts;
        private static readonly Database.Emotes Emotes = Db.Get().Emotes;
        private static readonly Database.Expressions Expressions = Db.Get().Expressions;

        public static readonly string ETERNALDECAY_SHUAILAO = GetEffectID("shuailao");
        public static readonly string ETERNALDECAY_BAOZHIQI = GetEffectID("baozhiqi");
        public static readonly string ETERNALDECAY_LUMINESCENCEKING = GetEffectID("LuminescenceKing");

        public static readonly string ETERNALDECAY_ABYSSO_0 = GetEffectID("Abyssophobia_0");
        public static readonly string ETERNALDECAY_ABYSSO_1 = GetEffectID("Abyssophobia_1");
        public static readonly string ETERNALDECAY_ABYSSO_2 = GetEffectID("Abyssophobia_2");
        public static readonly string ETERNALDECAY_ABYSSO_3 = GetEffectID("Abyssophobia_3");
        public static readonly string ETERNALDECAY_ABYSSO_4 = GetEffectID("Abyssophobia_4");
        public static readonly string ETERNALDECAY_ABYSSO_5 = GetEffectID("Abyssophobia_5");


        /// <summary>
        /// 衰老带来的虚弱 Debuff
        /// </summary>
        private static void AgingFatigue(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_SHUAILAO, 0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ETERNALDECAY_SHUAILAO.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ETERNALDECAY_SHUAILAO.TOOLTIP)
                .AddAttributeModifier(Attributes.Athletics.Id, -6f)   // 运动
                .AddAttributeModifier(Attributes.Strength.Id, -5f)    // 力量
                .AddAttributeModifier(Attributes.Digging.Id, -5f)     // 挖掘
                .AddAttributeModifier(Attributes.Immunity.Id, -2f)    // 免疫系统
                .ApplyTo(modifierSet);
        }
        private static void Baozhiqi(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_BAOZHIQI, 1000f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ETERNALDECAY_BAOZHIQI.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ETERNALDECAY_BAOZHIQI.TOOLTIP)
                .ApplyTo(modifierSet);
        }

        private static void Dihuang(ModifierSet modifierSet) 
        {
            new EffectBuilderUtil(ETERNALDECAY_LUMINESCENCEKING, 600f, false)
              .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.LUMINESCENCEKING.NAME)
              .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.LUMINESCENCEKING.TOOLTIP)
              .AddAttributeModifier(Attributes.Luminescence.Id, 5000f, false, false, true)
              .ApplyTo(modifierSet);
        }




        //////////////////////////////////////////////
        private static void Abyssophobia_0(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_0, 0f, false)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO0.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO0.TOOLTIP)
                .ApplyTo(modifierSet);
        }
        private static void Abyssophobia_1(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_1, 0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO1.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO1.TOOLTIP)
                .AddAttributeModifier(Attributes.AirConsumptionRate.Id, 0.03f) // 呼吸效率
                .AddAttributeModifier(Attributes.Construction.Id, -5f) // 建造
                .AddAttributeModifier(Attributes.Machinery.Id, -3f) // 机械
                .AddAttributeModifier(Attributes.Digging.Id, -3f) // 挖掘
                .AddAttributeModifier(Attributes.Athletics.Id, -8f) // 运动 
                .AddAttributeModifier(Attributes.Strength.Id, -3f) // 力量
                .ApplyTo(modifierSet);
        }
        private static void Abyssophobia_2(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_2, 0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO2.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO2.TOOLTIP)
                .AddAttributeModifier(Attributes.AirConsumptionRate.Id, 0.06f) // 呼吸效率
                .AddAttributeModifier(Attributes.Construction.Id, -8f) // 建造
                .AddAttributeModifier(Attributes.Machinery.Id, -5f) // 机械
                .AddAttributeModifier(Attributes.Digging.Id, -5f) // 挖掘
                .AddAttributeModifier(Attributes.Athletics.Id, -10f) // 运动 
                .AddAttributeModifier(Attributes.Strength.Id, -5f) // 力量
                .ApplyTo(modifierSet);
        }
        private static void Abyssophobia_3(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_3, 0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO3.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO3.TOOLTIP)
                 .AddAttributeModifier(Attributes.AirConsumptionRate.Id, 0.09f) // 呼吸效率
                .AddAttributeModifier(Attributes.Construction.Id, -10f) // 建造
                .AddAttributeModifier(Attributes.Machinery.Id, -8f) // 机械
                .AddAttributeModifier(Attributes.Digging.Id, -8f) // 挖掘
                .AddAttributeModifier(Attributes.Athletics.Id, -15f) // 运动 
                .AddAttributeModifier(Attributes.Strength.Id, -8f) // 力量
                .ApplyTo(modifierSet);
        }
        private static void Abyssophobia_4(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_4,0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO4.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO4.TOOLTIP)
                .AddAttributeModifier(Attributes.AirConsumptionRate.Id, 0.12f) // 呼吸效率
                .AddAttributeModifier(Attributes.Construction.Id, -15f) // 建造
                .AddAttributeModifier(Attributes.Machinery.Id, -10f) // 机械
                .AddAttributeModifier(Attributes.Digging.Id, -10f) // 挖掘
                .AddAttributeModifier(Attributes.Athletics.Id, -20f) // 运动 
                .AddAttributeModifier(Attributes.Strength.Id, -10f) // 力量
                .ApplyTo(modifierSet);
        }
        private static void Abyssophobia_5(ModifierSet modifierSet)
        {
            new EffectBuilderUtil(ETERNALDECAY_ABYSSO_5,0f, true)
                .SetEffectName(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO5.NAME)
                .SetEffectDescription(Configs.STRINGS.DUPLICANTS.MODIFIERS.ABYSSO.ABYSSO5.TOOLTIP)
                .AddAttributeModifier(Attributes.Construction.Id, -15f) // 建造
                .AddAttributeModifier(Attributes.Machinery.Id, -15f) // 机械
                .AddAttributeModifier(Attributes.Digging.Id, -15f) // 挖掘
                .AddAttributeModifier(Attributes.Athletics.Id, -30f) // 运动 
                .AddAttributeModifier(Attributes.Strength.Id, -15f) // 力量
                .AddAttributeModifier(Attributes.AirConsumptionRate.Id, 0.15f) // 呼吸效率
                .ApplyTo(modifierSet);
        }





    }
}
