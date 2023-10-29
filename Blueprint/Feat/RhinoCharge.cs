using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomComponent.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class RhinoCharge
    {
        private static readonly string FeatName = "RhinoCharge";
        public static readonly string FeatGuid = "{1CFB6F9F-7ED4-4FFC-89D5-C40925F17756}";

        private static readonly string DisplayName = "RhinoCharge.Name";
        private static readonly string Description = "RhinoCharge.Description";

        private const string RhinoChargeAbility = "RhinoCharge.RhinoChargeAbility";
        private static readonly string RhinoChargeAbilityGuid = "{D3610185-7B4D-431E-8A04-2C5570FDAB9A}";

        private const string RhinoChargebuff = "RhinoCharge.RhinoChargebuff";
        public static readonly string RhinoChargebuffGuid = "{7CD6719A-05F9-44CD-9E53-C0C1E82AC922}";

        public static void Configure()
        {
            var icon = FeatureRefs.FinalShifterAspectFeature.Reference.Get().Icon;

            var BuffRhinoCharge = BuffConfigurator.New(RhinoChargebuff, RhinoChargebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .Configure();

            var abilityTrick = AbilityConfigurator.New(RhinoChargeAbility, RhinoChargeAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(BuffRhinoCharge, ContextDuration.Fixed(1))
                        .Build())
                .SetCanTargetSelf(false)
                .SetCanTargetFriends(false)
                .SetCanTargetEnemies(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Unlimited)
                .SetType(AbilityType.Physical)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedBullRush.ToString())
                    .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 5)
                    .AddFacts(new() { abilityTrick })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
