using Kingmaker.Blueprints.Classes;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class KabririGhoul : UnitFactComponentDelegate<KabririGhoul.ComponentData>, IUnitSubscriber, ISubscriber
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
            if (Owner.HasFact(FeatureRefs.UndeadType.Reference) || Owner.HasFact(FeatureSelectionRefs.MythicLichSkeletonSelection.Reference))
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat2, null, null);
            }
            else
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat1, null, null);
            }
        }
        public class ComponentData
        {
            public EntityFact AppliedFact;
        }

        public BlueprintFeature feat1;
        public BlueprintFeature feat2;
    }
}
