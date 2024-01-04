using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.UnitLogic.Alignments;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using static TabletopTweaks.Core.NewUnitParts.CustomStatTypes;
using BlueprintCore.Conditions.Builder;
using static Dreamteck.Splines.SplineMesh;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic;
using PrestigePlus.Modify;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.UnitLogic.Buffs;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomComponent.PrestigeClass;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class UmbralAgent
    {
        private const string ArchetypeName = "UmbralAgent";
        public static readonly string ArchetypeGuid = "{18CA6378-AC25-4580-B684-31F929189459}";
        internal const string ArchetypeDisplayName = "UmbralAgent.Name";
        private const string ArchetypeDescription = "UmbralAgent.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "UmbralAgentPrestige";
        private static readonly string ClassProgressGuid = "{4B23C85C-1859-4878-AEE0-83587526ABCA}";

        private static readonly string ShadowJumpGuidFeat = "29C89DAC-EF33-489E-B3D6-3E15D68D4326";
        public static void Configure()
        {
            TenebrousFeat(); SAShadowChains(); GazeConfigure();
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, spellupgradeGuid, CreateProficiencies(), SABlessingDarkness(), TenebrousGuid, SAUmbralCourtier())
                .AddToLevelEntry(2, FeatureRefs.BlindFight.ToString())
                .AddToLevelEntry(3, ShadowChainsGuid)
                .AddToLevelEntry(4, ShadowJumpGuidFeat)
                .AddToLevelEntry(5, GazeGuid)
                .AddToLevelEntry(6, ShadowChains2Guid, ShadowJumpGuidFeat)
                .AddToLevelEntry(7, Gaze2Guid)
                .AddToLevelEntry(8, ShadowJumpGuidFeat)
                .AddToLevelEntry(9, Gaze3Guid)
                .AddToLevelEntry(10, ShadowChains3Guid, ShadowJumpGuidFeat)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { spellupgradeGuid, ShadowChainsGuid, ShadowChains2Guid, ShadowChains3Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { TenebrousGuid, GazeGuid, Gaze2Guid, Gaze3Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { ShadowJumpGuidFeat, ProficienciesGuid }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .AddSkipLevelsForSpellProgression(new int[] { 5, 9 })
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillUseMagicDevice, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 7)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 7)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 5)
                .AddPrerequisiteAlignment(AlignmentMaskType.Evil)
                .AddPrerequisiteFeature(FeatureRefs.Deceitful.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.SkillFocusThievery.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.SkillFocusDiplomacy.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddComponent<PrerequisiteCasterLevel>(c => { c.RequiredCasterLevel = 2; })
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var UmbralAgentref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = UmbralAgentref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Proficiencies = "UmbralAgent.Proficiencies";
        private static readonly string ProficienciesGuid = "{BEF4CC4E-DC23-45A1-9166-6D0E6A9FD288}";
        internal const string ProficienciesDisplayName = "UmbralAgentProficiencies.Name";
        private const string ProficienciesDescription = "UmbralAgentProficiencies.Description";

        private static readonly string ShadowJumpAblityGuid = "E1A86DF3-38E6-4E78-82B9-4EF7A6436BA8";
        private static readonly string ShadowJumpAblityGuid2 = "{8ED8B86D-A5DB-46FE-B88A-05098198041A}";
        private static readonly string ShadowJumpAblityResGuid = "E96F5710-5344-495F-A3C7-A4859C1ABF81";

        public static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddFacts(new() { ShadowJumpAblityGuid, ShadowJumpAblityGuid2 })
              .AddAbilityResources(resource: ShadowJumpAblityResGuid, restoreAmount: true)
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.SlingStaff,
                  })
              .Configure();
        }

        private const string Tenebrous = "UmbralAgent.Tenebrous";
        private static readonly string TenebrousGuid = "{3096368F-9B6A-4D06-9F47-4C2E17EF852B}";

        private const string TenebrousBuff = "UmbralAgent.TenebrousBuff";
        private static readonly string TenebrousBuffGuid = "{D32CC7CB-8AE7-4698-850E-7F6FB7A0507F}";

        internal const string TenebrousDisplayName = "UmbralAgentTenebrous.Name";
        private const string TenebrousDescription = "UmbralAgentTenebrous.Description";

        public static void TenebrousFeat()
        {
            var icon = AbilityRefs.Thoughtsense.Reference.Get().Icon;

            var MasterBuff = BuffConfigurator.New(TenebrousBuff, TenebrousBuffGuid)
              .SetDisplayName(TenebrousDisplayName)
              .SetDescription(TenebrousDescription)
              .SetIcon(icon)
              .AddIncreaseCasterLevel(value: ContextValues.Constant(2))
              .AddIncreaseAllSpellsDC(value: ContextValues.Constant(2))
              .SetStacking(StackingType.Prolong)
              .Configure();

            var action = ActionsBuilder.New()
            .ApplyBuff(MasterBuff, durationValue: ContextDuration.VariableDice(DiceType.D4, 1))
            .Build();

            FeatureSelectionConfigurator.New(Tenebrous, TenebrousGuid)
              .SetDisplayName(TenebrousDisplayName)
              .SetDescription(TenebrousDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddAbilityUseTrigger(action: action, checkSpellSchool: true, isSpellSchool: SpellSchool.Illusion)
              //.AddAbilityUseTrigger(action: action, checkAbilityType: true, type: AbilityType.SpellLike)
              .AddToAllFeatures(FeatureRefs.BolsteredSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.EmpowerSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.ExtendSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.HeightenSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.MaximizeSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.PersistentSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.QuickenSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.ReachSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.SelectiveSpellFeat.ToString())
              .Configure();
        }

        private const string UmbralCourtier = "UmbralAgent.UmbralCourtier";
        private static readonly string UmbralCourtierGuid = "{AA35026D-1881-480F-A3D4-EA33B587A692}";

        internal const string UmbralAgentUmbralCourtierDisplayName = "UmbralAgentUmbralCourtier.Name";
        private const string UmbralAgentUmbralCourtierDescription = "UmbralAgentUmbralCourtier.Description";
        public static BlueprintFeature SAUmbralCourtier()
        {
            var icon = FeatureRefs.Persuasive.Reference.Get().Icon;
            return FeatureConfigurator.New(UmbralCourtier, UmbralCourtierGuid)
              .SetDisplayName(UmbralAgentUmbralCourtierDisplayName)
              .SetDescription(UmbralAgentUmbralCourtierDescription)
              .SetIcon(icon)
              .AddContextStatBonus(stat: StatType.CheckBluff, value: ContextValues.Rank(), descriptor: ModifierDescriptor.Competence)
              .AddContextStatBonus(stat: StatType.CheckDiplomacy, value: ContextValues.Rank(), descriptor: ModifierDescriptor.Competence)
              .AddContextStatBonus(stat: StatType.SkillKnowledgeWorld, value: ContextValues.Rank(), descriptor: ModifierDescriptor.Competence)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string BlessingDarkness = "UmbralAgent.BlessingDarkness";
        private static readonly string BlessingDarknessGuid = "{E5512A80-4A17-4490-AAC9-0EBB1DB37DD1}";

        internal const string UmbralAgentBlessingDarknessDisplayName = "UmbralAgentBlessingDarkness.Name";
        private const string UmbralAgentBlessingDarknessDescription = "UmbralAgentBlessingDarkness.Description";
        public static BlueprintFeatureSelection SABlessingDarkness()
        {
            var icon = AbilityRefs.DarknessDomainBaseAbility.Reference.Get().Icon;

            ProgressionConfigurator.For(ProgressionRefs.DarknessDomainProgression)
              .SetClasses(new Blueprint<BlueprintCharacterClassReference>[] { CharacterClassRefs.ClericClass.Reference.Get(), CharacterClassRefs.InquisitorClass.Reference.Get(), CharacterClassRefs.HunterClass.Reference.Get(), BlueprintTool.GetRef<BlueprintCharacterClassReference>(ArchetypeGuid) })
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DarknessDomainProgressionDruid)
              .SetClasses(new Blueprint<BlueprintCharacterClassReference>[] { CharacterClassRefs.DruidClass.Reference.Get(), BlueprintTool.GetRef<BlueprintCharacterClassReference>(ArchetypeGuid) })
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DarknessDomainProgressionSecondary)
              .SetClasses(new Blueprint<BlueprintCharacterClassReference>[] { CharacterClassRefs.ClericClass.Reference.Get(), BlueprintTool.GetRef<BlueprintCharacterClassReference>(ArchetypeGuid) })
              .Configure();

            return FeatureSelectionConfigurator.New(BlessingDarkness, BlessingDarknessGuid)
              .SetDisplayName(UmbralAgentBlessingDarknessDisplayName)
              .SetDescription(UmbralAgentBlessingDarknessDescription)
              .SetIcon(icon)
              .AddNoSelectionIfAlreadyHasFeature(true)
              .AddToAllFeatures(ProgressionRefs.DarknessDomainProgression.ToString())
              .AddToAllFeatures(ProgressionRefs.DarknessDomainProgressionSecondary.ToString())
              .AddToAllFeatures(ProgressionRefs.DarknessDomainProgressionDruid.ToString())
              .SetIgnorePrerequisites(true)
              //.AddFacts(new() { ProgressionRefs.DarknessDomainProgression.ToString(), ProgressionRefs.DarknessDomainProgressionDruid.ToString(), ProgressionRefs.DarknessDomainProgressionSecondary.ToString() })
              .Configure();
        }

        private const string ShadowChains = "UmbralAgent.ShadowChains";
        private static readonly string ShadowChainsGuid = "{FD5F4479-47CC-4C86-91CA-B529CF2D01FD}";

        internal const string UmbralAgentShadowChainsDisplayName = "UmbralAgentShadowChains.Name";
        private const string UmbralAgentShadowChainsDescription = "UmbralAgentShadowChains.Description";

        private const string ShadowChains2 = "UmbralAgent.ShadowChains2";
        private static readonly string ShadowChains2Guid = "{16C6F014-064B-4BC8-B1BA-9DEBC24D9979}";

        private const string ShadowChains2Ability = "UmbralAgent.ShadowChains2Ability";
        private static readonly string ShadowChains2AbilityGuid = "{DA52C8D2-833C-4D6D-B689-BC971B15D4BC}";

        private const string ShadowChains2Buff = "UmbralAgent.ShadowChains2Buff";
        private static readonly string ShadowChains2BuffGuid = "{2CD60E61-7533-4C18-8747-CB8F7E1CDB50}";

        internal const string UmbralAgentShadowChains2DisplayName = "UmbralAgentShadowChains2.Name";
        private const string UmbralAgentShadowChains2Description = "UmbralAgentShadowChains2.Description";

        private const string ShadowChains3 = "UmbralAgent.ShadowChains3";
        private static readonly string ShadowChains3Guid = "{8146D8AA-C4CA-4924-BE9D-999BF59A139E}";

        internal const string UmbralAgentShadowChains3DisplayName = "UmbralAgentShadowChains3.Name";
        private const string UmbralAgentShadowChains3Description = "UmbralAgentShadowChains3.Description";
        public static void SAShadowChains()
        {
            var icon = AbilityRefs.DarknessDomainBaseAbility.Reference.Get().Icon;

            var chain = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(ShadowChainsGuid).CasterHasFact(ShadowChains2BuffGuid, true).Build(), ifTrue: ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Reflex, useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()//.HealTarget(ContextDice.Value(DiceType.One, ContextValues.Constant(1)))
                        .ApplyBuff(BuffRefs.EntangleBuff.ToString(), durationValue: ContextDuration.FixedDice(diceType: DiceType.D4)).Build()).Build())
                    .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(ShadowChains2BuffGuid).Build(), ifTrue: ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Reflex, useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                        .Add<KnotGrapple>(c => { c.isAway = false; }).Build()).Build())
                    .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(ShadowChains3Guid).Build(), ifTrue: ActionsBuilder.New()
                    .OnContextCaster(ActionsBuilder.New().RestoreResource(AbilityResourceRefs.DarknessDomainBaseResource.ToString(), 1).Build())
                    .Build())
                .Build();

            AbilityConfigurator.For(AbilityRefs.DarknessDomainBaseAbility)
              .EditComponent<AbilityEffectRunAction>(
                a => a.Actions.Actions = CommonTool.Append(a.Actions.Actions, chain.Actions))
              .AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Wisdom; })
              .Configure();

            FeatureConfigurator.New(ShadowChains, ShadowChainsGuid)
              .SetDisplayName(UmbralAgentShadowChainsDisplayName)
              .SetDescription(UmbralAgentShadowChainsDescription)
              .SetIcon(icon)
              .AddIncreaseResourceAmount(AbilityResourceRefs.DarknessDomainBaseResource.ToString(), 1)
              .AddComponent<AddStatBonus>(c =>
              {
                  c.Stat = CustomStatType.MeleeTouchReach.Stat();
                  c.Value = 5;
                  c.Descriptor = ModifierDescriptor.Feat;
              })
              .Configure();

            var Buff = BuffConfigurator.New(ShadowChains2Buff, ShadowChains2BuffGuid)
                .SetDisplayName(UmbralAgentShadowChains2DisplayName)
                .SetDescription(UmbralAgentShadowChains2Description)
                .SetIcon(icon)
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .AddComponent<ReplaceSingleCombatManeuverStat>(c => { c.Type = Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple; c.StatType = StatType.Wisdom; })
                //.AddReplaceSingleCombatManeuverStat(statType: StatType.Wisdom, type: Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple)
                .Configure();

            var ability3 = ActivatableAbilityConfigurator.New(ShadowChains2Ability, ShadowChains2AbilityGuid)
                .SetDisplayName(UmbralAgentShadowChains2DisplayName)
                .SetDescription(UmbralAgentShadowChains2Description)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately()
                .Configure();

            FeatureConfigurator.New(ShadowChains2, ShadowChains2Guid)
              .SetDisplayName(UmbralAgentShadowChains2DisplayName)
              .SetDescription(UmbralAgentShadowChains2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability3, PinAbilityGuid1, TieUpAbilityGuid, ReadyAbilityGuid, ReleaseAbilityGuid })
              .AddIncreaseResourceAmount(AbilityResourceRefs.DarknessDomainBaseResource.ToString(), 1)
              .AddComponent<AddStatBonus>(c =>
              {
                  c.Stat = CustomStatType.MeleeTouchReach.Stat();
                  c.Value = 5;
                  c.Descriptor = ModifierDescriptor.Feat;
              })
              .Configure();

            FeatureConfigurator.New(ShadowChains3, ShadowChains3Guid)
              .SetDisplayName(UmbralAgentShadowChains3DisplayName)
              .SetDescription(UmbralAgentShadowChains3Description)
              .SetIcon(icon)
              .AddComponent<ShadowConstrict>()
              .AddComponent<AddStatBonus>(c =>
              {
                  c.Stat = CustomStatType.MeleeTouchReach.Stat();
                  c.Value = 10;
                  c.Descriptor = ModifierDescriptor.Feat;
              })
              .Configure();
        }

        private static readonly string PinAbilityGuid1 = "{531632AA-0E0F-402C-8A07-18E8E0D35C80}";
        private static readonly string TieUpAbilityGuid = "{DB453CF7-8799-4FDD-941B-FA33EFF5771A}";
        private static readonly string ReadyAbilityGuid = "{A5057A11-9D24-46D8-9BE6-F5C7D605EDC5}";
        private static readonly string ReleaseAbilityGuid = "{A75ED2DD-7F0D-4367-9953-4179F3E531D2}";

        private static readonly string GazeName = "UmbralAgentGaze";
        public static readonly string GazeGuid = "{64815A7D-7869-4C70-9863-606BE4C451C5}";

        private static readonly string GazeDisplayName = "UmbralAgentGaze.Name";
        private static readonly string GazeDescription = "UmbralAgentGaze.Description";

        private static readonly string Gaze2Name = "UmbralAgentGaze2";
        public static readonly string Gaze2Guid = "{0EDA8475-F5EB-4882-87BA-831DAF226E49}";

        private static readonly string Gaze2DisplayName = "UmbralAgentGaze2.Name";
        private static readonly string Gaze2Description = "UmbralAgentGaze2.Description";

        private static readonly string Gaze3Name = "UmbralAgentGaze3";
        public static readonly string Gaze3Guid = "{59FFB353-B32F-463A-BBBA-DA3FE4A7587E}";

        private static readonly string Gaze3DisplayName = "UmbralAgentGaze3.Name";
        private static readonly string Gaze3Description = "UmbralAgentGaze3.Description";

        private static readonly string Gaze4Name = "UmbralAgentGaze4";
        public static readonly string Gaze4Guid = "{409A0B14-AF69-4569-9E9D-DB13D112A26E}";

        private static readonly string Gaze4DisplayName = "UmbralAgentGaze4.Name";
        private static readonly string Gaze4Description = "UmbralAgentGaze4.Description";

        private const string AuraBuff = "UmbralAgentStyle.Gazebuff";
        private static readonly string AuraBuffGuid = "{04803783-BFC4-4C4D-B1F5-A76A2C2ACE90}";

        private const string GazeAura = "UmbralAgentStyle.GazeAura";
        private static readonly string GazeAuraGuid = "{AF072518-2232-457B-8E2F-1026B16EE910}";

        private const string GazeAbility = "UmbralAgentStyle.GazeAbility";
        private static readonly string GazeAbilityGuid = "{A915B1BF-FF14-4A82-B0A7-7BED9CF6D032}";

        private const string GazeAbilityRes = "UmbralAgentStyle.GazeAbilityRes";
        private static readonly string GazeAbilityResGuid = "{BF4AE615-299C-45C8-9D92-A3278BC3CC4D}";

        public static void GazeConfigure()
        {
            var icon = AbilityRefs.BloodlineSerpentineScaledSoulCharmingGazeAbility.Reference.Get().Icon;

            var gaze = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(GazeGuid).IsEnemy().Build(), ifTrue: ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Will, useDCFromContextSavingThrow: true,
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                            .ApplyBuff(BuffRefs.Shaken.ToString(), durationValue: ContextDuration.FixedDice(diceType: DiceType.One)).Build()).Build())
                    .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Gaze2Guid).IsEnemy().Build(), ifTrue: ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Will, useDCFromContextSavingThrow: true,
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                            .ApplyBuff(BuffRefs.Shaken.ToString(), durationValue: ContextDuration.FixedDice(diceType: DiceType.D4)).Build()).Build())
                    .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Gaze3Guid).IsEnemy().Build(), ifTrue: ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Will, useDCFromContextSavingThrow: true,
                        onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                            .ApplyBuff(BuffRefs.Staggered.ToString(), durationValue: ContextDuration.FixedDice(diceType: DiceType.One))
                            .ApplyBuff(BuffRefs.Shaken.ToString(), durationValue: ContextDuration.FixedDice(diceType: DiceType.D4, bonus: 1))
                            .Build()).Build())
                    .Build())
                .Build();

            var area = AbilityAreaEffectConfigurator.New(GazeAura, GazeAuraGuid)
                .AddAbilityAreaEffectRunAction(round: gaze, unitEnter: gaze)
                .AddComponent<CustomDC>(c => { c.classguid = ArchetypeGuid; c.Property = StatType.Wisdom; })
                .SetAggroEnemies(true)
                .SetAffectEnemies(true)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Enemy)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(33.Feet())
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.GazeAttack)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.MindAffecting)
                .AddSpellDescriptorComponent(descriptor: SpellDescriptor.Fear)
                .Configure();

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(GazeDisplayName)
              .SetDescription(GazeDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(GazeAbilityRes, GazeAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }))
                .SetUseMax()
                .SetMax(10)
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(GazeAbility, GazeAbilityGuid)
                .SetDisplayName(GazeDisplayName)
                .SetDescription(GazeDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: Gaze4Guid)
                .SetDeactivateImmediately()
                .Configure();

            FeatureConfigurator.New(GazeName, GazeGuid)
                    .SetDisplayName(GazeDisplayName)
                    .SetDescription(GazeDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();

            FeatureConfigurator.New(Gaze2Name, Gaze2Guid)
                    .SetDisplayName(Gaze2DisplayName)
                    .SetDescription(Gaze2Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .AddRemoveFeatureOnApply(GazeGuid)
                    .Configure();

            FeatureConfigurator.New(Gaze3Name, Gaze3Guid)
                    .SetDisplayName(Gaze3DisplayName)
                    .SetDescription(Gaze3Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .AddRemoveFeatureOnApply(Gaze2Guid)
                    .Configure();

            FeatureConfigurator.New(Gaze4Name, Gaze4Guid, FeatureGroup.MythicAbility)
                    .SetDisplayName(Gaze4DisplayName)
                    .SetDescription(Gaze4Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(GazeGuid)
                    .Configure();

        }
    }
}
