using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using PrestigePlus.Blueprint.MythicGrapple;
using Kingmaker.RuleSystem.Rules;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Constable
    {
        private const string ArchetypeName = "Constable";
        private static readonly string ArchetypeGuid = "{A580225F-2DE4-41E1-BF4C-19442C82981D}";
        internal const string ArchetypeDisplayName = "Constable.Name";
        private const string ArchetypeDescription = "Constable.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.CavalierClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.CavalierMountSelection.ToString())
            .SetRemoveFeaturesEntry(3, FeatureRefs.CavalierCharge.ToString())
            .SetRemoveFeaturesEntry(5, FeatureRefs.CavalierBanner.ToString())
            .SetRemoveFeaturesEntry(11, FeatureRefs.CavalierMightyCharge.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.CavalierBannerGreater.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.CavalierSupremeCharge.ToString())
            .AddToAddFeatures(1, CreateApprehend())
            .AddToAddFeatures(2, CreateApprehend2())
            .AddToAddFeatures(4, FeatureRefs.HunterWoodlandStride.ToString())
            .AddToAddFeatures(7, Apprehend2Guid)
            .AddToAddFeatures(12, Apprehend2Guid)
            .AddToAddFeatures(17, Apprehend2Guid)
              .AddToClassSkills(StatType.SkillAthletics)
              .AddToClassSkills(StatType.SkillMobility)
              .AddToClassSkills(StatType.SkillPerception)
              .AddToClassSkills(StatType.SkillPersuasion)
              .SetReplaceClassSkills(true)
              .Configure();
        }

        private const string Apprehend = "Constable.Apprehend";
        private static readonly string ApprehendGuid = "{527A8ECD-941B-4D2F-89F2-FC53F3206BDF}";
        internal const string ApprehendDisplayName = "ConstableApprehend.Name";
        private const string ApprehendDescription = "ConstableApprehend.Description";

        public static BlueprintFeature CreateApprehend()
        {
            return FeatureConfigurator.New(Apprehend, ApprehendGuid)
              .SetDisplayName(ApprehendDisplayName)
              .SetDescription(ApprehendDescription)
              .SetIsClassFeature(true)
              .AddFacts(new() { FeatureRefs.ImprovedUnarmedStrike.ToString(), AerialAssault.StyleActivatableAbility3Guid })
              .Configure();
        }

        private const string Apprehend2 = "Constable.Apprehend2";
        private static readonly string Apprehend2Guid = "{80C96103-C78B-4FB4-9EC0-8A31C9346A36}";
        public static BlueprintFeature CreateApprehend2()
        {
            return FeatureConfigurator.New(Apprehend2, Apprehend2Guid)
              .SetDisplayName(ApprehendDisplayName)
              .SetDescription(ApprehendDescription)
              .SetIsClassFeature(true)
              .SetRanks(5)
              .AddContextStatBonus(StatType.SkillPerception, ContextValues.Rank())
              .AddCMBBonusForManeuver(checkFact: false, maneuvers: new CombatManeuver[] { CombatManeuver.Trip, CombatManeuver.Disarm, CombatManeuver.Grapple }, value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(Apprehend2Guid))
              .Configure();
        }

        
    }
}
