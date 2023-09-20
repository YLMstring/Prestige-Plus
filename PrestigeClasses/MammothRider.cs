using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
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
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.PrestigeClasses
{
    internal class MammothRider
    {
        private const string ArchetypeName = "MammothRider";
        private static readonly string ArchetypeGuid = "{FBD0C6EB-0513-483B-862F-A087D32B03D1}";
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
                .AddToLevelEntry(4, MistrustFeat())
                .AddToLevelEntry(5, RuggedSteedGuid, ValiantDevotionFeat())
                .AddToLevelEntry(6, BornSurvivorGuidFeat, SummonQuarrySelection())
                .AddToLevelEntry(7, RuggedSteedGuid, ReachSteedFeat())
                .AddToLevelEntry(8)
                .AddToLevelEntry(9, RuggedSteedGuid)
                .AddToLevelEntry(10)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName(ArchetypeDisplayName)
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

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var MammothRiderref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = MammothRiderref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string GiganticSteed = "MammothRider.GiganticSteed";
        private static readonly string GiganticSteedGuid = "{4DF25CF5-DCC7-42DD-9F17-77DA1BBEA52B}";

        internal const string GiganticSteedDisplayName = "MammothRiderGiganticSteed.Name";
        private const string GiganticSteedDescription = "MammothRiderGiganticSteed.Description";

        private const string GiganticSteed1 = "MammothRider.GiganticSteed1";
        private static readonly string GiganticSteed1Guid = "{D5D06F79-F7FF-4AB5-96FE-34CE19034EF4}";

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
              .AddChangeUnitSize(type: Kingmaker.Designers.Mechanics.Buffs.ChangeUnitSize.ChangeType.Delta, size: Kingmaker.Enums.Size.Fine, sizeDelta: 1)
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
              .AddFeatureToPet(feat)
              .AddFeatureToPet(feat2)
              .AddFeatureToPet(feat3)
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

        private const string GiganticSteed4 = "MammothRider.GiganticSteed4";
        private static readonly string GiganticSteed4Guid = "{FA43A17B-68CD-4EAC-BB07-A288D1085A00}";
        public static BlueprintFeature RuggedSteedFeat()
        {
            var icon = AbilityRefs.Barkskin.Reference.Get().Icon;

            var feat4 = FeatureConfigurator.New(GiganticSteed4, GiganticSteed4Guid)
              .SetDisplayName(RuggedSteedDisplayName)
              .SetDescription(RuggedSteedDescription)
              .SetIcon(icon)
              .AddACBonusAgainstFactOwner(bonus: 1, descriptor: Kingmaker.Enums.ModifierDescriptor.NaturalArmor, noFact: true)
              .SetRanks(10)
              .Configure();

            return FeatureConfigurator.New(RuggedSteed, RuggedSteedGuid)
              .SetDisplayName(RuggedSteedDisplayName)
              .SetDescription(RuggedSteedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat4)
              .AddFeatureToPet(GiganticSteed2Guid)
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
            var icon = FeatureRefs.ShamanHexAirBarrierFeature.Reference.Get().Icon;

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
        public static BlueprintFeature ValiantDevotionFeat()
        {
            var icon = FeatureRefs.ShamanHexAirBarrierFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(ValiantDevotion, ValiantDevotionGuid)
              .SetDisplayName(ValiantDevotionDisplayName)
              .SetDescription(ValiantDevotionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Charm, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Compulsion, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(4), spellDescriptor: SpellDescriptor.Fear, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
              .AddFeatureToPet(GiganticSteed3Guid)
              .Configure();
        }

        private const string SummonBigQuarry2 = "MammothRider.SummonBigQuarry2";
        private static readonly string SummonBigQuarryGuid2 = "{072D3C34-8448-4D87-B0D7-147D8AB10CEB}";

        internal const string SummonBigQuarryDisplayName = "MammothRiderSummonBigQuarry.Name";
        private const string SummonBigQuarryDescription = "MammothRiderSummonBigQuarry.Description";

        public static BlueprintFeatureSelection SummonQuarrySelection()
        {
            var icon = FeatureRefs.ImprovedQuarry.Reference.Get().Icon;

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
        public static BlueprintFeature ReachSteedFeat()
        {
            var icon = AbilityRefs.ArmyReachingStrike.Reference.Get().Icon;

            var feat5 = FeatureConfigurator.New(GiganticSteed5, GiganticSteed5Guid)
              .SetDisplayName(ReachSteedDisplayName)
              .SetDescription(ReachSteedDescription)
              .SetIcon(icon)
              .AddStatBonus(stat: StatType.Reach, value: 5)
              .SetRanks(10)
              .Configure();

            return FeatureConfigurator.New(ReachSteed, ReachSteedGuid)
              .SetDisplayName(ReachSteedDisplayName)
              .SetDescription(ReachSteedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFeatureToPet(feat5)
              .Configure();
        }

        private const string CombinedMight = "MammothRider.CombinedMight";
        private static readonly string CombinedMightGuid = "{A620DA1D-8224-458B-A748-11CFE7729782}";

        private const string CombinedMightBuff = "MammothRider.CombinedMightBuff";
        private static readonly string CombinedMightBuffGuid = "{49F7F0A5-B8E5-4C21-AFB7-E2E509D6C34E}";

        internal const string CombinedMightDisplayName = "MammothRiderCombinedMight.Name";
        private const string CombinedMightDescription = "MammothRiderCombinedMight.Description";
        public static BlueprintFeature CombinedMightFeat()
        {
            var icon = FeatureRefs.MightyRage.Reference.Get().Icon;

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
    }
}
