using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class WindPlusDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600E67C RID: 59004 RVA: 0x003B33C4 File Offset: 0x003B15C4
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spellbook == null)
            {
                return;
            }
            SpellDescriptorComponent component = evt.Spell.GetComponent<SpellDescriptorComponent>();
            if (component == null)
            {
                return;
            }
            SpellDescriptor spellDescriptor = component.Descriptor.Value;
            spellDescriptor = UnitPartChangeSpellElementalDamage.ReplaceSpellDescriptorIfCan(base.Owner, spellDescriptor);
            if (spellDescriptor.HasAnyFlag(SpellDescriptor.Cold) || spellDescriptor.HasAnyFlag(SpellDescriptor.Electricity) || spellDescriptor.HasAnyFlag(SpellDescriptor.Sonic))
            {
                int num = 1;
                int level = Owner.Progression.GetClassLevel(CharacterClassRefs.ArcanistClass.Reference);
                if (level >= 10)
                {
                    num = 2;
                }
                evt.AddBonusDC(num);
            }
        }

        // Token: 0x0600E67D RID: 59005 RVA: 0x003B3430 File Offset: 0x003B1630
        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }
    }
}
