using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Blueprints.ModReferences;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using System.Windows.Markup;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.RogueTalent;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.Modify;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class Hinterlander
    {
        private const string ArchetypeName = "Hinterlander";
        public static readonly string ArchetypeGuid = "{E6A41412-A7C7-4C9F-AB77-EA73835691A0}";
        internal const string ArchetypeDisplayName = "Hinterlander.Name";
        private const string ArchetypeDescription = "Hinterlander.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "HinterlanderPrestige";
        private static readonly string ClassProgressGuid = "{1146F84E-E765-43C4-9606-939AB6B46414}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateProficiencies(), SpellbookReplace.spellupgradeGuid, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), ImprovedFavorFeature(), ExtraFeat())
                .AddToLevelEntry(2, FeatureRefs.FastMovement.ToString(), FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
                .AddToLevelEntry(3, FeatureRefs.HunterWoodlandStride.ToString(), ExtraFeat2())
                .AddToLevelEntry(4)
                .AddToLevelEntry(5, ExtraFeat3())
                .AddToLevelEntry(6, FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
                .AddToLevelEntry(7, ImbueArrow.FeatGuid, FeatureRefs.ReachSpellFeat.ToString())
                .AddToLevelEntry(8, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), ImprovedFavorGuid, FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
                .AddToLevelEntry(9, FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .AddToLevelEntry(10, FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureSelectionRefs.FavoriteTerrainSelection.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { HinterlanderGuidFeat, HinterlanderGuidFeat2, HinterlanderGuidFeat3 }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { ImprovedFavorGuid, FeatureSelectionRefs.FavoriteEnemyRankUp.ToString() }))
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
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillStealth })
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteFeature(FeatureRefs.Endurance.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Longbow, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddComponent<PrerequisiteCasterLevel>(c => { c.RequiredCasterLevel = 1; })
                .AddPrerequisiteAlignment(AlignmentMaskType.Good, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string Proficiencies = "Hinterlander.Proficiencies";
        private static readonly string ProficienciesGuid = "{B4719B7D-30FB-47B5-B4F2-C4422171349B}";
        internal const string ProficienciesDisplayName = "HinterlanderProficiencies.Name";
        private const string ProficienciesDescription = "HinterlanderProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            var assProficiencies = FeatureRefs.OracleProficiency.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(assProficiencies.GetComponent<AddFacts>())
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.Longbow,
                  })
              .Configure();
        }

        private const string HinterlanderFeat = "HinterlanderFeat";
        private static readonly string HinterlanderGuidFeat = "{29BA69EA-1F01-42AE-B889-96D0CC6C8FCA}";

        internal const string ShadowFeatDisplayName = "HinterlanderFeat.Name";
        private const string ShadowFeatDescription = "HinterlanderFeat.Description";
        public static BlueprintFeatureSelection ExtraFeat()
        {
            return FeatureSelectionConfigurator.New(HinterlanderFeat, HinterlanderGuidFeat)
              .SetDisplayName(ShadowFeatDisplayName)
              .SetDescription(ShadowFeatDescription)
              .SetIgnorePrerequisites(true)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(FeatureRefs.RapidShotFeature.ToString())
              .AddToAllFeatures(FeatureRefs.FocusedShot.ToString())
              .AddToAllFeatures("53e8dde5-8986-4f6b-abc5-c7e5e39b1b9d") ///Deadeye’s Blessing from ttt
              .Configure(delayed: true);
        }

        private const string HinterlanderFeat2 = "HinterlanderFeat2";
        private static readonly string HinterlanderGuidFeat2 = "{E15B8684-4950-4AFC-876A-586B84A86461}";

        public static BlueprintFeatureSelection ExtraFeat2()
        {
            return FeatureSelectionConfigurator.New(HinterlanderFeat2, HinterlanderGuidFeat2)
              .SetDisplayName(ShadowFeatDisplayName)
              .SetDescription(ShadowFeatDescription)
              .SetIgnorePrerequisites(true)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(FeatureRefs.RapidShotFeature.ToString())
              .AddToAllFeatures(FeatureRefs.FocusedShot.ToString())
              .AddToAllFeatures("53e8dde5-8986-4f6b-abc5-c7e5e39b1b9d") ///Deadeye’s Blessing from ttt
              .AddToAllFeatures(FeatureRefs.ImprovedPreciseShot.ToString())
              .AddToAllFeatures(FeatureSelectionRefs.ZenArcherPointBlankMasterSelection.ToString())
              .AddToAllFeatures(FeatureRefs.Manyshot.ToString())
              .Configure(delayed: true);
        }

        private const string HinterlanderFeat3 = "HinterlanderFeat3";
        private static readonly string HinterlanderGuidFeat3 = "{DAD88C3A-E3C2-4CC1-AC5B-8D854334628B}";

        public static BlueprintFeatureSelection ExtraFeat3()
        {
            return FeatureSelectionConfigurator.New(HinterlanderFeat3, HinterlanderGuidFeat3)
              .SetDisplayName(ShadowFeatDisplayName)
              .SetDescription(ShadowFeatDescription)
              .SetIgnorePrerequisites(true)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(FeatureRefs.RapidShotFeature.ToString())
              .AddToAllFeatures(FeatureRefs.FocusedShot.ToString())
              .AddToAllFeatures("53e8dde5-8986-4f6b-abc5-c7e5e39b1b9d") ///Deadeye’s Blessing from ttt
              .AddToAllFeatures(FeatureRefs.ImprovedPreciseShot.ToString())
              .AddToAllFeatures(FeatureSelectionRefs.ZenArcherPointBlankMasterSelection.ToString())
              .AddToAllFeatures(FeatureRefs.Manyshot.ToString())
              .AddToAllFeatures(FeatureRefs.Mobility.ToString())
              .AddToAllFeatures("583da9f2-3981-471a-bd45-6ccc2be0b3a6") ///Pinpoint Targeting from expanded content
              .Configure(delayed: true);
        }

        private const string ImprovedFavor = "ImprovedFavor";
        private static readonly string ImprovedFavorGuid = "{2E70D3E4-567B-4E52-ACBA-F7AF43FADC2D}";
        internal const string ImprovedFavorDisplayName = "ImprovedFavor.Name";
        private const string ImprovedFavorDescription = "ImprovedFavor.Description";
        public static BlueprintFeatureSelection ImprovedFavorFeature()
        {
            var icon = FeatureRefs.RangerFavoredEnemy.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ImprovedFavor, ImprovedFavorGuid)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(ImprovedFavorFeature1())
              .AddToAllFeatures(ImprovedFavorFeature2())
              .AddToAllFeatures(ImprovedFavorFeature3())
              .AddToAllFeatures(ImprovedFavorFeature4())
              .AddToAllFeatures(ImprovedFavorFeature5())
              .Configure();
        }

        private const string ImprovedFavor1 = "ImprovedFavor1";
        private static readonly string ImprovedFavorGuid1 = "{E397E70C-5BF4-4B42-8461-1DB318AB7D64}";
        public static BlueprintFeature ImprovedFavorFeature1()
        {
            var icon = FeatureRefs.FavoriteEnemyAberrations.Reference.Get().Icon;
            return FeatureConfigurator.New(ImprovedFavor1, ImprovedFavorGuid1)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyAberrations.ToString())
              .AddSavingThrowBonusAgainstFact(checkedFact: FeatureRefs.AberrationType.ToString(), value: 2)
              .Configure();
        }

        private const string ImprovedFavor2 = "ImprovedFavor2";
        private static readonly string ImprovedFavorGuid2 = "{D3475BF1-1695-4ED9-9F63-9492D87A9323}";
        public static BlueprintFeature ImprovedFavorFeature2()
        {
            var icon = FeatureRefs.FavoriteEnemyConstructs.Reference.Get().Icon;
            return FeatureConfigurator.New(ImprovedFavor2, ImprovedFavorGuid2)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyConstructs.ToString())
              .AddPartialDRIgnore(checkWeaponCategories: true,
                                    reductionReduction: 10, useContextValue: false, weaponCategories: new WeaponCategory[]
                  {
              WeaponCategory.Longbow,
              WeaponCategory.Shortbow,
              WeaponCategory.Dart,
              WeaponCategory.LightCrossbow,
              WeaponCategory.HeavyCrossbow,
              WeaponCategory.Javelin,
              WeaponCategory.Sling,
              WeaponCategory.Shuriken,
              WeaponCategory.Ray,
              WeaponCategory.KineticBlast,
              WeaponCategory.Bomb,
              WeaponCategory.ThrowingAxe,
              WeaponCategory.SlingStaff
                  })
              .Configure();
        }

        private const string ImprovedFavor3 = "ImprovedFavor3";
        private static readonly string ImprovedFavorGuid3 = "{6E0F19EB-D656-4985-AAA0-247E74EB09AB}";
        public static BlueprintFeature ImprovedFavorFeature3()
        {
            var icon = FeatureRefs.FavoriteEnemyOutsider.Reference.Get().Icon;
            return FeatureConfigurator.New(ImprovedFavor3, ImprovedFavorGuid3)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyOutsider.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfMagic.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfStrength.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyDemonOfSlaughter.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GoodAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GoodAligned.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .Configure();
        }

        private const string ImprovedFavor4 = "ImprovedFavor4";
        private static readonly string ImprovedFavorGuid4 = "{1F915492-7A7C-4C71-8B4D-FAE8E116F58E}";
        public static BlueprintFeature ImprovedFavorFeature4()
        {
            var icon = FeatureRefs.FavoriteEnemyMonstrousHumanoid.Reference.Get().Icon;
            return FeatureConfigurator.New(ImprovedFavor4, ImprovedFavorGuid4)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyMonstrousHumanoid.ToString())
              .AddSavingThrowBonusAgainstFact(checkedFact: FeatureRefs.MonstrousHumanoidType.ToString(), value: 2)
              .Configure();
        }

        private const string ImprovedFavor5 = "ImprovedFavor5";
        private static readonly string ImprovedFavorGuid5 = "{60245588-125A-4F4C-909D-0929EDC0D346}";
        public static BlueprintFeature ImprovedFavorFeature5()
        {
            var icon = FeatureRefs.FavoriteEnemyUndead.Reference.Get().Icon;
            return FeatureConfigurator.New(ImprovedFavor5, ImprovedFavorGuid5)
              .SetDisplayName(ImprovedFavorDisplayName)
              .SetDescription(ImprovedFavorDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.FavoriteEnemyUndead.ToString())
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GhostTouch.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.GhostTouch.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .Configure();
        }
    }
}
