using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.Grapple
{
    internal class PPGrabBuffBase : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>
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

        public void OnEventAboutToTrigger(RuleCastSpell evt)
        {
            AbilityData spell = evt.Spell;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = base.Owner.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null || !UnitPartGrappleTargetPP.IsPinned)
            {
                return;
            }
            if (spell.Blueprint == null)
            {
                return;
            }
            if (spell.Blueprint.Type != Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Spell && spell.Blueprint.Type != Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.SpellLike)
            {
                return;
            }
            RuleCalculateCMB ruleCalculateCMB = new RuleCalculateCMB(UnitPartGrappleTargetPP.Initiator, base.Owner, CombatManeuver.Grapple);
            Rulebook.Trigger<RuleCalculateCMB>(ruleCalculateCMB);
            int result = ruleCalculateCMB.Result;
            RuleCalculateAbilityParams ruleCalculateAbilityParams = new RuleCalculateAbilityParams(base.Owner, spell);
            Rulebook.Trigger<RuleCalculateAbilityParams>(ruleCalculateAbilityParams);
            int spellLevel = ruleCalculateAbilityParams.Result.SpellLevel;
            int value = 10 + result + spellLevel;
            RuleCheckConcentration ruleCheckConcentration = new RuleCheckConcentration(base.Owner, spell);
            ruleCheckConcentration.CustomDC = new int?(value);
            base.Context.TriggerRule<RuleCheckConcentration>(ruleCheckConcentration);
            if (!ruleCheckConcentration.Success)
            {
                evt.ForceFail = true;
            }
        }

        // Token: 0x0600C1FD RID: 49661 RVA: 0x00327F28 File Offset: 0x00326128
        public void OnEventDidTrigger(RuleCastSpell evt)
        {
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
