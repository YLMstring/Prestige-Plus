using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(ForbidSpecificSpellsCast), nameof(ForbidSpecificSpellsCast.OnEventAboutToTrigger))]
    internal class TeleportMasterFix
    {
        static bool Prefix(ref ForbidSpecificSpellsCast __instance)
        {
            if (!ModMenu.ModMenu.GetSettingValue<bool>(Main.GetKey("telemaster"))) { return true; }
            if (__instance.Owner.HasFact(FeatureRefs.DimensionalRetribution.Reference) && __instance.Spells.Contains(AbilityRefs.KiAbudantStep.Reference.Get()))
            {
                return false;
            }
            return true;
        }
    }
}
