using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.Grapple;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class DivineDefiance
    {
        private static readonly string StyleName = "DivineDefianceStyle";
        public static readonly string StyleGuid = "{5E3685A7-AABA-45CF-9567-547FABD12E4B}";

        private static readonly string StyleDisplayName = "DivineDefianceStyle.Name";
        private static readonly string StyleDescription = "DivineDefianceStyle.Description";

        public static void StyleConfigure()
        {
            var icon = FeatureRefs.AtheismFeature.Reference.Get().Icon;

            FeatureConfigurator.New(StyleName, StyleGuid, FeatureGroup.Feat)
                    .SetDisplayName(StyleDisplayName)
                    .SetDescription(StyleDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.AtheismFeature.ToString())
                    .AddComponent<DivineDefianceBonus>()
                    .Configure();

        }

        private static readonly string IconoclastName = "DivineDefianceIconoclast";
        public static readonly string IconoclastGuid = "{7BFC6219-12DF-4E58-87E8-7C3A4626C9CD}";

        private static readonly string IconoclastDisplayName = "DivineDefianceIconoclast.Name";
        private static readonly string IconoclastDescription = "DivineDefianceIconoclast.Description";

        public static void IconoclastConfigure()
        {
            var icon = FeatureRefs.AtheismFeature.Reference.Get().Icon;

            var action = ActionsBuilder.New().Add<ContextActionIconoclast>().Build();

            FeatureConfigurator.New(IconoclastName, IconoclastGuid, FeatureGroup.Feat)
                    .SetDisplayName(IconoclastDisplayName)
                    .SetDescription(IconoclastDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedSunder.ToString())
                    .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddManeuverTrigger(action, Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, onlySuccess: true)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .Configure();

        }
    }
}
