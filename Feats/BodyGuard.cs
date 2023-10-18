using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class BodyGuard
    {
        private static readonly string FeatName = "FeatBodyGuard";
        private static readonly string FeatGuid = "{E1C5FEBC-7D2C-445D-A452-8A77C340E14F}";

        private static readonly string DisplayName = "FeatBodyGuard.Name";
        private static readonly string Description = "FeatBodyGuard.Description";

        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianBodyguardAbility.ToString() })
                    .AddAbilityResources(resource: AbilityResourceRefs.DivineGuardianTrothResource.ToString(), restoreAmount: true)
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string Feat2Name = "FeatHarmWay";
        private static readonly string Feat2Guid = "{F0611E7B-A68C-49E4-8BEC-97A1173AD609}";

        private static readonly string DisplayName2 = "FeatHarmWay.Name";
        private static readonly string Description2 = "FeatHarmWay.Description";

        public static void Configure2()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatGuid)
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.ToString() })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
