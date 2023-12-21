using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.GrappleFeat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Kidnapper
    {
        private const string ArchetypeName = "Kidnapper";
        private static readonly string ArchetypeGuid = "{AB447984-6208-4979-8535-DB6F1A70A778}";
        internal const string ArchetypeDisplayName = "Kidnapper.Name";
        private const string ArchetypeDescription = "Kidnapper.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.RogueClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureRefs.Trapfinding.ToString())
            .AddToRemoveFeatures(3, FeatureRefs.DangerSenseRogue.ToString())
            .AddToRemoveFeatures(6, FeatureRefs.DangerSenseRogue.ToString())
            .AddToRemoveFeatures(9, FeatureRefs.DangerSenseRogue.ToString())
            .AddToRemoveFeatures(12, FeatureRefs.DangerSenseRogue.ToString())
            .AddToRemoveFeatures(15, FeatureRefs.DangerSenseRogue.ToString())
            .AddToRemoveFeatures(18, FeatureRefs.DangerSenseRogue.ToString())
            .AddToAddFeatures(1, CreateCleanCapture())
            .AddToAddFeatures(3, CreateAbductor())
              .Configure();
        }

        private const string CleanCapture = "Kidnapper.CleanCapture";
        public static readonly string CleanCaptureGuid = "{72473103-E23F-4C87-839A-37490701F9AC}";

        internal const string CleanCaptureDisplayName = "KidnapperCleanCapture.Name";
        private const string CleanCaptureDescription = "KidnapperCleanCapture.Description";
        private static BlueprintFeature CreateCleanCapture()
        {
            var icon = AbilityRefs.Sleep.Reference.Get().Icon;

            return FeatureConfigurator.New(CleanCapture, CleanCaptureGuid)
              .SetDisplayName(CleanCaptureDisplayName)
              .SetDescription(CleanCaptureDescription)
              .SetIcon(icon)
              .AddToIsPrerequisiteFor(CreateCleanCapture1())
              .AddToIsPrerequisiteFor(CreateCleanCapture2())
              .Configure();
        }

        private const string CleanCapture1 = "Kidnapper.CleanCapture1";
        private static readonly string CleanCapture1Guid = "{538EF175-6943-4785-9F6C-25F29FF3053E}";

        internal const string CleanCapture1DisplayName = "KidnapperCleanCapture1.Name";
        private const string CleanCapture1Description = "KidnapperCleanCapture1.Description";
        private static BlueprintFeature CreateCleanCapture1()
        {
            var icon = AbilityRefs.Sleep.Reference.Get().Icon;

            return FeatureConfigurator.New(CleanCapture1, CleanCapture1Guid, FeatureGroup.RogueTalent)
              .SetDisplayName(CleanCapture1DisplayName)
              .SetDescription(CleanCapture1Description)
              .SetIcon(icon)
              .AddPrerequisiteFeature(CleanCaptureGuid)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteNoFeature(ImprovedGrapple.StyleGuid)
              .AddFacts(new() { ImprovedGrapple.StyleGuid })
              .Configure();
        }

        private const string CleanCapture2 = "Kidnapper.CleanCapture2";
        private static readonly string CleanCapture2Guid = "{56B29EEC-07C5-4DC6-821C-0578D957C6EF}";

        internal const string CleanCapture2DisplayName = "KidnapperCleanCapture2.Name";
        private const string CleanCapture2Description = "KidnapperCleanCapture2.Description";
        private static BlueprintFeature CreateCleanCapture2()
        {
            var icon = AbilityRefs.DeepSlumber.Reference.Get().Icon;

            return FeatureConfigurator.New(CleanCapture2, CleanCapture2Guid, FeatureGroup.RogueTalent)
              .SetDisplayName(CleanCapture2DisplayName)
              .SetDescription(CleanCapture2Description)
              .SetIcon(icon)
              .AddPrerequisiteFeature(CleanCaptureGuid)
              .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 6)
              .AddPrerequisiteStatValue(StatType.Dexterity, 13)
              .AddPrerequisiteFeature(ImprovedGrapple.StyleGuid)
              .AddPrerequisiteNoFeature(GreaterGrapple.FeatGuid)
              .AddFacts(new() { GreaterGrapple.FeatGuid })
              .Configure();
        }

        private const string Abductor = "Kidnapper.Abductor";
        private static readonly string AbductorGuid = "{A5E8BF66-C4D7-4AE5-9ED4-43577928401E}";

        internal const string AbductorDisplayName = "KidnapperAbductor.Name";
        private const string AbductorDescription = "KidnapperAbductor.Description";
        private static BlueprintFeature CreateAbductor()
        {
            var icon = FeatureRefs.FastStealth.Reference.Get().Icon;

            return FeatureConfigurator.New(Abductor, AbductorGuid)
              .SetDisplayName(AbductorDisplayName)
              .SetDescription(AbductorDescription)
              .SetIcon(icon)
              .AddCMBBonusForManeuver(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Rank())
              .AddCMDBonusAgainstManeuvers(maneuvers: new[] { Kingmaker.RuleSystem.Rules.CombatManeuver.Grapple }, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] {CharacterClassRefs.RogueClass.ToString()}).WithStartPlusDivStepProgression(3, 3, true))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }
    }
}
