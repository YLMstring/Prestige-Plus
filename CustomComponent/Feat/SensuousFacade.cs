using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SensuousFacade : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            foreach (ModifiableValue.Modifier modifier in Owner.Stats.AC.Modifiers)
            {
                if (modifier.ModDescriptor == ModifierDescriptor.Armor) { return; }
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(Owner.Stats.Charisma.Bonus, base.Runtime, ModifierDescriptor.Profane));
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            foreach (ModifiableValue.Modifier modifier in Owner.Stats.AC.Modifiers)
            {
                if (modifier.ModDescriptor == ModifierDescriptor.Armor) { return; }
            }
            evt.AddModifier(Owner.Stats.Charisma.Bonus, base.Fact, ModifierDescriptor.Profane);
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {
            
        }
    }
}
