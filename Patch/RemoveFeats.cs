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
            Remove("3ddce7711accaf144b33e9a6949bb61e");//hand
            Remove("0408028da24dd0348848f00c7134d211");//mythic hand
            Remove("2f2eb97cc7e04d72b1a71794cd741412");//dirty fighting
            Remove("1a0da3ae8cbe41e44bc4fa55c06ba7c1");//strike true
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
