using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AlliedRetributionTrigger : UnitFactComponentDelegate, ISubscriber, IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, IGlobalRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.Initiator.IsEnemy(Owner) && evt.Target.IsAlly(Owner) && evt.Initiator.DistanceTo(Owner) <= 60.Feet().Meters)
            {
                int level = Owner.Progression.GetClassLevel(golden);
                GameHelper.ApplyBuff(evt.Initiator, buff, new Rounds?(level.Rounds()));
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            
        }

        private static BlueprintCharacterClassReference golden = BlueprintTool.GetRef<BlueprintCharacterClassReference>(GoldenLegionnaire.ArchetypeGuid);
        private static BlueprintBuffReference buff = BlueprintTool.GetRef<BlueprintBuffReference>(GoldenLegionnaire.AlliedRetributionBuffGuid);
    }
}
