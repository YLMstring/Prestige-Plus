using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SurprisedManeuverComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateCMD>, IRulebookHandler<RuleCalculateCMD>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateCMD>.OnEventAboutToTrigger(RuleCalculateCMD evt)
        {
            if (evt.Target.CombatState.IsFlanked || Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted)
            {
                evt.AddModifier(-evt.Initiator.Descriptor.Stats.SneakAttack, Fact, ModifierDescriptor.Penalty);
            }
        }

        void IRulebookHandler<RuleCalculateCMD>.OnEventDidTrigger(RuleCalculateCMD evt)
        {
            
        }
    }
}
