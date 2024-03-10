using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.Archetype;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class AnkouShadow
    {
        private const string ArchetypeName = "AnkouShadow";
        private static readonly string ArchetypeGuid = "{13145231-4901-420A-B412-453D240B3DE2}";
        internal const string ArchetypeDisplayName = "AnkouShadow.Name";
        private const string ArchetypeDescription = "AnkouShadow.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.SlayerClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.SlayerStudyTargetFeature.ToString())
            .SetRemoveFeaturesEntry(5, FeatureRefs.SlayerStudyTargetFeature.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.SlayerSwiftStudyTargetFeature.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.SlayerStudyTargetFeature.ToString())
            .SetRemoveFeaturesEntry(15, FeatureRefs.SlayerStudyTargetFeature.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.SlayerStudyTargetFeature.ToString())
            .AddToAddFeatures(1, ShadowDoubleFeat())
            .AddToAddFeatures(2, ShadowDoubleGuid)
            .AddToAddFeatures(3, ShadowDoubleGuid)
            .AddToAddFeatures(4, ShadowDoubleGuid)
            .AddToAddFeatures(5, AnkouVisionFeat())
            .AddToAddFeatures(6, UnfetteredShadowsFeat())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.SlayerProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { AnkouVisionGuid, ShadowDoubleGuid, UnfetteredShadowsGuid })
                .Configure();
        }

        private const string AnkouVision = "AnkouShadow.AnkouVision";
        public static readonly string AnkouVisionGuid = "{3816676C-EE22-41AF-810E-34B2A75162BA}";

        private const string AnkouVisionAblity = "AnkouShadow.UseAnkouVision";
        private static readonly string AnkouVisionAblityGuid = "{4FC6989F-D84E-4798-8054-D2138A8B0036}";

        private const string AnkouVisionRes = "AnkouShadow.AnkouVisionRes";
        public static readonly string AnkouVisionResGuid = "{C320E977-B301-48E6-B2D2-ABE9837012AD}";

        internal const string AnkouVisionDisplayName = "AnkouShadowAnkouVision.Name";
        private const string AnkouVisionDescription = "AnkouShadowAnkouVision.Description";
        public static BlueprintFeature AnkouVisionFeat()
        {
            var icon = AbilityRefs.SeeInvisibility.Reference.Get().Icon;
            var fx = AbilityRefs.SeeInvisibility.Reference.Get().GetComponent<AbilitySpawnFx>();

            var abilityresourse = AbilityResourceConfigurator.New(AnkouVisionRes, AnkouVisionResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }))
                .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(BuffRefs.SeeInvisibilitytBuff.ToString(), ContextDuration.Fixed(10))
                    .Build();

            var ability = AbilityConfigurator.New(AnkouVisionAblity, AnkouVisionAblityGuid)
                .AddAbilityEffectRunAction(normal)
                .SetDisplayName(AnkouVisionDisplayName)
                .SetDescription(AnkouVisionDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.SpellLike)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(AnkouVision, AnkouVisionGuid)
              .SetDisplayName(AnkouVisionDisplayName)
              .SetDescription(AnkouVisionDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string ShadowDouble = "AnkouShadow.ShadowDouble";
        public static readonly string ShadowDoubleGuid = "{F3AEBB0E-1626-4CBF-98D9-3B29B8FC21D8}";

        private const string ShadowDoubleAblity = "AnkouShadow.UseShadowDouble";
        private static readonly string ShadowDoubleAblityGuid = "{84B095DD-B7A5-4E68-B789-DA9030F3BEB1}";

        private const string ShadowDoubleBuff = "AnkouShadow.ShadowDoubleBuff";
        public static readonly string ShadowDoubleBuffGuid = "{1C3DF714-18C4-48BB-B5BF-3E701778080E}";

        internal const string ShadowDoubleDisplayName = "AnkouShadowShadowDouble.Name";
        private const string ShadowDoubleDescription = "AnkouShadowShadowDouble.Description";
        public static BlueprintFeature ShadowDoubleFeat()
        {
            var icon = AbilityRefs.FalseLife.Reference.Get().Icon;

            var buff = BuffConfigurator.New(ShadowDoubleBuff, ShadowDoubleBuffGuid)
              .SetDisplayName(ShadowDoubleDisplayName)
              .SetDescription(ShadowDoubleDescription)
              .SetIcon(icon)
              .AddMirrorImage(ContextDice.Value(Kingmaker.RuleSystem.DiceType.Zero, 0, ContextValues.Rank()), 4)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(ShadowDoubleGuid))
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuffPermanent(buff)
                    .Build();

            var ability = AbilityConfigurator.New(ShadowDoubleAblity, ShadowDoubleAblityGuid)
                .AddAbilityEffectRunAction(normal)
                .SetDisplayName(ShadowDoubleDisplayName)
                .SetDescription(ShadowDoubleDescription)
                .SetIcon(icon)
                .SetIsFullRoundAction(true)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.SpellLike)
                .Configure();

            return FeatureConfigurator.New(ShadowDouble, ShadowDoubleGuid)
              .SetDisplayName(ShadowDoubleDisplayName)
              .SetDescription(ShadowDoubleDescription)
              .SetIcon(icon)
              .SetRanks(10)
              .AddFacts(new() { ability })
              .AddCombatStateTrigger(combatStartActions: ActionsBuilder.New().CastSpell(ability).Build())
              .Configure();
        }

        private const string UnfetteredShadows = "AnkouShadow.UnfetteredShadows";
        public static readonly string UnfetteredShadowsGuid = "{D7691C95-F888-4F5D-AB76-26CA05BCA902}";

        private const string UnfetteredShadowsAblity = "AnkouShadow.UseUnfetteredShadows";
        private static readonly string UnfetteredShadowsAblityGuid = "{895015F7-4545-4CDB-B744-258C0C6A5D1D}";

        private const string UnfetteredShadowsBuff = "AnkouShadow.UnfetteredShadowsBuff";
        public static readonly string UnfetteredShadowsBuffGuid = "{DF6A43AA-2431-4346-8D5C-62CCC35CBBC0}";

        private const string UnfetteredShadowsAblity2 = "AnkouShadow.UseUnfetteredShadows2";
        private static readonly string UnfetteredShadowsAblity2Guid = "{F1105FAC-394C-459D-9F67-0F49E296960C}";

        internal const string UnfetteredShadowsDisplayName = "AnkouShadowUnfetteredShadows.Name";
        private const string UnfetteredShadowsDescription = "AnkouShadowUnfetteredShadows.Description";

        private const string UnfetteredShadowsRes = "AnkouShadow.UnfetteredShadowsRes";
        public static readonly string UnfetteredShadowsResGuid = "{34368E5D-AFF1-49B3-A1B1-45A9878B70B0}";

        private const string UnfetteredShadowsBuff2 = "AnkouShadow.UnfetteredShadowsBuff2";
        public static readonly string UnfetteredShadowsBuff2Guid = "{F9E113CE-6A99-4D51-966D-F2F2AACB4F45}";
        public static BlueprintFeature UnfetteredShadowsFeat()
        {
            var icon = AbilityRefs.FalseLifeGreater.Reference.Get().Icon;
            var icon2 = AbilityRefs.ReducePersonMass.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(UnfetteredShadowsRes, UnfetteredShadowsResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(3)
                        .IncreaseByStat(Kingmaker.EntitySystem.Stats.StatType.Intelligence))
                .Configure();

            var buff2 = BuffConfigurator.New(UnfetteredShadowsBuff2, UnfetteredShadowsBuff2Guid)
              .SetDisplayName(UnfetteredShadowsDisplayName)
              .SetDescription(UnfetteredShadowsDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<AnkouShadowPenalty>()
              .Configure();

            var ability2 = AbilityConfigurator.New(UnfetteredShadowsAblity2, UnfetteredShadowsAblity2Guid)
                .AllowTargeting(false, true, false, false)
                .AddComponent<AnkouAttacks>(c => { c.CooldownBuff = buff2; })
                .SetDisplayName(UnfetteredShadowsDisplayName)
                .SetDescription(UnfetteredShadowsDescription)
                .SetIcon(icon)
                .AddAbilityCasterHasNoFacts(new() { buff2 })
                .SetRange(AbilityRange.Weapon)
                .SetType(AbilityType.Supernatural)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .Configure();

            var buff = BuffConfigurator.New(UnfetteredShadowsBuff, UnfetteredShadowsBuffGuid)
              .SetDisplayName(UnfetteredShadowsDisplayName)
              .SetDescription(UnfetteredShadowsDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability2 })
              .Configure();

            var normal = ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(10))
                    .Build();

            var ability = AbilityConfigurator.New(UnfetteredShadowsAblity, UnfetteredShadowsAblityGuid)
                .AddAbilityEffectRunAction(normal)
                .SetDisplayName(UnfetteredShadowsDisplayName)
                .SetDescription(UnfetteredShadowsDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(UnfetteredShadows, UnfetteredShadowsGuid)
              .SetDisplayName(UnfetteredShadowsDisplayName)
              .SetDescription(UnfetteredShadowsDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
