using BlueprintCore.Utils;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class EnergyArrow : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {
            try
            {
                int num = 0;
                foreach (DamageValue damageValue in evt.ResultList)
                {
                    num += damageValue.FinalValue;
                }
                int healValue = num;
                if (healValue > 0)
                {
                    Rulebook.Trigger<RuleHealDamage>(new RuleHealDamage(base.Owner, base.Owner, healValue));
                }
            }
            catch (Exception ex) { Logger.Error("Failed to heal energy arrow.", ex); }
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            try
            {
                foreach (BaseDamage baseDamage in evt.DamageBundle)
                {
                    baseDamage.AddDecline(new DamageDecline(DamageDeclineType.Total, base.Fact));
                }
            }
            catch (Exception ex) { Logger.Error("Failed to negate damage energy arrow.", ex); }
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {
            
        }

        void IRulebookHandler<RuleDealDamage>.OnEventAboutToTrigger(RuleDealDamage evt)
        {
            
        }
    }
}
