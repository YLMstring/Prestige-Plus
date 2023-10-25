using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Modify;
using PrestigePlus.Patch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class EsotericKnight
    {
        private const string ArchetypeName = "EsotericKnight";
        private static readonly string ArchetypeGuid = "{0829A992-9641-49ED-BE17-47E6BA023716}";
        internal const string ArchetypeDisplayName = "EsotericKnight.Name";
        private const string ArchetypeDescription = "EsotericKnight.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "EsotericKnightPrestige";
        private static readonly string ClassProgressGuid = "{91A782A8-AE78-4297-A507-E68D39F3C209}";

        private static readonly string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

        public static void Configure()
        {
            KineticSelectionFeat(); KnightSelectionFeat(); MentalSelectionFeat(); PsychicEsotericaFeat();
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, KnightSelectionGuid, KnightSelectionGuid, CreateProficiencies())
            .AddToLevelEntry(2, PsychicEsotericaGuid)
            .AddToLevelEntry(3, BattleMindGuid)
            .AddToLevelEntry(4, PsychicEsotericaGuid)
            .AddToLevelEntry(6, PsychicEsotericaGuid)
            .AddToLevelEntry(8, PsychicEsotericaGuid)
            .AddToLevelEntry(9, BattleMindGuid)
            .AddToLevelEntry(10, PsychicEsotericaGuid)
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName("")
            .SetDescription(ArchetypeDescription)
            .Configure();

            BlueprintCharacterClass archetype =
          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName(ArchetypeDisplayName)
            .SetLocalizedDescription(ArchetypeDescription)
            .SetSkillPoints(4)
            .SetHitDie(DiceType.D10)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(BABFull)
            .SetFortitudeSave(SavesPrestigeHigh)
            .SetReflexSave(SavesPrestigeLow)
            .SetWillSave(SavesPrestigeHigh)
            .SetProgression(progression)
            .AddSkipLevelsForSpellProgression(new int[] { 3, 5, 7, 9 })
            .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillLoreReligion, StatType.SkillKnowledgeArcana, StatType.SkillMobility })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteCasterTypeSpellLevel(false, false, 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteFeature(FeatureRefs.KineticBlastFeature.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var EsotericKnightref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = EsotericKnightref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Proficiencies = "EsotericKnight.Proficiencies";
        private static readonly string ProficienciesGuid = "{A05A60B4-2737-4AB9-95EC-75E0F4A8A870}";
        internal const string ProficienciesDisplayName = "EsotericKnightProficiencies.Name";
        private const string ProficienciesDescription = "EsotericKnightProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            var assProficiencies = FeatureRefs.SlayerProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(assProficiencies.GetComponent<AddFacts>())
              .AddProficiencies(
                armorProficiencies:
                  new ArmorProficiencyGroup[]
                  {
              ArmorProficiencyGroup.TowerShield,
                  })
              .Configure();
        }

        private const string MartialEsoterica = "EsotericKnight.MartialEsoterica";
        private static readonly string MartialEsotericaGuid = "{22239F48-F8D3-458E-9BAF-3C1AB55483A4}";

        internal const string MartialEsotericaDisplayName = "EsotericKnightMartialEsoterica.Name";
        private const string MartialEsotericaDescription = "EsotericKnightMartialEsoterica.Description";
        public static BlueprintProgression MartialEsotericaFeat()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;
            return ProgressionConfigurator.New(MartialEsoterica, MartialEsotericaGuid)
              .SetDisplayName(MartialEsotericaDisplayName)
              .SetDescription(MartialEsotericaDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToLevelEntry(3, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToLevelEntry(5, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToLevelEntry(7, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToLevelEntry(9, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .AddToLevelEntry(10, FeatureSelectionRefs.FighterFeatSelection.ToString())
              .Configure();
        }

        private const string KineticSelection = "EsotericKnight.KineticSelection";
        private static readonly string KineticSelectionGuid = "{C9373D0C-D33E-4975-9E95-B16E51CAFBD1}";

        internal const string KineticSelectionDisplayName = "KineticSelection.Name";
        private const string KineticSelectionDescription = "KineticSelection.Description";

        public static void KineticSelectionFeat()
        {
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(KineticSelection, KineticSelectionGuid)
              .SetDisplayName(KineticSelectionDisplayName)
              .SetDescription(KineticSelectionDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureSelectionRefs.InfusionSelection.ToString())
              .AddToAllFeatures(FeatureSelectionRefs.WildTalentSelection.ToString())
              .AddToAllFeatures(SABattleMind())
              .Configure();
        }

        private const string KineticEsoterica = "EsotericKnight.KineticEsoterica";
        private static readonly string KineticEsotericaGuid = "{EEA55FBD-5E91-4D9C-9EB6-C2E24CD41C87}";

        internal const string KineticEsotericaDisplayName = "EsotericKnightKineticEsoterica.Name";
        private const string KineticEsotericaDescription = "EsotericKnightKineticEsoterica.Description";

        private static readonly string FeatNamePro2 = "KineticEsotericaPro2";
        public static readonly string FeatGuidPro2 = "{AD71F26B-E83E-424F-9129-D7F38DFCB6CC}";
        public static BlueprintProgression KineticEsotericaFeat()
        {
            var icon = FeatureSelectionRefs.WildTalentSelection.Reference.Get().Icon;

            var featreal = FeatureConfigurator.New(FeatNamePro2, FeatGuidPro2)
                    .SetDisplayName(KineticEsotericaDisplayName)
                    .SetDescription(KineticEsotericaDescription)
                    .SetIcon(icon)
                    .AddComponent<KineticProgressionContinue>()
                    .SetRanks(20)
                    .SetHideInUI(true)
                    .Configure();

            ProgressionConfigurator.For(ProgressionRefs.KineticBlastProgression)
                .AddToFeaturesRankIncrease(featreal)
                .SetGiveFeaturesForPreviousLevels(true)
                .Configure();

            return ProgressionConfigurator.New(KineticEsoterica, KineticEsotericaGuid)
              .SetDisplayName(KineticEsotericaDisplayName)
              .SetDescription(KineticEsotericaDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteFeature(FeatureRefs.KineticBlastFeature.ToString())
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.KineticistClass.ToString(), modifier: 0.5, summand: 0)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(2, KineticSelectionGuid, featreal)
              .AddToLevelEntry(4, KineticSelectionGuid, featreal)
              .AddToLevelEntry(6, KineticSelectionGuid, featreal)
              .AddToLevelEntry(8, KineticSelectionGuid, featreal)
              .AddToLevelEntry(10, KineticSelectionGuid, featreal)
              .Configure();
        }

        private const string KnightSelection = "EsotericKnight.KnightSelection";
        private static readonly string KnightSelectionGuid = "{757E4730-C99F-4D9B-B795-BD927DA4F769}";

        internal const string KnightSelectionDisplayName = "EsotericKnightSelection.Name";
        private const string KnightSelectionDescription = "EsotericKnightSelection.Description";

        public static void KnightSelectionFeat()
        {
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(KnightSelection, KnightSelectionGuid)
              .SetDisplayName(KnightSelectionDisplayName)
              .SetDescription(KnightSelectionDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(MartialEsotericaFeat())
              .AddToAllFeatures(spellupgradeGuid)
              .AddToAllFeatures(KineticEsotericaFeat())
              .Configure();
        }

        private const string MentalSelection = "EsotericKnight.MentalSelection";
        private static readonly string MentalSelectionGuid = "{F4D9F946-21EC-44F7-B632-8BDEB46E4A8C}";

        internal const string MentalSelectionDisplayName = "EsotericMentalSelection.Name";
        private const string MentalSelectionDescription = "EsotericMentalSelection.Description";

        public static void MentalSelectionFeat()
        {
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(MentalSelection, MentalSelectionGuid)
              .SetDisplayName(MentalSelectionDisplayName)
              .SetDescription(MentalSelectionDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.CombatCasting.ToString())
              .AddToAllFeatures(FeatureRefs.ExtraRage.ToString())
              .AddToAllFeatures(FeatureRefs.ExtraRageInstinctualWarrior.ToString())
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(ParametrizedFeatureRefs.SpellFocus.ToString())
              .AddToAllFeatures(FeatureRefs.SpellPenetration.ToString())
              .Configure();
        }

        private const string PsychicEsoterica = "EsotericKnight.PsychicEsoterica";
        private static readonly string PsychicEsotericaGuid = "{75DE86C3-429E-4225-BD80-455081044BA1}";

        internal const string PsychicEsotericaDisplayName = "PsychicEsoterica.Name";
        private const string PsychicEsotericaDescription = "PsychicEsoterica.Description";

        public static void PsychicEsotericaFeat()
        {
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(PsychicEsoterica, PsychicEsotericaGuid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(MentalSelectionGuid)
              .AddToAllFeatures(ConfoundingProjectionFeat())
              .Configure();
        }

        private const string BattleMind = "EsotericKnight.BattleMind";
        private static readonly string BattleMindGuid = "{590B27BE-7F6A-4F03-9440-16AA4FDB7913}";

        internal const string EsotericKnightBattleMindDisplayName = "EsotericKnightBattleMind.Name";
        private const string EsotericKnightBattleMindDescription = "EsotericKnightBattleMind.Description";
        public static BlueprintFeature SABattleMind()
        {
            var icon = FeatureRefs.IntimidatingProwess.Reference.Get().Icon;
            return FeatureConfigurator.New(BattleMind, BattleMindGuid)
              .SetDisplayName(EsotericKnightBattleMindDisplayName)
              .SetDescription(EsotericKnightBattleMindDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BurnPerRoundResource.ToString(), ContextValues.Rank())
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BurnResource.ToString(), ContextValues.Rank())
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.DarkElementalistBurnResource.ToString(), ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(BattleMindGuid))
              .Configure();
        }

        private const string ConfoundingProjection = "EsotericKnight.ConfoundingProjection";
        public static readonly string ConfoundingProjectionGuid = "{EB739899-2921-46BD-AC91-EA71095D6249}";

        private const string ConfoundingProjectionAblity = "EsotericKnight.UseConfoundingProjection";
        private static readonly string ConfoundingProjectionAblityGuid = "{4C3E9C7A-DFDC-482C-8BC2-E95DF2FD6711}";

        private const string ConfoundingProjectionAblity2 = "EsotericKnight.UseConfoundingProjection2";
        public static readonly string ConfoundingProjectionAblity2Guid = "{3D8B1234-3971-44F6-94B6-33CC19BEB2BD}";

        private const string ConfoundingProjectionBuff = "EsotericKnight.ConfoundingProjectionBuff";
        public static readonly string ConfoundingProjectionBuffGuid = "{2A9BBC9C-DA70-4BA5-90B8-693CBC0B9CB9}";

        private const string ConfoundingProjectionBuff2 = "EsotericKnight.ConfoundingProjectionBuff2";
        private static readonly string ConfoundingProjectionBuff2Guid = "{774ACA8E-BF0B-49BA-AF53-DACE39B49AF8}";

        internal const string ConfoundingProjectionDisplayName = "EsotericKnightConfoundingProjection.Name";
        private const string ConfoundingProjectionDescription = "EsotericKnightConfoundingProjection.Description";

        public static BlueprintFeature ConfoundingProjectionFeat()
        {
            var icon = AbilityRefs.CallLightning.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(ConfoundingProjectionBuff2, ConfoundingProjectionBuff2Guid)
             .SetDisplayName(ConfoundingProjectionDisplayName)
             .SetDescription(ConfoundingProjectionDescription)
             .SetIcon(icon)
             .AddFacts(new() { ConfoundingProjectionAblity2Guid })
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            var Buff1 = BuffConfigurator.New(ConfoundingProjectionBuff, ConfoundingProjectionBuffGuid)
             .SetDisplayName(ConfoundingProjectionDisplayName)
             .SetDescription(ConfoundingProjectionDescription)
             .SetIcon(icon)
             //always flanked
             .Configure();

            var ability2 = AbilityConfigurator.New(ConfoundingProjectionAblity2, ConfoundingProjectionAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Fixed(1))
                        .Build())
                .SetDisplayName(ConfoundingProjectionDisplayName)
                .SetDescription(ConfoundingProjectionDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
                .SetCanTargetSelf(false)
                .SetCanTargetEnemies(true)
                .Configure();

            var ability = AbilityConfigurator.New(ConfoundingProjectionAblity, ConfoundingProjectionAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .Build())
                .SetDisplayName(ConfoundingProjectionDisplayName)
                .SetDescription(ConfoundingProjectionDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityKineticist(1, wildTalentBurnCost: 1)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(ConfoundingProjection, ConfoundingProjectionGuid)
              .SetDisplayName(ConfoundingProjectionDisplayName)
              .SetDescription(ConfoundingProjectionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, ability2, holder, holder, holder, holder, holder, holder, holder, holder })
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string StepthroughReality = "EsotericKnight.StepthroughReality";
        public static readonly string StepthroughRealityGuid = "{AC8AC4BA-D54C-411B-90AD-217A3B2849AD}";

        private const string StepthroughRealityAblity = "EsotericKnight.UseStepthroughReality";
        private static readonly string StepthroughRealityAblityGuid = "{70384D2E-C5E2-43BD-960D-67041AEDAAAE}";

        private const string StepthroughRealityAblity2 = "EsotericKnight.UseStepthroughReality2";
        public static readonly string StepthroughRealityAblity2Guid = "{DC6085B8-0B21-4665-A5CC-6B5AF614AB6C}";

        private const string StepthroughRealityBuff2 = "EsotericKnight.StepthroughRealityBuff2";
        private static readonly string StepthroughRealityBuff2Guid = "{A38BCB8B-D91F-4BCC-8944-B3ED4C116F45}";

        internal const string StepthroughRealityDisplayName = "EsotericKnightStepthroughReality.Name";
        private const string StepthroughRealityDescription = "EsotericKnightStepthroughReality.Description";

        public static BlueprintFeature StepthroughRealityFeat()
        {
            var icon = AbilityRefs.CallLightning.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(StepthroughRealityBuff2, StepthroughRealityBuff2Guid)
             .SetDisplayName(StepthroughRealityDisplayName)
             .SetDescription(StepthroughRealityDescription)
             .SetIcon(icon)
             .AddFacts(new() { StepthroughRealityAblity2Guid })
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            var ability2 = AbilityConfigurator.New(StepthroughRealityAblity2, StepthroughRealityAblity2Guid)
                .CopyFrom(
                AbilityRefs.DimensionDoor,
                typeof(SpellComponent),
                typeof(AbilityCustomDimensionDoor))
                .Configure();

            var ability = AbilityConfigurator.New(StepthroughRealityAblity, StepthroughRealityAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .Build())
                .SetDisplayName(StepthroughRealityDisplayName)
                .SetDescription(StepthroughRealityDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityKineticist(2, wildTalentBurnCost: 2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(StepthroughReality, StepthroughRealityGuid)
              .SetDisplayName(StepthroughRealityDisplayName)
              .SetDescription(StepthroughRealityDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, holder, holder, ability2, holder, holder, holder, holder, holder, holder })
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
