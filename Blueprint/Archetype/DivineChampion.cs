using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics.Properties;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class DivineChampion
    {
        private const string ArchetypeName = "DivineChampion";
        public static readonly string ArchetypeGuid = "{943017DB-F4D0-499A-8BF4-E7D9F2D867BC}";
        internal const string ArchetypeDisplayName = "DivineChampion.Name";
        private const string ArchetypeDescription = "DivineChampion.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.WarpriestClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
              .AddPrerequisiteNoFeature(DeificObedience.DeificObedienceGuid)
            .AddToRemoveFeatures(3, FeatureSelectionRefs.WarpriestFeatSelection.ToString())
            .AddToRemoveFeatures(6, FeatureSelectionRefs.WarpriestFeatSelection.ToString())
            .AddToRemoveFeatures(9, FeatureSelectionRefs.WarpriestFeatSelection.ToString())
            .AddToRemoveFeatures(12, FeatureSelectionRefs.WarpriestFeatSelection.ToString())
            .AddToRemoveFeatures(18, FeatureSelectionRefs.WarpriestFeatSelection.ToString())
            .AddToAddFeatures(3, DeificObedience.DeificObedienceGuid, Sentinel.BonusFeatGuid)
            .AddToAddFeatures(6, FeatureSelectionRefs.FavoriteEnemySelection.ToString())
            .AddToAddFeatures(9, Sentinel.DivineBoon1Guid, CreateFerventBoon())
            .AddToAddFeatures(12, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
            .AddToAddFeatures(18, FeatureSelectionRefs.FavoriteEnemySelection.ToString(), FeatureSelectionRefs.FavoriteEnemyRankUp.ToString())
              .Configure();
        }

        private const string FerventBoon = "DivineChampion.FerventBoon";
        private static readonly string FerventBoonGuid = "{839AAE50-FBED-40E9-9F91-82281A83BFCD}";

        private const string FerventBoonAbility = "DivineChampion.FerventBoonAbility";
        private static readonly string FerventBoonAbilityGuid = "{7242A1A3-52D9-4D93-86BE-7E19C0670367}";

        internal const string FerventBoonDisplayName = "DivineChampionFerventBoon.Name";
        private const string FerventBoonDescription = "DivineChampionFerventBoon.Description";
        private static BlueprintFeature CreateFerventBoon()
        {
            var icon = AbilityRefs.BlessingOfCourageAndLife.Reference.Get().Icon;

            var ability = AbilityConfigurator.New(FerventBoonAbility, FerventBoonAbilityGuid)
                .SetDisplayName(FerventBoonDisplayName)
                .SetDescription(FerventBoonDescription)
                .SetIcon(icon)
                .SetType(AbilityType.Special)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityEffectRunAction(actions: ActionsBuilder.New()
                  .RestoreResource(DeificObedience.DeificObedienceAblityResGuid, 2)
                  .Build())
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: AbilityResourceRefs.WarpriestFervorResource.ToString())
                .Configure();

            return FeatureConfigurator.New(FerventBoon, FerventBoonGuid)
              .SetDisplayName(FerventBoonDisplayName)
              .SetDescription(FerventBoonDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
