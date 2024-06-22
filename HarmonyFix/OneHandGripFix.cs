using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items.Slots;
using Kingmaker.Blueprints.Items.Weapons;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.CombatStyle;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ItemEntityWeapon), nameof(ItemEntityWeapon.CanTakeOneHand))]
    internal class OneHandGripFix
    {
        static void Postfix(ref bool __result, ref ItemEntityWeapon __instance, ref UnitEntityData unit)
        {
            try
            {
                if (__result) { return; }
                if (unit?.HasFact(Staff) == true && __instance.Blueprint.Category == Kingmaker.Enums.WeaponCategory.Quarterstaff)
                {
                    __result = true;
                }
                else if (unit?.HasFact(Staff2) == true && (__instance.Blueprint.FighterGroup == WeaponFighterGroupFlags.Spears || __instance.Blueprint.FighterGroup == WeaponFighterGroupFlags.Polearms))
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Main.Logger.Error("Fail to OneHandGripFix.", ex); }
        }

        private static BlueprintFeatureReference Staff = BlueprintTool.GetRef<BlueprintFeatureReference>(StaffMagus.QuarterstaffMasterGuid);
        private static BlueprintBuffReference Staff2 = BlueprintTool.GetRef<BlueprintBuffReference>(SpearDancingStyle.StylebuffGuid);
    }
}
