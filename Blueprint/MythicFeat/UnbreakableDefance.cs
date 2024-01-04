using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Blueprints;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class UnbreakableDefance
    {
        private static readonly string FeatName = "UnbreakableDefance";
        private static readonly string FeatGuid = "{8F200B67-D7DC-4934-961F-014214584A77}";

        private static readonly string DisplayName = "UnbreakableDefance.Name";
        private static readonly string Description = "UnbreakableDefance.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.DefensiveStanceFeature.Reference.Get().Icon;

            ActivatableAbilityConfigurator.For(ActivatableAbilityRefs.DefensiveStanceActivateableAbility)
            .EditComponent<ActivatableAbilityResourceLogic>(
                a => a.m_FreeBlueprint = BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatGuid))
              .Configure();

            ActivatableAbilityConfigurator.For(ActivatableAbilityRefs.DefensiveStanceFatigueActivateableAbility)
            .EditComponent<ActivatableAbilityResourceLogic>(
                a => a.m_FreeBlueprint = BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatGuid))
              .Configure();

            ActivatableAbilityConfigurator.For(ActivatableAbilityRefs.StonelordDefensiveStanceActivateableAbility)
            .EditComponent<ActivatableAbilityResourceLogic>(
                a => a.m_FreeBlueprint = BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatGuid))
              .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.DefensiveStanceFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteFeature(FeatureRefs.StonelordDefensiveStanceFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .Configure();
        }
    }
}
