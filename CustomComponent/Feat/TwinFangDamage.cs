using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class TwinFangDamage : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>
    {
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt.Initiator == Owner)
            {
                var list = new List<BaseDamage>();
                foreach (BaseDamage dmg in evt.ParentRule.DamageBundle)
                {
                    list.Add(dmg);
                }
                foreach (BaseDamage dmg in list)
                {
                    evt.ParentRule.Add(dmg);
                }
            }
            if (evt.Target == Owner)
            {
                foreach (BaseDamage dmg in evt.ParentRule.DamageBundle)
                {
                    dmg.AddDecline(new DamageDecline(DamageDeclineType.ByHalf, base.Fact));
                }
            }
        }
        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }
    }
}
