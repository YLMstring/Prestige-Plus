using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using BlueprintCore.Blueprints.References;

namespace PrestigePlus.Modify
{
    internal class DivineDefianceBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (!CheckConditionDivine(evt.Reason?.Caster)) { return; }
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.UntypedStackable));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.UntypedStackable));
            evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.UntypedStackable));
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {

        }

        public static bool CheckConditionDivine(UnitEntityData unit)
        {
            if (unit == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            if (unit.Progression.MythicLevel > 0)
            {
                return true;
            }
            using (List<ClassData>.Enumerator enumerator = unit.Descriptor.Progression.Classes.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.CharacterClass.IsDivineCaster)
                    {
                        return true;
                    }
                    if (enumerator.Current.CharacterClass == CharacterClassRefs.MonsterMythicClass.Reference.Get())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
