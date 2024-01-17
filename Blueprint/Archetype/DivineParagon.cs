using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class DivineParagon
    {
        private const string ArchetypeName = "DivineParagon";
        private static readonly string ArchetypeGuid = "{3348D94A-0FF4-4B48-BEB5-E6740F3AF2CF}";
        internal const string ArchetypeDisplayName = "DivineParagon.Name";
        private const string ArchetypeDescription = "DivineParagon.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.ClericClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .AddPrerequisiteNoFeature(DeificObedience.DeificObedienceGuid)
              .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.SecondDomainsSelection.ToString())
              .AddToAddFeatures(1, DeificObedience.DeificObedienceGuid, ExaltedEvangelist.BonusFeatGuid, Sentinel.BonusFeatGuid, FeatureSelectionRefs.SecondDomainsSelection.ToString(), CreateDevotedDomain())
              .AddToAddFeatures(5, Sentinel.DivineBoon1Guid)
              .AddToAddFeatures(11, Sentinel.DivineBoon2Guid)
              .AddToAddFeatures(14, Sentinel.DivineBoon3Guid)
              .Configure();
        }

        private const string DevotedDomain = "DivineParagon.DevotedDomain";
        private static readonly string DevotedDomainGuid = "{A8597331-3ABD-41E1-9A0D-816C79B42B78}";

        internal const string DevotedDomainDisplayName = "DivineParagonDevotedDomain.Name";
        private const string DevotedDomainDescription = "DivineParagonDevotedDomain.Description";

        private static BlueprintFeature CreateDevotedDomain()
        {
            var icon = AbilityRefs.RemoveParalysis.Reference.Get().Icon;

            return FeatureConfigurator.New(DevotedDomain, DevotedDomainGuid)
              .SetDisplayName(DevotedDomainDisplayName)
              .SetDescription(DevotedDomainDescription)
              .SetIcon(icon)
              .AddComponent<DevotedDomainSpell>()
              .Configure();
        }
    }
}
