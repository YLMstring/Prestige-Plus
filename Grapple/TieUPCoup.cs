using HarmonyLib;
using Kingmaker.GameModes;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    [HarmonyPatch(typeof(AbilityTargetHasConditionOrBuff), nameof(AbilityTargetHasConditionOrBuff.IsTargetRestrictionPassed))]
    internal class TieUPCoup
    {
        static void Postfix(ref bool __result, ref AbilityTargetHasConditionOrBuff __instance, ref TargetWrapper target)
        {
            if (__instance.Condition != Kingmaker.UnitLogic.UnitCondition.Helpless) 
            {
                return;
            }
            if (target.Unit.Descriptor.Get<UnitPartGrappleTargetPP>() == null)
            {
                return;
            }
            if (!target.Unit.Descriptor.Get<UnitPartGrappleTargetPP>().IsTiedUp)
            {
                return;
            }
            __result = true; 
        }
    }
}
