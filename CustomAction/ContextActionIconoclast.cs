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
using PrestigePlus.Modify;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.ElementsSystem;
using Mono.Cecil;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomAction
{
    internal class ContextActionIconoclast : ContextAction
    {
        public override string GetCaption()
        {
            return "Iconoclast";
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
            if (!DivineDefianceBonus.CheckConditionDivine(target)) { return; }
            Action2.Run();
        }
        
        public static ActionList Action1 = ActionsBuilder.New().CastSpell(AbilityRefs.DazzlingDisplayAction.ToString()).Build();
        public static ActionList Action2 = ActionsBuilder.New().OnContextCaster(Action1).Build();
    }
}
