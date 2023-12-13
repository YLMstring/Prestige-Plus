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

namespace PrestigePlus.Blueprint.Feat
{
    internal class DeificObedience
    {
        private const string DeificObedienceFeat = "DeificObedience.DeificObedience";
        public static readonly string DeificObedienceGuid = "{E017281A-7AA3-4C6D-AC27-2D57C45B4A81}";

        internal const string DeificObedienceDisplayName = "DeificObedience.Name";
        private const string DeificObedienceDescription = "DeificObedience.Description";

        private const string DeificObedienceAblityRes = "DeificObedienceAblityRes";
        private static readonly string DeificObedienceAblityResGuid = "{751BD21A-A532-4939-9DC9-EE26E7FCE6BB}";

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
              .AddPrerequisiteNoFeature(FeatureRefs.AtheismFeature.ToString())
              .AddPrerequisiteNoFeature(DeificObedienceGuid)
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

        private const string AuraBuff2 = "DeificObedienceStyle.Naderi3buff2";
        private static readonly string AuraBuff2Guid = "{35E97B8C-B385-4AC4-9556-D54DEF11DF56}";

        private const string Naderi3Aura = "DeificObedienceStyle.Naderi3Aura";
        private static readonly string Naderi3AuraGuid = "{0D79B107-54C2-4439-BF31-CA3B1C4A0A74}";

        private const string Naderi3Ability = "DeificObedienceStyle.Naderi3Ability";
        private static readonly string Naderi3AbilityGuid = "{BDC2A287-8FA8-4722-9206-A8E131D64EC8}";

        private const string Naderi3AbilityRes = "DeificObedienceStyle.Naderi3AbilityRes";
        private static readonly string Naderi3AbilityResGuid = "{489136AD-7D46-44BD-979C-8633B294F324}";

        public static BlueprintFeature NaderiSentinel3Feat()
        {
            var icon = AbilityRefs.IceBody.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AuraBuff2, AuraBuff2Guid)
              .SetDisplayName(Naderi3DisplayName)
              .SetDescription(Naderi3Description)
              .SetIcon(icon)
              .Configure();

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
              .AddToLevelEntry(2, CreateErastil1())
              .AddToLevelEntry(6, Erastil2Feat())
              .AddToLevelEntry(10, Erastil3Feat())
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
            var icon = FeatureRefs.HuntersBond.Reference.Get().Icon;

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
    }
}
