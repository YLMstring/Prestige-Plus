using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(RuleCalculateWeaponStats), nameof(RuleCalculateWeaponStats.OnTrigger))]
    internal class DCFlameBladeFix
    {
        //FlameBladeDervishCombat	4df40f35e124455c9e20dd44e7cacf12
        //FlameBladeWeapon	37dcc61efb354d42a37be7960faab94f
        //DivineTridentWeapon	99ad4bfcd19349f88a2042eb75182830
        private static BlueprintFeatureReference Dervish = BlueprintTool.GetRef<BlueprintFeatureReference>("4df40f35e124455c9e20dd44e7cacf12");
        private static BlueprintItemWeaponReference Flame1 = BlueprintTool.GetRef<BlueprintItemWeaponReference>("37dcc61efb354d42a37be7960faab94f");
        private static BlueprintItemWeaponReference Flame2 = BlueprintTool.GetRef<BlueprintItemWeaponReference>("99ad4bfcd19349f88a2042eb75182830");

        static void Prefix(ref RuleCalculateWeaponStats __instance)
        {
            try
            {
                if (!__instance.Initiator.HasFact(Dervish)) { return; }
                if (__instance.Weapon.Blueprint == Flame1.Get() || __instance.Weapon.Blueprint == Flame2.Get())
                {
                    __instance.DamageBonusStat = null;
                    if (__instance.Initiator.Stats.Charisma > 10)
                    {
                        __instance.DamageBonusStat = Kingmaker.EntitySystem.Stats.StatType.Charisma;
                    }
                }
            }
            catch (Exception ex) { Main.Logger.Error("Failed to DCFlameBladeFix.", ex); }
        }

    }
}
