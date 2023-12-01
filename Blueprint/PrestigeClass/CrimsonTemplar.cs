using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
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
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomComponent.Archetype;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class CrimsonTemplar
    {
        private const string ArchetypeName = "CrimsonTemplar";
        public static readonly string ArchetypeGuid = "{0FCFB57B-28C0-4DFE-AE60-7B6B8CDD3C68}";
        internal const string ArchetypeDisplayName = "CrimsonTemplar.Name";
        private const string ArchetypeDescription = "CrimsonTemplar.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "CrimsonTemplarPrestige";
        private static readonly string ClassProgressGuid = "{CAB1654D-9F7E-4BC4-A1BA-444197C7225D}";

        public static void Configure()
        {
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, FiendishStudiesFeat(), ObedienceFeatFeat(), ShieldWingsFeat())
            .AddToLevelEntry(2, FiendishStudies2Feat(), RuthlessnessFeat(), FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(3, BonusFeatGuid, ShieldWings3Feat())
            .AddToLevelEntry(4, DeificObedience.Ragathiel1Guid, HeavenlyFireConfigure())
            .AddToLevelEntry(5, FiendishStudies5Feat(), FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(6, BonusFeatGuid, ShieldWings6Feat())
            .AddToLevelEntry(7, DeificObedience.Ragathiel2Guid)
            .AddToLevelEntry(8, FeatureRefs.SneakAttack.ToString())
            .AddToLevelEntry(9, BonusFeatGuid, ShieldWings9Feat())
            .AddToLevelEntry(10, DeificObedience.Ragathiel3Guid, FiendishStudies10Feat())
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName("")
            .SetDescription(ArchetypeDescription)
            .Configure();

            BlueprintCharacterClass archetype =
          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName(ArchetypeDisplayName)
            .SetLocalizedDescription(ArchetypeDescription)
            .SetSkillPoints(2)
            .SetHitDie(DiceType.D10)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(BABFull)
            .SetFortitudeSave(SavesPrestigeHigh)
            .SetReflexSave(SavesPrestigeHigh)
            .SetWillSave(SavesPrestigeLow)
            .SetProgression(progression)
            .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPerception })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
            .AddPrerequisiteFeature(FeatureRefs.BastardSwordProficiency.ToString())
            .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
            .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
            .AddPrerequisiteFeature("F79778D7-281C-4B9D-8E77-8F86812707AA", group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteAlignment(Kingmaker.UnitLogic.Alignments.AlignmentMaskType.LawfulGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var CrimsonTemplarref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = CrimsonTemplarref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string BonusFeat = "CrimsonTemplar.BonusFeat";
        private static readonly string BonusFeatGuid = "{AD9CEF5D-AB20-440C-A0E9-98D52322544A}";

        internal const string BonusFeatDisplayName = "CrimsonTemplarBonusFeat.Name";
        private const string BonusFeatDescription = "CrimsonTemplarBonusFeat.Description";

        public static BlueprintFeatureSelection BonusFeatFeat()
        {
            return FeatureSelectionConfigurator.New(BonusFeat, BonusFeatGuid)
              .SetDisplayName(BonusFeatDisplayName)
              .SetDescription(BonusFeatDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddPrerequisiteFeature(DeificObedience.Ragathiel0Guid)
              .SetGroup(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string ObedienceFeat = "CrimsonTemplar.ObedienceFeat";
        private static readonly string ObedienceFeatGuid = "{AD9CEF5D-AB20-440C-A0E9-98D52322544A}";

        internal const string ObedienceFeatDisplayName = "CrimsonTemplarObedienceFeat.Name";
        private const string ObedienceFeatDescription = "CrimsonTemplarObedienceFeat.Description";

        public static BlueprintFeatureSelection ObedienceFeatFeat()
        {
            return FeatureSelectionConfigurator.New(ObedienceFeat, ObedienceFeatGuid)
              .SetDisplayName(ObedienceFeatDisplayName)
              .SetDescription(ObedienceFeatDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(DeificObedience.RagathielGuid)
              .AddToAllFeatures(BonusFeatFeat())
              .AddFacts(new() { DeificObedience.DeificObedienceGuid })
              .Configure();
        }

        private const string ShieldWings = "CrimsonTemplar.ShieldWings";
        private static readonly string ShieldWingsGuid = "{C0C8E7B3-1813-45F2-9FD0-F734FA8A9E17}";

        internal const string CrimsonTemplarShieldWingsDisplayName = "CrimsonTemplarShieldWings.Name";
        private const string CrimsonTemplarShieldWingsDescription = "CrimsonTemplarShieldWings.Description";
        public static BlueprintFeature ShieldWingsFeat()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureConfigurator.New(ShieldWings, ShieldWingsGuid)
              .SetDisplayName(CrimsonTemplarShieldWingsDisplayName)
              .SetDescription(CrimsonTemplarShieldWingsDescription)
              .SetIcon(icon)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(5), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .Configure();
        }

        private const string ShieldWings3 = "CrimsonTemplar.ShieldWings3";
        private static readonly string ShieldWings3Guid = "{14821ECC-B4EB-4114-98FF-476553C8BB75}";

        public static BlueprintFeature ShieldWings3Feat()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureConfigurator.New(ShieldWings3, ShieldWings3Guid)
              .SetDisplayName(CrimsonTemplarShieldWingsDisplayName)
              .SetDescription(CrimsonTemplarShieldWingsDescription)
              .SetIcon(icon)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(10), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .Configure();
        }

        private const string ShieldWings6 = "CrimsonTemplar.ShieldWings6";
        private static readonly string ShieldWings6Guid = "{829D76AD-4582-4D44-83EF-C365A47F55C8}";
        public static BlueprintFeature ShieldWings6Feat()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureConfigurator.New(ShieldWings6, ShieldWings6Guid)
              .SetDisplayName(CrimsonTemplarShieldWingsDisplayName)
              .SetDescription(CrimsonTemplarShieldWingsDescription)
              .SetIcon(icon)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Constant(30), type: Kingmaker.Enums.Damage.DamageEnergyType.Fire)
              .Configure();
        }

        private const string ShieldWings9 = "CrimsonTemplar.ShieldWings9";
        private static readonly string ShieldWings9Guid = "{CC8B78BA-B6FB-4BBF-855D-46CD867BD3C5}";
        public static BlueprintFeature ShieldWings9Feat()
        {
            var icon = FeatureRefs.Diehard.Reference.Get().Icon;
            return FeatureConfigurator.New(ShieldWings9, ShieldWings9Guid)
              .SetDisplayName(CrimsonTemplarShieldWingsDisplayName)
              .SetDescription(CrimsonTemplarShieldWingsDescription)
              .SetIcon(icon)
              .AddEnergyDamageImmunity(Kingmaker.Enums.Damage.DamageEnergyType.Fire, false)
              .Configure();
        }

        private static readonly string RuthlessnessFeatName = "Ruthlessness";
        public static readonly string RuthlessnessFeatGuid = "{2CDA0A2A-725F-4C3B-AC0D-4E69C9E8982D}";

        private static readonly string RuthlessnessDisplayName = "CrimsonTemplarRuthlessness.Name";
        private static readonly string RuthlessnessDescription = "CrimsonTemplarRuthlessness.Description";

        private const string RuthlessnessAbility = "CrimsonTemplar.RuthlessnessAbility";
        private static readonly string RuthlessnessAbilityGuid = "{9F227FC3-8C09-4AFF-B47E-8D22D501CC16}";

        public static BlueprintFeature RuthlessnessFeat()
        {
            var icon = FeatureRefs.MadDogThroatCutter.Reference.Get().Icon;

            var sword = ActionsBuilder.New()
                .Conditional(ConditionsBuilder.New().CasterHasFact(ThroatSlicer.FeatGuid).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(ThroatSlicer.ThroatSlicerbuffGuid, ContextDuration.Fixed(1), toCaster: true)
                        .Build())
                .CastSpell(AbilityRefs.CoupDeGraceAbility.ToString())
                .Build();

            var abilityTrick = AbilityConfigurator.New(RuthlessnessAbility, RuthlessnessAbilityGuid)
                .SetDisplayName(RuthlessnessDisplayName)
                .SetDescription(RuthlessnessDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(sword)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.CoupDeGrace)
                .SetRange(AbilityRange.Weapon)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.BastardSword })
                .Configure();

            return FeatureConfigurator.New(RuthlessnessFeatName, RuthlessnessFeatGuid)
                    .SetDisplayName(RuthlessnessDisplayName)
                    .SetDescription(RuthlessnessDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { abilityTrick })
                    .Configure();
        }

        private static readonly string HeavenlyFireName = "CrimsonTemplarHeavenlyFire";
        public static readonly string HeavenlyFireGuid = "{2314BD61-13F9-46B6-9A4C-CFA406E4F7E3}";

        private static readonly string HeavenlyFireDisplayName = "CrimsonTemplarHeavenlyFire.Name";
        private static readonly string HeavenlyFireDescription = "CrimsonTemplarHeavenlyFire.Description";

        private const string HeavenlyFireBuff = "CrimsonTemplarStyle.HeavenlyFirebuff";
        private static readonly string HeavenlyFireBuffGuid = "{AF5CF3D3-E4B4-40D8-8954-08A03FEEC6AF}";

        private const string HeavenlyFireAbility = "CrimsonTemplarStyle.HeavenlyFireAbility";
        private static readonly string HeavenlyFireAbilityGuid = "{1247797E-0A66-4C2F-B49D-1F7618F7424B}";

        private const string HeavenlyFireAbilityRes = "CrimsonTemplarStyle.HeavenlyFireAbilityRes";
        public static readonly string HeavenlyFireAbilityResGuid = "{7735FCE6-B80C-486A-96FA-1C4A414382A5}";

        public static BlueprintFeature HeavenlyFireConfigure()
        {
            var icon = AbilityRefs.ArmyMarkOfHeaven.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(HeavenlyFireAbilityRes, HeavenlyFireAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }))
                .SetUseMax()
                .SetMax(10)
                .Configure();

            var Buff1 = BuffConfigurator.New(HeavenlyFireBuff, HeavenlyFireBuffGuid)
              .SetDisplayName(HeavenlyFireDisplayName)
              .SetDescription(HeavenlyFireDescription)
              .SetIcon(icon)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .AddComponent<TemplarHeavenlyFire>()
              .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = abilityresourse.ToReference<BlueprintAbilityResourceReference>(); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(HeavenlyFireAbility, HeavenlyFireAbilityGuid)
                .SetDisplayName(HeavenlyFireDisplayName)
                .SetDescription(HeavenlyFireDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                //.SetIsOnByDefault(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(HeavenlyFireName, HeavenlyFireGuid)
                    .SetDisplayName(HeavenlyFireDisplayName)
                    .SetDescription(HeavenlyFireDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private const string FiendishStudies = "CrimsonTemplar.FiendishStudies";
        private static readonly string FiendishStudiesGuid = "{57692F7F-9719-4C9F-AA5F-36A05942C27C}";

        private const string FiendishStudiesAbility1 = "CrimsonTemplar.UseFiendishStudies1";
        private static readonly string FiendishStudiesAbilityGuid1 = "{9EBF85AC-D49B-4EDF-9FC5-4A8037E57D24}";

        private const string FiendishStudiesAbility2 = "CrimsonTemplar.UseFiendishStudies2";
        private static readonly string FiendishStudiesAbilityGuid2 = "{EB7BBF99-DFB4-4111-B1B8-BF4BCD44FFF2}";

        private const string FiendishStudiesBuff = "CrimsonTemplar.FiendishStudiesBuff";
        public static readonly string FiendishStudiesBuffGuid = "{0E676E84-BCA3-4437-9CEB-D63751BBA2DD}";

        internal const string FiendishStudiesDisplayName = "CrimsonTemplarFiendishStudies.Name";
        private const string FiendishStudiesDescription = "CrimsonTemplarFiendishStudies.Description";
        public static BlueprintFeature FiendishStudiesFeat()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(FiendishStudiesBuff, FiendishStudiesBuffGuid)
             .SetDisplayName(FiendishStudiesDisplayName)
             .SetDescription(FiendishStudiesDescription)
             .SetIcon(icon)
             .Configure();

            var action = ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build();

            var ability1 = AbilityConfigurator.New(FiendishStudiesAbility1, FiendishStudiesAbilityGuid1)
                .CopyFrom(
                AbilityRefs.SlayerStudyTargetAbility)
                .AddAbilityEffectRunAction(action)
                .SetDisplayName(FiendishStudiesDisplayName)
                .SetDescription(FiendishStudiesDescription)
                .SetIcon(icon)
                .AddAbilityTargetHasFact(new() { FeatureRefs.SubtypeEvil.ToString() })
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .Configure();

            AbilityConfigurator.New(FiendishStudiesAbility2, FiendishStudiesAbilityGuid2)
                .CopyFrom(
                AbilityRefs.SlayerStudyTargetAbility)
                .AddAbilityEffectRunAction(action)
                .SetDisplayName(FiendishStudiesDisplayName)
                .SetDescription(FiendishStudiesDescription)
                .SetIcon(icon)
                .AddAbilityTargetHasFact(new() { FeatureRefs.SubtypeEvil.ToString() })
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(FiendishStudies, FiendishStudiesGuid)
              .SetDisplayName(FiendishStudiesDisplayName)
              .SetDescription(FiendishStudiesDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability1 })
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().CastSpell(ability1).Build(), onlyHit: true, onlySneakAttack: true)
              .AddAttackBonusAgainstFactOwner(2, checkedFact: Buff1)
              .AddDamageBonusAgainstFactOwner(2, checkedFact: Buff1)
              .Configure();
        }

        private const string FiendishStudies5 = "CrimsonTemplar.FiendishStudies5";
        private static readonly string FiendishStudies5Guid = "{96012AA0-3497-4D78-9892-8DAEC8E03F61}";

        internal const string CrimsonTemplarFiendishStudies5DisplayName = "CrimsonTemplarFiendishStudies5.Name";
        private const string CrimsonTemplarFiendishStudies5Description = "CrimsonTemplarFiendishStudies5.Description";
        public static BlueprintFeature FiendishStudies5Feat()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(FiendishStudies5, FiendishStudies5Guid)
              .SetDisplayName(CrimsonTemplarFiendishStudies5DisplayName)
              .SetDescription(CrimsonTemplarFiendishStudies5Description)
              .SetIcon(icon)
              .AddAttackBonusAgainstFactOwner(2, checkedFact: FiendishStudiesBuffGuid)
              .AddDamageBonusAgainstFactOwner(2, checkedFact: FiendishStudiesBuffGuid)
              .Configure();
        }

        private const string FiendishStudies10 = "CrimsonTemplar.FiendishStudies10";
        private static readonly string FiendishStudies10Guid = "{C0B7F3B8-4974-43B8-A699-E5516D5350EF}";

        internal const string CrimsonTemplarFiendishStudies10DisplayName = "CrimsonTemplarFiendishStudies10.Name";
        private const string CrimsonTemplarFiendishStudies10Description = "CrimsonTemplarFiendishStudies10.Description";
        public static BlueprintFeature FiendishStudies10Feat()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(FiendishStudies10, FiendishStudies10Guid)
              .SetDisplayName(CrimsonTemplarFiendishStudies10DisplayName)
              .SetDescription(CrimsonTemplarFiendishStudies10Description)
              .SetIcon(icon)
              .AddAttackBonusAgainstFactOwner(2, checkedFact: FiendishStudiesBuffGuid)
              .AddDamageBonusAgainstFactOwner(2, checkedFact: FiendishStudiesBuffGuid)
              .Configure();
        }

        private const string FiendishStudies2 = "CrimsonTemplar.FiendishStudies2";
        private static readonly string FiendishStudies2Guid = "{C6241715-6ACB-4486-ABA2-0CE3D5BB16DC}";

        internal const string CrimsonTemplarFiendishStudies2DisplayName = "CrimsonTemplarFiendishStudies2.Name";
        private const string CrimsonTemplarFiendishStudies2Description = "CrimsonTemplarFiendishStudies2.Description";
        public static BlueprintFeature FiendishStudies2Feat()
        {
            var icon = AbilityRefs.SlayerStudyTargetAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(FiendishStudies2, FiendishStudies2Guid)
              .SetDisplayName(CrimsonTemplarFiendishStudies2DisplayName)
              .SetDescription(CrimsonTemplarFiendishStudies2Description)
              .SetIcon(icon)
              .AddFacts(new() { FiendishStudiesAbilityGuid2 })
              .Configure();
        }
    }
}
