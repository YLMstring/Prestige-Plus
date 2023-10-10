using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class DualFangFocus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>
    {
        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (!IsDual(Owner)) return;
            int bonus = Fact.GetRank();
            evt.AddModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Competence);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (!IsDual(Owner)) return;
            int bonus = Fact.GetRank();
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Competence));
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        public static bool IsDual(UnitEntityData unit) 
        {
            var secondaryHand = unit.Body.CurrentHandsEquipmentSet.SecondaryHand;
            //var primaryHand = unit.Body.CurrentHandsEquipmentSet.PrimaryHand;
            if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != unit.Body.EmptyHandWeapon)
            {
                return true;
            }
            return false;
        }
    }
}
