using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class BodyGuard
    {
        private static readonly string FeatName = "FeatBodyGuard";
        public static readonly string FeatGuid = "{E1C5FEBC-7D2C-445D-A452-8A77C340E14F}";

        private static readonly string DisplayName = "FeatBodyGuard.Name";
        private static readonly string Description = "FeatBodyGuard.Description";

        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianBodyguardAbility.ToString() })
                    .AddAbilityResources(resource: AbilityResourceRefs.DivineGuardianTrothResource.ToString(), restoreAmount: true)
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string Feat2Name = "FeatHarmWay";
        public static readonly string Feat2Guid = "{F0611E7B-A68C-49E4-8BEC-97A1173AD609}";

        private static readonly string DisplayName2 = "FeatHarmWay.Name";
        private static readonly string Description2 = "FeatHarmWay.Description";

        public static void Configure2()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatGuid)
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.ToString() })
                    .AddAbilityResources(resource: AbilityResourceRefs.DivineGuardianTrothResource.ToString(), restoreAmount: true)
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private const string NinjaTrickFeat = "NinjaTrick.NinjaTrick";
        public static readonly string NinjaTrickGuid = "{4C242C11-9754-47BF-BD40-5C10489999A3}";

        internal const string NinjaTrickDisplayName = "NinjaTrick.Name";
        private const string NinjaTrickDescription = "NinjaTrick.Description";
        public static void NinjaTrickConfigure()
        {
            var icon = AbilityRefs.TrickeryBlessingMajorAbility.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(NinjaTrickFeat, NinjaTrickGuid)
              .SetDisplayName(NinjaTrickDisplayName)
              .SetDescription(NinjaTrickDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(UnarmedCombatFeat())
              .AddToAllFeatures(ArrowSnatchFeat())
              .AddToAllFeatures(BloodDebtFeat())
              .AddToAllFeatures(KamiWardenFeat())
              .AddToAllFeatures(GreaterNameGuid)
              .AddToAllFeatures(StyleMasterGuid)
              .AddToAllFeatures(KatateDoriGuid)
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.RogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DarkLurkerRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.ExtraRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string UnarmedCombat = "NinjaTrick.UnarmedCombat";
        public static readonly string UnarmedCombatGuid = "{5BFCCF83-947B-4F32-8BFC-C304A8591594}";

        internal const string UnarmedCombatDisplayName = "NinjaTrickUnarmedCombat.Name";
        private const string UnarmedCombatDescription = "NinjaTrickUnarmedCombat.Description";

        public static BlueprintFeature UnarmedCombatFeat()
        {
            var icon = FeatureRefs.ImprovedUnarmedStrike.Reference.Get().Icon;

            return FeatureConfigurator.New(UnarmedCombat, UnarmedCombatGuid)
              .SetDisplayName(UnarmedCombatDisplayName)
              .SetDescription(UnarmedCombatDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddFacts(new() { FeatureRefs.ImprovedUnarmedStrike.ToString() })
              .Configure();
        }

        private const string ArrowSnatch = "NinjaTrick.ArrowSnatch";
        public static readonly string ArrowSnatchGuid = "{222D49BE-3885-41BF-8A34-EB2ED46950FB}";

        internal const string ArrowSnatchDisplayName = "NinjaTrickArrowSnatch.Name";
        private const string ArrowSnatchDescription = "NinjaTrickArrowSnatch.Description";

        public static BlueprintFeature ArrowSnatchFeat()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            return FeatureConfigurator.New(ArrowSnatch, ArrowSnatchGuid)
              .SetDisplayName(ArrowSnatchDisplayName)
              .SetDescription(ArrowSnatchDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.DeflectArrows.ToString())
              .AddFacts(new() { FeatureRefs.DeflectArrows.ToString() })
              .Configure();
        }

        private const string KatateDori = "NinjaTrick.KatateDori";
        public static readonly string KatateDoriGuid = "{9CBB2D2C-4741-438A-BA21-5080D3DDA010}";

        internal const string KatateDoriDisplayName = "NinjaTrickKatateDori.Name";
        private const string KatateDoriDescription = "NinjaTrickKatateDori.Description";

        public static BlueprintFeature KatateDoriFeat()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            return FeatureConfigurator.New(KatateDori, KatateDoriGuid)
              .SetDisplayName(KatateDoriDisplayName)
              .SetDescription(KatateDoriDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.DeflectArrows.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddPrerequisiteNoFeature(Juggler.SnatchArrowsGuid)
              .AddFacts(new() { Juggler.SnatchArrowsGuid })
              .Configure();
        }

        private const string BloodDebt = "NinjaTrick.BloodDebt";
        public static readonly string BloodDebtGuid = "{90FF3759-A8A7-44F1-AFB8-91D455D61E61}";

        internal const string BloodDebtDisplayName = "NinjaTrickBloodDebt.Name";
        private const string BloodDebtDescription = "NinjaTrickBloodDebt.Description";

        public static BlueprintFeature BloodDebtFeat()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.Reference.Get().Icon;

            return FeatureConfigurator.New(BloodDebt, BloodDebtGuid)
              .SetDisplayName(BloodDebtDisplayName)
              .SetDescription(BloodDebtDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(Feat2Guid)
              .AddFacts(new() { Feat2Guid })
              .Configure();
        }

        private const string KamiWarden = "NinjaTrick.KamiWarden";
        public static readonly string KamiWardenGuid = "{87E9F26F-622F-42BD-9000-0B66BD4F571E}";

        internal const string KamiWardenDisplayName = "NinjaTrickKamiWarden.Name";
        private const string KamiWardenDescription = "NinjaTrickKamiWarden.Description";

        public static BlueprintFeature KamiWardenFeat()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            return FeatureConfigurator.New(KamiWarden, KamiWardenGuid)
              .SetDisplayName(KamiWardenDisplayName)
              .SetDescription(KamiWardenDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.AdvanceTalents.ToString())
              .AddPrerequisiteNoFeature(FeatGuid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteNoFeature(Feat2Guid, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddFacts(new() { FeatGuid, Feat2Guid })
              .Configure();
        }

        private const string GreaterUnarmedStrike = "NinjaTrick.GreaterUnarmedStrike";
        public static readonly string GreaterUnarmedStrikeGuid = "{C55DC946-E283-4414-AB3E-5510E593D1D4}";

        internal const string GreaterUnarmedStrikeDisplayName = "NinjaTrickGreaterUnarmedStrike.Name";
        private const string GreaterUnarmedStrikeDescription = "NinjaTrickGreaterUnarmedStrike.Description";
        public static BlueprintProgression GreaterUnarmedStrikeFeat()
        {
            var icon = FeatureRefs.KiQuiveringPalmFeature.Reference.Get().Icon;
            var feat = ProgressionConfigurator.New(GreaterUnarmedStrike, GreaterUnarmedStrikeGuid)
              .SetDisplayName(GreaterUnarmedStrikeDisplayName)
              .SetDescription(GreaterUnarmedStrikeDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillAthletics, 3)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(3, FeatureRefs.MonkUnarmedStrikeLevel4.ToString())
              .AddToLevelEntry(10, FeatureRefs.MonkUnarmedStrikeLevel8.ToString())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DragonLevel2FeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.FighterFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.CombatTrick)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterTricksterCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.StudentOfWarCombatFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string GreaterNameFeat = "GreaterName.GreaterName";
        public static readonly string GreaterNameGuid = "{8334A97B-17E0-4885-AE60-AF656A1F10C7}";

        internal const string GreaterNameDisplayName = "GreaterName.Name";
        private const string GreaterNameDescription = "GreaterName.Description";
        public static BlueprintFeatureSelection GreaterNameConfigure()
        {
            var icon = FeatureRefs.KiQuiveringPalmFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(GreaterNameFeat, GreaterNameGuid)
              .SetDisplayName(GreaterNameDisplayName)
              .SetDescription(GreaterNameDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddPrerequisiteNoFeature(GreaterUnarmedStrikeGuid)
              .SetIgnorePrerequisites(true)
              .SetObligatory(false)
              .AddToAllFeatures(GreaterUnarmedStrikeFeat())
              .Configure();
        }

        private const string StyleMasterFeat = "StyleMaster.StyleMaster";
        public static readonly string StyleMasterGuid = "{AF7F5406-0F87-4F51-A21D-F8F99B5C1320}";

        internal const string StyleMasterDisplayName = "StyleMaster.Name";
        private const string StyleMasterDescription = "StyleMaster.Description";
        public static BlueprintFeatureSelection StyleMasterConfigure()
        {
            var icon = FeatureRefs.CraneStyleFeat.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(StyleMasterFeat, StyleMasterGuid)
              .SetDisplayName(StyleMasterDisplayName)
              .SetDescription(StyleMasterDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(StyleMasterGuid)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .Configure();
        }
    }
}
