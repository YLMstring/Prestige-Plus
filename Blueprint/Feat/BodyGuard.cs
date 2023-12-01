using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Feat
{
    internal class BodyGuard
    {
        private static readonly string FeatName = "FeatBodyGuard";
        public static readonly string FeatGuid = "{E1C5FEBC-7D2C-445D-A452-8A77C340E14F}";

        private static readonly string DisplayName = "FeatBodyGuard.Name";
        private static readonly string Description = "FeatBodyGuard.Description";

        public static void Configure()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianBodyguardAbility.Reference.Get().Icon;

            FeatureConfigurator.New(FeatName, FeatGuid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName)
                    .SetDescription(Description)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatureRefs.CombatReflexes.ToString())
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianBodyguardAbility.ToString() })
                    .AddAbilityResources(resource: AbilityResourceRefs.DivineGuardianTrothResource.ToString(), restoreAmount: true)
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private static readonly string Feat2Name = "FeatHarmWay";
        public static readonly string Feat2Guid = "{F0611E7B-A68C-49E4-8BEC-97A1173AD609}";

        private static readonly string DisplayName2 = "FeatHarmWay.Name";
        private static readonly string Description2 = "FeatHarmWay.Description";

        public static void Configure2()
        {
            var icon = ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.Reference.Get().Icon;

            FeatureConfigurator.New(Feat2Name, Feat2Guid, Kingmaker.Blueprints.Classes.FeatureGroup.Feat)
                    .SetDisplayName(DisplayName2)
                    .SetDescription(Description2)
                    .SetIcon(icon)
                    .AddPrerequisiteFeature(FeatGuid)
                    .AddFacts(new() { FeatureRefs.MythicIgnoreAlignmentRestrictions.ToString(), AbilityRefs.DivineGuardianTrothAbility.ToString(), ActivatableAbilityRefs.DivineGuardianInHarmsWayAbility.ToString() })
                    .AddToGroups(Kingmaker.Blueprints.Classes.FeatureGroup.CombatFeat)
                    .Configure();
        }

        private const string NinjaTrickFeat = "NinjaTrick.NinjaTrick";
        public static readonly string NinjaTrickGuid = "{4C242C11-9754-47BF-BD40-5C10489999A3}";

        internal const string NinjaTrickDisplayName = "NinjaTrick.Name";
        private const string NinjaTrickDescription = "NinjaTrick.Description";
        public static void NinjaTrickConfigure()
        {
            var icon = AbilityRefs.TrickeryBlessingMajorAbility.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(NinjaTrickFeat, NinjaTrickGuid)
              .SetDisplayName(NinjaTrickDisplayName)
              .SetDescription(NinjaTrickDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(UnarmedCombatFeat())
              .AddToAllFeatures(ArrowSnatchFeat())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.RogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DarkLurkerRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.ExtraRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.LoremasterRogueTalentSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string UnarmedCombat = "NinjaTrick.UnarmedCombat";
        public static readonly string UnarmedCombatGuid = "{5BFCCF83-947B-4F32-8BFC-C304A8591594}";

        internal const string UnarmedCombatDisplayName = "NinjaTrickUnarmedCombat.Name";
        private const string UnarmedCombatDescription = "NinjaTrickUnarmedCombat.Description";

        public static BlueprintFeature UnarmedCombatFeat()
        {
            var icon = FeatureRefs.ImprovedUnarmedStrike.Reference.Get().Icon;

            return FeatureConfigurator.New(UnarmedCombat, UnarmedCombatGuid)
              .SetDisplayName(UnarmedCombatDisplayName)
              .SetDescription(UnarmedCombatDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.ImprovedUnarmedStrike.ToString() })
              .Configure();
        }

        private const string ArrowSnatch = "NinjaTrick.ArrowSnatch";
        public static readonly string ArrowSnatchGuid = "{222D49BE-3885-41BF-8A34-EB2ED46950FB}";

        internal const string ArrowSnatchDisplayName = "NinjaTrickArrowSnatch.Name";
        private const string ArrowSnatchDescription = "NinjaTrickArrowSnatch.Description";

        public static BlueprintFeature ArrowSnatchFeat()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            return FeatureConfigurator.New(ArrowSnatch, ArrowSnatchGuid)
              .SetDisplayName(ArrowSnatchDisplayName)
              .SetDescription(ArrowSnatchDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddFacts(new() { FeatureRefs.DeflectArrows.ToString() })
              .Configure();
        }
    }
}
