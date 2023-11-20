using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using Kingmaker.UnitLogic;
using BlueprintCore.Blueprints.References;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem;

namespace PrestigePlus.CustomComponent.Archetype
{
    internal class DefendChargeComp : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ITargetRulebookSubscriber, IConcentrationBonusProvider
    {
        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            evt.AddBonusConcentration(GetBonus());
        }

        void IRulebookHandler<RuleCalculateAbilityParams>.OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {

        }

        private int GetBonus()
        {
            if (Owner.Spellbooks?.Any() == true) 
            {
                var caster = Buff?.Context?.MaybeCaster;
                if (caster == null) return 0;
                int level = caster.Progression.GetClassLevel(CharacterClassRefs.RangerClass.Reference);
                int num = 2;
                if (level >= 20) num += 8;
                else if (level >= 15) num += 6;
                else if (level >= 10) num += 4;
                else if (level >= 5) num += 2;
                return num;
            }
            return 0;
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            evt.AddModifier(GetBonus(), Fact, ModifierDescriptor.Dodge);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        int IConcentrationBonusProvider.GetStaticConcentrationBonus(EntityFactComponent runtime)
        {
            int staticConcentrationBonus;
            using (runtime.RequestEventContext())
            {
                staticConcentrationBonus = GetBonus();
            }
            return staticConcentrationBonus;
        }
    }
}

