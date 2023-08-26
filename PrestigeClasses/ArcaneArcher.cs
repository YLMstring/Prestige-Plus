using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Feats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Utility;
using PrestigePlus.Modify;

namespace PrestigePlus.PrestigeClasses
{
    internal class ArcaneArcher
    {
        private const string ArchetypeName = "ArcaneArcher";
        private static readonly string ArchetypeGuid = "{80CFDDF1-B798-4974-A7E3-D9C26BA29BC2}";
        internal const string ArchetypeDisplayName = "ArcaneArcher.Name";
        private const string ArchetypeDescription = "ArcaneArcher.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "ArcaneArcherPrestige";
        private static readonly string ClassProgressGuid = "{FCE16335-E269-4687-B787-7C73685529CB}";

        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateProficiencies(), spellupgradeGuid, EnhanceArrows1Feature())
                .AddToLevelEntry(2, ImbueArrow.FeatGuid, FeatureRefs.ReachSpellFeat.ToString())
                .AddToLevelEntry(3, EnhanceArrows2Feature())
                .AddToLevelEntry(4, CreateSeekerArrow())
                .AddToLevelEntry(5, EnhanceArrows4Feature())
                .AddToLevelEntry(6, CreatePhaseArrow())
                .AddToLevelEntry(7, EnhanceArrows2Guid)
                .AddToLevelEntry(8, CreateHailArrow())
                .AddToLevelEntry(9, EnhanceArrows3Feature())
                .AddToLevelEntry(10, CreateDeathArrow())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { EnhanceArrows1Guid, EnhanceArrows2Guid, EnhanceArrows3Guid, EnhanceArrows4Guid }))
                    ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName(ArchetypeDisplayName)
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
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeLow)
                .SetProgression(progression)
                .AddSkipLevelsForSpellProgression(new int[] {5,9})
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillStealth })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddPrerequisiteFeature(FeatureRefs.PointBlankShot.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteFeature(FeatureRefs.PreciseShot.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .AddPrerequisiteFeature(FeatureRefs.WeaponFocusLongbow.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.WeaponFocusShortbow.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteCasterTypeSpellLevel(true, false, 1, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var ArcaneArcherref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = ArcaneArcherref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Proficiencies = "ArcaneArcher.Proficiencies";
        private static readonly string ProficienciesGuid = "{3486544B-AD52-436A-A0BE-E5B8C70FDE47}";
        internal const string ProficienciesDisplayName = "ArcaneArcherProficiencies.Name";
        private const string ProficienciesDescription = "ArcaneArcherProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            var assProficiencies = FeatureRefs.SlayerProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(assProficiencies.GetComponent<AddFacts>())
              .AddProficiencies(
                armorProficiencies:
                  new Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup[]
                  {
              Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.TowerShield,
                  })
              .Configure();
        }

        private const string EnhanceArrows1 = "EnhanceArrows1";
        private static readonly string EnhanceArrows1Guid = "{6D9BF4E9-4370-49B8-A510-1E0DD286CE52}";

        internal const string EnhanceArrows1DisplayName = "EnhanceArrows1.Name";
        private const string EnhanceArrows1Description = "EnhanceArrows1.Description";
        public static BlueprintFeature EnhanceArrows1Feature()
        {
            var icon = FeatureRefs.ImbueArrowFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(EnhanceArrows1, EnhanceArrows1Guid)
              .SetDisplayName(EnhanceArrows1DisplayName)
              .SetDescription(EnhanceArrows1Description)
              .SetIcon(icon)
              .AddWeaponGroupEnchant(bonus:1, descriptor: ModifierDescriptor.Enhancement, weaponGroup: Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroup.Bows)
              .Configure();
        }

        private const string EnhanceArrows2 = "EnhanceArrows2";
        private static readonly string EnhanceArrows2Guid = "{E264D79C-1965-4EBD-8093-EAC731E7067F}";

        internal const string EnhanceArrows2DisplayName = "EnhanceArrows2.Name";
        private const string EnhanceArrows2Description = "EnhanceArrows2.Description";

        private const string EnhanceArrows2Res = "EnhanceArrows2Res";
        private static readonly string EnhanceArrows2ResGuid = "{D552BF34-6026-4774-878C-0ABA1209C03D}";

        private const string EnhanceArrows2buff1 = "EnhanceArrows2buff1";
        private static readonly string EnhanceArrows2buff1Guid = "{5FA32BB3-C724-4C64-AF42-80BC901C6C8F}";

        private const string EnhanceArrows2buff2 = "EnhanceArrows2buff2";
        private static readonly string EnhanceArrows2buff2Guid = "{172CA3EB-632A-4968-B33D-1431CEAB0ADE}";

        private const string EnhanceArrows2buff3 = "EnhanceArrows2buff3";
        private static readonly string EnhanceArrows2buff3Guid = "{7CB267A8-C18E-4883-B99A-D21219961B66}";

        private const string EnhanceArrows2buff4 = "EnhanceArrows2buff4";
        private static readonly string EnhanceArrows2buff4Guid = "{578D6372-A17F-440F-BF8B-3FC395D0E32C}";

        private const string EnhanceArrows2buff5 = "EnhanceArrows2buff5";
        private static readonly string EnhanceArrows2buff5Guid = "{A5A8B48E-24F2-4E23-B902-281E819A5EEB}";

        private const string EnhanceArrows2buff6 = "EnhanceArrows2buff6";
        private static readonly string EnhanceArrows2buff6Guid = "{C5029FD6-C331-42B9-9633-836D519329F7}";

        private const string EnhanceArrows2ability1 = "EnhanceArrows2ability1";
        private static readonly string EnhanceArrows2ability1Guid = "{F58711CA-A3D6-4378-9BA4-5AFAB66D18A2}";

        private const string EnhanceArrows2ability2 = "EnhanceArrows2ability2";
        private static readonly string EnhanceArrows2ability2Guid = "{20097FD7-6562-4F84-BD69-B4744EE724DB}";

        private const string EnhanceArrows2ability3 = "EnhanceArrows2ability3";
        private static readonly string EnhanceArrows2ability3Guid = "{E103CC80-6CE7-421D-8108-3D41260D3C86}";
        public static BlueprintFeature EnhanceArrows2Feature()
        {
            var icon = FeatureRefs.ImbueArrowFeature.Reference.Get().Icon; 
            var abilityresourse = AbilityResourceConfigurator.New(EnhanceArrows2Res, EnhanceArrows2ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .SetUseMax()
                .SetMax(1)
                .Configure();

            var Buff1 = BuffConfigurator.New(EnhanceArrows2buff1, EnhanceArrows2buff1Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.FlameStrike.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff2 = BuffConfigurator.New(EnhanceArrows2buff2, EnhanceArrows2buff2Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.IceBlastAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Frost.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Frost.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff3 = BuffConfigurator.New(EnhanceArrows2buff3, EnhanceArrows2buff3Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.ThunderstormBlastAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Shock.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Shock.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

             var Buff4 = BuffConfigurator.New(EnhanceArrows2buff4, EnhanceArrows2buff4Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.FlameStrike.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Flaming.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.FlamingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.FlamingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff5 = BuffConfigurator.New(EnhanceArrows2buff5, EnhanceArrows2buff5Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.IceBlastAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Frost.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Frost.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.IcyBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.IcyBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff6 = BuffConfigurator.New(EnhanceArrows2buff6, EnhanceArrows2buff6Guid)
             .SetDisplayName(EnhanceArrows2DisplayName)
             .SetDescription(EnhanceArrows2Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.ThunderstormBlastAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Shock.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Shock.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.ShockingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.ShockingBurst.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var ability1 = AbilityConfigurator.New(EnhanceArrows2ability1, EnhanceArrows2ability1Guid)
                .SetDisplayName(EnhanceArrows2DisplayName)
                .SetDescription(EnhanceArrows2Description)
                .SetIcon(AbilityRefs.FlameStrike.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows2ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 7, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .RemoveBuff(Buff5)
                        .RemoveBuff(Buff6)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff4)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff5)
                        .RemoveBuff(Buff6)
                        .Build())
                    .Build())
                .Configure();

            var ability2 = AbilityConfigurator.New(EnhanceArrows2ability2, EnhanceArrows2ability2Guid)
                .SetDisplayName(EnhanceArrows2DisplayName)
                .SetDescription(EnhanceArrows2Description)
                .SetIcon(AbilityRefs.IceBlastAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows2ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 7, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .RemoveBuff(Buff5)
                        .RemoveBuff(Buff6)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff5)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff6)
                        .Build())
                    .Build())
                .Configure();

            var ability3 = AbilityConfigurator.New(EnhanceArrows2ability3, EnhanceArrows2ability3Guid)
                .SetDisplayName(EnhanceArrows2DisplayName)
                .SetDescription(EnhanceArrows2Description)
                .SetIcon(AbilityRefs.ThunderstormBlastAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows2ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CharacterClass(true, ArchetypeGuid, 7, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff3)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff4)
                        .RemoveBuff(Buff5)
                        .RemoveBuff(Buff6)
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff6)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .RemoveBuff(Buff5)
                        .RemoveBuff(Buff1)
                        .Build())
                    .Build())
                .Configure();

            return FeatureConfigurator.New(EnhanceArrows2, EnhanceArrows2Guid)
              .SetDisplayName(EnhanceArrows2DisplayName)
              .SetDescription(EnhanceArrows2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability1, ability2, ability3 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string EnhanceArrows3 = "EnhanceArrows3";
        private static readonly string EnhanceArrows3Guid = "{D59B7809-D002-414D-A09C-0374CD858A18}";

        internal const string EnhanceArrows3DisplayName = "EnhanceArrows3.Name";
        private const string EnhanceArrows3Description = "EnhanceArrows3.Description";

        private const string EnhanceArrows3Res = "EnhanceArrows3Res";
        private static readonly string EnhanceArrows3ResGuid = "{444E6A0B-A598-4CB8-BF90-33B13CBF17FD}";

        private const string EnhanceArrows3buff1 = "EnhanceArrows3buff1";
        private static readonly string EnhanceArrows3buff1Guid = "{41DEC68E-DE86-486D-AF1F-A220CACE053D}";

        private const string EnhanceArrows3buff2 = "EnhanceArrows3buff2";
        private static readonly string EnhanceArrows3buff2Guid = "{9B48A45F-52A5-4998-8B28-37AD63552272}";

        private const string EnhanceArrows3buff3 = "EnhanceArrows3buff3";
        private static readonly string EnhanceArrows3buff3Guid = "{BF6304DA-7461-4DCD-BC65-B37E75F456AD}";

        private const string EnhanceArrows3buff4 = "EnhanceArrows3buff4";
        private static readonly string EnhanceArrows3buff4Guid = "{AFAAE9C8-6127-4864-B243-5AD80E583AD8}";

        private const string EnhanceArrows3ability1 = "EnhanceArrows3ability1";
        private static readonly string EnhanceArrows3ability1Guid = "{A7CC4AA7-B3CC-4B02-92A6-69436F5E278B}";

        private const string EnhanceArrows3ability2 = "EnhanceArrows3ability2";
        private static readonly string EnhanceArrows3ability2Guid = "{58F71635-6897-42C6-910C-4384C0B525ED}";

        private const string EnhanceArrows3ability3 = "EnhanceArrows3ability3";
        private static readonly string EnhanceArrows3ability3Guid = "{F60475BB-AE69-4EA8-BF8A-A1924113C6EF}";

        private const string EnhanceArrows3ability4 = "EnhanceArrows3ability4";
        private static readonly string EnhanceArrows3ability4Guid = "{1BB6EE68-A1BA-44C8-ACD5-A31D8C77D404}";
        public static BlueprintFeature EnhanceArrows3Feature()
        {
            var icon = FeatureRefs.ImbueArrowFeature.Reference.Get().Icon;
            var abilityresourse = AbilityResourceConfigurator.New(EnhanceArrows3Res, EnhanceArrows3ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .SetUseMax()
                .SetMax(1)
                .Configure();

            var Buff1 = BuffConfigurator.New(EnhanceArrows3buff1, EnhanceArrows3buff1Guid)
             .SetDisplayName(EnhanceArrows3DisplayName)
             .SetDescription(EnhanceArrows3Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.ChaosDomainBaseAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Anarchic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Anarchic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff2 = BuffConfigurator.New(EnhanceArrows3buff2, EnhanceArrows3buff2Guid)
             .SetDisplayName(EnhanceArrows3DisplayName)
             .SetDescription(EnhanceArrows3Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.LawDomainBaseAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Axiomatic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Axiomatic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff3 = BuffConfigurator.New(EnhanceArrows3buff3, EnhanceArrows3buff3Guid)
             .SetDisplayName(EnhanceArrows3DisplayName)
             .SetDescription(EnhanceArrows3Description)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .SetIcon(AbilityRefs.GoodDomainBaseAbility.Reference.Get().Icon)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Holy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Holy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .Configure();

            var Buff4 = BuffConfigurator.New(EnhanceArrows3buff4, EnhanceArrows3buff4Guid)
            .SetDisplayName(EnhanceArrows3DisplayName)
            .SetDescription(EnhanceArrows3Description)
            .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
            .SetIcon(AbilityRefs.EvilDomainBaseAbility.Reference.Get().Icon)
            .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Unholy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
            .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Unholy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
            .Configure();

            var ability1 = AbilityConfigurator.New(EnhanceArrows3ability1, EnhanceArrows3ability1Guid)
                .SetDisplayName(EnhanceArrows3DisplayName)
                .SetDescription(EnhanceArrows3Description)
                .SetIcon(AbilityRefs.ChaosDomainBaseAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows3ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Lawful, true, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .Build())
                    .Build())
                .Configure();

            var ability2 = AbilityConfigurator.New(EnhanceArrows3ability2, EnhanceArrows3ability2Guid)
                .SetDisplayName(EnhanceArrows3DisplayName)
                .SetDescription(EnhanceArrows3Description)
                .SetIcon(AbilityRefs.LawDomainBaseAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows3ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                   .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Chaotic, true, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff2)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff4)
                        .Build())
                    .Build())
                .Configure();

            var ability3 = AbilityConfigurator.New(EnhanceArrows3ability3, EnhanceArrows3ability3Guid)
                .SetDisplayName(EnhanceArrows3DisplayName)
                .SetDescription(EnhanceArrows3Description)
                .SetIcon(AbilityRefs.GoodDomainBaseAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows3ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Evil, true, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff3)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff1)
                        .RemoveBuff(Buff4)
                        .Build())
                    .Build())
                .Configure();

            var ability4 = AbilityConfigurator.New(EnhanceArrows3ability4, EnhanceArrows3ability4Guid)
                .SetDisplayName(EnhanceArrows3DisplayName)
                .SetDescription(EnhanceArrows3Description)
                .SetIcon(AbilityRefs.EvilDomainBaseAbility.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnhanceArrows3ResGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Good, true, false).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff4)
                        .RemoveBuff(Buff2)
                        .RemoveBuff(Buff3)
                        .RemoveBuff(Buff1)
                        .Build())
                    .Build())
                .Configure();

            return FeatureConfigurator.New(EnhanceArrows3, EnhanceArrows3Guid)
              .SetDisplayName(EnhanceArrows3DisplayName)
              .SetDescription(EnhanceArrows3Description)
              .SetIcon(icon)
              .AddFacts(new() { ability1, ability2, ability3, ability4 })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string EnhanceArrows4 = "EnhanceArrows4";
        private static readonly string EnhanceArrows4Guid = "{1DA01231-4001-46F3-8A31-E748AE8DFBF3}";

        internal const string EnhanceArrows4DisplayName = "EnhanceArrows4.Name";
        private const string EnhanceArrows4Description = "EnhanceArrows4.Description";
        public static BlueprintFeature EnhanceArrows4Feature()
        {
            var icon = FeatureRefs.ImbueArrowFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(EnhanceArrows4, EnhanceArrows4Guid)
              .SetDisplayName(EnhanceArrows4DisplayName)
              .SetDescription(EnhanceArrows4Description)
              .SetIcon(icon)
              .AddStatBonusWeaponRestriction(category: WeaponCategory.Longbow, checkCategory: true, stat: StatType.Reach, value: 50)
              .AddStatBonusWeaponRestriction(category: WeaponCategory.Shortbow, checkCategory: true, stat: StatType.Reach, value: 40)
              .AddStatBonusWeaponRestriction(category: WeaponCategory.LightCrossbow, checkCategory: true, stat: StatType.Reach, value: 50)
              .AddStatBonusWeaponRestriction(category: WeaponCategory.HeavyCrossbow, checkCategory: true, stat: StatType.Reach, value: 50)
              .Configure();
        }

        private const string SeekerArrow = "ArcaneArcher.SeekerArrow";
        private static readonly string SeekerArrowGuid = "{06CE3FAE-AD2C-4196-8CAF-1258A937829C}";
        internal const string SeekerArrowDisplayName = "ArcaneArcherSeekerArrow.Name";
        private const string SeekerArrowDescription = "ArcaneArcherSeekerArrow.Description";

        private const string SeekerArrowAblity = "ArcaneArcher.UseSeekerArrow";
        private static readonly string SeekerArrowAblityGuid = "{B0F3D8C1-7D16-447E-86DD-F4CC340EB55C}";

        private const string SeekerArrowBuff = "ArcaneArcher.SeekerArrowBuff";
        private static readonly string SeekerArrowBuffGuid = "{5DB32A50-E3B4-434A-851E-3FF15F77B0C3}";

        private const string SeekerArrowAblityRes = "ArcaneArcher.SeekerArrowRes";
        private static readonly string SeekerArrowAblityResGuid = "{D27600C2-7AC0-4B4E-B5C1-2D65FA580491}";

        private static BlueprintFeature CreateSeekerArrow()
        {
            var icon = FeatureRefs.PointBlankShot.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SeekerArrowBuff, SeekerArrowBuffGuid)
             .SetDisplayName(SeekerArrowDisplayName)
             .SetDescription(SeekerArrowDescription)
             .SetIcon(icon)
             .AddIgnoreConcealment()
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, triggerBeforeAttack: false)
             .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(SeekerArrowAblityRes, SeekerArrowAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 2))
                .SetUseMax()
                .SetMax(4)
                .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Fixed(1), toCaster: true)
                .RangedAttack(autoHit: false, extraAttack: false)
                .Build();

            var ability = AbilityConfigurator.New(SeekerArrowAblity, SeekerArrowAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SeekerArrowDisplayName)
                .SetDescription(SeekerArrowDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] {WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow})
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Weapon)
                .AddLineOfSightIgnorance()
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: SeekerArrowAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(SeekerArrow, SeekerArrowGuid)
              .SetDisplayName(SeekerArrowDisplayName)
              .SetDescription(SeekerArrowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string PhaseArrow = "ArcaneArcher.PhaseArrow";
        private static readonly string PhaseArrowGuid = "{247B5788-5D3A-41D3-914C-1BC3DE1B9114}";
        internal const string PhaseArrowDisplayName = "ArcaneArcherPhaseArrow.Name";
        private const string PhaseArrowDescription = "ArcaneArcherPhaseArrow.Description";

        private const string PhaseArrowAblity = "ArcaneArcher.UsePhaseArrow";
        private static readonly string PhaseArrowAblityGuid = "{53F00F51-1BAF-47E1-937C-E42AAC88976A}";

        private const string PhaseArrowBuff = "ArcaneArcher.PhaseArrowBuff";
        private static readonly string PhaseArrowBuffGuid = "{B9534BBA-2E4A-4F79-A5A6-BDFCB1B5CF36}";

        private const string PhaseArrowEnchant = "ArcaneArcher.PhaseArrowEnchant";
        private static readonly string PhaseArrowEnchantGuid = "{CC9A3D1F-E651-4CA1-91AE-F7824AA2757A}";

        private const string PhaseArrowAblityRes = "ArcaneArcher.PhaseArrowRes";
        private static readonly string PhaseArrowAblityResGuid = "{C34B584B-C132-4ADA-A929-FEBC6A15880C}";

        private static BlueprintFeature CreatePhaseArrow()
        {
            var icon = FeatureRefs.PointBlankMasterLongbow.Reference.Get().Icon;

            var enchant = WeaponEnchantmentConfigurator.New(PhaseArrowEnchant, PhaseArrowEnchantGuid)
                .SetEnchantName(PhaseArrowDisplayName)
                .SetDescription(PhaseArrowDescription)
                .AddComponent(new PhaseArrow())
                .Configure();

            var Buff1 = BuffConfigurator.New(PhaseArrowBuff, PhaseArrowBuffGuid)
             .SetDisplayName(PhaseArrowDisplayName)
             .SetDescription(PhaseArrowDescription)
             .SetIcon(icon)
             .AddIgnoreConcealment()
             .AddBuffEnchantAnyWeapon(enchant, Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(enchant, Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, triggerBeforeAttack: false)
             .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(PhaseArrowAblityRes, PhaseArrowAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 4))
                .SetUseMax()
                .SetMax(3)
                .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(Buff1, ContextDuration.Fixed(1), toCaster: true)
                .RangedAttack(autoHit: false, extraAttack: false)
                .Build();

            var ability = AbilityConfigurator.New(PhaseArrowAblity, PhaseArrowAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(PhaseArrowDisplayName)
                .SetDescription(PhaseArrowDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Weapon)
                .AddLineOfSightIgnorance()
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: PhaseArrowAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(PhaseArrow, PhaseArrowGuid)
              .SetDisplayName(PhaseArrowDisplayName)
              .SetDescription(PhaseArrowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string HailArrow = "ArcaneArcher.HailArrow";
        private static readonly string HailArrowGuid = "{188337A9-939D-4AF2-9030-34C0AD102F46}";
        internal const string HailArrowDisplayName = "ArcaneArcherHailArrow.Name";
        private const string HailArrowDescription = "ArcaneArcherHailArrow.Description";

        private const string HailArrowAblity = "ArcaneArcher.UseHailArrow";
        private static readonly string HailArrowAblityGuid = "{54EA02D4-2089-4003-8CFD-7DF653790676}";

        private const string HailArrowAblityRes = "ArcaneArcher.HailArrowRes";
        private static readonly string HailArrowAblityResGuid = "{6EFB1675-724E-451C-A953-2843441F99F8}";

        private static BlueprintFeature CreateHailArrow()
        {
            var icon = FeatureRefs.Manyshot.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(HailArrowAblityRes, HailArrowAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var shoot = ActionsBuilder.New()
                .RangedAttack(autoHit: false, extraAttack: false)
                .Build();

            var ability = AbilityConfigurator.New(HailArrowAblity, HailArrowAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(HailArrowDisplayName)
                .SetDescription(HailArrowDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow })
                .AddAbilityTargetsAround(includeDead: false, radius: 100.Feet(), targetType: TargetType.Enemy)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: HailArrowAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(HailArrow, HailArrowGuid)
              .SetDisplayName(HailArrowDisplayName)
              .SetDescription(HailArrowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string DeathArrow = "ArcaneArcher.DeathArrow";
        private static readonly string DeathArrowGuid = "{8656CF6B-810D-4493-8898-51991ABC73DE}";
        internal const string DeathArrowDisplayName = "ArcaneArcherDeathArrow.Name";
        private const string DeathArrowDescription = "ArcaneArcherDeathArrow.Description";

        private const string DeathArrowAblity = "ArcaneArcher.UseDeathArrow";
        private static readonly string DeathArrowAblityGuid = "{C40F3E29-8B09-42D9-BE48-6DBCA0CE6F20}";

        private const string DeathArrowBuff = "ArcaneArcher.DeathArrowBuff";
        private static readonly string DeathArrowBuffGuid = "{8372EE2A-F104-4B33-A569-E8802BEA15D4}";

        private const string DeathArrowAblityRes = "ArcaneArcher.DeathArrowRes";
        private static readonly string DeathArrowAblityResGuid = "{F1997102-8B5D-47CD-B329-0C8A39272CF7}";

        private static BlueprintFeature CreateDeathArrow()
        {
            var icon = AbilityRefs.FingerOfDeath.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(DeathArrowAblityRes, DeathArrowAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var shoot = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Fortitude, customDC: ContextValues.Rank(), useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New().ConditionalSaved(failed: ActionsBuilder.New().Kill(UnitState.DismemberType.Normal).Build()).Build())
                    .Build();

            var Buff1 = BuffConfigurator.New(DeathArrowBuff, DeathArrowBuffGuid)
             .SetDisplayName(DeathArrowDisplayName)
             .SetDescription(DeathArrowDescription)
             .SetIcon(icon)
             .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Charisma).WithBonusValueProgression(20, false))
             .AddInitiatorAttackWithWeaponTrigger(action: shoot, actionsOnInitiator: false, triggerBeforeAttack: false, onlyHit: true)
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, triggerBeforeAttack: false)
             .Configure();

            var ability = AbilityConfigurator.New(DeathArrowAblity, DeathArrowAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffPermanent(Buff1).Build())
                .SetDisplayName(DeathArrowDisplayName)
                .SetDescription(DeathArrowDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow })
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: DeathArrowAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(DeathArrow, DeathArrowGuid)
              .SetDisplayName(DeathArrowDisplayName)
              .SetDescription(DeathArrowDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
