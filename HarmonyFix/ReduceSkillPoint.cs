using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomComponent;
using PrestigePlus.GrappleMechanic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Root.CheatRoot;
using static Kingmaker.Utility.UnitDescription.UnitDescription;
using ClassData = Kingmaker.UnitLogic.ClassData;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(LevelUpController), nameof(LevelUpController.NeedToSetName))]
    internal class ReduceSkillPoint
    {
        static void Postfix(ref UnitDescriptor unit)
        {
            int cl = 0;
            foreach (ClassData classData in unit.Progression.Classes)
            {
                if (!classData.CharacterClass.IsMythic)
                {
                    // don't change this
                    if (classData.Level > 0)
                    {
                        cl += classData.Level;
                    }
                }
            }
            foreach (var feat in unit.Progression.Features)
            {
                var comp = feat.GetComponent<FakeLevelUpClass>();
                if (comp == null) continue;
                cl -= 1;
            }
            unit.Progression.CharacterLevel = cl;
        }
    }

    [HarmonyPatch(typeof(LevelUpHelper), nameof(LevelUpHelper.GetSpentSkillPoints))]
    internal class ReduceSkillPoint2
    {
        static void Postfix(ref int __result, ref UnitDescriptor unit)
        {
            foreach (var feat in unit.Progression.Features)
            {
                var comp = feat.GetComponent<FakeLevelUpClass>();
                if (comp == null) continue;
                var realclazz = BlueprintTool.GetRef<BlueprintCharacterClassReference>(comp.clazz)?.Get();
                if (realclazz == null) continue;
                __result += realclazz.SkillPoints;
            }
        }
    }
}
