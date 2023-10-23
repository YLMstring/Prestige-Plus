using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.GrappleFeat
{
    internal class DramaticSlam
    {
        private static readonly string FeatName = "DramaticSlam";
        public static readonly string FeatGuid = "{201C54A4-F2D2-43BA-A4AA-7A9F53745896}";

        private static readonly string DisplayName = "DramaticSlam.Name";
        private static readonly string Description = "DramaticSlam.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.ImprovedSunder.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 9, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 6, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.DazzlingDisplayFeature.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(SavageSlam.FeatGuid)
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
