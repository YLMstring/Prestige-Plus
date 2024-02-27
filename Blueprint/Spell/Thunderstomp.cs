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
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.Spell
{
    internal class Thunderstomp
    {
        private const string ThunderstompAbility = "NewSpell.UseThunderstomp";
        public static readonly string ThunderstompAbilityGuid = "{D2F2ABB9-5B63-4FBA-A250-0D70ECCF9943}";

        internal const string DisplayName = "NewSpellThunderstomp.Name";
        private const string Description = "NewSpellThunderstomp.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.EarthBlastAbility.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                ThunderstompAbility, ThunderstompAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, true, false, false)
              .SetAnimation(CastAnimationStyle.Omni)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.Kinetic_EarthBlast00_Projectile_Up.ToString() }, type: AbilityProjectileType.Simple, needAttackRoll: false)
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken)
              .SetSpellResistance()
              .AddToSpellLists(level: 1, SpellList.Hunter)
              .AddToSpellLists(level: 1, SpellList.Druid)
              .AddToSpellLists(level: 1, SpellList.Bloodrager)
              .AddToSpellLists(level: 1, SpellList.Wizard)
              .AddToSpellLists(level: 1, SpellList.Magus)
              .AddToSpellLists(level: 1, SpellList.Ranger)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, useCastingStat: true, useCasterLevelAsBaseAttack: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
