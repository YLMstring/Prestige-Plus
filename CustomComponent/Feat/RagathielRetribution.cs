using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class RagathielRetribution : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        // Token: 0x0600E9FE RID: 59902 RVA: 0x003BE6F0 File Offset: 0x003BC8F0
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            var caster = Buff.Context.MaybeCaster;
            if (evt.Target == base.Owner && evt.Initiator == caster)
            {
                BaseDamage weaponDamage = evt.DamageBundle.WeaponDamage;
                if (weaponDamage?.CreateTypeDescription()?.Physical?.Form != Kingmaker.Enums.Damage.PhysicalDamageForm.Slashing)
                {
                    return;
                }
                int bonus = caster.Progression.CharacterLevel / 2;
                weaponDamage.AddModifierTargetRelated(bonus, base.Fact, ModifierDescriptor.Sacred);
            }
        }

        // Token: 0x0600E9FF RID: 59903 RVA: 0x003BE751 File Offset: 0x003BC951
        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }
    }
}
