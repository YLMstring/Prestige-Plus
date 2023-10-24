using Kingmaker.PubSubSystem;
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
            
        }

        void IRulebookHandler<RuleCalculateCMD>.OnEventDidTrigger(RuleCalculateCMD evt)
        {
            if (evt.IsTargetFlatFooted || evt.Target.CombatState.IsFlanked)
            {
                evt.Result -= Owner.Descriptor.Stats.SneakAttack;
            }
        }
    }
}
