using BlueprintCore.Utils;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Designers;
using Kingmaker.DialogSystem.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.TimedProbabilityCurve;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class TryDeflectRay : UnitFactComponentDelegate, IRulebookHandler<RuleAttackRoll>, ISubscriber, IUnitSubscriber, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (evt.AttackType == AttackType.RangedTouch && !evt.IsFake && Buff != null && evt.Initiator.IsPlayersEnemy)
            {
                if (Game.Instance.TimeController.GameTime - Owner.CombatState.m_LastDeflectArrowTime < 1.Rounds().Seconds || Rulebook.Trigger(new RuleCheckTargetFlatFooted(evt.Initiator, Owner)).IsFlatFooted)
                {
                    return;
                }
                evt.AutoMiss = true;
                Owner.CombatState.m_LastDeflectArrowTime = Game.Instance.TimeController.GameTime;
                EventBus.RaiseEvent(delegate (IUILogUnitDeflectArrowHandler h)
                {
                    h.HandleUnitDeflectArrow(Owner);
                }, true);
                var maybeCaster = Owner;
                GameHelper.ApplyBuff(maybeCaster, Buff, new Rounds?(2.Rounds()));
                int max = 1;
                if (maybeCaster.HasFact(Fast))
                {
                    max += maybeCaster.GetFact(Fast).GetRank();
                }
                if (maybeCaster.HasFact(Mythic))
                {
                    max += maybeCaster.Progression.MythicLevel / 2;
                }
                int time = maybeCaster.GetFact(Buff).GetRank();
                if (time < max)
                {
                    Owner.CombatState.m_LastDeflectArrowTime = default;
                }
            }
        }
        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {

        }

        private static BlueprintBuffReference Buff = BlueprintTool.GetRef<BlueprintBuffReference>(Juggler.FastReactionsBuffGuid);
        private static BlueprintFeatureReference Fast = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.FastReactionsGuid);
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(Juggler.DeflectArrowsMythicGuid);
    }
}
