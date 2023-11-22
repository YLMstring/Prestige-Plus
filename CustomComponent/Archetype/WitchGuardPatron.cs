using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class WitchGuardPatron : UnitFactComponentDelegate, ILevelUpCompleteUIHandler, IGlobalSubscriber, ISubscriber
    {
        public void HandleLevelUpComplete(UnitEntityData unit, bool isChargen)
        {
            GetProgression();
        }

        public override void OnActivate()
        {
            GetProgression();
        }
        private void AddSpell(int SpellLevel, BlueprintAbility Spell)
        {
            ClassData classData = Owner.Progression.GetClassData(CharacterClassRefs.RangerClass.Reference);
            if (classData == null || SpellLevel == 0 || Spell == null)
            {
                return;
            }
            BlueprintSpellbook spellbook = classData.Spellbook;
            if (spellbook == null)
            {
                return;
            }
            foreach (Spellbook spellbook2 in Owner.Spellbooks)
            {
                bool flag = spellbook2.Blueprint == spellbook;
                bool flag2 = spellbook2.GetKnownSpells(SpellLevel).Any((p) => p.Blueprint == Spell);
                bool flag3 = spellbook2.MaxSpellLevel >= SpellLevel;
                if (flag && !flag2 && flag3)
                {
                    spellbook2.AddKnown(SpellLevel, Spell, true);
                }
            }
        }

        private void GetProgression()
        {
            var list = FeatureSelectionRefs.WitchPatronSelection.Reference.Get().m_AllFeatures;
            foreach (var prog in list)
            {
                var pro = prog?.Get()?.ToReference<BlueprintProgressionReference>();
                if (pro != null && Owner.HasFact(pro))
                {
                    GetSpell(pro);
                }
            }
        }

        private void GetSpell(BlueprintProgression pro)
        {
            foreach (var entry in pro.LevelEntries)
            {
                if (entry?.Level <= Owner.Progression.GetClassLevel(CharacterClassRefs.RangerClass.Reference))
                {
                    var feat = entry.m_Features.First()?.Get()?.GetComponent<AddKnownSpell>();
                    if (feat != null)
                    {
                        AddSpell(feat.SpellLevel, feat.Spell);
                    }
                }
            }
        }
    }
}
