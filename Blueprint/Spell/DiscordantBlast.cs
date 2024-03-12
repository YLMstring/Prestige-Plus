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
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using BlueprintCore.Conditions.Builder;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using BlueprintCore.Conditions.Builder.ContextEx;

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

        internal const string DisplayName1 = "NewSpellDiscordantBlast1.Name";
        private const string Description1 = "NewSpellDiscordantBlast1.Description";

        internal const string DisplayName2 = "NewSpellDiscordantBlast2.Name";
        private const string Description2 = "NewSpellDiscordantBlast2.Description";

        public static void Configure()
        {
            var icon = AbilityRefs.CacophonousCall.Reference.Get().Icon;
            var icon2 = AbilityRefs.CacophonousCallMass.Reference.Get().Icon;
            var fx = AbilityRefs.SoundBurst.Reference.Get().GetComponent<AbilitySpawnFx>();

            var ability1 = AbilityConfigurator.NewSpell(
                DiscordantBlastAbility1, DiscordantBlastAbility1Guid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName1)
              .SetDescription(Description1)
              .SetIcon(icon)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Maximize, Metamagic.Empower, Metamagic.Reach)
              .SetSpellResistance()
              .AddSpellDescriptorComponent(SpellDescriptor.Sonic)
              .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Any, radius: 10.Feet(), spreadSpeed: 20.Feet(), condition: ConditionsBuilder.New().IsCaster(true).Build())
              .AddComponent(fx)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush, newStat: StatType.Charisma, useCasterLevelAsBaseAttack: true)
                  .DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Sonic), ContextDice.Value(DiceType.D6, 3, 0))
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();

            var ability2 = AbilityConfigurator.NewSpell(
                DiscordantBlastAbility2, DiscordantBlastAbility2Guid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName2)
              .SetDescription(Description2)
              .SetIcon(icon2)
              .AllowTargeting(true, true, true, false)
              .AddAbilityDeliverProjectile(projectiles: new() { ProjectileRefs.SonicCone30Feet00.ToString() }, type: AbilityProjectileType.Cone, length: 30.Feet(), lineWidth: 5.Feet(), needAttackRoll: false)
              .SetRange(AbilityRange.Projectile)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Maximize, Metamagic.Empower, Metamagic.Reach)
              .SetSpellResistance()
              .AddSpellDescriptorComponent(SpellDescriptor.Sonic)
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush, newStat: StatType.Charisma, useCasterLevelAsBaseAttack: true)
                  .DealDamage(DamageTypes.Energy(Kingmaker.Enums.Damage.DamageEnergyType.Sonic), ContextDice.Value(DiceType.D6, 3, 0))
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();

            AbilityConfigurator.NewSpell(
                DiscordantBlastAbility, DiscordantBlastAbilityGuid, SpellSchool.Evocation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AllowTargeting(false, false, false, false)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Selective, Metamagic.Heighten, Metamagic.Quicken, Metamagic.Maximize, Metamagic.Empower, Metamagic.Reach)
              .SetSpellResistance()
              .AddSpellDescriptorComponent(SpellDescriptor.Sonic)
              .AddToSpellLists(level: 4, SpellList.Bard)
              .AddAbilityVariants(new() { ability1, ability2 })
              .AddCraftInfoComponent(
                aOEType: CraftAOE.AOE,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Other)
              .Configure();
        }
    }
}
