using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class SmashingComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Target == null) return;
            if (!evt.Success) return;
            if (evt.Type != starter) return;
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(evt.Initiator, evt.Target, effect, null);
            ruleCombatManeuver.OverrideBonus = new int?(evt.TotalBonusValue);
            Rulebook.Trigger(ruleCombatManeuver);
        }
        public CombatManeuver starter = CombatManeuver.SunderArmor;
        public CombatManeuver effect = CombatManeuver.SunderArmor;
    }
}
