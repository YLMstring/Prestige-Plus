using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.UnitLogic.Abilities.Components;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Mechanic
{
    internal class AddToResourceFact
    {
        public static void Patch(string abilityguid, bool isIncrease, BlueprintUnitFact fact) 
        {
            var ability = BlueprintTool.GetRef<BlueprintAbilityReference>(abilityguid)?.Get()?.GetComponent<AbilityResourceLogic>();
            if (ability == null) { return; }
            if (isIncrease)
            {
                ability.ResourceCostIncreasingFacts.Add(fact.ToReference<BlueprintUnitFactReference>());
            }
            else
            {
                ability.ResourceCostDecreasingFacts.Add(fact.ToReference<BlueprintUnitFactReference>());
            }
        }
    }
}
