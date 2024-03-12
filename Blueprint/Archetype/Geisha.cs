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
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

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
        public static readonly string GeishaKnowledgeGuid = "{B8AF58F5-592B-4A27-B66A-E13FAD5A0CEB}";

        internal const string GeishaKnowledgeDisplayName = "GeishaGeishaKnowledge.Name";
        private const string GeishaKnowledgeDescription = "GeishaGeishaKnowledge.Description";
        public static BlueprintFeature GeishaKnowledgeFeat()
        {
            var icon = AbilityRefs.LuckBlessingMinorAbility.Reference.Get().Icon;

            return FeatureConfigurator.New(GeishaKnowledge, GeishaKnowledgeGuid)
              .SetDisplayName(GeishaKnowledgeDisplayName)
              .SetDescription(GeishaKnowledgeDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.SkillKnowledgeWorld, ContextValues.Rank())
              .AddContextStatBonus(StatType.SkillPersuasion, ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.BardClass.ToString() }, min: 2).WithDiv2Progression())
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string TeaCeremony = "Geisha.TeaCeremony";
        public static readonly string TeaCeremonyGuid = "{3ABA8C4F-9051-4FAE-8A70-893DECAE4E78}";

        private const string TeaCeremonyAblity = "Geisha.UseTeaCeremony";
        private static readonly string TeaCeremonyAblityGuid = "{F79C25D6-6F67-4CA7-A8F4-700DBB067840}";

        private const string TeaCeremonyBuff1 = "Geisha.TeaCeremonyBuff1";
        public static readonly string TeaCeremonyBuff1Guid = "{05F4C765-F36B-4ACE-9DAC-7E3E94E582AC}";

        private const string TeaCeremonyBuff2 = "Geisha.TeaCeremonyBuff2";
        public static readonly string TeaCeremonyBuff2Guid = "{6395F96C-62F7-42F5-9B7B-FC0B07E3A18B}";

        internal const string TeaCeremonyDisplayName = "GeishaTeaCeremony.Name";
        private const string TeaCeremonyDescription = "GeishaTeaCeremony.Description";
        public static BlueprintFeature TeaCeremonyFeat()
        {
            var icon = FeatureRefs.InspireCourageFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(TeaCeremonyBuff1, TeaCeremonyBuff1Guid)
              .CopyFrom(
                BuffRefs.InspireCourageEffectBuff,
                typeof(AddContextStatBonus),
                typeof(ContextRankConfig),
                typeof(SavingThrowContextBonusAgainstDescriptor))
              .SetDisplayName(TeaCeremonyDisplayName)
              .Configure();

            var buff2 = BuffConfigurator.New(TeaCeremonyBuff2, TeaCeremonyBuff2Guid)
              .CopyFrom(
                BuffRefs.InspireCourageMythicEffectBuff,
                typeof(AddContextStatBonus),
                typeof(ContextRankConfig),
                typeof(SavingThrowContextBonusAgainstDescriptor),
                typeof(ContextCalculateSharedValue))
              .SetDisplayName(TeaCeremonyDisplayName)
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(100))
                    .Build();

            var mythic = ActionsBuilder.New()
                    .ApplyBuff(buff2, ContextDuration.Fixed(100))
                    .Build();

            var ability = AbilityConfigurator.New(TeaCeremonyAblity, TeaCeremonyAblityGuid)
                .AllowTargeting(false, false, true, true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Conditional(ConditionsBuilder.New().CasterHasFact(FeatureRefs.MythicInspire.ToString()).Build(),
                        ifTrue: mythic, ifFalse: normal)
                        .Build())
                .SetDisplayName(TeaCeremonyDisplayName)
                .SetDescription(TeaCeremonyDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddAbilityCasterHasFacts(new() { FeatureRefs.InspireCourageFeature.ToString() })
                .AddAbilityCasterInCombat(true)
                .AddAbilityResourceLogic(4, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony, TeaCeremonyGuid)
              .SetDisplayName(TeaCeremonyDisplayName)
              .SetDescription(TeaCeremonyDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony2 = "Geisha.TeaCeremony2";
        public static readonly string TeaCeremony2Guid = "{8EED8E5B-49D5-4F26-9E77-A9142D313713}";

        private const string TeaCeremony2Ablity = "Geisha.UseTeaCeremony2";
        private static readonly string TeaCeremony2AblityGuid = "{BC3E4257-1908-4D3C-B2C9-B68B84FF46CF}";

        private const string TeaCeremony2Buff1 = "Geisha.TeaCeremony2Buff1";
        public static readonly string TeaCeremony2Buff1Guid = "{49F31697-744D-4941-94AC-25580F60E684}";

        private const string TeaCeremony2Buff2 = "Geisha.TeaCeremony2Buff2";
        public static readonly string TeaCeremony2Buff2Guid = "{FE25FDE6-8FDA-4360-814A-419008FCB69D}";

        internal const string TeaCeremony2DisplayName = "GeishaTeaCeremony2.Name";
        private const string TeaCeremony2Description = "GeishaTeaCeremony2.Description";
        public static BlueprintFeature TeaCeremony2Feat()
        {
            var icon = FeatureRefs.InspireCompetenceFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(TeaCeremony2Buff1, TeaCeremony2Buff1Guid)
              .CopyFrom(
                BuffRefs.InspireCompetenceEffectBuff,
                typeof(AddContextStatBonus),
                typeof(ContextRankConfig),
                typeof(BuffAllSkillsBonusAbilityValue))
              .SetDisplayName(TeaCeremony2DisplayName)
              .Configure();

            var buff2 = BuffConfigurator.New(TeaCeremony2Buff2, TeaCeremony2Buff2Guid)
              .CopyFrom(
                BuffRefs.InspireCompetenceMythicEffectBuff,
                typeof(AddContextStatBonus),
                typeof(ContextRankConfig),
                typeof(BuffAllSkillsBonusAbilityValue),
                typeof(ContextCalculateSharedValue))
              .SetDisplayName(TeaCeremony2DisplayName)
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(100))
                    .Build();

            var mythic = ActionsBuilder.New()
                    .ApplyBuff(buff2, ContextDuration.Fixed(100))
                    .Build();

            var ability = AbilityConfigurator.New(TeaCeremony2Ablity, TeaCeremony2AblityGuid)
                .AllowTargeting(false, false, true, true)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .Conditional(ConditionsBuilder.New().CasterHasFact(FeatureRefs.MythicInspire.ToString()).Build(),
                        ifTrue: mythic, ifFalse: normal)
                        .Build())
                .SetDisplayName(TeaCeremony2DisplayName)
                .SetDescription(TeaCeremony2Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddAbilityCasterHasFacts(new() { FeatureRefs.InspireCompetenceFeature.ToString() })
                .AddAbilityCasterInCombat(true)
                .AddAbilityResourceLogic(4, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony2, TeaCeremony2Guid)
              .SetDisplayName(TeaCeremony2DisplayName)
              .SetDescription(TeaCeremony2Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony3 = "Geisha.TeaCeremony3";
        public static readonly string TeaCeremony3Guid = "{5161E6EF-5515-4C75-A4AE-6D634C7026C2}";

        private const string TeaCeremony3Ablity = "Geisha.UseTeaCeremony3";
        private static readonly string TeaCeremony3AblityGuid = "{D33DF460-3834-43C0-8285-11EB1AA7E606}";

        private const string TeaCeremony3Buff1 = "Geisha.TeaCeremony3Buff1";
        public static readonly string TeaCeremony3Buff1Guid = "{F2787F58-3A4A-4CEC-B237-2B1740397541}";

        internal const string TeaCeremony3DisplayName = "GeishaTeaCeremony3.Name";
        private const string TeaCeremony3Description = "GeishaTeaCeremony3.Description";
        public static BlueprintFeature TeaCeremony3Feat()
        {
            var icon = FeatureRefs.InspireGreatnessFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(TeaCeremony3Buff1, TeaCeremony3Buff1Guid)
              .CopyFrom(
                BuffRefs.InspireGreatnessEffectBuff,
                typeof(AddContextStatBonus),
                typeof(TemporaryHitPointsConstitutionBased),
                typeof(SpellDescriptorComponent))
              .SetDisplayName(TeaCeremony3DisplayName)
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(100))
                    .Build();

            var ability = AbilityConfigurator.New(TeaCeremony3Ablity, TeaCeremony3AblityGuid)
                .AllowTargeting(false, false, true, true)
                .AddAbilityEffectRunAction(normal)
                .SetDisplayName(TeaCeremony3DisplayName)
                .SetDescription(TeaCeremony3Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddAbilityCasterHasFacts(new() { FeatureRefs.InspireGreatnessFeature.ToString() })
                .AddAbilityCasterInCombat(true)
                .AddAbilityResourceLogic(4, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
                .Configure();

            return FeatureConfigurator.New(TeaCeremony3, TeaCeremony3Guid)
              .SetDisplayName(TeaCeremony3DisplayName)
              .SetDescription(TeaCeremony3Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TeaCeremony4 = "Geisha.TeaCeremony4";
        public static readonly string TeaCeremony4Guid = "{0B64B50B-6C98-4DF9-9B57-15EE44D3E465}";

        private const string TeaCeremony4Ablity = "Geisha.UseTeaCeremony4";
        private static readonly string TeaCeremony4AblityGuid = "{E2ABDF24-351D-4D10-B956-63A0EA6096F1}";

        private const string TeaCeremony4Buff1 = "Geisha.TeaCeremony4Buff1";
        public static readonly string TeaCeremony4Buff1Guid = "{2DD07C15-6376-49B6-9BB7-25B7FCD24BA8}";

        internal const string TeaCeremony4DisplayName = "GeishaTeaCeremony4.Name";
        private const string TeaCeremony4Description = "GeishaTeaCeremony4.Description";
        public static BlueprintFeature TeaCeremony4Feat()
        {
            var icon = FeatureRefs.InspireHeroicsFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(TeaCeremony4Buff1, TeaCeremony4Buff1Guid)
              .CopyFrom(
                BuffRefs.InspireHeroicsEffectBuff,
                typeof(AddContextStatBonus))
              .SetDisplayName(TeaCeremony4DisplayName)
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(100))
                    .Build();

            var ability = AbilityConfigurator.New(TeaCeremony4Ablity, TeaCeremony4AblityGuid)
                .AllowTargeting(false, false, true, true)
                .AddAbilityEffectRunAction(normal)
                .SetDisplayName(TeaCeremony4DisplayName)
                .SetDescription(TeaCeremony4Description)
                .SetIcon(icon)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Supernatural)
                .AddAbilityCasterHasFacts(new() { FeatureRefs.InspireHeroicsFeature.ToString() })
                .AddAbilityCasterInCombat(true)
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
