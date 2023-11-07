using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class TightFollowerAction : ContextAction
    {
        public override string GetCaption()
        {
            return "TightFollower";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null || Target.Point == null || !caster.HasSwiftAction() || !caster.HasFact(RageBuff)) { return; }
            if (caster.HasFact(TargetBuff)) { return; }
            var ally = caster;
            foreach (var unit in caster.Group)
            {
                if (unit.GetFact(TargetBuff)?.MaybeContext?.MaybeCaster == caster)
                {
                    ally = unit;
                    break;
                }
            }
            if (ally != caster && ally.DistanceTo(Target.Point) <= 10.Feet().Meters && caster.DistanceTo(Target.Point) <= caster.CombatSpeedMps * 3f) 
            {
                var mount = caster.Get<UnitPartRider>()?.SaddledUnit;
                if (mount != null)
                {
                    mount.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                    mount.Position = Target.Point;
                    mount.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0);
                }
                caster.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                caster.Position = Target.Point;
                caster.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift, false, 0);
            }
        }
        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(FuriousGuardian.ChosenAllyBuffGuid);
        private static readonly BlueprintBuffReference RageBuff = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.StandartRageBuff.ToString());
    }
}
