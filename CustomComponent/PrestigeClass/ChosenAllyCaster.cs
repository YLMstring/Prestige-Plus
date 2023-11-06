using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using static Kingmaker.Blueprints.Area.FactHolder;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ChosenAllyCaster : UnitFactComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (Owner.HasFact(TargetBuff) || !Owner.HasFact(RageBuff)) { return; }
            foreach (var unit in Owner.Group)
            {
                if (unit.GetFact(TargetBuff)?.MaybeContext?.MaybeCaster == Owner)
                {
                    if (unit.DistanceTo(Owner) <= 8.Feet().Meters)
                    {
                        evt.AddModifier(-1, base.Fact, Kingmaker.Enums.ModifierDescriptor.Penalty);
                    }
                    return;
                }
            }    
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }
        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(FuriousGuardian.ChosenAllyBuffGuid);
        private static readonly BlueprintBuffReference RageBuff = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.StandartRageBuff.ToString());
    }
}
