using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.Enums;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class HiddenSpellComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, IInitiatorRulebookSubscriber, IGlobalRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, IGlobalRulebookSubscriber
    {
        private int HiddenLevel(UnitEntityData target)
        {
            if (target.IsDirectlyControllable) { return 0; }
            int level = Fact.GetRank();
            if (target.IsPlayerFaction) { return level; }
            if (target.Memory.Enemies.Count == 0) { return level; }
            bool unknown = true;
            foreach (var info in target.Memory.Enemies)
            {
                if (Owner == info.Unit)
                {
                    unknown = false;
                }
            }
            if (unknown) { return level; }
            if (level < 2) { return 0; }
            if (target.CombatState.IsFlanked) { return level - 1; }
            if (Rulebook.Trigger(new RuleCheckTargetFlatFooted(Owner, target)).IsFlatFooted)
            {
                return level - 1;
            }
            return 0;
        }

        void IRulebookHandler<RuleSpellResistanceCheck>.OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            if (evt.Ability.Type == AbilityType.Spell)
            {
                evt.AddSpellPenetration(HiddenLevel(evt.Target), ModifierDescriptor.Circumstance);
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (evt.Reason?.Caster == Owner && evt.Reason.Ability?.Blueprint.Type == AbilityType.Spell)
            {
                int penalty = -HiddenLevel(evt.Initiator);
                if (penalty < 0)
                {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(penalty, base.Runtime, ModifierDescriptor.Penalty));
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(penalty, base.Runtime, ModifierDescriptor.Penalty));
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(penalty, base.Runtime, ModifierDescriptor.Penalty));
                }
            }
        }

        void IRulebookHandler<RuleSpellResistanceCheck>.OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {
            
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }
    }
}
