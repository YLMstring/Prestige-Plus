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
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.CustomAction.OtherManeuver;

namespace PrestigePlus.Blueprint.Spell
{
    internal class BladeLash
    {
        private const string BladeLashAbility = "NewSpell.UseBladeLash";
        public static readonly string BladeLashAbilityGuid = "{01346696-ECFF-4F51-8BB2-1D9DCA00041B}";

        internal const string DisplayName = "NewSpellBladeLash.Name";
        private const string Description = "NewSpellBladeLash.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BladeWhirlwindAbility.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                .Add<ContextActionPPmaneuver>(c => { c.Maneuver = Kingmaker.RuleSystem.Rules.CombatManeuver.Trip; c.Penalty = 10; c.Mod = ModifierDescriptor.UntypedStackable; c.useWeapon = true; })
                .Build();

            AbilityConfigurator.NewSpell(
                BladeLashAbility, BladeLashAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, true, false, false)
              .SetAnimation(CastAnimationStyle.Special)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.Sithhud_SwordTraceLine00.ToString() }, type: AbilityProjectileType.Simple, needAttackRoll: false)
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
