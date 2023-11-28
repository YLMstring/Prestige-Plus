using Kingmaker.Blueprints.Root;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using BlueprintCore.Blueprints.References;
using Kingmaker.Designers;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class DirtyTrickPlus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type == CombatManeuver.DirtyTrickEntangle && evt.Target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff))
            {
                oldbuff = BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff;
                newbuff = BuffRefs.Staggered.Reference.Get();
            }
            else if (evt.Type == CombatManeuver.DirtyTrickSickened && evt.Target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff))
            {
                oldbuff = BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff;
                newbuff = BuffRefs.Nauseated.Reference.Get();
            }
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (!evt.Success || oldbuff == null) return;
            var target = evt.Target;
            var caster = evt.Initiator;
            if (target.HasFact(oldbuff) && !target.HasFact(newbuff))
            {
                var buff = target.GetFact(oldbuff) as Buff;
                if (buff == null) { return; }
                var time = buff.EndTime - Game.Instance.TimeController.GameTime;
                target.AddBuff(newbuff, caster, time);
                if (target.HasFact(newbuff))
                {
                    GameHelper.RemoveBuff(target, oldbuff);
                }
            }
            oldbuff = null;
            newbuff = null;
        }

        private BlueprintBuff oldbuff = null;
        private BlueprintBuff newbuff = null;
    }
}
