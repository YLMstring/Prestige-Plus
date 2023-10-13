using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Maneuvers;
using HarmonyLib;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionSetKiThrow : ContextAction
    {
        public override string GetCaption()
        {
            return "Throw";
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
                PFLog.Default.Error("Unit can't throw themselves", Array.Empty<object>());
                return;
            }
            if (maybeCaster.Get<UnitPartKiThrow>())
            {
                maybeCaster.Get<UnitPartKiThrow>().Target.Add(unit);
            }
            maybeCaster.Ensure<UnitPartKiThrow>().Init(unit, CasterBuff, base.Context);
        }

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{98558112-717B-456D-838F-EFF5061F4D38}");
    }
}
