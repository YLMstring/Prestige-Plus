using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.GameModes.GameModeType;

namespace PrestigePlus.Modify
{
    internal class TurtleSpecialAC : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            int num = 1;
            if (Owner.HasFact(Shell))
            {
                num = 2;
            }
            evt.AddModifier(num, base.Fact, ModifierDescriptor.Shield);
            if (evt.AttackType.IsTouch() && Owner.HasFact(Clutch))
            {
                evt.AddModifier(num, base.Fact, ModifierDescriptor.UntypedStackable);
            }
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (Owner.HasFact(Shell))
            {
                evt.CriticalConfirmationBonus -= 4;
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }

        private static BlueprintFeatureReference Clutch = BlueprintTool.GetRef<BlueprintFeatureReference>("{E1EE097B-644D-4280-B4E9-0BBF470729C8}");
        private static BlueprintFeatureReference Shell = BlueprintTool.GetRef<BlueprintFeatureReference>("{952D988B-C54C-41F9-95BB-17E6BE7E22FA}");
    }
}
