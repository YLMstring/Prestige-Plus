using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Owlcat.Runtime.UI.ConsoleTools.GamepadInput;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.CustomComponent.Charge;
using PrestigePlus.Blueprint.MythicGrapple;
using Kingmaker.Enums;

namespace PrestigePlus.Feats
{
    internal class StagStyle
    {
        private static readonly string StyleName = "StagStyle";
        public static readonly string StyleGuid = "{A6A49BAB-F3DB-498D-8563-55028FDDEB14}";

        private static readonly string StyleDisplayName = "StagStyle.Name";
        private static readonly string StyleDescription = "StagStyle.Description";

        private const string Stylebuff = "StagStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{21F094D4-1D59-400B-9CEB-558E6218FB0C}";

        private const string StyleActivatableAbility = "StagStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{68FAB881-4AFD-4389-B9E9-B2E735303E04}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.WildShapeIWolfFeature.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<StagChargeAttackLimit>()
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.CombatStyle)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                    .AddFacts(new() { ability, AerialAssault.ReleaseAbilityGuid })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }

        private static readonly string HornsName = "StagHorns";
        public static readonly string HornsGuid = "{77627CEF-E655-4DB5-A0EF-5A7F48081639}";

        private static readonly string HornsDisplayName = "StagHorns.Name";
        private static readonly string HornsDescription = "StagHorns.Description";

        public static void HornsConfigure()
        {
            var icon = FeatureRefs.WildShapeIWolfFeature.Reference.Get().Icon;

            var grab = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(StylebuffGuid).Build(),ifTrue: ActionsBuilder.New()
                    .Add<PPActionGrapple>(c => { c.isAway = false; })
                    .Build())
                .Build();

            var grapple = ActionsBuilder.New()
                .CombatManeuver(onSuccess: grab, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Build();

            FeatureConfigurator.New(HornsName, HornsGuid, FeatureGroup.Feat)
                    .SetDisplayName(HornsDisplayName)
                    .SetDescription(HornsDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddInitiatorAttackWithWeaponTrigger(grapple, category: WeaponCategory.UnarmedStrike, checkWeaponCategory: true, onCharge: true, onlyHit: true)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddFacts(new() { PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
                    .Configure();
        }

        private static readonly string SubmissionName = "StagSubmission";
        public static readonly string SubmissionGuid = "{87D3F5CC-45B0-4AF2-ACC6-4880B0391BDD}";

        private static readonly string SubmissionDisplayName = "StagSubmission.Name";
        private static readonly string SubmissionDescription = "StagSubmission.Description";

        public static void SubmissionConfigure()
        {
            var icon = FeatureRefs.WildShapeIWolfFeature.Reference.Get().Icon;

            var BuffPin = BuffConfigurator.New(Pinbuff, PinbuffGuid)
              .SetDisplayName(PinDisplayName)
              .SetDescription(PinDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityPin = ActivatableAbilityConfigurator.New(PinAbility, PinAbilityGuid)
                .SetDisplayName(PinDisplayName)
                .SetDescription(PinDescription)
                .SetIcon(FeatureRefs.TripMythicFeat.Reference.Get().Icon)
                .SetBuff(BuffPin)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(SubmissionName, SubmissionGuid, FeatureGroup.Feat)
                    .SetDisplayName(SubmissionDisplayName)
                    .SetDescription(SubmissionDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(HornsGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .AddFacts(new() { abilityPin })
                    .Configure();

        }

        private const string PinAbility = "StagSubmission.PinAbility";
        private static readonly string PinAbilityGuid = "{C9DF1149-D0FE-45AA-BB0D-E30E409A509E}";

        private static readonly string PinDisplayName = "StagSubmissionPin.Name";
        private static readonly string PinDescription = "StagSubmissionPin.Description";

        private const string Pinbuff = "StagSubmission.Pinbuff";
        private static readonly string PinbuffGuid = "{166DF6CC-25B6-4864-9F1D-C9EFF2AA6869}";

        private static readonly string PinAbilityGuid1 = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";

    }
}
