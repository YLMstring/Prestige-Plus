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

namespace PrestigePlus.Patch
{
    internal class PatchKinetic
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void PatchDomains(string fref)
        {
            var feature = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(fref).Get();
            var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
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
                    IEnumerable<ContextRankConfig> crcs = feature.GetComponents<ContextRankConfig>();
                    //Logger.Info("start2");
                    if (crcs != null && crcs.Any())
                    {
                        foreach (ContextRankConfig crc in crcs)
                        {
                            if (isConfigClassBased(crc))
                            {
                                Logger.Info(feature.ToString());
                                if (crc.m_FeatureList.Any() && crc.m_FeatureList.Contains(feat)) { continue; }
                                Logger.Info(feature.ToString());
                                crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
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
                var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
                var actions = ability.Get().GetComponent<AbilityEffectRunAction>()?.Actions?.Actions;
                if (actions != null)
                {
                    foreach (var action in actions)
                    {
                        var action2 = action as ContextActionApplyBuff;
                        if (action2?.m_Buff != null)
                        {
                            ContextRankConfig config = action2.m_Buff.Get().GetComponent<ContextRankConfig>();
                            if (isConfigClassBased(config))
                            {
                                Logger.Info(action2.m_Buff.ToString());
                                if (config.m_FeatureList.Any() && config.m_FeatureList.Contains(feat)) { continue; }
                                Logger.Info(action2.m_Buff.ToString());
                                config.m_FeatureList = config.m_FeatureList.AddToArray(feat);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit buff.", e); }
        }

        public static void PatchVariant(BlueprintUnitFactReference ability)
        {
            try
            {
                var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
                var variants = ability.Get().GetComponent<AbilityVariants>()?.m_Variants;
                if (variants != null)
                {
                    var abilities = new BlueprintUnitFactReference[0];
                    foreach (var stuff in variants)
                    {
                        abilities = abilities.AddToArray(BlueprintTool.GetRef<BlueprintUnitFactReference>(stuff.Guid.ToString()));
                    }
                    PatchAbility(abilities);
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit variant.", e); }
        }

        public static void PatchAbility(BlueprintUnitFactReference[] abilities)
        {
            try
            {
                if (abilities == null) { return; }
                var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
                    foreach (BlueprintUnitFactReference ability in abilities)
                    {
                        if (ability == null) { continue; }
                        ContextRankConfig crc = ability.Get().GetComponent<ContextRankConfig>();
                        if (isConfigClassBased(crc))
                        {
                            Logger.Info(ability.ToString());
                            if (crc.m_FeatureList.Any() && crc.m_FeatureList.Contains(feat)) { PatchBuff(ability); PatchVariant(ability); continue; }
                            Logger.Info(ability.ToString());
                            crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
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
                if (progress?.LevelEntries?.Length > 0) { Logger.Info("no entry"); return; }
                foreach (LevelEntry entry in progress.LevelEntries)
                {
                    foreach (BlueprintFeatureBase feature in entry.Features)
                    {
                        var pro = feature as BlueprintProgression;
                        if (pro.LevelEntries?.Length > 0)
                        {
                            PatchProgression(feature.AssetGuid.ToString());
                        }
                        else
                        {
                            PatchDomains(feature.AssetGuid.ToString());
                        }
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit progression.", e); }
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
                //Logger.Info("start15");
                return false;
            }
            catch { Logger.Error("Failed to get isConfigClassBased."); return false; }
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
            var list3 = FeatureSelectionRefs.ElementalFocusSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list3)
            {
                PatchProgression(domain.Guid.ToString());
            }
        }
    }
}
