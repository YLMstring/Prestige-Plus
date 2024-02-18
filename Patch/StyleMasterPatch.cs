using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Feat;
using HarmonyLib;
using PrestigePlus.Blueprint.Archetype;

namespace PrestigePlus.Patch
{
    internal class StyleMasterPatch
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            var target = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(BodyGuard.StyleMasterGuid)?.Get();
            var target2 = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(ManyMonk.ManyFeat2Guid)?.Get();
            if (target == null) { return; }
            target.m_AllFeatures ??= new BlueprintFeatureReference[] { };
            var feats = FeatureSelectionRefs.FighterFeatSelection.Reference.Get();
            foreach (var feat in feats.m_AllFeatures)
            {
                if (feat.NameSafe().Contains("Style"))
                {
                    if (feat == FeatureRefs.CraneStyleRiposteFeat.Reference) continue;
                    if (feat == FeatureRefs.CraneStyleWingFeat.Reference) continue;
                    target.m_AllFeatures = CommonTool.Append(target.m_AllFeatures, feat);
                    if (target2 != null && feat.Get().IsPrerequisiteFor?.Count() > 0)
                    {
                        foreach (var feat2 in feat.Get().IsPrerequisiteFor)
                        {
                            if (target2.m_AllFeatures.Contains(feat2)) continue;
                            target2.m_AllFeatures = CommonTool.Append(target2.m_AllFeatures, feat2);
                            if (feat2.Get().IsPrerequisiteFor?.Count() > 0)
                            {
                                foreach (var feat3 in feat.Get().IsPrerequisiteFor)
                                {
                                    if (target2.m_AllFeatures.Contains(feat3)) continue;
                                    target2.m_AllFeatures = CommonTool.Append(target2.m_AllFeatures, feat3);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
