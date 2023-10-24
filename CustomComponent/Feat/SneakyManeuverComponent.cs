using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SneakyManeuverComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            //Logger.Info("start1");
            if (Owner.HasFact(maneuver)) { return; }
            //Logger.Info("start2");
            if (evt.AttackType != AttackType.Melee && evt.AttackType != Kingmaker.RuleSystem.AttackType.Touch) { return; }
            //Logger.Info("start3");
            if (!evt.Target.CombatState.IsFlanked && !Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, evt.Target)).IsFlatFooted) { return; }
            //Logger.Info("start4");
            if (evt.Result != AttackResult.Hit && evt.Result != AttackResult.CriticalHit) { return; }
            //Logger.Info(evt.D20.ToString());
            //Logger.Info(evt.AttackBonus.ToString());
            //Logger.Info(evt.TargetAC.ToString());
            if (evt.D20 + evt.AttackBonus < evt.TargetAC + 2 && evt.D20 != 20) { return; }
            //Logger.Info("start5");
            evt.IsSneakAttack = false;
            evt.IsSneakAttackUsed = true;
            GameHelper.ApplyBuff(Owner, maneuver, new Rounds?(1.Rounds()));
        }

        private static BlueprintBuffReference maneuver = BlueprintTool.GetRef<BlueprintBuffReference>(SneakyManeuver.SneakyManeuverBuff2Guid);
    }
}
