using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class PPLearnSpell : UnitFactComponentDelegate, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber
    {
        public BlueprintCharacterClass CharacterClass;
        public BlueprintAbility Spell;
        public int level;
        public override void OnTurnOn()
        {
            this.Learn();
        }
        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            this.Learn();
        }

        private void Learn()
        {
            if (this.CharacterClass != null)
            {
                LearnList(this.CharacterClass);
            }
            else
            {
                foreach (var data in Owner.Progression.Classes)
                {
                    LearnList(data.CharacterClass);
                }
            }
        }
        private void LearnList(BlueprintCharacterClass clazz)
        {
            ClassData classData = base.Owner.Progression.GetClassData(clazz);
            if (classData == null)
            {
                return;
            }
            Spellbook spellbook = base.Owner.DemandSpellbook(clazz);
            if (spellbook.GetKnownSpells(level).All((AbilityData spellFromSpellbook) => spellFromSpellbook.Blueprint != Spell))
            {
                spellbook.AddKnown(level, Spell, true);
            }
        }
    }
}
