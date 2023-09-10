using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Owlcat.Runtime.UI.ConsoleTools.GamepadInput;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    internal class ImprovedGrapple
    {
        private static readonly string StyleName = "ImprovedGrapple";
        public static readonly string StyleGuid = "{D74F645A-D0F2-470B-B68B-E76EC083A6D8}";

        private static readonly string StyleDisplayName = "ImprovedGrapple.Name";
        private static readonly string StyleDescription = "ImprovedGrapple.Description";

        private static readonly string DisplayName = "Grappled.Name";
        private static readonly string Description = "Grappled.Description";

        private const string Stylebuff = "ImprovedGrapple.Stylebuff";
        private static readonly string StylebuffGuid = "{D6D08842-8E03-4A9D-81B8-1D9FB2245649}";

        private const string Stylebuff2 = "ImprovedGrapple.Stylebuff2";
        private static readonly string StylebuffGuid2 = "{F505D659-0610-41B1-B178-E767CCB9292E}";

        private const string StyleAbility = "ImprovedGrapple.StyleAbility";
        private static readonly string StyleAbilityGuid = "{3011EAD3-71BD-4F62-B535-877285808A0E}";

        private const string PinAbility = "ImprovedGrapple.PinAbility";
        private static readonly string PinAbilityGuid = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";

        private static readonly string PinDisplayName = "GrappledPin.Name";
        private static readonly string PinDescription = "GrappledPin.Description";

        private const string Pinbuff = "ImprovedGrapple.Pinbuff";
        private static readonly string PinbuffGuid = "{D3B37428-69BE-43F2-83DA-04A38D35CDCB}";

        private const string TieUpAbility = "ImprovedGrapple.TieUpAbility";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";

        private static readonly string TieUpDisplayName = "GrappledTieUp.Name";
        private static readonly string TieUpDescription = "GrappledTieUp.Description";

        private const string TieUpbuff = "ImprovedGrapple.TieUpbuff";
        private static readonly string TieUpbuffGuid = "{B14E98A0-53AC-4212-86A0-29D1CA1D8446}";

        private const string ReleaseAbility = "ImprovedGrapple.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";

        private static readonly string ReleaseDisplayName = "GrappledRelease.Name";
        private static readonly string ReleaseDescription = "GrappledRelease.Description";

        public static void StyleConfigure()
        {
            var icon = FeatureRefs.ImprovedBullRush.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<PPGrabInitiatorBuff>()
              .Configure();

            var Buff2 = BuffConfigurator.New(Stylebuff2, StylebuffGuid2)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<PPGrabTargetBuff>()
              .Configure();

            var grab = ActionsBuilder.New()
                .Add<PPActionGrapple>(c => { c.CasterBuff = Buff; c.TargetBuff = Buff2; })
                .Build();

            var grapple = ActionsBuilder.New()
                .CombatManeuver(onSuccess: grab, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Build();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(grapple)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetRange(AbilityRange.Touch)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
                .AddAbilityCasterHasNoFacts(new() { StylebuffGuid })
                .AddAbilityTargetHasFact(new() { StylebuffGuid2 }, inverted: true)
                .Configure();

            var BuffPin = BuffConfigurator.New(Pinbuff, PinbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityPin = ActivatableAbilityConfigurator.New(PinAbility, PinAbilityGuid)
                .SetDisplayName(PinDisplayName)
                .SetDescription(PinDescription)
                .SetIcon(icon)
                .SetBuff(BuffPin)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffTieUp = BuffConfigurator.New(TieUpbuff, TieUpbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTieUp = ActivatableAbilityConfigurator.New(TieUpAbility, TieUpAbilityGuid)
                .SetDisplayName(TieUpDisplayName)
                .SetDescription(TieUpDescription)
                .SetIcon(icon)
                .SetBuff(BuffTieUp)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var abilityRelease = AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<PPActionGrappleRelease>()
                    .Build())
                .SetDisplayName(ReleaseDisplayName)
                .SetDescription(ReleaseDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddFacts(new() { ability, abilityRelease, abilityPin, abilityTieUp })
                    .AddComponent<ConditionTwoFreeHand>()
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddCMDBonusAgainstManeuvers(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();

        }
    }
}
