using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Blueprint.SpecificManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Enums;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class ContextActionPPmaneuver : ContextAction
    {
        public override string GetCaption()
        {
            return "PPmaneuver";
        }

        public override void RunAction()
        {
            if (Target.Unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (Context.MaybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            ItemEntityWeapon weapon = null;
            if (useWeapon)
            {
                weapon = Context.MaybeCaster.Body.PrimaryHand.MaybeWeapon;
            }
            SweepManeuver.ActManeuver(Context.MaybeCaster, Target.Unit, Penalty, Maneuver, weapon, Roll, Mod);
        }
        public int Penalty = 0;
        public CombatManeuver Maneuver = CombatManeuver.Disarm;
        public bool useWeapon = false;
        public RuleRollD20 Roll;
        public ModifierDescriptor Mod = ModifierDescriptor.Penalty;
    }
}
