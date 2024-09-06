using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ZonKuthonSpellComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var spell = evt.Reason?.Ability?.Blueprint;
            if (spell != null && DestructiveSpellComp.HasDamage(spell))
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(2, base.Runtime, ModifierDescriptor.Sacred));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(2, base.Runtime, ModifierDescriptor.Sacred));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(2, base.Runtime, ModifierDescriptor.Sacred));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }
    }
}
