using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Geisha
    {
        private const string ArchetypeName = "Geisha";
        private static readonly string ArchetypeGuid = "{D1DCCEB2-31C8-429D-B12B-E4018129C47B}";
        internal const string ArchetypeDisplayName = "Geisha.Name";
        private const string ArchetypeDescription = "Geisha.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BardClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.BardProficiencies.ToString(), FeatureRefs.BardicKnowledge.ToString())
            .AddToAddFeatures(1, GeishaKnowledgeFeat(), TeaCeremonyFeat(), FeatureRefs.ScribingScrolls.ToString(), FeatureRefs.MonkWeaponProficiency.ToString())
            .AddToAddFeatures(2, TeaCeremony2Feat())
            .AddToAddFeatures(3, TeaCeremony3Feat())
            .AddToAddFeatures(4, TeaCeremony4Feat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.BardProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { TeaCeremonyGuid, TeaCeremony2Guid, TeaCeremony3Guid, TeaCeremony4Guid })
                .Configure();
        }

        private const string GeishaKnowledge = "Geisha.GeishaKnowledge";
        public static readonly string GeishaKnowledgeGuid = "{8213B3DE-F1A2-46C2-A721-C68BDBB54100}";

        private const string GeishaKnowledgeAblity = "Geisha.UseGeishaKnowledge";
        private static readonly string GeishaKnowledgeAblityGuid = "{ECADC0DE-3397-49F6-9967-B434BC6991E2}";

        internal const string GeishaKnowledgeDisplayName = "GeishaGeishaKnowledge.Name";
        private const string GeishaKnowledgeDescription = "GeishaGeishaKnowledge.Description";
        public static BlueprintFeature GeishaKnowledgeFeat()
        {
            var icon = AbilityRefs.SoundBurst.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(GeishaKnowledgeAblity, GeishaKnowledgeAblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D4, bonus: ContextValues.Rank(), diceCount: 1), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic), half: true)
                        .Build())
                .SetDisplayName(GeishaKnowledgeDisplayName)
                .SetDescription(GeishaKnowledgeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Long)
                .SetType(AbilityType.Supernatural)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BardClass.ToString() }))
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(GeishaKnowledge, GeishaKnowledgeGuid)
              .SetDisplayName(GeishaKnowledgeDisplayName)
              .SetDescription(GeishaKnowledgeDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony = "Geisha.TeaCeremony";
        public static readonly string TeaCeremonyGuid = "{299FD602-6A89-4F7C-AA42-B93A1CDCA07D}";

        private const string TeaCeremonyAblity = "Geisha.UseTeaCeremony";
        private static readonly string TeaCeremonyAblityGuid = "{5B3C3314-A456-49B7-9D87-0C2621764A25}";

        internal const string TeaCeremonyDisplayName = "GeishaTeaCeremony.Name";
        private const string TeaCeremonyDescription = "GeishaTeaCeremony.Description";
        public static BlueprintFeature TeaCeremonyFeat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TeaCeremonyAblity, TeaCeremonyAblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 4), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(TeaCeremonyDisplayName)
                .SetDescription(TeaCeremonyDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony, TeaCeremonyGuid)
              .SetDisplayName(TeaCeremonyDisplayName)
              .SetDescription(TeaCeremonyDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony2 = "Geisha.TeaCeremony2";
        public static readonly string TeaCeremony2Guid = "{56CB3228-9CC1-4CD1-8C57-138DC46A4EF2}";

        private const string TeaCeremony2Ablity = "Geisha.UseTeaCeremony2";
        private static readonly string TeaCeremony2AblityGuid = "{AABCD29E-C2C3-4DAE-8CCF-705278C886AE}";

        internal const string TeaCeremony2DisplayName = "GeishaTeaCeremony2.Name";
        private const string TeaCeremony2Description = "GeishaTeaCeremony2.Description";
        public static BlueprintFeature TeaCeremony2Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TeaCeremony2Ablity, TeaCeremony2AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 8), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(TeaCeremony2DisplayName)
                .SetDescription(TeaCeremony2Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony2, TeaCeremony2Guid)
              .SetDisplayName(TeaCeremony2DisplayName)
              .SetDescription(TeaCeremony2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony3 = "Geisha.TeaCeremony3";
        public static readonly string TeaCeremony3Guid = "{954F968C-6A26-4C1E-8E52-555C987D6305}";

        private const string TeaCeremony3Ablity = "Geisha.UseTeaCeremony3";
        private static readonly string TeaCeremony3AblityGuid = "{9E8F5F3B-F990-4AFC-B770-019AD6D93BD1}";

        internal const string TeaCeremony3DisplayName = "GeishaTeaCeremony3.Name";
        private const string TeaCeremony3Description = "GeishaTeaCeremony3.Description";
        public static BlueprintFeature TeaCeremony3Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TeaCeremony3Ablity, TeaCeremony3AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 12), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(TeaCeremony3DisplayName)
                .SetDescription(TeaCeremony3Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony3, TeaCeremony3Guid)
              .SetDisplayName(TeaCeremony3DisplayName)
              .SetDescription(TeaCeremony3Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony4 = "Geisha.TeaCeremony4";
        public static readonly string TeaCeremony4Guid = "{969EB602-8BF1-4ECF-8F69-89FA58D5196F}";

        private const string TeaCeremony4Ablity = "Geisha.UseTeaCeremony4";
        private static readonly string TeaCeremony4AblityGuid = "{C3FC91CD-E6DB-4FBB-A869-EA3B5E5D57A1}";

        internal const string TeaCeremony4DisplayName = "GeishaTeaCeremony4.Name";
        private const string TeaCeremony4Description = "GeishaTeaCeremony4.Description";
        public static BlueprintFeature TeaCeremony4Feat()
        {
            var icon = AbilityRefs.Weird.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(TeaCeremony4Ablity, TeaCeremony4AblityGuid)
                .AllowTargeting(enemies: true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .DealDamage(value: ContextDice.Value(DiceType.D6, bonus: ContextValues.Property(Kingmaker.UnitLogic.Mechanics.Properties.UnitProperty.StatBonusCharisma, true), diceCount: 16), damageType: DamageTypes.Energy(type: Kingmaker.Enums.Damage.DamageEnergyType.Sonic))
                        .Build())
                .SetDisplayName(TeaCeremony4DisplayName)
                .SetDescription(TeaCeremony4Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .AddAbilityDeliverProjectile(Kingmaker.EntitySystem.Stats.StatType.Dexterity, projectiles: new() { ProjectileRefs.SonicCommonRay00_Projectile.ToString() },
                type: AbilityProjectileType.Simple, isHandOfTheApprentice: false, lineWidth: 5.Feet(), needAttackRoll: true, weapon: ItemWeaponRefs.RayItem.ToString())
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(4, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony4, TeaCeremony4Guid)
              .SetDisplayName(TeaCeremony4DisplayName)
              .SetDescription(TeaCeremony4Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
