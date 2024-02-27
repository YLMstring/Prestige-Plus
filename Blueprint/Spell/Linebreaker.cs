using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomComponent.Spell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Blueprints.ModReferences;

namespace PrestigePlus.Blueprint.Spell
{
    internal class Linebreaker
    {
        private const string LinebreakerAbility = "NewSpell.UseLinebreaker";
        public static readonly string LinebreakerAbilityGuid = "{0AE339C4-9054-4A0C-82E0-E20D1FE50B43}";

        private const string LinebreakerBuff = "NewSpell.LinebreakerBuff";
        private static readonly string LinebreakerBuffGuid = "{DBBD44D5-184A-496E-9C8A-5F355A2B6FE8}";

        internal const string DisplayName = "NewSpellLinebreaker.Name";
        private const string Description = "NewSpellLinebreaker.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BreakControlAbility.Reference.Get().Icon;

            var buff = BuffConfigurator.New(LinebreakerBuff, LinebreakerBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Overrun, Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush }, value: 2)
              .Configure();

            AbilityConfigurator.NewSpell(
                LinebreakerAbility, LinebreakerAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetAnimation(CastAnimationStyle.Omni)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Extend)
              .AddToSpellLists(level: 1, SpellList.Alchemist)
              .AddToSpellLists(level: 1, SpellList.Inquisitor)
              .AddToSpellLists(level: 1, SpellList.Magus)
              .AddToSpellLists(level: 1, SpellList.Paladin)
              .AddToSpellLists(level: 1, SpellList.Ranger)
              .AddToSpellList(level: 1, ModSpellListRefs.AntipaladinSpelllist.ToString())
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Variable(ContextValues.Rank(), Kingmaker.UnitLogic.Mechanics.DurationRate.Minutes), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
