using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomAction;
using Kingmaker.Blueprints;

namespace PrestigePlus.Blueprint.RogueTalent
{
    internal class StrengthSurge
    {
        private const string StrengthSurgePower = "Barbarian.StrengthSurge";
        public static readonly string StrengthSurgeGuid = "{A7B7D768-B3AB-4105-AFAB-5EAD68E1C616}";

        internal const string StrengthSurgeDisplayName = "BarbarianStrengthSurge.Name";
        private const string StrengthSurgeDescription = "BarbarianStrengthSurge.Description";

        private const string StrengthSurgeAbility = "Barbarian.StrengthSurgeAbility";
        private static readonly string StrengthSurgeAbilityGuid = "{C89457E0-7082-4158-A0E6-87D25C5C8AEE}";

        private const string StrengthSurgeAbilityRes = "Barbarian.StrengthSurgeAbilityRes";
        private static readonly string StrengthSurgeAbilityResGuid = "{6586F1EE-2336-4F75-9FE1-3E8A4A6CCF31}";

        private const string StrengthSurgeBuff = "Barbarian.StrengthSurgeBuff";
        private static readonly string StrengthSurgeBuffGuid = "{F71E8D85-7DE6-46EB-B955-5ECD8E39A1A5}";

        public static BlueprintFeature StrengthSurgeFeat()
        {
            var icon = FeatureRefs.GreaterRageFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(StrengthSurgeAbilityRes, StrengthSurgeAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var Buff = BuffConfigurator.New(StrengthSurgeBuff, StrengthSurgeBuffGuid)
              .SetDisplayName(StrengthSurgeDisplayName)
              .SetDescription(StrengthSurgeDescription)
              .SetIcon(icon)
              .AddCMBBonus(value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BarbarianClass.ToString(), CharacterClassRefs.BloodragerClass.ToString(), CharacterClassRefs.SkaldClass.ToString() }))
              .AddManeuverTrigger(ActionsBuilder.New().RemoveBuff(StrengthSurgeBuffGuid, toCaster: true).Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.None, false)
              .Configure();

            var ability = AbilityConfigurator.New(StrengthSurgeAbility, StrengthSurgeAbilityGuid)
                .CopyFrom(
                AbilityRefs.DemonRageActivateAbility,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(2))
                        .Build())
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartFocusedRageBuff.ToString()).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(2))
                        .Build())
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.BloodragerStandartRageBuff.ToString()).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(2))
                        .Build())
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.InspiredRageBuff.ToString()).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(2))
                        .Build())
                    .Build())
                .SetDisplayName(StrengthSurgeDisplayName)
                .SetDescription(StrengthSurgeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            var action = ActionsBuilder.New()
                .Add<PPrestoreRes>(c => { c.m_Resource = abilityresourse.ToReference<BlueprintAbilityResourceReference>(); c.Value = 1; })
                .Build();

            return FeatureConfigurator.New(StrengthSurgePower, StrengthSurgeGuid, FeatureGroup.RagePower)
              .SetDisplayName(StrengthSurgeDisplayName)
              .SetDescription(StrengthSurgeDescription)
              .SetIcon(icon)
              .AddFactsChangeTrigger(new() { BuffRefs.StandartRageBuff.ToString(), BuffRefs.StandartFocusedRageBuff.ToString(), BuffRefs.BloodragerStandartRageBuff.ToString(), BuffRefs.InspiredRageBuff.ToString() }, onFactGainedActions: action)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string RagingThrowPower = "Barbarian.RagingThrow";
        public static readonly string RagingThrowGuid = "{EC769C88-80CC-4246-8F62-6DBB7AC8AA06}";

        internal const string RagingThrowDisplayName = "FeatRagingThrow.Name";
        private const string RagingThrowDescription = "FeatRagingThrow.Description";

        private const string RagingThrowAbility = "Barbarian.RagingThrowAbility";
        private static readonly string RagingThrowAbilityGuid = "{158083ED-DA80-4B40-A517-BF35A2D5F617}";

        private const string RagingThrowBuff = "Barbarian.RagingThrowBuff";
        public static readonly string RagingThrowBuffGuid = "{44AFDB84-40D8-49A4-8FE9-B6630C057F5F}";

        public static BlueprintFeature RagingThrowFeat()
        {
            var icon = FeatureRefs.MightyRage.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(RagingThrowBuff, RagingThrowBuffGuid)
              .SetDisplayName(RagingThrowDisplayName)
              .SetDescription(RagingThrowDescription)
              .SetIcon(icon)
              .AddComponent<RagingThrowDamage>()
              .AddCMBBonusForManeuver(value: ContextValues.Rank(), maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush })
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Constitution))
              .AddManeuverTrigger(ActionsBuilder.New()
                    .RemoveBuff(RagingThrowBuffGuid, toCaster: true)
                    .Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush, false)
              .AddBuffActions(activated: ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New()
                        .UseOr()
                        .CasterHasFact(BuffRefs.StandartRageBuff.ToString())
                        .CasterHasFact(BuffRefs.StandartFocusedRageBuff.ToString())
                        .CasterHasFact(BuffRefs.BloodragerStandartRageBuff.ToString())
                        .CasterHasFact(BuffRefs.InspiredRageEffectBuff.ToString())
                        .CasterHasFact(BuffRefs.InspiredRageEffectBuffMythic.ToString())
                        .Build(), 
                    ifFalse: ActionsBuilder.New()
                        .RemoveSelf()
                        .Build())
                    .Build())
              .Configure();

            var ability = AbilityConfigurator.New(RagingThrowAbility, RagingThrowAbilityGuid)
                .CopyFrom(
                AbilityRefs.DemonRageActivateAbility,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ContextSpendResource(AbilityResourceRefs.RageResourse.ToString(), 1)
                    .ContextSpendResource(AbilityResourceRefs.BloodragerRageResource.ToString(), 1)
                    .ContextSpendResource(AbilityResourceRefs.FocusedRageResourse.ToString(), 1)
                    .ApplyBuff(Buff, ContextDuration.Fixed(1))
                    .Build())
                .SetDisplayName(RagingThrowDisplayName)
                .SetDescription(RagingThrowDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(RagingThrowPower, RagingThrowGuid, FeatureGroup.Feat)
              .SetDisplayName(RagingThrowDisplayName)
              .SetDescription(RagingThrowDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Strength, 13)
              .AddPrerequisiteStatValue(StatType.Constitution, 13)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
              .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
