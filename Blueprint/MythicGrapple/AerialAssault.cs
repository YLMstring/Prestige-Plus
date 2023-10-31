using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.TurnBasedModifiers;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomComponent.Charge;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.MythicGrapple
{
    internal class AerialAssault
    {
        private static readonly string FeatName = "AerialAssault";
        public static readonly string FeatGuid = "{96E7DFF8-63D5-4846-920B-0D5C7E7DB823}";

        private static readonly string DisplayName = "AerialAssault.Name";
        private static readonly string Description = "AerialAssault.Description";

        private const string ReleaseAbility = "AerialAssault.ReleaseAbility";
        public static readonly string ReleaseAbilityGuid = "{F8C24B38-13B5-4708-BFA3-E3DC34C84461}";

        private const string StyleActivatableAbility = "AerialAssault.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{1FF9E62B-E39C-4F32-8ED2-160633EB892C}";

        private const string Stylebuff2 = "AerialAssault.Stylebuff2";
        public static readonly string Stylebuff2Guid = "{A8710AE9-2052-4494-89E3-08E30A8CC95D}";

        private const string StyleActivatableAbility3 = "AerialAssault.StyleActivatableAbility3";
        private static readonly string StyleActivatableAbility3Guid = "{3B783C46-7BE2-4587-920A-0BBCA4FAEBF9}";

        private const string Stylebuff3 = "AerialAssault.Stylebuff3";
        public static readonly string Stylebuff3Guid = "{302B7432-7F03-4D5E-89C6-BB01227ABFD4}";

        private static readonly string DisplayName2 = "BasicImprovedCharge.Name";
        private static readonly string Description2 = "BasicImprovedCharge.Description";

        private static readonly string DisplayName3 = "BasicImprovedCharge3.Name";
        private static readonly string Description3 = "BasicImprovedCharge3.Description";

        private static readonly string DisplayName4 = "BasicImprovedCharge4.Name";
        private static readonly string Description4 = "BasicImprovedCharge4.Description";

        public static void ConfigureAbility()
        {
            var icon = FeatureRefs.ArmyChargeAbilityFeature.Reference.Get().Icon;
            AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .CopyFrom(
                AbilityRefs.ChargeAbility,
                typeof(AbilityRequirementHasCondition),
                typeof(AbilityCasterHasNoFacts),
                typeof(AbilityIsFullRoundInTurnBased),
                typeof(AbilityRequirementCanMove))
                .AddComponent<TryAboveAttack>()
                .SetDisplayName(DisplayName2)
                .SetDescription(Description2)
                .SetIcon(icon)
                .AddAbilityCasterHasNoFacts(new() { BuffRefs.MountedBuff.Reference.Get() })
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.DoubleMove)
                .SetType(AbilityType.Physical)
                .Configure();
        }
        public static void Configure()
        {
            var icon = FeatureRefs.ArmyChargeAbilityFeature.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff2, Stylebuff2Guid)
              .SetDisplayName(DisplayName4)
              .SetDescription(Description4)
              .SetIcon(icon)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(DisplayName4)
                .SetDescription(Description4)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            var ability2 = ActivatableAbilityConfigurator.New(StyleActivatableAbility3, StyleActivatableAbility3Guid)
                .SetDisplayName(DisplayName3)
                .SetDescription(Description3)
                .SetIcon(icon)
                .SetBuff(Stylebuff3Guid)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            BuffConfigurator.New(Stylebuff3, Stylebuff3Guid)
              .SetDisplayName(DisplayName3)
              .SetDescription(Description3)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddBuffActions(newRound: ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1)).Build())
              .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();
        }
    }
}
