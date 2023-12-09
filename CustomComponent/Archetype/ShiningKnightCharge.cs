using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class ShiningKnightCharge : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.AttackRoll.IsHit || evt.Target == null || !evt.Target.HasFact(BuffRefs.SmiteEvilBuff.Reference)) { return; }
            int level = Owner.Progression.GetClassLevel(CharacterClassRefs.PaladinClass.Reference);
            int dc = Owner.Stats.Charisma.Bonus + 10 + level / 2;
            bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveWill, dc)
            {
                IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
            }, evt.Target.Context, true).Success;
            if (!pass)
            {
                GameHelper.ApplyBuff(evt.Target, BuffRefs.ShamanHexFearfulGazePanickedBuff.Reference, new Rounds?((level / 2).Rounds()));
            }

        }
    }
}

