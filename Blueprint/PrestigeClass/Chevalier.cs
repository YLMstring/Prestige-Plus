using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using static Dreamteck.Splines.SplineMesh;
using System.IO.Ports;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Designers.Mechanics.Facts;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class Chevalier
    {
        private const string ArchetypeName = "Chevalier";
        private static readonly string ArchetypeGuid = "{CEE13F69-676D-4A3E-B352-7F989B2EE9DA}";
        internal const string ArchetypeDisplayName = "Chevalier.Name";
        private const string ArchetypeDescription = "Chevalier.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "ChevalierPrestige";
        private static readonly string ClassProgressGuid = "{A14DC2DF-BC52-414F-8503-87E19CE23E09}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, FeatureRefs.AuraOfCourageFeature.ToString(), CreateReckless(), FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
                .AddToLevelEntry(2, CreateControllCharge(), CreateStubbornMind())
                .AddToLevelEntry(3, FeatureRefs.PoisonImmunity.ToString(), CreateSmite())
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
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillLoreNature, StatType.SkillMobility, StatType.SkillPersuasion })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 1)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 1)
                .AddPrerequisiteAlignment(AlignmentMaskType.Good)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Chevalierref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Chevalierref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Reckless = "Chevalier.Reckless";
        private static readonly string RecklessGuid = "{8E229789-C76E-4FDB-AE0D-3532BEF28BF3}";

        private const string RecklessBuff = "Chevalier.RecklessBuff";
        private static readonly string RecklessGuidBuff = "{3574A095-46EC-4F8E-AFD8-9253258C6A49}";

        internal const string RecklessDisplayName = "ChevalierReckless.Name";
        private const string RecklessDescription = "ChevalierReckless.Description";
        private static BlueprintFeature CreateReckless()
        {
            var icon = AbilityRefs.ChargeAbility.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(RecklessBuff, RecklessGuidBuff)
              .SetDisplayName(RecklessDisplayName)
              .SetDescription(RecklessDescription)
              .SetIcon(icon)
              //.AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank(), ModifierDescriptor.Morale)
              .AddComponent<AddContextStatBonus>(c => { c.Stat = StatType.AdditionalAttackBonus; c.Value = ContextValues.Rank(); c.Descriptor = ModifierDescriptor.Morale; })
              .AddDamageBonusConditional(ContextValues.Rank(), descriptor: ModifierDescriptor.Morale)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }))
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(buff: Buff1, durationValue: ContextDuration.Fixed(1))
                .Build();

            return FeatureConfigurator.New(Reckless, RecklessGuid)
              .SetDisplayName(RecklessDisplayName)
              .SetDescription(RecklessDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddCombatStateTrigger(combatStartActions: action)
              .Configure();
        }

        private const string ControllCharge = "Chevalier.ControllCharge";
        private static readonly string ControllChargeGuid = "{48AF1EDA-5A68-4C0F-B62F-7F4F9A7A721C}";

        private const string ControllChargeBuff = "Chevalier.ControllChargeBuff";
        private static readonly string ControllChargeGuidBuff = "{1AE0EC6C-3E58-4309-8B49-17CDD0EFD65B}";

        internal const string ControllChargeDisplayName = "ChevalierControllCharge.Name";
        private const string ControllChargeDescription = "ChevalierControllCharge.Description";
        private static BlueprintFeature CreateControllCharge()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ControllChargeBuff, ControllChargeGuidBuff)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              //.AddContextStatBonus(StatType.AC, value: 2)
              .AddComponent<AddContextStatBonus>(c => { c.Stat = StatType.AC; c.Value = 2; })
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

        private const string StubbornMind = "Chevalier.StubbornMind";
        private static readonly string StubbornMindGuid = "{42D656DB-1669-417D-87F1-0BBFD93A141B}";

        internal const string StubbornMindDisplayName = "ChevalierStubbornMind.Name";
        private const string StubbornMindDescription = "ChevalierStubbornMind.Description";
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

        private const string Smite = "Chevalier.Smite";
        private static readonly string SmiteGuid = "{08B9C71F-D39B-4ED1-806E-D7FE7B8B36E3}";

        internal const string SmiteDisplayName = "ChevalierSmite.Name";
        private const string SmiteDescription = "ChevalierSmite.Description";

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
