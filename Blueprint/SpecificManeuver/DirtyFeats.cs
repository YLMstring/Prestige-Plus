using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.BasicEx;

namespace PrestigePlus.Blueprint.SpecificManeuver
{
    internal class DirtyFeats
    {
        private static readonly string FeatName = "FeatQuickDirtyTrick";
        private static readonly string FeatGuid = "{75E92D69-1A88-4B7A-A679-5FAF954CE690}";

        private static readonly string DisplayName = "FeatQuickDirtyTrick.Name";
        private static readonly string Description = "FeatQuickDirtyTrick.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.OracleRevelationManeuverMasteryDirtyTrick.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Intelligence, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddFacts(new() { ReplaceAttack.DirtyBlindQuick2AbilityGuid, ReplaceAttack.DirtyEntangleQuick2AbilityGuid, ReplaceAttack.DirtySickenQuick2AbilityGuid })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string StyleName = "DirtyTrickMaster";
        public static readonly string StyleGuid = "{B36A5D42-BD74-4A0F-9F40-9D202B67D783}";

        private static readonly string StyleDisplayName = "DirtyTrickMaster.Name";
        private static readonly string StyleDescription = "DirtyTrickMaster.Description";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                
                    .Build();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickEntangle, true)
                    .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrickSickened, true)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 11)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.GreaterDirtyTrick.ToString())
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string AngelName = "DrivingAssaultAngel";
        public static readonly string AngelGuid = "{AA48D934-F938-433A-AC09-6DBCE6C7D242}";

        private static readonly string AngelDisplayName = "DrivingAssaultAngel.Name";
        private static readonly string AngelDescription = "DrivingAssaultAngel.Description";

        private const string Angelbuff = "DrivingAssaultAngel.Angelbuff";
        private static readonly string AngelbuffGuid = "{63B6ACBF-15F7-4927-88C4-637841D071C9}";

        private const string Angelbuff2 = "DrivingAssaultAngel.Angelbuff2";
        private static readonly string Angelbuff2Guid = "{875A2F41-8FDF-4F84-825F-281828FCE49A}";

        private const string AngelActivatableAbility = "DrivingAssaultAngel.AngelActivatableAbility";
        private static readonly string AngelActivatableAbilityGuid = "{C46D9760-C6BC-4188-A013-1E979E6453C5}";
        public static void AngelConfigure()
        {
            var icon = FeatureRefs.DisplayWeaponProwess.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                        .ApplyBuff(Angelbuff2Guid, ContextDuration.Fixed(1))
                        .Build();

            var Buff = BuffConfigurator.New(Angelbuff, AngelbuffGuid)
              .SetDisplayName(AngelDisplayName)
              .SetDescription(AngelDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddInitiatorAttackWithWeaponTrigger(action, actionsOnInitiator: true, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee, onlyOnFullAttack: true, onlyOnFirstAttack: true, triggerBeforeAttack: true)
              .Configure();

            var action2 = ActionsBuilder.New()
                        .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrick)
                        .Build();

            BuffConfigurator.New(Angelbuff2, Angelbuff2Guid)
              .SetDisplayName(AngelDisplayName)
              .SetDescription(AngelDescription)
              .SetIcon(icon)
              //.AddAttackBonusConditional(bonus: ContextValues.Constant(-2), descriptor: ModifierDescriptor.Penalty)
              .AddContextStatBonus(StatType.AdditionalAttackBonus, -2, ModifierDescriptor.Penalty)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrick }, value: ContextValues.Constant(2))
              .AddInitiatorAttackWithWeaponTrigger(action2, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee, onlyOnFullAttack: true, onlyOnFirstHit: true, onlyHit: true)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(AngelActivatableAbility, AngelActivatableAbilityGuid)
                .SetDisplayName(AngelDisplayName)
                .SetDescription(AngelDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(AngelName, AngelGuid)
                    .SetDisplayName(AngelDisplayName)
                    .SetDescription(AngelDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, FollowActivatableAbilityGuid })
                    .Configure();
        }

        private static readonly string FollowDisplayName = "DrivingAssaultFollow.Name";
        private static readonly string FollowDescription = "DrivingAssaultFollow.Description";

        private const string Followbuff = "DrivingAssaultFollow.Followbuff";
        private static readonly string FollowbuffGuid = "{7F33CB9D-B39D-44D4-9A4B-054EB44E0854}";

        private const string FollowActivatableAbility = "DrivingAssaultFollow.FollowActivatableAbility";
        private static readonly string FollowActivatableAbilityGuid = "{880334EA-D852-49AB-AB41-2574DF79CCEB}";
        public static void FollowConfigure()
        {
            var icon = FeatureRefs.FastMovement.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Followbuff, FollowbuffGuid)
              .SetDisplayName(FollowDisplayName)
              .SetDescription(FollowDescription)
              .SetIcon(icon)
              .AddComponent<DirtyTrickFollower>()
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            ActivatableAbilityConfigurator.New(FollowActivatableAbility, FollowActivatableAbilityGuid)
                .SetDisplayName(FollowDisplayName)
                .SetDescription(FollowDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();
        }

        private static readonly string MonsterName = "DrivingAssaultMonster";
        public static readonly string MonsterGuid = "{9516B0D1-ABE1-4994-9B27-C4A8D15EEB2C}";

        private static readonly string MonsterDisplayName = "DrivingAssaultMonster.Name";
        private static readonly string MonsterDescription = "DrivingAssaultMonster.Description";

        private const string Monsterbuff = "DrivingAssaultMonster.Monsterbuff";
        private static readonly string MonsterbuffGuid = "{98DC9E8F-E5DA-4F8A-A17E-0DB01CABB2F7}";

        private const string MonsterActivatableAbility = "DrivingAssaultMonster.MonsterActivatableAbility";
        private static readonly string MonsterActivatableAbilityGuid = "{DCE8709E-0B4D-41F5-AD05-268405DBFC76}";
        public static void MonsterConfigure()
        {
            var icon = FeatureRefs.ShifterAspectElephant.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                        .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.DirtyTrick)
                        .Build();

            var Buff = BuffConfigurator.New(Monsterbuff, MonsterbuffGuid)
              .SetDisplayName(MonsterDisplayName)
              .SetDescription(MonsterDescription)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee, onlyHit: true, onCharge: true)
              .AddComponent<DirtyTrickMonster>()
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(MonsterActivatableAbility, MonsterActivatableAbilityGuid)
                .SetDisplayName(MonsterDisplayName)
                .SetDescription(MonsterDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(MonsterName, MonsterGuid, FeatureGroup.Feat)
                    .SetDisplayName(MonsterDisplayName)
                    .SetDescription(MonsterDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedDirtyTrick.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.Endurance.ToString())
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string DragName = "AngelDrag";
        public static readonly string DragGuid = "{A9BF0D47-08BD-4FEE-9CA2-1B329E1DCBD4}";

        private static readonly string DragDisplayName = "AngelDrag.Name";
        private static readonly string DragDescription = "AngelDrag.Description";

        private const string StyleAbility = "AngelDrag.StyleAbility";
        private static readonly string StyleAbilityGuid = "{93359A78-51FD-4B3A-846F-C4405E6D3F69}";
        public static void DragConfigure()
        {
            var icon = FeatureRefs.Outflank.Reference.Get().Icon;

            var grab = ActionsBuilder.New()
                .Add<ContextActionCastling>()
                .Build();

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .SetDisplayName(DragDisplayName)
                .SetDescription(DragDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(grab)
                .SetType(AbilityType.Physical)
                .SetCanTargetEnemies(false)
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(true)
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(5)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            FeatureConfigurator.New(DragName, DragGuid)
                    .SetDisplayName(DragDisplayName)
                    .SetDescription(DragDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
