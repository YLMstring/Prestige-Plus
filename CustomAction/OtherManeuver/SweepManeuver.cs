using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.SpecificManeuver;
using Kingmaker.Designers;
using Kingmaker.Utility;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class SweepManeuver : ContextAction
    {
        public override string GetCaption()
        {
            return "SweepManeuver";
        }

        public override void RunAction()
        {
            if (Target.Unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (Context.MaybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (Context.MaybeCaster.HasFact(CasterBuff)) { return; }
            List<UnitEntityData> list = new() { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in Context.MaybeCaster.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (unit != Target.Unit && !unit.Descriptor.State.IsDead && unit.DistanceTo(Target.Unit) <= 8.Feet().Meters)
                {
                    list.Add(unit);
                }
            }
            if (list.Count == 0) { return; }
            list.Sort((u1, u2) => u1.SqrDistanceTo(Target.Unit).CompareTo(u2.SqrDistanceTo(Target.Unit)));
            GameHelper.ApplyBuff(Context.MaybeCaster, CasterBuff, new Rounds?(1.Rounds()));
            ActManeuver(Context.MaybeCaster, list.First(), -5);
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target, int penalty)
        {
            var maneuver = CombatManeuver.Disarm;
            ItemEntityWeapon weapon = caster.Body.EmptyHandWeapon;
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };
            AttackBonusRule.AddModifier(penalty, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
            ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new(caster, target, maneuver, AttackBonusRule);
            _ = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
        }
        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>(RangedDisarm.SweepingDisarmBuffGuid);
    }
}