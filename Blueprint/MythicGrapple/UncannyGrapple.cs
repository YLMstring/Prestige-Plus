using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.MythicGrapple
{
    internal class UncannyGrapple
    {
        private static readonly string FeatName = "UncannyGrapple";
        public static readonly string FeatGuid = "{046A0C22-F8A9-45B7-B370-4F4B18DB20B2}";

        private static readonly string DisplayName = "UncannyGrapple.Name";
        private static readonly string Description = "UncannyGrapple.Description";

        private const string UncannyGrappleAbility = "UncannyGrapple.UncannyGrappleAbility";
        private static readonly string UncannyGrappleAbilityGuid = "{BC2D9270-2C8E-4AC0-AB71-7F22BF2E3882}";

        private const string UncannyGrapplebuff = "UncannyGrapple.UncannyGrapplebuff";
        private static readonly string UncannyGrapplebuffGuid = "{E4A64303-1E48-4339-AF81-B4D1BD00DB74}";
        public static void Configure()
        {
            var icon = FeatureRefs.TripMythicFeat.Reference.Get().Icon;

            var BuffUncannyGrapple = BuffConfigurator.New(UncannyGrapplebuff, UncannyGrapplebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(UncannyGrappleAbility, UncannyGrappleAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(BuffUncannyGrapple)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(SavageSlam.FeatGuid)
                    .AddFacts(new() { abilityTrick })
                    .Configure();
        }
    }
}
