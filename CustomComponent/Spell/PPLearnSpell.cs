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
        public override void OnActivate()
        {
            this.Learn();
        }
        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            this.Learn();
        }

        private void Learn()
        {
            if (Spell == null) { return; }
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
            Owner.GetSpellbook(clazz)?.AddKnown(level, Spell, true);
        }
    }
}
