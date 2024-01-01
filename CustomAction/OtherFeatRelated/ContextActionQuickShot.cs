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
using Kingmaker.Items;
using Kingmaker.UnitLogic.Mechanics;
using static Pathfinding.Util.RetainedGizmos;
using BlueprintCore.Actions.Builder;
using Kingmaker.ElementsSystem;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    internal class ContextActionQuickShot : ContextAction
    {
        public override string GetCaption()
        {
            return "QuickShot";
        }

        public override void RunAction()
        {
            var caster = Context.MaybeCaster;
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            ItemEntityWeapon weapon = caster.Body.PrimaryHand.Weapon;
            if (weapon == null) { weapon = caster.Body.EmptyHandWeapon; }
            if (!weapon.Blueprint.IsRanged) { return; }
            List<UnitEntityData> list = new List<UnitEntityData> { };
            foreach (UnitGroupMemory.UnitInfo unitInfo in caster.Memory.Enemies)
            {
                UnitEntityData unit = unitInfo.Unit;
                var range = weapon.AttackRange.Meters;
                if (!unit.Descriptor.State.IsDead && caster.DistanceTo(unit) <= range)
                {
                    list.Add(unit);
                }
            }
            if (list.Count == 0) { return; }
            list.Sort((u1, u2) => u1.SqrDistanceTo(caster).CompareTo(u2.SqrDistanceTo(caster)));
            using (Context.GetDataScope(list.First()))
            {
                Actions.Run();
            }
        }
        private static ActionList Actions = ActionsBuilder.New().RangedAttack().Build();
    }
}
