using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class SkilledRiderSave : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (Owner.GetRider() == null) return;
            var num = Owner.GetRider().Stats.Charisma.Bonus;
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(num, base.Runtime));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(num, base.Runtime));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(num, base.Runtime));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }
    }
}