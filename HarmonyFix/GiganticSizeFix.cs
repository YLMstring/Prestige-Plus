using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.NewUnitParts;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitState), nameof(UnitState.Size), MethodType.Getter)]
    internal class GiganticSizeFix
    {
        static void Postfix(UnitState __instance, ref Size __result)
        {
            if (__instance.Owner.HasFact(Ace))
            {
                __result += 1;
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(MammothRider.GiganticSteed1Guid);
    }
}
