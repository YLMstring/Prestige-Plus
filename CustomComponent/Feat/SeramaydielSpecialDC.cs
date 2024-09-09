using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.UnitLogic.Buffs.Blueprints;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class SeramaydielSpecialDC : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public BlueprintAbility Ability;
        public BlueprintBuff buff;
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (evt.Spell == this.Ability)
            {
                evt.ReplaceDC = new int?(Owner.GetFact(buff)?.GetRank() ?? 0);
            }
        }

        // Token: 0x0600EBAD RID: 60333 RVA: 0x003C895C File Offset: 0x003C6B5C
        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

    }
}