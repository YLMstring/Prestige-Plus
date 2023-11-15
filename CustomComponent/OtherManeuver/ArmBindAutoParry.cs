using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.OtherManeuver
{
    internal class ArmBindAutoParry : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Weapon?.Blueprint.IsNatural == true && evt.Weapon.Blueprint.Category != Kingmaker.Enums.WeaponCategory.UnarmedStrike && !evt.IsFake)
            {
                evt.AutoMiss = true;
                if (Buff.Rank > 1)
                {
                    Buff.Rank--;
                }
                else
                {
                    Buff.Remove();
                }
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }
    }
}
