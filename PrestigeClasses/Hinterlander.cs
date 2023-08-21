using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using PrestigePlus.Feats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.PrestigeClasses
{
    internal class Hinterlander
    {
        private const string ArchetypeName = "Hinterlander";
        private static readonly string ArchetypeGuid = "{E6A41412-A7C7-4C9F-AB77-EA73835691A0}";
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
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, spellupgradeGuid, FeatureSelectionRefs.FavoriteEnemySelection.ToString())
                .AddToLevelEntry(2, FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
                .AddToLevelEntry(3, FeatureRefs.BloodlineFeyWoodlandStride.ToString())
                .AddToLevelEntry(4)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, FeatureSelectionRefs.FavoriteTerrainSelection.ToString())
                .AddToLevelEntry(7, ImbueArrow.FeatGuid, FeatureRefs.ReachSpellFeat.ToString())
                .AddToLevelEntry(8, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
                .AddToLevelEntry(9, FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .AddToLevelEntry(10, FeatureSelectionRefs.FavoriteTerrainSelectionRankUp.ToString())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.Evasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedEvasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.UncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedUncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>() }))
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
                .AddPrerequisiteFeature(FeatureRefs.WeaponFocusLongbow.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteCasterTypeSpellLevel(true, false, 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteCasterTypeSpellLevel(false, false, 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Hinterlanderref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Hinterlanderref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }
    }
}
