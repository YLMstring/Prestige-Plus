using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using UnityEngine;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.RuleSystem.Rules;
using PrestigePlus.Feats;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Designers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Blueprints.Items.Weapons;

namespace PrestigePlus.Grapple
{
    internal class KnotGrapple : ContextAction
    {
        public override string GetCaption()
        {
            return "Knot Grapple";
        }

        public void RunActionOriginal()
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            BlueprintItemWeapon Weapon = BlueprintTool.GetRef<BlueprintItemWeaponReference>(ItemWeaponRefs.RayItem.ToString()); 
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, null, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
            };
            //ruleAttackWithWeapon.AttackRoll.AttackType = AttackType.RangedTouch;
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = maybeCaster.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP != null) { return; }
            if (!ruleAttackWithWeapon.AttackRoll.IsHit) { return; }
            RunGrapple();
        }

        public void RunGrapple()
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
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
            var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, unit, maybeCaster.Body.EmptyHandWeapon, 0){ };
            if (isAway)
            {
                AttackBonusRule.AddModifier(maybeCaster.Descriptor.Progression.MythicLevel, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
            }
            else
            {
                AttackBonusRule.AddModifier(5, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
            }
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, CombatManeuver.Grapple, AttackBonusRule);
            //if (!isAway) { ruleCombatManeuver.ReplaceBaseStat = Kingmaker.EntitySystem.Stats.StatType.Wisdom; }
            ruleCombatManeuver = (Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success)
            {
                maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, CasterBuff, base.Context);
                unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, TargetBuff, base.Context);
            }
        }

        public override void RunAction()
        {
            RunGrapple();
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D4DD258E-B9F1-42D1-9BD0-ADBD217AFE23}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
        public bool isAway = true;
    }
}
