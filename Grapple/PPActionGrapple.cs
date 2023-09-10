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
    internal class PPActionGrapple : ContextAction
    {

        // Token: 0x0600CBFE RID: 52222 RVA: 0x0034ECC6 File Offset: 0x0034CEC6
        public override string GetCaption()
        {
            return "Grapple";
        }

        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            UnitEntityData unit = base.Target.Unit;
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (unit == maybeCaster)
            {
                PFLog.Default.Error("Unit can't grapple themselves", Array.Empty<object>());
                return;
            }
            if (maybeCaster.Get<UnitPartGrappleInitiatorPP>())
            {
                PFLog.Default.Error("Caster already grapple", Array.Empty<object>());
                return;
            }
            if (unit.Get<UnitPartGrappleTargetPP>())
            {
                PFLog.Default.Error("Target already grappled", Array.Empty<object>());
                return;
            }
            maybeCaster.Ensure<UnitPartGrappleInitiatorPP>().Init(unit, this.CasterBuff, base.Context);
            unit.Ensure<UnitPartGrappleTargetPP>().Init(maybeCaster, this.TargetBuff, base.Context);
        }

        // Token: 0x040087CF RID: 34767

        public BlueprintBuff CasterBuff;

        // Token: 0x040087D0 RID: 34768

        public BlueprintBuff TargetBuff;
    }
}
