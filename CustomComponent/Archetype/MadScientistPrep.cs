using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
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
        public ActionList Action;
        void ISpellBookRest.OnSpellBookRestHandler(UnitEntityData unit)
        {
            if (unit != Owner || unit.Spellbooks?.Count() == 0) return;
            foreach (var book in unit.Spellbooks)
            {
                if (book.Blueprint.CharacterClass == CharacterClassRefs.AlchemistClass.Reference.Get())
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var slots = book.GetMemorizedSpellSlots(i);
                        foreach (var slot in slots)
                        {
                            if (slot.SpellShell == null || !book.IsKnownOnLevel(slot.SpellShell.Blueprint, i))
                            {
                                var spell = book.Blueprint.SpellList.SpellsByLevel[i + 1]?.Spells?.Random();
                                slot.SpellShell = new Kingmaker.UnitLogic.Abilities.AbilityData(spell, book, i)
                                {
                                    IsTemporary = true
                                };
                                slot.Available = true;
                                Fact.RunActionInContext(Action);
                            }
                        }
                    }
                    return;
                }
            }
        }
    }
}
