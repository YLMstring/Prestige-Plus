using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.OtherFeatRelated;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class HighGuardian
    {
        private const string ArchetypeName = "HighGuardian";
        private static readonly string ArchetypeGuid = "{0120D0F9-EDE4-4D0F-8E83-8D2908BD45DD}";
        internal const string ArchetypeDisplayName = "HighGuardian.Name";
        private const string ArchetypeDescription = "HighGuardian.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToRemoveFeatures(2, FeatureSelectionRefs.FighterFeatSelection.ToString(), FeatureRefs.Bravery.ToString())
            .AddToRemoveFeatures(4, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToRemoveFeatures(6, FeatureRefs.Bravery.ToString())
            .AddToRemoveFeatures(10, FeatureRefs.Bravery.ToString())
            .AddToRemoveFeatures(14, FeatureRefs.Bravery.ToString())
            .AddToRemoveFeatures(18, FeatureRefs.Bravery.ToString())
            .AddToAddFeatures(1, RightHandConfigure())
            .AddToAddFeatures(2, CreateDefenderReflexes(), CreateUnassailableAllegiance())
            .AddToAddFeatures(4, FeatureRefs.DivineGuardianTrothFeature.ToString())
            .AddToAddFeatures(6, UnassailableAllegianceGuid)
            .AddToAddFeatures(10, UnassailableAllegianceGuid)
            .AddToAddFeatures(14, UnassailableAllegianceGuid)
            .AddToAddFeatures(18, UnassailableAllegianceGuid)
            .Configure();

            FeatureConfigurator.For(FeatureRefs.CombatReflexes)
                .AddPrerequisiteNoFeature(DefenderReflexesGuid)
                .Configure();
        }

        private static readonly string RightHandName = "HighGuardianRightHand";
        public static readonly string RightHandGuid = "{8CF81BF8-AC54-4459-9F97-342C52C03F87}";

        private static readonly string RightHandDisplayName = "HighGuardianRightHand.Name";
        private static readonly string RightHandDescription = "HighGuardianRightHand.Description";

        private const string RightHandAbility = "HighGuardianRightHand.RightHandAbility";
        private static readonly string RightHandAbilityGuid = "{F93465E3-7171-449F-BA87-45B2FC37F24A}";
        public static BlueprintFeature RightHandConfigure()
        {
            var icon = FeatureRefs.CoordinatedDefense.Reference.Get().Icon;

            var grab = ActionsBuilder.New()
                .Add<ContextActionCastling>(c => { c.consume5 = false; })
                .Build();

            var ability = AbilityConfigurator.New(RightHandAbility, RightHandAbilityGuid)
                .SetDisplayName(RightHandDisplayName)
                .SetDescription(RightHandDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(grab)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(false)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(true)
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(5)
                .AddAbilityTargetHasFact(new() { BuffRefs.DivineGuardianTrothBuff.ToString() })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(RightHandName, RightHandGuid)
                    .SetDisplayName(RightHandDisplayName)
                    .SetDescription(RightHandDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ability })
                    .AddAbilityResources(resource: AbilityResourceRefs.DivineGuardianTrothResource.ToString(), restoreAmount: true)
                    .Configure();
        }

        private const string DefenderReflexes = "HighGuardian.DefenderReflexes";
        public static readonly string DefenderReflexesGuid = "{39AA4CC0-0C01-42E4-813E-20EF8A89B12D}";

        internal const string DefenderReflexesDisplayName = "HighGuardianDefenderReflexes.Name";
        private const string DefenderReflexesDescription = "HighGuardianDefenderReflexes.Description";
        private static BlueprintFeature CreateDefenderReflexes()
        {
            var icon = FeatureRefs.CombatReflexes.Reference.Get().Icon;

            return FeatureConfigurator.New(DefenderReflexes, DefenderReflexesGuid)
              .SetDisplayName(DefenderReflexesDisplayName)
              .SetDescription(DefenderReflexesDescription)
              .SetIcon(icon)
              .AddRemoveFeatureOnApply(FeatureRefs.CombatReflexes.ToString())
              .AddCondition(Kingmaker.UnitLogic.UnitCondition.AttackOfOpportunityBeforeInitiative)
              .AddRecalculateOnStatChange(stat: StatType.Strength)
              .AddDerivativeStatBonus(StatType.Strength, StatType.AttackOfOpportunityCount, Kingmaker.Enums.ModifierDescriptor.Feat)
              .Configure();
        }

        private const string UnassailableAllegiance = "HighGuardian.UnassailableAllegiance";
        private static readonly string UnassailableAllegianceGuid = "{FEC67879-0CB3-41BA-AA85-DB2150DD4608}";

        internal const string UnassailableAllegianceDisplayName = "HighGuardianUnassailableAllegiance.Name";
        private const string UnassailableAllegianceDescription = "HighGuardianUnassailableAllegiance.Description";

        private static BlueprintFeature CreateUnassailableAllegiance()
        {
            var icon = FeatureRefs.IronWill.Reference.Get().Icon;

            return FeatureConfigurator.New(UnassailableAllegiance, UnassailableAllegianceGuid)
              .SetDisplayName(UnassailableAllegianceDisplayName)
              .SetDescription(UnassailableAllegianceDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .AddSavingThrowBonusAgainstDescriptor(value: 1, spellDescriptor: SpellDescriptor.Compulsion, bonus: 0)
              .Configure();
        }
    }
}

