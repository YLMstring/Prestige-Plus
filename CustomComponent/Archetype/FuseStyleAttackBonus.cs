using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class FuseStyleAttackBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            int num = 0;
            foreach (ActivatableAbility activatableAbility in base.Owner.ActivatableAbilities.RawFacts)
            {
                if (activatableAbility.Blueprint.Group == ActivatableAbilityGroup.CombatStyle && (activatableAbility.IsStarted || activatableAbility.IsOn))
                {
                    num += activatableAbility.Blueprint.WeightInGroup;
                }
            }
            evt.AddModifier(num, base.Fact);
        }

        // Token: 0x0600E945 RID: 59717 RVA: 0x003BCB81 File Offset: 0x003BAD81
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}
