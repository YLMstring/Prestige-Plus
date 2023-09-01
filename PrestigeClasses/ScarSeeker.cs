using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
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
using Kingmaker.AreaLogic.SummonPool;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using static Dreamteck.Splines.SplineMesh;

namespace PrestigePlus.PrestigeClasses
{
    internal class ScarSeeker
    {
        private const string ArchetypeName = "ScarSeeker";
        private static readonly string ArchetypeGuid = "{A9827D49-8599-4525-B763-0E4554DCC1A0}";
        internal const string ArchetypeDisplayName = "ScarSeeker.Name";
        private const string ArchetypeDescription = "ScarSeeker.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "ScarSeekerPrestige";
        private static readonly string ClassProgressGuid = "{9A7C75F8-529F-48AA-8702-CD2AB693EA68}";

        private static readonly string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

        public static void Configure()
        {
            EnduringScarFeat();
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, EnduringScarGuid, PurifyFeat(), spellupgradeGuid, FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString())
            .AddToLevelEntry(2, FeatureRefs.LayOnHandsFeature.ToString())
            .AddToLevelEntry(3, EnduringScarGuid)
            .AddToLevelEntry(4, SSChampionHonor())
            .AddToLevelEntry(5, EnduringScarGuid)
            .AddToLevelEntry(6, PurifyImprovedFeat())
            .AddToLevelEntry(7, EnduringScarGuid)
            .AddToLevelEntry(8, FeatureRefs.SmiteEvilAdditionalUse.ToString())
            .AddToLevelEntry(9, EnduringScarGuid, PurifyGreaterFeat())
            .AddToLevelEntry(10, TrueMFeat())
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName(ArchetypeDisplayName)
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
            .SetReflexSave(SavesPrestigeLow)
            .SetWillSave(SavesPrestigeHigh)
            .SetProgression(progression)
            .AddSkipLevelsForSpellProgression(new int[] { 3, 5, 7, 9 })
            .SetClassSkills(new StatType[] { StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillKnowledgeArcana, StatType.SkillPersuasion, StatType.SkillPerception })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 3)
            .AddPrerequisiteStatValue(StatType.SkillLoreNature, 3)
            .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
            .AddPrerequisiteFeature(FeatureRefs.Toughness.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.All)
            .AddPrerequisiteAlignment(AlignmentMaskType.LawfulGood, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteAlignment(AlignmentMaskType.NeutralGood, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, checkInProgression: true, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
            .Configure();

        Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var ScarSeekerref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = ScarSeekerref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);

            ///put healing in here

            var damage = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Rank()), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine))
                .RemoveSelf()
                .Build();

            var Buff1 = BuffConfigurator.New(HealingBuff, HealingBuffGuid)
                .SetDisplayName(HealingDisplayName)
                .SetDescription(HealingDescription)
                .AddBuffActions(activated: damage)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] {CharacterClassRefs.PaladinClass.ToString(), ArchetypeGuid}).WithDiv2Progression())
                .Configure();

            var purify = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(HealingGuid).CasterHasFact(Healing2BuffGuid).Build(), ifTrue: ActionsBuilder.New()
                    .HealTarget(ContextDice.Value(diceType: DiceType.D6, diceCount: ContextValues.Rank()))
                    .ContextSpendResource(EnduringScarAblityResGuid, ContextValues.Constant(1))
                    .ApplyBuffPermanent(Buff1, toCaster: true)
                    .Build())
                .Build();

            AbilityConfigurator.For(AbilityRefs.LayOnHandsOthers)
              .EditComponent<AbilityEffectRunAction>(
                a => a.Actions.Actions = CommonTool.Append(a.Actions.Actions, purify.Actions))
              .EditComponent<ContextRankConfig>(
                a => a.m_Class = CommonTool.Append(a.m_Class, archetype.ToReference<BlueprintCharacterClassReference>()))
              .Configure();

            AbilityConfigurator.For(AbilityRefs.LayOnHandsSelf)
              .EditComponent<ContextRankConfig>(
                a => a.m_Class = CommonTool.Append(a.m_Class, archetype.ToReference<BlueprintCharacterClassReference>()))
              .Configure();

            AbilityResourceConfigurator.For(AbilityResourceRefs.LayOnHandsResource)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid, CharacterClassRefs.PaladinClass.ToString() }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 0)
                        .IncreaseByStat(stat: StatType.Charisma))
              .Configure();
        }

        private const string EnduringScar = "ScarSeeker.EnduringScar";
        private static readonly string EnduringScarGuid = "{C9AD9C94-25C2-466A-A33C-1B386DA41BB9}";

        internal const string EnduringScarDisplayName = "EnduringScar.Name";
        private const string EnduringScarDescription = "EnduringScar.Description";

        private const string EnduringScarAblityRes = "EnduringScarAblityRes";
        private static readonly string EnduringScarAblityResGuid = "{CDE7679C-3AE2-48E6-8103-7ECCAF3EB9BC}";
        public static void EnduringScarFeat()
        {
            var icon = AbilityRefs.BloodBoilerAbility.Reference.Get().Icon;

            BlueprintAbilityResource Scarabilityresourse = AbilityResourceConfigurator.New(EnduringScarAblityRes, EnduringScarAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1, startingLevel: 0)
                        .IncreaseByStat(stat: StatType.Charisma))
                .Configure();

            FeatureSelectionConfigurator.New(EnduringScar, EnduringScarGuid)
              .SetDisplayName(EnduringScarDisplayName)
              .SetDescription(EnduringScarDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(BloodFeat())
              .AddToAllFeatures(BloodImprovedFeat())
              .AddToAllFeatures(BloodGreaterFeat())
              .AddToAllFeatures(HealingFeat())
              .AddToAllFeatures(MartyrFeat())
              .AddToAllFeatures(MercyFeat())
              .AddToAllFeatures(SacrificeFeat())
              .AddToAllFeatures(SufferingFeat())
              .AddToAllFeatures(SufferingImprovedFeat())
              .AddToAllFeatures(TenacityFeat())
              .AddAbilityResources(resource: Scarabilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Blood = "ScarSeeker.Blood";
        private static readonly string BloodGuid = "{D1025DD5-2965-40C5-A041-F928B4757901}";

        private const string BloodAblity = "ScarSeeker.UseBlood";
        private static readonly string BloodAblityGuid = "{8A2DC5F3-A529-4FBD-9ED3-EBA6DE968BEC}";

        private const string BloodBuff = "ScarSeeker.BloodBuff";
        private static readonly string BloodBuffGuid = "{DAE72E0A-9F44-4CD3-A26E-3D48DFEB23A1}";

        internal const string BloodDisplayName = "ScarSeekerBlood.Name";
        private const string BloodDescription = "ScarSeekerBlood.Description";
        public static BlueprintFeature BloodFeat()
        {
            var icon = FeatureRefs.BleedingInfusionFeature.Reference.Get().Icon;

            var VicousSelf = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(1)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BloodImprovedGuid).Build(),   
                    ifTrue: ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(1)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                        .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BloodGreaterGuid).Build(),
                    ifTrue: ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(1)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                        .Build())
                .Build();

            var VicousEnemy = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(2)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BloodImprovedGuid).Build(),
                    ifTrue: ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(2)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                        .Build())
                .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(BloodGreaterGuid).Build(),
                    ifTrue: ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(2)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Magic))
                        .Build())
                .Build();

            var Buff1 = BuffConfigurator.New(BloodBuff, BloodBuffGuid)
             .SetDisplayName(BloodDisplayName)
             .SetDescription(BloodDescription)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: VicousEnemy, actionsOnInitiator: false, triggerBeforeAttack: false, onlyHit: true, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee)
             .AddInitiatorAttackWithWeaponTrigger(action: VicousSelf, actionsOnInitiator: true, triggerBeforeAttack: false, onlyHit: true, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee)
             .Configure();

            var ability = AbilityConfigurator.New(BloodAblity, BloodAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Buff1).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank()))
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff1)
                        .RestoreResource(EnduringScarAblityResGuid, 1)
                        .Build())
                    .Build())
                .SetDisplayName(BloodDisplayName)
                .SetDescription(BloodDescription)
                .SetIcon(icon)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Charisma, min: 1))
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnduringScarAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Blood, BloodGuid)
              .SetDisplayName(BloodDisplayName)
              .SetDescription(BloodDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: EnduringScarAblityResGuid, restoreAmount: true)
              .Configure();
        }

        private const string BloodImproved = "ScarSeeker.BloodImproved";
        private static readonly string BloodImprovedGuid = "{0655DD6E-DE33-4CA1-A8F8-5EF4C0B32A08}";

        public static BlueprintFeature BloodImprovedFeat()
        {
            var icon = FeatureRefs.BleedingInfusionFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(BloodImproved, BloodImprovedGuid)
              .SetDisplayName(BloodDisplayName)
              .SetDescription(BloodDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 5)
              .AddPrerequisiteFeature(BloodGuid)
              .Configure();
        }

        private const string BloodGreater = "ScarSeeker.BloodGreater";
        private static readonly string BloodGreaterGuid = "{987EC280-2304-413D-8809-274B1E5C9557}";


        public static BlueprintFeature BloodGreaterFeat()
        {
            var icon = FeatureRefs.BleedingInfusionFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(BloodGreater, BloodGreaterGuid)
              .SetDisplayName(BloodDisplayName)
              .SetDescription(BloodDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 9)
              .AddPrerequisiteFeature(BloodImprovedGuid)
              .Configure();
        }

        private const string Healing = "ScarSeeker.Healing";
        private static readonly string HealingGuid = "{3964263A-93A0-417E-ACF9-A7740E8431BE}";

        internal const string HealingDisplayName = "ScarSeekerHealing.Name";
        private const string HealingDescription = "ScarSeekerHealing.Description";

        private const string HealingBuff = "ScarSeeker.HealingBuff";
        private static readonly string HealingBuffGuid = "{D5C4FE81-DFAF-44F0-8C83-A79ACF29F482}";

        private const string Healing2Buff = "ScarSeeker.Healing2Buff";
        private static readonly string Healing2BuffGuid = "{EF1E02C1-E1A4-4838-8D28-7CFC928D89DF}";

        private const string HealingAblity = "ScarSeeker.UseHealing";
        private static readonly string HealingAblityGuid = "{6B5B056C-E172-4FC4-B704-F930B1CA7232}";

        public static BlueprintFeature HealingFeat()
        {
            var icon = AbilityRefs.LayOnHandsOthers.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Healing2Buff, Healing2BuffGuid)
                .SetDisplayName(HealingDisplayName)
                .SetDescription(HealingDescription)
                .SetIcon(icon)
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("{CDE7679C-3AE2-48E6-8103-7ECCAF3EB9BC}"); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(HealingAblity, HealingAblityGuid)
                .SetDisplayName(HealingDisplayName)
                .SetDescription(HealingDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: EnduringScarAblityResGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(Buff2)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(Healing, HealingGuid)
              .SetDisplayName(HealingDisplayName)
              .SetDescription(HealingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Martyr = "ScarSeeker.Martyr";
        private static readonly string MartyrGuid = "{0F8EA66B-01FB-47C7-B238-00313A4F84F4}";

        internal const string MartyrDisplayName = "ScarSeekerMartyr.Name";
        private const string MartyrDescription = "ScarSeekerMartyr.Description";

        private const string MartyrBuff = "ScarSeeker.MartyrBuff";
        private static readonly string MartyrBuffGuid = "{D66076FB-62A4-42BC-8600-5FA93DC14545}";

        private const string Martyr2Buff = "ScarSeeker.Martyr2Buff";
        private static readonly string Martyr2BuffGuid = "{2B39CD25-71CE-4EFE-A08D-53914C74C094}";

        private const string MartyrAblity = "ScarSeeker.UseMartyr";
        private static readonly string MartyrAblityGuid = "{46A1C325-332E-4D1F-89B3-F5BD51073DF3}";

        private const string Martyr2Ablity = "ScarSeeker.UseMartyr2";
        private static readonly string Martyr2AblityGuid = "{62FD011A-746D-48A9-B101-B2AE1488D349}";

        private const string Martyr3Ablity = "ScarSeeker.UseMartyr3";
        private static readonly string Martyr3AblityGuid = "{54BEFE9A-84D6-4DE2-B042-E151C4F3C6AD}";

        private const string layonhandres = "9dedf41d995ff4446a181f143c3db98c";
        public static BlueprintFeature MartyrFeat()
        {
            var icon = FeatureRefs.AuraOfRighteousnessFeature.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(Martyr2Buff, Martyr2BuffGuid)
                .SetDisplayName(MartyrDisplayName)
                .SetDescription(MartyrDescription)
                .SetIcon(icon)
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = AbilityResourceRefs.LayOnHandsResource.Reference.Get().ToReference<BlueprintAbilityResourceReference>(); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var ability3 = ActivatableAbilityConfigurator.New(Martyr3Ablity, Martyr3AblityGuid)
                .SetDisplayName(MartyrDisplayName)
                .SetDescription(MartyrDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: layonhandres, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(Buff2)
                .SetDeactivateImmediately()
                .Configure();

            var ability2 = AbilityConfigurator.New(Martyr2Ablity, Martyr2AblityGuid)
                .CopyFrom(
                AbilityRefs.ChannelEnergy,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().IsEnemy().Build(),
                    ifTrue: ActionsBuilder.New()
                        .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Evil).Build(),
                        ifTrue: ActionsBuilder.New()
                            .DealDamage(value: ContextDice.Value(DiceType.D6, diceCount: ContextValues.Constant(1)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine), halfIfSaved: true)
                            .Build())
                        .Build(),
                    ifFalse: ActionsBuilder.New()
                        .HealTarget(ContextDice.Value(diceType: DiceType.D6, diceCount: ContextValues.Constant(1)))
                        .Build())
                    .Conditional(conditions: ConditionsBuilder.New().IsCaster().Build(),
                    ifTrue: ActionsBuilder.New()
                            .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff2).Build(),
                            ifTrue: ActionsBuilder.New()
                                .ContextSpendResource(layonhandres, ContextValues.Constant(1))
                                .CastSpell(Martyr2AblityGuid)
                                .Build())
                            .Build())
                    .Build(), savingThrowType: SavingThrowType.Will)
                .SetDisplayName(MartyrDisplayName)
                .SetDescription(MartyrDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Any, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.DamageBonus))
                .AddComponent<CustomDC>()
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var ability = AbilityConfigurator.New(MartyrAblity, MartyrAblityGuid)
                .CopyFrom(
                AbilityRefs.ChannelEnergy,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Omni)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().IsEnemy().Build(),
                    ifTrue: ActionsBuilder.New()
                        .Conditional(conditions: ConditionsBuilder.New().Alignment(AlignmentComponent.Evil).Build(),
                        ifTrue: ActionsBuilder.New()
                            .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Rank(type: AbilityRankType.DamageBonus), diceCount: ContextValues.Constant(4)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine), halfIfSaved: true)
                            .Build())
                        .Build(),
                    ifFalse: ActionsBuilder.New()
                        .HealTarget(ContextDice.Value(diceType: DiceType.D6, bonus: ContextValues.Rank(type: AbilityRankType.DamageBonus), diceCount: ContextValues.Constant(4)))
                        .Build())
                    .Conditional(conditions: ConditionsBuilder.New().IsCaster().Build(),
                    ifTrue: ActionsBuilder.New()
                            .ContextSpendResource(layonhandres, ContextValues.Constant(1))
                            .CastSpell(ability2)
                            .Build())
                    .Build(), savingThrowType: SavingThrowType.Will)
                .SetDisplayName(MartyrDisplayName)
                .SetDescription(MartyrDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Any, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.DamageBonus))
                .AddComponent<CustomDC>()
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            var martyr = ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff2).Build(),
                    ifTrue: ActionsBuilder.New()
                        ///.ContextSpendResource(EnduringScarAblityResGuid, ContextValues.Constant(1))
                        .CastSpell(MartyrAblityGuid)
                        .Build())
                    .Build();

            var Buff1 = BuffConfigurator.New(MartyrBuff, MartyrBuffGuid)
             .SetDisplayName(MartyrDisplayName)
             .SetDescription(MartyrDescription)
             .SetIcon(icon)
             .AddBuffActions(activated: martyr)
             .Configure();

            return FeatureConfigurator.New(Martyr, MartyrGuid)
              .SetDisplayName(MartyrDisplayName)
              .SetDescription(MartyrDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddIncomingDamageTrigger(reduceBelowZero: true, actions: martyr, actionsOnInitiator: false)
              .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>() {ability3})
              .Configure();
        }

        private const string Mercy = "ScarSeeker.Mercy";
        private static readonly string MercyGuid = "{B32A86C2-821F-4CF7-9C68-C833F5F13B21}";

        internal const string MercyDisplayName = "ScarSeekerMercy.Name";
        private const string MercyDescription = "ScarSeekerMercy.Description";
        public static BlueprintProgression MercyFeat()
        {
            var icon = FeatureRefs.AuraOfFaithFeature.Reference.Get().Icon;
            return ProgressionConfigurator.New(Mercy, MercyGuid)
              .SetDisplayName(MercyDisplayName)
              .SetDescription(MercyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(HealingGuid)
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.PaladinClass.ToString(), modifier: 1, summand: 0)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, FeatureSelectionRefs.SelectionMercy.ToString())
              .AddToLevelEntry(2, FeatureSelectionRefs.SelectionMercy.ToString())
              .AddToLevelEntry(3, FeatureSelectionRefs.SelectionMercy.ToString())
              ///.AddFeatureOnApply(FeatureSelectionRefs.SelectionMercy.ToString())
              .Configure();
        }

        private const string Sacrifice = "ScarSeeker.Sacrifice";
        private static readonly string SacrificeGuid = "{5E36775A-B86F-419A-8ACE-1E58A104B913}";

        internal const string SacrificeDisplayName = "ScarSeekerSacrifice.Name";
        private const string SacrificeDescription = "ScarSeekerSacrifice.Description";

        private const string SacrificeBuff = "ScarSeeker.SacrificeBuff";
        private static readonly string SacrificeBuffGuid = "{5A57038D-7FE6-46CD-8117-4DE2CC17688D}";

        private const string Sacrifice2Buff = "ScarSeeker.Sacrifice2Buff";
        private static readonly string Sacrifice2BuffGuid = "{7200D413-D37F-4505-9127-AC4E8A963797}";

        private const string SacrificeAblity = "ScarSeeker.UseSacrifice";
        private static readonly string SacrificeAblityGuid = "{338C175B-4EEC-493A-A990-A2F16E1B5CE5}";

        private const string Sacrifice3Buff = "ScarSeeker.UseSacrifice2";
        private static readonly string Sacrifice3BuffGuid = "{2C267092-076B-47B4-AA41-1C683F46EB2E}";

        private const string Sacrifice3Ablity = "ScarSeeker.UseSacrifice3";
        private static readonly string Sacrifice3AblityGuid = "{39773764-4538-4A14-864B-7E87BDF007E2}";

        private const string Sacrifice4Buff = "ScarSeeker.Sacrifice4Buff";
        private static readonly string Sacrifice4BuffGuid = "{726C7558-B709-4DC3-B88B-92001D4B3744}";

        public static BlueprintFeature SacrificeFeat()
        {
            var icon = FeatureRefs.AuraOfCourageFeature.Reference.Get().Icon;

            var ActivatableBuff = BuffConfigurator.New(Sacrifice2Buff, Sacrifice2BuffGuid)
                .SetDisplayName(SacrificeDisplayName)
                .SetDescription(SacrificeDescription)
                .SetIcon(icon)
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("{CDE7679C-3AE2-48E6-8103-7ECCAF3EB9BC}"); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var ability3 = ActivatableAbilityConfigurator.New(Sacrifice3Ablity, Sacrifice3AblityGuid)
                .SetDisplayName(SacrificeDisplayName)
                .SetDescription(SacrificeDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: EnduringScarAblityResGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(ActivatableBuff)
                .SetDeactivateImmediately()
                .Configure();

            var Buff2 = BuffConfigurator.New(Sacrifice3Buff, Sacrifice3BuffGuid)
             .SetDisplayName(SacrificeDisplayName)
             .SetDescription(SacrificeDescription)
             .SetIcon(icon)
             .AddBuffActions(activated: ActionsBuilder.New()
                    .ContextSpendResource(EnduringScarAblityResGuid, ContextValues.Constant(1))
                    .Build())
             .Configure();

            var Buff4 = BuffConfigurator.New(Sacrifice4Buff, Sacrifice4BuffGuid)
             .SetDisplayName(SacrificeDisplayName)
             .SetDescription(SacrificeDescription)
             .SetIcon(icon)
             .AddACBonusAgainstAttacks(armorClassBonus: 1, descriptor: ModifierDescriptor.Deflection)
             .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Resistance, value: 1)
             .Configure();

            var Buff1 = BuffConfigurator.New(SacrificeBuff, SacrificeBuffGuid)
             .SetDisplayName(SacrificeDisplayName)
             .SetDescription(SacrificeDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
             .AddComponent<ScarSacrifice>(a => { a.m_CheckBuff = ActivatableBuff.ToReference<BlueprintBuffReference>();  a.m_CooldownBuff = Buff2.ToReference<BlueprintBuffReference>(); a.m_KeepBuff = Buff4.ToReference<BlueprintBuffReference>(); })
             .Configure();

            var ability = AbilityConfigurator.New(SacrificeAblity, SacrificeAblityGuid)
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
                .SetDisplayName(SacrificeDisplayName)
                .SetDescription(SacrificeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(false)
                .Configure();

            return FeatureConfigurator.New(Sacrifice, SacrificeGuid)
              .SetDisplayName(SacrificeDisplayName)
              .SetDescription(SacrificeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 3)
              .AddFacts(new List<Blueprint<BlueprintUnitFactReference>>() { ability3, ability })
              .Configure();
        }

        private const string Suffering = "ScarSeeker.Suffering";
        private static readonly string SufferingGuid = "{566DBFE0-1A87-441B-A7C8-D5937C5B62B2}";

        private const string SufferingAblity = "ScarSeeker.UseSuffering";
        private static readonly string SufferingAblityGuid = "{6934360B-8D7A-4736-BA3C-2B27CD03ADB9}";

        private const string SufferingBuff = "ScarSeeker.SufferingBuff";
        private static readonly string SufferingBuffGuid = "{0E34DAF7-D68E-4B44-B48F-9C85000EB59B}";

        internal const string SufferingDisplayName = "ScarSeekerSuffering.Name";
        private const string SufferingDescription = "ScarSeekerSuffering.Description";
        public static BlueprintFeature SufferingFeat()
        {
            var icon = FeatureRefs.BodyOfStoneFeature.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SufferingBuff, SufferingBuffGuid)
             .SetDisplayName(SufferingDisplayName)
             .SetDescription(SufferingDescription)
             .SetIcon(icon)
             .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(2))
             .AddSavingThrowBonusAgainstDescriptor(value: 2, spellDescriptor: SpellDescriptor.Fear)
             .AddSavingThrowBonusAgainstDescriptor(value: 2, spellDescriptor: SpellDescriptor.Emotion)
             .Configure();

            var ability = AbilityConfigurator.New(SufferingAblity, SufferingAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Buff1).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Constant(1)))
                        .Build())
                    .Build())
                .SetDisplayName(SufferingDisplayName)
                .SetDescription(SufferingDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnduringScarAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Suffering, SufferingGuid)
              .SetDisplayName(SufferingDisplayName)
              .SetDescription(SufferingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: EnduringScarAblityResGuid, restoreAmount: true)
              .Configure();
        }

        private const string SufferingImproved = "ScarSeeker.SufferingImproved";
        private static readonly string SufferingImprovedGuid = "{B8F164CE-564E-4878-8547-156BFC785AD4}";

        private const string SufferingImprovedAblity = "ScarSeeker.UseSufferingImproved";
        private static readonly string SufferingImprovedAblityGuid = "{60447CB7-6CC0-4E43-A40F-1E6175D7DF57}";

        private const string SufferingImprovedBuff = "ScarSeeker.SufferingImprovedBuff";
        private static readonly string SufferingImprovedBuffGuid = "{9F3FDB7B-1DE5-4806-8201-5A4CE03898C2}";

        public static BlueprintFeature SufferingImprovedFeat()
        {
            var icon = FeatureRefs.BodyOfStoneFeature.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SufferingImprovedBuff, SufferingImprovedBuffGuid)
             .SetDisplayName(SufferingDisplayName)
             .SetDescription(SufferingDescription)
             .SetIcon(icon)
             .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Rank())
             .AddSavingThrowBonusAgainstDescriptor(bonus: ContextValues.Rank(), spellDescriptor: SpellDescriptor.Fear)
             .AddSavingThrowBonusAgainstDescriptor(bonus: ContextValues.Rank(), spellDescriptor: SpellDescriptor.Emotion)
             .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithDiv2Progression())
             .Configure();

            var ability = AbilityConfigurator.New(SufferingImprovedAblity, SufferingImprovedAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(Buff1).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Constant(1)))
                        .Build())
                    .Build())
                .SetDisplayName(SufferingDisplayName)
                .SetDescription(SufferingDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: EnduringScarAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(SufferingImproved, SufferingImprovedGuid)
              .SetDisplayName(SufferingDisplayName)
              .SetDescription(SufferingDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddRemoveFeatureOnApply(SufferingAblityGuid)
              .AddAbilityResources(resource: EnduringScarAblityResGuid, restoreAmount: true)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 7)
              .AddPrerequisiteFeature(SufferingGuid)
              .Configure();
        }

        private const string Tenacity = "ScarSeeker.Tenacity";
        private static readonly string TenacityGuid = "{581A07AD-3D6D-4A7F-92B7-9006797AB5C1}";

        internal const string TenacityDisplayName = "ScarSeekerTenacity.Name";
        private const string TenacityDescription = "ScarSeekerTenacity.Description";

        private const string TenacityBuff = "ScarSeeker.TenacityBuff";
        private static readonly string TenacityBuffGuid = "{CB71F900-48B9-4BBE-BBE5-62846416551F}";

        private const string Tenacity2Buff = "ScarSeeker.Tenacity2Buff";
        private static readonly string Tenacity2BuffGuid = "{79219EFC-0A28-43CB-B4F0-D416AD83E9A8}";

        private const string TenacityAblity = "ScarSeeker.UseTenacity";
        private static readonly string TenacityAblityGuid = "{00AAF781-6FDF-4EBA-A24E-7C5332A4879C}";

        public static BlueprintFeature TenacityFeat()
        {
            var icon = FeatureRefs.Stalwart.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(TenacityBuff, TenacityBuffGuid)
                .SetDisplayName(TenacityDisplayName)
                .SetDescription(TenacityDescription)
                .SetIcon(icon)
                .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Morale, value: 1)
                .AddBuffAllSavesBonus(value: 1)
                .AddBuffAllSkillsBonus(descriptor: ModifierDescriptor.Morale, value: 1)
                .AddBuffAllSkillsBonus(value: 1)
                .AddBuffAbilityRollsBonus(affectAllStats: true, descriptor: ModifierDescriptor.Morale, value: 1)
                .AddBuffAbilityRollsBonus(affectAllStats: true, value: 1)
                .AddAttackBonusConditional(bonus: ContextValues.Constant(1), descriptor: ModifierDescriptor.Morale)
                .AddAttackBonusConditional(bonus: ContextValues.Constant(1))
                .Configure();

            var action = ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff1, negate: true).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank()))
                        .ContextSpendResource(EnduringScarAblityResGuid, 1)
                        .Build())
                    .Build();

            var action2 = ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().IsFlatFooted().HasFact(Buff1, negate: true).Build(), ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank()))
                        .ContextSpendResource(EnduringScarAblityResGuid, 1)
                        .Build())
                    .Build();

            var Buff2 = BuffConfigurator.New(Tenacity2Buff, Tenacity2BuffGuid)
                .SetDisplayName(TenacityDisplayName)
                .SetDescription(TenacityDescription)
                .SetIcon(icon)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(stat: StatType.Charisma, min: 1))
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("{CDE7679C-3AE2-48E6-8103-7ECCAF3EB9BC}"); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .AddTargetAttackRollTrigger(criticalHit: true, actionOnSelf: action)
                .AddTargetAttackRollTrigger(criticalHit: false, actionOnSelf: action2)
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(TenacityAblity, TenacityAblityGuid)
                .SetDisplayName(TenacityDisplayName)
                .SetDescription(TenacityDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: EnduringScarAblityResGuid, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(Buff2)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(Tenacity, TenacityGuid)
              .SetDisplayName(TenacityDisplayName)
              .SetDescription(TenacityDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string PurifyImproved = "ScarSeeker.PurifyImproved";
        private static readonly string PurifyImprovedGuid = "{CEA360AD-EE9E-433E-8FDA-D95AEC573C2C}";

        public static BlueprintFeature PurifyImprovedFeat()
        {
            var icon = FeatureRefs.PurityOfBody.Reference.Get().Icon;
            return FeatureConfigurator.New(PurifyImproved, PurifyImprovedGuid)
              .SetDisplayName(Purify2DisplayName)
              .SetDescription(Purify2Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();
        }

        private const string PurifyGreater = "ScarSeeker.PurifyGreater";
        private static readonly string PurifyGreaterGuid = "{C28D0B00-11A3-4DFB-A6E9-1B40AED76E57}";

        public static BlueprintFeature PurifyGreaterFeat()
        {
            var icon = FeatureRefs.PurityOfBody.Reference.Get().Icon;
            return FeatureConfigurator.New(PurifyGreater, PurifyGreaterGuid)
              .SetDisplayName(Purify2DisplayName)
              .SetDescription(Purify2Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();
        }

        private const string Purify = "ScarSeeker.Purify";
        private static readonly string PurifyGuid = "{70861342-3A8F-4D18-8AD7-9620BC238806}";

        private const string PurifyAblity = "ScarSeeker.UsePurify";
        private static readonly string PurifyAblityGuid = "{0A39B300-DF57-425E-9A91-76CA00C8D276}";

        private const string PurifyBuff = "ScarSeeker.PurifyBuff";
        private static readonly string PurifyBuffGuid = "{ACE3C7D5-00FA-4DE9-BD32-6D56EC5C58B7}";

        private const string Purify2Buff = "ScarSeeker.Purify2Buff";
        private static readonly string Purify2BuffGuid = "{8E930B48-C905-42B6-8A6E-BB6E84A1432E}";

        private const string Purify3Buff = "ScarSeeker.Purify3Buff";
        private static readonly string Purify3BuffGuid = "{9D881C51-F097-4BDC-9551-E088DA2A7415}";

        private const string Purify4Buff = "ScarSeeker.Purify4Buff";
        private static readonly string Purify4BuffGuid = "{7576C7DE-484B-4AC5-8DDE-378FC739F4DC}";

        internal const string PurifyDisplayName = "ScarSeekerPurify.Name";
        private const string PurifyDescription = "ScarSeekerPurify.Description";

        internal const string Purify2DisplayName = "ScarSeekerPurify2.Name";
        private const string Purify2Description = "ScarSeekerPurify2.Description";
        public static BlueprintFeature PurifyFeat()
        {
            var icon = FeatureRefs.PurityOfBody.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(PurifyBuff, PurifyBuffGuid)
             .SetDisplayName(PurifyDisplayName)
             .SetDescription(PurifyDescription)
             .SetIcon(icon)
             .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Morale, value: 1)
             .AddBuffAllSkillsBonus(descriptor: ModifierDescriptor.Morale, value: 1)
             .AddBuffAbilityRollsBonus(affectAllStats: true, descriptor: ModifierDescriptor.Morale, value: 1)
             .AddAttackBonusConditional(bonus: ContextValues.Constant(1), descriptor: ModifierDescriptor.Morale)
             .Configure();

            var Buff2 = BuffConfigurator.New(Purify2Buff, Purify2BuffGuid)
             .SetDisplayName(PurifyDisplayName)
             .SetDescription(PurifyDescription)
             .SetIcon(icon)
             .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Morale, value: 2)
             .AddBuffAllSkillsBonus(descriptor: ModifierDescriptor.Morale, value: 2)
             .AddBuffAbilityRollsBonus(affectAllStats: true, descriptor: ModifierDescriptor.Morale, value: 2)
             .AddAttackBonusConditional(bonus: ContextValues.Constant(2), descriptor: ModifierDescriptor.Morale)
             .Configure();

            var Buff3 = BuffConfigurator.New(Purify3Buff, Purify3BuffGuid)
             .SetDisplayName(PurifyDisplayName)
             .SetDescription(PurifyDescription)
             .SetIcon(icon)
             .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Morale, value: 3)
             .AddBuffAllSkillsBonus(descriptor: ModifierDescriptor.Morale, value: 3)
             .AddBuffAbilityRollsBonus(affectAllStats: true, descriptor: ModifierDescriptor.Morale, value: 3)
             .AddAttackBonusConditional(bonus: ContextValues.Constant(3), descriptor: ModifierDescriptor.Morale)
             .Configure();

            var Buff4 = BuffConfigurator.New(Purify4Buff, Purify4BuffGuid)
             .SetDisplayName(PurifyDisplayName)
             .SetDescription(PurifyDescription)
             .SetIcon(icon)
             .AddComponent<PainfulPurification>(c => { c.Action = ActionsBuilder.New().RemoveSelf().Build(); })
             ///.AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }))
             .AddBuffActions(activated: ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.Zero, bonus: ContextValues.Constant(1), diceCount: ContextValues.Constant(0)), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine))
                .Build())
             .SetFlags(new Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags[] { Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi })
             .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Stack)
             .Configure();

            var ability = AbilityConfigurator.New(PurifyAblity, PurifyAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    //some kind of burn
                    .ApplyBuffPermanent(Buff4)
                    .RestoreResource(EnduringScarAblityResGuid, ContextValues.Constant(1))
                    .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(TrueMGuid).Build(),
                            ifTrue: ActionsBuilder.New()
                                .ApplyBuff(TrueMBuffGuid, ContextDuration.Variable(ContextValues.Constant(1)))
                                .Build())
                    .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(PurifyGreaterGuid).CasterHasFact(Buff2).Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff3, ContextDuration.Variable(ContextValues.Constant(600)))
                        .RemoveBuff(Buff2)
                        .Build(),
                    ifFalse: ActionsBuilder.New()
                        .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(PurifyImprovedGuid).CasterHasFact(Buff1).Build(),
                        ifTrue: ActionsBuilder.New()
                            .ApplyBuff(Buff2, ContextDuration.Variable(ContextValues.Constant(600)))
                            .RemoveBuff(Buff1)
                            .Build(),
                        ifFalse: ActionsBuilder.New()
                            .Conditional(conditions: ConditionsBuilder.New().CasterHasFact(PurifyImprovedGuid).Build(),
                            ifTrue: ActionsBuilder.New()
                                .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Constant(600)))
                                .Build())
                            .Build())
                        .Build())
                    .Build())
                .SetDisplayName(PurifyDisplayName)
                .SetDescription(PurifyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(false)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            return FeatureConfigurator.New(Purify, PurifyGuid)
              .SetDisplayName(PurifyDisplayName)
              .SetDescription(PurifyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string ChampionHonor = "ScarSeeker.ChampionHonor";
        private static readonly string ChampionHonorGuid = "{51E0B578-6289-4348-AC97-BF0C3B5C35A2}";

        internal const string ScarSeekerChampionHonorDisplayName = "ScarSeekerChampionHonor.Name";
        private const string ScarSeekerChampionHonorDescription = "ScarSeekerChampionHonor.Description";
        public static BlueprintFeatureSelection SSChampionHonor()
        {
            var icon = AbilityRefs.SmiteEvilAbility.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChampionHonor, ChampionHonorGuid)
              .SetDisplayName(ScarSeekerChampionHonorDisplayName)
              .SetDescription(ScarSeekerChampionHonorDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.SmiteEvilFeature.ToString())
              .AddToAllFeatures(FeatureRefs.SmiteEvilAdditionalUse.ToString())
              .AddDamageBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.SmiteEvilBuff.ToString())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithLinearProgression(1, -3))
              .Configure();
        }

        private const string TrueM = "ScarSeeker.TrueM";
        private static readonly string TrueMGuid = "{6A523C2D-9BD3-46E3-A585-04630EFEBA64}";

        private const string TrueMBuff = "ScarSeeker.TrueMBuff";
        private static readonly string TrueMBuffGuid = "{6DB9C8A4-B862-48EE-9BC2-92609B4330E3}";

        private const string SummonPool = "ScarSeeker.SummonPool";
        private static readonly string SummonPoolGuid = "{D41FC2B1-469F-48D4-8DB7-8F5A2AD74EBC}";

        internal const string TrueMDisplayName = "ScarSeekerTrueM.Name";
        private const string TrueMDescription = "ScarSeekerTrueM.Description";
        public static BlueprintFeature TrueMFeat()
        {
            var icon = FeatureRefs.FlamewardenPhoenixRising.Reference.Get().Icon;

            var summonpool = SummonPoolConfigurator.New(SummonPool, SummonPoolGuid)
                .SetDoNotRemoveDeadUnits(false)
                .Configure();

            BuffConfigurator.New(TrueMBuff, TrueMBuffGuid)
             .SetDisplayName(TrueMDisplayName)
             .SetDescription(TrueMDescription)
             .SetIcon(icon)
             .AddDeathActions(deathTrigger: DeathActions.DeathTrigger.OnUnitDeath, checkResource: false, withUnconsciousLifeState: false,
                    actions: ActionsBuilder.New()
                        .SpawnUnlinkedMonster(monster: UnitRefs.MonadicDevaSummoned.ToString(), setCasterGroup: true)
                        .Build())
             .Configure();

            return FeatureConfigurator.New(TrueM, TrueMGuid)
              .SetDisplayName(TrueMDisplayName)
              .SetDescription(TrueMDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .Configure();
        }
    }
}
