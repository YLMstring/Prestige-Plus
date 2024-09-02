using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Assets.Controllers.GlobalMap;
using Kingmaker.Blueprints;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.Globalmap.State;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using static RootMotion.FinalIK.GenericPoser;

namespace PrestigePlus.Blueprint
{
    internal class LazyMap
    {
        public static void Configure()
        {
            var root = RootRefs.BlueprintRoot.Reference.Get().GlobalMap;
            var map = root.m_StartGlobalMap.Get();
            map.MechanicsSpeedBase *= 1000;
            map.VisualSpeedBase *= 1000;
            root.ArmyMovementPointsPerMileCost = 0;
        }
    }

    [HarmonyPatch(typeof(LocationRevealController), nameof(LocationRevealController.GetRevealRange))]
    internal class LazyMapFix1
    {
        static void Postfix(ref float __result)
        {
            if (ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("lazymap")))
            {
                __result = 1000 * __result;
            }
        }
    }

    [HarmonyPatch(typeof(GlobalMapArmyState), nameof(GlobalMapArmyState.SpendMovementPoints))]
    internal class LazyMapFix2
    {
        static bool Prefix()
        {
            return !ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("lazymap"));
        }
    }
}
