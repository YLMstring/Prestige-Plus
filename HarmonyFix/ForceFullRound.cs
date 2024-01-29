using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Combat;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RequireFullRoundAction), MethodType.Getter)]
    internal class ForceFullRound
    {
        static void Postfix(ref bool __result, ref AbilityData __instance)
        {
            try
            {
                if (__result) { return; }
                if (__instance.Caster.HasFact(ForceFull) && __instance.Blueprint.Type == AbilityType.Spell && DestructiveSpellComp.HasDamage(__instance.Blueprint))
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ForceFullRound.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference ForceFull = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Rovagug3BuffGuid);
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.RuntimeActionType), MethodType.Getter)]
    internal class ForceFullRound2
    {
        static void Postfix(ref UnitCommand.CommandType __result, ref AbilityData __instance)
        {
            try
            {
                if (__result == UnitCommand.CommandType.Standard) { return; }
                if (__instance.Caster.HasFact(ForceFull) && __instance.Blueprint.Type == AbilityType.Spell && DestructiveSpellComp.HasDamage(__instance.Blueprint))
                {
                    __result = UnitCommand.CommandType.Standard;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ForceFullRound2.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference ForceFull = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Rovagug3BuffGuid);
    }
}
