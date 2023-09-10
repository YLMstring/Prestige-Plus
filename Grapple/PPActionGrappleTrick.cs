using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace PrestigePlus.Grapple
{
    internal class PPActionGrappleTrick : ContextAction
    {

        // Token: 0x0600CBFE RID: 52222 RVA: 0x0034ECC6 File Offset: 0x0034CEC6
        public override string GetCaption()
        {
            return "Grapple";
        }

        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            UnitGrappleControllerPP.GrappleTrick(maybeCaster, false, Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard);
        }

        // Token: 0x040087CF RID: 34767

    }
}
