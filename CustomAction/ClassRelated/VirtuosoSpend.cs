using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomAction.ClassRelated
{
    internal class VirtuosoSpend : ContextAction
    {
        public override string GetCaption()
        {
            return "VirtuosoSpend";
        }

        public override void RunAction()
        {
            if (Context.MaybeCaster == null) { return; }
            int num = 0;
            foreach (ActivatableAbility activatableAbility in Context.MaybeCaster.ActivatableAbilities.RawFacts)
            {
                if (activatableAbility.Blueprint.Group == ActivatableAbilityGroup.BardicPerformance && (activatableAbility.IsStarted || activatableAbility.IsOn))
                {
                    num += activatableAbility.Blueprint.WeightInGroup;
                }
            }
            if (num < 2) { return; }
            var Res = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(ResGuid);
            if (Res?.Get() != null && Context.MaybeCaster.Descriptor.Resources.GetResource(Res) != null)
            {
                Context.MaybeCaster.Descriptor.Resources.Spend(Res, 1);
            }
        }

        public string ResGuid;
    }
}
