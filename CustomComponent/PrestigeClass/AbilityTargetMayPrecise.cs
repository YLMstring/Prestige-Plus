using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.FactLogic;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AbilityTargetMayPrecise : BlueprintComponent, IAbilityTargetRestriction
    {
        // Token: 0x0600D407 RID: 54279 RVA: 0x0036F52C File Offset: 0x0036D72C
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
        {
            UnitEntityData unit = target.Unit;
            if (unit == null)
            {
                return false;
            }
            foreach (var feat in unit.Progression.Features)
            {
                if (feat.GetComponent<AddImmunityToPrecisionDamage>())
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x0600D408 RID: 54280 RVA: 0x0036F5FC File Offset: 0x0036D7FC
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            return "Any effect or immunity that prevents precision damage negates this ability";
        }
    }
}
