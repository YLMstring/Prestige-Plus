using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Blueprints;
using Mono.Cecil;
using static Kingmaker.Blueprints.BlueprintAbilityResource;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AbilityRestoreSoulSpell : AbilityApplyEffect, IAbilityRestriction, IAbilityRequiredParameters
    {
        public AbilityParameter RequiredParameters
        {
            get
            {
                return AbilityParameter.Spellbook | AbilityParameter.SpellLevel | AbilityParameter.SpellSlot;
            }
        }
        public override void Apply(AbilityExecutionContext context, TargetWrapper target)
        {
            if (context.Ability.ParamSpellbook == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spellbook is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellLevel == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell level is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellLevel.Value > this.SpellLevel)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Invalid target spell level {0}: {1}", context.Ability.ParamSpellLevel.Value, context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            SpellSlot notAvailableSpellSlot = GetNotAvailableSpellSlot(context.Ability);
            if (notAvailableSpellSlot != null)
            {
                notAvailableSpellSlot.Available = true;
            }
            else
            {
                context.Ability.ParamSpellbook.RestoreSpontaneousSlots(context.Ability.ParamSpellLevel.Value, 1);
            }
            context.MaybeOwner?.Descriptor.Resources.Spend(RequiredResource, context.Ability.ParamSpellLevel.Value * 2);
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            if (ability.ParamSpellbook == null || ability.ParamSpellLevel == null || (ability.ParamSpellLevel.Value > this.SpellLevel))
            {
                return false;
            }
            if (!ability.Caster.Resources.HasEnoughResource(RequiredResource, ability.ParamSpellLevel.Value * 2))
            {
                return false;
            }
            int spontaneousSlots = ability.ParamSpellbook.GetSpontaneousSlots(ability.ParamSpellLevel.Value);
            int spellsPerDay = ability.ParamSpellbook.GetSpellsPerDay(ability.ParamSpellLevel.Value);
            if (spontaneousSlots < spellsPerDay || GetNotAvailableSpellSlot(ability) != null) 
            { 
                return true; 
            }
            return false;
        }

        public string GetAbilityRestrictionUIText()
        {
            return "Not Proper Spell Slot";
        }

        private static SpellSlot GetNotAvailableSpellSlot(AbilityData ability)
        {
            AbilityData abilityData = ability.ParamSpellSlot?.SpellShell;
            if (abilityData?.Spellbook == null)
            {
                return null;
            }
            foreach (SpellSlot spellSlot in abilityData.Spellbook.GetMemorizedSpellSlots(abilityData.SpellLevel))
            {
                if (!spellSlot.Available && spellSlot.SpellShell == abilityData)
                {
                    return spellSlot;
                }
            }
            return null;
        }

        public int SpellLevel = 5;
        public BlueprintAbilityResource RequiredResource;
    }
}
