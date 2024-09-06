using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.Archetype;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.FactLogic;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class FatedChampion
    {
        private const string ArchetypeName = "FatedChampion";
        private static readonly string ArchetypeGuid = "{6848CEF0-701C-45BF-A43E-A58E403579DD}";
        internal const string ArchetypeDisplayName = "FatedChampion.Name";
        private const string ArchetypeDescription = "FatedChampion.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.SkaldClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(2, FeatureRefs.SkaldWellVersed.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.DirgeOfDoom.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.MasterSkald.ToString())
            .AddToAddFeatures(2, CreateWatcher())
            .AddToAddFeatures(10, CreateShieldForesight())
            .AddToAddFeatures(20, CreateNotThisDay())
              .Configure();
        }

        private const string ShieldForesight = "FatedChampion.ShieldForesight";
        public static readonly string ShieldForesightGuid = "{09C3C05C-D642-4A43-97A3-4B0F065D235C}";

        private const string ShieldForesightFeat = "FatedChampion.UseShieldForeFeat";
        public static readonly string ShieldForesightFeatGuid = "{3E41D736-B7E9-4978-8FFE-5EF37034DFCE}";

        private const string ShieldForesightBuff = "FatedChampion.ShieldForesightBuff";
        private static readonly string ShieldForesightBuffGuid = "{22F69E4B-DFCF-4A35-B16E-AD588A3A6CB8}";

        internal const string ShieldForesightDisplayName = "FatedChampionShieldForesight.Name";
        private const string ShieldForesightDescription = "FatedChampionShieldForesight.Description";

        public static BlueprintFeature CreateShieldForesight()
        {
            var icon = AbilityRefs.Foresight.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(ShieldForesightFeat, ShieldForesightFeatGuid)
              .SetDisplayName(ShieldForesightDisplayName)
              .SetDescription(ShieldForesightDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(spellDescriptor: SpellDescriptor.Fear, value: 5)
              .Configure();

            var Buff = BuffConfigurator.New(ShieldForesightBuff, ShieldForesightBuffGuid)
             .SetDisplayName(ShieldForesightDisplayName)
             .SetDescription(ShieldForesightDescription)
             .SetIcon(icon)
             .AddFacts(new() { feat })
             .AddBuffDescriptorImmunity(false, SpellDescriptor.Fear)
             .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Fear)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .Configure();

            return FeatureConfigurator.New(ShieldForesight, ShieldForesightGuid)
              .SetDisplayName(ShieldForesightDisplayName)
              .SetDescription(ShieldForesightDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddBuffExtraEffects(BuffRefs.InspiredRageBuff.ToString(), extraEffectBuff: Buff)
              .Configure();
        }

        private const string Watcher = "FatedChampion.Watcher";
        private static readonly string WatcherGuid = "{2D8C398F-84E9-492F-854E-B99154755692}";

        internal const string WatcherDisplayName = "FatedChampionWatcher.Name";
        private const string WatcherDescription = "FatedChampionWatcher.Description";

        private static BlueprintFeature CreateWatcher()
        {
            var icon = AbilityRefs.Web.Reference.Get().Icon;

            return FeatureConfigurator.New(Watcher, WatcherGuid)
              .SetDisplayName(WatcherDisplayName)
              .SetDescription(WatcherDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.Initiative, ContextValues.Rank(), ModifierDescriptor.Insight)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { CharacterClassRefs.SkaldClass.ToString() }).WithDiv2Progression())
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string NotThisDay = "FatedChampion.NotThisDay";
        public static readonly string NotThisDayGuid = "{42886C52-AB8C-47E3-B253-2F1A45D1423A}";

        internal const string NotThisDayDisplayName = "FatedChampionNotThisDay.Name";
        private const string NotThisDayDescription = "FatedChampionNotThisDay.Description";

        private static readonly string DefensiveGuid = "3967B46C-99CA-41E9-B7A2-96644EED8C96";
        public static BlueprintFeature CreateNotThisDay()
        {
            var icon = FeatureRefs.SlipperyMind.Reference.Get().Icon;

            return FeatureConfigurator.New(NotThisDay, NotThisDayGuid)
              .SetDisplayName(NotThisDayDisplayName)
              .SetDescription(NotThisDayDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.SlipperyMind.ToString(), DefensiveGuid })
              .Configure();
        }
    }
}
