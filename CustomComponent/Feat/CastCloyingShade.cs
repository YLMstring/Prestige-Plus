using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Blueprint.RogueTalent;
using PrestigePlus.GrappleMechanic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class CastCloyingShade : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCastSpell>, IRulebookHandler<RuleCastSpell>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleCastSpell>.OnEventAboutToTrigger(RuleCastSpell evt)
        {
            var SpellBlue = evt.Spell?.Blueprint;
            bool iscloy = false;
            if (SpellBlue == AbilityRefs.KiAbudantStep.Reference.Get() || SpellBlue == AbilityRefs.ScaledFistAbudantStep.Reference.Get() || SpellBlue == AbilityRefs.DimensionDoor.Reference.Get() || SpellBlue == AbilityRefs.DimensionDoorCasterOnly.Reference.Get())
            {
                iscloy = true;
            }
            if (Display.Get() && SpellBlue == Display.Get())
            {
                iscloy = true;
            }
            if (Display1.Get() && SpellBlue == Display1.Get())
            {
                iscloy = true;
            }
            if (Display2.Get() && SpellBlue == Display2.Get())
            {
                iscloy = true;
            }
            if (Display3.Get() && SpellBlue == Display3.Get())
            {
                iscloy = true;
            }
            if (Display4.Get() && SpellBlue == Display4.Get())
            {
                iscloy = true;
            }
            if (Display5.Get() && SpellBlue == Display5.Get())
            {
                iscloy = true;
            }
            if (Display6.Get() && SpellBlue == Display6.Get())
            {
                iscloy = true;
            }
            if (Display7.Get() && SpellBlue == Display7.Get())
            {
                iscloy = true;
            }
            if (Display8.Get() && SpellBlue == Display8.Get())
            {
                iscloy = true;
            }
            if (Display9.Get() && SpellBlue == Display9.Get())
            {
                iscloy = true;
            }
            if (iscloy)
            {
                Fact.RunActionInContext(Actions);
                Owner.Remove<UnitPartKiThrow>();
                Owner.Ensure<UnitPartKiThrow>().Init(Owner, CasterBuff, Context);
            }
        }

        void IRulebookHandler<RuleCastSpell>.OnEventDidTrigger(RuleCastSpell evt)
        {
            
        }

        private static BlueprintAbilityReference Display = BlueprintTool.GetRef<BlueprintAbilityReference>(ShadowDancer.ShadowJumpAblityGuid);
        private static BlueprintAbilityReference Display1 = BlueprintTool.GetRef<BlueprintAbilityReference>("db81aed3-aa80-4725-868a-df54594104ba");
        private static BlueprintAbilityReference Display2 = BlueprintTool.GetRef<BlueprintAbilityReference>("94f80bb6-8519-460e-a1a4-60d4d75f19a9");
        private static BlueprintAbilityReference Display3 = BlueprintTool.GetRef<BlueprintAbilityReference>("2df92cb1-ee39-455c-b39b-07d067821b9c");
        private static BlueprintAbilityReference Display4 = BlueprintTool.GetRef<BlueprintAbilityReference>("9061c630-313e-4399-ae06-e4bd0b389c88");
        private static BlueprintAbilityReference Display5 = BlueprintTool.GetRef<BlueprintAbilityReference>("7bd1b3f5-a58e-47df-8069-cfa76a84d820");
        private static BlueprintAbilityReference Display6 = BlueprintTool.GetRef<BlueprintAbilityReference>("587f8c97-d5ba-4f69-a0dc-38dc8fb9f8a8");
        private static BlueprintAbilityReference Display7 = BlueprintTool.GetRef<BlueprintAbilityReference>("ff24e8b8-3a73-47cd-aa52-c956251c6129");
        private static BlueprintAbilityReference Display8 = BlueprintTool.GetRef<BlueprintAbilityReference>("80920565-edff-4e87-b060-1151dffd0fe2");
        private static BlueprintAbilityReference Display9 = BlueprintTool.GetRef<BlueprintAbilityReference>("aac36b12-c3d0-4a4c-84f2-4c014a09b858");

        private static BlueprintBuffReference CasterBuff = BlueprintTool.GetRef<BlueprintBuffReference>(CloyingShades.CloyingShadesBuffGuid);

        private static ActionList Actions = ActionsBuilder.New()
            .CastSpell(CloyingShades.CloyingShadesAbilityGuid)
            //.ApplyBuffWithDurationSeconds(CloyingShades.CloyingShadesBuffGuid, (float)0.5, toCaster: true)
            .Build();
    }
}
