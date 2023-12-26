using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class DirtyFighting
    {
        private const string DirtyFightingFeat = "Feat.DirtyFighting";
        public static readonly string DirtyFightingGuid = "{BA31F32D-B546-474B-884F-A5C61E0B80AF}";

        internal const string DirtyFightingDisplayName = "FeatDirtyFighting.Name";
        private const string DirtyFightingDescription = "FeatDirtyFighting.Description";
        public static void DirtyFightingConfigure()
        {
            var icon = FeatureRefs.DirtyTrickMythicFeat.Reference.Get().Icon;
            
            FeatureConfigurator.New(DirtyFightingFeat, DirtyFightingGuid, FeatureGroup.Feat)
              .SetDisplayName(DirtyFightingDisplayName)
              .SetDescription(DirtyFightingDescription)
              .SetIcon(icon)
              .AddComponent<DirtyFightingFlank>()
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }
    }
}
