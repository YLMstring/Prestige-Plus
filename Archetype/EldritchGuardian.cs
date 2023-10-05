using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Archetype
{
    internal class EldritchGuardian
    {
        private const string ArchetypeName = "EldritchGuardian";
        private static readonly string ArchetypeGuid = "{7E4C3B30-1890-4E53-9C30-6326C6742AE5}";
        internal const string ArchetypeDisplayName = "EldritchGuardian.Name";
        private const string ArchetypeDescription = "EldritchGuardian.Description";
        public static void Configure()
        {
              ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
              .AddToRemoveFeatures(1, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToRemoveFeatures(2, FeatureSelectionRefs.FighterFeatSelection.ToString(), FeatureRefs.Bravery.ToString())
              .AddToRemoveFeatures(6, FeatureRefs.Bravery.ToString())
              .AddToRemoveFeatures(10, FeatureRefs.Bravery.ToString())
              .AddToRemoveFeatures(14, FeatureRefs.Bravery.ToString())
              .AddToAddFeatures(1, FeatureSelectionRefs.Familiar.ToString())
              .AddToAddFeatures(2, CreateShareTraining(), CreateSteelWill())
              .AddToAddFeatures(6, SteelWillGuid)
              .AddToAddFeatures(10, SteelWillGuid)
              .AddToAddFeatures(14, SteelWillGuid)
              .AddToAddFeatures(18, SteelWillGuid)
                .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeArcana)
                .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillKnowledgeWorld)
                .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillLoreNature)
                .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillPerception)
                .AddToClassSkills(Kingmaker.EntitySystem.Stats.StatType.SkillUseMagicDevice)
                .SetReplaceClassSkills(true)
                .Configure();
        }

        private const string ShareTraining = "EldritchGuardian.ShareTraining";
        private static readonly string ShareTrainingGuid = "{11CE494F-69A7-4B36-8A22-A6C6F6C3089E}";

        internal const string ShareTrainingDisplayName = "EldritchGuardianShareTraining.Name";
        private const string ShareTrainingDescription = "EldritchGuardianShareTraining.Description";
        private static BlueprintFeature CreateShareTraining()
        {
            var icon = FeatureSelectionRefs.AnimalCompanionSelectionDruid.Reference.Get().Icon;

            return FeatureConfigurator.New(ShareTraining, ShareTrainingGuid)
              .SetDisplayName(ShareTrainingDisplayName)
              .SetDescription(ShareTrainingDescription)
              .SetIcon(icon)
              .AddComponent<EldritchGuardianShareTraining>()
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string SteelWill = "EldritchGuardian.SteelWill";
        private static readonly string SteelWillGuid = "{5BC2A21F-89C9-42E7-8A26-DC55E5B6A957}";

        internal const string SteelWillDisplayName = "EldritchGuardianSteelWill.Name";
        private const string SteelWillDescription = "EldritchGuardianSteelWill.Description";

        private static BlueprintFeature CreateSteelWill()
        {
            var icon = FeatureRefs.IronWill.Reference.Get().Icon;

            return FeatureConfigurator.New(SteelWill, SteelWillGuid)
              .SetDisplayName(SteelWillDisplayName)
              .SetDescription(SteelWillDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .AddSavingThrowBonusAgainstDescriptor(value: 1, spellDescriptor: SpellDescriptor.MindAffecting)
              .Configure();
        }
    }
}
