using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Components;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Modify
{
    internal class SolarInvocationComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>
    {
        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            if (!evt.Target.Descriptor.Alignment.ValueRaw.HasComponent(AlignmentComponent.Evil)) return;
            int bonus = GetBonus();
            evt.AddModifier(bonus, base.Fact, ModifierDescriptor.Competence);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.Target.Descriptor.Alignment.ValueRaw.HasComponent(AlignmentComponent.Evil)) return;
            int bonus = GetBonus();
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus, base.Runtime, ModifierDescriptor.Competence));
        }

        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventDidTrigger(RuleCalculateAttackBonus evt)
        {

        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }

        private int GetBonus()
        {
            var caster = Buff?.Context?.MaybeCaster;
            if (caster == null) return 0;
            int level = caster.Progression.GetClassLevel(BlueprintTool.GetRef<BlueprintCharacterClassReference>(AnchoriteofDawn.ArchetypeGuid));
            int num = 0;
            if (caster.HasFact(BlueprintTool.GetRef<BlueprintBuffReference>(AnchoriteofDawn.SolarBuffGuid4))) num += 1;
            if (level >= 9) num += 3;
            else if (level >= 5) num += 2;
            else if (level >= 1) num += 1;
            return num;
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            var caster = Buff?.Context?.MaybeCaster;
            if (caster == null || !caster.HasFact(Def1) || (!caster.HasFact(Def2) && caster != Owner)) return;
            int bonus = GetBonus();
            evt.AddModifier(bonus, base.Fact, ModifierDescriptor.Sacred);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {
            
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var caster = Buff?.Context?.MaybeCaster;
            if (caster == null || !caster.HasFact(Def1) || (!caster.HasFact(Def2) && caster != Owner)) return;
            int bonus = GetBonus();
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(bonus, base.Runtime, ModifierDescriptor.Dodge));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        private static BlueprintFeatureReference Def1 = BlueprintTool.GetRef<BlueprintFeatureReference>(AnchoriteofDawn.SolarDefense1Guid);
        private static BlueprintFeatureReference Def2 = BlueprintTool.GetRef<BlueprintFeatureReference>(AnchoriteofDawn.SolarDefense2Guid);
    }
}
