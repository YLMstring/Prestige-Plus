using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.GrappleFeat;
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
            .AddToRemoveFeatures(1, FeatureRefs.MonkFlurryOfBlowstUnlock.ToString())
            .AddToRemoveFeatures(4, FeatureRefs.StillMind.ToString())
            .AddToRemoveFeatures(5, FeatureRefs.PurityOfBody.ToString())
            .AddToRemoveFeatures(10, FeatureSelectionRefs.MonkKiPowerSelection.ToString())
            .AddToRemoveFeatures(11, FeatureRefs.MonkFlurryOfBlowstLevel11Unlock.ToString())
            .AddToRemoveFeatures(14, FeatureSelectionRefs.MonkKiPowerSelection.ToString())
            .AddToAddFeatures(1, CreateFlurry())
            .AddToAddFeatures(8, CreateFlurry8())
            .AddToAddFeatures(10, CreateSweeping())
            .AddToAddFeatures(15, CreateFlurry15())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MonkProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { FlurryGuid, Flurry8Guid, Flurry15Guid })
                .Configure();
        }

        private const string Flurry = "ManeuverMaster.Flurry";
        public static readonly string FlurryGuid = "{08B7F7DD-D332-4B51-8C1A-D52EBF065CFB}";

        internal const string FlurryDisplayName = "ManeuverMasterFlurry.Name";
        private const string FlurryDescription = "ManeuverMasterFlurry.Description";

        private const string Flurrybuff = "ManeuverFlurry.Flurrybuff";
        public static readonly string FlurrybuffGuid = "{89CCCD57-CC47-4443-A192-1FF7D18F2A3C}";

        private const string FlurryActivatableAbility = "ManeuverFlurry.FlurryActivatableAbility";
        private static readonly string FlurryActivatableAbilityGuid = "{260D7372-AB40-42A7-8611-56BE31CEC209}";
        private static BlueprintFeature CreateFlurry()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Flurrybuff, FlurrybuffGuid)
              .SetDisplayName(FlurryDisplayName)
              .SetDescription(FlurryDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
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
              .AddFacts(new() { ability })
              .AddToIsPrerequisiteFor(CreateFlurry1())
              .AddToIsPrerequisiteFor(CreateFlurry2())
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
        private static readonly string Flurry1Guid = "{538EF175-6943-4785-9F6C-25F29FF3053E}";

        internal const string Flurry1DisplayName = "ManeuverMasterFlurry1.Name";
        private const string Flurry1Description = "ManeuverMasterFlurry1.Description";
        private static BlueprintFeatureSelection CreateFlurry1()
        {
            var icon = AbilityRefs.MasteryOfFlesh.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(Flurry1, Flurry1Guid)
              .SetDisplayName(Flurry1DisplayName)
              .SetDescription(Flurry1Description)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FlurryGuid)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.ImprovedBullRush.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedDisarm.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedSunder.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedTrip.ToString())
              .AddToAllFeatures(ImprovedGrapple.StyleGuid)
              .SetHideNotAvailibleInUI(true)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MonkBonusFeatSelectionLevel1)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MonkBonusFeatSelectionLevel6)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string Flurry2 = "ManeuverMaster.Flurry2";
        private static readonly string Flurry2Guid = "{56B29EEC-07C5-4DC6-821C-0578D957C6EF}";

        internal const string Flurry2DisplayName = "ManeuverMasterFlurry2.Name";
        private const string Flurry2Description = "ManeuverMasterFlurry2.Description";
        private static BlueprintFeature CreateFlurry2()
        {
            var icon = AbilityRefs.MasteryOfFlesh.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(Flurry2, Flurry2Guid)
              .SetDisplayName(Flurry2DisplayName)
              .SetDescription(Flurry2Description)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FlurryGuid)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.GreaterBullRush.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterDisarm.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterSunder.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterTrip.ToString())
              .AddToAllFeatures(GreaterGrapple.FeatGuid)
              .SetHideNotAvailibleInUI(true)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MonkBonusFeatSelectionLevel6)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MonkBonusFeatSelectionLevel10)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string Sweeping = "ManeuverMaster.Sweeping";
        private static readonly string SweepingGuid = "{C0599FEF-70F7-4589-8BAF-8062CFAFB5E5}";

        private const string SweepingBuff = "ManeuverMaster.SweepingBuff";
        private static readonly string SweepingBuffGuid = "{ADFDFAB8-1FD0-4501-92DC-17704994CF63}";

        internal const string SweepingDisplayName = "ManeuverMasterSweeping.Name";
        private const string SweepingDescription = "ManeuverMasterSweeping.Description";
        private static BlueprintFeature CreateSweeping()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

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
    }
}
