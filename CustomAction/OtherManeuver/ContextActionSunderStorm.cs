using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Utility;
using System.Runtime.Remoting.Contexts;
using Kingmaker.UnitLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
using Mono.Cecil;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.Items;
using Kingmaker.Visual.Animation.Kingmaker;
using PrestigePlus.Blueprint.GrappleFeat;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class ContextActionSunderStorm : ContextAction
    {
        public override string GetCaption()
        {
            return "SunderStorm";
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            try
            {
                UnitEntityData unit = Target.Unit;
                if (unit == null)
                {
                    PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                    return;
                }
                UnitEntityData maybeCaster = Context.MaybeCaster;
                if (maybeCaster == null)
                {
                    PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                    return;
                }
                if (unit == maybeCaster)
                {
                    PFLog.Default.Error("Unit can't sunder themselves", Array.Empty<object>());
                    return;
                }
                if (!maybeCaster.CombatState.EngagedUnits.Contains(unit))
                {
                    return;
                }
                if (HitFirst && !RunAttackRule(maybeCaster, unit))
                {
                    return;
                }
                ItemEntityWeapon weapon;
                if (UseWeapon)
                {
                    weapon = maybeCaster.GetThreatHand()?.Weapon;
                    if (weapon == null) { weapon = maybeCaster.Body.EmptyHandWeapon; }
                }
                else
                {
                    weapon = maybeCaster.Body.EmptyHandWeapon;
                }
                var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, unit, weapon, 0) { };
                ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, type, AttackBonusRule);
                Rulebook.Trigger(ruleCombatManeuver);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }

        public CombatManeuver type = CombatManeuver.SunderArmor;
        public bool UseWeapon = false;
        public bool HitFirst = false;

        private static bool RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.GetThreatHandMelee();
            if (weapon == null) { return false; }
            var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
            maybeCaster.View.AnimationManager.Execute(attackAnimation);
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, weapon.Weapon, 0)
            {
                Reason = maybeCaster.Context,
                AutoHit = false,
                AutoCriticalThreat = false,
                AutoCriticalConfirmation = false,
                ExtraAttack = true,
                IsFullAttack = false,
                AttackNumber = 0,
                AttacksCount = 1
            };
            maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
            return ruleAttackWithWeapon.AttackRoll.IsHit;
        }
    }
}
