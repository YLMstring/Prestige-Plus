using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class MadScientistPrep : UnitFactComponentDelegate, ISpellBookRest
    {
        void ISpellBookRest.OnSpellBookRestHandler(UnitEntityData unit)
        {
            if (unit.Spellbooks?.Count() == 0) return;
            foreach (var book in unit.Spellbooks)
            {
                if (book.Blueprint.CharacterClass == CharacterClassRefs.AlchemistClass.Reference.Get())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var slots = book.GetMemorizedSpellSlots(i);
                        foreach (var slot in slots)
                        {
                            if (slot.SpellShell == null)
                            {
                                var spell = book.Blueprint.SpellList.SpellsByLevel[slot.SpellLevel + 1]?.Spells?.Random();
                                slot.SpellShell = new Kingmaker.UnitLogic.Abilities.AbilityData(spell, unit);
                                var action = ActionsBuilder.New().DealDamageToAbility(Kingmaker.EntitySystem.Stats.StatType.Wisdom, ContextDice.Value(Kingmaker.RuleSystem.DiceType.D3, 1, 0)).Build();
                                Fact.RunActionInContext(action);
                            }
                        }
                    }
                    return;
                }
            }
        }
    }
}
