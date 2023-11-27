using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Blueprints.Root;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs;

namespace PrestigePlus.CustomAction.OtherManeuver
{
    internal class DirtyTrickMaster : ContextAction
    {
        public override string GetCaption()
        {
            return "StealAction";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public void ActManeuver(UnitEntityData caster, UnitEntityData target)
        {
            if (target == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return;
            }
            if (caster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            //var buffold = BlueprintRoot.Instance.SystemMechanics.DirtyTrickSickenedBuff;
            //var buffnew = BuffRefs.Nauseated.Reference.Get();
            //var buffold = BlueprintRoot.Instance.SystemMechanics.DirtyTrickEntangledBuff;
            //var buffnew = BuffRefs.Daze.Reference.Get();
            if (target.HasFact(buffold) && !target.HasFact(buffnew))
            {
                var buff = target.GetFact(buffold) as Buff;
                if (buff == null) { return; }
                var time = buff.EndTime - Game.Instance.TimeController.GameTime;
                target.AddBuff(buffnew, caster, time);
                if (target.HasFact(buffnew))
                {
                    GameHelper.RemoveBuff(target, buffold);
                }
            }
        }

        public BlueprintBuff buffold;
        public BlueprintBuff buffnew;
    }
}
