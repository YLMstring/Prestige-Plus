using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class RecalculateEachRound : UnitFactComponentDelegate, IUnitNewCombatRoundHandler, IGlobalSubscriber, ISubscriber
    {
        void IUnitNewCombatRoundHandler.HandleNewCombatRound(UnitEntityData unit)
        {
            if (unit == base.Owner)
            {
                Fact.Reapply();
            }
        }
    }
}