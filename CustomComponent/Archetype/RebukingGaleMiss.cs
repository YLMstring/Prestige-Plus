using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class RebukingGaleMiss : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber, IUnitSubscriber, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.AttackType == Kingmaker.RuleSystem.AttackType.RangedTouch || evt.AttackType == Kingmaker.RuleSystem.AttackType.Ranged)
            {
                evt.AutoMiss = true;
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }
    }
}
