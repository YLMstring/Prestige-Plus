using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchDomain
    {
        public static void PatchDomains(string fref)
        {
            ProgressionConfigurator.For(fref)
                .AddToFeaturesRankIncrease(AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .Configure(delayed: true);
        }

        public static void Patch()
        {
            var list = FeatureSelectionRefs.DomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list)
            {
                PatchDomains(domain.Guid.ToString());
            }
            var list2 = FeatureSelectionRefs.SecondDomainsSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list2)
            {
                PatchDomains(domain.Guid.ToString());
            }
            var list3 = FeatureSelectionRefs.DruidDomainSelection.Reference.Get().m_AllFeatures;
            foreach (var domain in list3)
            {
                PatchDomains(domain.Guid.ToString());
            }
        }
    }
}
