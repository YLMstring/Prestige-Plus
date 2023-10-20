using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomAction;
using PrestigePlus.Modify;

namespace PrestigePlus.Maneuvers
{
    internal class RangedTrip
    {
        private const string AceTrip = "AceTrip";
        public static readonly string AceTripGuid = "{2F5DA0E5-5C57-4BF1-8954-C910958E132D}";

        internal const string AceTripDisplayName = "AceTrip.Name";
        private const string AceTripDescription = "AceTrip.Description";
        public static BlueprintFeature AceTripFeature()
        {
            var icon = FeatureRefs.SnapShot.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionAceTrip>()
                .Build();

            return FeatureConfigurator.New(AceTrip, AceTripGuid, FeatureGroup.Feat)
              .SetDisplayName(AceTripDisplayName)
              .SetDescription(AceTripDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteFeature(FeatureRefs.DeadlyAimFeature.ToString())
              .AddPrerequisiteFeature(TripRangedGuid)
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingBows.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingCrossbows.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddPrerequisiteFeature(FeatureRefs.WeaponTrainingThrown.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
              .AddManeuverTrigger(shoot, Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, true)
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string TripRanged = "TripFeat.TripRanged";
        private static readonly string TripRangedGuid = "{765EF30B-BCE5-45E8-9326-858BC16CD102}";
        internal const string TripRangedDisplayName = "TripFeatTripRanged.Name";
        private const string TripRangedDescription = "TripFeatTripRanged.Description";

        private const string TripRangedAblity = "TripFeat.UseTripRanged";
        private static readonly string TripRangedAblityGuid = "{2EBE8356-E4AD-43D7-A5E0-E24E822CBF95}";

        public static BlueprintFeature CreateTripRanged()
        {
            var icon = FeatureRefs.SnapShot.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionRangedTrip>()
                .Build();

            var ability = AbilityConfigurator.New(TripRangedAblity, TripRangedAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(TripRangedDisplayName)
                .SetDescription(TripRangedDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetIsFullRoundAction(true)
                .AddAbilityCasterMainWeaponCheck(new WeaponCategory[] { WeaponCategory.Longbow, WeaponCategory.Shortbow, WeaponCategory.LightCrossbow, WeaponCategory.HeavyCrossbow, WeaponCategory.ThrowingAxe, WeaponCategory.Dart, WeaponCategory.Javelin, WeaponCategory.SlingStaff, WeaponCategory.Sling })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Thrown)
                .SetRange(AbilityRange.Weapon)
                .Configure();

            return FeatureConfigurator.New(TripRanged, TripRangedGuid, FeatureGroup.Feat)
              .SetDisplayName(TripRangedDisplayName)
              .SetDescription(TripRangedDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteFeature(FeatureRefs.DeadlyAimFeature.ToString())
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
              .AddFacts(new() { ability })
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string VindictiveFall = "TripFeat.VindictiveFall";
        private static readonly string VindictiveFallGuid = "{18F000CF-33BE-4421-A44D-444670C0BEC4}";
        internal const string VindictiveFallDisplayName = "TripFeatVindictiveFall.Name";
        private const string VindictiveFallDescription = "TripFeatVindictiveFall.Description";

        private const string VindictiveFallAblity = "TripFeat.UseVindictiveFall";
        private static readonly string VindictiveFallAblityGuid = "{C11E564F-CDC3-42D7-BF87-E5D040CBB8D3}";

        public static BlueprintFeature CreateVindictiveFall()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .Add<ContextActionAceTrip>()
                .Build();

            var ability = AbilityConfigurator.New(VindictiveFallAblity, VindictiveFallAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(VindictiveFallDisplayName)
                .SetDescription(VindictiveFallDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .Configure();

            return FeatureConfigurator.New(VindictiveFall, VindictiveFallGuid, FeatureGroup.Feat)
              .SetDisplayName(VindictiveFallDisplayName)
              .SetDescription(VindictiveFallDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.LightningReflexes.ToString())
              .AddComponent<HandleVindictiveFall>()
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
