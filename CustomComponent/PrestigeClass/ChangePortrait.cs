using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ChangePortrait : UnitFactComponentDelegate<ChangePortrait.ComponentData>
    {
        public override void OnTurnOn()
        {
            Data.portrait = Owner.Blueprint.PortraitSafe;
            Owner.Descriptor.UISettings.SetPortrait(this.Portrait);
        }
        public BlueprintPortrait Portrait;
        // Token: 0x0600C23C RID: 49724 RVA: 0x0032B7EC File Offset: 0x003299EC
        public override void OnTurnOff()
        {
            if (Data.portrait != null)
            {
                Owner.Descriptor.UISettings.SetPortrait(Data.portrait);
            }
        }

        public class ComponentData
        {
            public BlueprintPortrait portrait;
        }
    }
}
