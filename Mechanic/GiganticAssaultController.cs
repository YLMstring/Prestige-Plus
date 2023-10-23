using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.GrappleMechanic
{
    internal class GiganticAssaultController : BaseUnitController
    {
        public override void TickOnUnit(UnitEntityData unit)
        {
            //GameHelper.RemoveBuff(unit, Buff);
            if (!unit.HasFact(Base)) return;
            UnitEntityData mount = unit.GetSaddledUnit();
            if (mount == null)
            {
                GameHelper.RemoveBuff(unit, BlueprintRoot.Instance.SystemMechanics.ChargeBuff);
            }
            else if (!unit.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff))
            {
                unit.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, unit.Context, new TimeSpan?(1.Rounds().Seconds));
                mount.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.ChargeBuff, mount.Context, new TimeSpan?(1.Rounds().Seconds));
            }

        }

        //private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(CloyingShades.CloyingShadesBuffGuid);
        private static BlueprintFeatureReference Base = BlueprintTool.GetRef<BlueprintFeatureReference>("{D47DC15C-3A96-4358-A652-DB9E632009A7}");
    }
}
