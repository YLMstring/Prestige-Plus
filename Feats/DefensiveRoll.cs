using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Feats
{
    internal class DefensiveRoll
    {
        private static readonly string FeatName = "DefensiveRoll";
        private static readonly string FeatGuid = "3967B46C-99CA-41E9-B7A2-96644EED8C96";

        private static readonly string DisplayName = "DefensiveRoll.Name";
        private static readonly string Description = "DefensiveRoll.Description";

        private const string DefensiveRollBuff = "DefensiveRollBuff";
        private static readonly string DefensiveRollGuidBuff = "A812D3F2-BBF8-4EF1-A627-9FBFCAC810F8";

        private const string DefensiveRollBuff2 = "DefensiveRollBuff2";
        private static readonly string DefensiveRollGuidBuff2 = "D813AAE6-03A9-4122-BF75-280666EF3587";

        private const string DefensiveRollBuff3 = "DefensiveRollBuff3";
        private static readonly string DefensiveRollGuidBuff3 = "1A59633B-FE44-4B74-8ABD-41C33BD5EF19";

        public static void Configure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;
            var icon2 = FeatureRefs.UncannyDodge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(DefensiveRollBuff, DefensiveRollGuidBuff)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddUnitHealthGuard(0)
              .AddBuffOnHealthTickingTrigger((float)0.05, DefensiveRollGuidBuff2)
              .Configure();

            var Buff3 = BuffConfigurator.New(DefensiveRollBuff3, DefensiveRollGuidBuff3)
              .CopyFrom(
                BuffRefs.CloakofDreamsEffectBuff,
                typeof(AddIncomingDamageTrigger),
                typeof(RemoveWhenCombatEnded))
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon2)
              .AddUnitHealthGuard(0)
              .AddBuffActions(deactivated: ActionsBuilder.New()
                .Conditional(conditions: ConditionsBuilder.New().IsFlatFooted(), ifFalse: ActionsBuilder.New()
                    .HealTarget(ContextDice.Value(DiceType.One))
                    .Build())
                .Build())
              .Configure();

            var Buff2 = BuffConfigurator.New(DefensiveRollBuff2, DefensiveRollGuidBuff2)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon2)
              .AddBuffActions(activated: ActionsBuilder.New()
                .RemoveSelf() 
                .ApplyBuff(buff: Buff3, durationValue: ContextDuration.Fixed(1))
                .RemoveBuff(Buff1)
                .Build())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.RogueTalentSelection)
                .AddToAllFeatures(FeatureConfigurator.New(FeatName, FeatGuid)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.AdvanceTalents.ToString())
                    .AddRestTrigger(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build())
                    .AddFacts(new() { Buff1 })
                    .Configure())
                .Configure(delayed: true);
        }
    }
}
