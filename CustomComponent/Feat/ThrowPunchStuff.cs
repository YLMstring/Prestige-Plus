using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
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
using static Kingmaker.GameModes.GameModeType;
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
                var abilitydata = GetData(Owner);
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

        public static AbilityData GetData(UnitEntityData caster)
        {
            ClassData classData1 = null;
            foreach (ClassData classData2 in caster.Descriptor.Progression.Classes)
            {
                if (classData2.Spellbook != null)
                {
                    if (classData1 == null) { classData1 = classData2; }
                    else if (classData2.Level + classData2.Spellbook.CasterLevelModifier > classData1.Level + classData1.Spellbook.CasterLevelModifier)
                    {
                        classData1 = classData2;
                    }
                }
            }
            return new AbilityData(AbilityRefs.ExpeditiousRetreat.Reference.Get(), caster, null, classData1?.Spellbook);
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon?.Blueprint.Category != WeaponCategory.UnarmedStrike) { return; }
            var abilitydata = GetData(Owner);
            var cont = abilitydata.CreateExecutionContext(Owner, null);
            int num = cont.Params.CasterLevel;
            ModifierDescriptor des = ModifierDescriptor.UntypedStackable;
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
