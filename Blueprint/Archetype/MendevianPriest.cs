using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class MendevianPriest
    {
        private const string ArchetypeName = "MendevianPriest";
        private static readonly string ArchetypeGuid = "{A9175EE3-ADF9-49E8-A1B7-C1887A283A7C}";
        internal const string ArchetypeDisplayName = "MendevianPriest.Name";
        private const string ArchetypeDescription = "MendevianPriest.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.ClericClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.SecondDomainsSelection.ToString())
            .AddToAddFeatures(1, FeatureRefs.HeavyArmorProficiency.ToString())
            .AddToAddFeatures(4, CreateTeamwork())
            .AddToAddFeatures(8, TeamworkGuid)
              .Configure();
        }

        private const string Teamwork = "MendevianPriest.Teamwork";
        private static readonly string TeamworkGuid = "{72B09233-3C47-43B0-86E4-6C3F68F85697}";

        internal const string TeamworkDisplayName = "MendevianPriestTeamwork.Name";
        private const string TeamworkDescription = "MendevianPriestTeamwork.Description";

        private static BlueprintFeatureSelection CreateTeamwork()
        {
            var icon = FeatureRefs.TeamworkFeature.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(Teamwork, TeamworkGuid)
              .SetDisplayName(TeamworkDisplayName)
              .SetDescription(TeamworkDescription)
              .SetIcon(icon)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureSelectionRefs.TeamworkFeat.ToString())
              .AddToAllFeatures(FeatureRefs.SelectiveChannel.ToString())
              .AddToAllFeatures(FeatureRefs.SpellPenetration.ToString())
              .AddToAllFeatures(FeatureRefs.GreaterSpellPenetration.ToString())
              .Configure();
        }
    }
}
