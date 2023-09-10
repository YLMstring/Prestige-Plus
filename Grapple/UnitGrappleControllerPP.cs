using Kingmaker;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.GameModes;
using JetBrains.Annotations;
using Kingmaker.Designers;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.EntitySystem.Stats;
using static Kingmaker.UnitLogic.Interaction.SpawnerInteractionPart;
using static HarmonyLib.Code;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace PrestigePlus.Grapple
{
    internal class UnitGrappleControllerPP : BaseUnitController
    {
        public override void TickOnUnit(UnitEntityData unit)
        {
            TickForTarget(unit);
            TickForInitiator(unit);
        }

        // Token: 0x0600A6AE RID: 42670 RVA: 0x002B6B08 File Offset: 0x002B4D08s
        private static void TickForTarget(UnitEntityData unit)
        {
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = unit.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null)
            {
                return;
            }
            TimeSpan gameTime = Game.Instance.TimeController.GameTime;
            UnitEntityData value = UnitPartGrappleTargetPP.Initiator.Value;
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = value?.Get<UnitPartGrappleInitiatorPP>();
            if (value == null || UnitPartGrappleInitiatorPP == null || UnitPartGrappleInitiatorPP.Target != unit || !value.Descriptor.State.IsConscious)
            {
                unit.Remove<UnitPartGrappleTargetPP>();
                return;
            }
            if (UnitPartGrappleTargetPP.HasMoved)
            {
                ReleaseGrapple(value);
                return;
            }     
        }

        // Token: 0x0600A6AF RID: 42671 RVA: 0x002B6BD8 File Offset: 0x002B4DD8
        private static void TickForInitiator(UnitEntityData unit)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = unit.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                return;
            }
            if (UnitPartGrappleInitiatorPP.HasMoved)
            {
                ReleaseGrapple(unit);
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = value?.Get<UnitPartGrappleTargetPP>();
            if (value == null || UnitPartGrappleTargetPP == null || UnitPartGrappleTargetPP.Initiator != unit || !value.Descriptor.State.IsConscious)
            {
                unit.Remove<UnitPartGrappleInitiatorPP>();
            }
        }

        public static void ReleaseGrapple(UnitEntityData grappleInitiator)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = grappleInitiator.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            grappleInitiator.Remove<UnitPartGrappleInitiatorPP>();
            if (value == null)
            {
                return;
            }
            value.Remove<UnitPartGrappleTargetPP>();
        }

        public static bool GrappleTrick(UnitEntityData grappleInitiator, bool isMaintain, CommandType Action)
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = grappleInitiator.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                PFLog.Default.Warning("UnitPartGrappleInitiatorPP is missing", Array.Empty<object>());
                return false;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = value.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null) { return false; }
            if (isMaintain) { grappleInitiator.SpendAction(Action, false, 0); }
            if (!UnitPartGrappleTargetPP.IsTiedUp && !grappleInitiator.Context.TriggerRule(new RuleCombatManeuver(grappleInitiator, value, CombatManeuver.Grapple, null)).Success)
            {
                return isMaintain;
            }
            if (!UnitPartGrappleTargetPP.IsPinned && !grappleInitiator.HasFact(NoPin))
            {
                UnitPartGrappleTargetPP.TrySetPinned();
                UnitPartGrappleInitiatorPP.TrySetPinning();
                return false;
            }
            if (!UnitPartGrappleTargetPP.IsTiedUp && !grappleInitiator.HasFact(NoTieUp))
            {
                UnitPartGrappleTargetPP.TrySetTiedUp();
                UnitPartGrappleInitiatorPP.TryClearPinning();
                return false;
            }
            UnitGrappleInitiatorAttackPP cmd = new UnitGrappleInitiatorAttackPP(value);
            grappleInitiator.Commands.AddToQueue(cmd);
            return false;
        }

        public static bool TryBreakFree(UnitEntityData grappleInitiator, UnitEntityData grappleTarget)
        {
            if (grappleInitiator == null)
            {
                return false;
            }
            if (grappleTarget == null)
            {
                return false;
            }
            if (grappleTarget.Get<UnitPartGrappleTargetPP>() == null) { return false; }
            var context1 = grappleInitiator.Context;
            var context2 = grappleTarget.Context;
            RuleCalculateCMD ruleCalculateCMD = new RuleCalculateCMD(grappleTarget, grappleInitiator, CombatManeuver.Grapple);
            ruleCalculateCMD = (context1?.TriggerRule(ruleCalculateCMD)) ?? Rulebook.Trigger(ruleCalculateCMD);
            int dc = ruleCalculateCMD.Result;
            if (grappleTarget.Get<UnitPartGrappleTargetPP>().IsTiedUp)
            {
                RuleCalculateCMB ruleCalculateCMB2 = new RuleCalculateCMB(grappleInitiator, grappleTarget, CombatManeuver.Grapple);
                ruleCalculateCMB2 = (context1?.TriggerRule(ruleCalculateCMB2)) ?? Rulebook.Trigger(ruleCalculateCMB2);
                dc = Math.Max(ruleCalculateCMB2.Result + 20, dc);
            }
            RuleCalculateCMB ruleCalculateCMB = new RuleCalculateCMB(grappleTarget, grappleInitiator, CombatManeuver.Grapple);
            ruleCalculateCMB = (context2?.TriggerRule(ruleCalculateCMB)) ?? Rulebook.Trigger(ruleCalculateCMB);
            if (grappleTarget.Stats.SkillThievery < ruleCalculateCMB.Result)
            {
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(grappleTarget, grappleInitiator, CombatManeuver.Grapple, null);
                ruleCombatManeuver = (context2?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                return ruleCombatManeuver.Success;
            }
            StatType statType = StatType.SkillThievery;
            return GameHelper.TriggerSkillCheck(new RuleSkillCheck(grappleTarget, statType, dc)
            {
                IgnoreDifficultyBonusToDC = grappleInitiator.IsPlayersEnemy
            }, context2, true).Success;
        }

        private static BlueprintBuffReference NoPin = BlueprintTool.GetRef<BlueprintBuffReference>("{D3B37428-69BE-43F2-83DA-04A38D35CDCB}");
        private static BlueprintBuffReference NoTieUp = BlueprintTool.GetRef<BlueprintBuffReference>("{B14E98A0-53AC-4212-86A0-29D1CA1D8446}");
    }

    [HarmonyPatch(typeof(GameModesFactory), nameof(GameModesFactory.Initialize))]
    class PatchGrapple
    {
        static void Postfix()
        {
            GameModesFactory.Register(new UnitGrappleControllerPP(), new GameModeType[] { GameModesFactory.Default });
        }
    }
}

