using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Commands.Base;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.FeatureSelector;
using Kingmaker.Blueprints.Classes.Selection;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(CharGenFeatureSelectorPhaseVM), nameof(CharGenFeatureSelectorPhaseVM.CheckIsCompleted))]
    internal class SkippableSelectionFix
    {
        static void Postfix(ref bool __result, ref CharGenFeatureSelectorPhaseVM __instance)
        {
            if (__result) { return; }
            if (__instance.FeatureSelectorStateVM?.Feature is BlueprintFeatureSelection feats && !feats.IsObligatory())
            {
                __result = true;
            }
        }
    }
}
