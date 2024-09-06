using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using static Kingmaker.EntitySystem.EntityDataBase;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class ZonKuthonTripComp : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCombatManeuver>, IRulebookHandler<RuleCombatManeuver>, ISubscriber, IInitiatorRulebookSubscriber
    {
        // Token: 0x0600EA10 RID: 59920 RVA: 0x003C2DD5 File Offset: 0x003C0FD5
        public void OnEventAboutToTrigger(RuleCombatManeuver evt)
        {
        }

        // Token: 0x0600EA11 RID: 59921 RVA: 0x003C2DD7 File Offset: 0x003C0FD7
        public void OnEventDidTrigger(RuleCombatManeuver evt)
        {
            if (evt.Type == CombatManeuver.Trip && evt.Success)
            {
                var wep = Owner.GetThreatHandMelee()?.Weapon;
                if (wep?.Blueprint?.Category == WeaponCategory.Flail || wep?.Blueprint?.Category == WeaponCategory.HeavyFlail)
                {
                    if (Owner.CombatState.AttackOfOpportunity(evt.Target))
                    {
                        Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(Owner, evt.Target, false);
                    }
                }
            }
        }
    }
}
