using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using PrestigePlus.Grapple;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Designers.Mechanics.Buffs;
using static Kingmaker.UI.CanvasScalerWorkaround;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Runtime.Remoting.Contexts;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;

namespace PrestigePlus.Maneuvers
{
    [HarmonyPatch(typeof(UnitAttackOfOpportunity), nameof(UnitAttackOfOpportunity.OnAction))]
    internal class SeizetheOpFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static bool Prefix(ref UnitAttackOfOpportunity __instance, ref UnitCommand.ResultType __result)
        {
            try
            {
                var caster = __instance.Executor;
                var target = __instance.Target;
                var maneuver = CombatManeuver.None;
                if (target.Descriptor.State.IsDead || !caster.HasFact(TheFeat)) { return true; }
                if (caster.HasFact(Vital) && caster.HasFact(VitalFeat))
                {
                    AbilityCustomVitalStrike.VitalStrikePart vitalStrikePart = caster.Ensure<AbilityCustomVitalStrike.VitalStrikePart>();
                    vitalStrikePart.Caster = caster;
                    vitalStrikePart.VitalStrikeMod = 2;
                    if (caster.HasFact(VitalFeat2))
                    {
                        vitalStrikePart.VitalStrikeMod = 3;
                    }
                    if (caster.HasFact(VitalFeat3))
                    {
                        vitalStrikePart.VitalStrikeMod = 4;
                    }
                    vitalStrikePart.MythicFact = caster.GetFact(VitalFeat4);
                    vitalStrikePart.Rowdy = caster.HasFact(VitalFeat5);
                    vitalStrikePart.Fact = caster.Facts.m_Facts.First();
                    var ruleAttackWithWeapon = Rulebook.Trigger(new RuleAttackWithWeapon(caster, target, __instance.Hand.Weapon, 0)
                    {
                        IsAttackOfOpportunity = true,
                        ForceFlatFooted = __instance.ForceFlatFooted
                    });
                    vitalStrikePart.SetDeferredRules(ruleAttackWithWeapon);
                    vitalStrikePart.RemoveSelfIfDone();
                    __result = UnitCommand.ResultType.Success;
                    return false;
                }
                else if (caster.HasFact(BullRush) && caster.HasFact(BullRushFeat))
                {
                    if (target.State.HasCondition(UnitCondition.ForceMove)) { return true; }
                    maneuver = CombatManeuver.BullRush;
                }
                else if (caster.HasFact(DirtyBlind) && caster.HasFact(DirtyFeat))
                {
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickBlindnessBuff)) { return true; }
                    maneuver = CombatManeuver.DirtyTrickBlind;
                }
                else if (caster.HasFact(DirtyEntangle) && caster.HasFact(DirtyFeat))
                {
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff)) { return true; }
                    maneuver = CombatManeuver.DirtyTrickEntangle;
                }
                else if (caster.HasFact(DirtySicken) && caster.HasFact(DirtyFeat))
                {
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff)) { return true; }
                    maneuver = CombatManeuver.DirtyTrickSickened;
                }
                else if (caster.HasFact(Disarm) && caster.HasFact(DisarmFeat))
                {
                    var threat = target.GetThreatHandMelee();
                    if (threat == null || threat.MaybeWeapon == null || threat.MaybeWeapon.Blueprint.IsNatural || threat.MaybeWeapon.Blueprint.IsUnarmed) { return true; }
                    maneuver = CombatManeuver.Disarm;
                }
                else if (caster.HasFact(Sunder) && caster.HasFact(SunderFeat))
                {
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff) && !caster.HasFact(SunderReal)) { return true; }
                    maneuver = CombatManeuver.SunderArmor;
                }
                else if (caster.HasFact(Trip) && caster.HasFact(TripFeat))
                {
                    if (!target.CanBeKnockedOff()) { return true; }
                    maneuver = CombatManeuver.Trip;
                }
                else if (caster.HasFact(Grapple) && caster.HasFact(GrappleFeat))
                {
                    //Logger.Info("start grapple");
                    if (caster.Get<UnitPartGrappleInitiatorPP>() || target.Get<UnitPartGrappleTargetPP>() || !ConditionTwoFreeHand.CheckCondition2(caster)) { return true; }
                    maneuver = CombatManeuver.Grapple;
                }
                if (maneuver == CombatManeuver.None) { return true; }
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, maneuver, null);
                ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                if (ruleCombatManeuver.Success && maneuver == CombatManeuver.Grapple) 
                {
                    caster.Ensure<UnitPartGrappleInitiatorPP>().Init(target, CasterBuff, target.Context);
                    target.Ensure<UnitPartGrappleTargetPP>().Init(caster, TargetBuff, caster.Context);
                }
                __result = UnitCommand.ResultType.Success;
                return false;
            }
            catch (Exception ex) { Logger.Error("Failed to replace op attack.", ex); return true; }
        }

        private const string SeizetheOpportunityAbility = "SeizetheOpportunity.SeizetheOpportunityAbility";
        private static readonly string SeizetheOpportunityAbilityGuid = "{52AE2CA9-7FA4-45E8-8D7B-22AC1C7F0F6C}";

        private const string SeizetheOpportunitybuff = "SeizetheOpportunity.SeizetheOpportunitybuff";
        private static readonly string SeizetheOpportunitybuffGuid = "{367BB862-C085-4DED-9676-0BA0C0BF2DC6}";

        private const string SeizetheBullRushAbility = "SeizetheBullRush.SeizetheBullRushAbility";
        private static readonly string SeizetheBullRushAbilityGuid = "{823D990A-F6F2-462E-A3FD-095AA6013803}";

        private const string SeizetheBullRushbuff = "SeizetheBullRush.SeizetheBullRushbuff";
        private static readonly string SeizetheBullRushbuffGuid = "{FDD7D762-A448-48FB-B72C-709D14285FF6}";

        private const string SeizetheDirtyBlindAbility = "SeizetheDirtyBlind.SeizetheDirtyBlindAbility";
        private static readonly string SeizetheDirtyBlindAbilityGuid = "{42CE505F-94C8-4B1A-8262-F1643A0E4740}";

        private const string SeizetheDirtyBlindbuff = "SeizetheDirtyBlind.SeizetheDirtyBlindbuff";
        private static readonly string SeizetheDirtyBlindbuffGuid = "{6142C847-22F1-410F-A132-9545D7404F4A}";

        private const string SeizetheDirtyEntangleAbility = "SeizetheDirtyEntangle.SeizetheDirtyEntangleAbility";
        private static readonly string SeizetheDirtyEntangleAbilityGuid = "{AB159D3D-1590-4F79-87FC-5BB2C647AE85}";

        private const string SeizetheDirtyEntanglebuff = "SeizetheDirtyEntangle.SeizetheDirtyEntanglebuff";
        private static readonly string SeizetheDirtyEntanglebuffGuid = "{723AC061-8A31-485B-976D-2E233B5B4B9E}";

        private const string SeizetheDirtySickenAbility = "SeizetheDirtySicken.SeizetheDirtySickenAbility";
        private static readonly string SeizetheDirtySickenAbilityGuid = "{0AD95F62-8AFE-4590-9CD4-C27F25B467CF}";

        private const string SeizetheDirtySickenbuff = "SeizetheDirtySicken.SeizetheDirtySickenbuff";
        private static readonly string SeizetheDirtySickenbuffGuid = "{4B94C35F-0F34-494F-9CF3-BB2BAD84FCF9}";

        private const string SeizetheDisarmAbility = "SeizetheDisarm.SeizetheDisarmAbility";
        private static readonly string SeizetheDisarmAbilityGuid = "{4C4FC861-F89B-4C77-BD41-3F91E955FF66}";

        private const string SeizetheDisarmbuff = "SeizetheDisarm.SeizetheDisarmbuff";
        private static readonly string SeizetheDisarmbuffGuid = "{0E686D2C-2CBB-43F4-A247-110A85B938C1}";

        private const string SeizetheSunderAbility = "SeizetheSunder.SeizetheSunderAbility";
        private static readonly string SeizetheSunderAbilityGuid = "{EA74C387-BC06-451C-9D0A-48F8AD5527D4}";

        private const string SeizetheSunderbuff = "SeizetheSunder.SeizetheSunderbuff";
        private static readonly string SeizetheSunderbuffGuid = "{E4FD5F1E-A417-47CE-AF53-CD0FC91225B0}";

        private const string SeizetheTripAbility = "SeizetheTrip.SeizetheTripAbility";
        private static readonly string SeizetheTripAbilityGuid = "{4F4B9302-3A9A-4785-8CF6-D1909161EFF3}";

        private const string SeizetheTripbuff = "SeizetheTrip.SeizetheTripbuff";
        private static readonly string SeizetheTripbuffGuid = "{8DFA0D89-D1D5-4A63-AF3E-37655D9935D5}";

        private const string SeizetheGrappleAbility = "SeizetheGrapple.SeizetheGrappleAbility";
        private static readonly string SeizetheGrappleAbilityGuid = "{E070A95B-49E8-4ABF-8F95-18CCB5024ACC}";

        private const string SeizetheGrapplebuff = "SeizetheGrapple.SeizetheGrapplebuff";
        private static readonly string SeizetheGrapplebuffGuid = "{55BD97E6-84AB-4E2A-9F0D-EF8A730DC7CF}";

        private static readonly BlueprintBuffReference BullRush = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheBullRushbuffGuid);
        private static readonly BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyBlindbuffGuid);
        private static readonly BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtyEntanglebuffGuid);
        private static readonly BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDirtySickenbuffGuid);
        private static readonly BlueprintBuffReference Disarm = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheDisarmbuffGuid);
        private static readonly BlueprintBuffReference Grapple = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheGrapplebuffGuid);
        private static readonly BlueprintBuffReference Sunder = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheSunderbuffGuid);
        private static readonly BlueprintBuffReference Trip = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheTripbuffGuid);
        private static readonly BlueprintBuffReference Vital = BlueprintTool.GetRef<BlueprintBuffReference>(SeizetheOpportunitybuffGuid);

        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{D6D08842-8E03-4A9D-81B8-1D9FB2245649}");
        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");

        private static readonly BlueprintFeatureReference TheFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(SeizetheOpportunity.FeatGuid);

        private static readonly BlueprintFeatureReference BullRushFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedBullRush.ToString());
        private static readonly BlueprintFeatureReference DirtyFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDirtyTrick.ToString());
        private static readonly BlueprintFeatureReference DisarmFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedDisarm.ToString());
        private static readonly BlueprintFeatureReference GrappleFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(ImprovedGrapple.StyleGuid);
        private static readonly BlueprintFeatureReference SunderFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedSunder.ToString());
        private static readonly BlueprintFeatureReference TripFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.ImprovedTrip.ToString());
        private static readonly BlueprintFeatureReference VitalFeat = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeature.ToString());

        private static readonly BlueprintFeatureReference VitalFeat2 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeatureImproved.ToString());
        private static readonly BlueprintFeatureReference VitalFeat3 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeFeatureGreater.ToString());

        private static readonly BlueprintFeatureReference VitalFeat4 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.VitalStrikeMythicFeat.ToString());
        private static readonly BlueprintFeatureReference VitalFeat5 = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.RowdyVitalDamage.ToString());

        private static readonly BlueprintAbilityReference VitalAbility = BlueprintTool.GetRef<BlueprintAbilityReference>(AbilityRefs.VitalStrikeAbility.ToString());

        private static readonly BlueprintFeatureReference SunderReal = BlueprintTool.GetRef<BlueprintFeatureReference>(GreaterSunderTabletop.GreaterSunderTabletopGuid);
    }
}
