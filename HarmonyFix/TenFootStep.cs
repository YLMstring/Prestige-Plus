using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(TurnController), "get_FiveFootStep")]
    internal class TenFootStep
    {
        static void Postfix(ref Feet __result, ref TurnController __instance)
        {
            try
            {
                if (__instance.Rider?.HasFact(Dancer) == true && __instance.Rider.HasFact(jab))
                {
                    __result = (__result.Value + 5).Feet();
                    return;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to TenFootStep.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Dancer = BlueprintTool.GetRef<BlueprintFeatureReference>(JabbingStyle.DancerGuid);
        private static BlueprintBuffReference jab = BlueprintTool.GetRef<BlueprintBuffReference>(JabbingStyle.StylebuffGuid);
    }
}
