using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class MenhirSavant
    {
        private const string ArchetypeName = "MenhirSavant";
        private static readonly string ArchetypeGuid = "{87DBE586-F38F-4B48-AC14-7CF04164F508}";
        internal const string ArchetypeDisplayName = "MenhirSavant.Name";
        private const string ArchetypeDescription = "MenhirSavant.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.DruidClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureRefs.NatureSense.ToString())
            .SetRemoveFeaturesEntry(2, FeatureRefs.DruidWoodlandStride.ToString())
            .AddToAddFeatures(2, PlaceMagicFeat())
            .AddToAddFeatures(9, CreateWalkLines())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.DruidProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { WalkLinesGuid, PlaceMagicGuid })
                .Configure();
        }

        private const string WalkLines = "MenhirSavant.WalkLines";
        public static readonly string WalkLinesGuid = "{AD0FC28A-E37E-4535-A082-CC1A535B0613}";

        internal const string WalkLinesDisplayName = "MenhirSavantWalkLines.Name";
        private const string WalkLinesDescription = "MenhirSavantWalkLines.Description";

        public static BlueprintFeature CreateWalkLines()
        {
            var icon = AbilityRefs.KiAbudantStep.Reference.Get().Icon;

            return FeatureConfigurator.New(WalkLines, WalkLinesGuid)
              .SetDisplayName(WalkLinesDisplayName)
              .SetDescription(WalkLinesDescription)
              .SetIcon(icon)
              .AddFacts(new() { ShadowDancer.ShadowJumpAblityGuid2 })
              .AddAbilityResources(resource: ShadowDancer.ShadowJumpAblityResGuid, restoreAmount: true)
              .AddIncreaseResourceAmountBySharedValue(false, ShadowDancer.ShadowJumpAblityResGuid, ContextValues.Property(UnitProperty.StatBonusWisdom))
              .Configure();
        }

        private const string PlaceMagic = "MenhirSavant.PlaceMagic";
        public static readonly string PlaceMagicGuid = "{09CBFD91-6583-464B-AE88-F93120A1D22F}";

        private const string PlaceMagicAblity = "MenhirSavant.UsePlaceMagic";
        private static readonly string PlaceMagicAblityGuid = "{40779FD4-19D2-45C3-A018-D5BFD4C2AE86}";

        private const string PlaceMagicBuff2 = "MenhirSavant.PlaceMagicBuff2";
        private static readonly string PlaceMagicBuff2Guid = "{85734B85-EF44-4325-B28C-B782FF853C4A}";

        internal const string PlaceMagicDisplayName = "MenhirSavantPlaceMagic.Name";
        private const string PlaceMagicDescription = "MenhirSavantPlaceMagic.Description";

        private const string PlaceMagicAblityRes = "MenhirSavant.PlaceMagicRes";
        public static readonly string PlaceMagicAblityResGuid = "{CB37DC09-3E57-4591-986A-8C7D7A4BD466}";
        public static BlueprintFeature PlaceMagicFeat()
        {
            var icon = FeatureRefs.Geomancy.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(PlaceMagicAblityRes, PlaceMagicAblityResGuid)
                .SetMaxAmount(ResourceAmountBuilder.New(3).IncreaseByStat(StatType.Wisdom))
                .Configure();

            var Buff2 = BuffConfigurator.New(PlaceMagicBuff2, PlaceMagicBuff2Guid)
             .SetDisplayName(PlaceMagicDisplayName)
             .SetDescription(PlaceMagicDescription)
             .SetIcon(icon)
             .AddIncreaseCasterLevel(value: 1)
             .Configure();

            var ability = AbilityConfigurator.New(PlaceMagicAblity, PlaceMagicAblityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff2, ContextDuration.Fixed(1))
                        .Build())
                .SetDisplayName(PlaceMagicDisplayName)
                .SetDescription(PlaceMagicDescription)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(PlaceMagic, PlaceMagicGuid)
              .SetDisplayName(PlaceMagicDisplayName)
              .SetDescription(PlaceMagicDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
