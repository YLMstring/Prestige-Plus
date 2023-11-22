﻿using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Utils;
using Epic.OnlineServices;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class ConstructRider
    {
        private const string ArchetypeName = "ConstructRider";
        private static readonly string ArchetypeGuid = "{58482D82-A927-47BF-A1D8-4E0F2E95FF4C}";
        internal const string ArchetypeDisplayName = "ConstructRider.Name";
        private const string ArchetypeDescription = "ConstructRider.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.AlchemistClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetReplaceSpellbook(CreateSpellbook())
            .AddToRemoveFeatures(1, FeatureRefs.BrewPotions.ToString(), FeatureRefs.MutagenFeature.ToString())
            .AddToRemoveFeatures(4, FeatureSelectionRefs.DiscoverySelection.ToString())
            .AddToRemoveFeatures(14, FeatureRefs.PersistantMutagen.ToString())
            .AddToAddFeatures(1, SummonAnimalSelection(), CreateDiminishedExtracts())
            .AddToAddFeatures(4, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeWorld)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillUseMagicDevice)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillThievery)
              .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillMobility)
              .SetReplaceClassSkills(true)
              .Configure();
        }

        private const string DiminishedExtracts = "ConstructRider.DiminishedExtracts";
        private static readonly string DiminishedExtractsGuid = "{CA2C60AF-0960-4556-9348-5EE106601210}";

        private const string DiminishedExtracts2 = "ConstructRider.DiminishedExtracts2";
        private static readonly string DiminishedExtracts2Guid = "{0EE891E2-1703-4CD0-BA95-77CEBFBA9201}";

        internal const string DiminishedExtractsDisplayName = "ConstructRiderDiminishedExtracts.Name";
        private const string DiminishedExtractsDescription = "ConstructRiderDiminishedExtracts.Description";
        private static BlueprintFeature CreateDiminishedExtracts()
        {
            var icon = FeatureSelectionRefs.AnimalCompanionSelectionDruid.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(DiminishedExtracts2, DiminishedExtracts2Guid)
              .SetDisplayName(DiminishedExtractsDisplayName)
              .SetDescription(DiminishedExtractsDescription)
              .SetIcon(icon)
              .AddConstructHealth()
              .AddFacts(new() { FeatureRefs.ImmunityToSleep.ToString(), FeatureRefs.ImmunityToParalysis.ToString(), FeatureRefs.ImmunityToDeathEffects.ToString(), FeatureRefs.ImmunityToPoison.ToString(), BuffRefs.BleedImmunity.ToString(), FeatureRefs.ImmunityToAbilityDamage.ToString(), FeatureRefs.ImmunityToStun.ToString() })
              .Configure();

            return FeatureConfigurator.New(DiminishedExtracts, DiminishedExtractsGuid)
              .SetDisplayName(DiminishedExtractsDisplayName)
              .SetDescription(DiminishedExtractsDescription)
              .SetIcon(icon)
              .AddFeatureToPet(feat, Kingmaker.Enums.PetType.AnimalCompanion)
              .Configure();
        }

        private const string SummonBigAnimal2 = "ConstructRider.SummonBigAnimal2";
        private static readonly string SummonBigAnimalGuid2 = "{AD8FEBAE-20B0-4BB7-AC78-4156D204B301}";

        private const string ClassProgressName2 = "ConstructRiderPrestige2";
        private static readonly string ClassProgressGuid2 = "{C15E3E4A-3A5C-4752-B0D0-D0D6F4558C17}";

        internal const string CraftMountDisplayName = "ConstructRiderCraftMount.Name";
        private const string CraftMountDescription = "ConstructRiderCraftMount.Description";
        public static BlueprintFeatureSelection SummonAnimalSelection()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName2, ClassProgressGuid2)
                .CopyFrom(ProgressionRefs.CavalierMountProgression, typeof(LevelEntry), typeof(UIGroup))
                .SetClasses(ArchetypeGuid)
                .Configure();

            return FeatureSelectionConfigurator.New(SummonBigAnimal2, SummonBigAnimalGuid2)
              .CopyFrom(
                FeatureSelectionRefs.CavalierMountSelection)
              .SetDisplayName(CraftMountDisplayName)
              .SetDescription(CraftMountDescription)
              .AddFeatureOnApply(progression)
              .AddFeatureOnApply(FeatureRefs.AnimalCompanionRank.ToString())
              .AddFeatureOnApply(FeatureRefs.MountTargetFeature.ToString())
              .AddFeatureOnApply(FeatureSelectionRefs.AnimalCompanionArchetypeSelection.ToString())
              .Configure(delayed: true);
        }

        private const string SpellBook = "ConstructRider.SpellBook";
        private static readonly string SpellBookGuid = "{32664180-11A8-4628-BE3B-CE164EB6E54F}";
        private static BlueprintSpellbook CreateSpellbook()
        {
            var spellbook = SpellbookConfigurator.New(SpellBook, SpellBookGuid)
              .SetName(ArchetypeDisplayName)
              .SetSpellsPerDay(GetSpellSlots())
              .SetSpellsKnown(SpellsTableRefs.AlchemistSpellLevels.ToString())
              .SetSpellList(SpellListRefs.AlchemistSpellList.ToString())
              .SetCharacterClass(CharacterClassRefs.AlchemistClass.ToString())
              .SetCastingAttribute(StatType.Charisma)
              .SetSpontaneous(true)
              .SetIsArcane(true)
              .Configure(delayed: true);

            var Alchemist = CharacterClassRefs.AlchemistClass.ToString();

            // Update references to the Alchemist spellbook to include the new spellbook
            FeatureConfigurator.For(FeatureRefs.DLC3_ArcaneModFeature)
              .EditComponent<AddAbilityUseTrigger>(
                c =>
                  c.m_Spellbooks = CommonTool.Append(c.m_Spellbooks, spellbook.ToReference<BlueprintSpellbookReference>()))
              .Configure();

            return spellbook;
        }

        // Generates the diminished spell slots
        private const string SpellTable = "ConstructRider.SpellTable";
        private static readonly string SpellTableGuid = "{03768157-2848-49F8-960B-30B33A48D1BB}";
        private static BlueprintSpellsTable GetSpellSlots()
        {
            var AlchemistSpellSlots = SpellsTableRefs.AlchemistSpellLevels.Reference.Get();
            var levelEntries =
              AlchemistSpellSlots.Levels.Select(
                l =>
                {
                    var count = l.Count.Select(c => Math.Max(0, c - 1)).ToArray();
                    return new SpellsLevelEntry { Count = count };
                });
            return SpellsTableConfigurator.New(SpellTable, SpellTableGuid)
              .SetLevels(levelEntries.ToArray())
              .Configure();
        }
    }
}