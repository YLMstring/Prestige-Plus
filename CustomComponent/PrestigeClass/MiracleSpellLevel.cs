using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class MiracleSpellLevel : UnitFactComponentDelegate<TruthSeekerLore.ComponentData>, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            BlueprintSpellbookReference Book = BlueprintTool.GetRef<BlueprintSpellbookReference>(ExaltedEvangelist.SpellBookGuid);
            var spellbook = Owner.GetSpellbook(Book);
            if (spellbook != null && evt.Spellbook == spellbook)
            {
                evt.ReplaceSpellLevel = 7;
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        public string book;
    }
}
