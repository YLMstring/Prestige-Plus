using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Gunslinger
{
    internal class Maverick
    {
        private const string ArchetypeName = "Maverick";
        private static readonly string ArchetypeGuid = "{0F17D2E5-309B-4DA0-A82F-C478E147EB60}";
        internal const string ArchetypeDisplayName = "Maverick.Name";
        private const string ArchetypeDescription = "Maverick.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, GunslingerMain.ArchetypeGuid)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .SetRemoveFeaturesEntry(1, GunslingerMain.DodgeGuid)
              .SetRemoveFeaturesEntry(3, GunslingerMain.InitiativeGuid)
              .AddToAddFeatures(3, FeatureRefs.ImprovedUnarmedStrike.ToString(), FeatureRefs.DazzlingDisplayFeature.ToString())
              .Configure();
        }
    }
}
