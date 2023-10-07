using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionDoKiThrow : ContextAction
    {
        public override string GetCaption()
        {
            return "Throw";
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
            if (Target == null) return;
            Vector3 point = Target.Point;
            if (Target.Unit != null) 
            { 
                point = Target.Unit.Position; 
            }
            if (maybeCaster.Get<UnitPartKiThrow>())
            {
                var target = maybeCaster.Get<UnitPartKiThrow>().Target;

                if (target != null)
                {
                    target.Value.Position = point;
                }
                maybeCaster.Remove<UnitPartKiThrow>();
            }
        }
    }
}
