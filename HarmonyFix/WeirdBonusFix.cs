using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UI.MVVM._VM.Slots;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Utility;
using Kingmaker.AI.Blueprints;
using Kingmaker.AI;
using Kingmaker.View;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.RuleSystem.Rules.Abilities;
using BlueprintCore.Utils;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using JetBrains.Annotations;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.TurnBasedMode;
using static Kingmaker.Controllers.Combat.UnitCombatState;
using Kingmaker.TurnBasedMode.Controllers;
using Kingmaker;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch]
    internal static class WeirdBonusFix
    {
        public static IEnumerable<(A, B)> Zip<A, B>(this IEnumerable<A> first, IEnumerable<B> second) => first.Zip(second, (a, b) => (a, b));

        [HarmonyTargetMethod]
        static MethodInfo TargetMethod()
        {
            var interfaceMap = typeof(ModifyD20).GetInterfaceMap(typeof(IRulebookHandler<RuleCalculateAttackBonus>));

            if (interfaceMap.InterfaceMethods
                .Zip(interfaceMap.TargetMethods)
                .TryFind((pair) =>
                    pair.Item1.Name == nameof(IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger),
                    out var pair))
            {
                return pair.Item2;
            }

            throw new Exception("Missing interface method");
        }

        [HarmonyPrefix]
        static bool Prefix(ref ModifyD20 __instance)
        {
            if (__instance.Rule != RuleType.AttackRoll) { return false; }
            RulebookEvent previousEvent = Rulebook.CurrentContext.PreviousEvent;
            if (previousEvent == null)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(SpellTurning), nameof(SpellTurning.OnEventDidTrigger))]
    internal class WeirdBonusFix3
    {
        static bool Prefix(ref RuleCastSpell evt)
        {
            if (evt?.Spell?.IsAOE == true)
            {
                return false;
            }
            if (evt?.Spell?.Range == AbilityRange.Touch)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(AiAction), nameof(AiAction.IsTargetValidInternal))]
    internal class WeirdBonusFix4
    {
        static void Postfix(ref DecisionContext context, ref bool __result)
        {
            try
            {
                if (!__result) { return; }
                if (context?.Ability == null)
                {
                    return;
                }
                if (context?.Target?.Unit == null)
                {
                    return;
                }
                if (context.Ability.CanTarget(context.Target.Unit) == false)
                {
                    __result = false;
                    return;
                }
                if (context.Ability.IsAOE)
                {
                    return;
                }
                if (context.Ability.Range == AbilityRange.Touch)
                {
                    return;
                }
                if (context.Ability.SpellResistance != true)
                {
                    return;
                }
                if (context.Target.Unit.HasFact(FeatureRefs.BeltOfArodenSpellTurningFeature.Reference) == true)
                {
                    __result = false;
                    return;
                }
                if (context.Target.Unit.HasFact(Raz) == true)
                {
                    __result = false;
                    return;
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to WeirdBonusFix4", ex); }
        }

        private static BlueprintFeatureReference Raz = BlueprintTool.GetRef<BlueprintFeatureReference>("13d5818737694021b001641437a4ba29");
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.CanTarget))]
    internal class WeirdBonusFix8
    {
        static void Postfix(ref TargetWrapper target, ref AbilityData __instance, ref bool __result)
        {
            try
            {
                if (!__result) { return; }
                if (__instance.Caster.Unit == target?.Unit) { return; }
                var resist = target?.Unit?.Get<UnitPartSpellResistance>();
                if (resist != null)
                {
                    AbilityExecutionContext abilityExecutionContext = new(__instance, __instance.CalculateParams(), target.Unit, null, __instance.Caster.Unit);
                    if (resist.IsImmune(abilityExecutionContext) == true)
                    {
                        __result = false;
                    }
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to WeirdBonusFix8", ex); }
        }
    }

    [HarmonyPatch(typeof(UnitCommand), nameof(UnitCommand.Interrupt))]
    internal class WeirdBonusFix9
    {
        static void Prefix(ref UnitCommand __instance)
        {
            try
            {
                if (__instance.ForcedPath == null && !__instance.IsStarted)
                {
                    Main.Logger.Info("Restore the action because it's not started");
                    var caster = __instance.Executor;
                    caster.CombatState.Cooldown.StandardAction = 0f;
                    var state = Game.Instance.TurnBasedCombatController?.CurrentTurn?.GetActionsStates(caster)?.ActionsStates;
                    if (state != null)
                    {
                        Game.Instance.TurnBasedCombatController.CurrentTurn.GetActionsStates(caster).ActionsStates.Standard =
                            new CombatAction(CombatAction.ActivityState.Lost, CombatAction.ActivityState.Available, CombatAction.ActivityState.Available, 0f);
                    }
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to WeirdBonusFix9", ex); }
        }
    }

    [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.FindPet))]
    internal class WeirdBonusFix5
    {
        static bool Prefix(ref BlueprintFeature feature, ref UnitEntityData __result)
        {
            if (feature == null)
            {
                __result = null;
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(RuleDealStatDamage), nameof(RuleDealStatDamage.OnTrigger))]
    internal class WeirdBonusFix6
    {
        static void Postfix(ref RuleDealStatDamage __instance)
        {
            if (__instance.Stat == null)
            {
                return;
            }
            int lost = __instance.Stat.Damage + __instance.Stat.Drain;
            int gain = __instance.Stat.BaseValue;
            foreach (var mod in __instance.Stat.Modifiers)
            {
                if (mod.ModValue > 0)
                {
                    gain += mod.ModValue;
                }
            }
            //Main.Logger.Info("Total " + gain.ToString() + " Lost " + lost.ToString());
            if (gain < lost)
            {
                __instance.Target.Descriptor.State.MarkedForDeath = true;
            }
        }
    }

    [HarmonyPatch(typeof(SavingThrowBonusAgainstDescriptor), nameof(SavingThrowBonusAgainstDescriptor.OnEventAboutToTrigger))]
    internal class WeirdBonusFix7
    {
        static void Prefix(ref SavingThrowBonusAgainstDescriptor __instance)
        {
            __instance.Bonus ??= 0;
        }
    }
}
