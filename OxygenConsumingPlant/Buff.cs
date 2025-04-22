using Database;
using KModTool;
using OxygenConsumingPlant.Tool;

namespace OxygenConsumingPlant
{
   
    internal class Buff
    {
       
        public static void Register(ModifierSet parent)
        {
            Attributes attributes = Db.Get().Attributes;
            Amounts amounts = Db.Get().Amounts;
            new KModEffectConfigurator("wajue", 10f, false)
                .SetEffectName(STRINGS.DUPLICANTS.MODIFIERS.WAJUE.NAME)
                .SetEffectDescription(STRINGS.DUPLICANTS.MODIFIERS.WAJUE.DESC)
                .AddAttributeModifier(attributes.Digging.Id, 6f, false, false, true)
                .AddAttributeModifier(attributes.CarryAmount.Id, 2000f, false, false, true)
                .AddAttributeModifier(amounts.Calories.deltaAttribute.Id, -333.3f, false, false, true)
                .ApplyTo(parent);
            new KModEffectConfigurator("huifu", 10f, false)
                .SetEffectName("植物清香")
                .SetEffectDescription("植物香味使复制人们心旷神怡")
                .AddAttributeModifier(amounts.Stamina.deltaAttribute.Id, 1.0333333f, false, false, true)
                .AddAttributeModifier(amounts.Stress.deltaAttribute.Id, -1f, false, false, true)
                .ApplyTo(parent);
            


        }
    }
}