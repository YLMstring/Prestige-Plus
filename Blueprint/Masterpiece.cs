using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Spell;

namespace PrestigePlus.Blueprint
{
    internal class Masterpiece
    {
        private const string BardMasterpieceFeat = "BardMasterpiece.BardMasterpiece";
        public static readonly string BardMasterpieceGuid = "{22DF5EBC-A04E-4C3B-B72B-D6A032DF26BB}";

        internal const string BardMasterpieceDisplayName = "BardMasterpiece.Name";
        private const string BardMasterpieceDescription = "BardMasterpiece.Description";
        public static void BardMasterpieceConfigure()
        {
            var icon = AbilityRefs.DeadlyPerformanceAbility.Reference.Get().Icon;

            var feat = FeatureSelectionConfigurator.New(BardMasterpieceFeat, BardMasterpieceGuid)
              .SetDisplayName(BardMasterpieceDisplayName)
              .SetDescription(BardMasterpieceDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(CreateTripleTime())
              .AddToAllFeatures(CreateTwistingSteel())
              .AddToAllFeatures(CreateStoneFace())
              .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BasicFeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.DragonLevel2FeatSelection)
                .AddToAllFeatures(feat)
                .Configure();
        }

        private const string TripleTime = "Masterpiece.TripleTime";
        private static readonly string TripleTimeGuid = "{EA78ACB4-D560-44C3-A6DA-2E3DDD20006B}";
        internal const string TripleTimeDisplayName = "MasterpieceTripleTime.Name";
        private const string TripleTimeDescription = "MasterpieceTripleTime.Description";

        private const string TripleTimeAblity = "Masterpiece.UseTripleTime";
        private static readonly string TripleTimeAblityGuid = "{AD42CAF0-4315-4C08-8EBB-1FB888F32F84}";

        private const string TripleTimeBuff = "Masterpiece.TripleTimeBuff";
        private static readonly string TripleTimeBuffGuid = "{025E7123-73F7-4B38-B063-8F1CB3A4B3F3}";

        public static BlueprintFeature CreateTripleTime()
        {
            var icon = AbilityRefs.Lich1SiphonTimeAbility.Reference.Get().Icon;
            var fx = AbilityRefs.SoothingPerformanceAbility.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff = BuffConfigurator.New(TripleTimeBuff, TripleTimeBuffGuid)
              .SetDisplayName(TripleTimeDisplayName)
              .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .AddBuffMovementSpeed(value: 5, descriptor: ModifierDescriptor.Enhancement)
              .Configure();

            var shoot = ActionsBuilder.New()
                .ApplyBuff(buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Hours))
                .Build();

            var ability = AbilityConfigurator.New(TripleTimeAblity, TripleTimeAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(TripleTimeDisplayName)
                .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Personal)
              .AddAbilityTargetsAround(includeDead: false, targetType: Kingmaker.UnitLogic.Abilities.Components.TargetType.Ally, radius: 100.Feet(), spreadSpeed: 20.Feet())
              .AddComponent(fx)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .Configure();

            return FeatureConfigurator.New(TripleTime, TripleTimeGuid)
              .SetDisplayName(TripleTimeDisplayName)
              .SetDescription(TripleTimeDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillUseMagicDevice, 3)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string TwistingSteel = "Masterpiece.TwistingSteel";
        private static readonly string TwistingSteelGuid = "{C73EB87C-ED4E-42E8-82C9-C73C793B8797}";
        internal const string TwistingSteelDisplayName = "MasterpieceTwistingSteel.Name";
        private const string TwistingSteelDescription = "MasterpieceTwistingSteel.Description";

        private const string TwistingSteelAblity = "Masterpiece.UseTwistingSteel";
        private static readonly string TwistingSteelAblityGuid = "{B2C149BD-CE23-4753-9D75-9A74C642C9E3}";
        public static BlueprintFeature CreateTwistingSteel()
        {
            var icon = AbilityRefs.SwordOfEternalSquireAbility.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .CastSpell(ShieldOther.ShieldOtherAbilityGuid)
                .Build();

            var ability = AbilityConfigurator.New(TwistingSteelAblity, TwistingSteelAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(TwistingSteelDisplayName)
                .SetDescription(TwistingSteelDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Custom)
              .SetCustomRange(5)
              .AllowTargeting(false, false, true, false)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .Configure();

            return FeatureConfigurator.New(TwistingSteel, TwistingSteelGuid)
              .SetDisplayName(TwistingSteelDisplayName)
              .SetDescription(TwistingSteelDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string StoneFace = "Masterpiece.StoneFace";
        private static readonly string StoneFaceGuid = "{21606A26-8EA7-4538-92E2-3AC549548EFA}";
        internal const string StoneFaceDisplayName = "MasterpieceStoneFace.Name";
        private const string StoneFaceDescription = "MasterpieceStoneFace.Description";

        private const string StoneFaceAblity = "Masterpiece.UseStoneFace";
        private static readonly string StoneFaceAblityGuid = "{DF05FC9B-9F39-4D26-AD3A-51E21EC6AC06}";
        public static BlueprintFeature CreateStoneFace()
        {
            var icon = AbilityRefs.StoneGolemSlow.Reference.Get().Icon;

            var shoot = ActionsBuilder.New()
                .CastSpell(AbilityRefs.StoneToFlesh.ToString())
                .Build();

            var ability = AbilityConfigurator.New(StoneFaceAblity, StoneFaceAblityGuid)
                .AddAbilityEffectRunAction(shoot)
                .SetType(AbilityType.Supernatural)
                .SetDisplayName(StoneFaceDisplayName)
                .SetDescription(StoneFaceDescription)
              .SetIcon(icon)
              .SetRange(AbilityRange.Close)
              .AllowTargeting(false, false, true, false)
              .SetIsFullRoundAction(true)
              .AddAbilityCasterInCombat(true)
              .AddAbilityResourceLogic(2, isSpendResource: true, requiredResource: AbilityResourceRefs.BardicPerformanceResource.ToString())
              .Configure();

            return FeatureConfigurator.New(StoneFace, StoneFaceGuid)
              .SetDisplayName(StoneFaceDisplayName)
              .SetDescription(StoneFaceDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.SkillPersuasion, 7)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
