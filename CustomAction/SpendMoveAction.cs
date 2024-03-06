using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.ElementsSystem;
using BlueprintCore.Blueprints.References;
using Kingmaker.Designers;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.CustomAction
{
    internal class SpendMoveAction : ContextAction
    {
        public override string GetCaption()
        {
            return "SpendMoveAction";
        }

        internal ActionList OnHit;
        public override void RunAction()
        {
            var unit = Context.MaybeCaster;
            if (unit == null || unit.UsedOneMoveAction()) { return; }
            unit.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move, false, 0);
            OnHit?.Run();
        }
    }
}
