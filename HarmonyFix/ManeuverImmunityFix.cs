using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Grapple;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ManeuverImmunity), nameof(ManeuverImmunity.OnEventAboutToTrigger))]
    internal class ManeuverImmunityFix
    {
        static void Postfix(ref ManeuverImmunity __instance, ref RuleCombatManeuver evt)
        {
            if (evt.Type == CombatManeuver.Grapple)
            {
                //if (__instance.Type != CombatManeuver.Grapple && __instance.Type != CombatManeuver.None) { return; } wtf?
                //if (evt.Initiator.Get<UnitPartGrappleTargetPP>()) { evt.AutoFailure = false; return; } doesn't make sense?
                if (evt.Initiator.HasFact(ChargeGrappleBuff)) { evt.AutoFailure = false; return; }
                if (evt.Initiator.HasFact(Aerial) && evt.Initiator.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff)) { evt.AutoFailure = false; return; }
                if (evt.Initiator.HasFact(Master)) { evt.AutoFailure = false; return; }
            }
            else if (evt.Type == CombatManeuver.Trip)
            {
                if (evt.Initiator.HasFact(Ace)) { evt.AutoFailure = false; return; }
            }
        }

        private static BlueprintBuffReference ChargeGrappleBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{C5F4DDFE-CA2E-4309-90BB-1BB5C0F32E78}");

        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>("{E6382367-C2B3-4A72-8B3E-C5C7C5841874}");
        private static BlueprintFeatureReference Aerial = BlueprintTool.GetRef<BlueprintFeatureReference>(AerialAssault.FeatGuid);

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedTrip.AceTripGuid);
    }
}
