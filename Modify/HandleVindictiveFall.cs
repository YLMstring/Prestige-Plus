using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.Modify
{
    internal class HandleVindictiveFall : UnitFactComponentDelegate, IKnockOffHandler, IUnitSubscriber, ISubscriber
    {
        void IKnockOffHandler.HandleKnockOff(UnitEntityData initiator, UnitEntityData target)
        {
            if (target != Owner) { return; }
            List<UnitEntityData> list = new List<UnitEntityData> { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in Owner.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (!unit.Descriptor.State.IsDead && Owner.CombatState.EngagedUnits.Contains(unit))
                {
                    list.Add(unit);
                }
            }
            if (list.Count == 0) { return; }
            list.Sort((UnitEntityData u1, UnitEntityData u2) => u1.SqrDistanceTo(Owner).CompareTo(u2.SqrDistanceTo(Owner)));
            ActManeuver(Owner, list.First());
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            var maneuver = CombatManeuver.Trip;
            ItemEntityWeapon weapon = caster.Body.EmptyHandWeapon;
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };          
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, AttackBonusRule);
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);           
        }
    }
}
