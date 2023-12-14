using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker;
using PrestigePlus.CustomAction.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class GorumUnarmed : ContextAction
    {
        public override string GetCaption()
        {
            return "GorumUnarmed";
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
                RunAttackRule(maybeCaster, unit);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.Body.EmptyHandWeapon;
            var twohand = maybeCaster.GetThreatHandMelee()?.Weapon?.HoldInTwoHands;
            if (weapon != null && twohand == true)
            {
                var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                maybeCaster.View.AnimationManager.Execute(attackAnimation);
                int penalty = 8;
                if (maybeCaster.HasFact(FeatureRefs.TwoWeaponFighting.Reference))
                {
                    penalty = 2;
                }
                if (maybeCaster.HasFact(FeatureRefs.TwoWeaponFightingMythicFeat.Reference))
                {
                    penalty = 0;
                }
                RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, penalty)
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
            }
        }
    }
}