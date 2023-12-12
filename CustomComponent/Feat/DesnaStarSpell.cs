using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class DesnaStarSpell : UnitFactComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>
    {
        void IRulebookHandler<RuleCalculateDamage>.OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            if (evt.Reason.Ability?.Blueprint.Type != Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Spell)
            {
                return;
            }
            if (Owner.HasFact(star) || Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Evening || Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Night)
            {
                EnergyDamage dmg = new(new DiceFormula(2, DiceType.D6), 0, Kingmaker.Enums.Damage.DamageEnergyType.Divine);
                evt.ParentRule?.Add(dmg);
            }
        }
        void IRulebookHandler<RuleCalculateDamage>.OnEventDidTrigger(RuleCalculateDamage evt)
        {
            
        }

        private static BlueprintFeatureReference star = BlueprintTool.GetRef<BlueprintFeatureReference>(DeificObedience.Desna3Guid);
    }
}
