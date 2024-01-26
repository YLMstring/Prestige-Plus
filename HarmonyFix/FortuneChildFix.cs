using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.TotalBonusValue), MethodType.Getter)]
    internal class FortuneChildFix
    {
        static void Postfix(ref RulebookEvent __instance, ref int __result)
        {
            if (__instance.Initiator.HasFact(Ace))
            {
                foreach (var mod in __instance.AllBonuses)
                {
                    if (mod.Descriptor == ModifierDescriptor.Luck)
                    {
                        __result += 1;
                        return;
                    }
                }
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }
}
