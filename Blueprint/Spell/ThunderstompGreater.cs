using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;

namespace PrestigePlus.Blueprint.Spell
{
    internal class ThunderstompGreater
    {
        private const string ThunderstompGreaterAbility = "NewSpell.UseThunderstompGreater";
        public static readonly string ThunderstompGreaterAbilityGuid = "{D04BC997-A936-4E4A-91D2-FD02A4C73723}";

        internal const string DisplayName = "NewSpellThunderstompGreater.Name";
        private const string Description = "NewSpellThunderstompGreater.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.DeadlyEarthEarthBlastAbility.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                ThunderstompGreaterAbility, ThunderstompGreaterAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(true, true, true, false)
              .SetAnimation(CastAnimationStyle.Directional)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.Kinetic_EarthBlastLine00.ToString() }, type: AbilityProjectileType.Line, length: 60.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
              .SetRange(AbilityRange.Projectile)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken)
              .SetSpellResistance()
              .AddToSpellLists(level: 3, SpellList.Hunter)
              .AddToSpellLists(level: 3, SpellList.Druid)
              .AddToSpellLists(level: 3, SpellList.Bloodrager)
              .AddToSpellLists(level: 3, SpellList.Wizard)
              .AddToSpellLists(level: 3, SpellList.Magus)
              .AddToSpellLists(level: 3, SpellList.Ranger)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, useCastingStat: true, useCasterLevelAsBaseAttack: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
