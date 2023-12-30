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
            .AddToAddFeatures(1, FeatureRefs.DivineGuardianTrothFeature.ToString(), FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
            .AddToAddFeatures(2, CreateDefenderReflexes(), CreateUnassailableAllegiance())
            .AddToAddFeatures(4, BodyGuard.FeatGuid, BodyGuard.Feat2Guid)
            .AddToAddFeatures(6, UnassailableAllegianceGuid)
            .AddToAddFeatures(10, UnassailableAllegianceGuid)
            .AddToAddFeatures(14, UnassailableAllegianceGuid)
            .AddToAddFeatures(18, UnassailableAllegianceGuid)
            .Configure();

            FeatureConfigurator.For(FeatureRefs.CombatReflexes)
                .AddPrerequisiteNoFeature(DefenderReflexesGuid)
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
              .AddSavingThrowBonusAgainstDescriptor(value: 1, spellDescriptor: SpellDescriptor.Compulsion)
              .Configure();
        }
    }
}

