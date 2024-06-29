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
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using PrestigePlus.Blueprint;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.GrappleFeat;
using PrestigePlus.Blueprint.MythicFeat;
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
using static Pathfinding.Util.RetainedGizmos;

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
                if (caster.HasFact(Assault) && caster.HasFact(BuffRefs.ChargeBuff.Reference))
                {
                    __instance.IsCharge = true;
                }
                if (!attack.Weapon.Blueprint.IsMelee) { return true; }
                if (caster.Body?.EmptyHandWeapon == null) { return true; }
                var challenged = target.GetFact(BuffRefs.CavalierChallengeBuffTarget.Reference) as Buff;
                if (challenged?.Context?.MaybeCaster == caster && caster.HasFact(Hammer) && !caster.HasFact(HammerCoolDown) && __instance.IsAttackFull)
                {
                    GameHelper.ApplyBuff(caster, HammerCoolDown, new Rounds?(1.Rounds()));
                    var maneuver = CombatManeuver.None;
                    if (caster.HasFact(Sunder))
                    {
                        maneuver = CombatManeuver.SunderArmor;
                    }
                    else if (caster.HasFact(Grapple))
                    {
                        maneuver = CombatManeuver.Grapple;
                        if (caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.HasFreeHand(caster))
                        {
                            maneuver = CombatManeuver.None;
                        }
                    }
                    if (maneuver != CombatManeuver.None)
                    {
                        var AttackBonusRule2 = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
                        ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule2);
                        TriggerManeuver(caster, target, AttackBonusRule2, maneuver);
                    }
                }
                if (challenged?.Context?.MaybeCaster == caster && caster.HasFact(Seal) && !caster.HasFact(SealCoolDown) && __instance.IsAttackFull)
                {
                    GameHelper.ApplyBuff(caster, SealCoolDown, new Rounds?(1.Rounds()));
                    var maneuver = CombatManeuver.None;
                    if (caster.HasFact(BullRush))
                    {
                        maneuver = CombatManeuver.BullRush;
                    }
                    else if (caster.HasFact(Trip))
                    {
                        maneuver = CombatManeuver.Trip;
                    }
                    if (maneuver != CombatManeuver.None)
                    {
                        var AttackBonusRule3 = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
                        ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule3);
                        TriggerManeuver(caster, target, AttackBonusRule3, maneuver);
                    }
                }
                var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
                int penalty = -attack.AttackBonusPenalty + DualPenalty(caster, attack);
                AttackBonusRule.AddModifier(penalty, descriptor: ModifierDescriptor.Penalty);
                ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                if (caster.HasFact(Flurry) && !caster.HasFact(FlurryCoolDown) && __instance.IsAttackFull)
                {
                    GameHelper.ApplyBuff(caster, FlurryCoolDown, new Rounds?(1.Rounds()));
                    var maneuver = CombatManeuver.None;
                    if (caster.HasFact(BullRush) && caster.HasFact(BullRushFeat))
                    {
                        maneuver = CombatManeuver.BullRush;
                    }
                    else if (caster.HasFact(DirtyBlind) && caster.HasFact(DirtyFeat))
                    {
                        maneuver = CombatManeuver.DirtyTrickBlind;
                    }
                    else if (caster.HasFact(DirtyEntangle) && caster.HasFact(DirtyFeat))
                    {
                        maneuver = CombatManeuver.DirtyTrickEntangle;
                    }
                    else if (caster.HasFact(DirtySicken) && caster.HasFact(DirtyFeat))
                    {
                        maneuver = CombatManeuver.DirtyTrickSickened;
                    }
                    else if (caster.HasFact(Disarm) && caster.HasFact(DisarmFeat))
                    {
                        maneuver = CombatManeuver.Disarm;
                    }
                    else if (caster.HasFact(Sunder) && caster.HasFact(SunderFeat))
                    {
                        maneuver = CombatManeuver.SunderArmor;
                    }
                    else if (caster.HasFact(Trip) && caster.HasFact(TripFeat))
                    {
                        maneuver = CombatManeuver.Trip;
                    }
                    else if (caster.HasFact(Grapple) && caster.HasFact(GrappleFeat))
                    { 
                        maneuver = CombatManeuver.Grapple;
                        if (caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.HasFreeHand(caster)) 
                        {
                            maneuver = CombatManeuver.None;
                        }
                    }
                    if (maneuver != CombatManeuver.None)
                    {
                        TriggerManeuver(caster, target, AttackBonusRule, maneuver);
                        if (caster.HasFact(Flurry8))
                        {
                            TriggerManeuver(caster, target, AttackBonusRule, maneuver);
                        }
                        if (caster.HasFact(Flurry15))
                        {
                            TriggerManeuver(caster, target, AttackBonusRule, maneuver);
                        }
                    }
                    return false;
                }
                if (caster.HasFact(AerialBuff) && caster.HasFact(GrappleFeat) && __instance.IsCharge)
                {
                    GameHelper.RemoveBuff(caster, AerialBuff);
                    if (caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.HasFreeHand(caster)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.Grapple);
                    return false;
                }
                if (caster.HasFact(BullRush1) && __instance.IsCharge)
                {
                    GameHelper.RemoveBuff(caster, BullRush1);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.BullRush);
                    return false;
                }
                if (!__instance.IsAttackFull) { return true; }
                if (caster.HasFact(BullRush2))
                {
                    GameHelper.RemoveBuff(caster, BullRush2);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.BullRush);
                    return false;
                }
                if (caster.HasFact(BullRush3) && caster.HasFact(BullRush4))
                {
                    GameHelper.RemoveBuff(caster, BullRush3);
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.BullRush);
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
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.Disarm);
                    return false;
                }
                if (caster.HasFact(Sunder1) || caster.HasFact(Sunder2))
                {
                    GameHelper.RemoveBuff(caster, Sunder1);
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff) && !caster.HasFact(SunderReal)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.SunderArmor);
                    return false;
                }
                if (caster.HasFact(Trip1) || caster.HasFact(Trip2))
                {
                    GameHelper.RemoveBuff(caster, Trip1);
                    if (target.Descriptor.State.Prone.Active) { return true; }
                    if (!target.CanBeKnockedOff() && !caster.HasFact(Ace)) { return true; }
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.Trip);
                    return false;
                }
                if (caster.HasFact(DirtyTrick1))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick1);
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.DirtyTrickBlind);
                    return false;
                }
                if (caster.HasFact(DirtyTrick2))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick2);
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.DirtyTrickEntangle);
                    return false;
                }
                if (caster.HasFact(DirtyTrick3))
                {
                    GameHelper.RemoveBuff(caster, DirtyTrick3);
                    TriggerManeuver(caster, target, AttackBonusRule, CombatManeuver.DirtyTrickSickened);
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
            if (unit.HasFact(Mantis) && maybeWeapon?.Blueprint.Category == WeaponCategory.SawtoothSabre && maybeWeapon2?.Blueprint.Category == WeaponCategory.SawtoothSabre)
            {
                return 0;
            }
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

        public static void TriggerManeuver(UnitEntityData caster, UnitEntityData target, RuleCalculateAttackBonus AttackBonusRule, CombatManeuver type)
        {
            RuleCombatManeuver ruleCombatManeuver = new(caster, target, type, AttackBonusRule);
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success && type == CombatManeuver.Grapple)
            {
                caster.Ensure<UnitPartGrappleInitiatorPP>().Init(target, CasterBuff, target.Context);
                target.Ensure<UnitPartGrappleTargetPP>().Init(caster, TargetBuff, caster.Context);
            }
        }

        private static readonly BlueprintFeatureReference Mantis = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Achaekek3Guid);

        private static readonly BlueprintFeatureReference TWF = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFighting.ToString());
        private static readonly BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFightingMythicFeat.ToString());

        private static readonly BlueprintFeatureReference SunderReal = BlueprintTool.GetRef<BlueprintFeatureReference>(GreaterSunderTabletop.GreaterSunderTabletopGuid);
        private static readonly BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedTrip.AceTripGuid);

        private static readonly BlueprintFeatureReference Steal = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedDisarm.AceDisarmGuid);
        private static readonly BlueprintFeatureReference Arm = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedDisarm.ArmBindGuid);

        private static readonly BlueprintFeatureReference Assault = BlueprintTool.GetRef<BlueprintFeatureReference>(GiganticAssault.FeatGuid);

        private static readonly BlueprintFeatureReference Flurry = BlueprintTool.GetRef<BlueprintFeatureReference>(ManeuverMaster.FlurryFeatGuid);
        private static readonly BlueprintBuffReference FlurryCoolDown = BlueprintTool.GetRef<BlueprintBuffReference>(ManeuverMaster.FlurryCoolDownbuffGuid);
        private static readonly BlueprintBuffReference Flurry8 = BlueprintTool.GetRef<BlueprintBuffReference>(ManeuverMaster.Flurry8buffGuid);
        private static readonly BlueprintBuffReference Flurry15 = BlueprintTool.GetRef<BlueprintBuffReference>(ManeuverMaster.Flurry15buffGuid);

        private static readonly BlueprintFeatureReference Hammer = BlueprintTool.GetRef<BlueprintFeatureReference>(Inquisition.HammerChallengeGuid);
        private static readonly BlueprintBuffReference HammerCoolDown = BlueprintTool.GetRef<BlueprintBuffReference>(Inquisition.ChallengeAuraBuffGuid);

        private static readonly BlueprintFeatureReference Seal = BlueprintTool.GetRef<BlueprintFeatureReference>(Inquisition.SealChallengeGuid);
        private static readonly BlueprintBuffReference SealCoolDown = BlueprintTool.GetRef<BlueprintBuffReference>(Inquisition.SealChallengeAuraBuffGuid);

        private static readonly string SeizetheBullRushbuffGuid = "{FDD7D762-A448-48FB-B72C-709D14285FF6}";
        private static readonly string SeizetheDirtyBlindbuffGuid = "{6142C847-22F1-410F-A132-9545D7404F4A}";
        private static readonly string SeizetheDirtyEntanglebuffGuid = "{723AC061-8A31-485B-976D-2E233B5B4B9E}";
        private static readonly string SeizetheDirtySickenbuffGuid = "{4B94C35F-0F34-494F-9CF3-BB2BAD84FCF9}";
        private static readonly string SeizetheDisarmbuffGuid = "{0E686D2C-2CBB-43F4-A247-110A85B938C1}";
        private static readonly string SeizetheSunderbuffGuid = "{E4FD5F1E-A417-47CE-AF53-CD0FC91225B0}";
        private static readonly string SeizetheTripbuffGuid = "{8DFA0D89-D1D5-4A63-AF3E-37655D9935D5}";
        private static readonly string SeizetheGrapplebuffGuid = "{55BD97E6-84AB-4E2A-9F0D-EF8A730DC7CF}";

        private static readonly BlueprintBuffReference BullRush = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheBullRushbuffGuid);
        private static readonly BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyBlindbuffGuid);
        private static readonly BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyEntanglebuffGuid);
        private static readonly BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtySickenbuffGuid);
        private static readonly BlueprintBuffReference Disarm = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDisarmbuffGuid);
        private static readonly BlueprintBuffReference Grapple = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheGrapplebuffGuid);
        private static readonly BlueprintBuffReference Sunder = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheSunderbuffGuid);
        private static readonly BlueprintBuffReference Trip = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheTripbuffGuid);

        public static readonly BlueprintFeatureReference BullRushFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedBullRush.ToString());
        public static readonly BlueprintFeatureReference DirtyFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDirtyTrick.ToString());
        public static readonly BlueprintFeatureReference DisarmFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDisarm.ToString());
        public static readonly BlueprintFeatureReference GrappleFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(ImprovedGrapple.StyleGuid);
        public static readonly BlueprintFeatureReference SunderFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedSunder.ToString());
        public static readonly BlueprintFeatureReference TripFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedTrip.ToString());
    }
}
