using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class InternalAlchemist
    {
        private const string ArchetypeName = "InternalAlchemist";
        private static readonly string ArchetypeGuid = "{56C10E53-861A-4DAE-8009-C87A69B9EB5A}";
        internal const string ArchetypeDisplayName = "InternalAlchemist.Name";
        private const string ArchetypeDescription = "InternalAlchemist.Description";
        public static void Configure()
        {
            ProgressionConfigurator.For(ProgressionRefs.AlchemistProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { DiseaseResistanceGuid, FeatureRefs.PurityOfBody.ToString() })
                .Configure();

            InternalBonusFeatConfigure();

            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.AlchemistClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.AlchemistThrowAnything.ToString(), FeatureRefs.BrewPotions.ToString())
            .AddToAddFeatures(2, CreateDiseaseResistance())
            .AddToAddFeatures(5, DiseaseResistanceGuid)
            .AddToAddFeatures(6, FeatureRefs.UncannyDodgeChecker.ToString())
            .AddToAddFeatures(8, DiseaseResistanceGuid)
            .AddToAddFeatures(10, FeatureRefs.PurityOfBody.ToString())
              .Configure();
        }

        private const string DiseaseResistance = "InternalAlchemist.DiseaseResistance";
        private static readonly string DiseaseResistanceGuid = "{9789DA30-B04F-4E09-870D-6F97060D6B10}";

        internal const string DiseaseResistanceDisplayName = "InternalAlchemistDiseaseResistance.Name";
        private const string DiseaseResistanceDescription = "InternalAlchemistDiseaseResistance.Description";

        private static BlueprintFeature CreateDiseaseResistance()
        {
            var icon = AbilityRefs.AngelWardOfDisease.Reference.Get().Icon;

            return FeatureConfigurator.New(DiseaseResistance, DiseaseResistanceGuid)
              .SetDisplayName(DiseaseResistanceDisplayName)
              .SetDescription(DiseaseResistanceDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(value: 2, spellDescriptor: SpellDescriptor.Disease)
              .SetRanks(5)
              .Configure();
        }

        private const string InternalBonusFeatFeat = "InternalAlchemist.InternalBonusFeat";
        public static readonly string InternalBonusFeatGuid = "{A59966E9-A57B-4240-8104-691CF521EE48}";

        internal const string InternalBonusFeatDisplayName = "InternalBonusFeat.Name";
        private const string InternalBonusFeatDescription = "InternalBonusFeat.Description";
        public static void InternalBonusFeatConfigure()
        {
            var icon = AbilityRefs.TrickeryBlessingMajorAbility.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(InternalBonusFeatFeat, InternalBonusFeatGuid)
              .SetDisplayName(InternalBonusFeatDisplayName)
              .SetDescription(InternalBonusFeatDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddPrerequisiteArchetypeLevel(ArchetypeGuid, CharacterClassRefs.AlchemistClass.ToString())
              .AddToAllFeatures(FeatureRefs.Alertness.ToString())
              .AddToAllFeatures(FeatureRefs.ExtraKi.ToString())
              .AddToAllFeatures(FeatureRefs.GreatFortitude.ToString())
              .AddToAllFeatures(ParametrizedFeatureRefs.ImprovedCritical.ToString())
              .AddToAllFeatures(FeatureRefs.Improved_Initiative.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddToAllFeatures(BodyGuard.GreaterUnarmedStrikeGuid)
              .AddToAllFeatures(FeatureRefs.IronWill.ToString())
              .AddToAllFeatures(FeatureRefs.LightningReflexes.ToString())
              .AddToAllFeatures(FeatureRefs.StunningFist.ToString())
              .AddToAllFeatures(ParametrizedFeatureRefs.WeaponFocus.ToString())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DiscoverySelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

    }
}
