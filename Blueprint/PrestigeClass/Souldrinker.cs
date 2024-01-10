using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Prerequisites;
using PrestigePlus.Blueprint.Feat;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.CustomComponent.PrestigeClass;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.CustomAction.ClassRelated;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class Souldrinker
    {
        private const string ArchetypeName = "Souldrinker";
        public static readonly string ArchetypeGuid = "{DD6B63C0-3EFE-4761-A845-39FC2174C00F}";
        internal const string ArchetypeDisplayName = "Souldrinker.Name";
        private const string ArchetypeDescription = "Souldrinker.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "SouldrinkerPrestige";
        private static readonly string ClassProgressGuid = "{50ABFB30-EAAB-449E-A0F5-F0815C3FCE50}";

        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateProficiencies(), Sentinel.BonusFeatGuid, spellupgradeGuid)
                .AddToLevelEntry(2, CreateEnergyDrain())
                .AddToLevelEntry(3, Sentinel.DivineBoon1Guid)
                .AddToLevelEntry(4)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, Sentinel.DivineBoon2Guid)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8, CacodaemonFeature())
                .AddToLevelEntry(9, Sentinel.DivineBoon3Guid)
                .AddToLevelEntry(10)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { CacodaemonGuid }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
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
                .SetHitDie(DiceType.D6)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABLow)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .AddSkipLevelsForSpellProgression(new int[] { 6 })
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteFeature(DeificObedience.CharonGuid, group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(DeificObedience.SzurielGuid, group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.GreatFortitude.ToString())
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 7)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 7)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 7)
                .AddComponent<PrerequisiteCasterLevel>(c => { c.RequiredCasterLevel = 2; })
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Souldrinkerref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Souldrinkerref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Proficiencies = "Souldrinker.Proficiencies";
        private static readonly string ProficienciesGuid = "{C722B77C-84F8-48D7-8F02-F8E44452F76A}";
        internal const string ProficienciesDisplayName = "SouldrinkerProficiencies.Name";
        private const string ProficienciesDescription = "SouldrinkerProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent<AddDeityWeaponPro>()
              .Configure();
        }

        private const string Cacodaemon = "Cacodaemon";
        private static readonly string CacodaemonGuid = "{DDFD1C42-FEA4-44A4-8DD1-7937291CFC98}";

        internal const string CacodaemonDisplayName = "Cacodaemon.Name";
        private const string CacodaemonDescription = "Cacodaemon.Description";
        public static BlueprintFeature CacodaemonFeature()
        {
            var icon = AbilityRefs.SummonMonsterIVd3.Reference.Get().Icon;
            return FeatureConfigurator.New(Cacodaemon, CacodaemonGuid)
              .SetDisplayName(CacodaemonDisplayName)
              .SetDescription(CacodaemonDescription)
              .SetIcon(icon)

              .Configure();
        }

        private const string EnergyDrain = "Souldrinker.EnergyDrain";
        private static readonly string EnergyDrainGuid = "{F3F672DD-B795-4D29-8AED-9E3AEF3F9C4B}";
        internal const string EnergyDrainDisplayName = "SouldrinkerEnergyDrain.Name";
        private const string EnergyDrainDescription = "SouldrinkerEnergyDrain.Description";

        private const string EnergyDrainAblity = "Souldrinker.UseEnergyDrain";
        private static readonly string EnergyDrainAblityGuid = "{DF19BAFE-E415-4AB0-B541-53DA08AA7AD2}";

        private const string EnergyDrainBuff = "Souldrinker.EnergyDrainBuff";
        public static readonly string EnergyDrainBuffGuid = "{8DA300BB-107D-45B6-9EC9-D6C854110240}";

        private const string EnergyDrainBuff2 = "Souldrinker.EnergyDrainBuff2";
        public static readonly string EnergyDrainBuff2Guid = "{09C06EEA-288A-4E5D-BD44-E205D6FB3B36}";

        private const string EnergyDrainBuff3 = "Souldrinker.EnergyDrainBuff3";
        public static readonly string EnergyDrainBuff3Guid = "{6E867862-CDCF-4E10-943E-065AFAF23186}";

        private const string EnergyDrainBuff4 = "Souldrinker.EnergyDrainBuff4";
        public static readonly string EnergyDrainBuff4Guid = "{39C536E3-D824-4DAB-B775-66AC78E5E86C}";

        private static BlueprintFeature CreateEnergyDrain()
        {
            var icon = AbilityRefs.BloodlineUndeadGraspOfTheDeadAbility.Reference.Get().Icon;

            BuffConfigurator.New(EnergyDrainBuff, EnergyDrainBuffGuid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: 5)
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff2, EnergyDrainBuff2Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: 10)
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff3, EnergyDrainBuff3Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: 15)
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff4, EnergyDrainBuff4Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: 20)
             .Configure();

            var ability = AbilityConfigurator.New(EnergyDrainAblity, EnergyDrainAblityGuid)
                .CopyFrom(
                AbilityRefs.WarpriestFervorNegativeAbility,
                typeof(AbilityDeliverTouch))
                .SetDisplayName(EnergyDrainDisplayName)
                .SetDescription(EnergyDrainDescription)
                .SetIcon(icon)
                .AllowTargeting(false, true, false, false)
                .AddAbilityTargetHasFact(new() { FeatureRefs.ConstructType.ToString(), FeatureRefs.UndeadType.ToString() }, inverted: true)
                .SetSpellDescriptor(Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.NegativeLevel)
                .SetType(AbilityType.Special)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .DealDamageTemporaryNegativeLevels(ContextDuration.Fixed(1, DurationRate.Days), ContextDice.Value(DiceType.Zero, 0, ContextValues.Rank()), setFactAsReason: true)
                    .Add<EnergyDrainFalseLife>()
                    .Build())
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((4, 1), (20, 2)))
                .Configure();

            return FeatureConfigurator.New(EnergyDrain, EnergyDrainGuid)
              .SetDisplayName(EnergyDrainDisplayName)
              .SetDescription(EnergyDrainDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
