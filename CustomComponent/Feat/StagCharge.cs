using BlueprintCore.Utils;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.Pathfinding;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.View;
using PrestigePlus.Grapple;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.UnitLogic;
using UnityEngine;
using Owlcat.Runtime.Core.Utils;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class StagCharge : AbilityCustomLogic, IAbilityTargetRestriction, IAbilityMinRangeProvider
    {
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
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                yield break;
            }
            UnitEntityData caster = context.Caster;
            if (caster.GetThreatHandMelee(true) == null)
            {
                PFLog.Default.Error("Invalid caster's weapon", Array.Empty<object>());
                yield break;
            }
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
            //caster.Descriptor.AddBuff(CastBuff, context, new TimeSpan?(1.Rounds().Seconds));
            caster.Descriptor.State.IsCharging = true;
            UnitAttack attack = new UnitAttack(target, null);
            attack.Init(caster);
            IEnumerator turnBasedRoutine = null;
            IEnumerator runtimeRoutine = null;
            for (; ; )
            {
                IEnumerator enumerator;
                if (CombatController.IsInTurnBasedCombat())
                {
                    enumerator = turnBasedRoutine = turnBasedRoutine ?? TurnBasesRoutine(caster, target, attack);
                }
                else
                {
                    enumerator = runtimeRoutine = runtimeRoutine ?? RuntimeRoutine(caster, target, attack, endPoint);
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
                UnitMovementAgent agentASP = caster.View.AgentASP;
                float timeSinceStart = 0f;
                while (attack.ShouldUnitApproach)
                {
                    if (Game.Instance.TurnBasedCombatController.WaitingForUI)
                    {
                        yield return null;
                    }
                    else
                    {
                        timeSinceStart += Game.Instance.TimeController.GameDeltaTime;
                        if (timeSinceStart > 6f)
                        {
                            PFLog.TBM.Log("Charge: timeSinceStart > 6f", Array.Empty<object>());
                            break;
                        }
                        if (caster.GetThreatHand() == null)
                        {
                            PFLog.TBM.Log("Charge: caster.GetThreatHand() == null", Array.Empty<object>());
                            break;
                        }
                        if (!caster.Descriptor.State.CanMove)
                        {
                            PFLog.TBM.Log("Charge: !caster.Descriptor.State.CanMove", Array.Empty<object>());
                            break;
                        }
                        if (!agentASP)
                        {
                            PFLog.TBM.Log("Charge: !(bool)caster.View.AgentASP", Array.Empty<object>());
                            break;
                        }
                        if (!agentASP.IsReallyMoving)
                        {
                            agentASP.ForcePath(new ForcedPath(new List<Vector3>
                            {
                                caster.Position,
                                target.Position
                            }), true);
                            if (!agentASP.IsReallyMoving)
                            {
                                PFLog.TBM.Log("Charge: !caster.View.AgentASP.IsReallyMoving", Array.Empty<object>());
                                break;
                            }
                        }
                        agentASP.MaxSpeedOverride = new float?(Math.Max(agentASP.MaxSpeedOverride.GetValueOrDefault(), caster.CombatSpeedMps * 2f));
                        yield return null;
                    }
                }
                agentASP = null;
            }
            else
            {
                while (IsMountCharging(caster))
                {
                    yield return null;
                }
            }
            caster.View.StopMoving();
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
                UnitEntityData rider = caster.GetRider();
                if (rider != null)
                {
                    if (rider.Commands.Attack != null)
                    {
                        attack.AddRiderCommand(rider.Commands.Attack);
                        rider.Commands.Attack.AddMountCommand(attack);
                    }
                }
                else if (mount != null && mount.Commands.Attack != null)
                {
                    attack.AddMountCommand(mount.Commands.Attack);
                    mount.Commands.Attack.AddRiderCommand(attack);
                }
                caster.Commands.AddToQueueFirst(attack);
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
                        PFLog.Default.Log("Charge: passedDistance > maxDistance || !attack.ShouldUnitApproach", Array.Empty<object>());
                        break;
                    }
                    if (caster.GetThreatHand() == null)
                    {
                        PFLog.Default.Log("Charge: caster.GetThreatHand() == null", Array.Empty<object>());
                        break;
                    }
                    Vector3 position = target.Position;
                    if (!caster.HasFact(CasterBuff) && ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, position) != position)
                    {
                        PFLog.Default.Log("Charge: obstacle != newEndPoint", Array.Empty<object>());
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
                    PFLog.Default.Log("Charge: !caster.View.MovementAgent.IsReallyMoving", Array.Empty<object>());
                }
            }
            else
            {
                while (IsMountCharging(caster))
                {
                    yield return null;
                }
            }
            if (!attack.ShouldUnitApproach)
            {
                attack.IgnoreCooldown(null);
                attack.IsCharge = true;
            }
            UnitEntityData rider = caster.GetRider();
            if (rider != null)
            {
                if (rider.Commands.Attack != null)
                {
                    attack.AddRiderCommand(rider.Commands.Attack);
                    rider.Commands.Attack.AddMountCommand(attack);
                }
            }
            else if (mount != null && mount.Commands.Attack != null)
            {
                attack.AddMountCommand(mount.Commands.Attack);
                mount.Commands.Attack.AddRiderCommand(attack);
            }
            caster.Commands.AddToQueueFirst(attack);
            yield break;
        }

        // Token: 0x0600CFA5 RID: 53157 RVA: 0x0035C9EC File Offset: 0x0035ABEC
        private static bool IsMountCharging(UnitEntityData rider)
        {
            UnitEntityData saddledUnit = rider.GetSaddledUnit();
            if (saddledUnit == null)
            {
                return false;
            }
            UnitUseAbility unitUseAbility = saddledUnit.Commands.Standard as UnitUseAbility;
            return unitUseAbility != null && unitUseAbility.Ability.Blueprint.GetComponent<StagCharge>();
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
        private bool CheckTargetRestriction(UnitEntityData caster, TargetWrapper targetWrapper, [CanBeNull] out LocalizedString failReason)
        {
            UnitEntityData unitEntityData = targetWrapper != null ? targetWrapper.Unit : null;
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
            if (ObstacleAnalyzer.TraceAlongNavmesh(caster.Position, unitEntityData.Position) != unitEntityData.Position && !caster.HasFact(CasterBuff))
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
                        if (magnitude < (caster.View.Corpulence + unitEntityData2.View.Corpulence) * 0.8f && (!caster.HasFact(CasterBuff) || !unitEntityData2.IsAlly(caster)))
                        {
                            failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.ObstacleBetweenCasterAndTarget;
                            return false;
                        }
                    }
                }
            }
            UnitEntityData unitEntityData3 = caster.GetSaddledUnit() ?? caster;
            bool flag = caster.State.IsCharging || saddledUnit != null && saddledUnit.State.IsCharging || unitEntityData3 != null && unitEntityData3.State.IsCharging;
            if (CombatController.IsInTurnBasedCombat() && caster.IsCurrentUnit() && !flag && unitEntityData3.CombatState.TBM.TimeMoved > 0f)
            {
                failReason = BlueprintRoot.Instance.LocalizedTexts.Reasons.AlreadyMovedThisTurn;
                return false;
            }
            failReason = null;
            return true;
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{21F094D4-1D59-400B-9CEB-558E6218FB0C}");
    }
}
