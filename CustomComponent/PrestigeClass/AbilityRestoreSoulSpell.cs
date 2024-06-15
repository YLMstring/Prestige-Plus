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
            Spellbook paramSpellbook = context.Ability.ParamSpellbook;
            int? paramSpellLevel = context.Ability.ParamSpellLevel;
            if (paramSpellbook == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spellbook is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            if (paramSpellLevel == null)
            {
                PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell level is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                return;
            }
            int num = 11;
            if (paramSpellbook.Blueprint.Spontaneous)
            {
                paramSpellbook.RestoreSpontaneousSlots(paramSpellLevel.Value, 1);
                if (context.Ability.ParamSpellLevel != null && context.Ability.ParamSpellLevel > 0)
                {
                    num = (int)context.Ability.ParamSpellLevel * 2;
                }
                context.MaybeOwner?.Descriptor.Resources.Spend(RequiredResource, num);
                return;
            }
            foreach (SpellSlot spellSlot in paramSpellbook.GetMemorizedSpellSlots(paramSpellLevel.Value))
            {
                SpellSlot paramSpellSlot = context.Ability.ParamSpellSlot;
                if ((paramSpellSlot?.SpellShell) == null || context.Ability.ParamSpellSlot.SpellShell == null)
                {
                    PFLog.Default.Error(context.AbilityBlueprint, string.Format("Target spell is missing: {0}", context.AbilityBlueprint), Array.Empty<object>());
                    break;
                }
                if (!spellSlot.Available && spellSlot.SpellShell == context.Ability.ParamSpellSlot.SpellShell)
                {
                    spellSlot.Available = true;
                    break;
                }
            }
            if (context.Ability.ParamSpellSlot != null)
            {
                num = context.Ability.ParamSpellSlot.SpellLevel * 2;
            }
            context.MaybeOwner?.Descriptor.Resources.Spend(RequiredResource, num);
        }

        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            int num = 11;
            if (ability.ParamSpellLevel is not null and > 0)
            {
                num = ability.ParamSpellLevel.Value * 2;
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
}
