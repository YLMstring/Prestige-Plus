using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.PrestigeClass;
using PrestigePlus.Modify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Patch
{
    internal class PatchHolyVindicator
    {
        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        public static void Patch()
        {
            var progress = BlueprintTool.GetRef<BlueprintProgressionReference>("7162d325539a4c66a163fe49c42f279a")?.Get();
            if (progress == null) { Logger.Info("not found hv");  return; }
            var Spell = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(SpellbookReplace.spellupgradeGuid);
            var old = BlueprintTool.GetRef<BlueprintFeatureBaseReference>("7d94808783e742e8bb8dc4d82e30ca0e");
            var feat1 = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(HolyVindicator.DivineWrathGuid);
            var feat2 = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(HolyVindicator.DivineJudgmentGuid);
            var feat3 = BlueprintTool.GetRef<BlueprintFeatureBaseReference>(HolyVindicator.DivineRetributionGuid);

            progress.LevelEntries
                        .Where(entry => entry.Level == 1)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(Spell);
                        });

            progress.LevelEntries
                        .Where(entry => entry.Level == 2)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Remove(old);
                        });

            progress.LevelEntries
                        .Where(entry => entry.Level == 4)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(feat1);
                        });

            progress.LevelEntries
                        .Where(entry => entry.Level == 7)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(feat2);
                        });

            progress.LevelEntries
                        .Where(entry => entry.Level == 10)
                        .ForEach(entry =>
                        {
                            entry.m_Features.Add(feat3);
                        });
        }
    }
}
