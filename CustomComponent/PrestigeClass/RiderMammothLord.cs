using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker;
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
using static Pathfinding.Util.RetainedGizmos;

namespace PrestigePlus.Modify
{
    internal class RiderMammothLord : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.AttackRoll.IsHit || evt.Target == null || evt.Target.HasFact(Cooldown)) { return; }
            UnitEntityData mount = Owner.GetSaddledUnit();
            if (mount != null) RiderAction(evt);
            UnitEntityData rider = Owner.GetRider();
            if (rider != null) MountAction(evt);
        }
        private static BlueprintBuffReference Cooldown = BlueprintTool.GetRef<BlueprintBuffReference>("{9FC08BA6-263E-4E95-96C2-55E3835314DA}");
        private static BlueprintBuffReference buff1 = BlueprintTool.GetRef<BlueprintBuffReference>("{ED201D33-F183-47B5-A43F-4C10D4260948}");
        private static BlueprintBuffReference buff2 = BlueprintTool.GetRef<BlueprintBuffReference>("{ECB56394-C6DA-4EFB-AE8C-93B058632DAB}");

        private static BlueprintBuffReference stun = BlueprintTool.GetRef<BlueprintBuffReference>(BuffRefs.Stunned.ToString());
        private void RiderAction(RuleAttackWithWeapon evt)
        {
            GameHelper.ApplyBuff(evt.Target, buff1, new Rounds?(1.Rounds()));
            if (evt.Target.HasFact(buff1) && evt.Target.HasFact(buff2))
            {
                FinalAction(evt, Owner.GetSaddledUnit());
            }
        }

        private void MountAction(RuleAttackWithWeapon evt)
        {
            GameHelper.ApplyBuff(evt.Target, buff2, new Rounds?(1.Rounds()));
            if (evt.Target.HasFact(buff1) && evt.Target.HasFact(buff2))
            {
                FinalAction(evt, Owner);
            }
        }

        private void FinalAction(RuleAttackWithWeapon evt, UnitEntityData mount)
        {
            var round = new Rounds?(14400.Rounds());
            GameHelper.ApplyBuff(evt.Target, Cooldown, round);
            GameHelper.RemoveBuff(evt.Target, buff1);
            GameHelper.RemoveBuff(evt.Target, buff2);
            int dc = mount.Stats.Strength.Bonus + 20;
            bool pass = Owner.Context.TriggerRule(new RuleSavingThrow(evt.Target, Kingmaker.EntitySystem.Stats.SavingThrowType.Fortitude, dc)).Success;
            if (!pass)
            {
                GameHelper.ApplyBuff(evt.Target, stun, new Rounds?(1.Rounds()));
            }
        }
    }
}
