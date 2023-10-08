using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using PrestigePlus.Grapple;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Enums;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Feats;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionDoKiThrow : ContextAction
    {
        public override string GetCaption()
        {
            return "Throw";
        }

        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (Target == null) return;
            Vector3 point = Target.Point;
            if (Target.Unit != null)
            { 
                point = Target.Unit.Position;
                var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, Target.Unit, maybeCaster.Body.EmptyHandWeapon, 0) { };
                int penalty = -4;
                AttackBonusRule.AddModifier(penalty, descriptor: ModifierDescriptor.Penalty);
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, Target.Unit, CombatManeuver.BullRush, AttackBonusRule);
                ruleCombatManeuver = (Target.Unit.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                if (ruleCombatManeuver.Success && Target.Unit.CanBeKnockedOff())
                {
                    Target.Unit.Descriptor.State.Prone.ShouldBeActive = true;
                    EventBus.RaiseEvent(delegate (IKnockOffHandler h)
                    {
                        h.HandleKnockOff(maybeCaster, Target.Unit);
                    }, true);
                }
            }
            if (maybeCaster.Get<UnitPartKiThrow>())
            {
                var target = maybeCaster.Get<UnitPartKiThrow>().Target;
                if (target != null)
                {
                    target.Value.Position = point;
                    if (maybeCaster.HasFact(Grapple))
                    {
                        RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, Target.Unit, CombatManeuver.Grapple, null);
                        ruleCombatManeuver = (Target.Unit.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                        if (ruleCombatManeuver.Success)
                        {
                            maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(target, CasterBuff, Target.Unit.Context);
                            Target.Unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, TargetBuff, maybeCaster.Context);
                        }
                    }
                    if (maybeCaster.HasFact(Hit))
                    {
                        RunAttackRule(maybeCaster, Target.Unit);
                    }
                }
                maybeCaster.Remove<UnitPartKiThrow>();
            }
        }

        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.Body.EmptyHandWeapon;
            if (weapon == null) { return; }
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, weapon, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = true,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
        }
        private static BlueprintBuffReference Grapple = BlueprintTool.GetRef<BlueprintBuffReference>("{98558112-717B-456D-838F-EFF5061F4D38}");
        private static BlueprintBuffReference Hit = BlueprintTool.GetRef<BlueprintBuffReference>("{36DD4519-6A34-4A90-85A5-D77A2309A20D}");

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
    }
}
