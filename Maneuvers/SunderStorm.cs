using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using PrestigePlus.Modify;
using PrestigePlus.CustomAction;

namespace PrestigePlus.Maneuvers
{
    internal class SunderStorm
    {
        private const string SunderStormFeat = "Mythic.SunderStorm";
        private static readonly string SunderStormGuid = "{025D489D-605A-4BC8-915D-FDFB19D31D7B}";
        internal const string SunderStormDisplayName = "MythicSunderStorm.Name";
        private const string SunderStormDescription = "MythicSunderStorm.Description";

        private const string SunderStormAblity = "Mythic.UseSunderStorm";
        private static readonly string SunderStormAblityGuid = "{2EC425B2-2E4C-4FD3-8574-B6C2FF29D12D}";

        private const string SunderStormBuff = "Mythic.UseSunderStormBuff";
        public static readonly string SunderStormBuffGuid = "{1242A045-E81D-4CCB-BD81-9AC3BF99C119}";

        private const string SunderStormAblity2 = "Mythic.UseSunderStorm2";
        private static readonly string SunderStormAblityGuid2 = "{03F1AFBA-2890-4FB5-B5F1-BBDE8DDDB22E}";

        public static BlueprintFeature CreateSunderStorm()
        {
            var icon = FeatureRefs.CoordinatedManeuvers.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(SunderStormBuff, SunderStormBuffGuid)
              .SetDisplayName(SunderStormDisplayName)
              .SetDescription(SunderStormDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<TearApartSunder>()
              .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
              .SetRanks(20)
              .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
              .Configure();

            var action = ActionsBuilder.New()
                .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.MythicLevel, toCaster: true), diceCount: 1), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Divine))
                .Build();

            var shoot = ActionsBuilder.New()
                .Add<ContextActionSunderStorm>()
                .Build();

            var ability = AbilityConfigurator.New(SunderStormAblity, SunderStormAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Physical)
                .SetDisplayName(SunderStormDisplayName)
                .SetDescription(SunderStormDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityTargetsAround(includeDead: false, radius: 100.Feet(), targetType: TargetType.Enemy)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var cast = ActionsBuilder.New()
                .CastSpell(ability)
                .Build();

            var ability2 = AbilityConfigurator.New(SunderStormAblity2, SunderStormAblityGuid2)
                .AddAbilityEffectRunAction(ActionsBuilder.New().OnContextCaster(cast).Build())
                .SetType(AbilityType.Physical)
                .SetDisplayName(SunderStormDisplayName)
                .SetDescription(SunderStormDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Weapon)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(false)
                .SetCanTargetSelf(false)
                .AddAbilityCasterMainWeaponIsMelee()
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.BreathWeapon)
                .Configure();

            return FeatureConfigurator.New(SunderStormFeat, SunderStormGuid, FeatureGroup.MythicAbility)
              .SetDisplayName(SunderStormDisplayName)
              .SetDescription(SunderStormDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedSunder.ToString())
              .AddFacts(new() { ability2 })
              .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, true)
              .Configure();
        }
    }
}
