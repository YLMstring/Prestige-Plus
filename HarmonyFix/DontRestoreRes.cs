using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using BlueprintCore.Utils;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitAbilityResourceCollection))]
    [HarmonyPatch("Restore")]
    [HarmonyPatch(new Type[] { typeof(BlueprintScriptableObject) })]
    internal class DontRestoreRes
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static bool Prefix(ref BlueprintScriptableObject blueprint, ref UnitAbilityResourceCollection __instance)
        {
            try
            {
                if (Res.Get() == blueprint && __instance.m_Owner?.HasFact(Summon) != true)
                {
                    return false;
                }
            }
            catch (Exception e) { Logger.Error("fail DontRestoreRes", e); }
            return true;
        }

        private static BlueprintAbilityResourceReference Res = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(Souldrinker.SoulPoolAbilityResGuid);
        private static BlueprintFeatureReference Summon = BlueprintTool.GetRef<BlueprintFeatureReference>(Souldrinker.CacodaemonGuid);
    }
}
