using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class GiganticLimit : UnitBuffComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, IUnitNewCombatRoundHandler, IGlobalSubscriber
    {
        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
            if (!Owner.HasFact(BuffRefs.ChargeBuff.Reference) || Owner.Descriptor.State.Features.Pounce) { return; }
            RuleCalculateAttacksCount.AttacksCount primaryHand = evt.Result.PrimaryHand;
            RuleCalculateAttacksCount.AttacksCount secondaryHand = evt.Result.SecondaryHand;
            primaryHand.PenalizedAttacks = 0;
            primaryHand.HasteAttacks = 0;
            primaryHand.AdditionalAttacks = Math.Min(primaryHand.AdditionalAttacks, 1);
            secondaryHand.PenalizedAttacks = 0;
            secondaryHand.HasteAttacks = 0;
            secondaryHand.AdditionalAttacks = 0;
        }

        void IUnitNewCombatRoundHandler.HandleNewCombatRound(UnitEntityData unit)
        {
            if (unit != base.Owner) { return; }
            foreach (Buff buff in base.Owner.Buffs)
            {
                var comp = buff.Blueprint.GetComponent<AddAbilityUseTrigger>();
                var actions = comp.Action?.Actions;
                if (comp?.m_Ability == AbilityRefs.ChargeAbility.Reference && actions?.Any() == true)
                {
                    Fact.RunActionInContext(comp.Action);
                }
            }
        }
    }
}
