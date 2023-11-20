using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.GameModes.GameModeType;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class ViciousAimComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            var favor = Owner.Get<UnitPartFavoredEnemy>()?.Entries;
            if (favor == null || evt.Weapon?.Blueprint.IsRanged != true) { return; }
            int bonus = 0;
            foreach (var enemy in favor)
            {
                if (enemy.Bonus > bonus)
                {
                    bonus = enemy.Bonus;
                }
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus / 2, Fact, ModifierDescriptor.FavoredEnemy));
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            var favor = Owner.Get<UnitPartFavoredEnemy>()?.Entries;
            if (favor == null || evt.Weapon?.Blueprint.IsRanged != true) { return; }
            int bonus = 0;
            foreach (var enemy in favor)
            {
                if (enemy.Bonus > bonus)
                {
                    bonus = enemy.Bonus;
                }
            }
            evt.AddModifier(bonus / 2, Fact, ModifierDescriptor.FavoredEnemy);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            
        }
    }
}
