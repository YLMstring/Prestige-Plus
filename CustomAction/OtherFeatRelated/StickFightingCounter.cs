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
using Kingmaker.Designers;

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
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {

        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                return;
            }
            if (evt.Initiator.HasFact(TargetBuff) && !evt.IsHit && evt.Result != AttackResult.MirrorImage && evt.Result != AttackResult.Concealment && evt.AttackType == Kingmaker.RuleSystem.AttackType.Melee && Owner.CombatState.EngagedUnits.Contains(evt.Initiator))
            {
                var bp = Owner.GetThreatHand()?.Weapon?.Blueprint;
                if (bp != null && (bp.Category == Kingmaker.Enums.WeaponCategory.Club || bp.Category == Kingmaker.Enums.WeaponCategory.Quarterstaff))
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(Owner, evt.Initiator, false);
                    Data.LastUseTime = Game.Instance.TimeController.GameTime;
                }
                else if (Owner.HasFact(Spiral) && bp != null && (bp.FighterGroup == Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroupFlags.Spears || bp.FighterGroup == Kingmaker.Blueprints.Items.Weapons.WeaponFighterGroupFlags.Polearms))
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(Owner, evt.Initiator, false);
                    Data.LastUseTime = Game.Instance.TimeController.GameTime;
                }
            }
        }

        private static BlueprintBuffReference TargetBuff = BlueprintTool.GetRef<BlueprintBuffReference>(SmashingStyle.CounterBuffGuid);
        private static BlueprintBuffReference Spiral = BlueprintTool.GetRef<BlueprintBuffReference>(SpearDancingStyle.SpiralbuffGuid);
    }
}
