using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker;
using PrestigePlus.Blueprint.CombatStyle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using PrestigePlus.CustomComponent.Grapple;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class DisarmExtraAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "DisarmExtraAttack";
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
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (attacktype == 1 && !ConditionTwoFreeHand.CheckCondition2(caster))
            {
                return;
            }
            if (!isSwift)
            {
                AttackHit(caster, target);
            }
            else if (caster.HasSwiftAction())
            {
                caster.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0);
                AttackHit(caster, target);
                return;
            }
        }

        private bool AttackHit(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            ItemEntityWeapon weapon = null;
            switch (attacktype)
            {
                case 0:
                    weapon = maybeCaster.Body.EmptyHandWeapon;
                    break;
                case 1:
                    weapon = maybeCaster.Body.CurrentHandsEquipmentSet.PrimaryHand?.Weapon;
                    weapon ??= maybeCaster.Body.EmptyHandWeapon;
                    break;
                case 2:
                    weapon = maybeCaster.Body.CurrentHandsEquipmentSet.SecondaryHand?.Weapon;
                    break;
            }
            if (weapon == null || !weapon.Blueprint.IsMelee) { return false; }
            var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
            maybeCaster.View.AnimationManager.Execute(attackAnimation);
            RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, 0)
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

        public bool isSwift = true;
        public int attacktype = 0;
    }
}