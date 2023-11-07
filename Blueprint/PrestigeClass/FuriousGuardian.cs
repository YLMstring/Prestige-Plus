using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class FuriousGuardian
    {
        private const string ArchetypeName = "FuriousGuardian";
        public static readonly string ArchetypeGuid = "{D7381124-3BD3-4D63-BBB8-AF778DAA372D}";
        internal const string ArchetypeDisplayName = "FuriousGuardian.Name";
        private const string ArchetypeDescription = "FuriousGuardian.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "FuriousGuardianPrestige";
        private static readonly string ClassProgressGuid = "{B803064F-15FA-4450-BD88-48A82076246F}";

        public static void Configure()
        {
            DedicationsFeat(); 
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, ChosenAllyFeat(), DedicationsGuid, SAGuardianRage())
            .AddToLevelEntry(2, SAGuardedThoughts())
            .AddToLevelEntry(3, DedicationsGuid)
            .AddToLevelEntry(4, AdaptableGuardian4Feat())
            .AddToLevelEntry(5, DedicationsGuid)
            .AddToLevelEntry(6, TightFollowerFeat())
            .AddToLevelEntry(7, DedicationsGuid)
            .AddToLevelEntry(8, AdaptableGuardian8Feat(), ReactiveStrikeFeat())
            .AddToLevelEntry(9, DedicationsGuid)
            .AddToLevelEntry(10, SAUnbreakableDefender())
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
            .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 2)
            .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
            .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var FuriousGuardianref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = FuriousGuardianref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Dedications = "FuriousGuardian.Dedications";
        private static readonly string DedicationsGuid = "{553B034F-59BF-4FB9-8981-039574453903}";

        internal const string DedicationsDisplayName = "GuardianDedications.Name";
        private const string DedicationsDescription = "GuardianDedications.Description";

        public static void DedicationsFeat()
        {
            var icon = AbilityRefs.HeroicInvocation.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(Dedications, DedicationsGuid)
              .SetDisplayName(DedicationsDisplayName)
              .SetDescription(DedicationsDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(SAClothedCivilization())
              .AddToAllFeatures(DeflectArrowsFeat())
              .AddToAllFeatures(FormalTrainingFeat())
              .AddToAllFeatures(SAGreaterRage())
              .AddToAllFeatures(RagePowerFeat())
              .AddToAllFeatures(RagePower2Feat())
              .AddToAllFeatures(RagePower3Feat())
              .AddToAllFeatures(FeatureSelectionRefs.TeamworkFeat.ToString())
              .AddToAllFeatures(SAUncannyDodge1())
              .AddToAllFeatures(SAUncannyDodge2())
              .AddToAllFeatures(UncannyDodge3Feat())
              .Configure();
        }

        private const string ClothedCivilization = "FuriousGuardian.ClothedCivilization";
        private static readonly string ClothedCivilizationGuid = "{CFB68B78-0BC8-46AD-AAF4-1FA5555D8D13}";

        internal const string FuriousGuardianClothedCivilizationDisplayName = "FuriousGuardianClothedCivilization.Name";
        private const string FuriousGuardianClothedCivilizationDescription = "FuriousGuardianClothedCivilization.Description";
        public static BlueprintFeature SAClothedCivilization()
        {
            var icon = FeatureRefs.ArmorMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(ClothedCivilization, ClothedCivilizationGuid)
              .SetDisplayName(FuriousGuardianClothedCivilizationDisplayName)
              .SetDescription(FuriousGuardianClothedCivilizationDescription)
              .SetIcon(icon)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 5)
              .AddPrerequisiteFeature(FeatureRefs.FastMovement.ToString())
              .AddSpeedBonusInArmorCategory(10, new Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup[] { Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.Heavy }, Kingmaker.Enums.ModifierDescriptor.UntypedStackable, false)
              .AddFacts(new() { FeatureRefs.HeavyArmorProficiency.ToString() })
              .AddContextStatBonus(stat: StatType.CheckDiplomacy, value: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string DeflectArrows = "FuriousGuardian.DeflectArrows";
        public static readonly string DeflectArrowsGuid = "{F67A55D9-8458-4E47-97B6-757F904D33EE}";

        private const string DeflectArrowsBuff = "FuriousGuardian.DeflectArrowsBuff";
        public static readonly string DeflectArrowsBuffGuid = "{B1B7D9E0-1776-4D49-A600-BB6B2EAB031E}";

        internal const string DeflectArrowsDisplayName = "FuriousGuardianDeflectArrows.Name";
        private const string DeflectArrowsDescription = "FuriousGuardianDeflectArrows.Description";

        public static BlueprintFeature DeflectArrowsFeat()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            BuffConfigurator.New(DeflectArrowsBuff, DeflectArrowsBuffGuid)
             .SetDisplayName(DeflectArrowsDisplayName)
             .SetDescription(DeflectArrowsDescription)
             .SetIcon(icon)
             .AddDeflectArrows(restriction: Kingmaker.UnitLogic.FactLogic.DeflectArrows.RestrictionType.EmptyHand)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(DeflectArrows, DeflectArrowsGuid)
              .SetDisplayName(DeflectArrowsDisplayName)
              .SetDescription(DeflectArrowsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.DeflectArrows.ToString() })
              .Configure();
        }

        private const string UncannyDodge3 = "FuriousGuardian.UncannyDodge3";
        public static readonly string UncannyDodge3Guid = "{7BA3D31B-9A8A-430C-83FB-808B44617349}";

        private const string UncannyDodge3Buff = "FuriousGuardian.UncannyDodge3Buff";
        public static readonly string UncannyDodge3BuffGuid = "{72BED8B4-5E6B-4CC0-BC95-73F4948FC2C5}";

        internal const string UncannyDodge3DisplayName = "FuriousGuardianUncannyDodge3.Name";
        private const string UncannyDodge3Description = "FuriousGuardianUncannyDodge3.Description";

        public static BlueprintFeature UncannyDodge3Feat()
        {
            var icon = FeatureRefs.UncannyDodge.Reference.Get().Icon;

            BuffConfigurator.New(UncannyDodge3Buff, UncannyDodge3BuffGuid)
             .SetDisplayName(UncannyDodge3DisplayName)
             .SetDescription(UncannyDodge3Description)
             .SetIcon(icon)
             .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.CannotBeFlanked)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(UncannyDodge3, UncannyDodge3Guid)
              .SetDisplayName(UncannyDodge3DisplayName)
              .SetDescription(UncannyDodge3Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUncannyDodge.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUncannyDodgeTalent.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .SetHideNotAvailibleInUI(true)
              .Configure();
        }

        private const string UncannyDodge2 = "FuriousGuardian.UncannyDodge2";
        private static readonly string UncannyDodge2Guid = "{C136C02B-5841-4007-86C6-945E120932A2}";

        internal const string FuriousGuardianUncannyDodge2DisplayName = "FuriousGuardianUncannyDodge2.Name";
        private const string FuriousGuardianUncannyDodge2Description = "FuriousGuardianUncannyDodge2.Description";
        public static BlueprintFeature SAUncannyDodge2()
        {
            var icon = FeatureRefs.UncannyDodge.Reference.Get().Icon;
            return FeatureConfigurator.New(UncannyDodge2, UncannyDodge2Guid)
              .SetDisplayName(FuriousGuardianUncannyDodge2DisplayName)
              .SetDescription(FuriousGuardianUncannyDodge2Description)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.ImprovedUncannyDodge.ToString() })
              .AddPrerequisiteFeature(FeatureRefs.UncannyDodge.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.UncannyDodgeTalent.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteNoFeature(FeatureRefs.ImprovedUncannyDodge.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.ImprovedUncannyDodgeTalent.ToString())
              .SetHideNotAvailibleInUI(true)
              .Configure();
        }

        private const string UncannyDodge1 = "FuriousGuardian.UncannyDodge1";
        private static readonly string UncannyDodge1Guid = "{09266838-FD48-4F12-8568-0761D22E90F6}";

        internal const string FuriousGuardianUncannyDodge1DisplayName = "FuriousGuardianUncannyDodge1.Name";
        private const string FuriousGuardianUncannyDodge1Description = "FuriousGuardianUncannyDodge1.Description";
        public static BlueprintFeature SAUncannyDodge1()
        {
            var icon = FeatureRefs.UncannyDodge.Reference.Get().Icon;
            return FeatureConfigurator.New(UncannyDodge1, UncannyDodge1Guid)
              .SetDisplayName(FuriousGuardianUncannyDodge1DisplayName)
              .SetDescription(FuriousGuardianUncannyDodge1Description)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.UncannyDodge.ToString() })
              .AddPrerequisiteNoFeature(FeatureRefs.UncannyDodge.ToString())
              .AddPrerequisiteNoFeature(FeatureRefs.UncannyDodgeTalent.ToString())
              .SetHideNotAvailibleInUI(true)
              .Configure();
        }

        private const string FormalTraining = "FuriousGuardian.FormalTraining";
        private static readonly string FormalTrainingGuid = "{C3977719-0C91-449C-9F69-B3DA14579FA0}";

        internal const string FormalTrainingDisplayName = "GuardianFormalTraining.Name";
        private const string FormalTrainingDescription = "GuardianFormalTraining.Description";

        public static BlueprintFeatureSelection FormalTrainingFeat()
        {
            var icon = FeatureRefs.EldritchKnightDiverseTraining.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(FormalTraining, FormalTrainingGuid)
                .CopyFrom(
                FeatureSelectionRefs.CombatTrick)
              .SetDisplayName(FormalTrainingDisplayName)
              .SetDescription(FormalTrainingDescription)
              .SetIcon(icon)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.FighterClass.ToString(), modifier: 1, summand: 0)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 3)
              .AddPrerequisiteNoFeature(FormalTrainingGuid)
              //.SetHideNotAvailibleInUI(true)
              .Configure(delayed: true);
        }

        private const string RagePower = "FuriousGuardian.RagePower";
        private static readonly string RagePowerGuid = "{7A4CEFF3-564F-4A6E-9072-A142ABBAC954}";

        internal const string RagePowerDisplayName = "GuardianRagePower.Name";
        private const string RagePowerDescription = "GuardianRagePower.Description";

        public static BlueprintFeatureSelection RagePowerFeat()
        {
            var icon = FeatureRefs.RenewedVigorFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(RagePower, RagePowerGuid)
                .CopyFrom(
                FeatureSelectionRefs.RagePowerSelection)
              .SetDisplayName(RagePowerDisplayName)
              .SetDescription(RagePowerDescription)
              .SetIcon(icon)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.BarbarianClass.ToString(), modifier: 1, summand: 0)
              .AddPrerequisiteNoFeature(RagePowerGuid)
              .SetHideNotAvailibleInUI(true)
              .Configure(delayed: true);
        }

        private const string RagePower2 = "FuriousGuardian.RagePower2";
        private static readonly string RagePower2Guid = "{3E9136F2-CCB4-45A3-8458-B9D04855A14A}";

        public static BlueprintFeatureSelection RagePower2Feat()
        {
            var icon = FeatureRefs.RenewedVigorFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(RagePower2, RagePower2Guid)
                .CopyFrom(
                FeatureSelectionRefs.RagePowerSelection)
              .SetDisplayName(RagePowerDisplayName)
              .SetDescription(RagePowerDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(RagePowerGuid)
              .AddPrerequisiteNoFeature(RagePower2Guid)
              .SetHideNotAvailibleInUI(true)
              .Configure(delayed: true);
        }

        private const string RagePower3 = "FuriousGuardian.RagePower3";
        private static readonly string RagePower3Guid = "{901C41B3-FE67-44DD-9AC6-2955E98940D0}";

        public static BlueprintFeatureSelection RagePower3Feat()
        {
            var icon = FeatureRefs.RenewedVigorFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(RagePower3, RagePower3Guid)
                .CopyFrom(
                FeatureSelectionRefs.RagePowerSelection)
              .SetDisplayName(RagePowerDisplayName)
              .SetDescription(RagePowerDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(RagePower2Guid)
              .AddPrerequisiteNoFeature(RagePower3Guid)
              .SetHideNotAvailibleInUI(true)
              .Configure(delayed: true);
        }

        private const string GreaterRage = "FuriousGuardian.GreaterRage";
        private static readonly string GreaterRageGuid = "{F0D055C1-D0EE-4AB7-B1C7-15C89AA4F13F}";

        internal const string FuriousGuardianGreaterRageDisplayName = "FuriousGuardianGreaterRage.Name";
        private const string FuriousGuardianGreaterRageDescription = "FuriousGuardianGreaterRage.Description";
        public static BlueprintFeature SAGreaterRage()
        {
            var icon = FeatureRefs.GreaterRageFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(GreaterRage, GreaterRageGuid)
              .SetDisplayName(FuriousGuardianGreaterRageDisplayName)
              .SetDescription(FuriousGuardianGreaterRageDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.GreaterRageFeature.ToString() })
              .AddPrerequisiteClassLevel(ArchetypeGuid, 7)
              .Configure();
        }

        private const string GuardianRage = "FuriousGuardian.GuardianRage";
        private static readonly string GuardianRageGuid = "{39473A62-E2D6-4E6F-8DB2-3D3CC4F92EA8}";

        internal const string FuriousGuardianGuardianRageDisplayName = "FuriousGuardianGuardianRage.Name";
        private const string FuriousGuardianGuardianRageDescription = "FuriousGuardianGuardianRage.Description";
        public static BlueprintFeature SAGuardianRage()
        {
            var icon = FeatureRefs.RageFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(GuardianRage, GuardianRageGuid)
              .SetDisplayName(FuriousGuardianGuardianRageDisplayName)
              .SetDescription(FuriousGuardianGuardianRageDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.RageFeature.ToString() })
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.RageResourse.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, true))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string ChosenAlly = "FuriousGuardian.ChosenAlly";
        public static readonly string ChosenAllyGuid = "{B1D76188-4010-4B24-90FD-23B9E698EBDF}";

        private const string ChosenAllyAblity2 = "FuriousGuardian.UseChosenAlly2";
        public static readonly string ChosenAllyAblity2Guid = "{FD19A84E-64E6-416C-A037-101406E49073}";

        private const string ChosenAllyBuff = "FuriousGuardian.ChosenAllyBuff";
        public static readonly string ChosenAllyBuffGuid = "{B762ABF7-C98E-4A70-AB4C-BBDFA0571CC2}";

        internal const string ChosenAllyDisplayName = "FuriousGuardianChosenAlly.Name";
        private const string ChosenAllyDescription = "FuriousGuardianChosenAlly.Description";

        public static BlueprintFeature ChosenAllyFeat()
        {
            var icon = AbilityRefs.DivineGuardianTrothAbility.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(DeflectArrowsGuid),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuff(DeflectArrowsBuffGuid, ContextDuration.Fixed(1))
                        .Build())
                    .Conditional(ConditionsBuilder.New().CasterHasFact(UncannyDodge3Guid),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuff(UncannyDodge3BuffGuid, ContextDuration.Fixed(1))
                        .Build())
                    .Build();

            var Buff1 = BuffConfigurator.New(ChosenAllyBuff, ChosenAllyBuffGuid)
             .SetDisplayName(ChosenAllyDisplayName)
             .SetDescription(ChosenAllyDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .AddComponent<ChosenAllyTarget>()
             .AddNewRoundTrigger(newRoundActions: action)
             .AddCombatStateTrigger(combatStartActions: action)
             .Configure();

            var ability2 = AbilityConfigurator.New(ChosenAllyAblity2, ChosenAllyAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build())
                    .Build())
                .SetDisplayName(ChosenAllyDisplayName)
                .SetDescription(ChosenAllyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(ChosenAlly, ChosenAllyGuid)
              .SetDisplayName(ChosenAllyDisplayName)
              .SetDescription(ChosenAllyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .AddComponent<ChosenAllyCaster>()
              .Configure();
        }

        private const string UnbreakableDefender = "FuriousGuardian.UnbreakableDefender";
        private static readonly string UnbreakableDefenderGuid = "{7FF8AAB3-E55C-4B51-8DA0-A0AD7C921180}";

        internal const string FuriousGuardianUnbreakableDefenderDisplayName = "FuriousGuardianUnbreakableDefender.Name";
        private const string FuriousGuardianUnbreakableDefenderDescription = "FuriousGuardianUnbreakableDefender.Description";
        public static BlueprintFeature SAUnbreakableDefender()
        {
            var icon = FeatureRefs.IndomitableWill.Reference.Get().Icon;
            return FeatureConfigurator.New(UnbreakableDefender, UnbreakableDefenderGuid)
              .SetDisplayName(FuriousGuardianUnbreakableDefenderDisplayName)
              .SetDescription(FuriousGuardianUnbreakableDefenderDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.Diehard.ToString() })
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Fatigued)
              .Configure();
        }

        private const string GuardedThoughts = "FuriousGuardian.GuardedThoughts";
        private static readonly string GuardedThoughtsGuid = "{8C298FEE-312C-4C7E-B222-0CFCBC0A1FFB}";

        internal const string FuriousGuardianGuardedThoughtsDisplayName = "FuriousGuardianGuardedThoughts.Name";
        private const string FuriousGuardianGuardedThoughtsDescription = "FuriousGuardianGuardedThoughts.Description";
        public static BlueprintFeature SAGuardedThoughts()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(GuardedThoughts, GuardedThoughtsGuid)
              .SetDisplayName(FuriousGuardianGuardedThoughtsDisplayName)
              .SetDescription(FuriousGuardianGuardedThoughtsDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(5, spellDescriptor: SpellDescriptor.Charm, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Circumstance)
              .AddSavingThrowBonusAgainstDescriptor(5, spellDescriptor: SpellDescriptor.Compulsion, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Circumstance)
              .Configure();
        }

        private const string AdaptableGuardian4 = "FuriousGuardian.AdaptableGuardian4";
        public static readonly string AdaptableGuardian4Guid = "{02C47D70-7DE6-44F9-A7A9-5E1DAFCF73D9}";

        private const string AdaptableGuardian4Ablity2 = "FuriousGuardian.UseAdaptableGuardian42";
        public static readonly string AdaptableGuardian4Ablity2Guid = "{61044A2A-415B-479E-B9A2-B1FAD2637920}";

        internal const string AdaptableGuardian4DisplayName = "FuriousGuardianAdaptableGuardian4.Name";
        private const string AdaptableGuardian4Description = "FuriousGuardianAdaptableGuardian4.Description";

        public static BlueprintFeature AdaptableGuardian4Feat()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(AdaptableGuardian4Ablity2, AdaptableGuardian4Ablity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent(ChosenAllyBuffGuid)
                        .Build())
                    .Build())
                .SetDisplayName(AdaptableGuardian4DisplayName)
                .SetDescription(AdaptableGuardian4Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(AdaptableGuardian4, AdaptableGuardian4Guid)
              .SetDisplayName(AdaptableGuardian4DisplayName)
              .SetDescription(AdaptableGuardian4Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string AdaptableGuardian8 = "FuriousGuardian.AdaptableGuardian8";
        public static readonly string AdaptableGuardian8Guid = "{0921CC42-6186-417A-95CB-04D93DC895F7}";

        private const string AdaptableGuardian8Ablity2 = "FuriousGuardian.UseAdaptableGuardian82";
        public static readonly string AdaptableGuardian8Ablity2Guid = "{002069DA-F667-4F41-9ECA-9CAADB70D4D8}";

        internal const string AdaptableGuardian8DisplayName = "FuriousGuardianAdaptableGuardian8.Name";
        private const string AdaptableGuardian8Description = "FuriousGuardianAdaptableGuardian8.Description";

        public static BlueprintFeature AdaptableGuardian8Feat()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(AdaptableGuardian8Ablity2, AdaptableGuardian8Ablity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent(ChosenAllyBuffGuid)
                        .Build())
                    .Build())
                .SetDisplayName(AdaptableGuardian8DisplayName)
                .SetDescription(AdaptableGuardian8Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(AdaptableGuardian8, AdaptableGuardian8Guid)
              .SetDisplayName(AdaptableGuardian8DisplayName)
              .SetDescription(AdaptableGuardian8Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string TightFollower = "FuriousGuardian.TightFollower";
        public static readonly string TightFollowerGuid = "{EC077714-9D6D-433B-92E0-DB43AD354F78}";

        private const string TightFollowerAblity2 = "FuriousGuardian.UseTightFollower2";
        public static readonly string TightFollowerAblity2Guid = "{62A97B04-59B7-43D6-A8D6-AA2F7449DB95}";

        internal const string TightFollowerDisplayName = "FuriousGuardianTightFollower.Name";
        private const string TightFollowerDescription = "FuriousGuardianTightFollower.Description";

        public static BlueprintFeature TightFollowerFeat()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(TightFollowerAblity2, TightFollowerAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<TightFollowerAction>()
                    .Build())
                .SetDisplayName(TightFollowerDisplayName)
                .SetDescription(TightFollowerDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.DoubleMove)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(false)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(false)
                .SetCanTargetPoint(true)
                .Configure();

            return FeatureConfigurator.New(TightFollower, TightFollowerGuid)
              .SetDisplayName(TightFollowerDisplayName)
              .SetDescription(TightFollowerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string ReactiveStrike = "FuriousGuardian.ReactiveStrike";
        public static readonly string ReactiveStrikeGuid = "{B9874134-791F-45F9-9626-E1100A5D4EAE}";

        private const string ReactiveStrikeBuff = "FuriousGuardian.ReactiveStrikeBuff";
        public static readonly string ReactiveStrikeBuffGuid = "{EDE1D29E-E2FE-4803-8347-05E841628BA5}";

        internal const string ReactiveStrikeDisplayName = "FuriousGuardianReactiveStrike.Name";
        private const string ReactiveStrikeDescription = "FuriousGuardianReactiveStrike.Description";

        public static BlueprintFeature ReactiveStrikeFeat()
        {
            var icon = FeatureRefs.Opportunist.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ReactiveStrikeBuff, ReactiveStrikeBuffGuid)
             .SetDisplayName(ReactiveStrikeDisplayName)
             .SetDescription(ReactiveStrikeDescription)
             .SetIcon(icon)
             .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            return FeatureConfigurator.New(ReactiveStrike, ReactiveStrikeGuid)
              .SetDisplayName(ReactiveStrikeDisplayName)
              .SetDescription(ReactiveStrikeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddInitiatorAttackWithWeaponTrigger(onAttackOfOpportunity: true, triggerBeforeAttack: true, 
                    actionsOnInitiator: true, action: ActionsBuilder.New().ApplyBuff(Buff1, ContextDuration.Fixed(1)).Build())
              .AddComponent<ReactiveStrikeOpp>()
              .Configure();
        }
    }
}
