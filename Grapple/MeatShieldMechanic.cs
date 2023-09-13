using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.RuleSystem;
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.Grapple
{
    internal class MeatShieldMechanic : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, ITargetRulebookSubscriber
    {

        // Token: 0x170024CB RID: 9419
        // (get) Token: 0x0600E323 RID: 58147 RVA: 0x003A3DF0 File Offset: 0x003A1FF0
        public BlueprintBuff CooldownBuff;

        // Token: 0x0600E324 RID: 58148 RVA: 0x003A3E04 File Offset: 0x003A2004
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (Owner.HasFact(CooldownBuff))
            {
                return;
            }
            if (Owner.Get<UnitPartGrappleInitiatorPP>() == null)
            {
                return;
            }
            UnitEntityData maybeCaster = Owner.Get<UnitPartGrappleInitiatorPP>().Target;
            if (maybeCaster == null)
            {
                return;
            }
            if (maybeCaster == evt.Initiator)
            {
                return;
            }
            if (this.Check(maybeCaster))
            {
                evt.NewTarget = maybeCaster;
                evt.ReplaceTarget = true;
                GameHelper.ApplyBuff(Owner, CooldownBuff, new Rounds?(1.Rounds()));
            }
        }

        // Token: 0x0600E325 RID: 58149 RVA: 0x003A3E5B File Offset: 0x003A205B
        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
        }

        // Token: 0x0600E326 RID: 58150 RVA: 0x003A3E60 File Offset: 0x003A2060
        private bool Check(UnitEntityData meat)
        {
            UnitEntityData maybeCaster = meat;
            if (maybeCaster.DistanceTo(base.Owner) <= maybeCaster.Stats.Reach.BaseValue.Feet().Meters + maybeCaster.View.Corpulence / 2f + base.Owner.View.Corpulence / 2f + 5.Feet().Meters)
            {
                RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(Owner, meat, CombatManeuver.Grapple, null);
                ruleCombatManeuver = (Owner.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                if (ruleCombatManeuver.Success)
                { 
                    return true;
                }
                else
                {
                    UnitGrappleControllerPP.ReleaseGrapple(Owner);
                }
            }
            return false;
        }

        // Token: 0x04009527 RID: 38183
    }
}
