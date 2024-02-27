using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class InheritorSmiteComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600E7A9 RID: 59305 RVA: 0x003B71B4 File Offset: 0x003B53B4
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        // Token: 0x0600E7AA RID: 59306 RVA: 0x003B71B6 File Offset: 0x003B53B6
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit && evt.AttackType == Kingmaker.RuleSystem.AttackType.Melee)
            {
                (Fact as IFactContextOwner)?.RunActionInContext(onhit, evt.Target);
            }
            base.Owner.RemoveFact(base.Fact);
        }

        public ActionList onhit;
    }
}
