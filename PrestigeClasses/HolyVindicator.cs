using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using PrestigePlus.CustomAction;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.PrestigeClasses
{
    internal class HolyVindicator
    {
        private const string DivineWrath = "HolyVindicator.DivineWrath";
        public static readonly string DivineWrathGuid = "{D4A9E0F0-B2FE-4847-AB03-C2525EC8DC35}";

        private const string DivineWrathAblity = "HolyVindicator.UseDivineWrath";
        private static readonly string DivineWrathAblityGuid = "{387C7F32-F0EC-424B-AC1F-54E0E04A29EA}";

        private const string DivineWrathAblity2 = "HolyVindicator.UseDivineWrath2";
        public static readonly string DivineWrathAblity2Guid = "{D8D10401-1BC9-4CB4-B234-E516E1D3B0A7}";

        private const string DivineWrathBuff = "HolyVindicator.DivineWrathBuff";
        private static readonly string DivineWrathBuffGuid = "{5114C167-87F7-47F5-A657-23088D98CDB9}";

        internal const string DivineWrathDisplayName = "HolyVindicatorDivineWrath.Name";
        private const string DivineWrathDescription = "HolyVindicatorDivineWrath.Description";

        private static readonly string holy = "b5daf66532f5425aa22df5372c57d766";
        public static BlueprintFeature DivineWrathFeat()
        {
            var icon = AbilityRefs.Doom.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(DivineWrathAblity2, DivineWrathAblity2Guid)
                .CopyFrom(
                AbilityRefs.Doom,
                typeof(AbilityEffectRunAction),
                typeof(AbilitySpawnFx),
                typeof(AbilityTargetHasNoFactUnless),
                typeof(AbilityTargetHasFact),
                typeof(SpellDescriptorComponent),
                typeof(SpellComponent))
                .Configure();

            var VicousSelf = ActionsBuilder.New()
                .CastSpell(ability2)
                .OnOwner(ActionsBuilder.New().RemoveSelf().Build())
                .Build();

            var Buff1 = BuffConfigurator.New(DivineWrathBuff, DivineWrathBuffGuid)
             .SetDisplayName(DivineWrathDisplayName)
             .SetDescription(DivineWrathDescription)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: VicousSelf, onlyHit: true, criticalHit: true)
             .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: VicousSelf, criticalHit: true, onlyHit: true)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            var ability = AbilityConfigurator.New(DivineWrathAblity, DivineWrathAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build())
                .SetDisplayName(DivineWrathDisplayName)
                .SetDescription(DivineWrathDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            return FeatureConfigurator.New(DivineWrath, DivineWrathGuid)
              .SetDisplayName(DivineWrathDisplayName)
              .SetDescription(DivineWrathDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holy, new() { null, ability, null, null, null, null, null, null, null, null })
              .AddComponent<HolyVindicatorSpellDC>()
              .Configure();
        }

        private const string DivineJudgment = "HolyVindicator.DivineJudgment";
        public static readonly string DivineJudgmentGuid = "{1F5AC055-8281-4035-8C47-64C966E5F2C1}";

        private const string DivineJudgmentAblity = "HolyVindicator.UseDivineJudgment";
        private static readonly string DivineJudgmentAblityGuid = "{168AF405-45C1-4C06-96DC-A329CC50D2A5}";

        private const string DivineJudgmentAblity2 = "HolyVindicator.UseDivineJudgment2";
        public static readonly string DivineJudgmentAblity2Guid = "{26A9D10F-15C9-47E2-833C-3065BF9B7EE4}";

        private const string DivineJudgmentBuff = "HolyVindicator.DivineJudgmentBuff";
        private static readonly string DivineJudgmentBuffGuid = "{A86CFD4E-B35A-427A-8104-967D7DCD3703}";

        private const string DivineJudgmentBuff2 = "HolyVindicator.DivineJudgmentBuff2";
        public static readonly string DivineJudgmentBuff2Guid = "{80D3D119-A6B2-44B8-A6B1-4E071A0250BE}";

        internal const string DivineJudgmentDisplayName = "HolyVindicatorDivineJudgment.Name";
        private const string DivineJudgmentDescription = "HolyVindicatorDivineJudgment.Description";

        public static BlueprintFeature DivineJudgmentFeat()
        {
            var icon = AbilityRefs.JudgementAuraAbility.Reference.Get().Icon;

            var knell = ActionsBuilder.New()
                .ConditionalSaved(failed: ActionsBuilder.New()
                    .Kill(Kingmaker.UnitLogic.UnitState.DismemberType.LimbsApart)
                    .Add<DeathKnellBuff>()
                    .Build())
                .Build();

            var ability2 = AbilityConfigurator.New(DivineJudgmentAblity2, DivineJudgmentAblity2Guid)
                .CopyFrom(
                AbilityRefs.BestowCurseDeterioration,
                typeof(AbilitySpawnFx),
                typeof(SpellComponent))
                .AddAbilityEffectRunAction(knell, savingThrowType: SavingThrowType.Will)
                .SetType(AbilityType.Spell)
                .SetDisplayName(DivineJudgmentDisplayName)
                .SetDescription(DivineJudgmentDescription)
                .SetIcon(icon)
                .SetCanTargetEnemies(true)
                .SetCanTargetSelf(false)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
                .SetRange(AbilityRange.Long)
                .Configure();

            var VicousSelf = ActionsBuilder.New()
                .CastSpell(ability2)
                .OnOwner(ActionsBuilder.New().RemoveSelf().Build())
                .Build();

            var Buff1 = BuffConfigurator.New(DivineJudgmentBuff, DivineJudgmentBuffGuid)
             .SetDisplayName(DivineJudgmentDisplayName)
             .SetDescription(DivineJudgmentDescription)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: VicousSelf, onlyHit: true, reduceHPToZero: true)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            BuffConfigurator.New(DivineJudgmentBuff2, DivineJudgmentBuff2Guid)
             .SetDisplayName(DivineJudgmentDisplayName)
             .SetDescription(DivineJudgmentDescription)
             .SetIcon(icon)
             .AddStatBonus(ModifierDescriptor.Enhancement, stat: StatType.Strength, value: 2)
             .AddTemporaryHitPointsRandom(0, dice: new DiceFormula(1, DiceType.D8))
             .AddIncreaseCasterLevel(value: ContextValues.Constant(1))
             .Configure();

            var ability = AbilityConfigurator.New(DivineJudgmentAblity, DivineJudgmentAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build())
                .SetDisplayName(DivineJudgmentDisplayName)
                .SetDescription(DivineJudgmentDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            return FeatureConfigurator.New(DivineJudgment, DivineJudgmentGuid)
              .SetDisplayName(DivineJudgmentDisplayName)
              .SetDescription(DivineJudgmentDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holy, new() { null, null, ability, null, null, null, null, null, null, null })
              .AddComponent<HolyVindicatorSpellDC>(c => { c.level = 12; c.ability = DivineJudgmentAblity2Guid; })
              .Configure();
        }

        private const string DivineRetribution = "HolyVindicator.DivineRetribution";
        public static readonly string DivineRetributionGuid = "{15F2E4FF-AD66-459D-B0A5-6E5713D5B57B}";

        private const string DivineRetributionAblity = "HolyVindicator.UseDivineRetribution";
        private static readonly string DivineRetributionAblityGuid = "{A0CC4724-84ED-4E27-89DD-FEC1ADDE49F9}";

        private const string DivineRetributionAblity2 = "HolyVindicator.UseDivineRetribution2";
        public static readonly string DivineRetributionAblity2Guid = "{5163A7C9-72C6-424E-936C-301CF35D9EFA}";

        private const string DivineRetributionBuff = "HolyVindicator.DivineRetributionBuff";
        private static readonly string DivineRetributionBuffGuid = "{69AC578A-FBE0-42F3-BC6E-53F7F845C445}";

        internal const string DivineRetributionDisplayName = "HolyVindicatorDivineRetribution.Name";
        private const string DivineRetributionDescription = "HolyVindicatorDivineRetribution.Description";

        public static BlueprintFeature DivineRetributionFeat()
        {
            var icon = AbilityRefs.BestowCurseDeterioration.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(DivineRetributionAblity2, DivineRetributionAblity2Guid)
                .CopyFrom(
                AbilityRefs.BestowCurseDeterioration,
                typeof(AbilityEffectRunAction),
                typeof(AbilitySpawnFx),
                typeof(SpellDescriptorComponent),
                typeof(SpellComponent))
                .SetRange(AbilityRange.Long)
                .Configure();

            var VicousSelf = ActionsBuilder.New()
                .CastSpell(ability2)
                .OnOwner(ActionsBuilder.New().RemoveSelf().Build())
                .Build();

            var Buff1 = BuffConfigurator.New(DivineRetributionBuff, DivineRetributionBuffGuid)
             .SetDisplayName(DivineRetributionDisplayName)
             .SetDescription(DivineRetributionDescription)
             .SetIcon(icon)
             .AddInitiatorAttackWithWeaponTrigger(action: VicousSelf, onlyHit: true, criticalHit: true)
             .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: VicousSelf, criticalHit: true, onlyHit: true)
             .AddRestTrigger(ActionsBuilder.New().RemoveSelf().Build())
             .Configure();

            var ability = AbilityConfigurator.New(DivineRetributionAblity, DivineRetributionAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuffPermanent(Buff1)
                        .Build())
                .SetDisplayName(DivineRetributionDisplayName)
                .SetDescription(DivineRetributionDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .Configure();

            return FeatureConfigurator.New(DivineRetribution, DivineRetributionGuid)
              .SetDisplayName(DivineRetributionDisplayName)
              .SetDescription(DivineRetributionDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddSpontaneousSpellConversion(holy, new() { null, null, null, ability, null, null, null, null, null, null })
              .AddComponent<HolyVindicatorSpellDC>(c => { c.level = 13; c.ability = DivineRetributionAblity2Guid; })
              .Configure();
        }
    }
}
