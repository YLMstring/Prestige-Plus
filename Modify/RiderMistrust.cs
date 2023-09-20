using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.UnitLogic.Abilities.Blueprints;

namespace PrestigePlus.Modify
{
    internal class RiderMistrust : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleSavingThrow>.OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            if (!CheckConditionArcane() && evt.Reason.Context != null && evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.Type == AbilityType.Spell && evt.Reason.Ability.Spellbook != null && evt.Reason.Ability.Spellbook.Blueprint.IsArcane)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
            }
            if (!CheckConditionDivine() && evt.Reason.Context != null && evt.Reason.Ability != null && evt.Reason.Ability.Blueprint.Type == AbilityType.Spell && evt.Reason.Ability.Spellbook != null && !evt.Reason.Ability.Spellbook.Blueprint.IsArcane && !evt.Reason.Ability.Spellbook.Blueprint.IsAlchemist)
            {
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveWill.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveReflex.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
                evt.AddTemporaryModifier(evt.Initiator.Stats.SaveFortitude.AddModifier(2, base.Runtime, Kingmaker.Enums.ModifierDescriptor.Morale));
            }
        }

        void IRulebookHandler<RuleSavingThrow>.OnEventDidTrigger(RuleSavingThrow evt)
        {
            
        }

        public bool CheckConditionArcane()
        {
            if (Owner == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            foreach (ClassData classData in Owner.Descriptor.Progression.Classes)
            {
                BlueprintSpellbook spellbook = classData.Spellbook;
                if (spellbook != null && spellbook.IsArcane)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckConditionDivine()
        {
            if (Owner == null)
            {
                PFLog.Default.Error("Target unit is missing", Array.Empty<object>());
                return false;
            }
            using (List<ClassData>.Enumerator enumerator = Owner.Descriptor.Progression.Classes.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.CharacterClass.IsDivineCaster)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
