using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
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
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using UnityEngine;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class BullRushFollower : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (!evt.Success) return;
            if (evt.Type != CombatManeuver.BullRush) return;
            
            if (evt.Target?.Get<UnitPartForceMove>() != null && Owner.Get<UnitPartForceMove>() == null)
            {
                Feet feet = 5.Feet() + evt.SuccessMargin.Feet();
                Vector3 normalized = (evt.Target.Position - Owner.Position).normalized;
                Owner.Ensure<UnitPartForceMove>().Push(normalized, feet.Meters, true);
            }
        }
    }
}

