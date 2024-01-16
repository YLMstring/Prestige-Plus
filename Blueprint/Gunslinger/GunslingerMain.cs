using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using System;
using System.Collections.Generic;
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
        private const string MaximDescription = "GunslingerMaxim.Description";
        private const string MaximDescription2 = "GunslingerMaxim2.Description";

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

            var maxim = ItemWeaponConfigurator.New(MaximName, MaximGuid)
                .SetDisplayNameText(MaximDisplayName)
                .SetDescriptionText(MaximDescription)
                .SetFlavorText(MaximDescription2)
                .SetIcon(null)
                .SetCost(1500)
                .SetWeight(140)
                .SetIsNotable(true)
                .SetIsJunk(false)
                .SetDestructible(false)
                .SetType(WeaponTypeRefs.BombType.ToString())
                .SetSize(Size.Medium)
                .SetOverrideDamageDice(true)
                .SetDamageDice(new DiceFormula(2, DiceType.D8))
                .SetDamageType(damagetype)
                .AddEquipmentRestrictionClass(ArchetypeGuid)
                .Configure();
            
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1)
                .AddToLevelEntry(2, CreateControllCharge())
                .AddToLevelEntry(3, FeatureRefs.PoisonImmunity.ToString())
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

        private const string Reckless = "Gunslinger.Reckless";
        private static readonly string RecklessGuid = "{8E229789-C76E-4FDB-AE0D-3532BEF28BF3}";

        private const string RecklessBuff = "Gunslinger.RecklessBuff";
        private static readonly string RecklessGuidBuff = "{3574A095-46EC-4F8E-AFD8-9253258C6A49}";

        internal const string RecklessDisplayName = "GunslingerReckless.Name";
        private const string RecklessDescription = "GunslingerReckless.Description";
        private static BlueprintFeature CreateReckless()
        {
            var icon = AbilityRefs.ChargeAbility.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(RecklessBuff, RecklessGuidBuff)
              .SetDisplayName(RecklessDisplayName)
              .SetDescription(RecklessDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(ContextValues.Rank(), descriptor: ModifierDescriptor.Morale)
              //.AddContextStatBonus(StatType.AdditionalAttackBonus, ContextValues.Rank(), ModifierDescriptor.Morale)
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

        private const string ControllCharge = "Gunslinger.ControllCharge";
        private static readonly string ControllChargeGuid = "{48AF1EDA-5A68-4C0F-B62F-7F4F9A7A721C}";

        private const string ControllChargeBuff = "Gunslinger.ControllChargeBuff";
        private static readonly string ControllChargeGuidBuff = "{1AE0EC6C-3E58-4309-8B49-17CDD0EFD65B}";

        internal const string ControllChargeDisplayName = "GunslingerControllCharge.Name";
        private const string ControllChargeDescription = "GunslingerControllCharge.Description";
        private static BlueprintFeature CreateControllCharge()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ControllChargeBuff, ControllChargeGuidBuff)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              //.AddACBonusAgainstAttacks(armorClassBonus: 2)
              .AddContextStatBonus(StatType.AC, value: 2)
              .Configure();

            return FeatureConfigurator.New(ControllCharge, ControllChargeGuid)
              .SetDisplayName(ControllChargeDisplayName)
              .SetDescription(ControllChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.ChargeBuff.ToString(), extraEffectBuff: Buff1)
              .Configure();
        }
    }
}
