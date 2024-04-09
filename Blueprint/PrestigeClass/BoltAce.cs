﻿using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Modify;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using PrestigePlus.CustomComponent;
using PrestigePlus.CustomComponent.PrestigeClass;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class BoltAce
    {
        private const string ArchetypeName = "BoltAce";
        public static readonly string ArchetypeGuid = "{D5383625-4142-4A62-AA91-BD06F79F5DEB}";
        internal const string ArchetypeDisplayName = "BoltAce.Name";
        private const string ArchetypeDescription = "BoltAce.Description";

        private const string ClassProgressName = "BoltAcePrestige";
        public static readonly string ClassProgressGuid = "{04AA32FD-420F-4D5E-9FB3-F62136BDF25A}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateGrit(), SharpShootFeat(), CreateGunsmith())
                .AddToLevelEntry(2, CreateNimble())
                .AddToLevelEntry(3, CreateInitiative(), CreateResolve())
                .AddToLevelEntry(4, FeatureSelectionRefs.FighterFeatSelection.ToString())
                .AddToLevelEntry(5, CreateGunTraining())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SharpShootGuid, ResolveGuid, GunTrainingGuid }))
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
                .SetBaseAttackBonus(StatProgressionRefs.BABFull.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesHigh.ToString())
                .SetWillSave(StatProgressionRefs.SavesLow.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreNature, StatType.SkillThievery, StatType.SkillPersuasion, StatType.SkillPerception, StatType.SkillKnowledgeWorld })
                .AddPrerequisiteStatValue(StatType.SkillAthletics, 1)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 1)
                .AddPrerequisiteClassLevel(ArchetypeGuid, 5, not: true)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string Gunsmith = "Gunslinger.Gunsmith";
        private static readonly string GunsmithGuid = "{29F72FD6-0A56-4F52-8997-00A0C5672D74}";

        internal const string GunsmithDisplayName = "GunslingerGunsmith.Name";
        private const string GunsmithDescription = "GunslingerGunsmith.Description";
        private static BlueprintFeature CreateGunsmith()
        {
            var icon = FeatureRefs.Manyshot.Reference.Get().Icon;

            return FeatureConfigurator.New(Gunsmith, GunsmithGuid)
              .SetDisplayName(GunsmithDisplayName)
              .SetDescription(GunsmithDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.LightArmorProficiency.ToString() })
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.LightCrossbow,
              WeaponCategory.HeavyCrossbow,
                  })
              .Configure();
        }

        private const string GritFeature = "Gunslinger.Grit";
        private static readonly string GritFeatureGuid = "{689FA7B8-90ED-4B9A-89C9-83970FAC1F0D}";

        private const string GritResource = "Gunslinger.GritResource";
        public static readonly string GritResourceGuid = "{5E983BF8-BDE0-4FD5-B3CB-240B5A4B8BF5}";

        internal const string GritDisplayName = "GunslingerGrit.Name";
        private const string GritDescription = "GunslingerGrit.Description";

        private const string ConvertAblity1 = "Gunslinger.UseConvert";
        private static readonly string ConvertAblity1Guid = "{D8CE1AD5-0C85-4805-8669-799281586CFB}";

        internal const string GritDisplayName1 = "GunslingerGrit1.Name";
        private const string GritDescription1 = "GunslingerGrit1.Description";

        private const string ConvertAblity2 = "Gunslinger.UseConvert2";
        private static readonly string ConvertAblity2Guid = "{B1BAE0AB-950C-4EB7-B1AC-45DA0B6E6667}";

        internal const string GritDisplayName2 = "GunslingerGrit2.Name";
        private const string GritDescription2 = "GunslingerGrit2.Description";
        private static BlueprintFeature CreateGrit()
        {
            var icon = FeatureRefs.Bravery.Reference.Get().Icon;

            var swashres = "AC63BFCF-EC31-43DC-A5CE-04617A3BC854";
            var res = AbilityResourceConfigurator.New(GritResource, GritResourceGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(0).IncreaseByStat(StatType.Wisdom))
                .SetMin(1)
                .Configure();

            var ability1 = AbilityConfigurator.New(ConvertAblity1, ConvertAblity1Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .RestoreResource(swashres)
                    .Build())
                .SetDisplayName(GritDisplayName1)
                .SetDescription(GritDescription1)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: res)
                .SetActionBarAutoFillIgnored(true)
                .Configure();

            var ability2 = AbilityConfigurator.New(ConvertAblity2, ConvertAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .RestoreResource(res, 1)
                    .Build())
                .SetDisplayName(GritDisplayName2)
                .SetDescription(GritDescription2)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: swashres)
                .SetActionBarAutoFillIgnored(true)
                .Configure();

            DodgeFeat();

            return FeatureConfigurator.New(GritFeature, GritFeatureGuid)
                .SetDisplayName(GritDisplayName)
                .SetDescription(GritDescription)
                .SetIcon(icon)
                .AddFacts(new() { DodgeGuid, ability1, ability2 })
                .AddAbilityResources(resource: res, restoreAmount: true)
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(res, 1), actionsOnInitiator: true, criticalHit: true, group: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Crossbows, checkWeaponGroup: true)
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(res, 1), actionsOnInitiator: true, reduceHPToZero: true, group: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Crossbows, checkWeaponGroup: true)
                .Configure();
        }

        private const string Nimble = "Gunslinger.Nimble";
        private static readonly string NimbleGuid = "{9747226A-6953-4F3F-B01C-9BA6E2B47B88}";

        private const string NimbleBuff = "Gunslinger.NimbleBuff";
        private static readonly string NimbleGuidBuff = "{63609D02-8290-4A2B-9CD7-E59DB03A8FBE}";

        internal const string NimbleDisplayName = "GunslingerNimble.Name";
        private const string NimbleDescription = "GunslingerNimble.Description";
        private static BlueprintFeature CreateNimble()
        {
            var icon = FeatureRefs.Mobility.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(NimbleBuff, NimbleGuidBuff)
              .SetDisplayName(NimbleDisplayName)
              .SetDescription(NimbleDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddStatBonus(ModifierDescriptor.Dodge, false, StatType.AC, 1)
              .Configure();

            return FeatureConfigurator.New(Nimble, NimbleGuid)
              .SetDisplayName(NimbleDisplayName)
              .SetDescription(NimbleDescription)
              .SetIcon(icon)
              .AddBuffOnLightOrNoArmor(Buff1)
              .Configure();
        }

        private const string Initiative = "Gunslinger.Initiative";
        public static readonly string InitiativeGuid = "{03723CFE-2CA7-427D-BEEA-BCC351E9AE81}";

        internal const string InitiativeDisplayName = "GunslingerInitiative.Name";
        private const string InitiativeDescription = "GunslingerInitiative.Description";
        private static BlueprintFeature CreateInitiative()
        {
            var icon = FeatureRefs.Improved_Initiative.Reference.Get().Icon;

            return FeatureConfigurator.New(Initiative, InitiativeGuid)
              .SetDisplayName(InitiativeDisplayName)
              .SetDescription(InitiativeDescription)
              .SetIcon(icon)
              .AddStatBonus(stat: StatType.Initiative, value: 2)
              .Configure();
        }

        private const string GunTraining = "Gunslinger.GunTraining";
        private static readonly string GunTrainingGuid = "{48B6D9AF-C48D-4007-9A25-23E9523FB738}";

        internal const string GunTrainingDisplayName = "GunslingerGunTraining.Name";
        private const string GunTrainingDescription = "GunslingerGunTraining.Description";
        private static BlueprintFeature CreateGunTraining()
        {
            var icon = FeatureRefs.ClusteredShots.Reference.Get().Icon;

            return FeatureConfigurator.New(GunTraining, GunTrainingGuid)
              .SetDisplayName(GunTrainingDisplayName)
              .SetDescription(GunTrainingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.LightCrossbow, false, StatType.Dexterity, false)
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.HeavyCrossbow, false, StatType.Dexterity, false)
              .AddWeaponTypeCriticalMultiplierIncrease(1, WeaponTypeRefs.HeavyCrossbow.ToString())
              .AddWeaponTypeCriticalMultiplierIncrease(1, WeaponTypeRefs.LightCrossbow.ToString())
              .Configure();
        }

        private const string Dodge = "Gunslinger.Dodge";
        public static readonly string DodgeGuid = "{E857E9B8-8A53-4915-9EFA-A71CD57E792C}";

        internal const string DodgeDisplayName = "GunslingerDodge.Name";
        private const string DodgeDescription = "GunslingerDodge.Description";

        private const string Dodge2Buff = "Gunslinger.Dodge2Buff";
        private static readonly string Dodge2BuffGuid = "{CEEF3B49-6828-4F2C-A859-6D44AD55D28A}";

        private const string DodgeAblity = "Gunslinger.UseDodge";
        private static readonly string DodgeAblityGuid = "{5D300146-3884-4A6B-A75E-CEC32C7A64AB}";

        public static BlueprintFeature DodgeFeat()
        {
            var icon = FeatureRefs.Dodge.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Dodge2Buff, Dodge2BuffGuid)
                .SetDisplayName(DodgeDisplayName)
                .SetDescription(DodgeDescription)
                .SetIcon(icon)
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .AddComponent<DodgingGrit>(c => { c.Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(GritResourceGuid); })
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(GritResourceGuid); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(DodgeAblity, DodgeAblityGuid)
                .SetDisplayName(DodgeDisplayName)
                .SetDescription(DodgeDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: GritResourceGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(Buff2)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(Dodge, DodgeGuid)
              .SetDisplayName(DodgeDisplayName)
              .SetDescription(DodgeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Resolve = "BoltAce.Resolve";
        private static readonly string ResolveGuid = "{7B197C2C-9C64-4CE5-A7DF-015AD7B39BC0}";
        internal const string ResolveDisplayName = "BoltAceResolve.Name";
        private const string ResolveDescription = "BoltAceResolve.Description";

        private const string ResolveAblity = "BoltAce.UseResolve";
        private static readonly string ResolveAblityGuid = "{1A3C62D4-154E-46DD-A310-EFF346361749}";

        private const string ResolveBuff = "BoltAce.ResolveBuff";
        private static readonly string ResolveBuffGuid = "{5A033351-3962-484D-8BEE-01AB10225F73}";
        private static BlueprintFeature CreateResolve()
        {
            var icon = FeatureRefs.MythicResolveMythicFeat.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ResolveBuff, ResolveBuffGuid)
             .SetDisplayName(ResolveDisplayName)
             .SetDescription(ResolveDescription)
             .SetIcon(icon)
             .AddIgnoreConcealment()
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, onlyHit: false)
             .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Fixed(1), toCaster: true)
                .RangedAttack(autoHit: false, extraAttack: false)
                .Build();

            var ability = AbilityConfigurator.New(ResolveAblity, ResolveAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(ResolveDisplayName)
                .SetDescription(ResolveDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Weapon)
                .AddLineOfSightIgnorance()
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: GritResourceGuid)
                .Configure();

            return FeatureConfigurator.New(Resolve, ResolveGuid)
              .SetDisplayName(ResolveDisplayName)
              .SetDescription(ResolveDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string SharpShoot = "Gunslinger.SharpShoot";
        public static readonly string SharpShootGuid = "{BF36079D-E70E-4DEF-A8C0-C5BDDCC6AD70}";

        internal const string SharpShootDisplayName = "GunslingerSharpShoot.Name";
        private const string SharpShootDescription = "GunslingerSharpShoot.Description";

        private const string SharpShoot2Buff = "Gunslinger.SharpShoot2Buff";
        private static readonly string SharpShoot2BuffGuid = "{C5EE5F00-5994-4675-B5A3-B48574189126}";

        private const string SharpShootAblity = "Gunslinger.UseSharpShoot";
        private static readonly string SharpShootAblityGuid = "{C6832469-7D6B-42CE-80E6-B238BD74CA43}";

        public static BlueprintFeature SharpShootFeat()
        {
            var icon = AbilityRefs.FocusedShotAction.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(SharpShoot2Buff, SharpShoot2BuffGuid)
                .SetDisplayName(SharpShootDisplayName)
                .SetDescription(SharpShootDescription)
                .SetIcon(icon)
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .AddComponent<AttackTypeChangePP>(c => { c.Group = Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Crossbows; c.NewType = AttackType.RangedTouch; })
                .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New()
                        .ContextSpendResource(GritResourceGuid, 1)
                        .Build(), onlyHit: false, checkWeaponGroup: true, group: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Crossbows)
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(GritResourceGuid); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(SharpShootAblity, SharpShootAblityGuid)
                .SetDisplayName(SharpShootDisplayName)
                .SetDescription(SharpShootDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: GritResourceGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(Buff2)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(SharpShoot, SharpShootGuid)
              .SetDisplayName(SharpShootDisplayName)
              .SetDescription(SharpShootDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
