using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class KrakenDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Target == null) return;
            if (!evt.Success) return;
            int damage = Owner.Stats.Wisdom.Bonus;
            if (Owner.HasFact(Wrack))
            {
                damage += 4;
                ForceDamage Damage2 = new(new ModifiableDiceFormula(new DiceFormula(damage, DiceType.One)), 0);
                Rulebook.Trigger(new RuleDealDamage(Owner, evt.Target, Damage2));
            }
            else
            {
                PhysicalDamage Damage = new(new ModifiableDiceFormula(new DiceFormula(damage, DiceType.One)), 0, PhysicalDamageForm.Bludgeoning);
                Rulebook.Trigger(new RuleDealDamage(Owner, evt.Target, Damage));
            }
        }

        private static BlueprintFeatureReference Wrack = BlueprintTool.GetRef<BlueprintFeatureReference>("{527797B0-A313-453B-8558-A3F30C657623}");
    }
}
