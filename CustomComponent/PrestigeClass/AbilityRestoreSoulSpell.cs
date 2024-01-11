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
using System.Runtime.Remoting.Contexts;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AbilityRestoreSoulSpell : AbilityApplyEffect, IAbilityRestriction
    {
        public override void Apply(AbilityExecutionContext context, TargetWrapper target)
        {
            if (context.Ability.ParamSpellSlot == null || context.Ability.ParamSpellSlot.SpellShell == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (context.Ability.ParamSpellSlot.SpellShell.Spellbook == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Spellbook is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            SpellSlot notAvailableSpellSlot = AbilityRestoreSpellSlot.GetNotAvailableSpellSlot(context.Ability.ParamSpellSlot.SpellShell);
            if (notAvailableSpellSlot == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Can't find slot for restore: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            notAvailableSpellSlot.Available = true;
            int num = 11;
            if (context.Ability.ParamSpellSlot != null)
            {
                num = context.Ability.ParamSpellSlot.SpellLevel * 2;
            }
            context.MaybeOwner?.Descriptor.Resources.Spend(RequiredResource, num);
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            int num = 11;
            if (ability.ParamSpellSlot != null)
            {
                num = ability.ParamSpellSlot.SpellLevel * 2;
            }
            if (!ability.Caster.Resources.HasEnoughResource(RequiredResource, num))
            {
                return false;
            }
            return true;
        }

        public string GetAbilityRestrictionUIText()
        {
            return "Not Suitable Spell Slot";
        }

        public BlueprintAbilityResource RequiredResource;
    }

    internal class AbilityRestoreSoulSpell2 : AbilityApplyEffect, IAbilityRestriction
    {
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
            context.Ability.ParamSpellbook.RestoreSpontaneousSlots(context.Ability.ParamSpellLevel.Value, 1);
            int num = 11;
            if (context.Ability.ParamSpellLevel != null && context.Ability.ParamSpellLevel > 0)
            {
                num = (int)context.Ability.ParamSpellLevel * 2;
            }
            context.MaybeOwner?.Descriptor.Resources.Spend(RequiredResource, num);
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            int num = 11;
            if (ability.ParamSpellLevel != null && ability.ParamSpellLevel > 0)
            {
                num = (int)ability.ParamSpellLevel;
                var free = ability.ParamSpellbook?.Blueprint.Spontaneous;
                if (free == false) return false;
            }
            if (!ability.Caster.Resources.HasEnoughResource(RequiredResource, num * 2))
            {
                return false;
            }
            return true;
        }

        public string GetAbilityRestrictionUIText()
        {
            return "Not Suitable Spell Slot";
        }

        public BlueprintAbilityResource RequiredResource;
    }
}
