using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SavingAgainstAOE : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600E813 RID: 59411 RVA: 0x003B8470 File Offset: 0x003B6670
        public void OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            int value = this.Value + this.Bonus.Calculate(base.Context);
            var data = evt.Reason?.Context?.SourceAbilityContext?.Ability;
            if (data?.IsAOE == true && data.Blueprint.Type == Kingmaker.UnitLogic.Abilities.Blueprints.AbilityType.Spell)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(value, base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(value, base.Runtime, this.Descriptor));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(value, base.Runtime, this.Descriptor));
            }
        }

        // Token: 0x0600E814 RID: 59412 RVA: 0x003B857E File Offset: 0x003B677E
        public void OnEventDidTrigger(RuleSavingThrow evt)
        {
        }

        // Token: 0x040098C4 RID: 39108
        public ModifierDescriptor Descriptor;

        // Token: 0x040098C5 RID: 39109
        public int Value;

        // Token: 0x040098C6 RID: 39110
        public ContextValue Bonus;
    }
}
