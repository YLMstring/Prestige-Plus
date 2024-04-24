using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ShootingStarRequirement : BlueprintComponent, IAbilityRestriction
    {
        // Token: 0x0600D391 RID: 54161 RVA: 0x0036DA08 File Offset: 0x0036BC08
        public bool IsAbilityRestrictionPassed(AbilityData ability)
        {
            var caster = ability.Caster;
            if (caster.Alignment.ValueRaw == Kingmaker.Enums.Alignment.ChaoticGood && caster.Progression.GetClassLevel(CharacterClassRefs.BardClass.Reference) >= 10)
            {
                return true;
            }
            if (caster.Stats.Dexterity < 17)
            {
                return false;
            }
            if (caster.Stats.BaseAttackBonus < 11 && caster.Stats.SkillThievery < 11)
            {
                return false;
            }
            if (!caster.HasFact(FeatureRefs.PointBlankShot.Reference))
            {
                return false;
            }
            if (!caster.HasFact(FeatureRefs.RapidShotFeature.Reference))
            {
                return false;
            }
            return true;
        }

        // Token: 0x0600D392 RID: 54162 RVA: 0x0036DB5C File Offset: 0x0036BD5C
        public string GetAbilityRestrictionUIText()
        {
            return "Advanced Prerequisites: Dex 17; Point-Blank Shot; Rapid Shot; base attack bonus +11 or Thievery 11 ranks";
        }
    }
}
