using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionAceTrip : ContextAction
    {
        public override string GetCaption()
        {
            return "AceTrip";
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
            if (target.Descriptor.State.Prone.Active || (target.View != null && target.View.IsGetUp) || !target.Descriptor.State.HasConditionImmunity(UnitCondition.Prone)) { return; }
            target.Descriptor.State.Prone.ShouldBeActive = true;
            EventBus.RaiseEvent(delegate (IKnockOffHandler h)
            {
                h.HandleKnockOff(caster, target);
            }, true);
        }
    }
}