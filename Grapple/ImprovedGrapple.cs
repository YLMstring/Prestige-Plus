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
using Kingmaker.UnitLogic.Mechanics.Components;
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

        private const string Stylebuff = "ImprovedGrapple.Stylebuff";  //normal grapple
        private static readonly string StylebuffGuid = "{D6D08842-8E03-4A9D-81B8-1D9FB2245649}";

        private const string Stylebuff2 = "ImprovedGrapple.Stylebuff2";  //target
        private static readonly string StylebuffGuid2 = "{F505D659-0610-41B1-B178-E767CCB9292E}";

        private const string Stylebuff3 = "ImprovedGrapple.Stylebuff3";  //free not grappled
        private static readonly string StylebuffGuid3 = "{D4DD258E-B9F1-42D1-9BD0-ADBD217AFE23}";

        private const string Stylebuff4 = "ImprovedGrapple.Stylebuff4";  //charge grapple
        private static readonly string StylebuffGuid4 = "{C5F4DDFE-CA2E-4309-90BB-1BB5C0F32E78}";

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

        private const string ReadyAbility = "ImprovedGrapple.ReadyAbility";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";

        private static readonly string ReadyDisplayName = "GrappledReady.Name";
        private static readonly string ReadyDescription = "GrappledReady.Description";

        private const string Readybuff = "ImprovedGrapple.Readybuff";
        private static readonly string ReadybuffGuid = "{AD21943C-2AC2-465B-8A1E-F99F3446EBE4}";

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
              .SetFxOnStart("063ff6e114b9ff94c9b32ea0e5567c6a")
              .AddComponent<PPGrabTargetBuff>()
              .Configure();

            var Buff3 = BuffConfigurator.New(Stylebuff3, StylebuffGuid3)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<PPGrabInitiatorFree>()
              .Configure();

            var Buff4 = BuffConfigurator.New(Stylebuff4, StylebuffGuid4)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<PPGrabInitiatorBuff>()
              .Configure();

            var grab = ActionsBuilder.New()
                .Add<PPActionGrapple>(c => { c.isAway = false; })
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
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(2)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
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
                .SetIcon(FeatureRefs.Toughness.Reference.Get().Icon)
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
                .SetIcon(FeatureRefs.ImprovedDisarm.Reference.Get().Icon)
                .SetBuff(BuffTieUp)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffReady = BuffConfigurator.New(Readybuff, ReadybuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityReady = ActivatableAbilityConfigurator.New(ReadyAbility, ReadyAbilityGuid)
                .SetDisplayName(ReadyDisplayName)
                .SetDescription(ReadyDescription)
                .SetIcon(FeatureRefs.CraneStyleWingFeat.Reference.Get().Icon)
                .SetBuff(BuffReady)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var abilityRelease = AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<PPActionGrappleRelease>()
                    .Build())
                .SetDisplayName(ReleaseDisplayName)
                .SetDescription(ReleaseDescription)
                .SetIcon(FeatureRefs.CraneStyleRiposteFeat.Reference.Get().Icon)
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
                    .AddFacts(new() { ability, abilityRelease, abilityReady, abilityPin, abilityTieUp })
                    .AddComponent<ConditionTwoFreeHand>()
                    .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddCMDBonusAgainstManeuvers(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Constant(2))
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel6.ToString())
                    .AddToFeatureSelection(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
                    .Configure();

            FeatureConfigurator.For(FeatureRefs.AgileManeuvers)
                .AddPrerequisiteFeature(StyleGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            var grab2 = ActionsBuilder.New()
                .ApplyBuffPermanent(BuffRefs.GrapplingInfusionEffectBuff.ToString(), true)
                .Build();

            var grapple2 = ActionsBuilder.New()
                .CombatManeuver(onSuccess: grab2, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Build();

            //BuffConfigurator.For(BuffRefs.GrapplingInfusionBuff)
                //.EditComponent<AddKineticistInfusionDamageTrigger>(a => a.Actions.Actions = grapple2.Actions)
                //.Configure();

        }
    }
}
