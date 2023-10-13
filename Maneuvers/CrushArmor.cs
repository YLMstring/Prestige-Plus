using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    internal class CrushArmor
    {
        private static readonly string FeatName = "WeaponTrickCrushArmor";
        private static readonly string FeatGuid = "{F28922F8-D1CE-4635-995C-6E5FB0595037}";

        private static readonly string DisplayName = "WeaponTrickCrushArmor.Name";
        private static readonly string Description = "WeaponTrickCrushArmor.Description";

        public static void Configure()
        {
            var icon = FeatureRefs.PummelingStyle.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 1)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedSunder.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.PowerAttackFeature.ToString())
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .AddManeuverTrigger(ActionsBuilder.New().ApplyBuff(BuffRefs.Fatigued.ToString(), ContextDuration.Fixed(1)).Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.SunderArmor, true)
                    .Configure();
        }
    }
}
