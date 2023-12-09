using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Abilities.Components;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class BloodSpellConduit : ContextAction
    {
        public override string GetCaption()
        {
            return "BloodSpellConduit";
        }

        public override void RunAction()
        {
            var ability = Context.SourceAbilityContext.Ability?.ConvertedFrom;
            if (ability?.Blueprint.GetComponent<AbilityEffectStickyTouch>() == null && ability?.Blueprint.GetComponent<AbilityDeliverTouch>() == null) { return; }
            var type = Context.MaybeCaster.Body.Armor?.MaybeArmor?.ArmorType();
            if (type == Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.Medium || type == Kingmaker.Blueprints.Items.Armors.ArmorProficiencyGroup.Heavy) { return; }
            AbilityExecutionContext abilityContext3 = base.AbilityContext;
            bool isDuplicateSpellApplied = abilityContext3 != null && abilityContext3.IsDuplicateSpellApplied;
            Rulebook.Trigger(new RuleCastSpell(ability, base.Target)
            {
                IsDuplicateSpellApplied = isDuplicateSpellApplied
            });
            GameHelper.ApplyBuff(Context.MaybeCaster, CloseBuff, new Rounds?(1.Rounds()));
        }

        private static readonly BlueprintBuffReference CloseBuff = BlueprintTool.GetRef<BlueprintBuffReference>(BloodConduit.SpellConduitBuff2Guid);
    }
}
