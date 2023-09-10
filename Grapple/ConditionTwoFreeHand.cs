using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    internal class ConditionTwoFreeHand : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {

        private  bool CheckCondition()
        {
            var secondaryHand = Owner.Body.CurrentHandsEquipmentSet.SecondaryHand;
            var primaryHand = Owner.Body.CurrentHandsEquipmentSet.PrimaryHand;
            bool hasFreeHand = true;
            if (secondaryHand.HasShield)
            {
                var maybeShield = secondaryHand.MaybeShield;
                hasFreeHand &= maybeShield.Blueprint.Type.ProficiencyGroup == ArmorProficiencyGroup.Buckler;
            }
            else if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != Owner.Body.EmptyHandWeapon)
            {
                hasFreeHand = false;
            }
            if (primaryHand.HasWeapon && primaryHand.MaybeWeapon != Owner.Body.EmptyHandWeapon)
            {
                hasFreeHand = false;
            }
            return hasFreeHand;
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type != CombatManeuver.Grapple) { return; }
            if (CheckCondition()) { return; }
            evt.AddModifier(4, base.Fact, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            
        }
    }
}