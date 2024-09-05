using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.AI;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
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
                if (!__instance.Owner.Resources.HasEnoughResource(__instance.RequiredResource, amount) && __instance.OwnerBlueprint is BlueprintActivatableAbility ability)
                {
                    __instance.Fact.TurnOffImmediately();
                    Main.Logger.Info("stop activate ability");
                    int time = 6;
                    if (__instance.SpendType == ResourceSpendType.OncePerTenMinutes)
                    {
                        time = 600;
                    }
                    else if (__instance.SpendType == ResourceSpendType.OncePerHour)
                    {
                        time = 3600;
                    }
                    else if (__instance.SpendType == ResourceSpendType.OncePerMinute)
                    {
                        time = 60;
                    }
                    __instance.Owner.AddBuff(ability.Buff, __instance.Owner, TimeSpan.FromSeconds(time));
                }
                return false;
            }
            __instance.Fact.TurnOffImmediately();
            return false;
        }
    }
}
