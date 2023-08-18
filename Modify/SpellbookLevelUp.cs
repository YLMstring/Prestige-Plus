using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Components;
using System.Reflection;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using BlueprintCore.Blueprints.Configurators.Classes;

namespace PrestigePlus.Modify
{
    internal class SpellbookLevelUp
    {
        public static readonly string AccursedWitchName = "AccursedWitch";
        public static readonly string AccursedWitchGuid = "a479ce9c-dca2-4ac5-b638-93828839a4fa";

        public static readonly string AccursedWitchDisplayName = "AccursedWitch.Name";
        public static readonly string AccursedWitchDescription = "AccursedWitch.Description";

        private const string spellupgrade = "Spellupgrade";
        private static readonly string spellupgradeGuid = "{E995A687-EC47-4A29-9AFE-1C2715E0CC24}";

        internal const string SpellupgradeDisplayName = "Spellupgrade.Name";
        private const string SpellupgradeDescription = "Spellupgrade.Description";

        public static void Select()
        {
        FeatureSelectionConfigurator.New(spellupgrade, spellupgradeGuid)
              .SetDisplayName(SpellupgradeDisplayName)
              .SetDescription(SpellupgradeDescription)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .SetAllFeatures(new Blueprint<BlueprintFeatureReference>[] { SpellbookLevelUp.AccursedWitch(), SpellbookLevelUp.Alchemist(), SpellbookLevelUp.AngelfireApostle(), SpellbookLevelUp.Arcanist(), SpellbookLevelUp.ArmagsBlade(), SpellbookLevelUp.Bard(), SpellbookLevelUp.Bloodrager(), SpellbookLevelUp.Cleric(), SpellbookLevelUp.Crossblooded(), SpellbookLevelUp.Crusader(), SpellbookLevelUp.Druid(), SpellbookLevelUp.EldritchFont(), SpellbookLevelUp.EldritchScion(), SpellbookLevelUp.EldritchScoundrel(), SpellbookLevelUp.Empyreal(), SpellbookLevelUp.ExploiterWizard(), SpellbookLevelUp.Feyspeaker(), SpellbookLevelUp.Hunter(), SpellbookLevelUp.Inquisitor(), SpellbookLevelUp.LeyLineGuardianWitch(), SpellbookLevelUp.Magus(), SpellbookLevelUp.MasterSpy(), SpellbookLevelUp.NatureMage(), SpellbookLevelUp.Oracle(), SpellbookLevelUp.Paladin(), SpellbookLevelUp.Ranger(), SpellbookLevelUp.Sage(), SpellbookLevelUp.Shaman(), SpellbookLevelUp.Skald(), SpellbookLevelUp.Sorcerer(), SpellbookLevelUp.SwordSaint(), SpellbookLevelUp.ThassilonianAbjuration(), SpellbookLevelUp.ThassilonianConjuration(), SpellbookLevelUp.ThassilonianEnchantment(), SpellbookLevelUp.ThassilonianEvocation(), SpellbookLevelUp.ThassilonianIllusion(), SpellbookLevelUp.ThassilonianNecromancy(), SpellbookLevelUp.ThassilonianTransmutation(), SpellbookLevelUp.UnletteredArcanist(), SpellbookLevelUp.Warpriest(), SpellbookLevelUp.Witch(), SpellbookLevelUp.Wizard() })
              .Configure();
        }

        public static readonly List<BlueprintReference<BlueprintAbility>> ChannelPositiveHeal =
        new()
        {
          AbilityRefs.ChannelEnergy.Reference,
          AbilityRefs.ChannelEnergyHospitalerHeal.Reference,
          AbilityRefs.ChannelEnergyEmpyrealHeal.Reference,
          AbilityRefs.ChannelEnergyPaladinHeal.Reference,
          AbilityRefs.ShamanLifeSpiritChannelEnergy.Reference,
          AbilityRefs.OracleRevelationChannelAbility.Reference,
          AbilityRefs.WarpriestChannelEnergy.Reference,
          AbilityRefs.HexChannelerChannelEnergy.Reference,
          
        };
        public static BlueprintFeature AccursedWitch()
        {
            return FeatureConfigurator.New(AccursedWitchName, AccursedWitchGuid)
                .SetDisplayName(AccursedWitchDisplayName)
                .SetDescription(AccursedWitchDescription)
                .AddSpellbookLevel(SpellbookRefs.AccursedWitchSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string AlchemistName = "Alchemist";
        public static readonly string AlchemistGuid = "1dd65a2c-3e63-4ebb-baf3-d00c7c555969";

        public static readonly string AlchemistDisplayName = "Alchemist.Name";
        public static readonly string AlchemistDescription = "Alchemist.Description";

        public static BlueprintFeature Alchemist()
        {
            return FeatureConfigurator.New(AlchemistName, AlchemistGuid)
                .SetDisplayName(AlchemistDisplayName)
                .SetDescription(AlchemistDescription)
                .AddSpellbookLevel(SpellbookRefs.AlchemistSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string AngelfireApostleName = "AngelfireApostle";
        public static readonly string AngelfireApostleGuid = "5bee4bb9-2699-4a1a-99a5-9d33f7f96395";

        public static readonly string AngelfireApostleDisplayName = "AngelfireApostle.Name";
        public static readonly string AngelfireApostleDescription = "AngelfireApostle.Description";

        public static BlueprintFeature AngelfireApostle()
        {
            return FeatureConfigurator.New(AngelfireApostleName, AngelfireApostleGuid)
                .SetDisplayName(AngelfireApostleDisplayName)
                .SetDescription(AngelfireApostleDescription)
                .AddSpellbookLevel(SpellbookRefs.AngelfireApostleSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ArcanistName = "Arcanist";
        public static readonly string ArcanistGuid = "82891c6e-8b5f-4875-b97a-fab268fb2210";

        public static readonly string ArcanistDisplayName = "Arcanist.Name";
        public static readonly string ArcanistDescription = "Arcanist.Description";

        public static BlueprintFeature Arcanist()
        {
            return FeatureConfigurator.New(ArcanistName, ArcanistGuid)
                .SetDisplayName(ArcanistDisplayName)
                .SetDescription(ArcanistDescription)
                .AddSpellbookLevel(SpellbookRefs.ArcanistSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ArmagsBladeName = "ArmagsBlade";
        public static readonly string ArmagsBladeGuid = "149e513e-1ef3-413e-ac83-f1b9a846f844";

        public static readonly string ArmagsBladeDisplayName = "ArmagsBlade.Name";
        public static readonly string ArmagsBladeDescription = "ArmagsBlade.Description";

        public static BlueprintFeature ArmagsBlade()
        {
            return FeatureConfigurator.New(ArmagsBladeName, ArmagsBladeGuid)
                .SetDisplayName(ArmagsBladeDisplayName)
                .SetDescription(ArmagsBladeDescription)
                .AddSpellbookLevel(SpellbookRefs.ArmagsBladeSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string BardName = "Bard";
        public static readonly string BardGuid = "e8196afe-fd79-4fa9-aeff-d47e84a0c683";

        public static readonly string BardDisplayName = "Bard.Name";
        public static readonly string BardDescription = "Bard.Description";

        public static BlueprintFeature Bard()
        {
            return FeatureConfigurator.New(BardName, BardGuid)
                .SetDisplayName(BardDisplayName)
                .SetDescription(BardDescription)
                .AddSpellbookLevel(SpellbookRefs.BardSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string BloodragerName = "Bloodrager";
        public static readonly string BloodragerGuid = "459533eb-0d4f-47c4-82b3-fd9c08c08311";

        public static readonly string BloodragerDisplayName = "Bloodrager.Name";
        public static readonly string BloodragerDescription = "Bloodrager.Description";

        public static BlueprintFeature Bloodrager()
        {
            return FeatureConfigurator.New(BloodragerName, BloodragerGuid)
                .SetDisplayName(BloodragerDisplayName)
                .SetDescription(BloodragerDescription)
                .AddSpellbookLevel(SpellbookRefs.BloodragerSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ClericName = "Cleric";
        public static readonly string ClericGuid = "e42dd390-4b4c-4f75-b52b-9f0907a2d2db";

        public static readonly string ClericDisplayName = "Cleric.Name";
        public static readonly string ClericDescription = "Cleric.Description";

        public static BlueprintFeature Cleric()
        {
            return FeatureConfigurator.New(ClericName, ClericGuid)
                .SetDisplayName(ClericDisplayName)
                .SetDescription(ClericDescription)
                .AddSpellbookLevel(SpellbookRefs.ClericSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string CrossbloodedName = "Crossblooded";
        public static readonly string CrossbloodedGuid = "e3fe1442-3d9b-4b41-8895-1ff3e660beb8";

        public static readonly string CrossbloodedDisplayName = "Crossblooded.Name";
        public static readonly string CrossbloodedDescription = "Crossblooded.Description";

        public static BlueprintFeature Crossblooded()
        {
            return FeatureConfigurator.New(CrossbloodedName, CrossbloodedGuid)
                .SetDisplayName(CrossbloodedDisplayName)
                .SetDescription(CrossbloodedDescription)
                .AddSpellbookLevel(SpellbookRefs.CrossbloodedSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string CrusaderName = "Crusader";
        public static readonly string CrusaderGuid = "d8e26da5-28af-4162-81de-abe69d1f0e86";

        public static readonly string CrusaderDisplayName = "Crusader.Name";
        public static readonly string CrusaderDescription = "Crusader.Description";

        public static BlueprintFeature Crusader()
        {
            return FeatureConfigurator.New(CrusaderName, CrusaderGuid)
                .SetDisplayName(CrusaderDisplayName)
                .SetDescription(CrusaderDescription)
                .AddSpellbookLevel(SpellbookRefs.CrusaderSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string DruidName = "Druid";
        public static readonly string DruidGuid = "f1ee2fde-b649-4ec7-b2ea-53d8a11e8751";

        public static readonly string DruidDisplayName = "Druid.Name";
        public static readonly string DruidDescription = "Druid.Description";

        public static BlueprintFeature Druid()
        {
            return FeatureConfigurator.New(DruidName, DruidGuid)
                .SetDisplayName(DruidDisplayName)
                .SetDescription(DruidDescription)
                .AddSpellbookLevel(SpellbookRefs.DruidSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string EldritchFontName = "EldritchFont";
        public static readonly string EldritchFontGuid = "d68b222c-f3ca-4df0-b968-17a7c135e82b";

        public static readonly string EldritchFontDisplayName = "EldritchFont.Name";
        public static readonly string EldritchFontDescription = "EldritchFont.Description";

        public static BlueprintFeature EldritchFont()
        {
            return FeatureConfigurator.New(EldritchFontName, EldritchFontGuid)
                .SetDisplayName(EldritchFontDisplayName)
                .SetDescription(EldritchFontDescription)
                .AddSpellbookLevel(SpellbookRefs.EldritchFontSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string EldritchScionName = "EldritchScion";
        public static readonly string EldritchScionGuid = "9d00b336-6070-4155-a03f-c7d2f0de8c61";

        public static readonly string EldritchScionDisplayName = "EldritchScion.Name";
        public static readonly string EldritchScionDescription = "EldritchScion.Description";

        public static BlueprintFeature EldritchScion()
        {
            return FeatureConfigurator.New(EldritchScionName, EldritchScionGuid)
                .SetDisplayName(EldritchScionDisplayName)
                .SetDescription(EldritchScionDescription)
                .AddSpellbookLevel(SpellbookRefs.EldritchScionSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string EldritchScoundrelName = "EldritchScoundrel";
        public static readonly string EldritchScoundrelGuid = "4aa9ac23-00e2-4bde-aaaf-8b863684dac9";

        public static readonly string EldritchScoundrelDisplayName = "EldritchScoundrel.Name";
        public static readonly string EldritchScoundrelDescription = "EldritchScoundrel.Description";

        public static BlueprintFeature EldritchScoundrel()
        {
            return FeatureConfigurator.New(EldritchScoundrelName, EldritchScoundrelGuid)
                .SetDisplayName(EldritchScoundrelDisplayName)
                .SetDescription(EldritchScoundrelDescription)
                .AddSpellbookLevel(SpellbookRefs.EldritchScoundrelSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string EmpyrealName = "Empyreal";
        public static readonly string EmpyrealGuid = "d24fa235-6cfb-4a39-8d41-60772e4d1193";

        public static readonly string EmpyrealDisplayName = "Empyreal.Name";
        public static readonly string EmpyrealDescription = "Empyreal.Description";

        public static BlueprintFeature Empyreal()
        {
            return FeatureConfigurator.New(EmpyrealName, EmpyrealGuid)
                .SetDisplayName(EmpyrealDisplayName)
                .SetDescription(EmpyrealDescription)
                .AddSpellbookLevel(SpellbookRefs.EmpyrealSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ExploiterWizardName = "ExploiterWizard";
        public static readonly string ExploiterWizardGuid = "8936f0e1-2458-4b1b-81bf-d7322819df02";

        public static readonly string ExploiterWizardDisplayName = "ExploiterWizard.Name";
        public static readonly string ExploiterWizardDescription = "ExploiterWizard.Description";

        public static BlueprintFeature ExploiterWizard()
        {
            return FeatureConfigurator.New(ExploiterWizardName, ExploiterWizardGuid)
                .SetDisplayName(ExploiterWizardDisplayName)
                .SetDescription(ExploiterWizardDescription)
                .AddSpellbookLevel(SpellbookRefs.ExploiterWizardSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string FeyspeakerName = "Feyspeaker";
        public static readonly string FeyspeakerGuid = "41434221-40f3-44e3-ab75-5045674f3e95";

        public static readonly string FeyspeakerDisplayName = "Feyspeaker.Name";
        public static readonly string FeyspeakerDescription = "Feyspeaker.Description";

        public static BlueprintFeature Feyspeaker()
        {
            return FeatureConfigurator.New(FeyspeakerName, FeyspeakerGuid)
                .SetDisplayName(FeyspeakerDisplayName)
                .SetDescription(FeyspeakerDescription)
                .AddSpellbookLevel(SpellbookRefs.FeyspeakerSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string HunterName = "Hunter";
        public static readonly string HunterGuid = "a47c883d-e671-48c5-8048-880151b1f46e";

        public static readonly string HunterDisplayName = "Hunter.Name";
        public static readonly string HunterDescription = "Hunter.Description";

        public static BlueprintFeature Hunter()
        {
            return FeatureConfigurator.New(HunterName, HunterGuid)
                .SetDisplayName(HunterDisplayName)
                .SetDescription(HunterDescription)
                .AddSpellbookLevel(SpellbookRefs.HunterSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string InquisitorName = "Inquisitor";
        public static readonly string InquisitorGuid = "daefc670-6c9a-4487-9ed9-03370d50bc6d";

        public static readonly string InquisitorDisplayName = "Inquisitor.Name";
        public static readonly string InquisitorDescription = "Inquisitor.Description";

        public static BlueprintFeature Inquisitor()
        {
            return FeatureConfigurator.New(InquisitorName, InquisitorGuid)
                .SetDisplayName(InquisitorDisplayName)
                .SetDescription(InquisitorDescription)
                .AddSpellbookLevel(SpellbookRefs.InquisitorSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string LeyLineGuardianWitchName = "LeyLineGuardianWitch";
        public static readonly string LeyLineGuardianWitchGuid = "524523c0-7a2e-492b-a1e9-3439c5745095";

        public static readonly string LeyLineGuardianWitchDisplayName = "LeyLineGuardianWitch.Name";
        public static readonly string LeyLineGuardianWitchDescription = "LeyLineGuardianWitch.Description";

        public static BlueprintFeature LeyLineGuardianWitch()
        {
            return FeatureConfigurator.New(LeyLineGuardianWitchName, LeyLineGuardianWitchGuid)
                .SetDisplayName(LeyLineGuardianWitchDisplayName)
                .SetDescription(LeyLineGuardianWitchDescription)
                .AddSpellbookLevel(SpellbookRefs.LeyLineGuardianWitchSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string MagusName = "Magus";
        public static readonly string MagusGuid = "899cd325-87ae-491a-a4f7-fbdc4b6ecb02";

        public static readonly string MagusDisplayName = "Magus.Name";
        public static readonly string MagusDescription = "Magus.Description";

        public static BlueprintFeature Magus()
        {
            return FeatureConfigurator.New(MagusName, MagusGuid)
                .SetDisplayName(MagusDisplayName)
                .SetDescription(MagusDescription)
                .AddSpellbookLevel(SpellbookRefs.MagusSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string MasterSpyName = "MasterSpy";
        public static readonly string MasterSpyGuid = "b62d1706-8fab-496c-8cef-5ef7a6ae1ec9";

        public static readonly string MasterSpyDisplayName = "MasterSpy.Name";
        public static readonly string MasterSpyDescription = "MasterSpy.Description";

        public static BlueprintFeature MasterSpy()
        {
            return FeatureConfigurator.New(MasterSpyName, MasterSpyGuid)
                .SetDisplayName(MasterSpyDisplayName)
                .SetDescription(MasterSpyDescription)
                .AddSpellbookLevel(SpellbookRefs.MasterSpySpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string NatureMageName = "NatureMage";
        public static readonly string NatureMageGuid = "564c20e7-a440-448c-89eb-2f6f39862927";

        public static readonly string NatureMageDisplayName = "NatureMage.Name";
        public static readonly string NatureMageDescription = "NatureMage.Description";

        public static BlueprintFeature NatureMage()
        {
            return FeatureConfigurator.New(NatureMageName, NatureMageGuid)
                .SetDisplayName(NatureMageDisplayName)
                .SetDescription(NatureMageDescription)
                .AddSpellbookLevel(SpellbookRefs.NatureMageSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string OracleName = "Oracle";
        public static readonly string OracleGuid = "b7c50ca3-5f49-440b-8451-8abb2e87baad";

        public static readonly string OracleDisplayName = "Oracle.Name";
        public static readonly string OracleDescription = "Oracle.Description";

        public static BlueprintFeature Oracle()
        {
            return FeatureConfigurator.New(OracleName, OracleGuid)
                .SetDisplayName(OracleDisplayName)
                .SetDescription(OracleDescription)
                .AddSpellbookLevel(SpellbookRefs.OracleSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string PaladinName = "Paladin";
        public static readonly string PaladinGuid = "72184ecb-bfe7-4c43-8ef2-871e8efc7cbd";

        public static readonly string PaladinDisplayName = "Paladin.Name";
        public static readonly string PaladinDescription = "Paladin.Description";

        public static BlueprintFeature Paladin()
        {
            return FeatureConfigurator.New(PaladinName, PaladinGuid)
                .SetDisplayName(PaladinDisplayName)
                .SetDescription(PaladinDescription)
                .AddSpellbookLevel(SpellbookRefs.PaladinSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string RangerName = "Ranger";
        public static readonly string RangerGuid = "70269172-e282-4868-8458-99264eefc903";

        public static readonly string RangerDisplayName = "Ranger.Name";
        public static readonly string RangerDescription = "Ranger.Description";

        public static BlueprintFeature Ranger()
        {
            return FeatureConfigurator.New(RangerName, RangerGuid)
                .SetDisplayName(RangerDisplayName)
                .SetDescription(RangerDescription)
                .AddSpellbookLevel(SpellbookRefs.RangerSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string SageName = "Sage";
        public static readonly string SageGuid = "8a869778-692a-4e03-bd02-05862c852371";

        public static readonly string SageDisplayName = "Sage.Name";
        public static readonly string SageDescription = "Sage.Description";

        public static BlueprintFeature Sage()
        {
            return FeatureConfigurator.New(SageName, SageGuid)
                .SetDisplayName(SageDisplayName)
                .SetDescription(SageDescription)
                .AddSpellbookLevel(SpellbookRefs.SageSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ShamanName = "Shaman";
        public static readonly string ShamanGuid = "09fb6763-67af-4f21-b2c4-5fe70d997dc1";

        public static readonly string ShamanDisplayName = "Shaman.Name";
        public static readonly string ShamanDescription = "Shaman.Description";

        public static BlueprintFeature Shaman()
        {
            return FeatureConfigurator.New(ShamanName, ShamanGuid)
                .SetDisplayName(ShamanDisplayName)
                .SetDescription(ShamanDescription)
                .AddSpellbookLevel(SpellbookRefs.ShamanSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string SkaldName = "Skald";
        public static readonly string SkaldGuid = "8fc5f71a-cf9d-4e84-8fab-892d71928906";

        public static readonly string SkaldDisplayName = "Skald.Name";
        public static readonly string SkaldDescription = "Skald.Description";

        public static BlueprintFeature Skald()
        {
            return FeatureConfigurator.New(SkaldName, SkaldGuid)
                .SetDisplayName(SkaldDisplayName)
                .SetDescription(SkaldDescription)
                .AddSpellbookLevel(SpellbookRefs.SkaldSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string SorcererName = "Sorcerer";
        public static readonly string SorcererGuid = "442f3e70-fa83-4836-bebb-e159a4a6990c";

        public static readonly string SorcererDisplayName = "Sorcerer.Name";
        public static readonly string SorcererDescription = "Sorcerer.Description";

        public static BlueprintFeature Sorcerer()
        {

            return FeatureConfigurator.New(SorcererName, SorcererGuid)
                .SetDisplayName(SorcererDisplayName)
                .SetDescription(SorcererDescription)
                .AddSpellbookLevel(SpellbookRefs.SorcererSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string SwordSaintName = "SwordSaint";
        public static readonly string SwordSaintGuid = "26ad8592-0176-449d-bf85-df743b8d2289";

        public static readonly string SwordSaintDisplayName = "SwordSaint.Name";
        public static readonly string SwordSaintDescription = "SwordSaint.Description";

        public static BlueprintFeature SwordSaint()
        {
            return FeatureConfigurator.New(SwordSaintName, SwordSaintGuid)
                .SetDisplayName(SwordSaintDisplayName)
                .SetDescription(SwordSaintDescription)
                .AddSpellbookLevel(SpellbookRefs.SwordSaintSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianAbjurationName = "ThassilonianAbjuration";
        public static readonly string ThassilonianAbjurationGuid = "1a2b2fcd-1f14-446d-8383-7cb4c79bb28a";

        public static readonly string ThassilonianAbjurationDisplayName = "ThassilonianAbjuration.Name";
        public static readonly string ThassilonianAbjurationDescription = "ThassilonianAbjuration.Description";

        public static BlueprintFeature ThassilonianAbjuration()
        {
            return FeatureConfigurator.New(ThassilonianAbjurationName, ThassilonianAbjurationGuid)
                .SetDisplayName(ThassilonianAbjurationDisplayName)
                .SetDescription(ThassilonianAbjurationDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianAbjurationSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianConjurationName = "ThassilonianConjuration";
        public static readonly string ThassilonianConjurationGuid = "8bc9adfd-4e1e-4aee-bcb0-39bdcfac56c1";

        public static readonly string ThassilonianConjurationDisplayName = "ThassilonianConjuration.Name";
        public static readonly string ThassilonianConjurationDescription = "ThassilonianConjuration.Description";

        public static BlueprintFeature ThassilonianConjuration()
        {
            return FeatureConfigurator.New(ThassilonianConjurationName, ThassilonianConjurationGuid)
                .SetDisplayName(ThassilonianConjurationDisplayName)
                .SetDescription(ThassilonianConjurationDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianConjurationSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianEnchantmentName = "ThassilonianEnchantment";
        public static readonly string ThassilonianEnchantmentGuid = "487af3ca-25e9-4a4f-acc1-82d542d0865e";

        public static readonly string ThassilonianEnchantmentDisplayName = "ThassilonianEnchantment.Name";
        public static readonly string ThassilonianEnchantmentDescription = "ThassilonianEnchantment.Description";

        public static BlueprintFeature ThassilonianEnchantment()
        {
            return FeatureConfigurator.New(ThassilonianEnchantmentName, ThassilonianEnchantmentGuid)
                .SetDisplayName(ThassilonianEnchantmentDisplayName)
                .SetDescription(ThassilonianEnchantmentDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianEnchantmentSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianEvocationName = "ThassilonianEvocation";
        public static readonly string ThassilonianEvocationGuid = "2121dbbc-0cca-41d9-873a-295163b5e973";

        public static readonly string ThassilonianEvocationDisplayName = "ThassilonianEvocation.Name";
        public static readonly string ThassilonianEvocationDescription = "ThassilonianEvocation.Description";

        public static BlueprintFeature ThassilonianEvocation()
        {
            return FeatureConfigurator.New(ThassilonianEvocationName, ThassilonianEvocationGuid)
                .SetDisplayName(ThassilonianEvocationDisplayName)
                .SetDescription(ThassilonianEvocationDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianEvocationSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianIllusionName = "ThassilonianIllusion";
        public static readonly string ThassilonianIllusionGuid = "e820f6c0-bd7f-4bb4-808a-337ceebdda58";

        public static readonly string ThassilonianIllusionDisplayName = "ThassilonianIllusion.Name";
        public static readonly string ThassilonianIllusionDescription = "ThassilonianIllusion.Description";

        public static BlueprintFeature ThassilonianIllusion()
        {
            return FeatureConfigurator.New(ThassilonianIllusionName, ThassilonianIllusionGuid)
                .SetDisplayName(ThassilonianIllusionDisplayName)
                .SetDescription(ThassilonianIllusionDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianIllusionSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianNecromancyName = "ThassilonianNecromancy";
        public static readonly string ThassilonianNecromancyGuid = "5b377cef-b636-40bc-bb5a-16c24ab11cfe";

        public static readonly string ThassilonianNecromancyDisplayName = "ThassilonianNecromancy.Name";
        public static readonly string ThassilonianNecromancyDescription = "ThassilonianNecromancy.Description";

        public static BlueprintFeature ThassilonianNecromancy()
        {
            return FeatureConfigurator.New(ThassilonianNecromancyName, ThassilonianNecromancyGuid)
                .SetDisplayName(ThassilonianNecromancyDisplayName)
                .SetDescription(ThassilonianNecromancyDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianNecromancySpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string ThassilonianTransmutationName = "ThassilonianTransmutation";
        public static readonly string ThassilonianTransmutationGuid = "ddd3c4e8-928a-400c-9173-65626bd35239";

        public static readonly string ThassilonianTransmutationDisplayName = "ThassilonianTransmutation.Name";
        public static readonly string ThassilonianTransmutationDescription = "ThassilonianTransmutation.Description";

        public static BlueprintFeature ThassilonianTransmutation()
        {
            return FeatureConfigurator.New(ThassilonianTransmutationName, ThassilonianTransmutationGuid)
                .SetDisplayName(ThassilonianTransmutationDisplayName)
                .SetDescription(ThassilonianTransmutationDescription)
                .AddSpellbookLevel(SpellbookRefs.ThassilonianTransmutationSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string UnletteredArcanistName = "UnletteredArcanist";
        public static readonly string UnletteredArcanistGuid = "0fa3c7ae-3f98-471a-919e-124260ba7d5c";

        public static readonly string UnletteredArcanistDisplayName = "UnletteredArcanist.Name";
        public static readonly string UnletteredArcanistDescription = "UnletteredArcanist.Description";

        public static BlueprintFeature UnletteredArcanist()
        {
            return FeatureConfigurator.New(UnletteredArcanistName, UnletteredArcanistGuid)
                .SetDisplayName(UnletteredArcanistDisplayName)
                .SetDescription(UnletteredArcanistDescription)
                .AddSpellbookLevel(SpellbookRefs.UnletteredArcanistSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string WarpriestName = "Warpriest";
        public static readonly string WarpriestGuid = "568a5ec2-8d3f-4169-96af-0bfa38972b15";

        public static readonly string WarpriestDisplayName = "Warpriest.Name";
        public static readonly string WarpriestDescription = "Warpriest.Description";

        public static BlueprintFeature Warpriest()
        {
            return FeatureConfigurator.New(WarpriestName, WarpriestGuid)
                .SetDisplayName(WarpriestDisplayName)
                .SetDescription(WarpriestDescription)
                .AddSpellbookLevel(SpellbookRefs.WarpriestSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string WitchName = "Witch";
        public static readonly string WitchGuid = "dce771a5-fbd4-454c-a340-12b9edf0e120";

        public static readonly string WitchDisplayName = "Witch.Name";
        public static readonly string WitchDescription = "Witch.Description";

        public static BlueprintFeature Witch()
        {
            return FeatureConfigurator.New(WitchName, WitchGuid)
                .SetDisplayName(WitchDisplayName)
                .SetDescription(WitchDescription)
                .AddSpellbookLevel(SpellbookRefs.WitchSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }

        public static readonly string WizardName = "Wizard";
        public static readonly string WizardGuid = "1772a025-5160-43c2-992c-441ce929a5ff";

        public static readonly string WizardDisplayName = "Wizard.Name";
        public static readonly string WizardDescription = "Wizard.Description";

        public static BlueprintFeature Wizard()
        {
            return FeatureConfigurator.New(WizardName, WizardGuid)
                .SetDisplayName(WizardDisplayName)
                .SetDescription(WizardDescription)
                .AddSpellbookLevel(SpellbookRefs.WizardSpellbook.ToString())
                .SetRanks(10)
                .Configure();
        }
    }

}

