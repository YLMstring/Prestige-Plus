using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent
{
    internal class PPWeaponOverride : EmptyHandWeaponOverride, IUnitCombatHandler
    {
        void IUnitCombatHandler.HandleUnitJoinCombat(UnitEntityData unit)
        {
            if (unit == Owner)
            {
                RemoveWeapon();
                SetWeapon();
            }
        }

        void IUnitCombatHandler.HandleUnitLeaveCombat(UnitEntityData unit)
        {

        }
    }
}
