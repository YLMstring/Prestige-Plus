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
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.CustomComponent.PrestigeClass;

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
                .AddToLevelEntry(1, EnchantingTouchFeature(), MasterSpyFeat(), SpellbookReplace.spellupgradeGuid)
                .AddToLevelEntry(2, SeductiveIntuitionFeature())
                .AddToLevelEntry(3, HiddenSpellFeature())
                .AddToLevelEntry(4, SeductiveIntuitionGuid)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6, SeductiveIntuitionGuid, HiddenSpellGuid)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8, SeductiveIntuitionGuid)
                .AddToLevelEntry(9, HiddenSpellGuid)
                .AddToLevelEntry(10, SeductiveIntuitionGuid, EcstasyConfigure())
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

            FakeAlignedClass.AddtoMenu(archetype);
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
              //.AddPrerequisiteFeaturesFromList(new() { FeatureRefs.Deceitful.ToString(), FeatureRefs.IronWill.ToString() }, 2)
              .AddPrerequisiteFeature(FeatureRefs.Deceitful.ToString())
              .AddPrerequisiteFeature(FeatureRefs.IronWill.ToString())
              .SetIsClassFeature(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(2, MaskAlignmentFeature())
              .AddToLevelEntry(4, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(5, FeatureRefs.SlipperyMind.ToString())
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
              .AddComponent<HiddenSpellComp>()
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

        private const string EnchantingTouch = "EnchantingCourtesanEnchantingTouch";
        public static readonly string EnchantingTouchGuid = "{EBB40FD1-AB07-4894-B691-4E85CB90DC96}";

        internal const string EnchantingTouchDisplayName = "EnchantingCourtesanEnchantingTouch.Name";
        private const string EnchantingTouchDescription = "EnchantingCourtesanEnchantingTouch.Description";
        public static BlueprintFeature EnchantingTouchFeature()
        {
            var icon = AbilityRefs.MindBlankCommunal.Reference.Get().Icon;
            return FeatureConfigurator.New(EnchantingTouch, EnchantingTouchGuid)
              .SetDisplayName(EnchantingTouchDisplayName)
              .SetDescription(EnchantingTouchDescription)
              .SetIcon(icon)
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

        private static readonly string EcstasyName = "EnchantingCourtesanEcstasy";
        public static readonly string EcstasyGuid = "{49AC4C66-95CD-45AA-B923-0CE563C2CFB5}";

        private static readonly string EcstasyDisplayName = "EnchantingCourtesanEcstasy.Name";
        private static readonly string EcstasyDescription = "EnchantingCourtesanEcstasy.Description";

        private static readonly string Ecstasy1DisplayName = "EnchantingCourtesanEcstasy1.Name";
        private static readonly string Ecstasy1Description = "EnchantingCourtesanEcstasy1.Description";

        private static readonly string Ecstasy2DisplayName = "EnchantingCourtesanEcstasy2.Name";
        private static readonly string Ecstasy2Description = "EnchantingCourtesanEcstasy2.Description";

        private const string EcstasyAbility = "EnchantingCourtesan.EcstasyAbility";
        private static readonly string EcstasyAbilityGuid = "{19DA9656-42E1-4572-98F5-722060B5ECB9}";

        private const string EcstasyBuff = "EnchantingCourtesan.EcstasyBuff";
        private static readonly string EcstasyBuffGuid = "{ED5C140F-9C2C-4119-8427-AFC5C7FD3675}";

        private const string EcstasyBuff2 = "EnchantingCourtesan.EcstasyBuff2";
        private static readonly string EcstasyBuff2Guid = "{1C21BC0A-7FE4-4630-81AB-F6F766E4A666}";
        public static BlueprintFeature EcstasyConfigure()
        {
            var icon = AbilityRefs.JoyOfLife.Reference.Get().Icon;
            var fx = AbilityRefs.JoyfulRapture.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff1 = BuffConfigurator.New(EcstasyBuff, EcstasyBuffGuid)
                .SetDisplayName(Ecstasy1DisplayName)
                .SetDescription(Ecstasy1Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffsTTT>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; c.Continuous = true; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Stunned.ToString() })
                .Configure();

            var buff2 = BuffConfigurator.New(EcstasyBuff2, EcstasyBuff2Guid)
                .SetDisplayName(Ecstasy2DisplayName)
                .SetDescription(Ecstasy2Description)
                .SetIcon(icon)
                .AddComponent<SuppressBuffsTTT>(c => { c.Descriptor = SpellDescriptor.NegativeEmotion; c.Continuous = true; })
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .AddFacts(new() { BuffRefs.Staggered.ToString() })
                .Configure();

            var shoot = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(), useDCFromContextSavingThrow: true,
            onResult: ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New().ApplyBuff(buff1, ContextDuration.FixedDice(DiceType.D4)).Build(),
                                    succeed: ActionsBuilder.New().ApplyBuff(buff2, ContextDuration.FixedDice(DiceType.D4)).Build())
                        .Build())
                    .Build();

            var ability = AbilityConfigurator.New(EcstasyAbility, EcstasyAbilityGuid)
                .SetDisplayName(EcstasyDisplayName)
                .SetDescription(EcstasyDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .AddComponent<AbilityTargetMayPrecise>()
                .AllowTargeting(false, true, true, true)
                .AddAbilityDeliverTouch(false, null, ComponentMerge.Fail, ItemWeaponRefs.TouchItem.ToString())
                .AddAbilityEffectRunAction(shoot)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Wisdom).WithBonusValueProgression(20, false))
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Touch)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Emotion)
                .Configure();

            return FeatureConfigurator.New(EcstasyName, EcstasyGuid)
                    .SetDisplayName(EcstasyDisplayName)
                    .SetDescription(EcstasyDescription)
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
