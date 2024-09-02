using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Globalmap.Blueprints;
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
}
