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

namespace PrestigePlus.Modify
{
    internal class ShadowConstrict : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Target == null) return;
            if (!evt.Success) return;
            if (evt.Type != CombatManeuver.Grapple) return;
            ForceDamage Damage2 = new(new ModifiableDiceFormula(new DiceFormula(5, DiceType.D6)), 0);
            Rulebook.Trigger(new RuleDealDamage(Owner, evt.Target, Damage2));
        }

        //private static BlueprintFeatureReference Wrack = BlueprintTool.GetRef<BlueprintFeatureReference>("{527797B0-A313-453B-8558-A3F30C657623}");
        //private static BlueprintFeatureReference Shadow = BlueprintTool.GetRef<BlueprintFeatureReference>("{8146D8AA-C4CA-4924-BE9D-999BF59A139E}");
    }
}
