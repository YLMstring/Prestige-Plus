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
    internal class GreaterGrappleMythic
    {
        private static readonly string FeatName = "GreaterGrappleMythic";
        private static readonly string FeatGuid = "{27B59104-C22F-4E35-8743-BF08A3B2B870}";

        private static readonly string DisplayName = "GreaterGrappleMythic.Name";
        private static readonly string Description = "GreaterGrappleMythic.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(GreaterGrapple.FeatGuid)
                    .Configure();
        }
    }
}
