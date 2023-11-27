using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.Blueprint;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.Grapple;
using PrestigePlus.Grapple;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Kingmaker.UI.CanvasScalerWorkaround;
using static Kingmaker.Visual.CharacterSystem.CharacterStudio;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(UnitAttack), nameof(UnitAttack.TriggerAttackRule))]
    internal class ReplaceAttackFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static bool Prefix(ref UnitAttack __instance, ref AttackHandInfo attack)
        {
            try
            {
                var caster = __instance.Executor;
                var target = __instance.Target;
                if (!attack.Weapon.Blueprint.IsMelee) { return true; }
                if (caster.Body?.EmptyHandWeapon == null) { return true; }
                var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
                int penalty = -attack.AttackBonusPenalty + DualPenalty(caster, attack);
                AttackBonusRule.AddModifier(penalty, descriptor: ModifierDescriptor.Penalty);
                //ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                if (caster.HasFact(AerialBuff) && __instance.IsCharge)
                {
                    GameHelper.RemoveBuff(caster, AerialBuff);
                    if (caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.CheckCondition2(caster)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.Grapple, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    if (ruleCombatManeuver.Success)
                    {
                        caster.Ensure<UnitPartGrappleInitiatorPP>().Init(target, CasterBuff, target.Context);
                        target.Ensure<UnitPartGrappleTargetPP>().Init(caster, TargetBuff, caster.Context);
                    }
                    return false;
                }
                if (caster.HasFact(BullRush1) && __instance.IsCharge)
                {
                    GameHelper.RemoveBuff(caster, BullRush1);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.BullRush, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (!__instance.IsAttackFull) { return true; }
                if (caster.HasFact(BullRush2))
                {
                    GameHelper.RemoveBuff(caster, BullRush2);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.BullRush, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(BullRush3) && caster.HasFact(BullRush4))
                {
                    GameHelper.RemoveBuff(caster, BullRush3);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.BullRush, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Disarm1) || caster.HasFact(Disarm2))
                {
                    GameHelper.RemoveBuff(caster, Disarm1);
                    if (!caster.HasFact(Steal) && !caster.HasFact(Arm))
                    {
                        var threat = target.GetThreatHandMelee();
                        if (threat == null || threat.MaybeWeapon == null || threat.MaybeWeapon.Blueprint.IsNatural || threat.MaybeWeapon.Blueprint.IsUnarmed) { return true; }
                    }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.Disarm, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Sunder1) || caster.HasFact(Sunder2))
                {
                    GameHelper.RemoveBuff(caster, Sunder1);
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff) && !caster.HasFact(SunderReal)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.SunderArmor, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Trip1) || caster.HasFact(Trip2))
                {
                    GameHelper.RemoveBuff(caster, Trip1);
                    if (target.Descriptor.State.Prone.Active) { return true; }
                    if (!target.CanBeKnockedOff() && !caster.HasFact(Ace)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.Trip, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(DirtyTrick1))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick1);
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.DirtyTrickBlind, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(DirtyTrick2))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick2);
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.DirtyTrickEntangle, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(DirtyTrick3))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick3);
                    RuleCombatManeuver ruleCombatManeuver = new(caster, target, CombatManeuver.DirtyTrickSickened, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Logger.Error("Failed to replace attack.", ex); return true; }
        }

        private static BlueprintBuffReference Disarm1 = BlueprintTool.GetRef<BlueprintBuffReference>("{59ED0B02-A211-4323-9717-CBE6A1CD6846}");
        private static BlueprintBuffReference Disarm2 = BlueprintTool.GetRef<BlueprintBuffReference>("{49890773-D237-46D0-8C68-B666B0503523}");

        private static BlueprintBuffReference Sunder1 = BlueprintTool.GetRef<BlueprintBuffReference>("{B5FD1419-CED2-4450-9110-C2620CD88AC7}");
        private static BlueprintBuffReference Sunder2 = BlueprintTool.GetRef<BlueprintBuffReference>("{57476F97-D68A-4707-9B12-B2D7C5687F39}");

        private static BlueprintBuffReference Trip1 = BlueprintTool.GetRef<BlueprintBuffReference>("{8C577D9F-BA5B-4974-B91D-F94FF28A8501}");
        private static BlueprintBuffReference Trip2 = BlueprintTool.GetRef<BlueprintBuffReference>("{43ADBC25-7972-45D6-A3AB-356F16199D50}");

        private static BlueprintBuffReference BullRush1 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.BullRushbuffGuid);
        private static BlueprintBuffReference BullRush2 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.BullRushQuickbuffGuid);
        private static BlueprintBuffReference BullRush3 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.BullRushAngrybuffGuid);
        private static BlueprintBuffReference BullRush4 = BlueprintTool.GetRef<BlueprintBuffReference>(BullRushFeats.KnockbackbuffGuid);

        private static BlueprintBuffReference AerialBuff = BlueprintTool.GetRef<BlueprintBuffReference>(AerialAssault.Stylebuff2Guid);

        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{C5F4DDFE-CA2E-4309-90BB-1BB5C0F32E78}");
        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");

        private static BlueprintBuffReference DirtyTrick1 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.DirtyBlindQuickbuffGuid);
        private static BlueprintBuffReference DirtyTrick2 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.DirtyEntangleQuickbuffGuid);
        private static BlueprintBuffReference DirtyTrick3 = BlueprintTool.GetRef<BlueprintBuffReference>(ReplaceAttack.DirtySickenQuickbuffGuid);
        public static int DualPenalty(UnitEntityData unit, AttackHandInfo attack)
        {
            ItemEntityWeapon maybeWeapon = unit.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = unit.Body.SecondaryHand.MaybeWeapon;
            bool flag2 = unit.Descriptor.State.AdditionalFeatures.ShieldMaster;
            bool flag3 = maybeWeapon != null && maybeWeapon.IsShield || maybeWeapon2 != null && maybeWeapon2.IsShield;
            int second = 0;
            bool flag = attack.Weapon.Blueprint.IsNatural && unit.Descriptor.State.Features.MythicDemonNaturalAttacksNoSecondary;
            var rule = new RuleCalculateWeaponStats(unit, attack.Weapon, null, null);
            Rulebook.Trigger(rule);
            if (rule.IsSecondary && !flag)
            {
                int bonus2 = (attack.Weapon.Blueprint.IsNatural && unit.State.Features.ReduceSecondaryNaturalAttackPenalty) ? -2 : -5;
                second += bonus2;
            }
            if (attack.Weapon == null || maybeWeapon == null || maybeWeapon2 == null || maybeWeapon.Blueprint.IsNatural || maybeWeapon2.Blueprint.IsNatural || maybeWeapon == unit.Body.EmptyHandWeapon || maybeWeapon2 == unit.Body.EmptyHandWeapon || maybeWeapon != attack.Weapon && maybeWeapon2 != attack.Weapon || flag2 && flag3)
            {
                return second;
            }
            int rank = 0;
            if (unit.HasFact(TWF)) { rank = 2; }
            int num = rank > 1 ? unit.HasFact(Mythic) ? 0 : -2 : -4;
            int num2 = rank > 1 ? unit.HasFact(Mythic) ? 0 : -2 : -8;
            int num3 = attack.Weapon == maybeWeapon ? num : num2;
            UnitPartWeaponTraining unitPartWeaponTraining = unit.Get<UnitPartWeaponTraining>();
            bool flag4 = unit.State.Features.EffortlessDualWielding && unitPartWeaponTraining != null && unitPartWeaponTraining.IsSuitableWeapon(maybeWeapon2);
            if (!maybeWeapon2.Blueprint.IsLight && !maybeWeapon.Blueprint.Double && !maybeWeapon2.IsShield && !flag4)
            {
                num3 -= 2;
            }
            return num3 + second;
        }

        private static readonly BlueprintFeatureReference TWF = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFighting.ToString());
        private static readonly BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFightingMythicFeat.ToString());

        private static readonly BlueprintFeatureReference SunderReal = BlueprintTool.GetRef<BlueprintFeatureReference>(GreaterSunderTabletop.GreaterSunderTabletopGuid);
        private static readonly BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedTrip.AceTripGuid);

        private static readonly BlueprintFeatureReference Steal = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedDisarm.AceDisarmGuid);
        private static readonly BlueprintFeatureReference Arm = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedDisarm.ArmBindGuid);
    }
}
