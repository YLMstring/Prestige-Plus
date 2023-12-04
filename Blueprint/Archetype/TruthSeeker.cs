using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class TruthSeeker
    {
        private const string ArchetypeName = "TruthSeeker";
        private static readonly string ArchetypeGuid = "{DFFACD98-DF21-4B14-BC3D-F9BD4FE8865E}";
        internal const string ArchetypeDisplayName = "TruthSeeker.Name";
        private const string ArchetypeDescription = "TruthSeeker.Description";
        public static void Configure()
        {

            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.OracleClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(3, FeatureSelectionRefs.OracleRevelationSelection.ToString())
            .SetRemoveFeaturesEntry(15, FeatureSelectionRefs.OracleRevelationSelection.ToString())
            .AddToAddFeatures(1, FeatureRefs.SeekerTrapfinding.ToString())
            .AddToAddFeatures(3, CreateSeekerLore())
            .AddToAddFeatures(15, CreateSeekerMagic())
              .Configure();
        }

        private const string SeekerLore = "TruthSeeker.SeekerLore";
        private static readonly string SeekerLoreGuid = "{7F845B07-487C-401D-81F5-F42F3620CC63}";

        internal const string SeekerLoreDisplayName = "TruthSeekerSeekerLore.Name";
        private const string SeekerLoreDescription = "TruthSeekerSeekerLore.Description";

        private static BlueprintFeature CreateSeekerLore()
        {
            var icon = FeatureRefs.BardLoreMaster.Reference.Get().Icon;

            return FeatureConfigurator.New(SeekerLore, SeekerLoreGuid)
              .SetDisplayName(SeekerLoreDisplayName)
              .SetDescription(SeekerLoreDescription)
              .SetIcon(icon)
              .AddCasterLevelForSpellbook()
              .Configure();
        }

        private const string SeekerMagic = "TruthSeeker.SeekerMagic";
        private static readonly string SeekerMagicGuid = "{C90C0BED-284C-4566-A6F1-683F7D9BF5C9}";

        internal const string SeekerMagicDisplayName = "TruthSeekerSeekerMagic.Name";
        private const string SeekerMagicDescription = "TruthSeekerSeekerMagic.Description";

        private static BlueprintFeature CreateSeekerMagic()
        {
            var icon = FeatureRefs.BardLoreMaster.Reference.Get().Icon;

            return FeatureConfigurator.New(SeekerMagic, SeekerMagicGuid)
              .SetDisplayName(SeekerMagicDisplayName)
              .SetDescription(SeekerMagicDescription)
              .SetIcon(icon)

              .Configure();
        }
    }
}
