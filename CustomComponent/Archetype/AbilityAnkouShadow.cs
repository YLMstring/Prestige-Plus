using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class AbilityAnkouShadow : BlueprintComponent, IAbilityTargetRestriction
    {
        // Token: 0x0600D407 RID: 54279 RVA: 0x0036F52C File Offset: 0x0036D72C
        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
        {
            var image = caster.Get<UnitPartMirrorImage>();
            if (image == null) return false;
            int rank = 0;
            var fact = caster.GetFact(CooldownBuff);
            if (fact != null)
            {
                rank = fact.GetRank();
            }
            if (image.MechanicsImages.Count > rank)
            {
                return true;
            }
            return false;
        }

        // Token: 0x0600D408 RID: 54280 RVA: 0x0036F5FC File Offset: 0x0036D7FC
        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            return "All Shadow Doubles Have Taken Actions";
        }
        public BlueprintBuff CooldownBuff;
    }
}