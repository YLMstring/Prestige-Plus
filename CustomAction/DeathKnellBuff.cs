using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers;
using Kingmaker.Utility;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.PrestigeClasses;

namespace PrestigePlus.CustomAction
{
    internal class DeathKnellBuff : ContextAction
    {
        public override string GetCaption()
        {
            return "DeathKnell";
        }

        public override void RunAction()
        {
            ActManeuver(Context.MaybeCaster, Target.Unit);
        }

        public static void ActManeuver(UnitEntityData caster, UnitEntityData target)
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
            int level = target.Progression.CharacterLevel;
            int num = level * 100;
            GameHelper.ApplyBuff(caster, holy, new Rounds?(num.Rounds()));
        }

        private static BlueprintBuffReference holy = BlueprintTool.GetRef<BlueprintBuffReference>(HolyVindicator.DivineJudgmentBuff2Guid);
    }
}
