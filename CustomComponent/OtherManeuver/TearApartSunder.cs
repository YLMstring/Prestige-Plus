using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
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

namespace PrestigePlus.CustomComponent.OtherManeuver
{
    internal class TearApartSunder : UnitBuffComponentDelegate<ACBonusAgainstAttacks.RuntimeData>, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (Data.Initiator != null && evt.Initiator == Data.Initiator)
            {
                int bonus = CalculateNatureAndShieldBonuses(Owner);
                int penalty = Math.Min(bonus, Data.ACModifier);
                if (penalty < 1) return;
                evt.AddModifier(-penalty, Fact, ModifierDescriptor.Penalty);
            }
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
                num += modifier.ModDescriptor == ModifierDescriptor.NaturalArmor ? modifier.ModValue : 0;
                num2 += modifier.ModDescriptor == ModifierDescriptor.NaturalArmorEnhancement ? modifier.ModValue : 0;
                num3 += modifier.ModDescriptor == ModifierDescriptor.NaturalArmorForm ? modifier.ModValue : 0;
                num4 += modifier.ModDescriptor == ModifierDescriptor.Shield ? modifier.ModValue : 0;
                num5 += modifier.ModDescriptor == ModifierDescriptor.ShieldEnhancement ? modifier.ModValue : 0;
                num6 += modifier.ModDescriptor == ModifierDescriptor.Focus ? modifier.ModValue : 0;
                num7 += modifier.ModDescriptor == ModifierDescriptor.Armor ? modifier.ModValue : 0;
                num8 += modifier.ModDescriptor == ModifierDescriptor.ArmorEnhancement ? modifier.ModValue : 0;
                num9 += modifier.ModDescriptor == ModifierDescriptor.ArmorFocus ? modifier.ModValue : 0;
            }
            return num + num2 + num3 + num4 + num5 + num6 + num7 + num8 + num9;
        }
        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null || evt.AttackType.IsTouch()) { return; }
            int penalty = Math.Max(maybeCaster.Progression.MythicLevel / 2, 1) * Buff.Rank;
            Data.ACModifier = penalty;
            Data.Initiator = evt.Initiator;
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            Data.Clear();
        }
    }
}
