using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Root.CheatRoot;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(LevelUpHelper), nameof(LevelUpHelper.GetSpentSkillPoints))]
    internal class ReduceSkillPoint
    {
        static void Postfix(ref int __result, ref UnitDescriptor unit)
        {
            LogWrapper.Get("PrestigePlus").Info("start");
        }
    }
}
