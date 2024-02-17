using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.EntitySystem.Entities;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PrerequisitePatch
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            List<BlueprintFeatureReference> list = new() { };
            var feats = FeatureSelectionRefs.BasicFeatSelection.Reference.Get();
            var mabilities = FeatureSelectionRefs.MythicAbilitySelection.Reference.Get();
            var mfeats = FeatureSelectionRefs.MythicFeatSelection.Reference.Get();
            var obedience = BlueprintTool.GetRef<BlueprintFeatureSelectionReference>(DeificObedience.DeificObedienceGuid)?.Get()?.m_AllFeatures;
            if (obedience != null)
            {
                foreach (var feat in obedience)
                {
                    list.Add(feat);
                }
            }
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
                try
                {
                    foreach (var prerequisite in feat.Get().GetComponents<PrerequisiteFeature>())
                    {
                        var pre = prerequisite?.Feature;
                        if (pre != null)
                        {
                            pre.IsPrerequisiteFor ??= new List<BlueprintFeatureReference>();
                            if (!pre.IsPrerequisiteFor.Contains(feat))
                            {
                                pre.IsPrerequisiteFor.Add(feat);
                            }
                        }
                    }
                }
                catch (Exception e) { Logger.Error("Failed to edit " + feat.NameSafe(), e); }
            }
            var awt = FeatureSelectionRefs.AdvancedWeaponTraining1.Reference.Get().m_AllFeatures;
            FeatureSelectionRefs.AdvancedWeaponTraining2.Reference.Get().m_AllFeatures = awt;
            FeatureSelectionRefs.AdvancedWeaponTraining3.Reference.Get().m_AllFeatures = awt;
            FeatureSelectionRefs.AdvancedWeaponTraining4.Reference.Get().m_AllFeatures = awt;
        }
    }
}
