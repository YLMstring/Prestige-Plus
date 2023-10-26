using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Classes.Selection;
using System.Windows.Documents;
using Epic.OnlineServices;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Cheats;
using Pathfinding.Voxels;
using Kingmaker.Blueprints.Root;

namespace PrestigePlus.Patch
{
    internal class PatchKinetic
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintFeatureReference feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
        public static void PatchDomains(string fref)
        {
            var feature = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(fref).Get();
            try
            {
                var select = feature as BlueprintFeatureSelection;
                if (select?.m_AllFeatures?.Length > 0) 
                {
                    foreach (var domain in select.m_AllFeatures)
                    {
                        PatchDomains(domain.Guid.ToString());
                    }
                }
                var prog = feature as BlueprintProgression;
                if (prog != null)
                {
                    PatchProgression(feature.AssetGuid.ToString());
                }
                    IEnumerable<ContextRankConfig> crcs = feature.GetComponents<ContextRankConfig>();
                    if (crcs != null && crcs.Any())
                    {
                        foreach (ContextRankConfig crc in crcs)
                        {
                            if (PatchConfig(crc))
                            {
                                Logger.Info(feature.NameSafe());
                            }
                        }
                    }
                    //Logger.Info("start7");
                    var abilities = feature.GetComponent<AddFacts>();
                    PatchAbility(abilities?.m_Facts);
                    var abilities2 = feature.GetComponent<AddFeatureIfHasFact>();
                    PatchAbility(new[] { abilities2?.m_Feature });
                //Logger.Info("start8");
            } catch (Exception e) { Logger.Error("Failed to edit feature.", e); }
        }
        public static void PatchBuff(BlueprintUnitFactReference ability)
        {
            try
            {
                var actions = ability.Get().GetComponent<AbilityEffectRunAction>()?.Actions?.Actions;
                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        var action2 = action as ContextActionApplyBuff;
                        if (action2?.m_Buff != null)
                        {
                            IEnumerable<ContextRankConfig> crcs = action2.m_Buff.Get().GetComponents<ContextRankConfig>();
                            if (crcs != null && crcs.Any())
                            {
                                foreach (ContextRankConfig crc in crcs)
                                {
                                    if (PatchConfig(crc))
                                    {
                                        Logger.Info(action2.m_Buff.NameSafe());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit buff.", e); }
        }

        public static void PatchVariant(BlueprintUnitFactReference abilityBase)
        {
            try
            {
                var variants = abilityBase.Get().GetComponent<AbilityVariants>()?.m_Variants;
                if (variants != null)
                {
                    Logger.Info(abilityBase.NameSafe() + " detected");
                    foreach (BlueprintAbilityReference ability in variants)
                    {
                        if (ability == null) { continue; }
                        Logger.Info(ability.NameSafe() + " detected");
                        var crcs = ability.Get().GetComponents<ContextRankConfig>();
                        if (crcs != null && crcs.Any())
                        {
                            foreach (ContextRankConfig crc in crcs)
                            {
                                if (PatchConfig(crc))
                                {
                                    Logger.Info(ability.NameSafe());
                                }
                            }
                        }
                        PatchBuff(BlueprintTool.GetRef<BlueprintUnitFactReference>(ability.Guid.ToString()));
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit variant.", e); }
        }

        public static void PatchAbility(BlueprintUnitFactReference[] abilities)
        {
            try
            {
                if (abilities == null) { return; }
                    foreach (BlueprintUnitFactReference ability in abilities)
                    {
                        if (ability == null) { continue; }
                        //Logger.Info(ability.NameSafe() + " detected");
                        var crcs = ability.Get().GetComponents<ContextRankConfig>();
                    if (crcs != null && crcs.Any())
                    {
                        foreach (ContextRankConfig crc in crcs)
                        {
                            if (PatchConfig(crc))
                            {
                                Logger.Info(ability.NameSafe());
                            }
                        }
                    }
                    PatchBuff(ability);
                    PatchVariant(ability);
                    }
                
            }
            catch (Exception e) { Logger.Error("Failed to edit ability.", e); }
        }

        public static void PatchProgression(string fref)
        {
            try
            {
                var progress = BlueprintTool.GetRef<BlueprintProgressionReference>(fref).Get();
                if (progress?.LevelEntries?.Length > 0) 
                {
                    Logger.Info(progress.NameSafe());
                    foreach (LevelEntry entry in progress.LevelEntries)
                    {
                        foreach (BlueprintFeatureBase feature in entry.Features)
                        {
                            PatchDomains(feature.AssetGuid.ToString());
                        }
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit progression" + fref, e); }
        }
        private static bool isConfigClassBased(ContextRankConfig crc)
        {
            try
            {
                //Logger.Info("start11");
                if (crc == null) { return false; }
                //Logger.Info("start12");
                if (crc.m_FeatureList == null)
                {
                    crc.m_FeatureList = new BlueprintFeatureReference[0];
                }
                //Logger.Info("start13");
                if (crc.m_BaseValueType == ContextRankBaseValueType.ClassLevel) { crc.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype;  return true; }
                if (crc.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype) { crc.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype; return true; }
                if (crc.m_BaseValueType == ContextRankBaseValueType.OwnerSummClassLevelWithArchetype) { crc.m_BaseValueType = ContextRankBaseValueType.SummClassLevelWithArchetype; return true; }
                if (crc.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype) { return true; }
                return false;
            }
            catch { Logger.Error("Failed to get isConfigClassBased."); return false; }
        }

        public static bool PatchConfig(ContextRankConfig crc)
        {
            try
            {
                if (isConfigClassBased(crc))
                {
                    if (crc.m_FeatureList.Any() && crc.m_FeatureList.Contains(feat)) { return false; }
                    crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
                    return true;
                }
                return false;
            }
            catch (Exception e) { Logger.Error("Failed to edit config", e); return false; }
        }
        public static void Patch()
        {
            var list = FeatureSelectionRefs.InfusionSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list)
            {
                PatchDomains(domain.Guid.ToString());
            }
            var list2 = FeatureSelectionRefs.WildTalentSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list2)
            {
                PatchDomains(domain.Guid.ToString());
            }
        }
    }
}
