using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using System.Security.Cryptography;
using System.Net;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class AuthoritativeCommanComp : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, IInitiatorRulebookHandler<RuleCalculateWeaponStats>, IRulebookHandler<RuleCalculateWeaponStats>, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null) return;
            int bonus = 0;
            if (caster.Progression.GetClassLevel(guard) >= 4)
            {
                bonus = 1;
            }
            if (caster.Progression.GetClassLevel(guard) >= 6)
            {
                bonus = 2;
            }
            evt.AddModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Competence);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null) return;
            int bonus = 0;
            if (caster.Progression.GetClassLevel(guard) >= 3)
            {
                bonus = 1;
            }
            if (caster.Progression.GetClassLevel(guard) >= 6)
            {
                bonus = 2;
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(bonus, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Competence));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(bonus, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Competence));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(bonus, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Competence));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventAboutToTrigger(RuleCalculateWeaponStats evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null) return;
            int bonus = 0;
            if (caster.Progression.GetClassLevel(guard) >= 2)
            {
                bonus = 1;
            }
            if (caster.Progression.GetClassLevel(guard) >= 6)
            {
                bonus = 2;
            }
            evt.AddDamageModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Competence);
        }

        void IRulebookHandler<RuleCalculateWeaponStats>.OnEventDidTrigger(RuleCalculateWeaponStats evt)
        {
            
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null) return;
            int bonus = 1;
            if (caster.Progression.GetClassLevel(guard) >= 6)
            {
                bonus = 2;
            }
            evt.AddModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Competence);
        }

        void IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>.OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            
        }

        private static readonly BlueprintCharacterClassReference guard = BlueprintTool.GetRef<BlueprintCharacterClassReference>(GoldenLegionnaire.ArchetypeGuid);
    }
}