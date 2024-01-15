using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Enums;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.StoryEx;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.Blueprint.Spell;
using PrestigePlus.CustomComponent.Feat;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.CustomComponent;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using PrestigePlus.Modify;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Items.Armors;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.Blueprint.Archetype;
using Kingmaker.AreaLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Designers.Mechanics.Facts;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using PrestigePlus.Patch;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.CustomComponent.Archetype;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.RuleSystem.Rules.Damage;
using BlueprintCore.Actions.Builder.BasicEx;
using static Kingmaker.EntitySystem.EntityDataBase;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UI.MVVM._VM.Other;
using Kingmaker.UnitLogic;
using TabletopTweaks.Core.NewComponents;

namespace PrestigePlus.Blueprint.Feat
{
    internal class DeificObedience
    {
        private const string DeificObedienceFeat = "DeificObedience.DeificObedience";
        public static readonly string DeificObedienceGuid = "{E017281A-7AA3-4C6D-AC27-2D57C45B4A81}";

        internal const string DeificObedienceDisplayName = "DeificObedience.Name";
        private const string DeificObedienceDescription = "DeificObedience.Description";

        private const string DeificObedienceAblityRes = "DeificObedienceAblityRes";
        public static readonly string DeificObedienceAblityResGuid = "{751BD21A-A532-4939-9DC9-EE26E7FCE6BB}";

        private const string DeificObedienceAblity = "DeificObedienceAblity";
        private static readonly string DeificObedienceAblityGuid = "{7D26E0B2-7E4B-4F03-933F-33863F32058C}";
        public static void DeificObedienceConfigure()
        {
            var icon = AbilityRefs.Prayer.Reference.Get().Icon;

            var resourse = AbilityResourceConfigurator.New(DeificObedienceAblityRes, DeificObedienceAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(6))
                .Configure();

            AbilityConfigurator.New(DeificObedienceAblity, DeificObedienceAblityGuid)
                .CopyFrom(
                AbilityRefs.CommunityDomainGreaterAbility,
                typeof(AbilitySpawnFx))
                .SetDisplayName(DeificObedienceDisplayName)
                .SetDescription(DeificObedienceDescription)
                .SetIcon(icon)
                .AllowTargeting(self: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(conditions: ConditionsBuilder.New().HasFact(ArazniGuid).Build(),
                    ifTrue: ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: 0, diceCount: ContextValues.Constant(1)), damageType: DamageTypes.Direct())
                        .Build())
                    .Build())
                .Configure();

            var feat = FeatureSelectionConfigurator.New(DeificObedienceFeat, DeificObedienceGuid)
              .SetDisplayName(DeificObedienceDisplayName)
              .SetDescription(DeificObedienceDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(RagathielFeat())
              .AddToAllFeatures(ShelynFeat())
              .AddToAllFeatures(NaderiFeat())
              .AddToAllFeatures(DesnaFeat())
              .AddToAllFeatures(ErastilFeat())
              .AddToAllFeatures(GorumFeat())
              .AddToAllFeatures(MahathallahFeat())
              .AddToAllFeatures(NorgorberFeat())
              .AddToAllFeatures(OtolmensFeat())
              .AddToAllFeatures(LamashtuFeat())
              .AddToAllFeatures(LamashtuDemonFeat())
              .AddToAllFeatures(ArazniFeat())
              .AddToAllFeatures(CharonFeat())
              .AddToAllFeatures(SzurielFeat())
              .AddToAllFeatures(IomedaeFeat())
              .AddPrerequisiteNoFeature(FeatureRefs.AtheismFeature.ToString())
              .AddPrerequisiteNoFeature(DeificObedienceGuid)
              .AddPrerequisiteNoArchetype(DivineChampion.ArchetypeGuid, CharacterClassRefs.WarpriestClass.ToString())
              .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 3)
              .AddAbilityResources(resource: resourse, restoreAmount: true)
              .AddRestTrigger(ActionsBuilder.New().CastSpell(DeificObedienceAblityGuid).Build())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string Naderi = "DeificObedience.Naderi";
        public static readonly string NaderiGuid = "{0941C876-4D62-496E-B0C8-EA953B230A6F}";

        internal const string NaderiDisplayName = "DeificObedienceNaderi.Name";
        private const string NaderiDescription = "DeificObedienceNaderi.Description";
        public static BlueprintFeature NaderiFeat()
        {
            var icon = AbilityRefs.ShamanHexBeckoningChillAbility.Reference.Get().Icon;
            //"NaderiFeature": "36d75d0c-41fb-497c-98bf-3e07a1fe8b2e",

            return FeatureConfigurator.New(Naderi, NaderiGuid)
              .SetDisplayName(NaderiDisplayName)
              .SetDescription(NaderiDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature("36d75d0c-41fb-497c-98bf-3e07a1fe8b2e", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(NaderiSentinelFeat())
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.SkillPersuasion, 2)
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.SkillUseMagicDevice, 2)
              .Configure();
        }

        private const string NaderiSentinel = "DeificObedience.NaderiSentinel";
        public static readonly string NaderiSentinelGuid = "{73D86F82-44D7-471E-A711-1F72F3B98644}";

        internal const string NaderiSentinelDisplayName = "DeificObedienceNaderiSentinel.Name";
        private const string NaderiSentinelDescription = "DeificObedienceNaderiSentinel.Description";
        public static BlueprintProgression NaderiSentinelFeat()
        {
            var icon = AbilityRefs.ShamanHexBeckoningChillAbility.Reference.Get().Icon;

            return ProgressionConfigurator.New(NaderiSentinel, NaderiSentinelGuid)
              .SetDisplayName(NaderiSentinelDisplayName)
              .SetDescription(NaderiSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(NaderiGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateNaderiSentinel1())
              .AddToLevelEntry(16, NaderiSentinel2Feat())
              .AddToLevelEntry(20, NaderiSentinel3Feat())
              .Configure();
        }

        private const string NaderiSentinel1 = "SpellPower.NaderiSentinel1";
        public static readonly string NaderiSentinel1Guid = "{62A55541-7FBC-45D2-85AB-1E757D5F4DDD}";
        internal const string NaderiSentinel1DisplayName = "SpellPowerNaderi1.Name";
        private const string NaderiSentinel1Description = "SpellPowerNaderi1.Description";

        private const string NaderiSentinel1Ablity2 = "SpellPower.UseNaderiSentinel12";
        private static readonly string NaderiSentinel1Ablity2Guid = "{0C3A2B3E-BD1E-42EA-ADBD-00A14DEB7A89}";

        private static BlueprintFeature CreateNaderiSentinel1()
        {
            var icon = AbilityRefs.IcyPrison.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(NaderiSentinel1Ablity2, NaderiSentinel1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.Castigate,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(AbilityTargetHasNoFactUnless),
                typeof(AbilityTargetHasFact),
                typeof(SpellDescriptorComponent),
                typeof(ContextRankConfig))
                .SetIcon(icon)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(NaderiSentinel1, NaderiSentinel1Guid)
              .SetDisplayName(NaderiSentinel1DisplayName)
              .SetDescription(NaderiSentinel1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string Naderi2 = "DeificObedience.Naderi2";
        public static readonly string Naderi2Guid = "{CC567A04-40C7-4257-B23D-A4054CB0B0BD}";

        internal const string Naderi2DisplayName = "DeificObedienceNaderi2.Name";
        private const string Naderi2Description = "DeificObedienceNaderi2.Description";

        private const string Naderi2Buff = "DeificObedience.Naderi2Buff";
        private static readonly string Naderi2BuffGuid = "{202FD60C-3628-43BE-94ED-791D1150849B}";

        private const string Naderi2Ability = "DeificObedience.Naderi2Ability";
        private static readonly string Naderi2AbilityGuid = "{86C1A7A1-E263-42D0-8B4C-989D506153F2}";
        public static BlueprintFeature NaderiSentinel2Feat()
        {
            var icon = AbilityRefs.CrushingDespair.Reference.Get().Icon;

            var buff = BuffConfigurator.New(Naderi2Buff, Naderi2BuffGuid)
             .SetDisplayName(Naderi2DisplayName)
             .SetDescription(Naderi2Description)
             .SetIcon(icon)
             .AddCondition(Kingmaker.UnitLogic.UnitCondition.Staggered)
             .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalAttackBonus, value: -1)
             .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalDamage, value: -1)
             .AddBuffAllSavesBonus(ModifierDescriptor.Penalty, value: -1)
             .AddBuffAllSkillsBonus(ModifierDescriptor.Penalty, value: -1, multiplier: 1)
             .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
             .AddSpellDescriptorComponent(SpellDescriptor.Emotion)
             .AddSpellDescriptorComponent(SpellDescriptor.NegativeEmotion)
             .Configure();

            var ability = AbilityConfigurator.New(Naderi2Ability, Naderi2AbilityGuid)
                .SetDisplayName(Naderi2DisplayName)
                .SetDescription(Naderi2Description)
                .SetIcon(icon)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Projectile)
                .AllowTargeting(true, true, true, true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.NecromancyCone30Feet00.ToString() }, type: AbilityProjectileType.Cone, length: 30.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Property(UnitProperty.Level, toCaster: true)))
                        .Build())
                  .Build(), savingThrowType: SavingThrowType.Will)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
                .AddSpellDescriptorComponent(SpellDescriptor.Emotion)
                .AddSpellDescriptorComponent(SpellDescriptor.NegativeEmotion)
                .Configure();

            return FeatureConfigurator.New(Naderi2, Naderi2Guid)
              .SetDisplayName(Naderi2DisplayName)
              .SetDescription(Naderi2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private static readonly string Naderi3Name = "DeificObedienceNaderi3";
        public static readonly string Naderi3Guid = "{44AE0CA2-464E-4200-9C67-24D0CFCBAE1F}";

        private static readonly string Naderi3DisplayName = "DeificObedienceNaderi3.Name";
        private static readonly string Naderi3Description = "DeificObedienceNaderi3.Description";

        private const string AuraBuff = "DeificObedienceStyle.Naderi3buff";
        private static readonly string AuraBuffGuid = "{3A701136-709B-4F06-9202-F30AF7369F28}";

        private const string Naderi3Aura = "DeificObedienceStyle.Naderi3Aura";
        private static readonly string Naderi3AuraGuid = "{0D79B107-54C2-4439-BF31-CA3B1C4A0A74}";

        private const string Naderi3Ability = "DeificObedienceStyle.Naderi3Ability";
        private static readonly string Naderi3AbilityGuid = "{BDC2A287-8FA8-4722-9206-A8E131D64EC8}";

        private const string Naderi3AbilityRes = "DeificObedienceStyle.Naderi3AbilityRes";
        private static readonly string Naderi3AbilityResGuid = "{489136AD-7D46-44BD-979C-8633B294F324}";

        public static BlueprintFeature NaderiSentinel3Feat()
        {
            var icon = AbilityRefs.IceBody.Reference.Get().Icon;

            var area = AbilityAreaEffectConfigurator.New(Naderi3Aura, Naderi3AuraGuid)
                .SetAffectEnemies(true)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Any)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(13.Feet())
                .AddAbilityAreaEffectBuff(BuffRefs.SpellResistanceBuff.ToString())
                .AddContextCalculateAbilityParams(replaceCasterLevel: true, casterLevel: ContextValues.Property(UnitProperty.Level))
                .Configure();

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(Naderi3DisplayName)
              .SetDescription(Naderi3Description)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(Naderi3AbilityRes, Naderi3AbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(Naderi3Ability, Naderi3AbilityGuid)
                .SetDisplayName(Naderi3DisplayName)
                .SetDescription(Naderi3Description)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .Configure();

            return FeatureConfigurator.New(Naderi3Name, Naderi3Guid)
                    .SetDisplayName(Naderi3DisplayName)
                    .SetDescription(Naderi3Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .AddIncreaseResourceAmountBySharedValue(false, abilityresourse, ContextValues.Property(UnitProperty.Level))
                    .Configure();
        }

        private const string Shelyn = "DeificObedience.Shelyn";
        public static readonly string ShelynGuid = "{780B47D4-6220-4239-A2DD-4204B2374850}";

        internal const string ShelynDisplayName = "DeificObedienceShelyn.Name";
        private const string ShelynDescription = "DeificObedienceShelyn.Description";
        public static BlueprintFeature ShelynFeat()
        {
            var icon = FeatureRefs.ShelynFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Shelyn, ShelynGuid)
              .SetDisplayName(ShelynDisplayName)
              .SetDescription(ShelynDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ShelynFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.NeutralGood, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(ShelynSentinelFeat())
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.SkillUseMagicDevice, 4)
              .Configure();
        }

        private const string ShelynSentinel = "DeificObedience.ShelynSentinel";
        public static readonly string ShelynSentinelGuid = "{6B1C38C3-D16D-4D04-8A0A-516E117791FE}";

        internal const string ShelynSentinelDisplayName = "DeificObedienceShelynSentinel.Name";
        private const string ShelynSentinelDescription = "DeificObedienceShelynSentinel.Description";
        public static BlueprintProgression ShelynSentinelFeat()
        {
            var icon = FeatureRefs.ShelynFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(ShelynSentinel, ShelynSentinelGuid)
              .SetDisplayName(ShelynSentinelDisplayName)
              .SetDescription(ShelynSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(ShelynGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateShelynSentinel1())
              .AddToLevelEntry(16, ShelynSentinel2Feat())
              .AddToLevelEntry(20, ShelynSentinel3Feat())
              .Configure();
        }

        private const string ShelynSentinel1 = "SpellPower.ShelynSentinel1";
        public static readonly string ShelynSentinel1Guid = "{7ABBD953-BEB3-4344-BB5E-5BA5F20B945F}";
        internal const string ShelynSentinel1DisplayName = "SpellPowerShelynSentinel1.Name";
        private const string ShelynSentinel1Description = "SpellPowerShelynSentinel1.Description";

        private const string ShelynSentinel1Ablity = "SpellPower.UseShelynSentinel1";
        private static readonly string ShelynSentinel1AblityGuid = "{9CDA5D9C-3252-4EBD-91A3-52A4E2D9C140}";

        private const string ShelynSentinel1Ablity2 = "SpellPower.UseShelynSentinel12";
        private static readonly string ShelynSentinel1Ablity2Guid = "{770398F3-2919-4B75-B34F-F2473209E0B2}";

        private const string ShelynSentinel1Ablity3 = "SpellPower.UseShelynSentinel13";
        private static readonly string ShelynSentinel1Ablity3Guid = "{0DCFD954-2342-4005-8EFD-58A6DE272692}";

        private static BlueprintFeature CreateShelynSentinel1()
        {
            var icon = FeatureRefs.JumpUp.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(ShelynSentinel1Ablity, ShelynSentinel1AblityGuid)
                .CopyFrom(
                AbilityRefs.Entangle,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityAoERadius),
                typeof(ContextRankConfig),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(ShelynSentinel1Ablity2, ShelynSentinel1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.CatsGrace,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(ShelynSentinel1Ablity3, ShelynSentinel1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Haste,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(AbilityTargetsAround))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ShelynSentinel1, ShelynSentinel1Guid)
              .SetDisplayName(ShelynSentinel1DisplayName)
              .SetDescription(ShelynSentinel1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string ShelynSentinel2 = "DeificObedience.ShelynSentinel2";
        public static readonly string ShelynSentinel2Guid = "{0737982E-F3F6-4879-B1FA-68FA88DE626F}";

        internal const string ShelynSentinel2DisplayName = "DeificObedienceShelynSentinel2.Name";
        private const string ShelynSentinel2Description = "DeificObedienceShelynSentinel2.Description";

        public static BlueprintFeature ShelynSentinel2Feat()
        {
            var icon = FeatureRefs.SmiteEvilFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(ShelynSentinel2, ShelynSentinel2Guid)
              .SetDisplayName(ShelynSentinel2DisplayName)
              .SetDescription(ShelynSentinel2Description)
              .SetIcon(icon)
              .AddComponent<ShelynGloriousMight>()
              .Configure();
        }

        private const string ShelynSentinel3 = "DeificObedience.ShelynSentinel3";
        public static readonly string ShelynSentinel3Guid = "{F1AE3BE7-99FE-49F6-9C5E-BAC9B7F80468}";

        internal const string ShelynSentinel3DisplayName = "DeificObedienceShelynSentinel3.Name";
        private const string ShelynSentinel3Description = "DeificObedienceShelynSentinel3.Description";
        public static BlueprintFeature ShelynSentinel3Feat()
        {
            var icon = AbilityRefs.ResistElectricity.Reference.Get().Icon;

            return FeatureConfigurator.New(ShelynSentinel3, ShelynSentinel3Guid)
              .SetDisplayName(ShelynSentinel3DisplayName)
              .SetDescription(ShelynSentinel3Description)
              .SetIcon(icon)
              .AddDamageResistanceEnergy(healOnDamage: false, value: 15, type: Kingmaker.Enums.Damage.DamageEnergyType.Electricity)
              .Configure();
        }

        private const string Ragathiel = "DeificObedience.Ragathiel";
        public static readonly string RagathielGuid = "{4DCA067D-8EEF-405A-95E7-9687742603D9}";

        internal const string RagathielDisplayName = "DeificObedienceRagathiel.Name";
        private const string RagathielDescription = "DeificObedienceRagathiel.Description";
        public static BlueprintProgression RagathielFeat()
        {
            //"RagathielFeature": "F79778D7-281C-4B9D-8E77-8F86812707AA",
            var icon = AbilityRefs.InstantEnemy.Reference.Get().Icon;

            return ProgressionConfigurator.New(Ragathiel, RagathielGuid)
              .SetDisplayName(RagathielDisplayName)
              .SetDescription(RagathielDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature("F79778D7-281C-4B9D-8E77-8F86812707AA", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulGood, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Ragathiel0Feat())
              .AddToLevelEntry(12, CreateRagathiel1())
              .AddToLevelEntry(16, Ragathiel2Feat())
              .AddToLevelEntry(20, Ragathiel3Feat())
              .Configure();
        }

        private const string Ragathiel0 = "DeificObedience.Ragathiel0";
        public static readonly string Ragathiel0Guid = "{D0086A74-0DCE-42D3-A82C-047CD6777731}";

        public static BlueprintFeature Ragathiel0Feat()
        {
            var icon = AbilityRefs.InstantEnemy.Reference.Get().Icon;

            return FeatureConfigurator.New(Ragathiel0, Ragathiel0Guid)
              .SetDisplayName(RagathielDisplayName)
              .SetDescription(RagathielDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstAlignment(AlignmentComponent.Evil, 4, ModifierDescriptor.Sacred)
              .Configure();
        }

        private const string Ragathiel1 = "SpellPower.Ragathiel1";
        public static readonly string Ragathiel1Guid = "{71FF444A-0247-4EDA-984F-0834A543118B}";
        internal const string Ragathiel1DisplayName = "SpellPowerRagathiel1.Name";
        private const string Ragathiel1Description = "SpellPowerRagathiel1.Description";

        private const string Ragathiel1Ablity = "SpellPower.UseRagathiel1";
        private static readonly string Ragathiel1AblityGuid = "{017CEA4D-D232-4393-9D3B-290DA4817D04}";

        private const string Ragathiel1Ablity2 = "SpellPower.UseRagathiel12";
        private static readonly string Ragathiel1Ablity2Guid = "{DE7C5484-6EE6-42C8-A536-82C9233AAB73}";

        private const string Ragathiel1Ablity3 = "SpellPower.UseRagathiel13";
        private static readonly string Ragathiel1Ablity3Guid = "{20541493-3B03-4E89-AF94-2D90DCCB1915}";

        private static BlueprintFeature CreateRagathiel1()
        {
            var icon = AbilityRefs.BlessWeaponCast.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Ragathiel1Ablity, Ragathiel1AblityGuid)
                .CopyFrom(
                AbilityRefs.BlessWeaponCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(Ragathiel1Ablity2, Ragathiel1Ablity2Guid)
                .CopyFrom(
                LitanyRighteousness.LitanyRighteousnessAbilityGuid,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityTargetAlignment))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(Ragathiel1Ablity3, Ragathiel1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.MagicalVestment,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityVariants))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Ragathiel1, Ragathiel1Guid)
              .SetDisplayName(Ragathiel1DisplayName)
              .SetDescription(Ragathiel1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string Ragathiel2 = "DeificObedience.Ragathiel2";
        public static readonly string Ragathiel2Guid = "{201A980B-8B20-4F6D-95FD-45EB312A2BC9}";

        internal const string Ragathiel2DisplayName = "DeificObedienceRagathiel2.Name";
        private const string Ragathiel2Description = "DeificObedienceRagathiel2.Description";

        private const string Ragathiel2Buff = "DeificObedience.Ragathiel2Buff";
        private static readonly string Ragathiel2BuffGuid = "{480E3197-84B0-4688-859B-83A698270DE5}";
        public static BlueprintFeature Ragathiel2Feat()
        {
            var icon = AbilityRefs.VengefulComets.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Ragathiel2Buff, Ragathiel2BuffGuid)
             .SetDisplayName(Ragathiel2DisplayName)
             .SetDescription(Ragathiel2Description)
             .SetIcon(icon)
             .AddUniqueBuff()
             .AddComponent<RagathielRetribution>()
             .Configure();

            var action = ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days)).Build();

            return FeatureConfigurator.New(Ragathiel2, Ragathiel2Guid)
              .SetDisplayName(Ragathiel2DisplayName)
              .SetDescription(Ragathiel2Description)
              .SetIcon(icon)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action, onlyHit: true)
              .Configure();
        }

        private const string Ragathiel3 = "DeificObedience.Ragathiel3";
        public static readonly string Ragathiel3Guid = "{81F099E2-AE6A-402C-A7EE-46A22F9A7CFE}";

        internal const string Ragathiel3DisplayName = "DeificObedienceRagathiel3.Name";
        private const string Ragathiel3Description = "DeificObedienceRagathiel3.Description";

        private const string Ragathiel3Buff = "DeificObedience.Ragathiel3Buff";
        private static readonly string Ragathiel3BuffGuid = "{4C22DFD8-EBAA-4809-A23C-D838D4A98DFE}";

        private const string Ragathiel3Res = "DeificObedience.Ragathiel3Res";
        private static readonly string Ragathiel3ResGuid = "{9D7F9008-4F6F-4151-B2D7-857A8429DEF0}";

        private const string Ragathiel3Ability = "DeificObedience.Ragathiel3Ability";
        private static readonly string Ragathiel3AbilityGuid = "{8DD276F2-A592-48B0-B316-53BEE2919AAC}";
        public static BlueprintFeature Ragathiel3Feat()
        {
            var icon = AbilityRefs.HolyAura.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(Ragathiel3Res, Ragathiel3ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(Ragathiel3Ability, Ragathiel3AbilityGuid)
                .CopyFrom(
                AbilityRefs.HolyAura,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilitySpawnFx),
                typeof(ContextRankConfigs))
                .SetDisplayName(Ragathiel3DisplayName)
                .SetDescription(Ragathiel3Description)
                .SetType(AbilityType.SpellLike)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            var Buff = BuffConfigurator.New(Ragathiel3Buff, Ragathiel3BuffGuid)
             .SetDisplayName(Ragathiel3DisplayName)
             .SetDescription(Ragathiel3Description)
             .SetIcon(icon)
             .AddDamageResistancePhysical(Kingmaker.Enums.Damage.DamageAlignment.Good, bypassedByAlignment: true, isStackable: true, value: ContextValues.Constant(10), material: Kingmaker.Enums.Damage.PhysicalDamageMaterial.ColdIron, bypassedByMaterial: true)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Holy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Holy.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Enhancement5.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Enhancement5.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(Ragathiel3, Ragathiel3Guid)
              .SetDisplayName(Ragathiel3DisplayName)
              .SetDescription(Ragathiel3Description)
              .SetIcon(icon)
              .AddBuffExtraEffects(BuffRefs.HolyAuraBuff.ToString(), null, Buff, true)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Desna = "DeificObedience.Desna";
        public static readonly string DesnaGuid = "{E9564E2B-5167-4EBC-B5B9-8226E9D31D91}";

        internal const string DesnaDisplayName = "DeificObedienceDesna.Name";
        private const string DesnaDescription = "DeificObedienceDesna.Description";
        public static BlueprintProgression DesnaFeat()
        {
            var icon = FeatureRefs.DesnaFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(Desna, DesnaGuid)
              .SetDisplayName(DesnaDisplayName)
              .SetDescription(DesnaDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.DesnaFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticGood, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Desna0Feat())
              .AddToLevelEntry(12, CreateDesna1())
              .AddToLevelEntry(16, Desna2Feat())
              .AddToLevelEntry(20, Desna3Feat())
              .Configure();
        }

        private const string Desna0 = "DeificObedience.Desna0";
        public static readonly string Desna0Guid = "{427BB85A-3868-40B8-9932-74D6970D84F5}";

        public static BlueprintFeature Desna0Feat()
        {
            var icon = FeatureRefs.DesnaFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Desna0, Desna0Guid)
              .SetDisplayName(DesnaDisplayName)
              .SetDescription(DesnaDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Initiative, 1, Kingmaker.Enums.ModifierDescriptor.Luck)
              .Configure();
        }

        private const string Desna1 = "SpellPower.Desna1";
        public static readonly string Desna1Guid = "{3239ED4A-30A1-4B5B-B62F-36290790657D}";
        internal const string Desna1DisplayName = "SpellPowerDesna1.Name";
        private const string Desna1Description = "SpellPowerDesna1.Description";

        private const string Desna1Ablity = "SpellPower.UseDesna1";
        private static readonly string Desna1AblityGuid = "{4437B4A9-D9C2-42D7-8ADC-09A99130E34D}";
        private static BlueprintFeature CreateDesna1()
        {
            var icon = AbilityRefs.Longstrider.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Desna1Ablity, Desna1AblityGuid)
                .CopyFrom(
                AbilityRefs.Longstrider,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Desna1, Desna1Guid)
              .SetDisplayName(Desna1DisplayName)
              .SetDescription(Desna1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Desna2 = "DeificObedience.Desna2";
        public static readonly string Desna2Guid = "{50F9A384-882D-4514-B78A-ADA499DCBCAF}";

        internal const string Desna2DisplayName = "DeificObedienceDesna2.Name";
        private const string Desna2Description = "DeificObedienceDesna2.Description";

        public static BlueprintFeature Desna2Feat()
        {
            var icon = AbilityRefs.MageLight.Reference.Get().Icon;

            return FeatureConfigurator.New(Desna2, Desna2Guid)
              .SetDisplayName(Desna2DisplayName)
              .SetDescription(Desna2Description)
              .SetIcon(icon)
              .AddSpellPenetrationBonus(false, value: ContextValues.Rank())
              .AddConcentrationBonus(false, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Charisma))
              .AddRecalculateOnStatChange(stat: StatType.Charisma)
              .AddComponent<DesnaStarSpell>()
              .Configure();
        }

        private const string Desna3 = "DeificObedience.Desna3";
        public static readonly string Desna3Guid = "{9AFB427F-E07F-49A8-9E04-E99D3510D9B8}";

        internal const string Desna3DisplayName = "DeificObedienceDesna3.Name";
        private const string Desna3Description = "DeificObedienceDesna3.Description";
        public static BlueprintFeature Desna3Feat()
        {
            var icon = AbilityRefs.Starlight.Reference.Get().Icon;

            return FeatureConfigurator.New(Desna3, Desna3Guid)
              .SetDisplayName(Desna3DisplayName)
              .SetDescription(Desna3Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string Erastil = "DeificObedience.Erastil";
        public static readonly string ErastilGuid = "{6C186C6C-869F-4C8B-A7DF-2E628F81BD57}";

        internal const string ErastilDisplayName = "DeificObedienceErastil.Name";
        private const string ErastilDescription = "DeificObedienceErastil.Description";
        public static BlueprintProgression ErastilFeat()
        {
            var icon = FeatureRefs.ErastilFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(Erastil, ErastilGuid)
              .SetDisplayName(ErastilDisplayName)
              .SetDescription(ErastilDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ErastilFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulGood, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Erastil0Feat())
              .AddToLevelEntry(12, CreateErastil1())
              .AddToLevelEntry(16, Erastil2Feat())
              .AddToLevelEntry(20, Erastil3Feat())
              .Configure();
        }

        private const string Erastil0 = "DeificObedience.Erastil0";
        public static readonly string Erastil0Guid = "{A5221796-4BA3-482B-87B8-83DB1885D2A2}";

        public static BlueprintFeature Erastil0Feat()
        {
            var icon = FeatureRefs.ErastilFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Erastil0, Erastil0Guid)
              .SetDisplayName(ErastilDisplayName)
              .SetDescription(ErastilDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.SkillLoreNature, 4, ModifierDescriptor.Sacred)
              .Configure();
        }

        private const string Erastil1 = "SpellPower.Erastil1";
        public static readonly string Erastil1Guid = "{9FDB4A8C-5891-4657-9813-C252B8FE4482}";
        internal const string Erastil1DisplayName = "SpellPowerErastil1.Name";
        private const string Erastil1Description = "SpellPowerErastil1.Description";

        private const string Erastil1Ablity = "SpellPower.UseErastil1";
        private static readonly string Erastil1AblityGuid = "{4EEDD042-96E3-40EE-AFEB-B7F2CC94F136}";

        private const string Erastil1Ablity2 = "SpellPower.UseErastil12";
        private static readonly string Erastil1Ablity2Guid = "{5C3A8274-FF2F-4429-9F3A-BCB384FB2DB3}";

        private const string Erastil1Ablity3 = "SpellPower.UseErastil13";
        private static readonly string Erastil1Ablity3Guid = "{B04AC216-8BA2-4F87-BF13-76B6E20F619B}";

        private static BlueprintFeature CreateErastil1()
        {
            var icon = FeatureSelectionRefs.HuntersBondSelection.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Erastil1Ablity, Erastil1AblityGuid)
                .CopyFrom(
                AbilityRefs.CureLightWoundsCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(Erastil1Ablity2, Erastil1Ablity2Guid)
                .CopyFrom(
                ShieldOther.ShieldOtherAbilityGuid,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(Erastil1Ablity3, Erastil1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Prayer,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(AbilityTargetsAround))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Erastil1, Erastil1Guid)
              .SetDisplayName(Erastil1DisplayName)
              .SetDescription(Erastil1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string Erastil2 = "DeificObedience.Erastil2";
        public static readonly string Erastil2Guid = "{1C7A4B4B-8160-4A1A-8CBD-331F62CD9CFA}";

        internal const string Erastil2DisplayName = "DeificObedienceErastil2.Name";
        private const string Erastil2Description = "DeificObedienceErastil2.Description";

        private const string Erastil2Buff = "DeificObedience.Erastil2Buff";
        private static readonly string Erastil2BuffGuid = "{C09D214B-DFF2-455F-B902-3F1815AA0817}";

        private const string Erastil2Ability = "DeificObedience.Erastil2Ability";
        private static readonly string Erastil2AbilityGuid = "{F891098F-9580-41C1-816F-6937A98C85CD}";

        private const string Erastil2Res = "DeificObedience.Erastil2Res";
        private static readonly string Erastil2ResGuid = "{168B52DC-52D4-4ACB-AB68-6AF04A7D56A0}";
        public static BlueprintFeature Erastil2Feat()
        {
            var icon = FeatureRefs.FinalShifterAspectFeature.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(Erastil2Res, Erastil2ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var buff = BuffConfigurator.New(Erastil2Buff, Erastil2BuffGuid)
             .SetDisplayName(Erastil2DisplayName)
             .SetDescription(Erastil2Description)
             .SetIcon(icon)
             .AddComponent<TwinFangDamage>()
             .Configure();

            var ability = AbilityConfigurator.New(Erastil2Ability, Erastil2AbilityGuid)
                .SetDisplayName(Erastil2DisplayName)
                .SetDescription(Erastil2Description)
                .SetIcon(icon)
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .OnPets(actions: ActionsBuilder.New()
                        .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Property(UnitProperty.Level, toCaster: true)))
                        .Build(), petType: PetType.AnimalCompanion)
                  .Build())
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Erastil2, Erastil2Guid)
              .SetDisplayName(Erastil2DisplayName)
              .SetDescription(Erastil2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Erastil3 = "DeificObedience.Erastil3";
        public static readonly string Erastil3Guid = "{F743F8FB-2922-42C6-9783-9C363F77D176}";

        internal const string Erastil3DisplayName = "DeificObedienceErastil3.Name";
        private const string Erastil3Description = "DeificObedienceErastil3.Description";
        public static BlueprintFeature Erastil3Feat()
        {
            var icon = FeatureRefs.ErastilFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Erastil3, Erastil3Guid)
              .SetDisplayName(Erastil3DisplayName)
              .SetDescription(Erastil3Description)
              .SetIcon(icon)
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.Longbow, false, StatType.Wisdom, false)
              .AddWeaponTypeDamageStatReplacement(WeaponCategory.Shortbow, false, StatType.Wisdom, false)
              .AddFacts(new() { FeatureRefs.ZenArcherZenArcheryFeature.ToString() })
              .Configure();
        }

        private const string Gorum = "DeificObedience.Gorum";
        public static readonly string GorumGuid = "{9F501833-C90A-400A-8F45-3D5D9545F472}";

        internal const string GorumDisplayName = "DeificObedienceGorum.Name";
        private const string GorumDescription = "DeificObedienceGorum.Description";
        public static BlueprintFeature GorumFeat()
        {
            var icon = FeatureRefs.GorumFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Gorum, GorumGuid)
              .SetDisplayName(GorumDisplayName)
              .SetDescription(GorumDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.GorumFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteProficiency(armorProficiencies: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy }, new WeaponCategory[] {})
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(GorumSentinelFeat())
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.SkillAthletics, 4)
              .Configure();
        }

        private const string GorumSentinel = "DeificObedience.GorumSentinel";
        public static readonly string GorumSentinelGuid = "{785C6F05-3E5B-40FB-B3DC-4AE122A32B69}";

        internal const string GorumSentinelDisplayName = "DeificObedienceGorumSentinel.Name";
        private const string GorumSentinelDescription = "DeificObedienceGorumSentinel.Description";
        public static BlueprintProgression GorumSentinelFeat()
        {
            var icon = FeatureRefs.GorumFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(GorumSentinel, GorumSentinelGuid)
              .SetDisplayName(GorumSentinelDisplayName)
              .SetDescription(GorumSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(GorumGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateGorum1())
              .AddToLevelEntry(16, Gorum2Feat())
              .AddToLevelEntry(20, Gorum3Feat())
              .Configure();
        }

        private const string Gorum1 = "SpellPower.Gorum1";
        public static readonly string Gorum1Guid = "{76E9B76D-0713-4F45-82FB-45C1BB04A79E}";
        internal const string Gorum1DisplayName = "SpellPowerGorum1.Name";
        private const string Gorum1Description = "SpellPowerGorum1.Description";

        private const string Gorum1Ablity = "SpellPower.UseGorum1";
        private static readonly string Gorum1AblityGuid = "{1C296E11-81EF-4A30-A900-44B643607368}";

        private const string Gorum1Ablity2 = "SpellPower.UseGorum12";
        private static readonly string Gorum1Ablity2Guid = "{3BD4D94D-4556-4A9C-BCFC-9217CD4266A8}";

        private const string Gorum1Ablity3 = "SpellPower.UseGorum13";
        private static readonly string Gorum1Ablity3Guid = "{2273A436-C5B6-44AD-968E-E6DCD87B8934}";

        private static BlueprintFeature CreateGorum1()
        {
            var icon = FeatureRefs.ArmorMastery.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Gorum1Ablity, Gorum1AblityGuid)
                .CopyFrom(
                AbilityRefs.EnlargePerson,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityTargetHasFact),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(Gorum1Ablity2, Gorum1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.BullsStrength,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(Gorum1Ablity3, Gorum1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.BeastShapeI,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilitySpawnFx),
                typeof(AbilityExecuteActionOnCast),
                typeof(AbilityTargetHasFact),
                typeof(ContextRankConfig))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Gorum1, Gorum1Guid)
              .SetDisplayName(Gorum1DisplayName)
              .SetDescription(Gorum1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string Gorum2 = "DeificObedience.Gorum2";
        public static readonly string Gorum2Guid = "{0DA54EE5-B9B3-40D4-B081-8B9DF62389E2}";

        internal const string Gorum2DisplayName = "DeificObedienceGorum2.Name";
        private const string Gorum2Description = "DeificObedienceGorum2.Description";

        public static BlueprintFeature Gorum2Feat()
        {
            var icon = FeatureRefs.StunningFistSickenedFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Gorum2, Gorum2Guid)
              .SetDisplayName(Gorum2DisplayName)
              .SetDescription(Gorum2Description)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().Add<GorumUnarmed>().Build(), 
                    onlyOnFullAttack: true, onlyOnFirstAttack: true, onlyHit: false)
              .Configure();
        }

        private const string Gorum3 = "DeificObedience.Gorum3";
        public static readonly string Gorum3Guid = "{EF71C768-795A-4B5D-B237-E45BBDC4C5DA}";

        private const string Gorum3Buff = "DeificObedience.Gorum3Buff";
        public static readonly string Gorum3BuffGuid = "{9973727D-4F1F-4287-8A91-670B4D130130}";

        internal const string Gorum3DisplayName = "DeificObedienceGorum3.Name";
        private const string Gorum3Description = "DeificObedienceGorum3.Description";
        public static BlueprintFeature Gorum3Feat()
        {
            var icon = AbilityRefs.ChannelRage.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Gorum3Buff, Gorum3BuffGuid)
             .SetDisplayName(Gorum3DisplayName)
             .SetDescription(Gorum3Description)
             .SetIcon(icon)
             .AddStatBonus(stat: StatType.AdditionalAttackBonus, value: 2)
             .AddStatBonus(stat: StatType.AdditionalDamage, value: 2)
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(Gorum3, Gorum3Guid)
              .SetDisplayName(Gorum3DisplayName)
              .SetDescription(Gorum3Description)
              .SetIcon(icon)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: Buff)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.DemonRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string Mahathallah = "DeificObedience.Mahathallah";
        public static readonly string MahathallahGuid = "{9E8B62F4-284D-4A96-BDFF-9FD133E4ED5E}";

        internal const string MahathallahDisplayName = "DeificObedienceMahathallah.Name";
        private const string MahathallahDescription = "DeificObedienceMahathallah.Description";
        public static BlueprintFeature MahathallahFeat()
        {
            var icon = FeatureRefs.AsmodeusFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Mahathallah, MahathallahGuid)
              .SetDisplayName(MahathallahDisplayName)
              .SetDescription(MahathallahDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.AsmodeusFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(MahathallahExaltedFeat())
              .AddIncreaseSpellSchoolDC(2, ModifierDescriptor.UntypedStackable, SpellSchool.Illusion)
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Profane, spellDescriptor: SpellDescriptor.MindAffecting, value: 2)
              .Configure();
        }

        private const string MahathallahExalted = "DeificObedience.MahathallahExalted";
        public static readonly string MahathallahExaltedGuid = "{DA3AAF2F-2E98-451D-A8E5-3005F5A80FE5}";

        internal const string MahathallahExaltedDisplayName = "DeificObedienceMahathallahExalted.Name";
        private const string MahathallahExaltedDescription = "DeificObedienceMahathallahExalted.Description";
        public static BlueprintProgression MahathallahExaltedFeat()
        {
            var icon = FeatureRefs.AsmodeusFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(MahathallahExalted, MahathallahExaltedGuid)
              .SetDisplayName(MahathallahExaltedDisplayName)
              .SetDescription(MahathallahExaltedDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(MahathallahGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateMahathallahExalted1())
              .AddToLevelEntry(16, MahathallahExalted2Feat())
              .AddToLevelEntry(20, MahathallahExalted3Feat())
              .Configure();
        }

        private const string MahathallahExalted1 = "SpellPower.MahathallahExalted1";
        public static readonly string MahathallahExalted1Guid = "{93E1E1D5-21DB-4262-962C-920924C18FB0}";
        internal const string MahathallahExalted1DisplayName = "SpellPowerMahathallahExalted1.Name";
        private const string MahathallahExalted1Description = "SpellPowerMahathallahExalted1.Description";

        private const string MahathallahExalted1Ablity2 = "SpellPower.UseMahathallahExalted12";
        private static readonly string MahathallahExalted1Ablity2Guid = "{6E2C0DD3-C6D8-42C7-A59A-53E33BCE9DB9}";

        private static BlueprintFeature CreateMahathallahExalted1()
        {
            var icon = AbilityRefs.Invisibility.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(MahathallahExalted1Ablity2, MahathallahExalted1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.Invisibility,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(MahathallahExalted1, MahathallahExalted1Guid)
              .SetDisplayName(MahathallahExalted1DisplayName)
              .SetDescription(MahathallahExalted1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string MahathallahExalted2 = "DeificObedience.MahathallahExalted2";
        public static readonly string MahathallahExalted2Guid = "{69C25B88-4909-4B0D-9285-B15C0F4B2827}";

        internal const string MahathallahExalted2DisplayName = "DeificObedienceMahathallahExalted2.Name";
        private const string MahathallahExalted2Description = "DeificObedienceMahathallahExalted2.Description";

        private const string Mahathallah2Buff = "DeificObedience.Mahathallah2Buff";
        private static readonly string Mahathallah2BuffGuid = "{3036A36B-4B62-469D-8F3B-50B33AFEAB75}";

        private const string Mahathallah2Ability = "DeificObedience.Mahathallah2Ability";
        private static readonly string Mahathallah2AbilityGuid = "{D0E831B7-FCD7-419C-9BD3-4A97AAA1C6CE}";
        public static BlueprintFeature MahathallahExalted2Feat()
        {
            var icon = AbilityRefs.MindFog.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(Mahathallah2Buff, Mahathallah2BuffGuid)
              .SetDisplayName(MahathallahExalted2DisplayName)
              .SetDescription(MahathallahExalted2Description)
              .SetIcon(icon)
              .AddCombatStateTrigger(ActionsBuilder.New()
                    .DealDamageToAbility(StatType.Strength, ContextDice.Value(DiceType.D4, 1, 0), setFactAsReason: true)
                    .HealTarget(ContextDice.Value(DiceType.D6, 3, 0))
                    .Build())
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(Mahathallah2Ability, Mahathallah2AbilityGuid)
                .SetDisplayName(MahathallahExalted2DisplayName)
                .SetDescription(MahathallahExalted2Description)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetIsOnByDefault(true)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(MahathallahExalted2, MahathallahExalted2Guid)
              .SetDisplayName(MahathallahExalted2DisplayName)
              .SetDescription(MahathallahExalted2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string MahathallahExalted3 = "DeificObedience.MahathallahExalted3";
        public static readonly string MahathallahExalted3Guid = "{6BE27E0D-8EC7-4DAA-939C-B1BA4D02EA50}";

        internal const string MahathallahExalted3DisplayName = "DeificObedienceMahathallahExalted3.Name";
        private const string MahathallahExalted3Description = "DeificObedienceMahathallahExalted3.Description";
        public static BlueprintFeature MahathallahExalted3Feat()
        {
            var icon = AbilityRefs.BreathOfLifeTouch.Reference.Get().Icon;

            return FeatureConfigurator.New(MahathallahExalted3, MahathallahExalted3Guid)
              .SetDisplayName(MahathallahExalted3DisplayName)
              .SetDescription(MahathallahExalted3Description)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(spellDescriptor: SpellDescriptor.Poison, value: 4)
              .Configure();
        }

        private const string Norgorber = "DeificObedience.Norgorber";
        public static readonly string NorgorberGuid = "{7F8A6152-8E04-4D8F-B2A5-E7F1FDA51425}";

        internal const string NorgorberDisplayName = "DeificObedienceNorgorber.Name";
        private const string NorgorberDescription = "DeificObedienceNorgorber.Description";
        public static BlueprintProgression NorgorberFeat()
        {
            var icon = FeatureRefs.NorgorberFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(Norgorber, NorgorberGuid)
              .SetDisplayName(NorgorberDisplayName)
              .SetDescription(NorgorberDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.NorgorberFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Norgorber0Feat())
              .AddToLevelEntry(2, CreateNorgorber1())
              .AddToLevelEntry(6, Norgorber2Feat())
              .AddToLevelEntry(10, Norgorber3Feat())
              .Configure();
        }

        private const string Norgorber0 = "DeificObedience.Norgorber0";
        public static readonly string Norgorber0Guid = "{F29EDA1E-0A57-4CF0-ADFC-23D8CEEFA12F}";

        public static BlueprintFeature Norgorber0Feat()
        {
            var icon = FeatureRefs.NorgorberFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Norgorber0, Norgorber0Guid)
              .SetDisplayName(NorgorberDisplayName)
              .SetDescription(NorgorberDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string Norgorber1 = "SpellPower.Norgorber1";
        public static readonly string Norgorber1Guid = "{30082553-FF29-4C8E-8E04-FC7BA85DB284}";
        internal const string Norgorber1DisplayName = "SpellPowerNorgorber1.Name";
        private const string Norgorber1Description = "SpellPowerNorgorber1.Description";

        private const string Norgorber1Ablity3 = "SpellPower.UseNorgorber13";
        private static readonly string Norgorber1Ablity3Guid = "{7F6257D7-3124-46EB-B3FB-A389E174BF4F}";

        private static BlueprintFeature CreateNorgorber1()
        {
            var icon = AbilityRefs.PoisonCast.Reference.Get().Icon;

            var ability3 = AbilityConfigurator.New(Norgorber1Ablity3, Norgorber1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.PoisonCast,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilityEffectStickyTouch),
                typeof(AbilityTargetHasFact),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Norgorber1, Norgorber1Guid)
              .SetDisplayName(Norgorber1DisplayName)
              .SetDescription(Norgorber1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability3 })
              .Configure();
        }

        private const string Norgorber2 = "DeificObedience.Norgorber2";
        public static readonly string Norgorber2Guid = "{48F018B2-7953-4BAE-8754-A354E849DE6E}";

        internal const string Norgorber2DisplayName = "DeificObedienceNorgorber2.Name";
        private const string Norgorber2Description = "DeificObedienceNorgorber2.Description";
        public static BlueprintFeature Norgorber2Feat()
        {
            var icon = FeatureRefs.AcidBombsFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Norgorber2, Norgorber2Guid)
              .SetDisplayName(Norgorber2DisplayName)
              .SetDescription(Norgorber2Description)
              .SetIcon(icon)
              .AddComponent<NorgorberBomb>()
              .Configure();
        }

        private const string Norgorber3 = "DeificObedience.Norgorber3";
        public static readonly string Norgorber3Guid = "{87D51D12-352E-403B-A11A-EA5CAC6E010B}";

        internal const string Norgorber3DisplayName = "DeificObedienceNorgorber3.Name";
        private const string Norgorber3Description = "DeificObedienceNorgorber3.Description";

        private const string Norgorber3Buff = "DeificObedience.Norgorber3Buff";
        private static readonly string Norgorber3BuffGuid = "{5149D11A-8D7F-43BA-B483-C8182CA6C7B0}";

        private const string Norgorber3Res = "DeificObedience.Norgorber3Res";
        private static readonly string Norgorber3ResGuid = "{422F7482-BCA2-4216-9F5D-D6B05E732D66}";

        private const string Norgorber3Ability = "DeificObedience.Norgorber3Ability";
        private static readonly string Norgorber3AbilityGuid = "{F0D91E50-7848-484D-8286-9E2E15506377}";
        public static BlueprintFeature Norgorber3Feat()
        {
            var icon = AbilityRefs.InvisibilityGreater.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(Norgorber3Res, Norgorber3ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(Norgorber3Ability, Norgorber3AbilityGuid)
                .CopyFrom(
                AbilityRefs.InvisibilityGreater,
                typeof(SpellComponent))
                .SetDisplayName(Norgorber3DisplayName)
                .SetDescription(Norgorber3Description)
                .SetType(AbilityType.SpellLike)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Norgorber3BuffGuid, ContextDuration.Variable(ContextValues.Property(UnitProperty.Level), DurationRate.Minutes))
                        .Build())
                .SetRange(AbilityRange.Personal)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            var cats = new WeaponCategory [] { WeaponCategory.Shortsword, WeaponCategory.Shortbow, WeaponCategory.Longbow, WeaponCategory.LightRepeatingCrossbow,
                                                WeaponCategory.LightCrossbow, WeaponCategory.HeavyRepeatingCrossbow, WeaponCategory.HeavyCrossbow,
                                                WeaponCategory.HandCrossbow, WeaponCategory.Dart, WeaponCategory.Bomb, WeaponCategory.Javelin, WeaponCategory.Sling,
                                                WeaponCategory.Shuriken, WeaponCategory.SlingStaff, WeaponCategory.ThrowingAxe };

            BuffConfigurator.New(Norgorber3Buff, Norgorber3BuffGuid)
                .CopyFrom(
                BuffRefs.InvisibilityGreaterBuff,
                typeof(BuffInvisibility))
             .SetDisplayName(Norgorber3DisplayName)
             .SetDescription(Norgorber3Description)
             .SetIcon(icon)
             .AddStatBonus(ModifierDescriptor.Profane, false, StatType.SkillPerception, 4)
             .AddWeaponMultipleCategoriesAttackBonus(2, cats, ModifierDescriptor.Profane)
             .Configure();

            return FeatureConfigurator.New(Norgorber3, Norgorber3Guid)
              .SetDisplayName(Norgorber3DisplayName)
              .SetDescription(Norgorber3Description)
              .SetIcon(icon)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Otolmens = "DeificObedience.Otolmens";
        public static readonly string OtolmensGuid = "{143FD642-FDCD-46C9-90C1-BA12115FEFE2}";

        internal const string OtolmensDisplayName = "DeificObedienceOtolmens.Name";
        private const string OtolmensDescription = "DeificObedienceOtolmens.Description";
        public static BlueprintProgression OtolmensFeat()
        {
            var icon = FeatureRefs.SkillFocusKnowledgeWorld.Reference.Get().Icon;
            //"OtolmensFeature": "e7f5ed5c-afd1-413f-9e26-e7a669572e21",
            return ProgressionConfigurator.New(Otolmens, OtolmensGuid)
              .SetDisplayName(OtolmensDisplayName)
              .SetDescription(OtolmensDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature("e7f5ed5c-afd1-413f-9e26-e7a669572e21", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(1, Otolmens0Feat())
              .AddToLevelEntry(2, CreateOtolmens1())
              .AddToLevelEntry(6, Otolmens2Feat())
              .AddToLevelEntry(10, Otolmens3Feat())
              .Configure();
        }

        private const string Otolmens0 = "DeificObedience.Otolmens0";
        public static readonly string Otolmens0Guid = "{513F2BFE-18CF-4A61-BC7E-F9E789736E23}";

        public static BlueprintFeature Otolmens0Feat()
        {
            var icon = FeatureRefs.SkillFocusKnowledgeWorld.Reference.Get().Icon;

            return FeatureConfigurator.New(Otolmens0, Otolmens0Guid)
              .SetDisplayName(OtolmensDisplayName)
              .SetDescription(OtolmensDescription)
              .SetIcon(icon)
              .AddCriticalConfirmationBonus(4)
              .Configure();
        }

        private const string Otolmens1 = "SpellPower.Otolmens1";
        public static readonly string Otolmens1Guid = "{E1F9FE0A-9AB2-402C-94F9-08963BD19D6C}";
        internal const string Otolmens1DisplayName = "SpellPowerOtolmens1.Name";
        private const string Otolmens1Description = "SpellPowerOtolmens1.Description";

        private const string Otolmens1Ablity3 = "SpellPower.UseOtolmens13";
        private static readonly string Otolmens1Ablity3Guid = "{D47BD8DD-249A-4C11-B427-6D5454CB1314}";

        private static BlueprintFeature CreateOtolmens1()
        {
            var icon = FeatureRefs.CavalierTacticianFeature.Reference.Get().Icon;

            var ability3 = AbilityConfigurator.New(Otolmens1Ablity3, Otolmens1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Blink,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(ContextRankConfig),
                typeof(SpellDescriptorComponent))
                .SetIcon(icon)
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Otolmens1, Otolmens1Guid)
              .SetDisplayName(Otolmens1DisplayName)
              .SetDescription(Otolmens1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability3 })
              .Configure();
        }

        private const string Otolmens2 = "DeificObedience.Otolmens2";
        public static readonly string Otolmens2Guid = "{F350ED5E-16A4-40A6-AC4B-75022A3B576E}";

        internal const string Otolmens2DisplayName = "DeificObedienceOtolmens2.Name";
        private const string Otolmens2Description = "DeificObedienceOtolmens2.Description";

        private const string Otolmens2Buff = "DeificObedience.Otolmens2Buff";
        private static readonly string Otolmens2BuffGuid = "{6BE98800-95A4-4CDF-BA06-9F09185B4FBD}";

        private const string Otolmens2Res = "DeificObedience.Otolmens2Res";
        private static readonly string Otolmens2ResGuid = "{EBBC4C26-85C8-47EB-9865-2D66FEF47378}";

        private const string Otolmens2Ability = "DeificObedience.Otolmens2Ability";
        private static readonly string Otolmens2AbilityGuid = "{D95219AB-86A6-472B-87BE-E10CA80334D8}";
        public static BlueprintFeature Otolmens2Feat()
        {
            var icon = AbilityRefs.TrueStrike.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(Otolmens2Res, Otolmens2ResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(3))
                .Configure();

            var ability = AbilityConfigurator.New(Otolmens2Ability, Otolmens2AbilityGuid)
                .CopyFrom(
                AbilityRefs.TrueStrike,
                typeof(AbilitySpawnFx))
                .SetDisplayName(Otolmens2DisplayName)
                .SetDescription(Otolmens2Description)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Otolmens2BuffGuid, ContextDuration.Fixed(1))
                        .Build())
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            BuffConfigurator.New(Otolmens2Buff, Otolmens2BuffGuid)
                .CopyFrom(
                BuffRefs.TrueStrikeBuff,
                typeof(AddGenericStatBonus),
                typeof(IgnoreConcealment))
             .SetDisplayName(Otolmens2DisplayName)
             .SetDescription(Otolmens2Description)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveBuff(Otolmens2BuffGuid, toCaster: false).Build(), actionsOnInitiator: true, onlyHit: false)
             .AddPartialDRIgnore(false, reductionPenaltyModifier: ContextValues.Property(UnitProperty.Level), useContextValue: true)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Axiomatic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Axiomatic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
             .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Axiomatic.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.AdditionalLimb)
             .Configure();

            return FeatureConfigurator.New(Otolmens2, Otolmens2Guid)
              .SetDisplayName(Otolmens2DisplayName)
              .SetDescription(Otolmens2Description)
              .SetIcon(icon)
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string Otolmens3 = "DeificObedience.Otolmens3";
        public static readonly string Otolmens3Guid = "{E71A0DA7-C131-447C-85D3-AE658DFECB88}";

        private const string SpellBook = "Otolmens.SpellBook";
        public static readonly string SpellBookGuid = "{BAB7DFA4-021C-474A-A6D8-C755E88B9D59}";

        private const string SpellBookBuff = "Otolmens.SpellBookBuff";
        public static readonly string SpellBookBuffGuid = "{D15BC272-E4CC-4917-8086-79F69FC437A3}";

        internal const string Otolmens3DisplayName = "DeificObedienceOtolmens3.Name";
        private const string Otolmens3Description = "DeificObedienceOtolmens3.Description";
        public static BlueprintFeature Otolmens3Feat()
        {
            var icon = AbilityRefs.OverwhelmingPresence.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SpellBookBuff, SpellBookBuffGuid)
              .SetDisplayName(Otolmens3DisplayName)
              .SetDescription(Otolmens3Description)
              .SetIcon(icon)
              .AddForbidSpellbook(spellbook: SpellBookGuid)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(BlueprintBuff.Flags.RemoveOnRest)
              .Configure();

            var spellbook = SpellbookConfigurator.New(SpellBook, SpellBookGuid)
              .SetName(OtolmensDisplayName)
              .SetSpellsPerDay(ExaltedEvangelist.SpellTableGuid)
              .SetAllSpellsKnown(true)
              .SetSpellList(GraveSpellList.spelllist3guid)
              .SetCharacterClass(CharacterClassRefs.MonkClass.ToString())
              .SetCastingAttribute(StatType.Charisma)
              .SetHasSpecialSpellList(true)
              .SetSpontaneous(false)
              .SetIsArcane(false)
              .SetCantripsType(CantripsType.Orisions)
              .Configure(delayed: true);

            return FeatureConfigurator.New(Otolmens3, Otolmens3Guid)
              .SetDisplayName(Otolmens3DisplayName)
              .SetDescription(Otolmens3Description)
              .SetIcon(icon)
              .AddSpellbook(ContextValues.Property(UnitProperty.Level), spellbook: spellbook)
              .AddComponent<MiracleSpellLevel>(c => { c.book = SpellBookGuid; c.level = 9; c.buff = SpellBookBuffGuid; })
              .Configure();
        }

        private const string Lamashtu = "DeificObedience.Lamashtu";
        public static readonly string LamashtuGuid = "{EA5E5868-7281-49D4-8B1A-8FC5EAE5D039}";

        internal const string LamashtuDisplayName = "DeificObedienceLamashtu.Name";
        private const string LamashtuDescription = "DeificObedienceLamashtu.Description";
        public static BlueprintFeature LamashtuFeat()
        {
            var icon = FeatureRefs.LamashtuFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Lamashtu, LamashtuGuid)
              .SetDisplayName(LamashtuDisplayName)
              .SetDescription(LamashtuDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.LamashtuFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(LamashtuExaltedFeat())
              .AddStatBonus(ModifierDescriptor.NaturalArmor, false, StatType.AC, 1)
              .Configure();
        }

        private const string LamashtuDemon = "DeificObedience.LamashtuDemon";
        public static readonly string LamashtuDemonGuid = "{4C69BA62-9B69-44E2-B650-DEEB090CB958}";

        internal const string LamashtuDemonDisplayName = "DeificObedienceLamashtuDemon.Name";
        private const string LamashtuDemonDescription = "DeificObedienceLamashtuDemon.Description";
        public static BlueprintFeature LamashtuDemonFeat()
        {
            var icon = AbilityRefs.Feeblemind.Reference.Get().Icon;

            return FeatureConfigurator.New(LamashtuDemon, LamashtuDemonGuid)
              .SetDisplayName(LamashtuDemonDisplayName)
              .SetDescription(LamashtuDemonDescription)
              .SetIcon(icon)
              .AddPrerequisitePlayerHasFeature(FeatureRefs.DemonFirstAscension.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(LamashtuExaltedGuid)
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Profane, spellDescriptor: SpellDescriptor.Confusion, value: 4)
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Profane, spellDescriptor: SpellDescriptor.Polymorph, value: 4)
              .Configure();
        }

        private const string LamashtuExalted = "DeificObedience.LamashtuExalted";
        public static readonly string LamashtuExaltedGuid = "{62E72E97-3407-4964-8873-46ED0757B0DA}";

        internal const string LamashtuExaltedDisplayName = "DeificObedienceLamashtuExalted.Name";
        private const string LamashtuExaltedDescription = "DeificObedienceLamashtuExalted.Description";
        public static BlueprintProgression LamashtuExaltedFeat()
        {
            var icon = FeatureRefs.LamashtuFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(LamashtuExalted, LamashtuExaltedGuid)
              .SetDisplayName(LamashtuExaltedDisplayName)
              .SetDescription(LamashtuExaltedDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(LamashtuGuid, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(LamashtuDemonGuid, group: Prerequisite.GroupType.Any)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateLamashtu1())
              .AddToLevelEntry(16, LamashtuExalted2Feat())
              .AddToLevelEntry(20, LamashtuExalted3Feat())
              .Configure();
        }

        private const string Lamashtu1 = "SpellPower.Lamashtu1";
        public static readonly string Lamashtu1Guid = "{B546FA84-45BC-472D-8BA7-CFB9EB677FF9}";
        internal const string Lamashtu1DisplayName = "SpellPowerLamashtu1.Name";
        private const string Lamashtu1Description = "SpellPowerLamashtu1.Description";

        private const string Lamashtu1Ablity = "SpellPower.UseLamashtu1";
        private static readonly string Lamashtu1AblityGuid = "{F99A4ED4-9258-48F8-B46F-4F55FA6808EA}";

        private const string Lamashtu1Ablity2 = "SpellPower.UseLamashtu12";
        private static readonly string Lamashtu1Ablity2Guid = "{C667960A-44CA-433B-A07F-0BF545783E65}";

        private const string Lamashtu1Ablity3 = "SpellPower.UseLamashtu13";
        private static readonly string Lamashtu1Ablity3Guid = "{2DA69A18-0568-4A95-90D6-79751223DB3F}";

        private static BlueprintFeature CreateLamashtu1()
        {
            var icon = AbilityRefs.MortalTerror.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Lamashtu1Ablity, Lamashtu1AblityGuid)
                .CopyFrom(
                AbilityRefs.CauseFear,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilityTargetHasNoFactUnless),
                typeof(AbilityTargetHasFact),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(Lamashtu1Ablity2, Lamashtu1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.MortalTerror,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilityTargetHasNoFactUnless),
                typeof(AbilityTargetHasFact),
                typeof(AbilitySpawnFx),
                typeof(ContextRankConfig))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(Lamashtu1Ablity3, Lamashtu1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Fear,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilityDeliverProjectile),
                typeof(ContextRankConfig))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Lamashtu1, Lamashtu1Guid)
              .SetDisplayName(Lamashtu1DisplayName)
              .SetDescription(Lamashtu1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string Lamashtu2 = "DeificObedience.Lamashtu2";
        public static readonly string Lamashtu2Guid = "{0ACF8B8E-EA73-4D5A-96A6-4BC0303A8239}";

        internal const string Lamashtu2DisplayName = "DeificObedienceLamashtu2.Name";
        private const string Lamashtu2Description = "DeificObedienceLamashtu2.Description";
        public static BlueprintFeature LamashtuExalted2Feat()
        {
            var icon = FeatureRefs.ShamanHexFearfulGazeFeature.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                                    .DealDamageToAbility(StatType.Wisdom, ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, 1, 0), setFactAsReason: true)
                                    .Build();

            return FeatureConfigurator.New(Lamashtu2, Lamashtu2Guid)
              .SetDisplayName(Lamashtu2DisplayName)
              .SetDescription(Lamashtu2Description)
              .SetIcon(icon)
              .AddComponent<LamashtuMadness>(c => { c.Action = action; })
              .Configure();
        }

        private static readonly string Lamashtu3Name = "DeificObedienceLamashtu3";
        public static readonly string Lamashtu3Guid = "{F3250E50-B38F-4B8B-94EA-E9DA324918E8}";

        private static readonly string Lamashtu3DisplayName = "DeificObedienceLamashtu3.Name";
        private static readonly string Lamashtu3Description = "DeificObedienceLamashtu3.Description";

        private const string Lamashtu3Buff = "DeificObedienceStyle.Lamashtu3buff";
        private static readonly string Lamashtu3BuffGuid = "{AC45714B-373D-43D8-876F-A5AEA678FCE0}";

        private const string Lamashtu3Ability = "DeificObedienceStyle.Lamashtu3Ability";
        private static readonly string Lamashtu3AbilityGuid = "{812310B5-0504-4A44-A472-25E3C6D783B2}";

        private const string Lamashtu3AbilityRes = "DeificObedienceStyle.Lamashtu3AbilityRes";
        private static readonly string Lamashtu3AbilityResGuid = "{93E4D058-1485-49B0-9EF1-0300C79EA1D5}";

        public static BlueprintFeature LamashtuExalted3Feat()
        {
            var icon = AbilityRefs.BalefulPolymorph.Reference.Get().Icon;

            var buff = BuffConfigurator.New(Lamashtu3Buff, Lamashtu3BuffGuid)
              .CopyFrom(
                BuffRefs.BalefulPolymorphBuff,
                typeof(AddCondition),
                typeof(AddContextStatBonus),
                typeof(Polymorph),
                typeof(SpellDescriptorComponent),
                typeof(ReplaceSourceBone),
                typeof(ReplaceAsksList),
                typeof(ReplaceCastSource),
                typeof(ChangeImpatience),
                typeof(SuppressBuffs),
                typeof(AddBuffActions),
                typeof(BuffMovementSpeed))
              .SetDisplayName(Lamashtu3DisplayName)
              .SetDescription(Lamashtu3Description)
              .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New()
                        .DealDamage(DamageTypes.Direct(), ContextDice.Value(DiceType.D6, 1, 0))
                        .Build())
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalAttackBonus, value: -2)
              .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.AdditionalDamage, value: -2)
              .AddBuffAllSavesBonus(ModifierDescriptor.Penalty, value: -2)
              .AddBuffAllSkillsBonus(ModifierDescriptor.Penalty, value: -2, multiplier: 1)
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(Lamashtu3AbilityRes, Lamashtu3AbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(Lamashtu3Ability, Lamashtu3AbilityGuid)
                .CopyFrom(
                AbilityRefs.BalefulPolymorph,
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilityTargetHasFact),
                typeof(AbilitySpawnFx))
                .SetDisplayName(Lamashtu3DisplayName)
                .SetDescription(Lamashtu3Description)
                .SetType(AbilityType.SpellLike)
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .RemoveBuffsByDescriptor(SpellDescriptor.Polymorph, true)
                        .ApplyBuffPermanent(buff, isFromSpell: true)
                        .Build())
                  .Build(), savingThrowType: SavingThrowType.Fortitude)
                .AddPretendSpellLevel(spellLevel: 9)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Lamashtu3Name, Lamashtu3Guid)
                    .SetDisplayName(Lamashtu3DisplayName)
                    .SetDescription(Lamashtu3Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private const string Arazni = "DeificObedience.Arazni";
        public static readonly string ArazniGuid = "{9D53BBC5-1E4C-42D9-AEC0-89479F0FD840}";

        internal const string ArazniDisplayName = "DeificObedienceArazni.Name";
        private const string ArazniDescription = "DeificObedienceArazni.Description";
        public static BlueprintFeature ArazniFeat()
        {
            var icon = FeatureRefs.UrgathoaFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Arazni, ArazniGuid)
              .SetDisplayName(ArazniDisplayName)
              .SetDescription(ArazniDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.UrgathoaFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(ArazniSentinelFeat())
              .AddComponent<ArazniObedience>()
              .Configure();
        }

        private const string ArazniSentinel = "DeificObedience.ArazniSentinel";
        public static readonly string ArazniSentinelGuid = "{E99188EA-F3C8-4AE9-A1C0-E61D739546FD}";

        internal const string ArazniSentinelDisplayName = "DeificObedienceArazniSentinel.Name";
        private const string ArazniSentinelDescription = "DeificObedienceArazniSentinel.Description";
        public static BlueprintProgression ArazniSentinelFeat()
        {
            var icon = FeatureRefs.UrgathoaFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(ArazniSentinel, ArazniSentinelGuid)
              .SetDisplayName(ArazniSentinelDisplayName)
              .SetDescription(ArazniSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(ArazniGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateArazni1())
              .AddToLevelEntry(16, Arazni2Feat())
              .AddToLevelEntry(20, Arazni3Feat())
              .Configure();
        }

        private const string Arazni1 = "SpellPower.Arazni1";
        public static readonly string Arazni1Guid = "{4F105860-5A78-4C62-8264-72759F66B299}";
        internal const string Arazni1DisplayName = "SpellPowerArazni1.Name";
        private const string Arazni1Description = "SpellPowerArazni1.Description";

        private const string Arazni1Ablity = "SpellPower.UseArazni1";
        private static readonly string Arazni1AblityGuid = "{87F342E2-A823-428D-BDB4-A0DAF5BB1EB6}";

        private const string Arazni1Ablity2 = "SpellPower.UseArazni12";
        private static readonly string Arazni1Ablity2Guid = "{646EBA59-E44A-4F5A-A7C3-15096328E7C2}";

        private const string Arazni1Ablity3 = "SpellPower.UseArazni13";
        private static readonly string Arazni1Ablity3Guid = "{0A418C02-CFA7-48C3-A478-3572FF6D9605}";

        private static BlueprintFeature CreateArazni1()
        {
            var icon = FeatureRefs.FinesseTrainingRapier.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(Arazni1Ablity, Arazni1AblityGuid)
                .CopyFrom(
                AbilityRefs.DivineFavor,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx))
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability2 = AbilityConfigurator.New(Arazni1Ablity2, Arazni1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.EffortlessArmor,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(ContextRankConfig))
                .AddPretendSpellLevel(spellLevel: 2)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ability3 = AbilityConfigurator.New(Arazni1Ablity3, Arazni1Ablity3Guid)
                .CopyFrom(
                AbilityRefs.Rage,
                typeof(AbilityEffectRunAction),
                typeof(AbilityTargetsAround),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(6, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(Arazni1, Arazni1Guid)
              .SetDisplayName(Arazni1DisplayName)
              .SetDescription(Arazni1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2, ability3 })
              .Configure();
        }

        private const string Arazni2 = "DeificObedience.Arazni2";
        public static readonly string Arazni2Guid = "{9B9AFBD5-A3F8-4C01-A6BD-96420DE65DDB}";

        internal const string Arazni2DisplayName = "DeificObedienceArazni2.Name";
        private const string Arazni2Description = "DeificObedienceArazni2.Description";
        public static BlueprintFeature Arazni2Feat()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;

            return FeatureConfigurator.New(Arazni2, Arazni2Guid)
              .SetDisplayName(Arazni2DisplayName)
              .SetDescription(Arazni2Description)
              .SetIcon(icon)
              .AddBuffDescriptorImmunity(false, SpellDescriptor.Charm)
              .AddBuffDescriptorImmunity(false, SpellDescriptor.Compulsion)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Charm)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Compulsion)
              .AddSavingThrowBonusAgainstDescriptor(value: 4, spellDescriptor: SpellDescriptor.Fear)
              .Configure();
        }

        private const string Arazni3 = "DeificObedience.Arazni3";
        public static readonly string Arazni3Guid = "{C760D063-45D2-410D-AF42-56A14C181154}";

        internal const string Arazni3DisplayName = "DeificObedienceArazni3.Name";
        private const string Arazni3Description = "DeificObedienceArazni3.Description";
        public static BlueprintFeature Arazni3Feat()
        {
            var icon = FeatureRefs.Disruptive.Reference.Get().Icon;

            return FeatureConfigurator.New(Arazni3, Arazni3Guid)
              .SetDisplayName(Arazni3DisplayName)
              .SetDescription(Arazni3Description)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.Disruptive.ToString() })
              .Configure();
        }

        private const string Charon = "DeificObedience.Charon";
        public static readonly string CharonGuid = "{B6E7CE78-79CA-406D-B678-340CA69CC65B}";

        internal const string CharonDisplayName = "DeificObedienceCharon.Name";
        private const string CharonDescription = "DeificObedienceCharon.Description";
        public static BlueprintFeature CharonFeat()
        {
            var icon = AbilityRefs.DeathClutch.Reference.Get().Icon;

            return FeatureConfigurator.New(Charon, CharonGuid)
              .SetDisplayName(CharonDisplayName)
              .SetDescription(CharonDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ZonKuthonFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(CharonSentinelFeat())
              .AddSavingThrowBonusAgainstDescriptor(value: 4, spellDescriptor: SpellDescriptor.NegativeLevel, modifierDescriptor: ModifierDescriptor.Profane)
              .AddSavingThrowBonusAgainstDescriptor(value: 4, spellDescriptor: SpellDescriptor.ChannelNegativeHarm, modifierDescriptor: ModifierDescriptor.Profane)
              .AddSavingThrowBonusAgainstSchool(value: 4, school: SpellSchool.Necromancy, modifierDescriptor: ModifierDescriptor.Profane)
              .Configure();
        }

        private const string CharonSentinel = "DeificObedience.CharonSentinel";
        public static readonly string CharonSentinelGuid = "{85920F3C-C027-4B4C-AC2A-B88CEBF0A753}";

        internal const string CharonSentinelDisplayName = "DeificObedienceCharonSentinel.Name";
        private const string CharonSentinelDescription = "DeificObedienceCharonSentinel.Description";
        public static BlueprintProgression CharonSentinelFeat()
        {
            var icon = AbilityRefs.DeathClutch.Reference.Get().Icon;

            return ProgressionConfigurator.New(CharonSentinel, CharonSentinelGuid)
              .SetDisplayName(CharonSentinelDisplayName)
              .SetDescription(CharonSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(CharonGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateCharonSentinel1())
              .AddToLevelEntry(16, CharonSentinel2Feat())
              .AddToLevelEntry(20, CharonSentinel3Feat())
              .Configure();
        }

        private const string CharonSentinel1 = "SpellPower.CharonSentinel1";
        public static readonly string CharonSentinel1Guid = "{BE7F9649-5D74-4B40-9509-DD21F47D4AD6}";
        internal const string CharonSentinel1DisplayName = "SpellPowerCharon1.Name";
        private const string CharonSentinel1Description = "SpellPowerCharon1.Description";

        private const string CharonSentinel1Ablity2 = "SpellPower.UseCharonSentinel12";
        private static readonly string CharonSentinel1Ablity2Guid = "{64546C71-8FFD-4CC8-B82D-F2287C8EEEF0}";

        private static BlueprintFeature CreateCharonSentinel1()
        {
            var icon = AbilityRefs.SummonMonsterISingle.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(CharonSentinel1Ablity2, CharonSentinel1Ablity2Guid)
                .CopyFrom(
                AbilityRefs.SummonMonsterISingle,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(ContextRankConfig))
                .SetIcon(icon)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(CharonSentinel1, CharonSentinel1Guid)
              .SetDisplayName(CharonSentinel1DisplayName)
              .SetDescription(CharonSentinel1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string Charon2 = "DeificObedience.Charon2";
        public static readonly string Charon2Guid = "{03966D1F-4940-4CA6-B7BA-0A54F33CDBDD}";

        internal const string Charon2DisplayName = "DeificObedienceCharon2.Name";
        private const string Charon2Description = "DeificObedienceCharon2.Description";

        private const string Charon2Buff = "DeificObedience.Charon2Buff";
        private static readonly string Charon2BuffGuid = "{265B99BE-2DEA-4690-9CAF-12CADAA7301D}";

        private const string Charon2CoolBuff = "DeificObedience.Charon2CoolBuff";
        private static readonly string Charon2CoolBuffGuid = "{8DCD7F94-7A02-4329-9CB8-2830D82C9723}";

        private const string Charon2Ability = "DeificObedience.Charon2Ability";
        private static readonly string Charon2AbilityGuid = "{97FB71B8-0F1A-4DDA-B206-E9EB9A4DA0EC}";

        private const string Charon2Ability2 = "DeificObedience.Charon2Ability2";
        private static readonly string Charon2Ability2Guid = "{E822B9FD-9AA4-489B-BC55-E4D49C6B939F}";

        internal const string Charon2DisplayName2 = "DeificObedienceCharon22.Name";
        private const string Charon2Description2 = "DeificObedienceCharon22.Description";
        public static BlueprintFeature CharonSentinel2Feat()
        {
            var icon = FeatureRefs.GhostRiderGhostSpiritualBondFeature.Reference.Get().Icon;
            var icon2 = FeatureRefs.GhostRiderSpiritedMountFeature.Reference.Get().Icon;
            var fx = AbilityRefs.Weird.Reference.Get().GetComponent<AbilitySpawnFx>();

            var Buff = BuffConfigurator.New(Charon2Buff, Charon2BuffGuid)
             .SetDisplayName(Charon2DisplayName)
             .SetDescription(Charon2Description)
             .SetIcon(icon)
             .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.Strength, value: -6)
             .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.Dexterity, value: -6)
             .AddStatBonus(ModifierDescriptor.Penalty, stat: StatType.Constitution, value: -6)
             .AddSpellDescriptorComponent(SpellDescriptor.Curse)
             .Configure();

            var CoolDownBuff = BuffConfigurator.New(Charon2CoolBuff, Charon2CoolBuffGuid)
             .SetDisplayName(Charon2DisplayName)
             .SetDescription(Charon2Description)
             .SetIcon(icon)
             .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var ability = AbilityConfigurator.New(Charon2Ability, Charon2AbilityGuid)
                .SetDisplayName(Charon2DisplayName)
                .SetDescription(Charon2Description)
                .SetIcon(icon)
                .AddComponent(fx)
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(10.Feet())
                .AllowTargeting(false, true, false, false)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .AddAbilityTargetHasFact(new() { CoolDownBuff }, inverted: true)
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff, isFromSpell: true)
                        .Build(),
                        succeed: ActionsBuilder.New()
                        .ApplyBuff(CoolDownBuff, ContextDuration.Fixed(1))
                        .Build())
                  .Build(), savingThrowType: SavingThrowType.Fortitude)
                .AddSpellDescriptorComponent(SpellDescriptor.Curse)
                .AddComponent<CustomDC>(c => { c.characterlv = true; c.halfed = true; c.Property = StatType.Charisma; })
                .Configure();

            var ability2 = AbilityConfigurator.New(Charon2Ability2, Charon2Ability2Guid)
                .SetDisplayName(Charon2DisplayName2)
                .SetDescription(Charon2Description2)
                .SetIcon(icon2)
                .AddComponent(fx)
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 30.Feet(), spreadSpeed: 40.Feet(),
                        condition: ConditionsBuilder.New().HasBuff(CoolDownBuff, true).Build())
                .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff, isFromSpell: true)
                        .Build(),
                        succeed: ActionsBuilder.New()
                        .ApplyBuff(CoolDownBuff, ContextDuration.Fixed(1))
                        .Build())
                  .Build(), savingThrowType: SavingThrowType.Fortitude)
                .AddSpellDescriptorComponent(SpellDescriptor.Curse)
                .AddComponent<CustomDC>(c => { c.characterlv = true; c.halfed = true; c.Property = StatType.Charisma; })
                .Configure();

            return FeatureConfigurator.New(Charon2, Charon2Guid)
              .SetDisplayName(Charon2DisplayName)
              .SetDescription(Charon2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, ability2 })
              .Configure();
        }

        private static readonly string Charon3Name = "DeificObedienceCharon3";
        public static readonly string Charon3Guid = "{A3CB21DE-5F99-4883-B1D9-FB35D23F1101}";

        private static readonly string Charon3DisplayName = "DeificObedienceCharon3.Name";
        private static readonly string Charon3Description = "DeificObedienceCharon3.Description";

        private const string Charon3Ability = "DeificObedienceStyle.Charon3Ability";
        private static readonly string Charon3AbilityGuid = "{C3978AAF-F553-4AE4-9A7C-0424F0AA2187}";

        private const string Charon3AbilityRes = "DeificObedienceStyle.Charon3AbilityRes";
        private static readonly string Charon3AbilityResGuid = "{82335487-6D89-4BD7-8781-EB16952ABAF8}";

        public static BlueprintFeature CharonSentinel3Feat()
        {
            var icon = AbilityRefs.WailOfBanshee.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(Charon3AbilityRes, Charon3AbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(Charon3Ability, Charon3AbilityGuid)
                .CopyFrom(
                AbilityRefs.WailOfBanshee,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(AbilitySpawnFx),
                typeof(AbilityTargetsAround),
                typeof(AbilityDeliverDelay),
                typeof(AbilityDifficultyLimitDC),
                typeof(ContextRankConfig))
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 9)
                .AddSpellDescriptorComponent(SpellDescriptor.Death)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(Charon3Name, Charon3Guid)
                    .SetDisplayName(Charon3DisplayName)
                    .SetDescription(Charon3Description)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private const string Szuriel = "DeificObedience.Szuriel";
        public static readonly string SzurielGuid = "{1A12DE3E-E521-4DFA-9BD6-A340E0127EEE}";

        internal const string SzurielDisplayName = "DeificObedienceSzuriel.Name";
        private const string SzurielDescription = "DeificObedienceSzuriel.Description";
        public static BlueprintFeature SzurielFeat()
        {
            var icon = FeatureRefs.ExecutionerAssassinateFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Szuriel, SzurielGuid)
              .SetDisplayName(SzurielDisplayName)
              .SetDescription(SzurielDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.GyronnaFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(SzurielSentinelFeat())
              .AddSavingThrowBonusAgainstDescriptor(value: 4, spellDescriptor: SpellDescriptor.Fire, modifierDescriptor: ModifierDescriptor.Profane)
              .Configure();
        }

        private const string SzurielSentinel = "DeificObedience.SzurielSentinel";
        public static readonly string SzurielSentinelGuid = "{8EBC9513-483A-41CF-AA18-B9E683CD641F}";

        internal const string SzurielSentinelDisplayName = "DeificObedienceSzurielSentinel.Name";
        private const string SzurielSentinelDescription = "DeificObedienceSzurielSentinel.Description";
        public static BlueprintProgression SzurielSentinelFeat()
        {
            var icon = FeatureRefs.ExecutionerAssassinateFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(SzurielSentinel, SzurielSentinelGuid)
              .SetDisplayName(SzurielSentinelDisplayName)
              .SetDescription(SzurielSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(SzurielGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CharonSentinel1Guid)
              .AddToLevelEntry(16, Szuriel2Feat())
              .AddToLevelEntry(20, Szuriel3Feat())
              .Configure();
        }

        private const string Szuriel2 = "DeificObedience.Szuriel2";
        public static readonly string Szuriel2Guid = "{97FF9DE3-B575-4DA7-9643-3495E4CD891F}";

        internal const string Szuriel2DisplayName = "DeificObedienceSzuriel2.Name";
        private const string Szuriel2Description = "DeificObedienceSzuriel2.Description";

        public static BlueprintFeature Szuriel2Feat()
        {
            var icon = FeatureSelectionRefs.ExoticWeaponProficiencySelection.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(Szuriel2, Szuriel2Guid)
              .SetDisplayName(Szuriel2DisplayName)
              .SetDescription(Szuriel2Description)
              .SetIcon(icon);

            var weapons = FeatureSelectionRefs.ExoticWeaponProficiencySelection.Reference.Get().m_AllFeatures;
            foreach (var weapon in weapons)
            {
                feat.AddFacts(new() { weapon.Get() });
            }

            var cond = ConditionsBuilder.New()
                    .IsWeaponEquipped(category: WeaponCategory.Greatsword, checkWeaponCategory: true)
                    .Build();  

            return feat.AddFacts(new() { FeatureRefs.SimpleWeaponProficiency.ToString(), FeatureRefs.MartialWeaponProficiency.ToString() })
              .AddAttackBonusConditional(2, descriptor: ModifierDescriptor.Profane)
              .AddDamageBonusConditional(2, descriptor: ModifierDescriptor.Profane, onlyWeaponDamage: true)
              .AddAttackBonusConditional(4, true, cond, ModifierDescriptor.Profane)
              .AddDamageBonusConditional(4, true, cond, ModifierDescriptor.Profane, true)
              .Configure();
        }

        private const string Szuriel3 = "DeificObedience.Szuriel3";
        public static readonly string Szuriel3Guid = "{5E7FC4DF-2DDF-4718-887D-B7D5B75C2ADC}";

        internal const string Szuriel3DisplayName = "DeificObedienceSzuriel3.Name";
        private const string Szuriel3Description = "DeificObedienceSzuriel3.Description";
        public static BlueprintFeature Szuriel3Feat()
        {
            var icon = FeatureRefs.HellsSealFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Szuriel3, Szuriel3Guid)
              .SetDisplayName(Szuriel3DisplayName)
              .SetDescription(Szuriel3Description)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Strength, 2, ModifierDescriptor.Profane)
              .AddContextStatBonus(StatType.Constitution, 2, ModifierDescriptor.Profane)
              .Configure();
        }

        private const string Iomedae = "DeificObedience.Iomedae";
        public static readonly string IomedaeGuid = "{346F315B-BDDB-441C-8D07-E80B837293EB}";

        internal const string IomedaeDisplayName = "DeificObedienceIomedae.Name";
        private const string IomedaeDescription = "DeificObedienceIomedae.Description";
        public static BlueprintFeature IomedaeFeat()
        {
            var icon = FeatureRefs.IomedaeFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Iomedae, IomedaeGuid)
              .SetDisplayName(IomedaeDisplayName)
              .SetDescription(IomedaeDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.IomedaeFeature.ToString(), group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulGood, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(IomedaeSentinelFeat())
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.CheckDiplomacy, 4)
              .Configure();
        }

        private const string IomedaeSentinel = "DeificObedience.IomedaeSentinel";
        public static readonly string IomedaeSentinelGuid = "{21B9F247-5A20-4DDE-A1AB-6858A8EA8A93}";

        internal const string IomedaeSentinelDisplayName = "DeificObedienceIomedaeSentinel.Name";
        private const string IomedaeSentinelDescription = "DeificObedienceIomedaeSentinel.Description";
        public static BlueprintProgression IomedaeSentinelFeat()
        {
            var icon = FeatureRefs.IomedaeFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(IomedaeSentinel, IomedaeSentinelGuid)
              .SetDisplayName(IomedaeSentinelDisplayName)
              .SetDescription(IomedaeSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(IomedaeGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateIomedae1())
              .AddToLevelEntry(16, Iomedae2Feat())
              .AddToLevelEntry(20, Iomedae3Feat())
              .Configure();
        }

        private const string Iomedae1 = "SpellPower.Iomedae1";
        public static readonly string Iomedae1Guid = "{DC4C281F-067A-40BA-89DE-CF68CDFC6768}";
        internal const string Iomedae1DisplayName = "SpellPowerIomedae1.Name";
        private const string Iomedae1Description = "SpellPowerIomedae1.Description";
        private static BlueprintFeature CreateIomedae1()
        {
            var icon = FeatureRefs.CavalierCharge.Reference.Get().Icon;

            return FeatureConfigurator.New(Iomedae1, Iomedae1Guid)
              .SetDisplayName(Iomedae1DisplayName)
              .SetDescription(Iomedae1Description)
              .SetIcon(icon)
              .AddFacts(new() { Ragathiel1AblityGuid, Gorum1Ablity2Guid, Ragathiel1Ablity3Guid })
              .Configure();
        }

        private const string Iomedae2 = "DeificObedience.Iomedae2";
        public static readonly string Iomedae2Guid = "{9A8217DC-2D73-42A3-A16B-DFDBBD930A6B}";

        private const string Iomedae2Buff = "DeificObedience.Iomedae2Buff";
        public static readonly string Iomedae2BuffGuid = "{6946F51E-C203-4DC4-A2E7-85EA0CD11A2E}";

        internal const string Iomedae2DisplayName = "DeificObedienceIomedae2.Name";
        private const string Iomedae2Description = "DeificObedienceIomedae2.Description";

        public static BlueprintFeature Iomedae2Feat()
        {
            var icon = FeatureRefs.SmiteEvilFeature.Reference.Get().Icon;

            BuffConfigurator.New(Iomedae2Buff, Iomedae2BuffGuid)
             .SetDisplayName(Iomedae2DisplayName)
             .SetDescription(Iomedae2Description)
             .SetIcon(icon)
             .Configure();

            return FeatureConfigurator.New(Iomedae2, Iomedae2Guid)
              .SetDisplayName(Iomedae2DisplayName)
              .SetDescription(Iomedae2Description)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.SmiteEvilAdditionalUse.ToString() })
              .AddDamageBonusAgainstFactOwner(bonus: ContextValues.Rank(), checkedFact: BuffRefs.SmiteEvilBuff.ToString())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { Sentinel.ArchetypeGuid }))
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New()
                    .Add<ValorousSmite>()
                    .Build(), onlyHit: true)
              .Configure();
        }

        private const string Iomedae3 = "DeificObedience.Iomedae3";
        public static readonly string Iomedae3Guid = "{525DCF84-83DC-4F80-9C3C-D62231987BF1}";

        private const string Iomedae3Buff = "DeificObedience.Iomedae3Buff";
        public static readonly string Iomedae3BuffGuid = "{5AB133C7-1B36-4217-BAB8-473DE04848DA}";

        internal const string Iomedae3DisplayName = "DeificObedienceIomedae3.Name";
        private const string Iomedae3Description = "DeificObedienceIomedae3.Description";

        private const string Iomedae3Ablity = "DeificObedience.UseIomedae3";
        private static readonly string Iomedae3AblityGuid = "{926A3480-1E6F-42D4-B349-CF308E847311}";

        private const string Iomedae3AblityRes = "DeificObedience.Iomedae3Res";
        private static readonly string Iomedae3AblityResGuid = "{83EF86BB-7AA5-4A79-8FE9-3F3E7E7D941C}";
        public static BlueprintFeature Iomedae3Feat()
        {
            var icon = AbilityRefs.Banishment.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                    .CastSpell(AbilityRefs.Dismissal.ToString(), overrideSpellLevel: 9)
                    .Build();

            var Buff = BuffConfigurator.New(Iomedae3Buff, Iomedae3BuffGuid)
             .SetDisplayName(Iomedae3DisplayName)
             .SetDescription(Iomedae3Description)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: shoot, actionsOnInitiator: false, triggerBeforeAttack: false, onlyHit: true)
             .AddInitiatorAttackWithWeaponTrigger(action: ActionsBuilder.New().RemoveSelf().Build(), actionsOnInitiator: true, onlyHit: false)
             .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(Iomedae3AblityRes, Iomedae3AblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New(Iomedae3Ablity, Iomedae3AblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffPermanent(Buff).Build())
                .SetDisplayName(Iomedae3DisplayName)
                .SetDescription(Iomedae3Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: Iomedae3AblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Iomedae3, Iomedae3Guid)
              .SetDisplayName(Iomedae3DisplayName)
              .SetDescription(Iomedae3Description)
              .SetIcon(icon)
              .AddSpellPenetrationBonus(value: 1)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Milani = "DeificObedience.Milani";
        public static readonly string MilaniGuid = "{FB69E653-B6DF-44DD-B823-12BA534B4264}";

        internal const string MilaniDisplayName = "DeificObedienceMilani.Name";
        private const string MilaniDescription = "DeificObedienceMilani.Description";
        public static BlueprintFeature MilaniFeat()
        {
            var icon = FeatureRefs.ElectricRoseFeature.Reference.Get().Icon;
            //"MilaniFeature": "C5C537C7-77DB-48B7-BBE8-61414DB4D366",
            return FeatureConfigurator.New(Milani, MilaniGuid)
              .SetDisplayName(MilaniDisplayName)
              .SetDescription(MilaniDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature("C5C537C7-77DB-48B7-BBE8-61414DB4D366", group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulGood, group: Prerequisite.GroupType.Any)
              .AddToIsPrerequisiteFor(MilaniSentinelFeat())
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Sacred, spellDescriptor: SpellDescriptor.Charm, value: 2)
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Sacred, spellDescriptor: SpellDescriptor.Compulsion, value: 2)
              .AddComponent<ContextDispelBonusOnType>(c => {
                  c.Bonus = 2;
                  c.Type = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.CasterLevel;
              })
              .Configure();
        }

        private const string MilaniSentinel = "DeificObedience.MilaniSentinel";
        public static readonly string MilaniSentinelGuid = "{9E9CE292-B127-4B26-A81B-B2C5C4F62B60}";

        internal const string MilaniSentinelDisplayName = "DeificObedienceMilaniSentinel.Name";
        private const string MilaniSentinelDescription = "DeificObedienceMilaniSentinel.Description";
        public static BlueprintProgression MilaniSentinelFeat()
        {
            var icon = FeatureRefs.ElectricRoseFeature.Reference.Get().Icon;

            return ProgressionConfigurator.New(MilaniSentinel, MilaniSentinelGuid)
              .SetDisplayName(MilaniSentinelDisplayName)
              .SetDescription(MilaniSentinelDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(MilaniGuid)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToLevelEntry(12, CreateMilani1())
              .AddToLevelEntry(16, Milani2Feat())
              .AddToLevelEntry(20, Milani3Feat())
              .Configure();
        }

        private const string Milani1 = "SpellPower.Milani1";
        public static readonly string Milani1Guid = "{A3E1040E-7D95-487D-BD9E-1C9F513AB6A9}";
        internal const string Milani1DisplayName = "SpellPowerMilani1.Name";
        private const string Milani1Description = "SpellPowerMilani1.Description";

        private const string MilaniSentinel1Ablity = "SpellPower.UseMilaniSentinel1";
        private static readonly string MilaniSentinel1AblityGuid = "{10700DE2-BAAF-4C2B-91FE-177AB6A51C33}";

        private static BlueprintFeature CreateMilani1()
        {
            var icon = AbilityRefs.ProtectionFromEvil.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(MilaniSentinel1Ablity, MilaniSentinel1AblityGuid)
                .CopyFrom(
                AbilityRefs.ProtectionFromEvil,
                typeof(AbilityEffectRunAction),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent),
                typeof(AbilitySpawnFx))
                .SetIcon(icon)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: DeificObedienceAblityResGuid)
                .Configure();

            return FeatureConfigurator.New(Milani1, Milani1Guid)
              .SetDisplayName(Milani1DisplayName)
              .SetDescription(Milani1Description)
              .SetIcon(icon)
              .AddFacts(new() { ability, Erastil1Ablity2Guid, Ragathiel1Ablity3Guid })
              .Configure();
        }

        private const string Milani2 = "DeificObedience.Milani2";
        public static readonly string Milani2Guid = "{73A5F185-9CA5-4DF7-8586-1DAC09DDD86E}";

        internal const string Milani2DisplayName = "DeificObedienceMilani2.Name";
        private const string Milani2Description = "DeificObedienceMilani2.Description";

        public static BlueprintFeature Milani2Feat()
        {
            var icon = AbilityRefs.Restoration.Reference.Get().Icon;

            return FeatureConfigurator.New(Milani2, Milani2Guid)
              .SetDisplayName(Milani2DisplayName)
              .SetDescription(Milani2Description)
              .SetIcon(icon)
              .AddBuffDescriptorImmunity(false, SpellDescriptor.Charm)
              .AddBuffDescriptorImmunity(false, SpellDescriptor.Fear)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Charm)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Fear)
              .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Sacred, spellDescriptor: SpellDescriptor.Compulsion, value: 4)
              .Configure();
        }

        private const string Milani3 = "DeificObedience.Milani3";
        public static readonly string Milani3Guid = "{F2A2FB7F-63B8-4B24-B8C6-622E80D7FD24}";

        internal const string Milani3DisplayName = "DeificObedienceMilani3.Name";
        private const string Milani3Description = "DeificObedienceMilani3.Description";
        public static BlueprintFeature Milani3Feat()
        {
            var icon = FeatureRefs.FlamewardenPhoenixRising.Reference.Get().Icon;

            return FeatureConfigurator.New(Milani3, Milani3Guid)
              .SetDisplayName(Milani3DisplayName)
              .SetDescription(Milani3Description)
              .SetIcon(icon)
              .Configure();
        }
    }
}
