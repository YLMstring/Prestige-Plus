using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Items;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using static Kingmaker.UI.CanvasScalerWorkaround;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.PrestigeClass;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ReactiveStrikeOpp : UnitFactComponentDelegate, IGlobalRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IGlobalRulebookSubscriber
    {
        // Token: 0x0600E40C RID: 58380 RVA: 0x003A69EB File Offset: 0x003A4BEB
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        // Token: 0x0600E40D RID: 58381 RVA: 0x003A69F0 File Offset: 0x003A4BF0
        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.Initiator != base.Owner && Owner.HasFact(RageBuff) && !Owner.HasFact(CasterBuff) && evt.Target.GetFact(TargetBuff)?.MaybeContext?.MaybeCaster == Owner && base.Owner.CombatState.EngagedUnits.Contains(evt.Initiator))
            {
                Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner, evt.Initiator, false);
            }
        }

        private static readonly BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(FuriousGuardian.ChosenAllyBuffGuid);
        private static readonly BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>(FuriousGuardian.ReactiveStrikeBuffGuid);
        private static readonly BlueprintBuffReference RageBuff = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.StandartRageBuff.ToString());
    }
}
