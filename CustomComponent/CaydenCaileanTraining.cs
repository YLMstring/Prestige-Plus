using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using PrestigePlus.CustomComponent.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent
{
    internal class CaydenCaileanTraining : UnitFactComponentDelegate<CaydenCaileanTraining.ComponentData>, IUnitSubscriber, ISubscriber
    {
        public override void OnActivate()
        {
            this.Apply();
        }

        public override void OnDeactivate()
        {
            base.Owner.RemoveFact(base.Data.AppliedFact);
            base.Data.AppliedFact = null;
        }

        private void Apply()
        {
            if (base.Data.AppliedFact != null)
            {
                return;
            }
            if (Owner.HasFact(FeatureRefs.WeaponTrainingLightBlades.Reference))
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat2, null, null);
            }
            else
            {
                base.Data.AppliedFact = base.Owner.AddFact(FeatureRefs.WeaponTrainingLightBlades.Reference, null, null);
            }
        }
        public class ComponentData
        {
            public EntityFact AppliedFact;
        }

        public BlueprintFeature feat2;
    }
}
