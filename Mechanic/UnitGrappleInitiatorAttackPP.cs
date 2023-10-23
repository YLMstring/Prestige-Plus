using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Grapple
{
    internal class UnitGrappleInitiatorAttackPP : UnitCustomFreeAttackBase
    {
        public override UnitCustomFreeAttackBase.AttackMode Mode
        {
            get
            {
                return UnitCustomFreeAttackBase.AttackMode.DamageOnly;
            }
        }

        // Token: 0x0600C363 RID: 50019 RVA: 0x0032D1B8 File Offset: 0x0032B3B8
        public UnitGrappleInitiatorAttackPP([NotNull] UnitEntityData target) : base(target)
        {
        }

        // Token: 0x0600C364 RID: 50020 RVA: 0x0032D1C1 File Offset: 0x0032B3C1
        public override bool IsSuitableWeapon(ItemEntityWeapon weapon)
        {
            return !weapon.Blueprint.IsTwoHanded && weapon.Blueprint.IsMelee; 
        }
    }
}
