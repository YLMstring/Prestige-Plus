using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(PrerequisiteAlignment), nameof(PrerequisiteAlignment.CheckInternal))]
    internal class PRCalignmentFix
    {
        static void Postfix(ref bool __result, ref UnitDescriptor unit, ref PrerequisiteAlignment __instance)
        {
            try
            {
                if (__result) { return; }
                if (__instance.OwnerBlueprint is BlueprintCharacterClass clazz && clazz.PrestigeClass && unit.Progression.GetClassLevel(clazz) > 0)
                {
                    __result = true;
                }
            }
            catch (Exception ex) { Main.Logger.Error("Fail to PRCalignmentFix.", ex); }
        }
    }
}
