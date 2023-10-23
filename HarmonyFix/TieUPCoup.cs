using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.GameModes;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(AbilityTargetHasConditionOrBuff), nameof(AbilityTargetHasConditionOrBuff.IsTargetRestrictionPassed))]
    internal class TieUPCoup
    {
        static void Postfix(ref bool __result, ref AbilityTargetHasConditionOrBuff __instance, ref TargetWrapper target, ref UnitEntityData caster)
        {
            if (caster == null) { return; }
            var weapon = caster.GetThreatHandMelee();
            if (weapon == null) { return; }
            if (caster.HasFact(SlicerBuff) && weapon.Weapon.Blueprint.IsTwoHanded)
            {
                __result = false;
                return;
            }
            if (__instance.Condition != UnitCondition.Helpless)
            {
                return;
            }
            if (target.Unit.Descriptor.Get<UnitPartGrappleTargetPP>() == null)
            {
                return;
            }
            if (target.Unit.Descriptor.Get<UnitPartGrappleTargetPP>().IsTiedUp)
            {
                __result = true;
                return;
            }
            if (caster.HasFact(SlicerBuff) && target.Unit.Descriptor.Get<UnitPartGrappleTargetPP>().IsPinned)
            {
                __result = true;
                return;
            }
        }

        private static BlueprintBuffReference SlicerBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{2D59E73D-3E3A-448A-9ECB-0EAB89AD4009}");
    }
}
