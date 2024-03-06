using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction
{
    internal class SpendSwiftAction : ContextAction
    {
        public override string GetCaption()
        {
            return "SpendSwiftAction";
        }

        internal ActionList OnHit;
        public override void RunAction()
        {
            var unit = Context.MaybeCaster;
            if (unit == null || !unit.HasSwiftAction()) { return; }
            unit.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0);
            OnHit?.Run();
        }
    }
}
