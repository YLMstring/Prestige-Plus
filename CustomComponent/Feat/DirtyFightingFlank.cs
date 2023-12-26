using BlueprintCore.Blueprints.References;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class DirtyFightingFlank : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateCMB>, IRulebookHandler<RuleCalculateCMB>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateCMB>.OnEventAboutToTrigger(RuleCalculateCMB evt)
        {

        }

        void IRulebookHandler<RuleCalculateCMB>.OnEventDidTrigger(RuleCalculateCMB evt)
        {
            if (evt.AllBonuses.Any())
            {
                int apply = 0;
                foreach (var bonus in evt.AllBonuses)
                {
                    if (FeatureRefs.Outflank.Reference.Get() == bonus.Fact?.Blueprint)
                    {
                        return;
                    }
                    if (bonus.Type == BonusType.Flanking && bonus.Value < 4)
                    {
                        apply = 4 - bonus.Value;
                    }
                }
                if (apply > 0)
                {
                    evt.AddModifier(apply, BonusType.Flanking);
                    evt.Result = evt.Base.Result + evt.TotalBonusValue;
                }
            }
        }
    }
}
