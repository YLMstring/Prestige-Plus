using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Blueprint.SpecificManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class SanguineAngel
    {
        private const string ArchetypeName = "SanguineAngel";
        public static readonly string ArchetypeGuid = "{EBA75E50-F41C-48E5-852D-9575FB2C7DEF}";
        internal const string ArchetypeDisplayName = "SanguineAngel.Name";
        private const string ArchetypeDescription = "SanguineAngel.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "SanguineAngelPrestige";
        private static readonly string ClassProgressGuid = "{AF8E64F2-D1DC-4135-9A02-56AF03B5C404}";

        private static readonly string ProdigiousTwoWeaponFighting = "5725b7af6cd64bb8b53d6535f9de0161"; //from dark codex

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, SAHardenedHeart(), SAMaidenShield())
                .AddToLevelEntry(2, SATyrantDiscipline())
                .AddToLevelEntry(3, SAArmoredAngel(), FeatureRefs.ArmorTraining.ToString())
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(2)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteClassLevel(ArchetypeGuid, 3, not: true)
                .AddPrerequisiteProficiency(weaponProficiencies: new WeaponCategory[] { }, armorProficiencies: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy })
                .AddPrerequisiteFeature(FeatureRefs.ShieldBashFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Longsword, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulEvil, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var SanguineAngelref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = SanguineAngelref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string HardenedHeart = "SanguineAngel.HardenedHeart";
        private static readonly string HardenedHeartGuid = "{860526EC-DE4A-43C1-84B3-187B9B210E83}";

        internal const string SanguineAngelHardenedHeartDisplayName = "SanguineAngelHardenedHeart.Name";
        private const string SanguineAngelHardenedHeartDescription = "SanguineAngelHardenedHeart.Description";
        public static BlueprintFeature SAHardenedHeart()
        {
            var icon = FeatureRefs.IntimidatingProwess.Reference.Get().Icon;
            return FeatureConfigurator.New(HardenedHeart, HardenedHeartGuid)
              .SetDisplayName(SanguineAngelHardenedHeartDisplayName)
              .SetDescription(SanguineAngelHardenedHeartDescription)
              .SetIcon(icon)
              .AddContextStatBonus(stat: StatType.CheckIntimidate, value: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithCustomProgression((1, 2), (2, 4), (3, 6)))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string MaidenShield = "SanguineAngel.MaidenShield";
        private static readonly string MaidenShieldGuid = "{3858FCE1-EEEC-4D6D-B545-15459E154E97}";

        internal const string SanguineAngelMaidenShieldDisplayName = "SanguineAngelMaidenShield.Name";
        private const string SanguineAngelMaidenShieldDescription = "SanguineAngelMaidenShield.Description";
        public static BlueprintFeature SAMaidenShield()
        {
            var icon = FeatureRefs.ShieldBashFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(MaidenShield, MaidenShieldGuid)
              .SetDisplayName(SanguineAngelMaidenShieldDisplayName)
              .SetDescription(SanguineAngelMaidenShieldDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.TwoWeaponFighting.ToString(), ProdigiousTwoWeaponFighting })
              .Configure(delayed: true);
        }

        private const string FuriousHuntress = "SanguineAngel.FuriousHuntress";
        private static readonly string FuriousHuntressGuid = "{523BF46F-4E63-406C-A3B3-3B1E60287EFE}";

        internal const string SanguineAngelFuriousHuntressDisplayName = "SanguineAngelFuriousHuntress.Name";
        private const string SanguineAngelFuriousHuntressDescription = "SanguineAngelFuriousHuntress.Description";
        public static BlueprintFeature SAFuriousHuntress()
        {
            var icon = FeatureRefs.DeadlyAimFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(FuriousHuntress, FuriousHuntressGuid)
              .SetDisplayName(SanguineAngelFuriousHuntressDisplayName)
              .SetDescription(SanguineAngelFuriousHuntressDescription)
              .SetIcon(icon)
              .AddComponent<AttackStatReplacement>(c =>
              {
                  c.ReplacementStat = StatType.Strength; c.SubCategory = WeaponSubCategory.Ranged; c.CheckWeaponTypes = true;
                  c.m_WeaponTypes = new BlueprintWeaponTypeReference[] { WeaponTypeRefs.Longbow.Reference.Get().ToReference<BlueprintWeaponTypeReference>(), WeaponTypeRefs.Shortbow.Reference.Get().ToReference<BlueprintWeaponTypeReference>(), WeaponTypeRefs.CompositeLongbow.Reference.Get().ToReference<BlueprintWeaponTypeReference>(), WeaponTypeRefs.CompositeShortbow.Reference.Get().ToReference<BlueprintWeaponTypeReference>() };
              })
              .Configure();
        }

        private const string TyrantDiscipline = "SanguineAngel.TyrantDiscipline";
        private static readonly string TyrantDisciplineGuid = "{E00CCB8E-3C5F-4AB3-96DA-BB34624A7355}";

        internal const string SanguineAngelTyrantDisciplineDisplayName = "SanguineAngelTyrantDiscipline.Name";
        private const string SanguineAngelTyrantDisciplineDescription = "SanguineAngelTyrantDiscipline.Description";
        public static BlueprintFeatureSelection SATyrantDiscipline()
        {
            var feat = FeatureSelectionConfigurator.New(TyrantDiscipline, TyrantDisciplineGuid)
              .SetDisplayName(SanguineAngelTyrantDisciplineDisplayName)
              .SetDescription(SanguineAngelTyrantDisciplineDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 3)
              .AddToAllFeatures(BullRushFeats.DragGuid)
              .AddToAllFeatures(BullRushFeats.AngelGuid)
              .AddToAllFeatures(SAErinyesFury())
              .AddToAllFeatures(SAFuriousHuntress())
              .AddToAllFeatures(SAKinslayer())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();

            return feat;
        }

        private const string ArmoredAngel = "SanguineAngel.ArmoredAngel";
        private static readonly string ArmoredAngelGuid = "{48EF4C7B-EB18-42E3-A633-E8FC73A40E41}";

        internal const string SanguineAngelArmoredAngelDisplayName = "SanguineAngelArmoredAngel.Name";
        private const string SanguineAngelArmoredAngelDescription = "SanguineAngelArmoredAngel.Description";

        //private const string ArmorTrainingSelection = "354f1a44-26d2-4ea3-8718-905108f48e72"; //from ttt
        public static BlueprintFeatureSelection SAArmoredAngel()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ArmoredAngel, ArmoredAngelGuid)
              .SetDisplayName(SanguineAngelArmoredAngelDisplayName)
              .SetDescription(SanguineAngelArmoredAngelDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              //.AddFacts(new() { ArmorTrainingSelection })
              .AddToAllFeatures(SAUnyielding())
              .AddToAllFeatures(SAAngelDeath())
              .Configure(delayed: true);
        }

        private const string Unyielding = "SanguineAngel.Unyielding";
        private static readonly string UnyieldingGuid = "{632FE815-1BFD-48F1-9240-7A0290A6E507}";

        internal const string SanguineAngelUnyieldingDisplayName = "SanguineAngelUnyielding.Name";
        private const string SanguineAngelUnyieldingDescription = "SanguineAngelUnyielding.Description";
        public static BlueprintFeature SAUnyielding()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureConfigurator.New(Unyielding, UnyieldingGuid)
              .SetDisplayName(SanguineAngelUnyieldingDisplayName)
              .SetDescription(SanguineAngelUnyieldingDescription)
              .SetIcon(icon)
              .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(1))
              .Configure();
        }

        private const string AngelDeath = "SanguineAngel.AngelDeath";
        private static readonly string AngelDeathGuid = "{65B72805-CADE-4814-B3FD-F720AA386EA2}";

        internal const string SanguineAngelAngelDeathDisplayName = "SanguineAngelAngelDeath.Name";
        private const string SanguineAngelAngelDeathDescription = "SanguineAngelAngelDeath.Description";
        public static BlueprintFeature SAAngelDeath()
        {
            var icon = FeatureRefs.HellsSealFeature.Reference.Get().Icon;
            var comp = BuffRefs.SeeInvisibilitytBuff.Reference.Get().GetComponent<AddCondition>();

            return FeatureConfigurator.New(AngelDeath, AngelDeathGuid)
              .SetDisplayName(SanguineAngelAngelDeathDisplayName)
              .SetDescription(SanguineAngelAngelDeathDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.OutsiderType.ToString(), FeatureRefs.SubtypeEvil.ToString(), FeatureRefs.SubtypeLawful.ToString(), BuffRefs.WingsAngelBlack.ToString() })
              .AddBuffMovementSpeed(value: 20)
              .AddComponent(comp)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(30), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .AddPrerequisiteFeature(FeatureRefs.PreciseShot.ToString())
              .Configure();
        }

        private const string ErinyesFury = "SanguineAngel.ErinyesFury";
        private static readonly string ErinyesFuryGuid = "{4AA5F662-A811-4BAC-B71B-FC8CA30C9A7D}";

        internal const string SanguineAngelErinyesFuryDisplayName = "SanguineAngelErinyesFury.Name";
        private const string SanguineAngelErinyesFuryDescription = "SanguineAngelErinyesFury.Description";
        public static BlueprintFeature SAErinyesFury()
        {
            var icon = FeatureRefs.FocusedRageFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(ErinyesFury, ErinyesFuryGuid)
              .SetDisplayName(SanguineAngelErinyesFuryDisplayName)
              .SetDescription(SanguineAngelErinyesFuryDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.FocusedRageFeature.ToString() })
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.FocusedRageResourse.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithCustomProgression((1, 0), (2, 2), (3, 4)))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string Kinslayer = "SanguineAngel.Kinslayer";
        private static readonly string KinslayerGuid = "{A30A37C5-EC7A-4F38-8F74-BBD6833AE317}";

        internal const string SanguineAngelKinslayerDisplayName = "SanguineAngelKinslayer.Name";
        private const string SanguineAngelKinslayerDescription = "SanguineAngelKinslayer.Description";
        public static BlueprintFeature SAKinslayer()
        {
            var icon = FeatureRefs.RangerFavoredEnemy.Reference.Get().Icon;
            return FeatureConfigurator.New(Kinslayer, KinslayerGuid)
              .SetDisplayName(SanguineAngelKinslayerDisplayName)
              .SetDescription(SanguineAngelKinslayerDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.FavoriteEnemyHuman.ToString(), FeatureRefs.FavoriteEnemyElfs.ToString(), FeatureRefs.FavoriteEnemyDwarfs.ToString(), FeatureRefs.FavoriteEnemyHalfling.ToString(), FeatureRefs.FavoriteEnemyGnome.ToString() })
              .Configure();
        }
    }
}
