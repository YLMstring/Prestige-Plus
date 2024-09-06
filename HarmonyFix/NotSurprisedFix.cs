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
    [HarmonyPatch(typeof(UnitCombatState), nameof(UnitCombatState.CanActInCombat))]
    internal class NotSurprisedFix
    {
        static void Postfix(ref UnitCombatState __instance, ref bool __result)
        {
            if (!__result && __instance.Unit.HasFact(Mythic))
            {
                __result = __instance.m_InCombat;
            }
        }

        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Magdh2Guid);
    }
}
