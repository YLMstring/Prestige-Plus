using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
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
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
            if (!Owner.HasFact(BuffRefs.ChargeBuff.Reference) || Owner.Descriptor.State.Features.Pounce || Owner.HasFact(FeatureRefs.PummelingCharge.Reference)) { return; }
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
            if (!Owner.HasFact(BuffRefs.MountedBuff.Reference)) return;
            GameHelper.ApplyBuff(Owner, BuffRefs.ChargeBuff.Reference, new Rounds?(1.Rounds()));
            var list = new List<Buff>() { };
            foreach (Buff buff in base.Owner.Buffs)
            {
                list.Add(buff);
            }
            foreach (Buff buff in list)
            {
                var comp = buff.Blueprint.GetComponent<AddAbilityUseTrigger>();
                if (comp != null && comp.Ability == AbilityRefs.ChargeAbility.Reference.Get())
                {
                    Fact.RunActionInContext(comp.Action, Owner);
                }
            }
        }
    }
}
