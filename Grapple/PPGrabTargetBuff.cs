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
    internal class PPGrabTargetBuff : PPGrabBuffBase, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber
    {
        

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

        // Token: 0x0400826C RID: 33388
        [SerializeField]
        private int m_PinnedACBonus = -4;
    }
}
