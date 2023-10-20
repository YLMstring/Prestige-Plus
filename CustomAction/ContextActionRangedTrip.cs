using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
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
using Kingmaker.Utility;
using AK.Wwise;
using Kingmaker.Items.Slots;
using static Kingmaker.RuleSystem.Rules.RuleCalculateAttacksCount;
using static Kingmaker.Visual.Animation.IKController;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionRangedTrip : ContextAction
    {
        public override string GetCaption()
        {
            return "RangedTrip";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
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
            ItemEntityWeapon weapon = caster.GetThreatHand()?.Weapon;
            if (weapon == null) { weapon = caster.Body.EmptyHandWeapon; }
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };
            int penalty = -2;
            if (caster.DistanceTo(target) > 30.Feet().Meters)
            {
                penalty -= 2;
            }
            if (!caster.HasFact(AceFeat))
            {
                AttackBonusRule.AddModifier(penalty, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
            }
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, AttackBonusRule);
            ruleCombatManeuver.ReplaceBaseStat = Kingmaker.EntitySystem.Stats.StatType.Dexterity;
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success)
            {
                RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target, weapon, 0)
                {
                    Reason = caster.Context,
                    AutoHit = true,
                    AutoCriticalThreat = false,
                    AutoCriticalConfirmation = false,
                    ExtraAttack = true,
                    IsFullAttack = false,
                    AttackNumber = 0,
                    AttacksCount = 1
                };
                caster.Context.TriggerRule(ruleAttackWithWeapon);
            }
        }

        private static BlueprintFeatureReference AceFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedTrip.AceTripGuid);
    }
}
