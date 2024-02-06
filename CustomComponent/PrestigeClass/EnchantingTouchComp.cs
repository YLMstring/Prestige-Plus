using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class EnchantingTouchComp : UnitFactComponentDelegate, IUnitTryCastingDefensivelyHandler
    {
        UnitEntityData IUnitSubscriber.GetSubscribingUnit()
        {
            return Owner;
        }

        void IUnitTryCastingDefensivelyHandler.HandleUnitTryCastingDefensively(AbilityData ability, ref bool skipCasting)
        {
            if (ability.Blueprint.Type == AbilityType.Spell && ability.Range == AbilityRange.Touch)
            {
                skipCasting = true;
            }
        }
    }
}
