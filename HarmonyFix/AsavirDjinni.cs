using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(AbilityTargetIsSuitableMount), nameof(AbilityTargetIsSuitableMount.CanMount))]
    internal class AsavirDjinni
    {
        static void Postfix(UnitEntityData master, UnitEntityData pet, ref bool __result)
        {
            if (master != null && pet != null && pet.HasFact(Master) && pet.State.IsConscious && pet.GetActivePolymorph().Component == null)
            {
                __result = true;
            }
            if (master != null && pet != null && master.HasFact(Bond) && pet.State.IsConscious && pet.IsDirectlyControllable)
            {
                __result = true;
            }
        }

        private static BlueprintFeatureReference Master = BlueprintTool.GetRef<BlueprintFeatureReference>("{9FF96DBE-FA76-486A-A0B0-41A7788862BA}");
        private static BlueprintFeatureReference Bond = BlueprintTool.GetRef<BlueprintFeatureReference>("{BDF243F1-A851-4C3F-9F6B-64E4848A7AE3}");
    }
}
