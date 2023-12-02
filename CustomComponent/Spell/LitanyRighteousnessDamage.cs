using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class LitanyRighteousnessDamage : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (HasGoodAura(evt.ParentRule?.AttackRoll?.Initiator))
            {
                var list = new List<BaseDamage>();
                foreach (BaseDamage dmg in evt.ParentRule.DamageBundle)
                {
                    list.Add(dmg);
                }
                foreach (BaseDamage dmg in list)
                {
                    evt.ParentRule.Add(dmg);
                }
                if (Owner.HasFact(FeatureRefs.SubtypeEvil.Reference.Get()))
                {
                    Fact.RunActionInContext(Actions);
                }
            }
        }
        public ActionList Actions = ActionsBuilder.New().ApplyBuff(BuffRefs.DazzledBuff.ToString(), ContextDuration.VariableDice(Kingmaker.RuleSystem.DiceType.D4, 1, 0)).Build();
        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {
            
        }

        private bool HasGoodAura(UnitEntityData unit)
        {
            if (unit == null) { return false; }
            if (unit.Progression.GetClassLevel(CharacterClassRefs.PaladinClass.Reference.Get()) > 0)
            {
                return true;
            }
            if (unit.Progression.GetClassLevel(CharacterClassRefs.ClericClass.Reference.Get()) > 0 || unit.Progression.GetClassLevel(CharacterClassRefs.WarpriestClass.Reference.Get()) > 0)
            {
                if (unit.HasFact(FeatureRefs.GoodDomainAllowed.Reference.Get()))
                {
                    return true;
                }
            }
            if (unit.HasFact(FeatureRefs.SubtypeGood.Reference.Get()))
            {
                return true;
            }
            return false;
        }
    }
}
