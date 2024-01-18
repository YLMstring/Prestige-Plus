using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction
{
    internal class PPrestoreRes : ContextRestoreResource
    {
        public override void RunAction()
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit?.Resources?.ContainsResource(Resource) != true)
            {
                return;
            }
            base.RunAction();
        }
    }
}
