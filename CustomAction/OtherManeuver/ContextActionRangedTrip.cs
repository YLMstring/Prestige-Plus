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
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.Blueprint.SpecificManeuver;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class ContextActionRangedTrip : ContextAction
    {
        public override string GetCaption()
        {
            return "RangedTrip";
        }

        public override void RunAction()
        {
            if (ActManeuver(Context.MaybeCaster, Target.Unit, true) && maneuver == CombatManeuver.Disarm && Context.MaybeCaster.HasFact(Mythic))
            {
                ActManeuver(Context.MaybeCaster, Target.Unit, false);
            }
        }

        public bool ActManeuver(UnitEntityData caster, UnitEntityData target, bool attack)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return false;
            }
            ItemEntityWeapon weapon = caster.Body.PrimaryHand.Weapon;
            weapon ??= caster.Body.EmptyHandWeapon;
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };
            int penalty = -2;
            if (caster.DistanceTo(target) > 30.Feet().Meters)
            {
                penalty -= 2;
            }
            if (!caster.HasFact(BlueprintTool.GetRef<BlueprintFeatureReference>(Ace)))
            {
                AttackBonusRule.AddModifier(penalty, descriptor: Kingmaker.Enums.ModifierDescriptor.Penalty);
            }
            if (maneuver == CombatManeuver.Disarm && caster.HasFact(Mythic))
            {
                AttackBonusRule.AddModifier(caster.Progression.MythicLevel, descriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable);
            }
            ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new(caster, target, maneuver, AttackBonusRule)
            {
                ReplaceBaseStat = Kingmaker.EntitySystem.Stats.StatType.Dexterity
            };
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success && attack)
            {
                RuleAttackWithWeapon ruleAttackWithWeapon = new(caster, target, weapon, 0)
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
            return ruleCombatManeuver.Success;
        }

        public CombatManeuver maneuver = CombatManeuver.Trip;
        public string Ace = RangedTrip.AceTripGuid;
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedDisarm.MythicDisarmGuid);
    }
}
