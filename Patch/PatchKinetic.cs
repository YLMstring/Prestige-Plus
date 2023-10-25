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
                    //Logger.Info("start1");
                    IEnumerable<ContextRankConfig> crcs = feature.GetComponents<ContextRankConfig>();
                    //Logger.Info("start2");
                    if (crcs != null && crcs.Any())
                    {
                        //Logger.Info("start3");
                        foreach (ContextRankConfig crc in crcs)
                        {
                            //Logger.Info("start4");
                            if (isConfigClassBased(crc))
                            {
                                //Logger.Info("start5");
                                if (!crc.m_FeatureList.Contains(feat))
                                {
                                    //Logger.Info("start6");
                                    crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
                                }
                            }
                        }
                    }
                    //Logger.Info("start7");
                    PatchAbility(feature);
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
                                if (!config.m_FeatureList.Contains(feat))
                                {
                                    config.m_FeatureList = config.m_FeatureList.AddToArray(feat);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit buff.", e); }
        }

        public static void PatchAbility(BlueprintFeatureBase feature)
        {
            try
            {
                var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(EsotericKnight.FeatGuidPro2);
                AddFacts abilities = feature.GetComponent<AddFacts>();
                if (abilities != null)
                {
                    foreach (BlueprintUnitFactReference ability in abilities.m_Facts)
                    {
                        ContextRankConfig crc = ability.Get().GetComponent<ContextRankConfig>();
                        if (isConfigClassBased(crc))
                        {
                            if (!crc.m_FeatureList.Contains(feat))
                            {
                                crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
                            }
                        }
                        PatchBuff(ability);
                    }
                }
            }
            catch (Exception e) { Logger.Error("Failed to edit ability.", e); }
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
                if (crc.m_BaseValueType == ContextRankBaseValueType.ClassLevel) { return true; }
                //Logger.Info("start14");
                if (crc.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype) { return true; }
                if (crc.m_BaseValueType == ContextRankBaseValueType.OwnerSummClassLevelWithArchetype) { return true; }
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
        }
    }
}
