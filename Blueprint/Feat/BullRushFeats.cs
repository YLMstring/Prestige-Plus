using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction.GrappleThrow;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class BullRushFeats
    {
        private static readonly string FeatName = "FeatQuickBullRush";
        private static readonly string FeatGuid = "{15C0B67C-9CB7-47D4-A763-0D494840C767}";

        private static readonly string DisplayName = "FeatQuickBullRush.Name";
        private static readonly string Description = "FeatQuickBullRush.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.PummelingCharge.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                    .AddFacts(new() { ReplaceAttack.BullRushQuickAbilityGuid, ReplaceAttack.BullRushQuick2AbilityGuid })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string Feat2Name = "FeatKnockback";
        private static readonly string Feat2Guid = "{55DA148B-C5DA-42C2-9F29-30F2EEB75C9D}";

        private static readonly string DisplayName2 = "FeatKnockback.Name";
        private static readonly string Description2 = "FeatKnockback.Description";

        public static void Configure2()
        {
            var icon = ActivatableAbilityRefs.DivinationSchoolForetellAidToggleAbility.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, FeatureGroup.RagePower)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddBuffExtraEffects(BuffRefs.BloodragerStandartRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .AddBuffExtraEffects(BuffRefs.StandartFocusedRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .AddBuffExtraEffects(BuffRefs.StandartRageBuff.ToString(), extraEffectBuff: ReplaceAttack.BullRushAngry2buffGuid)
                    .Configure();
        }

        private static readonly string StyleName = "HurricanePunchStyle";
        public static readonly string StyleGuid = "{B36A5D42-BD74-4A0F-9F40-9D202B67D783}";

        private static readonly string StyleDisplayName = "HurricanePunchStyle.Name";
        private static readonly string StyleDescription = "HurricanePunchStyle.Description";

        private const string Stylebuff = "HurricanePunchStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{8F0F5E11-A4F0-4DEC-9731-292756B545D7}";

        private const string Stylebuff2 = "HurricanePunchStyle.Stylebuff2";
        private static readonly string Stylebuff2Guid = "{70D9D508-53D2-4662-844F-D2425B445F71}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var Buff2 = BuffConfigurator.New(Stylebuff2, Stylebuff2Guid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddAutoMetamagic(new() { AbilityRefs.BullRushAction.ToString() },
              metamagic: Metamagic.Quicken, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: true)
              .Configure();

            var action = ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().HasFact(Buff).Build(),
                    ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff, ContextDuration.Fixed(1))
                        .Build(),
                    ifTrue: ActionsBuilder.New()
                        .RemoveBuff(Buff)
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                        .Build())
                    .Build();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddInitiatorAttackWithWeaponTrigger(action, category: Kingmaker.Enums.WeaponCategory.UnarmedStrike, checkWeaponCategory: true, onlyHit: true, actionsOnInitiator: true)
                    .AddFacts(new() { FollowActivatableAbilityGuid })
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
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                        .ApplyBuff(Angelbuff2Guid, ContextDuration.Fixed(1))
                        .Build();

            var Buff = BuffConfigurator.New(Angelbuff, AngelbuffGuid)
              .SetDisplayName(AngelDisplayName)
              .SetDescription(AngelDescription)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action, actionsOnInitiator: true, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee, onlyOnFullAttack: true, onlyOnFirstAttack: true, triggerBeforeAttack: true)
              .Configure();

            var action2 = ActionsBuilder.New()
                        .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush)
                        .Build();

            BuffConfigurator.New(Angelbuff2, Angelbuff2Guid)
              .SetDisplayName(AngelDisplayName)
              .SetDescription(AngelDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Constant(-2), descriptor: ModifierDescriptor.Penalty)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush }, value: ContextValues.Constant(2))
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
            var icon = FeatureRefs.FlurryOfBlows.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Followbuff, FollowbuffGuid)
              .SetDisplayName(FollowDisplayName)
              .SetDescription(FollowDescription)
              .SetIcon(icon)
              .AddComponent<BullRushFollower>()
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
                        .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush)
                        .Build();

            var Buff = BuffConfigurator.New(Monsterbuff, MonsterbuffGuid)
              .SetDisplayName(MonsterDisplayName)
              .SetDescription(MonsterDescription)
              .SetIcon(icon)
              .AddInitiatorAttackWithWeaponTrigger(action, checkWeaponRangeType: true, rangeType: WeaponRangeType.Melee, onlyHit: true, onCharge: true)
              .AddComponent<BullRushMonster>()
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
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
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
