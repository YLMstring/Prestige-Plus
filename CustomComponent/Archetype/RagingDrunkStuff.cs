using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class RagingDrunkStuff : UnitFactComponentDelegate, ITickEachRound
    {
        void ITickEachRound.OnNewRound()
        {
            if (Owner.HasFact(BuffRefs.StandartRageBuff.Reference) || Owner.HasFact(BuffRefs.BloodragerStandartRageBuff.Reference) || Owner.HasFact(BuffRefs.StandartFocusedRageBuff.Reference))
            {
                if (Owner.HasMoveAction())
                {
                    Owner.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move, false, 0);
                    Owner.Resources.Restore(AbilityResourceRefs.RageResourse.Reference, 1);
                    Owner.Resources.Restore(AbilityResourceRefs.BloodragerRageResource.Reference, 1);
                    Owner.Resources.Restore(AbilityResourceRefs.FocusedRageResourse.Reference, 1);
                    GameHelper.ApplyBuff(Owner, Buff);
                }
            }
        }

        public override void OnActivate()
        {
            if (Owner.HasFact(BuffRefs.StandartRageBuff.Reference) || Owner.HasFact(BuffRefs.BloodragerStandartRageBuff.Reference) || Owner.HasFact(BuffRefs.StandartFocusedRageBuff.Reference))
            {
                if (Owner.HasMoveAction())
                {
                    Owner.SpendAction(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move, false, 0);
                    Owner.Resources.Restore(AbilityResourceRefs.RageResourse.Reference, 1);
                    Owner.Resources.Restore(AbilityResourceRefs.BloodragerRageResource.Reference, 1);
                    Owner.Resources.Restore(AbilityResourceRefs.FocusedRageResourse.Reference, 1);
                    GameHelper.ApplyBuff(Owner, Buff);
                }
            }
        }

        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(DrunkenBrute.RagingDrunkBuff3Guid);
    }
}
