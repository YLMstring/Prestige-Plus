using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using TurnBased.Controllers;
using Kingmaker.Utility;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ContextActionCastling : ContextAction
    {
        public override string GetCaption()
        {
            return "Castling";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target)
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
            if (!CombatController.IsInTurnBasedCombat() || Game.Instance.TurnBasedCombatController?.CurrentTurn == null) { return; }
            if (!Game.Instance.TurnBasedCombatController.CurrentTurn.HasFiveFootStep(caster) || !target.IsAlly(caster)) { return; }
            caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
            target.CombatState.PreventAttacksOfOpporunityNextFrame = true;
            var position = caster.Position;
            caster.Position = target.Position;
            target.Position = position;
            Game.Instance.TurnBasedCombatController.CurrentTurn.SetMetersMovedByFiveFootStep(caster, 5.Feet().Value);
        }
    }
}