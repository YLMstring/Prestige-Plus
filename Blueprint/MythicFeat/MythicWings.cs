using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class MythicWings
    {
        private static readonly string FeatName = "MythicWings";
        private static readonly string FeatGuid = "{E107DC01-E07A-4902-B23B-248BBE5E7F10}";

        private static readonly string DisplayName = "MythicWings.Name";
        private static readonly string Description = "MythicWings.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.Fear.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddAdditionalLimb(ItemWeaponRefs.Wing1d4.ToString())
                    .AddAdditionalLimb(ItemWeaponRefs.Wing1d4.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.DragonDiscipleBlindSense.ToString())
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();
        }

        private static readonly string Feat2Name = "MythicBreath";
        private static readonly string Feat2Guid = "{AA718E4F-D80C-47BF-A249-F2ACEA096D5A}";

        private static readonly string DisplayName2 = "MythicBreath.Name";
        private static readonly string Description2 = "MythicBreath.Description";

        public static void Configure2()
        {
            var icon = AbilityRefs.AzaraDragonBreathWeapon.Reference.Get().Icon;

            var action = ActionsBuilder.New().RestoreResource(AbilityResourceRefs.BloodlineDraconicBreathWeaponResource.ToString(), ContextValues.Constant(1)).Build();
            var action2 = ActionsBuilder.New().Build();

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.GoldenDragonBreathAbility.ToString()); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
                    .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().Randomize((action, 1), (action2, 2)).Build())
                    .AddCombatStateTrigger(combatEndActions: ActionsBuilder.New().RestoreResource(AbilityResourceRefs.BloodlineDraconicBreathWeaponResource.ToString(), ContextValues.Constant(100)).Build())
                    .AddPrerequisiteFeature(FeatureRefs.DragonDiscipleIntelligence.ToString())
                    .Configure();
        }
    }
}
