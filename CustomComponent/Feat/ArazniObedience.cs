using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ArazniObedience : UnitFactComponentDelegate<ArazniObedience.ComponentData>, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (Data.units.Count == 0) return;
            if (base.Data.LastUseTime + 10.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                Data.units.Clear();
                return;
            }
            var caster = evt.Reason?.Caster;
            if (caster != null && Data.units.Contains(caster))
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
                Data.units.Add(caster);
                Data.LastUseTime = Game.Instance.TimeController.GameTime;
            }
        }

        public class ComponentData
        {
            public List<UnitEntityData> units = new() { };
            public TimeSpan LastUseTime = default;
        }
    }
}
