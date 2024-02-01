using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Root;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.Modify;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class EnchantingCourtesan
    {
        private const string ArchetypeName = "EnchantingCourtesan";
        public static readonly string ArchetypeGuid = "{5EFD5A02-D3F0-4A1E-9E3A-D9C1368DB3FA}";
        internal const string ArchetypeDisplayName = "EnchantingCourtesan.Name";
        private const string ArchetypeDescription = "EnchantingCourtesan.Description";

        private const string ClassProgressName = "EnchantingCourtesanPrestige";
        private static readonly string ClassProgressGuid = "{9B060568-C3B7-4970-BDC3-69B934F8A2EA}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, MasterSpyFeat(), SpellbookReplace.spellupgradeGuid)
                .AddToLevelEntry(2, SeductiveIntuitionFeature())
                .AddToLevelEntry(3, HiddenSpellFeature())
                .AddToLevelEntry(4, SeductiveIntuitionGuid)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, SeductiveIntuitionGuid, HiddenSpellGuid)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8, SeductiveIntuitionGuid)
                .AddToLevelEntry(9, HiddenSpellGuid)
                .AddToLevelEntry(10, SeductiveIntuitionGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] {  }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
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
                .SetHitDie(DiceType.D6)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABLow.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillMobility, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillKnowledgeWorld, StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteParametrizedSpellSchoolFeature(ParametrizedFeatureRefs.SpellFocus.ToString(), SpellSchool.Divination, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteParametrizedSpellSchoolFeature(ParametrizedFeatureRefs.SpellFocus.ToString(), SpellSchool.Enchantment, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 5)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 2)
                .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 2)
                .AddComponent<PrerequisiteCasterLevel>(c => { c.RequiredCasterLevel = 2; })
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var EnchantingCourtesanref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = EnchantingCourtesanref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string MasterSpy = "EnchantingCourtesan.MasterSpy";
        public static readonly string MasterSpyGuid = "{C07AE528-7C26-40D9-9573-D0C351760E4C}";

        internal const string MasterSpyDisplayName = "EnchantingCourtesanMasterSpy.Name";
        private const string MasterSpyDescription = "EnchantingCourtesanMasterSpy.Description";

        public static BlueprintProgression MasterSpyFeat()
        {
            var icon = FeatureRefs.InspireCompetenceFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(MasterSpy, MasterSpyGuid)
              .SetDisplayName(MasterSpyDisplayName)
              .SetDescription(MasterSpyDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeaturesFromList(new() { FeatureRefs.Deceitful.ToString(), FeatureRefs.IronWill.ToString() }, 1)
              .SetIsClassFeature(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(2, MaskAlignmentFeature())
              .AddToLevelEntry(4, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(7, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(9, HiddenMindFeature())
              .AddToLevelEntry(10, FeatureRefs.SneakAttack.ToString())
              .Configure();
        }

        private const string SeductiveIntuition = "EnchantingCourtesanSeductiveIntuition";
        private static readonly string SeductiveIntuitionGuid = "{408692DA-8EFD-4A49-BA7E-51578894B5E5}";

        internal const string SeductiveIntuitionDisplayName = "EnchantingCourtesanSeductiveIntuition.Name";
        private const string SeductiveIntuitionDescription = "EnchantingCourtesanSeductiveIntuition.Description";
        public static BlueprintFeature SeductiveIntuitionFeature()
        {
            var icon = AbilityRefs.EuphoricTranquilityCast.Reference.Get().Icon;
            return FeatureConfigurator.New(SeductiveIntuition, SeductiveIntuitionGuid)
              .SetDisplayName(SeductiveIntuitionDisplayName)
              .SetDescription(SeductiveIntuitionDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.CheckBluff, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.CheckDiplomacy, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.SkillThievery, ContextValues.Rank(), ModifierDescriptor.Competence)
              .AddBuffAllSkillsBonusAbilityValue(ModifierDescriptor.Competence, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(SeductiveIntuitionGuid))
              .SetRanks(10)
              .Configure();
        }

        private const string HiddenSpell = "EnchantingCourtesanHiddenSpell";
        private static readonly string HiddenSpellGuid = "{89EFD710-16C2-43E3-B075-BCEAA6BB6C23}";

        internal const string HiddenSpellDisplayName = "EnchantingCourtesanHiddenSpell.Name";
        private const string HiddenSpellDescription = "EnchantingCourtesanHiddenSpell.Description";
        public static BlueprintFeature HiddenSpellFeature()
        {
            var icon = AbilityRefs.EuphoricTranquilityCast.Reference.Get().Icon;
            return FeatureConfigurator.New(HiddenSpell, HiddenSpellGuid)
              .SetDisplayName(HiddenSpellDisplayName)
              .SetDescription(HiddenSpellDescription)
              .SetIcon(icon)

              .SetRanks(10)
              .Configure();
        }

        private const string HiddenMind = "EnchantingCourtesanHiddenMind";
        public static readonly string HiddenMindGuid = "{01661653-5877-4A6B-B4DF-EE8D9C26AD47}";

        internal const string HiddenMindDisplayName = "EnchantingCourtesanHiddenMind.Name";
        private const string HiddenMindDescription = "EnchantingCourtesanHiddenMind.Description";
        public static BlueprintFeature HiddenMindFeature()
        {
            var icon = AbilityRefs.MindBlankCommunal.Reference.Get().Icon;
            return FeatureConfigurator.New(HiddenMind, HiddenMindGuid)
              .SetDisplayName(HiddenMindDisplayName)
              .SetDescription(HiddenMindDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(8, modifierDescriptor: ModifierDescriptor.Resistance, spellDescriptor: SpellDescriptor.MindAffecting)
              .AddFacts(new() { FeatureRefs.DivinationImmunityFeature.ToString() })
              .Configure();
        }

        private const string MaskAlignment = "EnchantingCourtesanMaskAlignment";
        private static readonly string MaskAlignmentGuid = "{EE2BF850-589D-49A1-98FB-2B11D0920860}";

        internal const string MaskAlignmentDisplayName = "EnchantingCourtesanMaskAlignment.Name";
        private const string MaskAlignmentDescription = "EnchantingCourtesanMaskAlignment.Description";
        public static BlueprintFeature MaskAlignmentFeature()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(MaskAlignment, MaskAlignmentGuid)
              .SetDisplayName(MaskAlignmentDisplayName)
              .SetDescription(MaskAlignmentDescription)
              .SetIcon(icon)
              .AddUndetectableAlignment()
              .Configure();
        }

        private static readonly string NarrowMissName = "EnchantingCourtesanNarrowMiss";
        public static readonly string NarrowMissGuid = "{6316C32A-BD50-4A18-BC96-CDEDF9BEDA10}";

        private static readonly string NarrowMissDisplayName = "EnchantingCourtesanNarrowMiss.Name";
        private static readonly string NarrowMissDescription = "EnchantingCourtesanNarrowMiss.Description";

        private const string NarrowMissAbility = "NarrowMiss.NarrowMissAbility";
        private static readonly string NarrowMissAbilityGuid = "{5A16E581-674C-4EB5-855D-FB91480023B3}";

        private const string NarrowMissBuff = "NarrowMiss.NarrowMissBuff";
        private static readonly string NarrowMissBuffGuid = "{82225953-51DA-4850-A1C4-2F8714EEB3CD}";

        private const string NarrowMissAblityRes = "EnchantingCourtesan.NarrowMissRes";
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
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.Blur)
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

        private static readonly string MisfortuneName = "EnchantingCourtesanMisfortune";
        public static readonly string MisfortuneGuid = "{BF8469EB-FBE5-4ABA-9393-2E7B168D9CA0}";

        private static readonly string MisfortuneDisplayName = "EnchantingCourtesanMisfortune.Name";
        private static readonly string MisfortuneDescription = "EnchantingCourtesanMisfortune.Description";

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
                .AddModifyD20(ActionsBuilder.New().RemoveSelf().Build(), rule: RuleType.SavingThrow, rollsAmount: 1, rerollOnlyIfSuccess: true, addSavingThrowBonus: true, value: -2, bonusDescriptor: ModifierDescriptor.Penalty)
                .AddSpellDescriptorComponent(SpellDescriptor.GazeAttack)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
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
                .AddSpellDescriptorComponent(SpellDescriptor.GazeAttack)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(MisfortuneName, MisfortuneGuid)
                    .SetDisplayName(MisfortuneDisplayName)
                    .SetDescription(MisfortuneDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private const string PerfectSurprise = "EnchantingCourtesanPerfectSurprise";
        private static readonly string PerfectSurpriseGuid = "{3E57F3EE-8C79-4896-AAD1-358018763EBD}";

        private const string PerfectSurprisePro = "EnchantingCourtesanPerfectSurprisePro";
        private static readonly string PerfectSurpriseProGuid = "{669E26D8-0AFC-491B-8C98-4EC9F9119D26}";

        internal const string PerfectSurpriseDisplayName = "EnchantingCourtesanPerfectSurprise.Name";
        private const string PerfectSurpriseDescription = "EnchantingCourtesanPerfectSurprise.Description";

        private const string PerfectSurpriseBuff = "PerfectSurprise.PerfectSurpriseBuff";
        private static readonly string PerfectSurpriseBuffGuid = "{C49585B9-F617-4936-818D-F36CABFA457D}";
        public static BlueprintFeature PerfectSurpriseFeature()
        {
            var icon = FeatureRefs.InspireGreatnessFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(PerfectSurpriseBuff, PerfectSurpriseBuffGuid)
                .SetDisplayName(PerfectSurpriseDisplayName)
                .SetDescription(PerfectSurpriseDescription)
                .SetIcon(icon)
                //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
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
              .AddContextRankConfig(ContextRankConfigs.CustomProperty(PerfectSurpriseProGuid, AbilityRankType.DamageDice))
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, false, AbilityRankType.DamageBonus))
              .AddInitiatorAttackWithWeaponTrigger(action2, onCharge: true, onlyHit: true, onlySneakAttack: true)
              .Configure();
        }
    }
}
