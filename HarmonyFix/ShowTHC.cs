using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UI;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using static Kingmaker.UI.CanvasScalerWorkaround;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(CursorController), nameof(CursorController.SetCursor))]
    internal class ShowTHC
    {

        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static void Prefix(ref CursorRoot.CursorType cursorType, ref string text, ref CursorController __instance)
        {
            try
            {
                if (cursorType != CursorRoot.CursorType.AttackCursor && cursorType != CursorRoot.CursorType.RangeAttackCursor) { return; }
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("thc"))) { return; }
                //if (!CombatController.IsInTurnBasedCombat()) { return; }
                UnitEntityData selectedUnit = CombatController.SelectedUnit;
                UnitPartRider unitPartRider = selectedUnit.Get<UnitPartRider>();
                UnitEntityData caster = (unitPartRider?.SaddledUnit) ?? selectedUnit;
                var weapon = caster.GetThreatHandMelee()?.Weapon;
                if (weapon == null) { weapon = caster.GetThreatHandRanged()?.Weapon; }
                if (weapon == null) { return; }
                RuleCalculateAttackBonusWithoutTarget rule = new(caster, weapon, 0);
                Rulebook.Trigger(rule);
                int ab = rule.Result;
                int ac = __instance.m_HoveredUnitData.Stats.AC;
                int chance = 105 - 5 * (ac - ab);
                chance = Math.Max(5, chance);
                chance = Math.Min(95, chance);
                string thc = chance.ToString() + "%";
                if (text == null)
                {
                    text = thc;
                }
                else
                {
                    text = thc + text;
                }
            }
            catch (Exception ex) { Logger.Error("fail THC", ex); }
        }
    }
}
