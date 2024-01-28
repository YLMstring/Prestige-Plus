using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
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
    [HarmonyPatch(typeof(RulebookEvent))]
    [HarmonyPatch("AddModifier")]
    [HarmonyPatch(new Type[] { typeof(Modifier) })]
    internal class FortuneChildFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static bool Prefix(ref RulebookEvent __instance, ref Modifier bonus)
        {
            Logger.Info(bonus.ToString() + " child");
            var fact = __instance.Initiator.GetFact(Ace);
            if (fact != null && bonus.Descriptor == ModifierDescriptor.Luck && bonus.Fact != fact)
            {
                Logger.Info(bonus.Value.ToString() + " child");
                __instance.AddModifier(bonus.Value + 1, bonus.Fact, ModifierDescriptor.Luck);
                return false;
            }
            return true;
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }
}
