using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.OtherFeatRelated;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class SoundStriker
    {
        private const string ArchetypeName = "SoundStriker";
        private static readonly string ArchetypeGuid = "{B723EA1E-36EC-4F0F-B12F-DA92AB5E594A}";
        internal const string ArchetypeDisplayName = "SoundStriker.Name";
        private const string ArchetypeDescription = "SoundStriker.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BardClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(3, FeatureRefs.InspireCompetenceFeature.ToString())
            .SetRemoveFeaturesEntry(6, FeatureRefs.FascinateFeature.ToString())
            .AddToAddFeatures(1, WordstrikeFeat())
            .AddToAddFeatures(2, WeirdWordsFeat())
            .AddToAddFeatures(3, WeirdWords2Feat())
            .AddToAddFeatures(4, WeirdWords3Feat())
            .AddToAddFeatures(5, WeirdWords4Feat())
            .AddToAddFeatures(6, WeirdWords5Feat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.BardProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { WordstrikeGuid, WeirdWordsGuid, WeirdWords2Guid, WeirdWords3Guid, WeirdWords4Guid, WeirdWords5Guid })
                .Configure();
        }

        private const string Wordstrike = "SoundStriker.Wordstrike";
        public static readonly string WordstrikeGuid = "{8213B3DE-F1A2-46C2-A721-C68BDBB54100}";

        private const string WordstrikeAblity = "SoundStriker.UseWordstrike";
        private static readonly string WordstrikeAblityGuid = "{ECADC0DE-3397-49F6-9967-B434BC6991E2}";

        internal const string WordstrikeDisplayName = "SoundStrikerWordstrike.Name";
        private const string WordstrikeDescription = "SoundStrikerWordstrike.Description";
        public static BlueprintFeature WordstrikeFeat()
        {
            var icon = AbilityRefs.SoundBurst.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WordstrikeAblity, WordstrikeAblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D4, bonus: ContextValues.Rank(), diceCount: 1), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic), half: true)
                        .Build())
                .SetDisplayName(WordstrikeDisplayName)
                .SetDescription(WordstrikeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Long)
                .SetType(AbilityType.Supernatural)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BardClass.ToString() }))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(Wordstrike, WordstrikeGuid)
              .SetDisplayName(WordstrikeDisplayName)
              .SetDescription(WordstrikeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WeirdWords = "SoundStriker.WeirdWords";
        public static readonly string WeirdWordsGuid = "{299FD602-6A89-4F7C-AA42-B93A1CDCA07D}";

        private const string WeirdWordsAblity = "SoundStriker.UseWeirdWords";
        private static readonly string WeirdWordsAblityGuid = "{5B3C3314-A456-49B7-9D87-0C2621764A25}";

        internal const string WeirdWordsDisplayName = "SoundStrikerWeirdWords.Name";
        private const string WeirdWordsDescription = "SoundStrikerWeirdWords.Description";
        public static BlueprintFeature WeirdWordsFeat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WeirdWordsAblity, WeirdWordsAblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 4), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(WeirdWordsDisplayName)
                .SetDescription(WeirdWordsDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WeirdWords, WeirdWordsGuid)
              .SetDisplayName(WeirdWordsDisplayName)
              .SetDescription(WeirdWordsDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WeirdWords2 = "SoundStriker.WeirdWords2";
        public static readonly string WeirdWords2Guid = "{56CB3228-9CC1-4CD1-8C57-138DC46A4EF2}";

        private const string WeirdWords2Ablity = "SoundStriker.UseWeirdWords2";
        private static readonly string WeirdWords2AblityGuid = "{AABCD29E-C2C3-4DAE-8CCF-705278C886AE}";

        internal const string WeirdWords2DisplayName = "SoundStrikerWeirdWords2.Name";
        private const string WeirdWords2Description = "SoundStrikerWeirdWords2.Description";
        public static BlueprintFeature WeirdWords2Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WeirdWords2Ablity, WeirdWords2AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 8), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(WeirdWords2DisplayName)
                .SetDescription(WeirdWords2Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WeirdWords2, WeirdWords2Guid)
              .SetDisplayName(WeirdWords2DisplayName)
              .SetDescription(WeirdWords2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WeirdWords3 = "SoundStriker.WeirdWords3";
        public static readonly string WeirdWords3Guid = "{954F968C-6A26-4C1E-8E52-555C987D6305}";

        private const string WeirdWords3Ablity = "SoundStriker.UseWeirdWords3";
        private static readonly string WeirdWords3AblityGuid = "{9E8F5F3B-F990-4AFC-B770-019AD6D93BD1}";

        internal const string WeirdWords3DisplayName = "SoundStrikerWeirdWords3.Name";
        private const string WeirdWords3Description = "SoundStrikerWeirdWords3.Description";
        public static BlueprintFeature WeirdWords3Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WeirdWords3Ablity, WeirdWords3AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 12), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(WeirdWords3DisplayName)
                .SetDescription(WeirdWords3Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WeirdWords3, WeirdWords3Guid)
              .SetDisplayName(WeirdWords3DisplayName)
              .SetDescription(WeirdWords3Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WeirdWords4 = "SoundStriker.WeirdWords4";
        public static readonly string WeirdWords4Guid = "{969EB602-8BF1-4ECF-8F69-89FA58D5196F}";

        private const string WeirdWords4Ablity = "SoundStriker.UseWeirdWords4";
        private static readonly string WeirdWords4AblityGuid = "{C3FC91CD-E6DB-4FBB-A869-EA3B5E5D57A1}";

        internal const string WeirdWords4DisplayName = "SoundStrikerWeirdWords4.Name";
        private const string WeirdWords4Description = "SoundStrikerWeirdWords4.Description";
        public static BlueprintFeature WeirdWords4Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WeirdWords4Ablity, WeirdWords4AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 16), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(WeirdWords4DisplayName)
                .SetDescription(WeirdWords4Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(4, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WeirdWords4, WeirdWords4Guid)
              .SetDisplayName(WeirdWords4DisplayName)
              .SetDescription(WeirdWords4Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WeirdWords5 = "SoundStriker.WeirdWords5";
        public static readonly string WeirdWords5Guid = "{9348CEE3-A9B1-4C17-8B5C-ED330D052276}";

        private const string WeirdWords5Ablity = "SoundStriker.UseWeirdWords5";
        private static readonly string WeirdWords5AblityGuid = "{1923E1BD-91FA-4B70-AC95-6672A2355A88}";

        internal const string WeirdWords5DisplayName = "SoundStrikerWeirdWords5.Name";
        private const string WeirdWords5Description = "SoundStrikerWeirdWords5.Description";
        public static BlueprintFeature WeirdWords5Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(WeirdWords5Ablity, WeirdWords5AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 20), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(WeirdWords5DisplayName)
                .SetDescription(WeirdWords5Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(5, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(WeirdWords5, WeirdWords5Guid)
              .SetDisplayName(WeirdWords5DisplayName)
              .SetDescription(WeirdWords5Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
