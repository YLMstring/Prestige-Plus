using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class UntamedRager
    {
        private const string ArchetypeName = "UntamedRager";
        private static readonly string ArchetypeGuid = "{2498CB91-8A76-4A18-A0D0-957AB6F57452}";
        internal const string ArchetypeDisplayName = "UntamedRager.Name";
        private const string ArchetypeDescription = "UntamedRager.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BarbarianClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetRemoveFeaturesEntry(2, FeatureRefs.UncannyDodgeChecker.ToString())
              .SetRemoveFeaturesEntry(5, FeatureRefs.ImprovedUncannyDodge.ToString())
              .SetRemoveFeaturesEntry(3, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(6, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(9, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(12, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(15, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(18, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(7, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(10, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(13, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(16, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(19, FeatureRefs.DamageReduction.ToString())
              .AddToAddFeatures(2, FeatureRefs.ImprovedDirtyTrick.ToString())
              .AddToAddFeatures(5, FeatureRefs.GreaterDirtyTrick.ToString())
              .AddToAddFeatures(3, CreateFeralAppearance())
              .AddToAddFeatures(4, FeralAppearanceGuid)
              .AddToAddFeatures(5, FeralAppearanceGuid)
              .AddToAddFeatures(12, FeralAppearanceGuid)
              .AddToAddFeatures(15, FeralAppearanceGuid)
              .AddToAddFeatures(18, FeralAppearanceGuid)
              .AddToAddFeatures(1, CreateDishonorable())
              .AddToAddFeatures(2, DishonorableGuid)
              .AddToAddFeatures(3, DishonorableGuid)
              .AddToAddFeatures(16, DishonorableGuid)
              .AddToAddFeatures(19, DishonorableGuid)
              .Configure();
        }

        private const string FeralAppearance = "UntamedRager.FeralAppearance";
        private static readonly string FeralAppearanceGuid = "{7F0FEBA3-6D50-4DF3-9BAE-2737B1C5914E}";

        internal const string FeralAppearanceDisplayName = "UntamedRagerFeralAppearance.Name";
        private const string FeralAppearanceDescription = "UntamedRagerFeralAppearance.Description";
        private static BlueprintFeature CreateFeralAppearance()
        {
            var icon = FeatureRefs.FeralChampionBlessing.Reference.Get().Icon;

            return FeatureConfigurator.New(FeralAppearance, FeralAppearanceGuid)
              .SetDisplayName(FeralAppearanceDisplayName)
              .SetDescription(FeralAppearanceDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddContextStatBonus(StatType.CheckIntimidate, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(FeralAppearanceGuid))
              .Configure();
        }

        private const string Dishonorable = "UntamedRager.Dishonorable";
        private static readonly string DishonorableGuid = "{2795CAF9-0854-4849-9C06-902494270858}";

        internal const string DishonorableDisplayName = "UntamedRagerDishonorable.Name";
        private const string DishonorableDescription = "UntamedRagerDishonorable.Description";
        private static BlueprintFeature CreateDishonorable()
        {
            var icon = FeatureRefs.HalfOrcFerocity.Reference.Get().Icon;
            var mans = new CombatManeuver[] { CombatManeuver.DirtyTrickBlind, CombatManeuver.DirtyTrickEntangle, CombatManeuver.DirtyTrickSickened };

            return FeatureConfigurator.New(Dishonorable, DishonorableGuid)
              .SetDisplayName(DishonorableDisplayName)
              .SetDescription(DishonorableDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddCMBBonusForManeuver(maneuvers: mans, value: ContextValues.Rank())
              .AddCMDBonusAgainstManeuvers(maneuvers: mans, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(FeralAppearanceGuid))
              .Configure();
        }
    }
}
