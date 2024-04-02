using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(PrerequisiteStatValue), nameof(PrerequisiteStatValue.GetStatValue))]
    internal class DirtyFightingReplace
    {
        static void Postfix(ref int __result, ref UnitDescriptor unit, ref PrerequisiteStatValue __instance)
        {
            if (unit.HasFact(Vital) && (__instance.Stat == StatType.BaseAttackBonus))
            {
                if (__instance.OwnerBlueprint is not BlueprintFeature blue) { return; }
                var prere = blue.GetComponents<PrerequisiteFeature>();
                if (prere == null) { return; }
                bool isVital = false;
                foreach (var pre in prere)
                {
                    if (pre.Feature == FeatureRefs.VitalStrikeFeature.Reference.Get())
                    {
                        isVital = true;
                        break;
                    }
                }
                if (!isVital) { return; }
                int usher = unit.Progression.GetClassLevel(Usher);
                if (usher >= 1) { __result = 1 + __result; }
                if (usher >= 5) { __result = 1 + __result; }
                if (usher >= 9) { __result = 1 + __result; }
            }
            if (unit.HasFact(Dirty) && (__instance.Stat == StatType.Dexterity || __instance.Stat == StatType.Intelligence))
            {
                if (__instance.OwnerBlueprint is not BlueprintFeature blue) { return; }
                if (blue.GetComponent<ManeuverBonus>() == null)
                {
                    var prere = blue.GetComponents<PrerequisiteFeature>();
                    if (prere == null) { return; }
                    bool isCMB = false;
                    foreach (var pre in prere)
                    {
                        if (pre.Feature?.GetComponent<ManeuverBonus>() != null)
                        {
                            isCMB = true;
                            break;
                        }
                    }
                    if (!isCMB) { return; }
                }
                __result = Math.Max(13, __result);
            }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference Dirty = BlueprintTool.GetRef<BlueprintFeatureReference>(DirtyFighting.DirtyFightingGuid);
        private static BlueprintFeatureReference Vital = BlueprintTool.GetRef<BlueprintFeatureReference>(MortalUsher.PsychicEsotericaGuid);
        private static BlueprintCharacterClassReference Usher = BlueprintTool.GetRef<BlueprintCharacterClassReference>(MortalUsher.ArchetypeGuid);
    }

    [HarmonyPatch(typeof(PrerequisiteFeature), nameof(PrerequisiteFeature.CheckInternal))]
    internal class DirtyFightingReplace2
    {
        static void Postfix(ref bool __result, ref FeatureSelectionState selectionState, ref UnitDescriptor unit, ref PrerequisiteFeature __instance)
        {
            if (__result) { return; }
            if (unit.Progression.Features.HasFact(FeatureRefs.KineticWarriorFeature.Reference) && __instance.Feature == FeatureRefs.CombatExpertiseFeature.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (unit.Progression.Features.HasFact(Heritage1) && __instance.Feature == RaceRefs.AasimarRace.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (unit.Progression.Features.HasFact(Heritage2) && __instance.Feature == RaceRefs.DhampirRace.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (unit.Progression.Features.HasFact(Heritage3) && __instance.Feature == RaceRefs.ElfRace.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (unit.Progression.Features.HasFact(Heritage4) && __instance.Feature == RaceRefs.KitsuneRace.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (unit.Progression.Features.HasFact(Defender) && __instance.Feature == FeatureRefs.CombatReflexes.Reference.Get())
            {
                __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
                return;
            }
            if (__instance.Feature != FeatureRefs.CombatExpertiseFeature.Reference.Get() && __instance.Feature != FeatureRefs.ImprovedUnarmedStrike.Reference.Get())
            {
                return;
            }
            if (!unit.Progression.Features.HasFact(Dirty))
            {
                return;
            }
            if (__instance.OwnerBlueprint is not BlueprintFeature blue) { return; }
            if (blue.GetComponent<ManeuverBonus>() == null)
            {
                var prere = blue.GetComponents<PrerequisiteFeature>();
                if (prere == null) { return; }
                bool isCMB = false;
                foreach (var pre in prere)
                {
                    if (pre.Feature?.GetComponent<ManeuverBonus>() != null)
                    {
                        isCMB = true;
                        break;
                    }
                }
                if (!isCMB) { return; }
            }
            __result = !(selectionState != null) || !selectionState.IsSelectedInChildren(__instance.Feature);
        }

        private static BlueprintFeatureReference Dirty = BlueprintTool.GetRef<BlueprintFeatureReference>(DirtyFighting.DirtyFightingGuid);
        private static BlueprintFeatureReference Defender = BlueprintTool.GetRef<BlueprintFeatureReference>(HighGuardian.DefenderReflexesGuid);

        private static BlueprintFeatureReference Heritage1 = BlueprintTool.GetRef<BlueprintFeatureReference>(RacialHeritage.CelestialCrusaderGuid);
        private static BlueprintFeatureReference Heritage2 = BlueprintTool.GetRef<BlueprintFeatureReference>(RacialHeritage.BloodDrinkGuid);
        private static BlueprintFeatureReference Heritage3 = BlueprintTool.GetRef<BlueprintFeatureReference>(RacialHeritage.DarkPowersGuid);
        private static BlueprintFeatureReference Heritage4 = BlueprintTool.GetRef<BlueprintFeatureReference>(RacialHeritage.MagicKitsuneGuid);
    }
}
