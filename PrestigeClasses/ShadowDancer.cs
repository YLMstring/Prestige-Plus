using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;
using Kingmaker.RuleSystem;
using Kingmaker.Blueprints.Classes.Prerequisites;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using System.Windows.Markup;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.Facts;
using BlueprintCore.Blueprints.Configurators.Facts;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Utils;
using System.Drawing;
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using static Kingmaker.UnitLogic.FactLogic.AddAbilityUseTriggerBySpellSchool;
using BlueprintCore.Blueprints.Configurators;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.EventConditionActionSystem.ContextData;
using BlueprintCore.Conditions.Builder;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;

namespace PrestigePlus.PrestigeClasses
{
    internal class ShadowDancer
    {
        private const string ArchetypeName = "ShadowDancer";
        private static readonly string ArchetypeGuid = "C15B5F6E-E37B-47DA-B382-EF15F585B35F";
        internal const string ArchetypeDisplayName = "ShadowDancer.Name";
        private const string ArchetypeDescription = "ShadowDancer.Description";

        private const string ABProgressName = "BABMedium";
        private static readonly string ABProgressGuid = "4c936de4249b61e419a3fb775b9f2581";

        private const string ForProgressName = "SavesPrestigeLow";
        private static readonly string ForProgressGuid = "dc5257e1100ad0d48b8f3b9798421c72";

        private const string ReProgressName = "SavesPrestigeHigh";
        private static readonly string ReProgressGuid = "1f309006cd2855e4e91a6c3707f3f700";

        private const string WiProgressName = "SavesPrestigeLow";
        private static readonly string WiProgressGuid = "dc5257e1100ad0d48b8f3b9798421c72";

        private const string ClassProgressName = "ShadowDancerPrestige";
        private static readonly string ClassProgressGuid = "0C28778D-E046-4B01-8CFC-873F920D8E03";

        private static readonly string DefensiveGuid = "3967B46C-99CA-41E9-B7A2-96644EED8C96";
        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, ProficienciesGuid, FeatureRefs.AssassinHideInPlainSight.ToString())
                .AddToLevelEntry(2, FeatureRefs.Evasion.ToString(), FeatureRefs.UncannyDodge.ToString())
                .AddToLevelEntry(3, FeatureSelectionRefs.RogueTalentSelection.ToString(), CreateShadowIllusion(), SummonShadowSelection())
                .AddToLevelEntry(4, CreateShadowCall(), ExtraFeat())
                .AddToLevelEntry(5, FeatureRefs.ImprovedUncannyDodge.ToString(), DefensiveGuid)
                .AddToLevelEntry(6, FeatureSelectionRefs.RogueTalentSelection.ToString(), ShadowJumpGuidFeat)
                .AddToLevelEntry(7, FeatureRefs.SlipperyMind.ToString())
                .AddToLevelEntry(8, CreateShadowPower(), ShadowJumpGuidFeat)
                .AddToLevelEntry(9, FeatureSelectionRefs.RogueTalentSelection.ToString())
                .AddToLevelEntry(10, FeatureRefs.ImprovedEvasion.ToString(), CreateShadowCall2(), ShadowJumpGuidFeat, CreateShadowMaster())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.Evasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedEvasion.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureRefs.UncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>(), FeatureRefs.ImprovedUncannyDodge.Reference.Get().ToReference<BlueprintFeatureBaseReference>() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { ShadowCallGuid, ShadowCallGuid2 }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName(ArchetypeDisplayName)
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(ABProgressGuid)
                .SetFortitudeSave(ForProgressGuid)
                .SetReflexSave(ReProgressGuid)
                .SetWillSave(WiProgressGuid)
                .SetProgression(progression)
                .SetIsArcaneCaster(false)
                .SetIsDivineCaster(false)
                .SetClassSkills(new StatType[] { StatType.SkillStealth, StatType.SkillThievery, StatType.SkillMobility, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .AddPrerequisiteStatValue(StatType.SkillMobility, 2)
                .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
                .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference [] result = new BlueprintCharacterClassReference [i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var shadowdancerref = archetype.ToReference<BlueprintCharacterClassReference>(); 
                result[i.m_CharacterClasses.Length] = shadowdancerref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }
        private const string Proficiencies = "ShadowDancer.Proficiencies";
        private static readonly string ProficienciesGuid = "D9B20A03-B2DD-4EB6-B00D-926E0D246E55";
        internal const string ProficienciesDisplayName = "ShadowDancerProficiencies.Name";
        private const string ProficienciesDescription = "ShadowDancerProficiencies.Description";

        private const string ShadowJump = "ShadowDancer.ShadowJump";
        private static readonly string ShadowJumpGuid = "5565506D-04DE-47EC-8545-C19ABCEA40B2";
        internal const string ShadowJumpDisplayName = "ShadowDancerShadowJump.Name";
        private const string ShadowJumpDescription = "ShadowDancerShadowJump.Description";

        private const string ShadowJumpAblity = "ShadowDancer.UseShadowJump";
        private static readonly string ShadowJumpAblityGuid = "E1A86DF3-38E6-4E78-82B9-4EF7A6436BA8";

        private const string ShadowJumpAblity2 = "ShadowDancer.UseShadowJump2";
        private static readonly string ShadowJumpAblityGuid2 = "{8ED8B86D-A5DB-46FE-B88A-05098198041A}";

        private const string ShadowJumpAblityRes = "ShadowDancer.ShadowJumpRes";
        private static readonly string ShadowJumpAblityResGuid = "E96F5710-5344-495F-A3C7-A4859C1ABF81";

        public static BlueprintFeature CreateProficiencies()
        {
            var abilityresourse = AbilityResourceConfigurator.New(ShadowJumpAblityRes, ShadowJumpAblityResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(0))
                .SetUseMax()
                .Configure();
            var ability = AbilityConfigurator.New(ShadowJumpAblity, ShadowJumpAblityGuid)
                .CopyFrom(
                AbilityRefs.KiAbudantStep,
                typeof(AbilityEffectRunAction),
                typeof(AbilityCustomDimensionDoor))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowJumpAblityResGuid)
                .SetDisplayName(ShadowJumpDisplayName)
                .SetDescription(ShadowJumpDescription)
                .Configure(delayed: true);

            var ability2 = AbilityConfigurator.New(ShadowJumpAblity2, ShadowJumpAblityGuid2)
                .CopyFrom(
                AbilityRefs.DimensionDoorBase,
                typeof(SpellComponent),
                typeof(LineOfSightIgnorance),
                typeof(AbilityVariants))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowJumpAblityResGuid)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(ShadowJumpDisplayName)
                .SetDescription(ShadowJumpDescription)
                .Configure(delayed: true);

            var assProficiencies = FeatureRefs.AssassinProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability, ability2 })
              .AddAbilityResources(resource: ShadowJumpAblityResGuid, restoreAmount: true)
              .AddComponent(assProficiencies.GetComponent<AddFacts>())
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.Club,
              WeaponCategory.Shortbow,
              WeaponCategory.Shortsword,
              WeaponCategory.LightCrossbow,
              WeaponCategory.HeavyCrossbow,
              WeaponCategory.Dagger,
              WeaponCategory.Dart,
              WeaponCategory.Quarterstaff,
              WeaponCategory.Rapier,
              WeaponCategory.LightMace,
              WeaponCategory.HeavyMace,
                  })
              .Configure();
        }

        private const string ShadowIllusion = "ShadowDancer.ShadowIllusion";
        private static readonly string ShadowIllusionGuid = "8B1BA1B5-CAA1-4B16-9B93-5F0D04C07ED3";
        internal const string ShadowIllusionDisplayName = "ShadowDancerShadowIllusion.Name";
        private const string ShadowIllusionDescription = "ShadowDancerShadowIllusion.Description";

        private const string ShadowIllusionAblity = "ShadowDancer.UseShadowIllusion";
        private static readonly string ShadowIllusionAblityGuid = "C86E5DEA-BCA7-45FE-B7D4-71D70C595E99";

        private const string ShadowIllusionAblityRes = "ShadowDancer.ShadowIllusionRes";
        private static readonly string ShadowIllusionAblityResGuid = "D84FE944-991E-4BA4-B424-7C28BBAABBE9";

        private static BlueprintFeature CreateShadowIllusion()
        {
            var icon = AbilityRefs.Vanish.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ShadowIllusionAblityRes, ShadowIllusionAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1))
                .SetUseMax()
                .SetMax(5)
                .Configure();
            var ability = AbilityConfigurator.New(ShadowIllusionAblity, ShadowIllusionAblityGuid)
                .CopyFrom(
                AbilityRefs.Vanish,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, max:5, min:3))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowIllusionAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(ShadowIllusion, ShadowIllusionGuid)
              .SetDisplayName(ShadowIllusionDisplayName)
              .SetDescription(ShadowIllusionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string ShadowCall = "ShadowDancer.ShadowCall";
        private static readonly string ShadowCallGuid = "3D5FDD8B-A70B-4E45-AE1F-0ECC9EDEDD43";

        private const string ShadowCall2 = "ShadowDancer.ShadowCall2";
        private static readonly string ShadowCallGuid2 = "A0D34CB8-9E9A-4FCA-8D32-EBBA568B5260";

        internal const string ShadowCallDisplayName = "ShadowDancerShadowCall.Name";
        private const string ShadowCallDescription = "ShadowDancerShadowCall.Description";

        private const string ShadowCallAblity = "ShadowDancer.UseShadowCall";
        private static readonly string ShadowCallAblityGuid = "FCC5BFC3-12D2-447E-9122-8049EE75D70E";

        private const string ShadowCallAblity2 = "ShadowDancer.UseShadowCall2";
        private static readonly string ShadowCallAblityGuid2 = "DE11BACF-29BC-43CD-8978-FB1B4127ECDD";

        private const string ShadowCallAblityRes = "ShadowDancer.ShadowCallRes";
        private static readonly string ShadowCallAblityResGuid = "D3547BA7-E807-4EF8-A6A0-1E489A022BF0";

        private static BlueprintFeature CreateShadowCall()
        {
            var icon = AbilityRefs.ShadowConjuration.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ShadowCallAblityRes, ShadowCallAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 4))
                .SetUseMax()
                .SetMax(4)
                .Configure();
            var ability = AbilityConfigurator.New(ShadowCallAblity, ShadowCallAblityGuid)
                .CopyFrom(
                AbilityRefs.ShadowConjuration,
                typeof(AbilityEffectRunAction),
                typeof(AbilityVariants),
                typeof(AbilityShadowSpell),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddAbilityCasterInCombat(true)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, max: 10, min: 4))
                .AddReplaceAbilityDC(stat: StatType.Charisma)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowCallAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(ShadowCall, ShadowCallGuid)
              .SetDisplayName(ShadowCallDisplayName)
              .SetDescription(ShadowCallDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private static BlueprintFeature CreateShadowCall2()
        {
            var icon = AbilityRefs.ShadowConjurationGreater.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(ShadowCallAblity2, ShadowCallAblityGuid2)
                .CopyFrom(
                AbilityRefs.ShadowConjurationGreater,
                typeof(AbilityEffectRunAction),
                typeof(AbilityVariants),
                typeof(AbilityShadowSpell),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddAbilityCasterInCombat(true)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, max: 10, min: 4))
                .AddReplaceAbilityDC(stat: StatType.Charisma)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowCallAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(ShadowCall2, ShadowCallGuid2)
              .SetDisplayName(ShadowCallDisplayName)
              .SetDescription(ShadowCallDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddRemoveFeatureOnApply(ShadowCallGuid)
              .Configure();
        }

        private const string ShadowPower = "ShadowDancer.ShadowPower";
        private static readonly string ShadowPowerGuid = "91CAC8B2-C480-4433-AC5A-2A84192DCCBC";
        internal const string ShadowPowerDisplayName = "ShadowDancerShadowPower.Name";
        private const string ShadowPowerDescription = "ShadowDancerShadowPower.Description";

        private const string ShadowPowerAblity = "ShadowDancer.UseShadowPower";
        private static readonly string ShadowPowerAblityGuid = "5B4EF2BE-B6AB-4E84-A18A-BDA2AC65FF2A";

        private const string ShadowPowerAblityRes = "ShadowDancer.ShadowPowerRes";
        private static readonly string ShadowPowerAblityResGuid = "AEBAB7E3-B037-4693-8E5F-44F7A078933C";

        private static BlueprintFeature CreateShadowPower()
        {
            var icon = AbilityRefs.ShadowEvocation.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ShadowPowerAblityRes, ShadowPowerAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 8))
                .SetUseMax()
                .SetMax(2)
                .Configure();
            var ability = AbilityConfigurator.New(ShadowPowerAblity, ShadowPowerAblityGuid)
                .CopyFrom(
                AbilityRefs.ShadowEvocation,
                typeof(AbilityEffectRunAction),
                typeof(AbilityVariants),
                typeof(AbilityShadowSpell),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddAbilityCasterInCombat(true)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, max: 10, min: 8))
                .AddReplaceAbilityDC(stat: StatType.Charisma)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ShadowPowerAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(ShadowPower, ShadowPowerGuid)
              .SetDisplayName(ShadowPowerDisplayName)
              .SetDescription(ShadowPowerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string ShadowJumpEx = "ShadowDancer.ShadowJumpEx";
        private static readonly string ShadowJumpGuidEx = "3F7AEF5C-F45F-4904-A04F-5F49C6D3816B";

        public static void ExtraShadowJump()
        {
            var icon = AbilityRefs.KiAbudantStep.Reference.Get().Icon;
            FeatureConfigurator.New(ShadowJumpEx, ShadowJumpGuidEx, FeatureGroup.Feat)
              .SetDisplayName(ShadowJumpDisplayName)
              .SetDescription(ShadowJumpDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 1)
              .AddIncreaseResourceAmount(ShadowJumpAblityResGuid, 4)
              .AddFeatureTagsComponent(FeatureTag.ClassSpecific)
              .Configure();
        }

        private const string ShadowJumpFeat = "ShadowDancer.ShadowJumpFeat";
        private static readonly string ShadowJumpGuidFeat = "29C89DAC-EF33-489E-B3D6-3E15D68D4326";

        internal const string ShadowFeatDisplayName = "ShadowDancerFeat.Name";
        private const string ShadowFeatDescription = "ShadowDancerFeat.Description";
        public static BlueprintFeatureSelection ExtraFeat()
        {
            var icon = AbilityRefs.KiAbudantStep.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ShadowJumpFeat, ShadowJumpGuidFeat)
              .SetDisplayName(ShadowFeatDisplayName)
              .SetDescription(ShadowFeatDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(ShadowJumpGuidEx)
              .AddToAllFeatures("b230b6ae-6c50-4d74-9674-cc3cd04e686d") ///FlickeringStepFeature from other mods
              .AddToAllFeatures("5110905a-4911-4916-a39c-9cda7a67eca0") ///DimensionalAgilityFeature
              .AddToAllFeatures("4673b517-5419-414e-b24b-a836bad7f8c5") ///DimensionalAssaultFeature
              .AddToAllFeatures("3fcf93ab-0b1b-4ed3-9cc1-cfec6935a824") ///DimensionalDervishFeature
              .Configure(delayed: true);
        }

        private const string SummonShadowAnimal = "ShadowDancer.SummonShadowAnimal";
        private static readonly string SummonShadowAnimalGuid = "37F5848E-B2B0-4C17-9E05-3A44CCB14F26";

        private const string SummonShadowAnimal2 = "ShadowDancer.SummonShadowAnimal2";
        private static readonly string SummonShadowAnimalGuid2 = "{B03092E4-DE18-4AA9-A6F7-80F84BAEC2F6}";

        internal const string SummonShadowAnimalDisplayName = "SummonShadowAnimal.Name";
        private const string SummonShadowAnimalDescription = "SummonShadowAnimal.Description";

        private const string ClassProgressName2 = "ShadowDancerPrestige2";
        private static readonly string ClassProgressGuid2 = "{C57E4F77-0BEB-45F6-ADA8-37BF3ADCBB5A}";
        public static BlueprintFeatureSelection SummonAnimalSelection()
        {
            var icon = AbilityRefs.SummonElvenSpirits.Reference.Get().Icon;

            var progression =
                ProgressionConfigurator.New(ClassProgressName2, ClassProgressGuid2)
                .CopyFrom(ProgressionRefs.SylvanSorcererAnimalCompanionProgression, typeof(LevelEntry), typeof(UIGroup))
                .SetClasses(ArchetypeGuid)
                .Configure();

            return FeatureSelectionConfigurator.New(SummonShadowAnimal2, SummonShadowAnimalGuid2)
              .CopyFrom(
                FeatureSelectionRefs.AnimalCompanionSelectionSylvanSorcerer)
              .AddFeatureOnApply(progression)
              .AddFeatureOnApply(FeatureRefs.AnimalCompanionRank.ToString())
              .AddFeatureOnApply(FeatureRefs.MountTargetFeature.ToString())
              .AddFeatureToPet(FeatureRefs.UndeadType.ToString())
              .AddFeatureOnApply(FeatureSelectionRefs.AnimalCompanionArchetypeSelection.ToString())
              .Configure(delayed: true);
        }
        public static BlueprintFeatureSelection SummonShadowSelection()
        {
            var icon = AbilityRefs.SummonElvenSpirits.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(SummonShadowAnimal, SummonShadowAnimalGuid)
              .SetDisplayName(SummonShadowAnimalDisplayName)
              .SetDescription(SummonShadowAnimalDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(CreateSummonShadow())
              .AddToAllFeatures(SummonAnimalSelection())
              .Configure(delayed: true);
        }

        private const string SummonShadow = "ShadowDancer.SummonShadow";
        private static readonly string SummonShadowGuid = "ACE9A82F-A4DD-476F-932A-1D240D1E4B97";
        internal const string SummonShadowDisplayName = "ShadowDancerSummonShadow.Name";
        private const string SummonShadowDescription = "ShadowDancerSummonShadow.Description";

        private const string SummonShadowAblity = "ShadowDancer.UseSummonShadow";
        private static readonly string SummonShadowAblityGuid = "{130D04FE-64DA-466F-9E62-914D7046FE1A}";

        private const string SummonShadowAblityRes = "ShadowDancer.SummonShadowRes";
        private static readonly string SummonShadowAblityResGuid = "{9A0859B4-FCB2-4ABD-AF62-79051C006909}";

        private const string SummonPool = "ShadowDancer.SummonPool";
        private static readonly string SummonPoolGuid = "{EE0B8E46-A28F-4913-82A3-D3E9DDFE8A37}";

        private const string ShadowEnhanceBuff = "ShadowEnhanceBuff";
        private static readonly string ShadowEnhanceGuidBuff = "{A9ADB781-2778-421F-8078-5AAB2FC906DC}";

        private const string ShadowEnhanceBuff2 = "ShadowEnhanceBuff2";
        private static readonly string ShadowEnhanceGuidBuff2 = "{C83A900B-35C2-4ABA-9D6F-FC661AC07802}";

        private const string ShadowEnhanceBuff3 = "ShadowEnhanceBuff3";
        private static readonly string ShadowEnhanceGuidBuff3 = "{15DFA947-288C-4FD4-A0B2-C7C518A791C6}";

        private const string ShadowEnhanceBuff4 = "ShadowEnhanceBuff4";
        private static readonly string ShadowEnhanceGuidBuff4 = "{855A8B79-12FC-47D2-AA57-80D6103DBBBC}";

        private const string ShadowEnhanceBuff5 = "ShadowEnhanceBuff5";
        private static readonly string ShadowEnhanceGuidBuff5 = "{9D56C8F1-25DF-493E-9949-36B6BB9B8D13}";
        private static BlueprintFeature CreateSummonShadow()
        {
            var icon = AbilityRefs.SummonElvenSpirits.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SummonShadowAblityRes, SummonShadowAblityResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .SetUseMax(false)
                .SetMax(10)
                .Configure();

            var summonpool = SummonPoolConfigurator.New(SummonPool, SummonPoolGuid)
                .SetDoNotRemoveDeadUnits(false)
                .Configure();

            var Buff1 = BuffConfigurator.New(ShadowEnhanceBuff, ShadowEnhanceGuidBuff)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.BaseAttackBonus).WithLinearProgression(1,-2, minProgressionValue: 0))
              .AddContextStatBonus(stat: StatType.BaseAttackBonus, value: ContextValues.Rank())
              .Configure();

            var Buff2 = BuffConfigurator.New(ShadowEnhanceBuff2, ShadowEnhanceGuidBuff2)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.HitPoints).WithLinearProgression(1, -20, minProgressionValue: 0))
              .AddContextStatBonus(stat: StatType.HitPoints, value: ContextValues.Rank())
              .Configure();

            var Buff3 = BuffConfigurator.New(ShadowEnhanceBuff3, ShadowEnhanceGuidBuff3)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.SaveFortitude).WithLinearProgression(1,-1, minProgressionValue: 0))
              .AddContextStatBonus(stat: StatType.SaveFortitude, value: ContextValues.Rank())
              .Configure();

            var Buff4 = BuffConfigurator.New(ShadowEnhanceBuff4, ShadowEnhanceGuidBuff4)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.SaveReflex).WithLinearProgression(1, -1, minProgressionValue: 0))
              .AddContextStatBonus(stat: StatType.SaveReflex, value: ContextValues.Rank())
              .Configure();

            var Buff5 = BuffConfigurator.New(ShadowEnhanceBuff5, ShadowEnhanceGuidBuff5)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddFacts(new() { FeatureRefs.ChannelResistance4.ToString() })
              .AddContextRankConfig(ContextRankConfigs.BaseStat(StatType.SaveWill).WithLinearProgression(1, -3, minProgressionValue: 0))
              .AddContextStatBonus(stat: StatType.SaveWill, value: ContextValues.Rank())
              .Configure();

            var ability = AbilityConfigurator.New(SummonShadowAblity, SummonShadowAblityGuid)
                .SetDisplayName(SummonShadowDisplayName)
                .SetDescription(SummonShadowDescription)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Close)
                .SetCanTargetPoint(true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ClearSummonPool(summonpool)
                    .SpawnMonsterUsingSummonPool(countValue: ContextDice.Value(DiceType.One), 
                        durationValue: ContextDuration.Fixed(14400), 
                        monster: UnitRefs.CR3_Shadow.ToString(), 
                        summonPool: summonpool,
                        afterSpawn: ActionsBuilder.New()
                            .ApplyBuffPermanent(Buff1)
                            .ApplyBuffPermanent(Buff2)
                            .ApplyBuffPermanent(Buff3)
                            .ApplyBuffPermanent(Buff4)
                            .ApplyBuffPermanent(Buff5)
                            .Build()))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(SummonShadow, SummonShadowGuid)
              .SetDisplayName(SummonShadowDisplayName)
              .SetDescription(SummonShadowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string ShadowMaster = "ShadowDancer.ShadowMaster";
        private static readonly string ShadowMasterGuid = "B830CAE7-247B-4C76-9A13-2AD6EAE2427E";

        private const string ShadowMasterBuff = "ShadowDancer.ShadowMasterBuff";
        private static readonly string ShadowMasterGuidBuff = "8A88D803-B26A-49C4-8ECC-77BE9172B541";

        internal const string ShadowMasterDisplayName = "ShadowDancerShadowMaster.Name";
        private const string ShadowMasterDescription = "ShadowDancerShadowMaster.Description";

        private const string ShadowHideAblity = "ShadowDancer.UseShadowHide";
        private static readonly string ShadowHideAblityGuid = "89D171AF-29EB-4CB2-A954-C4E399F8E02A";
        private static BlueprintFeature CreateShadowMaster()
        {
            var icon = AbilityRefs.Invisibility.Reference.Get().Icon;

            var ShadowBlindaction = ActionsBuilder.New()
            .ApplyBuff(buff: BuffRefs.BlindnessBuff.ToString(), toCaster: false, durationValue: ContextDuration.VariableDice(DiceType.D6, 1))
            .Build();

            var MasterBuff = BuffConfigurator.New(ShadowMasterBuff, ShadowMasterGuidBuff)
              .SetDisplayName(ShadowMasterDisplayName)
              .SetDescription(ShadowMasterDescription)
              .SetIcon(icon)
              .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(10))
              ///.AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid}).WithCustomProgression((9, 0), (10, 10)))
              .AddBuffAllSavesBonus(ModifierDescriptor.Luck, 2)
              .AddInitiatorAttackWithWeaponTrigger(criticalHit: true, action: ShadowBlindaction)
              .SetStacking(StackingType.Prolong)
              .Configure();

            var action = ActionsBuilder.New()
            .ApplyBuff(MasterBuff, durationValue: ContextDuration.VariableDice(DiceType.D4, 1))
            .Build();

            var ability = AbilityConfigurator.New(ShadowHideAblity, ShadowHideAblityGuid)
                .CopyFrom(
                AbilityRefs.AssassinHideInPlainSightAbility,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent))
                .AddAbilityExecuteActionOnCast(action)
                .Configure();

            return FeatureConfigurator.New(ShadowMaster, ShadowMasterGuid)
              .SetDisplayName(ShadowMasterDisplayName)
              .SetDescription(ShadowMasterDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddRemoveFeatureOnApply(FeatureRefs.AssassinHideInPlainSight.ToString())
              .AddAbilityUseTrigger(action: action, checkAbilityType: true, type: AbilityType.Spell)
              .AddAbilityUseTrigger(action: action, checkAbilityType: true, type: AbilityType.SpellLike)
              .Configure();
        }
    }
}

