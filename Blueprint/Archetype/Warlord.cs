using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Warlord
    {
        private const string ArchetypeName = "Warlord";
        private static readonly string ArchetypeGuid = "{E1C0D1F2-52D2-4251-901E-92C26463A921}";
        internal const string ArchetypeDisplayName = "Warlord.Name";
        private const string ArchetypeDescription = "Warlord.Description";

        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        //[HarmonyBefore(new string[] { "TabletopTweaks-Base" })]
        public static void Configure()
        {
            var arc = ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.FighterProficiencies.ToString())
            .SetRemoveFeaturesEntry(3, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(15, FeatureRefs.ArmorTraining.ToString())
            //.SetRemoveFeaturesEntry(7, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            //.SetRemoveFeaturesEntry(11, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            //.SetRemoveFeaturesEntry(15, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            .SetRemoveFeaturesEntry(19, FeatureRefs.ArmorMastery.ToString())
            .AddToAddFeatures(1, CreateProficiencies())
            .AddToAddFeatures(3, CreateBattleBravado())
            .AddToAddFeatures(7, CreateEvasiveDueling())
            .AddToAddFeatures(11, EvasiveDuelingGuid)
            .AddToAddFeatures(15, EvasiveDuelingGuid)
            .AddToAddFeatures(19, CreateBronzedSkin())
              .AddToClassSkills(StatType.SkillAthletics)
              .AddToClassSkills(StatType.SkillMobility)
              .AddToClassSkills(StatType.SkillKnowledgeWorld)
              .AddToClassSkills(StatType.SkillLoreNature)
              .AddToClassSkills(StatType.SkillPersuasion)
              .SetReplaceClassSkills(true)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.FighterProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { BattleBravadoGuid, EvasiveDuelingGuid, BronzedSkinGuid })
                .Configure();
        }

        private const string Proficiencies = "Warlord.Proficiencies";
        private static readonly string ProficienciesGuid = "{892286E2-E4DA-4C05-91EE-7C439FF3EB53}";
        internal const string ProficienciesDisplayName = "WarlordProficiencies.Name";
        private const string ProficienciesDescription = "WarlordProficiencies.Description";

        public static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.Rapier,
              WeaponCategory.Longsword,
              WeaponCategory.Shortspear,
              WeaponCategory.Longspear,
              WeaponCategory.Spear,
              WeaponCategory.Trident,
                  })
              .Configure();
        }

        private const string BattleBravado = "Warlord.BattleBravado";
        private static readonly string BattleBravadoGuid = "{22B70FE9-DFFE-4799-A5C5-BEAAD08BC90C}";

        private const string BattleBravado0 = "Warlord.BattleBravado0";
        private static readonly string BattleBravadoGuid0 = "{3B40ABCD-3CD8-4129-BB4F-98424ECDC192}";

        internal const string BattleBravadoDisplayName = "WarlordBattleBravado.Name";
        private const string BattleBravadoDescription = "WarlordBattleBravado.Description";
        private static BlueprintFeature CreateBattleBravado()
        {
            var icon = FeatureRefs.DisplayWeaponProwess.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(BattleBravado0, BattleBravadoGuid0)
              .SetDisplayName(BattleBravadoDisplayName)
              .SetDescription(BattleBravadoDescription)
              .SetIcon(icon)
              .AddFacts(new() { "64acb179cc6a4f19bb3513d094b28d02", "ffc307c9e39544968432a21ad842bf26" })
              .Configure();

            return FeatureConfigurator.New(BattleBravado, BattleBravadoGuid)
              .SetDisplayName(BattleBravadoDisplayName)
              .SetDescription(BattleBravadoDescription)
              .SetIcon(icon)
              .AddMonkNoArmorFeatureUnlock(feat)
              .Configure();
        }

        private const string BronzedSkin = "Warlord.BronzedSkin";
        private static readonly string BronzedSkinGuid = "{9A263B4B-6ACE-457D-AA6C-A133D260AD89}";

        internal const string BronzedSkinDisplayName = "WarlordBronzedSkin.Name";
        private const string BronzedSkinDescription = "WarlordBronzedSkin.Description";

        private static BlueprintFeature CreateBronzedSkin()
        {
            var icon = FeatureRefs.BloodragerDamageReduction.Reference.Get().Icon;

            return FeatureConfigurator.New(BronzedSkin, BronzedSkinGuid)
              .SetDisplayName(BronzedSkinDisplayName)
              .SetDescription(BronzedSkinDescription)
              .SetIcon(icon)
              .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(5))
              .Configure();
        }

        private const string EvasiveDueling = "Warlord.EvasiveDueling";
        private static readonly string EvasiveDuelingGuid = "{0BAE6C61-7E83-402E-80D7-4DCE379FF22B}";

        internal const string EvasiveDuelingDisplayName = "WarlordEvasiveDueling.Name";
        private const string EvasiveDuelingDescription = "WarlordEvasiveDueling.Description";

        private static BlueprintFeature CreateEvasiveDueling()
        {
            var icon = FeatureRefs.Dodge.Reference.Get().Icon;

            return FeatureConfigurator.New(EvasiveDueling, EvasiveDuelingGuid)
              .SetDisplayName(EvasiveDuelingDisplayName)
              .SetDescription(EvasiveDuelingDescription)
              .SetIcon(icon)
              .AddPrerequisiteArchetypeLevel(ArchetypeGuid, CharacterClassRefs.FighterClass.ToString())
              .SetHideNotAvailibleInUI(true)
              .AddContextStatBonus(StatType.AC, value: ContextValues.Rank(), descriptor: ModifierDescriptor.Dodge)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(EvasiveDuelingGuid))
              .SetRanks(20)
              .AddToFeatureSelection(FeatureSelectionRefs.FighterFeatSelection.ToString())
              .Configure();
        }
    }
}
