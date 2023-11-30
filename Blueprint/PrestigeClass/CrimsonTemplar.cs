using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class CrimsonTemplar
    {
        private const string ArchetypeName = "CrimsonTemplar";
        public static readonly string ArchetypeGuid = "{0FCFB57B-28C0-4DFE-AE60-7B6B8CDD3C68}";
        internal const string ArchetypeDisplayName = "CrimsonTemplar.Name";
        private const string ArchetypeDescription = "CrimsonTemplar.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "CrimsonTemplarPrestige";
        private static readonly string ClassProgressGuid = "{CAB1654D-9F7E-4BC4-A1BA-444197C7225D}";

        public static void Configure()
        {
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1)
            .AddToLevelEntry(2, FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(3, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(4, DeificObedience.Ragathiel1Guid)
            .AddToLevelEntry(5, FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(6, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(7, DeificObedience.Ragathiel2Guid)
            .AddToLevelEntry(8, FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(9, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(10, DeificObedience.Ragathiel3Guid)
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
            .SetReflexSave(SavesPrestigeHigh)
            .SetWillSave(SavesPrestigeLow)
            .SetProgression(progression)
            .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteFeature(FeatureRefs.BastardSwordProficiency.ToString())
            .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
            .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
            .AddPrerequisiteFeature("F79778D7-281C-4B9D-8E77-8F86812707AA", group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var CrimsonTemplarref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = CrimsonTemplarref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }
    }
}
