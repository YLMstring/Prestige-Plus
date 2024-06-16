using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ItemSlot), nameof(ItemSlot.Active), MethodType.Getter)]
    internal class MultiArmFix
    {
        static void Postfix(ref bool __result, ref ItemSlot __instance)
        {
            try
            {
                if (__result) { return; }
                if (__instance.Owner.HasFact(ForceFull))
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to MultiArmFix.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference ForceFull = BlueprintTool.GetRef<BlueprintFeatureReference>(RacialHeritage.MultiArmedGuid);
    }
}
