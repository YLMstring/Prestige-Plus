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
using PrestigePlus.CustomComponent.Archetype;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class DirtyTrickPlus : UnitFactComponentDelegate<DirtyTrickPlus.ComponentData>, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCombatManeuver>.OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type == CombatManeuver.DirtyTrickEntangle && evt.Target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff))
            {
                Data.oldbuff = BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff;
                Data.newbuff = BuffRefs.Staggered.Reference.Get();
            }
            else if (evt.Type == CombatManeuver.DirtyTrickSickened && evt.Target.HasFact(BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff))
            {
                Data.oldbuff = BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff;
                Data.newbuff = BuffRefs.Nauseated.Reference.Get();
            }
        }

        void IRulebookHandler<RuleCombatManeuver>.OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (!evt.Success || Data.oldbuff == null) return;
            var target = evt.Target;
            var caster = evt.Initiator;
            if (target.HasFact(Data.oldbuff) && !target.HasFact(Data.newbuff))
            {
                var buff = target.GetFact(Data.oldbuff) as Buff;
                if (buff == null) { return; }
                var time = buff.EndTime - Game.Instance.TimeController.GameTime;
                target.AddBuff(Data.newbuff, caster, time);
                if (target.HasFact(Data.newbuff))
                {
                    GameHelper.RemoveBuff(target, Data.oldbuff);
                }
            }
            Data.oldbuff = null;
            Data.newbuff = null;
        }

        public class ComponentData
        {
            public BlueprintBuff oldbuff = null;
            public BlueprintBuff newbuff = null;
        }
    }
}
