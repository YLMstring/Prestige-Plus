using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic;
using Owlcat.Runtime.Core.Utils;
using PrestigePlus.Blueprint.Feat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.PrestigeClass;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.Common;
using Kingmaker.Designers;
using Kingmaker.Utility;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class AnkouShadowPenalty : UnitFactComponentDelegate<AnkouShadowPenalty.ComponentData>, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public override void OnActivate()
        {
            
        }

        public override void OnDeactivate()
        {
            Data.cat.Clear();
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (Data.cat.Contains(evt.Target))
            {
                int chance = 20;
                if (Owner.HasFact(FeatureRefs.PowerfulShadows.Reference))
                {
                    chance += 20;
                }
                if (Owner.HasFact(BuffRefs.ShadowPlaneMagicBuff.Reference))
                {
                    chance += 10;
                }
                UIUtility.SendWarning("This attack has " + chance.ToString() + "% chance to become real.");
                if (UnityEngine.Random.Range(0, 100) >= chance)
                {
                    evt.AutoMiss = true;
                }
            }
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit && !Data.cat.Contains(evt.Target))
            {
                int dc = Owner.Stats.Intelligence.Bonus + 20;
                bool pass = Owner.Context.TriggerRule(new RuleSavingThrow(evt.Target, SavingThrowType.Will, dc)).Success;
                if (pass)
                {
                    Data.cat.Add(evt.Target);
                    UIUtility.SendWarning(evt.Target.CharacterName + " recognizes the illusion.");
                }
            }
        }

        public class ComponentData
        {
            public List<UnitEntityData> cat = new() { };
        }
    }
}
