using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class Emissary
    {
        private const string ArchetypeName = "Emissary";
        public static readonly string ArchetypeGuid = "{6FAB3818-B285-41F2-BB34-08ED040D493E}";
        internal const string ArchetypeDisplayName = "Emissary.Name";
        private const string ArchetypeDescription = "Emissary.Description";
        public static void Configure()
        {
            //"TrickRiding": "34d1bd97-971d-44d6-be78-e91e79fdedbd",
            //"MountedSkirmisher": "d3886adc-c849-4733-a1cb-a9b787b49576",
            
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.CavalierClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.CavalierTacticianFeatSelection.ToString(), FeatureRefs.CavalierTacticianFeature.ToString(), FeatureRefs.CavalierProficiencies.ToString())
            .SetRemoveFeaturesEntry(5, FeatureRefs.CavalierBanner.ToString())
            .SetRemoveFeaturesEntry(9, FeatureSelectionRefs.CavalierTacticianFeatSelection.ToString(), FeatureRefs.CavalierTacticianGreater.ToString())
            .SetRemoveFeaturesEntry(14, FeatureRefs.CavalierBannerGreater.ToString())
            .SetRemoveFeaturesEntry(20, FeatureRefs.CavalierSupremeCharge.ToString())
            .AddToAddFeatures(1, FeatureRefs.MountedCombat.ToString(), FeatureRefs.RangerProficiencies.ToString())
            .AddToAddFeatures(5, CreateBattlefieldAgility())
            .AddToAddFeatures(9, "34d1bd97-971d-44d6-be78-e91e79fdedbd")
            .AddToAddFeatures(14, "d3886adc-c849-4733-a1cb-a9b787b49576")
            .AddToAddFeatures(20, CreateErraticCharge())
              .Configure();
        }

        private const string BattlefieldAgility = "Emissary.BattlefieldAgility";
        private static readonly string BattlefieldAgilityGuid = "{431945F2-017F-4AE7-970E-F49E3B8A01C1}";

        internal const string BattlefieldAgilityDisplayName = "EmissaryBattlefieldAgility.Name";
        private const string BattlefieldAgilityDescription = "EmissaryBattlefieldAgility.Description";
        private static BlueprintFeature CreateBattlefieldAgility()
        {
            var icon = FeatureRefs.MountedShield.Reference.Get().Icon;

            return FeatureConfigurator.New(BattlefieldAgility, BattlefieldAgilityGuid)
              .SetDisplayName(BattlefieldAgilityDisplayName)
              .SetDescription(BattlefieldAgilityDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.Mobility.ToString() })
              .AddFeatureToPet(FeatureRefs.Mobility.ToString(), PetType.AnimalCompanion)
              .Configure();
        }

        private const string ErraticCharge = "Emissary.ErraticCharge";
        private static readonly string ErraticChargeGuid = "{01C8B159-E69B-4468-AC45-4E0ECDE99E00}";

        private const string ErraticChargeBuff = "Emissary.ErraticChargeBuff";
        private static readonly string ErraticChargeGuidBuff = "{78727615-7D5A-4FD9-8C07-4632492D131B}";

        internal const string ErraticChargeDisplayName = "EmissaryErraticCharge.Name";
        private const string ErraticChargeDescription = "EmissaryErraticCharge.Description";
        private static BlueprintFeature CreateErraticCharge()
        {
            var icon = FeatureRefs.SpiritedCharge.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ErraticChargeBuff, ErraticChargeGuidBuff)
              .SetDisplayName(ErraticChargeDisplayName)
              .SetDescription(ErraticChargeDescription)
              .SetIcon(icon)
              .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Pounce)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            return FeatureConfigurator.New(ErraticCharge, ErraticChargeGuid)
              .SetDisplayName(ErraticChargeDisplayName)
              .SetDescription(ErraticChargeDescription)
              .SetIcon(icon)
              .AddBuffExtraEffects(checkedBuff: BuffRefs.MountedBuff.ToString(), extraEffectBuff: Buff1)
              .Configure();
        }
    }
}
