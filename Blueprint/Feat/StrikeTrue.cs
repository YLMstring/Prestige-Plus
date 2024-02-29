using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class StrikeTrue
    {
        private static readonly string FeatName = "StrikeTrue";
        public static readonly string FeatGuid = "{65EDEF08-C4FF-4B97-9BBE-31D1A22D0919}";

        private static readonly string DisplayName = "FeatStrikeTrue.Name";
        private static readonly string Description = "FeatStrikeTrue.Description";

        private const string StrikeTrueAbility = "StrikeTrue.StrikeTrueAbility";
        private static readonly string StrikeTrueAbilityGuid = "{789F43A9-3111-4AA1-9279-28A5566C1AB3}";

        private const string StrikeTruebuff = "StrikeTrue.StrikeTruebuff";
        public static readonly string StrikeTruebuffGuid = "{18EA44F2-93BC-41DC-A42F-09A990B91524}";

        public static void Configure()
        {
            var icon = AbilityRefs.TrueStrike.Reference.Get().Icon;

            var BuffStrikeTrue = BuffConfigurator.New(StrikeTruebuff, StrikeTruebuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddRemoveBuffOnAttack()
              .AddAttackTypeAttackBonus(attackBonus: 4, type: Kingmaker.Enums.WeaponRangeType.Melee, checkFact: false, allTypesExcept: false, value: 1)
              .Configure();

            var abilityTrick = AbilityConfigurator.New(StrikeTrueAbility, StrikeTrueAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AllowTargeting(false, false, false, true)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .AddComponent(AbilityRefs.TrueStrike.Reference.Get().GetComponent<AbilitySpawnFx>())
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(BuffStrikeTrue, ContextDuration.Fixed(1))
                        .Build())
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(Kingmaker.EntitySystem.Stats.StatType.BaseAttackBonus, 6)
                    .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                    .AddFacts(new() { abilityTrick })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
