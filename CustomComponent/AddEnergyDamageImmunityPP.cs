using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.CustomComponent
{
    internal class AddEnergyDamageImmunityPP : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>
    {
        // Token: 0x0600C134 RID: 49460 RVA: 0x00327634 File Offset: 0x00325834
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                DamageEnergyType? damageEnergyType = (baseDamage is EnergyDamage energyDamage) ? new DamageEnergyType?(energyDamage.EnergyType) : null;
                if (damageEnergyType != null)
                {
                    DamageEnergyType? damageEnergyType2 = damageEnergyType;
                    DamageEnergyType energyType = this.EnergyType;
                    if (damageEnergyType2.GetValueOrDefault() == energyType & damageEnergyType2 != null)
                    {
                        baseDamage.AddDecline(new DamageDecline(DamageDeclineType.Total, base.Fact));
                    }
                }
            }
        }

        // Token: 0x0600C135 RID: 49461 RVA: 0x003276D4 File Offset: 0x003258D4
        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }

        // Token: 0x0600C136 RID: 49462 RVA: 0x003276D6 File Offset: 0x003258D6
        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
        }

        // Token: 0x0600C137 RID: 49463 RVA: 0x003276D8 File Offset: 0x003258D8
        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (this.HealOnDamage)
            {
                int num = 0;
                foreach (DamageValue damageValue in evt.ResultList)
                {
                    DamageEnergyType? damageEnergyType = (damageValue.Source is EnergyDamage energyDamage) ? new DamageEnergyType?(energyDamage.EnergyType) : null;
                    if (damageEnergyType != null)
                    {
                        DamageEnergyType? damageEnergyType2 = damageEnergyType;
                        DamageEnergyType energyType = this.EnergyType;
                        if (damageEnergyType2.GetValueOrDefault() == energyType & damageEnergyType2 != null)
                        {
                            num += damageValue.RollAndBonusValue;
                        }
                    }
                }
                int healValue = AddEnergyDamageImmunity.GetHealValue(num, this.m_HealRate);
                if (healValue > 0)
                {
                    Rulebook.Trigger(new RuleHealDamage(base.Owner, base.Owner, healValue));
                }
            }
        }

        // Token: 0x0600C138 RID: 49464 RVA: 0x003277B8 File Offset: 0x003259B8
        public static int GetHealValue(int damage, AddEnergyDamageImmunity.HealingRate healingRate)
        {
            if (healingRate == AddEnergyDamageImmunity.HealingRate.DamageAsIs)
            {
                return damage;
            }
            if (healingRate != AddEnergyDamageImmunity.HealingRate.DamageDiv3)
            {
                throw new ArgumentOutOfRangeException();
            }
            return damage / 3;
        }

        // Token: 0x04008298 RID: 33432
        public DamageEnergyType EnergyType;

        // Token: 0x04008299 RID: 33433
        public bool HealOnDamage;

        public AddEnergyDamageImmunity.HealingRate m_HealRate;
    }
}
