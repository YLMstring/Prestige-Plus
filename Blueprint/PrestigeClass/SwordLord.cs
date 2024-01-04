using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class SwordLord
    {
        private const string QuickDraw = "SwordLord.QuickDraw";
        private static readonly string QuickDrawGuid = "{40E0F0A9-A447-4891-B888-E75CC2E5CFDD}";

        internal const string QuickDrawDisplayName = "SwordLordQuickDraw.Name";
        private const string QuickDrawDescription = "SwordLordQuickDraw.Description";
        //"QuickDraw": "b7b65e54-2278-4145-8590-31fa48b785e9",
        public static void CreateQuickDraw()
        {
            var icon = FeatureRefs.Alertness.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(QuickDraw, QuickDrawGuid)
              .SetDisplayName(QuickDrawDisplayName)
              .SetDescription(QuickDrawDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact("b7b65e54-2278-4145-8590-31fa48b785e9", FeatureRefs.DuelingMastery.ToString(), false)
              .AddFeatureIfHasFact(FeatureRefs.Alertness.ToString(), FeatureRefs.DuelingMastery.ToString(), false)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.SwordlordProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }
    }
}
