using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class MagusCheatRay : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>, IUnitTryCastingDefensivelyHandler, IUnitSubscriber
    {
        void IUnitTryCastingDefensivelyHandler.HandleUnitTryCastingDefensively(AbilityData ability, ref bool forcedSuccess)
        {
            forcedSuccess = true;
        }

        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt)
        {
            Buff.Remove();
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.AttackType == Kingmaker.RuleSystem.AttackType.RangedTouch && !evt.IsFake)
            {
                if (Buff.GetRank() < 3)
                {
                    evt.AutoHit = true;
                    if (Buff.GetRank() == 2)
                    {
                        evt.AutoCriticalThreat = true;
                        evt.AutoCriticalConfirmation = true;
                    }
                    Buff.Rank = 3;
                }
                else
                {
                    evt.AutoMiss = true;
                }
            }
        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt)
        {
            
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }
    }
}
