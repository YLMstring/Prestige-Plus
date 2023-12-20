using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class CombatJuggling : UnitFactComponentDelegate<CombatJuggling.ComponentData>, IGlobalSubscriber, ISubscriber
    {
        // Token: 0x0600C35D RID: 50013 RVA: 0x0032FD90 File Offset: 0x0032DF90
        public override void OnTurnOn()
        {
            base.Owner.State.Features.DeflectArrows.Retain();
            base.Data.FeatureEnabled = true;
        }

        // Token: 0x0600C35E RID: 50014 RVA: 0x0032FD98 File Offset: 0x0032DF98
        public override void OnTurnOff()
        {
            if (base.Data.FeatureEnabled)
            {
                base.Owner.State.Features.DeflectArrows.Release();
                base.Data.FeatureEnabled = false;
            }
        }

        public class ComponentData
        {
            public bool FeatureEnabled;
        }
    }
}
