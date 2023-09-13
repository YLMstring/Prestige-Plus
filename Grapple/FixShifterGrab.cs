using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    [HarmonyPatch(typeof(ContextActionGrapple), nameof(ContextActionGrapple.RunAction))]
    internal class FixShifterGrab
    {
        static bool Prefix(ref ContextActionGrapple __instance)
        {
            UnitEntityData unit = __instance.Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            UnitEntityData maybeCaster = __instance.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return false;
            }
            if (unit == maybeCaster)
            {
                PFLog.Default.Error("Unit can't grapple themselves", Array.Empty<object>());
                return false;
            }
            if (maybeCaster.Get<UnitPartGrappleInitiatorPP>())
            {
                PFLog.Default.Error("Caster already grapple", Array.Empty<object>());
                return false;
            }
            if (unit.Get<UnitPartGrappleTargetPP>())
            {
                PFLog.Default.Error("Target already grappled", Array.Empty<object>());
                return false;
            }
            if (!maybeCaster.HasFact(Base))
            {
                return true;
            }
            maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, CasterBuff, __instance.Context);
            unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, TargetBuff, __instance.Context);
            return false;
        }

        private static BlueprintFeatureReference Base = BlueprintTool.GetRef<BlueprintFeatureReference>("{D74F645A-D0F2-470B-B68B-E76EC083A6D8}");

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
    }
}
