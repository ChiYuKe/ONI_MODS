using System;
using System.Collections.Generic;
using Klei.AI;


public class EffectBuilder
{
	public EffectBuilder(string ID, float duration, bool isBad)
	{
		this.name = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + ID.ToUpper() + ".NAME");
		this.description = Strings.Get("STRINGS.DUPLICANTS.MODIFIERS." + ID.ToUpper() + ".TOOLTIP");
		this.triggerFloatingText = true;
		this.showInUI = true;
		this.duration = duration;
		this.isBad = isBad;
		this.ID = ID;
		this.customIcon = "";
	}

	public EffectBuilder Icon(string icon)
	{
		this.customIcon = icon;
		return this;
	}

	public EffectBuilder Name(string name)
	{
		this.name = name;
		return this;
	}

	public EffectBuilder Description(string description)
	{
		this.description = description;
		return this;
	}

	public EffectBuilder Modifier(string id, float value, bool isMultiplier, bool uiOnly = false, bool readOnly = true)
	{
		this.modifiers = this.modifiers ?? new List<AttributeModifier>();
		this.modifiers.Add(new AttributeModifier(id, value, this.name, isMultiplier, uiOnly, readOnly));
		return this;
	}

	public EffectBuilder Modifier(string id, float value)
	{
		this.modifiers = this.modifiers ?? new List<AttributeModifier>();
		this.modifiers.Add(new AttributeModifier(id, value, this.name, false, false, true));
		return this;
	}

	public EffectBuilder Emote(string emoteAnim, float emoteCooldown)
	{
		this.emoteAnim = emoteAnim;
		this.emoteCooldown = emoteCooldown;
		return this;
	}

	public EffectBuilder EmotePrecondition(Reactable.ReactablePrecondition condition)
	{
		this.emotePreconditions = this.emotePreconditions ?? new List<Reactable.ReactablePrecondition>();
		this.emotePreconditions.Add(condition);
		return this;
	}

	public EffectBuilder HideFloatingText()
	{
		this.triggerFloatingText = false;
		return this;
	}

	public EffectBuilder HideInUI()
	{
		this.showInUI = false;
		return this;
	}

	public void Add(ModifierSet set)
	{
		Effect effect = new Effect(this.ID, this.name, this.description, this.duration, this.showInUI, this.triggerFloatingText, this.isBad, this.emoteAnim, this.emoteCooldown, this.stompGroup, this.customIcon);
		if (this.modifiers != null)
		{
			effect.SelfModifiers = this.modifiers;
		}
		if (this.emotePreconditions != null)
		{
			effect.emotePreconditions = this.emotePreconditions;
		}
		set.effects.Add(effect);
	}

	private readonly string ID;
	private string name;
	private string description;
	private readonly float duration;
	private bool triggerFloatingText;
	private bool showInUI;
	private readonly bool isBad;
	private List<AttributeModifier> modifiers;
	private string emoteAnim;
	private float emoteCooldown;
	private string customIcon;
	private string stompGroup;
	private List<Reactable.ReactablePrecondition> emotePreconditions;
}
