using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class SurpriseManeuver
    {
        private static readonly string FeatName = "RogueSurpriseManeuver";
        private static readonly string FeatGuid = "{7B038977-1E15-4E4C-B10B-13B7F5DB3BC7}";

        private static readonly string DisplayName = "RogueSurpriseManeuver.Name";
        private static readonly string Description = "RogueSurpriseManeuver.Description";
        public static void Configure()
        {
            var icon = FeatureRefs.JumpUp.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                .AddPrerequisiteFullStatValue(stat: Kingmaker.EntitySystem.Stats.StatType.SneakAttack, value: 3)
                .AddComponent<SurprisedManeuverComponent>()
                .Configure();
        }
    }
}
