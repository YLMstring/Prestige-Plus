using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ContextWildstrike : ContextAction
    {
        public override string GetCaption()
        {
            return "ContextWildstrike";
        }
        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            var target = Target.Unit;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            List<UnitEntityData> list = new() { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in target.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (target.CombatState.EngagedUnits.Contains(unit))
                {
                    list.Add(unit);
                }
            }
            if (list.Count == 0) { return; }
            list.Sort((u1, u2) => u1.SqrDistanceTo(Target.Unit).CompareTo(u2.SqrDistanceTo(Target.Unit)));
            Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(target, list.First(), false);
        }
    }
}