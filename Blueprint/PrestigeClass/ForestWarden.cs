using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomAction;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Gunslinger;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using PrestigePlus.CustomComponent;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.Blueprint.ManeuverFeat;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.CustomComponent.Feat;
using Kingmaker.UnitLogic.Mechanics.Properties;
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.CustomAction.OtherManeuver;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class ForestWarden
    {
        private const string ArchetypeName = "ForestWarden";
        public static readonly string ArchetypeGuid = "{89FFF2B7-0C77-4973-8631-5A4C94A3C1EB}";
        internal const string ArchetypeDisplayName = "ForestWarden.Name";
        private const string ArchetypeDescription = "ForestWarden.Description";

        private const string ClassProgressName = "ForestWardenPrestige";
        public static readonly string ClassProgressGuid = "{FAE0E13F-A118-440A-B313-95E68D1B33EA}";

        public static void Configure()
        {
            PsychicEsotericaFeat();
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, AlignSpamFeat(), CreateForestMagic1(), UnchainedHeartFeature(), ProficiencyFeature())
                .AddToLevelEntry(2, PsychicEsotericaGuid)
                .AddToLevelEntry(3, CreateForestMagic3(), FeatureRefs.SneakAttack.ToString(), SneakyManeuver.SneakyManeuverGuid)
                .AddToLevelEntry(4, UnchainedHeartGuid)
                .AddToLevelEntry(5, PsychicEsotericaGuid, CreateForestMagic5())
                .AddToLevelEntry(6, FeatureRefs.SneakAttack.ToString(), TrickShotConfigure())
                .AddToLevelEntry(7, CreateForestMagic7(), UnchainedHeartGuid)
                .AddToLevelEntry(8, PsychicEsotericaGuid)
                .AddToLevelEntry(9, CreateForestMagic9(), FeatureRefs.SneakAttack.ToString(), PreemptiveStrikeFeature())
                .AddToLevelEntry(10, FeatureRefs.HunterWoodlandStride.ToString(), FeatureRefs.AssassinHideInPlainSight.ToString())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { ForestMagic1Guid, ForestMagic3Guid, ForestMagic5Guid, ForestMagic7Guid, ForestMagic9Guid }))
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeizetheOpportunity.FeatGuid, BodyGuard.FeatGuid, BodyGuard.Feat2Guid, "8590fb52-921c-4365-832c-ca7635fd5a70", FeatureRefs.PerfectStrikeFeature.ToString() }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABMedium.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillKnowledgeArcana, StatType.SkillPerception, StatType.SkillLoreNature, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteAlignment(AlignmentMaskType.NeutralGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string PsychicEsoterica = "ForestWarden.PsychicEsoterica";
        private static readonly string PsychicEsotericaGuid = "{75A9BEDF-64D2-42B5-BA76-0A026627AB0F}";

        internal const string PsychicEsotericaDisplayName = "ForestWardenPsychicEsoterica.Name";
        private const string PsychicEsotericaDescription = "ForestWardenPsychicEsoterica.Description";

        public static void PsychicEsotericaFeat()
        {
            var icon = AbilityRefs.LifeBubble.Reference.Get().Icon;

            //SpringAttackFeatGuid = "9D46135E-3DC2-44B8-ABFD-45CA33805FF0";
            //PairedOpportunistsFeat = "41df43af-78bc-477a-a33a-e57d86ba8928";

            FeatureSelectionConfigurator.New(PsychicEsoterica, PsychicEsotericaGuid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(RangedDisarm.ArmBindGuid)
              .AddToAllFeatures(FeatureRefs.CombatExpertiseFeature.ToString())
              .AddToAllFeatures(CrushArmor.FeatGuid)
              .AddToAllFeatures(FeatureRefs.GreaterDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.HeightenSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.ImprovedDirtyTrick.ToString())
              .AddToAllFeatures(FeatureRefs.Mobility.ToString())
              .AddToAllFeatures("41df43af-78bc-477a-a33a-e57d86ba8928")
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(FeatureRefs.RapidShotFeature.ToString())
              .AddToAllFeatures(FeatureRefs.SiezeTheMoment.ToString())
              .AddToAllFeatures("9D46135E-3DC2-44B8-ABFD-45CA33805FF0")
              .AddToAllFeatures(RangedDisarm.StrikeSeizeGuid)
              .Configure();
        }

        private const string Proficiency = "ForestWardenProficiency";
        private static readonly string ProficiencyGuid = "{C2575C53-BA81-49E4-9011-BF20F8CB8BC3}";

        internal const string ProficiencyDisplayName = "ForestWardenProficiency.Name";
        private const string ProficiencyDescription = "ForestWardenProficiency.Description";
        public static BlueprintFeature ProficiencyFeature()
        {
            return FeatureConfigurator.New(Proficiency, ProficiencyGuid)
              .SetDisplayName(ProficiencyDisplayName)
              .SetDescription(ProficiencyDescription)
              .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.ToString(), FeatureRefs.MartialWeaponProficiency.ToString(), FeatureRefs.LightArmorProficiency.ToString(), FeatureRefs.BucklerProficiency.ToString() })
              .Configure();
        }

        private const string ForestMagic1 = "ForestWarden.ForestMagic1";
        public static readonly string ForestMagic1Guid = "{C074C94D-2392-4231-8ADB-56F3A3615141}";
        internal const string ForestMagic1DisplayName = "ForestWardenForestMagic1.Name";
        private const string ForestMagic1Description = "ForestWardenForestMagic1.Description";

        private const string ForestMagic1Res = "ForestWarden.ForestMagic1Res";
        private static readonly string ForestMagic1ResGuid = "{E3953B54-7A77-4B18-9069-AD2D8C2F87E8}";

        private const string ForestMagic1Ability = "ForestWarden.ForestMagic1Ability";
        private static readonly string ForestMagic1AbilityGuid = "{BD638A01-9F13-4ABF-A096-9226564D7C41}";

        private const string ForestMagic1Ablity3 = "ForestWarden.UseForestMagic13";
        private static readonly string ForestMagic1Ablity3Guid = "{CBC7D2AE-2B56-44D8-B072-E3212C696371}";
        private static BlueprintFeature CreateForestMagic1()
        {
            var icon = AbilityRefs.ForesterCamouflageAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ForestMagic1Res, ForestMagic1ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(ForestMagic1Ability, ForestMagic1AbilityGuid)
                .CopyFrom(
                AbilityRefs.Entangle,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityAoERadius),
                typeof(ContextRankConfig),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ForestMagic1Ablity3, ForestMagic1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Longstrider,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ForestMagic1, ForestMagic1Guid)
              .SetDisplayName(ForestMagic1DisplayName)
              .SetDescription(ForestMagic1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string ForestMagic3 = "ForestWarden.ForestMagic3";
        public static readonly string ForestMagic3Guid = "{E155BB25-1D73-4F55-88A3-A916FD5A8309}";
        internal const string ForestMagic3DisplayName = "ForestWardenForestMagic3.Name";
        private const string ForestMagic3Description = "ForestWardenForestMagic3.Description";

        private const string ForestMagic3Res = "ForestWarden.ForestMagic3Res";
        private static readonly string ForestMagic3ResGuid = "{DCD53689-C6A4-4222-8D75-5E947E131618}";

        private const string ForestMagic3Ability = "ForestWarden.ForestMagic3Ability";
        private static readonly string ForestMagic3AbilityGuid = "{3330680A-BB6E-47A0-8CBC-53C84FE62F04}";

        private const string ForestMagic3Ablity3 = "ForestWarden.UseForestMagic33";
        private static readonly string ForestMagic3Ablity3Guid = "{68048422-75E8-4DBA-BE83-E6E67CB04035}";
        private static BlueprintFeature CreateForestMagic3()
        {
            var icon = AbilityRefs.ForesterCamouflageAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ForestMagic3Res, ForestMagic3ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(ForestMagic3Ability, ForestMagic3AbilityGuid)
                .CopyFrom(
                AbilityRefs.CureLightWoundsCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ForestMagic3Ablity3, ForestMagic3Ablity3Guid)
                .CopyFrom(
                AbilityRefs.SpikeGrowth,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(ContextRankConfig),
                typeof(AbilityAoERadius))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ForestMagic3, ForestMagic3Guid)
              .SetDisplayName(ForestMagic3DisplayName)
              .SetDescription(ForestMagic3Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddIncreaseResourceAmount(ForestMagic1ResGuid, 2)
              .Configure();
        }

        private const string ForestMagic5 = "ForestWarden.ForestMagic5";
        public static readonly string ForestMagic5Guid = "{D7649678-BAE8-4918-A0F4-EA97B43FF850}";
        internal const string ForestMagic5DisplayName = "ForestWardenForestMagic5.Name";
        private const string ForestMagic5Description = "ForestWardenForestMagic5.Description";

        private const string ForestMagic5Res = "ForestWarden.ForestMagic5Res";
        private static readonly string ForestMagic5ResGuid = "{F8FD50D5-5CDA-4D67-88B8-91F8E0774AFA}";

        private const string ForestMagic5Ability = "ForestWarden.ForestMagic5Ability";
        private static readonly string ForestMagic5AbilityGuid = "{D98221F9-F06B-4356-AEAD-6BD9835EA625}";

        private const string ForestMagic5Ablity3 = "ForestWarden.UseForestMagic53";
        private static readonly string ForestMagic5Ablity3Guid = "{1AE19EB3-5F19-4360-99DF-1BB1B9F91A15}";
        private static BlueprintFeature CreateForestMagic5()
        {
            var icon = AbilityRefs.ForesterCamouflageAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ForestMagic5Res, ForestMagic5ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(ForestMagic5Ability, ForestMagic5AbilityGuid)
                .CopyFrom(
                AbilityRefs.HurricaneBow,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ForestMagic5Ablity3, ForestMagic5Ablity3Guid)
                .CopyFrom(
                AbilityRefs.LeadBlades,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ForestMagic5, ForestMagic5Guid)
              .SetDisplayName(ForestMagic5DisplayName)
              .SetDescription(ForestMagic5Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddIncreaseResourceAmount(ForestMagic1ResGuid, 2)
              .AddIncreaseResourceAmount(ForestMagic3ResGuid, 2)
              .Configure();
        }

        private const string ForestMagic7 = "ForestWarden.ForestMagic7";
        public static readonly string ForestMagic7Guid = "{7B197F29-3A0E-4531-90BE-3575F55A7EF4}";
        internal const string ForestMagic7DisplayName = "ForestWardenForestMagic7.Name";
        private const string ForestMagic7Description = "ForestWardenForestMagic7.Description";

        private const string ForestMagic7Res = "ForestWarden.ForestMagic7Res";
        private static readonly string ForestMagic7ResGuid = "{382DD7F7-6B21-45F0-8937-E6B43390FA59}";

        private const string ForestMagic7Ability = "ForestWarden.ForestMagic7Ability";
        private static readonly string ForestMagic7AbilityGuid = "{391F6D3D-914B-48EA-AEB9-9BC7B8D0B0B4}";

        private const string ForestMagic7Ablity3 = "ForestWarden.UseForestMagic73";
        private static readonly string ForestMagic7Ablity3Guid = "{BC839B8C-9CE3-41F2-9774-C81819A09CFA}";
        private static BlueprintFeature CreateForestMagic7()
        {
            var icon = AbilityRefs.ForesterCamouflageAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ForestMagic7Res, ForestMagic7ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(ForestMagic7Ability, ForestMagic7AbilityGuid)
                .CopyFrom(
                AbilityRefs.Barkskin,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ForestMagic7Ablity3, ForestMagic7Ablity3Guid)
                .CopyFrom(
                AbilityRefs.CureModerateWoundsCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ForestMagic7, ForestMagic7Guid)
              .SetDisplayName(ForestMagic7DisplayName)
              .SetDescription(ForestMagic7Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddIncreaseResourceAmount(ForestMagic3ResGuid, 2)
              .AddIncreaseResourceAmount(ForestMagic5ResGuid, 2)
              .Configure();
        }

        private const string ForestMagic9 = "ForestWarden.ForestMagic9";
        public static readonly string ForestMagic9Guid = "{032B27E9-9F9E-47E9-850C-86D222AAD86A}";
        internal const string ForestMagic9DisplayName = "ForestWardenForestMagic9.Name";
        private const string ForestMagic9Description = "ForestWardenForestMagic9.Description";

        private const string ForestMagic9Res = "ForestWarden.ForestMagic9Res";
        private static readonly string ForestMagic9ResGuid = "{D87278E7-62A3-48A4-BBF7-E51158B9B1AC}";

        private const string ForestMagic9Ability = "ForestWarden.ForestMagic9Ability";
        private static readonly string ForestMagic9AbilityGuid = "{DC847114-DB72-4875-855A-27B8F7E37FA6}";

        private const string ForestMagic9Ablity3 = "ForestWarden.UseForestMagic93";
        private static readonly string ForestMagic9Ablity3Guid = "{23A10F3D-7A69-4ACA-B8B6-9FFE48258D5E}";
        private static BlueprintFeature CreateForestMagic9()
        {
            var icon = AbilityRefs.ForesterCamouflageAbility.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(ForestMagic9Res, ForestMagic9ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(ForestMagic9Ability, ForestMagic9AbilityGuid)
                .CopyFrom(
                AbilityRefs.FreedomOfMovementCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 4)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ForestMagic9Ablity3, ForestMagic9Ablity3Guid)
                .CopyFrom(
                AbilityRefs.CureSeriousWoundsCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 4)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ForestMagic9, ForestMagic9Guid)
              .SetDisplayName(ForestMagic9DisplayName)
              .SetDescription(ForestMagic9Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddIncreaseResourceAmount(ForestMagic5ResGuid, 2)
              .AddIncreaseResourceAmount(ForestMagic7ResGuid, 2)
              .Configure();
        }

        private static readonly string TrickShotName = "ForestWardenTrickShot";
        public static readonly string TrickShotGuid = "{7A18A66C-3BD6-4944-BF64-553B6644EB3A}";

        private static readonly string TrickShotDisplayName = "ForestWardenTrickShot.Name";
        private static readonly string TrickShotDescription = "ForestWardenTrickShot.Description";
        private static readonly string TrickShotDisplayName1 = "ForestWardenTrickShot1.Name";
        private static readonly string TrickShotDescription1 = "ForestWardenTrickShot1.Description";
        private static readonly string TrickShotDisplayName2 = "ForestWardenTrickShot2.Name";
        private static readonly string TrickShotDescription2 = "ForestWardenTrickShot2.Description";
        private static readonly string TrickShotDisplayName3 = "ForestWardenTrickShot3.Name";
        private static readonly string TrickShotDescription3 = "ForestWardenTrickShot3.Description";

        private const string TrickShotAbility = "TrickShot.TrickShotAbility";
        private static readonly string TrickShotAbilityGuid = "{A9DD95BE-BF5F-4B80-8953-69366100AC0E}";

        private const string TrickShotAbility2 = "TrickShot.TrickShotAbility2";
        private static readonly string TrickShotAbility2Guid = "{A2323B32-215B-4CD0-B178-02C0AB60D15E}";

        private const string TrickShotAbility3 = "TrickShot.TrickShotAbility3";
        private static readonly string TrickShotAbility3Guid = "{BA652CD4-D37F-4E37-AC79-E41C3BE99A52}";
        public static BlueprintFeature TrickShotConfigure()
        {
            var icon = FeatureRefs.ThrowAnything.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TrickShotAbility, TrickShotAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<ContextActionRangedTrip>(c => { c.maneuver = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickBlind; c.Ace = FeatureRefs.MonsterMythicWeaponSizeFeature.ToString(); })
                    .Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(TrickShotDisplayName1)
                .SetDescription(TrickShotDescription1)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetIsFullRoundAction(true)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow, WeaponCategory.ThrowingAxe, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.SlingStaff, WeaponCategory.Sling })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            var ability2 = AbilityConfigurator.New(TrickShotAbility2, TrickShotAbility2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<ContextActionRangedTrip>(c => { c.maneuver = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle; c.Ace = FeatureRefs.MonsterMythicWeaponSizeFeature.ToString(); })
                    .Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(TrickShotDisplayName2)
                .SetDescription(TrickShotDescription2)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetIsFullRoundAction(true)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow, WeaponCategory.ThrowingAxe, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.SlingStaff, WeaponCategory.Sling })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            var ability3 = AbilityConfigurator.New(TrickShotAbility3, TrickShotAbility3Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<ContextActionRangedTrip>(c => { c.maneuver = Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened; c.Ace = FeatureRefs.MonsterMythicWeaponSizeFeature.ToString(); })
                    .Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(TrickShotDisplayName3)
                .SetDescription(TrickShotDescription3)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetIsFullRoundAction(true)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow, WeaponCategory.ThrowingAxe, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.SlingStaff, WeaponCategory.Sling })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            return FeatureConfigurator.New(TrickShotName, TrickShotGuid)
                    .SetDisplayName(TrickShotDisplayName)
                    .SetDescription(TrickShotDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, ability2, ability3 })
                    .Configure();
        }

        private const string UnchainedHeart = "ForestWardenUnchainedHeart";
        private static readonly string UnchainedHeartGuid = "{8AC62A19-97BD-48D5-B2CB-45B5D0D5D796}";

        internal const string UnchainedHeartDisplayName = "ForestWardenUnchainedHeart.Name";
        private const string UnchainedHeartDescription = "ForestWardenUnchainedHeart.Description";

        private const string UnchainedHeartBuff = "UnchainedHeart.UnchainedHeartBuff";
        public static readonly string UnchainedHeartBuffGuid = "{F10E288F-7A67-423F-8B14-098E39A18716}";
        public static BlueprintFeature UnchainedHeartFeature()
        {
            var icon = FeatureRefs.RenewedVigorFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(UnchainedHeartBuff, UnchainedHeartBuffGuid)
                .SetDisplayName(UnchainedHeartDisplayName)
                .SetDescription(UnchainedHeartDescription)
                .SetIcon(icon)
                .Configure();

            return FeatureConfigurator.New(UnchainedHeart, UnchainedHeartGuid)
              .SetDisplayName(UnchainedHeartDisplayName)
              .SetDescription(UnchainedHeartDescription)
              .SetIcon(icon)
              .AddComponent<UnchainedHeartComp>(c => { c.buff = buff; c.SpellDescriptor = SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion; })
              .AddAttackBonusAgainstFactOwner(bonus: 1, checkedFact: buff)
              .SetRanks(10)
              .Configure();
        }

        private const string PreemptiveStrike = "ForestWardenPreemptiveStrike";
        public static readonly string PreemptiveStrikeGuid = "{8A752251-C459-44B7-8EC6-DEADF71EFACA}";

        internal const string PreemptiveStrikeDisplayName = "ForestWardenPreemptiveStrike.Name";
        private const string PreemptiveStrikeDescription = "ForestWardenPreemptiveStrike.Description";
        public static BlueprintFeature PreemptiveStrikeFeature()
        {
            var icon = FeatureRefs.Opportunist.Reference.Get().Icon;

            return FeatureConfigurator.New(PreemptiveStrike, PreemptiveStrikeGuid)
              .SetDisplayName(PreemptiveStrikeDisplayName)
              .SetDescription(PreemptiveStrikeDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string AlignSpam = "ForestWarden.AlignSpam";
        public static readonly string AlignSpamGuid = "{24453B21-1438-405E-8296-B0523964CB00}";

        internal const string SanctifiedRogueDisplayName = "ForestWardenSanctifiedRogue.Name";
        private const string SanctifiedRogueDescription = "ForestWardenSanctifiedRogue.Description";

        public static BlueprintFeatureSelection AlignSpamFeat()
        {
            var icon = FeatureSelectionRefs.SlayerTalentSelection2.Reference.Get().Icon;

            string GunslingerClass = GunslingerMain.ArchetypeGuid;
            string AgentoftheGraveClass = AgentoftheGrave.ArchetypeGuid;
            string AnchoriteofDawnClass = AnchoriteofDawn.ArchetypeGuid;
            string ArcaneAcherClass = ArcaneArcher.ArchetypeGuid;
            string AsavirClass = Asavir.ArchetypeGuid;
            string ChevalierClass = Chevalier.ArchetypeGuid;
            string CrimsonTemplarClass = CrimsonTemplar.ArchetypeGuid;
            string DeadeyeDevoteeClass = DeadeyeDevotee.ArchetypeGuid;
            string DragonFuryClass = DragonFury.ArchetypeGuid;
            string EsotericKnightClass = EsotericKnight.ArchetypeGuid;
            string ExaltedEvangelistClass = ExaltedEvangelist.ArchetypeGuid;
            string FuriousGuardianClass = FuriousGuardian.ArchetypeGuid;
            string HalflingOpportunistClass = HalflingOpportunist.ArchetypeGuid;
            string HinterlanderClass = Hinterlander.ArchetypeGuid;
            string HorizonWalkerClass = HorizonWalker.ArchetypeGuid;
            string InheritorCrusaderClass = InheritorCrusader.ArchetypeGuid;
            string MammothRiderClass = MammothRider.ArchetypeGuid;
            string SanguineAngelClass = SanguineAngel.ArchetypeGuid;
            string ScarSeekerClass = ScarSeeker.ArchetypeGuid;
            string SentinelClass = Sentinel.ArchetypeGuid;
            string ShadowDancerClass = ShadowDancer.ArchetypeGuid;
            string SouldrinkerClass = Souldrinker.ArchetypeGuid;
            string UmbralAgentClass = UmbralAgent.ArchetypeGuid;
            string MicroAntiPaladinClass = "8939eff2-5a0a-4b77-ad1a-b6be4c760a6c";
            string OathbreakerClass = "B35CE8EE-32C2-4BFD-8884-740F13AAEE12";
            string DreadKnightClass = "D0EB4CA4-4E11-417C-9B2F-0208491067A0";
            string StargazerClass = "7e3cde18-3dad-43ab-a7cc-e14b6ca51216";
            string SwashbucklerClass = "338ABF27-23C1-4C1A-B0F1-7CD7E3020444";
            string HolyVindicatorClass = "b5daf66532f5425aa22df5372c57d766";
            string SummonerClass = "c6a9c7f9-bdce-4c89-aedf-cde62620b2b7";
            string LionBladeClass = LionBlade.ArchetypeGuid;
            string EnchantingCourtesanClass = EnchantingCourtesan.ArchetypeGuid;
            string HeritorKnightClass = HeritorKnight.ArchetypeGuid;
            string GoldenLegionnaireClass = GoldenLegionnaire.ArchetypeGuid;
            string BoltAceClass = BoltAce.ArchetypeGuid;
            string MortalUsherClass = MortalUsher.ArchetypeGuid;
            //string ForestWardenClass = ForestWarden.ArchetypeGuid;

            var list = new List<BlueprintFeature>();

            var ForestAlchemistClasspro = ProgressionConfigurator.New(ForestAlchemistClass0Align, ForestAlchemistClass0AlignGuid)
            .SetDisplayName(ForestAlchemistClass0AlignDisplayName)
            .SetDescription(ForestAlchemistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AlchemistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAlchemistClass2Align, ForestAlchemistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAlchemistClass4Align, ForestAlchemistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAlchemistClass6Align, ForestAlchemistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAlchemistClass8Align, ForestAlchemistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAlchemistClass10Align, ForestAlchemistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestAlchemistClasspro.Configure());
            var ForestArcaneTricksterClasspro = ProgressionConfigurator.New(ForestArcaneTricksterClass0Align, ForestArcaneTricksterClass0AlignGuid)
            .SetDisplayName(ForestArcaneTricksterClass0AlignDisplayName)
            .SetDescription(ForestArcaneTricksterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcaneTricksterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcaneTricksterClass2Align, ForestArcaneTricksterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcaneTricksterClass4Align, ForestArcaneTricksterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcaneTricksterClass6Align, ForestArcaneTricksterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcaneTricksterClass8Align, ForestArcaneTricksterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcaneTricksterClass10Align, ForestArcaneTricksterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcaneTricksterClasspro.Configure());
            var ForestArcanistClasspro = ProgressionConfigurator.New(ForestArcanistClass0Align, ForestArcanistClass0AlignGuid)
            .SetDisplayName(ForestArcanistClass0AlignDisplayName)
            .SetDescription(ForestArcanistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcanistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcanistClass2Align, ForestArcanistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcanistClass4Align, ForestArcanistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcanistClass6Align, ForestArcanistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcanistClass8Align, ForestArcanistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcanistClass10Align, ForestArcanistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcanistClasspro.Configure());
            var ForestAssassinClasspro = ProgressionConfigurator.New(ForestAssassinClass0Align, ForestAssassinClass0AlignGuid)
            .SetDisplayName(ForestAssassinClass0AlignDisplayName)
            .SetDescription(ForestAssassinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AssassinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAssassinClass2Align, ForestAssassinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAssassinClass4Align, ForestAssassinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAssassinClass6Align, ForestAssassinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAssassinClass8Align, ForestAssassinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAssassinClass10Align, ForestAssassinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestAssassinClasspro.Configure());
            var ForestBarbarianClasspro = ProgressionConfigurator.New(ForestBarbarianClass0Align, ForestBarbarianClass0AlignGuid)
            .SetDisplayName(ForestBarbarianClass0AlignDisplayName)
            .SetDescription(ForestBarbarianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBarbarianClass2Align, ForestBarbarianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBarbarianClass4Align, ForestBarbarianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBarbarianClass6Align, ForestBarbarianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBarbarianClass8Align, ForestBarbarianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBarbarianClass10Align, ForestBarbarianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBarbarianClasspro.Configure());
            var ForestBardClasspro = ProgressionConfigurator.New(ForestBardClass0Align, ForestBardClass0AlignGuid)
            .SetDisplayName(ForestBardClass0AlignDisplayName)
            .SetDescription(ForestBardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBardClass2Align, ForestBardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBardClass4Align, ForestBardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBardClass6Align, ForestBardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBardClass8Align, ForestBardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBardClass10Align, ForestBardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBardClasspro.Configure());
            var ForestBloodragerClasspro = ProgressionConfigurator.New(ForestBloodragerClass0Align, ForestBloodragerClass0AlignGuid)
            .SetDisplayName(ForestBloodragerClass0AlignDisplayName)
            .SetDescription(ForestBloodragerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BloodragerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBloodragerClass2Align, ForestBloodragerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBloodragerClass4Align, ForestBloodragerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBloodragerClass6Align, ForestBloodragerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBloodragerClass8Align, ForestBloodragerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBloodragerClass10Align, ForestBloodragerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBloodragerClasspro.Configure());
            var ForestCavalierClasspro = ProgressionConfigurator.New(ForestCavalierClass0Align, ForestCavalierClass0AlignGuid)
            .SetDisplayName(ForestCavalierClass0AlignDisplayName)
            .SetDescription(ForestCavalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.CavalierClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestCavalierClass2Align, ForestCavalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestCavalierClass4Align, ForestCavalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestCavalierClass6Align, ForestCavalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestCavalierClass8Align, ForestCavalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestCavalierClass10Align, ForestCavalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestCavalierClasspro.Configure());
            var ForestClericClasspro = ProgressionConfigurator.New(ForestClericClass0Align, ForestClericClass0AlignGuid)
            .SetDisplayName(ForestClericClass0AlignDisplayName)
            .SetDescription(ForestClericClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestClericClass2Align, ForestClericClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestClericClass4Align, ForestClericClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestClericClass6Align, ForestClericClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestClericClass8Align, ForestClericClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestClericClass10Align, ForestClericClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestClericClasspro.Configure());
            var ForestDragonDiscipleClasspro = ProgressionConfigurator.New(ForestDragonDiscipleClass0Align, ForestDragonDiscipleClass0AlignGuid)
            .SetDisplayName(ForestDragonDiscipleClass0AlignDisplayName)
            .SetDescription(ForestDragonDiscipleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DragonDiscipleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDragonDiscipleClass2Align, ForestDragonDiscipleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDragonDiscipleClass4Align, ForestDragonDiscipleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDragonDiscipleClass6Align, ForestDragonDiscipleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDragonDiscipleClass8Align, ForestDragonDiscipleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDragonDiscipleClass10Align, ForestDragonDiscipleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDragonDiscipleClasspro.Configure());
            var ForestDruidClasspro = ProgressionConfigurator.New(ForestDruidClass0Align, ForestDruidClass0AlignGuid)
            .SetDisplayName(ForestDruidClass0AlignDisplayName)
            .SetDescription(ForestDruidClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DruidClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDruidClass2Align, ForestDruidClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDruidClass4Align, ForestDruidClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDruidClass6Align, ForestDruidClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDruidClass8Align, ForestDruidClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDruidClass10Align, ForestDruidClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDruidClasspro.Configure());
            var ForestDuelistClasspro = ProgressionConfigurator.New(ForestDuelistClass0Align, ForestDuelistClass0AlignGuid)
            .SetDisplayName(ForestDuelistClass0AlignDisplayName)
            .SetDescription(ForestDuelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DuelistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDuelistClass2Align, ForestDuelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDuelistClass4Align, ForestDuelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDuelistClass6Align, ForestDuelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDuelistClass8Align, ForestDuelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDuelistClass10Align, ForestDuelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDuelistClasspro.Configure());
            var ForestEldritchKnightClasspro = ProgressionConfigurator.New(ForestEldritchKnightClass0Align, ForestEldritchKnightClass0AlignGuid)
            .SetDisplayName(ForestEldritchKnightClass0AlignDisplayName)
            .SetDescription(ForestEldritchKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchKnightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEldritchKnightClass2Align, ForestEldritchKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEldritchKnightClass4Align, ForestEldritchKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEldritchKnightClass6Align, ForestEldritchKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEldritchKnightClass8Align, ForestEldritchKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEldritchKnightClass10Align, ForestEldritchKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestEldritchKnightClasspro.Configure());
            var ForestEldritchScionClasspro = ProgressionConfigurator.New(ForestEldritchScionClass0Align, ForestEldritchScionClass0AlignGuid)
            .SetDisplayName(ForestEldritchScionClass0AlignDisplayName)
            .SetDescription(ForestEldritchScionClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchScionClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEldritchScionClass2Align, ForestEldritchScionClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEldritchScionClass4Align, ForestEldritchScionClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEldritchScionClass6Align, ForestEldritchScionClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEldritchScionClass8Align, ForestEldritchScionClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEldritchScionClass10Align, ForestEldritchScionClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestEldritchScionClasspro.Configure());
            var ForestFighterClasspro = ProgressionConfigurator.New(ForestFighterClass0Align, ForestFighterClass0AlignGuid)
            .SetDisplayName(ForestFighterClass0AlignDisplayName)
            .SetDescription(ForestFighterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.FighterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestFighterClass2Align, ForestFighterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestFighterClass4Align, ForestFighterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestFighterClass6Align, ForestFighterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestFighterClass8Align, ForestFighterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestFighterClass10Align, ForestFighterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestFighterClasspro.Configure());
            var ForestHellknightClasspro = ProgressionConfigurator.New(ForestHellknightClass0Align, ForestHellknightClass0AlignGuid)
            .SetDisplayName(ForestHellknightClass0AlignDisplayName)
            .SetDescription(ForestHellknightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHellknightClass2Align, ForestHellknightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHellknightClass4Align, ForestHellknightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHellknightClass6Align, ForestHellknightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHellknightClass8Align, ForestHellknightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHellknightClass10Align, ForestHellknightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHellknightClasspro.Configure());
            var ForestHellknightSigniferClasspro = ProgressionConfigurator.New(ForestHellknightSigniferClass0Align, ForestHellknightSigniferClass0AlignGuid)
            .SetDisplayName(ForestHellknightSigniferClass0AlignDisplayName)
            .SetDescription(ForestHellknightSigniferClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightSigniferClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHellknightSigniferClass2Align, ForestHellknightSigniferClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHellknightSigniferClass4Align, ForestHellknightSigniferClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHellknightSigniferClass6Align, ForestHellknightSigniferClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHellknightSigniferClass8Align, ForestHellknightSigniferClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHellknightSigniferClass10Align, ForestHellknightSigniferClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHellknightSigniferClasspro.Configure());
            var ForestHunterClasspro = ProgressionConfigurator.New(ForestHunterClass0Align, ForestHunterClass0AlignGuid)
            .SetDisplayName(ForestHunterClass0AlignDisplayName)
            .SetDescription(ForestHunterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HunterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHunterClass2Align, ForestHunterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHunterClass4Align, ForestHunterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHunterClass6Align, ForestHunterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHunterClass8Align, ForestHunterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHunterClass10Align, ForestHunterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHunterClasspro.Configure());
            var ForestInquisitorClasspro = ProgressionConfigurator.New(ForestInquisitorClass0Align, ForestInquisitorClass0AlignGuid)
            .SetDisplayName(ForestInquisitorClass0AlignDisplayName)
            .SetDescription(ForestInquisitorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.InquisitorClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestInquisitorClass2Align, ForestInquisitorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestInquisitorClass4Align, ForestInquisitorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestInquisitorClass6Align, ForestInquisitorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestInquisitorClass8Align, ForestInquisitorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestInquisitorClass10Align, ForestInquisitorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestInquisitorClasspro.Configure());
            var ForestKineticistClasspro = ProgressionConfigurator.New(ForestKineticistClass0Align, ForestKineticistClass0AlignGuid)
            .SetDisplayName(ForestKineticistClass0AlignDisplayName)
            .SetDescription(ForestKineticistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestKineticistClass2Align, ForestKineticistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestKineticistClass4Align, ForestKineticistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestKineticistClass6Align, ForestKineticistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestKineticistClass8Align, ForestKineticistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestKineticistClass10Align, ForestKineticistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestKineticistClasspro.Configure());
            var ForestLoremasterClasspro = ProgressionConfigurator.New(ForestLoremasterClass0Align, ForestLoremasterClass0AlignGuid)
            .SetDisplayName(ForestLoremasterClass0AlignDisplayName)
            .SetDescription(ForestLoremasterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.LoremasterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestLoremasterClass2Align, ForestLoremasterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestLoremasterClass4Align, ForestLoremasterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestLoremasterClass6Align, ForestLoremasterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestLoremasterClass8Align, ForestLoremasterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestLoremasterClass10Align, ForestLoremasterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestLoremasterClasspro.Configure());
            var ForestMagusClasspro = ProgressionConfigurator.New(ForestMagusClass0Align, ForestMagusClass0AlignGuid)
            .SetDisplayName(ForestMagusClass0AlignDisplayName)
            .SetDescription(ForestMagusClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMagusClass2Align, ForestMagusClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMagusClass4Align, ForestMagusClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMagusClass6Align, ForestMagusClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMagusClass8Align, ForestMagusClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMagusClass10Align, ForestMagusClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMagusClasspro.Configure());
            var ForestMonkClasspro = ProgressionConfigurator.New(ForestMonkClass0Align, ForestMonkClass0AlignGuid)
            .SetDisplayName(ForestMonkClass0AlignDisplayName)
            .SetDescription(ForestMonkClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMonkClass2Align, ForestMonkClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMonkClass4Align, ForestMonkClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMonkClass6Align, ForestMonkClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMonkClass8Align, ForestMonkClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMonkClass10Align, ForestMonkClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMonkClasspro.Configure());
            var ForestMysticTheurgeClasspro = ProgressionConfigurator.New(ForestMysticTheurgeClass0Align, ForestMysticTheurgeClass0AlignGuid)
            .SetDisplayName(ForestMysticTheurgeClass0AlignDisplayName)
            .SetDescription(ForestMysticTheurgeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MysticTheurgeClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMysticTheurgeClass2Align, ForestMysticTheurgeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMysticTheurgeClass4Align, ForestMysticTheurgeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMysticTheurgeClass6Align, ForestMysticTheurgeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMysticTheurgeClass8Align, ForestMysticTheurgeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMysticTheurgeClass10Align, ForestMysticTheurgeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMysticTheurgeClasspro.Configure());
            var ForestOracleClasspro = ProgressionConfigurator.New(ForestOracleClass0Align, ForestOracleClass0AlignGuid)
            .SetDisplayName(ForestOracleClass0AlignDisplayName)
            .SetDescription(ForestOracleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.OracleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestOracleClass2Align, ForestOracleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestOracleClass4Align, ForestOracleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestOracleClass6Align, ForestOracleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestOracleClass8Align, ForestOracleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestOracleClass10Align, ForestOracleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestOracleClasspro.Configure());
            var ForestPaladinClasspro = ProgressionConfigurator.New(ForestPaladinClass0Align, ForestPaladinClass0AlignGuid)
            .SetDisplayName(ForestPaladinClass0AlignDisplayName)
            .SetDescription(ForestPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.PaladinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestPaladinClass2Align, ForestPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestPaladinClass4Align, ForestPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestPaladinClass6Align, ForestPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestPaladinClass8Align, ForestPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestPaladinClass10Align, ForestPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestPaladinClasspro.Configure());
            var ForestRangerClasspro = ProgressionConfigurator.New(ForestRangerClass0Align, ForestRangerClass0AlignGuid)
            .SetDisplayName(ForestRangerClass0AlignDisplayName)
            .SetDescription(ForestRangerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RangerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestRangerClass2Align, ForestRangerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestRangerClass4Align, ForestRangerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestRangerClass6Align, ForestRangerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestRangerClass8Align, ForestRangerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestRangerClass10Align, ForestRangerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestRangerClasspro.Configure());
            var ForestRogueClasspro = ProgressionConfigurator.New(ForestRogueClass0Align, ForestRogueClass0AlignGuid)
            .SetDisplayName(ForestRogueClass0AlignDisplayName)
            .SetDescription(ForestRogueClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RogueClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestRogueClass2Align, ForestRogueClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestRogueClass4Align, ForestRogueClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestRogueClass6Align, ForestRogueClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestRogueClass8Align, ForestRogueClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestRogueClass10Align, ForestRogueClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestRogueClasspro.Configure());
            var ForestShamanClasspro = ProgressionConfigurator.New(ForestShamanClass0Align, ForestShamanClass0AlignGuid)
            .SetDisplayName(ForestShamanClass0AlignDisplayName)
            .SetDescription(ForestShamanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShamanClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShamanClass2Align, ForestShamanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShamanClass4Align, ForestShamanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShamanClass6Align, ForestShamanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShamanClass8Align, ForestShamanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShamanClass10Align, ForestShamanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestShamanClasspro.Configure());
            var ForestShifterClasspro = ProgressionConfigurator.New(ForestShifterClass0Align, ForestShifterClass0AlignGuid)
            .SetDisplayName(ForestShifterClass0AlignDisplayName)
            .SetDescription(ForestShifterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShifterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShifterClass2Align, ForestShifterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShifterClass4Align, ForestShifterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShifterClass6Align, ForestShifterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShifterClass8Align, ForestShifterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShifterClass10Align, ForestShifterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestShifterClasspro.Configure());
            var ForestSkaldClasspro = ProgressionConfigurator.New(ForestSkaldClass0Align, ForestSkaldClass0AlignGuid)
            .SetDisplayName(ForestSkaldClass0AlignDisplayName)
            .SetDescription(ForestSkaldClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SkaldClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSkaldClass2Align, ForestSkaldClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSkaldClass4Align, ForestSkaldClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSkaldClass6Align, ForestSkaldClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSkaldClass8Align, ForestSkaldClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSkaldClass10Align, ForestSkaldClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSkaldClasspro.Configure());
            var ForestSlayerClasspro = ProgressionConfigurator.New(ForestSlayerClass0Align, ForestSlayerClass0AlignGuid)
            .SetDisplayName(ForestSlayerClass0AlignDisplayName)
            .SetDescription(ForestSlayerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SlayerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSlayerClass2Align, ForestSlayerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSlayerClass4Align, ForestSlayerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSlayerClass6Align, ForestSlayerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSlayerClass8Align, ForestSlayerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSlayerClass10Align, ForestSlayerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSlayerClasspro.Configure());
            var ForestSorcererClasspro = ProgressionConfigurator.New(ForestSorcererClass0Align, ForestSorcererClass0AlignGuid)
            .SetDisplayName(ForestSorcererClass0AlignDisplayName)
            .SetDescription(ForestSorcererClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SorcererClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSorcererClass2Align, ForestSorcererClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSorcererClass4Align, ForestSorcererClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSorcererClass6Align, ForestSorcererClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSorcererClass8Align, ForestSorcererClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSorcererClass10Align, ForestSorcererClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSorcererClasspro.Configure());
            var ForestStalwartDefenderClasspro = ProgressionConfigurator.New(ForestStalwartDefenderClass0Align, ForestStalwartDefenderClass0AlignGuid)
            .SetDisplayName(ForestStalwartDefenderClass0AlignDisplayName)
            .SetDescription(ForestStalwartDefenderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StalwartDefenderClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStalwartDefenderClass2Align, ForestStalwartDefenderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStalwartDefenderClass4Align, ForestStalwartDefenderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStalwartDefenderClass6Align, ForestStalwartDefenderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStalwartDefenderClass8Align, ForestStalwartDefenderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStalwartDefenderClass10Align, ForestStalwartDefenderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestStalwartDefenderClasspro.Configure());
            var ForestStudentOfWarClasspro = ProgressionConfigurator.New(ForestStudentOfWarClass0Align, ForestStudentOfWarClass0AlignGuid)
            .SetDisplayName(ForestStudentOfWarClass0AlignDisplayName)
            .SetDescription(ForestStudentOfWarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StudentOfWarClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStudentOfWarClass2Align, ForestStudentOfWarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStudentOfWarClass4Align, ForestStudentOfWarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStudentOfWarClass6Align, ForestStudentOfWarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStudentOfWarClass8Align, ForestStudentOfWarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStudentOfWarClass10Align, ForestStudentOfWarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestStudentOfWarClasspro.Configure());
            var ForestSwordlordClasspro = ProgressionConfigurator.New(ForestSwordlordClass0Align, ForestSwordlordClass0AlignGuid)
            .SetDisplayName(ForestSwordlordClass0AlignDisplayName)
            .SetDescription(ForestSwordlordClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SwordlordClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSwordlordClass2Align, ForestSwordlordClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSwordlordClass4Align, ForestSwordlordClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSwordlordClass6Align, ForestSwordlordClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSwordlordClass8Align, ForestSwordlordClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSwordlordClass10Align, ForestSwordlordClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSwordlordClasspro.Configure());
            var ForestWarpriestClasspro = ProgressionConfigurator.New(ForestWarpriestClass0Align, ForestWarpriestClass0AlignGuid)
            .SetDisplayName(ForestWarpriestClass0AlignDisplayName)
            .SetDescription(ForestWarpriestClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WarpriestClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWarpriestClass2Align, ForestWarpriestClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWarpriestClass4Align, ForestWarpriestClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWarpriestClass6Align, ForestWarpriestClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWarpriestClass8Align, ForestWarpriestClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWarpriestClass10Align, ForestWarpriestClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWarpriestClasspro.Configure());
            var ForestWinterWitchClasspro = ProgressionConfigurator.New(ForestWinterWitchClass0Align, ForestWinterWitchClass0AlignGuid)
            .SetDisplayName(ForestWinterWitchClass0AlignDisplayName)
            .SetDescription(ForestWinterWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WinterWitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWinterWitchClass2Align, ForestWinterWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWinterWitchClass4Align, ForestWinterWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWinterWitchClass6Align, ForestWinterWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWinterWitchClass8Align, ForestWinterWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWinterWitchClass10Align, ForestWinterWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWinterWitchClasspro.Configure());
            var ForestWitchClasspro = ProgressionConfigurator.New(ForestWitchClass0Align, ForestWitchClass0AlignGuid)
            .SetDisplayName(ForestWitchClass0AlignDisplayName)
            .SetDescription(ForestWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWitchClass2Align, ForestWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWitchClass4Align, ForestWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWitchClass6Align, ForestWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWitchClass8Align, ForestWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWitchClass10Align, ForestWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWitchClasspro.Configure());
            var ForestWizardClasspro = ProgressionConfigurator.New(ForestWizardClass0Align, ForestWizardClass0AlignGuid)
            .SetDisplayName(ForestWizardClass0AlignDisplayName)
            .SetDescription(ForestWizardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WizardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWizardClass2Align, ForestWizardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWizardClass4Align, ForestWizardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWizardClass6Align, ForestWizardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWizardClass8Align, ForestWizardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWizardClass10Align, ForestWizardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWizardClasspro.Configure());
            var ForestGunslingerClasspro = ProgressionConfigurator.New(ForestGunslingerClass0Align, ForestGunslingerClass0AlignGuid)
            .SetDisplayName(ForestGunslingerClass0AlignDisplayName)
            .SetDescription(ForestGunslingerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GunslingerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestGunslingerClass2Align, ForestGunslingerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestGunslingerClass4Align, ForestGunslingerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestGunslingerClass6Align, ForestGunslingerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestGunslingerClass8Align, ForestGunslingerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestGunslingerClass10Align, ForestGunslingerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestGunslingerClasspro.Configure());
            var ForestAgentoftheGraveClasspro = ProgressionConfigurator.New(ForestAgentoftheGraveClass0Align, ForestAgentoftheGraveClass0AlignGuid)
            .SetDisplayName(ForestAgentoftheGraveClass0AlignDisplayName)
            .SetDescription(ForestAgentoftheGraveClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AgentoftheGraveClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAgentoftheGraveClass2Align, ForestAgentoftheGraveClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAgentoftheGraveClass4Align, ForestAgentoftheGraveClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAgentoftheGraveClass6Align, ForestAgentoftheGraveClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAgentoftheGraveClass8Align, ForestAgentoftheGraveClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAgentoftheGraveClass10Align, ForestAgentoftheGraveClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAgentoftheGraveClasspro.Configure());
            var ForestAnchoriteofDawnClasspro = ProgressionConfigurator.New(ForestAnchoriteofDawnClass0Align, ForestAnchoriteofDawnClass0AlignGuid)
            .SetDisplayName(ForestAnchoriteofDawnClass0AlignDisplayName)
            .SetDescription(ForestAnchoriteofDawnClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AnchoriteofDawnClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass2Align, ForestAnchoriteofDawnClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass4Align, ForestAnchoriteofDawnClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass6Align, ForestAnchoriteofDawnClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass8Align, ForestAnchoriteofDawnClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass10Align, ForestAnchoriteofDawnClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAnchoriteofDawnClasspro.Configure());
            var ForestArcaneAcherClasspro = ProgressionConfigurator.New(ForestArcaneAcherClass0Align, ForestArcaneAcherClass0AlignGuid)
            .SetDisplayName(ForestArcaneAcherClass0AlignDisplayName)
            .SetDescription(ForestArcaneAcherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ArcaneAcherClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcaneAcherClass2Align, ForestArcaneAcherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcaneAcherClass4Align, ForestArcaneAcherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcaneAcherClass6Align, ForestArcaneAcherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcaneAcherClass8Align, ForestArcaneAcherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcaneAcherClass10Align, ForestArcaneAcherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcaneAcherClasspro.Configure());
            var ForestAsavirClasspro = ProgressionConfigurator.New(ForestAsavirClass0Align, ForestAsavirClass0AlignGuid)
            .SetDisplayName(ForestAsavirClass0AlignDisplayName)
            .SetDescription(ForestAsavirClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AsavirClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAsavirClass2Align, ForestAsavirClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAsavirClass4Align, ForestAsavirClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAsavirClass6Align, ForestAsavirClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAsavirClass8Align, ForestAsavirClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAsavirClass10Align, ForestAsavirClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAsavirClasspro.Configure());
            var ForestChevalierClasspro = ProgressionConfigurator.New(ForestChevalierClass0Align, ForestChevalierClass0AlignGuid)
            .SetDisplayName(ForestChevalierClass0AlignDisplayName)
            .SetDescription(ForestChevalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ChevalierClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestChevalierClass2Align, ForestChevalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestChevalierClass4Align, ForestChevalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestChevalierClass6Align, ForestChevalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestChevalierClass8Align, ForestChevalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestChevalierClass10Align, ForestChevalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestChevalierClasspro.Configure());
            var ForestCrimsonTemplarClasspro = ProgressionConfigurator.New(ForestCrimsonTemplarClass0Align, ForestCrimsonTemplarClass0AlignGuid)
            .SetDisplayName(ForestCrimsonTemplarClass0AlignDisplayName)
            .SetDescription(ForestCrimsonTemplarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CrimsonTemplarClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestCrimsonTemplarClass2Align, ForestCrimsonTemplarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestCrimsonTemplarClass4Align, ForestCrimsonTemplarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestCrimsonTemplarClass6Align, ForestCrimsonTemplarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestCrimsonTemplarClass8Align, ForestCrimsonTemplarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestCrimsonTemplarClass10Align, ForestCrimsonTemplarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestCrimsonTemplarClasspro.Configure());
            var ForestDeadeyeDevoteeClasspro = ProgressionConfigurator.New(ForestDeadeyeDevoteeClass0Align, ForestDeadeyeDevoteeClass0AlignGuid)
            .SetDisplayName(ForestDeadeyeDevoteeClass0AlignDisplayName)
            .SetDescription(ForestDeadeyeDevoteeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DeadeyeDevoteeClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass2Align, ForestDeadeyeDevoteeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass4Align, ForestDeadeyeDevoteeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass6Align, ForestDeadeyeDevoteeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass8Align, ForestDeadeyeDevoteeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass10Align, ForestDeadeyeDevoteeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDeadeyeDevoteeClasspro.Configure());
            var ForestDragonFuryClasspro = ProgressionConfigurator.New(ForestDragonFuryClass0Align, ForestDragonFuryClass0AlignGuid)
            .SetDisplayName(ForestDragonFuryClass0AlignDisplayName)
            .SetDescription(ForestDragonFuryClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DragonFuryClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDragonFuryClass2Align, ForestDragonFuryClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDragonFuryClass4Align, ForestDragonFuryClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDragonFuryClass6Align, ForestDragonFuryClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDragonFuryClass8Align, ForestDragonFuryClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDragonFuryClass10Align, ForestDragonFuryClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDragonFuryClasspro.Configure());
            var ForestEsotericKnightClasspro = ProgressionConfigurator.New(ForestEsotericKnightClass0Align, ForestEsotericKnightClass0AlignGuid)
            .SetDisplayName(ForestEsotericKnightClass0AlignDisplayName)
            .SetDescription(ForestEsotericKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EsotericKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEsotericKnightClass2Align, ForestEsotericKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEsotericKnightClass4Align, ForestEsotericKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEsotericKnightClass6Align, ForestEsotericKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEsotericKnightClass8Align, ForestEsotericKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEsotericKnightClass10Align, ForestEsotericKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestEsotericKnightClasspro.Configure());
            var ForestExaltedEvangelistClasspro = ProgressionConfigurator.New(ForestExaltedEvangelistClass0Align, ForestExaltedEvangelistClass0AlignGuid)
            .SetDisplayName(ForestExaltedEvangelistClass0AlignDisplayName)
            .SetDescription(ForestExaltedEvangelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ExaltedEvangelistClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestExaltedEvangelistClass2Align, ForestExaltedEvangelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestExaltedEvangelistClass4Align, ForestExaltedEvangelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestExaltedEvangelistClass6Align, ForestExaltedEvangelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestExaltedEvangelistClass8Align, ForestExaltedEvangelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestExaltedEvangelistClass10Align, ForestExaltedEvangelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestExaltedEvangelistClasspro.Configure());
            var ForestFuriousGuardianClasspro = ProgressionConfigurator.New(ForestFuriousGuardianClass0Align, ForestFuriousGuardianClass0AlignGuid)
            .SetDisplayName(ForestFuriousGuardianClass0AlignDisplayName)
            .SetDescription(ForestFuriousGuardianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(FuriousGuardianClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestFuriousGuardianClass2Align, ForestFuriousGuardianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestFuriousGuardianClass4Align, ForestFuriousGuardianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestFuriousGuardianClass6Align, ForestFuriousGuardianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestFuriousGuardianClass8Align, ForestFuriousGuardianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestFuriousGuardianClass10Align, ForestFuriousGuardianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestFuriousGuardianClasspro.Configure());
            var ForestHalflingOpportunistClasspro = ProgressionConfigurator.New(ForestHalflingOpportunistClass0Align, ForestHalflingOpportunistClass0AlignGuid)
            .SetDisplayName(ForestHalflingOpportunistClass0AlignDisplayName)
            .SetDescription(ForestHalflingOpportunistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HalflingOpportunistClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHalflingOpportunistClass2Align, ForestHalflingOpportunistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHalflingOpportunistClass4Align, ForestHalflingOpportunistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHalflingOpportunistClass6Align, ForestHalflingOpportunistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHalflingOpportunistClass8Align, ForestHalflingOpportunistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHalflingOpportunistClass10Align, ForestHalflingOpportunistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHalflingOpportunistClasspro.Configure());
            var ForestHinterlanderClasspro = ProgressionConfigurator.New(ForestHinterlanderClass0Align, ForestHinterlanderClass0AlignGuid)
            .SetDisplayName(ForestHinterlanderClass0AlignDisplayName)
            .SetDescription(ForestHinterlanderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HinterlanderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHinterlanderClass2Align, ForestHinterlanderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHinterlanderClass4Align, ForestHinterlanderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHinterlanderClass6Align, ForestHinterlanderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHinterlanderClass8Align, ForestHinterlanderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHinterlanderClass10Align, ForestHinterlanderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHinterlanderClasspro.Configure());
            var ForestHorizonWalkerClasspro = ProgressionConfigurator.New(ForestHorizonWalkerClass0Align, ForestHorizonWalkerClass0AlignGuid)
            .SetDisplayName(ForestHorizonWalkerClass0AlignDisplayName)
            .SetDescription(ForestHorizonWalkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HorizonWalkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHorizonWalkerClass2Align, ForestHorizonWalkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHorizonWalkerClass4Align, ForestHorizonWalkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHorizonWalkerClass6Align, ForestHorizonWalkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHorizonWalkerClass8Align, ForestHorizonWalkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHorizonWalkerClass10Align, ForestHorizonWalkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHorizonWalkerClasspro.Configure());
            var ForestInheritorCrusaderClasspro = ProgressionConfigurator.New(ForestInheritorCrusaderClass0Align, ForestInheritorCrusaderClass0AlignGuid)
            .SetDisplayName(ForestInheritorCrusaderClass0AlignDisplayName)
            .SetDescription(ForestInheritorCrusaderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(InheritorCrusaderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestInheritorCrusaderClass2Align, ForestInheritorCrusaderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestInheritorCrusaderClass4Align, ForestInheritorCrusaderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestInheritorCrusaderClass6Align, ForestInheritorCrusaderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestInheritorCrusaderClass8Align, ForestInheritorCrusaderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestInheritorCrusaderClass10Align, ForestInheritorCrusaderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestInheritorCrusaderClasspro.Configure());
            var ForestMammothRiderClasspro = ProgressionConfigurator.New(ForestMammothRiderClass0Align, ForestMammothRiderClass0AlignGuid)
            .SetDisplayName(ForestMammothRiderClass0AlignDisplayName)
            .SetDescription(ForestMammothRiderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MammothRiderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMammothRiderClass2Align, ForestMammothRiderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMammothRiderClass4Align, ForestMammothRiderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMammothRiderClass6Align, ForestMammothRiderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMammothRiderClass8Align, ForestMammothRiderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMammothRiderClass10Align, ForestMammothRiderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMammothRiderClasspro.Configure());
            var ForestSanguineAngelClasspro = ProgressionConfigurator.New(ForestSanguineAngelClass0Align, ForestSanguineAngelClass0AlignGuid)
            .SetDisplayName(ForestSanguineAngelClass0AlignDisplayName)
            .SetDescription(ForestSanguineAngelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SanguineAngelClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSanguineAngelClass2Align, ForestSanguineAngelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSanguineAngelClass4Align, ForestSanguineAngelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSanguineAngelClass6Align, ForestSanguineAngelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSanguineAngelClass8Align, ForestSanguineAngelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSanguineAngelClass10Align, ForestSanguineAngelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSanguineAngelClasspro.Configure());
            var ForestScarSeekerClasspro = ProgressionConfigurator.New(ForestScarSeekerClass0Align, ForestScarSeekerClass0AlignGuid)
            .SetDisplayName(ForestScarSeekerClass0AlignDisplayName)
            .SetDescription(ForestScarSeekerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ScarSeekerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestScarSeekerClass2Align, ForestScarSeekerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestScarSeekerClass4Align, ForestScarSeekerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestScarSeekerClass6Align, ForestScarSeekerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestScarSeekerClass8Align, ForestScarSeekerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestScarSeekerClass10Align, ForestScarSeekerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestScarSeekerClasspro.Configure());
            var ForestSentinelClasspro = ProgressionConfigurator.New(ForestSentinelClass0Align, ForestSentinelClass0AlignGuid)
            .SetDisplayName(ForestSentinelClass0AlignDisplayName)
            .SetDescription(ForestSentinelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SentinelClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSentinelClass2Align, ForestSentinelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSentinelClass4Align, ForestSentinelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSentinelClass6Align, ForestSentinelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSentinelClass8Align, ForestSentinelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSentinelClass10Align, ForestSentinelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSentinelClasspro.Configure());
            var ForestShadowDancerClasspro = ProgressionConfigurator.New(ForestShadowDancerClass0Align, ForestShadowDancerClass0AlignGuid)
            .SetDisplayName(ForestShadowDancerClass0AlignDisplayName)
            .SetDescription(ForestShadowDancerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ShadowDancerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShadowDancerClass2Align, ForestShadowDancerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShadowDancerClass4Align, ForestShadowDancerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShadowDancerClass6Align, ForestShadowDancerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShadowDancerClass8Align, ForestShadowDancerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShadowDancerClass10Align, ForestShadowDancerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestShadowDancerClasspro.Configure());
            var ForestSouldrinkerClasspro = ProgressionConfigurator.New(ForestSouldrinkerClass0Align, ForestSouldrinkerClass0AlignGuid)
            .SetDisplayName(ForestSouldrinkerClass0AlignDisplayName)
            .SetDescription(ForestSouldrinkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SouldrinkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSouldrinkerClass2Align, ForestSouldrinkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSouldrinkerClass4Align, ForestSouldrinkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSouldrinkerClass6Align, ForestSouldrinkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSouldrinkerClass8Align, ForestSouldrinkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSouldrinkerClass10Align, ForestSouldrinkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSouldrinkerClasspro.Configure());
            var ForestUmbralAgentClasspro = ProgressionConfigurator.New(ForestUmbralAgentClass0Align, ForestUmbralAgentClass0AlignGuid)
            .SetDisplayName(ForestUmbralAgentClass0AlignDisplayName)
            .SetDescription(ForestUmbralAgentClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(UmbralAgentClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestUmbralAgentClass2Align, ForestUmbralAgentClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestUmbralAgentClass4Align, ForestUmbralAgentClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestUmbralAgentClass6Align, ForestUmbralAgentClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestUmbralAgentClass8Align, ForestUmbralAgentClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestUmbralAgentClass10Align, ForestUmbralAgentClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestUmbralAgentClasspro.Configure());
            var ForestMicroAntiPaladinClasspro = ProgressionConfigurator.New(ForestMicroAntiPaladinClass0Align, ForestMicroAntiPaladinClass0AlignGuid)
            .SetDisplayName(ForestMicroAntiPaladinClass0AlignDisplayName)
            .SetDescription(ForestMicroAntiPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MicroAntiPaladinClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass2Align, ForestMicroAntiPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass4Align, ForestMicroAntiPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass6Align, ForestMicroAntiPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass8Align, ForestMicroAntiPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass10Align, ForestMicroAntiPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMicroAntiPaladinClasspro.Configure());
            var ForestOathbreakerClasspro = ProgressionConfigurator.New(ForestOathbreakerClass0Align, ForestOathbreakerClass0AlignGuid)
            .SetDisplayName(ForestOathbreakerClass0AlignDisplayName)
            .SetDescription(ForestOathbreakerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(OathbreakerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestOathbreakerClass2Align, ForestOathbreakerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestOathbreakerClass4Align, ForestOathbreakerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestOathbreakerClass6Align, ForestOathbreakerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestOathbreakerClass8Align, ForestOathbreakerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestOathbreakerClass10Align, ForestOathbreakerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestOathbreakerClasspro.Configure());
            var ForestDreadKnightClasspro = ProgressionConfigurator.New(ForestDreadKnightClass0Align, ForestDreadKnightClass0AlignGuid)
            .SetDisplayName(ForestDreadKnightClass0AlignDisplayName)
            .SetDescription(ForestDreadKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DreadKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDreadKnightClass2Align, ForestDreadKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDreadKnightClass4Align, ForestDreadKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDreadKnightClass6Align, ForestDreadKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDreadKnightClass8Align, ForestDreadKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDreadKnightClass10Align, ForestDreadKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDreadKnightClasspro.Configure());
            var ForestStargazerClasspro = ProgressionConfigurator.New(ForestStargazerClass0Align, ForestStargazerClass0AlignGuid)
            .SetDisplayName(ForestStargazerClass0AlignDisplayName)
            .SetDescription(ForestStargazerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(StargazerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStargazerClass2Align, ForestStargazerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStargazerClass4Align, ForestStargazerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStargazerClass6Align, ForestStargazerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStargazerClass8Align, ForestStargazerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStargazerClass10Align, ForestStargazerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestStargazerClasspro.Configure());
            var ForestSwashbucklerClasspro = ProgressionConfigurator.New(ForestSwashbucklerClass0Align, ForestSwashbucklerClass0AlignGuid)
            .SetDisplayName(ForestSwashbucklerClass0AlignDisplayName)
            .SetDescription(ForestSwashbucklerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SwashbucklerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSwashbucklerClass2Align, ForestSwashbucklerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSwashbucklerClass4Align, ForestSwashbucklerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSwashbucklerClass6Align, ForestSwashbucklerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSwashbucklerClass8Align, ForestSwashbucklerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSwashbucklerClass10Align, ForestSwashbucklerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSwashbucklerClasspro.Configure());
            var ForestHolyVindicatorClasspro = ProgressionConfigurator.New(ForestHolyVindicatorClass0Align, ForestHolyVindicatorClass0AlignGuid)
            .SetDisplayName(ForestHolyVindicatorClass0AlignDisplayName)
            .SetDescription(ForestHolyVindicatorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HolyVindicatorClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHolyVindicatorClass2Align, ForestHolyVindicatorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHolyVindicatorClass4Align, ForestHolyVindicatorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHolyVindicatorClass6Align, ForestHolyVindicatorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHolyVindicatorClass8Align, ForestHolyVindicatorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHolyVindicatorClass10Align, ForestHolyVindicatorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHolyVindicatorClasspro.Configure());
            var ForestSummonerClasspro = ProgressionConfigurator.New(ForestSummonerClass0Align, ForestSummonerClass0AlignGuid)
            .SetDisplayName(ForestSummonerClass0AlignDisplayName)
            .SetDescription(ForestSummonerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SummonerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSummonerClass2Align, ForestSummonerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSummonerClass4Align, ForestSummonerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSummonerClass6Align, ForestSummonerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSummonerClass8Align, ForestSummonerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSummonerClass10Align, ForestSummonerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSummonerClasspro.Configure());
            var ForestLionBladeClasspro = ProgressionConfigurator.New(ForestLionBladeClass0Align, ForestLionBladeClass0AlignGuid)
            .SetDisplayName(ForestLionBladeClass0AlignDisplayName)
            .SetDescription(ForestLionBladeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(LionBladeClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestLionBladeClass2Align, ForestLionBladeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestLionBladeClass4Align, ForestLionBladeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestLionBladeClass6Align, ForestLionBladeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestLionBladeClass8Align, ForestLionBladeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestLionBladeClass10Align, ForestLionBladeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestLionBladeClasspro.Configure());
            var ForestEnchantingCourtesanClasspro = ProgressionConfigurator.New(ForestEnchantingCourtesanClass0Align, ForestEnchantingCourtesanClass0AlignGuid)
            .SetDisplayName(ForestEnchantingCourtesanClass0AlignDisplayName)
            .SetDescription(ForestEnchantingCourtesanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EnchantingCourtesanClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass2Align, ForestEnchantingCourtesanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass4Align, ForestEnchantingCourtesanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass6Align, ForestEnchantingCourtesanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass8Align, ForestEnchantingCourtesanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass10Align, ForestEnchantingCourtesanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestEnchantingCourtesanClasspro.Configure());
            var ForestHeritorKnightClasspro = ProgressionConfigurator.New(ForestHeritorKnightClass0Align, ForestHeritorKnightClass0AlignGuid)
            .SetDisplayName(ForestHeritorKnightClass0AlignDisplayName)
            .SetDescription(ForestHeritorKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HeritorKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHeritorKnightClass2Align, ForestHeritorKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHeritorKnightClass4Align, ForestHeritorKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHeritorKnightClass6Align, ForestHeritorKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHeritorKnightClass8Align, ForestHeritorKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHeritorKnightClass10Align, ForestHeritorKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHeritorKnightClasspro.Configure());
            var ForestGoldenLegionnaireClasspro = ProgressionConfigurator.New(ForestGoldenLegionnaireClass0Align, ForestGoldenLegionnaireClass0AlignGuid)
            .SetDisplayName(ForestGoldenLegionnaireClass0AlignDisplayName)
            .SetDescription(ForestGoldenLegionnaireClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GoldenLegionnaireClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass2Align, ForestGoldenLegionnaireClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass4Align, ForestGoldenLegionnaireClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass6Align, ForestGoldenLegionnaireClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass8Align, ForestGoldenLegionnaireClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass10Align, ForestGoldenLegionnaireClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestGoldenLegionnaireClasspro.Configure());
            var ForestBoltAceClasspro = ProgressionConfigurator.New(ForestBoltAceClass0Align, ForestBoltAceClass0AlignGuid)
            .SetDisplayName(ForestBoltAceClass0AlignDisplayName)
            .SetDescription(ForestBoltAceClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(BoltAceClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBoltAceClass2Align, ForestBoltAceClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBoltAceClass4Align, ForestBoltAceClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBoltAceClass6Align, ForestBoltAceClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBoltAceClass8Align, ForestBoltAceClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBoltAceClass10Align, ForestBoltAceClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestBoltAceClasspro.Configure());
            var ForestMortalUsherClasspro = ProgressionConfigurator.New(ForestMortalUsherClass0Align, ForestMortalUsherClass0AlignGuid)
            .SetDisplayName(ForestMortalUsherClass0AlignDisplayName)
            .SetDescription(ForestMortalUsherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MortalUsherClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMortalUsherClass2Align, ForestMortalUsherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMortalUsherClass4Align, ForestMortalUsherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMortalUsherClass6Align, ForestMortalUsherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMortalUsherClass8Align, ForestMortalUsherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMortalUsherClass10Align, ForestMortalUsherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMortalUsherClasspro.Configure());

            var select = FeatureSelectionConfigurator.New(AlignSpam, AlignSpamGuid)
              .SetDisplayName(SanctifiedRogueDisplayName)
              .SetDescription(SanctifiedRogueDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false);

            foreach (var feature in list)
            {
                select = select.AddToAllFeatures(feature);
            }

            return select.Configure();
        }

        private const string ForestAlchemistClass0Align = "ForestAlchemistClass0Align";
        private static readonly string ForestAlchemistClass0AlignGuid = "7307436f-df22-4e99-933d-3872213304c8";
        internal const string ForestAlchemistClass0AlignDisplayName = "EvangelistAlchemistClass0Align.Name";
        private const string ForestAlchemistClass0AlignDescription = "EvangelistAlchemistClass0Align.Description";
        private const string ForestAlchemistClass2Align = "ForestAlchemistClass2Align";
        private static readonly string ForestAlchemistClass2AlignGuid = "0f9b9d08-69a3-4074-885b-d9b0e96cccb7";
        private const string ForestAlchemistClass4Align = "ForestAlchemistClass4Align";
        private static readonly string ForestAlchemistClass4AlignGuid = "bfabb083-d575-492b-a8c2-52fb772ae6e4";
        private const string ForestAlchemistClass6Align = "ForestAlchemistClass6Align";
        private static readonly string ForestAlchemistClass6AlignGuid = "8c23f460-0929-4dcc-9591-349766297a77";
        private const string ForestAlchemistClass8Align = "ForestAlchemistClass8Align";
        private static readonly string ForestAlchemistClass8AlignGuid = "9c5c2f0d-e7f4-4fb2-bc25-a93554dc4967";
        private const string ForestAlchemistClass10Align = "ForestAlchemistClass10Align";
        private static readonly string ForestAlchemistClass10AlignGuid = "72b82191-051c-472c-b7f5-bf91f9b2edbf";
        private const string ForestArcaneTricksterClass0Align = "ForestArcaneTricksterClass0Align";
        private static readonly string ForestArcaneTricksterClass0AlignGuid = "23b559fc-40e0-4353-a948-d942dcc7d18e";
        internal const string ForestArcaneTricksterClass0AlignDisplayName = "EvangelistArcaneTricksterClass0Align.Name";
        private const string ForestArcaneTricksterClass0AlignDescription = "EvangelistArcaneTricksterClass0Align.Description";
        private const string ForestArcaneTricksterClass2Align = "ForestArcaneTricksterClass2Align";
        private static readonly string ForestArcaneTricksterClass2AlignGuid = "e9f1148c-0610-45a7-a01a-75acf951e2c5";
        private const string ForestArcaneTricksterClass4Align = "ForestArcaneTricksterClass4Align";
        private static readonly string ForestArcaneTricksterClass4AlignGuid = "52aac8c4-8321-4e25-bb1a-99604b229706";
        private const string ForestArcaneTricksterClass6Align = "ForestArcaneTricksterClass6Align";
        private static readonly string ForestArcaneTricksterClass6AlignGuid = "ccc66390-ec47-4289-ba23-d1c10fdbd7fd";
        private const string ForestArcaneTricksterClass8Align = "ForestArcaneTricksterClass8Align";
        private static readonly string ForestArcaneTricksterClass8AlignGuid = "1005279e-0781-45f5-b661-2d94f13051ae";
        private const string ForestArcaneTricksterClass10Align = "ForestArcaneTricksterClass10Align";
        private static readonly string ForestArcaneTricksterClass10AlignGuid = "7568062a-4924-4882-88c5-f9ead84764dd";
        private const string ForestArcanistClass0Align = "ForestArcanistClass0Align";
        private static readonly string ForestArcanistClass0AlignGuid = "d3ad299f-16bf-4448-9b5b-140d32900ed0";
        internal const string ForestArcanistClass0AlignDisplayName = "EvangelistArcanistClass0Align.Name";
        private const string ForestArcanistClass0AlignDescription = "EvangelistArcanistClass0Align.Description";
        private const string ForestArcanistClass2Align = "ForestArcanistClass2Align";
        private static readonly string ForestArcanistClass2AlignGuid = "76b93229-47ab-47d9-90c0-12e74f74ff38";
        private const string ForestArcanistClass4Align = "ForestArcanistClass4Align";
        private static readonly string ForestArcanistClass4AlignGuid = "90cec6a2-f4be-4426-85b7-cbb99d2ada28";
        private const string ForestArcanistClass6Align = "ForestArcanistClass6Align";
        private static readonly string ForestArcanistClass6AlignGuid = "d9b8618a-bcff-4eac-ae79-b5748f3cfef7";
        private const string ForestArcanistClass8Align = "ForestArcanistClass8Align";
        private static readonly string ForestArcanistClass8AlignGuid = "fd17e3be-6d09-44f0-a303-067d68350542";
        private const string ForestArcanistClass10Align = "ForestArcanistClass10Align";
        private static readonly string ForestArcanistClass10AlignGuid = "e4acecb8-a836-408a-9d64-3a9199e1fd0c";
        private const string ForestAssassinClass0Align = "ForestAssassinClass0Align";
        private static readonly string ForestAssassinClass0AlignGuid = "0c17c781-a0e9-4a49-9795-9581a7bf4b70";
        internal const string ForestAssassinClass0AlignDisplayName = "EvangelistAssassinClass0Align.Name";
        private const string ForestAssassinClass0AlignDescription = "EvangelistAssassinClass0Align.Description";
        private const string ForestAssassinClass2Align = "ForestAssassinClass2Align";
        private static readonly string ForestAssassinClass2AlignGuid = "d041719a-0482-441d-b9ea-4c91cfec636c";
        private const string ForestAssassinClass4Align = "ForestAssassinClass4Align";
        private static readonly string ForestAssassinClass4AlignGuid = "6411991d-cf68-4088-b306-59ceac9196bc";
        private const string ForestAssassinClass6Align = "ForestAssassinClass6Align";
        private static readonly string ForestAssassinClass6AlignGuid = "bd870e7f-c2b0-4b80-81a6-b68bc877d83b";
        private const string ForestAssassinClass8Align = "ForestAssassinClass8Align";
        private static readonly string ForestAssassinClass8AlignGuid = "d81b4f4e-fcbe-406e-a218-114d7a3fd6ce";
        private const string ForestAssassinClass10Align = "ForestAssassinClass10Align";
        private static readonly string ForestAssassinClass10AlignGuid = "63e6064f-8f7b-44c0-8c8a-5e4d3789d55b";
        private const string ForestBarbarianClass0Align = "ForestBarbarianClass0Align";
        private static readonly string ForestBarbarianClass0AlignGuid = "556c74f3-5365-4df8-b8e6-7c472d89f6f0";
        internal const string ForestBarbarianClass0AlignDisplayName = "EvangelistBarbarianClass0Align.Name";
        private const string ForestBarbarianClass0AlignDescription = "EvangelistBarbarianClass0Align.Description";
        private const string ForestBarbarianClass2Align = "ForestBarbarianClass2Align";
        private static readonly string ForestBarbarianClass2AlignGuid = "5e5ebfb2-df16-4f85-8bf6-93d725e85876";
        private const string ForestBarbarianClass4Align = "ForestBarbarianClass4Align";
        private static readonly string ForestBarbarianClass4AlignGuid = "5fd682a2-7c7e-442c-a128-95ff64cfad86";
        private const string ForestBarbarianClass6Align = "ForestBarbarianClass6Align";
        private static readonly string ForestBarbarianClass6AlignGuid = "3adbb656-a13e-4860-b626-c7b60d6965cb";
        private const string ForestBarbarianClass8Align = "ForestBarbarianClass8Align";
        private static readonly string ForestBarbarianClass8AlignGuid = "0cb9e1c4-a680-4781-847d-cc483b6cf4cf";
        private const string ForestBarbarianClass10Align = "ForestBarbarianClass10Align";
        private static readonly string ForestBarbarianClass10AlignGuid = "75ac75fb-e15c-409a-9f78-2ab959355b2a";
        private const string ForestBardClass0Align = "ForestBardClass0Align";
        private static readonly string ForestBardClass0AlignGuid = "16489b7a-0ff8-4a6c-bd37-f92eab188626";
        internal const string ForestBardClass0AlignDisplayName = "EvangelistBardClass0Align.Name";
        private const string ForestBardClass0AlignDescription = "EvangelistBardClass0Align.Description";
        private const string ForestBardClass2Align = "ForestBardClass2Align";
        private static readonly string ForestBardClass2AlignGuid = "7c692ddf-6b83-4bb7-89f9-b3a12a26c72b";
        private const string ForestBardClass4Align = "ForestBardClass4Align";
        private static readonly string ForestBardClass4AlignGuid = "5b8a64a5-1d32-40d8-a059-e122f7184d5e";
        private const string ForestBardClass6Align = "ForestBardClass6Align";
        private static readonly string ForestBardClass6AlignGuid = "94de8100-0662-436f-ac51-312468de961a";
        private const string ForestBardClass8Align = "ForestBardClass8Align";
        private static readonly string ForestBardClass8AlignGuid = "c82dc1b9-5e58-4204-b02e-a8422748a07b";
        private const string ForestBardClass10Align = "ForestBardClass10Align";
        private static readonly string ForestBardClass10AlignGuid = "f42607ee-911e-4499-81ee-0de4af279202";
        private const string ForestBloodragerClass0Align = "ForestBloodragerClass0Align";
        private static readonly string ForestBloodragerClass0AlignGuid = "d90a7f44-44a3-47b4-aaef-0434aaf9e36e";
        internal const string ForestBloodragerClass0AlignDisplayName = "EvangelistBloodragerClass0Align.Name";
        private const string ForestBloodragerClass0AlignDescription = "EvangelistBloodragerClass0Align.Description";
        private const string ForestBloodragerClass2Align = "ForestBloodragerClass2Align";
        private static readonly string ForestBloodragerClass2AlignGuid = "b3f28157-9208-4d9d-a15b-526a4fa04cac";
        private const string ForestBloodragerClass4Align = "ForestBloodragerClass4Align";
        private static readonly string ForestBloodragerClass4AlignGuid = "02baf7ee-4da7-4250-a6ea-3d24cb8fc195";
        private const string ForestBloodragerClass6Align = "ForestBloodragerClass6Align";
        private static readonly string ForestBloodragerClass6AlignGuid = "a28044fc-b9f8-4e6a-aade-986e3b027e8f";
        private const string ForestBloodragerClass8Align = "ForestBloodragerClass8Align";
        private static readonly string ForestBloodragerClass8AlignGuid = "1c5b62a0-9419-4753-986d-93f1d8979506";
        private const string ForestBloodragerClass10Align = "ForestBloodragerClass10Align";
        private static readonly string ForestBloodragerClass10AlignGuid = "9c276a5f-9b38-4b9f-858f-fd511b30177a";
        private const string ForestCavalierClass0Align = "ForestCavalierClass0Align";
        private static readonly string ForestCavalierClass0AlignGuid = "5cd0296b-3e80-4f82-a94e-be2586ff1576";
        internal const string ForestCavalierClass0AlignDisplayName = "EvangelistCavalierClass0Align.Name";
        private const string ForestCavalierClass0AlignDescription = "EvangelistCavalierClass0Align.Description";
        private const string ForestCavalierClass2Align = "ForestCavalierClass2Align";
        private static readonly string ForestCavalierClass2AlignGuid = "16756e18-fb1e-4fef-acb9-eaae67c38d15";
        private const string ForestCavalierClass4Align = "ForestCavalierClass4Align";
        private static readonly string ForestCavalierClass4AlignGuid = "08303999-b3ec-4248-881f-26b9af080aea";
        private const string ForestCavalierClass6Align = "ForestCavalierClass6Align";
        private static readonly string ForestCavalierClass6AlignGuid = "3d255036-f011-4b5e-8588-02eb6d360721";
        private const string ForestCavalierClass8Align = "ForestCavalierClass8Align";
        private static readonly string ForestCavalierClass8AlignGuid = "a6f40e1f-48bd-45e4-bb68-5ced4e565de9";
        private const string ForestCavalierClass10Align = "ForestCavalierClass10Align";
        private static readonly string ForestCavalierClass10AlignGuid = "9113f6ac-d918-4191-94bc-b5b8ba179a49";
        private const string ForestClericClass0Align = "ForestClericClass0Align";
        private static readonly string ForestClericClass0AlignGuid = "0e02c9e9-c669-41ce-a260-730ca970a3dc";
        internal const string ForestClericClass0AlignDisplayName = "EvangelistClericClass0Align.Name";
        private const string ForestClericClass0AlignDescription = "EvangelistClericClass0Align.Description";
        private const string ForestClericClass2Align = "ForestClericClass2Align";
        private static readonly string ForestClericClass2AlignGuid = "dffd4321-eae0-4809-9cbd-5214e7d5b74e";
        private const string ForestClericClass4Align = "ForestClericClass4Align";
        private static readonly string ForestClericClass4AlignGuid = "d0372c4d-3a17-43a0-ba46-23c9ad1bf833";
        private const string ForestClericClass6Align = "ForestClericClass6Align";
        private static readonly string ForestClericClass6AlignGuid = "ed42e369-531c-48e6-9a6f-d7857680efb0";
        private const string ForestClericClass8Align = "ForestClericClass8Align";
        private static readonly string ForestClericClass8AlignGuid = "997e9bde-1004-4e55-a856-523896e65a49";
        private const string ForestClericClass10Align = "ForestClericClass10Align";
        private static readonly string ForestClericClass10AlignGuid = "cbaa00fa-41fa-4ace-bd49-1e4a82f3abc1";
        private const string ForestDragonDiscipleClass0Align = "ForestDragonDiscipleClass0Align";
        private static readonly string ForestDragonDiscipleClass0AlignGuid = "6cd0727a-6c4e-414c-892a-2e34a3b8966b";
        internal const string ForestDragonDiscipleClass0AlignDisplayName = "EvangelistDragonDiscipleClass0Align.Name";
        private const string ForestDragonDiscipleClass0AlignDescription = "EvangelistDragonDiscipleClass0Align.Description";
        private const string ForestDragonDiscipleClass2Align = "ForestDragonDiscipleClass2Align";
        private static readonly string ForestDragonDiscipleClass2AlignGuid = "6df4ede1-2e86-4b18-9762-2e3ebb246875";
        private const string ForestDragonDiscipleClass4Align = "ForestDragonDiscipleClass4Align";
        private static readonly string ForestDragonDiscipleClass4AlignGuid = "95d7bcd5-213f-484e-9ff5-566512d07f5e";
        private const string ForestDragonDiscipleClass6Align = "ForestDragonDiscipleClass6Align";
        private static readonly string ForestDragonDiscipleClass6AlignGuid = "0e03e8b9-25e9-4e0f-8ab3-6d15acc2f396";
        private const string ForestDragonDiscipleClass8Align = "ForestDragonDiscipleClass8Align";
        private static readonly string ForestDragonDiscipleClass8AlignGuid = "68b35546-4e85-479c-80a8-538e903ac3ed";
        private const string ForestDragonDiscipleClass10Align = "ForestDragonDiscipleClass10Align";
        private static readonly string ForestDragonDiscipleClass10AlignGuid = "6142d420-fd89-4c80-900c-f113d123c140";
        private const string ForestDruidClass0Align = "ForestDruidClass0Align";
        private static readonly string ForestDruidClass0AlignGuid = "ee8a2175-88cf-41c9-a0c7-9be5a7c0fbc8";
        internal const string ForestDruidClass0AlignDisplayName = "EvangelistDruidClass0Align.Name";
        private const string ForestDruidClass0AlignDescription = "EvangelistDruidClass0Align.Description";
        private const string ForestDruidClass2Align = "ForestDruidClass2Align";
        private static readonly string ForestDruidClass2AlignGuid = "5d01b7b1-d805-432a-acf6-9b8f28d17ecc";
        private const string ForestDruidClass4Align = "ForestDruidClass4Align";
        private static readonly string ForestDruidClass4AlignGuid = "fe527eb5-26f8-41ec-8fa5-1137fc07049f";
        private const string ForestDruidClass6Align = "ForestDruidClass6Align";
        private static readonly string ForestDruidClass6AlignGuid = "ae7c24e3-d81d-4360-bb0d-a90815fa10f8";
        private const string ForestDruidClass8Align = "ForestDruidClass8Align";
        private static readonly string ForestDruidClass8AlignGuid = "8ede0650-5361-4c85-b276-a374b65931ff";
        private const string ForestDruidClass10Align = "ForestDruidClass10Align";
        private static readonly string ForestDruidClass10AlignGuid = "e3c8bf3b-b0fc-41b2-9203-5a003d565132";
        private const string ForestDuelistClass0Align = "ForestDuelistClass0Align";
        private static readonly string ForestDuelistClass0AlignGuid = "6dcba279-ab12-4763-a306-aee7f65c5726";
        internal const string ForestDuelistClass0AlignDisplayName = "EvangelistDuelistClass0Align.Name";
        private const string ForestDuelistClass0AlignDescription = "EvangelistDuelistClass0Align.Description";
        private const string ForestDuelistClass2Align = "ForestDuelistClass2Align";
        private static readonly string ForestDuelistClass2AlignGuid = "ed316f2c-fbd8-4e15-8e8e-491674812d54";
        private const string ForestDuelistClass4Align = "ForestDuelistClass4Align";
        private static readonly string ForestDuelistClass4AlignGuid = "ad64976a-d297-4399-8c01-cbed74498c54";
        private const string ForestDuelistClass6Align = "ForestDuelistClass6Align";
        private static readonly string ForestDuelistClass6AlignGuid = "058d6222-fbb1-4d95-a72f-48ca6302a5b3";
        private const string ForestDuelistClass8Align = "ForestDuelistClass8Align";
        private static readonly string ForestDuelistClass8AlignGuid = "7ff1a9c3-216f-4f57-a5b5-012dbd6859b9";
        private const string ForestDuelistClass10Align = "ForestDuelistClass10Align";
        private static readonly string ForestDuelistClass10AlignGuid = "261282bb-79d6-4922-bb0e-be988ab6b1fd";
        private const string ForestEldritchKnightClass0Align = "ForestEldritchKnightClass0Align";
        private static readonly string ForestEldritchKnightClass0AlignGuid = "eabc5c0c-bf38-437d-aff1-558b40cff5ae";
        internal const string ForestEldritchKnightClass0AlignDisplayName = "EvangelistEldritchKnightClass0Align.Name";
        private const string ForestEldritchKnightClass0AlignDescription = "EvangelistEldritchKnightClass0Align.Description";
        private const string ForestEldritchKnightClass2Align = "ForestEldritchKnightClass2Align";
        private static readonly string ForestEldritchKnightClass2AlignGuid = "5517115d-6712-4e95-b171-493df4ae20f4";
        private const string ForestEldritchKnightClass4Align = "ForestEldritchKnightClass4Align";
        private static readonly string ForestEldritchKnightClass4AlignGuid = "039c6920-e773-452c-a604-b9136a83382e";
        private const string ForestEldritchKnightClass6Align = "ForestEldritchKnightClass6Align";
        private static readonly string ForestEldritchKnightClass6AlignGuid = "39877df2-dc2f-4b11-a7f7-26e4cd057c5f";
        private const string ForestEldritchKnightClass8Align = "ForestEldritchKnightClass8Align";
        private static readonly string ForestEldritchKnightClass8AlignGuid = "44ee8b37-2d12-4227-bfc0-ab056cef5d62";
        private const string ForestEldritchKnightClass10Align = "ForestEldritchKnightClass10Align";
        private static readonly string ForestEldritchKnightClass10AlignGuid = "024dd962-3d6c-4114-ba19-d3171bdb4ce8";
        private const string ForestEldritchScionClass0Align = "ForestEldritchScionClass0Align";
        private static readonly string ForestEldritchScionClass0AlignGuid = "f3ec869a-0c86-4f6b-8dcc-107f314fa034";
        internal const string ForestEldritchScionClass0AlignDisplayName = "EvangelistEldritchScionClass0Align.Name";
        private const string ForestEldritchScionClass0AlignDescription = "EvangelistEldritchScionClass0Align.Description";
        private const string ForestEldritchScionClass2Align = "ForestEldritchScionClass2Align";
        private static readonly string ForestEldritchScionClass2AlignGuid = "19d74c78-933c-4567-ad4d-afb94ea5ef07";
        private const string ForestEldritchScionClass4Align = "ForestEldritchScionClass4Align";
        private static readonly string ForestEldritchScionClass4AlignGuid = "e15e176d-9ce8-4ac6-8743-5b6bbc47789b";
        private const string ForestEldritchScionClass6Align = "ForestEldritchScionClass6Align";
        private static readonly string ForestEldritchScionClass6AlignGuid = "61e7c702-0f77-4e1a-92c5-731d59a68f38";
        private const string ForestEldritchScionClass8Align = "ForestEldritchScionClass8Align";
        private static readonly string ForestEldritchScionClass8AlignGuid = "e47d9155-f0b9-4769-9b19-2679a9888838";
        private const string ForestEldritchScionClass10Align = "ForestEldritchScionClass10Align";
        private static readonly string ForestEldritchScionClass10AlignGuid = "612c12ba-ffae-4db6-ac7b-10f23e2416f4";
        private const string ForestFighterClass0Align = "ForestFighterClass0Align";
        private static readonly string ForestFighterClass0AlignGuid = "5b34e7a4-5b5f-4cf9-9292-c9150abbd902";
        internal const string ForestFighterClass0AlignDisplayName = "EvangelistFighterClass0Align.Name";
        private const string ForestFighterClass0AlignDescription = "EvangelistFighterClass0Align.Description";
        private const string ForestFighterClass2Align = "ForestFighterClass2Align";
        private static readonly string ForestFighterClass2AlignGuid = "9bde4917-fa9b-4aee-a548-d243ddb7aedc";
        private const string ForestFighterClass4Align = "ForestFighterClass4Align";
        private static readonly string ForestFighterClass4AlignGuid = "fce0dc57-cf81-4aa2-ac69-084191fe30dd";
        private const string ForestFighterClass6Align = "ForestFighterClass6Align";
        private static readonly string ForestFighterClass6AlignGuid = "026805e7-6151-48d3-9085-410dbda7ebde";
        private const string ForestFighterClass8Align = "ForestFighterClass8Align";
        private static readonly string ForestFighterClass8AlignGuid = "6e73d393-5496-4b9a-8571-88c4989177e8";
        private const string ForestFighterClass10Align = "ForestFighterClass10Align";
        private static readonly string ForestFighterClass10AlignGuid = "8bc50f32-28b6-4f94-b550-7d2d662ab4eb";
        private const string ForestHellknightClass0Align = "ForestHellknightClass0Align";
        private static readonly string ForestHellknightClass0AlignGuid = "6cebba60-979c-4517-92df-f57bebc55daf";
        internal const string ForestHellknightClass0AlignDisplayName = "EvangelistHellknightClass0Align.Name";
        private const string ForestHellknightClass0AlignDescription = "EvangelistHellknightClass0Align.Description";
        private const string ForestHellknightClass2Align = "ForestHellknightClass2Align";
        private static readonly string ForestHellknightClass2AlignGuid = "4701538f-8824-41cd-a9e5-1aacaf5053a7";
        private const string ForestHellknightClass4Align = "ForestHellknightClass4Align";
        private static readonly string ForestHellknightClass4AlignGuid = "66ec97aa-d879-4a4c-bb52-12bab03bdc9c";
        private const string ForestHellknightClass6Align = "ForestHellknightClass6Align";
        private static readonly string ForestHellknightClass6AlignGuid = "293bca5b-ee06-43c9-ab1c-59023d00c422";
        private const string ForestHellknightClass8Align = "ForestHellknightClass8Align";
        private static readonly string ForestHellknightClass8AlignGuid = "3610f863-0942-4607-a323-fd76bfbc60e8";
        private const string ForestHellknightClass10Align = "ForestHellknightClass10Align";
        private static readonly string ForestHellknightClass10AlignGuid = "c204871f-d7dd-43a8-a2ef-d21f5ac51638";
        private const string ForestHellknightSigniferClass0Align = "ForestHellknightSigniferClass0Align";
        private static readonly string ForestHellknightSigniferClass0AlignGuid = "e5523fad-377d-4bf4-833c-95ad7ca45a87";
        internal const string ForestHellknightSigniferClass0AlignDisplayName = "EvangelistHellknightSigniferClass0Align.Name";
        private const string ForestHellknightSigniferClass0AlignDescription = "EvangelistHellknightSigniferClass0Align.Description";
        private const string ForestHellknightSigniferClass2Align = "ForestHellknightSigniferClass2Align";
        private static readonly string ForestHellknightSigniferClass2AlignGuid = "d78ffc0f-a58b-41d6-bed1-97df39536ba3";
        private const string ForestHellknightSigniferClass4Align = "ForestHellknightSigniferClass4Align";
        private static readonly string ForestHellknightSigniferClass4AlignGuid = "d679f9e0-f851-4f06-a920-bf1aefb48902";
        private const string ForestHellknightSigniferClass6Align = "ForestHellknightSigniferClass6Align";
        private static readonly string ForestHellknightSigniferClass6AlignGuid = "a63b4efc-073c-40c5-8e6b-4f4569d0b15b";
        private const string ForestHellknightSigniferClass8Align = "ForestHellknightSigniferClass8Align";
        private static readonly string ForestHellknightSigniferClass8AlignGuid = "2d6ac868-b187-4ff9-bd5a-48091db31b4a";
        private const string ForestHellknightSigniferClass10Align = "ForestHellknightSigniferClass10Align";
        private static readonly string ForestHellknightSigniferClass10AlignGuid = "36d7947e-e3d3-41f9-8f75-0714c012a902";
        private const string ForestHunterClass0Align = "ForestHunterClass0Align";
        private static readonly string ForestHunterClass0AlignGuid = "2383a77e-70e9-43ed-8d7a-aec313789d2e";
        internal const string ForestHunterClass0AlignDisplayName = "EvangelistHunterClass0Align.Name";
        private const string ForestHunterClass0AlignDescription = "EvangelistHunterClass0Align.Description";
        private const string ForestHunterClass2Align = "ForestHunterClass2Align";
        private static readonly string ForestHunterClass2AlignGuid = "e97f60e3-6733-4c84-91a4-5ed0c1998773";
        private const string ForestHunterClass4Align = "ForestHunterClass4Align";
        private static readonly string ForestHunterClass4AlignGuid = "24d4b02a-0e61-479f-9fb3-4d1d0b659bec";
        private const string ForestHunterClass6Align = "ForestHunterClass6Align";
        private static readonly string ForestHunterClass6AlignGuid = "ad1528ba-2411-4581-b273-857b3171cfa8";
        private const string ForestHunterClass8Align = "ForestHunterClass8Align";
        private static readonly string ForestHunterClass8AlignGuid = "08967cb1-cb51-4524-9771-9f2d1cb67f8b";
        private const string ForestHunterClass10Align = "ForestHunterClass10Align";
        private static readonly string ForestHunterClass10AlignGuid = "dc01a554-e5f8-45a8-8fe6-0080fb60394f";
        private const string ForestInquisitorClass0Align = "ForestInquisitorClass0Align";
        private static readonly string ForestInquisitorClass0AlignGuid = "0332d9da-9383-46c9-8035-43a6f7cf9c0a";
        internal const string ForestInquisitorClass0AlignDisplayName = "EvangelistInquisitorClass0Align.Name";
        private const string ForestInquisitorClass0AlignDescription = "EvangelistInquisitorClass0Align.Description";
        private const string ForestInquisitorClass2Align = "ForestInquisitorClass2Align";
        private static readonly string ForestInquisitorClass2AlignGuid = "7d261ae5-8854-4ef3-8d78-cd6b280b8774";
        private const string ForestInquisitorClass4Align = "ForestInquisitorClass4Align";
        private static readonly string ForestInquisitorClass4AlignGuid = "b246d27d-2234-472b-a66f-58117c186ea9";
        private const string ForestInquisitorClass6Align = "ForestInquisitorClass6Align";
        private static readonly string ForestInquisitorClass6AlignGuid = "59d62906-0e96-49f2-9808-5b79637e075a";
        private const string ForestInquisitorClass8Align = "ForestInquisitorClass8Align";
        private static readonly string ForestInquisitorClass8AlignGuid = "4dbc575c-3c75-442b-aaef-6972aa0ca692";
        private const string ForestInquisitorClass10Align = "ForestInquisitorClass10Align";
        private static readonly string ForestInquisitorClass10AlignGuid = "be009a1b-6378-445b-8c92-f5ed14d3e041";
        private const string ForestKineticistClass0Align = "ForestKineticistClass0Align";
        private static readonly string ForestKineticistClass0AlignGuid = "6b500f32-0d02-4788-81ad-494bc3fcc4cc";
        internal const string ForestKineticistClass0AlignDisplayName = "EvangelistKineticistClass0Align.Name";
        private const string ForestKineticistClass0AlignDescription = "EvangelistKineticistClass0Align.Description";
        private const string ForestKineticistClass2Align = "ForestKineticistClass2Align";
        private static readonly string ForestKineticistClass2AlignGuid = "4caf19ed-28f6-4a43-88b3-9d05c269b3a8";
        private const string ForestKineticistClass4Align = "ForestKineticistClass4Align";
        private static readonly string ForestKineticistClass4AlignGuid = "bcb2f964-939b-427d-8de5-a2d7d947896e";
        private const string ForestKineticistClass6Align = "ForestKineticistClass6Align";
        private static readonly string ForestKineticistClass6AlignGuid = "cda29320-e380-4b34-8200-410f47063750";
        private const string ForestKineticistClass8Align = "ForestKineticistClass8Align";
        private static readonly string ForestKineticistClass8AlignGuid = "88f28c7d-a900-4442-85dd-58527a0b6155";
        private const string ForestKineticistClass10Align = "ForestKineticistClass10Align";
        private static readonly string ForestKineticistClass10AlignGuid = "cd31c387-6cdc-42c5-b6f5-3d28d9495630";
        private const string ForestLoremasterClass0Align = "ForestLoremasterClass0Align";
        private static readonly string ForestLoremasterClass0AlignGuid = "f8992d2f-6067-4b86-884d-f30eddba79f9";
        internal const string ForestLoremasterClass0AlignDisplayName = "EvangelistLoremasterClass0Align.Name";
        private const string ForestLoremasterClass0AlignDescription = "EvangelistLoremasterClass0Align.Description";
        private const string ForestLoremasterClass2Align = "ForestLoremasterClass2Align";
        private static readonly string ForestLoremasterClass2AlignGuid = "1bd73f12-77b6-4e41-85f5-c39a83c10f33";
        private const string ForestLoremasterClass4Align = "ForestLoremasterClass4Align";
        private static readonly string ForestLoremasterClass4AlignGuid = "50a4b68d-064c-4d72-99a4-a090d369fbee";
        private const string ForestLoremasterClass6Align = "ForestLoremasterClass6Align";
        private static readonly string ForestLoremasterClass6AlignGuid = "24595265-791e-4da6-b228-172a154daec3";
        private const string ForestLoremasterClass8Align = "ForestLoremasterClass8Align";
        private static readonly string ForestLoremasterClass8AlignGuid = "29fa3e99-86bf-43c2-ae2e-bb219a04c209";
        private const string ForestLoremasterClass10Align = "ForestLoremasterClass10Align";
        private static readonly string ForestLoremasterClass10AlignGuid = "6e9f8d5b-fd20-4519-8f87-8791ab1cce22";
        private const string ForestMagusClass0Align = "ForestMagusClass0Align";
        private static readonly string ForestMagusClass0AlignGuid = "f0c67785-735f-4df8-847d-970bb9a1bf1f";
        internal const string ForestMagusClass0AlignDisplayName = "EvangelistMagusClass0Align.Name";
        private const string ForestMagusClass0AlignDescription = "EvangelistMagusClass0Align.Description";
        private const string ForestMagusClass2Align = "ForestMagusClass2Align";
        private static readonly string ForestMagusClass2AlignGuid = "74dd8fbc-0402-4aa4-ae4f-f70d9ea8c6cf";
        private const string ForestMagusClass4Align = "ForestMagusClass4Align";
        private static readonly string ForestMagusClass4AlignGuid = "7f74c153-be15-476a-a9e0-47e361a86c38";
        private const string ForestMagusClass6Align = "ForestMagusClass6Align";
        private static readonly string ForestMagusClass6AlignGuid = "a341ee80-15d7-406c-8a51-9324b4a93960";
        private const string ForestMagusClass8Align = "ForestMagusClass8Align";
        private static readonly string ForestMagusClass8AlignGuid = "a722972b-6cfd-4ba7-ad54-149823526a23";
        private const string ForestMagusClass10Align = "ForestMagusClass10Align";
        private static readonly string ForestMagusClass10AlignGuid = "e4d4330b-01c3-4088-af9d-df85556eab2b";
        private const string ForestMonkClass0Align = "ForestMonkClass0Align";
        private static readonly string ForestMonkClass0AlignGuid = "a30307aa-36b9-4e73-8d50-fdcf876a0f22";
        internal const string ForestMonkClass0AlignDisplayName = "EvangelistMonkClass0Align.Name";
        private const string ForestMonkClass0AlignDescription = "EvangelistMonkClass0Align.Description";
        private const string ForestMonkClass2Align = "ForestMonkClass2Align";
        private static readonly string ForestMonkClass2AlignGuid = "7197e8fb-017c-4f51-9c35-2935242161b6";
        private const string ForestMonkClass4Align = "ForestMonkClass4Align";
        private static readonly string ForestMonkClass4AlignGuid = "4413e6a2-d1a2-46c5-94d9-5b0e24fd03f7";
        private const string ForestMonkClass6Align = "ForestMonkClass6Align";
        private static readonly string ForestMonkClass6AlignGuid = "d3cc9b09-7a04-4af9-aa62-4f21cbc85b4b";
        private const string ForestMonkClass8Align = "ForestMonkClass8Align";
        private static readonly string ForestMonkClass8AlignGuid = "7810973e-eb96-42e3-b2b5-4f7fad6e4276";
        private const string ForestMonkClass10Align = "ForestMonkClass10Align";
        private static readonly string ForestMonkClass10AlignGuid = "3c6e34b0-4396-46ce-ac1d-bf3020f72f87";
        private const string ForestMysticTheurgeClass0Align = "ForestMysticTheurgeClass0Align";
        private static readonly string ForestMysticTheurgeClass0AlignGuid = "3b78a0aa-c539-4b77-bbf5-bc995a41e393";
        internal const string ForestMysticTheurgeClass0AlignDisplayName = "EvangelistMysticTheurgeClass0Align.Name";
        private const string ForestMysticTheurgeClass0AlignDescription = "EvangelistMysticTheurgeClass0Align.Description";
        private const string ForestMysticTheurgeClass2Align = "ForestMysticTheurgeClass2Align";
        private static readonly string ForestMysticTheurgeClass2AlignGuid = "979fbc9e-3e4c-49e3-9ee5-c7eb92e5e07e";
        private const string ForestMysticTheurgeClass4Align = "ForestMysticTheurgeClass4Align";
        private static readonly string ForestMysticTheurgeClass4AlignGuid = "bb54385a-eebc-4ba9-b94b-68010b46a09f";
        private const string ForestMysticTheurgeClass6Align = "ForestMysticTheurgeClass6Align";
        private static readonly string ForestMysticTheurgeClass6AlignGuid = "7fe2d18a-00e7-4bfb-a274-e02ae008b9c3";
        private const string ForestMysticTheurgeClass8Align = "ForestMysticTheurgeClass8Align";
        private static readonly string ForestMysticTheurgeClass8AlignGuid = "92f9f67d-019e-414a-a8fc-18ead61ee4db";
        private const string ForestMysticTheurgeClass10Align = "ForestMysticTheurgeClass10Align";
        private static readonly string ForestMysticTheurgeClass10AlignGuid = "e65529c9-7a92-48b4-9cd0-624df4f62a6c";
        private const string ForestOracleClass0Align = "ForestOracleClass0Align";
        private static readonly string ForestOracleClass0AlignGuid = "73a8fc0b-3e0a-44cb-99b9-3b1789992ed3";
        internal const string ForestOracleClass0AlignDisplayName = "EvangelistOracleClass0Align.Name";
        private const string ForestOracleClass0AlignDescription = "EvangelistOracleClass0Align.Description";
        private const string ForestOracleClass2Align = "ForestOracleClass2Align";
        private static readonly string ForestOracleClass2AlignGuid = "4a2e73bc-402e-4324-9a63-f54cbaa2b6b6";
        private const string ForestOracleClass4Align = "ForestOracleClass4Align";
        private static readonly string ForestOracleClass4AlignGuid = "16ea0407-7fad-4397-b167-327547feda55";
        private const string ForestOracleClass6Align = "ForestOracleClass6Align";
        private static readonly string ForestOracleClass6AlignGuid = "5ac7673e-c27a-4820-b2c5-e4b47a563d7d";
        private const string ForestOracleClass8Align = "ForestOracleClass8Align";
        private static readonly string ForestOracleClass8AlignGuid = "3942c2a2-cdc6-4e93-9bf4-c3a30737ecea";
        private const string ForestOracleClass10Align = "ForestOracleClass10Align";
        private static readonly string ForestOracleClass10AlignGuid = "0b9c2d9d-3761-4b49-80e3-75ea4c2d928c";
        private const string ForestPaladinClass0Align = "ForestPaladinClass0Align";
        private static readonly string ForestPaladinClass0AlignGuid = "27eb7d59-3dd5-4145-ac39-1938a7771f20";
        internal const string ForestPaladinClass0AlignDisplayName = "EvangelistPaladinClass0Align.Name";
        private const string ForestPaladinClass0AlignDescription = "EvangelistPaladinClass0Align.Description";
        private const string ForestPaladinClass2Align = "ForestPaladinClass2Align";
        private static readonly string ForestPaladinClass2AlignGuid = "ad1c53b2-49e3-4933-8a0f-599778e9bea2";
        private const string ForestPaladinClass4Align = "ForestPaladinClass4Align";
        private static readonly string ForestPaladinClass4AlignGuid = "a8c7b9fc-b99c-44ec-85b8-04e45fe916df";
        private const string ForestPaladinClass6Align = "ForestPaladinClass6Align";
        private static readonly string ForestPaladinClass6AlignGuid = "1c90e89f-6ff9-44e5-a71f-24f8ea86222b";
        private const string ForestPaladinClass8Align = "ForestPaladinClass8Align";
        private static readonly string ForestPaladinClass8AlignGuid = "9325d0df-eca6-453d-a922-5d189f594497";
        private const string ForestPaladinClass10Align = "ForestPaladinClass10Align";
        private static readonly string ForestPaladinClass10AlignGuid = "7d06ccce-7716-44ea-a351-3cb90ac5dff0";
        private const string ForestRangerClass0Align = "ForestRangerClass0Align";
        private static readonly string ForestRangerClass0AlignGuid = "1ceb665c-a3a6-44ba-9829-a7dcefaa06a8";
        internal const string ForestRangerClass0AlignDisplayName = "EvangelistRangerClass0Align.Name";
        private const string ForestRangerClass0AlignDescription = "EvangelistRangerClass0Align.Description";
        private const string ForestRangerClass2Align = "ForestRangerClass2Align";
        private static readonly string ForestRangerClass2AlignGuid = "ea3ebc48-02e8-4d19-a8f7-13e7eadc9a5e";
        private const string ForestRangerClass4Align = "ForestRangerClass4Align";
        private static readonly string ForestRangerClass4AlignGuid = "e42ecf00-7a2e-43be-9eef-fbd3f8cd64d7";
        private const string ForestRangerClass6Align = "ForestRangerClass6Align";
        private static readonly string ForestRangerClass6AlignGuid = "6c6a53d0-2ccc-46f7-9b8b-57d9f52e0446";
        private const string ForestRangerClass8Align = "ForestRangerClass8Align";
        private static readonly string ForestRangerClass8AlignGuid = "590ee48d-3ba0-4837-bf64-ec454add5449";
        private const string ForestRangerClass10Align = "ForestRangerClass10Align";
        private static readonly string ForestRangerClass10AlignGuid = "676f2907-b498-49b0-a170-d0a762075f82";
        private const string ForestRogueClass0Align = "ForestRogueClass0Align";
        private static readonly string ForestRogueClass0AlignGuid = "67a884e0-cd0f-4bf8-8b71-963965c3b46a";
        internal const string ForestRogueClass0AlignDisplayName = "EvangelistRogueClass0Align.Name";
        private const string ForestRogueClass0AlignDescription = "EvangelistRogueClass0Align.Description";
        private const string ForestRogueClass2Align = "ForestRogueClass2Align";
        private static readonly string ForestRogueClass2AlignGuid = "7efce79b-0471-421b-8db5-6d3f068449c5";
        private const string ForestRogueClass4Align = "ForestRogueClass4Align";
        private static readonly string ForestRogueClass4AlignGuid = "86b3056c-fcbb-4c81-93a0-27e9c991c878";
        private const string ForestRogueClass6Align = "ForestRogueClass6Align";
        private static readonly string ForestRogueClass6AlignGuid = "6dd848b0-1404-47d2-9314-733380a26510";
        private const string ForestRogueClass8Align = "ForestRogueClass8Align";
        private static readonly string ForestRogueClass8AlignGuid = "38eb2ada-0f49-470c-96f4-0391a361fbb8";
        private const string ForestRogueClass10Align = "ForestRogueClass10Align";
        private static readonly string ForestRogueClass10AlignGuid = "f0f92440-a5f8-43b4-88d9-9c574f209f06";
        private const string ForestShamanClass0Align = "ForestShamanClass0Align";
        private static readonly string ForestShamanClass0AlignGuid = "4ecf90d6-0861-43f1-b6e8-9a0c78726844";
        internal const string ForestShamanClass0AlignDisplayName = "EvangelistShamanClass0Align.Name";
        private const string ForestShamanClass0AlignDescription = "EvangelistShamanClass0Align.Description";
        private const string ForestShamanClass2Align = "ForestShamanClass2Align";
        private static readonly string ForestShamanClass2AlignGuid = "54063dc5-88e8-441c-813e-d1468d9d5188";
        private const string ForestShamanClass4Align = "ForestShamanClass4Align";
        private static readonly string ForestShamanClass4AlignGuid = "b6ffe683-825d-41d6-afd7-bffea5ff0e89";
        private const string ForestShamanClass6Align = "ForestShamanClass6Align";
        private static readonly string ForestShamanClass6AlignGuid = "5eb04e71-9c13-4755-84ef-19b3bd900a15";
        private const string ForestShamanClass8Align = "ForestShamanClass8Align";
        private static readonly string ForestShamanClass8AlignGuid = "242d5d88-0c44-4f07-9444-1c4b752bd802";
        private const string ForestShamanClass10Align = "ForestShamanClass10Align";
        private static readonly string ForestShamanClass10AlignGuid = "5e05a3bb-b0e1-4845-a578-718ced649e4f";
        private const string ForestShifterClass0Align = "ForestShifterClass0Align";
        private static readonly string ForestShifterClass0AlignGuid = "cc5da49c-4902-429f-b4dc-bb17147b92ea";
        internal const string ForestShifterClass0AlignDisplayName = "EvangelistShifterClass0Align.Name";
        private const string ForestShifterClass0AlignDescription = "EvangelistShifterClass0Align.Description";
        private const string ForestShifterClass2Align = "ForestShifterClass2Align";
        private static readonly string ForestShifterClass2AlignGuid = "a4288b52-d949-470a-a203-c98d76370928";
        private const string ForestShifterClass4Align = "ForestShifterClass4Align";
        private static readonly string ForestShifterClass4AlignGuid = "a7081629-13b6-49c4-876b-97439c30f33b";
        private const string ForestShifterClass6Align = "ForestShifterClass6Align";
        private static readonly string ForestShifterClass6AlignGuid = "58e3bb25-bb18-4028-83d4-8207d7932821";
        private const string ForestShifterClass8Align = "ForestShifterClass8Align";
        private static readonly string ForestShifterClass8AlignGuid = "a8eba56c-5517-4dc2-aeb5-af1f58cc3c0f";
        private const string ForestShifterClass10Align = "ForestShifterClass10Align";
        private static readonly string ForestShifterClass10AlignGuid = "07b5f2fb-e007-4245-8667-33142a15c4dc";
        private const string ForestSkaldClass0Align = "ForestSkaldClass0Align";
        private static readonly string ForestSkaldClass0AlignGuid = "af9e0473-b227-41da-9ef8-1c279fedaec5";
        internal const string ForestSkaldClass0AlignDisplayName = "EvangelistSkaldClass0Align.Name";
        private const string ForestSkaldClass0AlignDescription = "EvangelistSkaldClass0Align.Description";
        private const string ForestSkaldClass2Align = "ForestSkaldClass2Align";
        private static readonly string ForestSkaldClass2AlignGuid = "d406aa7a-7bb6-457b-8cbc-9bf085f3329e";
        private const string ForestSkaldClass4Align = "ForestSkaldClass4Align";
        private static readonly string ForestSkaldClass4AlignGuid = "743b8443-fe00-4b7b-93a2-8493c93c499d";
        private const string ForestSkaldClass6Align = "ForestSkaldClass6Align";
        private static readonly string ForestSkaldClass6AlignGuid = "cbe563b3-8730-4fe3-b9f6-79d26b1e2a74";
        private const string ForestSkaldClass8Align = "ForestSkaldClass8Align";
        private static readonly string ForestSkaldClass8AlignGuid = "2392542a-3a30-428d-972a-db6ae7abf0d8";
        private const string ForestSkaldClass10Align = "ForestSkaldClass10Align";
        private static readonly string ForestSkaldClass10AlignGuid = "7fd4ac59-6185-442e-b293-806bddb30ffd";
        private const string ForestSlayerClass0Align = "ForestSlayerClass0Align";
        private static readonly string ForestSlayerClass0AlignGuid = "67b32a04-11b0-4ac4-8a80-a1a86391cc63";
        internal const string ForestSlayerClass0AlignDisplayName = "EvangelistSlayerClass0Align.Name";
        private const string ForestSlayerClass0AlignDescription = "EvangelistSlayerClass0Align.Description";
        private const string ForestSlayerClass2Align = "ForestSlayerClass2Align";
        private static readonly string ForestSlayerClass2AlignGuid = "11938c6c-bdc4-4356-85ca-9189132770e5";
        private const string ForestSlayerClass4Align = "ForestSlayerClass4Align";
        private static readonly string ForestSlayerClass4AlignGuid = "105d231a-aca0-4b86-bf2e-f575edaa2152";
        private const string ForestSlayerClass6Align = "ForestSlayerClass6Align";
        private static readonly string ForestSlayerClass6AlignGuid = "e98183f4-7a28-430b-bc18-6928c54b2d2f";
        private const string ForestSlayerClass8Align = "ForestSlayerClass8Align";
        private static readonly string ForestSlayerClass8AlignGuid = "4f34ee6a-a92e-4d70-86f9-622abe1279f3";
        private const string ForestSlayerClass10Align = "ForestSlayerClass10Align";
        private static readonly string ForestSlayerClass10AlignGuid = "d23ad380-96af-4f09-97fb-53fef1112fe3";
        private const string ForestSorcererClass0Align = "ForestSorcererClass0Align";
        private static readonly string ForestSorcererClass0AlignGuid = "917b85c7-7fe9-4534-924b-14889b0e7b2a";
        internal const string ForestSorcererClass0AlignDisplayName = "EvangelistSorcererClass0Align.Name";
        private const string ForestSorcererClass0AlignDescription = "EvangelistSorcererClass0Align.Description";
        private const string ForestSorcererClass2Align = "ForestSorcererClass2Align";
        private static readonly string ForestSorcererClass2AlignGuid = "31029d09-87f5-4028-8b82-460849f813fe";
        private const string ForestSorcererClass4Align = "ForestSorcererClass4Align";
        private static readonly string ForestSorcererClass4AlignGuid = "662b4896-f1c0-4c9c-a29b-c3923ece88c4";
        private const string ForestSorcererClass6Align = "ForestSorcererClass6Align";
        private static readonly string ForestSorcererClass6AlignGuid = "187b1ac6-b3d1-482e-be60-01d0a491831f";
        private const string ForestSorcererClass8Align = "ForestSorcererClass8Align";
        private static readonly string ForestSorcererClass8AlignGuid = "ccb4592e-7876-482c-b894-5b111600061f";
        private const string ForestSorcererClass10Align = "ForestSorcererClass10Align";
        private static readonly string ForestSorcererClass10AlignGuid = "96bfa019-2bbe-40de-b445-3227477a05ce";
        private const string ForestStalwartDefenderClass0Align = "ForestStalwartDefenderClass0Align";
        private static readonly string ForestStalwartDefenderClass0AlignGuid = "0223c9a6-6422-46df-a791-7ed571fafdef";
        internal const string ForestStalwartDefenderClass0AlignDisplayName = "EvangelistStalwartDefenderClass0Align.Name";
        private const string ForestStalwartDefenderClass0AlignDescription = "EvangelistStalwartDefenderClass0Align.Description";
        private const string ForestStalwartDefenderClass2Align = "ForestStalwartDefenderClass2Align";
        private static readonly string ForestStalwartDefenderClass2AlignGuid = "1d0f5fb3-7a24-4770-a775-dc32c4a79f6d";
        private const string ForestStalwartDefenderClass4Align = "ForestStalwartDefenderClass4Align";
        private static readonly string ForestStalwartDefenderClass4AlignGuid = "4e5198a4-5112-467a-9de5-e6195aac9d7a";
        private const string ForestStalwartDefenderClass6Align = "ForestStalwartDefenderClass6Align";
        private static readonly string ForestStalwartDefenderClass6AlignGuid = "fa5692f2-9bb9-4043-9658-4dc3524eaf3b";
        private const string ForestStalwartDefenderClass8Align = "ForestStalwartDefenderClass8Align";
        private static readonly string ForestStalwartDefenderClass8AlignGuid = "48bc337a-a300-4b9c-8804-d32e2a3cce15";
        private const string ForestStalwartDefenderClass10Align = "ForestStalwartDefenderClass10Align";
        private static readonly string ForestStalwartDefenderClass10AlignGuid = "2ec42800-06a7-4212-8e64-93d0a50c0df4";
        private const string ForestStudentOfWarClass0Align = "ForestStudentOfWarClass0Align";
        private static readonly string ForestStudentOfWarClass0AlignGuid = "622707bd-9fd6-4244-b88b-d6ebca664453";
        internal const string ForestStudentOfWarClass0AlignDisplayName = "EvangelistStudentOfWarClass0Align.Name";
        private const string ForestStudentOfWarClass0AlignDescription = "EvangelistStudentOfWarClass0Align.Description";
        private const string ForestStudentOfWarClass2Align = "ForestStudentOfWarClass2Align";
        private static readonly string ForestStudentOfWarClass2AlignGuid = "8a5e3dc3-c8a1-4eaf-9a71-83bc7f4c1205";
        private const string ForestStudentOfWarClass4Align = "ForestStudentOfWarClass4Align";
        private static readonly string ForestStudentOfWarClass4AlignGuid = "dace5fc7-8228-4ac4-9836-244e67f64573";
        private const string ForestStudentOfWarClass6Align = "ForestStudentOfWarClass6Align";
        private static readonly string ForestStudentOfWarClass6AlignGuid = "01cdc9a1-8c73-4da2-b6ac-e53df9e01891";
        private const string ForestStudentOfWarClass8Align = "ForestStudentOfWarClass8Align";
        private static readonly string ForestStudentOfWarClass8AlignGuid = "b43dfda4-f1d5-4f0b-9f62-392b6d331e02";
        private const string ForestStudentOfWarClass10Align = "ForestStudentOfWarClass10Align";
        private static readonly string ForestStudentOfWarClass10AlignGuid = "714937ab-3b70-4c23-a2a5-1320495ce68c";
        private const string ForestSwordlordClass0Align = "ForestSwordlordClass0Align";
        private static readonly string ForestSwordlordClass0AlignGuid = "a4fb2853-eb33-4cc5-a969-aa36a676d3a4";
        internal const string ForestSwordlordClass0AlignDisplayName = "EvangelistSwordlordClass0Align.Name";
        private const string ForestSwordlordClass0AlignDescription = "EvangelistSwordlordClass0Align.Description";
        private const string ForestSwordlordClass2Align = "ForestSwordlordClass2Align";
        private static readonly string ForestSwordlordClass2AlignGuid = "c3866a26-165b-4139-a080-077fea37dd75";
        private const string ForestSwordlordClass4Align = "ForestSwordlordClass4Align";
        private static readonly string ForestSwordlordClass4AlignGuid = "f010bd0a-79f5-4212-aa99-733467f9b74e";
        private const string ForestSwordlordClass6Align = "ForestSwordlordClass6Align";
        private static readonly string ForestSwordlordClass6AlignGuid = "6d0571e0-67d9-48d4-bd59-4a97e6282c03";
        private const string ForestSwordlordClass8Align = "ForestSwordlordClass8Align";
        private static readonly string ForestSwordlordClass8AlignGuid = "eec3777f-75fe-422b-b267-f789db686ada";
        private const string ForestSwordlordClass10Align = "ForestSwordlordClass10Align";
        private static readonly string ForestSwordlordClass10AlignGuid = "7479ddf5-1174-4aa3-b0a8-bff5dd14cf79";
        private const string ForestWarpriestClass0Align = "ForestWarpriestClass0Align";
        private static readonly string ForestWarpriestClass0AlignGuid = "8283940b-d916-44cc-9da8-98237155da5e";
        internal const string ForestWarpriestClass0AlignDisplayName = "EvangelistWarpriestClass0Align.Name";
        private const string ForestWarpriestClass0AlignDescription = "EvangelistWarpriestClass0Align.Description";
        private const string ForestWarpriestClass2Align = "ForestWarpriestClass2Align";
        private static readonly string ForestWarpriestClass2AlignGuid = "4e6eedb5-d93d-42ee-9a4d-d23dd4f95f5b";
        private const string ForestWarpriestClass4Align = "ForestWarpriestClass4Align";
        private static readonly string ForestWarpriestClass4AlignGuid = "c215cc43-f57d-4901-8e4e-5e570ec1521c";
        private const string ForestWarpriestClass6Align = "ForestWarpriestClass6Align";
        private static readonly string ForestWarpriestClass6AlignGuid = "02c16bf6-d8be-4929-8dc9-562723201357";
        private const string ForestWarpriestClass8Align = "ForestWarpriestClass8Align";
        private static readonly string ForestWarpriestClass8AlignGuid = "81f08027-2f33-46cf-8c53-93ed610a00ad";
        private const string ForestWarpriestClass10Align = "ForestWarpriestClass10Align";
        private static readonly string ForestWarpriestClass10AlignGuid = "1ee11228-cc44-4310-b765-27f317d1c0b2";
        private const string ForestWinterWitchClass0Align = "ForestWinterWitchClass0Align";
        private static readonly string ForestWinterWitchClass0AlignGuid = "ba3aedc5-e0cf-446c-bf5b-c631f4b2495a";
        internal const string ForestWinterWitchClass0AlignDisplayName = "EvangelistWinterWitchClass0Align.Name";
        private const string ForestWinterWitchClass0AlignDescription = "EvangelistWinterWitchClass0Align.Description";
        private const string ForestWinterWitchClass2Align = "ForestWinterWitchClass2Align";
        private static readonly string ForestWinterWitchClass2AlignGuid = "d5ee260f-55cc-4a2b-a612-8d34d78bd4b8";
        private const string ForestWinterWitchClass4Align = "ForestWinterWitchClass4Align";
        private static readonly string ForestWinterWitchClass4AlignGuid = "182dfaf0-c185-4f31-a63a-069fae2395cd";
        private const string ForestWinterWitchClass6Align = "ForestWinterWitchClass6Align";
        private static readonly string ForestWinterWitchClass6AlignGuid = "e6479f19-cec0-41b6-8a08-a23114b4720f";
        private const string ForestWinterWitchClass8Align = "ForestWinterWitchClass8Align";
        private static readonly string ForestWinterWitchClass8AlignGuid = "70def093-22de-49d3-af3d-9cb5b988ff41";
        private const string ForestWinterWitchClass10Align = "ForestWinterWitchClass10Align";
        private static readonly string ForestWinterWitchClass10AlignGuid = "b382b763-5a2e-4510-a66f-536b6452843f";
        private const string ForestWitchClass0Align = "ForestWitchClass0Align";
        private static readonly string ForestWitchClass0AlignGuid = "444611ae-095a-457e-aad1-23771d19e20b";
        internal const string ForestWitchClass0AlignDisplayName = "EvangelistWitchClass0Align.Name";
        private const string ForestWitchClass0AlignDescription = "EvangelistWitchClass0Align.Description";
        private const string ForestWitchClass2Align = "ForestWitchClass2Align";
        private static readonly string ForestWitchClass2AlignGuid = "00d7e493-6c1d-4104-b7a6-0a1f5800944f";
        private const string ForestWitchClass4Align = "ForestWitchClass4Align";
        private static readonly string ForestWitchClass4AlignGuid = "08978978-9bbf-4dfa-943e-0ec1af1c9cf8";
        private const string ForestWitchClass6Align = "ForestWitchClass6Align";
        private static readonly string ForestWitchClass6AlignGuid = "c30d29ae-aa78-4cda-a0b6-7c3c08e58954";
        private const string ForestWitchClass8Align = "ForestWitchClass8Align";
        private static readonly string ForestWitchClass8AlignGuid = "b69ac157-251d-487a-a608-658537dee33d";
        private const string ForestWitchClass10Align = "ForestWitchClass10Align";
        private static readonly string ForestWitchClass10AlignGuid = "cfd5bcec-0ebc-47e4-8b16-c18f799218b7";
        private const string ForestWizardClass0Align = "ForestWizardClass0Align";
        private static readonly string ForestWizardClass0AlignGuid = "4588fe9a-edd4-42eb-82df-88ac0d8db995";
        internal const string ForestWizardClass0AlignDisplayName = "EvangelistWizardClass0Align.Name";
        private const string ForestWizardClass0AlignDescription = "EvangelistWizardClass0Align.Description";
        private const string ForestWizardClass2Align = "ForestWizardClass2Align";
        private static readonly string ForestWizardClass2AlignGuid = "c25b2c5c-7d44-43ae-9279-8e904c3d0030";
        private const string ForestWizardClass4Align = "ForestWizardClass4Align";
        private static readonly string ForestWizardClass4AlignGuid = "456ac88d-4ed2-4406-8a79-7c7f86fb76ec";
        private const string ForestWizardClass6Align = "ForestWizardClass6Align";
        private static readonly string ForestWizardClass6AlignGuid = "40ea72fa-38a3-49c6-b983-e2716e997f45";
        private const string ForestWizardClass8Align = "ForestWizardClass8Align";
        private static readonly string ForestWizardClass8AlignGuid = "afe4eedc-74f4-4af7-ae65-1fdd80e5b3d0";
        private const string ForestWizardClass10Align = "ForestWizardClass10Align";
        private static readonly string ForestWizardClass10AlignGuid = "8aeaf5db-8ccb-4a9d-af13-76a6f30e7f45";
        private const string ForestGunslingerClass0Align = "ForestGunslingerClass0Align";
        private static readonly string ForestGunslingerClass0AlignGuid = "5b57c1bb-1b98-4516-97a0-092bf7db39ad";
        internal const string ForestGunslingerClass0AlignDisplayName = "EvangelistGunslingerClass0Align.Name";
        private const string ForestGunslingerClass0AlignDescription = "EvangelistGunslingerClass0Align.Description";
        private const string ForestGunslingerClass2Align = "ForestGunslingerClass2Align";
        private static readonly string ForestGunslingerClass2AlignGuid = "da1d6f19-0ad1-4d96-8f47-e2d69d7240e7";
        private const string ForestGunslingerClass4Align = "ForestGunslingerClass4Align";
        private static readonly string ForestGunslingerClass4AlignGuid = "714eb2b1-14f1-4843-8613-8f11167e3779";
        private const string ForestGunslingerClass6Align = "ForestGunslingerClass6Align";
        private static readonly string ForestGunslingerClass6AlignGuid = "728d23c3-8169-4849-86db-f1b930c0e24b";
        private const string ForestGunslingerClass8Align = "ForestGunslingerClass8Align";
        private static readonly string ForestGunslingerClass8AlignGuid = "b9392681-db92-44e8-8c60-cb452214a0d7";
        private const string ForestGunslingerClass10Align = "ForestGunslingerClass10Align";
        private static readonly string ForestGunslingerClass10AlignGuid = "86e0b835-3989-4ecc-b2b4-fadfbd45d355";
        private const string ForestAgentoftheGraveClass0Align = "ForestAgentoftheGraveClass0Align";
        private static readonly string ForestAgentoftheGraveClass0AlignGuid = "f8403d88-fef7-4fae-87f4-2d2f12149569";
        internal const string ForestAgentoftheGraveClass0AlignDisplayName = "EvangelistAgentoftheGraveClass0Align.Name";
        private const string ForestAgentoftheGraveClass0AlignDescription = "EvangelistAgentoftheGraveClass0Align.Description";
        private const string ForestAgentoftheGraveClass2Align = "ForestAgentoftheGraveClass2Align";
        private static readonly string ForestAgentoftheGraveClass2AlignGuid = "74b13997-be93-4091-868f-43d8989bf74a";
        private const string ForestAgentoftheGraveClass4Align = "ForestAgentoftheGraveClass4Align";
        private static readonly string ForestAgentoftheGraveClass4AlignGuid = "09835219-5941-48bb-9776-f534b7857b48";
        private const string ForestAgentoftheGraveClass6Align = "ForestAgentoftheGraveClass6Align";
        private static readonly string ForestAgentoftheGraveClass6AlignGuid = "7c41cc94-83d3-483a-94c5-f0f21c89ac0f";
        private const string ForestAgentoftheGraveClass8Align = "ForestAgentoftheGraveClass8Align";
        private static readonly string ForestAgentoftheGraveClass8AlignGuid = "b909b864-87fc-4aa0-a950-d81806e68506";
        private const string ForestAgentoftheGraveClass10Align = "ForestAgentoftheGraveClass10Align";
        private static readonly string ForestAgentoftheGraveClass10AlignGuid = "2753396b-99ce-401a-9540-685aa18a7956";
        private const string ForestAnchoriteofDawnClass0Align = "ForestAnchoriteofDawnClass0Align";
        private static readonly string ForestAnchoriteofDawnClass0AlignGuid = "1595a7c2-aac7-40c9-8bbd-cd8c55016c61";
        internal const string ForestAnchoriteofDawnClass0AlignDisplayName = "EvangelistAnchoriteofDawnClass0Align.Name";
        private const string ForestAnchoriteofDawnClass0AlignDescription = "EvangelistAnchoriteofDawnClass0Align.Description";
        private const string ForestAnchoriteofDawnClass2Align = "ForestAnchoriteofDawnClass2Align";
        private static readonly string ForestAnchoriteofDawnClass2AlignGuid = "9c3fcf1a-9b4f-429a-acdb-e12755c6121d";
        private const string ForestAnchoriteofDawnClass4Align = "ForestAnchoriteofDawnClass4Align";
        private static readonly string ForestAnchoriteofDawnClass4AlignGuid = "529a2fa0-6e30-4599-b825-8cb666bfe233";
        private const string ForestAnchoriteofDawnClass6Align = "ForestAnchoriteofDawnClass6Align";
        private static readonly string ForestAnchoriteofDawnClass6AlignGuid = "71e592a6-76a1-4b40-a826-39bf60ba67ba";
        private const string ForestAnchoriteofDawnClass8Align = "ForestAnchoriteofDawnClass8Align";
        private static readonly string ForestAnchoriteofDawnClass8AlignGuid = "2cfab91b-db6d-4863-a78d-8e752287f5f8";
        private const string ForestAnchoriteofDawnClass10Align = "ForestAnchoriteofDawnClass10Align";
        private static readonly string ForestAnchoriteofDawnClass10AlignGuid = "4c26fe61-4128-402b-b53d-473442af821b";
        private const string ForestArcaneAcherClass0Align = "ForestArcaneAcherClass0Align";
        private static readonly string ForestArcaneAcherClass0AlignGuid = "ccad2904-d08c-43c6-9c67-e28bb0815de9";
        internal const string ForestArcaneAcherClass0AlignDisplayName = "EvangelistArcaneAcherClass0Align.Name";
        private const string ForestArcaneAcherClass0AlignDescription = "EvangelistArcaneAcherClass0Align.Description";
        private const string ForestArcaneAcherClass2Align = "ForestArcaneAcherClass2Align";
        private static readonly string ForestArcaneAcherClass2AlignGuid = "5b42bd82-58b8-4291-8522-2b882154b09e";
        private const string ForestArcaneAcherClass4Align = "ForestArcaneAcherClass4Align";
        private static readonly string ForestArcaneAcherClass4AlignGuid = "ca6e5cba-c79e-449b-a60d-59b83502c712";
        private const string ForestArcaneAcherClass6Align = "ForestArcaneAcherClass6Align";
        private static readonly string ForestArcaneAcherClass6AlignGuid = "940e5c46-dc80-4d18-a151-7722eb42649b";
        private const string ForestArcaneAcherClass8Align = "ForestArcaneAcherClass8Align";
        private static readonly string ForestArcaneAcherClass8AlignGuid = "5679e00e-1b3b-4b91-a954-549a32aa73dc";
        private const string ForestArcaneAcherClass10Align = "ForestArcaneAcherClass10Align";
        private static readonly string ForestArcaneAcherClass10AlignGuid = "f445eb32-5812-4113-b764-da1361cd7bfe";
        private const string ForestAsavirClass0Align = "ForestAsavirClass0Align";
        private static readonly string ForestAsavirClass0AlignGuid = "07254950-308b-48d0-acaf-2936d85631bf";
        internal const string ForestAsavirClass0AlignDisplayName = "EvangelistAsavirClass0Align.Name";
        private const string ForestAsavirClass0AlignDescription = "EvangelistAsavirClass0Align.Description";
        private const string ForestAsavirClass2Align = "ForestAsavirClass2Align";
        private static readonly string ForestAsavirClass2AlignGuid = "a5396376-4d29-4dcf-a4e5-c115ec95ec0d";
        private const string ForestAsavirClass4Align = "ForestAsavirClass4Align";
        private static readonly string ForestAsavirClass4AlignGuid = "3c33e6ff-46cd-46fa-aed8-4ec20bb22897";
        private const string ForestAsavirClass6Align = "ForestAsavirClass6Align";
        private static readonly string ForestAsavirClass6AlignGuid = "894d62a3-198d-4145-9b25-3b3e7227b6f9";
        private const string ForestAsavirClass8Align = "ForestAsavirClass8Align";
        private static readonly string ForestAsavirClass8AlignGuid = "7f2e5270-1b17-4c76-b57d-e5dfdb745a5b";
        private const string ForestAsavirClass10Align = "ForestAsavirClass10Align";
        private static readonly string ForestAsavirClass10AlignGuid = "ef3fa7a6-20f7-49e0-a00b-d960ef010c77";
        private const string ForestChevalierClass0Align = "ForestChevalierClass0Align";
        private static readonly string ForestChevalierClass0AlignGuid = "f79b4e56-a27a-4cfa-81f5-186b2e97d145";
        internal const string ForestChevalierClass0AlignDisplayName = "EvangelistChevalierClass0Align.Name";
        private const string ForestChevalierClass0AlignDescription = "EvangelistChevalierClass0Align.Description";
        private const string ForestChevalierClass2Align = "ForestChevalierClass2Align";
        private static readonly string ForestChevalierClass2AlignGuid = "0121458c-b9c6-42f8-9f95-b20fa421829b";
        private const string ForestChevalierClass4Align = "ForestChevalierClass4Align";
        private static readonly string ForestChevalierClass4AlignGuid = "3968a4eb-cd94-4f81-9145-941cd102ae17";
        private const string ForestChevalierClass6Align = "ForestChevalierClass6Align";
        private static readonly string ForestChevalierClass6AlignGuid = "033e3d14-fd50-4b87-b1bd-2625ef143f70";
        private const string ForestChevalierClass8Align = "ForestChevalierClass8Align";
        private static readonly string ForestChevalierClass8AlignGuid = "508381ec-11ba-48a3-9d08-efebb49b950d";
        private const string ForestChevalierClass10Align = "ForestChevalierClass10Align";
        private static readonly string ForestChevalierClass10AlignGuid = "3cad5b92-b93a-4e69-9a2b-8f44773c8454";
        private const string ForestCrimsonTemplarClass0Align = "ForestCrimsonTemplarClass0Align";
        private static readonly string ForestCrimsonTemplarClass0AlignGuid = "1cdcbc1c-c62f-4273-9927-6b0ff62729ad";
        internal const string ForestCrimsonTemplarClass0AlignDisplayName = "EvangelistCrimsonTemplarClass0Align.Name";
        private const string ForestCrimsonTemplarClass0AlignDescription = "EvangelistCrimsonTemplarClass0Align.Description";
        private const string ForestCrimsonTemplarClass2Align = "ForestCrimsonTemplarClass2Align";
        private static readonly string ForestCrimsonTemplarClass2AlignGuid = "284c3983-953a-4b04-9788-4364cd1ee61f";
        private const string ForestCrimsonTemplarClass4Align = "ForestCrimsonTemplarClass4Align";
        private static readonly string ForestCrimsonTemplarClass4AlignGuid = "b8ed2583-94fb-440d-a4eb-d15821a67de5";
        private const string ForestCrimsonTemplarClass6Align = "ForestCrimsonTemplarClass6Align";
        private static readonly string ForestCrimsonTemplarClass6AlignGuid = "fa58b43b-b7d8-4de6-96c0-4abe1a209fff";
        private const string ForestCrimsonTemplarClass8Align = "ForestCrimsonTemplarClass8Align";
        private static readonly string ForestCrimsonTemplarClass8AlignGuid = "f150c65d-ecf1-4f27-be65-06424031abf7";
        private const string ForestCrimsonTemplarClass10Align = "ForestCrimsonTemplarClass10Align";
        private static readonly string ForestCrimsonTemplarClass10AlignGuid = "8fd7954f-45fd-4d9d-83b6-2c1d69c36f42";
        private const string ForestDeadeyeDevoteeClass0Align = "ForestDeadeyeDevoteeClass0Align";
        private static readonly string ForestDeadeyeDevoteeClass0AlignGuid = "9a16245e-4179-4bd7-811a-33b0747a9e87";
        internal const string ForestDeadeyeDevoteeClass0AlignDisplayName = "EvangelistDeadeyeDevoteeClass0Align.Name";
        private const string ForestDeadeyeDevoteeClass0AlignDescription = "EvangelistDeadeyeDevoteeClass0Align.Description";
        private const string ForestDeadeyeDevoteeClass2Align = "ForestDeadeyeDevoteeClass2Align";
        private static readonly string ForestDeadeyeDevoteeClass2AlignGuid = "864f293b-d13e-45d6-9db4-a250b0980474";
        private const string ForestDeadeyeDevoteeClass4Align = "ForestDeadeyeDevoteeClass4Align";
        private static readonly string ForestDeadeyeDevoteeClass4AlignGuid = "fd875343-15a6-4cd2-b26a-3e4bb02b5df3";
        private const string ForestDeadeyeDevoteeClass6Align = "ForestDeadeyeDevoteeClass6Align";
        private static readonly string ForestDeadeyeDevoteeClass6AlignGuid = "8fb4b860-b027-45a9-bd01-4448e5a0ff85";
        private const string ForestDeadeyeDevoteeClass8Align = "ForestDeadeyeDevoteeClass8Align";
        private static readonly string ForestDeadeyeDevoteeClass8AlignGuid = "e4ddd38b-8e48-496f-8b23-8c2dd34483a2";
        private const string ForestDeadeyeDevoteeClass10Align = "ForestDeadeyeDevoteeClass10Align";
        private static readonly string ForestDeadeyeDevoteeClass10AlignGuid = "06129e6c-c68c-4e69-8734-c7c7b9140f8e";
        private const string ForestDragonFuryClass0Align = "ForestDragonFuryClass0Align";
        private static readonly string ForestDragonFuryClass0AlignGuid = "f715853d-97b0-4364-9c1d-84f5849689b0";
        internal const string ForestDragonFuryClass0AlignDisplayName = "EvangelistDragonFuryClass0Align.Name";
        private const string ForestDragonFuryClass0AlignDescription = "EvangelistDragonFuryClass0Align.Description";
        private const string ForestDragonFuryClass2Align = "ForestDragonFuryClass2Align";
        private static readonly string ForestDragonFuryClass2AlignGuid = "ba3ab15f-cfa5-4270-81c9-480d0d555da6";
        private const string ForestDragonFuryClass4Align = "ForestDragonFuryClass4Align";
        private static readonly string ForestDragonFuryClass4AlignGuid = "8b5cbffd-37c8-453d-b3c6-ec36a7322c96";
        private const string ForestDragonFuryClass6Align = "ForestDragonFuryClass6Align";
        private static readonly string ForestDragonFuryClass6AlignGuid = "18c3fcde-839e-43e3-867d-c85d4e23bd7d";
        private const string ForestDragonFuryClass8Align = "ForestDragonFuryClass8Align";
        private static readonly string ForestDragonFuryClass8AlignGuid = "6f8067a9-4013-4035-9580-2796550a49c6";
        private const string ForestDragonFuryClass10Align = "ForestDragonFuryClass10Align";
        private static readonly string ForestDragonFuryClass10AlignGuid = "045b56f7-8369-4a61-b38c-1f33661083e5";
        private const string ForestEsotericKnightClass0Align = "ForestEsotericKnightClass0Align";
        private static readonly string ForestEsotericKnightClass0AlignGuid = "23d496ef-8b9b-4d11-8c7c-1b932a4d6878";
        internal const string ForestEsotericKnightClass0AlignDisplayName = "EvangelistEsotericKnightClass0Align.Name";
        private const string ForestEsotericKnightClass0AlignDescription = "EvangelistEsotericKnightClass0Align.Description";
        private const string ForestEsotericKnightClass2Align = "ForestEsotericKnightClass2Align";
        private static readonly string ForestEsotericKnightClass2AlignGuid = "3cc7e1a6-f532-4376-a0e7-5bc55124e38f";
        private const string ForestEsotericKnightClass4Align = "ForestEsotericKnightClass4Align";
        private static readonly string ForestEsotericKnightClass4AlignGuid = "06fbc751-8a5f-4d05-b1ca-744931df768f";
        private const string ForestEsotericKnightClass6Align = "ForestEsotericKnightClass6Align";
        private static readonly string ForestEsotericKnightClass6AlignGuid = "21fd0182-2ca5-4a1c-adce-298a4560bb7c";
        private const string ForestEsotericKnightClass8Align = "ForestEsotericKnightClass8Align";
        private static readonly string ForestEsotericKnightClass8AlignGuid = "3744fc11-d112-4aef-b7f1-a245e9861374";
        private const string ForestEsotericKnightClass10Align = "ForestEsotericKnightClass10Align";
        private static readonly string ForestEsotericKnightClass10AlignGuid = "19f7b257-9d1a-49f2-a17c-1f697fdf9b6c";
        private const string ForestExaltedEvangelistClass0Align = "ForestExaltedEvangelistClass0Align";
        private static readonly string ForestExaltedEvangelistClass0AlignGuid = "8a041fa5-8bbc-46d1-8fd7-839d92d6e741";
        internal const string ForestExaltedEvangelistClass0AlignDisplayName = "EvangelistExaltedEvangelistClass0Align.Name";
        private const string ForestExaltedEvangelistClass0AlignDescription = "EvangelistExaltedEvangelistClass0Align.Description";
        private const string ForestExaltedEvangelistClass2Align = "ForestExaltedEvangelistClass2Align";
        private static readonly string ForestExaltedEvangelistClass2AlignGuid = "796d2c99-fc69-4b4f-93c9-9cc4ce0227aa";
        private const string ForestExaltedEvangelistClass4Align = "ForestExaltedEvangelistClass4Align";
        private static readonly string ForestExaltedEvangelistClass4AlignGuid = "cf23ec10-4685-48eb-bd33-c77fe6d03160";
        private const string ForestExaltedEvangelistClass6Align = "ForestExaltedEvangelistClass6Align";
        private static readonly string ForestExaltedEvangelistClass6AlignGuid = "099a8eeb-9b31-4593-90a8-a1368e9fc2ff";
        private const string ForestExaltedEvangelistClass8Align = "ForestExaltedEvangelistClass8Align";
        private static readonly string ForestExaltedEvangelistClass8AlignGuid = "e34d56ef-825d-45a5-b816-a6357ae12b5d";
        private const string ForestExaltedEvangelistClass10Align = "ForestExaltedEvangelistClass10Align";
        private static readonly string ForestExaltedEvangelistClass10AlignGuid = "c224cc08-2e3c-4d8c-9693-95ed9992e0a9";
        private const string ForestFuriousGuardianClass0Align = "ForestFuriousGuardianClass0Align";
        private static readonly string ForestFuriousGuardianClass0AlignGuid = "c0c4350e-5ad7-4030-b943-baac11e62cd7";
        internal const string ForestFuriousGuardianClass0AlignDisplayName = "EvangelistFuriousGuardianClass0Align.Name";
        private const string ForestFuriousGuardianClass0AlignDescription = "EvangelistFuriousGuardianClass0Align.Description";
        private const string ForestFuriousGuardianClass2Align = "ForestFuriousGuardianClass2Align";
        private static readonly string ForestFuriousGuardianClass2AlignGuid = "e2db2190-d342-4b72-8106-fac4ca109254";
        private const string ForestFuriousGuardianClass4Align = "ForestFuriousGuardianClass4Align";
        private static readonly string ForestFuriousGuardianClass4AlignGuid = "a280b781-59e2-47b6-ba43-820d6758d5de";
        private const string ForestFuriousGuardianClass6Align = "ForestFuriousGuardianClass6Align";
        private static readonly string ForestFuriousGuardianClass6AlignGuid = "9193b16b-e20d-49e3-93b0-9aebd0d65b88";
        private const string ForestFuriousGuardianClass8Align = "ForestFuriousGuardianClass8Align";
        private static readonly string ForestFuriousGuardianClass8AlignGuid = "c0d004b2-a359-4c68-885c-0df7f1cb42e3";
        private const string ForestFuriousGuardianClass10Align = "ForestFuriousGuardianClass10Align";
        private static readonly string ForestFuriousGuardianClass10AlignGuid = "2359c536-67be-4596-a0fe-fb040f7aed11";
        private const string ForestHalflingOpportunistClass0Align = "ForestHalflingOpportunistClass0Align";
        private static readonly string ForestHalflingOpportunistClass0AlignGuid = "4c9ca78c-7f77-473b-a82c-8a91f8c231fd";
        internal const string ForestHalflingOpportunistClass0AlignDisplayName = "EvangelistHalflingOpportunistClass0Align.Name";
        private const string ForestHalflingOpportunistClass0AlignDescription = "EvangelistHalflingOpportunistClass0Align.Description";
        private const string ForestHalflingOpportunistClass2Align = "ForestHalflingOpportunistClass2Align";
        private static readonly string ForestHalflingOpportunistClass2AlignGuid = "912b5676-accb-45ee-9e49-cf9997060ee2";
        private const string ForestHalflingOpportunistClass4Align = "ForestHalflingOpportunistClass4Align";
        private static readonly string ForestHalflingOpportunistClass4AlignGuid = "d259f9bd-fa58-4776-a715-9506bcd87fc1";
        private const string ForestHalflingOpportunistClass6Align = "ForestHalflingOpportunistClass6Align";
        private static readonly string ForestHalflingOpportunistClass6AlignGuid = "b1348173-7771-4200-876a-aa4bef0793f7";
        private const string ForestHalflingOpportunistClass8Align = "ForestHalflingOpportunistClass8Align";
        private static readonly string ForestHalflingOpportunistClass8AlignGuid = "26f2e1b1-0dca-4093-b272-dc3dd720df29";
        private const string ForestHalflingOpportunistClass10Align = "ForestHalflingOpportunistClass10Align";
        private static readonly string ForestHalflingOpportunistClass10AlignGuid = "43bce0b0-01dc-4f75-9021-07dcfe59fb0c";
        private const string ForestHinterlanderClass0Align = "ForestHinterlanderClass0Align";
        private static readonly string ForestHinterlanderClass0AlignGuid = "5020f898-751e-42df-bac9-f652e20ca837";
        internal const string ForestHinterlanderClass0AlignDisplayName = "EvangelistHinterlanderClass0Align.Name";
        private const string ForestHinterlanderClass0AlignDescription = "EvangelistHinterlanderClass0Align.Description";
        private const string ForestHinterlanderClass2Align = "ForestHinterlanderClass2Align";
        private static readonly string ForestHinterlanderClass2AlignGuid = "36c65c23-db06-49ec-a9da-e790e85692e1";
        private const string ForestHinterlanderClass4Align = "ForestHinterlanderClass4Align";
        private static readonly string ForestHinterlanderClass4AlignGuid = "5887313e-89ad-42b4-892c-75b18a10f3a6";
        private const string ForestHinterlanderClass6Align = "ForestHinterlanderClass6Align";
        private static readonly string ForestHinterlanderClass6AlignGuid = "21408995-19ca-4b1a-873c-7cf2af2eb704";
        private const string ForestHinterlanderClass8Align = "ForestHinterlanderClass8Align";
        private static readonly string ForestHinterlanderClass8AlignGuid = "4a5a7d1f-62bf-414a-9438-e8fcefbc548b";
        private const string ForestHinterlanderClass10Align = "ForestHinterlanderClass10Align";
        private static readonly string ForestHinterlanderClass10AlignGuid = "8cb9759b-1e06-461d-b527-b5358ad3b41f";
        private const string ForestHorizonWalkerClass0Align = "ForestHorizonWalkerClass0Align";
        private static readonly string ForestHorizonWalkerClass0AlignGuid = "81269e14-c527-4992-a954-25deaf6cf617";
        internal const string ForestHorizonWalkerClass0AlignDisplayName = "EvangelistHorizonWalkerClass0Align.Name";
        private const string ForestHorizonWalkerClass0AlignDescription = "EvangelistHorizonWalkerClass0Align.Description";
        private const string ForestHorizonWalkerClass2Align = "ForestHorizonWalkerClass2Align";
        private static readonly string ForestHorizonWalkerClass2AlignGuid = "62f59d5e-16ff-4c0c-ac36-3e8f35c6d7dd";
        private const string ForestHorizonWalkerClass4Align = "ForestHorizonWalkerClass4Align";
        private static readonly string ForestHorizonWalkerClass4AlignGuid = "10542cef-b135-4a8f-ae0f-3cbeda5ea609";
        private const string ForestHorizonWalkerClass6Align = "ForestHorizonWalkerClass6Align";
        private static readonly string ForestHorizonWalkerClass6AlignGuid = "57681abb-ee88-4beb-ac28-23d59f31e511";
        private const string ForestHorizonWalkerClass8Align = "ForestHorizonWalkerClass8Align";
        private static readonly string ForestHorizonWalkerClass8AlignGuid = "9cf82d07-5ad8-4915-adcd-1df020a0eb7d";
        private const string ForestHorizonWalkerClass10Align = "ForestHorizonWalkerClass10Align";
        private static readonly string ForestHorizonWalkerClass10AlignGuid = "a0035859-3b95-4894-840b-c7fde7bc15b4";
        private const string ForestInheritorCrusaderClass0Align = "ForestInheritorCrusaderClass0Align";
        private static readonly string ForestInheritorCrusaderClass0AlignGuid = "200b94c6-cef6-4081-9529-61b9c9b489ef";
        internal const string ForestInheritorCrusaderClass0AlignDisplayName = "EvangelistInheritorCrusaderClass0Align.Name";
        private const string ForestInheritorCrusaderClass0AlignDescription = "EvangelistInheritorCrusaderClass0Align.Description";
        private const string ForestInheritorCrusaderClass2Align = "ForestInheritorCrusaderClass2Align";
        private static readonly string ForestInheritorCrusaderClass2AlignGuid = "4958bb63-83a1-4708-b41c-6f3766ac6475";
        private const string ForestInheritorCrusaderClass4Align = "ForestInheritorCrusaderClass4Align";
        private static readonly string ForestInheritorCrusaderClass4AlignGuid = "6c8d14df-704f-42f6-a6f4-93af3ac23da2";
        private const string ForestInheritorCrusaderClass6Align = "ForestInheritorCrusaderClass6Align";
        private static readonly string ForestInheritorCrusaderClass6AlignGuid = "6b84cce5-e7cc-4383-99bb-2f5b2210bdf1";
        private const string ForestInheritorCrusaderClass8Align = "ForestInheritorCrusaderClass8Align";
        private static readonly string ForestInheritorCrusaderClass8AlignGuid = "994a0a0f-3d47-4d1b-b5b2-1a86ba44a2f6";
        private const string ForestInheritorCrusaderClass10Align = "ForestInheritorCrusaderClass10Align";
        private static readonly string ForestInheritorCrusaderClass10AlignGuid = "87cc6e18-88b3-429a-9512-c88857d59672";
        private const string ForestMammothRiderClass0Align = "ForestMammothRiderClass0Align";
        private static readonly string ForestMammothRiderClass0AlignGuid = "cb5788ea-9fd4-401b-b5b7-cbf9ecf0a7a8";
        internal const string ForestMammothRiderClass0AlignDisplayName = "EvangelistMammothRiderClass0Align.Name";
        private const string ForestMammothRiderClass0AlignDescription = "EvangelistMammothRiderClass0Align.Description";
        private const string ForestMammothRiderClass2Align = "ForestMammothRiderClass2Align";
        private static readonly string ForestMammothRiderClass2AlignGuid = "3686be3b-a0ae-4fb2-b425-12bf0d745d12";
        private const string ForestMammothRiderClass4Align = "ForestMammothRiderClass4Align";
        private static readonly string ForestMammothRiderClass4AlignGuid = "db8ca65e-20ec-454e-85ef-31aab31f812f";
        private const string ForestMammothRiderClass6Align = "ForestMammothRiderClass6Align";
        private static readonly string ForestMammothRiderClass6AlignGuid = "597ad6c3-31ff-4c18-aab3-9f1d476cd9a9";
        private const string ForestMammothRiderClass8Align = "ForestMammothRiderClass8Align";
        private static readonly string ForestMammothRiderClass8AlignGuid = "1f127102-d662-4881-931e-f30f7a83281f";
        private const string ForestMammothRiderClass10Align = "ForestMammothRiderClass10Align";
        private static readonly string ForestMammothRiderClass10AlignGuid = "3a0ac4d5-bc9f-440e-932e-82c8677cc28c";
        private const string ForestSanguineAngelClass0Align = "ForestSanguineAngelClass0Align";
        private static readonly string ForestSanguineAngelClass0AlignGuid = "31243442-d846-4e6b-bdd0-30ccf0911145";
        internal const string ForestSanguineAngelClass0AlignDisplayName = "EvangelistSanguineAngelClass0Align.Name";
        private const string ForestSanguineAngelClass0AlignDescription = "EvangelistSanguineAngelClass0Align.Description";
        private const string ForestSanguineAngelClass2Align = "ForestSanguineAngelClass2Align";
        private static readonly string ForestSanguineAngelClass2AlignGuid = "39802aef-89f1-4c00-87ad-8667f8698ae3";
        private const string ForestSanguineAngelClass4Align = "ForestSanguineAngelClass4Align";
        private static readonly string ForestSanguineAngelClass4AlignGuid = "a0480404-a0ed-47a4-bf77-b8b6fff0552c";
        private const string ForestSanguineAngelClass6Align = "ForestSanguineAngelClass6Align";
        private static readonly string ForestSanguineAngelClass6AlignGuid = "9134b7ee-76bf-48b5-8b31-410599c38e3a";
        private const string ForestSanguineAngelClass8Align = "ForestSanguineAngelClass8Align";
        private static readonly string ForestSanguineAngelClass8AlignGuid = "7b1d9888-03be-437e-af3a-3e597ba64f88";
        private const string ForestSanguineAngelClass10Align = "ForestSanguineAngelClass10Align";
        private static readonly string ForestSanguineAngelClass10AlignGuid = "7603a407-2622-4a55-9f33-65b8fa9fdf8d";
        private const string ForestScarSeekerClass0Align = "ForestScarSeekerClass0Align";
        private static readonly string ForestScarSeekerClass0AlignGuid = "fc2aba1d-1c20-4786-bb5d-f922857a18f4";
        internal const string ForestScarSeekerClass0AlignDisplayName = "EvangelistScarSeekerClass0Align.Name";
        private const string ForestScarSeekerClass0AlignDescription = "EvangelistScarSeekerClass0Align.Description";
        private const string ForestScarSeekerClass2Align = "ForestScarSeekerClass2Align";
        private static readonly string ForestScarSeekerClass2AlignGuid = "e9d4d2cc-6a23-4d7b-a88f-427c0764de2e";
        private const string ForestScarSeekerClass4Align = "ForestScarSeekerClass4Align";
        private static readonly string ForestScarSeekerClass4AlignGuid = "040dc4d7-9534-4596-8fd0-12a84cbe8509";
        private const string ForestScarSeekerClass6Align = "ForestScarSeekerClass6Align";
        private static readonly string ForestScarSeekerClass6AlignGuid = "93e0b698-1249-401e-a4b4-d56a8fa8495a";
        private const string ForestScarSeekerClass8Align = "ForestScarSeekerClass8Align";
        private static readonly string ForestScarSeekerClass8AlignGuid = "3ac21cd6-c779-416d-93e9-7e092b42d07c";
        private const string ForestScarSeekerClass10Align = "ForestScarSeekerClass10Align";
        private static readonly string ForestScarSeekerClass10AlignGuid = "8a2642ac-ef25-4a92-8a04-41220e71dfa8";
        private const string ForestSentinelClass0Align = "ForestSentinelClass0Align";
        private static readonly string ForestSentinelClass0AlignGuid = "c273d1ad-675e-4f4f-b546-89e0a5f580a2";
        internal const string ForestSentinelClass0AlignDisplayName = "EvangelistSentinelClass0Align.Name";
        private const string ForestSentinelClass0AlignDescription = "EvangelistSentinelClass0Align.Description";
        private const string ForestSentinelClass2Align = "ForestSentinelClass2Align";
        private static readonly string ForestSentinelClass2AlignGuid = "1656d666-f688-41b4-9ef5-fe0916133d7a";
        private const string ForestSentinelClass4Align = "ForestSentinelClass4Align";
        private static readonly string ForestSentinelClass4AlignGuid = "195a053b-01f8-47b6-9a09-19158845f494";
        private const string ForestSentinelClass6Align = "ForestSentinelClass6Align";
        private static readonly string ForestSentinelClass6AlignGuid = "75212680-dfb9-439a-aa37-99b09279bea7";
        private const string ForestSentinelClass8Align = "ForestSentinelClass8Align";
        private static readonly string ForestSentinelClass8AlignGuid = "6997dd07-bc71-436e-a814-40b11ff87ea7";
        private const string ForestSentinelClass10Align = "ForestSentinelClass10Align";
        private static readonly string ForestSentinelClass10AlignGuid = "3ea9d391-321e-4ff0-939c-f11e107359ab";
        private const string ForestShadowDancerClass0Align = "ForestShadowDancerClass0Align";
        private static readonly string ForestShadowDancerClass0AlignGuid = "7a33b577-4a4e-4080-b3b5-7997c047af42";
        internal const string ForestShadowDancerClass0AlignDisplayName = "EvangelistShadowDancerClass0Align.Name";
        private const string ForestShadowDancerClass0AlignDescription = "EvangelistShadowDancerClass0Align.Description";
        private const string ForestShadowDancerClass2Align = "ForestShadowDancerClass2Align";
        private static readonly string ForestShadowDancerClass2AlignGuid = "2040b9dc-8bcc-4cfe-8738-fe3c9ba074ba";
        private const string ForestShadowDancerClass4Align = "ForestShadowDancerClass4Align";
        private static readonly string ForestShadowDancerClass4AlignGuid = "92f5669e-98ae-4aed-8a2a-82bcd3f4f092";
        private const string ForestShadowDancerClass6Align = "ForestShadowDancerClass6Align";
        private static readonly string ForestShadowDancerClass6AlignGuid = "247010a3-666c-4ece-86f1-0470f8006f14";
        private const string ForestShadowDancerClass8Align = "ForestShadowDancerClass8Align";
        private static readonly string ForestShadowDancerClass8AlignGuid = "adf3b2b6-c7ad-4fe8-8297-f135fbe68e70";
        private const string ForestShadowDancerClass10Align = "ForestShadowDancerClass10Align";
        private static readonly string ForestShadowDancerClass10AlignGuid = "2d98edcb-b2db-4d2e-b230-54fa1e32482b";
        private const string ForestSouldrinkerClass0Align = "ForestSouldrinkerClass0Align";
        private static readonly string ForestSouldrinkerClass0AlignGuid = "5fbb8391-1fe6-4731-bc96-a87cd90d8583";
        internal const string ForestSouldrinkerClass0AlignDisplayName = "EvangelistSouldrinkerClass0Align.Name";
        private const string ForestSouldrinkerClass0AlignDescription = "EvangelistSouldrinkerClass0Align.Description";
        private const string ForestSouldrinkerClass2Align = "ForestSouldrinkerClass2Align";
        private static readonly string ForestSouldrinkerClass2AlignGuid = "06eb82da-0af5-48e8-bf31-85f0349dce16";
        private const string ForestSouldrinkerClass4Align = "ForestSouldrinkerClass4Align";
        private static readonly string ForestSouldrinkerClass4AlignGuid = "80bc7ed8-5a33-4769-8f5e-c444d063574b";
        private const string ForestSouldrinkerClass6Align = "ForestSouldrinkerClass6Align";
        private static readonly string ForestSouldrinkerClass6AlignGuid = "8cba57bf-c5c0-4314-ac21-622e76ac4134";
        private const string ForestSouldrinkerClass8Align = "ForestSouldrinkerClass8Align";
        private static readonly string ForestSouldrinkerClass8AlignGuid = "4910890c-109d-44dd-bbbf-9c12488f4dbd";
        private const string ForestSouldrinkerClass10Align = "ForestSouldrinkerClass10Align";
        private static readonly string ForestSouldrinkerClass10AlignGuid = "e200d305-e7ef-455b-a34f-381aff38ee0c";
        private const string ForestUmbralAgentClass0Align = "ForestUmbralAgentClass0Align";
        private static readonly string ForestUmbralAgentClass0AlignGuid = "4ef82933-91a4-42f9-b5f2-dfcb00464aea";
        internal const string ForestUmbralAgentClass0AlignDisplayName = "EvangelistUmbralAgentClass0Align.Name";
        private const string ForestUmbralAgentClass0AlignDescription = "EvangelistUmbralAgentClass0Align.Description";
        private const string ForestUmbralAgentClass2Align = "ForestUmbralAgentClass2Align";
        private static readonly string ForestUmbralAgentClass2AlignGuid = "d636c99e-8c25-4faa-9958-d36dd20ec399";
        private const string ForestUmbralAgentClass4Align = "ForestUmbralAgentClass4Align";
        private static readonly string ForestUmbralAgentClass4AlignGuid = "0477f43b-d2a1-4108-867b-3c0dab772770";
        private const string ForestUmbralAgentClass6Align = "ForestUmbralAgentClass6Align";
        private static readonly string ForestUmbralAgentClass6AlignGuid = "9f08d2e1-89e3-40b6-ab3a-d26bb32b2a98";
        private const string ForestUmbralAgentClass8Align = "ForestUmbralAgentClass8Align";
        private static readonly string ForestUmbralAgentClass8AlignGuid = "68d1b069-7d7e-418b-b264-52be425e1fa2";
        private const string ForestUmbralAgentClass10Align = "ForestUmbralAgentClass10Align";
        private static readonly string ForestUmbralAgentClass10AlignGuid = "49b1fff7-5618-44bc-8a8f-be45e1041a51";
        private const string ForestMicroAntiPaladinClass0Align = "ForestMicroAntiPaladinClass0Align";
        private static readonly string ForestMicroAntiPaladinClass0AlignGuid = "e7b94a1f-e10e-4868-a9d9-e2769c5d9ebc";
        internal const string ForestMicroAntiPaladinClass0AlignDisplayName = "EvangelistMicroAntiPaladinClass0Align.Name";
        private const string ForestMicroAntiPaladinClass0AlignDescription = "EvangelistMicroAntiPaladinClass0Align.Description";
        private const string ForestMicroAntiPaladinClass2Align = "ForestMicroAntiPaladinClass2Align";
        private static readonly string ForestMicroAntiPaladinClass2AlignGuid = "08aa385e-aa4f-4777-93cf-4e868dfc3aba";
        private const string ForestMicroAntiPaladinClass4Align = "ForestMicroAntiPaladinClass4Align";
        private static readonly string ForestMicroAntiPaladinClass4AlignGuid = "fe4e1300-61b1-42a3-85f7-8c6ebcb5b4a3";
        private const string ForestMicroAntiPaladinClass6Align = "ForestMicroAntiPaladinClass6Align";
        private static readonly string ForestMicroAntiPaladinClass6AlignGuid = "96dba32e-923d-4a33-a534-5568c2e1c030";
        private const string ForestMicroAntiPaladinClass8Align = "ForestMicroAntiPaladinClass8Align";
        private static readonly string ForestMicroAntiPaladinClass8AlignGuid = "eb93f966-01ae-492d-acc1-28c80fb83b01";
        private const string ForestMicroAntiPaladinClass10Align = "ForestMicroAntiPaladinClass10Align";
        private static readonly string ForestMicroAntiPaladinClass10AlignGuid = "003c9083-99a5-4378-a01d-bb2a3913f585";
        private const string ForestOathbreakerClass0Align = "ForestOathbreakerClass0Align";
        private static readonly string ForestOathbreakerClass0AlignGuid = "81113918-7637-461c-ab10-a01554c7f8fd";
        internal const string ForestOathbreakerClass0AlignDisplayName = "EvangelistOathbreakerClass0Align.Name";
        private const string ForestOathbreakerClass0AlignDescription = "EvangelistOathbreakerClass0Align.Description";
        private const string ForestOathbreakerClass2Align = "ForestOathbreakerClass2Align";
        private static readonly string ForestOathbreakerClass2AlignGuid = "b73c1ebd-663a-4fa8-8c3a-70c90ec17f58";
        private const string ForestOathbreakerClass4Align = "ForestOathbreakerClass4Align";
        private static readonly string ForestOathbreakerClass4AlignGuid = "291fc207-6492-4671-a6d3-f29591be5f95";
        private const string ForestOathbreakerClass6Align = "ForestOathbreakerClass6Align";
        private static readonly string ForestOathbreakerClass6AlignGuid = "22737c5c-8fb8-40b2-9023-01c8346f0ae3";
        private const string ForestOathbreakerClass8Align = "ForestOathbreakerClass8Align";
        private static readonly string ForestOathbreakerClass8AlignGuid = "e5add993-c709-4135-a23a-c140d5ab8e55";
        private const string ForestOathbreakerClass10Align = "ForestOathbreakerClass10Align";
        private static readonly string ForestOathbreakerClass10AlignGuid = "c5fedc80-d88c-4ece-9af2-97b060012682";
        private const string ForestDreadKnightClass0Align = "ForestDreadKnightClass0Align";
        private static readonly string ForestDreadKnightClass0AlignGuid = "e90fe073-500a-4109-897c-90f961eee57e";
        internal const string ForestDreadKnightClass0AlignDisplayName = "EvangelistDreadKnightClass0Align.Name";
        private const string ForestDreadKnightClass0AlignDescription = "EvangelistDreadKnightClass0Align.Description";
        private const string ForestDreadKnightClass2Align = "ForestDreadKnightClass2Align";
        private static readonly string ForestDreadKnightClass2AlignGuid = "03f84447-a212-4711-8da8-1067d0483241";
        private const string ForestDreadKnightClass4Align = "ForestDreadKnightClass4Align";
        private static readonly string ForestDreadKnightClass4AlignGuid = "8d9d1b0a-3f51-46c3-b6e7-b5eaec950c49";
        private const string ForestDreadKnightClass6Align = "ForestDreadKnightClass6Align";
        private static readonly string ForestDreadKnightClass6AlignGuid = "0cdec48d-5599-4fb2-84d9-41d7cf9bd390";
        private const string ForestDreadKnightClass8Align = "ForestDreadKnightClass8Align";
        private static readonly string ForestDreadKnightClass8AlignGuid = "bbb21177-d045-445b-b6fb-8f7aca61e310";
        private const string ForestDreadKnightClass10Align = "ForestDreadKnightClass10Align";
        private static readonly string ForestDreadKnightClass10AlignGuid = "db95d463-2fb5-44da-8178-4ef4eecaff3d";
        private const string ForestStargazerClass0Align = "ForestStargazerClass0Align";
        private static readonly string ForestStargazerClass0AlignGuid = "53e839d7-6ea5-455e-9328-19bdaafa1379";
        internal const string ForestStargazerClass0AlignDisplayName = "EvangelistStargazerClass0Align.Name";
        private const string ForestStargazerClass0AlignDescription = "EvangelistStargazerClass0Align.Description";
        private const string ForestStargazerClass2Align = "ForestStargazerClass2Align";
        private static readonly string ForestStargazerClass2AlignGuid = "335729d5-8116-4b93-bf3b-5d26432b2ce2";
        private const string ForestStargazerClass4Align = "ForestStargazerClass4Align";
        private static readonly string ForestStargazerClass4AlignGuid = "947745df-ac3b-423b-bdc5-381045fec1da";
        private const string ForestStargazerClass6Align = "ForestStargazerClass6Align";
        private static readonly string ForestStargazerClass6AlignGuid = "59c6e508-42da-4dfd-ac78-9f63ac9cbb80";
        private const string ForestStargazerClass8Align = "ForestStargazerClass8Align";
        private static readonly string ForestStargazerClass8AlignGuid = "7a6fff24-4364-424a-ad24-6d3f29d65763";
        private const string ForestStargazerClass10Align = "ForestStargazerClass10Align";
        private static readonly string ForestStargazerClass10AlignGuid = "db950d8a-5a5b-46e4-abc7-38f35c5556b4";
        private const string ForestSwashbucklerClass0Align = "ForestSwashbucklerClass0Align";
        private static readonly string ForestSwashbucklerClass0AlignGuid = "fcbb6aaf-04a0-4f47-84fe-68978fb822bb";
        internal const string ForestSwashbucklerClass0AlignDisplayName = "EvangelistSwashbucklerClass0Align.Name";
        private const string ForestSwashbucklerClass0AlignDescription = "EvangelistSwashbucklerClass0Align.Description";
        private const string ForestSwashbucklerClass2Align = "ForestSwashbucklerClass2Align";
        private static readonly string ForestSwashbucklerClass2AlignGuid = "11d4ffac-3bd9-466c-aaef-666d489d29ae";
        private const string ForestSwashbucklerClass4Align = "ForestSwashbucklerClass4Align";
        private static readonly string ForestSwashbucklerClass4AlignGuid = "938143ea-d239-43c7-9336-bc251545a69b";
        private const string ForestSwashbucklerClass6Align = "ForestSwashbucklerClass6Align";
        private static readonly string ForestSwashbucklerClass6AlignGuid = "ac4b19d1-c85c-486e-8d6f-7c34bf6c96f6";
        private const string ForestSwashbucklerClass8Align = "ForestSwashbucklerClass8Align";
        private static readonly string ForestSwashbucklerClass8AlignGuid = "a48f5669-96e0-4137-8fc1-3a18fc094d5a";
        private const string ForestSwashbucklerClass10Align = "ForestSwashbucklerClass10Align";
        private static readonly string ForestSwashbucklerClass10AlignGuid = "956dba62-d4d3-41a2-a9ca-eef0dba877a2";
        private const string ForestHolyVindicatorClass0Align = "ForestHolyVindicatorClass0Align";
        private static readonly string ForestHolyVindicatorClass0AlignGuid = "cfc639e7-9b74-4f10-b7a6-4b4e08e78787";
        internal const string ForestHolyVindicatorClass0AlignDisplayName = "EvangelistHolyVindicatorClass0Align.Name";
        private const string ForestHolyVindicatorClass0AlignDescription = "EvangelistHolyVindicatorClass0Align.Description";
        private const string ForestHolyVindicatorClass2Align = "ForestHolyVindicatorClass2Align";
        private static readonly string ForestHolyVindicatorClass2AlignGuid = "07144892-4c2c-48ab-a37a-35d73d458194";
        private const string ForestHolyVindicatorClass4Align = "ForestHolyVindicatorClass4Align";
        private static readonly string ForestHolyVindicatorClass4AlignGuid = "4e9aa4e4-79f8-4eea-8a5c-9fe5a280bdd3";
        private const string ForestHolyVindicatorClass6Align = "ForestHolyVindicatorClass6Align";
        private static readonly string ForestHolyVindicatorClass6AlignGuid = "c2323ea5-ba81-4a84-bcd6-f161b476912b";
        private const string ForestHolyVindicatorClass8Align = "ForestHolyVindicatorClass8Align";
        private static readonly string ForestHolyVindicatorClass8AlignGuid = "055de9be-3e8e-4c48-af4a-77477dde22f4";
        private const string ForestHolyVindicatorClass10Align = "ForestHolyVindicatorClass10Align";
        private static readonly string ForestHolyVindicatorClass10AlignGuid = "035520b2-88ff-42b6-8f1a-7af8b409ba81";
        private const string ForestSummonerClass0Align = "ForestSummonerClass0Align";
        private static readonly string ForestSummonerClass0AlignGuid = "986d2f53-0ccd-4c4b-8568-c59f5007de5a";
        internal const string ForestSummonerClass0AlignDisplayName = "EvangelistSummonerClass0Align.Name";
        private const string ForestSummonerClass0AlignDescription = "EvangelistSummonerClass0Align.Description";
        private const string ForestSummonerClass2Align = "ForestSummonerClass2Align";
        private static readonly string ForestSummonerClass2AlignGuid = "b4dca409-1aa3-461c-8437-f7e07bfa7699";
        private const string ForestSummonerClass4Align = "ForestSummonerClass4Align";
        private static readonly string ForestSummonerClass4AlignGuid = "17c73e14-461d-47bb-ab22-8ca8bdb2d3f1";
        private const string ForestSummonerClass6Align = "ForestSummonerClass6Align";
        private static readonly string ForestSummonerClass6AlignGuid = "263c7ca4-843e-496e-ac91-64019dae5ea3";
        private const string ForestSummonerClass8Align = "ForestSummonerClass8Align";
        private static readonly string ForestSummonerClass8AlignGuid = "df279fcf-14dc-4c3e-95a4-b37170b1138f";
        private const string ForestSummonerClass10Align = "ForestSummonerClass10Align";
        private static readonly string ForestSummonerClass10AlignGuid = "84617b70-2dd6-4b01-b3bb-b1d7215ac86e";
        private const string ForestLionBladeClass0Align = "ForestLionBladeClass0Align";
        private static readonly string ForestLionBladeClass0AlignGuid = "20f1f223-2cfe-4e4f-9eb5-af4544df3139";
        internal const string ForestLionBladeClass0AlignDisplayName = "EvangelistLionBladeClass0Align.Name";
        private const string ForestLionBladeClass0AlignDescription = "EvangelistLionBladeClass0Align.Description";
        private const string ForestLionBladeClass2Align = "ForestLionBladeClass2Align";
        private static readonly string ForestLionBladeClass2AlignGuid = "9a2dbecb-2629-47c4-8d86-8de852170994";
        private const string ForestLionBladeClass4Align = "ForestLionBladeClass4Align";
        private static readonly string ForestLionBladeClass4AlignGuid = "e9d03895-a88b-497b-a04e-38292ff3eaa6";
        private const string ForestLionBladeClass6Align = "ForestLionBladeClass6Align";
        private static readonly string ForestLionBladeClass6AlignGuid = "c7e3fd85-a814-4fe4-bda9-d6b657511faf";
        private const string ForestLionBladeClass8Align = "ForestLionBladeClass8Align";
        private static readonly string ForestLionBladeClass8AlignGuid = "ed4f4a69-0e06-4486-bee9-dac141bee9ff";
        private const string ForestLionBladeClass10Align = "ForestLionBladeClass10Align";
        private static readonly string ForestLionBladeClass10AlignGuid = "a42aa038-5f1b-4b4e-8cc6-888246cb6d05";
        private const string ForestEnchantingCourtesanClass0Align = "ForestEnchantingCourtesanClass0Align";
        private static readonly string ForestEnchantingCourtesanClass0AlignGuid = "ce8d153d-b66d-443f-b3bf-c05364444186";
        internal const string ForestEnchantingCourtesanClass0AlignDisplayName = "EvangelistEnchantingCourtesanClass0Align.Name";
        private const string ForestEnchantingCourtesanClass0AlignDescription = "EvangelistEnchantingCourtesanClass0Align.Description";
        private const string ForestEnchantingCourtesanClass2Align = "ForestEnchantingCourtesanClass2Align";
        private static readonly string ForestEnchantingCourtesanClass2AlignGuid = "c0b4548e-1441-4603-88e0-754688cb4a4d";
        private const string ForestEnchantingCourtesanClass4Align = "ForestEnchantingCourtesanClass4Align";
        private static readonly string ForestEnchantingCourtesanClass4AlignGuid = "c80ec049-61d6-44ce-a833-23a4fb2e8094";
        private const string ForestEnchantingCourtesanClass6Align = "ForestEnchantingCourtesanClass6Align";
        private static readonly string ForestEnchantingCourtesanClass6AlignGuid = "87f00505-820c-430e-8b1f-825826b4d7c0";
        private const string ForestEnchantingCourtesanClass8Align = "ForestEnchantingCourtesanClass8Align";
        private static readonly string ForestEnchantingCourtesanClass8AlignGuid = "d44ec85f-f79a-4ecc-957d-2cd811c873d8";
        private const string ForestEnchantingCourtesanClass10Align = "ForestEnchantingCourtesanClass10Align";
        private static readonly string ForestEnchantingCourtesanClass10AlignGuid = "4fd71182-87b7-4387-8b2b-d34049ca5d6a";
        private const string ForestHeritorKnightClass0Align = "ForestHeritorKnightClass0Align";
        private static readonly string ForestHeritorKnightClass0AlignGuid = "3b8ccd40-11dd-4d04-8783-878c19a6fb39";
        internal const string ForestHeritorKnightClass0AlignDisplayName = "EvangelistHeritorKnightClass0Align.Name";
        private const string ForestHeritorKnightClass0AlignDescription = "EvangelistHeritorKnightClass0Align.Description";
        private const string ForestHeritorKnightClass2Align = "ForestHeritorKnightClass2Align";
        private static readonly string ForestHeritorKnightClass2AlignGuid = "a7517cee-882e-4468-8910-cd532f06fe59";
        private const string ForestHeritorKnightClass4Align = "ForestHeritorKnightClass4Align";
        private static readonly string ForestHeritorKnightClass4AlignGuid = "f5301c2b-7c6e-4706-9bcb-907abde69d8b";
        private const string ForestHeritorKnightClass6Align = "ForestHeritorKnightClass6Align";
        private static readonly string ForestHeritorKnightClass6AlignGuid = "86bf517a-7f09-43e3-b4ca-d6d38b30b892";
        private const string ForestHeritorKnightClass8Align = "ForestHeritorKnightClass8Align";
        private static readonly string ForestHeritorKnightClass8AlignGuid = "8a5ebc3f-2f6c-46f1-a049-8fae9c051384";
        private const string ForestHeritorKnightClass10Align = "ForestHeritorKnightClass10Align";
        private static readonly string ForestHeritorKnightClass10AlignGuid = "3efe799a-5dc7-4ad9-a946-33eb5b94b989";
        private const string ForestGoldenLegionnaireClass0Align = "ForestGoldenLegionnaireClass0Align";
        private static readonly string ForestGoldenLegionnaireClass0AlignGuid = "2b13eeeb-aac8-40a2-bdb2-477fb28ce392";
        internal const string ForestGoldenLegionnaireClass0AlignDisplayName = "EvangelistGoldenLegionnaireClass0Align.Name";
        private const string ForestGoldenLegionnaireClass0AlignDescription = "EvangelistGoldenLegionnaireClass0Align.Description";
        private const string ForestGoldenLegionnaireClass2Align = "ForestGoldenLegionnaireClass2Align";
        private static readonly string ForestGoldenLegionnaireClass2AlignGuid = "3d080156-b133-4319-99da-23e0cc2c99a7";
        private const string ForestGoldenLegionnaireClass4Align = "ForestGoldenLegionnaireClass4Align";
        private static readonly string ForestGoldenLegionnaireClass4AlignGuid = "a55a3420-f8f4-4ed6-af49-1d1f4818174a";
        private const string ForestGoldenLegionnaireClass6Align = "ForestGoldenLegionnaireClass6Align";
        private static readonly string ForestGoldenLegionnaireClass6AlignGuid = "646cd777-caae-47d1-9b31-d7a9560c12ad";
        private const string ForestGoldenLegionnaireClass8Align = "ForestGoldenLegionnaireClass8Align";
        private static readonly string ForestGoldenLegionnaireClass8AlignGuid = "89f0209a-c2bc-4e51-b15c-7522884a13d7";
        private const string ForestGoldenLegionnaireClass10Align = "ForestGoldenLegionnaireClass10Align";
        private static readonly string ForestGoldenLegionnaireClass10AlignGuid = "980d703d-ba2c-40cf-ba62-e47aa7c638e1";
        private const string ForestBoltAceClass0Align = "ForestBoltAceClass0Align";
        private static readonly string ForestBoltAceClass0AlignGuid = "01d2da95-21ca-4a88-98e4-9753b3bcffd9";
        internal const string ForestBoltAceClass0AlignDisplayName = "EvangelistBoltAceClass0Align.Name";
        private const string ForestBoltAceClass0AlignDescription = "EvangelistBoltAceClass0Align.Description";
        private const string ForestBoltAceClass2Align = "ForestBoltAceClass2Align";
        private static readonly string ForestBoltAceClass2AlignGuid = "110b13c5-d1a8-441a-b2b2-f02ffc46b72d";
        private const string ForestBoltAceClass4Align = "ForestBoltAceClass4Align";
        private static readonly string ForestBoltAceClass4AlignGuid = "19705513-8f65-43c7-a22e-51b8d25304ad";
        private const string ForestBoltAceClass6Align = "ForestBoltAceClass6Align";
        private static readonly string ForestBoltAceClass6AlignGuid = "5f974501-0cb7-4b4b-8915-0252e47788ae";
        private const string ForestBoltAceClass8Align = "ForestBoltAceClass8Align";
        private static readonly string ForestBoltAceClass8AlignGuid = "5d537521-5b54-4ce9-bc14-04e67b8951ab";
        private const string ForestBoltAceClass10Align = "ForestBoltAceClass10Align";
        private static readonly string ForestBoltAceClass10AlignGuid = "53fefc6b-834a-4e09-8798-ca7779e38d01";
        private const string ForestMortalUsherClass0Align = "ForestMortalUsherClass0Align";
        private static readonly string ForestMortalUsherClass0AlignGuid = "3afef363-e83b-4d3f-ad23-d886a8d92271";
        internal const string ForestMortalUsherClass0AlignDisplayName = "EvangelistMortalUsherClass0Align.Name";
        private const string ForestMortalUsherClass0AlignDescription = "EvangelistMortalUsherClass0Align.Description";
        private const string ForestMortalUsherClass2Align = "ForestMortalUsherClass2Align";
        private static readonly string ForestMortalUsherClass2AlignGuid = "55c698c6-44ce-4e78-a6c5-217a32d92f16";
        private const string ForestMortalUsherClass4Align = "ForestMortalUsherClass4Align";
        private static readonly string ForestMortalUsherClass4AlignGuid = "2a447c92-75eb-4345-a0bd-92a0eea68983";
        private const string ForestMortalUsherClass6Align = "ForestMortalUsherClass6Align";
        private static readonly string ForestMortalUsherClass6AlignGuid = "90e2dfac-3559-4cf1-b9dc-c9db5c935a1f";
        private const string ForestMortalUsherClass8Align = "ForestMortalUsherClass8Align";
        private static readonly string ForestMortalUsherClass8AlignGuid = "7614ea11-641e-4d16-8edd-28d33c4cde39";
        private const string ForestMortalUsherClass10Align = "ForestMortalUsherClass10Align";
        private static readonly string ForestMortalUsherClass10AlignGuid = "36907ba5-1c3e-448b-9727-4654d55d55ca";

    }
}
