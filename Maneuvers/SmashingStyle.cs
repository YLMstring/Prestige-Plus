using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class SmashingStyle
    {
        private static readonly string StyleName = "SmashingStyle";
        public static readonly string StyleGuid = "{E3C2AB0C-3B88-4535-99A1-7983D56EF17E}";

        private static readonly string StyleDisplayName = "SmashingStyle.Name";
        private static readonly string StyleDescription = "SmashingStyle.Description";

        private const string Stylebuff = "SmashingStyle.Stylebuff";
        private static readonly string StylebuffGuid = "{E6CE860E-DC12-4C6F-93B2-EDD92E950CA7}";

        private const string StyleActivatableAbility = "SmashingStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{86F54DEC-5E97-4A25-80CF-EFCF349476C6}";

        private const string Stylebuff2 = "SmashingStyle.Stylebuff2";
        private static readonly string StylebuffGuid2 = "{7AC25C26-E732-4F83-A31D-747CC302912E}";

        private const string StyleActivatableAbility2 = "SmashingStyle.StyleActivatableAbility2";
        private static readonly string StyleActivatableAbilityGuid2 = "{ABA76A50-74F3-4C66-BD37-0631A63437A9}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.KiDiamondBodyFeature.Reference.Get().Icon;

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<SmashingComponent>(c => { c.effect = Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush; })
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(FeatureRefs.KiDiamondSoulFeature.Reference.Get().Icon)
                .SetBuff(Buff)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.CombatStyle)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(true)
                .Configure();

            var Buff2 = BuffConfigurator.New(Stylebuff2, StylebuffGuid2)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddComponent<SmashingComponent>(c => { c.effect = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip; })
              .Configure();

            var ability2 = ActivatableAbilityConfigurator.New(StyleActivatableAbility2, StyleActivatableAbilityGuid2)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(FeatureRefs.KiDiamondBodyFeature.Reference.Get().Icon)
                .SetBuff(Buff2)
                .SetDeactivateImmediately(true)
                .SetActivationType(AbilityActivationType.Immediately)
                .SetGroup(ActivatableAbilityGroup.CombatStyle)
                .SetWeightInGroup(1)
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Strength, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedSunder.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString())
                    .AddFacts(new() { ability, ability2 })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }
    }
}
