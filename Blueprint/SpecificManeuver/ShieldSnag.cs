using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.CustomComponent.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.Blueprint.SpecificManeuver
{
    internal class ShieldSnag
    {
        private static readonly string FeatName = "ShieldSnagFeat";
        private static readonly string FeatBuffName = "ShieldSnagFeatBuff";
        private static readonly string FeatAbilityName = "ShieldSnagFeatAbility";

        private static readonly string FeatGuid = "{1CDFAC84-CC97-4B02-B2ED-5E8E56448494}";
        private static readonly string FeatBuffGuid = "{54531F74-8C51-4F4E-BF76-F91867D60F00}";
        private static readonly string FeatAbilityGuid = "{4ECFD361-62C0-4757-B718-74D9DB08909E}";

        internal const string DisplayName = "ShieldSnag.Name";
        private static readonly string Description = "ShieldSnag.Description";
        private static readonly Sprite Icon = FeatureRefs.ShieldBashFeature.Reference.Get().Icon;

        public static void Configure()
        {
            var buff =
                BuffConfigurator.New(FeatBuffName, FeatBuffGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(Icon)
                    .AddComponent<ShieldSnagComponent>()
                    .Configure();

            var ability =
                ActivatableAbilityConfigurator.New(FeatAbilityName, FeatAbilityGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(Icon)
                    .SetDeactivateImmediately()
                    .SetBuff(buff)
                    .SetIsOnByDefault(true)
                    .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, FeatureGroup.CombatFeat, FeatureGroup.Feat, FeatureGroup.RangerStyle)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .AddFeatureTagsComponent(FeatureTag.Melee | FeatureTag.CombatManeuver)
                .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                .AddPrerequisiteFeature(FeatureRefs.ShieldBashFeature.ToString())
                .AddPrerequisiteFeature(FeatureRefs.ShieldsProficiency.ToString())
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
                .AddToRangerStyles(RangerStyle.Shield2)
                .AddFacts(new() { ability })
                .SetIcon(Icon)
                .Configure(delayed: true);
        }
    }
}
