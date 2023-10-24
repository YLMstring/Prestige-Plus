using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
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
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (Owner.HasFact(maneuver)) { return; }
            if (!evt.IsTargetFlatFooted && !evt.TargetIsFlanked) { return; }
            if (evt.Result != AttackResult.Hit && evt.Result != AttackResult.CriticalHit) { return; }
            if (evt.D20 + evt.AttackBonus < evt.TargetAC + 2 && evt.D20 != 20) { return; }
            evt.IsSneakAttack = false;
            evt.IsSneakAttackUsed = true;
            GameHelper.ApplyBuff(Owner, maneuver, new Rounds?(1.Rounds()));
        }

        private static BlueprintBuffReference maneuver = BlueprintTool.GetRef<BlueprintBuffReference>(SneakyManeuver.SneakyManeuverBuff2Guid);
    }
}
