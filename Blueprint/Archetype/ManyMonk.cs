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
using PrestigePlus.CustomComponent.Archetype;

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
            .AddToAddFeatures(1, CreateManyFeat1(), CreateFuse())
            .AddToAddFeatures(2, ManyFeat1Guid)
            .AddToAddFeatures(5, CreateManyFeat2())
            .AddToAddFeatures(6, ManyFeat2Guid)
            .AddToAddFeatures(8, CreateFuse8())
            .AddToAddFeatures(9, ManyFeat2Guid)
            .AddToAddFeatures(10, ManyFeat2Guid)
            .AddToAddFeatures(13, ManyFeat2Guid)
            .AddToAddFeatures(14, ManyFeat2Guid)
            .AddToAddFeatures(15, CreateFuse15())
            .AddToAddFeatures(17, ManyFeat2Guid)
            .AddToAddFeatures(18, ManyFeat2Guid)
            .AddToAddFeatures(20, CreateFuse20())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MonkProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { FuseGuid, Fuse8Guid, Fuse15Guid, Fuse20Guid })
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { ManyFeat1Guid, ManyFeat2Guid })
                .Configure();
        }

        private const string Fuse = "ManyMonk.Fuse";
        public static readonly string FuseGuid = "{215C042D-A354-4AC9-AD19-5A9FBA9EBBE5}";

        internal const string FuseDisplayName = "ManyMonkFuse.Name";
        private const string FuseDescription = "ManyMonkFuse.Description";
        private static BlueprintFeature CreateFuse()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            return FeatureConfigurator.New(Fuse, FuseGuid)
              .SetDisplayName(FuseDisplayName)
              .SetDescription(FuseDescription)
              .SetIcon(icon)
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();
        }

        private const string Fuse8 = "ManyMonk.Fuse8";
        public static readonly string Fuse8Guid = "{875D706F-C309-490A-A20B-5238D65694A3}";

        internal const string Fuse8DisplayName = "ManyMonkFuse8.Name";
        private const string Fuse8Description = "ManyMonkFuse8.Description";
        private static BlueprintFeature CreateFuse8()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            return FeatureConfigurator.New(Fuse8, Fuse8Guid)
              .SetDisplayName(Fuse8DisplayName)
              .SetDescription(Fuse8Description)
              .SetIcon(icon)
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .AddComponent<FuseStyleAttackBonus>()
              .Configure();
        }

        private const string Fuse15 = "ManyMonk.Fuse15";
        public static readonly string Fuse15Guid = "{469BE056-D1FB-41A3-B896-7A20E85EEEDD}";

        internal const string Fuse15DisplayName = "ManyMonkFuse15.Name";
        private const string Fuse15Description = "ManyMonkFuse15.Description";
        private static BlueprintFeature CreateFuse15()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            return FeatureConfigurator.New(Fuse15, Fuse15Guid)
              .SetDisplayName(Fuse15DisplayName)
              .SetDescription(Fuse15Description)
              .SetIcon(icon)
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();
        }

        private const string Fuse20 = "ManyMonk.Fuse20";
        public static readonly string Fuse20Guid = "{8EF717B4-C44E-47F7-A720-1AF10C45F943}";

        private const string Fuse201 = "ManyMonk.Fuse201";
        public static readonly string Fuse201Guid = "{485EEB14-3E1B-439F-AE69-E53D2622512D}";

        private const string Fuse202 = "ManyMonk.Fuse202";
        public static readonly string Fuse202Guid = "{4023A57D-09B7-45A1-B828-BFAB7753469C}";

        private const string Fuse203 = "ManyMonk.Fuse203";
        public static readonly string Fuse203Guid = "{8F4FF68E-9324-41A1-8CF0-FB735BA5B5F5}";

        private const string Fuse204 = "ManyMonk.Fuse204";
        public static readonly string Fuse204Guid = "{E6F5DB42-C1D2-46E1-83F2-558B33F8DC90}";

        private const string Fuse205 = "ManyMonk.Fuse205";
        public static readonly string Fuse205Guid = "{8F029F73-6642-40D0-8A60-6FF8C48297E8}";

        private const string Fuse206 = "ManyMonk.Fuse206";
        public static readonly string Fuse206Guid = "{B8A36150-C0BF-4F0A-9B88-C37DE5475998}";

        private const string Fuse207 = "ManyMonk.Fuse207";
        public static readonly string Fuse207Guid = "{DC25DB68-42E9-40E0-9DC9-1F218D2AF533}";

        internal const string Fuse20DisplayName = "ManyMonkFuse20.Name";
        private const string Fuse20Description = "ManyMonkFuse20.Description";
        private static BlueprintFeature CreateFuse20()
        {
            var icon = AbilityRefs.RestorationGreater.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(Fuse201, Fuse201Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat2 = FeatureConfigurator.New(Fuse202, Fuse202Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat3 = FeatureConfigurator.New(Fuse203, Fuse203Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat4 = FeatureConfigurator.New(Fuse204, Fuse204Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat5 = FeatureConfigurator.New(Fuse205, Fuse205Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat6 = FeatureConfigurator.New(Fuse206, Fuse206Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            var feat7 = FeatureConfigurator.New(Fuse207, Fuse207Guid)
              .SetHideInCharacterSheetAndLevelUp()
              .SetHideInUI()
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .Configure();

            return FeatureConfigurator.New(Fuse20, Fuse20Guid)
              .SetDisplayName(Fuse20DisplayName)
              .SetDescription(Fuse20Description)
              .SetIcon(icon)
              .AddIncreaseActivatableAbilityGroupSize(ActivatableAbilityGroup.CombatStyle)
              .AddFacts(new() { feat1, feat2, feat3, feat4, feat5, feat6, feat7 })
              .Configure();
        }

        private const string ManyFeat1 = "ManyMonk.ManyFeat1";
        private static readonly string ManyFeat1Guid = "{66C578E8-04E2-4E48-BC2A-ADC29F390EB3}";

        internal const string ManyFeat1DisplayName = "ManyMonkManyFeat1.Name";
        private const string ManyFeat1Description = "ManyMonkManyFeat1.Description";
        private static BlueprintFeatureSelection CreateManyFeat1()
        {
            var icon = FeatureRefs.CraneStyleFeat.Reference.Get().Icon;

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
            var icon = FeatureRefs.CraneStyleFeat.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(ManyFeat2, ManyFeat2Guid)
              .SetDisplayName(ManyFeat2DisplayName)
              .SetDescription(ManyFeat2Description)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(ManyFeat1Guid)
              .Configure();
        }
    }
}
