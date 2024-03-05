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
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class GoldenLegionnaireRetaliate : UnitFactComponentDelegate<GoldenLegionnaireRetaliate.ComponentData>, IGlobalRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IGlobalRulebookSubscriber
    {
        // Token: 0x0600E74E RID: 59214 RVA: 0x003B624F File Offset: 0x003B444F
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
        }

        // Token: 0x0600E74F RID: 59215 RVA: 0x003B6254 File Offset: 0x003B4454
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (!evt.IsHit || evt.Target == Owner) return;
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime)
            {
                return;
            }
            if (evt.Initiator.IsEnemy(Owner) && evt.Target.IsAlly(Owner) && evt.Target.DistanceTo(Owner) <= 5.Feet().Meters && Owner.CombatState.EngagedUnits.Contains(evt.Initiator))
            {
                Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(base.Owner, evt.Initiator, false);
                Data.LastUseTime = Game.Instance.TimeController.GameTime;
            }
        }

        public class ComponentData
        {
            public TimeSpan LastUseTime;
        }
    }
}
