using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class AbilityTargetLessHD : BlueprintComponent, IAbilityTargetRestriction
    {
        // Token: 0x0600D407 RID: 54279 RVA: 0x0036F52C File Offset: 0x0036D72C
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
        {
            UnitEntityData unit = target.Unit;
            if (unit == null)
            {
                return false;
            }
            return unit.Progression.CharacterLevel <= caster.Progression.CharacterLevel;
        }

        // Token: 0x0600D408 RID: 54280 RVA: 0x0036F5FC File Offset: 0x0036D7FC
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            return "Creatures with more Hit Dice than yours are unaffected";
        }
    }
}
