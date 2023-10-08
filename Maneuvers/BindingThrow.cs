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
        public static readonly string DragGuid = "{D4583C87-10C9-468D-8413-B58638FE31EF}";

        private static readonly string DragDisplayName = "BindingThrow.Name";
        private static readonly string DragDescription = "BindingThrow.Description";

        private const string StyleAbility = "BindingThrow.StyleAbility";
        private static readonly string StyleAbilityGuid = "{50C570BA-7454-40D6-BD8A-BE3F653CAD0B}";

        private const string StyleBuff = "BindingThrow.StyleBuff";
        private static readonly string StyleBuffGuid = "{98558112-717B-456D-838F-EFF5061F4D38}";
        public static void DragConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var buff = BuffConfigurator.New(StyleBuff, StyleBuffGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .Configure();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)))
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
