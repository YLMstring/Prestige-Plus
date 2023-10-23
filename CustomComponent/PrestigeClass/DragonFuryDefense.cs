using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class DragonFuryDefense : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (!DualFangFocus.IsDual(Owner)) return;
            int bonus = Fact.GetRank();
            evt.AddModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Shield);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

    }
}
