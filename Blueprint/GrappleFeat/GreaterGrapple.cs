using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomAction.GrappleThrow;

namespace PrestigePlus.Blueprint.GrappleFeat
{
    internal class GreaterGrapple
    {
        private static readonly string FeatName = "GreaterGrapple";
        public static readonly string FeatGuid = "{CB3B7666-0AD1-4ADD-8157-BAC7E2A15D5A}";

        private static readonly string DisplayName = "GreaterGrapple.Name";
        private static readonly string Description = "GreaterGrapple.Description";

        private const string TrickAbility = "ImprovedGrapple.TrickAbility";
        private static readonly string TrickAbilityGuid = "{10A04AAC-5F03-42C2-9468-BB8872B4EF4B}";

        public static void Configure()
        {
            var icon = FeatureRefs.GreaterBullRush.Reference.Get().Icon;

            var abilityTrick = AbilityConfigurator.New(TrickAbility, TrickAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<PPActionGrappleTrick>()
                    .Build())
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { abilityTrick })
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
