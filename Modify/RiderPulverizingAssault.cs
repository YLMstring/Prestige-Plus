using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.Modify
{
    internal class RiderPulverizingAssault : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.AttackRoll.IsHit) { return; }
            if (Owner.HasFact(Cooldown) || !Owner.HasFact(BlueprintRoot.Instance.SystemMechanics.ChargeBuff))
            {
                return;
            }
            UnitEntityData mount = Owner.GetRider();
            if (mount == null) return;
            int dc = Owner.Stats.Strength.Bonus + 20;
            bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveFortitude, dc)
            {
                IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
            }, evt.Target.Context, true).Success;
            if (!pass)
            {
                int time = UnityEngine.Random.Range(1, 5);
                GameHelper.ApplyBuff(evt.Target, stagger, new Rounds?(time.Rounds()));
            }
            GameHelper.ApplyBuff(Owner, Cooldown, new Rounds?(1.Rounds()));
        }
        private static BlueprintBuffReference Cooldown = BlueprintTool.GetRef<BlueprintBuffReference>("{7EA9F7F5-408B-48BD-B7F6-44AA5B56CEDA}");
        private static BlueprintBuffReference stagger = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Staggered.ToString());
    }
}
