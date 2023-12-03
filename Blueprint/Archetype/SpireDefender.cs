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
using Kingmaker.Blueprints.Items.Armors;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.Archetype;

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
            .SetRemoveFeaturesEntry(7, FeatureRefs.ArcaneMediumArmor.ToString())
            .SetRemoveFeaturesEntry(13, FeatureRefs.ArcaneHeavyArmor.ToString())
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
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddArcaneArmorProficiency(new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Light, ArmorProficiencyGroup.Medium, ArmorProficiencyGroup.Heavy } )
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

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MagusArcanaSelection)
                .AddToAllFeatures(FeatureSelectionRefs.WitchFamiliarSelection.ToString())
                .Configure();

            return FeatureConfigurator.New(ManeuverMastery, ManeuverMasteryGuid, FeatureGroup.MagusArcana)
              .SetDisplayName(ManeuverMasteryDisplayName)
              .SetDescription(ManeuverMasteryDescription)
              .SetIcon(icon)
              .AddCMBBonus(value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.MagusClass.ToString() }).WithStartPlusDivStepProgression(4, 1, true))
              .Configure();
        }

        private const string ReachSpellstrike = "SpireDefender.ReachSpellstrike";
        public static readonly string ReachSpellstrikeGuid = "{31FE5B08-D5FA-46ED-8718-707CEC057B93}";

        internal const string ReachSpellstrikeDisplayName = "SpireDefenderReachSpellstrike.Name";
        private const string ReachSpellstrikeDescription = "SpireDefenderReachSpellstrike.Description";

        public static BlueprintFeature CreateReachSpellstrike()
        {
            var icon = FeatureRefs.EldritchArcherRangedSpellStrike.Reference.Get().Icon;

            return FeatureConfigurator.New(ReachSpellstrike, ReachSpellstrikeGuid, FeatureGroup.MagusArcana)
              .SetDisplayName(ReachSpellstrikeDisplayName)
              .SetDescription(ReachSpellstrikeDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.EldritchArcherRangedSpellStrike.ToString())
              .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 9)
              .Configure();
        }

        private const string CloseRange = "SpireDefender.CloseRange";
        public static readonly string CloseRangeGuid = "{B4507A54-B704-46D7-A755-A6EF953E14A6}";

        internal const string CloseRangeDisplayName = "SpireDefenderCloseRange.Name";
        private const string CloseRangeDescription = "SpireDefenderCloseRange.Description";

        private const string CloseRangeAblity2 = "SpireDefender.UseCloseRange2";
        public static readonly string CloseRangeAblity2Guid = "{6020BA13-BF55-4820-9C2B-B0EF5BCE554E}";

        private const string CloseRangeBuff2 = "SpireDefender.CloseRangeBuff2";
        public static readonly string CloseRangeBuff2Guid = "{6E7757FD-BFAB-44DD-8E17-0B8A158A98D5}";
        public static BlueprintFeature CreateCloseRange()
        {
            var icon = FeatureRefs.SpellStrikeFeature.Reference.Get().Icon;

            BuffConfigurator.New(CloseRangeBuff2, CloseRangeBuff2Guid)
             .SetDisplayName(CloseRangeDisplayName)
             .SetDescription(CloseRangeDescription)
             .SetIcon(icon)
             .AddComponent<MagusCheatRay>()
             .AddPointBlankMaster(WeaponCategory.Ray)
             .SetRanks(2)
             //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            var ability2 = AbilityConfigurator.New(CloseRangeAblity2, CloseRangeAblity2Guid)
              .AllowTargeting(enemies: true)
              .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
              .SetRange(AbilityRange.Touch)
              .SetType(AbilityType.Spell)
              .AddAbilityDeliverTouch(touchWeapon: ItemWeaponRefs.TouchItem.ToString())
              .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Touch)
              .AddAbilityEffectRunAction(ActionsBuilder.New().Add<MagusCloseRange>().Build())
              .AddToSpellLists(level: 0, SpellList.Magus)
                .SetDisplayName(CloseRangeDisplayName)
                .SetDescription(CloseRangeDescription)
                .SetIcon(icon)
                .Configure();

            return FeatureConfigurator.New(CloseRange, CloseRangeGuid, FeatureGroup.MagusArcana)
              .SetDisplayName(CloseRangeDisplayName)
              .SetDescription(CloseRangeDescription)
              .SetIcon(icon)
              .AddSpontaneousSpellConversion(CharacterClassRefs.MagusClass.ToString(), new() { ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2, ability2 })
              .AddPrerequisiteFeature(FeatureRefs.SpellStrikeFeature.ToString())
              .Configure();
        }
    }
}
