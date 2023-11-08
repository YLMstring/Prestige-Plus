using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ThrowPunchStuff : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>, IRulebookHandler<RuleAttackWithWeaponResolve>, ISubscriber, IInitiatorRulebookSubscriber, IUnitSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventAboutToTrigger(RuleAttackWithWeaponResolve evt)
        {
            if (evt.AttackWithWeapon?.Weapon?.Blueprint.Category == Kingmaker.Enums.WeaponCategory.UnarmedStrike)
            {
                List<BaseDamage> list = TempList.Get<BaseDamage>();
                foreach (BaseDamage damage in evt.Damage.DamageBundle)
                {
                    list.Add(this.ChangeType(damage));
                }
                evt.Damage.Remove((BaseDamage _) => true);
                foreach (BaseDamage damage2 in list)
                {
                    evt.Damage.Add(damage2);
                }
            }
        }

        void IRulebookHandler<RuleAttackWithWeaponResolve>.OnEventDidTrigger(RuleAttackWithWeaponResolve evt)
        {
            
        }

        private BaseDamage ChangeType(BaseDamage damage)
		{
            ForceDamage Damage2 = new(new ModifiableDiceFormula(damage.Dice), damage.Bonus);
            Damage2.CopyFrom(damage);
			return Damage2;
		}

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon?.Blueprint.Category != WeaponCategory.UnarmedStrike) { return; }
            int num = 0;
            ModifierDescriptor des = ModifierDescriptor.UntypedStackable;
            foreach (ClassData classData2 in Owner.Descriptor.Progression.Classes)
            {
                if (classData2.Spellbook != null)
                {
                    num += Math.Max(classData2.Level + classData2.Spellbook.CasterLevelModifier, 0);
                }
            }
            num -= Owner.Stats.BaseAttackBonus;
            if (num < 0)
            {
                des = ModifierDescriptor.Penalty;
            }
            evt.AddModifier(num, base.Fact, des);
            ModifiableValueAttributeStat modifiableValueAttributeStat = base.Owner.Stats.GetStat(evt.AttackBonusStat) as ModifiableValueAttributeStat;
            CharacterStats stats = Owner.Stats;
            var value = ((stats.Charisma >= stats.Intelligence && stats.Charisma >= stats.Wisdom) ? StatType.Charisma : ((stats.Wisdom > stats.Intelligence) ? StatType.Wisdom : StatType.Intelligence));
            ModifiableValueAttributeStat modifiableValueAttributeStat2 = base.Owner.Stats.GetStat(value) as ModifiableValueAttributeStat;
            bool flag = modifiableValueAttributeStat2 != null && modifiableValueAttributeStat != null && modifiableValueAttributeStat2.Bonus >= modifiableValueAttributeStat.Bonus;
            if (flag)
            {
                evt.AttackBonusStat = value;
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            
        }
    }
}
