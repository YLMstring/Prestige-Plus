using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.PrestigeClasses
{
    internal class HalflingOpportunist
    {
        private const string ArchetypeName = "HalflingOpportunist";
        private static readonly string ArchetypeGuid = "{AEA57FFB-36F0-43AB-9BA6-BFB3B0D9AFF9}";
        internal const string ArchetypeDisplayName = "HalflingOpportunist.Name";
        private const string ArchetypeDescription = "HalflingOpportunist.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "HalflingOpportunistPrestige";
        private static readonly string ClassProgressGuid = "{BB330A5A-1949-46D3-B34D-662876F43011}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, FeatureRefs.AuraOfCourageFeature.ToString(), CreateExploitive(), FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
                .AddToLevelEntry(2, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(3, FeatureRefs.CannyObserver.ToString())
                .AddToLevelEntry(4, FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(5, FeatureRefs.CannyObserver.ToString())
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
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillMobility, StatType.SkillPersuasion })
                .AddPrerequisiteFeature(RaceRefs.HalflingRace.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.DefensiveCombatTraining.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var HalflingOpportunistref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = HalflingOpportunistref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Exploitive = "HalflingOpportunist.Exploitive";
        private static readonly string ExploitiveGuid = "{1A5088E4-3A14-465F-98B5-408B7ACB6627}";

        private const string ExploitiveBuff = "HalflingOpportunist.ExploitiveBuff";
        private static readonly string ExploitiveGuidBuff = "{1F58B574-C1D0-4D95-9888-2F765FBA7669}";

        private const string ExploitiveBuff2 = "HalflingOpportunist.ExploitiveBuff2";
        private static readonly string ExploitiveGuidBuff2 = "{7838922C-2131-42C5-824F-587AE3626D9D}";

        private const string ExploitiveBuff3 = "HalflingOpportunist.ExploitiveBuff3";
        private static readonly string ExploitiveGuidBuff3 = "{3952D4EF-8136-48ED-BA4A-8CFE13325B2D}";

        internal const string ExploitiveDisplayName = "HalflingOpportunistExploitive.Name";
        private const string ExploitiveDescription = "HalflingOpportunistExploitive.Description";
        private static BlueprintFeature CreateExploitive()
        {
            var icon = AbilityRefs.SpellDanceAbility.Reference.Get().Icon;

            var selfbuff = BuffConfigurator.New(ExploitiveBuff, ExploitiveGuidBuff)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((2, 3), (4, 4), (5, 5)))
              .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveSelf().Build(), onOwner: true)
              .Configure();

            var enemydebuff = BuffConfigurator.New(ExploitiveBuff2, ExploitiveGuidBuff2)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Rank(), descriptor: ModifierDescriptor.Penalty)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((2, 3), (4, 4), (5, 5)))
              .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveSelf().Build(), onOwner: true)
              .Configure();

            var cooldownbuff = BuffConfigurator.New(ExploitiveBuff3, ExploitiveGuidBuff3)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var action = ActionsBuilder.New()
                //component
                .Build();

            return FeatureConfigurator.New(Exploitive, ExploitiveGuid)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action, onlyMelee: true, triggerBeforeAttack: true)
              .Configure();
        }

        private const string ControllCharge = "HalflingOpportunist.ControllCharge";
        private static readonly string ControllChargeGuid = "{48AF1EDA-5A68-4C0F-B62F-7F4F9A7A721C}";

        private const string ControllChargeBuff = "HalflingOpportunist.ControllChargeBuff";
        private static readonly string ControllChargeGuidBuff = "{1AE0EC6C-3E58-4309-8B49-17CDD0EFD65B}";

        internal const string ControllChargeDisplayName = "HalflingOpportunistControllCharge.Name";
        private const string ControllChargeDescription = "HalflingOpportunistControllCharge.Description";
        private static BlueprintFeature CreateControllCharge()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ControllChargeBuff, ControllChargeGuidBuff)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              .AddACBonusAgainstAttacks(armorClassBonus: 2)
              .Configure();

            var feat = FeatureConfigurator.New(ControllCharge, ControllChargeGuid)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();

            var action = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(feat).Build(), ifTrue: ActionsBuilder.New()
                    .ApplyBuff(buff: Buff1, durationValue: ContextDuration.Fixed(1))
                    .Build())
                .Build();

            BuffConfigurator.For(BuffRefs.ChargeBuff)
            .EditComponent<AddFactContextActions>(
                a => a.Activated.Actions = CommonTool.Append(a.Activated.Actions, action.Actions))
              .Configure();

            return feat;
        }

        private const string StubbornMind = "HalflingOpportunist.StubbornMind";
        private static readonly string StubbornMindGuid = "{42D656DB-1669-417D-87F1-0BBFD93A141B}";

        internal const string StubbornMindDisplayName = "HalflingOpportunistStubbornMind.Name";
        private const string StubbornMindDescription = "HalflingOpportunistStubbornMind.Description";
        private static BlueprintFeature CreateStubbornMind()
        {
            var icon = FeatureRefs.SlipperyMind.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(StubbornMind, StubbornMindGuid)
              .SetDisplayName(StubbornMindDisplayName)
              .SetDescription(StubbornMindDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSavingThrowBonusAgainstDescriptor(ContextValues.Rank(), spellDescriptor: SpellDescriptor.MindAffecting)
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Strength).WithBonusValueProgression(0))
              .AddRecalculateOnStatChange(stat: StatType.Strength, useKineticistMainStat: false)
              .Configure();

            return feat;
        }

        private const string Smite = "HalflingOpportunist.Smite";
        private static readonly string SmiteGuid = "{08B9C71F-D39B-4ED1-806E-D7FE7B8B36E3}";

        internal const string SmiteDisplayName = "HalflingOpportunistSmite.Name";
        private const string SmiteDescription = "HalflingOpportunistSmite.Description";

        private static readonly string InheritorGuid = "{772F4078-A60A-4D0F-B06C-61E56C4688D7}";
        private static BlueprintFeature CreateSmite()
        {
            var feat = FeatureConfigurator.New(Smite, SmiteGuid)
                .CopyFrom(
                FeatureRefs.SmiteEvilFeature,
                typeof(AddFacts),
                typeof(AddAbilityResources))
              .SetDisplayName(SmiteDisplayName)
              .SetDescription(SmiteDescription)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.SmiteEvilFeature.ToString() })
              .AddDamageBonusAgainstFactOwner(bonus: ContextValues.Rank(AbilityRankType.DamageBonus), checkedFact: BuffRefs.SmiteEvilBuff.ToString())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { InheritorGuid, CharacterClassRefs.PaladinClass.ToString() }, excludeClasses: true, type: AbilityRankType.DamageBonus).WithLinearProgression(1, 0))
              .Configure();

            return feat;
        }
    }
}
