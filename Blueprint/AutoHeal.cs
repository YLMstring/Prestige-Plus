using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.CustomAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint
{
    internal class AutoHeal
    {
        private static readonly string StyleName = "AutoHealbuff";
        public static readonly string StyleGuid = "{46B4F85D-5E83-4383-A7E1-09275454EC44}";

        private static readonly string StyleDisplayName = "AutoHeal.Name";
        private static readonly string StyleDescription = "AutoHeal.Description";

        private const string StyleAbility = "AutoHeal.StyleAbility";
        private static readonly string StyleAbilityGuid = "{C02FA1FA-25CE-46FF-802A-21B9D7BDA125}";
        public static void StyleConfigure()
        {
            var icon = FeatureRefs.LayOnHandsFeature.Reference.Get().Icon;
            var icon2 = FeatureRefs.ChannelEnergyHospitalerFeature.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .CopyFrom(
                AbilityRefs.LayOnHandsSelf,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<SpendGoldHeal>()
                    .Build())
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .Configure();

            var ability2 = AbilityConfigurator.New(StyleName, StyleGuid)
                .CopyFrom(
                AbilityRefs.ChannelEnergyPaladinHeal,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<SpendGoldHeal>()
                    .Build())
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon2)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityTargetsAround(radius: 30.Feet(), targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.SkillAbilities)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();

        }
    }
}
