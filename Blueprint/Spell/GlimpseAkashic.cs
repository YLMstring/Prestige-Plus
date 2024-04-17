using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;

namespace PrestigePlus.Blueprint.Spell
{
    internal class GlimpseAkashic
    {
        private const string GlimpseAkashicAbility = "NewSpell.UseGlimpseAkashic";
        public static readonly string GlimpseAkashicAbilityGuid = "{4C3799A6-58C1-4912-9697-4CB5E919333B}";

        private const string GlimpseAkashicAbility2 = "NewSpell.UseGlimpseAkashic2";
        public static readonly string GlimpseAkashicAbility2Guid = "{6E7D60C3-3BB6-4BF5-BD6B-4760CD9D932E}";

        private const string GlimpseAkashicBuff = "NewSpell.GlimpseAkashicBuff";
        private static readonly string GlimpseAkashicBuffGuid = "{9E781EC7-E6DA-4F94-8156-0E645822E6EF}";

        private const string GlimpseAkashicBuff2 = "NewSpell.GlimpseAkashicBuff2";
        private static readonly string GlimpseAkashicBuff2Guid = "{F7AF5505-9566-4B85-8159-362B974DB6A3}";

        internal const string DisplayName = "NewSpellGlimpseAkashic.Name";
        private const string Description = "NewSpellGlimpseAkashic.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.TrueSeeing.Reference.Get().Icon;
            var fx = AbilityRefs.TrueSeeing.Reference.Get().GetComponent<AbilitySpawnFx>();

            var enemybuff = BuffConfigurator.New(GlimpseAkashicBuff2, GlimpseAkashicBuff2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<GlimpseAkashicComp>()
              .AddBuffActions(deactivated: ActionsBuilder.New()
                    .ApplyBuff(BuffRefs.Stunned.ToString(), ContextDuration.Fixed(2))
                    .ApplyBuff(BuffRefs.Nauseated.ToString(), ContextDuration.Fixed(10))
                    .Build())
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(enemybuff, ContextDuration.Fixed(10), isFromSpell: true)
                .RemoveBuff(GlimpseAkashicBuffGuid)
                .Build();

            var ability2 = AbilityConfigurator.New(GlimpseAkashicAbility2, GlimpseAkashicAbility2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent(fx)
              .SetAnimation(CastAnimationStyle.Omni)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetType(AbilityType.Special)
              .Configure();

            var selfbuff = BuffConfigurator.New(GlimpseAkashicBuff, GlimpseAkashicBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .AddAbilityUseTrigger(ability: ability2, action: action, actionsOnTarget: true, forOneSpell: true)
              .Configure();

            AbilityConfigurator.NewSpell(
                GlimpseAkashicAbility, GlimpseAkashicAbilityGuid, SpellSchool.Divination, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetLocalizedDuration(Duration.TwentyFourHours)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .SetIsFullRoundAction(true)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(selfbuff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days), isFromSpell: true)
                  .Build())
              .Configure();
        }
    }
}
