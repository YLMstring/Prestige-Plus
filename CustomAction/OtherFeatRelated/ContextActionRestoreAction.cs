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

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ContextActionRestoreAction : ContextAction
    {
        public override string GetCaption()
        {
            return "QuickShot";
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
            Game.Instance.TurnBasedCombatController?.CurrentTurn?.UpdateCurrentActionStates(caster, false, true);
        }
    }
}
