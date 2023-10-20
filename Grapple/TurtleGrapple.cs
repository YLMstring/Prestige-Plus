using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker.Designers;
using Kingmaker.Utility;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;

namespace PrestigePlus.Grapple
{
    internal class TurtleGrapple : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {

        public void RunGrapple(RuleAttackRoll evt)
        {
            UnitEntityData unit = evt.Initiator;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = Owner;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (unit == maybeCaster)
            {
                PFLog.Default.Error("Unit can't grapple themselves", Array.Empty<object>());
                return;
            }
            if (maybeCaster.Get<UnitPartGrappleInitiatorPP>())
            {
                PFLog.Default.Error("Caster already grapple", Array.Empty<object>());
                return;
            }
            if (unit.Get<UnitPartGrappleTargetPP>())
            {
                PFLog.Default.Error("Target already grappled", Array.Empty<object>());
                return;
            }
            var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, unit, maybeCaster.Body.EmptyHandWeapon, 0) { };
            AttackBonusRule.AddModifier(-2, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
            Rulebook.Trigger(AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, CombatManeuver.Grapple, AttackBonusRule);
            ruleCombatManeuver = (Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            GameHelper.ApplyBuff(maybeCaster, TurtleCoolDown, duration: new Rounds?(1.Rounds()));
            if (ruleCombatManeuver.Success)
            {
                maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, CasterBuff, base.Context);
                unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, TargetBuff, base.Context);
            }
        }

        public void TurtleAction(RuleAttackRoll evt)
        {
            UnitEntityData maybeCaster = Owner;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (!maybeCaster.HasFact(Turtle) || maybeCaster.HasFact(TurtleCoolDown))
            {
                return;
            }
            RunGrapple(evt);
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit) { return; }
            TurtleAction(evt);
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
        public bool isAway;

        private static BlueprintBuffReference Turtle = BlueprintTool.GetRef<BlueprintBuffReference>("{F86754BC-0488-4DBE-9371-17A70CF3BAE2}");
        private static BlueprintBuffReference TurtleCoolDown = BlueprintTool.GetRef<BlueprintBuffReference>("{5D7C0ACD-C537-45F7-A5E8-97949278F72F}");
    }
}
