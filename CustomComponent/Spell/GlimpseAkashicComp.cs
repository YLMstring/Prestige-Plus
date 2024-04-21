using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.CustomComponent.Spell
{
    internal class GlimpseAkashicComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            evt.AddDamageModifier(GetBonus(), base.Fact, ModifierDescriptor.Circumstance);
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            evt.AddModifier(GetBonus(), base.Fact, ModifierDescriptor.Circumstance);
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {

        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {

        }
        public override void OnTurnOn()
        {
            int value = GetBonus();
            foreach (StatType type in StatTypeHelper.Skills)
            {
                base.Owner.Stats.GetStat<ModifiableValue>(type).AddModifier(value, base.Runtime, ModifierDescriptor.Circumstance);
            }
            foreach (StatType type in StatTypeHelper.Saves)
            {
                base.Owner.Stats.GetStat<ModifiableValue>(type).AddModifier(value, base.Runtime, ModifierDescriptor.Circumstance);
            }
        }

        // Token: 0x0600E9FF RID: 59903 RVA: 0x003BEC94 File Offset: 0x003BCE94
        public override void OnTurnOff()
        {
            foreach (StatType type in StatTypeHelper.Skills)
            {
                ModifiableValue stat = base.Owner.Stats.GetStat<ModifiableValue>(type);
                stat?.RemoveModifiersFrom(base.Runtime);
            }
            foreach (StatType type in StatTypeHelper.Saves)
            {
                ModifiableValue stat = base.Owner.Stats.GetStat<ModifiableValue>(type);
                stat?.RemoveModifiersFrom(base.Runtime);
            }
        }
        private int GetBonus()
        {
            return Buff.Context?.Params?.CasterLevel ?? 0;
        }
    }
}