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
    internal class DirtyFightingFlank : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateCMB>, IRulebookHandler<RuleCalculateCMB>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateCMB>.OnEventAboutToTrigger(RuleCalculateCMB evt)
        {
            if (evt.Target.CombatState.IsFlanked)
            {
                evt.AddModifier(4, BonusType.Flanking);
            }
        }

        void IRulebookHandler<RuleCalculateCMB>.OnEventDidTrigger(RuleCalculateCMB evt)
        {
            
        }
    }
}
