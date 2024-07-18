using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.MVVM._VM.ServiceWindows.Spellbook.KnownSpells;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Owlcat.Runtime.Core.Utils;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ThrowPunchStuff : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeaponResolve>, IRulebookHandler<RuleAttackWithWeaponResolve>, ISubscriber, IInitiatorRulebookSubscriber, IUnitSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IInitiatorRulebookHandler<RuleCalculateAttacksCount>, IRulebookHandler<RuleCalculateAttacksCount>
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
                var abilitydata = new AbilityData(AbilityRefs.ExpeditiousRetreat.Reference.Get(), Owner);
                var cont = abilitydata.CreateExecutionContext(Owner, null);
                if (Rulebook.Trigger(new RuleSpellResistanceCheck(cont, evt.Target)).IsSpellResisted) { return; }
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
            CharacterStats stats = Owner.Stats;
            var value = ((stats.Charisma >= stats.Intelligence && stats.Charisma >= stats.Wisdom) ? StatType.Charisma : ((stats.Wisdom > stats.Intelligence) ? StatType.Wisdom : StatType.Intelligence));
            evt.AttackBonusStat = value;
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventAboutToTrigger(RuleCalculateAttacksCount evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttacksCount>.OnEventDidTrigger(RuleCalculateAttacksCount evt)
        {
            if (Owner.HasFact(Mythic)) { return; }
            RuleCalculateAttacksCount.AttacksCount primaryHand = evt.Result.PrimaryHand;
            RuleCalculateAttacksCount.AttacksCount secondaryHand = evt.Result.SecondaryHand;
            primaryHand.PenalizedAttacks = 0;
            primaryHand.HasteAttacks = 0;
            primaryHand.AdditionalAttacks = Math.Min(primaryHand.AdditionalAttacks, 1);
            secondaryHand.PenalizedAttacks = 0;
            secondaryHand.HasteAttacks = 0;
            secondaryHand.AdditionalAttacks = 0;
        }
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(MageHandTrick.MageHandMythicFeatGuid);
    }
}
