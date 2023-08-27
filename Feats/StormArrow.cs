using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Feats
{
    internal class StormArrow
    {
        private static readonly string FeatName = "StormArrow";
        private static readonly string FeatGuid = "{546C0AA1-250D-46EC-B243-6E00751E87D0}";

        private static readonly string DisplayName = "StormArrow.Name";
        private static readonly string Description = "StormArrow.Description";

        private static readonly string HailArrowGuid = "{188337A9-939D-4AF2-9030-34C0AD102F46}";
        private static readonly string HailArrowAblityResGuid = "{6EFB1675-724E-451C-A953-2843441F99F8}";
        public static void Configure()
        {
            var icon = FeatureRefs.Manyshot.Reference.Get().Icon;

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MythicAbilitySelection)
                .AddToAllFeatures(FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(HailArrowGuid)
                    .AddIncreaseResourceAmountBySharedValue(false, HailArrowAblityResGuid, ContextValues.Rank())
                    .AddContextRankConfig(ContextRankConfigs.MythicLevel().WithCustomProgression((1, 0), (3,1),(5,2),(7,3), (9, 4), (10, 5)))
                    .Configure())
                .Configure(delayed: true);
        }
    }
}
