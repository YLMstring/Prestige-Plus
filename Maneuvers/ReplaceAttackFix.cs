using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Maneuvers
{
    [HarmonyPatch(typeof(RuleAttackWithWeapon), nameof(RuleAttackWithWeapon.OnTrigger))]
    internal class ReplaceAttackFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus"); 
        static bool Prefix(ref RuleAttackWithWeapon __instance)
        {
            try
            {
                var caster = __instance.Initiator;
                var target = __instance.Target;
                if (!__instance.IsFullAttack || !__instance.Weapon.Blueprint.IsMelee) { return true; }
                if (caster.HasFact(Disarm1) || caster.HasFact(Disarm2))
                {
                    GameHelper.RemoveBuff(caster, Disarm1);
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.DisarmMainHandBuff)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(target, caster, CombatManeuver.Disarm, null);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Sunder1) || caster.HasFact(Sunder2))
                {
                    GameHelper.RemoveBuff(caster, Sunder1);
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(target, caster, CombatManeuver.SunderArmor, null);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Trip1) || caster.HasFact(Trip2))
                {
                    GameHelper.RemoveBuff(caster, Trip1);
                    if (target.Descriptor.State.Prone.Active) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(target, caster, CombatManeuver.Trip, null);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                return true;
            }
            catch (Exception ex) { Logger.Error("Failed to replace attack.", ex); return true; }
        }

        private static BlueprintBuffReference Disarm1 = BlueprintTool.GetRef<BlueprintBuffReference>("{59ED0B02-A211-4323-9717-CBE6A1CD6846}");
        private static BlueprintBuffReference Disarm2 = BlueprintTool.GetRef<BlueprintBuffReference>("{49890773-D237-46D0-8C68-B666B0503523}");

        private static BlueprintBuffReference Sunder1 = BlueprintTool.GetRef<BlueprintBuffReference>("{B5FD1419-CED2-4450-9110-C2620CD88AC7}");
        private static BlueprintBuffReference Sunder2 = BlueprintTool.GetRef<BlueprintBuffReference>("{57476F97-D68A-4707-9B12-B2D7C5687F39}");

        private static BlueprintBuffReference Trip1 = BlueprintTool.GetRef<BlueprintBuffReference>("{8C577D9F-BA5B-4974-B91D-F94FF28A8501}");
        private static BlueprintBuffReference Trip2 = BlueprintTool.GetRef<BlueprintBuffReference>("{43ADBC25-7972-45D6-A3AB-356F16199D50}");
    }
}
