using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.IsMoveActionRestricted))]
    internal class ForestWardenSurprise
    {
        static void Postfix(ref bool __result, ref UnitEntityData __instance)
        {
            try
            {
                if (__instance.HasFact(Feature) && !__instance.Descriptor.State.HasCondition(UnitCondition.Staggered))
                {
                    __result = false;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ForestWardenSurprise.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ForestWarden.PreemptiveStrikeGuid);
    }

    [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.HasFullRoundAction))]
    internal class ForestWardenSurprise2
    {
        static void Postfix(ref bool __result, ref UnitEntityData __instance)
        {
            try
            {
                if (__instance.HasFact(Feature) && __instance.IsSurprising() && !__instance.IsActingSurpriseCommands())
                {
                    __result = false;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ForestWardenSurprise2.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Feature = BlueprintTool.GetRef<BlueprintFeatureReference>(ForestWarden.PreemptiveStrikeGuid);
    }
}
