using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class GuidedHand
    {
        private const string GuidedHandFeat = "Feat.GuidedHand";
        private static readonly string GuidedHandGuid = "{56296CA5-A642-4930-B595-22AA3F039A64}";

        internal const string GuidedHandDisplayName = "FeatGuidedHand.Name";
        private const string GuidedHandDescription = "FeatGuidedHand.Description";
        public static void GuidedHandConfigure()
        {
            var icon = AbilityRefs.LayOnHandsSelf.Reference.Get().Icon;
            //ChannelSmiteFeature	8623a8c41c2740fcb04861a3d7ce083a
            FeatureConfigurator.New(GuidedHandFeat, GuidedHandGuid, FeatureGroup.Feat)
              .SetDisplayName(GuidedHandDisplayName)
              .SetDescription(GuidedHandDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature("8623a8c41c2740fcb04861a3d7ce083a", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature("dff4ee6c9a66c8146b30a8af6f7feebf", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.SelectiveChannel.ToString(), group: Prerequisite.GroupType.Any)
              .AddComponent<GuidedHandWis>()
              .Configure();

        }

        private const string GuidedHandMythic = "Feat.GuidedHandMythic";
        private static readonly string GuidedHandMythicGuid = "{9F1E37F4-9238-454E-B265-C7E0AFA07823}";

        internal const string GuidedHandMythicDisplayName = "FeatGuidedHandMythic.Name";
        private const string GuidedHandMythicDescription = "FeatGuidedHandMythic.Description";
        public static void GuidedHandMythicFeat()
        {
            var icon = AbilityRefs.BurningHandsElecricity.Reference.Get().Icon;

            FeatureConfigurator.New(GuidedHandMythic, GuidedHandMythicGuid, FeatureGroup.MythicFeat)
              .SetDisplayName(GuidedHandMythicDisplayName)
              .SetDescription(GuidedHandMythicDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(GuidedHandGuid)
              .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
              .AddComponent<GuidedHandDamage>()
              .Configure();

        }
    }
}
