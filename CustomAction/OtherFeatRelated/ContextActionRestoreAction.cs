using BlueprintCore.Actions.Builder;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.PubSubSystem;
using TurnBased.Controllers;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.TurnBasedMode.Controllers;
using Newtonsoft.Json.Linq;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ContextActionRestoreAction : ContextAction
    {
        public override string GetCaption()
        {
            return "RestoreAction";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }           
            caster.CombatState.Cooldown.StandardAction = 0f;
            if (caster.UsedTwoMoveAction())
            {
                caster.CombatState.Cooldown.MoveAction = caster.IsMoveActionRestricted() ? 0f : 3f;
            }
            var state = Game.Instance.TurnBasedCombatController?.CurrentTurn?.GetActionsStates(caster)?.ActionsStates;
            if (state != null)
            {
                Game.Instance.TurnBasedCombatController.CurrentTurn.GetActionsStates(caster).ActionsStates.Standard = 
                    new CombatAction(CombatAction.ActivityState.Available, CombatAction.ActivityState.Available, CombatAction.ActivityState.Available, 0f);
            }
        }
    }
}
