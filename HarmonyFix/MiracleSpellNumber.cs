using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.GetSpellsPerDay))]
    internal class MiracleSpellNumber
    {
        static void Postfix(ref int __result, ref Spellbook __instance)
        {
            if (__instance.Blueprint == Book.Get() || __instance.Blueprint == Book2.Get())
            {
                __result = 30;
            }
        }

        private static BlueprintSpellbookReference Book = BlueprintTool.GetRef<BlueprintSpellbookReference>(ExaltedEvangelist.SpellBookGuid);
        private static BlueprintSpellbookReference Book2 = BlueprintTool.GetRef<BlueprintSpellbookReference>(DeificObedience.SpellBookGuid);
    }

    [HarmonyPatch(typeof(Spellbook), nameof(Spellbook.SpendInternal))]
    internal class MiracleSpellNumber2
    {
        static void Postfix(ref bool __result, ref Spellbook __instance)
        {
            if (__instance.Blueprint == Book.Get() || __instance.Blueprint == Book2.Get())
            {
                __result = true;
            }
        }

        private static BlueprintSpellbookReference Book = BlueprintTool.GetRef<BlueprintSpellbookReference>(ExaltedEvangelist.SpellBookGuid);
        private static BlueprintSpellbookReference Book2 = BlueprintTool.GetRef<BlueprintSpellbookReference>(DeificObedience.SpellBookGuid);
    }
}
