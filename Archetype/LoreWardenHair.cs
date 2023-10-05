using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Archetype
{
    internal class LoreWardenHair : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleDealDamage>.OnEventAboutToTrigger(RuleDealDamage evt)
        {
            if (evt.AttackRoll?.Result == AttackResult.CriticalHit)
            {
                int dc = evt.AttackRoll.CriticalConfirmationRoll;
                if (GameHelper.TriggerSkillCheck(new RuleSkillCheck(Owner, Kingmaker.EntitySystem.Stats.StatType.SkillMobility, dc)
                {
                    IgnoreDifficultyBonusToDC = false
                }, Owner.Context, true).Success)
                {
                    evt.AttackRoll.Result = AttackResult.Hit;
                }
                Fact.RunActionInContext(Action, evt.Target);
                if (!Owner.Descriptor.Resources.HasEnoughResource(Resource, 1))
                {
                    Owner.Descriptor.RemoveFact(Buff);
                }
            }
        }

        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {

        }

        private static BlueprintAbilityResourceReference Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("{993D5BAE-1EC5-487D-8216-C81190A7FF59}");
        public ActionList Action = ActionsBuilder.New().ContextSpendResource(Resource, ContextValues.Constant(1)).Build();
    }
}
