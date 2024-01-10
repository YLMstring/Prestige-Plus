using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class SoulPointStuff : UnitFactComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleDrainEnergy>, IRulebookHandler<RuleDrainEnergy>
    {
        void IRulebookHandler<RuleDrainEnergy>.OnEventAboutToTrigger(RuleDrainEnergy evt)
        {
            
        }

        void IRulebookHandler<RuleDrainEnergy>.OnEventDidTrigger(RuleDrainEnergy evt)
        {
            int point = evt.Result;
            if (point > 0)
            {
                Owner.Descriptor.Resources.Restore(this.Resource, point);
            }
        }

        public BlueprintAbilityResource Resource;
    }
}
