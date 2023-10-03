using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Parts;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers;

namespace PrestigePlus.BasePrestigeEnhance
{
    internal class SynthesisComponent : UnitBuffComponentDelegate, ISubscriber, IInitiatorRulebookSubscriber, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>
    {
        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt)
        {

        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt)
        {
            try
            {
                if (evt.Spell?.Spellbook == Owner.Ensure<UnitPartMysticTheurge>().DivineSpellbook)
                {
                    GameHelper.RemoveBuff(Owner, Divine);
                    return;
                }
                if (evt.Spell?.Spellbook == Owner.Ensure<UnitPartMysticTheurge>().ArcaneSpellbook)
                {
                    GameHelper.RemoveBuff(Owner, Arcane);
                    return;
                }
            }
            catch (Exception ex) { Logger.Error("Failed to synthesis.", ex); }
        }
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        private static BlueprintBuffReference Arcane = BlueprintTool.GetRef<BlueprintBuffReference>("{71740950-0B75-4157-9B40-85B80703AA7B}");
        private static BlueprintBuffReference Divine = BlueprintTool.GetRef<BlueprintBuffReference>("{806AC2E8-5B0E-46BE-841F-FF280D72CEFD}");
    }
}
