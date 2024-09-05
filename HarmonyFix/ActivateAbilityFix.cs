using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.AI;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ActivatableAbilityResourceLogic), nameof(ActivatableAbilityResourceLogic.SpendResource))]
    internal class ActivateAbilityFix
    {
        static bool Prefix(ref ActivatableAbilityResourceLogic __instance)
        {
            if (__instance.SpendType != ResourceSpendType.NewRound && __instance.SpendType != ResourceSpendType.OncePerTenMinutes && __instance.SpendType != ResourceSpendType.OncePerMinute && __instance.SpendType != ResourceSpendType.OncePerHour)
            {
                return true;
            }
            if (__instance.IsFreeBecauseOfFact)
            {
                return false;
            }
            if (!__instance.RequiredResource)
            {
                if (__instance.Fact.SourceItem != null && (!__instance.Fact.SourceItem.SpendCharges() || __instance.Fact.SourceItem.Charges == 0))
                {
                    __instance.Fact.TurnOffImmediately();
                }
                return false;
            }
            int amount = __instance.CalcResourceCost();
            if (__instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, amount))
            {
                __instance.Owner.Resources.Spend(__instance.RequiredResource, amount);
                Main.Logger.Info("stop activate ability0");
                if (!__instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, amount))
                {
                    __instance.Fact.Stop();
                    __instance.Owner.Resources.Restore(__instance.RequiredResource, amount);
                }
                return false;
            }
            __instance.Fact.TurnOffImmediately();
            return false;
        }
    }
}
