using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
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
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class HorizonWalker
    {
        private const string ArchetypeName = "HorizonWalker";
        public static readonly string ArchetypeGuid = "{381A5F93-83E8-40C8-866B-ADD25318A009}";
        internal const string ArchetypeDisplayName = "HorizonWalker.Name";
        private const string ArchetypeDescription = "HorizonWalker.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "HorizonWalkerPrestige";
        private static readonly string ClassProgressGuid = "{3ED05776-45AF-42AA-8DEE-99E823310D88}";

        public static void Configure()
        {
            LikeyAstralFeat(); SATerrainDominance();

            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
                .AddToLevelEntry(2, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString(), TerrainMasteryGuid)
                .AddToLevelEntry(3, TerrainDominanceGuid)
                .AddToLevelEntry(4, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString(), TerrainMasteryGuid)
                .AddToLevelEntry(5, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .AddToLevelEntry(6, TerrainMasteryGuid, TerrainDominanceGuid)
                .AddToLevelEntry(7, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .AddToLevelEntry(8, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString(), TerrainMasteryGuid)
                .AddToLevelEntry(9, TerrainDominanceGuid)
                .AddToLevelEntry(10, FeatureSelectionRefs.FavoriteTerrainSelection.ToString(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString(), MasterLandsFeat())
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeLow)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion, StatType.SkillKnowledgeArcana, StatType.SkillLoreNature, StatType.SkillAthletics, StatType.SkillStealth })
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 6)
                .AddPrerequisiteFeature(FeatureRefs.Endurance.ToString())
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainAbyss)
              .SetRanks(20)
              .AddComponent<AbyssDominance>(a => a.m_CheckedFacts = new BlueprintUnitFactReference[] { BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatureRefs.DemonOfStrengthFeature.ToString()), BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatureRefs.DemonOfSlaughterFeature.ToString()), BlueprintTool.GetRef<BlueprintUnitFactReference>(FeatureRefs.DemonOfMagicFeature.ToString()) })
              .SetReapplyOnLevelUp(true)
              .Configure();

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainDesert)
            .SetRanks(20)
            .Configure();

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainForest)
              .SetRanks(20)
              .Configure();

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainHighlands)
              .SetRanks(20)
              .Configure();

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainUnderground)
              .SetRanks(20)
              .Configure();

            FeatureConfigurator.For(FeatureRefs.FavoriteTerrainUrban)
              .SetRanks(20)
              .Configure();
        }

        private const string MasteryDesert = "HorizonWalker.MasteryDesert";
        private static readonly string MasteryDesertGuid = "{E73AD539-A25A-4DC6-9511-DE11D3AD30E2}";

        internal const string MasteryDesertDisplayName = "HorizonWalkerMasteryDesert.Name";
        private const string MasteryDesertDescription = "HorizonWalkerMasteryDesert.Description";
        public static BlueprintFeature MasteryDesertFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainDesert.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryDesert, MasteryDesertGuid)
              .SetDisplayName(MasteryDesertDisplayName)
              .SetDescription(MasteryDesertDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainDesert.ToString())
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Exhausted)
              .Configure();
        }

        private const string DominanceDesert = "HorizonWalker.DominanceDesert";
        private static readonly string DominanceDesertGuid = "{31B235D8-B2AF-438F-91A5-325CD099FC4D}";

        internal const string DominanceDesertDisplayName = "HorizonWalkerDominanceDesert.Name";
        private const string DominanceDesertDescription = "HorizonWalkerDominanceDesert.Description";
        public static BlueprintFeature DominanceDesertFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainDesert.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceDesert, DominanceDesertGuid)
              .SetDisplayName(DominanceDesertDisplayName)
              .SetDescription(DominanceDesertDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryDesertGuid)
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Fatigued)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(10), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .Configure();
        }

        private const string MasteryForest = "HorizonWalker.MasteryForest";
        private static readonly string MasteryForestGuid = "{DF4CFEF7-A0F0-4181-A108-F3BD9725B6BA}";

        internal const string MasteryForestDisplayName = "HorizonWalkerMasteryForest.Name";
        private const string MasteryForestDescription = "HorizonWalkerMasteryForest.Description";
        public static BlueprintFeature MasteryForestFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainForest.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryForest, MasteryForestGuid)
              .SetDisplayName(MasteryForestDisplayName)
              .SetDescription(MasteryForestDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainForest.ToString())
              .AddCMDBonusAgainstManeuvers(maneuvers: new CombatManeuver[] { CombatManeuver.Grapple }, value: 4)
              .AddBuffSkillStealthBonus(ModifierDescriptor.Competence, 4)
              .Configure();
        }

        private const string DominanceForest = "HorizonWalker.DominanceForest";
        private static readonly string DominanceForestGuid = "{7B7DFF52-085D-4A97-9B9D-2D09B0CF197E}";

        internal const string DominanceForestDisplayName = "HorizonWalkerDominanceForest.Name";
        private const string DominanceForestDescription = "HorizonWalkerDominanceForest.Description";
        public static BlueprintFeature DominanceForestFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainForest.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceForest, DominanceForestGuid)
              .SetDisplayName(DominanceForestDisplayName)
              .SetDescription(DominanceForestDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryForestGuid)
              .AddBuffMovementSpeed(value: 10)
              .Configure();
        }

        private const string MasteryHighlands = "HorizonWalker.MasteryHighlands";
        private static readonly string MasteryHighlandsGuid = "{7DFB9A42-3C09-4F38-8673-12900B50573E}";

        internal const string MasteryHighlandsDisplayName = "HorizonWalkerMasteryHighlands.Name";
        private const string MasteryHighlandsDescription = "HorizonWalkerMasteryHighlands.Description";
        public static BlueprintFeature MasteryHighlandsFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainHighlands.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryHighlands, MasteryHighlandsGuid)
              .SetDisplayName(MasteryHighlandsDisplayName)
              .SetDescription(MasteryHighlandsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainHighlands.ToString())
              .AddBuffSkillBonus(StatType.SkillAthletics, 4, ModifierDescriptor.Competence)
              .Configure();
        }

        private const string DominanceHighlands = "HorizonWalker.DominanceHighlands";
        private static readonly string DominanceHighlandsGuid = "{5E882F1C-C32A-4CF9-A08C-D50645FB6BD2}";

        internal const string DominanceHighlandsDisplayName = "HorizonWalkerDominanceHighlands.Name";
        private const string DominanceHighlandsDescription = "HorizonWalkerDominanceHighlands.Description";
        public static BlueprintFeature DominanceHighlandsFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainHighlands.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceHighlands, DominanceHighlandsGuid)
              .SetDisplayName(DominanceHighlandsDisplayName)
              .SetDescription(DominanceHighlandsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryHighlandsGuid)
              .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(2), bypassedByMaterial: true, material: Kingmaker.Enums.Damage.PhysicalDamageMaterial.Adamantite)
              .Configure();
        }

        private const string MasteryUnderground = "HorizonWalker.MasteryUnderground";
        private static readonly string MasteryUndergroundGuid = "{2239473A-71FE-4019-AA6A-ABB12D966630}";

        internal const string MasteryUndergroundDisplayName = "HorizonWalkerMasteryUnderground.Name";
        private const string MasteryUndergroundDescription = "HorizonWalkerMasteryUnderground.Description";
        public static BlueprintFeature MasteryUndergroundFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainUnderground.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryUnderground, MasteryUndergroundGuid)
              .SetDisplayName(MasteryUndergroundDisplayName)
              .SetDescription(MasteryUndergroundDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainUnderground.ToString())
              .AddFacts(new() { FeatureRefs.BlindFight.ToString() })
              .Configure();
        }

        private const string DominanceUnderground = "HorizonWalker.DominanceUnderground";
        private static readonly string DominanceUndergroundGuid = "{C175910C-0BA2-4EFB-A2D9-EF1E0B7FFA69}";

        internal const string DominanceUndergroundDisplayName = "HorizonWalkerDominanceUnderground.Name";
        private const string DominanceUndergroundDescription = "HorizonWalkerDominanceUnderground.Description";
        public static BlueprintFeature DominanceUndergroundFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainUnderground.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceUnderground, DominanceUndergroundGuid)
              .SetDisplayName(DominanceUndergroundDisplayName)
              .SetDescription(DominanceUndergroundDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryUndergroundGuid)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 5)
              .AddFacts(new() { FeatureRefs.Tremorsense.ToString() })
              .Configure();
        }

        private const string MasteryUrban = "HorizonWalker.MasteryUrban";
        private static readonly string MasteryUrbanGuid = "{8CD55127-1655-4741-AB1B-16E91070F38E}";

        internal const string MasteryUrbanDisplayName = "HorizonWalkerMasteryUrban.Name";
        private const string MasteryUrbanDescription = "HorizonWalkerMasteryUrban.Description";
        public static BlueprintFeature MasteryUrbanFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainUrban.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryUrban, MasteryUrbanGuid)
              .SetDisplayName(MasteryUrbanDisplayName)
              .SetDescription(MasteryUrbanDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainUrban.ToString())
              .AddBuffSkillBonus(StatType.SkillPersuasion, 4, ModifierDescriptor.Competence)
              .Configure();
        }

        private const string DominanceUrban = "HorizonWalker.DominanceUrban";
        private static readonly string DominanceUrbanGuid = "{DA2B733E-726B-49C5-BB6C-0E3D66F76FDA}";

        internal const string DominanceUrbanDisplayName = "HorizonWalkerDominanceUrban.Name";
        private const string DominanceUrbanDescription = "HorizonWalkerDominanceUrban.Description";
        public static BlueprintFeature DominanceUrbanFeat()
        {
            var icon = FeatureRefs.FavoriteTerrainUrban.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceUrban, DominanceUrbanGuid)
              .SetDisplayName(DominanceUrbanDisplayName)
              .SetDescription(DominanceUrbanDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryUrbanGuid)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.LawfulAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.LawfulAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.LawfulAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.AdditionalLimb)
              .AddFacts(new() { FeatureRefs.FavoriteEnemyHuman.ToString() })
              .Configure();
        }

        private const string LikeyAstral = "HorizonWalker.LikeyAstral";
        private static readonly string LikeyAstralGuid = "{C2DA1F8D-A22F-4CF1-A9DA-A66452EE4209}";

        internal const string LikeyAstralDisplayName = "HorizonWalkerLikeyAstral.Name";
        private const string LikeyAstralDescription = "HorizonWalkerLikeyAstral.Description";
        public static BlueprintFeature LikeyAstralFeat()
        {
            var icon = FeatureRefs.KiAbudantStepFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(LikeyAstral, LikeyAstralGuid)
              .SetDisplayName(LikeyAstralDisplayName)
              .SetDescription(LikeyAstralDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetRanks(4)
              .AddToFeatureSelection(FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
              .AddToFeatureSelection(FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
              .Configure();
        }

        private const string MasteryAstral = "HorizonWalker.MasteryAstral";
        private static readonly string MasteryAstralGuid = "{8A66D765-0F11-4D54-A275-4580F47DA329}";

        internal const string MasteryAstralDisplayName = "HorizonWalkerMasteryAstral.Name";
        private const string MasteryAstralDescription = "HorizonWalkerMasteryAstral.Description";
        public static BlueprintFeature MasteryAstralFeat()
        {
            var icon = FeatureRefs.KiAbudantStepFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(MasteryAstral, MasteryAstralGuid)
              .SetDisplayName(MasteryAstralDisplayName)
              .SetDescription(MasteryAstralDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(LikeyAstralGuid)
              .Configure();
        }

        private const string DominanceAstral = "HorizonWalker.DominanceAstral";
        public static readonly string DominanceAstralGuid = "{C0439BF8-CED9-485C-95EE-8EC1BEC14F9A}";

        internal const string DominanceAstralDisplayName = "HorizonWalkerDominanceAstral.Name";
        private const string DominanceAstralDescription = "HorizonWalkerDominanceAstral.Description";
        public static BlueprintFeature DominanceAstralFeat()
        {
            var icon = FeatureRefs.KiAbudantStepFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(DominanceAstral, DominanceAstralGuid)
              .SetDisplayName(DominanceAstralDisplayName)
              .SetDescription(DominanceAstralDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(MasteryAstralGuid)
              .AddFacts(new() { ShadowDancer.ShadowJumpAblityGuid2 })
              .AddAbilityResources(resource: ShadowDancer.ShadowJumpAblityResGuid, restoreAmount: true)
              .AddIncreaseResourceAmountBySharedValue(false, ShadowDancer.ShadowJumpAblityResGuid, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom).WithBonusValueProgression(3, false))
              .AddRecalculateOnStatChange(stat: StatType.Wisdom)
              .AddDamageBonusAgainstFactOwner(damageBonus: 1, checkedFact: FeatureRefs.OutsiderType.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddDamageBonusAgainstFactOwner(damageBonus: 1, checkedFact: FeatureRefs.DemonOfMagicFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddDamageBonusAgainstFactOwner(damageBonus: 1, checkedFact: FeatureRefs.DemonOfSlaughterFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddDamageBonusAgainstFactOwner(damageBonus: 1, checkedFact: FeatureRefs.DemonOfStrengthFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddAttackBonusAgainstFactOwner(attackBonus: 1, checkedFact: FeatureRefs.OutsiderType.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddAttackBonusAgainstFactOwner(attackBonus: 1, checkedFact: FeatureRefs.DemonOfMagicFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddAttackBonusAgainstFactOwner(attackBonus: 1, checkedFact: FeatureRefs.DemonOfSlaughterFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .AddAttackBonusAgainstFactOwner(attackBonus: 1, checkedFact: FeatureRefs.DemonOfStrengthFeature.ToString(), descriptor: ModifierDescriptor.Competence)
              .Configure();
        }

        private const string MasterLands = "HorizonWalker.MasterLands";
        private static readonly string MasterLandsGuid = "{C091B506-EF15-42B6-BF74-44D0DA67D519}";

        private const string MasterLands2 = "HorizonWalker.MasterLands2";
        private static readonly string MasterLandsGuid2 = "{B1144F81-E919-481A-B90A-CB87DC763AC2}";

        internal const string MasterLandsDisplayName = "HorizonWalkerMasterLands.Name";
        private const string MasterLandsDescription = "HorizonWalkerMasterLands.Description";
        public static BlueprintFeatureSelection MasterLandsFeat()
        {
            var icon = FeatureRefs.HideInPlainSight.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(MasterLands, MasterLandsGuid)
              .SetDisplayName(MasterLandsDisplayName)
              .SetDescription(MasterLandsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.FavoriteTerrainAbyss.ToString(), FeatureRefs.FavoriteTerrainDesert.ToString(), FeatureRefs.FavoriteTerrainForest.ToString(), FeatureRefs.FavoriteTerrainHighlands.ToString(), FeatureRefs.FavoriteTerrainUnderground.ToString(), FeatureRefs.FavoriteTerrainUrban.ToString() })
              .AddBuffAllSavesBonus(value: 1)
              .Configure();

            return FeatureSelectionConfigurator.New(MasterLands2, MasterLandsGuid2)
              .SetDisplayName(MasterLandsDisplayName)
              .SetDescription(MasterLandsDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(feat)
              .Configure();
        }

        private const string TerrainMastery = "HorizonWalker.TerrainMastery";
        private static readonly string TerrainMasteryGuid = "{D213FF17-43B5-4E3C-AD00-F77C1A9F32AE}";

        internal const string HorizonWalkerTerrainMasteryDisplayName = "HorizonWalkerTerrainMastery.Name";
        private const string HorizonWalkerTerrainMasteryDescription = "HorizonWalkerTerrainMastery.Description";
        public static BlueprintFeatureSelection SATerrainMastery()
        {
            return FeatureSelectionConfigurator.New(TerrainMastery, TerrainMasteryGuid)
              .SetDisplayName(HorizonWalkerTerrainMasteryDisplayName)
              .SetDescription(HorizonWalkerTerrainMasteryDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(AbyssMasteryFeat())
              .AddToAllFeatures(MasteryAstralFeat())
              .AddToAllFeatures(MasteryDesertFeat())
              .AddToAllFeatures(MasteryForestFeat())
              .AddToAllFeatures(MasteryHighlandsFeat())
              .AddToAllFeatures(MasteryUndergroundFeat())
              .AddToAllFeatures(MasteryUrbanFeat())
              .Configure();
        }

        private const string TerrainDominance = "HorizonWalker.TerrainDominance";
        private static readonly string TerrainDominanceGuid = "{4FA81D98-A6FF-45B6-A778-129A0B425B2A}";

        internal const string HorizonWalkerTerrainDominanceDisplayName = "HorizonWalkerTerrainDominance.Name";
        private const string HorizonWalkerTerrainDominanceDescription = "HorizonWalkerTerrainDominance.Description";
        public static BlueprintFeatureSelection SATerrainDominance()
        {
            return FeatureSelectionConfigurator.New(TerrainDominance, TerrainDominanceGuid)
              .SetDisplayName(HorizonWalkerTerrainDominanceDisplayName)
              .SetDescription(HorizonWalkerTerrainDominanceDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(AbyssDominanceFeat())
              .AddToAllFeatures(DominanceAstralFeat())
              .AddToAllFeatures(DominanceDesertFeat())
              .AddToAllFeatures(DominanceForestFeat())
              .AddToAllFeatures(DominanceHighlandsFeat())
              .AddToAllFeatures(DominanceUndergroundFeat())
              .AddToAllFeatures(DominanceUrbanFeat())
              .AddToAllFeatures(SATerrainMastery())
              .Configure();
        }

        private const string AbyssDominance = "HorizonWalker.AbyssDominance";
        private static readonly string AbyssDominanceGuid = "{59352D08-B18B-45B8-B2BB-528479DAFC8F}";

        internal const string AbyssDominanceDisplayName = "HorizonWalkerAbyssDominance.Name";
        private const string AbyssDominanceDescription = "HorizonWalkerAbyssDominance.Description";
        public static BlueprintFeature AbyssDominanceFeat()
        {
            var icon = FeatureRefs.RangerFavoredEnemy.Reference.Get().Icon;

            return FeatureConfigurator.New(AbyssDominance, AbyssDominanceGuid)
              .SetDisplayName(AbyssDominanceDisplayName)
              .SetDescription(AbyssDominanceDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(AbyssMasteryGuid)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GoodAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GoodAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GoodAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.AdditionalLimb)
              .Configure();
        }

        private const string AbyssMastery = "HorizonWalker.AbyssMastery";
        private static readonly string AbyssMasteryGuid = "{1E0D2EF1-AB61-4EC0-A8EF-26F125DF45CD}";

        internal const string AbyssMasteryDisplayName = "HorizonWalkerAbyssMastery.Name";
        private const string AbyssMasteryDescription = "HorizonWalkerAbyssMastery.Description";
        public static BlueprintFeature AbyssMasteryFeat()
        {
            var icon = FeatureRefs.RangerFavoredEnemy.Reference.Get().Icon;

            return FeatureConfigurator.New(AbyssMastery, AbyssMasteryGuid)
              .SetDisplayName(AbyssMasteryDisplayName)
              .SetDescription(AbyssMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteTerrainAbyss.ToString())
              .Configure();
        }
    }
}
