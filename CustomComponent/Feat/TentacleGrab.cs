using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
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
    internal class TentacleGrab : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type == CombatManeuver.Grapple && !Owner.Body.IsPolymorphed)
            {
                evt.AddModifier(4, base.Fact);
            }
        }

        // Token: 0x0600E4FE RID: 58622 RVA: 0x003AE98B File Offset: 0x003ACB8B
        public void OnEventDidTrigger(RuleCombatManeuver evt)
        {
        }
    }
}
