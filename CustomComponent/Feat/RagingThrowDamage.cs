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
using PrestigePlus.Blueprint;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class RagingThrowDamage : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {

        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Target == null) return;
            if (!evt.Success || Owner.HasFact(Throw)) return;
            if (evt.Type != CombatManeuver.BullRush) return;
            int damage = Owner.Stats.Strength.Bonus + Owner.Stats.Constitution.Bonus;
            PhysicalDamage Damage = new(new ModifiableDiceFormula(new DiceFormula(damage, DiceType.One)), 0, PhysicalDamageForm.Bludgeoning);
            Rulebook.Trigger(new RuleDealDamage(Owner, evt.Target, Damage));
        }

        private static BlueprintBuffReference Throw = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.BullRushAngry2buffGuid);
    }
}