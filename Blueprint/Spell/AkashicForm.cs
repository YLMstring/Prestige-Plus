using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.ModReferences;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.AVEx;

namespace PrestigePlus.Blueprint.Spell
{
    internal class AkashicForm
    {
        private const string AkashicFormAbility = "NewSpell.UseAkashicForm";
        public static readonly string AkashicFormAbilityGuid = "{58BCB999-B3AD-46C2-89DC-68CBA9C50BD0}";

        private const string AkashicFormBuff = "NewSpell.AkashicFormBuff";
        private static readonly string AkashicFormBuffGuid = "{E85BEB8F-916C-4DC3-B0E9-7B05C38F7B1E}";

        internal const string DisplayName = "NewSpellAkashicForm.Name";
        private const string Description = "NewSpellAkashicForm.Description";

        internal const string Duration = "PPNewDuration24h.Name";
        public static void Configure()
        {
            var icon = AbilityRefs.PerfectForm.Reference.Get().Icon;

            var buff = BuffConfigurator.New(AkashicFormBuff, AkashicFormBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddImmortality()
              .AddIncomingDamageTrigger(ActionsBuilder.New()
                    .HealTarget(ContextDice.Value(Kingmaker.RuleSystem.DiceType.Zero, 0, 9999))
                    .SpawnFx("749ad3759dc93d64dba70a84d48135b5")
                    .RemoveSelf()
                    .Build(), reduceBelowZero: true)
              .Configure();

            AbilityConfigurator.NewSpell(
                AkashicFormAbility, AkashicFormAbilityGuid, SpellSchool.Necromancy, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetAnimation(CastAnimationStyle.SelfTouch)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetLocalizedDuration(Duration)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Extend)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Days), isFromSpell: true)
                  .Build())
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .Configure();
        }
    }
}
