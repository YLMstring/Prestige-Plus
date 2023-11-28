using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class EldritchAssaultCheck : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600E68F RID: 59023 RVA: 0x003B357F File Offset: 0x003B177F
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (UnitPartConcealment.Calculate(evt.Target, Owner, true) == Kingmaker.Enums.Concealment.Total)
            {
                evt.AutoCriticalConfirmation = true;
            }
        }

        // Token: 0x0600E690 RID: 59024 RVA: 0x003B3588 File Offset: 0x003B1788
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
        }
    }
}
