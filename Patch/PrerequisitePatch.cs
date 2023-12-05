using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PrerequisitePatch
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            List<BlueprintFeatureReference> list = new() { };
            var feats = FeatureSelectionRefs.BasicFeatSelection.Reference.Get();
            var mabilities = FeatureSelectionRefs.MythicAbilitySelection.Reference.Get();
            var mfeats = FeatureSelectionRefs.MythicFeatSelection.Reference.Get();
            foreach (var feat in feats.m_Features)
            {
                list.Add(feat);
            }
            foreach (var feat in mabilities.m_Features)
            {
                list.Add(feat);
            }
            foreach (var feat in mfeats.m_Features)
            {
                list.Add(feat);
            }
            foreach (var feat in list)
            {
                foreach (var prerequisite in feat.Get().GetComponents<PrerequisiteFeature>())
                {
                    var pre = prerequisite?.Feature;
                    if (pre != null)
                    {
                        pre.IsPrerequisiteFor ??= new List<BlueprintFeatureReference>();
                    }
                    if (!pre.IsPrerequisiteFor.Contains(feat))
                    {
                        pre.IsPrerequisiteFor.Add(feat);
                    }
                }
            }
        }
    }
}
