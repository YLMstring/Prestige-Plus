using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Craft;
using PrestigePlus.CustomComponent.Spell;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;

namespace PrestigePlus.Blueprint.Spell
{
    internal class HermeanPotential
    {
        private const string HermeanPotentialAbility = "NewSpell.UseHermeanPotential";
        public static readonly string HermeanPotentialAbilityGuid = "{37853550-902A-4CAD-AE16-833C5F4C6B17}";

        private const string HermeanPotentialBuff = "NewSpell.HermeanPotentialBuff";
        private static readonly string HermeanPotentialBuffGuid = "{E9C184BB-7DB4-45F3-8D42-FCDC9E8CE735}";

        internal const string DisplayName = "NewSpellHermeanPotential.Name";
        private const string Description = "NewSpellHermeanPotential.Description";
        public static void Configure()
        {
            var icon = AbilityRefs.BestowGraceCast.Reference.Get().Icon;

            var buff = BuffConfigurator.New(HermeanPotentialBuff, HermeanPotentialBuffGuid)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent<HermeanPotentialComp>()
              .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion)
              .Configure();

            AbilityConfigurator.NewSpell(
                HermeanPotentialAbility, HermeanPotentialAbilityGuid, SpellSchool.Enchantment, canSpecialize: false)
              .SetDisplayName(DisplayName)
              .SetDescription(Description)
              .SetIcon(icon)
              .AddComponent(AbilityRefs.Sleep.Reference.Get().GetComponent<AbilitySpawnFx>())
              .AllowTargeting(false, false, true, true)
              .SetAnimation(CastAnimationStyle.Touch)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .AddToSpellLists(level: 1, SpellList.Witch)
              .SetSpellDescriptor(SpellDescriptor.MindAffecting | SpellDescriptor.Compulsion)
              .SetLocalizedDuration(Duration.MinutePerLevel)
              .SetAvailableMetamagic(Metamagic.CompletelyNormal, Metamagic.Extend, Metamagic.Heighten)
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
