using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using PrestigePlus.CustomAction.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.Mechanics.Components;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.ElementsSystem;
using Kingmaker.Blueprints.Facts;

namespace PrestigePlus.CustomAction
{
    internal class HighestHDenemy : ContextAction
    {
        public override string GetCaption()
        {
            return "HighestHDenemy";
        }

        public override void RunAction()
        {
            if (Context.MaybeCaster == null || Target.Unit == null)
            {
                return;
            }
            List<UnitEntityData> list = new() { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in Context.MaybeCaster.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                if (!unit.Descriptor.State.IsDead && unit.DistanceTo(Context.MaybeCaster) <= 30.Feet().Meters)
                {
                    list.Add(unit);
                }
            }
            if (list.Count == 0) { return; }
            list.Sort((u1, u2) => u1.Progression.CharacterLevel.CompareTo(u2.Progression.CharacterLevel));
            list.Reverse();
            int num = 0;
            foreach (UnitEntityData unit in list)
            {
                num++;
                if (unit == Target.Unit)
                {
                    Action.Run();
                    return;
                }
                if (num >= Context.Params.CasterLevel / 4)
                {
                    return;
                }
            }
        }

        public ActionList Action;
    }
}
