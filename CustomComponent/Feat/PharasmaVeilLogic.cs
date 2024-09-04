﻿using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class PharasmaVeilLogic : UnitFactComponentDelegate<KabririGhoul.ComponentData>, IUnitSubscriber, ISubscriber
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
            if (Owner.Progression.GetClassLevel(CharacterClassRefs.OracleClass.Reference) >= 1)
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat1, null, null);
                LevelUpController controller = Kingmaker.Game.Instance?.LevelUpController;
                controller?.State.AddSelection(null, CharacterClassRefs.OracleClass.Reference.Get(), FeatureSelectionRefs.OracleRevelationSelection.Reference.Get(), 2);
            }
            else
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat2, null, null);
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
