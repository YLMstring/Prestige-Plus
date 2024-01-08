using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.OtherManeuver;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class ManeuverMaster
    {
        private const string ArchetypeName = "ManeuverMaster";
        private static readonly string ArchetypeGuid = "{7DEB963D-5449-4949-A90A-FA9FE1D25554}";
        internal const string ArchetypeDisplayName = "ManeuverMaster.Name";
        private const string ArchetypeDescription = "ManeuverMaster.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MonkClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureRefs.MonkFlurryOfBlowstUnlock.ToString(), FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
            .AddToRemoveFeatures(2, FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
            .AddToRemoveFeatures(4, FeatureRefs.StillMind.ToString())
            .AddToRemoveFeatures(5, FeatureRefs.PurityOfBody.ToString())
            .AddToRemoveFeatures(6, FeatureSelectionRefs.MonkBonusFeatSelectionLevel6.ToString())
            .AddToRemoveFeatures(10, FeatureSelectionRefs.MonkKiPowerSelection.ToString(), FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToRemoveFeatures(11, FeatureRefs.MonkFlurryOfBlowstLevel11Unlock.ToString())
            .AddToRemoveFeatures(14, FeatureSelectionRefs.MonkKiPowerSelection.ToString(), FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToRemoveFeatures(18, FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToAddFeatures(1, CreateFlurry(), CreateFlurry1())
            .AddToAddFeatures(2, Flurry1Guid)
            .AddToAddFeatures(4, ReliableFeat())
            .AddToAddFeatures(5, MeditativeFeat())
            .AddToAddFeatures(6, CreateFlurry2())
            .AddToAddFeatures(8, CreateFlurry8())
            .AddToAddFeatures(10, CreateSweeping(), CreateFlurry3())
            .AddToAddFeatures(14, CreateWhirlwind(), Flurry3Guid)
            .AddToAddFeatures(15, CreateFlurry15())
            .AddToAddFeatures(18, Flurry3Guid)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MonkProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { FlurryGuid, Flurry8Guid, Flurry15Guid })
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { Flurry1Guid, Flurry2Guid, Flurry3Guid })
                .Configure();
        }

        private const string Flurry = "ManeuverMaster.Flurry";
        public static readonly string FlurryGuid = "{08B7F7DD-D332-4B51-8C1A-D52EBF065CFB}";

        private const string FlurryFeat = "ManeuverMaster.FlurryFeat";
        public static readonly string FlurryFeatGuid = "{C4D92D6C-368A-49B3-93F2-5A7D12850B93}";

        internal const string FlurryDisplayName = "ManeuverMasterFlurry.Name";
        private const string FlurryDescription = "ManeuverMasterFlurry.Description";

        private const string Flurrybuff = "ManeuverFlurry.Flurrybuff";
        public static readonly string FlurrybuffGuid = "{89CCCD57-CC47-4443-A192-1FF7D18F2A3C}";

        private const string FlurryCoolDownbuff = "ManeuverFlurry.FlurryCoolDownbuff";
        public static readonly string FlurryCoolDownbuffGuid = "{1355973E-3E06-459C-8A41-E83A0A889371}";

        private const string FlurryActivatableAbility = "ManeuverFlurry.FlurryActivatableAbility";
        private static readonly string FlurryActivatableAbilityGuid = "{260D7372-AB40-42A7-8611-56BE31CEC209}";
        private static BlueprintFeature CreateFlurry()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(FlurryFeat, FlurryFeatGuid)
                .SetDisplayName(FlurryDisplayName)
                .SetDescription(FlurryDescription)
                .SetIcon(icon)
                .AddBuffExtraAttack(false, number: 1)
                .AddCMBBonus(value: ContextValues.Rank())
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.MonkClass.ToString() }).WithStartPlusDivStepProgression(4, 1, true))
                .Configure();

            BuffConfigurator.New(FlurryCoolDownbuff, FlurryCoolDownbuffGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buff = BuffConfigurator.New(Flurrybuff, FlurrybuffGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddMonkNoArmorFeatureUnlock(feat)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -2)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(FlurryActivatableAbility, FlurryActivatableAbilityGuid)
                .SetDisplayName(FlurryDisplayName)
                .SetDescription(FlurryDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            return FeatureConfigurator.New(Flurry, FlurryGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability, SeizetheOpportunity.ManeuverGuid })
              .Configure();
        }

        private const string Flurry8 = "ManeuverMaster.Flurry8";
        public static readonly string Flurry8Guid = "{E5CEDED4-321E-48E6-8641-B36BDAEBD741}";

        internal const string Flurry8DisplayName = "ManeuverMasterFlurry8.Name";
        private const string Flurry8Description = "ManeuverMasterFlurry8.Description";

        private const string Flurry8buff = "ManeuverFlurry.Flurry8buff";
        public static readonly string Flurry8buffGuid = "{1D73AD78-519E-47F4-AD13-F72A7BA9341C}";

        private const string Flurry8ActivatableAbility = "ManeuverFlurry.Flurry8ActivatableAbility";
        private static readonly string Flurry8ActivatableAbilityGuid = "{090CCF50-4767-4051-A96E-65C19921B204}";
        private static BlueprintFeature CreateFlurry8()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Flurry8buff, Flurry8buffGuid)
              .SetDisplayName(Flurry8DisplayName)
              .SetDescription(Flurry8Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -3)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(Flurry8ActivatableAbility, Flurry8ActivatableAbilityGuid)
                .SetDisplayName(Flurry8DisplayName)
                .SetDescription(Flurry8Description)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .Configure();

            return FeatureConfigurator.New(Flurry8, Flurry8Guid)
              .SetDisplayName(Flurry8DisplayName)
              .SetDescription(Flurry8Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Flurry15 = "ManeuverMaster.Flurry15";
        public static readonly string Flurry15Guid = "{4D5E9FB9-48A6-42C4-936D-725A9CC91843}";

        internal const string Flurry15DisplayName = "ManeuverMasterFlurry15.Name";
        private const string Flurry15Description = "ManeuverMasterFlurry15.Description";

        private const string Flurry15buff = "ManeuverFlurry.Flurry15buff";
        public static readonly string Flurry15buffGuid = "{595B0559-342E-411A-8FA5-18ABD1593A27}";

        private const string Flurry15ActivatableAbility = "ManeuverFlurry.Flurry1ActivatableAbility";
        private static readonly string Flurry15ActivatableAbilityGuid = "{63730852-9F8B-43DC-B1DD-62479CA44362}";
        private static BlueprintFeature CreateFlurry15()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Flurry15buff, Flurry15buffGuid)
              .SetDisplayName(Flurry15DisplayName)
              .SetDescription(Flurry15Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -4)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(Flurry15ActivatableAbility, Flurry15ActivatableAbilityGuid)
                .SetDisplayName(Flurry15DisplayName)
                .SetDescription(Flurry15Description)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .Configure();

            return FeatureConfigurator.New(Flurry15, Flurry15Guid)
              .SetDisplayName(Flurry15DisplayName)
              .SetDescription(Flurry15Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Flurry1 = "ManeuverMaster.Flurry1";
        private static readonly string Flurry1Guid = "{82B3CBD0-CF0F-4306-8E28-CFB0E61211B0}";

        internal const string Flurry1DisplayName = "ManeuverMasterFlurry1.Name";
        private const string Flurry1Description = "ManeuverMasterFlurry1.Description";
        private static BlueprintFeatureSelection CreateFlurry1()
        {
            var icon = FeatureRefs.DefensiveSpinFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(Flurry1, Flurry1Guid)
              .SetDisplayName(Flurry1DisplayName)
              .SetDescription(Flurry1Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(true)
              .SetObligatory(true)
              .AddToAllFeatures(FeatureRefs.ImprovedBullRush.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedDisarm.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedSunder.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedTrip.ToString())
              .AddToAllFeatures(ImprovedGrapple.StyleGuid)
              .AddToAllFeatures(FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
              .Configure();
        }

        private const string Flurry2 = "ManeuverMaster.Flurry2";
        private static readonly string Flurry2Guid = "{6EF04B19-805B-4C8E-BFFB-E027D6737BFE}";

        internal const string Flurry2DisplayName = "ManeuverMasterFlurry2.Name";
        private const string Flurry2Description = "ManeuverMasterFlurry2.Description";
        private static BlueprintFeature CreateFlurry2()
        {
            var icon = FeatureRefs.DefensiveSpinFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(Flurry2, Flurry2Guid)
              .SetDisplayName(Flurry2DisplayName)
              .SetDescription(Flurry2Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(true)
              .SetObligatory(true)
              .AddToAllFeatures(FeatureRefs.GreaterBullRush.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDisarm.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterSunder.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterTrip.ToString())
              .AddToAllFeatures(GreaterGrapple.FeatGuid)
              .AddToAllFeatures(Flurry1Guid)
              .AddToAllFeatures(FeatureSelectionRefs.MonkBonusFeatSelectionLevel6.ToString())
              .Configure();
        }

        private const string Flurry3 = "ManeuverMaster.Flurry3";
        private static readonly string Flurry3Guid = "{B427FE51-1092-46E8-B5D2-0460C6FDDC27}";
        private static BlueprintFeature CreateFlurry3()
        {
            var icon = FeatureRefs.DefensiveSpinFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(Flurry3, Flurry3Guid)
              .SetDisplayName(Flurry2DisplayName)
              .SetDescription(Flurry2Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(true)
              .SetObligatory(true)
              .AddToAllFeatures(FeatureRefs.GreaterBullRush.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDisarm.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterSunder.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterTrip.ToString())
              .AddToAllFeatures(GreaterGrapple.FeatGuid)
              .AddToAllFeatures(Flurry1Guid)
              .AddToAllFeatures(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
              .Configure();
        }

        private const string Sweeping = "ManeuverMaster.Sweeping";
        private static readonly string SweepingGuid = "{C0599FEF-70F7-4589-8BAF-8062CFAFB5E5}";

        private const string SweepingBuff = "ManeuverMaster.SweepingBuff";
        private static readonly string SweepingBuffGuid = "{ADFDFAB8-1FD0-4501-92DC-17704994CF63}";

        internal const string SweepingDisplayName = "ManeuverMasterSweeping.Name";
        private const string SweepingDescription = "ManeuverMasterSweeping.Description";
        private static BlueprintFeature CreateSweeping()
        {
            var icon = FeatureRefs.ShakeItOff.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(SweepingBuff, SweepingBuffGuid)
             .SetDisplayName(SweepingDisplayName)
             .SetDescription(SweepingDescription)
             .SetIcon(icon)
             .AddFreeActionSpell(AbilityRefs.BullRushAction.ToString())
             .AddFreeActionSpell(AbilityRefs.DirtyTrickBlindnessAction.ToString())
             .AddFreeActionSpell(AbilityRefs.DirtyTrickEntangleAction.ToString())
             .AddFreeActionSpell(AbilityRefs.DirtyTrickSickenedAction.ToString())
             .AddFreeActionSpell(AbilityRefs.DisarmAction.ToString())
             .AddFreeActionSpell(AbilityRefs.SunderAction.ToString())
             .AddFreeActionSpell(AbilityRefs.TripAction.ToString())
             .AddFreeActionSpell(ImprovedGrapple.StyleAbilityGuid)
             .AddAbilityUseTrigger(new() { AbilityRefs.BullRushAction.ToString(), AbilityRefs.DirtyTrickBlindnessAction.ToString(),
                AbilityRefs.DirtyTrickEntangleAction.ToString(), AbilityRefs.DirtyTrickSickenedAction.ToString(), AbilityRefs.DisarmAction.ToString(),
                AbilityRefs.DisarmAction.ToString(), AbilityRefs.SunderAction.ToString(), AbilityRefs.TripAction.ToString(),
                ImprovedGrapple.StyleAbilityGuid }, action: ActionsBuilder.New().RemoveSelf().Build(), forMultipleSpells: true)
             .Configure();

            return FeatureConfigurator.New(Sweeping, SweepingGuid)
              .SetDisplayName(SweepingDisplayName)
              .SetDescription(SweepingDescription)
              .SetIcon(icon)
              .AddAbilityUseTrigger(new() { AbilityRefs.BullRushAction.ToString(), AbilityRefs.DirtyTrickBlindnessAction.ToString(),
                AbilityRefs.DirtyTrickEntangleAction.ToString(), AbilityRefs.DirtyTrickSickenedAction.ToString(), AbilityRefs.DisarmAction.ToString(),
                AbilityRefs.DisarmAction.ToString(), AbilityRefs.SunderAction.ToString(), AbilityRefs.TripAction.ToString(),
                ImprovedGrapple.StyleAbilityGuid }, action: ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1)).Build(),
                afterCast: true, oncePerContext: true, forMultipleSpells: true)
              .Configure();
        }

        private const string Reliable = "ManeuverMaster.Reliable";
        public static readonly string ReliableGuid = "{A469E55D-3278-4385-BF69-C93D8C81F7B6}";

        private const string ReliableAblity = "ManeuverMaster.UseReliable";
        private static readonly string ReliableAblityGuid = "{A54B577A-EED0-496A-9A9D-548B43AB5D30}";

        private const string ReliableBuff2 = "ManeuverMaster.ReliableBuff2";
        private static readonly string ReliableBuff2Guid = "{EA858E67-8F2E-40D8-9226-2DEEAFB98E1A}";

        internal const string ReliableDisplayName = "ManeuverMasterReliable.Name";
        private const string ReliableDescription = "ManeuverMasterReliable.Description";

        public static BlueprintFeature ReliableFeat()
        {
            var icon = FeatureRefs.LegSweepFeature.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(ReliableBuff2, ReliableBuff2Guid)
             .SetDisplayName(ReliableDisplayName)
             .SetDescription(ReliableDescription)
             .SetIcon(icon)
             .AddModifyD20(rule: RuleType.Maneuver, replace: false, rollsAmount: 1, takeBest: true, dispellOnRerollFinished: true)
             .Configure();

            var ability = AbilityConfigurator.New(ReliableAblity, ReliableAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                        .Build())
                .SetDisplayName(ReliableDisplayName)
                .SetDescription(ReliableDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.KiPowerResource.ToString())
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Extraordinary)
                .Configure();

            return FeatureConfigurator.New(Reliable, ReliableGuid)
              .SetDisplayName(ReliableDisplayName)
              .SetDescription(ReliableDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Meditative = "ManeuverMaster.Meditative";
        public static readonly string MeditativeGuid = "{34FBA5AB-0FD6-42AF-A21A-D1D06137B66A}";

        private const string MeditativeAblity = "ManeuverMaster.UseMeditative";
        private static readonly string MeditativeAblityGuid = "{CAD2CD9B-3D95-49E4-9812-C88922F1952D}";

        private const string MeditativeBuff2 = "ManeuverMaster.MeditativeBuff2";
        private static readonly string MeditativeBuff2Guid = "{216F6093-EE95-4D89-BCC5-4ADD1195C428}";

        internal const string MeditativeDisplayName = "ManeuverMasterMeditative.Name";
        private const string MeditativeDescription = "ManeuverMasterMeditative.Description";

        public static BlueprintFeature MeditativeFeat()
        {
            var icon = FeatureRefs.PurityOfBody.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(MeditativeBuff2, MeditativeBuff2Guid)
             .SetDisplayName(MeditativeDisplayName)
             .SetDescription(MeditativeDescription)
             .SetIcon(icon)
             .AddCMBBonus(value: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusWisdom))
             .Configure();

            var ability = AbilityConfigurator.New(MeditativeAblity, MeditativeAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                        .Build())
                .SetDisplayName(MeditativeDisplayName)
                .SetDescription(MeditativeDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Extraordinary)
                .Configure();

            return FeatureConfigurator.New(Meditative, MeditativeGuid)
              .SetDisplayName(MeditativeDisplayName)
              .SetDescription(MeditativeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WhirlwindFeat = "ManeuverMaster.Whirlwind";
        private static readonly string WhirlwindGuid = "{356CD77A-ABDD-4FCD-91B6-9E866B954A79}";
        internal const string WhirlwindDisplayName = "ManeuverMasterWhirlwind.Name";
        private const string WhirlwindDescription = "ManeuverMasterWhirlwind.Description";

        private const string WhirlwindAblity = "ManeuverMaster.UseWhirlwind";
        private static readonly string WhirlwindAblityGuid = "{CF9D45C8-96EB-4FD2-B32C-EFBCC03F2F78}";

        private const string WhirlwindRes = "ManeuverMaster.UseWhirlwindRes";
        public static readonly string WhirlwindResGuid = "{9348F1F3-91C7-48FC-8F0C-47FC069CF279}";

        private const string WhirlwindAblity2 = "ManeuverMaster.UseWhirlwind2";
        private static readonly string WhirlwindAblityGuid2 = "{46F5D117-98ED-4E05-8666-509FDF26FBFB}";

        public static BlueprintFeature CreateWhirlwind()
        {
            var icon = FeatureRefs.ShakeItOff.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(WhirlwindRes, WhirlwindResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var shoot = ActionsBuilder.New()
                .Add<ContextActionSunderStorm>(c => { c.UseAnyType = true; })
                .Build();

            var ability = AbilityConfigurator.New(WhirlwindAblity, WhirlwindAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(WhirlwindDisplayName)
                .SetDescription(WhirlwindDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 50.Feet(), targetType: TargetType.Enemy)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var cast = ActionsBuilder.New()
                .CastSpell(ability)
                .Build();

            var ability2 = AbilityConfigurator.New(WhirlwindAblity2, WhirlwindAblityGuid2)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(cast).Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(WhirlwindDisplayName)
                .SetDescription(WhirlwindDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(WhirlwindFeat, WhirlwindGuid)
              .SetDisplayName(WhirlwindDisplayName)
              .SetDescription(WhirlwindDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
