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
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.Blueprints.Classes.Spells;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class UnchainedHeartComp : UnitFactComponentDelegate<ArazniObedience>, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (evt.Reason.Context != null && (evt.Reason.Context.SpellDescriptor & this.SpellDescriptor) != Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.None)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(1 + Fact.GetRank(), base.Runtime));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            bool flag = evt.Reason.Context != null && (evt.Reason.Context.SpellDescriptor & this.SpellDescriptor) != Kingmaker.Blueprints.Classes.Spells.SpellDescriptor.None;
            var caster = evt.Reason?.Caster;
            if (evt.IsPassed && caster != null && evt.Type == Kingmaker.EntitySystem.Stats.SavingThrowType.Will && flag)
            {
                int round = UnityEngine.Random.Range(1, 5);
                GameHelper.ApplyBuff(caster, buff, new Rounds?(round.Rounds()));
            }
        }

        public BlueprintBuff buff;
        public SpellDescriptorWrapper SpellDescriptor;
    }
}
