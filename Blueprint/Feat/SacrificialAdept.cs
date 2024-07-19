using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Feat
{
    internal class SacrificialAdept
    {
        private static readonly string FeatName = "SacrificialAdept";
        public static readonly string FeatGuid = "{6BF49E34-FEE8-4E9E-A966-92658EA8EF31}";

        private static readonly string DisplayName = "SacrificialAdept.Name";
        private static readonly string Description = "SacrificialAdept.Description";

        private const string SacrificialAdeptAbility = "SacrificialAdept.SacrificialAdeptAbility";
        private static readonly string SacrificialAdeptAbilityGuid = "{76D2ACE2-8480-4A6B-825A-A2F756364A0D}";

        private const string SacrificialAdeptbuff = "SacrificialAdept.SacrificialAdeptbuff";
        public static readonly string SacrificialAdeptbuffGuid = "{94CA592F-6D87-4571-A638-B5C95315D6D3}";

        public static void Configure()
        {
            var icon = AbilityRefs.Dictum.Reference.Get().Icon;

            var BuffSacrificialAdept = BuffConfigurator.New(SacrificialAdeptbuff, SacrificialAdeptbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<SacrificialAdeptComp>()
              .AddUniqueBuff()
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(BuffSacrificialAdept, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days))
                .Build();

            var abilityTrick = AbilityConfigurator.New(SacrificialAdeptAbility, SacrificialAdeptAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddComponent(AbilityRefs.NegativeEruption.Reference.Get().GetComponent<AbilitySpawnFx>())
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .OnContextCaster(action, true)
                        .Kill(Kingmaker.UnitLogic.UnitState.DismemberType.LimbsApart)
                        .Build())
                .AllowTargeting(false, false, true, false)
                .AddAbilityCasterInCombat(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .SetType(AbilityType.Supernatural)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 4)
                    .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 4)
                    .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 4)
                    .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 4)
                    .AddPrerequisiteAlignment(AlignmentMaskType.Evil)
                    .AddComponent<PrerequisiteSpellLevel>(c => { c.RequiredSpellLevel = 3; })
                    .AddFacts(new() { abilityTrick })
                    .Configure();
        }
    }

    internal class SacrificialAdeptComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (evt.Reason?.Ability?.Blueprint.Type == AbilityType.Spell && Buff?.Context?.MaybeCaster?.State?.IsDead == true)
            {
                evt.AddModifier(2, Fact);
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {

        }

        void IRulebookHandler<RuleSpellResistanceCheck>.OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            if (Buff?.Context?.MaybeCaster?.State?.IsDead == true)
            {
                evt.AddSpellPenetration(2);
            }
        }

        void IRulebookHandler<RuleSpellResistanceCheck>.OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {

        }
    }
}
