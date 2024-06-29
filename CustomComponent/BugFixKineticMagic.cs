using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Blueprints.References;
using Owlcat.Runtime.Core.Utils;

namespace PrestigePlus.CustomComponent
{
    internal class BugFixKineticMagic : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
            //Main.Logger.Info("source1" + evt.DamageBundle.First()?.SourceFact?.ToString() + "source2" + evt.SourceAbility?.ToString() + "source3" + evt.SourceArea?.ToString());
            if (evt.SourceArea?.GetComponent<ContextCalculateAbilityParamsBasedOnClass>()?.CharacterClass == CharacterClassRefs.KineticistClass.Reference.Get()) 
            {
                List<BaseDamage> list = TempList.Get<BaseDamage>();
                foreach (BaseDamage damage in evt.DamageBundle)
                {
                    list.Add(this.ChangeType(damage));
                }
                evt.Remove((BaseDamage _) => true);
                foreach (BaseDamage damage2 in list)
                {
                    evt.Add(damage2);
                }
            }
        }

        public BaseDamage ChangeType(BaseDamage damage)
        {
            if (damage is PhysicalDamage phy && phy.EnchantmentTotal < 1)
            {
                var des = damage.CreateTypeDescription();
                des.Physical.EnhancementTotal = 1;
                BaseDamage baseDamage = des.CreateDamage(new ModifiableDiceFormula(damage.Dice), damage.Bonus);
                baseDamage.CopyFrom(damage);
                return baseDamage;
            }
            return damage; 
        }

        // Token: 0x0600C137 RID: 49463 RVA: 0x003276D8 File Offset: 0x003258D8
        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            
        }
    }
}
