using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.Modify
{
    internal class RogueAnotherDay : UnitFactComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleDealDamage>.OnEventAboutToTrigger(RuleDealDamage evt)
        {
            
        }

        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {
            if (CheckReduceBelowZero(evt) && CheckAttackType(evt.DamageBundle)) 
            {
                int num = 0;
                foreach (DamageValue damageValue in evt.ResultList)
                {
                    num += damageValue.FinalValue;
                }
                int healValue = num;
                if (healValue > 0)
                {
                    Rulebook.Trigger(new RuleHealDamage(base.Owner, base.Owner, evt.Result));
                }
                GameHelper.ApplyBuff(Owner, stagger, new Rounds?(1.Rounds()));
                Fact.RunActionInContext(Action, evt.Target);
                GameHelper.RemoveBuff(Owner, Cooldown);
            }
        }

        private bool CheckAttackType(IDamageBundleReadonly damage)
        {
            return damage.Weapon != null && damage.WeaponDamage != null && damage.Weapon.Blueprint.AttackType == AttackType.Melee;
        }

        private bool CheckReduceBelowZero(RuleDealDamage evt)
        {
            return evt.Target.HPLeft <= 0 && evt.Target.HPLeft + evt.Result > 0;
        }

        private static BlueprintBuffReference Cooldown = BlueprintTool.GetRef<BlueprintBuffReference>("A812D3F2-BBF8-4EF1-A627-9FBFCAC810F8");
        private static BlueprintBuffReference stagger = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Staggered.ToString());
        
        private static BlueprintAbilityResourceReference Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>("{D6CD7849-DF81-4528-90C8-A78350CAEBC1}");
        public ActionList Action = ActionsBuilder.New().ContextSpendResource(Resource, ContextValues.Constant(1)).Build();
    }
}
