using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class LoreWardenKnow : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, IInitiatorRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>
    {
        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (Buff?.Context?.MaybeCaster != evt.Initiator) return;
            int bonus = 2;
            if (evt.Initiator.Descriptor.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference.Get()) >= 19)
            {
                bonus = 3;
            }
            evt.AddModifier(bonus, Fact, Kingmaker.Enums.ModifierDescriptor.Insight);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (Buff?.Context?.MaybeCaster != evt.Initiator) return;
            int bonus = 2;
            if (evt.Initiator.Descriptor.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference.Get()) >= 19)
            {
                bonus = 3;
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus, Runtime, Kingmaker.Enums.ModifierDescriptor.Insight));
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (Buff?.Context?.MaybeCaster != evt.Target) return;
            int bonus = 0;
            if (evt.Target.Descriptor.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference.Get()) >= 11)
            {
                bonus = 2;
            }
            if (evt.Target.Descriptor.Progression.GetClassLevel(CharacterClassRefs.FighterClass.Reference.Get()) >= 19)
            {
                bonus = 3;
            }
            evt.AddModifier(bonus, Fact, Kingmaker.Enums.ModifierDescriptor.Insight);
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {

        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }
    }
}
