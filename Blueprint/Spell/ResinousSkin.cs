using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ResinousSkin
    {
        private const string ResinousSkinAbility = "NewSpell.UseResinousSkin";
        public static readonly string ResinousSkinAbilityGuid = "{4B8F3792-A86E-45F4-B40A-2C01805F1E5E}";

        private const string ResinousSkinBuff = "NewSpell.ResinousSkinBuff";
        private static readonly string ResinousSkinBuffGuid = "{3FF99136-6A0C-4CC8-9A23-A9218B4202A9}";

        internal const string DisplayName = "NewSpellResinousSkin.Name";
        private const string Description = "NewSpellResinousSkin.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BlessingOfTheSalamander.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                    .SavingThrow(type: SavingThrowType.Reflex, useDCFromContextSavingThrow: true,
                    onResult: ActionsBuilder.New()
                        .ConditionalSaved(failed: ActionsBuilder.New()
                            .CombatManeuver(ActionsBuilder.New().Build(), CombatManeuver.Disarm)
                            .Build())
                        .Build())
                    .Build();

            var buff = BuffConfigurator.New(ResinousSkinBuff, ResinousSkinBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddDamageResistancePhysical(form: PhysicalDamageForm.Piercing, bypassedByForm: true, value: 5, bypassedByMagic: true, minEnhancementBonus: 1, or: true)
              .AddCMDBonusAgainstManeuvers(ModifierDescriptor.Circumstance, new CombatManeuver[] { CombatManeuver.Disarm }, 4)
              .AddCMBBonusForManeuver(null, null, ModifierDescriptor.Circumstance, new CombatManeuver[] { CombatManeuver.Grapple }, 2)
              .AddCMDBonusAgainstManeuvers(maneuvers: new[] { CombatManeuver.Grapple }, value: 2)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action, onlyHit: true, onlyMelee: true, waitForAttackResolve: true)
              .Configure();

            AbilityConfigurator.NewSpell(
                ResinousSkinAbility, ResinousSkinAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(AbilityRefs.FreedomOfMovement.Reference.Get().LocalizedDuration)
              .SetLocalizedSavingThrow(AbilityRefs.IcyPrison.Reference.Get().LocalizedSavingThrow)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .AddToSpellLists(level: 3, SpellList.Alchemist)
              .AddToSpellLists(level: 3, SpellList.Bloodrager)
              .AddToSpellLists(level: 3, SpellList.Druid)
              .AddToSpellLists(level: 3, SpellList.Hunter)
              .AddToSpellLists(level: 3, SpellList.Wizard)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.TenMinutes), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.Reflex,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
