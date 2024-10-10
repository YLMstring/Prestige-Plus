using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitEntityData), nameof(UnitEntityData.HasSwiftAction))]
    internal class BanSwiftFix
    {
        static void Postfix(ref bool __result, ref UnitEntityData __instance)
        {
            try
            {
                if (!__result) { return; }
                if (__instance.HasFact(Ban))
                {
                    __result = false;
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to BanSwiftFix.", ex); }
        }
        private static BlueprintBuffReference Ban = BlueprintTool.GetRef<BlueprintBuffReference>(DeificObedience.Groetus3AuraBuffGuid);
    }
}
