using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using static ChromaSDK.ChromaAnimationAPI;
using static Kingmaker.GameModes.GameModeType;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RuleCalculateDamage), nameof(RuleCalculateDamage.ApplyPostCritBonusModifications))]
    internal class WeakenCritical
    {
        static void Postfix(ref RuleCalculateDamage __instance, ref BaseDamage damage, ref float rollAndBonus)
        {
            try
            {
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("weakencrit"))) { return; }
                int num = damage.CriticalModifier ?? 1;
                if (__instance.CritImmunity && !__instance.IgnoreCritImmunity)
                {
                    num = 1;
                }
                SettingsEntityEnum<CriticalHitPower> enemyCriticalHits = SettingsRoot.Difficulty.EnemyCriticalHits;
                int num3 = (!__instance.Target.IsPlayerFaction || enemyCriticalHits == CriticalHitPower.Normal || damage.Bonus + damage.BonusTargetRelated < 0) ? num : 1;
                if (num3 <= 1 || __instance.Target.IsPlayerFaction) { return; }
                var num4 = (num3 - 1) * damage.EmpowerBonus;
                rollAndBonus -= (damage.Bonus + damage.BonusTargetRelated) * num4;
            }
            catch (Exception ex) { Logger.Error("Failed to WeakenCritical.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
    }
}
