using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Root.CheatRoot;
using static Kingmaker.Utility.UnitDescription.UnitDescription;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(LevelUpHelper), nameof(LevelUpHelper.GetSpentSkillPoints))]
    internal class ReduceSkillPoint
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

    [HarmonyPatch(typeof(UnitProgressionData), nameof(UnitProgressionData.SetupLevelsIfNecessary))]
    internal class ReduceSkillPoint2
    {
        static void Prefix(ref UnitProgressionData __instance)
        {
            if (__instance.m_CharacterLevel == null || __instance.m_MythicLevel == null)
            {
                __instance.m_CharacterLevel = new int?(0);
                __instance.m_MythicLevel = new int?(0);
                foreach (Kingmaker.UnitLogic.ClassData classData in __instance.Classes)
                {
                    if (classData.CharacterClass.IsMythic)
                    {
                        __instance.m_MythicLevel += classData.Level;
                    }
                    else
                    {
                        __instance.m_CharacterLevel += classData.Level;
                    }
                }
                foreach (var feat in __instance.Features)
                {
                    var comp = feat.GetComponent<FakeLevelUpClass>();
                    if (comp == null) continue;
                    var realclazz = BlueprintTool.GetRef<BlueprintCharacterClassReference>(comp.clazz)?.Get();
                    if (realclazz == null) continue;
                    __instance.m_CharacterLevel -= 1;
                }
            }
        }
    }
}
