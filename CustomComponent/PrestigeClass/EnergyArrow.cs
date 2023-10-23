using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
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
    [TypeId("{52F7DBF3-1AD6-4445-A87F-0D6C0B5E840E}")]
    internal class EnergyArrow : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");

        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {
            try
            {
                int num = 0;
                //Logger.Info("start heal");
                foreach (DamageValue damageValue in evt.ResultList)
                {
                    num += damageValue.RollAndBonusValue;
                    //Logger.Info(num.ToString());
                }
                int healValue = num;
                //Logger.Info("finish heal");
                if (healValue > 0)
                {
                    Rulebook.Trigger(new RuleHealDamage(base.Owner, base.Owner, healValue));
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
