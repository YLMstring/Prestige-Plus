using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PrestigePlus.Modify
{
    internal class DFbindwind : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            //Logger.Info("check");
            if (evt.Result == AttackResult.Miss)
            {
                //Logger.Info("start");
                Feet feet = 5.Feet();
                Vector3 normalized = (evt.Target.Position - evt.Initiator.Position).normalized;
                evt.Target.Ensure<UnitPartForceMove>().Push(normalized, feet.Meters, false);
            }
        }
    }
}
