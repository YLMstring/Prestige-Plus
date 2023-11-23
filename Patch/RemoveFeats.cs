using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace PrestigePlus.Patch
{
    internal class RemoveFeats
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            Remove("c0d00f98-0a49-4088-8b5b-4da84b1274bc");//slam
        }

        private static void Remove(string guid)
        {
            var feat = BlueprintTool.GetRef<BlueprintFeatureReference>(guid)?.Get();
            if (feat != null)
            {
                feat.AddPrerequisiteFeature(FeatureRefs.MonsterMythicWeaponSizeFeature.Reference);
                feat.HideNotAvailibleInUI = true;
                //feat.HideInUI = true;
            } 
        }
    }
}
