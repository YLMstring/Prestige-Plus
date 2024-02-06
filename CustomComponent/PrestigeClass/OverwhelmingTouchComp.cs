using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class OverwhelmingTouchComp : UnitFactComponentDelegate, ISubscriber, IGlobalRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, IGlobalRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (evt.Reason?.Caster == Owner && evt.Reason.Ability?.Blueprint.Type == AbilityType.Spell && evt.Reason.Ability.Range == AbilityRange.Touch)
            {
                GameHelper.ApplyBuff(evt.Initiator, TargetBuff, new Rounds?(1.Rounds()));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(EnchantingCourtesan.OverwhelmingBuffGuid);
    }
}
