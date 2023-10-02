using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class MetaphysicalSneakFix : UnitFactComponentDelegate,
        ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>, IInitiatorRulebookHandler<RuleAttackRoll>,
        IRulebookHandler<RuleAttackRoll>
    {
        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            
        }

        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
            if (evt.ParentRule == null) return;
            if (evt.ParentRule.DamageBundle == null) return;
            bool isSneak = false;
            foreach (var damage in evt.ParentRule.DamageBundle)
            {
                if (damage != null && damage.Sneak) 
                { 
                    damage.Precision = false; 
                    isSneak = true;
                }
            }
            if (isSneak)
            {
                Fact.RunActionInContext(Action, evt.Target);
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            evt.ImmuneToSneakAttack = false;
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }

        public ActionList Action = ActionsBuilder.New().CastSpell(AbilityRefs.StudentOfWarKnowYourEnemyAbility.ToString()).Build();
    }
}
