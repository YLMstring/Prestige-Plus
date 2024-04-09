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
            if (num > 0)
            {
                Context.SpellLevel = num;
            }
            if (Context.MaybeCaster == null || Context.SpellLevel < 1) { return; }
            var Res = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(ResGuid);
            if (Res?.Get() != null && Context.MaybeCaster.Descriptor.Resources.GetResource(Res) != null)
            {
                Context.MaybeCaster.Descriptor.Resources.Restore(Res, Context.SpellLevel);
            }
            else if (Res2Guid != null)
            {
                var Res2 = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(Res2Guid);
                if (Res2?.Get() != null && Context.MaybeCaster.Descriptor.Resources.GetResource(Res2) != null)
                {
                    Context.MaybeCaster.Descriptor.Resources.Restore(Res2, Context.SpellLevel);
                }
            }
        }

        public string ResGuid;
        public string Res2Guid;
        public int num = 0;
    }
}
