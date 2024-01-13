using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Charge
{
    internal class ChargeConditions : BlueprintComponent, IAbilityRestriction
    {
        string IAbilityRestriction.GetAbilityRestrictionUIText()
        {
            return "Fatigued, Entangled or DifficultTerrain";
        }

        bool IAbilityRestriction.IsAbilityRestrictionPassed(AbilityData ability)
        {
            if (ability.Caster.State.HasCondition(Kingmaker.UnitLogic.UnitCondition.Fatigued))
            {
                return false;
            }
            if (ability.Caster.State.HasCondition(Kingmaker.UnitLogic.UnitCondition.Entangled))
            {
                return false;
            }
            if (ability.Caster.State.HasCondition(Kingmaker.UnitLogic.UnitCondition.DifficultTerrain) && !ability.Caster.HasFact(StagBuff))
            {
                return false;
            }
            return true;
        }
        private static BlueprintBuffReference StagBuff = BlueprintTool.GetRef<BlueprintBuffReference>("{21F094D4-1D59-400B-9CEB-558E6218FB0C}");
    }
}
