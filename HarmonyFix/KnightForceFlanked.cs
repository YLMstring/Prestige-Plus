using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChromaSDK.ChromaAnimationAPI;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitCombatState), "get_IsFlanked")]
    internal class KnightForceFlanked
    {
        static void Postfix(ref bool __result, ref UnitCombatState __instance)
        {
            try
            {
                if (__instance.Unit.HasFact(Flanked))
                {
                    __result = true;
                    return;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ForceFlanked.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Flanked = BlueprintTool.GetRef<BlueprintBuffReference>(EsotericKnight.ConfoundingProjectionBuffGuid);
    }
}
