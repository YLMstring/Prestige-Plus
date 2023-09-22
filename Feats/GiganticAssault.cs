using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class GiganticAssault
    {
        private static readonly string FeatName = "GiganticAssault";
        private static readonly string FeatGuid = "{D47DC15C-3A96-4358-A652-DB9E632009A7}";

        private static readonly string DisplayName = "GiganticAssault.Name";
        private static readonly string Description = "GiganticAssault.Description";

        private const string ReleaseAbility = "GiganticAssault.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{0A583282-B9AD-45A1-BE18-328F79D92C69}";

        private const string ReleaseAbilitybuff = "GiganticAssault.ReleaseAbilitybuff";
        private static readonly string ReleaseAbilitybuffGuid = "{747F27A3-58AD-496D-9E46-D24FE93B71AA}";
        public static void Configure()
        {
            var icon = FeatureRefs.ArmyChargeAbilityFeature.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(ReleaseAbilitybuff, ReleaseAbilitybuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.MythicAbility)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteClassLevel(MammothRider.ArchetypeGuid, 8)
                    //.AddFacts(new() { ability })
                    .Configure();
        }
    }
}
}
