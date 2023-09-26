using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
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
                    .Configure();
        }
    }
}
