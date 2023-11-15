using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ScarTenacity : EntityFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }
        public ActionList ActionOnSelf;
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if ((evt.IsCriticalConfirmed && !evt.FortificationNegatesCriticalHit) || evt.IsSneakAttack)
            {
                using (ContextData<ContextAttackData>.Request().Setup(evt, null, 0, 0))
                {
                    IFactContextOwner factContextOwner2 = Fact as IFactContextOwner;
                    factContextOwner2?.RunActionInContext(ActionOnSelf, evt.Target);
                }
            }
        }
    }
}
