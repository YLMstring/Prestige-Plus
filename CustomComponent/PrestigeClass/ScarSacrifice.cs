using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using BlueprintCore.Utils;
using Kingmaker.Designers;
using Kingmaker.Blueprints.JsonSystem;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    [TypeId("{E086C833-5097-415D-8243-6AEC44B01A7E}")]
    internal class ScarSacrifice : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public BlueprintBuff CheckBuff
        {
            get
            {
                BlueprintBuffReference checkBuff = m_CheckBuff;
                if (checkBuff == null)
                {
                    return null;
                }
                return checkBuff.Get();
            }
        }

        public BlueprintBuff CooldownBuff
        {
            get
            {
                BlueprintBuffReference cooldownBuff = m_CooldownBuff;
                if (cooldownBuff == null)
                {
                    return null;
                }
                return cooldownBuff.Get();
            }
        }

        public BlueprintBuff KeepBuff
        {
            get
            {
                BlueprintBuffReference keepBuff = m_KeepBuff;
                if (keepBuff == null)
                {
                    return null;
                }
                return keepBuff.Get();
            }
        }

        private bool Check1(RuleDealDamage evt)
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null) { return false; }
            if (evt.Target == null) { return false; }
            if (!evt.Target.Descriptor.HasFact(KeepBuff) && maybeCaster.Descriptor.HasFact(CooldownBuff)) { return false; }
            if (!evt.Target.Descriptor.HasFact(KeepBuff) && !maybeCaster.Descriptor.HasFact(CheckBuff)) { return false; }
            return maybeCaster.DistanceTo(Owner) <= 30.Feet().Meters && maybeCaster.Descriptor.State.CanAct && !maybeCaster.Descriptor.State.HasCondition(UnitCondition.Confusion);
        }

        void IRulebookHandler<RuleDealDamage>.OnEventAboutToTrigger(RuleDealDamage evt)
        {
            UnitEntityData maybeCaster = Buff.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                return;
            }
            if (evt.Target == null) { return; }
            if (Check1(evt))
            {
                try
                {
                    evt.RedirectionTarget = maybeCaster;
                    evt.RedirectedPercent = 50;
                    if (!maybeCaster.Descriptor.HasFact(CooldownBuff))
                    {
                        GameHelper.ApplyBuff(maybeCaster, CooldownBuff, new Rounds?(1.Rounds()));
                        GameHelper.ApplyBuff(evt.Target, KeepBuff, new Rounds?(1.Rounds()));
                    }
                }
                catch (Exception ex) { Logger.Error("Failed to negate damage sacrifice.", ex); }
            }
        }



        void IRulebookHandler<RuleDealDamage>.OnEventDidTrigger(RuleDealDamage evt)
        {

        }

        [SerializeField]
        [FormerlySerializedAs("CheckBuff")]
        public BlueprintBuffReference m_CheckBuff;

        [SerializeField]
        [FormerlySerializedAs("CooldownBuff")]
        public BlueprintBuffReference m_CooldownBuff;

        [SerializeField]
        [FormerlySerializedAs("KeepBuff")]
        public BlueprintBuffReference m_KeepBuff;
    }
}
