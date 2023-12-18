using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(DeflectArrows), nameof(DeflectArrows.CheckRestriction))]
    internal class CombatJuggleFix
    {
        static void Postfix(ref DeflectArrows __instance, ref bool __result)
        {
            if (!__result && (__instance.Owner.HasFact(Ace) || __instance.Owner.HasFact(Buff)))
            {
                __result = true;
            }
        }

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.CombatJugglingGuid);
        private static BlueprintFeatureReference Buff = BlueprintTool.GetRef<BlueprintFeatureReference>(FuriousGuardian.DeflectArrowsBuffGuid);
    }
}
