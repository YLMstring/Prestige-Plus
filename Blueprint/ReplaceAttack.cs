using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint
{
    internal class ReplaceAttack
    {
        private const string DisarmAbility = "Disarm.DisarmAbility";
        private static readonly string DisarmAbilityGuid = "{901B42B3-8B0E-4B06-B2F4-E46A207B8E52}";

        private const string Disarmbuff = "Disarm.Disarmbuff";
        private static readonly string DisarmbuffGuid = "{59ED0B02-A211-4323-9717-CBE6A1CD6846}";

        private const string Disarm2Ability = "Disarm.Disarm2Ability";
        private static readonly string Disarm2AbilityGuid = "{60E22A2E-E9A4-459E-B7AD-649BD1E9415F}";

        private const string Disarm2buff = "Disarm.Disarm2buff";
        private static readonly string Disarm2buffGuid = "{49890773-D237-46D0-8C68-B666B0503523}";

        private static readonly string DisarmDisplayName = "Disarm.Name";
        private static readonly string DisarmDescription = "Disarm.Description";

        private static readonly string Disarm2DisplayName = "Disarm2.Name";
        private static readonly string Disarm2Description = "Disarm2.Description";
        public static void DisarmConfigure()
        {
            var icon = FeatureRefs.ImprovedDisarm.Reference.Get().Icon;

            var BuffDisarm = BuffConfigurator.New(Disarmbuff, DisarmbuffGuid)
              .SetDisplayName(DisarmDisplayName)
              .SetDescription(DisarmDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(DisarmAbility, DisarmAbilityGuid)
                .SetDisplayName(DisarmDisplayName)
                .SetDescription(DisarmDescription)
                .SetIcon(icon)
                .SetBuff(BuffDisarm)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffDisarm2 = BuffConfigurator.New(Disarm2buff, Disarm2buffGuid)
              .SetDisplayName(DisarmDisplayName)
              .SetDescription(DisarmDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick2 = ActivatableAbilityConfigurator.New(Disarm2Ability, Disarm2AbilityGuid)
                .SetDisplayName(Disarm2DisplayName)
                .SetDescription(Disarm2Description)
                .SetIcon(icon)
                .SetBuff(BuffDisarm2)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.ImprovedDisarm)
                    .AddFacts(new() { abilityTrick, abilityTrick2 })
                    .Configure();
        }

        private const string SunderAbility = "Sunder.SunderAbility";
        private static readonly string SunderAbilityGuid = "{F874313F-B384-4BA9-AB7F-F8C5D081E769}";

        private const string Sunderbuff = "Sunder.Sunderbuff";
        private static readonly string SunderbuffGuid = "{B5FD1419-CED2-4450-9110-C2620CD88AC7}";

        private const string Sunder2Ability = "Sunder.Sunder2Ability";
        private static readonly string Sunder2AbilityGuid = "{3F18762C-3A4D-4250-B7D0-4C023B182606}";

        private const string Sunder2buff = "Sunder.Sunder2buff";
        private static readonly string Sunder2buffGuid = "{57476F97-D68A-4707-9B12-B2D7C5687F39}";

        private static readonly string SunderDisplayName = "Sunder.Name";
        private static readonly string SunderDescription = "Sunder.Description";

        private static readonly string Sunder2DisplayName = "Sunder2.Name";
        private static readonly string Sunder2Description = "Sunder2.Description";
        public static void SunderConfigure()
        {
            var icon = FeatureRefs.ImprovedSunder.Reference.Get().Icon;

            var BuffSunder = BuffConfigurator.New(Sunderbuff, SunderbuffGuid)
              .SetDisplayName(SunderDisplayName)
              .SetDescription(SunderDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(SunderAbility, SunderAbilityGuid)
                .SetDisplayName(SunderDisplayName)
                .SetDescription(SunderDescription)
                .SetIcon(icon)
                .SetBuff(BuffSunder)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffSunder2 = BuffConfigurator.New(Sunder2buff, Sunder2buffGuid)
              .SetDisplayName(SunderDisplayName)
              .SetDescription(SunderDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick2 = ActivatableAbilityConfigurator.New(Sunder2Ability, Sunder2AbilityGuid)
                .SetDisplayName(Sunder2DisplayName)
                .SetDescription(Sunder2Description)
                .SetIcon(icon)
                .SetBuff(BuffSunder2)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.ImprovedSunder)
                    .AddFacts(new() { abilityTrick, abilityTrick2 })
                    .Configure();
        }

        private const string TripAbility = "Trip.TripAbility";
        private static readonly string TripAbilityGuid = "{3C2295BD-A5B6-4B17-A06A-AD86D237936A}";

        private const string Tripbuff = "Trip.Tripbuff";
        private static readonly string TripbuffGuid = "{8C577D9F-BA5B-4974-B91D-F94FF28A8501}";

        private const string Trip2Ability = "Trip.Trip2Ability";
        private static readonly string Trip2AbilityGuid = "{C86ED543-EA4C-45F3-BD7F-01F906EEEB28}";

        private const string Trip2buff = "Trip.Trip2buff";
        private static readonly string Trip2buffGuid = "{43ADBC25-7972-45D6-A3AB-356F16199D50}";

        private static readonly string TripDisplayName = "Trip.Name";
        private static readonly string TripDescription = "Trip.Description";

        private static readonly string Trip2DisplayName = "Trip2.Name";
        private static readonly string Trip2Description = "Trip2.Description";
        public static void TripConfigure()
        {
            var icon = FeatureRefs.ImprovedTrip.Reference.Get().Icon;

            var BuffTrip = BuffConfigurator.New(Tripbuff, TripbuffGuid)
              .SetDisplayName(TripDisplayName)
              .SetDescription(TripDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(TripAbility, TripAbilityGuid)
                .SetDisplayName(TripDisplayName)
                .SetDescription(TripDescription)
                .SetIcon(icon)
                .SetBuff(BuffTrip)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            var BuffTrip2 = BuffConfigurator.New(Trip2buff, Trip2buffGuid)
              .SetDisplayName(TripDisplayName)
              .SetDescription(TripDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick2 = ActivatableAbilityConfigurator.New(Trip2Ability, Trip2AbilityGuid)
                .SetDisplayName(Trip2DisplayName)
                .SetDescription(Trip2Description)
                .SetIcon(icon)
                .SetBuff(BuffTrip2)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.ImprovedTrip)
                    .AddFacts(new() { abilityTrick, abilityTrick2 })
                    .Configure();
        }

        private const string BullRushAbility = "BullRush.BullRushAbility";
        private static readonly string BullRushAbilityGuid = "{5DD99370-2845-4B1A-A4EE-7DD3C10B0E3D}";

        private const string BullRushbuff = "BullRush.BullRushbuff";
        public static readonly string BullRushbuffGuid = "{AFBE010F-82C8-4ABD-84D6-78F584928399}";

        private const string BullRush2Ability = "BullRush.BullRush2Ability";
        private static readonly string BullRush2AbilityGuid = "{5FEFC722-45AB-4CA3-A860-462669EC8CEA}";

        private const string BullRush2buff = "BullRush.BullRush2buff";
        public static readonly string BullRush2buffGuid = "{E78853A3-7B2C-40B6-831F-824B1423F7F6}";

        private static readonly string BullRushDisplayName = "BullRush.Name";
        private static readonly string BullRushDescription = "BullRush.Description";

        private static readonly string BullRush2DisplayName = "BullRush2.Name";
        private static readonly string BullRush2Description = "BullRush2.Description";
        public static void BullRushConfigure()
        {
            var icon = FeatureRefs.ImprovedBullRush.Reference.Get().Icon;

            var BuffBullRush = BuffConfigurator.New(BullRushbuff, BullRushbuffGuid)
              .SetDisplayName(BullRushDisplayName)
              .SetDescription(BullRushDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            ActivatableAbilityConfigurator.New(BullRushAbility, BullRushAbilityGuid)
                .Configure();

            var action = ActionsBuilder.New().ApplyBuff(BuffBullRush, ContextDuration.Fixed(1)).Build();
            var action2 = ActionsBuilder.New().RemoveBuff(BuffBullRush).Build();

            var BuffBullRush2 = BuffConfigurator.New(BullRush2buff, BullRush2buffGuid)
              .SetDisplayName(BullRush2DisplayName)
              .SetDescription(BullRush2Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffActions(newRound: action, activated: action, deactivated: action2)
              .Configure();

            var abilityTrick2 = ActivatableAbilityConfigurator.New(BullRush2Ability, BullRush2AbilityGuid)
                .SetDisplayName(BullRush2DisplayName)
                .SetDescription(BullRush2Description)
                .SetIcon(icon)
                .SetBuff(BuffBullRush2)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.ImprovedBullRush)
                    .AddFacts(new() { abilityTrick2 })
                    .Configure();
        }

        private const string BullRushQuickAbility = "BullRushQuick.BullRushQuickAbility";
        public static readonly string BullRushQuickAbilityGuid = "{36B7B5DB-43B6-4ECA-B939-61B612D4F479}";

        private const string BullRushQuickbuff = "BullRushQuick.BullRushQuickbuff";
        public static readonly string BullRushQuickbuffGuid = "{D059FCDE-0119-4C94-83B5-F41076C95187}";

        private const string BullRushQuick2Ability = "BullRushQuick.BullRushQuick2Ability";
        public static readonly string BullRushQuick2AbilityGuid = "{3B615307-DBBE-445E-BA8B-13F5388207F3}";

        private const string BullRushQuick2buff = "BullRushQuick.BullRushQuick2buff";
        public static readonly string BullRushQuick2buffGuid = "{6082D84D-808A-4596-AE98-983A54CEC05F}";

        private static readonly string BullRushQuickDisplayName = "BullRushQuick.Name";
        private static readonly string BullRushQuickDescription = "BullRushQuick.Description";

        private static readonly string BullRushQuick2DisplayName = "BullRushQuick2.Name";
        private static readonly string BullRushQuick2Description = "BullRushQuick2.Description";
        public static void BullRushQuickConfigure()
        {
            var icon = FeatureRefs.PummelingCharge.Reference.Get().Icon;

            var BuffBullRushQuick = BuffConfigurator.New(BullRushQuickbuff, BullRushQuickbuffGuid)
              .SetDisplayName(BullRushQuickDisplayName)
              .SetDescription(BullRushQuickDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            ActivatableAbilityConfigurator.New(BullRushQuickAbility, BullRushQuickAbilityGuid)
                .Configure();

            var action = ActionsBuilder.New().ApplyBuff(BuffBullRushQuick, ContextDuration.Fixed(1)).Build();
            var action2 = ActionsBuilder.New().RemoveBuff(BuffBullRushQuick).Build();

            var BuffBullRushQuick2 = BuffConfigurator.New(BullRushQuick2buff, BullRushQuick2buffGuid)
              .SetDisplayName(BullRushQuick2DisplayName)
              .SetDescription(BullRushQuick2Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffActions(newRound: action, activated: action, deactivated: action2)
              .Configure();

            ActivatableAbilityConfigurator.New(BullRushQuick2Ability, BullRushQuick2AbilityGuid)
                .SetDisplayName(BullRushQuick2DisplayName)
                .SetDescription(BullRushQuick2Description)
                .SetIcon(icon)
                .SetBuff(BuffBullRushQuick2)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(false)
                .Configure();
        }

        private const string BullRushAngrybuff = "BullRushAngry.BullRushAngrybuff";
        public static readonly string BullRushAngrybuffGuid = "{0C9B7561-E15E-4F86-9C60-0D347ED6FD50}";

        private const string BullRushAngry2buff = "BullRushAngry.BullRushAngry2buff";
        public static readonly string BullRushAngry2buffGuid = "{04F5EBF5-9ACB-4750-8C90-E37E365EFBB8}";

        public static void BullRushAngryConfigure()
        {
            var icon = FeatureRefs.ImprovedBullRush.Reference.Get().Icon;

            var BuffBullRushAngry = BuffConfigurator.New(BullRushAngrybuff, BullRushAngrybuffGuid)
              .SetDisplayName(BullRushQuickDisplayName)
              .SetDescription(BullRushQuickDescription)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var action = ActionsBuilder.New().ApplyBuff(BuffBullRushAngry, ContextDuration.Fixed(1)).Build();
            var action2 = ActionsBuilder.New().RemoveBuff(BuffBullRushAngry).Build();

            BuffConfigurator.New(BullRushAngry2buff, BullRushAngry2buffGuid)
              .SetDisplayName(BullRushQuick2DisplayName)
              .SetDescription(BullRushQuick2Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffActions(newRound: action, activated: action, deactivated: action2)
              .AddComponent<KrakenDamage>(c => { c.type = Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush; c.stat = Kingmaker.EntitySystem.Stats.StatType.Strength; })
              .Configure();
        }

    }
}
