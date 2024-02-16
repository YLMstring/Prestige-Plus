using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.AreaLogic;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewComponents;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class MammothRider
    {
        private const string ArchetypeName = "MammothRider";
        public static readonly string ArchetypeGuid = "{FBD0C6EB-0513-483B-862F-A087D32B03D1}";
        internal const string ArchetypeDisplayName = "MammothRider.Name";
        private const string ArchetypeDescription = "MammothRider.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "MammothRiderPrestige";
        private static readonly string ClassProgressGuid = "{E181B319-4335-4C21-8401-DF2DD15753D3}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, GiganticSteedFeat(), SummonAnimalSelection())
                .AddToLevelEntry(2, ExtraFeat(), ColossusHunterFeat())
                .AddToLevelEntry(3, RuggedSteedFeat())
                .AddToLevelEntry(4, MistrustFeat(), FeatureRefs.IntimidatingProwess.ToString())
                .AddToLevelEntry(5, RuggedSteedGuid, ValiantDevotionFeat())
                .AddToLevelEntry(6, BornSurvivorGuidFeat, SummonQuarrySelection())
                .AddToLevelEntry(7, RuggedSteedGuid, ReachSteedFeat())
                .AddToLevelEntry(8, CombinedMightFeat())
                .AddToLevelEntry(9, PulverizingAssaultFeat(), RuggedSteedGuid)
                .AddToLevelEntry(10, MammothLordFeat())
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D12)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeLow)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillLoreNature, StatType.SkillMobility })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddPrerequisiteStatValue(StatType.SkillMobility, 9)
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 9)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string GiganticSteed = "MammothRider.GiganticSteed";
        private static readonly string GiganticSteedGuid = "{4DF25CF5-DCC7-42DD-9F17-77DA1BBEA52B}";

        internal const string GiganticSteedDisplayName = "MammothRiderGiganticSteed.Name";
        private const string GiganticSteedDescription = "MammothRiderGiganticSteed.Description";

        private const string GiganticSteed1 = "MammothRider.GiganticSteed1";
        public static readonly string GiganticSteed1Guid = "{D5D06F79-F7FF-4AB5-96FE-34CE19034EF4}";

        private const string GiganticSteed2 = "MammothRider.GiganticSteed2";
        private static readonly string GiganticSteed2Guid = "{19D65C1A-348D-45F6-A4AF-DAD42E6B4966}";

        private const string GiganticSteed3 = "MammothRider.GiganticSteed3";
        private static readonly string GiganticSteed3Guid = "{D110315F-EDD1-4E10-97EF-62EC7BE3C652}";
        public static BlueprintFeature GiganticSteedFeat()
        {
            var icon = AbilityRefs.AnimalGrowth.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(GiganticSteed1, GiganticSteed1Guid)
              .SetDisplayName(GiganticSteedDisplayName)
              .SetDescription(GiganticSteedDescription)
              .SetIcon(icon)
              //.AddChangeUnitSize(type: Kingmaker.Designers.Mechanics.Buffs.ChangeUnitSize.ChangeType.Delta, size: Kingmaker.Enums.Size.Fine, sizeDelta: 1)
              //.AddComponent<ChangeUnitBaseSize>(c => { c.Size = Kingmaker.Enums.Size.Huge; c.m_Type = TabletopTweaks.Core.NewUnitParts.UnitPartBaseSizeAdjustment.ChangeType.Value; })
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.Penalty, false, StatType.Dexterity, -2)
              .Configure();

            var feat2 = FeatureConfigurator.New(GiganticSteed2, GiganticSteed2Guid)
              .SetDisplayName(GiganticSteedDisplayName)
              .SetDescription(GiganticSteedDescription)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, false, StatType.Strength, 2)
              .SetRanks(10)
              .Configure();

            var feat3 = FeatureConfigurator.New(GiganticSteed3, GiganticSteed3Guid)
              .SetDisplayName(GiganticSteedDisplayName)
              .SetDescription(GiganticSteedDescription)
              .SetIcon(icon)
              .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.UntypedStackable, false, StatType.Constitution, 2)
              .SetRanks(10)
              .Configure();

            return FeatureConfigurator.New(GiganticSteed, GiganticSteedGuid)
              .SetDisplayName(GiganticSteedDisplayName)
              .SetDescription(GiganticSteedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .AddComponent<GiganticSteedLogic>()
              .Configure();
        }

        private const string SummonBigAnimal2 = "MammothRider.SummonBigAnimal2";
        private static readonly string SummonBigAnimalGuid2 = "{EE2E50B6-19D1-4DC3-A3FD-F14C5E5251B5}";

        private const string ClassProgressName2 = "MammothRiderPrestige2";
        private static readonly string ClassProgressGuid2 = "{72CF94B0-8FDF-4B96-B8FF-5FEDF439D872}";
        public static BlueprintFeatureSelection SummonAnimalSelection()
        {
            //var icon = AbilityRefs.SummonElvenSpirits.Reference.Get().Icon;

            var progression =
                ProgressionConfigurator.New(ClassProgressName2, ClassProgressGuid2)
                .CopyFrom(ProgressionRefs.SylvanSorcererAnimalCompanionProgression, typeof(LevelEntry), typeof(UIGroup))
                .SetClasses(ArchetypeGuid)
                .Configure();

            return FeatureSelectionConfigurator.New(SummonBigAnimal2, SummonBigAnimalGuid2)
              .CopyFrom(
                FeatureSelectionRefs.AnimalCompanionSelectionSylvanSorcerer)
              .AddFeatureOnApply(progression)
              .AddFeatureOnApply(FeatureRefs.AnimalCompanionRank.ToString())
              .AddFeatureOnApply(FeatureRefs.MountTargetFeature.ToString())
              .AddFeatureOnApply(FeatureSelectionRefs.AnimalCompanionArchetypeSelection.ToString())
              .Configure(delayed: true);
        }

        private const string BornSurvivorFeat = "MammothRider.BornSurvivorFeat";
        private static readonly string BornSurvivorGuidFeat = "{5FC8297D-3927-4AD9-ABB3-12913D816F26}";

        internal const string ShadowFeatDisplayName = "MammothRiderFeat.Name";
        private const string ShadowFeatDescription = "MammothRiderFeat.Description";
        public static BlueprintFeatureSelection ExtraFeat()
        {
            //var icon = AbilityRefs.KiAbudantStep.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(BornSurvivorFeat, BornSurvivorGuidFeat)
              .SetDisplayName(ShadowFeatDisplayName)
              .SetDescription(ShadowFeatDescription)
              //.SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.GreatFortitude.ToString())
              .AddToAllFeatures(FeatureRefs.IronWill.ToString())
              .AddToAllFeatures(FeatureRefs.LightningReflexes.ToString())
              .AddToAllFeatures(FeatureRefs.GreatFortitudeImproved.ToString())
              .AddToAllFeatures(FeatureRefs.IronWillImproved.ToString())
              .AddToAllFeatures(FeatureRefs.LightningReflexesImproved.ToString())
              .Configure();
        }

        private const string RuggedSteed = "MammothRider.RuggedSteed";
        private static readonly string RuggedSteedGuid = "{EB7C6028-97F1-4CDA-99B2-3AB8A22D8F2D}";

        internal const string RuggedSteedDisplayName = "MammothRiderRuggedSteed.Name";
        private const string RuggedSteedDescription = "MammothRiderRuggedSteed.Description";
        public static BlueprintFeature RuggedSteedFeat()
        {
            var icon = AbilityRefs.Barkskin.Reference.Get().Icon;

            return FeatureConfigurator.New(RuggedSteed, RuggedSteedGuid)
              .SetDisplayName(RuggedSteedDisplayName)
              .SetDescription(RuggedSteedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();
        }

        private const string ColossusHunter = "MammothRider.ColossusHunter";
        private static readonly string ColossusHunterGuid = "{658E8A16-A9BF-45F4-B79C-B64385E7649A}";

        internal const string ColossusHunterDisplayName = "MammothRiderColossusHunter.Name";
        private const string ColossusHunterDescription = "MammothRiderColossusHunter.Description";
        public static BlueprintFeature ColossusHunterFeat()
        {
            var icon = FeatureRefs.FavoredEnemyLargeFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(ColossusHunter, ColossusHunterGuid)
              .SetDisplayName(ColossusHunterDisplayName)
              .SetDescription(ColossusHunterDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAttackBonusAgainstSize(size: Kingmaker.Enums.Size.Large, value: ContextValues.Constant(1))
              .AddAttackBonusAgainstSize(size: Kingmaker.Enums.Size.Huge, value: ContextValues.Constant(1))
              .AddAttackBonusAgainstSize(size: Kingmaker.Enums.Size.Gargantuan, value: ContextValues.Constant(2))
              .AddAttackBonusAgainstSize(size: Kingmaker.Enums.Size.Colossal, value: ContextValues.Constant(2))
              .AddDamageBonusConditional(checkWielder: false, conditions: ConditionsBuilder.New()
                    .Size(size: Kingmaker.Enums.Size.Large, exactlyEquals: true)
                    .Build(), bonus: ContextValues.Constant(1))
              .AddDamageBonusConditional(checkWielder: false, conditions: ConditionsBuilder.New()
                    .Size(size: Kingmaker.Enums.Size.Huge, exactlyEquals: true)
                    .Build(), bonus: ContextValues.Constant(1))
              .AddDamageBonusConditional(checkWielder: false, conditions: ConditionsBuilder.New()
                    .Size(size: Kingmaker.Enums.Size.Gargantuan, exactlyEquals: true)
                    .Build(), bonus: ContextValues.Constant(2))
              .AddDamageBonusConditional(checkWielder: false, conditions: ConditionsBuilder.New()
                    .Size(size: Kingmaker.Enums.Size.Colossal, exactlyEquals: true)
                    .Build(), bonus: ContextValues.Constant(2))
              .Configure();
        }

        private const string Mistrust = "MammothRider.Mistrust";
        private static readonly string MistrustGuid = "{1207C7A4-6BF4-4F48-844D-F83C7089487D}";

        internal const string MistrustDisplayName = "MammothRiderMistrust.Name";
        private const string MistrustDescription = "MammothRiderMistrust.Description";
        public static BlueprintFeature MistrustFeat()
        {
            var icon = FeatureRefs.AuraOfResolveFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Mistrust, MistrustGuid)
              .SetDisplayName(MistrustDisplayName)
              .SetDescription(MistrustDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddComponent<RiderMistrust>()
              .Configure();
        }

        private const string ValiantDevotion = "MammothRider.ValiantDevotion";
        private static readonly string ValiantDevotionGuid = "{5E062447-8282-4B2E-99F5-FEBBCE781D4A}";

        internal const string ValiantDevotionDisplayName = "MammothRiderValiantDevotion.Name";
        private const string ValiantDevotionDescription = "MammothRiderValiantDevotion.Description";

        private const string GiganticSteed6 = "MammothRider.GiganticSteed6";
        private static readonly string GiganticSteed6Guid = "{E6623989-2B79-42B1-A4AE-582ECFA96FEE}";
        public static BlueprintFeature ValiantDevotionFeat()
        {
            var icon = FeatureRefs.AuraOfCourageFeature.Reference.Get().Icon;

            var feat6 = FeatureConfigurator.New(GiganticSteed6, GiganticSteed6Guid)
              .SetDisplayName(ValiantDevotionDisplayName)
              .SetDescription(ValiantDevotionDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Charm, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Compulsion, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Fear, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .Configure();

            return FeatureConfigurator.New(ValiantDevotion, ValiantDevotionGuid)
              .SetDisplayName(ValiantDevotionDisplayName)
              .SetDescription(ValiantDevotionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat6, PetType.AnimalCompanion)
              .Configure();
        }

        private const string SummonBigQuarry2 = "MammothRider.SummonBigQuarry2";
        private static readonly string SummonBigQuarryGuid2 = "{072D3C34-8448-4D87-B0D7-147D8AB10CEB}";

        internal const string SummonBigQuarryDisplayName = "MammothRiderSummonBigQuarry.Name";
        private const string SummonBigQuarryDescription = "MammothRiderSummonBigQuarry.Description";

        public static BlueprintFeatureSelection SummonQuarrySelection()
        {
            var icon = FeatureRefs.Quarry.Reference.Get().Icon;

            FeatureConfigurator.For(FeatureRefs.ImprovedQuarry)
              .AddPrerequisiteFeature(FeatureRefs.Quarry.ToString())
              .Configure();

            return FeatureSelectionConfigurator.New(SummonBigQuarry2, SummonBigQuarryGuid2)
              .SetDisplayName(SummonBigQuarryDisplayName)
              .SetDescription(SummonBigQuarryDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.Quarry.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedQuarry.ToString())
              .Configure();
        }

        private const string ReachSteed = "MammothRider.ReachSteed";
        private static readonly string ReachSteedGuid = "{B431E996-D704-4046-BF1E-7FB46A117867}";

        internal const string ReachSteedDisplayName = "MammothRiderReachSteed.Name";
        private const string ReachSteedDescription = "MammothRiderReachSteed.Description";

        private const string GiganticSteed5 = "MammothRider.GiganticSteed5";
        private static readonly string GiganticSteed5Guid = "{D20FC086-5821-430F-A7BC-357FE7A5DA08}";

        private const string ReleaseAbility = "GiganticAssault.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{0A583282-B9AD-45A1-BE18-328F79D92C69}";

        private const string ReleaseAbilitybuff = "GiganticAssault.ReleaseAbilitybuff";
        private static readonly string ReleaseAbilitybuffGuid = "{747F27A3-58AD-496D-9E46-D24FE93B71AA}";
        public static BlueprintFeature ReachSteedFeat()
        {
            var icon = FeatureRefs.DragonStyle.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(ReleaseAbilitybuff, ReleaseAbilitybuffGuid)
              .SetDisplayName(ReachSteedDisplayName)
              .SetDescription(ReachSteedDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddStatBonus(stat: StatType.Reach, value: 4)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .SetDisplayName(ReachSteedDisplayName)
                .SetDescription(ReachSteedDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            var feat5 = FeatureConfigurator.New(GiganticSteed5, GiganticSteed5Guid)
              .SetDisplayName(ReachSteedDisplayName)
              .SetDescription(ReachSteedDescription)
              .SetIcon(icon)
              //.AddStatBonus(stat: StatType.Reach, value: 4)
              .AddFacts(new() { ability })
              .Configure();

            return FeatureConfigurator.New(ReachSteed, ReachSteedGuid)
              .SetDisplayName(ReachSteedDisplayName)
              .SetDescription(ReachSteedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat5, PetType.AnimalCompanion)
              .Configure();
        }

        private const string CombinedMight = "MammothRider.CombinedMight";
        public static readonly string CombinedMightGuid = "{A620DA1D-8224-458B-A748-11CFE7729782}";

        private const string CombinedMightBuff = "MammothRider.CombinedMightBuff";
        private static readonly string CombinedMightBuffGuid = "{49F7F0A5-B8E5-4C21-AFB7-E2E509D6C34E}";

        internal const string CombinedMightDisplayName = "MammothRiderCombinedMight.Name";
        private const string CombinedMightDescription = "MammothRiderCombinedMight.Description";
        public static BlueprintFeature CombinedMightFeat()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            BuffConfigurator.New(CombinedMightBuff, CombinedMightBuffGuid)
              .SetDisplayName(CombinedMightDisplayName)
              .SetDescription(CombinedMightDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            return FeatureConfigurator.New(CombinedMight, CombinedMightGuid)
              .SetDisplayName(CombinedMightDisplayName)
              .SetDescription(CombinedMightDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddComponent<RiderCombinedMight>()
              .Configure();
        }

        private const string PulverizingAssault = "MammothRider.PulverizingAssault";
        private static readonly string PulverizingAssaultGuid = "{248EB49D-DDB4-4846-995F-2C2947163CB6}";

        private const string PulverizingAssault2 = "MammothRider.PulverizingAssault2";
        private static readonly string PulverizingAssault2Guid = "{D2E3342A-F5CD-4CCA-AA57-BDF25D7C8C76}";

        private const string PulverizingAssaultBuff = "MammothRider.PulverizingAssaultBuff";
        private static readonly string PulverizingAssaultBuffGuid = "{7EA9F7F5-408B-48BD-B7F6-44AA5B56CEDA}";

        internal const string PulverizingAssaultDisplayName = "MammothRiderPulverizingAssault.Name";
        private const string PulverizingAssaultDescription = "MammothRiderPulverizingAssault.Description";
        public static BlueprintFeature PulverizingAssaultFeat()
        {
            var icon = FeatureRefs.MountedCombat.Reference.Get().Icon;

            var bam = ActionsBuilder.New()
                .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(), useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                    .ApplyBuff(BuffRefs.Staggered.ToString(), durationValue: ContextDuration.VariableDice(DiceType.D4, 1))
                    .Build()).Build())
                .RemoveBuff(PulverizingAssaultBuffGuid, toCaster: true)
                .Build();

            var buff = BuffConfigurator.New(PulverizingAssaultBuff, PulverizingAssaultBuffGuid)
              .SetDisplayName(PulverizingAssaultDisplayName)
              .SetDescription(PulverizingAssaultDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              //.AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Strength).WithBonusValueProgression(20, false))
              //.AddInitiatorAttackWithWeaponTrigger(bam, onCharge: true, onlyHit: true)
              .Configure();

            var feat = FeatureConfigurator.New(PulverizingAssault2, PulverizingAssault2Guid)
              .SetDisplayName(PulverizingAssaultDisplayName)
              .SetDescription(PulverizingAssaultDescription)
              .SetIcon(icon)
              .AddComponent<RiderPulverizingAssault>()
              .Configure();

            return FeatureConfigurator.New(PulverizingAssault, PulverizingAssaultGuid)
              .SetDisplayName(PulverizingAssaultDisplayName)
              .SetDescription(PulverizingAssaultDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              //.AddFeatureToPet(GiganticSteed3Guid)
              .Configure();
        }

        private const string MammothLord = "MammothRider.MammothLord";
        private static readonly string MammothLordGuid = "{9D9ECF52-56BD-43DE-969E-A3D33F3AE6B7}";

        private const string MammothLord2 = "MammothRider.MammothLord2";
        private static readonly string MammothLord2Guid = "{A0B7A217-C1B0-4C12-88E5-CD745233E453}";

        private const string MammothLordBuff = "MammothRider.MammothLordBuff";
        private static readonly string MammothLordBuffGuid = "{9FC08BA6-263E-4E95-96C2-55E3835314DA}";

        private const string MammothLordBuff1 = "MammothRider.MammothLordBuff1";
        private static readonly string MammothLordBuff1Guid = "{ED201D33-F183-47B5-A43F-4C10D4260948}";

        private const string MammothLordBuff2 = "MammothRider.MammothLordBuff2";
        private static readonly string MammothLordBuff2Guid = "{ECB56394-C6DA-4EFB-AE8C-93B058632DAB}";

        internal const string MammothLordDisplayName = "MammothRiderMammothLord.Name";
        private const string MammothLordDescription = "MammothRiderMammothLord.Description";
        public static BlueprintFeature MammothLordFeat()
        {
            var icon = FeatureRefs.MightyRage.Reference.Get().Icon;

            var buff = BuffConfigurator.New(MammothLordBuff, MammothLordBuffGuid)
              .SetDisplayName(MammothLordDisplayName)
              .SetDescription(MammothLordDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var buff1 = BuffConfigurator.New(MammothLordBuff1, MammothLordBuff1Guid)
              .SetDisplayName(MammothLordDisplayName)
              .SetDescription(MammothLordDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var buff2 = BuffConfigurator.New(MammothLordBuff2, MammothLordBuff2Guid)
              .SetDisplayName(MammothLordDisplayName)
              .SetDescription(MammothLordDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var feat = FeatureConfigurator.New(MammothLord2, MammothLord2Guid)
              .SetDisplayName(MammothLordDisplayName)
              .SetDescription(MammothLordDescription)
              .SetIcon(icon)
              .AddComponent<RiderMammothLord>()
              .Configure();

            return FeatureConfigurator.New(MammothLord, MammothLordGuid)
              .SetDisplayName(MammothLordDisplayName)
              .SetDescription(MammothLordDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddConditionImmunity(UnitCondition.Dazed)
              .AddConditionImmunity(UnitCondition.Fatigued)
              .AddConditionImmunity(UnitCondition.Shaken)
              .AddConditionImmunity(UnitCondition.Sickened)
              .AddConditionImmunity(UnitCondition.Staggered)
              .AddConditionImmunity(UnitCondition.Stunned)
              .AddEvasion(SavingThrowType.Fortitude)
              .AddEvasion(SavingThrowType.Will)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .AddComponent<RiderMammothLord>()
              .Configure();
        }
    }
}
