using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Commands;
using PrestigePlus.Blueprint.Archetype;
using Kingmaker.UnitLogic;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitAttack), nameof(UnitAttack.IsFullAttackRestricted))]
    internal class ForceFullAttackFix
    {
        static void Postfix(ref UnitAttack __instance, ref bool __result)
        {
            if (__instance.Executor.HasFact(RapidBuff))
            {
                __result = true;
            }
        }

        private static BlueprintBuffReference RapidBuff = BlueprintTool.GetRef<BlueprintBuffReference>(DawnflowerDervish.RapidAttackBuffGuid);
    }
}
