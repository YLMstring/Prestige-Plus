using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class NorgorberBomb : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackRoll>.OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            
        }

        void IRulebookHandler<RuleAttackRoll>.OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (evt.IsHit && evt.Weapon?.Blueprint.Type == WeaponTypeRefs.BombType.Reference.Get())
            {
                var action = AbilityRefs.Poison.Reference.Get().GetComponent<AbilityEffectRunAction>()?.Actions.Actions.First() as ContextActionConditionalSaved;
                if (action?.Failed == null) return;
                int dc = Owner.Stats.Wisdom.Bonus + 13;
                bool pass = GameHelper.TriggerSkillCheck(new RuleSkillCheck(evt.Target, Kingmaker.EntitySystem.Stats.StatType.SaveWill, dc)
                {
                    IgnoreDifficultyBonusToDC = evt.Target.IsPlayersEnemy
                }, evt.Target.Context, true).Success;
                if (!pass)
                {
                    IFactContextOwner factContextOwner = base.Fact as IFactContextOwner;
                    factContextOwner?.RunActionInContext(action.Failed, evt.Target);
                }
            }
        }
    }
}
