using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.OnTrigger))]
    internal class FortuneChildFix
    {
        static void Postfix(ref RulebookEvent __instance, ref RulebookEventContext context)
        {
            var fact = __instance.Initiator?.GetFact(Master);
            if (fact != null)
            {
                foreach (var mod in __instance.AllBonuses)
                {
                    if (mod.Descriptor == ModifierDescriptor.Luck)
                    {
                        __instance.AddModifier(1, fact, ModifierDescriptor.UntypedStackable);
                        return;
                    }
                }
            }
        }

        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }
}
