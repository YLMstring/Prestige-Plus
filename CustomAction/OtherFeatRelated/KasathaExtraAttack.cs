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
using Kingmaker.Items.Slots;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.UnitLogic.Parts;
using static Kingmaker.EntitySystem.Persistence.Versioning.PlayerUpgraderOnlyActions.EnsureUniqueItems;

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
                if (maybeCaster.Body.IsPolymorphed)
                {
                    return;
                }
                var weapons = new List<ItemEntityWeapon> { };
                foreach (var slot in maybeCaster.Body.EquipmentSlots)
                {
                    if (slot is HandSlot && slot != maybeCaster.Body.PrimaryHand && slot != maybeCaster.Body.SecondaryHand)
                    {
                        if (slot.HasItem && slot.Item is ItemEntityWeapon weapon)
                        {
                            weapons.Add(weapon);
                            Logger.Info(weapon.Name);
                        }
                    }
                }
                if (!weapons.Any()) return;
                var wep = maybeCaster.Body.AddAdditionalLimb(weapons[0].Blueprint, null);
                RunAttackRule(maybeCaster, unit, maybeCaster.Body.AdditionalLimbs[wep].MaybeWeapon);
                maybeCaster.Body.RemoveAdditionalLimb(wep);
                if (weapons.Count() < 2) return;
                wep = maybeCaster.Body.AddAdditionalLimb(weapons[1].Blueprint, null);
                RunAttackRule(maybeCaster, unit, maybeCaster.Body.AdditionalLimbs[wep].MaybeWeapon);
                maybeCaster.Body.RemoveAdditionalLimb(wep);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
        private static void RunAttackRule(UnitEntityData maybeCaster, UnitEntityData unit, ItemEntityWeapon weapon)
        {
            if (weapon != null && !weapon.HoldInTwoHands)
            {
                int rank = 0;
                if (maybeCaster.HasFact(FeatureRefs.TwoWeaponFighting.Reference)) { rank = 2; }
                int num2 = rank > 1 ? maybeCaster.HasFact(FeatureRefs.TwoWeaponFightingMythicFeat.Reference) ? 0 : -2 : -8;
                UnitPartWeaponTraining unitPartWeaponTraining = maybeCaster.Get<UnitPartWeaponTraining>();
                bool flag4 = maybeCaster.State.Features.EffortlessDualWielding && unitPartWeaponTraining != null && unitPartWeaponTraining.IsSuitableWeapon(weapon);
                if (!weapon.Blueprint.IsLight && !weapon.Blueprint.Double && !weapon.IsShield && !flag4)
                {
                    num2 -= 2;
                }
                RuleAttackWithWeapon ruleAttackWithWeapon = new(maybeCaster, unit, weapon, -num2)
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
