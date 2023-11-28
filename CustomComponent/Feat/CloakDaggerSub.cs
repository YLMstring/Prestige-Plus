using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using PrestigePlus.Blueprint.CombatStyle;
using PrestigePlus.CustomAction.OtherManeuver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class CloakDaggerSub : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (evt.IsAttackOfOpportunity && CloakDaggerManeuver.IsChosenWeapon(Owner))
            {
                var maneuver = CombatManeuver.None;
                var caster = Owner;
                if (caster.HasFact(DirtyBlind))
                {
                    maneuver = CombatManeuver.DirtyTrickBlind;
                }
                else if (caster.HasFact(DirtyEntangle))
                {
                    maneuver = CombatManeuver.DirtyTrickEntangle;
                }
                else if (caster.HasFact(DirtySicken))
                {
                    maneuver = CombatManeuver.DirtyTrickSickened;
                }
                SweepManeuver.ActManeuver(caster, evt.Target, 0, maneuver);
            }
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {

        }

        private static BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleBlindbuffGuid);
        private static BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleEntanglebuffGuid);
        private static BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleSickenbuffGuid);
    }
}

