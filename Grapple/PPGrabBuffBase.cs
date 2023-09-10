using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.Grapple
{
    internal class PPGrabBuffBase : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>
    {
        public override void OnTurnOn()
        {
            base.OnTurnOn();
            base.Owner.Stats.Dexterity.AddModifier(this.m_DexterityBonus, base.Runtime, ModifierDescriptor.Penalty);
        }

        // Token: 0x0600C1F3 RID: 49651 RVA: 0x00327CF8 File Offset: 0x00325EF8
        public override void OnTurnOff()
        {
            base.OnTurnOff();
            base.Owner.Stats.Dexterity.RemoveModifiersFrom(base.Runtime);
        }

        // Token: 0x0600C1F4 RID: 49652 RVA: 0x00327D1C File Offset: 0x00325F1C
        public void OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            RuleCombatManeuver ruleCombatManeuver = Rulebook.CurrentContext.LastEvent<RuleCombatManeuver>();
            if (ruleCombatManeuver != null && ruleCombatManeuver.Type == CombatManeuver.Grapple)
            {
                return;
            }
            evt.AddModifier(this.m_AttackRollBonus, base.Fact, ModifierDescriptor.Penalty);
        }

        // Token: 0x0600C1F5 RID: 49653 RVA: 0x00327D55 File Offset: 0x00325F55
        public void OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
        }

        // Token: 0x0600C1FE RID: 49662 RVA: 0x00327F2A File Offset: 0x0032612A
        public void OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {
        }

        // Token: 0x0600C1FF RID: 49663 RVA: 0x00327F2C File Offset: 0x0032612C
        public void OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
            RuleCalculateAttacksCount.AttacksCount primaryHand = evt.Result.PrimaryHand;
            RuleCalculateAttacksCount.AttacksCount secondaryHand = evt.Result.SecondaryHand;
            DisableTwoHandedAttack(primaryHand);
            DisableTwoHandedAttack(secondaryHand);
        }

        // Token: 0x0600C200 RID: 49664 RVA: 0x00327F5B File Offset: 0x0032615B
        private static void DisableTwoHandedAttack(RuleCalculateAttacksCount.AttacksCount attacksCount)
        {
            if (attacksCount.IsTwoHanded)
            {
                attacksCount.HasteAttacks = 0;
                attacksCount.PenalizedAttacks = 0;
                attacksCount.AdditionalAttacks = 0;
            }
        }
        // Token: 0x0400826A RID: 33386
        [SerializeField]
        public int m_AttackRollBonus = -2;

        // Token: 0x0400826B RID: 33387
        [SerializeField]
        public int m_DexterityBonus = -4;
    }
}
