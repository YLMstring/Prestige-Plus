using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.UnitLogic.Buffs.Components;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class HermeanPotentialComp : UnitBuffComponentDelegate<HermeanPotentialComp.ComponentData>, IInitiatorRulebookHandler<RuleRollD20>, IRulebookHandler<RuleRollD20>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleRollD20 evt)
        {
            if (evt.IsFake)
            {
                return;
            }
            RulebookEvent previousEvent = Rulebook.CurrentContext.PreviousEvent;
            if (previousEvent != null && this.CheckRule(previousEvent))
            {
                base.Data.Roll = evt;
                base.Data.num = base.Data.num + 1;
                evt.AddReroll(1, true, base.Fact);
            }
        }
        public void OnEventDidTrigger(RuleRollD20 evt)
        {
            if (base.Data.Roll == evt && base.Data.num - 1 >= GetBonus() / 5)
            {
                base.Owner.RemoveFact(base.Fact);
                base.Data.Roll = null;
            }
        }

        private int GetBonus()
        {
            int num = Buff.Context?.Params?.CasterLevel ?? 0;
            return Math.Min(num, 20);
        }

        // Token: 0x0600E729 RID: 59177 RVA: 0x003B5AB0 File Offset: 0x003B3CB0
        private bool CheckRule(RulebookEvent rule)
        {
            if (rule is RuleAttackRoll)
            {
                return true;
            }
            if (rule is RuleSkillCheck)
            {
                return true;
            }
            if (rule is RuleSavingThrow)
            {
                return true;
            }
            if (rule is RuleCombatManeuver)
            {
                return true;
            }
            return false;
        }

        // Token: 0x0600E72A RID: 59178 RVA: 0x003B5B34 File Offset: 0x003B3D34
        public class ComponentData
        {
            public RuleRollD20 Roll;
            public int num = 0;
        }
    }
}

