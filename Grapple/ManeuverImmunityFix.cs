using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    [HarmonyPatch(typeof(ManeuverImmunity), nameof(ManeuverImmunity.OnEventAboutToTrigger))]
    internal class ManeuverImmunityFix
    {
        static void Postfix(ref ManeuverImmunity __instance, ref RuleCombatManeuver evt)
        {
            if (evt.Type != CombatManeuver.Grapple) { return; }
            if (__instance.Type != CombatManeuver.Grapple && __instance.Type != CombatManeuver.None) { return; }
            if (evt.Initiator == null) { return; }
            if (evt.Initiator.Get<UnitPartGrappleTargetPP>()) { evt.AutoFailure = false; return; }
            if (evt.Initiator.HasFact(ChargeGrappleBuff)) { evt.AutoFailure = false; return; }
            if (evt.Initiator.HasFact(CasterBuff)) { evt.AutoFailure = false; return; }
            if (evt.Initiator.HasFact(Master)) { evt.AutoFailure = false; return; }
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{E78853A3-7B2C-40B6-831F-824B1423F7F6}");
        private static BlueprintBuffReference ChargeGrappleBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{C5F4DDFE-CA2E-4309-90BB-1BB5C0F32E78}");

        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>("{E6382367-C2B3-4A72-8B3E-C5C7C5841874}");
    }
}
