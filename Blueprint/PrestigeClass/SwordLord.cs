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
using PrestigePlus.Patch;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;

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
              .SetReapplyOnLevelUp()
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.SwordlordProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }

        private const string HellDomain = "SwordLord.HellDomain";
        private static readonly string HellDomainGuid = "{09698710-8D01-47A0-B86A-6A8265E13BBC}";

        private const string HellDomainfeat = "SwordLord.HellDomainfeat";
        public static readonly string HellDomainfeatGuid = "{C014978C-E88C-4302-801E-5B1C9B5ECA6C}";

        public static BlueprintProgression HellDomainFeat()
        {
            var icon = FeatureRefs.WarriorPriest.Reference.Get().Icon;

            var feat1 = FeatureConfigurator.New(HellDomainfeat, HellDomainfeatGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIsClassFeature(true)
              .SetRanks(20)
              .SetHideInUI(true)
              .Configure();

            return ProgressionConfigurator.New(HellDomain, HellDomainGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddPrerequisiteFeature(FeatureRefs.WarriorPriest.ToString())
              .AddToClasses(CharacterClassRefs.HellknightSigniferClass.ToString())
              .AddToLevelEntry(1, feat1, FeatureRefs.SelectiveChannel.ToString())
              .AddToLevelEntry(2, feat1)
                .AddToLevelEntry(3, feat1)
                .AddToLevelEntry(4, feat1)
                .AddToLevelEntry(5, feat1)
                .AddToLevelEntry(6, feat1)
                .AddToLevelEntry(7, feat1)
                .AddToLevelEntry(8, feat1)
                .AddToLevelEntry(9, feat1)
                .AddToLevelEntry(10, feat1)
              .Configure();
        }

        private const string Catechesis = "SwordLord.Catechesis";
        private static readonly string CatechesisGuid = "{A23D1F1C-2EAB-4BEC-BA7C-4B6BE9BABCD2}";

        internal const string CatechesisDisplayName = "SwordLordCatechesis.Name";
        private const string CatechesisDescription = "SwordLordCatechesis.Description";
        public static void CreateCatechesis()
        {
            var icon = FeatureRefs.WarriorPriest.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(Catechesis, CatechesisGuid)
              .SetDisplayName(CatechesisDisplayName)
              .SetDescription(CatechesisDescription)
              .SetIcon(icon)
              .AddToAllFeatures(HellDomainFeat())
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.HellknightSigniferProgression)
               .AddToLevelEntry(1, feat)
               .Configure();
        }
    }
}
