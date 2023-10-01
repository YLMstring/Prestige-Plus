using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class SwiftDeath
    {
        private static readonly string FeatName = "SwiftDeath1";
        private static readonly string FeatGuid = "{2870F027-44FD-42C0-83F2-1103615550DE}";

        private static readonly string Feat0Name = "SwiftDeath0";
        private static readonly string Feat0Guid = "{8B8AE2D5-EC40-4B73-9829-DA8194B5CA68}";

        private static readonly string DisplayName = "SwiftDeath1.Name";
        private static readonly string Description = "SwiftDeath1.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.AssassinDeathAttackAbilityStandard.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(Feat0Name, Feat0Guid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityKillStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityParalyzeStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddFeatureOnClassLevel(beforeThisLevel: false, clazz: CharacterClassRefs.AssassinClass.ToString(), level: 2, feature: feat)
                    .AddFacts(new() { FeatureRefs.AssassinDeathAttackStandard.ToString()})
                    .AddPrerequisiteClassLevel(CharacterClassRefs.AssassinClass.ToString(), 1)
                    .Configure();
        }

        private static readonly string Feat2Name = "SwiftDeath2";
        private static readonly string Feat2Guid = "{30C395EA-1E96-4F90-9D2C-2E94230B2327}";

        private static readonly string Feat3Name = "SwiftDeath3";
        private static readonly string Feat3Guid = "{3EFB56F5-41E3-4328-9AB2-B4FD202059CA}";

        private static readonly string Feat4Name = "SwiftDeath4";
        private static readonly string Feat4Guid = "{08496AC0-4230-4665-8A84-DB0BE8CA7C19}";

        private static readonly string DisplayName2 = "SwiftDeath2.Name";
        private static readonly string Description2 = "SwiftDeath2.Description";

        public static void Configure2()
        {
            var icon = AbilityRefs.AssassinDeathAttackAbilityStandard.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(Feat3Name, Feat3Guid)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityKillStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityParalyzeStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift; })
                    .AddRemoveFeatureOnApply(Feat0Guid)
                    .Configure();

            var feat4 = FeatureConfigurator.New(Feat4Name, Feat4Guid)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityKillStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.AssassinDeathAttackAbilityParalyzeStandard.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .Configure();

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatGuid)
                    .AddFeatureOnClassLevel(beforeThisLevel: true, clazz: CharacterClassRefs.AssassinClass.ToString(), level: 2, feature: feat4)
                    .AddFeatureOnClassLevel(beforeThisLevel: false, clazz: CharacterClassRefs.AssassinClass.ToString(), level: 2, feature: feat)
                    .Configure();
        }
    }
}
