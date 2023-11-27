using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Range), MethodType.Getter)]
    internal class MageShieldRange
    {
        static void Postfix(ref AbilityData __instance, ref AbilityRange __result)
        {
            //Logger.Info("bababa");
            if (__instance.Blueprint == AbilityRefs.MageShield.Reference.Get())
            {
                //Logger.Info("bababa1");
                if (__instance.Caster.HasFact(Ace))
                {
                    //Logger.Info("bababa2");
                    if (__result == AbilityRange.Personal)
                    {
                        //Logger.Info("bababa3");
                        __result = AbilityRange.Touch;
                    }
                }
            }
        }
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(MageHandTrick.ShieldMainFeatGuid);
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.TargetAnchor), MethodType.Getter)]
    internal class MageShieldRange2
    {
        static void Postfix(ref AbilityData __instance, ref AbilityTargetAnchor __result)
        {
            //Logger.Info("bababa");
            if (__instance.Blueprint == AbilityRefs.MageShield.Reference.Get())
            {
                //Logger.Info("bababa1");
                if (__instance.Caster.HasFact(Ace))
                {
                    //Logger.Info("bababa2");
                    if (__result == AbilityTargetAnchor.Owner)
                    {
                        //Logger.Info("bababa3");
                        __result = AbilityTargetAnchor.Unit;
                    }
                }
            }
        }
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(MageHandTrick.ShieldMainFeatGuid);
    }
}
