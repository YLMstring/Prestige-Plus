using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker;
using UnityEngine;

namespace PrestigePlus.Grapple
{
    internal class PPGrabTargetBuff : PPGrabBuffBase, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookHandler<RuleCheckTargetFlatFooted>, IRulebookHandler<RuleCheckTargetFlatFooted>, ITargetRulebookSubscriber
    {
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

        // Token: 0x0600C201 RID: 49665 RVA: 0x00327F7C File Offset: 0x0032617C
        public void OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = base.Owner.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null || !UnitPartGrappleTargetPP.IsPinned)
            {
                return;
            }
            evt.AddModifier(this.m_PinnedACBonus, base.Fact, ModifierDescriptor.Penalty);
        }

        // Token: 0x0600C202 RID: 49666 RVA: 0x00327FB5 File Offset: 0x003261B5
        public void OnEventDidTrigger(RuleCalculateAC evt)
        {
        }

        // Token: 0x0600C203 RID: 49667 RVA: 0x00327FB7 File Offset: 0x003261B7
        public void OnEventAboutToTrigger(RuleCheckTargetFlatFooted evt)
        {
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = base.Owner.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null || !UnitPartGrappleTargetPP.IsPinned)
            {
                return;
            }
            evt.ForceFlatFooted = true;
        }

        // Token: 0x0600C204 RID: 49668 RVA: 0x00327FC0 File Offset: 0x003261C0
        public void OnEventDidTrigger(RuleCheckTargetFlatFooted evt)
        {
        }

        // Token: 0x0400826C RID: 33388
        [SerializeField]
        private int m_PinnedACBonus = -4;
    }
}
