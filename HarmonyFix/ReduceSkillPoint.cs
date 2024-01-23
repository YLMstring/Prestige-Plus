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
}
