using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.RuleSystem.Rules.Damage;
using BlueprintCore.Blueprints.References;
using Kingmaker.RuleSystem;
using PrestigePlus.CustomComponent.PrestigeClass;
using Kingmaker.Utility;
using Kingmaker;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class VexGiantComp : UnitBuffComponentDelegate<VexGiantComp.ComponentData>, ITargetRulebookHandler<RuleCalculateCMB>, IRulebookHandler<RuleCalculateCMB>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ITargetRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>
    {
        void IRulebookHandler<RuleCalculateCMB>.OnEventAboutToTrigger(RuleCalculateCMB evt)
        {
            if (IsGiant() && evt.Initiator == Buff?.Context?.MaybeCaster)
            {
                evt.AddModifier(4, Fact, ModifierDescriptor.Insight);
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.RuleAttackWithWeapon?.IsAttackOfOpportunity == true && IsGiant() && evt.Target == Buff?.Context?.MaybeCaster)
            {
                evt.AutoMiss = true;
                Owner.CombatState.AttackOfOpportunityCount++;
            }
        }

        void IRulebookHandler<RuleCalculateCMB>.OnEventDidTrigger(RuleCalculateCMB evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {

        }

        private bool IsGiant()
        {
            var caster = Buff?.Context?.MaybeCaster;
            return AbilityTargetIsSuitableMountSize.CanMount(caster, Owner);
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                return;
            }
            var type = evt.ParentRule?.AttackRoll?.AttackType;
            if ((type == AttackType.Melee || type == AttackType.Touch) && IsGiant() && evt.Initiator == Buff?.Context?.MaybeCaster)
            {
                var dam = evt.ParentRule.DamageBundle?.First();
                if (dam != null)
                {
                    evt.ParentRule.Add(dam.CreateTypeDescription().CreateDamage(new DiceFormula(1, DiceType.D6), 0));
                    Data.LastUseTime = Game.Instance.TimeController.GameTime;
                }
                
            }
        }

        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {

        }
        public class ComponentData
        {
            public TimeSpan LastUseTime;
        }
    }
}

