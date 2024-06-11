using HarmonyLib;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RuleAttackWithWeapon), nameof(RuleAttackWithWeapon.WeaponStats), MethodType.Getter)]
    internal class SecondaryAttackFix
    {
        static void Postfix(RuleAttackWithWeapon __instance, ref RuleCalculateWeaponStats __result)
        {
            if (__instance.IsFullAttack == false)
            {
                __result.IsSecondaryOverride = false;
                __result.IsSecondary = false;
            }
        }
    }
}
