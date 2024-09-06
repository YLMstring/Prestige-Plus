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
using Kingmaker.UnitLogic.Commands.Base;
using Microsoft.Build.Utilities;
using Kingmaker.Pathfinding;
using Kingmaker.Controllers.Combat;

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
                if (!__instance.Executor.IsDirectlyControllable && !__instance.Executor.UsedOneMoveAction() && __instance is UnitCommand cmd && cmd.ForcedPath != null && turn?.Rider == __instance.Executor)
                {
                    var length = cmd.ForcedPath.GetTotalLength();
                    if (length > 0 && length <= 8f.Feet().Meters)
                    {
                        //Main.Logger.Info(length.ToString()); 
                        __instance.ForceFullAttack = true;
                    } 
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to ForceFullAttackFix.", ex); }
        }

        private static BlueprintBuffReference RapidBuff = BlueprintTool.GetRef<BlueprintBuffReference>(DawnflowerDervish.RapidAttackBuffGuid);
    }

    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.ShouldAttackOnDisengage))]
    internal class ForceFullAttackFix2
    {
        static void Postfix(ref UnitEntityData target, ref bool __result)
        {
            try
            {
                if (!__result) { return; }
                if (target.IsDirectlyControllable) { return; }
                var path = target.View?.AgentASP?.Path ?? target.View?.AgentASP?.RequestedPath;
                if (path == null) { return; }
                if (path.GetTotalLength() > 8f.Feet().Meters)
                {
                    return;
                }
                __result = false;
            }
            catch (Exception ex) { Main.Logger.Error("Failed to ForceFullAttackFix2.", ex); }
        }
    }
}
