using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Blueprint.GrappleFeat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class ManyMonk
    {
        private const string ArchetypeName = "ManyMonk";
        private static readonly string ArchetypeGuid = "{743A86F0-5807-4114-BB69-4A3AD75C6C5C}";
        internal const string ArchetypeDisplayName = "ManyMonk.Name";
        private const string ArchetypeDescription = "ManyMonk.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MonkClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureRefs.MonkFlurryOfBlowstUnlock.ToString(), FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
            .AddToRemoveFeatures(2, FeatureSelectionRefs.MonkBonusFeatSelectionLevel1.ToString())
            .AddToRemoveFeatures(5, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .AddToRemoveFeatures(6, FeatureSelectionRefs.MonkBonusFeatSelectionLevel6.ToString())
            .AddToRemoveFeatures(9, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .AddToRemoveFeatures(10, FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToRemoveFeatures(11, FeatureRefs.MonkFlurryOfBlowstLevel11Unlock.ToString())
            .AddToRemoveFeatures(13, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .AddToRemoveFeatures(14, FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToRemoveFeatures(17, FeatureSelectionRefs.MonkStyleStrike.ToString())
            .AddToRemoveFeatures(18, FeatureSelectionRefs.MonkBonusFeatSelectionLevel10.ToString())
            .AddToRemoveFeatures(20, FeatureRefs.KiPerfectSelfFeature.ToString())
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

        private const string Flurry = "ManyMonk.Flurry";
        public static readonly string FlurryGuid = "{08B7F7DD-D332-4B51-8C1A-D52EBF065CFB}";

        private const string FlurryFeat = "ManyMonk.FlurryFeat";
        public static readonly string FlurryFeatGuid = "{C4D92D6C-368A-49B3-93F2-5A7D12850B93}";

        internal const string FlurryDisplayName = "ManyMonkFlurry.Name";
        private const string FlurryDescription = "ManyMonkFlurry.Description";

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
                .Configure();

            BuffConfigurator.New(FlurryCoolDownbuff, FlurryCoolDownbuffGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -2)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buff = BuffConfigurator.New(Flurrybuff, FlurrybuffGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddMonkNoArmorFeatureUnlock(feat)
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

        private const string Flurry8 = "ManyMonk.Flurry8";
        public static readonly string Flurry8Guid = "{E5CEDED4-321E-48E6-8641-B36BDAEBD741}";

        internal const string Flurry8DisplayName = "ManyMonkFlurry8.Name";
        private const string Flurry8Description = "ManyMonkFlurry8.Description";

        private const string Flurry8buff = "ManeuverFlurry.Flurry8buff";
        public static readonly string Flurry8buffGuid = "{1D73AD78-519E-47F4-AD13-F72A7BA9341C}";

        private const string Flurry9buff = "ManeuverFlurry.Flurry9buff";
        public static readonly string Flurry9buffGuid = "{21B758EC-4AC3-4E45-AAB8-DEA7D8F1AFBA}";

        private const string Flurry8ActivatableAbility = "ManeuverFlurry.Flurry8ActivatableAbility";
        private static readonly string Flurry8ActivatableAbilityGuid = "{090CCF50-4767-4051-A96E-65C19921B204}";
        private static BlueprintFeature CreateFlurry8()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Flurry9buff, Flurry9buffGuid)
              .SetDisplayName(Flurry8DisplayName)
              .SetDescription(Flurry8Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -3)
              .Configure();

            var Buff = BuffConfigurator.New(Flurry8buff, Flurry8buffGuid)
              .SetDisplayName(Flurry8DisplayName)
              .SetDescription(Flurry8Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffExtraEffects(FlurryCoolDownbuffGuid, extraEffectBuff: Buff2)
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

        private const string Flurry15 = "ManyMonk.Flurry15";
        public static readonly string Flurry15Guid = "{4D5E9FB9-48A6-42C4-936D-725A9CC91843}";

        internal const string Flurry15DisplayName = "ManyMonkFlurry15.Name";
        private const string Flurry15Description = "ManyMonkFlurry15.Description";

        private const string Flurry15buff = "ManeuverFlurry.Flurry15buff";
        public static readonly string Flurry15buffGuid = "{595B0559-342E-411A-8FA5-18ABD1593A27}";

        private const string Flurry16buff = "ManeuverFlurry.Flurry16buff";
        public static readonly string Flurry16buffGuid = "{43ED4525-DF36-44DF-8FD6-8456A4E1E976}";

        private const string Flurry15ActivatableAbility = "ManeuverFlurry.Flurry1ActivatableAbility";
        private static readonly string Flurry15ActivatableAbilityGuid = "{63730852-9F8B-43DC-B1DD-62479CA44362}";
        private static BlueprintFeature CreateFlurry15()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Flurry16buff, Flurry16buffGuid)
              .SetDisplayName(Flurry15DisplayName)
              .SetDescription(Flurry15Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddCMBBonus(descriptor: ModifierDescriptor.Penalty, value: -4)
              .Configure();

            var Buff = BuffConfigurator.New(Flurry15buff, Flurry15buffGuid)
              .SetDisplayName(Flurry15DisplayName)
              .SetDescription(Flurry15Description)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffExtraEffects(FlurryCoolDownbuffGuid, extraEffectBuff: Buff2)
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

        private const string ManyFeat1 = "ManyMonk.ManyFeat1";
        private static readonly string ManyFeat1Guid = "{66C578E8-04E2-4E48-BC2A-ADC29F390EB3}";

        internal const string ManyFeat1DisplayName = "ManyMonkManyFeat1.Name";
        private const string ManyFeat1Description = "ManyMonkManyFeat1.Description";
        private static BlueprintFeatureSelection CreateManyFeat1()
        {
            var icon = FeatureRefs.DefensiveSpinFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(ManyFeat1, ManyFeat1Guid)
              .SetDisplayName(ManyFeat1DisplayName)
              .SetDescription(ManyFeat1Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(true)
              .SetObligatory(true)
              .AddToAllFeatures(BodyGuard.StyleMasterGuid)
              .AddToAllFeatures(FeatureRefs.ElementalFistFeature.ToString())
              .Configure();
        }

        private const string ManyFeat2 = "ManyMonk.ManyFeat2";
        public static readonly string ManyFeat2Guid = "{8E7962C4-C4DB-494F-A1BF-B796F0C799BA}";

        internal const string ManyFeat2DisplayName = "ManyMonkManyFeat2.Name";
        private const string ManyFeat2Description = "ManyMonkManyFeat2.Description";
        private static BlueprintFeature CreateManyFeat2()
        {
            var icon = FeatureRefs.DefensiveSpinFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(ManyFeat2, ManyFeat2Guid)
              .SetDisplayName(ManyFeat2DisplayName)
              .SetDescription(ManyFeat2Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(ManyFeat1Guid)
              .Configure();
        }

        private const string Flurry3 = "ManyMonk.Flurry3";
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
    }
}
