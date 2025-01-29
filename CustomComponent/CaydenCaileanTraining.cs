using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
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

    internal class UrgathoaTraining : UnitFactComponentDelegate<UrgathoaTraining.ComponentData>, IUnitSubscriber, ISubscriber
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
            if (!Owner.HasFact(FeatureRefs.WeaponTrainingHeavyBlades.Reference))
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat2, null, null);
            }
            else
            {
                base.Data.AppliedFact = base.Owner.AddFact(FeatureRefs.WeaponTrainingHeavyBlades.Reference, null, null);
            }
        }
        public class ComponentData
        {
            public EntityFact AppliedFact;
        }

        public BlueprintFeature feat2;
    }

    internal class UrgathoaBraveTraining : UnitFactComponentDelegate<UrgathoaBraveTraining.ComponentData>, IUnitSubscriber, ISubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>
    {

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var caster = evt.Reason?.Caster;
            if (caster?.HasFact(FeatureRefs.UndeadType.Reference) == true)
            {
                int num = Owner.GetFact(FeatureRefs.Bravery.Reference).GetRank();
                if (num < 2) { return; }
                if (!evt.Reason.Context.SpellDescriptor.HasAnyFlag(SpellDescriptor.Fear))
                {
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(num, base.Runtime));
                }
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(num, base.Runtime));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(num, base.Runtime));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }
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
            if (!Owner.HasFact(FeatureRefs.Bravery.Reference))
            {
                base.Data.AppliedFact = base.Owner.AddFact(feat2, null, null);
            }
            else
            {
                base.Data.AppliedFact = base.Owner.AddFact(FeatureRefs.Bravery.Reference, null, null);
            }
        }
        public class ComponentData
        {
            public EntityFact AppliedFact;
        }

        public BlueprintFeature feat2;
    }
}
