using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class TearApartSunder : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            UnitEntityData maybeCaster = base.Buff.Context.MaybeCaster;
            if (maybeCaster == null || evt.AttackType.IsTouch()) { return; }
            int penalty = maybeCaster.Progression.MythicLevel * Buff.Rank / 2;
            int bonus = CalculateNatureAndShieldBonuses(Owner);
            if (penalty < 1) { penalty = 1; }
            if (penalty > bonus) { penalty = bonus; }
            evt.AddModifier(-penalty, base.Fact, Kingmaker.Enums.ModifierDescriptor.Penalty);
        }
        private static int CalculateNatureAndShieldBonuses(UnitEntityData unit)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            int num6 = 0;
            int num7 = 0;
            int num8 = 0;
            int num9 = 0;
            foreach (ModifiableValue.Modifier modifier in unit.Stats.AC.Modifiers)
            {
                num += ((modifier.ModDescriptor == ModifierDescriptor.NaturalArmor) ? modifier.ModValue : 0);
                num2 += ((modifier.ModDescriptor == ModifierDescriptor.NaturalArmorEnhancement) ? modifier.ModValue : 0);
                num3 += ((modifier.ModDescriptor == ModifierDescriptor.NaturalArmorForm) ? modifier.ModValue : 0);
                num4 += ((modifier.ModDescriptor == ModifierDescriptor.Shield) ? modifier.ModValue : 0);
                num5 += ((modifier.ModDescriptor == ModifierDescriptor.ShieldEnhancement) ? modifier.ModValue : 0);
                num6 += ((modifier.ModDescriptor == ModifierDescriptor.Focus) ? modifier.ModValue : 0);
                num7 += ((modifier.ModDescriptor == ModifierDescriptor.Armor) ? modifier.ModValue : 0);
                num8 += ((modifier.ModDescriptor == ModifierDescriptor.ArmorEnhancement) ? modifier.ModValue : 0);
                num9 += ((modifier.ModDescriptor == ModifierDescriptor.ArmorFocus) ? modifier.ModValue : 0);
            }
            return num + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
        }
        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {
            
        }
    }
}
