using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class GiganticLimit : UnitBuffComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>
    {
        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {

        }

        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
            if (!Owner.HasFact(ChargeBuff) || Owner.Descriptor.State.Features.Pounce) { return; }
            RuleCalculateAttacksCount.AttacksCount primaryHand = evt.Result.PrimaryHand;
            RuleCalculateAttacksCount.AttacksCount secondaryHand = evt.Result.SecondaryHand;
            primaryHand.PenalizedAttacks = 0;
            primaryHand.HasteAttacks = 0;
            primaryHand.AdditionalAttacks = Math.Min(primaryHand.AdditionalAttacks, 1);
            secondaryHand.PenalizedAttacks = 0;
            secondaryHand.HasteAttacks = 0;
            secondaryHand.AdditionalAttacks = 0;
        }
        private static BlueprintBuffReference ChargeBuff = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.ChargeBuff.ToString());
    }
}
