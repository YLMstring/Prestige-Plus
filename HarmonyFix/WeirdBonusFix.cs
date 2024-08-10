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
        static bool Prefix()
        {
            RulebookEvent previousEvent = Rulebook.CurrentContext.PreviousEvent;
            if (previousEvent == null)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(AiBrainController), nameof(AiBrainController.CalculateTargetsScore))]
    internal class WeirdBonusFix2
    {
        static void Postfix(ref bool __result, ref AiActionCustom __instance, ref UnitEntityData bestTarget, ref AiAction bestAction)
        {
            if (__result && bestAction is AiActionCustom cus && bestTarget != cus.m_Ability?.Caster)
            {
                var ability = cus.m_Ability?.Blueprint;
                if (ability == null) return;
                if (!ability.CanTargetEnemies && !ability.CanTargetFriends && !ability.CanTargetPoint && ability.CanTargetSelf)
                {
                    bestTarget = cus.m_Ability?.Caster;
                }
            }
        }
    }

    [HarmonyPatch(typeof(UnitHelper), nameof(UnitHelper.IsAttackOfOpportunityReach))]
    internal class WeirdBonusFix3
    {
        static bool Prefix(ref UnitEntityData enemy, ref UnitEntityData unit, ref bool __result)
        {
            if (unit?.View == null || enemy?.View == null)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
