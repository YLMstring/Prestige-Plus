using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.OtherManeuver;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.SpecificManeuver
{
    internal class RangedDisarm
    {
        private const string AceDisarm = "AceDisarm";
        public static readonly string AceDisarmGuid = "{66395822-8C92-4069-AFF2-421B0FC40754}";

        internal const string AceDisarmDisplayName = "AceDisarm.Name";
        private const string AceDisarmDescription = "AceDisarm.Description";
        public static BlueprintFeature AceDisarmFeature()
        {
            var icon = FeatureRefs.SnapShot.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<StealAction>()
                .Build();

            return FeatureConfigurator.New(AceDisarm, AceDisarmGuid, FeatureGroup.Feat)
              .SetDisplayName(AceDisarmDisplayName)
              .SetDescription(AceDisarmDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteFeature(FeatureRefs.DeadlyAimFeature.ToString())
              .AddPrerequisiteFeature(RangeDisarmGuid)
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingBows.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingCrossbows.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingThrown.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string RangeDisarm = "DisarmFeat.RangeDisarm";
        private static readonly string RangeDisarmGuid = "{ADF76E51-1CDB-4BB4-B11A-9B5D4050B654}";
        internal const string RangeDisarmDisplayName = "DisarmFeatRangeDisarm.Name";
        private const string RangeDisarmDescription = "DisarmFeatRangeDisarm.Description";

        private const string RangeDisarmAblity = "DisarmFeat.UseRangeDisarm";
        private static readonly string RangeDisarmAblityGuid = "{52C4449E-AF44-4A79-AE86-B713BA6E7EAD}";

        public static BlueprintFeature CreateRangeDisarm()
        {
            var icon = FeatureRefs.SnapShot.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionRangedTrip>(c => { c.maneuver = Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm; c.Ace = AceDisarmGuid; })
                .Build();

            var ability = AbilityConfigurator.New(RangeDisarmAblity, RangeDisarmAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(RangeDisarmDisplayName)
                .SetDescription(RangeDisarmDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetIsFullRoundAction(true)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow, WeaponCategory.ThrowingAxe, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.SlingStaff, WeaponCategory.Sling })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            return FeatureConfigurator.New(RangeDisarm, RangeDisarmGuid, FeatureGroup.Feat)
              .SetDisplayName(RangeDisarmDisplayName)
              .SetDescription(RangeDisarmDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteFeature(FeatureRefs.DeadlyAimFeature.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
              .AddFacts(new() { ability })
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string MythicDisarm = "DisarmFeat.MythicDisarm";
        public static readonly string MythicDisarmGuid = "{6021670F-046E-4177-B430-A4D3C9C39FFF}";
        internal const string MythicDisarmDisplayName = "DisarmFeatMythicDisarm.Name";
        private const string MythicDisarmDescription = "DisarmFeatMythicDisarm.Description";

        public static BlueprintFeature CreateMythicDisarm()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            return FeatureConfigurator.New(MythicDisarm, MythicDisarmGuid, FeatureGroup.MythicFeat)
              .SetDisplayName(MythicDisarmDisplayName)
              .SetDescription(MythicDisarmDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(RangeDisarmGuid)
              .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
              .Configure();
        }

        private const string SweepingDisarm = "SweepingDisarm";
        public static readonly string SweepingDisarmGuid = "{49A182FB-C45F-4B43-9E02-E2156E5E62D8}";

        private const string SweepingDisarmBuff = "SweepingDisarmBuff";
        public static readonly string SweepingDisarmBuffGuid = "{D8C3E51D-3A2D-4A66-86DD-CEF1D821DF96}";

        internal const string SweepingDisarmDisplayName = "SweepingDisarm.Name";
        private const string SweepingDisarmDescription = "SweepingDisarm.Description";
        public static BlueprintFeature SweepingDisarmFeature()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(SweepingDisarmBuff, SweepingDisarmBuffGuid)
                .SetDisplayName(SweepingDisarmDisplayName)
                .SetDescription(SweepingDisarmDescription)
                .SetIcon(icon)
                .Configure();

            var shoot = ActionsBuilder.New()
                .Add<SweepManeuver>()
                .Build();

            return FeatureConfigurator.New(SweepingDisarm, SweepingDisarmGuid, FeatureGroup.Feat)
              .SetDisplayName(SweepingDisarmDisplayName)
              .SetDescription(SweepingDisarmDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Intelligence, 13)
              .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedDisarm.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
              .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().RemoveBuff(buff).Build())
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string ArmBind = "ArmBind";
        public static readonly string ArmBindGuid = "{892169D2-550F-4D99-9571-8A30C60BFFFD}";

        private const string ArmBindBuff = "ArmBindBuff";
        public static readonly string ArmBindBuffGuid = "{7E0B0105-FF73-43A7-B04D-10479986B6DB}";

        internal const string ArmBindDisplayName = "ArmBind.Name";
        private const string ArmBindDescription = "ArmBind.Description";
        public static BlueprintFeature ArmBindFeature()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(ArmBindBuff, ArmBindBuffGuid)
                .SetDisplayName(ArmBindDisplayName)
                .SetDescription(ArmBindDescription)
                .SetIcon(icon)
                .SetRanks(10)
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
                .AddComponent<ArmBindAutoParry>()
                .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1))
                .Build();

            return FeatureConfigurator.New(ArmBind, ArmBindGuid, FeatureGroup.Feat)
              .SetDisplayName(ArmBindDisplayName)
              .SetDescription(ArmBindDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
              .AddPrerequisiteFeature(FeatureRefs.DoubleSlice.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string FollowUpStrike = "FollowUpStrike";
        public static readonly string FollowUpStrikeGuid = "{2FD62420-E37E-4263-8915-5BCB84597083}";

        internal const string FollowUpStrikeDisplayName = "FollowUpStrike.Name";
        private const string FollowUpStrikeDescription = "FollowUpStrike.Description";

        private const string FollowUpStrikeAbility = "FollowUpStrike.FollowUpStrikeAbility";
        private static readonly string FollowUpStrikeAbilityGuid = "{098728C9-0A86-4A20-B15E-52E5413CED95}";

        private const string FollowUpStrikebuff = "FollowUpStrike.FollowUpStrikebuff";
        private static readonly string FollowUpStrikebuffGuid = "{F5F134A7-E113-4351-B35D-784D11B158A8}";
        public static BlueprintFeature FollowUpStrikeFeature()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<DisarmExtraAttack>()
                .Build();

            var BuffFollowUpStrike = BuffConfigurator.New(FollowUpStrikebuff, FollowUpStrikebuffGuid)
              .SetDisplayName(FollowUpStrikeDisplayName)
              .SetDescription(FollowUpStrikeDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(FollowUpStrikeAbility, FollowUpStrikeAbilityGuid)
                .SetDisplayName(FollowUpStrikeDisplayName)
                .SetDescription(FollowUpStrikeDescription)
                .SetIcon(icon)
                .SetBuff(BuffFollowUpStrike)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            return FeatureConfigurator.New(FollowUpStrike, FollowUpStrikeGuid, FeatureGroup.Feat)
              .SetDisplayName(FollowUpStrikeDisplayName)
              .SetDescription(FollowUpStrikeDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilityTrick })
              .AddPrerequisiteStatValue(StatType.Intelligence, 13)
              .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedDisarm.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 6, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string BreakGuard = "BreakGuard";
        public static readonly string BreakGuardGuid = "{4D409F8D-AF8A-428B-952B-DE0975A5DC18}";

        internal const string BreakGuardDisplayName = "BreakGuard.Name";
        private const string BreakGuardDescription = "BreakGuard.Description";

        private const string BreakGuardAbility = "BreakGuard.BreakGuardAbility";
        private static readonly string BreakGuardAbilityGuid = "{59A6F271-A1A3-4639-B9C5-23F053A1AFF4}";

        private const string BreakGuardbuff = "BreakGuard.BreakGuardbuff";
        private static readonly string BreakGuardbuffGuid = "{10C96991-DFF2-4178-AE18-5F8C4F4F2FD4}";
        public static BlueprintFeature BreakGuardFeature()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<DisarmExtraAttack>(c => { c.attacktype = 2; })
                .Build();

            var BuffBreakGuard = BuffConfigurator.New(BreakGuardbuff, BreakGuardbuffGuid)
              .SetDisplayName(BreakGuardDisplayName)
              .SetDescription(BreakGuardDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, true)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(BreakGuardAbility, BreakGuardAbilityGuid)
                .SetDisplayName(BreakGuardDisplayName)
                .SetDescription(BreakGuardDescription)
                .SetIcon(icon)
                .SetBuff(BuffBreakGuard)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            return FeatureConfigurator.New(BreakGuard, BreakGuardGuid, FeatureGroup.Feat)
              .SetDisplayName(BreakGuardDisplayName)
              .SetDescription(BreakGuardDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilityTrick })
              .AddPrerequisiteStatValue(StatType.Dexterity, 15)
              .AddPrerequisiteStatValue(StatType.Intelligence, 13)
              .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedDisarm.ToString())
              .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string StrikeSeize = "StrikeSeize";
        public static readonly string StrikeSeizeGuid = "{3AEF53F8-9223-4E60-A275-0518C90E683A}";

        internal const string StrikeSeizeDisplayName = "StrikeSeize.Name";
        private const string StrikeSeizeDescription = "StrikeSeize.Description";
        public static BlueprintFeature StrikeSeizeFeature()
        {
            var icon = FeatureRefs.CleaveFeature.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<DisarmExtraAttack>(c => { c.attacktype = 1; c.isSwift = false; })
                .Build();

            return FeatureConfigurator.New(StrikeSeize, StrikeSeizeGuid, FeatureGroup.Feat)
              .SetDisplayName(StrikeSeizeDisplayName)
              .SetDescription(StrikeSeizeDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedDisarm.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
              .AddAbilityUseTrigger(ability: AbilityRefs.DisarmAction.ToString(), action: shoot, actionsOnTarget: true, forOneSpell: true)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }
    }
}
