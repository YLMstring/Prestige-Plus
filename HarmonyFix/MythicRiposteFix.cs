using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using PrestigePlus.BasePrestigeEnhance;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(DuelistParry), nameof(DuelistParry.OnEventAboutToTrigger))]
    internal class MythicRiposteFix
    {
        static void Postfix(ref DuelistParry __instance, ref RuleAttackRoll evt)
        {
            if (!__instance.Owner.HasFact(Mythic)) { return; }
            int num = __instance.Owner.Descriptor.Progression.MythicLevel;
            ModifiableValue additionalAttackBonus = __instance.Owner.Stats.AdditionalAttackBonus;
            evt.AddTemporaryModifier(additionalAttackBonus.AddModifier(num, __instance.Runtime, ModifierDescriptor.UntypedStackable));
        }

        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>("{655910EE-C044-4A18-A725-84946B8B1110}");
    }

    [HarmonyPatch(typeof(DuelistParry), nameof(DuelistParry.OnEventDidTrigger))]
    internal class MythicRiposteFix2
    {
        static bool Prefix(ref DuelistParry __instance, ref RuleAttackRoll evt)
        {
            RuleAttackRoll.ParryData parry = evt.Parry;
            if (!((parry?.Initiator) != __instance.Owner))
            {
                RuleAttackRoll.ParryData parry2 = evt.Parry;
                if (parry2 == null || parry2.IsTriggered)
                {
                    if (evt.Result == AttackResult.Parried)
                    {
                        var duration = new Rounds?(1.Rounds());
                        TimeSpan? duration2 = duration != null ? new TimeSpan?(duration.Value.Seconds) : null;
                        if (__instance.Owner.HasFact(Parryplus))
                        {
                            evt.Initiator.AddBuff(DeBuff, __instance.Owner, duration2);
                        }
                        if (__instance.Owner.State.Features.DuelistRiposte)
                        {
                            SurprisingStrategy1(__instance.Owner, evt.Initiator);
                            if (!__instance.Owner.HasFact(Mythic) || __instance.Owner.HasFact(Mythic2)) { return true; }
                            __instance.Owner.AddBuff(Buff, __instance.Owner, duration2);
                        }
                    }
                }
            }
            return true;
        }

        private static bool SurprisingStrategy1(UnitEntityData caster, UnitEntityData target)
        {
            var maneuver = CombatManeuver.None;
            if (caster.HasFact(DirtyBlind))
            {
                maneuver = CombatManeuver.DirtyTrickBlind;
            }
            else if (caster.HasFact(DirtyEntangle))
            {
                maneuver = CombatManeuver.DirtyTrickEntangle;
            }
            else if (caster.HasFact(DirtySicken))
            {
                maneuver = CombatManeuver.DirtyTrickSickened;
            }
            else if (caster.HasFact(Disarm))
            {
                maneuver = CombatManeuver.Disarm;
            }
            else if (caster.HasFact(Sunder))
            {
                maneuver = CombatManeuver.SunderArmor;
            }
            if (maneuver == CombatManeuver.None) { return true; }
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
            AttackBonusRule.AddModifier(2, descriptor: ModifierDescriptor.Morale);
            ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, AttackBonusRule);
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            return true;
        }
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>("{655910EE-C044-4A18-A725-84946B8B1110}");
        private static BlueprintFeatureReference Mythic2 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.EverReady.ToString());
        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>("{2682CE40-4F60-482C-909B-90C991BFEFAC}");

        private static BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(SurprisingStrategy.SurprisingDirtyBlindbuffGuid);
        private static BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(SurprisingStrategy.SurprisingDirtyEntanglebuffGuid);
        private static BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(SurprisingStrategy.SurprisingDirtySickenbuffGuid);
        private static BlueprintBuffReference Disarm = BlueprintTool.GetRef<BlueprintBuffReference>(SurprisingStrategy.SurprisingDisarmbuffGuid);
        private static BlueprintBuffReference Sunder = BlueprintTool.GetRef<BlueprintBuffReference>(SurprisingStrategy.SurprisingSunderbuffGuid);

        private static BlueprintBuffReference DeBuff = BlueprintTool.GetRef<BlueprintBuffReference>(MythicRiposte.ExploitiveGuidBuff2);
        private static BlueprintFeatureReference Parryplus = BlueprintTool.GetRef<BlueprintFeatureReference>(MythicRiposte.Feat2Guid);
    }
}
