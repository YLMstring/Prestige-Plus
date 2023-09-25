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
    internal class ManeuverExpert
    {
        private static readonly string FeatName = "ManeuverExpert";
        private static readonly string FeatGuid = "{27B59104-C22F-4E35-8743-BF08A3B2B870}";

        private static readonly string DisplayName = "ManeuverExpert.Name";
        private static readonly string Description = "ManeuverExpert.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.LastStand.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddBuffAllSavesBonus(Kingmaker.Enums.ModifierDescriptor.Penalty, -1)
                    .AddFacts( new() { FeatureRefs.ImprovedBullRush.ToString(), FeatureRefs.ImprovedDirtyTrick.ToString(), FeatureRefs.ImprovedDisarm.ToString(), FeatureRefs.ImprovedSunder.ToString(), FeatureRefs.ImprovedTrip.ToString(),
                    FeatureRefs.GreaterBullRush.ToString(), FeatureRefs.GreaterDirtyTrick.ToString(), FeatureRefs.GreaterDisarm.ToString(), FeatureRefs.GreaterSunder.ToString(), FeatureRefs.GreaterTrip.ToString(), ImprovedGrapple.StyleGuid, GreaterGrapple.FeatGuid,
                    FeatureRefs.ImprovedOverrun.ToString(), AbilityRefs.OverrunAbility.ToString()})
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }
    }
}
