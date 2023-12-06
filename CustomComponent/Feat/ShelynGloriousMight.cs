using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ShelynGloriousMight : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (Owner.HasFact(FeatureRefs.SmiteEvilFeature.Reference))
            {
                if (evt.Target.HasFact(BuffRefs.SmiteEvilBuff.Reference))
                {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(Owner.Stats.Charisma.Bonus, base.Runtime));
                }
            }
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (Owner.HasFact(FeatureRefs.SmiteEvilFeature.Reference))
            {
                if (evt.Target.HasFact(BuffRefs.SmiteEvilBuff.Reference))
                {
                    evt.AddModifier(Owner.Stats.Charisma.Bonus, base.Fact);
                }
            }
            else
            {
                if (evt.Target.Descriptor.Alignment.ValueRaw.HasComponent(AlignmentComponent.Evil))
                {
                    evt.AddModifier(2, base.Fact);
                }
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {
            
        }
    }
}
