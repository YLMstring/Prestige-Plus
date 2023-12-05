﻿using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
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
            //var spelllist = SpellListRefs.ClericSpellList.Reference.Get();
            if (spellbook != null && evt.Spellbook == spellbook && SpellList.Contains(evt.Spell))
            {
                evt.Result.SpellLevelCost -= evt.AppliedMetamagics.Count;
                if (evt.BaseLevel + evt.Result.SpellLevelCost < 0)
                {
                    evt.Result.SpellLevelCost = -evt.BaseLevel;
                }
            }
        }

        public override void OnActivate()
        {
            var list = FeatureSelectionRefs.OracleMysterySelection.Reference.Get().m_AllFeatures;
            foreach (var mystery in list)
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
