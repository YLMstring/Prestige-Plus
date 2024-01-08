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
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.CustomComponent.Grapple;
using PrestigePlus.Grapple;
using static Kingmaker.Visual.CharacterSystem.CharacterStudio;
using static RootMotion.FinalIK.IKSolverVR;
using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Buffs;

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
                var caster = maybeCaster;
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
                    weapon ??= maybeCaster.Body.EmptyHandWeapon;
                }
                else
                {
                    weapon = maybeCaster.Body.EmptyHandWeapon;
                }
                var AttackBonusRule = new RuleCalculateAttackBonus(maybeCaster, unit, weapon, 0) { };
                ContextActionCombatTrickery.TriggerMRule(ref AttackBonusRule);
                var maneuver = type;
                if (UseAnyType)
                {
                    maneuver = CombatManeuver.None;
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
                        if (caster.Get<UnitPartGrappleInitiatorPP>() || unit.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.HasFreeHand(caster)) { return; }
                        maneuver = CombatManeuver.Grapple;
                    }
                }
                if (maneuver == CombatManeuver.None) { return; }
                RuleCombatManeuver ruleCombatManeuver = new(maybeCaster, unit, maneuver, AttackBonusRule);
                ruleCombatManeuver = Rulebook.Trigger(ruleCombatManeuver);
                if (ruleCombatManeuver.Success && maneuver == CombatManeuver.Grapple)
                {
                    caster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, CasterBuff, unit.Context);
                    unit.Ensure<UnitPartGrappleTargetPP>().Init(caster, TargetBuff, caster.Context);
                }
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }

        public CombatManeuver type = CombatManeuver.SunderArmor;
        public bool UseWeapon = false;
        public bool HitFirst = false;
        public bool UseAnyType = false;
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

        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");
    }
}
