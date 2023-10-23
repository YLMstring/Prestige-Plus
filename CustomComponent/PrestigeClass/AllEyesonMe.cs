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
using Kingmaker.UnitLogic.Parts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Components;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AllEyesonMe : UnitBuffComponentDelegate, ISubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookHandler<RuleCheckTargetFlatFooted>, IRulebookHandler<RuleCheckTargetFlatFooted>, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null || evt.Initiator == null || maybeCaster == evt.Initiator)
            {
                return;
            }
            evt.AddModifier(-2, Fact, ModifierDescriptor.Penalty);
        }

        void IRulebookHandler<RuleCheckTargetFlatFooted>.OnEventAboutToTrigger(RuleCheckTargetFlatFooted evt)
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null || evt.Initiator == null || maybeCaster == evt.Initiator)
            {
                return;
            }
            evt.ForceFlatFooted = true;
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleCheckTargetFlatFooted>.OnEventDidTrigger(RuleCheckTargetFlatFooted evt)
        {

        }
    }
}
