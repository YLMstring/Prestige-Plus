using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class TruthSooker
    {
        private const string ArchetypeName = "TruthSooker";
        private static readonly string ArchetypeGuid = "{A986BF3A-26BB-4F48-B667-23A0B6073A3A}";
        internal const string ArchetypeDisplayName = "TruthSooker.Name";
        private const string ArchetypeDescription = "TruthSooker.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.SorcererClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.SorcererBloodlineSelection.ToString())
            .AddToAddFeatures(1, FeatureSelectionRefs.SeekerBloodlineSelection.ToString())
            .AddToAddFeatures(3, CreateSookerLore())
            .AddToAddFeatures(15, CreateSookerMagic())
              .Configure();
        }

        private const string SookerLore = "TruthSooker.SookerLore";
        private static readonly string SookerLoreGuid = "{C98695EC-2BD7-482C-B176-F5C37588741B}";

        internal const string SookerLoreDisplayName = "TruthSookerSookerLore.Name";
        private const string SookerLoreDescription = "TruthSookerSookerLore.Description";

        private static BlueprintFeature CreateSookerLore()
        {
            var icon = FeatureSelectionRefs.ExtraDiscoverySelection.Reference.Get().Icon;

            return FeatureConfigurator.New(SookerLore, SookerLoreGuid)
              .SetDisplayName(SookerLoreDisplayName)
              .SetDescription(SookerLoreDescription)
              .SetIcon(icon)
              .AddComponent<TruthSookerLore>()
              .Configure();
        }

        private const string SookerMagic = "TruthSooker.SookerMagic";
        private static readonly string SookerMagicGuid = "{94F50C26-589A-481F-88B4-573707E09DE0}";

        internal const string SookerMagicDisplayName = "TruthSookerSookerMagic.Name";
        private const string SookerMagicDescription = "TruthSookerSookerMagic.Description";

        private static BlueprintFeature CreateSookerMagic()
        {
            var icon = FeatureRefs.SelectiveSpellFeat.Reference.Get().Icon;

            return FeatureConfigurator.New(SookerMagic, SookerMagicGuid)
              .SetDisplayName(SookerMagicDisplayName)
              .SetDescription(SookerMagicDescription)
              .SetIcon(icon)
              .AddComponent<TruthSookerMagic>()
              .Configure();
        }
    }
}
