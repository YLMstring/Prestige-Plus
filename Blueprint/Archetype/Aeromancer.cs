using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.Blueprint.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Aeromancer
    {
        private const string ArchetypeName = "Aeromancer";
        public static readonly string ArchetypeGuid = "{2BFBA710-06C5-446E-B7B3-426EE41236C9}";
        internal const string ArchetypeDisplayName = "Aeromancer.Name";
        private const string ArchetypeDescription = "Aeromancer.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.ArcanistClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToRemoveFeatures(5, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToRemoveFeatures(11, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
            .AddToAddFeatures(1, DeificObedience.DeificObedienceGuid, Sentinel.BonusFeatGuid)
            .AddToAddFeatures(5, FeatureSelectionRefs.FavoriteEnemySelection.ToString())
            .AddToAddFeatures(11, Sentinel.DivineBoon1Guid, CreateFerventBoon())
              .Configure();
        }

        private const string FerventBoon = "Aeromancer.FerventBoon";
        private static readonly string FerventBoonGuid = "{839AAE50-FBED-40E9-9F91-82281A83BFCD}";

        private const string FerventBoonAbility = "Aeromancer.FerventBoonAbility";
        private static readonly string FerventBoonAbilityGuid = "{7242A1A3-52D9-4D93-86BE-7E19C0670367}";

        internal const string FerventBoonDisplayName = "AeromancerFerventBoon.Name";
        private const string FerventBoonDescription = "AeromancerFerventBoon.Description";
        private static BlueprintFeature CreateFerventBoon()
        {
            var icon = FeatureRefs.WarpriestFervorQuickenCast.Reference.Get().Icon;

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
