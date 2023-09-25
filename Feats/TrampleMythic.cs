using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class TrampleMythic
    {
        private static readonly string FeatName = "TrampleMythic";
        private static readonly string FeatGuid = "{703E342C-23B4-42E9-8A9D-27C51AFE4E12}";

        private static readonly string DisplayName = "TrampleMythic.Name";
        private static readonly string Description = "TrampleMythic.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.Trample.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    //.AddPrerequisiteFeature(FeatureRefs.Trample.ToString())
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.TrampleAbility.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.OverrunAbility.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }
    }
}
