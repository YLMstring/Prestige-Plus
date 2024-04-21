using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class DebilitatingPain
    {
        private const string DebilitatingPainAbility = "NewSpell.UseDebilitatingPain";
        public static readonly string DebilitatingPainAbilityGuid = "{EDD43108-E61A-4386-9462-4B6F6E6F4BCE}";

        internal const string DisplayName = "NewSpellDebilitatingPain.Name";
        private const string Description = "NewSpellDebilitatingPain.Description";

        internal const string Duration = "PPNewDuration1/3round.Name";
        public static void Configure()
        {
            var icon = AbilityRefs.CacophonousCall.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                DebilitatingPainAbility, DebilitatingPainAbilityGuid, SpellSchool.Enchantment, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(Duration)
              .AllowTargeting(false, true, false, false)
              .SetAnimation(CastAnimationStyle.Directional)
              .AddComponent(AbilityRefs.CacophonousCall.Reference.Get().GetComponent<AbilitySpawnFx>())
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Reach, Metamagic.Extend, Metamagic.Persistent)
              .SetSpellResistance()
              .AddToSpellLists(level: 3, SpellList.Witch)
              .SetSpellDescriptor(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion | SpellDescriptor.NegativeEmotion)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ConditionalSaved(failed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.Stunned.ToString(), ContextDuration.Variable(ContextValues.Rank()))
                        .Build(), succeed: ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.Daze.ToString(), ContextDuration.Fixed(1))
                        .Build())
                  .Build(), savingThrowType: Kingmaker.EntitySystem.Stats.SavingThrowType.Will)
              .AddContextRankConfig(ContextRankConfigs.CasterLevel().WithDivStepProgression(3))
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.Will,
                spellType: CraftSpellType.Debuff)
              .Configure();
        }
    }
}
