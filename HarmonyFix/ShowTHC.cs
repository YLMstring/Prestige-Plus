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
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Items;
using BlueprintCore.Blueprints.References;
using Kingmaker.Localization;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;

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
                if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("thc")) || !CombatController.IsInTurnBasedCombat()) { return; }
                var caster = CombatController.SelectedUnit;
                if (cursorType == CursorRoot.CursorType.AttackCursor || cursorType == CursorRoot.CursorType.RangeAttackCursor) 
                {
                    var target = __instance.m_HoveredUnitData;
                    if (target == null) { return; }
                    var weapon = caster.Body.PrimaryHand.MaybeWeapon;
                    if (weapon == null) { weapon = caster.Body.EmptyHandWeapon; }
                    if (weapon == null) { return; }
                    int ab = Rulebook.Trigger(new RuleCalculateAttackBonus(caster, target, weapon, 0)).Result;
                    int ac = Rulebook.Trigger(new RuleCalculateAC(caster, target, weapon.Blueprint.AttackType)).Result;
                    int chance = 105 - 5 * (ac - ab);
                    chance = Math.Max(5, chance);
                    chance = Math.Min(95, chance);
                    string thc = chance.ToString() + "% ";
                    if (text == null)
                    {
                        text = thc;
                    }
                    else
                    {
                        text = thc + text;
                    }
                }
                else if (cursorType == CursorRoot.CursorType.CastCursor)
                {
                    var target = Game.Instance.TurnBasedCombatController?.CurrentTurn?.m_HighlightedUnit;
                    var ability = Game.Instance.TurnBasedCombatController.CurrentTurn.CurrentAbility;
                    if (ability == null || target == null) { return; }
                    int chance = 100;
                    int Touchchance = 100;
                    var save = ability.Blueprint.LocalizedSavingThrow;
                    int stat = GetSavestat(save, target);
                    if (stat != 100)
                    {
                        int dc = ability.CalculateParams().DC;
                        chance = 5 * (dc - stat) - 5;
                        chance = Math.Max(5, chance);
                        chance = Math.Min(95, chance);
                    }
                    ItemEntityWeapon weapon = null;
                    if (ability.Blueprint.GetComponent<AbilityEffectStickyTouch>() || ability.Blueprint.GetComponent<AbilityDeliverTouch>())
                    {
                        if (ability.Range == Kingmaker.UnitLogic.Abilities.Blueprints.AbilityRange.Touch)
                        {
                            weapon = ItemWeaponRefs.TouchItem.Reference.Get().CreateEntity<ItemEntityWeapon>();
                        }
                        else
                        {
                            weapon = ItemWeaponRefs.RayItem.Reference.Get().CreateEntity<ItemEntityWeapon>();
                        }
                    }
                    else if (ability.Blueprint.GetComponent<AbilityDeliverProjectile>()?.NeedAttackRoll == true)
                    {
                        weapon = ItemWeaponRefs.RayItem.Reference.Get().CreateEntity<ItemEntityWeapon>();
                    }
                    if (weapon != null)
                    {
                        int ab = Rulebook.Trigger(new RuleCalculateAttackBonus(caster, target, weapon, 0)).Result;
                        int ac = Rulebook.Trigger(new RuleCalculateAC(caster, target, weapon.Blueprint.AttackType)).Result;
                        Touchchance = 105 - 5 * (ac - ab);
                        Touchchance = Math.Max(5, Touchchance);
                        Touchchance = Math.Min(95, Touchchance);
                        weapon.Dispose();
                    }
                    string thc = "";
                    if (chance < 100)
                    {
                        thc = chance.ToString() + "%";
                    }
                    if (Touchchance < 100)
                    {
                        thc = Touchchance.ToString() + "% " + thc;
                    }
                    if (text == null)
                    {
                        text = thc;
                    }
                    else
                    {
                        text = thc + text;
                    }
                }              
            }
            catch (Exception ex) { Logger.Error("fail THC" + cursorType.ToString(), ex); }
        }

        private static int GetSavestat(LocalizedString save, UnitEntityData target)
        {
            if (save == null || save == "") { return 100; }
            var saveStr = save.ToString().First();
            if (saveStr == AbilityRefs.FlareBurst.Reference.Get().LocalizedSavingThrow.ToString().First())
            {
                return target.Stats.SaveFortitude;
            }
            else if (saveStr == AbilityRefs.Grease.Reference.Get().LocalizedSavingThrow.ToString().First())
            {
                return target.Stats.SaveReflex;
            }
            else if (saveStr == AbilityRefs.Daze.Reference.Get().LocalizedSavingThrow.ToString().First())
            {
                return target.Stats.SaveWill;
            }
            return 100;
        }
    }
}
