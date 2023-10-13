using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Utility;
using System.Runtime.Remoting.Contexts;
using Kingmaker.UnitLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
using Mono.Cecil;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionSunderStorm : ContextAction
    {
        public override string GetCaption()
        {
            return "SunderStorm";
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        // Token: 0x0600CBFF RID: 52223 RVA: 0x0034ECD0 File Offset: 0x0034CED0
        public override void RunAction()
        {
            try
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
                    PFLog.Default.Error("Unit can't sunder themselves", Array.Empty<object>());
                    return;
                }
                float radius = 5.Feet().Meters;
                var hand = maybeCaster.GetThreatHandMelee();
                Logger.Info("get hand");
                if (hand != null)
                {
                    Logger.Info("get range");
                    var range = maybeCaster.GetThreatRange(hand);
                    if (range != null && range.HasValue)
                    {
                        radius = range.Value + maybeCaster.View.Corpulence / 2f + unit.View.Corpulence / 2f;
                        Logger.Info(radius.ToString());
                    }
                }
                Logger.Info(unit.DistanceTo(maybeCaster).ToString());
                if (unit.DistanceTo(maybeCaster) > radius)
                {
                    return;
                }
                Logger.Info("sunder");
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(maybeCaster, unit, CombatManeuver.SunderArmor, null);
                Rulebook.Trigger(ruleCombatManeuver);
            }
            catch (Exception ex) { Logger.Error("Failed to storm.", ex); }
        }
    }
}
