using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
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
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static void PostFix(ref RulebookEvent __instance, ref Modifier bonus)
        {
            var fact = __instance.Initiator.GetFact(Ace);
            if (fact != null && bonus.Descriptor == ModifierDescriptor.Luck && bonus.Fact != fact)
            {
                __instance.AddModifier(bonus.Value + 1, fact, ModifierDescriptor.Luck);
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }

    [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.CopyModifiersFrom))]
    internal class FortuneChildFix2
    {
        static void Postfix(ref RulebookEvent __instance, ref RulebookEvent source)
        {
            if (source.m_ModifiableBonus == null || source.m_ModifiableBonus.ModifiersCount == 0)
            {
                return;
            }
            var fact = __instance.Initiator.GetFact(Ace);
            if (fact == null) { return; }
            foreach (var bonus in source.m_ModifiableBonus.m_Modifiers)
            {
                if (bonus.Descriptor == ModifierDescriptor.Luck && bonus.Fact != fact)
                {
                    __instance.AddModifier(bonus.Value + 1, fact, ModifierDescriptor.Luck);
                }
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }

    [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.AddTemporaryModifier))]
    internal class FortuneChildFix3
    {
        static void PostFix(ref RulebookEvent __instance, ref ModifiableValue.Modifier modifier)
        {
            var fact = __instance.Initiator.GetFact(Ace);
            if (fact != null && modifier.ModDescriptor == ModifierDescriptor.Luck && modifier.Source != fact)
            {
                modifier.ModValue = 1 + modifier.ModValue;
                modifier.Source = fact;
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }

    [HarmonyPatch(typeof(RulebookEvent), nameof(RulebookEvent.CopyTemporaryModifiersFrom))]
    internal class FortuneChildFix4
    {
        static void Postfix(ref RulebookEvent __instance, ref RulebookEvent rule)
        {
            if (__instance.m_Triggered || rule.m_TemporaryModifiers == null || rule.m_TemporaryModifiers.Count == 0)
            {
                return;
            }
            var fact = __instance.Initiator.GetFact(Ace);
            if (fact == null) { return; }
            foreach (var bonus in rule.m_TemporaryModifiers)
            {
                if (bonus.ModDescriptor == ModifierDescriptor.Luck && bonus.Source != fact)
                {
                    bonus.ModValue = 1 + bonus.ModValue;
                    bonus.Source = fact;
                }
            }
        }
        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Chaldira2Guid);
    }
}
