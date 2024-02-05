using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
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
    [HarmonyPatch(typeof(RuleCheckCastingDefensively), nameof(RuleCheckCastingDefensively.Success), MethodType.Getter)]
    internal class CastingDefensiveFix
    {
        static void Postfix(ref bool __result, ref RuleCheckCastingDefensively __instance)
        {
            try
            {
                if (__result) { return; }
                if (__instance.Initiator.HasFact(Touch) && __instance.Spell?.Blueprint.Type == AbilityType.Spell && __instance.Spell.Range == AbilityRange.Touch)
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to CastingDefensiveFix.", ex); }
        }

        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Touch = BlueprintTool.GetRef<BlueprintBuffReference>(EnchantingCourtesan.EnchantingTouchGuid);
    }
}
