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
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Feats
{
    internal class UnfairGrip
    {
        private static readonly string FeatName = "UnfairGrip";
        private static readonly string FeatGuid = "{D6FB8873-0F92-4BBD-A162-BD72C3852028}";

        private static readonly string DisplayName = "UnfairGrip.Name";
        private static readonly string Description = "UnfairGrip.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.CriticalFocus.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .AddCMDBonusAgainstManeuvers(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(1))
                    .Configure();
        }
    }
}
