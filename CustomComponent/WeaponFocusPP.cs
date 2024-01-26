using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace PrestigePlus.CustomComponent
{
    internal class WeaponFocusPP : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x170025EE RID: 9710
        // (get) Token: 0x0600E943 RID: 59715 RVA: 0x003BCB1C File Offset: 0x003BAD1C
        public BlueprintWeaponType WeaponType;

        // Token: 0x0600E944 RID: 59716 RVA: 0x003BCB30 File Offset: 0x003BAD30
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon == null) return;
            if (evt.Weapon.Blueprint.Type == this.WeaponType)
            {
                evt.AddModifier(this.AttackBonus, base.Fact, this.Des);
            }
            else
            {
                evt.AddModifier(this.AttackBonus2, base.Fact, this.Des);
            }
        }

        // Token: 0x0600E945 RID: 59717 RVA: 0x003BCB81 File Offset: 0x003BAD81
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        // Token: 0x04009980 RID: 39296
        public int AttackBonus;
        public int AttackBonus2 = 0;
        // Token: 0x04009981 RID: 39297
        public ModifierDescriptor Des = ModifierDescriptor.UntypedStackable;
    }
}
