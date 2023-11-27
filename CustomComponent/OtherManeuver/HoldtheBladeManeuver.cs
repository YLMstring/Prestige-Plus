using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.ContextData;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.SpecificManeuver;
using PrestigePlus.CustomAction.OtherManeuver;
using PrestigePlus.CustomComponent.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.OtherManeuver
{
    internal class HoldtheBladeManeuver : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.RuleAttackWithWeapon?.Weapon?.Blueprint.IsMelee == true && evt.IsHit && (Owner.CombatState.IsFlanked || evt.IsSneakAttack))
            {
                if (Owner.HasFact(CasterBuff) || !ConditionTwoFreeHand.HasFreeHand(Owner)) { return; }
                SweepManeuver.ActManeuver(Owner, evt.Initiator, 0);
                GameHelper.ApplyBuff(Owner, CasterBuff, new Rounds?(1.Rounds()));
            }
        }

        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>(RangedDisarm.HoldtheBladebuffGuid);
    }
}