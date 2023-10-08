using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class BindingThrow
    {
        private static readonly string DragName = "BindingThrow";
        public static readonly string DragGuid = "{B39490B1-5BF5-40E4-B2CB-3CBEA44FBC1C}";

        private static readonly string DragDisplayName = "BindingThrow.Name";
        private static readonly string DragDescription = "BindingThrow.Description";

        private const string StyleAbility = "BindingThrow.StyleAbility";
        private static readonly string StyleAbilityGuid = "{52C990D0-BCB3-4AAD-8007-AB398043E26F}";

        private const string StyleBuff = "BindingThrow.StyleBuff";
        private static readonly string StyleBuffGuid = "{F57450C7-C684-489B-ACCE-31FD37E2324C}";
        public static void DragConfigure()
        {
            var icon = AbilityRefs.ArmyShifterGrabAbility.Reference.Get().Icon;

            var buff = BuffConfigurator.New(StyleBuff, StyleBuffGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .Configure();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid, FeatureGroup.Feat)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedTrip.ToString())
                    .AddPrerequisiteFeature(KiThrow.DragGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
