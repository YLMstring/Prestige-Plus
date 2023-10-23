using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class ForceSneakDamage : UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleAttackRoll>,
        IRulebookHandler<RuleAttackRoll>,
        ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookHandler<RulePrepareDamage>,
        IRulebookHandler<RulePrepareDamage>
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (opp)
            {
                evt.IsSneakAttack = true;
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.IsAttackOfOpportunity)
            {
                opp = true;
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            opp = false;
        }

        void IRulebookHandler<RulePrepareDamage>.OnEventAboutToTrigger(RulePrepareDamage evt)
        {
            
        }

        void IRulebookHandler<RulePrepareDamage>.OnEventDidTrigger(RulePrepareDamage evt)
        {
            if (!opp) return;
            if (evt.ParentRule == null) return;
            if (evt.ParentRule.DamageBundle == null) return;
            foreach (var damage in evt.ParentRule.DamageBundle)
            {
                if (damage != null && damage.Sneak) { damage.Precision = false; }
            }
        }

        private bool opp = false;
    }
}
