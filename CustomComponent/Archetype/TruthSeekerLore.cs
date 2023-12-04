using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class TruthSeekerLore : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            var spellbook = Owner.GetSpellbook(CharacterClassRefs.OracleClass.Reference);
            var spelllist = SpellListRefs.ClericSpellList.Reference.Get();
            var spell = evt.AbilityData?.ConvertedFrom?.Blueprint ?? evt.Spell;
            if (spellbook != null && evt.Spellbook == spellbook && !spelllist.Contains(spell))
            {
                evt.AddBonusCasterLevel(4, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
            
        }
    }
}
