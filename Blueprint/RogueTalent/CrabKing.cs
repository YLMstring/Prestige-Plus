using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.RogueTalent
{
    internal class CrabKing
    {
        private static readonly string FeatName = "CrabKing";
        private static readonly string FeatGuid = "{64A38F59-3739-469A-9BE7-86F6E0AD9A78}";

        private static readonly string DisplayName = "CrabKing.Name";
        private static readonly string Description = "CrabKing.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.DruidNatureBond.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Familiar)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddToFeatureSelection(FeatureSelectionRefs.Familiar.ToString())
                    .AddToFeatureSelection(FeatureSelectionRefs.ElementalWhispersSelection.ToString())
                    .Configure();
        }

        private static readonly string Feat2Name = "Tentacle";
        private static readonly string Feat2Guid = "{2C36C615-58FF-4C1A-B9E9-F0261B6006A1}";

        private static readonly string DisplayName2 = "TentacleGrab.Name";
        private static readonly string Description2 = "TentacleGrab.Description";

        public static void Configure2()
        {
            var icon = FeatureRefs.PoisonImmunity.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.Discovery)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(4))
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.VivisectionistDiscovery)
                    .Configure();
        }
    }
}
