using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    [HarmonyPatch(typeof(AbilityData), "get_ActionType")]
    internal class SynthesisFix
    {
        static void Postfix(ref UnitCommand.CommandType __result, ref AbilityData __instance)
        {
            try
            {
                if (__instance.Caster == null || __result != UnitCommand.CommandType.Standard || __instance.Blueprint.IsFullRoundAction)
                {
                    return;
                }
                int spell = (__instance.Caster.Progression.GetClassLevel(Mystic) + 1) / 2;
                if (__instance.SpellLevel > spell)
                {
                    return;
                }
                if (__instance.Spellbook == __instance.Caster.Ensure<UnitPartMysticTheurge>().DivineSpellbook && __instance.Caster.HasFact(Divine))
                {
                    __result = UnitCommand.CommandType.Free;
                    return;
                }
                if (__instance.Spellbook == __instance.Caster.Ensure<UnitPartMysticTheurge>().ArcaneSpellbook && __instance.Caster.HasFact(Arcane))
                {
                    __result = UnitCommand.CommandType.Free;
                    return;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to synthesis.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Arcane = BlueprintTool.GetRef<BlueprintBuffReference>("{71740950-0B75-4157-9B40-85B80703AA7B}");
        private static BlueprintBuffReference Divine = BlueprintTool.GetRef<BlueprintBuffReference>("{806AC2E8-5B0E-46BE-841F-FF280D72CEFD}");

        private static BlueprintCharacterClassReference Mystic = BlueprintTool.GetRef<BlueprintCharacterClassReference>(CharacterClassRefs.MysticTheurgeClass.ToString());
    }
}
