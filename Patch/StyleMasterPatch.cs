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

namespace PrestigePlus.Patch
{
    internal class StyleMasterPatch
    {
        public static void Patch()
        {
            var target = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(BodyGuard.StyleMasterGuid)?.Get();
            if (target == null) { return; }
            target.m_Features ??= new BlueprintFeatureReference[] { };
            var feats = FeatureSelectionRefs.FighterFeatSelection.Reference.Get();
            foreach (var feat in feats.m_Features)
            {
                if (feat.Get().Groups.Contains(Kingmaker.Blueprints.Classes.FeatureGroup.StyleFeat))
                {
                    target.m_Features.AddToArray(feat);
                }
            }
        }
    }
}
