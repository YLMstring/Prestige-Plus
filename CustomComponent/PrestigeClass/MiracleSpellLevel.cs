using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class MiracleSpellLevel : UnitFactComponentDelegate<TruthSeekerLore.ComponentData>, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleApplySpell>, IRulebookHandler<RuleApplySpell>
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            BlueprintSpellbookReference Book = BlueprintTool.GetRef<BlueprintSpellbookReference>(book);
            var spellbook = Owner.GetSpellbook(Book);
            if (spellbook != null && evt.Spellbook == spellbook)
            {
                evt.ReplaceSpellLevel = level;
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        void IRulebookHandler<RuleApplySpell>.OnEventAboutToTrigger(RuleApplySpell evt)
        {
            
        }

        void IRulebookHandler<RuleApplySpell>.OnEventDidTrigger(RuleApplySpell evt)
        {
            BlueprintSpellbookReference Book = BlueprintTool.GetRef<BlueprintSpellbookReference>(book);
            var spellbook = Owner.GetSpellbook(Book);
            var ability = evt.Spell.StickyTouch ?? evt.Spell;
            ability = ability.ConvertedFrom ?? evt.Spell;
            if (spellbook != null && ability.Spellbook == spellbook && evt.Spell.Blueprint.GetComponent<AbilityEffectStickyTouch>() == null)
            {
                GameHelper.ApplyBuff(Owner, BlueprintTool.GetRef<BlueprintBuffReference>(buff));
            }
        }

        public string book;
        public string buff;
        public int level;
    }
}
