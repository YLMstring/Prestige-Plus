using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class DeadlyDealer : AutoMetamagic
    {
        public AbilityRange Range;
    }

    [HarmonyPatch(typeof(AutoMetamagic), nameof(AutoMetamagic.IsSuitableAbility))]
    internal class TieUPCoup
    {
        static void Postfix(ref bool __result, ref AutoMetamagic __instance, ref BlueprintAbility ability)
        {
            if (__result && __instance is DeadlyDealer deal)
            {
                if (ability.Range != deal.Range)
                {
                    __result = false;
                }
            }
        }
    }
}
