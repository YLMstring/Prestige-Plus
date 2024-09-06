using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.JoinCombat))]
    internal class NotSurprisedFix
    {
        static void Postfix(ref UnitCombatState __instance)
        {
            if (__instance.NotSurprised || !__instance.Unit.HasFact(Mythic)) { return; }
            __instance.NotSurprised = true;
        }

        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Magdh2Guid);
    }
}
