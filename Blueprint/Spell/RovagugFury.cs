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
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;

namespace PrestigePlus.Blueprint.Spell
{
    internal class RovagugFury
    {
        private const string RovagugFuryAbility = "NewSpell.UseRovagugFury";
        public static readonly string RovagugFuryAbilityGuid = "{4C9D7C2A-48D4-470B-A9E0-CA9AF5444224}";

        internal const string DisplayName = "NewSpellRovagugFury.Name";
        private const string Description = "NewSpellRovagugFury.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.DeadlyEarthEarthBlastAbility.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                RovagugFuryAbility, RovagugFuryAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(true, true, true, false)
              .SetAnimation(CastAnimationStyle.Directional)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.SonicCone30Feet00.ToString() }, type: AbilityProjectileType.Cone, length: 30.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
              .SetRange(AbilityRange.Projectile)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken)
              .SetSpellResistance()
              .AddToSpellLists(level: 2, SpellList.Cleric)
              .AddToSpellLists(level: 2, SpellList.Warpriest)
              .AddToSpellLists(level: 2, SpellList.Inquisitor)
              .AddToSpellLists(level: 2, SpellList.Bloodrager)
              .AddToSpellLists(level: 2, SpellList.Wizard)
              .AddToSpellLists(level: 2, SpellList.Witch)
              .AddAbilityCasterHasFacts(new() { FeatureRefs.RovagugFeature.Reference.Get() })
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Trip)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
