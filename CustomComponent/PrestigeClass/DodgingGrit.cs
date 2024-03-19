using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.Utility;
using Kingmaker;
using Kingmaker.EntitySystem.Entities;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class DodgingGrit : UnitFactComponentDelegate<DodgingGrit.ComponentData>, ITargetRulebookHandler<RuleAttackRoll>, IRulebookHandler<RuleAttackRoll>, ISubscriber, ITargetRulebookSubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>
    {      
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (Data.LastUseTime + 1.Rounds().Seconds > Game.Instance.TimeController.GameTime || evt.IsFake)
            {
                return;
            }
            if (evt.AttackType == Kingmaker.RuleSystem.AttackType.Ranged || evt.AttackType == Kingmaker.RuleSystem.AttackType.RangedTouch)
            {
                base.Data.Initiator = evt.Initiator;
            }
        }

        // Token: 0x0600E355 RID: 58197 RVA: 0x003A9656 File Offset: 0x003A7856
        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            base.Data.Initiator = null;
        }

        // Token: 0x0600E356 RID: 58198 RVA: 0x003A9664 File Offset: 0x003A7864
        public void OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            if (base.Data.Initiator != null && evt.Initiator == base.Data.Initiator)
            {
                evt.AddModifier(2, base.Fact, ModifierDescriptor.UntypedStackable);
                Data.LastUseTime = Game.Instance.TimeController.GameTime;
                Owner.Descriptor.Resources.Spend(this.Resource, 1);
            }
        }

        // Token: 0x0600E357 RID: 58199 RVA: 0x003A96C3 File Offset: 0x003A78C3
        public void OnEventDidTrigger(RuleCalculateAC evt)
        {
        }
        public class ComponentData
        {
            public TimeSpan LastUseTime;
            public UnitEntityData Initiator;
        }

        public BlueprintAbilityResource Resource;
    }
}

