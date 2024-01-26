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
    internal class WeaponTypeDamageBonusPP : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {

        public BlueprintWeaponType WeaponType;

        // Token: 0x0600E999 RID: 59801 RVA: 0x003BDBD5 File Offset: 0x003BBDD5
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon.Blueprint.Type == this.WeaponType)
            {
                evt.AddDamageModifier(this.DamageBonus, base.Fact, Des);
            }
            else
            {
                evt.AddDamageModifier(this.DamageBonus2, base.Fact, Des);
            }
        }

        // Token: 0x0600E99A RID: 59802 RVA: 0x003BDC0F File Offset: 0x003BBE0F
        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }

        // Token: 0x040099C6 RID: 39366
        public int DamageBonus;
        public int DamageBonus2 = 0;
        public ModifierDescriptor Des = ModifierDescriptor.UntypedStackable;
    }
}