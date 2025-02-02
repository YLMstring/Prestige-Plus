﻿using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomComponent;
using BlueprintCore.Utils.Types;
using PrestigePlus.CustomComponent.PrestigeClass;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Blueprints.Classes.Prerequisites;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class Sentinel
    {
        private const string ArchetypeName = "Sentinel";
        public static readonly string ArchetypeGuid = "{D5D53228-7081-423F-BCA1-5704C7D0FD07}";
        internal const string ArchetypeDisplayName = "Sentinel.Name";
        private const string ArchetypeDescription = "Sentinel.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "SentinelPrestige";
        private static readonly string ClassProgressGuid = "{C76A9A39-7B70-4DDB-BEC4-FC4BE8AE694D}";

        public static void Configure()
        {
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, BonusFeatFeat(), FeatureRefs.RangerProficiencies.ToString(), ChooseGoodEvilFeat())
            .AddToLevelEntry(2, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(3, DivineBoon1Feat())
            .AddToLevelEntry(4, DivineQuicknessFeat())
            .AddToLevelEntry(5, StalwartFeat(), AlignedStrikeFeat())
            .AddToLevelEntry(6, DivineBoon2Feat())
            .AddToLevelEntry(7, FeatureSelectionRefs.FighterFeatSelection.ToString(), PracticedCombatantFeat())
            .AddToLevelEntry(8, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(9, DivineBoon3Feat())
            .AddToLevelEntry(10, UnstoppableWarriorFeat())
            .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FiendishStudiesGuid, FiendishStudies5Guid, FiendishStudies10Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SymbolicWeapon3Guid, SymbolicWeapon6Guid, SymbolicWeapon9Guid, SymbolicWeaponGuid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { DivineBoon1Guid, DivineBoon2Guid, DivineBoon3Guid }))
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName("")
            .SetDescription(ArchetypeDescription)
            .Configure();

            BlueprintCharacterClass archetype =
          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName(ArchetypeDisplayName)
            .SetLocalizedDescription(ArchetypeDescription)
            .SetSkillPoints(2)
            .SetHitDie(DiceType.D10)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(BABFull)
            .SetFortitudeSave(SavesPrestigeHigh)
            .SetReflexSave(SavesPrestigeLow)
            .SetWillSave(SavesPrestigeLow)
            .SetProgression(progression)
            .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPersuasion, StatType.SkillPerception })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteFeature(DeificObedience.DeificObedienceGuid)
            .AddComponent<PrerequisiteDivineWeapon>()
            .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string BonusFeat = "Sentinel.BonusFeat";
        public static readonly string BonusFeatGuid = "{283F021D-C8A3-4D2B-ACAA-9DC46E77CD8E}";

        internal const string BonusFeatDisplayName = "SentinelBonusFeat.Name";
        private const string BonusFeatDescription = "SentinelBonusFeat.Description";

        public static BlueprintFeatureSelection BonusFeatFeat()
        {
            var icon = AbilityRefs.Prayer.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(BonusFeat, BonusFeatGuid)
              .SetDisplayName(BonusFeatDisplayName)
              .SetDescription(BonusFeatDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(DeificObedience.ShelynSentinelGuid)
              .AddToAllFeatures(DeificObedience.NaderiSentinelGuid)
              .AddToAllFeatures(DeificObedience.GorumSentinelGuid)
              .AddToAllFeatures(DeificObedience.ArazniSentinelGuid)
              .AddToAllFeatures(DeificObedience.CharonSentinelGuid)
              .AddToAllFeatures(DeificObedience.SzurielSentinelGuid)
              .AddToAllFeatures(DeificObedience.IomedaeSentinelGuid)
              .AddToAllFeatures(DeificObedience.MilaniSentinelGuid)
              .AddToAllFeatures(DeificObedience.SocothbenothSentinelGuid)
              .AddToAllFeatures(DeificObedience.ChaldiraSentinelGuid)
              .AddToAllFeatures(DeificObedience.NocticulaSentinelGuid)
              .AddToAllFeatures(DeificObedience.AchaekekSentinelGuid)
              .AddToAllFeatures(DeificObedience.ZonKuthonSentinelGuid)
              .AddToAllFeatures(DeificObedience.CaydenCaileanSentinelGuid)
              .AddToAllFeatures(DeificObedience.UrgathoaSentinelGuid)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .Configure();
        }

        private const string ChooseGoodEvil = "Sentinel.ChooseGoodEvil";
        private static readonly string ChooseGoodEvilGuid = "{A38B4CD7-6F1A-4869-AEB4-96052D107405}";

        internal const string ChooseGoodEvilDisplayName = "SentinelChooseGoodEvil.Name";
        private const string ChooseGoodEvilDescription = "SentinelChooseGoodEvil.Description";

        public static BlueprintFeatureSelection ChooseGoodEvilFeat()
        {
            var icon = AbilityRefs.BlessWeapon.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChooseGoodEvil, ChooseGoodEvilGuid)
              .SetDisplayName(ChooseGoodEvilDisplayName)
              .SetDescription(ChooseGoodEvilDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(SymbolicWeaponFeat())
              .AddToAllFeatures(EvilWeaponFeat())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string SymbolicWeapon = "Sentinel.SymbolicWeapon";
        private static readonly string SymbolicWeaponGuid = "{D3F71CE4-7D23-4A1C-B4A0-40D61C60E5BB}";

        internal const string SentinelSymbolicWeaponDisplayName = "SentinelSymbolicWeapon.Name";
        private const string SentinelSymbolicWeaponDescription = "SentinelSymbolicWeapon.Description";
        public static BlueprintFeature SymbolicWeaponFeat()
        {
            var icon = AbilityRefs.BlessWeapon.Reference.Get().Icon;
            return FeatureConfigurator.New(SymbolicWeapon, SymbolicWeaponGuid)
              .SetDisplayName(SentinelSymbolicWeaponDisplayName)
              .SetDescription(SentinelSymbolicWeaponDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.EvilDomainAllowed.ToString())
              .AddComponent<SymbolicWeaponBonus>()
              .Configure();
        }

        private const string EvilWeapon = "Sentinel.EvilWeapon";
        private static readonly string EvilWeaponGuid = "{E8A7180C-511D-460D-B8A4-AA57DD334DF3}";

        internal const string SentinelEvilWeaponDisplayName = "SentinelEvilWeapon.Name";
        private const string SentinelEvilWeaponDescription = "SentinelEvilWeapon.Description";
        public static BlueprintFeature EvilWeaponFeat()
        {
            var icon = AbilityRefs.InflictLightWounds.Reference.Get().Icon;
            return FeatureConfigurator.New(EvilWeapon, EvilWeaponGuid)
              .SetDisplayName(SentinelEvilWeaponDisplayName)
              .SetDescription(SentinelEvilWeaponDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.GoodDomainAllowed.ToString())
              .AddComponent<SymbolicWeaponBonus>(c => { c.Des = Kingmaker.Enums.ModifierDescriptor.Profane; })
              .Configure();
        }

        private const string DivineQuickness = "Sentinel.DivineQuickness";
        private static readonly string DivineQuicknessGuid = "{B1524FB4-72A5-4030-B03E-6E5EC2BC1EC1}";

        internal const string SentinelDivineQuicknessDisplayName = "SentinelDivineQuickness.Name";
        private const string SentinelDivineQuicknessDescription = "SentinelDivineQuickness.Description";
        public static BlueprintFeature DivineQuicknessFeat()
        {
            var icon = FeatureRefs.Improved_Initiative.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineQuickness, DivineQuicknessGuid)
              .SetDisplayName(SentinelDivineQuicknessDisplayName)
              .SetDescription(SentinelDivineQuicknessDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(SymbolicWeaponGuid, DivineQuicknessGoodFeat())
              .AddFeatureIfHasFact(EvilWeaponGuid, DivineQuicknessEvilFeat())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string DivineQuicknessGood = "Sentinel.DivineQuicknessGood";
        private static readonly string DivineQuicknessGoodGuid = "{FC9F0348-31A8-4D28-8A55-F4BDA6639DEF}";

        internal const string SentinelDivineQuicknessGoodDisplayName = "SentinelDivineQuicknessGood.Name";
        private const string SentinelDivineQuicknessGoodDescription = "SentinelDivineQuicknessGood.Description";
        public static BlueprintFeature DivineQuicknessGoodFeat()
        {
            var icon = FeatureRefs.Improved_Initiative.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineQuicknessGood, DivineQuicknessGoodGuid)
              .SetDisplayName(SentinelDivineQuicknessGoodDisplayName)
              .SetDescription(SentinelDivineQuicknessGoodDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Initiative, ContextValues.Rank(), Kingmaker.Enums.ModifierDescriptor.Sacred)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((7, 2), (20, 4)))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string DivineQuicknessEvil = "Sentinel.DivineQuicknessEvil";
        private static readonly string DivineQuicknessEvilGuid = "{8F27AC4D-DAC5-4969-8451-718044507C1E}";

        internal const string SentinelDivineQuicknessEvilDisplayName = "SentinelDivineQuicknessEvil.Name";
        private const string SentinelDivineQuicknessEvilDescription = "SentinelDivineQuicknessEvil.Description";
        public static BlueprintFeature DivineQuicknessEvilFeat()
        {
            var icon = FeatureRefs.Improved_Initiative.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineQuicknessEvil, DivineQuicknessEvilGuid)
              .SetDisplayName(SentinelDivineQuicknessEvilDisplayName)
              .SetDescription(SentinelDivineQuicknessEvilDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Initiative, ContextValues.Rank(), Kingmaker.Enums.ModifierDescriptor.Profane)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((7, 2), (20, 4)))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string AlignedStrike = "Sentinel.AlignedStrike";
        private static readonly string AlignedStrikeGuid = "{34361C89-9763-45CF-85ED-F41490F45C8C}";

        internal const string SentinelAlignedStrikeDisplayName = "SentinelAlignedStrike.Name";
        private const string SentinelAlignedStrikeDescription = "SentinelAlignedStrike.Description";
        public static BlueprintFeature AlignedStrikeFeat()
        {
            var icon = AbilityRefs.AlignWeapon.Reference.Get().Icon;
            return FeatureConfigurator.New(AlignedStrike, AlignedStrikeGuid)
              .SetDisplayName(SentinelAlignedStrikeDisplayName)
              .SetDescription(SentinelAlignedStrikeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ArcaneArcher.EnhanceArrows3Guid })
              .Configure();
        }

        private const string Stalwart = "Sentinel.Stalwart";
        private static readonly string StalwartGuid = "{AA5AAEED-1F62-4C1C-B3A5-7FCD17135A75}";

        internal const string SentinelStalwartDisplayName = "SentinelStalwart.Name";
        private const string SentinelStalwartDescription = "SentinelStalwart.Description";
        public static BlueprintFeature StalwartFeat()
        {
            var icon = FeatureRefs.BloodFeaster.Reference.Get().Icon;
            return FeatureConfigurator.New(Stalwart, StalwartGuid)
              .SetDisplayName(SentinelStalwartDisplayName)
              .SetDescription(SentinelStalwartDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(SymbolicWeaponGuid, StalwartGoodFeat())
              .AddFeatureIfHasFact(EvilWeaponGuid, StalwartEvilFeat())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string StalwartGood = "Sentinel.StalwartGood";
        private static readonly string StalwartGoodGuid = "{99AC5A5E-51F8-45C9-BB77-EA6093B86DAF}";

        internal const string SentinelStalwartGoodDisplayName = "SentinelStalwartGood.Name";
        private const string SentinelStalwartGoodDescription = "SentinelStalwartGood.Description";
        public static BlueprintFeature StalwartGoodFeat()
        {
            var icon = FeatureRefs.BloodFeaster.Reference.Get().Icon;
            return FeatureConfigurator.New(StalwartGood, StalwartGoodGuid)
              .SetDisplayName(SentinelStalwartGoodDisplayName)
              .SetDescription(SentinelStalwartGoodDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstSpellType(false, 2, Kingmaker.Enums.ModifierDescriptor.Sacred)
              .Configure();
        }

        private const string StalwartEvil = "Sentinel.StalwartEvil";
        private static readonly string StalwartEvilGuid = "{B68F3A76-BA5C-4BF8-890D-A7EA5EF4A3B3}";

        internal const string SentinelStalwartEvilDisplayName = "SentinelStalwartEvil.Name";
        private const string SentinelStalwartEvilDescription = "SentinelStalwartEvil.Description";
        public static BlueprintFeature StalwartEvilFeat()
        {
            var icon = FeatureRefs.BloodFeaster.Reference.Get().Icon;
            return FeatureConfigurator.New(StalwartEvil, StalwartEvilGuid)
              .SetDisplayName(SentinelStalwartEvilDisplayName)
              .SetDescription(SentinelStalwartEvilDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstSpellType(false, 2, Kingmaker.Enums.ModifierDescriptor.Profane)
              .Configure();
        }

        private const string PracticedCombatant = "Sentinel.PracticedCombatant";
        private static readonly string PracticedCombatantGuid = "{33ACC856-57E9-47F4-9AEA-4FEE855521AB}";

        internal const string SentinelPracticedCombatantDisplayName = "SentinelPracticedCombatant.Name";
        private const string SentinelPracticedCombatantDescription = "SentinelPracticedCombatant.Description";
        public static BlueprintFeature PracticedCombatantFeat()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;
            return FeatureConfigurator.New(PracticedCombatant, PracticedCombatantGuid)
              .SetDisplayName(SentinelPracticedCombatantDisplayName)
              .SetDescription(SentinelPracticedCombatantDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(SymbolicWeaponGuid, PracticedCombatantGoodFeat())
              .AddFeatureIfHasFact(EvilWeaponGuid, PracticedCombatantEvilFeat())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string PracticedCombatantGood = "Sentinel.PracticedCombatantGood";
        private static readonly string PracticedCombatantGoodGuid = "{8E843BDF-A56F-492F-BDC9-C83422D0FADA}";

        internal const string SentinelPracticedCombatantGoodDisplayName = "SentinelPracticedCombatantGood.Name";
        private const string SentinelPracticedCombatantGoodDescription = "SentinelPracticedCombatantGood.Description";
        public static BlueprintFeature PracticedCombatantGoodFeat()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;
            return FeatureConfigurator.New(PracticedCombatantGood, PracticedCombatantGoodGuid)
              .SetDisplayName(SentinelPracticedCombatantGoodDisplayName)
              .SetDescription(SentinelPracticedCombatantGoodDescription)
              .SetIcon(icon)
              .AddCMBBonus(checkFact: false, descriptor: Kingmaker.Enums.ModifierDescriptor.Sacred, value: 2)
              .Configure();
        }

        private const string PracticedCombatantEvil = "Sentinel.PracticedCombatantEvil";
        private static readonly string PracticedCombatantEvilGuid = "{43D04BB6-1DE6-48C0-A6D8-E00934A5E17F}";

        internal const string SentinelPracticedCombatantEvilDisplayName = "SentinelPracticedCombatantEvil.Name";
        private const string SentinelPracticedCombatantEvilDescription = "SentinelPracticedCombatantEvil.Description";
        public static BlueprintFeature PracticedCombatantEvilFeat()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;
            return FeatureConfigurator.New(PracticedCombatantEvil, PracticedCombatantEvilGuid)
              .SetDisplayName(SentinelPracticedCombatantEvilDisplayName)
              .SetDescription(SentinelPracticedCombatantEvilDescription)
              .SetIcon(icon)
              .AddCMBBonus(checkFact: false, descriptor: Kingmaker.Enums.ModifierDescriptor.Profane, value: 2)
              .Configure();
        }

        private const string UnstoppableWarrior = "Sentinel.UnstoppableWarrior";
        public static readonly string UnstoppableWarriorGuid = "{078C0239-DBC1-4AE1-84D4-4959AF80AFE1}";

        internal const string UnstoppableWarriorDisplayName = "SentinelUnstoppableWarrior.Name";
        private const string UnstoppableWarriorDescription = "SentinelUnstoppableWarrior.Description";

        private const string UnstoppableWarriorRes = "Sentinel.UnstoppableWarriorRes";
        private static readonly string UnstoppableWarriorResGuid = "{2AFFC616-338C-463E-A74B-4C18DD2461EC}";

        private const string UnstoppableWarriorAbility = "Sentinel.UnstoppableWarriorAbility";
        private static readonly string UnstoppableWarriorAbilityGuid = "{D084A41F-F57C-4DEB-9633-774CAB259C89}";
        public static BlueprintFeature UnstoppableWarriorFeat()
        {
            var icon = AbilityRefs.CureCriticalWounds.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(UnstoppableWarriorRes, UnstoppableWarriorResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(UnstoppableWarriorAbility, UnstoppableWarriorAbilityGuid)
                .CopyFrom(
                AbilityRefs.CureCriticalWounds,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilitySpawnFx),
                typeof(ContextRankConfigs),
                typeof(AbilityTargetHasFact))
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Personal)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(UnstoppableWarrior, UnstoppableWarriorGuid)
              .SetDisplayName(UnstoppableWarriorDisplayName)
              .SetDescription(UnstoppableWarriorDescription)
              .SetIcon(icon)
              .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(5))
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { UnitFactRefs.Ferocity.ToString(), ability })
              .Configure();
        }

        private const string DivineBoon1 = "Sentinel.DivineBoon1";
        public static readonly string DivineBoon1Guid = "{1CA3A398-D861-41BB-B713-9D2D3BDD6718}";

        internal const string SentinelDivineBoon1DisplayName = "SentinelDivineBoon1.Name";
        private const string SentinelDivineBoon1Description = "SentinelDivineBoon1.Description";
        public static BlueprintFeature DivineBoon1Feat()
        {
            var icon = AbilityRefs.Virtue.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineBoon1, DivineBoon1Guid)
              .SetDisplayName(SentinelDivineBoon1DisplayName)
              .SetDescription(SentinelDivineBoon1Description)
              .SetIcon(icon)
              .AddComponent<DivineBoon>(c => { c.level = 12; })
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string DivineBoon2 = "Sentinel.DivineBoon2";
        public static readonly string DivineBoon2Guid = "{575DC4FC-3703-428A-92AE-204812921BD1}";

        internal const string SentinelDivineBoon2DisplayName = "SentinelDivineBoon2.Name";
        private const string SentinelDivineBoon2Description = "SentinelDivineBoon2.Description";
        public static BlueprintFeature DivineBoon2Feat()
        {
            var icon = AbilityRefs.Virtue.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineBoon2, DivineBoon2Guid)
              .SetDisplayName(SentinelDivineBoon2DisplayName)
              .SetDescription(SentinelDivineBoon2Description)
              .SetIcon(icon)
              .AddComponent<DivineBoon>(c => { c.level = 16; })
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string DivineBoon3 = "Sentinel.DivineBoon3";
        public static readonly string DivineBoon3Guid = "{C4E8E721-3EEB-4984-9E66-EDCDD70D403C}";

        internal const string SentinelDivineBoon3DisplayName = "SentinelDivineBoon3.Name";
        private const string SentinelDivineBoon3Description = "SentinelDivineBoon3.Description";
        public static BlueprintFeature DivineBoon3Feat()
        {
            var icon = AbilityRefs.Virtue.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineBoon3, DivineBoon3Guid)
              .SetDisplayName(SentinelDivineBoon3DisplayName)
              .SetDescription(SentinelDivineBoon3Description)
              .SetIcon(icon)
              .AddComponent<DivineBoon>(c => { c.level = 20; })
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }
    }
}
