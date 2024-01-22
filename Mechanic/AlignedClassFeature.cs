using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using PrestigePlus.CustomAction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Mechanic
{
    internal class AlignedClassFeature
    {
        private static readonly string StyleDisplayName = "AlignedClass.Name";
        private static readonly string StyleDescription = "AlignedClass.Description";

        private const string StyleAbility = "AlignedClass.StyleAbility";
        private static readonly string StyleAbilityGuid = "{E6210547-F73A-4590-8EA0-FF32A493C43A}";
        public static void StyleConfigure()
        {
            var icon = AbilityRefs.ArrowOfLaw.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(StyleAbility, StyleAbilityGuid)
                .CopyFrom(
                AbilityRefs.LayOnHandsSelf,
                typeof(AbilitySpawnFx))
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<AlignedClassClear>()
                    .Build())
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .AddAbilityCasterInCombat(true)
                .SetActionBarAutoFillIgnored(true)
                .AddHideFeatureInInspect()
                .Configure();

            FeatureConfigurator.For(FeatureRefs.SkillAbilities)
                    .AddFacts(new() { ability })
                    .Configure();
        }
    }
}
