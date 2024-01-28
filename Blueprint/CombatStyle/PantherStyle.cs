using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.ActivatableAbilities;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.CombatStyle
{
    internal class PantherStyle
    {
        private static readonly string StyleName = "PantherStyle";
        public static readonly string StyleGuid = "{17D9E137-579A-44C9-8BCE-436AC8AF5F5B}";

        private static readonly string StyleDisplayName = "PantherStyle.Name";
        private static readonly string StyleDescription = "PantherStyle.Description";

        private const string Stylebuff = "PantherStyle.Stylebuff";
        public static readonly string StylebuffGuid = "{BA145C1C-8A78-4204-9438-DC531A90B5AD}";

        private const string StyleActivatableAbility = "PantherStyle.StyleActivatableAbility";
        private static readonly string StyleActivatableAbilityGuid = "{39AE0805-D5DC-42D3-A182-1855AF58DDC7}";

        public static void StyleConfigure()
        {
            var icon = FeatureRefs.ShiftersRushFeature.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                .Add<PantherAttack>()
                .Build();

            var action2 = ActionsBuilder.New()
                .Add<PantherAttack>(c => { c.isparry = true; })
                .Build();

            var Buff = BuffConfigurator.New(Stylebuff, StylebuffGuid)
              .SetDisplayName(StyleDisplayName)
              .SetDescription(StyleDescription)
              .SetIcon(icon)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action, onAttackOfOpportunity: true, waitForAttackResolve: true, onlyHit: false)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action2, onAttackOfOpportunity: true, triggerBeforeAttack: true, onlyHit: false)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StyleActivatableAbility, StyleActivatableAbilityGuid)
                .SetDisplayName(StyleDisplayName)
                .SetDescription(StyleDescription)
                .SetIcon(icon)
                .SetBuff(Buff)
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
                    .AddPrerequisiteStatValue(StatType.Wisdom, 13)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddFacts(new() { ability })
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string ParryName = "PantherParry";
        public static readonly string ParryGuid = "{CC6C387D-55DA-4D51-8501-A279EE943E7B}";

        private static readonly string ParryDisplayName = "PantherParry.Name";
        private static readonly string ParryDescription = "PantherParry.Description";

        private const string Stylebuff3 = "PantherStyle.Stylebuff3";
        public static readonly string StylebuffGuid3 = "{B1471E2B-0946-42D8-A562-07FB797260BE}";

        public static void ParryConfigure()
        {
            var icon = FeatureRefs.ShifterACBonusUnlock.Reference.Get().Icon;

            BuffConfigurator.New(Stylebuff3, StylebuffGuid3)
              .SetDisplayName(ParryDisplayName)
              .SetDescription(ParryDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(-2, descriptor: ModifierDescriptor.Penalty)
              .AddDamageBonusConditional(-2, descriptor: ModifierDescriptor.Penalty, onlyWeaponDamage: true)
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveBuff(StylebuffGuid3, toCaster: false).Build(), actionsOnInitiator: true, onlyHit: false)
              .Configure();

            FeatureConfigurator.New(ParryName, ParryGuid, FeatureGroup.Feat)
                    .SetDisplayName(ParryDisplayName)
                    .SetDescription(ParryDescription)
                    .SetIcon(icon)
                    .AddComponent<PantherKillAttacker>()
                    .AddPrerequisiteStatValue(StatType.Wisdom, 15)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddPrerequisiteFeature(ClawGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();
        }

        private static readonly string ClawName = "PantherClaw";
        public static readonly string ClawGuid = "{379AE1CF-1747-4503-8F24-544ECFC5B026}";

        private static readonly string ClawDisplayName = "PantherClaw.Name";
        private static readonly string ClawDescription = "PantherClaw.Description";

        private const string Stylebuff2 = "PantherStyle.Stylebuff2";
        public static readonly string StylebuffGuid2 = "{23636B2C-6C2E-4A3D-BAD6-C0488354992B}";
        public static void ClawConfigure()
        {
            var icon = FeatureRefs.ShifterClawsFeatureAddLevel.Reference.Get().Icon;

            BuffConfigurator.New(Stylebuff2, StylebuffGuid2)
              .SetDisplayName(ClawDisplayName)
              .SetDescription(ClawDescription)
              .SetIcon(icon)
              .SetRanks(20)
              .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Rank)
              .Configure();

            FeatureConfigurator.New(ClawName, ClawGuid, FeatureGroup.Feat)
                    .SetDisplayName(ClawDisplayName)
                    .SetDescription(ClawDescription)
                    .SetIcon(icon)
                    .AddPrerequisiteStatValue(StatType.Wisdom, 15)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
                    .AddPrerequisiteFeature(StyleGuid)
                    .AddToGroups(FeatureGroup.CombatFeat)
                    .AddToGroups(FeatureGroup.StyleFeat)
                    .Configure();

        }
    }
}
