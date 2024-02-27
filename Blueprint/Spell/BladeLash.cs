using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class BladeLash
    {
        private const string BladeLashAbility = "NewSpell.UseBladeLash";
        public static readonly string BladeLashAbilityGuid = "{01346696-ECFF-4F51-8BB2-1D9DCA00041B}";

        private const string BladeLashBuff = "NewSpell.BladeLashBuff";
        private static readonly string BladeLashBuffGuid = "{64C6A71F-E46B-4E18-812E-4B1CB3C2BCC5}";

        internal const string DisplayName = "NewSpellBladeLash.Name";
        private const string Description = "NewSpellBladeLash.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BladeWhirlwindAbility.Reference.Get().Icon;

            var action2 = ActionsBuilder.New()
                .RemoveSelf()
                .Build();

            var buff = BuffConfigurator.New(BladeLashBuff, BladeLashBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Trip }, value: 10)
              .AddManeuverTrigger(action2)
              .Configure();

            var action = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1), toCaster: true)
                .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip)
                .Build();

            AbilityConfigurator.NewSpell(
                BladeLashAbility, BladeLashAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetAnimation(CastAnimationStyle.Immediate)
              .AddComponent(AbilityRefs.DazzlingDisplayAction.Reference.Get().GetComponent<AbilitySpawnFx>())
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(20)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken)
              .AddToSpellLists(level: 1, SpellList.Bloodrager)
              .AddToSpellLists(level: 1, SpellList.Magus)
              .AddAbilityCasterMainWeaponIsMelee()
              .AddAbilityEffectRunAction(action)
              .SetSpellResistance(false)
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
