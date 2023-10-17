using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;

namespace PrestigePlus.Modify
{
    internal class DawnRagingSongComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonus>, IRulebookHandler<RuleCalculateAttackBonus>, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>
    {
        void IRulebookHandler<RuleCalculateAttackBonus>.OnEventAboutToTrigger(RuleCalculateAttackBonus evt)
        {
            var type = evt.Weapon?.Blueprint?.AttackType;
            if (type != Kingmaker.RuleSystem.AttackType.Melee) return;
            int bonus = GetBonus2();
            evt.AddModifier(bonus, base.Fact, ModifierDescriptor.Rage);
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            var type = evt.Weapon?.Blueprint?.AttackType;
            if (type != Kingmaker.RuleSystem.AttackType.Melee) return;
            int bonus = GetBonus2();
            evt.AddTemporaryModifier(evt.Initiator.Stats.AdditionalDamage.AddModifier(bonus, base.Runtime, ModifierDescriptor.Rage));
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
            var progression = caster.Progression.GetProgression(BlueprintTool.GetRef<BlueprintProgressionReference>(AnchoriteofDawn.FocusedRagingSongGuid));
            if (progression == null) return 0;
            int level = progression.Level;
            int num = 0;
            if (level >= 20) num += 6;
            else if (level >= 16) num += 5;
            else if (level >= 12) num += 4;
            else if (level >= 8) num += 3;
            else if (level >= 4) num += 2;
            return num;
        }

        private int GetBonus2()
        {
            var caster = Buff?.Context?.MaybeCaster;
            if (caster == null) return 0;
            var progression = caster.Progression.GetProgression(BlueprintTool.GetRef<BlueprintProgressionReference>(AnchoriteofDawn.FocusedRagingSongGuid));
            if (progression == null) return 0;
            int level = progression.Level;
            int num = 0;
            if (level >= 16) num += 3;
            else if (level >= 12) num += 2;
            return num;
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            int bonus = GetBonus();
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(bonus, base.Runtime, ModifierDescriptor.Rage));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }
    }
}