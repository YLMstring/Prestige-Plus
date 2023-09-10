using Kingmaker;
using Kingmaker.Controllers.Units;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.Utility;
using Kingmaker.Enums;
using System.Data;
using PrestigePlus.Feats;

namespace PrestigePlus.Grapple
{
    internal class PPGrabInitiatorBuff : PPGrabBuffBase, ITickEachRound, IUnitNewCombatRoundHandler, IGlobalSubscriber, ISubscriber, IPolymorphDeactivatedHandler, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>
    {
        public void HandleNewCombatRound(UnitEntityData unit)
        {
            if (!CombatController.IsInTurnBasedCombat())
            {
                return;
            }
            if (unit == null)
            {
                return;
            }
            if (unit == base.Owner)
            {
                HandleNewRound();
            }
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = base.Owner.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                PFLog.Default.Warning("UnitPartGrappleInitiatorPP is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            if (unit == value)
            {
                HandleBreakFree();
            }
        }

        // Token: 0x0600C1F8 RID: 49656 RVA: 0x00327D93 File Offset: 0x00325F93
        public void OnNewRound()
        {
            if (CombatController.IsInTurnBasedCombat())
            {
                return;
            }
            HandleBreakFree();
            HandleNewRound();
        }

        private void HandleBreakFree()
        {
            UnitPartGrappleInitiatorPP UnitPartGrappleInitiatorPP = base.Owner.Get<UnitPartGrappleInitiatorPP>();
            if (UnitPartGrappleInitiatorPP == null)
            {
                PFLog.Default.Warning("UnitPartGrappleInitiatorPP is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData value = UnitPartGrappleInitiatorPP.Target.Value;
            UnitPartGrappleTargetPP UnitPartGrappleTargetPP = value.Get<UnitPartGrappleTargetPP>();
            if (UnitPartGrappleTargetPP == null)
            {
                return;
            }
            if (UnitGrappleControllerPP.TryBreakFree(Owner, value))
            {
                value.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard, false, 0);
                UnitGrappleControllerPP.ReleaseGrapple(base.Owner);
                return;
            }
        }

        // Token: 0x0600C1F9 RID: 49657 RVA: 0x00327DA4 File Offset: 0x00325FA4
        private void HandleNewRound()
        {
            var type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard;
            if (Owner.HasFact(GreaterGrapple))
            {
                type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move;
            }
            if (UnitGrappleControllerPP.GrappleTrick(Owner, true, type))
            {
                if (Owner.HasFact(GreaterGrapple) && Owner.HasFact(GreaterGrappleMythic))
                {
                    if (UnitGrappleControllerPP.GrappleTrick(Owner, true, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard))
                    {
                        UnitGrappleControllerPP.ReleaseGrapple(Owner);
                    }
                    return;
                }
                UnitGrappleControllerPP.ReleaseGrapple(Owner);
                return;
            }
        }


        // Token: 0x0600C1FA RID: 49658 RVA: 0x00327E41 File Offset: 0x00326041
        public void OnPolymorphDeactivated(UnitEntityData unit, Polymorph polymorph)
        {
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {
            
        }

        private static BlueprintFeatureReference GreaterGrapple = BlueprintTool.GetRef<BlueprintFeatureReference>("{CB3B7666-0AD1-4ADD-8157-BAC7E2A15D5A}");
        private static BlueprintFeatureReference GreaterGrappleMythic = BlueprintTool.GetRef<BlueprintFeatureReference>("{27B59104-C22F-4E35-8743-BF08A3B2B870}");
    }
}
