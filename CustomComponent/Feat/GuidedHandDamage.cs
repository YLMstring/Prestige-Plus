using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    [TypeId("{7A6AB932-67E6-49B5-B0D5-4C1E58C1082B}")]
    internal class GuidedHandDamage : UnitFactComponentDelegate<GuidedHandDamage.ComponentData>, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (Data.cat.Count() == 0)
            {
                Data.cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
            }
            if (Data.cat.Contains(evt.Weapon.Blueprint.Category))
            {
                evt.OverrideDamageBonusStat(StatType.Wisdom);
                evt.TwoHandedStatReplacement = true;
                if (evt.Weapon.HoldInTwoHands)
                {
                    evt.DamageBonusStatMultiplier = 1.5f;
                }
            }
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        public override void OnActivate()
        {
            Data.cat = PrerequisiteDivineWeapon.GetFavoredWeapon(Owner);
        }

        public override void OnDeactivate()
        {
            Data.cat.Clear();
        }

        public class ComponentData
        {
            public List<WeaponCategory> cat = new() { };
        }       
    }
}
