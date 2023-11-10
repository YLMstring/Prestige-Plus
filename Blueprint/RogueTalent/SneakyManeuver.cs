using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.Feat;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.RogueTalent
{
    internal class SneakyManeuver
    {
        private const string SneakyManeuverPower = "Rogue.SneakyManeuver";
        private static readonly string SneakyManeuverGuid = "{73359324-59F2-4256-8B9E-5B6214CBB001}";

        internal const string SneakyManeuverDisplayName = "RogueSneakyManeuver.Name";
        private const string SneakyManeuverDescription = "RogueSneakyManeuver.Description";

        private const string SneakyManeuverAbility = "Rogue.SneakyManeuverAbility";
        public static readonly string SneakyManeuverAbilityGuid = "{C5F2E13D-D25F-44AE-8A52-08EAA304F8D4}";

        private const string SneakyManeuverBuff2 = "Rogue.SneakyManeuverBuff2";
        public static readonly string SneakyManeuverBuff2Guid = "{5026607E-DC41-401D-89B0-AD95A623FB3F}";

        private const string SneakyManeuverBuff = "Rogue.SneakyManeuverBuff";
        public static readonly string SneakyManeuverBuffGuid = "{E5CB60EA-3C4A-421C-8321-CD093EDA73DD}";

        public static BlueprintFeature SneakyManeuverFeat()
        {
            var icon = ActivatableAbilityRefs.DebilitatingInjuryBewilderedAbility.Reference.Get().Icon;

            var buff = BuffConfigurator.New(SneakyManeuverBuff, SneakyManeuverBuffGuid)
              .SetDisplayName(SneakyManeuverDisplayName)
              .SetDescription(SneakyManeuverDescription)
              .SetIcon(icon)
              .AddComponent<SneakyManeuverComponent>()
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var abilityTrick = ActivatableAbilityConfigurator.New(SneakyManeuverAbility, SneakyManeuverAbilityGuid)
                .SetDisplayName(SneakyManeuverDisplayName)
                .SetDescription(SneakyManeuverDescription)
                .SetIcon(icon)
                .SetBuff(buff)
                .SetDeactivateImmediately()
                .SetIsOnByDefault(true)
                .Configure();

            BuffConfigurator.New(SneakyManeuverBuff2, SneakyManeuverBuff2Guid)
              .SetDisplayName(SneakyManeuverDisplayName)
              .SetDescription(SneakyManeuverDescription)
              .SetIcon(icon)
              //.SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddAutoMetamagic(new() { AbilityRefs.DirtyTrickBlindnessAction.ToString(), AbilityRefs.DirtyTrickEntangleAction.ToString(), AbilityRefs.DirtyTrickSickenedAction.ToString(), AbilityRefs.DisarmAction.ToString(), AbilityRefs.SunderAction.ToString(), AbilityRefs.TripAction.ToString() }, 
              metamagic: Metamagic.Quicken, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: true)
              //.AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().RemoveSelf().Build())
              .Configure();

            return FeatureConfigurator.New(SneakyManeuverPower, SneakyManeuverGuid)
              .SetDisplayName(SneakyManeuverDisplayName)
              .SetDescription(SneakyManeuverDescription)
              .SetIcon(icon)
              .AddPrerequisiteFullStatValue(stat: Kingmaker.EntitySystem.Stats.StatType.SneakAttack, value: 1)
              .AddFacts(new() { abilityTrick })
              .AddToGroups(FeatureGroup.RogueTalent)
              .Configure();
        }
    }
}
