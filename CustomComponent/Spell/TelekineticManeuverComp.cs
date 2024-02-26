using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class TelekineticManeuverComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateBaseCMB>, IRulebookHandler<RuleCalculateBaseCMB>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>
    {
        // Token: 0x0600E7D7 RID: 59351 RVA: 0x003B7673 File Offset: 0x003B5873
        public void OnEventAboutToTrigger(RuleCalculateBaseCMB evt)
        {
            var cont = Buff.Context?.SourceAbilityContext;
            evt.ReplaceBAB = cont?.Params?.CasterLevel;
            evt.ReplaceStrength = cont?.Ability?.Spellbook?.Blueprint?.CastingAttribute;
        }

        // Token: 0x0600E7D8 RID: 59352 RVA: 0x003B7686 File Offset: 0x003B5886
        public void OnEventDidTrigger(RuleCalculateBaseCMB evt)
        {
            
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            var cont = Buff.Context?.SourceAbilityContext;
            if (cont == null) { return; }
            if (Rulebook.Trigger(new RuleSpellResistanceCheck(cont, evt.Target)).IsSpellResisted)
            {
                evt.AutoFailure = true;
            }
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            Buff.Remove();
        }
    }
}