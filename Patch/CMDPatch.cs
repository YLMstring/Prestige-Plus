using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class CMDPatch
    {
        public static void Patch()
        {
            FeatureConfigurator.For(FeatureRefs.DifficultOpponent)
                .AddComponent<DifficultCMDPatch>()
                .Configure();
        }

        public static void Patch2()
        {
            FeatureConfigurator.For(FeatureRefs.SkillAbilities)
                .AddComponent<DifficultCMDPatch2>()
                .Configure();
        }
    }
}
