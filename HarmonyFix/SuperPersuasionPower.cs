using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RuleStatCheck), nameof(RuleStatCheck.StatType), MethodType.Getter)]
    internal class SuperPersuasionPower
    {
        static void Postfix(ref RuleStatCheck __instance, ref StatType __result)
        {
            if (__result == StatType.CheckIntimidate && __instance.Initiator.HasFact(Bluff))
            {
                int inti = __instance.Initiator.Stats.CheckIntimidate;
                int bluf = __instance.Initiator.Stats.CheckBluff;
                int dipl = __instance.Initiator.Stats.CheckDiplomacy;
                if (bluf > inti)
                {
                    __result = StatType.CheckBluff;
                }
                if (dipl > inti && dipl > bluf && __instance.Initiator.HasFact(Dip))
                {
                    __result = StatType.CheckDiplomacy;
                }
            }
        }
        private static BlueprintFeatureReference Dip = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Mephistopheles2Guid);
        private static BlueprintFeatureReference Bluff = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Mephistopheles3Guid);
    }
}
