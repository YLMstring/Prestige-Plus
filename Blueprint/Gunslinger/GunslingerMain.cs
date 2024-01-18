using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Items.Ecnchantments;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Gunslinger
{
    internal class GunslingerMain
    {
        private const string ArchetypeName = "Gunslinger";
        private static readonly string ArchetypeGuid = "{9F8F23C3-1E2E-42E9-9D6E-EC0EF9C67B6B}";
        internal const string ArchetypeDisplayName = "Gunslinger.Name";
        private const string ArchetypeDescription = "Gunslinger.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";

        private const string ClassProgressName = "GunslingerPrestige";
        private static readonly string ClassProgressGuid = "{1105E041-ABDC-4C03-85A9-1AD79626475D}";

        private const string MaximName = "GunslingerWeapon";
        private static readonly string MaximGuid = "{F497D51D-2B4F-4ADA-9300-EC2266DD7255}";
        internal const string MaximDisplayName = "GunslingerMaxim.Name";
        internal const string MaximDisplayName2 = "GunslingerMaxim2.Name";
        private const string MaximDescription = "GunslingerMaxim.Description";
        private const string MaximDescription2 = "GunslingerMaxim2.Description";

        private const string GuntypeName = "GunslingerWeaponType";
        private static readonly string GuntypeGuid = "{8BB782AE-6EFD-45C9-944B-0E73D6EB49D8}";

        private const string MaximEnchantName = "GunslingerWeaponEnchant";
        private static readonly string MaximEnchantGuid = "{CA8D8C72-209B-45FD-B039-9A8239ADC751}";
        internal const string MaximEnchantDisplayName = "GunslingerMaximEnchant.Name";
        private const string MaximEnchantDescription = "GunslingerMaximEnchant.Description";

        public static void Configure()
        {
            var damagetype = new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription
            {
                Type = Kingmaker.RuleSystem.Rules.Damage.DamageType.Physical,
                Physical = new Kingmaker.RuleSystem.Rules.Damage.DamageTypeDescription.PhysicalData
                {
                    Form = Kingmaker.Enums.Damage.PhysicalDamageForm.Bludgeoning
                }
            };

            var gunenchant = WeaponEnchantmentConfigurator.New(MaximEnchantName, MaximEnchantGuid)
                .SetEnchantName(MaximEnchantDisplayName)
                .SetDescription(MaximEnchantDescription)
                .AddUnitFeatureEquipment(null)
                .Configure();

            var guntype = WeaponTypeConfigurator.New(GuntypeName, GuntypeGuid)
                .SetCategory(WeaponCategory.Javelin)
                .SetTypeNameText(MaximDisplayName2)
                .SetDefaultNameText(MaximDisplayName2)
                .SetIcon(null)
                .SetVisualParameters(null)
                .SetAttackType(AttackType.Ranged)
                .SetAttackRange(60.Feet())
                .SetIsTwoHanded(true)
                .SetBaseDamage(new DiceFormula(2, DiceType.D8))
                .SetDamageType(damagetype)
                .SetCriticalRollEdge(20)
                .SetCriticalModifier(Kingmaker.Enums.Damage.DamageCriticalModifierType.X4)
                .SetWeight(140)
                .AddToEnchantments(gunenchant)
                .Configure();

            var maxim = ItemWeaponConfigurator.New(MaximName, MaximGuid)
                .SetDisplayNameText(MaximDisplayName)
                .SetDescriptionText(MaximDescription)
                .SetFlavorText(MaximDescription2)
                .SetIcon(null)
                .SetVisualParameters(null)
                .SetCost(1500)
                .SetIsNotable(true)
                .SetIsJunk(false)
                .SetDestructible(false)
                .SetType(guntype)
                .SetSize(Kingmaker.Enums.Size.Medium)
                .AddEquipmentRestrictionClass(ArchetypeGuid)
                .Configure();
            
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateGunsmith(), CreateGrit(), DodgeFeat())
                .AddToLevelEntry(2, CreateNimble())
                .AddToLevelEntry(3, CreateInitiative())
                .AddToLevelEntry(4, FeatureSelectionRefs.FighterFeatSelection.ToString())
                .AddToLevelEntry(5, CreateGunTraining())
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetLocalizedDescriptionShort(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(false)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(StatProgressionRefs.SavesHigh.Reference.Get())
                .SetReflexSave(StatProgressionRefs.SavesHigh.Reference.Get())
                .SetWillSave(StatProgressionRefs.SavesLow.Reference.Get())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillThievery, StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillPerception, StatType.SkillPersuasion })
                .SetRecommendedAttributes(StatType.Dexterity, StatType.Wisdom)
                .SetDifficulty(1)
                .AddToStartingItems(ItemWeaponRefs.ColdIronScimitar.Reference.Get(), ItemArmorRefs.LeatherStandard.Reference.Get(), maxim, ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get())
                .SetStartingGold(411)
                .AddPrerequisiteClassLevel(ArchetypeGuid, 5, not: true)
                .AddPrerequisiteNoClassLevel(CharacterClassRefs.AnimalClass.Reference.Get())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var Gunslingerref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = Gunslingerref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Gunsmith = "Gunslinger.Gunsmith";
        private static readonly string GunsmithGuid = "{29F72FD6-0A56-4F52-8997-00A0C5672D74}";

        internal const string GunsmithDisplayName = "GunslingerGunsmith.Name";
        private const string GunsmithDescription = "GunslingerGunsmith.Description";
        private static BlueprintFeature CreateGunsmith()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            return FeatureConfigurator.New(Gunsmith, GunsmithGuid)
              .SetDisplayName(GunsmithDisplayName)
              .SetDescription(GunsmithDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.ToString(), FeatureRefs.MartialWeaponProficiency.ToString(), FeatureRefs.LightArmorProficiency.ToString() })
              .Configure();
        }

        private const string GritFeature = "Gunslinger.Grit";
        private static readonly string GritFeatureGuid = "{689FA7B8-90ED-4B9A-89C9-83970FAC1F0D}";

        private const string GritResource = "Gunslinger.GritResource";
        private static readonly string GritResourceGuid = "{5E983BF8-BDE0-4FD5-B3CB-240B5A4B8BF5}";

        internal const string GritDisplayName = "GunslingerGrit.Name";
        private const string GritDescription = "GunslingerGrit.Description";

        private const string ConvertAblity1 = "Gunslinger.UseConvert";
        private static readonly string ConvertAblity1Guid = "{D8CE1AD5-0C85-4805-8669-799281586CFB}";

        internal const string GritDisplayName1 = "GunslingerGrit.Name1";
        private const string GritDescription1 = "GunslingerGrit.Description1";

        private const string ConvertAblity2 = "Gunslinger.UseConvert2";
        private static readonly string ConvertAblity2Guid = "{B1BAE0AB-950C-4EB7-B1AC-45DA0B6E6667}";

        internal const string GritDisplayName2 = "GunslingerGrit.Name2";
        private const string GritDescription2 = "GunslingerGrit.Description2";
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

            var ability2 = AbilityConfigurator.New(ConvertAblity2, ConvertAblity2)
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

            return FeatureConfigurator.New(GritFeature, GritFeatureGuid)
                .SetDisplayName(GritDisplayName)
                .SetDescription(GritDescription)
                .SetIcon(icon)
                .AddFacts(new() { ability1, ability2 })
                .AddAbilityResources(resource: res, restoreAmount: true)
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(res, 1), actionsOnInitiator: true, duelistWeapon: true, criticalHit: true)
                .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RestoreResource(res, 1), actionsOnInitiator: true, duelistWeapon: true, reduceHPToZero: true)
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
            var icon = AbilityRefs.ChargeAbility.Reference.Get().Icon;

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
        private static readonly string InitiativeGuid = "{03723CFE-2CA7-427D-BEEA-BCC351E9AE81}";

        internal const string InitiativeDisplayName = "GunslingerInitiative.Name";
        private const string InitiativeDescription = "GunslingerInitiative.Description";
        private static BlueprintFeature CreateInitiative()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

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
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            return FeatureConfigurator.New(GunTraining, GunTrainingGuid)
              .SetDisplayName(GunTrainingDisplayName)
              .SetDescription(GunTrainingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.HeavyRepeatingCrossbow, false, StatType.Dexterity, false)
              .Configure();
        }

        private const string Dodge = "Gunslinger.Dodge";
        private static readonly string DodgeGuid = "{E857E9B8-8A53-4915-9EFA-A71CD57E792C}";

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
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .AddACBonusAgainstAttacks(false, true, 2, notArmorCategory: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy })
                .AddTargetAttackWithWeaponTrigger(ActionsBuilder.New()
                        .ContextSpendResource(GritResourceGuid, 1)
                        .Build(), onlyHit: false, onlyRanged: true, waitForAttackResolve: true)
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
    }
}
