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
    [HarmonyPatch(typeof(RuleAttackRoll), nameof(RuleAttackRoll.OnTrigger))]
    internal class SecondaryAttackFix
    {
        static void Prefix(RuleAttackRoll __instance)
        {
            if (__instance.RuleAttackWithWeapon?.IsFullAttack == false)
            {
                __instance.RuleAttackWithWeapon.WeaponStats.IsSecondaryOverride = false;
                __instance.RuleAttackWithWeapon.WeaponStats.IsSecondary = false;
            }
        }
    }
}
