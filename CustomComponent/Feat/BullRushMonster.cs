using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class BullRushMonster : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (!evt.Success) return;
            if (evt.Type != CombatManeuver.BullRush) return;
            if (evt.SuccessMargin >= 10)
            {
                if (evt.Target.CanBeKnockedOff())
                {
                    evt.Target.Descriptor.State.Prone.ShouldBeActive = true;
                    EventBus.RaiseEvent(delegate (IKnockOffHandler h)
                    {
                        h.HandleKnockOff(Owner, evt.Target);
                    }, true);
                }
            }
        }
    }
}
