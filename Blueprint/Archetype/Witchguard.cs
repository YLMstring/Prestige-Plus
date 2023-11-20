using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.Feat;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Conditions.Builder;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Witchguard
    {
        private const string ArchetypeName = "Witchguard";
        private static readonly string ArchetypeGuid = "{BC15A296-BE61-4F73-B545-1C89EE576360}";
        internal const string ArchetypeDisplayName = "Witchguard.Name";
        private const string ArchetypeDescription = "Witchguard.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.RangerClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(3, FeatureRefs.Endurance.ToString())
            .SetRemoveFeaturesEntry(4, FeatureSelectionRefs.HuntersBondSelection.ToString())
            .AddToAddFeatures(3, BodyGuard.FeatGuid)
            .AddToAddFeatures(4, DefendChargeFeat(), CreatePatron())
            .AddToAddFeatures(7, BodyGuard.Feat2Guid)
              .Configure();
        }

        private const string DefendCharge = "Witchguard.DefendCharge";
        private static readonly string DefendChargeGuid = "{44D3A5A3-A5E0-46D4-B3D0-495BAFDDE9A1}";

        private const string DefendChargeBuff = "Witchguard.DefendChargeBuff";
        private static readonly string DefendChargeBuffGuid = "{618637A2-96F9-41D6-8BFD-337CCDB93C12}";

        private const string DefendChargeAbility = "Witchguard.DefendChargeAbility";
        private static readonly string DefendChargeAbilityGuid = "{F1DAD82F-419C-45A6-A02D-80C0F4F60521}";

        private const string DefendChargeAblityRes = "WitchguardDefendChargeAblityRes";
        private static readonly string DefendChargeAblityResGuid = "{87E66155-DEB9-489D-8B7A-8E84556B9503}";

        internal const string DefendChargeDisplayName = "WitchguardDefendCharge.Name";
        private const string DefendChargeDescription = "WitchguardDefendCharge.Description";
        public static BlueprintFeature DefendChargeFeat()
        {
            var icon = FeatureRefs.SavingSlash.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(DefendChargeAblityRes, DefendChargeAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { CharacterClassRefs.RangerClass.ToString() }, startingLevel: 1, levelsPerStep: 3, bonusPerStep: 1))
                .SetUseMax()
                .SetMax(6)
                .Configure();

            var Buff1 = BuffConfigurator.New(DefendChargeBuff, DefendChargeBuffGuid)
             .SetDisplayName(DefendChargeDisplayName)
             .SetDescription(DefendChargeDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .AddComponent<DefendChargeComp>()
             .Configure();

            var ability = AbilityConfigurator.New(DefendChargeAbility, DefendChargeAbilityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Point)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Variable(ContextValues.Rank()))
                        .Build())
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
                .SetDisplayName(DefendChargeDisplayName)
                .SetDescription(DefendChargeDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Touch)
                .SetType(AbilityType.Special)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .SetCanTargetPoint(false)
                .SetCanTargetSelf(false)
                .Configure();

            return FeatureConfigurator.New(DefendCharge, DefendChargeGuid)
              .SetDisplayName(DefendChargeDisplayName)
              .SetDescription(DefendChargeDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }

        private const string Patron = "Witchguard.Patron";
        private static readonly string PatronGuid = "{6B8EFA1F-CC2D-4CAC-8920-90A2E7920A4C}";

        internal const string PatronDisplayName = "WitchguardPatron.Name";
        private const string PatronDescription = "WitchguardPatron.Description";

        private static BlueprintFeatureSelection CreatePatron()
        {
            var icon = FeatureRefs.WitchPatron.Reference.Get().Icon;

            return FeatureSelectionConfigurator.New(Patron, PatronGuid)
                .CopyFrom(FeatureSelectionRefs.WitchPatronSelection.Reference)
              .SetDisplayName(PatronDisplayName)
              .SetDescription(PatronDescription)
              .SetIcon(icon)
              .AddComponent<WitchGuardPatron>()
              .Configure();
        }
    }
}
