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

namespace PrestigePlus.Blueprint.Spell
{
    internal class DebilitatingPainMass
    {
        private const string DebilitatingPainMassAbility = "NewSpell.UseDebilitatingPainMass";
        public static readonly string DebilitatingPainMassAbilityGuid = "{776EEFCB-0BA8-41BF-AE8C-EBA58A19FF29}";

        internal const string DisplayName = "NewSpellDebilitatingPainMass.Name";
        private const string Description = "NewSpellDebilitatingPainMass.Description";

        internal const string Duration = "PPNewDuration1/3round.Name";
        public static void Configure()
        {
            var icon = AbilityRefs.WitchHexAgonyAbility.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                DebilitatingPainMassAbility, DebilitatingPainMassAbilityGuid, SpellSchool.Enchantment, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(Duration)
              .SetAnimation(CastAnimationStyle.Omni)
              .SetRange(AbilityRange.Personal)
              .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Enemy, radius: 20.Feet(), spreadSpeed: 20.Feet())
              .SetType(AbilityType.Spell)
              .AddSpellResistance()
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Extend, Metamagic.Persistent)
              .AddToSpellLists(level: 5, SpellList.Witch)
              .SetSpellDescriptor(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.NegativeEmotion)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .Add<HighestHDenemy>(c => { c.Action = ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.Stunned.ToString(), ContextDuration.Variable(ContextValues.Rank()))
                        .Build(), succeed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.Daze.ToString(), ContextDuration.Fixed(1))
                        .Build())
                  .Build(); })
                  .Build(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Will)
              .AddContextRankConfig(ContextRankConfigs.CasterLevel().WithDivStepProgression(3))
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.Will,
                spellType: CraftSpellType.Debuff)
              .Configure();
        }
    }
}
