using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomComponent.Spell;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem.Rules;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Supernaturalist
    {
        private const string ArchetypeName = "Supernaturalist";
        private static readonly string ArchetypeGuid = "{FE10CC0C-6F52-4892-9760-A8748B0BB26B}";
        internal const string ArchetypeDisplayName = "Supernaturalist.Name";
        private const string ArchetypeDescription = "Supernaturalist.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.NatureSense.ToString(), FeatureRefs.DruidProficiencies.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.WildShapeIWolfFeature.ToString(), FeatureRefs.ResistNaturesLure.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.WildShapeIILeopardFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalSmallFeature.ToString())
            .SetRemoveFeaturesEntry(8, FeatureRefs.WildShapeIVBearFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddMedium.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.WildShapeIIISmilodonFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString(), FeatureRefs.WildShapeElementalFeatureAddLarge.ToString(), FeatureRefs.WildShapeIVShamblingMoundFeature.ToString())
            .SetRemoveFeaturesEntry(12, FeatureRefs.WildShapeElementaHugeFeature.ToString(), FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(16, FeatureRefs.WildShapeExtraUse.ToString())
            .SetRemoveFeaturesEntry(18, FeatureRefs.WildShapeExtraUse.ToString())
            .AddToAddFeatures(1, CreateProficiencies())
            .AddToAddFeatures(3, CreateParanormalScholar3())
            .AddToAddFeatures(6, CreateParanormalScholar6())
            .AddToAddFeatures(9, CreateParanormalScholar9())
            .AddToAddFeatures(12, CreateParanormalScholar12())
            .AddToAddFeatures(15, CreateParanormalScholar15())
            .AddToAddFeatures(18, CreateParanormalScholar18())
            .AddToAddFeatures(4, CreateSpiritBonus())
            .AddToAddFeatures(7, SpiritBonusGuid)
            .AddToAddFeatures(11, SpiritBonusGuid)
            .AddToAddFeatures(15, SpiritBonusGuid)
            .AddToAddFeatures(19, SpiritBonusGuid)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DruidProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { ParanormalScholar3Guid, ParanormalScholar6Guid, ParanormalScholar9Guid, ParanormalScholar12Guid, ParanormalScholar15Guid, ParanormalScholar18Guid })
                .Configure();
        }

        private const string Proficiencies = "Supernaturalist.Proficiencies";
        private static readonly string ProficienciesGuid = "{D46EAF7C-1690-4F41-B2B6-85B8655F1073}";
        internal const string ProficienciesDisplayName = "SupernaturalistProficiencies.Name";
        private const string ProficienciesDescription = "SupernaturalistProficiencies.Description";
        public static BlueprintFeature CreateProficiencies()
        {
            var dProficiencies = FeatureRefs.DruidProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(dProficiencies.GetComponent<AddProficiencies>())
              .AddFacts(new() { FeatureRefs.ShieldsProficiency.ToString() })
              .Configure();
        }

        private const string ParanormalScholar3 = "Supernaturalist.ParanormalScholar3";
        public static readonly string ParanormalScholar3Guid = "{27BF8095-31F9-499F-8D6A-277D46378418}";

        internal const string ParanormalScholar3DisplayName = "SupernaturalistParanormalScholar3.Name";
        private const string ParanormalScholar3Description = "SupernaturalistParanormalScholar3.Description";

        public static BlueprintFeature CreateParanormalScholar3()
        {
            var icon = AbilityRefs.ThornBody.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar3, ParanormalScholar3Guid)
              .SetDisplayName(ParanormalScholar3DisplayName)
              .SetDescription(ParanormalScholar3Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(HermeanPotential.HermeanPotentialAbilityGuid);
                  c.level = 1;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar6 = "Supernaturalist.ParanormalScholar6";
        public static readonly string ParanormalScholar6Guid = "{9868BE72-1794-432D-9593-B40609A2CC81}";

        internal const string ParanormalScholar6DisplayName = "SupernaturalistParanormalScholar6.Name";
        private const string ParanormalScholar6Description = "SupernaturalistParanormalScholar6.Description";

        public static BlueprintFeature CreateParanormalScholar6()
        {
            var icon = AbilityRefs.ThornBody.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar6, ParanormalScholar6Guid)
              .SetDisplayName(ParanormalScholar6DisplayName)
              .SetDescription(ParanormalScholar6Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPain.DebilitatingPainAbilityGuid);
                  c.level = 3;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar9 = "Supernaturalist.ParanormalScholar9";
        public static readonly string ParanormalScholar9Guid = "{64182E9A-27FB-42FA-9308-7E2A5AA0AB90}";

        internal const string ParanormalScholar9DisplayName = "SupernaturalistParanormalScholar9.Name";
        private const string ParanormalScholar9Description = "SupernaturalistParanormalScholar9.Description";

        public static BlueprintFeature CreateParanormalScholar9()
        {
            var icon = AbilityRefs.ThornBody.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar9, ParanormalScholar9Guid)
              .SetDisplayName(ParanormalScholar9DisplayName)
              .SetDescription(ParanormalScholar9Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(DebilitatingPainMass.DebilitatingPainMassAbilityGuid);
                  c.level = 5;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar12 = "Supernaturalist.ParanormalScholar12";
        public static readonly string ParanormalScholar12Guid = "{90986E5A-0A0F-449E-8F2E-75FEB5E72825}";

        internal const string ParanormalScholar12DisplayName = "SupernaturalistParanormalScholar12.Name";
        private const string ParanormalScholar12Description = "SupernaturalistParanormalScholar12.Description";

        public static BlueprintFeature CreateParanormalScholar12()
        {
            var icon = AbilityRefs.ShadowConjurationGreater.Reference.Get().Icon;
            // "ShadowEnchantmentGreater": "ba079628-2748-4eb3-8bf0-b6aadd9f5f22", ttt

            return FeatureConfigurator.New(ParanormalScholar12, ParanormalScholar12Guid)
              .SetDisplayName(ParanormalScholar12DisplayName)
              .SetDescription(ParanormalScholar12Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>("ba079628-2748-4eb3-8bf0-b6aadd9f5f22");
                  c.level = 6;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar15 = "Supernaturalist.ParanormalScholar15";
        public static readonly string ParanormalScholar15Guid = "{1ECE9A0E-609C-4B18-969B-082298C61C26}";

        internal const string ParanormalScholar15DisplayName = "SupernaturalistParanormalScholar15.Name";
        private const string ParanormalScholar15Description = "SupernaturalistParanormalScholar15.Description";

        public static BlueprintFeature CreateParanormalScholar15()
        {
            var icon = AbilityRefs.ThornBody.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar15, ParanormalScholar15Guid)
              .SetDisplayName(ParanormalScholar15DisplayName)
              .SetDescription(ParanormalScholar15Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(GlimpseAkashic.GlimpseAkashicAbilityGuid);
                  c.level = 8;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string ParanormalScholar18 = "Supernaturalist.ParanormalScholar18";
        public static readonly string ParanormalScholar18Guid = "{C20564B2-B1B7-44D0-A613-23B4E12343DB}";

        internal const string ParanormalScholar18DisplayName = "SupernaturalistParanormalScholar18.Name";
        private const string ParanormalScholar18Description = "SupernaturalistParanormalScholar18.Description";

        public static BlueprintFeature CreateParanormalScholar18()
        {
            var icon = AbilityRefs.ThornBody.Reference.Get().Icon;

            return FeatureConfigurator.New(ParanormalScholar18, ParanormalScholar18Guid)
              .SetDisplayName(ParanormalScholar18DisplayName)
              .SetDescription(ParanormalScholar18Description)
              .SetIcon(icon)
              .AddComponent<PPLearnSpell>(c =>
              {
                  c.Spell = BlueprintTool.GetRef<BlueprintAbilityReference>(AkashicForm.AkashicFormAbilityGuid);
                  c.level = 9;
                  c.CharacterClass = CharacterClassRefs.DruidClass.Reference.Get();
              })
              .Configure();
        }

        private const string SpiritBonus = "Supernaturalist.SpiritBonus";
        private static readonly string SpiritBonusGuid = "{7D72C6EA-8612-4B2D-96AA-EEE6878F1F5F}";

        internal const string SpiritBonusDisplayName = "SupernaturalistSpiritBonus.Name";
        private const string SpiritBonusDescription = "SupernaturalistSpiritBonus.Description";
        private static BlueprintFeature CreateSpiritBonus()
        {
            var icon = AbilityRefs.PrimalRegression.Reference.Get().Icon;

            return FeatureConfigurator.New(SpiritBonus, SpiritBonusGuid)
              .SetDisplayName(SpiritBonusDisplayName)
              .SetDescription(SpiritBonusDescription)
              .SetIcon(icon)
              .SetRanks(5)
              .AddStatBonus(stat: StatType.SkillLoreNature, value: 1)
              .AddWeaponGroupAttackBonus(1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddWeaponGroupDamageBonus(damageBonus: 1, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Natural)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(SpiritBonusGuid))
              .Configure();
        }

        private const string FungalCompanion = "Supernaturalist.FungalCompanion";
        public static readonly string FungalCompanionGuid = "{9236353B-C49B-46AB-A0D2-9BED174FD06D}";

        private const string FungalCompanionFeat = "Supernaturalist.UseFungalCompanion";
        private static readonly string FungalCompanionFeatGuid = "{8C386267-494C-4C45-8C36-C17585C1966E}";

        internal const string FungalCompanionDisplayName = "SupernaturalistFungalCompanion.Name";
        private const string FungalCompanionDescription = "SupernaturalistFungalCompanion.Description";

        public static BlueprintFeature FungalCompanionConfigure()
        {
            var icon = AbilityRefs.FungalInfestationAbility.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(FungalCompanionFeat, FungalCompanionFeatGuid)
              .SetDisplayName(FungalCompanionDisplayName)
              .SetDescription(FungalCompanionDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.PlantType.ToString() })
              .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.Speed, -10)
              .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.Strength, 4)
              .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.Dexterity, -2)
              .AddStatBonus(ModifierDescriptor.UntypedStackable, false, StatType.Constitution, 4)
              .AddStatBonus(ModifierDescriptor.ArmorFocus, false, StatType.AC, 2)
              .Configure();

            return FeatureConfigurator.New(FungalCompanion, FungalCompanionGuid)
              .SetDisplayName(FungalCompanionDisplayName)
              .SetDescription(FungalCompanionDescription)
              .SetIcon(icon)
              .AddFeatureToPet(feat, PetType.AnimalCompanion)
              .Configure();
        }
    }
}
