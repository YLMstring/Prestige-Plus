using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class RippleWater
    {
        private static readonly string FeatName = "RippleWater";
        public static readonly string FeatGuid = "{A2A76E5C-DBFC-40DA-AF18-06DBEE934588}";

        private static readonly string FeatNamePro = "RippleWaterPro";
        public static readonly string FeatGuidPro = "{F2675BE8-0E12-4D43-B485-708DA87A972F}";

        private static readonly string DisplayName = "RippleWater.Name";
        private static readonly string Description = "RippleWater.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.DeathWard.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(FeatNamePro, FeatGuidPro)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetClasses(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.MonkClass.ToString()))
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(2)
              .AddToLevelEntry(3, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(4)
              .AddToLevelEntry(5, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(6)
              .AddToLevelEntry(7, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(8)
              .AddToLevelEntry(9, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(10)
              .AddToLevelEntry(11, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(12)
              .AddToLevelEntry(13, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(14)
              .AddToLevelEntry(15, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(16)
              .AddToLevelEntry(17, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(18)
              .AddToLevelEntry(19, FeatureRefs.KineticBlastFeature.ToString())
              .AddToLevelEntry(20)
              .Configure();

            var feat = FeatureSelectionConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
                    .AddPrerequisiteFeature(FeatureRefs.KiPowerFeature.ToString())
                    .AddToAllFeatures(pro)
                    //.AddToAllFeatures(FeatureRefs.ImprovedBullRush.ToString())
                    //.AddToAllFeatures(FeatureRefs.ImprovedTrip.ToString())
                    //.AddFacts(new() { pro })
                    .AddPointBlankMaster(Kingmaker.Enums.WeaponCategory.KineticBlast)
                    .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.KiPowerResource.ToString(), ContextValues.Rank())
                    .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.KineticistClass.ToString() }).WithOnePlusDiv2Progression())
                    //.AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    //.AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    //.AddToFeatureSelection(FeatureSelectionRefs.BasicFeatSelection.ToString())
                    //.AddToFeatureSelection(FeatureSelectionRefs.FighterFeatSelection.ToString())
                    //.AddToFeatureSelection(FeatureSelectionRefs.CombatTrick.ToString())
                    //.AddToFeatureSelection(FeatureSelectionRefs.LoremasterCombatFeatSelection.ToString())
                    //.AddToFeatureSelection(FeatureSelectionRefs.LoremasterTricksterCombatFeatSelection.ToString())
                    //.AddToFeatureSelection(FeatureSelectionRefs.StudentOfWarCombatFeatSelection.ToString())
                    .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.FighterFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.CombatTrick)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterTricksterCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.StudentOfWarCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }
    }
}
