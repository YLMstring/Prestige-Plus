using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class SymbolicWeaponBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (cat != null && cat == evt.Weapon.Blueprint.Category)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(GetBonus(), base.Runtime, Des));
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (cat != null && cat == evt.Weapon.Blueprint.Category)
            {
                evt.AddModifier(GetBonus(), base.Fact, Des);
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {

        }

        private int GetBonus()
        {
            int level = Owner.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(Sentinel.ArchetypeGuid));
            if (level >= 9)
            {
                return 4;
            }
            else if (level >= 6)
            {
                return 3;
            }
            else if (level >= 3)
            {
                return 2;
            }
            return 1;
        }

        public override void OnActivate()
        {
            cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
        }

        public override void OnDeactivate()
        {
            cat = null;
        }

        private WeaponCategory? cat = null;
        public ModifierDescriptor Des = ModifierDescriptor.Sacred;
    }
}
