using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class NocticulaDefensive : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            foreach (UnitGroupMemory.UnitInfo unitInfo in Owner.Memory.UnitsList)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (unit.DistanceTo(Owner) <= 5.Feet().Meters && !unit.Descriptor.State.IsDead && unit.IsAlly(Owner))
                {
                    return;
                }
            }
            evt.AddModifier(1, base.Fact, Kingmaker.Enums.ModifierDescriptor.Morale);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            foreach (UnitGroupMemory.UnitInfo unitInfo in Owner.Memory.UnitsList)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (unit.DistanceTo(Owner) <= 5.Feet().Meters && !unit.Descriptor.State.IsDead && unit.IsAlly(Owner))
                {
                    return;
                }
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(1, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(1, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(1, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }
    }
}
