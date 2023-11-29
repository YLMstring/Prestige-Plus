using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.CombatStyle
{
    internal class CloakDaggerStyle
    {
        private static readonly string StyleName = "CloakDaggerStyle";
        public static readonly string StyleGuid = "{0E788797-B9D4-44B5-A8B2-C1DEC6332E8C}";

        private static readonly string StyleDisplayName = "CloakDaggerStyle.Name";
        private static readonly string StyleDescription = "CloakDaggerStyle.Description";

        private const string Stylebuff = "CloakDaggerStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{DC8638AA-4467-4DEB-9276-85836624A623}";

        private const string StyleActivatableAbility = "CloakDaggerStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{A594E81C-8762-4511-AEA7-3964EEA858DB}";

        private const string Stylebuff2 = "CloakDaggerStyle.Stylebuff2";
        private static readonly string StylebuffGuid2 = "{F25539BA-026F-440A-B16C-7E3C3859D4B2}";

        public const string CloakDaggerStyleBlindAbility = "CloakDaggerStyleBlind.CloakDaggerStyleBlindAbility";
        public static readonly string CloakDaggerStyleBlindAbilityGuid = "{3C26A2F8-24C4-4F35-8B97-B9A3E311D26B}";

        public const string CloakDaggerStyleBlindbuff = "CloakDaggerStyleBlind.CloakDaggerStyleBlindbuff";
        public static readonly string CloakDaggerStyleBlindbuffGuid = "{EEFE4829-79DD-46A7-8428-395DA2326537}";

        public const string CloakDaggerStyleEntangleAbility = "CloakDaggerStyleEntangle.CloakDaggerStyleEntangleAbility";
        public static readonly string CloakDaggerStyleEntangleAbilityGuid = "{24366C7C-DE88-4ED2-8F74-F9FDD0320DAC}";

        public const string CloakDaggerStyleEntanglebuff = "CloakDaggerStyleEntangle.CloakDaggerStyleEntanglebuff";
        public static readonly string CloakDaggerStyleEntanglebuffGuid = "{A362F018-7274-4525-8264-E0FACD789D81}";

        public const string CloakDaggerStyleSickenAbility = "CloakDaggerStyleSicken.CloakDaggerStyleSickenAbility";
        public static readonly string CloakDaggerStyleSickenAbilityGuid = "{24E85A8A-45C2-4FCB-AE5D-3CE94619F3D6}";

        public const string CloakDaggerStyleSickenbuff = "CloakDaggerStyleSicken.CloakDaggerStyleSickenbuff";
        public static readonly string CloakDaggerStyleSickenbuffGuid = "{BDCE6887-BAB6-48BD-8A6B-354CEE27EED8}";

        public static void StyleConfigure()
        {
            var icon = FeatureRefs.SneakAttack.Reference.Get().Icon;

            var BuffCloakDaggerStyleBlind = BuffConfigurator.New(CloakDaggerStyleBlindbuff, CloakDaggerStyleBlindbuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddBuffActions(activated: ActionsBuilder.New()
                .RemoveBuff(CloakDaggerStyleEntanglebuffGuid)
                .RemoveBuff(CloakDaggerStyleSickenbuffGuid)
                .Build())
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyBlind = ActivatableAbilityConfigurator.New(CloakDaggerStyleBlindAbility, CloakDaggerStyleBlindAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(AbilityRefs.DirtyTrickBlindnessAction.Reference.Get().Icon)
                .SetBuff(BuffCloakDaggerStyleBlind)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffCloakDaggerStyleEntangle = BuffConfigurator.New(CloakDaggerStyleEntanglebuff, CloakDaggerStyleEntanglebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddBuffActions(activated: ActionsBuilder.New()
                .RemoveBuff(CloakDaggerStyleBlindbuffGuid)
                .RemoveBuff(CloakDaggerStyleSickenbuffGuid)
                .Build())
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyEntangle = ActivatableAbilityConfigurator.New(CloakDaggerStyleEntangleAbility, CloakDaggerStyleEntangleAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(AbilityRefs.DirtyTrickEntangleAction.Reference.Get().Icon)
                .SetBuff(BuffCloakDaggerStyleEntangle)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var BuffCloakDaggerStyleSicken = BuffConfigurator.New(CloakDaggerStyleSickenbuff, CloakDaggerStyleSickenbuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddBuffActions(activated: ActionsBuilder.New()
                .RemoveBuff(CloakDaggerStyleBlindbuffGuid)
                .RemoveBuff(CloakDaggerStyleEntanglebuffGuid)
                .Build())
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtySicken = ActivatableAbilityConfigurator.New(CloakDaggerStyleSickenAbility, CloakDaggerStyleSickenAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(AbilityRefs.DirtyTrickSickenedAction.Reference.Get().Icon)
                .SetBuff(BuffCloakDaggerStyleSicken)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false).SetActionBarAutoFillIgnored(true)
                .Configure();

            var Buff2 = BuffConfigurator.New(Stylebuff2, StylebuffGuid2)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<CloakDaggerManeuver>()
              //.AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, triggerBeforeAttack: false)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                .Build();

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddCombatStateTrigger(combatStartActions: action)
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
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
                    .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString())
                    .AddFacts(new() { ability, abilityDirtyBlind, abilityDirtyEntangle, abilityDirtySicken })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string SubterfugeName = "CloakDaggerSubterfuge";
        public static readonly string SubterfugeGuid = "{45245FB8-4806-4A24-BD33-55F12C6BE69C}";

        private static readonly string SubterfugeDisplayName = "CloakDaggerSubterfuge.Name";
        private static readonly string SubterfugeDescription = "CloakDaggerSubterfuge.Description";

        public static void SubterfugeConfigure()
        {
            var icon = FeatureRefs.SneakAttack.Reference.Get().Icon;

            var disarm = ActionsBuilder.New()
                        .CombatManeuver(ActionsBuilder.New().Build(), CombatManeuver.Disarm)
                        .Build();

            FeatureConfigurator.New(SubterfugeName, SubterfugeGuid, FeatureGroup.Feat)
                    .SetDisplayName(SubterfugeDisplayName)
                    .SetDescription(SubterfugeDescription)
                    .SetIcon(icon)
                    .AddComponent<CloakDaggerSub>()
                    .AddManeuverTrigger(disarm, CombatManeuver.DirtyTrickSickened, true)
                    .AddManeuverTrigger(disarm, CombatManeuver.DirtyTrickBlind, true)
                    .AddManeuverTrigger(disarm, CombatManeuver.DirtyTrickEntangle, true)
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 11)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
                    .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string TacticsName = "CloakDaggerTactics";
        public static readonly string TacticsGuid = "{57D326A1-A4B5-40AB-A083-28862B2AF02C}";

        private static readonly string TacticsDisplayName = "CloakDaggerTactics.Name";
        private static readonly string TacticsDescription = "CloakDaggerTactics.Description";

        private const string Stylebuff3 = "CloakDaggerStyle.Stylebuff3";
        public static readonly string Stylebuff3Guid = "{0D44DE20-A2F9-4F4A-B17C-3C32FE5D8AD4}";

        public static void TacticsConfigure()
        {
            var icon = FeatureRefs.SneakAttack.Reference.Get().Icon;

            var buff = BuffConfigurator.New(Stylebuff3, Stylebuff3Guid)
              .SetDisplayName(TacticsDisplayName)
              .SetDescription(TacticsDescription)
              .SetIcon(icon)
              .AddComponent<CloakDaggerTac>()
              //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            FeatureConfigurator.New(TacticsName, TacticsGuid, FeatureGroup.Feat)
                    .SetDisplayName(TacticsDisplayName)
                    .SetDescription(TacticsDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 16)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(SubterfugeGuid)
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
                    .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString())
                    .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)))
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }
    }
}
