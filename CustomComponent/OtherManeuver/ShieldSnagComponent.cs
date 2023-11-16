using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using PrestigePlus.CustomAction.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.OtherManeuver
{
    internal class ShieldSnagComponent : UnitBuffComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            var weapon = evt.Weapon;
            var category = weapon?.Blueprint?.Category;
            if (category != WeaponCategory.SpikedHeavyShield && category != WeaponCategory.SpikedLightShield && category != WeaponCategory.WeaponHeavyShield && category != WeaponCategory.WeaponLightShield) { return; }
            if (evt.AttackRoll.IsHit)
            {
                SweepManeuver.ActManeuver(Owner, evt.Target, 0, type, weapon, evt.AttackRoll.D20);
            }
        }

        public CombatManeuver type = CombatManeuver.Disarm;
    }
}
