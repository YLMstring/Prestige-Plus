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
using Kingmaker.Items;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class KasathaExtraAttack : ContextAction
    {
        public override string GetCaption()
        {
            return "KasathaExtraAttack";
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
                if (maybeCaster.Body.CurrentHandEquipmentSetIndex == 1 || maybeCaster.Body.IsPolymorphed)
                {
                    return;
                }
                var sets = unit.Body.HandsEquipmentSets;
                var weapon = maybeCaster.Body.EmptyHandWeapon;
                ItemEntityWeapon maybeWeapon = sets[1]?.PrimaryHand.Weapon ?? weapon;
                ItemEntityWeapon maybeWeapon2 = sets[1]?.SecondaryHand.Weapon ?? weapon;
                RunAttackRule(maybeCaster, unit, maybeWeapon);
                RunAttackRule(maybeCaster, unit, maybeWeapon2);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit, ItemEntityWeapon weapon)
        {
            if (weapon != null && !weapon.HoldInTwoHands)
            {
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
                    IsFullAttack = true,
                    AttackNumber = 0,
                    AttacksCount = 1
                };
                maybeCaster.Context.TriggerRule(ruleAttackWithWeapon);
            }
        }
    }
}
