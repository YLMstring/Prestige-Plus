using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Maneuvers;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;

namespace PrestigePlus.CustomComponent.PrestigeClass
{
    internal class ChosenAllyTarget : UnitBuffComponentDelegate, ITargetRulebookHandler<RuleCalculateAC>, IRulebookHandler<RuleCalculateAC>, ISubscriber, ITargetRulebookSubscriber, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>
    {
        void IRulebookHandler<RuleCalculateAC>.OnEventAboutToTrigger(RuleCalculateAC evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null || caster.DistanceTo(Owner) > 8.Feet().Meters) return;
            int bonus = 1;
            if (caster.Progression.GetClassLevel(guard) >= 5)
            {
                bonus = 2;
            }
            if (caster.Progression.GetClassLevel(guard) >= 10)
            {
                bonus = 3;
            }
            evt.AddModifier(bonus, base.Fact, Kingmaker.Enums.ModifierDescriptor.Dodge);
        }

        void IRulebookHandler<RuleCalculateAC>.OnEventDidTrigger(RuleCalculateAC evt)
        {

        }

        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            var caster = Buff.Context?.MaybeCaster;
            if (caster == null || caster.DistanceTo(Owner) > 8.Feet().Meters || !caster.Memory.Contains(Owner)) return;
            int bonus = 1;
            if (caster.Progression.GetClassLevel(guard) >= 5)
            {
                bonus = 2;
            }
            if (caster.Progression.GetClassLevel(guard) >= 10)
            {
                bonus = 3;
            }
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(bonus, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Dodge));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        private static readonly BlueprintCharacterClassReference guard = BlueprintTool.GetRef<BlueprintCharacterClassReference>(FuriousGuardian.ArchetypeGuid);

    }
}