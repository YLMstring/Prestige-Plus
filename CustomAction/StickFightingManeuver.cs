using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Controllers.Combat.UnitCombatState;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomAction
{
    internal class StickFightingManeuver : ContextAction
    {
        public override string GetCaption()
        {
            return "StickFightingManeuver";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit, true);
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target, bool UseWeapon)
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
            }
            if (maneuver == CombatManeuver.None) { return; }
            ItemEntityWeapon weapon;
            if (UseWeapon)
            {
                weapon = caster.GetThreatHand()?.Weapon;
                if (weapon == null) { weapon = caster.Body.EmptyHandWeapon; }
            }
            else
            {
                weapon = caster.Body.EmptyHandWeapon;
            }
            var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, weapon, 0) { };
            Rulebook.Trigger(AttackBonusRule);
            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, AttackBonusRule);
            ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
            if (ruleCombatManeuver.Success)
            {
                if (maneuver != CombatManeuver.Grapple || caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.CheckCondition2(caster)) { return; }
                caster.Ensure<UnitPartGrappleInitiatorPP>().Init(target, CasterBuff, target.Context);
                target.Ensure<UnitPartGrappleTargetPP>().Init(caster, TargetBuff, caster.Context);
            }
        }

        private static readonly string SeizetheBullRushbuffGuid = "{FDD7D762-A448-48FB-B72C-709D14285FF6}";

        private static readonly string SeizetheDirtyBlindbuffGuid = "{6142C847-22F1-410F-A132-9545D7404F4A}";

        private static readonly string SeizetheDirtyEntanglebuffGuid = "{723AC061-8A31-485B-976D-2E233B5B4B9E}";

        private static readonly string SeizetheDirtySickenbuffGuid = "{4B94C35F-0F34-494F-9CF3-BB2BAD84FCF9}";

        private static readonly string SeizetheDisarmbuffGuid = "{0E686D2C-2CBB-43F4-A247-110A85B938C1}";

        private static readonly string SeizetheSunderbuffGuid = "{E4FD5F1E-A417-47CE-AF53-CD0FC91225B0}";

        private static readonly string SeizetheTripbuffGuid = "{8DFA0D89-D1D5-4A63-AF3E-37655D9935D5}";

        private static readonly string SeizetheGrapplebuffGuid = "{55BD97E6-84AB-4E2A-9F0D-EF8A730DC7CF}";

        private static BlueprintBuffReference BullRush = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheBullRushbuffGuid);
        private static BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyBlindbuffGuid);
        private static BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyEntanglebuffGuid);
        private static BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtySickenbuffGuid);
        private static BlueprintBuffReference Disarm = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDisarmbuffGuid);
        private static BlueprintBuffReference Grapple = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheGrapplebuffGuid);
        private static BlueprintBuffReference Sunder = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheSunderbuffGuid);
        private static BlueprintBuffReference Trip = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheTripbuffGuid);

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");

        private static BlueprintFeatureReference BullRushFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedBullRush.ToString());
        private static BlueprintFeatureReference DirtyFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDirtyTrick.ToString());
        private static BlueprintFeatureReference DisarmFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDisarm.ToString());
        private static BlueprintFeatureReference GrappleFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(ImprovedGrapple.StyleGuid);
        private static BlueprintFeatureReference SunderFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedSunder.ToString());
        private static BlueprintFeatureReference TripFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedTrip.ToString());
    }
}
