using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomComponent.BasePrestigeEnhance
{
    internal class PenitentCMD : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateCMD>, IRulebookHandler<RuleCalculateCMD>, ISubscriber, ITargetRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateCMD evt)
        {
            var buffs = evt.Initiator.GetFacts(BuffRefs.CavalierChallengeBuffTarget.Reference);
            if (buffs == null || buffs.Count() == 0 ) { return; }
            foreach ( var buff in buffs )
            {
                if (buff.MaybeContext?.MaybeCaster == Owner)
                {
                    evt.AddModifier(1 + Owner.Progression.GetClassLevel(CharacterClassRefs.CavalierClass.Reference) / 4, base.Fact, ModifierDescriptor.Morale);
                    return;
                }
            }
        }
        public void OnEventDidTrigger(RuleCalculateCMD evt)
        {
        }
    }
}