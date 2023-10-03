using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class LesserSpellSynthesis
    {
        private static readonly string FeatName = "SpellSynthesis";
        private static readonly string FeatGuid = "{7DC2D7E4-D129-40C1-9153-1B273C062C60}";

        private static readonly string DisplayName = "SpellSynthesis.Name";
        private static readonly string Description = "SpellSynthesis.Description";

        private const string ReleaseAbility = "SpellSynthesis.ReleaseAbility";
        private static readonly string ReleaseAbilityGuid = "{5EB707B8-B8AF-47D6-BEA4-74CA2500FCBD}";

        private const string SpellSynthesisbuff = "SpellSynthesis.SpellSynthesisbuff";
        private static readonly string SpellSynthesisbuffGuid = "{71740950-0B75-4157-9B40-85B80703AA7B}";

        private const string SpellSynthesis2buff = "SpellSynthesis.SpellSynthesis2buff";
        private static readonly string SpellSynthesis2buffGuid = "{806AC2E8-5B0E-46BE-841F-FF280D72CEFD}";

        private const string SpellSynthesisRes = "SpellSynthesisRes";
        private static readonly string SpellSynthesisResGuid = "{A0A77ED0-E29B-4BFB-8DB9-B78CC85BDE6E}";
        public static void Configure()
        {
            var icon = FeatureRefs.SpellDanceFeature.Reference.Get().Icon;

            BlueprintAbilityResource Scarabilityresourse = AbilityResourceConfigurator.New(SpellSynthesisRes, SpellSynthesisResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var BuffSpellSynthesis = BuffConfigurator.New(SpellSynthesisbuff, SpellSynthesisbuffGuid)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<SynthesisComponent>()
              .Configure();

            var BuffSpellSynthesis2 = BuffConfigurator.New(SpellSynthesis2buff, SpellSynthesis2buffGuid)
              .SetIcon(icon)
              .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<SynthesisComponent>()
              .Configure();

            var feat = FeatureConfigurator.New(FeatName2, FeatGuid2, Kingmaker.Blueprints.Classes.FeatureGroup.MythicFeat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatGuid)
                    .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
                    .Configure();

            var ability = AbilityConfigurator.New(ReleaseAbility, ReleaseAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(BuffSpellSynthesis, ContextDuration.Fixed(1))
                    .ApplyBuff(BuffSpellSynthesis2, ContextDuration.Fixed(1))
                    .Conditional(ConditionsBuilder.New().CasterHasFact(feat).Build(), ifTrue:
                        ActionsBuilder.New().RestoreResource(SpellSynthesisResGuid, ContextValues.Constant(1)))
                    .Build())
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetIsFullRoundAction(true)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: SpellSynthesisResGuid)
                .Configure();

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.MysticTheurgeCombinedSpells1.ToString())
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: Scarabilityresourse, restoreAmount: true)
                    .Configure();
        }

        private static readonly string FeatName2 = "SpellSynthesis2";
        private static readonly string FeatGuid2 = "{4CB16192-C447-4500-A921-9A4964C47D19}";

        private static readonly string DisplayName2 = "SpellSynthesis2.Name";
        private static readonly string Description2 = "SpellSynthesis2.Description";
    }
}
