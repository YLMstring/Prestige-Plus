using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent
{
    internal class ContextSavingThrowBonusAgainstFact : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            try
            {
                UnitEntityData caster = evt.Reason.Caster;
                UnitDescriptor unitDescriptor = caster != null ? caster.Descriptor : null;
                //Logger.Info("start Add save");
                if (unitDescriptor != null && unitDescriptor.HasFact(CheckedFact))
                {
                    //Logger.Info("Add save1");
                    int num = Bonus.Calculate(Fact.MaybeContext) + Value * Fact.GetRank();
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(num, Runtime, Descriptor));
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(num, Runtime, Descriptor));
                    evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(num, Runtime, Descriptor));
                    //Logger.Info("Add save2");
                }
            }
            catch (Exception ex) { Logger.Error("Failed to ContextSavingThrowBonusAgainstFact.", ex); }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }

        public ContextValue Bonus;

        public BlueprintBuffReference CheckedFact;

        // Token: 0x04009644 RID: 38468
        public ModifierDescriptor Descriptor;

        // Token: 0x04009645 RID: 38469
        public int Value;

    }
}
