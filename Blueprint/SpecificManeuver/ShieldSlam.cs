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
    internal class ShieldSlam
    {
        private static readonly string FeatName = "ShieldSlamFeat";
        private static readonly string FeatBuffName = "ShieldSlamFeatBuff";
        private static readonly string FeatAbilityName = "ShieldSlamFeatAbility";

        private static readonly string FeatGuid = "{4ED73B13-9F74-44D9-BC82-27AB8660A8AF}";
        private static readonly string FeatBuffGuid = "{FE2B3014-3809-4028-8B7B-C10DDF4747E3}";
        private static readonly string FeatAbilityGuid = "{A32FE799-07FA-416B-B655-6DD1059FFCFA}";

        internal const string DisplayName = "ShieldSlam.Name";
        private static readonly string Description = "ShieldSlam.Description";
        private static readonly Sprite Icon = FeatureRefs.ShieldBashFeature.Reference.Get().Icon;

        public static void Configure()
        {
            var buff =
                BuffConfigurator.New(FeatBuffName, FeatBuffGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(Icon)
                    .AddComponent<ShieldSnagComponent>(c => { c.type = Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush; })
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
