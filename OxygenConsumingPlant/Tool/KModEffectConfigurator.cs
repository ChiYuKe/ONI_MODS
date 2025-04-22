using Klei.AI;
using System.Collections.Generic;

namespace KModTool
{
    public class KModEffectConfigurator
    {
        public KModEffectConfigurator(string effectID, float duration, bool isNegative)
        {
            this.EffectID = effectID;
            this.EffectDuration = duration;
            this.IsNegativeEffect = isNegative;
            this.EffectName = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effectID.ToUpper() + ".NAME");
            this.EffectDescription = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + effectID.ToUpper() + ".TOOLTIP");
            this.ShowFloatingText = true;
            this.DisplayInUI = true;
            this.IconPath = "";
        }

        public KModEffectConfigurator SetEffectName(string name)
        {
            this.EffectName = name;
            return this;
        }

        public KModEffectConfigurator SetEffectDescription(string description)
        {
            this.EffectDescription = description;
            return this;
        }

        public KModEffectConfigurator AddAttributeModifier(string attributeID, float value, bool isMultiplier = false, bool uiOnly = false, bool readOnly = true)
        {
            this.AttributeModifiers = this.AttributeModifiers ?? new List<AttributeModifier>();
            this.AttributeModifiers.Add(new AttributeModifier(attributeID, value, this.EffectName, isMultiplier, uiOnly, readOnly));
            return this;
        }

        public KModEffectConfigurator SetAnimation(string animationName, float cooldown)
        {
            this.AnimationName = animationName;
            this.AnimationCooldown = cooldown;
            return this;
        }

        public KModEffectConfigurator AddAnimationPrecondition(Reactable.ReactablePrecondition condition)
        {
            this.AnimationPreconditions = this.AnimationPreconditions ?? new List<Reactable.ReactablePrecondition>();
            this.AnimationPreconditions.Add(condition);
            return this;
        }

        public KModEffectConfigurator HideFloatingText()
        {
            this.ShowFloatingText = false;
            return this;
        }

        public KModEffectConfigurator HideInUI()
        {
            this.DisplayInUI = false;
            return this;
        }

        public void ApplyTo(ModifierSet modifierSet)
        {
            Effect effect = new Effect(
                this.EffectID,
                this.EffectName,
                this.EffectDescription,
                this.EffectDuration,
                this.DisplayInUI,
                this.ShowFloatingText,
                this.IsNegativeEffect,
                this.AnimationName,
                this.AnimationCooldown,
                this.Group,
                this.IconPath
            );

            if (this.AttributeModifiers != null)
            {
                effect.SelfModifiers = this.AttributeModifiers;
            }

            if (this.AnimationPreconditions != null)
            {
                effect.emotePreconditions = this.AnimationPreconditions;
            }

            modifierSet.effects.Add(effect);
        }

        private readonly string EffectID;

        private string EffectName;

        private string EffectDescription;

        private readonly float EffectDuration;

        private bool ShowFloatingText;

        private bool DisplayInUI;

        private readonly bool IsNegativeEffect;

        private List<AttributeModifier> AttributeModifiers;

        private string AnimationName;

        private float AnimationCooldown;

        private string IconPath;

        private string Group;

        private List<Reactable.ReactablePrecondition> AnimationPreconditions;
    }
}