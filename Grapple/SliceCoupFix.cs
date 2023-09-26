using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    internal class SliceCoupFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus"); 
        [HarmonyPatch(typeof(UnitPartAbilityModifiers), nameof(UnitPartAbilityModifiers.RequireFullRound))]
        static void Postfix(ref bool __result, ref UnitPartAbilityModifiers __instance, ref BlueprintAbility ability)
        {
            try
            {
                if (!__result) { return; }
                if (ability == Slicer.Get() && __instance.Owner != null && __instance.Owner.HasFact(SliceBuff))
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to slice.", ex); }
        }
        private static BlueprintAbilityReference Slicer = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.CoupDeGraceAbility.ToString());
        private static BlueprintBuffReference SliceBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{2D59E73D-3E3A-448A-9ECB-0EAB89AD4009}");
    }
}
