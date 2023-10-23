using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder;

namespace PrestigePlus.RogueTalent
{
    internal class QuickShot
    {
        private static readonly string FeatName = "QuickShot";
        private static readonly string FeatGuid = "{55A4FBA2-D6F4-4528-8635-EA10D4F851AC}";

        private static readonly string DisplayName = "QuickShot.Name";
        private static readonly string Description = "QuickShot.Description";
        public static void Configure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                .Build();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.RogueTalent)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddPrerequisiteFeature(FeatureRefs.AdvanceTalents.ToString())
                .Configure();
        }
    }
}
