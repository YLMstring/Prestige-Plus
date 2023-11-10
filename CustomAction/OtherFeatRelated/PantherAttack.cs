using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Visual.Animation.Kingmaker;
using PrestigePlus.Blueprint.GrappleFeat;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.CombatStyle;
using Kingmaker.Designers;
using Kingmaker.Controllers.Units;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class PantherAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "PantherAttack";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public void ActManeuver(UnitEntityData caster, UnitEntityData target)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (caster == null || !caster.View.IsMoving())
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (caster.HasFact(Parry) && !isparry) { return; }
            if (!caster.HasFact(Parry) && isparry) { return; }
            int time = caster.Stats.Wisdom.Bonus;
            if (caster.HasFact(Claw) && time >= 1)
            {
                var used = caster.GetFact(CastBuff)?.GetRank();
                if (used == null || used < time)
                {
                    GameHelper.ApplyBuff(caster, CastBuff, new Rounds?(1.Rounds()));
                    if (AttackHit(caster, target) && isparry)
                    {
                        GameHelper.ApplyBuff(target, TargetBuff, new Rounds?(1.Rounds()));
                        UnitLifeController.ForceTickOnUnit(target);
                    }
                    return;
                }
            }
            if (caster.HasSwiftAction())
            {
                caster.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0);
                if (AttackHit(caster, target) && isparry)
                {
                    GameHelper.ApplyBuff(target, TargetBuff, new Rounds?(1.Rounds()));
                    UnitLifeController.ForceTickOnUnit(target);
                }
            }
        }

        private static bool AttackHit(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.Body.EmptyHandWeapon;
            if (weapon == null) { return false; }
            var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
            maybeCaster.View.AnimationManager.Execute(attackAnimation);
            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(maybeCaster, unit, weapon, 0)
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
            return maybeCaster.Context.TriggerRule(ruleAttackWithWeapon).AttackRoll.IsHit;
        }

        public bool isparry = false;
        private static BlueprintFeatureReference Parry = BlueprintTool.GetRef<BlueprintFeatureReference>(PantherStyle.ParryGuid);
        private static BlueprintFeatureReference Claw = BlueprintTool.GetRef<BlueprintFeatureReference>(PantherStyle.ClawGuid);
        private static BlueprintBuffReference CastBuff = BlueprintTool.GetRef<BlueprintBuffReference>(PantherStyle.StylebuffGuid2);
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(PantherStyle.StylebuffGuid3);
    }
}

