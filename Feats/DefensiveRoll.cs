using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.BasicEx;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
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
using PrestigePlus.Modify;
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

        private const string DefensiveRollAbility = "DefensiveRoll.DefensiveRollAbility";
        private static readonly string DefensiveRollAbilityGuid = "{8CE3C5BA-3FCB-43B7-A90A-D89F0EEA9A21}";

        private const string DefensiveRollAblityRes = "DefensiveRollAblityRes";
        private static readonly string DefensiveRollAblityResGuid = "{D6CD7849-DF81-4528-90C8-A78350CAEBC1}";
        public static void Configure()
        {
            var icon = FeatureRefs.Evasion.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(DefensiveRollBuff, DefensiveRollGuidBuff)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<RogueAnotherDay>()
              .Configure();

            BlueprintAbilityResource Scarabilityresourse = AbilityResourceConfigurator.New(DefensiveRollAblityRes, DefensiveRollAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(DefensiveRollAbility, DefensiveRollAbilityGuid)
                .SetDisplayName(DisplayName)
                .SetDescription(Description)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateImmediately()
                .AddActivatableAbilityResourceLogic(requiredResource: Scarabilityresourse, spendType: Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetIsOnByDefault(true)
                .Configure();

                FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.RogueTalent)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.AdvanceTalents.ToString())
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: Scarabilityresourse, restoreAmount: true)
                    .Configure();
        }
    }
}
