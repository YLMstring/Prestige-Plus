using JetBrains.Annotations;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using Kingmaker.Visual.Animation.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Kingmaker.Blueprints;
using Kingmaker.Items;
using Kingmaker.Pathfinding;
using Owlcat.Runtime.Core.Utils;
using System.Collections;
using TurnBased.Controllers;
using BlueprintCore.Utils;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using static Kingmaker.UI.CanvasScalerWorkaround;
using System.Runtime.Remoting.Contexts;
using PrestigePlus.Grapple;
using PrestigePlus.Blueprint.MythicGrapple;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Designers;
using Kingmaker.UnitLogic.Parts;
using PrestigePlus.GrappleMechanic;

namespace PrestigePlus.CustomComponent.Charge
{
    internal class TryAboveAttack : AbilityCustomLogic, IAbilityTargetRestriction, IAbilityMinRangeProvider
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public override bool IsEngageUnit
        {
            get
            {
                return true;
            }
        }

        // Token: 0x0600CFA2 RID: 53154 RVA: 0x0035C993 File Offset: 0x0035AB93
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper targetWrapper)
        {
            UnitEntityData target = targetWrapper.Unit;
            if (target == null)
            {
                Logger.Info("Target unit is missing");
                yield break;
            }
            UnitEntityData caster = context.Caster;
            if (caster.GetThreatHandMelee(true) == null)
            {
                Logger.Info("Invalid caster's weapon");
                yield break;
            }
            //if (caster.GetSaddledUnit() != null) { caster.GetSaddledUnit().Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, new TimeSpan?(1.Rounds().Seconds)); }
            Vector3 position = caster.Position;
            Vector3 endPoint = target.Position;
            caster.View.StopMoving();
            caster.View.AgentASP.IsCharging = true;
            caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
            {
                position,
                endPoint
            }), true);
            caster.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, context, new TimeSpan?(1.Rounds().Seconds));
            caster.Descriptor.State.IsCharging = true;
            UnitAttack attack = new(target, null);
            attack.Init(caster);
            if (CombatController.IsInTurnBasedCombat())
            {
                attack.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free;
                attack.ReactionAction = true;
            }
            IEnumerator turnBasedRoutine = null;
            IEnumerator runtimeRoutine = null;
            for (; ; )
            {
                IEnumerator enumerator;
                if (CombatController.IsInTurnBasedCombat())
                {
                    enumerator = turnBasedRoutine ??= TurnBasesRoutine(caster, target, attack);
                }
                else
                {
                    enumerator = runtimeRoutine ??= RuntimeRoutine(caster, target, attack, endPoint);
                }
                if (!enumerator.MoveNext())
                {
                    break;
                }
                yield return null;
            }
            yield break;
        }

        // Token: 0x0600CFA3 RID: 53155 RVA: 0x0035C9A9 File Offset: 0x0035ABA9
        private static IEnumerator TurnBasesRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack)
        {
            UnitEntityData mount = caster.GetSaddledUnit();
            if (mount == null)
            {
                //Logger.Info("start charge2");
                UnitMovementAgent agentASP = caster.View.AgentASP;
                float timeSinceStart = 0f;
                while (attack.ShouldUnitApproach)
                {
                    //Logger.Info("start charge3");
                    if (Game.Instance.TurnBasedCombatController.WaitingForUI)
                    {
                        yield return null;
                    }
                    else
                    {
                        //Logger.Info("start charge4");
                        timeSinceStart += Game.Instance.TimeController.GameDeltaTime;
                        if (timeSinceStart > 6f)
                        {
                            Logger.Info("Charge: timeSinceStart > 6f");
                            break;
                        }
                        if (caster.GetThreatHand() == null)
                        {
                            Logger.Info("Charge: caster.GetThreatHand() == null");
                            break;
                        }
                        if (!caster.Descriptor.State.CanMove && !caster.Get<UnitPartForceMove>())
                        {
                            Logger.Info("Charge: !caster.Descriptor.State.CanMove");
                            break;
                        }
                        if (!agentASP)
                        {
                            Logger.Info("Charge: !(bool)caster.View.AgentASP");
                            break;
                        }
                        //Logger.Info("start charge5");
                        if (!agentASP.IsReallyMoving)
                        {
                            //Logger.Info("start charge6");
                            agentASP.ForcePath(new ForcedPath(new List<Vector3>
                            {
                                caster.Position,
                                target.Position
                            }), true);
                            if (!agentASP.IsReallyMoving)
                            {
                                Logger.Info("Charge: !caster.View.AgentASP.IsReallyMoving");
                                break;
                            }
                        }
                        agentASP.MaxSpeedOverride = new float?(Math.Max(agentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f));
                        yield return null;
                    }
                }
            }
            caster.View.StopMoving();
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
                caster.Commands.AddToQueueFirst(attack);
                Logger.Info("start charge10");
            }
            yield break;
        }

        // Token: 0x0600CFA4 RID: 53156 RVA: 0x0035C9C6 File Offset: 0x0035ABC6
        private static IEnumerator RuntimeRoutine(UnitEntityData caster, UnitEntityData target, UnitAttack attack, Vector3 endPoint)
        {
            float maxDistance = GetMaxRangeMeters(caster);
            UnitEntityData mount = caster.GetSaddledUnit();
            if (mount == null)
            {
                float passedDistance = 0f;
                while (caster.View.MovementAgent.IsReallyMoving)
                {
                    float valueOrDefault = caster.View.MovementAgent.MaxSpeedOverride.GetValueOrDefault();
                    caster.View.MovementAgent.MaxSpeedOverride = new float?(Math.Max(valueOrDefault, caster.CombatSpeedMps * 2f));
                    passedDistance += (caster.Position - caster.PreviousPosition).magnitude;
                    if (passedDistance > maxDistance || !attack.ShouldUnitApproach)
                    {
                        PFLog.Default.Log("Charge: passedDistance > maxDistance || !attack.ShouldUnitApproach");
                        break;
                    }
                    if (caster.GetThreatHand() == null)
                    {
                        PFLog.Default.Log("Charge: caster.GetThreatHand() == null");
                        break;
                    }
                    Vector3 position = target.Position;
                    if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, position) != position && !caster.HasFact(StagBuff))
                    {
                        PFLog.Default.Log("Charge: obstacle != newEndPoint");
                        break;
                    }
                    if (position != endPoint)
                    {
                        endPoint = position;
                        caster.View.AgentASP.ForcePath(new ForcedPath(new List<Vector3>
                        {
                            caster.Position,
                            endPoint
                        }), true);
                    }
                    yield return null;
                }
                if (!caster.View.MovementAgent.IsReallyMoving)
                {
                    PFLog.Default.Log("Charge: !caster.View.MovementAgent.IsReallyMoving");
                }
            }
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
            }
            caster.Commands.AddToQueueFirst(attack);
            yield break;
        }

        // Token: 0x0600CFA6 RID: 53158 RVA: 0x0035CA38 File Offset: 0x0035AC38
        public override void Cleanup(AbilityExecutionContext context)
        {
            context.Caster.View.AgentASP.IsCharging = false;
            context.Caster.View.AgentASP.MaxSpeedOverride = null;
            context.Caster.Descriptor.State.IsCharging = false;
        }

        // Token: 0x0600CFA7 RID: 53159 RVA: 0x0035CA90 File Offset: 0x0035AC90
        public static float GetMinRangeMeters(UnitEntityData caster, [CanBeNull] UnitEntityData target)
        {
            float num = target != null ? target.View.Corpulence : 0.5f;
            if (Game.Instance.Player.IsTurnBasedModeOn())
            {
                return TurnController.MetersOfFiveFootStep + GameConsts.MinWeaponRange.Meters + caster.View.Corpulence + num;
            }
            return 10.Feet().Meters + caster.View.Corpulence + num;
        }

        // Token: 0x0600CFA8 RID: 53160 RVA: 0x0035CB03 File Offset: 0x0035AD03
        public float GetMinRangeMeters(UnitEntityData caster)
        {
            return GetMinRangeMeters(caster, null);
        }

        // Token: 0x0600CFA9 RID: 53161 RVA: 0x0035CB0C File Offset: 0x0035AD0C
        public static float GetMaxRangeMeters(UnitEntityData caster)
        {
            return caster.CombatSpeedMps * 6f;
        }

        // Token: 0x0600CFAA RID: 53162 RVA: 0x0035CB1C File Offset: 0x0035AD1C
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper targetWrapper)
        {
            LocalizedString localizedString;
            return CheckTargetRestriction(caster, targetWrapper, out localizedString);
        }

        // Token: 0x0600CFAB RID: 53163 RVA: 0x0035CB34 File Offset: 0x0035AD34
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            LocalizedString localizedString;
            CheckTargetRestriction(caster, target, out localizedString);
            return localizedString;
        }

        // Token: 0x0600CFAC RID: 53164 RVA: 0x0035CB54 File Offset: 0x0035AD54
        public static bool CheckTargetRestriction(UnitEntityData caster, TargetWrapper targetWrapper, [CanBeNull] out LocalizedString failReason, UnitEntityData target = null)
        {
            UnitEntityData unitEntityData = targetWrapper != null ? targetWrapper.Unit : target;
            if (unitEntityData == null)
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsInvalid;
                return false;
            }
            float magnitude = (unitEntityData.Position - caster.Position).magnitude;
            if (magnitude > GetMaxRangeMeters(caster))
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooFar;
                return false;
            }
            if (magnitude < GetMinRangeMeters(caster, unitEntityData))
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.TargetIsTooClose;
                return false;
            }
            if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, unitEntityData.Position) != unitEntityData.Position && !caster.HasFact(StagBuff))
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
                return false;
            }
            UnitEntityData saddledUnit = caster.GetSaddledUnit();
            if (!(saddledUnit ?? caster).View.MovementAgent.AvoidanceDisabled)
            {
                float num = caster.View.Corpulence + unitEntityData.View.Corpulence;
                ItemEntityWeapon firstWeapon = caster.GetFirstWeapon();
                float valueOrDefault = (num + (firstWeapon != null ? new float?(firstWeapon.AttackRange.Meters) : null)).GetValueOrDefault();
                Vector2 normalized = (unitEntityData.Position - caster.Position).To2D().normalized;
                Vector2 a = unitEntityData.Position.To2D() - normalized * valueOrDefault;
                foreach (UnitEntityData unitEntityData2 in Game.Instance.State.AwakeUnits)
                {
                    if (!(unitEntityData2 == caster) && !(unitEntityData2 == unitEntityData) && unitEntityData2.View && !unitEntityData2.View.MovementAgent.AvoidanceDisabled)
                    {
                        magnitude = (a - unitEntityData2.Position.To2D()).magnitude;
                        if (magnitude < (caster.View.Corpulence + unitEntityData2.View.Corpulence) * 0.8f && (!caster.HasFact(StagBuff) || !unitEntityData2.IsAlly(caster)))
                        {
                            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
                            return false;
                        }
                    }
                }
            }
            UnitEntityData unitEntityData3 = caster.GetSaddledUnit() ?? caster;
            bool flag = caster.State.IsCharging || saddledUnit != null && saddledUnit.State.IsCharging || unitEntityData3 != null && unitEntityData3.State.IsCharging;
            if (CombatController.IsInTurnBasedCombat() && caster.IsCurrentUnit() && !flag && unitEntityData3.CombatState.TBM.TimeMoved > 0f && target == null)
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.AlreadyMovedThisTurn;
                return false;
            }
            failReason = null;
            return true;
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{C5F4DDFE-CA2E-4309-90BB-1BB5C0F32E78}");
        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{F505D659-0610-41B1-B178-E767CCB9292E}");

        private static BlueprintBuffReference AerialBuff = BlueprintTool.GetRef<BlueprintBuffReference>(AerialAssault.Stylebuff2Guid);
        private static BlueprintBuffReference StagBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{21F094D4-1D59-400B-9CEB-558E6218FB0C}");

        //private static BlueprintBuffReference RhinoBuff = BlueprintTool.GetRef<BlueprintBuffReference>(RhinoCharge.RhinoChargebuffGuid);
    }
}
