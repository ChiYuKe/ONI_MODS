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
    }
}
