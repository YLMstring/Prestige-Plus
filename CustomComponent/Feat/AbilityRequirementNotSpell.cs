using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Localization;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class AbilityRequirementNotSpell : BlueprintComponent, IAbilityRestriction, IAbilityVisibilityProvider
    {
        // Token: 0x0600D391 RID: 54161 RVA: 0x0036DA08 File Offset: 0x0036BC08
        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            if (ability.Spellbook != null)
            {
                return false;
            }
            return true;
        }

        // Token: 0x0600D392 RID: 54162 RVA: 0x0036DB5C File Offset: 0x0036BD5C
        public string GetAbilityRestrictionUIText()
        {
            return "It's not a spell. Please look for the masterpiece in your ability tab!";
        }
        bool IAbilityVisibilityProvider.IsAbilityVisible(AbilityData ability)
        {
            if (ability.Spellbook != null)
            {
                return false;
            }
            return true;
        }
    }
}