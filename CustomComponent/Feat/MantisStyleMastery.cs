using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class MantisStyleMastery : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RulePrepareDamage>, IRulebookHandler<RulePrepareDamage>
    {
        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            ItemEntityWeapon maybeWeapon = Owner.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = Owner.Body.SecondaryHand.MaybeWeapon;
            int bonus = 0;
            if (maybeWeapon?.Blueprint.Category == WeaponCategory.Falcata && maybeWeapon2?.Blueprint.Category == WeaponCategory.Falcata)
            {
                foreach (var mod in evt.m_ModifiableBonus.Modifiers)
                {
                    if (mod.Fact?.Blueprint?.GetComponent<TwoWeaponFightingAttackPenalty>() != null)
                    {
                        bonus += mod.Value;
                    }
                }
                evt.AddModifier(bonus, base.Fact, ModifierDescriptor.UntypedStackable);
            }
        }

        // Token: 0x0600E8E3 RID: 59619 RVA: 0x003BB797 File Offset: 0x003B9997
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public void OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            if (this.ConditionsChecker(evt))
            {
                BaseDamage weaponDamage = evt.DamageBundle.WeaponDamage;
                if (weaponDamage == null)
                {
                    return;
                }
                int bonus = 0;
                foreach (var damage in evt.ParentRule.DamageBundle)
                {
                    if (damage != null && damage.Sneak)
                    {
                        bonus += damage.Dice.BaseFormula.Rolls;
                    }
                }
                BaseDamage baseDamage = weaponDamage.CreateTypeDescription().CreateDamage(new DiceFormula(0, DiceType.Zero), bonus);
                baseDamage.Sneak = true;
                baseDamage.Precision = true;
                evt.Add(baseDamage);
            }
        }

        // Token: 0x0600E422 RID: 58402 RVA: 0x003ABC0C File Offset: 0x003A9E0C
        public void OnEventDidTrigger(RulePrepareDamage evt)
        {
        }

        // Token: 0x0600E423 RID: 58403 RVA: 0x003ABC10 File Offset: 0x003A9E10
        private bool ConditionsChecker(RulePrepareDamage evt)
        {
            ItemEntityWeapon weapon = evt.DamageBundle.Weapon;
            if (weapon?.Blueprint.Category == WeaponCategory.Falcata)
            {
                return true;
            }
            return false;
        }
    }
}
