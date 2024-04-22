using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.Feat;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ShootingStarAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "ShootingStarAttack";
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
                var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                maybeCaster.View.AnimationManager.Execute(attackAnimation);
                if (RunAttackRule(maybeCaster, unit, false))
                {
                    int time = UnityEngine.Random.Range(0, 4);
                    if (time > 0) { RunAttackRule(maybeCaster, unit, true); }
                    if (time > 1) { RunAttackRule(maybeCaster, unit, true); }
                    if (time > 2) { RunAttackRule(maybeCaster, unit, true); }
                }
                GameHelper.RemoveBuff(maybeCaster, Buff);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
        private bool RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit, bool auto)
        {
            var weapon = maybeCaster.Body.PrimaryHand.Weapon;
            if (weapon != null)
            { 
                RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, 0)
                {
                    Reason = maybeCaster.Context,
                    AutoHit = auto,
                    AutoCriticalThreat = false,
                    AutoCriticalConfirmation = false,
                    ExtraAttack = true,
                    IsFullAttack = false,
                    AttackNumber = 0,
                    AttacksCount = 1
                };
                var rule = maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
                return rule.AttackRoll.IsHit;
            }
            return false;
        }

        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(ShootingStar.ShootingStarBuff2Guid);
    }
}
