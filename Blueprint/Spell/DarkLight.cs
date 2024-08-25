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
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;

namespace PrestigePlus.Blueprint.Spell
{
    internal class DarkLight
    {
        private const string DarkLightAbility = "NewSpell.UseDarkLight";
        public static readonly string DarkLightAbilityGuid = "{64EF4466-C43D-4152-B2F1-532870BF6307}";

        internal const string DisplayName = "NewSpellDarkLight.Name";
        private const string Description = "NewSpellDarkLight.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.CircleOfDeath.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                DarkLightAbility, DarkLightAbilityGuid, SpellSchool.Evocation, canSpecialize: true)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(Duration.RoundPerLevel)
              .AddComponent(AbilityRefs.Harm.Reference.Get().GetComponent<AbilitySpawnFx>())
              .AllowTargeting(true, true, true, true)
              .SetRange(AbilityRange.Medium)
              .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Any, radius: 20.Feet(), spreadSpeed: 30.Feet())
              .SetType(AbilityType.Spell)
              .AddSpellResistance()
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Extend, Metamagic.Persistent, Metamagic.Selective)
              .AddToSpellLists(level: 3, SpellList.Wizard)
              .AddToSpellLists(level: 3, SpellList.Cleric)
              .AddToSpellLists(level: 3, SpellList.Warpriest)
              .AddToSpellLists(level: 3, SpellList.Druid)
              .AddToSpellLists(level: 3, SpellList.Hunter)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .Conditional(ConditionsBuilder.New().HasFact(FeatureRefs.UndeadType.ToString(), true).HasFact(FeatureRefs.ConstructType.ToString(), true).Build(),
                        ifTrue: ActionsBuilder.New()
                            .ConditionalSaved(failed: ActionsBuilder.New()
                            .ApplyBuff(BuffRefs.BlindnessBuff.ToString(), ContextDuration.Variable(ContextValues.Rank()))
                            .Build(), succeed: ActionsBuilder.New()
                            .ApplyBuff(BuffRefs.DazzledBuff.ToString(), ContextDuration.Variable(ContextValues.Rank()))
                            .Build())
                        .Build())
                  .Build(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude)
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.Fortitude,
                spellType: CraftSpellType.Debuff)
              .Configure();
        }
    }
}
