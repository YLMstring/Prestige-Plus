using BlueprintCore.Utils;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker;
using Kingmaker.UnitLogic.Mechanics.Actions;
using PrestigePlus.Grapple;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic;
using PrestigePlus.Blueprint.PrestigeClass;

namespace PrestigePlus.HarmonyFix
{
    [HarmonyPatch(typeof(DraconicBloodlineArcana), nameof(DraconicBloodlineArcana.OnEventAboutToTrigger))]
    internal class RemoveDragonArcana
    {
        static bool Prefix(ref DraconicBloodlineArcana __instance)
        {
            if (__instance.Owner.HasFact(Eso))
            {
                return false;
            }
            return true;
        }

        private static BlueprintProgressionReference Eso = BlueprintTool.GetRef<BlueprintProgressionReference>(SwordLord.VariantBloodlineGuid);
    }
}
