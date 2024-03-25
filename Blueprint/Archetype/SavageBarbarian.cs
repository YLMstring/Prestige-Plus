using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using PrestigePlus.CustomComponent;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class SavageBarbarian
    {
        private const string ArchetypeName = "SavageBarbarian";
        private static readonly string ArchetypeGuid = "{F3CA42E4-079E-4C38-994B-36350E8AB1B0}";
        internal const string ArchetypeDisplayName = "SavageBarbarian.Name";
        private const string ArchetypeDescription = "SavageBarbarian.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BarbarianClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetRemoveFeaturesEntry(3, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(6, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(9, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(12, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(15, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(18, FeatureRefs.DangerSenseBarbarian.ToString())
              .SetRemoveFeaturesEntry(7, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(10, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(13, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(16, FeatureRefs.DamageReduction.ToString())
              .SetRemoveFeaturesEntry(19, FeatureRefs.DamageReduction.ToString())
              .AddToAddFeatures(3, CreateNakedCourage())
              .AddToAddFeatures(4, NakedCourageGuid)
              .AddToAddFeatures(5, NakedCourageGuid)
              .AddToAddFeatures(12, NakedCourageGuid)
              .AddToAddFeatures(15, NakedCourageGuid)
              .AddToAddFeatures(18, NakedCourageGuid)
              .AddToAddFeatures(1, CreateNaturalToughness())
              .AddToAddFeatures(2, NaturalToughnessGuid)
              .AddToAddFeatures(3, NaturalToughnessGuid)
              .AddToAddFeatures(16, NaturalToughnessGuid)
              .AddToAddFeatures(19, NaturalToughnessGuid)
              .Configure();
        }

        private const string NakedCourage = "SavageBarbarian.NakedCourage";
        private static readonly string NakedCourageGuid = "{AAFE92DD-F99E-47D3-A54F-ED2816420257}";

        private const string NakedCourage0 = "SavageBarbarian.NakedCourage0";
        private static readonly string NakedCourageGuid0 = "{FAE65171-3570-4C80-B4B6-3FCF6AC7A992}";

        internal const string NakedCourageDisplayName = "SavageBarbarianNakedCourage.Name";
        private const string NakedCourageDescription = "SavageBarbarianNakedCourage.Description";
        private static BlueprintFeature CreateNakedCourage()
        {
            var icon = FeatureRefs.DivineGuardianCourageousDefense.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(NakedCourage0, NakedCourageGuid0)
              .SetDisplayName(NakedCourageDisplayName)
              .SetDescription(NakedCourageDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Rank(), modifierDescriptor: ModifierDescriptor.Morale, spellDescriptor: SpellDescriptor.Fear)
              .AddContextStatBonus(StatType.AC, ContextValues.Rank(), ModifierDescriptor.Dodge)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(NakedCourageGuid))
              .Configure();

            return FeatureConfigurator.New(NakedCourage, NakedCourageGuid)
              .SetDisplayName(NakedCourageDisplayName)
              .SetDescription(NakedCourageDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddComponent<ArmorUnlockPP>(c => {
                  c.NewFact = feat.ToReference<BlueprintUnitFactReference>();
                  c.NoArmor = true;
              })
              .Configure();
        }

        private const string NaturalToughness = "SavageBarbarian.NaturalToughness";
        private static readonly string NaturalToughnessGuid = "{C12A5439-E85A-45FA-86F6-0DA5788D5AA7}";

        private const string NaturalToughness0 = "SavageBarbarian.NaturalToughness0";
        private static readonly string NaturalToughnessGuid0 = "{63A10378-CFFD-4FB6-BB16-763D73C5E39F}";

        internal const string NaturalToughnessDisplayName = "SavageBarbarianNaturalToughness.Name";
        private const string NaturalToughnessDescription = "SavageBarbarianNaturalToughness.Description";
        private static BlueprintFeature CreateNaturalToughness()
        {
            var icon = FeatureRefs.DivineGuardianCourageousDefense.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(NaturalToughness0, NaturalToughnessGuid0)
              .SetDisplayName(NaturalToughnessDisplayName)
              .SetDescription(NaturalToughnessDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.AC, ContextValues.Rank(), ModifierDescriptor.NaturalArmor)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(NaturalToughnessGuid))
              .Configure();

            return FeatureConfigurator.New(NaturalToughness, NaturalToughnessGuid)
              .SetDisplayName(NaturalToughnessDisplayName)
              .SetDescription(NaturalToughnessDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddComponent<ArmorUnlockPP>(c => {
                  c.NewFact = feat.ToReference<BlueprintUnitFactReference>();
                  c.NoArmor = true;
              })
              .Configure();
        }
    }
}
