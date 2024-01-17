using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Toxicologist
    {
        private const string ArchetypeName = "Toxicologist";
        private static readonly string ArchetypeGuid = "{8C90A5EC-3CB9-4C75-832B-B468091E8544}";
        internal const string ArchetypeDisplayName = "Toxicologist.Name";
        private const string ArchetypeDescription = "Toxicologist.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureRefs.DruidWoodlandStride.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.ResistNaturesLure.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .SetRemoveFeaturesEntry(8, FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.WildShapeElementaHugeFeature.ToString())
            .AddToAddFeatures(1, CreateExpandedRepertoire())
            .AddToAddFeatures(4, ExpertPoisonerConfigure())
              .Configure();

        }

        private const string ExpandedRepertoire = "Toxicologist.ExpandedRepertoire";
        public static readonly string ExpandedRepertoireGuid = "{B2F76FAE-B259-4953-9D4B-2131524E24CE}";

        internal const string ExpandedRepertoireDisplayName = "ToxicologistExpandedRepertoire.Name";
        private const string ExpandedRepertoireDescription = "ToxicologistExpandedRepertoire.Description";

        public static BlueprintFeature CreateExpandedRepertoire()
        {
            var icon = AbilityRefs.StinkingCloud.Reference.Get().Icon;

            return FeatureConfigurator.New(ExpandedRepertoire, ExpandedRepertoireGuid)
              .SetDisplayName(ExpandedRepertoireDisplayName)
              .SetDescription(ExpandedRepertoireDescription)
              .SetIcon(icon)
              .AddKnownSpell(characterClass: CharacterClassRefs.DruidClass.ToString(), spellLevel: 3, spell: AbilityRefs.StinkingCloud.ToString())
              .AddKnownSpell(characterClass: CharacterClassRefs.DruidClass.ToString(), spellLevel: 5, spell: AbilityRefs.Cloudkill.ToString())
              .Configure();
        }

        private const string ExpertPoisoner = "Toxicologist.ExpertPoisoner";
        public static readonly string ExpertPoisonerGuid = "{6625455D-05A4-4A2A-835B-18C806467081}";

        internal const string ExpertPoisonerDisplayName = "ToxicologistExpertPoisoner.Name";
        private const string ExpertPoisonerDescription = "ToxicologistExpertPoisoner.Description";

        public static BlueprintFeature ExpertPoisonerConfigure()
        {
            var icon = AbilityRefs.Poison.Reference.Get().Icon;

            return FeatureConfigurator.New(ExpertPoisoner, ExpertPoisonerGuid)
              .SetDisplayName(ExpertPoisonerDisplayName)
              .SetDescription(ExpertPoisonerDescription)
              .SetIcon(icon)
              .AddIncreaseSpellDescriptorDC(1, SpellDescriptor.Poison, spellsOnly: true)
              .Configure();
        }
    }
}
