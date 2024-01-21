using Kingmaker;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ArazniObedience : UnitFactComponentDelegate<ArazniObedience>, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var caster = evt.Reason?.Caster;
            if (caster?.HasFact(buff) == true)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(4, base.Runtime, ModifierDescriptor.Profane));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            var type = evt.Reason?.Ability?.Blueprint.Type;
            var caster = evt.Reason?.Caster;
            if (!evt.IsPassed && caster != null && (type == AbilityType.Spell || type == AbilityType.SpellLike))
            {
                GameHelper.ApplyBuff(caster, buff, new Rounds?(10.Rounds()));
            }
        }

        public BlueprintBuff buff;
    }
}
