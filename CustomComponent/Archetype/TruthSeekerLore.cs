using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    [TypeId("{C53D7688-EF62-41C1-8AE8-F32F36C4050F}")]
    internal class TruthSeekerLore : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            var spellbook = Owner.GetSpellbook(CharacterClassRefs.OracleClass.Reference);
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
            var list = FeatureSelectionRefs.OracleMysterySelection.Reference.Get().m_AllFeatures;
            foreach ( var mystery in list )
            {
                if (!Owner.HasFact(mystery)) { continue; }
                var comps = mystery.Get().GetComponents<AddFeatureOnClassLevel>();
                if (comps?.Any() != true) { continue; }
                foreach (var item in comps)
                {
                    if (item.Level != 2) { continue; }
                    var spells = item.m_Feature?.Get().GetComponents<AddKnownSpell>();
                    if (spells?.Any() != true) { continue; }
                    foreach (var spell in spells)
                    {
                        if (spell.m_Spell == null) { continue; }
                        SpellList.Add(spell.m_Spell);
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
