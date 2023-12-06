using BlueprintCore.Blueprints.Configurators.Classes;
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
            .AddToLevelEntry(1, BonusFeatFeat())
            .AddToLevelEntry(2, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(3)
            .AddToLevelEntry(4)
            .AddToLevelEntry(5)
            .AddToLevelEntry(6)
            .AddToLevelEntry(7, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(8, FeatureSelectionRefs.FighterFeatSelection.ToString())
            .AddToLevelEntry(9)
            .AddToLevelEntry(10)
            .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FiendishStudiesGuid, FiendishStudies5Guid, FiendishStudies10Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SymbolicWeapon3Guid, SymbolicWeapon6Guid, SymbolicWeapon9Guid, SymbolicWeaponGuid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { DeificObedience.Ragathiel1Guid, DeificObedience.Ragathiel2Guid, DeificObedience.Ragathiel3Guid }))
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

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Sentinelref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Sentinelref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string BonusFeat = "Sentinel.BonusFeat";
        private static readonly string BonusFeatGuid = "{283F021D-C8A3-4D2B-ACAA-9DC46E77CD8E}";

        internal const string BonusFeatDisplayName = "SentinelBonusFeat.Name";
        private const string BonusFeatDescription = "SentinelBonusFeat.Description";

        public static BlueprintFeatureSelection BonusFeatFeat()
        {
            return FeatureSelectionConfigurator.New(BonusFeat, BonusFeatGuid)
              .SetDisplayName(BonusFeatDisplayName)
              .SetDescription(BonusFeatDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(DeificObedience.ShelynSentinelGuid)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .Configure();
        }

        private const string SymbolicWeapon = "Sentinel.SymbolicWeapon";
        private static readonly string SymbolicWeaponGuid = "{D3F71CE4-7D23-4A1C-B4A0-40D61C60E5BB}";

        internal const string SentinelSymbolicWeaponDisplayName = "SentinelSymbolicWeapon.Name";
        private const string SentinelSymbolicWeaponDescription = "SentinelSymbolicWeapon.Description";
        public static BlueprintFeature SymbolicWeaponFeat()
        {
            var icon = FeatureRefs.WeaponBondFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(SymbolicWeapon, SymbolicWeaponGuid)
              .SetDisplayName(SentinelSymbolicWeaponDisplayName)
              .SetDescription(SentinelSymbolicWeaponDescription)
              .SetIcon(icon)
              .AddComponent<SymbolicWeaponBonus>()
              .Configure();
        }
    }
}
