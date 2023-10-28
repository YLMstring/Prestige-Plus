using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Designers.Mechanics.Facts;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class SpireDefender
    {
        private const string ArchetypeName = "SpireDefender";
        private static readonly string ArchetypeGuid = "{2A2C6545-B3E5-4392-92BD-5A04DF4FF1FF}";
        internal const string ArchetypeDisplayName = "SpireDefender.Name";
        private const string ArchetypeDescription = "SpireDefender.Description";

        private static readonly LogWrapper Logger = LogWrapper.Get("PrestigePlus");
        //[HarmonyBefore(new string[] { "TabletopTweaks-Base" })]
        public static void Configure()
        {
            CreateManeuverMastery();
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MagusClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.MagusProficiencies.ToString())
            .SetRemoveFeaturesEntry(4, FeatureRefs.MagusSpellRecallFeature.ToString())
            .AddToAddFeatures(1, CreateProficiencies(), FeatureRefs.CombatExpertiseFeature.ToString(), FeatureRefs.Dodge.ToString())
            .AddToAddFeatures(4, ArcaneAugmentationFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.MagusProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] {  })
                .Configure();
        }

        private const string Proficiencies = "SpireDefender.Proficiencies";
        private static readonly string ProficienciesGuid = "{BFF59BB4-ECED-4109-9D37-775BC75B593F}";
        internal const string ProficienciesDisplayName = "SpireDefenderProficiencies.Name";
        private const string ProficienciesDescription = "SpireDefenderProficiencies.Description";

        public static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
                .CopyFrom(
                FeatureRefs.MagusProficiencies,
                typeof(ArcaneArmorProficiency))
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.Rapier,
              WeaponCategory.Longsword,
              WeaponCategory.Scimitar,
              WeaponCategory.Kukri,
              WeaponCategory.Sai,
              WeaponCategory.Nunchaku,
                  })
              .Configure();
        }

        private const string ArcaneAugmentation = "SpireDefender.ArcaneAugmentation";
        public static readonly string ArcaneAugmentationGuid = "{BD164797-FC66-4751-B3A6-446413BA9568}";

        private const string ArcaneAugmentationAblity = "SpireDefender.UseArcaneAugmentation";
        private static readonly string ArcaneAugmentationAblityGuid = "{E5B7EDB7-3001-4EB1-A50D-E61B79036A70}";

        private const string ArcaneAugmentationBuff2 = "SpireDefender.ArcaneAugmentationBuff2";
        private static readonly string ArcaneAugmentationBuff2Guid = "{E39EDFE6-D74D-4A96-B818-0225BD71BC31}";

        internal const string ArcaneAugmentationDisplayName = "SpireDefenderArcaneAugmentation.Name";
        private const string ArcaneAugmentationDescription = "SpireDefenderArcaneAugmentation.Description";

        public static BlueprintFeature ArcaneAugmentationFeat()
        {
            var icon = AbilityRefs.Transformation.Reference.Get().Icon;

            var Buff2 = BuffConfigurator.New(ArcaneAugmentationBuff2, ArcaneAugmentationBuff2Guid)
             .SetDisplayName(ArcaneAugmentationDisplayName)
             .SetDescription(ArcaneAugmentationDescription)
             .SetIcon(icon)
             .AddContextStatBonus(StatType.SkillAthletics, 5, ModifierDescriptor.Competence)
             .AddContextStatBonus(StatType.SkillMobility, 5, ModifierDescriptor.Competence)
             .AddContextStatBonus(StatType.SkillAthletics, ContextValues.Rank(), ModifierDescriptor.Enhancement)
             .AddContextStatBonus(StatType.SkillMobility, ContextValues.Rank(), ModifierDescriptor.Enhancement)
             .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.MagusClass.ToString() }).WithStartPlusDivStepProgression(3, 7, true))
             .Configure();

            var ability = AbilityConfigurator.New(ArcaneAugmentationAblity, ArcaneAugmentationAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(ArcaneAugmentationDisplayName)
                .SetDescription(ArcaneAugmentationDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString())
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(ArcaneAugmentation, ArcaneAugmentationGuid)
              .SetDisplayName(ArcaneAugmentationDisplayName)
              .SetDescription(ArcaneAugmentationDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string ManeuverMastery = "SpireDefender.ManeuverMastery";
        private static readonly string ManeuverMasteryGuid = "{3561921A-F5E9-46CE-B41C-306A8D23C6B9}";

        internal const string ManeuverMasteryDisplayName = "SpireDefenderManeuverMastery.Name";
        private const string ManeuverMasteryDescription = "SpireDefenderManeuverMastery.Description";

        private static BlueprintFeature CreateManeuverMastery()
        {
            var icon = FeatureRefs.MagusSpellRecallFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(ManeuverMastery, ManeuverMasteryGuid, FeatureGroup.MagusArcana)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .AddCMBBonus(value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.MagusClass.ToString() }).WithStartPlusDivStepProgression(4, 1, true))
              .Configure();
        }
    }
}
