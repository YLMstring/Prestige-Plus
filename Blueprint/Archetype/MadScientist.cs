using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.StoryEx;
using Kingmaker.ElementsSystem;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.CustomAction.ClassRelated;
using Kingmaker.Enums;
using Kingmaker.EntitySystem.Stats;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class MadScientist
    {
        private const string ArchetypeName = "MadScientist";
        private static readonly string ArchetypeGuid = "{3FBF33FD-7A5D-419E-A80F-63378F4E956D}";
        internal const string ArchetypeDisplayName = "MadScientist.Name";
        private const string ArchetypeDescription = "MadScientist.Description";
        public static void Configure()
        {
            ProgressionConfigurator.For(ProgressionRefs.AlchemistProgression)
                .AddToUIGroups(new Blueprint<BlueprintFeatureBaseReference>[] { MadMutagenGuid, MadGeniusGuid, FeatureSelectionRefs.DiscoverySelection.ToString() })
                .Configure();

            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.AlchemistClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureSelectionRefs.DiscoverySelection.ToString())
            .SetRemoveFeaturesEntry(4, FeatureSelectionRefs.DiscoverySelection.ToString())
            .AddToAddFeatures(2, CreateMadGenius())
            .AddToAddFeatures(4, MadMutagenFeat())
              .Configure();
        }

        private const string MadMutagen = "MadScientist.MadMutagen";
        private static readonly string MadMutagenGuid = "{29FD0859-2710-4ADF-A723-30B80FB3CCC2}";

        private const string MadMutagenAbility = "MadScientist.MadMutagenAbility";
        private static readonly string MadMutagenAbilityGuid = "{1C43C161-7537-4116-86B4-84D5E58F8D62}";

        internal const string MadMutagenDisplayName = "MadScientistMadMutagen.Name";
        private const string MadMutagenDescription = "MadScientistMadMutagen.Description";

        private const string MadMutagenBuff = "MadScientist.MadMutagenBuff";
        private static readonly string MadMutagenBuffGuid = "{80A57B8A-5B05-4544-87F2-544FE4EE59FA}";
        public static BlueprintFeature MadMutagenFeat()
        {
            var icon = FeatureRefs.GrandCognatogenFeature.Reference.Get().Icon;

            var action = ActionsBuilder.New()
                        .DealDamageToAbility(StatType.Wisdom, ContextDice.Value(Kingmaker.RuleSystem.DiceType.D4, 1, 0))
                        .RestoreResource(AbilityResourceRefs.MutagenResource.ToString(), 1)
                        .Build();

            var Buff = BuffConfigurator.New(MadMutagenBuff, MadMutagenBuffGuid)
             .SetDisplayName(MadMutagenDisplayName)
             .SetDescription(MadMutagenDescription)
             .SetIcon(icon)
             .AddBuffActions(deactivated: action)
             .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Prolong)
             .Configure();

            var ability = AbilityConfigurator.New(MadMutagenAbility, MadMutagenAbilityGuid)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Potion)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(Buff, ContextDuration.Fixed(1, Kingmaker.UnitLogic.Mechanics.DurationRate.Hours)).Build())
                .SetDisplayName(MadMutagenDisplayName)
                .SetDescription(MadMutagenDescription)
                .SetIcon(icon)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Special)
                .AddAbilityCasterInCombat(true)
                .Configure();

            return FeatureConfigurator.New(MadMutagen, MadMutagenGuid)
              .SetDisplayName(MadMutagenDisplayName)
              .SetDescription(MadMutagenDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string MadGenius = "MadScientist.MadGenius";
        private static readonly string MadGeniusGuid = "{E8E28770-D72F-4D66-B1F8-378D80C0632D}";

        internal const string MadGeniusDisplayName = "MadScientistMadGenius.Name";
        private const string MadGeniusDescription = "MadScientistMadGenius.Description";

        private static BlueprintFeature CreateMadGenius()
        {
            var icon = AbilityRefs.JoyfulRapture.Reference.Get().Icon;

            return FeatureConfigurator.New(MadGenius, MadGeniusGuid)
              .SetDisplayName(MadGeniusDisplayName)
              .SetDescription(MadGeniusDescription)
              .SetIcon(icon)
              .AddComponent<MadScientistPrep>()
              .Configure();
        }
    }
}
