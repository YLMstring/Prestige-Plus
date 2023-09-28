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

namespace PrestigePlus.Maneuvers
{
    internal class StrengthSurge
    {
        private const string StrengthSurgePower = "Barbarian.StrengthSurge";
        private static readonly string StrengthSurgeGuid = "{A7B7D768-B3AB-4105-AFAB-5EAD68E1C616}";

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
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BarbarianClass.ToString(), CharacterClassRefs.BloodragerClass.ToString() }))
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
                .RestoreResource(abilityresourse, ContextValues.Constant(1))
                .Build();

            BuffConfigurator.For(BuffRefs.StandartRageBuff)
            .EditComponent<AddFactContextActions>(
                a => a.Activated.Actions = CommonTool.Append(a.Activated.Actions, action.Actions))
              .Configure();

            BuffConfigurator.For(BuffRefs.StandartFocusedRageBuff)
            .EditComponent<AddFactContextActions>(
                a => a.Activated.Actions = CommonTool.Append(a.Activated.Actions, action.Actions))
              .Configure();

            BuffConfigurator.For(BuffRefs.BloodragerStandartRageBuff)
            .EditComponent<AddFactContextActions>(
                a => a.Activated.Actions = CommonTool.Append(a.Activated.Actions, action.Actions))
              .Configure();

            return FeatureConfigurator.New(StrengthSurgePower, StrengthSurgeGuid)
              .SetDisplayName(StrengthSurgeDisplayName)
              .SetDescription(StrengthSurgeDescription)
              .SetIcon(icon)
              .AddToGroups(FeatureGroup.RagePower)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
