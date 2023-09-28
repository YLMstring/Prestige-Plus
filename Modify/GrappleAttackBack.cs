using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class GrappleAttackBack : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateCMD>, IRulebookHandler<RuleCalculateCMD>, ISubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateCMD evt)
        {
            if (evt.Type != CombatManeuver.None && evt.Type == CombatManeuver.Grapple)
            {
                if (evt.Initiator.Get<UnitPartGrappleTargetPP>()) { return; }
                float? threatRange = evt.Target.GetThreatRange(evt.Target.Body.PrimaryHand);
                if (threatRange != null && evt.Target.IsAttackOfOpportunityReach(evt.Initiator, threatRange.Value))
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(evt.Target, evt.Initiator, false);
                }
            }
        }

        // Token: 0x0600E233 RID: 57907 RVA: 0x003A0F26 File Offset: 0x0039F126
        public void OnEventDidTrigger(RuleCalculateCMD evt)
        {
        }
    }
}
