using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using PrestigePlus.CustomAction.OtherManeuver;
using static Pathfinding.Util.RetainedGizmos;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.CombatStyle;

namespace PrestigePlus.CustomComponent.Feat
{
    internal class CloakDaggerManeuver : UnitBuffComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (IsChosenWeapon(Owner))
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
            Buff.Remove();
        }

        public static bool IsChosenWeapon(UnitEntityData unit)
        {
            var result = unit.GetThreatHandMelee()?.Weapon?.Blueprint.IsLight;
            if (result == true && unit.HasFact(DirtyBuff))
            {
                return true;
            }
            return false;
        }

        private static BlueprintBuffReference DirtyBlind = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleBlindbuffGuid);
        private static BlueprintBuffReference DirtyEntangle = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleEntanglebuffGuid);
        private static BlueprintBuffReference DirtySicken = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.CloakDaggerStyleSickenbuffGuid);

        private static BlueprintBuffReference DirtyBuff = BlueprintTool.GetRef<BlueprintBuffReference>(CloakDaggerStyle.StylebuffGuid);
    }
}

