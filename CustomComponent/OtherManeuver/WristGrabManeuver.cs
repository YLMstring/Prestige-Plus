using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.OtherManeuver
{
    internal class WristGrabManeuver : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (!evt.IsHit && evt.Result != AttackResult.MirrorImage && evt.Result != AttackResult.Concealment && evt.D20 + evt.AttackBonus <= evt.TargetAC - 5)
            {
                if (Owner.HasFact(CasterBuff) || !Owner.CombatState.EngagedUnits.Contains(evt.Initiator)) { return; }
                SweepManeuver.ActManeuver(Owner, evt.Initiator, 0);
                GameHelper.ApplyBuff(Owner, CasterBuff, new Rounds?(1.Rounds()));
            }
        }

        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>(RangedDisarm.WristGrabbuffGuid);
    }
}
