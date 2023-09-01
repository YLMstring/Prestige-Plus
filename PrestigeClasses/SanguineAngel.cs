using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.PrestigeClasses
{
    internal class SanguineAngel
    {
        private const string ArchetypeName = "SanguineAngel";
        private static readonly string ArchetypeGuid = "{EBA75E50-F41C-48E5-852D-9575FB2C7DEF}";
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
                .AddToLevelEntry(1, FeatureRefs.TwoWeaponFighting.ToString(), ProdigiousTwoWeaponFighting)
                .AddToLevelEntry(2)
                .AddToLevelEntry(3, FeatureRefs.ArmorTraining.ToString())
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName(ArchetypeDisplayName)
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
                .AddPrerequisiteProficiency(weaponProficiencies: new WeaponCategory[] {  }, armorProficiencies: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy })
                .AddPrerequisiteFeature(FeatureRefs.ShieldBashFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteFeature(FeatureRefs.WeaponFocusLongsword.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
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
    }
}
