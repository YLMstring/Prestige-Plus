using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;

namespace PrestigePlus.Blueprint.Spell
{
    internal class PilferingHand
    {
        private const string PilferingHandAbility = "NewSpell.UsePilferingHand";
        public static readonly string PilferingHandAbilityGuid = "{9916D96D-04BD-4604-A0D6-3D1FEA6DCAAE}";

        internal const string DisplayName = "NewSpellPilferingHand.Name";
        private const string Description = "NewSpellPilferingHand.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.WrackingRay.Reference.Get().Icon;

            AbilityConfigurator.NewSpell(
                PilferingHandAbility, PilferingHandAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, true, false, false)
              .SetAnimation(CastAnimationStyle.Omni)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.BuffRay00_Abjuration.ToString() }, type: AbilityProjectileType.Simple, needAttackRoll: false)
              .SetRange(AbilityRange.Close)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Heighten, Metamagic.Quicken)
              .SetSpellResistance()
              .AddToSpellLists(level: 2, SpellList.Bard)
              .AddToSpellLists(level: 2, SpellList.Cleric)
              .AddToSpellLists(level: 2, SpellList.Wizard)
              .AddToSpellLists(level: 2, SpellList.Magus)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.Disarm, useCastingStat: true, useCasterLevelAsBaseAttack: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
