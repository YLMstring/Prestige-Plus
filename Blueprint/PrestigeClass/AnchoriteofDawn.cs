﻿using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.AreaLogic;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Pathfinding.Voxels;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Buffs.Components;
using System.Drawing;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Patch;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class AnchoriteofDawn
    {
        private const string ArchetypeName = "AnchoriteofDawn";
        public static readonly string ArchetypeGuid = "{CE227602-D387-4B37-A382-852EAF2D9F9B}";
        internal const string ArchetypeDisplayName = "AnchoriteofDawn.Name";
        private const string ArchetypeDescription = "AnchoriteofDawn.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "AnchoriteofDawnPrestige";
        private static readonly string ClassProgressGuid = "{27930DC5-8896-4017-AB49-985340E6328C}";

        public static void Configure()
        {
            CredenceFeat();
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, SASolarInvocation(), SpellbookReplace.spellupgradeGuid)
                .AddToLevelEntry(2, CredenceGuid)
                .AddToLevelEntry(3, SABaskRadiance())
                .AddToLevelEntry(4, CredenceGuid)
                .AddToLevelEntry(5, SASolarMove())
                .AddToLevelEntry(6, CredenceGuid)
                .AddToLevelEntry(7, CreateSunbeam())
                .AddToLevelEntry(8, CredenceGuid)
                .AddToLevelEntry(9)
                .AddToLevelEntry(10, SASolarSwift(), DawnFeat(), CredenceGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { spellupgradeGuid, ShadowChainsGuid, ShadowChains2Guid, ShadowChains3Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { CredenceGuid, SolarGuid, Solar2Guid, Solar3Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { }))
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
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddPrerequisiteAlignment(AlignmentMaskType.Good)
                .AddComponent<PrerequisiteSpellLevel>(c => { c.RequiredSpellLevel = 2; })
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string Credence = "AnchoriteofDawn.Credence";
        private static readonly string CredenceGuid = "{702A2959-B2EC-4149-A677-67BF30A7B363}";

        internal const string CredenceDisplayName = "AnchoriteofDawnCredence.Name";
        private const string CredenceDescription = "AnchoriteofDawnCredence.Description";

        public static void CredenceFeat()
        {
            var icon = AbilityRefs.Thoughtsense.Reference.Get().Icon;
            //"DervishDance": "bdaf6052-e215-4eec-9ad5-c1b3f380cae0",
            FocusedFeat1Feat();
            FeatureSelectionConfigurator.New(Credence, CredenceGuid)
              .SetDisplayName(CredenceDisplayName)
              .SetDescription(CredenceDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures("bdaf6052-e215-4eec-9ad5-c1b3f380cae0")
              .AddToAllFeatures(SADivineLight())
              .AddToAllFeatures(SAExtraInvocations())
              .AddToAllFeatures(FocusedFeat1Guid)
              .AddToAllFeatures(FeatureRefs.AugmentSummoning.ToString())
              .AddToAllFeatures(SASolarDefense1())
              .AddToAllFeatures(SASolarDefense2())
              .AddToAllFeatures(SolarWeaponsFeat())
              .AddToAllFeatures(SolarWeapons4Guid)
              .AddToAllFeatures(SASunBlade())
              .Configure(delayed: true);
        }

        private const string SolarInvocation = "AnchoriteofDawn.SolarInvocation";
        private static readonly string SolarInvocationGuid = "{51D06D50-859A-460A-B191-E178EA181899}";

        private const string FreeSolarInvocation = "AnchoriteofDawn.FreeSolarInvocation";
        private static readonly string FreeSolarInvocationGuid = "{ADAD9870-6E8C-49F6-B56B-6EED7C3EF8C6}";

        internal const string AnchoriteofDawnSolarInvocationDisplayName = "AnchoriteofDawnSolarInvocation.Name";
        private const string AnchoriteofDawnSolarInvocationDescription = "AnchoriteofDawnSolarInvocation.Description";

        internal const string FreeInvocationDisplayName = "FreeSolarInvocation.Name";
        private const string FreeInvocationDescription = "FreeSolarInvocation.Description";

        private const string SolarAbility = "AnchoriteStyle.SolarAbility";
        private static readonly string SolarAbilityGuid = "{45FA4862-FC2B-40E3-9688-90F3F7ABE8EC}";

        private const string SolarAbility2 = "AnchoriteStyle.SolarAbility2";
        private static readonly string SolarAbilityGuid2 = "{F334A8AD-138B-4E42-B728-1B8DF461DC5F}";

        private const string SolarAbility3 = "AnchoriteStyle.SolarAbility3";
        private static readonly string SolarAbilityGuid3 = "{EEE1CADE-7B70-4B79-9CCE-D2D3488BA9A4}";

        private const string SolarAbilityRes = "AnchoriteStyle.SolarAbilityRes";
        private static readonly string SolarAbilityResGuid = "{04B41671-24D9-4598-BF98-4928F118F3E2}";

        private const string SolarBuff = "AnchoriteStyle.Gazebuff";
        private static readonly string SolarBuffGuid = "{E6FAC7DB-0DF8-488C-91C5-CAD7247FFB1A}";

        private const string SolarBuff2 = "AnchoriteStyle.Gazebuff2";
        private static readonly string SolarBuffGuid2 = "{92537510-0C34-48DE-A78E-F4AC8791605A}";

        private const string SolarBuff3 = "AnchoriteStyle.Gazebuff3";
        private static readonly string SolarBuffGuid3 = "{6EB3BA91-3A21-41ED-AF79-91A4D90A8F99}";

        private const string SolarBuff4 = "AnchoriteStyle.Gazebuff4";
        public static readonly string SolarBuffGuid4 = "{973B8E27-94EE-4E9E-9C86-8D8F17C88A44}";

        private const string SolarBuff5 = "AnchoriteStyle.Gazebuff5";
        public static readonly string SolarBuffGuid5 = "{1E228A48-776D-4035-99DB-BCC08F6762FB}";

        private const string GazeAura = "AnchoriteStyle.GazeAura";
        private static readonly string GazeAuraGuid = "{048B72FD-2E5E-4346-9B4D-C5C1B6944599}";
        public static BlueprintFeature SASolarInvocation()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SolarAbilityRes, SolarAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }, 2)
                        .IncreaseByStat(StatType.Charisma))
                .Configure();

            var solarweaponremove = ActionsBuilder.New().RemoveBuff(SolarWeaponsBuffGuid).RemoveBuff(SolarWeaponsBuffplusGuid).Build();
            var solarweapongive2 = ActionsBuilder.New()
                .ApplyBuffPermanent(SolarWeaponsBuffplusGuid)
                .Build();
            var solarweapongive = ActionsBuilder.New()
                .ApplyBuffPermanent(SolarWeaponsBuffGuid)
                .Conditional(ConditionsBuilder.New().CasterHasFact(SolarWeapons4Guid).Build(), ifTrue: solarweapongive2)
                .Build();
            var solarweapon = ActionsBuilder.New()
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons1BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons2BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons3BuffGuid).Build(), ifTrue: solarweapongive)
                .Build();

            var Buff2 = BuffConfigurator.New(SolarBuff2, SolarBuffGuid2)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddBuffActions(deactivated: solarweaponremove, activated: solarweapon)
              .Configure();

            var givebuff = ActionsBuilder.New().ApplyBuffPermanent(Buff2).Build();
            var retrivebuff = ActionsBuilder.New().RemoveBuff(Buff2).Build();

            var area = AbilityAreaEffectConfigurator.New(GazeAura, GazeAuraGuid)
                .SetAffectEnemies(false)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(33.Feet())
                .AddAbilityAreaEffectBuff(Buff2, condition: ConditionsBuilder.New().CasterHasFact(BaskRadianceGuid).IsCaster(true).Build())
                .Configure();

            var Buff3 = BuffConfigurator.New(SolarBuff3, SolarBuffGuid3)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var Buff5 = BuffConfigurator.New(SolarBuff5, SolarBuffGuid5)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .SetStacking(StackingType.Summ)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var Buff1 = BuffConfigurator.New(SolarBuff, SolarBuffGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddBuffActions(activated: ActionsBuilder.New()
                .OnPets(givebuff)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons1BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons2BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons3BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New()
                    .HasBuff(Buff5).Build(),
                    ifFalse: ActionsBuilder.New().RemoveSelf().Build())
                .Build(),
                deactivated: ActionsBuilder.New().OnPets(retrivebuff).RemoveBuff(SolarWeaponsBuffGuid).RemoveBuff(SolarWeaponsBuffplusGuid).Build())
              .AddAuraFeatureComponent(Buff3)
              .Configure();

            BuffConfigurator.New(SolarBuff4, SolarBuffGuid4)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddComponent<SolarInvocationComp>()
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddBuffActions(activated: ActionsBuilder.New()
                .OnPets(givebuff)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons1BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons2BuffGuid).Build(), ifTrue: solarweapongive)
                .Conditional(ConditionsBuilder.New().HasFact(SolarWeapons3BuffGuid).Build(), ifTrue: solarweapongive)
                .Build(),
                deactivated: ActionsBuilder.New().OnPets(retrivebuff).RemoveBuff(SolarWeaponsBuffGuid).RemoveBuff(SolarWeaponsBuffplusGuid).Build())
              .AddAuraFeatureComponent(Buff3)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(SolarAbility, SolarAbilityGuid)
                .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
                .SetDescription(AnchoriteofDawnSolarInvocationDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: FreeSolarInvocationGuid)
                .Configure();

            ActivatableAbilityConfigurator.New(SolarAbility2, SolarAbilityGuid2)
                .SetDisplayName(AnchoriteSolarMoveDisplayName)
                .SetDescription(AnchoriteSolarMoveDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: FreeSolarInvocationGuid)
                .Configure();

            ActivatableAbilityConfigurator.New(SolarAbility3, SolarAbilityGuid3)
                .SetDisplayName(AnchoriteSolarSwiftDisplayName)
                .SetDescription(AnchoriteSolarSwiftDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: FreeSolarInvocationGuid)
                .Configure();

            FeatureConfigurator.New(FreeSolarInvocation, FreeSolarInvocationGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(FreeInvocationDisplayName)
              .SetDescription(FreeInvocationDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(ExtraInvocationsGuid)
              .Configure();

            var action = ActionsBuilder.New().ApplyBuff(Buff5, durationValue: ContextDuration.Fixed(2)).Build();

            return FeatureConfigurator.New(SolarInvocation, SolarInvocationGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddAbilityUseTrigger(action: action, checkSpellSchool: true, isSpellSchool: SpellSchool.Evocation)
              //.AddComponent<DomainProgressionContinue>()
              .Configure();
        }

        private const string BaskRadiance = "Anchorite.BaskRadiance";
        private static readonly string BaskRadianceGuid = "{B566CA3D-C3F2-4CDD-B33E-F3E3D1257A62}";

        internal const string AnchoriteBaskRadianceDisplayName = "AnchoriteBaskRadiance.Name";
        private const string AnchoriteBaskRadianceDescription = "AnchoriteBaskRadiance.Description";
        public static BlueprintFeature SABaskRadiance()
        {
            var icon = AbilityRefs.Sunbeam.Reference.Get().Icon;
            return FeatureConfigurator.New(BaskRadiance, BaskRadianceGuid)
              .SetDisplayName(AnchoriteBaskRadianceDisplayName)
              .SetDescription(AnchoriteBaskRadianceDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string SolarMove = "Anchorite.SolarMove";
        private static readonly string SolarMoveGuid = "{95811779-A38E-40AF-AF88-494B8C70F74D}";

        internal const string AnchoriteSolarMoveDisplayName = "AnchoriteSolarMove.Name";
        private const string AnchoriteSolarMoveDescription = "AnchoriteSolarMove.Description";
        public static BlueprintFeature SASolarMove()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarMove, SolarMoveGuid)
              .SetDisplayName(AnchoriteSolarMoveDisplayName)
              .SetDescription(AnchoriteSolarMoveDescription)
              .SetIcon(icon)
              .AddFacts(new() { SolarAbilityGuid2 })
              .Configure();
        }

        private const string SolarSwift = "Anchorite.SolarSwift";
        private static readonly string SolarSwiftGuid = "{81157D97-7D4D-4910-B25A-F4129CFAAE3E}";

        internal const string AnchoriteSolarSwiftDisplayName = "AnchoriteSolarSwift.Name";
        private const string AnchoriteSolarSwiftDescription = "AnchoriteSolarSwift.Description";
        public static BlueprintFeature SASolarSwift()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarSwift, SolarSwiftGuid)
              .SetDisplayName(AnchoriteSolarSwiftDisplayName)
              .SetDescription(AnchoriteSolarSwiftDescription)
              .SetIcon(icon)
              .AddFacts(new() { SolarAbilityGuid3 })
              .Configure();
        }

        private const string Sunbeam = "Anchorite.Sunbeam";
        private static readonly string SunbeamGuid = "{7A85CDC3-35C5-4FA6-A83F-812C19B91676}";
        internal const string SunbeamDisplayName = "AnchoriteSunbeam.Name";
        private const string SunbeamDescription = "AnchoriteSunbeam.Description";

        private const string SunbeamAblity = "Anchorite.UseSunbeam";
        private static readonly string SunbeamAblityGuid = "{92AFBDAE-29CD-4576-A812-7A169C308835}";

        private const string SunbeamAblityRes = "Anchorite.SunbeamRes";
        private static readonly string SunbeamAblityResGuid = "{3109B0BB-F006-4988-9FFB-1AA846B87AAC}";

        private static BlueprintFeature CreateSunbeam()
        {
            var icon = AbilityRefs.Sunbeam.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SunbeamAblityRes, SunbeamAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 3, bonusPerStep: 1, startingLevel: 7))
                .SetUseMax()
                .SetMax(2)
                .Configure();
            var ability = AbilityConfigurator.New(SunbeamAblity, SunbeamAblityGuid)
                .CopyFrom(
                AbilityRefs.Sunbeam,
                typeof(AbilityEffectRunAction),
                typeof(AbilityDeliverProjectile),
                typeof(ContextRankConfig),
                typeof(SpellComponent))
                .SetType(AbilityType.SpellLike)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: SunbeamAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Sunbeam, SunbeamGuid)
              .SetDisplayName(SunbeamDisplayName)
              .SetDescription(SunbeamDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Dawn = "Anchorite.DawnInvocation";
        private static readonly string DawnGuid = "{DD8D398C-A81C-4E7F-A5EB-291787FAF4B0}";

        private const string DawnAblity = "Anchorite.UseDawnInvocation";
        private static readonly string DawnAblityGuid = "{BBE7E7B3-A634-42FF-B31F-4FD997BE848B}";

        private const string DawnAblityRes = "Anchorite.UseDawnInvocationRes";
        private static readonly string DawnAblityResGuid = "{8C824C12-30C4-412B-9ED1-1011FFD0C385}";

        internal const string DawnDisplayName = "AnchoriteDawnInvocation.Name";
        private const string DawnDescription = "AnchoriteDawnInvocation.Description";
        public static BlueprintFeature DawnFeat()
        {
            var icon = FeatureRefs.SunDomainGreaterFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(DawnAblityRes, DawnAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(DawnAblity, DawnAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(SolarBuffGuid4, ContextDuration.Variable(ContextValues.Constant(10)))
                    .Build())
                .SetDisplayName(DawnDisplayName)
                .SetDescription(DawnDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Dawn, DawnGuid)
              .SetDisplayName(DawnDisplayName)
              .SetDescription(DawnDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddAutoMetamagic(new() { ability }, metamagic: Metamagic.Quicken, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any)
              .Configure();
        }

        private const string DivineLight = "Anchorite.DivineLight";
        private static readonly string DivineLightGuid = "{D04D760F-C282-4E09-AD5F-8BCBD5EA864D}";

        internal const string AnchoriteDivineLightDisplayName = "AnchoriteDivineLight.Name";
        private const string AnchoriteDivineLightDescription = "AnchoriteDivineLight.Description";
        public static BlueprintFeature SADivineLight()
        {
            var icon = AbilityRefs.PillarOfLife.Reference.Get().Icon;
            return FeatureConfigurator.New(DivineLight, DivineLightGuid)
              .SetDisplayName(AnchoriteDivineLightDisplayName)
              .SetDescription(AnchoriteDivineLightDescription)
              .SetIcon(icon)
              .AddAuraFeatureComponent(SolarBuffGuid5)
              .AddIncreaseResourceAmount(SolarAbilityResGuid, 2)
              .Configure();
        }

        private const string ExtraInvocations = "Anchorite.ExtraInvocations";
        private static readonly string ExtraInvocationsGuid = "{B6BC18B5-AD61-4F65-B11C-069090DB3378}";

        internal const string AnchoriteExtraInvocationsDisplayName = "AnchoriteExtraInvocations.Name";
        private const string AnchoriteExtraInvocationsDescription = "AnchoriteExtraInvocations.Description";
        public static BlueprintFeature SAExtraInvocations()
        {
            var icon = FeatureRefs.DomainMastery.Reference.Get().Icon;
            return FeatureConfigurator.New(ExtraInvocations, ExtraInvocationsGuid)
              .SetDisplayName(AnchoriteExtraInvocationsDisplayName)
              .SetDescription(AnchoriteExtraInvocationsDescription)
              .SetIcon(icon)
              .AddIncreaseResourceAmountBySharedValue(false, SolarAbilityResGuid, ContextValues.Rank(type: AbilityRankType.StatBonus))
              .AddIncreaseResourceAmountBySharedValue(false, SolarAbilityResGuid, ContextValues.Rank(type: AbilityRankType.ProjectilesCount))
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Charisma, type: AbilityRankType.StatBonus))
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }, type: AbilityRankType.ProjectilesCount).WithBonusValueProgression(0, true))
              .Configure();
        }

        private const string FocusedAnimalCompanion = "Anchorite.FocusedAnimalCompanion";
        private static readonly string FocusedAnimalCompanionGuid = "{BE30F67B-D2D5-4257-92FB-60032E534792}";

        internal const string AnchoriteFocusedAnimalCompanionDisplayName = "AnchoriteFocusedAnimalCompanion.Name";
        private const string AnchoriteFocusedAnimalCompanionDescription = "AnchoriteFocusedAnimalCompanion.Description";

        public static BlueprintFeature SAFocusedAnimalCompanion()
        {
            var icon = FeatureSelectionRefs.AnimalCompanionSelectionDruid.Reference.Get().Icon;
            return FeatureConfigurator.New(FocusedAnimalCompanion, FocusedAnimalCompanionGuid)
              .SetDisplayName(AnchoriteFocusedAnimalCompanionDisplayName)
              .SetDescription(AnchoriteFocusedAnimalCompanionDescription)
              .SetIcon(icon)
              .AddPrerequisitePet(type: PetType.AnimalCompanion)
              .AddComponent<CompanionBoonClassLevel>(c => { c.m_RankFeature = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.AnimalCompanionRank.ToString()); })
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string SunBlade = "Anchorite.SunBlade";
        private static readonly string SunBladeGuid = "{A406BEDD-E65B-47C3-90A0-883B6352430E}";

        private const string SunBladeBuff = "Anchorite.SunBladeBuff";
        public static readonly string SunBladeBuffGuid = "{BDA0B3EA-10A8-4A6A-BBD4-6EA6B043BCAD}";

        internal const string AnchoriteSunBladeDisplayName = "AnchoriteSunBlade.Name";
        private const string AnchoriteSunBladeDescription = "AnchoriteSunBlade.Description";
        public static BlueprintFeature SASunBlade()
        {
            var icon = AbilityRefs.Flare.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(SunBladeBuff, SunBladeBuffGuid)
                .CopyFrom(
                BuffRefs.MageLightBuff,
                typeof(UniqueBuff))
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            return FeatureConfigurator.New(SunBlade, SunBladeGuid)
              .SetDisplayName(AnchoriteSunBladeDisplayName)
              .SetDescription(AnchoriteSunBladeDescription)
              .SetIcon(icon)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.BaneUndead.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.BaneUndead.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .AddAuraFeatureComponent(Buff)
              .Configure();
        }

        private const string FocusedBane = "Anchorite.FocusedBane";
        private static readonly string FocusedBaneGuid = "{3DEDACFA-06C3-42DE-A930-6B0093159EE8}";

        internal const string FocusedBaneDisplayName = "AnchoriteFocusedBane.Name";
        private const string FocusedBaneDescription = "AnchoriteFocusedBane.Description";
        public static BlueprintProgression FocusedBaneFeat()
        {
            var icon = FeatureRefs.InquisitorBaneGreaterFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(FocusedBane, FocusedBaneGuid)
              .SetDisplayName(FocusedBaneDisplayName)
              .SetDescription(FocusedBaneDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteClassLevel(CharacterClassRefs.InquisitorClass.ToString(), 1)
              .AddToClasses(CharacterClassRefs.InquisitorClass.ToString())
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(5, FeatureRefs.InquisitorBaneNormalFeatureAdd.ToString())
              .AddToLevelEntry(12, FeatureRefs.InquisitorBaneGreaterFeature.ToString())
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.InquisitorBaneResource.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, false))
              .Configure();
        }

        private const string FocusedRagingSong = "Anchorite.FocusedRagingSong";
        public static readonly string FocusedRagingSongGuid = "{CEA73061-1C07-48B1-A069-FB5B698219F7}";

        internal const string FocusedRagingSongDisplayName = "AnchoriteFocusedRagingSong.Name";
        private const string FocusedRagingSongDescription = "AnchoriteFocusedRagingSong.Description";

        private const string AnchoriteSongPlusfeat = "Anchorite.AnchoriteSongPlusfeat";
        public static readonly string AnchoriteSongPlusfeatGuid = "{09AA488A-8E30-4868-AF86-22E678B6BAC4}";

        private const string AnchoriteSongPlusfeat1 = "Anchorite.AnchoriteSongPlusfeat1";
        public static readonly string AnchoriteSongPlusfeat1Guid = "{C19350F5-AC99-4E94-8439-7E5116CC565E}";
        public static BlueprintProgression FocusedRagingSongFeat()
        {
            var icon = FeatureRefs.RagingSong.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(AnchoriteSongPlusfeat, AnchoriteSongPlusfeatGuid)
              .SetDisplayName(FocusedRagingSongDisplayName)
              .SetDescription(FocusedRagingSongDescription)
              .SetIsClassFeature(true)
              .SetRanks(20)
              .SetHideInUI(true)
              .Configure();

            var feat = ProgressionConfigurator.New(AnchoriteSongPlusfeat1, AnchoriteSongPlusfeat1Guid)
              .SetDisplayName(FocusedRagingSongDisplayName)
              .SetDescription(FocusedRagingSongDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, feat1)
              .AddToLevelEntry(2, feat1)
                .AddToLevelEntry(3, feat1)
                .AddToLevelEntry(4, feat1)
                .AddToLevelEntry(5, feat1)
                .AddToLevelEntry(6, feat1)
                .AddToLevelEntry(7, feat1)
                .AddToLevelEntry(8, feat1)
                .AddToLevelEntry(9, feat1)
                .AddToLevelEntry(10, feat1)
              .Configure();

            return ProgressionConfigurator.New(FocusedRagingSong, FocusedRagingSongGuid)
              .SetDisplayName(FocusedRagingSongDisplayName)
              .SetDescription(FocusedRagingSongDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteClassLevel(CharacterClassRefs.SkaldClass.ToString(), 1)
              .AddToClasses(CharacterClassRefs.SkaldClass.ToString())
              .AddToClasses(ArchetypeGuid)
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.RagingSongResource.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, true))
              .AddToLevelEntry(1, feat)
              .AddToLevelEntry(6, FeatureRefs.SongOfStrength.ToString())
              .AddToLevelEntry(7, FeatureRefs.SKaldMovePerformance.ToString())
              .AddToLevelEntry(10, FeatureRefs.DirgeOfDoom.ToString())
              .AddToLevelEntry(13, FeatureRefs.SkaldSwiftPerformance.ToString())
              .AddToLevelEntry(14, FeatureRefs.SongOfTheFallen.ToString())
              .Configure();
        }

        private const string FocusedSacredWeapon = "Anchorite.FocusedSacredWeapon";
        private static readonly string FocusedSacredWeaponGuid = "{46879D7A-0384-4305-BE93-774BFAE80931}";

        internal const string FocusedSacredWeaponDisplayName = "AnchoriteFocusedSacredWeapon.Name";
        private const string FocusedSacredWeaponDescription = "AnchoriteFocusedSacredWeapon.Description";

        private const string FocusedSacredWeapon1 = "Anchorite.FocusedSacredWeapon1";
        private static readonly string FocusedSacredWeaponGuid1 = "{071B69C1-AD72-499A-9AD2-3A8A3E7EC235}";

        private const string FocusedSacredWeapon2 = "Anchorite.FocusedSacredWeapon2";
        private static readonly string FocusedSacredWeaponGuid2 = "{77AE9B7D-2050-4968-ACA1-D660F32BDC88}";

        private const string FocusedSacredWeapon3 = "Anchorite.FocusedSacredWeapon3";
        private static readonly string FocusedSacredWeaponGuid3 = "{2862910F-BC27-41CD-96E1-03DDF7EBE137}";

        private const string FocusedSacredWeapon4 = "Anchorite.FocusedSacredWeapon4";
        private static readonly string FocusedSacredWeaponGuid4 = "{191B8C30-3ACF-4BBA-96B4-1D7F2A087DE0}";
        public static BlueprintProgression FocusedSacredWeaponFeat()
        {
            var icon = FeatureRefs.SacredWeaponEnchantFeature.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(FocusedSacredWeapon1, FocusedSacredWeaponGuid1)
              .SetDisplayName(FocusedSacredWeaponDisplayName)
              .SetDescription(FocusedSacredWeaponDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAuraFeatureComponent(BuffRefs.WarpriestSacredWeaponBuff1d8.ToString())
              .AddRemoveFeatureOnApply(FeatureRefs.WarpriestSacredWeaponBaseDamageFeature.ToString())
              .Configure();

            var feat2 = FeatureConfigurator.New(FocusedSacredWeapon2, FocusedSacredWeaponGuid2)
              .SetDisplayName(FocusedSacredWeaponDisplayName)
              .SetDescription(FocusedSacredWeaponDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAuraFeatureComponent(BuffRefs.WarpriestSacredWeaponBuff1d10.ToString())
              .Configure();

            var feat3 = FeatureConfigurator.New(FocusedSacredWeapon3, FocusedSacredWeaponGuid3)
              .SetDisplayName(FocusedSacredWeaponDisplayName)
              .SetDescription(FocusedSacredWeaponDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAuraFeatureComponent(BuffRefs.WarpriestSacredWeaponBuff2d6.ToString())
              .Configure();

            var feat4 = FeatureConfigurator.New(FocusedSacredWeapon4, FocusedSacredWeaponGuid4)
              .SetDisplayName(FocusedSacredWeaponDisplayName)
              .SetDescription(FocusedSacredWeaponDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddAuraFeatureComponent(BuffRefs.WarpriestSacredWeaponBuff2d8.ToString())
              .Configure();

            return ProgressionConfigurator.New(FocusedSacredWeapon, FocusedSacredWeaponGuid)
              .SetDisplayName(FocusedSacredWeaponDisplayName)
              .SetDescription(FocusedSacredWeaponDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteClassLevel(CharacterClassRefs.WarpriestClass.ToString(), 1)
              .AddToClasses(CharacterClassRefs.WarpriestClass.ToString())
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(4, FeatureRefs.SacredWeaponEnchantFeature.ToString())
              .AddToLevelEntry(5, feat)
              .AddToLevelEntry(8, FeatureRefs.SacredWeaponEnchantPlus2.ToString())
              .AddToLevelEntry(10, feat2)
              .AddToLevelEntry(12, FeatureRefs.SacredWeaponEnchantPlus3.ToString())
              .AddToLevelEntry(15, feat3)
              .AddToLevelEntry(16, FeatureRefs.SacredWeaponEnchantPlus4.ToString())
              .AddToLevelEntry(20, feat4, FeatureRefs.SacredWeaponEnchantPlus5.ToString())
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.SacredWeaponEnchantResource.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, false))
              .Configure();
        }

        private const string AnchoriteFavoredEnemy = "Anchorite.AnchoriteFavoredEnemy";
        private static readonly string AnchoriteFavoredEnemyGuid = "{4937E730-03C3-43A5-BD36-F1AF2B5280F1}";

        internal const string AnchoriteFavoredEnemyDisplayName = "AnchoriteAnchoriteFavoredEnemy.Name";
        private const string AnchoriteFavoredEnemyDescription = "AnchoriteAnchoriteFavoredEnemy.Description";
        public static BlueprintProgression AnchoriteFavoredEnemyFeat()
        {
            var icon = FeatureSelectionRefs.FavoriteEnemySelection.Reference.Get().Icon;

            return ProgressionConfigurator.New(AnchoriteFavoredEnemy, AnchoriteFavoredEnemyGuid)
              .SetDisplayName(AnchoriteFavoredEnemyDisplayName)
              .SetDescription(AnchoriteFavoredEnemyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteClassLevel(CharacterClassRefs.RangerClass.ToString(), 1)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(4, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
              .AddToLevelEntry(9, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
              .Configure();
        }

        private const string AnchoriteDomainPlus = "Anchorite.AnchoriteDomainPlus";
        private static readonly string AnchoriteDomainPlusGuid = "{3A51D6BA-38E5-4D58-B60B-19DDB4EC131F}";

        private const string AnchoriteDomainPlusfeat = "Anchorite.AnchoriteDomainPlusfeat";
        public static readonly string AnchoriteDomainPlusfeatGuid = "{9E2EFBD5-9595-4645-B719-30A8C52F7B88}";

        internal const string AnchoriteDomainPlusDisplayName = "AnchoriteAnchoriteDomainPlus.Name";
        private const string AnchoriteDomainPlusDescription = "AnchoriteAnchoriteDomainPlus.Description";

        public static BlueprintProgression AnchoriteDomainPlusFeat()
        {
            var icon = AbilityRefs.Bless.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(AnchoriteDomainPlusfeat, AnchoriteDomainPlusfeatGuid)
              .SetDisplayName(AnchoriteDomainPlusDisplayName)
              .SetDescription(AnchoriteDomainPlusDescription)
              .SetIsClassFeature(true)
              .SetRanks(20)
              .SetHideInUI(true)
              .AddComponent<DomainProgressionContinue>()
              .Configure();

            return ProgressionConfigurator.New(AnchoriteDomainPlus, AnchoriteDomainPlusGuid)
              .SetDisplayName(AnchoriteDomainPlusDisplayName)
              .SetDescription(AnchoriteDomainPlusDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, feat1)
              .AddToLevelEntry(2, feat1)
                .AddToLevelEntry(3, feat1)
                .AddToLevelEntry(4, feat1)
                .AddToLevelEntry(5, feat1)
                .AddToLevelEntry(6, feat1)
                .AddToLevelEntry(7, feat1)
                .AddToLevelEntry(8, feat1)
                .AddToLevelEntry(9, feat1)
                .AddToLevelEntry(10, feat1)
              .Configure();
        }

        private const string FocusedFeat1 = "AnchoriteofDawn.FocusedFeat1";
        private static readonly string FocusedFeat1Guid = "{64F6953E-9F78-4245-A6B4-9950CED96AB0}";

        internal const string FocusedFeatDisplayName = "AnchoriteofDawnFocusedFeat.Name";
        private const string FocusedFeatDescription = "AnchoriteofDawnFocusedFeat.Description";

        public static void FocusedFeat1Feat()
        {
            var icon = AbilityRefs.Thoughtsense.Reference.Get().Icon;
            FeatureSelectionConfigurator.New(FocusedFeat1, FocusedFeat1Guid)
              .SetDisplayName(FocusedFeatDisplayName)
              .SetDescription(FocusedFeatDescription)
              //.SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(SAFocusedAnimalCompanion())
              .AddToAllFeatures(FocusedBaneFeat())
              .AddToAllFeatures(FocusedRagingSongFeat())
              .AddToAllFeatures(AnchoriteDomainPlusFeat())
              .AddToAllFeatures(AnchoriteFavoredEnemyFeat())
              .AddToAllFeatures(FocusedSacredWeaponFeat())
              .Configure();
        }

        private const string SolarDefense1Feat = "Anchorite.SolarDefense1";
        public static readonly string SolarDefense1Guid = "{288619F4-7A85-4C07-B8F2-D2D67FCE15C2}";

        internal const string AnchoriteSolarDefenseDisplayName = "AnchoriteSolarDefense.Name";
        private const string AnchoriteSolarDefenseDescription = "AnchoriteSolarDefense.Description";
        public static BlueprintFeature SASolarDefense1()
        {
            var icon = AbilityRefs.InspiringRecovery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarDefense1Feat, SolarDefense1Guid)
              .SetDisplayName(AnchoriteSolarDefenseDisplayName)
              .SetDescription(AnchoriteSolarDefenseDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string SolarDefense2 = "Anchorite.SolarDefense2";
        public static readonly string SolarDefense2Guid = "{E8851BCE-4A3B-4C4A-BA59-95CDAD6EB33A}";
        public static BlueprintFeature SASolarDefense2()
        {
            var icon = AbilityRefs.InspiringRecovery.Reference.Get().Icon;
            return FeatureConfigurator.New(SolarDefense2, SolarDefense2Guid)
              .SetDisplayName(AnchoriteSolarDefenseDisplayName)
              .SetDescription(AnchoriteSolarDefenseDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(SolarDefense1Guid)
              .Configure();
        }

        private const string SolarWeaponspro = "Anchorite.SolarWeaponspro";
        private static readonly string SolarWeaponsproGuid = "{B42E313D-E2C2-4318-A643-16DE6F039B39}";

        private const string SolarWeapons = "Anchorite.SolarWeapons";
        private static readonly string SolarWeaponsGuid = "{C84FC245-EBCC-4FEA-B11F-EFA4BBDC9293}";

        private const string SolarWeapons2 = "Anchorite.SolarWeapons2";
        private static readonly string SolarWeapons2Guid = "{EA533457-8B2D-4E77-8CAB-016E1755480E}";

        private const string SolarWeapons3 = "Anchorite.SolarWeapons3";
        private static readonly string SolarWeapons3Guid = "{9CE6A893-E2B9-423C-8872-2437BDEE339B}";

        private const string SolarWeapons4 = "Anchorite.SolarWeapons4";
        private static readonly string SolarWeapons4Guid = "{27A12886-91E9-486C-ACE8-B8D0C224C1FB}";

        internal const string SolarWeaponsDisplayName = "AnchoriteSolarWeapons.Name";
        private const string SolarWeaponsDescription = "AnchoriteSolarWeapons.Description";

        private const string SolarWeaponsBuff = "Anchorite.SolarWeaponsBuff";
        private static readonly string SolarWeaponsBuffGuid = "{847C0D5B-645B-44DD-BAA4-4394B4E70745}";

        private const string SolarWeaponsBuffplus = "Anchorite.SolarWeaponsBuffplus";
        private static readonly string SolarWeaponsBuffplusGuid = "{6AB7B3D4-92BC-4329-85A1-C732A2E60760}";

        private const string SolarWeapons1Buff = "Anchorite.SolarWeapons1Buff";
        private static readonly string SolarWeapons1BuffGuid = "{7BE7C008-F328-4171-9260-4B9160D3E734}";

        private const string SolarWeapons2Buff = "Anchorite.SolarWeapons2Buff";
        private static readonly string SolarWeapons2BuffGuid = "{6036FE18-493D-43FD-969F-34AA1DC2439F}";

        private const string SolarWeaponsAblity = "Anchorite.UseSolarWeapons";
        private static readonly string SolarWeaponsAblityGuid = "{F8023F5E-3E4A-46E9-AD70-4F156A2DE8F5}";

        private const string SolarWeapons2Ablity = "Anchorite.UseSolarWeapons2";
        private static readonly string SolarWeapons2AblityGuid = "{14531E1A-59AC-4A62-BBD6-B574899D0772}";

        private const string SolarWeapons3Ablity = "Anchorite.UseSolarWeapons3";
        private static readonly string SolarWeapons3AblityGuid = "{2F935ABE-3D11-40DE-A35A-AFE774B848C3}";

        private const string SolarWeapons3Buff = "Anchorite.SolarWeapons3Buff";
        private static readonly string SolarWeapons3BuffGuid = "{3E2C82A8-F523-4AD9-830D-AC17479E1128}";

        public static BlueprintProgression SolarWeaponsFeat()
        {
            var icon = AbilityRefs.Heroism.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(SolarWeaponsBuff, SolarWeaponsBuffGuid)
             .SetDisplayName(SolarWeaponsDisplayName)
             .SetDescription(SolarWeaponsDescription)
             .SetIcon(icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var Buffplus = BuffConfigurator.New(SolarWeaponsBuffplus, SolarWeaponsBuffplusGuid)
             .SetDisplayName(SolarWeaponsDisplayName)
             .SetDescription(SolarWeaponsDescription)
             .SetIcon(icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.FlamingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.FlamingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var Buff1 = BuffConfigurator.New(SolarWeapons1Buff, SolarWeapons1BuffGuid)
             .SetDisplayName(SolarWeaponsDisplayName)
             .SetDescription(SolarWeaponsDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .Configure();

            var Buff2 = BuffConfigurator.New(SolarWeapons2Buff, SolarWeapons2BuffGuid)
             .SetDisplayName(SolarWeaponsDisplayName)
             .SetDescription(SolarWeaponsDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .Configure();

            var Buff3 = BuffConfigurator.New(SolarWeapons3Buff, SolarWeapons3BuffGuid)
             .SetDisplayName(SolarWeaponsDisplayName)
             .SetDescription(SolarWeaponsDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .Configure();

            var ability = AbilityConfigurator.New(SolarWeaponsAblity, SolarWeaponsAblityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff1).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff1)
                        .Build())
                    .Build())
                .SetDisplayName(SolarWeaponsDisplayName)
                .SetDescription(SolarWeaponsDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(true)
                .Configure();

            var ability2 = AbilityConfigurator.New(SolarWeapons2Ablity, SolarWeapons2AblityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff2).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff2)
                        .Build())
                    .Build())
                .SetDisplayName(SolarWeaponsDisplayName)
                .SetDescription(SolarWeaponsDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(true)
                .Configure();

            var ability3 = AbilityConfigurator.New(SolarWeapons3Ablity, SolarWeapons3AblityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff3).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff3)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff3)
                        .Build())
                    .Build())
                .SetDisplayName(SolarWeaponsDisplayName)
                .SetDescription(SolarWeaponsDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(true)
                .Configure();

            var feat1 = FeatureConfigurator.New(SolarWeapons, SolarWeaponsGuid)
              .SetDisplayName(SolarWeaponsDisplayName)
              .SetDescription(SolarWeaponsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>() { ability })
              .Configure();

            var feat2 = FeatureConfigurator.New(SolarWeapons2, SolarWeapons2Guid)
              .SetDisplayName(SolarWeaponsDisplayName)
              .SetDescription(SolarWeaponsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>() { ability2 })
              .Configure();

            var feat3 = FeatureConfigurator.New(SolarWeapons3, SolarWeapons3Guid)
              .SetDisplayName(SolarWeaponsDisplayName)
              .SetDescription(SolarWeaponsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>() { ability3 })
              .Configure();

            FeatureConfigurator.New(SolarWeapons4, SolarWeapons4Guid)
              .SetDisplayName(SolarWeaponsDisplayName)
              .SetDescription(SolarWeaponsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(SolarWeaponsGuid)
              .Configure();

            return ProgressionConfigurator.New(SolarWeaponspro, SolarWeaponsproGuid)
              .SetDisplayName(SolarWeaponsDisplayName)
              .SetDescription(SolarWeaponsDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, feat1)
              .AddToLevelEntry(5, feat2)
              .AddToLevelEntry(10, feat3)
              .Configure();
        }
    }
}
