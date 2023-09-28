using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class SeizetheOpportunity
    {
        private static readonly string FeatName = "SeizetheOpportunity";
        public static readonly string FeatGuid = "{14BA3865-0457-4008-AB94-B01D4C430FDF}";

        private static readonly string DisplayName = "SeizetheOpportunity.Name";
        private static readonly string Description = "SeizetheOpportunity.Description";

        private const string SeizetheAbility = "SeizetheOpportunity.SeizetheOpportunityAbility2";
        private static readonly string SeizetheAbilityGuid = "{0AAE0748-42B6-404B-84B5-8F6745E28701}";

        private const string SeizetheOpportunityAbility = "SeizetheOpportunity.SeizetheOpportunityAbility";
        private static readonly string SeizetheOpportunityAbilityGuid = "{52AE2CA9-7FA4-45E8-8D7B-22AC1C7F0F6C}";

        private const string SeizetheOpportunitybuff = "SeizetheOpportunity.SeizetheOpportunitybuff";
        private static readonly string SeizetheOpportunitybuffGuid = "{367BB862-C085-4DED-9676-0BA0C0BF2DC6}";

        private const string SeizetheBullRushAbility = "SeizetheBullRush.SeizetheBullRushAbility";
        private static readonly string SeizetheBullRushAbilityGuid = "{823D990A-F6F2-462E-A3FD-095AA6013803}";

        private const string SeizetheBullRushbuff = "SeizetheBullRush.SeizetheBullRushbuff";
        private static readonly string SeizetheBullRushbuffGuid = "{FDD7D762-A448-48FB-B72C-709D14285FF6}";

        private const string SeizetheDirtyBlindAbility = "SeizetheDirtyBlind.SeizetheDirtyBlindAbility";
        private static readonly string SeizetheDirtyBlindAbilityGuid = "{42CE505F-94C8-4B1A-8262-F1643A0E4740}";

        private const string SeizetheDirtyBlindbuff = "SeizetheDirtyBlind.SeizetheDirtyBlindbuff";
        private static readonly string SeizetheDirtyBlindbuffGuid = "{6142C847-22F1-410F-A132-9545D7404F4A}";

        private const string SeizetheDirtyEntangleAbility = "SeizetheDirtyEntangle.SeizetheDirtyEntangleAbility";
        private static readonly string SeizetheDirtyEntangleAbilityGuid = "{AB159D3D-1590-4F79-87FC-5BB2C647AE85}";

        private const string SeizetheDirtyEntanglebuff = "SeizetheDirtyEntangle.SeizetheDirtyEntanglebuff";
        private static readonly string SeizetheDirtyEntanglebuffGuid = "{723AC061-8A31-485B-976D-2E233B5B4B9E}";

        private const string SeizetheDirtySickenAbility = "SeizetheDirtySicken.SeizetheDirtySickenAbility";
        private static readonly string SeizetheDirtySickenAbilityGuid = "{0AD95F62-8AFE-4590-9CD4-C27F25B467CF}";

        private const string SeizetheDirtySickenbuff = "SeizetheDirtySicken.SeizetheDirtySickenbuff";
        private static readonly string SeizetheDirtySickenbuffGuid = "{4B94C35F-0F34-494F-9CF3-BB2BAD84FCF9}";

        private const string SeizetheDisarmAbility = "SeizetheDisarm.SeizetheDisarmAbility";
        private static readonly string SeizetheDisarmAbilityGuid = "{4C4FC861-F89B-4C77-BD41-3F91E955FF66}";

        private const string SeizetheDisarmbuff = "SeizetheDisarm.SeizetheDisarmbuff";
        private static readonly string SeizetheDisarmbuffGuid = "{0E686D2C-2CBB-43F4-A247-110A85B938C1}";

        private const string SeizetheSunderAbility = "SeizetheSunder.SeizetheSunderAbility";
        private static readonly string SeizetheSunderAbilityGuid = "{EA74C387-BC06-451C-9D0A-48F8AD5527D4}";

        private const string SeizetheSunderbuff = "SeizetheSunder.SeizetheSunderbuff";
        private static readonly string SeizetheSunderbuffGuid = "{E4FD5F1E-A417-47CE-AF53-CD0FC91225B0}";

        private const string SeizetheTripAbility = "SeizetheTrip.SeizetheTripAbility";
        private static readonly string SeizetheTripAbilityGuid = "{4F4B9302-3A9A-4785-8CF6-D1909161EFF3}";

        private const string SeizetheTripbuff = "SeizetheTrip.SeizetheTripbuff";
        private static readonly string SeizetheTripbuffGuid = "{8DFA0D89-D1D5-4A63-AF3E-37655D9935D5}";

        private const string SeizetheGrappleAbility = "SeizetheGrapple.SeizetheGrappleAbility";
        private static readonly string SeizetheGrappleAbilityGuid = "{E070A95B-49E8-4ABF-8F95-18CCB5024ACC}";

        private const string SeizetheGrapplebuff = "SeizetheGrapple.SeizetheGrapplebuff";
        private static readonly string SeizetheGrapplebuffGuid = "{55BD97E6-84AB-4E2A-9F0D-EF8A730DC7CF}";
        public static void Configure()
        {
            var icon = FeatureRefs.VitalStrikeFeature.Reference.Get().Icon;

            var BuffSeizetheOpportunity = BuffConfigurator.New(SeizetheOpportunitybuff, SeizetheOpportunitybuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityOpportunity = ActivatableAbilityConfigurator.New(SeizetheOpportunityAbility, SeizetheOpportunityAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
                .SetBuff(BuffSeizetheOpportunity)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheBullRush = BuffConfigurator.New(SeizetheBullRushbuff, SeizetheBullRushbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityBullRush = ActivatableAbilityConfigurator.New(SeizetheBullRushAbility, SeizetheBullRushAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.BullRushAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheBullRush)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheDirtyBlind = BuffConfigurator.New(SeizetheDirtyBlindbuff, SeizetheDirtyBlindbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyBlind = ActivatableAbilityConfigurator.New(SeizetheDirtyBlindAbility, SeizetheDirtyBlindAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickBlindnessAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheDirtyBlind)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheDirtyEntangle = BuffConfigurator.New(SeizetheDirtyEntanglebuff, SeizetheDirtyEntanglebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtyEntangle = ActivatableAbilityConfigurator.New(SeizetheDirtyEntangleAbility, SeizetheDirtyEntangleAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickEntangleAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheDirtyEntangle)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheDirtySicken = BuffConfigurator.New(SeizetheDirtySickenbuff, SeizetheDirtySickenbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDirtySicken = ActivatableAbilityConfigurator.New(SeizetheDirtySickenAbility, SeizetheDirtySickenAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DirtyTrickSickenedAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheDirtySicken)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheDisarm = BuffConfigurator.New(SeizetheDisarmbuff, SeizetheDisarmbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityDisarm = ActivatableAbilityConfigurator.New(SeizetheDisarmAbility, SeizetheDisarmAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.DisarmAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheDisarm)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheSunder = BuffConfigurator.New(SeizetheSunderbuff, SeizetheSunderbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilitySunder = ActivatableAbilityConfigurator.New(SeizetheSunderAbility, SeizetheSunderAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.SunderAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheSunder)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheTrip = BuffConfigurator.New(SeizetheTripbuff, SeizetheTripbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityTrip = ActivatableAbilityConfigurator.New(SeizetheTripAbility, SeizetheTripAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.TripAction.Reference.Get().Icon)
                .SetBuff(BuffSeizetheTrip)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSeizetheGrapple = BuffConfigurator.New(SeizetheGrapplebuff, SeizetheGrapplebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(AbilityRefs.VitalStrikeAbility.Reference.Get().Icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityGrapple = ActivatableAbilityConfigurator.New(SeizetheGrappleAbility, SeizetheGrappleAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(AbilityRefs.ArmyShifterGrabAbility.Reference.Get().Icon)
                .SetBuff(BuffSeizetheGrapple)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(SeizetheAbility, SeizetheAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddActivationDisable()
                .SetDeactivateImmediately()
                .SetActivationType(AbilityActivationType.Immediately)
                .AddActivatableAbilityVariants(variants: new() { abilityBullRush, abilityDirtyBlind, abilityDirtyEntangle, abilityDirtySicken, abilityDisarm, abilityGrapple, abilitySunder, abilityTrip, abilityOpportunity })
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddFacts(new() { ability, abilityBullRush, abilityDirtyBlind, abilityDirtyEntangle, abilityDirtySicken, abilityDisarm, abilityGrapple, abilitySunder, abilityTrip, abilityOpportunity })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
