using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.Mechanics.Facts;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic;
using Kingmaker.Blueprints;
using static Kingmaker.UI.CanvasScalerWorkaround;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(WeaponParametersDamageBonus), nameof(WeaponParametersDamageBonus.CalculatePowerAttackScaling))]
    internal class DFSharpFang
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        static bool Prefix(ref WeaponParametersDamageBonus __instance, ref int damageBonus, RuleCalculateWeaponStats evt)
        {
            try
            {
                int damage = damageBonus;
                if (!__instance.PowerAttackScaling) { return true; }
                var secondaryHand = __instance.Owner.Body.CurrentHandsEquipmentSet.SecondaryHand;
                //fix secondary weapon damage not half bug
                if (secondaryHand.HasWeapon && secondaryHand.MaybeWeapon != __instance.Owner.Body.EmptyHandWeapon && secondaryHand.MaybeWeapon == evt.Weapon)
                {
                    damage /= 2;
                    if (__instance.Owner.HasFact(TWF))
                    {
                        damage *= 2;
                    }
                }
                if (__instance.Owner.HasFact(TWF2) && !evt.Weapon.HoldInTwoHands)
                {
                    if (evt.Initiator.Descriptor.HasFact(__instance.GreaterPowerAttackBlueprint))
                    {
                        damage += damage;
                    }
                    else if (!evt.Initiator.HasFact(__instance.MythicBlueprint))
                    {
                        damage += damage / 2;
                    }
                    else
                    {
                        damage += damage * 2 / 3;
                    }
                }
                damageBonus = damage;
                return true;
            }
            catch (Exception ex) { Logger.Error("Failed to DFSharpFang.", ex); return true; }
        }

        private static BlueprintFeatureReference TWF = BlueprintTool.GetRef<BlueprintFeatureReference>(DragonFury.SharpFangGuid);
        private static BlueprintFeatureReference TWF2 = BlueprintTool.GetRef<BlueprintFeatureReference>(DragonFury.ViciousFangGuid);
    }
}
