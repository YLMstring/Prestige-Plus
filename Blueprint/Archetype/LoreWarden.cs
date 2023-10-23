using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.CustomComponent.Archetype;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class LoreWarden
    {
        private const string ArchetypeName = "LoreWarden";
        private static readonly string ArchetypeGuid = "{D818901A-B616-45CC-A6CE-9F109F5E2725}";
        internal const string ArchetypeDisplayName = "LoreWarden.Name";
        private const string ArchetypeDescription = "LoreWarden.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.FighterClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.FighterProficiencies.ToString())
            .SetRemoveFeaturesEntry(2, FeatureRefs.Bravery.ToString())
            .SetRemoveFeaturesEntry(3, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.ArmorTraining.ToString())
            .SetRemoveFeaturesEntry(15, FeatureRefs.ArmorTraining.ToString())
            //.SetRemoveFeaturesEntry(7, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            //.SetRemoveFeaturesEntry(11, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            //.SetRemoveFeaturesEntry(15, ModFeatureSelectionRefs.ArmorTrainingSelection.ToString())
            .SetRemoveFeaturesEntry(19, FeatureRefs.ArmorMastery.ToString())
            .AddToAddFeatures(1, CreateProficiencies())
            .AddToAddFeatures(2, FeatureRefs.CombatExpertiseFeature.ToString())
            .AddToAddFeatures(3, SwordsSecretFeat())
            .AddToAddFeatures(7, SwordsSecretGuid)
            .AddToAddFeatures(11, SwordsSecretGuid)
            .AddToAddFeatures(15, SwordsSecretGuid)
            .AddToAddFeatures(19, SwordsSecretGuid)
              .AddToClassSkills(StatType.SkillAthletics)
              .AddToClassSkills(StatType.SkillKnowledgeArcana)
              .AddToClassSkills(StatType.SkillKnowledgeWorld)
              .AddToClassSkills(StatType.SkillLoreNature)
              .AddToClassSkills(StatType.SkillPersuasion)
              .SetReplaceClassSkills(true)
              .SetAddSkillPoints(2)
              .Configure();
        }

        private const string Proficiencies = "LoreWarden.Proficiencies";
        private static readonly string ProficienciesGuid = "{E1894AF5-6E25-49CE-A003-16412E32C276}";
        internal const string ProficienciesDisplayName = "LoreWardenProficiencies.Name";
        private const string ProficienciesDescription = "LoreWardenProficiencies.Description";

        public static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.ToString(), FeatureRefs.MartialWeaponProficiency.ToString(), FeatureRefs.LightArmorProficiency.ToString() })
              .Configure();
        }

        private const string SwordsSecret = "LoreWarden.SwordsSecret";
        private static readonly string SwordsSecretGuid = "{4E43252C-8C8B-4C7A-A8A3-C666711E0EC5}";

        internal const string SwordsSecretDisplayName = "LoreWardenSwordsSecret.Name";
        private const string SwordsSecretDescription = "LoreWardenSwordsSecret.Description";

        public static BlueprintFeatureSelection SwordsSecretFeat()
        {
            var icon = FeatureRefs.DuelistParryFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(SwordsSecret, SwordsSecretGuid)
              .SetDisplayName(SwordsSecretDisplayName)
              .SetDescription(SwordsSecretDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(ExploitWeaknessFeat())
              .AddToAllFeatures(HairFeat())
              .AddToAllFeatures(KnowThyFeat())
              .AddToAllFeatures(ManeuverMasteryFeat())
              .AddToAllFeatures(SwiftAssessmentFeat())
              .Configure();
        }

        private const string ManeuverMastery = "LoreWarden.ManeuverMastery";
        private static readonly string ManeuverMasteryGuid = "{F51AB5E2-3809-4E21-A5FF-809EA39A220B}";

        private const string ManeuverMastery1 = "LoreWarden.ManeuverMastery1";
        private static readonly string ManeuverMasteryGuid1 = "{439AF2C4-0E51-4538-94D6-A139BCAF6057}";

        private const string ManeuverMastery2 = "LoreWarden.ManeuverMastery2";
        private static readonly string ManeuverMasteryGuid2 = "{C6B1A786-2C73-4C94-A98B-4FC63A4093E4}";

        private const string ManeuverMastery3 = "LoreWarden.ManeuverMastery3";
        private static readonly string ManeuverMasteryGuid3 = "{0156840A-0E08-4AA9-867E-A544F0885297}";

        private const string ManeuverMastery4 = "LoreWarden.ManeuverMastery4";
        private static readonly string ManeuverMasteryGuid4 = "{69446283-9004-4AB5-974D-2B267BD20EE9}";

        internal const string ManeuverMasteryDisplayName = "LoreWardenManeuverMastery.Name";
        private const string ManeuverMasteryDescription = "LoreWardenManeuverMastery.Description";
        public static BlueprintProgression ManeuverMasteryFeat()
        {
            var icon = FeatureRefs.BullRushMythicFeat.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(ManeuverMastery1, ManeuverMasteryGuid1)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCMBBonus(value: ContextValues.Constant(2))
              .AddCMDBonus(value: ContextValues.Constant(2))
              .Configure();

            var feat2 = FeatureConfigurator.New(ManeuverMastery2, ManeuverMasteryGuid2)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCMBBonus(value: ContextValues.Constant(2))
              .AddCMDBonus(value: ContextValues.Constant(2))
              .Configure();

            var feat3 = FeatureConfigurator.New(ManeuverMastery3, ManeuverMasteryGuid3)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCMBBonus(value: ContextValues.Constant(2))
              .AddCMDBonus(value: ContextValues.Constant(2))
              .Configure();

            var feat4 = FeatureConfigurator.New(ManeuverMastery4, ManeuverMasteryGuid4)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCMBBonus(value: ContextValues.Constant(2))
              .AddCMDBonus(value: ContextValues.Constant(2))
              .Configure();

            return ProgressionConfigurator.New(ManeuverMastery, ManeuverMasteryGuid)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .SetClasses(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.FighterClass.ToString()))
              .AddToLevelEntry(3, feat)
              .AddToLevelEntry(7, feat2)
              .AddToLevelEntry(11, feat3)
              .AddToLevelEntry(15, feat4)
              .Configure();
        }

        private const string KnowThy = "LoreWarden.KnowThy";
        private static readonly string KnowThyGuid = "{18D5CDC5-5BA1-4F33-A145-711C48990DD3}";

        private const string KnowThyAbility = "LoreWarden.UseKnowThy";
        private static readonly string KnowThyAbilityGuid = "{70274631-CE9A-45C7-AE1B-8550CCFD008D}";

        private const string KnowThyAbility1 = "LoreWarden.UseKnowThy1";
        private static readonly string KnowThyAbilityGuid1 = "{084A3FDB-1119-4421-B633-491B08FD4C44}";

        private const string KnowThyAbility2 = "LoreWarden.UseKnowThy2";
        private static readonly string KnowThyAbilityGuid2 = "{DB19BEA6-852E-4B2F-8F86-DEC97C057931}";

        private const string KnowThyBuff = "LoreWarden.KnowThyBuff";
        private static readonly string KnowThyBuffGuid = "{C82AFDAB-48A7-4150-A7D5-72849ED589B8}";

        internal const string KnowThyDisplayName = "LoreWardenKnowThy.Name";
        private const string KnowThyDescription = "LoreWardenKnowThy.Description";
        public static BlueprintFeature KnowThyFeat()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(KnowThyBuff, KnowThyBuffGuid)
             .SetDisplayName(KnowThyDisplayName)
             .SetDescription(KnowThyDescription)
             .SetIcon(icon)
             .AddComponent<LoreWardenKnow>()
             .AddUniqueBuff()
             .Configure();

            var action = ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank()))
                        .Build();

            var ability = AbilityConfigurator.New(KnowThyAbility, KnowThyAbilityGuid)
                .CopyFrom(
                AbilityRefs.StudentOfWarKnowYourEnemyAbility)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(BuffRefs.StudentOfWarKnowYourEnemyBuff.ToString()).Build(),
                    ifFalse: ActionsBuilder.New()
                        .MakeKnowledgeCheck(successActions: action)
                        .Build(),
                    ifTrue: action)
                    .Build())
                .SetDisplayName(KnowThyDisplayName)
                .SetDescription(KnowThyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.FighterClass.ToString() }, max: 10, min: 2).WithDiv2Progression())
                .Configure();

            var ability1 = AbilityConfigurator.New(KnowThyAbility1, KnowThyAbilityGuid1)
                .CopyFrom(
                AbilityRefs.StudentOfWarKnowYourEnemyAbility)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(BuffRefs.StudentOfWarKnowYourEnemyBuff.ToString()).Build(),
                    ifFalse: ActionsBuilder.New()
                        .MakeKnowledgeCheck(successActions: action)
                        .Build(),
                    ifTrue: action)
                    .Build())
                .SetDisplayName(KnowThyDisplayName)
                .SetDescription(KnowThyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.FighterClass.ToString() }, max: 10, min: 2).WithDiv2Progression())
                .Configure();

            var ability2 = AbilityConfigurator.New(KnowThyAbility2, KnowThyAbilityGuid2)
                .CopyFrom(
                AbilityRefs.StudentOfWarKnowYourEnemyAbility)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(BuffRefs.StudentOfWarKnowYourEnemyBuff.ToString()).Build(),
                    ifFalse: ActionsBuilder.New()
                        .MakeKnowledgeCheck(successActions: action)
                        .Build(),
                    ifTrue: action)
                    .Build())
                .SetDisplayName(KnowThyDisplayName)
                .SetDescription(KnowThyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.FighterClass.ToString() }, max: 10, min: 2).WithDiv2Progression())
                .Configure();

            return FeatureConfigurator.New(KnowThy, KnowThyGuid)
              .SetDisplayName(KnowThyDisplayName)
              .SetDescription(KnowThyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string SwiftAssessment = "LoreWarden.SwiftAssessment";
        private static readonly string SwiftAssessmentGuid = "{C69F5DF7-8308-46C2-B7D7-A81E015BD45D}";

        private const string SwiftAssessment1 = "LoreWarden.SwiftAssessment1";
        private static readonly string SwiftAssessmentGuid1 = "{972196ED-D424-4715-BB7E-FDD83D74BE0D}";

        private const string SwiftAssessment2 = "LoreWarden.SwiftAssessment2";
        private static readonly string SwiftAssessmentGuid2 = "{5A72F7A4-97D1-4217-8A50-DD2FCAA94C8E}";

        internal const string SwiftAssessmentDisplayName = "LoreWardenSwiftAssessment.Name";
        private const string SwiftAssessmentDescription = "LoreWardenSwiftAssessment.Description";
        public static BlueprintProgression SwiftAssessmentFeat()
        {
            var icon = FeatureRefs.Quarry.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(SwiftAssessment1, SwiftAssessmentGuid1)
              .SetDisplayName(SwiftAssessmentDisplayName)
              .SetDescription(SwiftAssessmentDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { KnowThyAbilityGuid1 })
              .Configure();

            var feat2 = FeatureConfigurator.New(SwiftAssessment2, SwiftAssessmentGuid2)
              .SetDisplayName(SwiftAssessmentDisplayName)
              .SetDescription(SwiftAssessmentDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { KnowThyAbilityGuid2 })
              .Configure();

            return ProgressionConfigurator.New(SwiftAssessment, SwiftAssessmentGuid)
              .SetDisplayName(SwiftAssessmentDisplayName)
              .SetDescription(SwiftAssessmentDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.FighterClass.ToString()))
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteFeature(KnowThyGuid)
              .AddToLevelEntry(7, feat)
              .AddToLevelEntry(15, feat2)
              .Configure();
        }

        private const string ExploitWeakness = "LoreWarden.ExploitWeakness";
        private static readonly string ExploitWeaknessGuid = "{A96F47B0-680E-4E84-91DA-2881DA8B8B9F}";

        private const string ExploitWeakness1 = "LoreWarden.ExploitWeakness1";
        private static readonly string ExploitWeaknessGuid1 = "{00FE279C-665A-429C-B15F-BFCF7D3C7880}";

        private const string ExploitWeakness2 = "LoreWarden.ExploitWeakness2";
        private static readonly string ExploitWeaknessGuid2 = "{88485FBA-0464-4C51-957B-8FF8714130D5}";

        private const string ExploitWeakness3 = "LoreWarden.ExploitWeakness3";
        private static readonly string ExploitWeaknessGuid3 = "{B37B0CCC-483B-44DC-8DD5-E320D8BE3495}";

        private const string ExploitWeaknessBuff = "LoreWarden.ExploitWeaknessBuff";
        private static readonly string ExploitWeaknessBuffGuid = "{5159A82C-56DD-499D-9668-55F3B9A70909}";

        private const string ExploitWeaknessBuff2 = "LoreWarden.ExploitWeaknessBuff2";
        private static readonly string ExploitWeaknessBuffGuid2 = "{76BC782C-6A36-4E32-AF2B-069F950CCBB6}";

        internal const string ExploitWeaknessDisplayName = "LoreWardenExploitWeakness.Name";
        private const string ExploitWeaknessDescription = "LoreWardenExploitWeakness.Description";
        public static BlueprintProgression ExploitWeaknessFeat()
        {
            var icon = AbilityRefs.SenseVitals.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ExploitWeaknessBuff, ExploitWeaknessBuffGuid)
             .SetDisplayName(ExploitWeaknessDisplayName)
             .SetDescription(ExploitWeaknessDescription)
             .SetIcon(icon)
             .AddInitiatorCritAutoconfirm()
             .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveSelf().Build(), true, criticalHit: true)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var Buff2 = BuffConfigurator.New(ExploitWeaknessBuff2, ExploitWeaknessBuffGuid2)
             .SetDisplayName(ExploitWeaknessDisplayName)
             .SetDescription(ExploitWeaknessDescription)
             .SetIcon(icon)
             .AddPartialDRIgnore(checkWeaponCategories: false, reductionReduction: 5, useContextValue: false)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var feat = FeatureConfigurator.New(ExploitWeakness1, ExploitWeaknessGuid1)
              .SetDisplayName(ExploitWeaknessDisplayName)
              .SetDescription(ExploitWeaknessDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCriticalConfirmationBonus(value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.FighterClass.ToString() }).WithDivStepProgression(3))
              .Configure();

            var feat2 = FeatureConfigurator.New(ExploitWeakness2, ExploitWeaknessGuid2)
              .SetDisplayName(ExploitWeaknessDisplayName)
              .SetDescription(ExploitWeaknessDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddInitiatorAttackRollTrigger(ActionsBuilder.New().ApplyBuff(Buff2, ContextDuration.Fixed(1)).Build(), onOwner: true, criticalHit: true)
              .Configure();

            var feat3 = FeatureConfigurator.New(ExploitWeakness3, ExploitWeaknessGuid3)
              .SetDisplayName(ExploitWeaknessDisplayName)
              .SetDescription(ExploitWeaknessDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().ApplyBuff(Buff1, ContextDuration.Fixed(1)))
              .Configure();

            return ProgressionConfigurator.New(ExploitWeakness, ExploitWeaknessGuid)
              .SetDisplayName(ExploitWeaknessDisplayName)
              .SetDescription(ExploitWeaknessDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.FighterClass.ToString()))
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(3, feat)
              .AddToLevelEntry(11, feat2)
              .AddToLevelEntry(19, feat3)
              .Configure();
        }

        private const string Hair = "LoreWarden.Hair";
        private static readonly string HairGuid = "{6CE8F049-D762-4888-8963-4E8EB30A8C42}";

        private const string Hair2 = "LoreWarden.Hair2";
        private static readonly string HairGuid2 = "{23E114AE-6038-44CC-A44E-C9A47BEEDDA9}";

        private const string Hair3 = "LoreWarden.Hair3";
        private static readonly string HairGuid3 = "{C79DDFCF-1AE4-41DF-9F14-FD4A25BEDB80}";

        private const string HairBuff = "LoreWarden.HairBuff";
        private static readonly string HairBuffGuid = "{84707934-BE3E-4BB3-8BFF-A51B6B4BA62D}";

        private const string HairAbility = "LoreWarden.HairAbility";
        private static readonly string HairAbilityGuid = "{E00ADA56-C128-4710-ADDE-DD23D43F510D}";

        private const string HairAblityRes = "LoreWardenHairAblityRes";
        private static readonly string HairAblityResGuid = "{993D5BAE-1EC5-487D-8216-C81190A7FF59}";

        internal const string HairDisplayName = "LoreWardenHair.Name";
        private const string HairDescription = "LoreWardenHair.Description";
        public static BlueprintProgression HairFeat()
        {
            var icon = FeatureRefs.SavingSlash.Reference.Get().Icon;

            BlueprintAbilityResource Scarabilityresourse = AbilityResourceConfigurator.New(HairAblityRes, HairAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var Buff1 = BuffConfigurator.New(HairBuff, HairBuffGuid)
             .SetDisplayName(HairDisplayName)
             .SetDescription(HairDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddComponent<LoreWardenHair>()
             .Configure();

            var ability = ActivatableAbilityConfigurator.New(HairAbility, HairAbilityGuid)
                .SetDisplayName(HairDisplayName)
                .SetDescription(HairDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateImmediately()
                .AddActivatableAbilityResourceLogic(requiredResource: Scarabilityresourse, spendType: Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetIsOnByDefault(true)
                .Configure();

            var feat2 = FeatureConfigurator.New(Hair2, HairGuid2)
              .SetDisplayName(HairDisplayName)
              .SetDescription(HairDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: Scarabilityresourse, restoreAmount: true)
              .Configure();

            var feat3 = FeatureConfigurator.New(Hair3, HairGuid3)
              .SetDisplayName(HairDisplayName)
              .SetDescription(HairDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddIncreaseResourceAmount(Scarabilityresourse, 1)
              .Configure();

            return ProgressionConfigurator.New(Hair, HairGuid)
              .SetDisplayName(HairDisplayName)
              .SetDescription(HairDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.FighterClass.ToString()))
              .SetGiveFeaturesForPreviousLevels(true)
              //.AddPrerequisiteClassLevel(CharacterClassRefs.FighterClass.ToString(), 11)
              .AddToLevelEntry(3, feat2)
              .AddToLevelEntry(15, feat3)
              .Configure();
        }
    }
}
