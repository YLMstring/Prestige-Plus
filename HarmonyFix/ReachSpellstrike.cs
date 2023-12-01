using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitPartMagus), nameof(UnitPartMagus.IsSuitableForEldritchArcherSpellStrike))]
    internal class ReachSpellStrike
    {
        static void Postfix(ref UnitPartMagus __instance, ref AbilityData spell, ref bool __result)
        {
            if (!__result && __instance.Owner.HasFact(Ace))
            {
                if (spell.Blueprint.Range == Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Touch && __instance.IsSpellFromMagusSpellList(spell))
                {
                    __result = true;
                }
            }
        }

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(SpireDefender.ReachSpellstrikeGuid);
    }

    [HarmonyPatch(typeof(AbilityData), nameof(AbilityData.IsMagusSpell))]
    internal class CloseSpellStrike
    {
        static void Postfix(ref AbilityData __instance, ref bool isMagus)
        {
            var unitPartMagus = __instance.Caster.Get<UnitPartMagus>();
            if (__instance.Caster.HasFact(Ace) && unitPartMagus?.IsSuitableForEldritchArcherSpellStrike(__instance) == true && unitPartMagus.Spellstrike.Active)
            {
                isMagus = true;
            }
        }

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(SpireDefender.CloseRangeGuid);
    }
}
