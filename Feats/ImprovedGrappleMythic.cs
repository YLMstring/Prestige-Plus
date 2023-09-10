using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Grapple;

namespace PrestigePlus.Feats
{
    internal class ImprovedGrappleMythic
    {
        private static readonly string FeatName = "ImprovedGrappleMythic";
        private static readonly string FeatGuid = "{A72E9323-21A1-4702-AD50-0C212630FAB1}";

        private static readonly string DisplayName = "ImprovedGrappleMythic.Name";
        private static readonly string Description = "ImprovedGrappleMythic.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Rank())
                    .AddCMDBonusAgainstManeuvers(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Rank())
                    .AddContextRankConfig(ContextRankConfigs.MythicLevel().WithDiv2Progression())
                    .Configure();
        }
    }
}
