using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes;
using System.Security.Cryptography;
using Kingmaker.Blueprints.Items.Weapons;

namespace PrestigePlus.CustomComponent
{
    internal class _20RMBfix : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, IUnitSubscriber, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(Buffnum(), base.Runtime));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
        }
        public void OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            if (evt.Weapon.Blueprint.IsMelee)
            {
                evt.AddDamageModifier(Buffnum(), base.Fact, ModifierDescriptor.UntypedStackable);
            }
        }

        // Token: 0x0600E99A RID: 59802 RVA: 0x003BDC0F File Offset: 0x003BBE0F
        public void OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
        }

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            if (evt.Weapon?.Blueprint?.IsMelee == true)
            {
                evt.AddModifier(Buffnum(), base.Fact, ModifierDescriptor.UntypedStackable);
            }
        }

        // Token: 0x0600E945 RID: 59717 RVA: 0x003BCB81 File Offset: 0x003BAD81
        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public int Buffnum()
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster?.HasFact(feat) == true)
            {
                if (caster.Progression.MythicLevel == 10)
                {
                    return 4;
                }
                else if(caster.Progression.MythicLevel >= 7)
                {
                    return 3;
                }
                else if (caster.Progression.MythicLevel >= 4)
                {
                    return 2;
                }
                else { return 1; }
            }
            return 0; 
        }

        public BlueprintFeature feat;
    }
}
