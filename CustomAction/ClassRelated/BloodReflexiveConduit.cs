using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.ElementsSystem;
using PrestigePlus.Grapple;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class BloodReflexiveConduit : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateCMD>, IRulebookHandler<RuleCalculateCMD>, ISubscriber, ITargetRulebookSubscriber
    {
        // Token: 0x0600E553 RID: 58707 RVA: 0x003AF714 File Offset: 0x003AD914
        public void OnEventAboutToTrigger(RuleCalculateCMD evt)
        {
            if (evt.Type != CombatManeuver.None && evt.Type == ManeuverType)
            {
                if (evt.Type == CombatManeuver.Grapple && evt.Initiator.Get<UnitPartGrappleTargetPP>()) { return; }
                (base.Fact as IFactContextOwner)?.RunActionInContext(Action, Owner);
            }
        }

        // Token: 0x0600E554 RID: 58708 RVA: 0x003AF792 File Offset: 0x003AD992
        public void OnEventDidTrigger(RuleCalculateCMD evt)
        {
        }

        public CombatManeuver ManeuverType;
        public ActionList Action;
    }
}