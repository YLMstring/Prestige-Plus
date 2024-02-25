using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.PrestigeClass;
using Kingmaker.Utility;

namespace PrestigePlus.Blueprint.Feat
{
    internal class DiverseObedience
    {
        private static readonly string FeatNamePro = "DiverseObediencePro";
        public static readonly string FeatGuidPro = "{49DA599E-F9C5-4F20-8009-B5F640980F7D}";

        private static readonly string DisplayName = "DiverseObedience.Name";
        private static readonly string Description = "DiverseObedience.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.SearingLight.Reference.Get().Icon;

            var pro = ProgressionConfigurator.New(FeatNamePro, FeatGuidPro)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddPrerequisiteFeature(DeificObedience.DeificObedienceGuid)
              .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.SkillLoreReligion, 5)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, ExaltedEvangelist.BonusFeatGuid, Sentinel.BonusFeatGuid)
              .AddToLevelEntry(10, Sentinel.DivineBoon1Guid)
              .AddToLevelEntry(14, Sentinel.DivineBoon2Guid)
              .AddToLevelEntry(18, Sentinel.DivineBoon3Guid)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(pro)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DragonLevel2FeatSelection)
                .AddToAllFeatures(pro)
                .Configure();
        }
    }
}
