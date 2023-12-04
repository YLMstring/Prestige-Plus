using BlueprintCore.Blueprints.References;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TabletopTweaks.Core.NewUnitParts.UnitPartAgeTTT;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class TruthSeekerMagic : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleApplyMetamagic>, IRulebookHandler<RuleApplyMetamagic>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleApplyMetamagic>.OnEventAboutToTrigger(RuleApplyMetamagic evt)
        {
            
        }

        void IRulebookHandler<RuleApplyMetamagic>.OnEventDidTrigger(RuleApplyMetamagic evt)
        {
            var spellbook = Owner.GetSpellbook(CharacterClassRefs.OracleClass.Reference);
            var spelllist = SpellListRefs.ClericSpellList.Reference.Get();
            if (spellbook != null && evt.Spellbook == spellbook && !spelllist.Contains(evt.Spell))
            {
                evt.Result.SpellLevelCost -= evt.AppliedMetamagics.Count;
                if (evt.BaseLevel + evt.Result.SpellLevelCost < 0)
                {
                    evt.Result.SpellLevelCost = -evt.BaseLevel;
                }
            }
        }
    }
}
