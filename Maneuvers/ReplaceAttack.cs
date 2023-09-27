using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
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

    }
}
