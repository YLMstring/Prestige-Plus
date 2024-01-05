using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Feats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.MythicFeat
{
    internal class BondGenie
    {
        private static readonly string FeatName = "BondGenie";
        private static readonly string FeatGuid = "{BDF243F1-A851-4C3F-9F6B-64E4848A7AE3}";

        private static readonly string AbilityName = "BondGenieAbility";
        private static readonly string AbilityGuid = "{9BCF737D-791A-437B-A529-56C658C4E519}";

        private static readonly string DisplayName = "BondGenie.Name";
        private static readonly string Description = "BondGenie.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.DruidNatureBond.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(AbilityName, AbilityGuid)
                .CopyFrom(AbilityRefs.MountTargetAbility,
                typeof(AbilityEffectRunAction),
                typeof(AbilityTargetIsSuitableMount),
                typeof(AbilityTargetIsSuitableMountSize),
                typeof(AbilityCasterNotPolymorphed))
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .AddAbilityCasterHasNoFacts(new() { BuffRefs.MountedBuff.ToString() })
                .SetHidden(false)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(Asavir.ShaitanGuid)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
