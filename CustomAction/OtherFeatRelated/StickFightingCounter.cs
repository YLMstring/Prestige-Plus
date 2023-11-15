using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Pathfinding.Util.RetainedGizmos;
using Kingmaker.Designers.Mechanics.Facts;
using Newtonsoft.Json;
using Kingmaker;
using PrestigePlus.Blueprint.CombatStyle;

namespace PrestigePlus.CustomAction.OtherFeatRelated
{
    public class StickData
    {
        // Token: 0x040095D2 RID: 38354
        [JsonProperty]
        public TimeSpan LastUseTime;
    }
    internal class StickFightingCounter : UnitFactComponentDelegate<StickData>, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber
    {
        //private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            //Logger.Info("start counter");
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                //Logger.Info("cooldown");
                return;
            }
            if (evt.Target.HasFact(Feat) && evt.Initiator.HasFact(TargetBuff) && !evt.IsHit && evt.Result != AttackResult.MirrorImage && evt.Result != AttackResult.Concealment && evt.AttackType == Kingmaker.RuleSystem.AttackType.Melee && Owner.CombatState.EngagedUnits.Contains(evt.Initiator))
            {
                //Logger.Info("start2");
                var bp = evt.Target.GetThreatHand()?.Weapon?.Blueprint;
                if (bp != null && (bp.Category == Kingmaker.Enums.WeaponCategory.Club || bp.Category == Kingmaker.Enums.WeaponCategory.Quarterstaff))
                {
                    //Logger.Info("start3");
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(Owner, evt.Initiator, false);
                    Data.LastUseTime = Game.Instance.TimeController.GameTime;
                }
            }
        }

        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(SmashingStyle.CounterBuffGuid);
        private static BlueprintFeatureReference Feat = BlueprintTool.GetRef<BlueprintFeatureReference>(SmashingStyle.CounterGuid);
    }
}
