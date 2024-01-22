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
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.PrestigeClass;
using Kingmaker.AreaLogic.SummonPool;
using PrestigePlus.HarmonyFix;
using PrestigePlus.CustomComponent;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class EsotericKnight
    {
        private const string ArchetypeName = "EsotericKnight";
        public static readonly string ArchetypeGuid = "{0829A992-9641-49ED-BE17-47E6BA023716}";
        internal const string ArchetypeDisplayName = "EsotericKnight.Name";
        private const string ArchetypeDescription = "EsotericKnight.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "EsotericKnightPrestige";
        private static readonly string ClassProgressGuid = "{91A782A8-AE78-4297-A507-E68D39F3C209}";
        public static void Configure()
        {
            KnightSelectionFeat(); MentalSelectionFeat(); PsychicEsotericaFeat();
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, KnightSelectionGuid, KnightSelectionGuid, CreateProficiencies())
            .AddToLevelEntry(2, PsychicEsotericaGuid)
            .AddToLevelEntry(4, PsychicEsotericaGuid)
            .AddToLevelEntry(6, PsychicEsotericaGuid)
            .AddToLevelEntry(8, PsychicEsotericaGuid)
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
            .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillLoreReligion, StatType.SkillKnowledgeArcana, StatType.SkillMobility })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
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
              .AddFacts(new() { FeatureRefs.TowerShieldProficiency.ToString()})
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

        private const string KineticEsoterica = "EsotericKnight.KineticEsoterica";
        private static readonly string KineticEsotericaGuid = "{EEA55FBD-5E91-4D9C-9EB6-C2E24CD41C87}";

        internal const string KineticEsotericaDisplayName = "EsotericKnightKineticEsoterica.Name";
        private const string KineticEsotericaDescription = "EsotericKnightKineticEsoterica.Description";

        private static readonly string FeatNamePro2 = "KineticEsotericaPro2";
        public static readonly string FeatGuidPro2 = "{AD71F26B-E83E-424F-9129-D7F38DFCB6CC}";
        public static BlueprintProgression KineticEsotericaFeat()
        {
            var icon = AbilityRefs.KnowledgeDomainGreaterAbility.Reference.Get().Icon;

            var featreal = FeatureConfigurator.New(FeatNamePro2, FeatGuidPro2)
                    .SetDisplayName(KineticEsotericaDisplayName)
                    .SetDescription(KineticEsotericaDescription)
                    .SetIcon(icon)
                    .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.Reference; })
                    .SetRanks(20)
                    .SetHideInUI(true)
                    .Configure();

            return ProgressionConfigurator.New(KineticEsoterica, KineticEsotericaGuid)
              .SetDisplayName(KineticEsotericaDisplayName)
              .SetDescription(KineticEsotericaDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
              .AddToLevelEntry(2, featreal)
              .AddToLevelEntry(4, featreal)
              .AddToLevelEntry(6, featreal)
              .AddToLevelEntry(8, featreal)
              .AddToLevelEntry(10, featreal)
              .Configure();
        }

        private const string DivinePsychic = "EsotericKnight.DivinePsychic";
        private static readonly string DivinePsychicGuid = "{D715CE8C-0E09-4FCD-9BB2-305D06EEF459}";

        internal const string DivinePsychicDisplayName = "EsotericKnightDivinePsychic.Name";
        private const string DivinePsychicDescription = "EsotericKnightDivinePsychic.Description";

        private static readonly string FeatNamePro3 = "DivinePsychicPro2";
        public static readonly string FeatGuidPro3 = "{62390821-FCDC-4230-B4C5-01F6BDDD3A7E}";
        public static BlueprintProgression DivinePsychicFeat()
        {
            var icon = AbilityRefs.PolarMidnight.Reference.Get().Icon;

            var featreal = FeatureConfigurator.New(FeatNamePro3, FeatGuidPro3)
                    .SetDisplayName(DivinePsychicDisplayName)
                    .SetDescription(DivinePsychicDescription)
                    .SetIcon(icon)
                    .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.Reference; })
                    .SetRanks(20)
                    .SetHideInUI(true)
                    .Configure();

            return ProgressionConfigurator.New(DivinePsychic, DivinePsychicGuid)
              .SetDisplayName(DivinePsychicDisplayName)
              .SetDescription(DivinePsychicDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1)
              .AddToLevelEntry(2, featreal)
              .AddToLevelEntry(4, featreal)
              .AddToLevelEntry(6, featreal)
              .AddToLevelEntry(8, featreal)
              .AddToLevelEntry(10, featreal)
              .Configure();
        }

        private const string KnightSelection = "EsotericKnight.KnightSelection";
        private static readonly string KnightSelectionGuid = "{757E4730-C99F-4D9B-B795-BD927DA4F769}";

        internal const string KnightSelectionDisplayName = "EsotericKnightSelection.Name";
        private const string KnightSelectionDescription = "EsotericKnightSelection.Description";

        public static void KnightSelectionFeat()
        {
            var icon = AbilityRefs.KnowledgeDomainGreaterAbility.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(KnightSelection, KnightSelectionGuid)
              .SetDisplayName(KnightSelectionDisplayName)
              .SetDescription(KnightSelectionDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(MartialEsotericaFeat())
              .AddToAllFeatures(KineticEsotericaFeat())
              .AddToAllFeatures(DivinePsychicFeat())
              .Configure();
        }

        private const string MentalSelection = "EsotericKnight.MentalSelection";
        private static readonly string MentalSelectionGuid = "{F4D9F946-21EC-44F7-B632-8BDEB46E4A8C}";

        internal const string MentalSelectionDisplayName = "EsotericMentalSelection.Name";
        private const string MentalSelectionDescription = "EsotericMentalSelection.Description";

        public static void MentalSelectionFeat()
        {
            var icon = AbilityRefs.LifeBubble.Reference.Get().Icon;

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
            var icon = AbilityRefs.LifeBubble.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(PsychicEsoterica, PsychicEsotericaGuid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(MentalSelectionGuid)
              .AddToAllFeatures(PsychicDaringFeat())
              .AddToAllFeatures(PsychicRageFeat())
              .AddToAllFeatures(ConfoundingProjectionFeat())
              .AddToAllFeatures(PhantomArmoryFeat())
              .AddToAllFeatures(ShadowProjectionFeat())
              .AddToAllFeatures(StepthroughRealityFeat())
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
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

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
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
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
            var icon = AbilityRefs.DimensionDoorCasterOnly.Reference.Get().Icon;

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
                AbilityRefs.DimensionDoorCasterOnly,
                typeof(SpellComponent),
                typeof(AbilityCustomDimensionDoor))
                .SetDisplayName(StepthroughRealityDisplayName)
                .SetDescription(StepthroughRealityDescription)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            var ability = AbilityConfigurator.New(StepthroughRealityAblity, StepthroughRealityAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
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

        private const string PsychicRage = "EsotericKnight.PsychicRage";
        public static readonly string PsychicRageGuid = "{279288A3-F361-416B-B2C9-287BB436D273}";

        private const string PsychicRageAblity = "EsotericKnight.UsePsychicRage";
        private static readonly string PsychicRageAblityGuid = "{A7FE50E5-B247-40FD-8575-7C166A1240D9}";

        private const string PsychicRageAblity2 = "EsotericKnight.UsePsychicRage2";
        public static readonly string PsychicRageAblity2Guid = "{994FBCA3-A3A9-462C-B399-A65A52673CB0}";

        internal const string PsychicRageDisplayName = "EsotericKnightPsychicRage.Name";
        private const string PsychicRageDescription = "EsotericKnightPsychicRage.Description";

        public static BlueprintFeature PsychicRageFeat()
        {
            var icon = FeatureRefs.FocusedRageFeature.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(PsychicRageAblity2, PsychicRageAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Add<SpellSlotToRes>(c => { c.ResGuid = AbilityResourceRefs.RageResourse.ToString(); })
                        .Add<SpellSlotToRes>(c => { c.ResGuid = AbilityResourceRefs.RageshaperShifterResource.ToString(); })
                        .Add<SpellSlotToRes>(c => { c.ResGuid = AbilityResourceRefs.BloodragerRageResource.ToString(); })
                        .Add<SpellSlotToRes>(c => { c.ResGuid = AbilityResourceRefs.FocusedRageResourse.ToString(); })
                        .Add<SpellSlotToRes>(c => { c.ResGuid = AbilityResourceRefs.DemonRageResource.ToString(); })
                        .Build())
                .SetDisplayName(PsychicRageDisplayName)
                .SetDescription(PsychicRageDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var ability = AbilityConfigurator.New(PsychicRageAblity, PsychicRageAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .RestoreResource(AbilityResourceRefs.RageResourse.ToString(), 4)
                        .RestoreResource(AbilityResourceRefs.BloodragerRageResource.ToString(), 4)
                        .RestoreResource(AbilityResourceRefs.FocusedRageResourse.ToString(), 4)
                        .RestoreResource(AbilityResourceRefs.DemonRageResource.ToString(), 4)
                        .Build())
                .SetDisplayName(PsychicRageDisplayName)
                .SetDescription(PsychicRageDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityKineticist(2, wildTalentBurnCost: 2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(PsychicRage, PsychicRageGuid)
              .SetDisplayName(PsychicRageDisplayName)
              .SetDescription(PsychicRageDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2 })
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string PsychicDaring = "EsotericKnight.PsychicDaring";
        public static readonly string PsychicDaringGuid = "{A40C8449-7751-4072-94A4-2A6227C90B1B}";

        private const string PsychicDaringAblity = "EsotericKnight.UsePsychicDaring";
        private static readonly string PsychicDaringAblityGuid = "{03D83F58-1656-45D7-BB95-94E188546396}";

        private const string PsychicDaringAblity2 = "EsotericKnight.UsePsychicDaring2";
        public static readonly string PsychicDaringAblity2Guid = "{5F11D0BD-0DD3-4101-BBB3-78E81DB17501}";

        internal const string PsychicDaringDisplayName = "EsotericKnightPsychicDaring.Name";
        private const string PsychicDaringDescription = "EsotericKnightPsychicDaring.Description";

        public static BlueprintFeature PsychicDaringFeat()
        {
            var icon = AbilityRefs.Heroism.Reference.Get().Icon;
            var swashres = "AC63BFCF-EC31-43DC-A5CE-04617A3BC854";

            var ability2 = AbilityConfigurator.New(PsychicDaringAblity2, PsychicDaringAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Add<SpellSlotToRes>(c => { c.ResGuid = swashres; })
                        .Build())
                .SetDisplayName(PsychicDaringDisplayName)
                .SetDescription(PsychicDaringDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var ability = AbilityConfigurator.New(PsychicDaringAblity, PsychicDaringAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .RestoreResource(swashres, 4)
                        .Build())
                .SetDisplayName(PsychicDaringDisplayName)
                .SetDescription(PsychicDaringDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityKineticist(2, wildTalentBurnCost: 2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(PsychicDaring, PsychicDaringGuid)
              .SetDisplayName(PsychicDaringDisplayName)
              .SetDescription(PsychicDaringDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2 })
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string PhantomArmory = "EsotericKnight.PhantomArmory";
        public static readonly string PhantomArmoryGuid = "{3ED4BC97-F870-485B-9087-63D20A033885}";

        private const string PhantomArmoryAblity = "EsotericKnight.UsePhantomArmory";
        private static readonly string PhantomArmoryAblityGuid = "{A2D08649-80CE-456D-8E73-972327997102}";

        private const string PhantomArmoryAblity2 = "EsotericKnight.UsePhantomArmory2";
        public static readonly string PhantomArmoryAblity2Guid = "{F8D140DA-63F0-4DAA-AC26-28A6227F475C}";

        private const string PhantomArmoryBuff2 = "EsotericKnight.PhantomArmoryBuff2";
        private static readonly string PhantomArmoryBuff2Guid = "{851D405A-BE48-4E81-84A6-9F101986499E}";

        internal const string PhantomArmoryDisplayName = "EsotericKnightPhantomArmory.Name";
        private const string PhantomArmoryDescription = "EsotericKnightPhantomArmory.Description";

        public static BlueprintFeature PhantomArmoryFeat()
        {
            var icon = FeatureRefs.DestructionDomainBaseFeature.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(PhantomArmoryBuff2, PhantomArmoryBuff2Guid)
             .SetDisplayName(PhantomArmoryDisplayName)
             .SetDescription(PhantomArmoryDescription)
             .SetIcon(icon)
             .AddComponent<GhostArmorComponent>()
             .Configure();

            var ability2 = AbilityConfigurator.New(PhantomArmoryAblity2, PhantomArmoryAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(PhantomArmoryDisplayName)
                .SetDescription(PhantomArmoryDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            var ability = AbilityConfigurator.New(PhantomArmoryAblity, PhantomArmoryAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(PhantomArmoryDisplayName)
                .SetDescription(PhantomArmoryDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityKineticist(2, wildTalentBurnCost: 2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(PhantomArmory, PhantomArmoryGuid)
              .SetDisplayName(PhantomArmoryDisplayName)
              .SetDescription(PhantomArmoryDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, holder, ability2, holder, holder, holder, holder, holder, holder, holder })
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string ShadowProjection = "EsotericKnight.ShadowProjection";
        public static readonly string ShadowProjectionGuid = "{C72C31B1-E202-49AC-B742-D1E572EA48DA}";

        private const string ShadowProjectionAblity = "EsotericKnight.UseShadowProjection";
        private static readonly string ShadowProjectionAblityGuid = "{953B5DEB-393C-4BCB-B992-548152CD14A2}";

        private const string ShadowProjectionAblity2 = "EsotericKnight.UseShadowProjection2";
        public static readonly string ShadowProjectionAblity2Guid = "{E711BC72-802B-4D8A-8255-8118327B5787}";

        private const string ShadowProjectionBuff2 = "EsotericKnight.ShadowProjectionBuff2";
        private static readonly string ShadowProjectionBuff2Guid = "{D391CB63-1FE8-4C61-8B46-63CD47189F2B}";

        internal const string ShadowProjectionDisplayName = "EsotericKnightShadowProjection.Name";
        private const string ShadowProjectionDescription = "EsotericKnightShadowProjection.Description";

        public static BlueprintFeature ShadowProjectionFeat()
        {
            var icon = AbilityRefs.Scare.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(ShadowProjectionBuff2, ShadowProjectionBuff2Guid)
             .SetDisplayName(ShadowProjectionDisplayName)
             .SetDescription(ShadowProjectionDescription)
             .SetIcon(icon)
             .AddFacts(new() { ShadowProjectionAblity2Guid })
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            var ability2 = AbilityConfigurator.New(ShadowProjectionAblity2, ShadowProjectionAblity2Guid)
                .SetDisplayName(ShadowProjectionDisplayName)
                .SetDescription(ShadowProjectionDescription)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Close)
                .SetCanTargetPoint(true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }))
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ClearSummonPool(ShadowDancer.SummonPoolGuid)
                    .SpawnMonsterUsingSummonPool(countValue: ContextDice.Value(DiceType.One),
                        durationValue: ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes),
                        monster: UnitRefs.CR3_Shadow.ToString(),
                        summonPool: ShadowDancer.SummonPoolGuid,
                        afterSpawn: ActionsBuilder.New()
                            .ApplyBuffPermanent(ShadowDancer.ShadowEnhanceGuidBuff)
                            .ApplyBuffPermanent(ShadowDancer.ShadowEnhanceGuidBuff2)
                            .ApplyBuffPermanent(ShadowDancer.ShadowEnhanceGuidBuff3)
                            .ApplyBuffPermanent(ShadowDancer.ShadowEnhanceGuidBuff4)
                            .ApplyBuffPermanent(ShadowDancer.ShadowEnhanceGuidBuff5)
                            .Build()))
                .Configure();

            var ability = AbilityConfigurator.New(ShadowProjectionAblity, ShadowProjectionAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                        .Build())
                .SetDisplayName(ShadowProjectionDisplayName)
                .SetDescription(ShadowProjectionDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityKineticist(2, wildTalentBurnCost: 2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var holder = ArchetypeGuid;

            return FeatureConfigurator.New(ShadowProjection, ShadowProjectionGuid)
              .SetDisplayName(ShadowProjectionDisplayName)
              .SetDescription(ShadowProjectionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holder, new() { holder, holder, holder, ability2, holder, holder, holder, holder, holder, holder })
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
