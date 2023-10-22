using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using PrestigePlus.PrestigeClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchHolyVindicator
    {
        public static void Patch()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

            ProgressionConfigurator.For("7162d325539a4c66a163fe49c42f279a")
                .RemoveFromLevelEntries(2, "7d94808783e742e8bb8dc4d82e30ca0e")
                .AddToLevelEntries(1, spellupgradeGuid)
                .AddToLevelEntries(3, HolyVindicator.DivineWrathFeat())
                .AddToLevelEntries(3, HolyVindicator.DivineJudgmentFeat())
                .AddToLevelEntries(3, HolyVindicator.DivineRetributionFeat())
                .Configure(delayed: true);
        }
    }
}
