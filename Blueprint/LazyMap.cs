using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Assets.Controllers.GlobalMap;
using Kingmaker.Blueprints;
using Kingmaker.Globalmap.Blueprints;
using Kingmaker.RuleSystem.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityEngine;

namespace PrestigePlus.Blueprint
{
    internal class LazyMap
    {
        public static void Configure()
        {
            var map = RootRefs.BlueprintRoot.Reference.Get().GlobalMap.m_StartGlobalMap.Get();
            map.ArmySpeedFactor *= 1000;
            map.PartySpeedFactor *= 1000;
            map.ExploreDistance *= 1000;
        }
    }

    [HarmonyPatch(typeof(GlobalMapMovementController), nameof(GlobalMapMovementController.CalcPlayerSpeedModifiers))]
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
}
