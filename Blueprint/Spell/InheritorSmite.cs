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
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Blueprint.Spell
{
    internal class InheritorSmite
    {
        private const string InheritorSmiteAbility = "NewSpell.UseInheritorSmite";
        public static readonly string InheritorSmiteAbilityGuid = "{94E98C0D-2281-401C-AEA7-C4E122281309}";

        private const string InheritorSmiteBuff = "NewSpell.InheritorSmiteBuff";
        private static readonly string InheritorSmiteBuffGuid = "{9E4AE19B-693A-42AE-A9C5-C193D4D5268A}";

        private const string InheritorSmiteBuff2 = "NewSpell.InheritorSmiteBuff2";
        private static readonly string InheritorSmiteBuff2Guid = "{6F9F68D1-BC57-47B2-9DDF-D04C528DF82A}";

        internal const string DisplayName = "NewSpellInheritorSmite.Name";
        private const string Description = "NewSpellInheritorSmite.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.SmiteEvilAbility.Reference.Get().Icon;

            var action2 = ActionsBuilder.New()
                .RemoveSelf()
                .Build();

            var buff2 = BuffConfigurator.New(InheritorSmiteBuff2, InheritorSmiteBuff2Guid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddCMBBonus(descriptor: ModifierDescriptor.Sacred, value: 5)
              .AddManeuverTrigger(action2)
              .Configure();

            var action = ActionsBuilder.New()
                .CombatManeuver(ActionsBuilder.New().Build(), Kingmaker.RuleSystem.Rules.CombatManeuver.BullRush)
                .RemoveSelf()
                .Build();

            var buff = BuffConfigurator.New(InheritorSmiteBuff, InheritorSmiteBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddAttackTypeAttackBonus(attackBonus: 5, descriptor: ModifierDescriptor.Sacred, type: WeaponRangeType.Melee)
              .AddInitiatorAttackRollTrigger(action, false, onlyHit: true)
              .AddInitiatorAttackRollTrigger(action2, false, onlyMiss: true)
              .Configure();

            AbilityConfigurator.NewSpell(
                InheritorSmiteAbility, InheritorSmiteAbilityGuid, SpellSchool.Transmutation, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
              .AllowTargeting(false, false, true, false)
              .SetAnimation(CastAnimationStyle.Self)
              .SetRange(AbilityRange.Personal)
              .SetType(AbilityType.Spell)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal)
              .AddToSpellLists(level: 2, SpellList.Cleric)
              .AddToSpellLists(level: 2, SpellList.Warpriest)
              .AddToSpellLists(level: 2, SpellList.Paladin)
              .AddToSpellLists(level: 2, SpellList.Inquisitor)
              .AddAbilityCasterHasFacts(new() { FeatureRefs.IomedaeFeature.Reference.Get() })
              .AddAbilityEffectRunAction(
                actions: ActionsBuilder.New()
                  .ApplyBuff(buff, ContextDuration.Fixed(2), isFromSpell: true)
                  .Build())
              .AddCraftInfoComponent(
                aOEType: CraftAOE.None,
                savingThrow: CraftSavingThrow.None,
                spellType: CraftSpellType.Buff)
              .Configure();
        }
    }
}
