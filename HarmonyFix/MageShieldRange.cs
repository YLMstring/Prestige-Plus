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
using PrestigePlus.Blueprint.PrestigeClass;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.Range), MethodType.Getter)]
    internal class MageShieldRange
    {
        static void Postfix(ref AbilityData __instance, ref AbilityRange __result)
        {
            if (__instance.Blueprint == AbilityRefs.MageShield.Reference.Get())
            {
                if (__instance.Caster.HasFact(Ace))
                {
                    if (__result == AbilityRange.Personal)
                    {
                        __result = AbilityRange.Touch;
                    }
                }
            }
            if (__instance.Caster?.HasFact(Touch) == true)
            {
                if (__instance.Blueprint.School.HasFlag(SpellSchool.Enchantment) || __instance.Blueprint.School.HasFlag(SpellSchool.Divination))
                {
                    __result = AbilityRange.Touch;
                }
            }
        }
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(MageHandTrick.ShieldMainFeatGuid);
        private static BlueprintBuffReference Touch = BlueprintTool.GetRef<BlueprintBuffReference>(EnchantingCourtesan.EnchantingTouchBuffGuid);
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.TargetAnchor), MethodType.Getter)]
    internal class MageShieldRange2
    {
        static void Postfix(ref AbilityData __instance, ref AbilityTargetAnchor __result)
        {
            if (__instance.Blueprint == AbilityRefs.MageShield.Reference.Get())
            {
                if (__instance.Caster.HasFact(Ace))
                {
                    if (__result == AbilityTargetAnchor.Owner)
                    {
                        __result = AbilityTargetAnchor.Unit;
                    }
                }
            }
        }
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(MageHandTrick.ShieldMainFeatGuid);
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.AbilityTargetsAround), MethodType.Getter)]
    internal class MageShieldRange3
    {
        static void Postfix(ref AbilityData __instance, ref AbilityTargetsAround __result)
        {
            if (__instance.Caster?.HasFact(Touch) == true)
            {
                if (__instance.Blueprint.School.HasFlag(SpellSchool.Enchantment) || __instance.Blueprint.School.HasFlag(SpellSchool.Divination))
                {
                    __result = null;
                }
            }
        }
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Touch = BlueprintTool.GetRef<BlueprintBuffReference>(EnchantingCourtesan.EnchantingTouchBuffGuid);
    }
}
