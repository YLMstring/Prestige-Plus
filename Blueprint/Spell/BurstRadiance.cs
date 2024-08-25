using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Conditions.Builder;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.CustomAction;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.RuleSystem;

namespace PrestigePlus.Blueprint.Spell
{
    internal class BurstRadiance
    {
        private const string BurstRadianceAbility = "NewSpell.UseBurstRadiance";
        public static readonly string BurstRadianceAbilityGuid = "{B24637CF-E246-4526-B43B-B463B441BCB2}";

        internal const string DisplayName = "NewSpellBurstRadiance.Name";
        private const string Description = "NewSpellBurstRadiance.Description";

        internal const string Duration = "PPNewDuration1d4round.Name";
        public static void Configure()
        {
            var icon = AbilityRefs.Stormbolts.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                BurstRadianceAbility, BurstRadianceAbilityGuid, SpellSchool.Evocation, canSpecialize: true)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(Duration)
              .AllowTargeting(true, true, true, true)
              .AddComponent(AbilityRefs.Flare.Reference.Get().GetComponent<AbilitySpawnFx>())
              .SetRange(AbilityRange.Long)
              .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Any, radius: 10.Feet(), spreadSpeed: 15.Feet())
              .SetType(AbilityType.Spell)
              .SetSpellResistance()
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Extend, Metamagic.Persistent, Metamagic.Selective, Metamagic.Empower, Metamagic.Maximize, Metamagic.Bolstered)
              .AddToSpellLists(level: 2, SpellList.Wizard)
              .AddToSpellLists(level: 2, SpellList.Cleric)
              .AddToSpellLists(level: 2, SpellList.Warpriest)
              .AddToSpellLists(level: 2, SpellList.Druid)
              .AddToSpellLists(level: 2, SpellList.Hunter)
              .SetSpellDescriptor(SpellDescriptor.Good)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.BlindnessBuff.ToString(), ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D4))
                        .Build(), succeed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.DazzledBuff.ToString(), ContextDuration.FixedDice(Kingmaker.RuleSystem.DiceType.D4))
                        .Build())
                  .Conditional(ConditionsBuilder.New().Alignment(Kingmaker.Enums.AlignmentComponent.Evil).Build(),
                        ifTrue: ActionsBuilder.New()
                        .DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Magic), ContextDice.Value(DiceType.D4, ContextValues.Rank(), 0))
                        .Build())
                  .Build(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Reflex)
              .AddContextRankConfig(ContextRankConfigs.CasterLevel(max: 5))
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.Reflex,
                spellType: CraftSpellType.Debuff)
              .Configure();
        }
    }
}
