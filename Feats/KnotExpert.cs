using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class KnotExpert
    {
        private static readonly string FeatName = "KnotExpert";
        private static readonly string FeatGuid = "{B7D6C2B2-F365-4E59-9CBC-4F750015F250}";

        private static readonly string DisplayName = "KnotExpert.Name";
        private static readonly string Description = "KnotExpert.Description";

        private const string ReleaseAbility = "KnotExpert.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{D2433C2A-3BBE-4112-87AC-39FD749AC983}";
        public static void Configure()
        {
            var icon = FeatureRefs.EntanglingInfusionFeature.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.Kinetic_EarthBlastLine00.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: FeetExtension.Feet(5), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<KnotGrapple>()
                    .Build())
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
