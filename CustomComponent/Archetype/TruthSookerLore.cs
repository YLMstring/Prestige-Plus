using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Utility;
using Kingmaker.Blueprints.JsonSystem;

namespace PrestigePlus.CustomComponent.Archetype
{
    [TypeId("{50DFB7EA-F736-4472-8691-BC6CD7A5233F}")]
    internal class TruthSookerLore : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            var spellbook = Owner.GetSpellbook(CharacterClassRefs.SorcererClass.Reference);
            var spell = evt.AbilityData?.ConvertedFrom?.Blueprint ?? evt.Spell;
            if (SpellList.Count == 0) { OnActivate(); }
            if (spellbook != null && evt.Spellbook == spellbook && SpellList.Contains(spell))
            {
                evt.AddBonusCasterLevel(4, Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
            }
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        public override void OnActivate()
        {
            var list = FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().m_AllFeatures;
            foreach (var blood in list)
            {
                if (!Owner.HasFact(blood)) { continue; }
                var lines = blood.Get() as BlueprintProgression;
                var comps = lines?.LevelEntries;
                if (comps?.Any() != true) { continue; }
                foreach (var item in comps)
                {
                    if (item.m_Features?.Any() != true) { continue; }
                    foreach(var feature in item.m_Features)
                    {
                        var spells = feature.Get().GetComponents<AddKnownSpell>();
                        if (spells?.Any() != true) { continue; }
                        foreach (var spell in spells)
                        {
                            if (spell.m_Spell == null) { continue; }
                            SpellList.Add(spell.m_Spell);
                        }
                    }
                }
            }
        }

        public override void OnDeactivate()
        {
            SpellList = new() { };
        }

        private List<BlueprintAbility> SpellList = new() { };
    }
}
