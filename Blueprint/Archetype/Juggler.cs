using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.GrappleFeat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Juggler
    {
        private const string ArchetypeName = "Juggler";
        private static readonly string ArchetypeGuid = "{40A8EEC5-5AA1-4E24-BE19-5A0AA4E4069D}";
        internal const string ArchetypeDisplayName = "Juggler.Name";
        private const string ArchetypeDescription = "Juggler.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.BardClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .AddToRemoveFeatures(1, FeatureRefs.BardProficiencies.ToString(), FeatureRefs.BardicKnowledge.ToString())
            .AddToRemoveFeatures(2, FeatureRefs.BardTalent.ToString(), FeatureRefs.BardWellVersed.ToString())
            .AddToRemoveFeatures(5, FeatureRefs.BardLoreMaster.ToString())
            .AddToRemoveFeatures(12, FeatureRefs.SoothingPerformanceFeature.ToString())
            .AddToAddFeatures(1, FeatureRefs.DeflectArrows.ToString(), CreateProficiencies())
            .AddToAddFeatures(2, FeatureRefs.Evasion.ToString(), CreateCombatJuggling())
            .AddToAddFeatures(5, CreateSnatchArrows())
            .AddToAddFeatures(11, CreateFastReactions())
            .AddToAddFeatures(12, FeatureRefs.ImprovedEvasion.ToString())
            .AddToAddFeatures(17, FastReactionsGuid)
              .Configure();
        }

        private const string Proficiencies = "Juggler.Proficiencies";
        private static readonly string ProficienciesGuid = "{3486544B-AD52-436A-A0BE-E5B8C70FDE47}";
        internal const string ProficienciesDisplayName = "JugglerProficiencies.Name";
        private const string ProficienciesDescription = "JugglerProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            var assProficiencies = FeatureRefs.BardProficiencies.Reference.Get();
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent(assProficiencies.GetComponent<ArcaneArmorProficiency>())
              .AddFacts(new() { FeatureRefs.LightArmorProficiency.ToString(), FeatureRefs.SimpleWeaponProficiency.ToString() })
              .AddProficiencies(
                weaponProficiencies:
                  new WeaponCategory[]
                  {
              WeaponCategory.ThrowingAxe,
                  })
              .Configure();
        }

        private const string FastReactions = "Juggler.FastReactions";
        public static readonly string FastReactionsGuid = "{A9173F19-2238-4D6A-8789-97A00DECDFF4}";

        private const string FastReactionsBuff = "Juggler.FastReactionsBuff";
        public static readonly string FastReactionsBuffGuid = "{CAD15218-FAF3-4FB2-A9D3-F466FA599FA3}";

        internal const string FastReactionsDisplayName = "JugglerFastReactions.Name";
        private const string FastReactionsDescription = "JugglerFastReactions.Description";
        private static BlueprintFeature CreateFastReactions()
        {
            var icon = FeatureRefs.WarpriestFervorQuickenCast.Reference.Get().Icon;

            BuffConfigurator.New(FastReactionsBuff, FastReactionsBuffGuid)
              .SetDisplayName(FastReactionsDisplayName)
              .SetDescription(FastReactionsDescription)
              .SetIcon(icon)
              .SetRanks(10)
              //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            return FeatureConfigurator.New(FastReactions, FastReactionsGuid)
              .SetDisplayName(FastReactionsDisplayName)
              .SetDescription(FastReactionsDescription)
              .SetIcon(icon)
              .SetRanks(2)
              .Configure();
        }

        private const string CombatJuggling = "Juggler.CombatJuggling";
        public static readonly string CombatJugglingGuid = "{14D7C55D-05D5-44EB-96BB-4E0A79C4FB39}";

        internal const string CombatJugglingDisplayName = "JugglerCombatJuggling.Name";
        private const string CombatJugglingDescription = "JugglerCombatJuggling.Description";
        private static BlueprintFeature CreateCombatJuggling()
        {
            var icon = FeatureRefs.WarpriestFervorQuickenCast.Reference.Get().Icon;

            return FeatureConfigurator.New(CombatJuggling, CombatJugglingGuid)
              .SetDisplayName(CombatJugglingDisplayName)
              .SetDescription(CombatJugglingDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string SnatchArrows = "Juggler.SnatchArrows";
        public static readonly string SnatchArrowsGuid = "{91D15A79-7104-4FCF-9FBD-4641B42B05E0}";

        internal const string SnatchArrowsDisplayName = "JugglerSnatchArrows.Name";
        private const string SnatchArrowsDescription = "JugglerSnatchArrows.Description";
        private static BlueprintFeature CreateSnatchArrows()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            return FeatureConfigurator.New(SnatchArrows, SnatchArrowsGuid, FeatureGroup.Feat)
              .SetDisplayName(SnatchArrowsDisplayName)
              .SetDescription(SnatchArrowsDescription)
              .SetIcon(icon)
              .AddPrerequisiteStatValue(StatType.Dexterity, 15)
              .AddPrerequisiteFeature(FeatureRefs.DeflectArrows.ToString())
              .AddPrerequisiteFeature(FeatureRefs.ImprovedUnarmedStrike.ToString())
              .AddToGroups(FeatureGroup.CombatFeat)
              .Configure();
        }

        private const string DeflectArrowsMythic = "Juggler.DeflectArrowsMythic";
        private static readonly string DeflectArrowsMythicGuid = "{56F1B86D-A970-4B57-A4C7-1AF95292424B}";

        internal const string DeflectArrowsMythicDisplayName = "JugglerDeflectArrowsMythic.Name";
        private const string DeflectArrowsMythicDescription = "JugglerDeflectArrowsMythic.Description";
        public static BlueprintFeature CreateDeflectArrowsMythic()
        {
            var icon = FeatureRefs.DeflectArrows.Reference.Get().Icon;

            return FeatureConfigurator.New(DeflectArrowsMythic, DeflectArrowsMythicGuid, FeatureGroup.MythicFeat)
              .SetDisplayName(DeflectArrowsMythicDisplayName)
              .SetDescription(DeflectArrowsMythicDescription)
              .SetIcon(icon)
              .AddPrerequisiteFeature(FeatureRefs.DeflectArrows.ToString())
              .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
              .Configure();
        }
    }
}
