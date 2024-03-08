using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class LamashtuMadness : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (evt.Reason.Context != null && evt.Reason.Caster != null &&(evt.Reason.Context.SpellDescriptor & SpellDescriptor.Compulsion) != SpellDescriptor.None)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
                int dc = Owner.Stats.Wisdom.Bonus + 10 + Owner.Progression.CharacterLevel / 2;
                bool pass = Owner.Context.TriggerRule(new RuleSavingThrow(evt.Reason.Caster, Kingmaker.EntitySystem.Stats.SavingThrowType.Will, dc)).Success;
                if (!pass)
                {
                    IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                    factContextOwner?.RunActionInContext(this.Action, evt.Reason.Caster);
                }
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        public ActionList Action;
    }
}
