using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.Animation.Kingmaker;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Archetype;
using Kingmaker.Designers;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class OneTouchAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "OneTouchAttack";
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
        private void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit)
        {
            var weapon = maybeCaster.Body.EmptyHandWeapon;
            if (anyweapon)
            {
                weapon = maybeCaster.GetThreatHand()?.Weapon ?? weapon;
            }
            if (weapon != null)
            {
                var attackAnimation = maybeCaster.View.AnimationManager.CreateHandle(UnitAnimationType.SpecialAttack);
                maybeCaster.View.AnimationManager.Execute(attackAnimation);
                RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, 0)
                {
                    Reason = maybeCaster.Context,
                    AutoHit = false,
                    AutoCriticalThreat = false,
                    AutoCriticalConfirmation = false,
                    ExtraAttack = true,
                    IsFullAttack = true,
                    AttackNumber = 0,
                    AttacksCount = 1
                };
                int num = maybeCaster.Progression.GetClassLevel(CharacterClassRefs.MonkClass.Reference);
                if (!ki) { num /= 2; }
                if (anyweapon) { num = 0; }
                ruleAttackWithWeapon.AddTemporaryModifier(maybeCaster.Stats.AdditionalDamage.AddModifier(num));
                maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
                GameHelper.RemoveBuff(maybeCaster, Buff);
                GameHelper.RemoveBuff(maybeCaster, Buff2);
            }
        }

        public bool anyweapon = false;
        public bool ki = false;
        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(ManeuverMaster.OneTouchBuff2Guid);
        private static BlueprintBuffReference Buff2 = BlueprintTool.GetRef<BlueprintBuffReference>(WeaponMaster.UnstoppableStrikeBuff2Guid);
    }
}