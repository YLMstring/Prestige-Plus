using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.BasePrestigeEnhance
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
            if (!__instance.Owner.HasFact(Mythic) || __instance.Owner.HasFact(Mythic2)) { return true; }
            RuleAttackRoll.ParryData parry = evt.Parry;
            if (!(((parry != null) ? parry.Initiator : null) != __instance.Owner))
            {
                RuleAttackRoll.ParryData parry2 = evt.Parry;
                if (parry2 == null || parry2.IsTriggered)
                {
                    if (evt.Result == AttackResult.Parried && __instance.Owner.State.Features.DuelistRiposte)
                    {
                        var duration = new Rounds?(1.Rounds());
                        TimeSpan? duration2 = (duration != null) ? new TimeSpan?(duration.Value.Seconds) : null;
                        __instance.Owner.AddBuff(Buff, __instance.Owner, duration2);
                    }
                }
            }
            return true;
        }
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>("{655910EE-C044-4A18-A725-84946B8B1110}");
        private static BlueprintFeatureReference Mythic2 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.EverReady.ToString());
        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>("{2682CE40-4F60-482C-909B-90C991BFEFAC}");
    }
}
