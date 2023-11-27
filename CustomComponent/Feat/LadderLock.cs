using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Root;
using PrestigePlus.CustomComponent.Grapple;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class LadderLock : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (!evt.Success || !ConditionTwoFreeHand.CheckCondition2(Owner)) return;
            if (evt.Type != CombatManeuver.DirtyTrickBlind && evt.Type != CombatManeuver.DirtyTrickSickened) return;
            Rounds rounds = evt.IncreasedDuration ? (evt.SuccessMargin / 5 + RulebookEvent.Dice.D4).Rounds() : (evt.SuccessMargin / 5 + 1).Rounds();
            evt.Target.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff, evt.Initiator, new TimeSpan?(rounds.Seconds));
        }
    }
}