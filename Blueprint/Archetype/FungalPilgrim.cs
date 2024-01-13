using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Enums;
using Kingmaker.Utility;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class FungalPilgrim
    {
        private const string ArchetypeName = "FungalPilgrim";
        private static readonly string ArchetypeGuid = "{F13C78E4-91E6-423D-95FE-B49832C01611}";
        internal const string ArchetypeDisplayName = "FungalPilgrim.Name";
        private const string ArchetypeDescription = "FungalPilgrim.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.DruidBondSelection.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.WildShapeIWolfFeature.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.WildShapeIILeopardFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .SetRemoveFeaturesEntry(8, FeatureRefs.WildShapeIVBearFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.WildShapeIIISmilodonFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.WildShapeElementaHugeFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(16, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(18, FeatureRefs.WildShapeExtraUse.ToString())
            .AddToAddFeatures(1, FeatureSelectionRefs.AnimalCompanionSelectionDruid.ToString())
            .AddToAddFeatures(4, FungalCompanionConfigure())
            .AddToAddFeatures(15, CreateTransformation())
              .Configure();

        }

        private const string Transformation = "FungalPilgrim.Transformation";
        public static readonly string TransformationGuid = "{7E89DBAA-35C5-4741-AC5D-37828D25E7E6}";

        internal const string TransformationDisplayName = "FungalPilgrimTransformation.Name";
        private const string TransformationDescription = "FungalPilgrimTransformation.Description";

        public static BlueprintFeature CreateTransformation()
        {
            var icon = AbilityRefs.FungalInfestationAbility.Reference.Get().Icon;

            return FeatureConfigurator.New(Transformation, TransformationGuid)
              .SetDisplayName(TransformationDisplayName)
              .SetDescription(TransformationDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.PlantType.ToString() })
              .Configure();
        }

        private const string FungalCompanion = "FungalPilgrim.FungalCompanion";
        public static readonly string FungalCompanionGuid = "{9236353B-C49B-46AB-A0D2-9BED174FD06D}";

        private const string FungalCompanionFeat = "FungalPilgrim.UseFungalCompanion";
        private static readonly string FungalCompanionFeatGuid = "{8C386267-494C-4C45-8C36-C17585C1966E}";

        internal const string FungalCompanionDisplayName = "FungalPilgrimFungalCompanion.Name";
        private const string FungalCompanionDescription = "FungalPilgrimFungalCompanion.Description";

        public static BlueprintFeature FungalCompanionConfigure()
        {
            var icon = AbilityRefs.FungalBurstAbility.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(FungalCompanionFeat, FungalCompanionFeatGuid)
              .SetDisplayName(FungalCompanionDisplayName)
              .SetDescription(FungalCompanionDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.PlantType.ToString() })
              .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.Speed, -10)
              .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.Strength, 4)
              .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.Dexterity, 2)
              .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.Constitution, 4)
              .AddStatBonus(ModifierDescriptor.ArmorFocus, false, StatType.AC, 2)
              .Configure();

            return FeatureConfigurator.New(FungalCompanion, FungalCompanionGuid)
              .SetDisplayName(FungalCompanionDisplayName)
              .SetDescription(FungalCompanionDescription)
              .SetIcon(icon)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .Configure();
        }
    }
}
