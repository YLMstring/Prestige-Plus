using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Commands;
using PrestigePlus.Blueprint.Archetype;
using Kingmaker.UnitLogic;
using Kingmaker;
using static Kingmaker.UI.CanvasScalerWorkaround;
using PrestigePlus.Blueprint;
using Kingmaker.Utility;
using Kingmaker.EntitySystem.Entities;
using UniRx;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitAttack), nameof(UnitAttack.InitAttacks))]
    internal class ForceFullAttackFix
    {
        static void Prefix(ref UnitAttack __instance)
        {
            try
            {
                var turn = Game.Instance.TurnBasedCombatController?.CurrentTurn;
                if (__instance.Executor.HasFact(RapidBuff) && turn?.Rider == __instance.Executor)
                {
                    __instance.ForceFullAttack = true;
                }
                if (turn?.Rider == __instance.Executor && !__instance.Executor.IsDirectlyControllable && !__instance.Executor.IsMoveActionRestricted() && !__instance.Executor.IsStandardActionRestricted())
                {
                    var distance = turn.GetTimeMoved(__instance.Executor) * __instance.Executor.CurrentSpeedMps;
                    if (distance <= 6.Feet().Meters)
                    {
                        __instance.ForceFullAttack = true;
                        Main.Logger.Info(distance.ToString());
                    }
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to ForceFullAttackFix.", ex); }
        }

        private static BlueprintBuffReference RapidBuff = BlueprintTool.GetRef<BlueprintBuffReference>(DawnflowerDervish.RapidAttackBuffGuid);
    }
}
