using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;

namespace PrestigePlus.Maneuvers
{
    [HarmonyPatch(typeof(UnitAttack), nameof(UnitAttack.TriggerAttackRule))]
    internal class ReplaceAttackFix
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus"); 
        static bool Prefix(ref UnitAttack __instance, ref AttackHandInfo attack)
        {
            try
            {
                var caster = __instance.Executor;
                var target = __instance.Target;
                if (!__instance.IsAttackFull || !attack.Weapon.Blueprint.IsMelee) { return true; }
                Logger.Info(attack.AttackBonusPenalty.ToString());
                var AttackBonusRule = new RuleCalculateAttackBonus(caster, target, caster.Body.EmptyHandWeapon, 0) { };
                int penalty = -attack.AttackBonusPenalty + DualPenalty(caster, attack);
                AttackBonusRule.AddModifier(penalty, descriptor: ModifierDescriptor.Penalty);
                if (caster.HasFact(Disarm1) || caster.HasFact(Disarm2))
                {
                    GameHelper.RemoveBuff(caster, Disarm1);
                    var threat = target.GetThreatHandMelee();
                    if (threat == null || threat.MaybeWeapon == null || threat.MaybeWeapon.Blueprint.IsNatural || threat.MaybeWeapon.Blueprint.IsUnarmed) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, CombatManeuver.Disarm, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Sunder1) || caster.HasFact(Sunder2))
                {
                    GameHelper.RemoveBuff(caster, Sunder1);
                    if (target.HasFact(BlueprintRoot.Instance.SystemMechanics.SunderArmorBuff)) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, CombatManeuver.SunderArmor, AttackBonusRule);
                    ruleCombatManeuver = (target.Context?.TriggerRule(ruleCombatManeuver)) ?? Rulebook.Trigger(ruleCombatManeuver);
                    return false;
                }
                if (caster.HasFact(Trip1) || caster.HasFact(Trip2))
                {
                    GameHelper.RemoveBuff(caster, Trip1);
                    if (!target.CanBeKnockedOff()) { return true; }
                    RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(caster, target, CombatManeuver.Trip, AttackBonusRule);
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

        public static int DualPenalty(UnitEntityData unit, AttackHandInfo attack)
        {
            ItemEntityWeapon maybeWeapon = unit.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = unit.Body.SecondaryHand.MaybeWeapon;
            bool flag2 = unit.Descriptor.State.AdditionalFeatures.ShieldMaster;
            bool flag3 = (maybeWeapon != null && maybeWeapon.IsShield) || (maybeWeapon2 != null && maybeWeapon2.IsShield);
            if (attack.Weapon == null || maybeWeapon == null || maybeWeapon2 == null || maybeWeapon.Blueprint.IsNatural || maybeWeapon2.Blueprint.IsNatural || maybeWeapon == unit.Body.EmptyHandWeapon || maybeWeapon2 == unit.Body.EmptyHandWeapon || (maybeWeapon != attack.Weapon && maybeWeapon2 != attack.Weapon) || (flag2 && flag3))
            {
                return 0;
            }
            int rank = 0;
            if (unit.HasFact(TWF)) { rank = 2; }
            int num = (rank > 1) ? (unit.HasFact(Mythic) ? 0 : -2) : -4;
            int num2 = (rank > 1) ? (unit.HasFact(Mythic) ? 0 : -2) : -8;
            int num3 = (attack.Weapon == maybeWeapon) ? num : num2;
            UnitPartWeaponTraining unitPartWeaponTraining = unit.Get<UnitPartWeaponTraining>();
            bool flag4 = unit.State.Features.EffortlessDualWielding && unitPartWeaponTraining != null && unitPartWeaponTraining.IsSuitableWeapon(maybeWeapon2);
            if (!maybeWeapon2.Blueprint.IsLight && !maybeWeapon.Blueprint.Double && !maybeWeapon2.IsShield && !flag4)
            {
                num3 -= 2;
            }
            return num3;
        }

        private static BlueprintFeatureReference TWF = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFighting.ToString());
        private static BlueprintFeatureReference Mythic = BlueprintTool.GetRef<BlueprintFeatureReference>(FeatureRefs.TwoWeaponFightingMythicFeat.ToString());
    }
}
