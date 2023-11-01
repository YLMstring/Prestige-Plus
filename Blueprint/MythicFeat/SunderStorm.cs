using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using PrestigePlus.Modify;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.OtherManeuver;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Maneuvers
{
    internal class SunderStorm
    {
        private const string SunderStormFeat = "Mythic.SunderStorm";
        private static readonly string SunderStormGuid = "{025D489D-605A-4BC8-915D-FDFB19D31D7B}";
        internal const string SunderStormDisplayName = "MythicSunderStorm.Name";
        private const string SunderStormDescription = "MythicSunderStorm.Description";

        private const string SunderStormAblity = "Mythic.UseSunderStorm";
        private static readonly string SunderStormAblityGuid = "{2EC425B2-2E4C-4FD3-8574-B6C2FF29D12D}";

        private const string SunderStormBuff = "Mythic.UseSunderStormBuff";
        public static readonly string SunderStormBuffGuid = "{1242A045-E81D-4CCB-BD81-9AC3BF99C119}";

        private const string SunderStormAblity2 = "Mythic.UseSunderStorm2";
        private static readonly string SunderStormAblityGuid2 = "{03F1AFBA-2890-4FB5-B5F1-BBDE8DDDB22E}";

        public static BlueprintFeature CreateSunderStorm()
        {
            var icon = FeatureRefs.CoordinatedManeuvers.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(SunderStormBuff, SunderStormBuffGuid)
              .SetDisplayName(SunderStormDisplayName)
              .SetDescription(SunderStormDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<TearApartSunder>()
              .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
              .SetRanks(20)
              .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
              .Configure();

            var action = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.MythicLevel, toCaster: true), diceCount: 1), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine))
                .Build();

            var shoot = ActionsBuilder.New()
                .Add<ContextActionSunderStorm>()
                .Build();

            var ability = AbilityConfigurator.New(SunderStormAblity, SunderStormAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SunderStormDisplayName)
                .SetDescription(SunderStormDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 50.Feet(), targetType: TargetType.Enemy)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var cast = ActionsBuilder.New()
                .CastSpell(ability)
                .Build();

            var ability2 = AbilityConfigurator.New(SunderStormAblity2, SunderStormAblityGuid2)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(cast).Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(SunderStormDisplayName)
                .SetDescription(SunderStormDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            return FeatureConfigurator.New(SunderStormFeat, SunderStormGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(SunderStormDisplayName)
              .SetDescription(SunderStormDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedSunder.ToString())
              .AddFacts(new() { ability2 })
              .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, true)
              .Configure();
        }

        private const string CleavingSweepFeat = "Mythic.CleavingSweep";
        private static readonly string CleavingSweepGuid = "{B21B2583-75AA-46AD-87DE-EDB05995EEA8}";
        internal const string CleavingSweepDisplayName = "MythicCleavingSweep.Name";
        private const string CleavingSweepDescription = "MythicCleavingSweep.Description";

        private const string CleavingSweepAblity = "Mythic.UseCleavingSweep";
        private static readonly string CleavingSweepAblityGuid = "{AA4D762A-A6E0-4B36-939C-2CD0E795FEF0}";

        private const string CleavingSweepAblity2 = "Mythic.UseCleavingSweep2";
        private static readonly string CleavingSweepAblityGuid2 = "{D73BB7A1-7CBA-4994-ABE3-A515EA6957B6}";

        public static BlueprintFeature CreateCleavingSweep()
        {
            var icon = FeatureRefs.CleavingFinish.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionSunderStorm>(c => { c.type = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip; c.UseWeapon = true; })
                .Build();

            var ability = AbilityConfigurator.New(CleavingSweepAblity, CleavingSweepAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(CleavingSweepDisplayName)
                .SetDescription(CleavingSweepDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 50.Feet(), targetType: TargetType.Enemy)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var cast = ActionsBuilder.New()
                .CastSpell(ability)
                .Build();

            var ability2 = AbilityConfigurator.New(CleavingSweepAblity2, CleavingSweepAblityGuid2)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(cast).Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(CleavingSweepDisplayName)
                .SetDescription(CleavingSweepDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Greataxe })
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            return FeatureConfigurator.New(CleavingSweepFeat, CleavingSweepGuid, FeatureGroup.Feat)
              .SetDisplayName(CleavingSweepDisplayName)
              .SetDescription(CleavingSweepDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Strength, 15)
              .AddPrerequisiteFeature(FeatureRefs.CleaveFeature.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedTrip.ToString())
              .AddPrerequisiteParametrizedWeaponFeature(ParametrizedFeatureRefs.WeaponFocus.ToString(), WeaponCategory.Greataxe)
              .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 11)
              .AddFacts(new() { ability2 })
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string DriveBackFeat = "Mythic.DriveBack";
        private static readonly string DriveBackGuid = "{210FB0A5-8DF4-47F0-ACB4-F43960487EBC}";
        internal const string DriveBackDisplayName = "MythicDriveBack.Name";
        private const string DriveBackDescription = "MythicDriveBack.Description";

        private const string DriveBackAblity = "Mythic.UseDriveBack";
        private static readonly string DriveBackAblityGuid = "{C1D508C9-8474-41F0-8116-CCF7343B69E0}";

        private const string DriveBackAblity2 = "Mythic.UseDriveBack2";
        private static readonly string DriveBackAblityGuid2 = "{BE344D8F-A1CA-4054-8AC4-B70FDF706B82}";

        public static BlueprintFeature CreateDriveBack()
        {
            var icon = FeatureRefs.IndomitableWill.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionSunderStorm>(c => { c.type = Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush; c.UseWeapon = true; c.HitFirst = true; })
                .Build();

            var ability = AbilityConfigurator.New(DriveBackAblity, DriveBackAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(DriveBackDisplayName)
                .SetDescription(DriveBackDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 50.Feet(), targetType: TargetType.Enemy)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var cast = ActionsBuilder.New()
                .CastSpell(ability)
                .Build();

            var ability2 = AbilityConfigurator.New(DriveBackAblity2, DriveBackAblityGuid2)
                .CopyFrom(
                AbilityRefs.DazzlingDisplayAction,
                typeof(AbilitySpawnFx))
                .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(cast).Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(DriveBackDisplayName)
                .SetDescription(DriveBackDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            return FeatureConfigurator.New(DriveBackFeat, DriveBackGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(DriveBackDisplayName)
              .SetDescription(DriveBackDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Dexterity, 13)
              .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.Intelligence, 13)
              .AddPrerequisiteFeature(FeatureRefs.Dodge.ToString())
              .AddPrerequisiteFeature(FeatureRefs.Mobility.ToString())
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string CombatTrickeryFeat = "Mythic.CombatTrickery";
        private static readonly string CombatTrickeryGuid = "{DF34E004-39DA-4663-987C-1B4B0AA16CC6}";
        internal const string CombatTrickeryDisplayName = "MythicCombatTrickery.Name";
        private const string CombatTrickeryDescription = "MythicCombatTrickery.Description";

        private const string CombatTrickeryAblity = "Mythic.UseCombatTrickery";
        private static readonly string CombatTrickeryAblityGuid = "{43E9FED2-AE57-4050-83B6-C14FD9F21611}";

        public static BlueprintFeature CreateCombatTrickery()
        {
            var icon = FeatureRefs.BestJokes.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionCombatTrickery>()
                .Build();

            var ability = AbilityConfigurator.New(CombatTrickeryAblity, CombatTrickeryAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(CombatTrickeryDisplayName)
                .SetDescription(CombatTrickeryDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 5.Feet(), targetType: TargetType.Enemy)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.SelfTouch)
                .Configure();

            return FeatureConfigurator.New(CombatTrickeryFeat, CombatTrickeryGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(CombatTrickeryDisplayName)
              .SetDescription(CombatTrickeryDescription)
              .SetIcon(icon)
              .AddPrerequisiteFullStatValue(stat: Kingmaker.EntitySystem.Stats.StatType.SneakAttack, value: 2)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
