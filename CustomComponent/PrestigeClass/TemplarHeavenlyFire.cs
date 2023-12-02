using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class TemplarHeavenlyFire : UnitFactComponentDelegate,
        ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>
    {
        void IRulebookHandler<RulePrepareDamage>.OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            
        }

        void IRulebookHandler<RulePrepareDamage>.OnEventDidTrigger(RulePrepareDamage evt)
        {
            if (evt.ParentRule == null) return;
            if (evt.ParentRule.DamageBundle == null) return;
            bool isSneak = false;
            foreach (var damage in evt.ParentRule.DamageBundle)
            {
                if (damage != null && damage.Sneak)
                {
                    isSneak = true;
                    break;
                }
            }
            if (isSneak)
            {
                int level = Owner.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CrimsonTemplar.ArchetypeGuid));
                int dc = 10 + level + Owner.Stats.Charisma.Bonus;
                if (evt.Target.HasFact(FeatureRefs.SubtypeEvil.Reference))
                {
                    if (level >= 10)
                    {
                        dc += 6;
                    }
                    else if (level >= 5)
                    {
                        dc += 4;
                    }
                    else
                    {
                        dc += 2;
                    }
                }
                bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, dc)
                {
                    IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
                }, evt.Target.Context, true).Success;
                if (!pass)
                {
                    var action = ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(level)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine), half: true)
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(level)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Fire), half: true)
                        .Build();
                    Fact.RunActionInContext(action, evt.Target);
                }
                Owner.Descriptor.Resources.Spend(BlueprintTool.GetRef<BlueprintAbilityResourceReference>(CrimsonTemplar.HeavenlyFireAbilityResGuid), 1);
            }
        }
    }
}
