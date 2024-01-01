using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.MythicGrapple;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Modify;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.CustomAction.OtherFeatRelated;
using System.Drawing;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Constable
    {
        private const string ArchetypeName = "Constable";
        private static readonly string ArchetypeGuid = "{A580225F-2DE4-41E1-BF4C-19442C82981D}";
        internal const string ArchetypeDisplayName = "Constable.Name";
        private const string ArchetypeDescription = "Constable.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.CavalierClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.CavalierMountSelection.ToString())
            .SetRemoveFeaturesEntry(3, FeatureRefs.CavalierCharge.ToString())
            .SetRemoveFeaturesEntry(5, FeatureRefs.CavalierBanner.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.CavalierMightyCharge.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.CavalierBannerGreater.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.CavalierSupremeCharge.ToString())
            .AddToAddFeatures(1, CreateApprehend())
            .AddToAddFeatures(2, CreateApprehend2())
            .AddToAddFeatures(4, FeatureRefs.HunterWoodlandStride.ToString())
            .AddToAddFeatures(5, BadgeFeat())
            .AddToAddFeatures(7, Apprehend2Guid)
            .AddToAddFeatures(11, InstantOrderFeat())
            .AddToAddFeatures(12, Apprehend2Guid)
            .AddToAddFeatures(14, GreaterBadgeFeat())
            .AddToAddFeatures(17, Apprehend2Guid)
            .AddToAddFeatures(20, CreateInstantOrderMove())
              .AddToClassSkills(StatType.SkillAthletics)
              .AddToClassSkills(StatType.SkillMobility)
              .AddToClassSkills(StatType.SkillPerception)
              .AddToClassSkills(StatType.SkillPersuasion)
              .SetReplaceClassSkills(true)
              .Configure();
        }

        private const string Apprehend = "Constable.Apprehend";
        private static readonly string ApprehendGuid = "{527A8ECD-941B-4D2F-89F2-FC53F3206BDF}";
        internal const string ApprehendDisplayName = "ConstableApprehend.Name";
        private const string ApprehendDescription = "ConstableApprehend.Description";

        public static BlueprintFeature CreateApprehend()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;
            return FeatureConfigurator.New(Apprehend, ApprehendGuid)
              .SetDisplayName(ApprehendDisplayName)
              .SetDescription(ApprehendDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.ImprovedUnarmedStrike.ToString(), AerialAssault.StyleActivatableAbility3Guid })
              .Configure();
        }

        private const string Apprehend2 = "Constable.Apprehend2";
        private static readonly string Apprehend2Guid = "{80C96103-C78B-4FB4-9EC0-8A31C9346A36}";
        public static BlueprintFeature CreateApprehend2()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;
            return FeatureConfigurator.New(Apprehend2, Apprehend2Guid)
              .SetDisplayName(ApprehendDisplayName)
              .SetDescription(ApprehendDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetRanks(5)
              .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank())
              .AddCMBBonusForManeuver(checkFact: false, maneuvers: new CombatManeuver[] { CombatManeuver.Trip, CombatManeuver.Disarm, CombatManeuver.Grapple }, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(Apprehend2Guid))
              .Configure();
        }

        private static readonly string BadgeName = "ConstableBadge";
        public static readonly string BadgeGuid = "{ACC289B3-67EE-4918-9571-CB045D1F01D5}";

        private static readonly string BadgeDisplayName = "ConstableBadge.Name";
        private static readonly string BadgeDescription = "ConstableBadge.Description";

        private const string AuraBuff = "Constable.Badgebuff";
        private static readonly string AuraBuffGuid = "{776DA948-3A6C-41ED-8B84-ACF9E32F7E70}";

        private const string AuraBuff2 = "Constable.Badgebuff2";
        private static readonly string AuraBuff2Guid = "{41D0DB7F-AA2E-45FA-8C3C-833DF46F6814}";

        private const string BadgeAura = "Constable.BadgeAura";
        private static readonly string BadgeAuraGuid = "{E251A253-36FD-4C36-B308-90EBAFA7261A}";

        public static BlueprintFeature BadgeFeat()
        {
            var icon = AbilityRefs.IceBody.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(AuraBuff2, AuraBuff2Guid)
              .SetDisplayName(BadgeDisplayName)
              .SetDescription(BadgeDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(bonus: ContextValues.Rank(AbilityRankType.SpeedBonus), spellDescriptor: SpellDescriptor.Compulsion)
              .AddSavingThrowBonusAgainstDescriptor(bonus: ContextValues.Rank(AbilityRankType.SpeedBonus), spellDescriptor: SpellDescriptor.Charm)
              .AddSavingThrowBonusAgainstDescriptor(bonus: ContextValues.Rank(AbilityRankType.SpeedBonus), spellDescriptor: SpellDescriptor.Fear)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.CavalierClass.ToString() }, type: AbilityRankType.SpeedBonus).WithStartPlusDivStepProgression(5))
              .AddAttackBonusAgainstFactOwner(bonus: ContextValues.Rank(AbilityRankType.DamageBonus), checkedFact: BuffRefs.CavalierChallengeBuffTarget.ToString(), descriptor: ModifierDescriptor.Morale)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.CavalierClass.ToString() }, type: AbilityRankType.DamageBonus).WithStartPlusDivStepProgression(5, 5))
              .Configure();

            var area = AbilityAreaEffectConfigurator.New(BadgeAura, BadgeAuraGuid)
                .SetTargetType(BlueprintAbilityAreaEffect.TargetType.Ally)
                .SetAffectDead(false)
                .SetShape(AreaEffectShape.Cylinder)
                .SetSize(33.Feet())
                .AddAbilityAreaEffectBuff(Buff2, false, ConditionsBuilder.New().TargetIsYourself(true).Build())
                .Configure();

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(BadgeDisplayName)
              .SetDescription(BadgeDescription)
              .SetIcon(icon)
              .AddAreaEffect(area)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            return FeatureConfigurator.New(BadgeName, BadgeGuid)
                    .SetDisplayName(BadgeDisplayName)
                    .SetDescription(BadgeDescription)
                    .SetIcon(icon)
                    .AddAuraFeatureComponent(Buff1)
                    .Configure();
        }

        private const string GreaterBadge = "Constable.GreaterBadge";
        private static readonly string GreaterBadgeGuid = "{91380B1C-701F-4BD4-83E1-EB4B59440E21}";

        internal const string GreaterBadgeDisplayName = "ConstableGreaterBadge.Name";
        private const string GreaterBadgeDescription = "ConstableGreaterBadge.Description";

        private const string GreaterBadgeAbility = "Constable.GreaterBadgeAbility";
        private static readonly string GreaterBadgeAbilityGuid = "{A028ABF1-8707-4BBF-AFA6-8CC7D163EF39}";

        private const string GreaterBadgeCooldownBuff = "Constable.GreaterBadgeCooldownBuff";
        private static readonly string GreaterBadgeCooldownBuffGuid = "{8257C172-2F03-4721-B7A3-9696B9069E2A}";

        private const string GreaterBadgeBuff = "Constable.GreaterBadgeBuff";
        private static readonly string GreaterBadgeBuffGuid = "{04613219-EC7A-40A4-90D5-0A4103BA6A91}";

        public static BlueprintFeature GreaterBadgeFeat()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;

            var CooldownBuff = BuffConfigurator.New(GreaterBadgeCooldownBuff, GreaterBadgeCooldownBuffGuid)
                .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
                .AddToFlags(BlueprintBuff.Flags.RemoveOnRest)
                .Configure();

            var Buff = BuffConfigurator.New(GreaterBadgeBuff, GreaterBadgeBuffGuid)
              .SetDisplayName(GreaterBadgeDisplayName)
              .SetDescription(GreaterBadgeDescription)
              .SetIcon(icon)
              .AddSpellDescriptorComponent(descriptor: SpellDescriptor.TemporaryHP)
              .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.CavalierClass.ToString() }, type: AbilityRankType.SpeedBonus).WithBonusValueProgression(0, true))
              .Configure();

            var ability = AbilityConfigurator.New(GreaterBadgeAbility, GreaterBadgeAbilityGuid)
                .CopyFrom(
                AbilityRefs.FalseLife,
                typeof(AbilitySpawnFx))
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().HasFact(CooldownBuff, true).TargetIsYourself(true).Build(),
                    ifTrue: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes))
                        .ApplyBuff(CooldownBuff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days))
                        .Build())
                    .Build())
                .SetDisplayName(GreaterBadgeDisplayName)
                .SetDescription(GreaterBadgeDescription)
                .SetIcon(icon)
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Extraordinary)
                .Configure();

            return FeatureConfigurator.New(GreaterBadge, GreaterBadgeGuid)
              .SetDisplayName(GreaterBadgeDisplayName)
              .SetDescription(GreaterBadgeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string InstantOrder = "Constable.InstantOrder";
        private static readonly string InstantOrderGuid = "{C3CEC9C3-B2D6-483C-A996-38E5172A2774}";

        internal const string InstantOrderDisplayName = "ConstableInstantOrder.Name";
        private const string InstantOrderDescription = "ConstableInstantOrder.Description";

        private const string InstantOrderAbility = "Constable.InstantOrderAbility";
        private static readonly string InstantOrderAbilityGuid = "{3E0CAC1D-E913-48E7-8E87-7F750644D4DA}";

        private const string InstantOrderCooldownBuff = "Constable.InstantOrderCooldownBuff";
        private static readonly string InstantOrderCooldownBuffGuid = "{D1BA3070-0726-475C-B9E3-4ECB663B0379}";

        public static BlueprintFeature InstantOrderFeat()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;

            var CooldownBuff = BuffConfigurator.New(InstantOrderCooldownBuff, InstantOrderCooldownBuffGuid)
                .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            var ability = AbilityConfigurator.New(InstantOrderAbility, InstantOrderAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Add<InstantOrderAttack>()
                        .ApplyBuff(CooldownBuff, ContextDuration.Fixed(1), toCaster: true)
                        .Build())
                .SetDisplayName(InstantOrderDisplayName)
                .SetDescription(InstantOrderDescription)
                .SetIcon(icon)
                .AllowTargeting(false, false, true, false)
                .AddAbilityCasterHasNoFacts(new() { CooldownBuff })
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Extraordinary)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .Configure();

            return FeatureConfigurator.New(InstantOrder, InstantOrderGuid)
              .SetDisplayName(InstantOrderDisplayName)
              .SetDescription(InstantOrderDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string InstantOrderMove = "Constable.InstantOrderMove";
        private static readonly string InstantOrderMoveGuid = "{208D53F3-AAAF-4813-88BF-CE1F5636C60A}";
        internal const string InstantOrderMoveDisplayName = "ConstableInstantOrderMove.Name";
        private const string InstantOrderMoveDescription = "ConstableInstantOrderMove.Description";

        public static BlueprintFeature CreateInstantOrderMove()
        {
            var icon = AbilityRefs.Glitterdust.Reference.Get().Icon;
            return FeatureConfigurator.New(InstantOrderMove, InstantOrderMoveGuid)
              .SetDisplayName(InstantOrderMoveDisplayName)
              .SetDescription(InstantOrderMoveDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddComponent<ChangeActionSpell>(a => { a.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>(InstantOrderAbilityGuid); a.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .Configure();
        }
    }
}
