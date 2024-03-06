using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class UnitedDefensePenalty : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            int bonus = 1;
            if (Owner.Progression.GetClassLevel(guard) >= 6)
            {
                bonus = 2;
            }
            if (Owner.Progression.GetClassLevel(guard) >= 10)
            {
                bonus = 3;
            }
            evt.AddModifier(-bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Penalty);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        private static readonly BlueprintCharacterClassReference guard = BlueprintTool.GetRef<BlueprintCharacterClassReference>(GoldenLegionnaire.ArchetypeGuid);
    }
}