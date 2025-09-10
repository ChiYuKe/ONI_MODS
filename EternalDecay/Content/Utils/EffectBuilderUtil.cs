using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Klei.AI;

namespace CykUtils
{
    internal class EffectBuilderUtil
    {
        /// <summary>
        /// 构造一个 Effect 配置器。
        /// </summary>
        /// <param name="effectID">Effect 的唯一 ID。</param>
        /// <param name="duration">持续时间（秒）。</param>
        /// <param name="isNegative">是否为负面效果。</param>
        public EffectBuilderUtil(string effectID, float duration, bool isNegative)
        {
            _effectID = effectID;
            _effectDuration = duration;
            _isNegativeEffect = isNegative;

            _effectName = string.Empty;
            _effectDescription = string.Empty;
            _showFloatingText = true;
            _displayInUI = true;
            _iconPath = string.Empty;
            _group = string.Empty;
        }

        #region 链式配置方法

        public EffectBuilderUtil SetEffectName(string name)
        {
            _effectName = name;
            return this;
        }

        public EffectBuilderUtil SetEffectDescription(string description)
        {
            _effectDescription = description;
            return this;
        }

        public EffectBuilderUtil AddAttributeModifier(string attributeID, float value, bool isMultiplier = false, bool uiOnly = false, bool readOnly = true)
        {
            _attributeModifiers ??= new List<AttributeModifier>();
            _attributeModifiers.Add(new AttributeModifier(attributeID, value, _effectName, isMultiplier, uiOnly, readOnly));
            return this;
        }

        public EffectBuilderUtil SetAnimation(string animationName, float cooldown)
        {
            _animationName = animationName;
            _animationCooldown = cooldown;
            return this;
        }

        public EffectBuilderUtil AddAnimationPrecondition(Reactable.ReactablePrecondition condition)
        {
            _animationPreconditions ??= new List<Reactable.ReactablePrecondition>();
            _animationPreconditions.Add(condition);
            return this;
        }

        public EffectBuilderUtil HideFloatingText()
        {
            _showFloatingText = false;
            return this;
        }

        public EffectBuilderUtil HideInUI()
        {
            _displayInUI = false;
            return this;
        }

        #endregion

        #region 构建与应用

        /// <summary>
        /// 构建 Effect 对象。
        /// </summary>
        /// <returns>构建完成的 Effect 实例。</returns>
        private Effect BuildEffect()
        {
            // 如果 EffectName/Description 为空，从字符串表获取
            string name = string.IsNullOrEmpty(_effectName)
                ? Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{_effectID.ToUpper()}.NAME")
                : _effectName;

            string description = string.IsNullOrEmpty(_effectDescription)
                ? Strings.Get($"STRINGS.DUPLICANTS.MODIFIERS.{_effectID.ToUpper()}.TOOLTIP")
                : _effectDescription;

            var effect = new Effect(
                _effectID,
                name,
                description,
                _effectDuration,
                _displayInUI,
                _showFloatingText,
                _isNegativeEffect,
                _animationName,
                _animationCooldown,
                _group,
                _iconPath
            );

            if (_attributeModifiers != null)
                effect.SelfModifiers = _attributeModifiers;

            if (_animationPreconditions != null)
                effect.emotePreconditions = _animationPreconditions;

            return effect;
        }

        /// <summary>
        /// 将 Effect 应用到指定的 ModifierSet 中。
        /// </summary>
        /// <param name="modifierSet">目标 ModifierSet。</param>
        public void ApplyTo(ModifierSet modifierSet)
        {
            if (modifierSet == null) throw new ArgumentNullException(nameof(modifierSet));
            modifierSet.effects.Add(BuildEffect());
        }

        #endregion

        #region 私有字段

        private readonly string _effectID;
        private string _effectName;
        private string _effectDescription;
        private readonly float _effectDuration;
        private readonly bool _isNegativeEffect;

        private bool _showFloatingText;
        private bool _displayInUI;
        private string _iconPath;
        private string _group;

        private List<AttributeModifier> _attributeModifiers;
        private string _animationName;
        private float _animationCooldown;
        private List<Reactable.ReactablePrecondition> _animationPreconditions;

        #endregion

    }
}
