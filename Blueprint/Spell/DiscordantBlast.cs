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
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Blueprint.Spell
{
    internal class DiscordantBlast
    {
        private const string DiscordantBlastAbility = "NewSpell.UseDiscordantBlast";
        public static readonly string DiscordantBlastAbilityGuid = "{8A4AF435-BC9D-4998-A6B5-9F08E7F8336D}";

        private const string DiscordantBlastAbility1 = "NewSpell.UseDiscordantBlast1";
        public static readonly string DiscordantBlastAbility1Guid = "{FA4E9929-E1F6-4B3C-B783-B12233294A80}";

        private const string DiscordantBlastAbility2 = "NewSpell.UseDiscordantBlast2";
        public static readonly string DiscordantBlastAbility2Guid = "{2C9D334C-786F-48B1-BFDB-C618EF63DB1C}";


        internal const string DisplayName = "NewSpellDiscordantBlast.Name";
        private const string Description = "NewSpellDiscordantBlast.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.DeadlyEarthEarthBlastAbility.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                DiscordantBlastAbility, DiscordantBlastAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(true, true, true, false)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.Kinetic_EarthBlastLine00.ToString() }, type: AbilityProjectileType.Line, length: 60.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
              .SetRange(AbilityRange.Projectile)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Maximize, Metamagic.Empower, Metamagic.Reach)
              .SetSpellResistance()
              .AddToSpellLists(level: 4, SpellList.Bard)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip, newStat: StatType.Charisma, useCasterLevelAsBaseAttack: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
