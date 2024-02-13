using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.RogueTalent;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class LionBlade
    {
        private const string ArchetypeName = "LionBlade";
        public static readonly string ArchetypeGuid = "{F89F7611-EC5E-4829-9BC1-1F98D5B558F2}";
        internal const string ArchetypeDisplayName = "LionBlade.Name";
        private const string ArchetypeDescription = "LionBlade.Description";

        private const string ClassProgressName = "LionBladePrestige";
        private static readonly string ClassProgressGuid = "{C68888D6-4451-4860-BA90-06F576013670}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, BardicPerformanceFeat(), InspirePoiseFeature(), FeatureRefs.Mobility.ToString())
                .AddToLevelEntry(2, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(3, FeatureRefs.AssassinHideInPlainSight.ToString())
                .AddToLevelEntry(4, PerfectSurpriseFeature(), FeatureRefs.FastMovement.ToString())
                .AddToLevelEntry(5, MisfortuneConfigure(), InspirePoiseGuid)
                .AddToLevelEntry(6, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(7, CloudMindFeature(), FeatureRefs.HunterWoodlandStride.ToString())
                .AddToLevelEntry(8, NarrowMissConfigure())
                .AddToLevelEntry(9, InspirePoiseGuid)
                .AddToLevelEntry(10, SilentSoulFeature(), FeatureRefs.SneakAttack.ToString())
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
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteFullStatValue(stat: StatType.SneakAttack, value: 2)
                .AddPrerequisiteFeaturesFromList(new() { FeatureRefs.InspireCompetenceFeature.ToString(), FeatureRefs.SenseiInspireCompetenceFeature.ToString(), CloyingShades.CloyingShadesGuid }, 1)
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
                .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 3)
                .AddPrerequisiteFeaturesFromList(new() { FeatureRefs.Deceitful.ToString(), FeatureRefs.SkillFocusDiplomacy.ToString() }, 1)
                .AddPrerequisiteFeature(FeatureRefs.Improved_Initiative.ToString())
                .Configure();

            var list = new List<ContextRankConfig>() { };

            list.AddRange(BuffRefs.InspireCourageEffectBuff.Reference.Get().GetComponents<ContextRankConfig>());
            list.AddRange(BuffRefs.InspireCourageMythicEffectBuff.Reference.Get().GetComponents<ContextRankConfig>());
            list.AddRange(BuffRefs.InspireCompetenceEffectBuff.Reference.Get().GetComponents<ContextRankConfig>());
            list.AddRange(BuffRefs.InspireCompetenceMythicEffectBuff.Reference.Get().GetComponents<ContextRankConfig>());
            list.AddRange(BuffRefs.InspireTranquilityEffectBuff.Reference.Get().GetComponents<ContextRankConfig>());
            list.AddRange(BuffRefs.InspireTranquilityEffectBuffMythic.Reference.Get().GetComponents<ContextRankConfig>());

            foreach (var item in list)
            {
                if (item.m_Class?.Length > 0)
                {
                    CommonTool.Append(item.m_Class, archetype.ToReference<BlueprintCharacterClassReference>());
                }
            }

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string BardicPerformance = "LionBlade.BardicPerformance";
        public static readonly string BardicPerformanceGuid = "{5BA6961C-2228-4DDB-A46C-7A095C2D5677}";

        internal const string BardicPerformanceDisplayName = "LionBladeBardicPerformance.Name";
        private const string BardicPerformanceDescription = "LionBladeBardicPerformance.Description";

        public static BlueprintProgression BardicPerformanceFeat()
        {
            var icon = FeatureRefs.InspireCompetenceFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(BardicPerformance, BardicPerformanceGuid)
              .SetDisplayName(BardicPerformanceDisplayName)
              .SetDescription(BardicPerformanceDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddToClasses(CharacterClassRefs.BardClass.ToString())
              .AddToClasses(ArchetypeGuid)
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.BardicPerformanceResource.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, true))
              .AddToLevelEntry(1, FeatureRefs.BardicPerformanceResourceFact.ToString())
              .AddToLevelEntry(7, FeatureRefs.BardMovePerformance.ToString())
              .AddToLevelEntry(13, FeatureRefs.BardSwiftPerformance.ToString())
              .Configure();
        }

        private const string InspirePoise = "LionBladeInspirePoise";
        private static readonly string InspirePoiseGuid = "{37B8ABCD-4064-4C40-A26D-F4809B8BDB40}";

        internal const string InspirePoiseDisplayName = "LionBladeInspirePoise.Name";
        private const string InspirePoiseDescription = "LionBladeInspirePoise.Description";
        public static BlueprintFeature InspirePoiseFeature()
        {
            var icon = AbilityRefs.EuphoricTranquilityCast.Reference.Get().Icon;
            return FeatureConfigurator.New(InspirePoise, InspirePoiseGuid)
              .SetDisplayName(InspirePoiseDisplayName)
              .SetDescription(InspirePoiseDescription)
              .SetIcon(icon)
              .AddBuffAllSkillsBonusAbilityValue(ModifierDescriptor.Competence, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(InspirePoiseGuid).WithBonusValueProgression(0, true))
              .SetRanks(10)
              .Configure();
        }

        private const string CloudMind = "LionBladeCloudMind";
        public static readonly string CloudMindGuid = "{A7C73185-C581-4EDC-8A95-EEE3D4369830}";

        internal const string CloudMindDisplayName = "LionBladeCloudMind.Name";
        private const string CloudMindDescription = "LionBladeCloudMind.Description";
        public static BlueprintFeature CloudMindFeature()
        {
            var icon = AbilityRefs.MindFog.Reference.Get().Icon;
            return FeatureConfigurator.New(CloudMind, CloudMindGuid)
              .SetDisplayName(CloudMindDisplayName)
              .SetDescription(CloudMindDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Insight, false, StatType.SkillStealth, 5)
              .AddFacts(new() { FeatureRefs.DivinationImmunityFeature.ToString() })
              .Configure();
        }

        private const string SilentSoul = "LionBladeSilentSoul";
        private static readonly string SilentSoulGuid = "{8F05CF79-26E2-4741-96B4-AFE8FD2A57BA}";

        internal const string SilentSoulDisplayName = "LionBladeSilentSoul.Name";
        private const string SilentSoulDescription = "LionBladeSilentSoul.Description";
        public static BlueprintFeature SilentSoulFeature()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(SilentSoul, SilentSoulGuid)
              .SetDisplayName(SilentSoulDisplayName)
              .SetDescription(SilentSoulDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.SkillStealth, 10)
              .AddSpellResistanceAgainstSpellDescriptor(spellDescriptor: SpellDescriptor.MindAffecting, value: 20)
              .Configure();
        }

        private static readonly string NarrowMissName = "LionBladeNarrowMiss";
        public static readonly string NarrowMissGuid = "{6316C32A-BD50-4A18-BC96-CDEDF9BEDA10}";

        private static readonly string NarrowMissDisplayName = "LionBladeNarrowMiss.Name";
        private static readonly string NarrowMissDescription = "LionBladeNarrowMiss.Description";

        private const string NarrowMissAbility = "NarrowMiss.NarrowMissAbility";
        private static readonly string NarrowMissAbilityGuid = "{5A16E581-674C-4EB5-855D-FB91480023B3}";

        private const string NarrowMissBuff = "NarrowMiss.NarrowMissBuff";
        private static readonly string NarrowMissBuffGuid = "{82225953-51DA-4850-A1C4-2F8714EEB3CD}";

        private const string NarrowMissAblityRes = "LionBlade.NarrowMissRes";
        public static readonly string NarrowMissAblityResGuid = "{92FD5B7B-9BA8-4D4A-B9BF-23182F894090}";
        public static BlueprintFeature NarrowMissConfigure()
        {
            var icon = AbilityRefs.Displacement.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(NarrowMissAblityRes, NarrowMissAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1))
                .SetUseMax()
                .SetMax(5)
                .Configure();

            var buff = BuffConfigurator.New(NarrowMissBuff, NarrowMissBuffGuid)
                .SetDisplayName(NarrowMissDisplayName)
                .SetDescription(NarrowMissDescription)
                .SetIcon(icon)
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.WindsOfVengenance)
                .Configure();

            var ability = AbilityConfigurator.New(NarrowMissAbility, NarrowMissAbilityGuid)
                .SetDisplayName(NarrowMissDisplayName)
                .SetDescription(NarrowMissDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(NarrowMissName, NarrowMissGuid)
                    .SetDisplayName(NarrowMissDisplayName)
                    .SetDescription(NarrowMissDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private static readonly string MisfortuneName = "LionBladeMisfortune";
        public static readonly string MisfortuneGuid = "{BF8469EB-FBE5-4ABA-9393-2E7B168D9CA0}";

        private static readonly string MisfortuneDisplayName = "LionBladeMisfortune.Name";
        private static readonly string MisfortuneDescription = "LionBladeMisfortune.Description";

        private const string MisfortuneAbility = "Misfortune.MisfortuneAbility";
        private static readonly string MisfortuneAbilityGuid = "{C14D3F23-7746-45BC-A65F-8683362FEF1F}";

        private const string MisfortuneBuff = "Misfortune.MisfortuneBuff";
        private static readonly string MisfortuneBuffGuid = "{9A6EF146-F8D3-4852-BA56-33CAB76CBD52}";
        public static BlueprintFeature MisfortuneConfigure()
        {
            var icon = FeatureRefs.DirgeOfDoomFeature.Reference.Get().Icon;
            var fx = AbilityRefs.PredictionOfFailure.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff = BuffConfigurator.New(MisfortuneBuff, MisfortuneBuffGuid)
                .SetDisplayName(MisfortuneDisplayName)
                .SetDescription(MisfortuneDescription)
                .SetIcon(icon)
                .AddModifyD20(ActionsBuilder.New().RemoveSelf().Build(), rule: RuleType.SavingThrow, rollsAmount: 1, addSavingThrowBonus: true, value: -2, bonusDescriptor: ModifierDescriptor.Penalty)
                .AddSpellDescriptorComponent(SpellDescriptor.GazeAttack | SpellDescriptor.MindAffecting)
                .Configure();

            var ability = AbilityConfigurator.New(MisfortuneAbility, MisfortuneAbilityGuid)
                .SetDisplayName(MisfortuneDisplayName)
                .SetDescription(MisfortuneDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .AllowTargeting(false, true, false, false)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Close)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddSpellDescriptorComponent(SpellDescriptor.GazeAttack | SpellDescriptor.MindAffecting)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(MisfortuneName, MisfortuneGuid)
                    .SetDisplayName(MisfortuneDisplayName)
                    .SetDescription(MisfortuneDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private const string PerfectSurprise = "LionBladePerfectSurprise";
        private static readonly string PerfectSurpriseGuid = "{3E57F3EE-8C79-4896-AAD1-358018763EBD}";

        private const string PerfectSurprisePro = "LionBladePerfectSurprisePro";
        private static readonly string PerfectSurpriseProGuid = "{669E26D8-0AFC-491B-8C98-4EC9F9119D26}";

        internal const string PerfectSurpriseDisplayName = "LionBladePerfectSurprise.Name";
        private const string PerfectSurpriseDescription = "LionBladePerfectSurprise.Description";

        private const string PerfectSurpriseBuff = "PerfectSurprise.PerfectSurpriseBuff";
        private static readonly string PerfectSurpriseBuffGuid = "{C49585B9-F617-4936-818D-F36CABFA457D}";
        public static BlueprintFeature PerfectSurpriseFeature()
        {
            var icon = FeatureRefs.InspireGreatnessFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(PerfectSurpriseBuff, PerfectSurpriseBuffGuid)
                .SetDisplayName(PerfectSurpriseDisplayName)
                .SetDescription(PerfectSurpriseDescription)
                .SetIcon(icon)
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            UnitPropertyConfigurator.New(PerfectSurprisePro, PerfectSurpriseProGuid)
                        .AddClassLevelGetter(clazz: ArchetypeGuid)
                        .AddSimplePropertyGetter(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusIntelligence)
                        .SetBaseValue(10)
                        .Configure();

            var action = ActionsBuilder.New()
                        .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(AbilityRankType.DamageDice),
                            onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New()
                                .ApplyBuff(BuffRefs.Unconsious.ToString(), ContextDuration.Variable(ContextValues.Rank(AbilityRankType.DamageBonus)))
                                .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days))
                                .Build(), succeed: ActionsBuilder.New()
                                .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days))
                                .Build())
                            .Build())
                        .Build();

            var action2 = ActionsBuilder.New()
                            .Conditional(ConditionsBuilder.New().HasFact(buff).Build(), ifFalse: action)
                            .Build();

            return FeatureConfigurator.New(PerfectSurprise, PerfectSurpriseGuid)
              .SetDisplayName(PerfectSurpriseDisplayName)
              .SetDescription(PerfectSurpriseDescription)
              .SetIcon(icon)
              .AddRecalculateOnStatChange(stat: StatType.Intelligence)
              .AddContextRankConfig(ContextRankConfigs.CustomProperty(PerfectSurpriseProGuid, AbilityRankType.DamageDice))
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, false, AbilityRankType.DamageBonus))
              .AddInitiatorAttackWithWeaponTrigger(action2, onCharge: true, onlyHit: true, onlySneakAttack: true)
              .Configure();
        }
        
    }
}
