using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(MetamagicRodMechanics), nameof(MetamagicRodMechanics.IsAbilityFromStickyTouch))]
    internal class CloseRangeEatMeta
    {
        static void Postfix(ref AbilityData ability, ref bool __result)
        {
            if (ability?.Blueprint == BlueprintTool.GetRef<BlueprintAbilityReference>(SpireDefender.CloseRangeAblity2Guid).GetBlueprint())
            {
                __result = true;
            }
        }

        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
    }
}
