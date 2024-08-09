﻿using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ModifyD20), nameof(IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger))]
    internal class WeirdBonusFix
    {
        static bool Prefix()
        {
            RulebookEvent previousEvent = Rulebook.CurrentContext.PreviousEvent;
            if (previousEvent == null)
            {
                return false;
            }
            return true;
        }
    }

    [HarmonyPatch(typeof(MechanicsContext), nameof(MechanicsContext.SourceAbility), MethodType.Getter)]
    internal class WeirdBonusFix2
    {
        static void Postfix(ref BlueprintAbility __result)
        {
            __result ??= AbilityRefs.LoreReligionUseAbility.Reference;
        }
    }
}