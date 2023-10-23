using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Entities;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AsavirInspiringLeader : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            BaseDamage first = evt.DamageBundle.First;
            if (first == null || evt.DamageBundle.Weapon == null)
            {
                return;
            }
            first.AddModifierTargetRelated(Bonus(), Fact, ModifierDescriptor.Morale);
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            evt.AddModifier(Bonus(), Fact, ModifierDescriptor.Morale);
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {

        }

        private int Bonus()
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                return 0;
            }
            return maybeCaster.Stats.Charisma.Bonus;
        }
    }
}
