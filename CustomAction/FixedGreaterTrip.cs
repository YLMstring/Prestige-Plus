using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Maneuvers;

namespace PrestigePlus.CustomAction
{
    internal class FixedGreaterTrip : ContextAction
    {
        public override string GetCaption()
        {
            return "FixedGreaterTrip";
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
            if (!target.Descriptor.State.Prone.ShouldBeActive && !caster.HasFact(Ace)) { return; }
            Game.Instance.CombatEngagementController.ProvokeAttackOfOpportunity(target);
        }

        private static BlueprintFeatureReference Ace = BlueprintTool.GetRef<BlueprintFeatureReference>(RangedTrip.AceTripGuid);
    }
}