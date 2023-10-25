using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class SpellSlotToRes : ContextAction
    {
        public override string GetCaption()
        {
            return "SpellSlotToRes";
        }

        public override void RunAction()
        {
            if (Context.MaybeCaster == null || Context.SpellLevel < 1) { return; }
            var Res = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(ResGuid);
            if (!Res?.Get())
            {
                return;
            }
            Context.MaybeCaster.Descriptor.Resources.Restore(Res, Context.SpellLevel);
        }

        public string ResGuid;
    }
}
