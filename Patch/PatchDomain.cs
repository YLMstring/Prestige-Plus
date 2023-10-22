using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchDomain
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void PatchDomains(string fref)
        {
            ProgressionConfigurator.For(fref)
                .AddToFeaturesRankIncrease(AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .Configure(delayed: true);

            var progress = BlueprintTool.GetRef<BlueprintProgressionReference>(fref).Get();
            var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(AnchoriteofDawn.AnchoriteDomainPlusfeatGuid);

            foreach (LevelEntry entry in progress.LevelEntries)
            {
                foreach (BlueprintFeatureBase feature in entry.Features)
                {
                    IEnumerable<ContextRankConfig> crcs = feature.GetComponents<ContextRankConfig>();
                    if (crcs.Any())
                    {
                        foreach (ContextRankConfig crc in crcs)
                        {
                            if (isConfigClassBased(crc))
                            {
                                if (!crc.m_FeatureList.Contains(feat))
                                {
                                    crc.m_FeatureList = crc.m_FeatureList.AddToArray(feat);
                                }
                            }
                        }
                    }

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
                    }
                }
            }
        }

        private static bool isConfigClassBased(ContextRankConfig crc)
        {
            if (crc == null || crc.m_FeatureList == null || crc.m_FeatureList.Length == 0) { return false; }
            if (crc.m_BaseValueType == ContextRankBaseValueType.ClassLevel) { return true; }
            if (crc.m_BaseValueType == ContextRankBaseValueType.MaxClassLevelWithArchetype) { return true; }
            if (crc.m_BaseValueType == ContextRankBaseValueType.OwnerSummClassLevelWithArchetype) { return true; }
            if (crc.m_BaseValueType == ContextRankBaseValueType.SummClassLevelWithArchetype) { return true; }
            return false;
        }

        public static void Patch()
        {
            var list = FeatureSelectionRefs.DomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list)
            {
                PatchDomains(domain.Guid.ToString());
            }
            var list2 = FeatureSelectionRefs.SecondDomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list2)
            {
                PatchDomains(domain.Guid.ToString());
            }
            var list3 = FeatureSelectionRefs.DruidDomainSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list3)
            {
                PatchDomains(domain.Guid.ToString());
            }
        }
    }
}
