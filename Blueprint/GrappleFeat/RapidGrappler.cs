using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.GrappleFeat
{
    internal class RapidGrappler
    {
        private static readonly string FeatName = "RapidGrappler";
        public static readonly string FeatGuid = "{57525C0C-F78C-4946-83DE-BDE3272413D4}";

        private static readonly string DisplayName = "RapidGrappler.Name";
        private static readonly string Description = "RapidGrappler.Description";

        private const string RapidGrabAbility = "RapidGrappler.RapidGrabAbility";
        private static readonly string RapidGrabAbilityGuid = "{8EF784B4-B7C2-498A-9A0B-EECD0506D6D7}";

        private const string RapidGrabbuff = "RapidGrappler.RapidGrabbuff";
        private static readonly string RapidGrabbuffGuid = "{78A6C2A5-FFD0-4740-837A-ED69D9D8A030}";
        public static void Configure()
        {
            var icon = FeatureRefs.AgileManeuvers.Reference.Get().Icon;

            var BuffRapidGrab = BuffConfigurator.New(RapidGrabbuff, RapidGrabbuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(RapidGrabAbility, RapidGrabAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(BuffRapidGrab)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 9, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 9, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                    .AddPrerequisiteStatValue(StatType.Dexterity, 13)
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
                    .AddPrerequisiteFeature(GreaterGrapple.FeatGuid)
                    .AddFacts(new() { abilityTrick })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }
    }
}
