using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Classes;

namespace PrestigePlus.Modify
{            ///FeatureReplaceSpellbookConfigurator.New(SorcererName, SorcererGuid)
             ///.AddSpellbook

    internal class SpellbookReplace
    {
        public static readonly string AccursedWitchName = "2AccursedWitch";
        public static readonly string AccursedWitchGuid = "a479ce9c-dca2-4ac5-b638-93828839a4fa";

        public static readonly string AccursedWitchDisplayName = "AccursedWitch.Name";
        public static readonly string AccursedWitchDescription = "AccursedWitch.Description";

        private const string spellupgrade = "Spellupgrade2";
        public static readonly string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";

        internal const string SpellupgradeDisplayName = "Spellupgrade.Name";
        private const string SpellupgradeDescription = "Spellupgrade.Description";

        public static void Select()
        {
            FeatureSelectionConfigurator.New(spellupgrade, spellupgradeGuid)
                  .SetDisplayName(SpellupgradeDisplayName)
                  .SetDescription(SpellupgradeDescription)
                  .SetIgnorePrerequisites(false)
                  .SetObligatory(false)
                  .SetAllFeatures(new Blueprint<BlueprintFeatureReference>[] { AccursedWitch(), Alchemist(), AngelfireApostle(), Arcanist(), ArmagsBlade(), Bard(), Bloodrager(), Cleric(), Crossblooded(), Crusader(), Druid(), EldritchFont(), EldritchScion(), EldritchScoundrel(), Empyreal(), ExploiterWizard(), Feyspeaker(), Hunter(), Inquisitor(), LeyLineGuardianWitch(), Magus(), MasterSpy(), NatureMage(), Oracle(), Paladin(), Ranger(), Sage(), Shaman(), Skald(), Sorcerer(), SwordSaint(), ThassilonianAbjuration(), ThassilonianConjuration(), ThassilonianEnchantment(), ThassilonianEvocation(), ThassilonianIllusion(), ThassilonianNecromancy(), ThassilonianTransmutation(), UnletteredArcanist(), Warpriest(), Witch(), Wizard(), DreadKnight(), ClawOfTheFalseWyrm(), SilverChampion(), RavenerHunter(), LivingScripture(), SkulkingHunter(), SwornOfTheEldest(), ArrowsongMinstrel(), Antipaladin(), ChannelerOfTheUnknown(), Myrmidarch() })
                  .Configure(delayed: true);
        }

        public static BlueprintFeatureReplaceSpellbook AccursedWitch()
        {
            return FeatureReplaceSpellbookConfigurator.New(AccursedWitchName, AccursedWitchGuid)
                .SetDisplayName(AccursedWitchDisplayName)
                .SetDescription(AccursedWitchDescription)
                .SetSpellbook(SpellbookRefs.AccursedWitchSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.AccursedWitchSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string AlchemistName = "2Alchemist";
        public static readonly string AlchemistGuid = "1dd65a2c-3e63-4ebb-baf3-d00c7c555969";

        public static readonly string AlchemistDisplayName = "Alchemist.Name";
        public static readonly string AlchemistDescription = "Alchemist.Description";

        public static BlueprintFeatureReplaceSpellbook Alchemist()
        {
            return FeatureReplaceSpellbookConfigurator.New(AlchemistName, AlchemistGuid)
                .SetDisplayName(AlchemistDisplayName)
                .SetDescription(AlchemistDescription)
                .SetSpellbook(SpellbookRefs.AlchemistSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.AlchemistSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string AngelfireApostleName = "2AngelfireApostle";
        public static readonly string AngelfireApostleGuid = "5bee4bb9-2699-4a1a-99a5-9d33f7f96395";

        public static readonly string AngelfireApostleDisplayName = "AngelfireApostle.Name";
        public static readonly string AngelfireApostleDescription = "AngelfireApostle.Description";

        public static BlueprintFeatureReplaceSpellbook AngelfireApostle()
        {
            return FeatureReplaceSpellbookConfigurator.New(AngelfireApostleName, AngelfireApostleGuid)
                .SetDisplayName(AngelfireApostleDisplayName)
                .SetDescription(AngelfireApostleDescription)
                .SetSpellbook(SpellbookRefs.AngelfireApostleSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.AngelfireApostleSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ArcanistName = "2Arcanist";
        public static readonly string ArcanistGuid = "82891c6e-8b5f-4875-b97a-fab268fb2210";

        public static readonly string ArcanistDisplayName = "Arcanist.Name";
        public static readonly string ArcanistDescription = "Arcanist.Description";

        public static BlueprintFeatureReplaceSpellbook Arcanist()
        {
            return FeatureReplaceSpellbookConfigurator.New(ArcanistName, ArcanistGuid)
                .SetDisplayName(ArcanistDisplayName)
                .SetDescription(ArcanistDescription)
                .SetSpellbook(SpellbookRefs.ArcanistSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ArcanistSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ArmagsBladeName = "2ArmagsBlade";
        public static readonly string ArmagsBladeGuid = "149e513e-1ef3-413e-ac83-f1b9a846f844";

        public static readonly string ArmagsBladeDisplayName = "ArmagsBlade.Name";
        public static readonly string ArmagsBladeDescription = "ArmagsBlade.Description";

        public static BlueprintFeatureReplaceSpellbook ArmagsBlade()
        {
            return FeatureReplaceSpellbookConfigurator.New(ArmagsBladeName, ArmagsBladeGuid)
                .SetDisplayName(ArmagsBladeDisplayName)
                .SetDescription(ArmagsBladeDescription)
                .SetSpellbook(SpellbookRefs.ArmagsBladeSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ArmagsBladeSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string BardName = "2Bard";
        public static readonly string BardGuid = "e8196afe-fd79-4fa9-aeff-d47e84a0c683";

        public static readonly string BardDisplayName = "Bard.Name";
        public static readonly string BardDescription = "Bard.Description";

        public static BlueprintFeatureReplaceSpellbook Bard()
        {
            return FeatureReplaceSpellbookConfigurator.New(BardName, BardGuid)
                .SetDisplayName(BardDisplayName)
                .SetDescription(BardDescription)
                .SetSpellbook(SpellbookRefs.BardSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.BardSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string BloodragerName = "2Bloodrager";
        public static readonly string BloodragerGuid = "459533eb-0d4f-47c4-82b3-fd9c08c08311";

        public static readonly string BloodragerDisplayName = "Bloodrager.Name";
        public static readonly string BloodragerDescription = "Bloodrager.Description";

        public static BlueprintFeatureReplaceSpellbook Bloodrager()
        {
            return FeatureReplaceSpellbookConfigurator.New(BloodragerName, BloodragerGuid)
                .SetDisplayName(BloodragerDisplayName)
                .SetDescription(BloodragerDescription)
                .SetSpellbook(SpellbookRefs.BloodragerSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.BloodragerSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ClericName = "2Cleric";
        public static readonly string ClericGuid = "e42dd390-4b4c-4f75-b52b-9f0907a2d2db";

        public static readonly string ClericDisplayName = "Cleric.Name";
        public static readonly string ClericDescription = "Cleric.Description";

        public static BlueprintFeatureReplaceSpellbook Cleric()
        {
            return FeatureReplaceSpellbookConfigurator.New(ClericName, ClericGuid)
                .SetDisplayName(ClericDisplayName)
                .SetDescription(ClericDescription)
                .SetSpellbook(SpellbookRefs.ClericSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ClericSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string CrossbloodedName = "2Crossblooded";
        public static readonly string CrossbloodedGuid = "e3fe1442-3d9b-4b41-8895-1ff3e660beb8";

        public static readonly string CrossbloodedDisplayName = "Crossblooded.Name";
        public static readonly string CrossbloodedDescription = "Crossblooded.Description";

        public static BlueprintFeatureReplaceSpellbook Crossblooded()
        {
            return FeatureReplaceSpellbookConfigurator.New(CrossbloodedName, CrossbloodedGuid)
                .SetDisplayName(CrossbloodedDisplayName)
                .SetDescription(CrossbloodedDescription)
                .SetSpellbook(SpellbookRefs.CrossbloodedSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.CrossbloodedSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string CrusaderName = "2Crusader";
        public static readonly string CrusaderGuid = "d8e26da5-28af-4162-81de-abe69d1f0e86";

        public static readonly string CrusaderDisplayName = "Crusader.Name";
        public static readonly string CrusaderDescription = "Crusader.Description";

        public static BlueprintFeatureReplaceSpellbook Crusader()
        {
            return FeatureReplaceSpellbookConfigurator.New(CrusaderName, CrusaderGuid)
                .SetDisplayName(CrusaderDisplayName)
                .SetDescription(CrusaderDescription)
                .SetSpellbook(SpellbookRefs.CrusaderSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.CrusaderSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string DruidName = "2Druid";
        public static readonly string DruidGuid = "f1ee2fde-b649-4ec7-b2ea-53d8a11e8751";

        public static readonly string DruidDisplayName = "Druid.Name";
        public static readonly string DruidDescription = "Druid.Description";

        public static BlueprintFeatureReplaceSpellbook Druid()
        {
            return FeatureReplaceSpellbookConfigurator.New(DruidName, DruidGuid)
                .SetDisplayName(DruidDisplayName)
                .SetDescription(DruidDescription)
                .SetSpellbook(SpellbookRefs.DruidSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.DruidSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string EldritchFontName = "2EldritchFont";
        public static readonly string EldritchFontGuid = "d68b222c-f3ca-4df0-b968-17a7c135e82b";

        public static readonly string EldritchFontDisplayName = "EldritchFont.Name";
        public static readonly string EldritchFontDescription = "EldritchFont.Description";

        public static BlueprintFeatureReplaceSpellbook EldritchFont()
        {
            return FeatureReplaceSpellbookConfigurator.New(EldritchFontName, EldritchFontGuid)
                .SetDisplayName(EldritchFontDisplayName)
                .SetDescription(EldritchFontDescription)
                .SetSpellbook(SpellbookRefs.EldritchFontSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.EldritchFontSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string EldritchScionName = "2EldritchScion";
        public static readonly string EldritchScionGuid = "9d00b336-6070-4155-a03f-c7d2f0de8c61";

        public static readonly string EldritchScionDisplayName = "EldritchScion.Name";
        public static readonly string EldritchScionDescription = "EldritchScion.Description";

        public static BlueprintFeatureReplaceSpellbook EldritchScion()
        {
            return FeatureReplaceSpellbookConfigurator.New(EldritchScionName, EldritchScionGuid)
                .SetDisplayName(EldritchScionDisplayName)
                .SetDescription(EldritchScionDescription)
                .SetSpellbook(SpellbookRefs.EldritchScionSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.EldritchScionSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string EldritchScoundrelName = "2EldritchScoundrel";
        public static readonly string EldritchScoundrelGuid = "4aa9ac23-00e2-4bde-aaaf-8b863684dac9";

        public static readonly string EldritchScoundrelDisplayName = "EldritchScoundrel.Name";
        public static readonly string EldritchScoundrelDescription = "EldritchScoundrel.Description";

        public static BlueprintFeatureReplaceSpellbook EldritchScoundrel()
        {
            return FeatureReplaceSpellbookConfigurator.New(EldritchScoundrelName, EldritchScoundrelGuid)
                .SetDisplayName(EldritchScoundrelDisplayName)
                .SetDescription(EldritchScoundrelDescription)
                .SetSpellbook(SpellbookRefs.EldritchScoundrelSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.EldritchScoundrelSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string EmpyrealName = "2Empyreal";
        public static readonly string EmpyrealGuid = "d24fa235-6cfb-4a39-8d41-60772e4d1193";

        public static readonly string EmpyrealDisplayName = "Empyreal.Name";
        public static readonly string EmpyrealDescription = "Empyreal.Description";

        public static BlueprintFeatureReplaceSpellbook Empyreal()
        {
            return FeatureReplaceSpellbookConfigurator.New(EmpyrealName, EmpyrealGuid)
                .SetDisplayName(EmpyrealDisplayName)
                .SetDescription(EmpyrealDescription)
                .SetSpellbook(SpellbookRefs.EmpyrealSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.EmpyrealSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ExploiterWizardName = "2ExploiterWizard";
        public static readonly string ExploiterWizardGuid = "8936f0e1-2458-4b1b-81bf-d7322819df02";

        public static readonly string ExploiterWizardDisplayName = "ExploiterWizard.Name";
        public static readonly string ExploiterWizardDescription = "ExploiterWizard.Description";

        public static BlueprintFeatureReplaceSpellbook ExploiterWizard()
        {
            return FeatureReplaceSpellbookConfigurator.New(ExploiterWizardName, ExploiterWizardGuid)
                .SetDisplayName(ExploiterWizardDisplayName)
                .SetDescription(ExploiterWizardDescription)
                .SetSpellbook(SpellbookRefs.ExploiterWizardSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ExploiterWizardSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string FeyspeakerName = "2Feyspeaker";
        public static readonly string FeyspeakerGuid = "41434221-40f3-44e3-ab75-5045674f3e95";

        public static readonly string FeyspeakerDisplayName = "Feyspeaker.Name";
        public static readonly string FeyspeakerDescription = "Feyspeaker.Description";

        public static BlueprintFeatureReplaceSpellbook Feyspeaker()
        {
            return FeatureReplaceSpellbookConfigurator.New(FeyspeakerName, FeyspeakerGuid)
                .SetDisplayName(FeyspeakerDisplayName)
                .SetDescription(FeyspeakerDescription)
                .SetSpellbook(SpellbookRefs.FeyspeakerSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.FeyspeakerSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string HunterName = "2Hunter";
        public static readonly string HunterGuid = "a47c883d-e671-48c5-8048-880151b1f46e";

        public static readonly string HunterDisplayName = "Hunter.Name";
        public static readonly string HunterDescription = "Hunter.Description";

        public static BlueprintFeatureReplaceSpellbook Hunter()
        {
            return FeatureReplaceSpellbookConfigurator.New(HunterName, HunterGuid)
                .SetDisplayName(HunterDisplayName)
                .SetDescription(HunterDescription)
                .SetSpellbook(SpellbookRefs.HunterSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.HunterSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string InquisitorName = "2Inquisitor";
        public static readonly string InquisitorGuid = "daefc670-6c9a-4487-9ed9-03370d50bc6d";

        public static readonly string InquisitorDisplayName = "Inquisitor.Name";
        public static readonly string InquisitorDescription = "Inquisitor.Description";

        public static BlueprintFeatureReplaceSpellbook Inquisitor()
        {
            return FeatureReplaceSpellbookConfigurator.New(InquisitorName, InquisitorGuid)
                .SetDisplayName(InquisitorDisplayName)
                .SetDescription(InquisitorDescription)
                .SetSpellbook(SpellbookRefs.InquisitorSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.InquisitorSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string LeyLineGuardianWitchName = "2LeyLineGuardianWitch";
        public static readonly string LeyLineGuardianWitchGuid = "524523c0-7a2e-492b-a1e9-3439c5745095";

        public static readonly string LeyLineGuardianWitchDisplayName = "LeyLineGuardianWitch.Name";
        public static readonly string LeyLineGuardianWitchDescription = "LeyLineGuardianWitch.Description";

        public static BlueprintFeatureReplaceSpellbook LeyLineGuardianWitch()
        {
            return FeatureReplaceSpellbookConfigurator.New(LeyLineGuardianWitchName, LeyLineGuardianWitchGuid)
                .SetDisplayName(LeyLineGuardianWitchDisplayName)
                .SetDescription(LeyLineGuardianWitchDescription)
                .SetSpellbook(SpellbookRefs.LeyLineGuardianWitchSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.LeyLineGuardianWitchSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string MagusName = "2Magus";
        public static readonly string MagusGuid = "899cd325-87ae-491a-a4f7-fbdc4b6ecb02";

        public static readonly string MagusDisplayName = "Magus.Name";
        public static readonly string MagusDescription = "Magus.Description";

        public static BlueprintFeatureReplaceSpellbook Magus()
        {
            return FeatureReplaceSpellbookConfigurator.New(MagusName, MagusGuid)
                .SetDisplayName(MagusDisplayName)
                .SetDescription(MagusDescription)
                .SetSpellbook(SpellbookRefs.MagusSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.MagusSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string MasterSpyName = "2MasterSpy";
        public static readonly string MasterSpyGuid = "b62d1706-8fab-496c-8cef-5ef7a6ae1ec9";

        public static readonly string MasterSpyDisplayName = "MasterSpy.Name";
        public static readonly string MasterSpyDescription = "MasterSpy.Description";

        public static BlueprintFeatureReplaceSpellbook MasterSpy()
        {
            return FeatureReplaceSpellbookConfigurator.New(MasterSpyName, MasterSpyGuid)
                .SetDisplayName(MasterSpyDisplayName)
                .SetDescription(MasterSpyDescription)
                .SetSpellbook(SpellbookRefs.MasterSpySpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.MasterSpySpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string NatureMageName = "2NatureMage";
        public static readonly string NatureMageGuid = "564c20e7-a440-448c-89eb-2f6f39862927";

        public static readonly string NatureMageDisplayName = "NatureMage.Name";
        public static readonly string NatureMageDescription = "NatureMage.Description";

        public static BlueprintFeatureReplaceSpellbook NatureMage()
        {
            return FeatureReplaceSpellbookConfigurator.New(NatureMageName, NatureMageGuid)
                .SetDisplayName(NatureMageDisplayName)
                .SetDescription(NatureMageDescription)
                .SetSpellbook(SpellbookRefs.NatureMageSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.NatureMageSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string OracleName = "2Oracle";
        public static readonly string OracleGuid = "b7c50ca3-5f49-440b-8451-8abb2e87baad";

        public static readonly string OracleDisplayName = "Oracle.Name";
        public static readonly string OracleDescription = "Oracle.Description";

        public static BlueprintFeatureReplaceSpellbook Oracle()
        {
            return FeatureReplaceSpellbookConfigurator.New(OracleName, OracleGuid)
                .SetDisplayName(OracleDisplayName)
                .SetDescription(OracleDescription)
                .SetSpellbook(SpellbookRefs.OracleSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.OracleSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string PaladinName = "2Paladin";
        public static readonly string PaladinGuid = "72184ecb-bfe7-4c43-8ef2-871e8efc7cbd";

        public static readonly string PaladinDisplayName = "Paladin.Name";
        public static readonly string PaladinDescription = "Paladin.Description";

        public static BlueprintFeatureReplaceSpellbook Paladin()
        {
            return FeatureReplaceSpellbookConfigurator.New(PaladinName, PaladinGuid)
                .SetDisplayName(PaladinDisplayName)
                .SetDescription(PaladinDescription)
                .SetSpellbook(SpellbookRefs.PaladinSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.PaladinSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string RangerName = "2Ranger";
        public static readonly string RangerGuid = "70269172-e282-4868-8458-99264eefc903";

        public static readonly string RangerDisplayName = "Ranger.Name";
        public static readonly string RangerDescription = "Ranger.Description";

        public static BlueprintFeatureReplaceSpellbook Ranger()
        {
            return FeatureReplaceSpellbookConfigurator.New(RangerName, RangerGuid)
                .SetDisplayName(RangerDisplayName)
                .SetDescription(RangerDescription)
                .SetSpellbook(SpellbookRefs.RangerSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.RangerSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string SageName = "2Sage";
        public static readonly string SageGuid = "8a869778-692a-4e03-bd02-05862c852371";

        public static readonly string SageDisplayName = "Sage.Name";
        public static readonly string SageDescription = "Sage.Description";

        public static BlueprintFeatureReplaceSpellbook Sage()
        {
            return FeatureReplaceSpellbookConfigurator.New(SageName, SageGuid)
                .SetDisplayName(SageDisplayName)
                .SetDescription(SageDescription)
                .SetSpellbook(SpellbookRefs.SageSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.SageSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ShamanName = "2Shaman";
        public static readonly string ShamanGuid = "09fb6763-67af-4f21-b2c4-5fe70d997dc1";

        public static readonly string ShamanDisplayName = "Shaman.Name";
        public static readonly string ShamanDescription = "Shaman.Description";

        public static BlueprintFeatureReplaceSpellbook Shaman()
        {
            return FeatureReplaceSpellbookConfigurator.New(ShamanName, ShamanGuid)
                .SetDisplayName(ShamanDisplayName)
                .SetDescription(ShamanDescription)
                .SetSpellbook(SpellbookRefs.ShamanSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ShamanSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string SkaldName = "2Skald";
        public static readonly string SkaldGuid = "8fc5f71a-cf9d-4e84-8fab-892d71928906";

        public static readonly string SkaldDisplayName = "Skald.Name";
        public static readonly string SkaldDescription = "Skald.Description";

        public static BlueprintFeatureReplaceSpellbook Skald()
        {
            return FeatureReplaceSpellbookConfigurator.New(SkaldName, SkaldGuid)
                .SetDisplayName(SkaldDisplayName)
                .SetDescription(SkaldDescription)
                .SetSpellbook(SpellbookRefs.SkaldSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.SkaldSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string SorcererName = "2Sorcerer";
        public static readonly string SorcererGuid = "442f3e70-fa83-4836-bebb-e159a4a6990c";

        public static readonly string SorcererDisplayName = "Sorcerer.Name";
        public static readonly string SorcererDescription = "Sorcerer.Description";

        public static BlueprintFeatureReplaceSpellbook Sorcerer()
        {

            return FeatureReplaceSpellbookConfigurator.New(SorcererName, SorcererGuid)
                .SetDisplayName(SorcererDisplayName)
                .SetDescription(SorcererDescription)
                .SetSpellbook(SpellbookRefs.SorcererSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.SorcererSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string SwordSaintName = "2SwordSaint";
        public static readonly string SwordSaintGuid = "26ad8592-0176-449d-bf85-df743b8d2289";

        public static readonly string SwordSaintDisplayName = "SwordSaint.Name";
        public static readonly string SwordSaintDescription = "SwordSaint.Description";

        public static BlueprintFeatureReplaceSpellbook SwordSaint()
        {
            return FeatureReplaceSpellbookConfigurator.New(SwordSaintName, SwordSaintGuid)
                .SetDisplayName(SwordSaintDisplayName)
                .SetDescription(SwordSaintDescription)
                .SetSpellbook(SpellbookRefs.SwordSaintSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.SwordSaintSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianAbjurationName = "2ThassilonianAbjuration";
        public static readonly string ThassilonianAbjurationGuid = "1a2b2fcd-1f14-446d-8383-7cb4c79bb28a";

        public static readonly string ThassilonianAbjurationDisplayName = "ThassilonianAbjuration.Name";
        public static readonly string ThassilonianAbjurationDescription = "ThassilonianAbjuration.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianAbjuration()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianAbjurationName, ThassilonianAbjurationGuid)
                .SetDisplayName(ThassilonianAbjurationDisplayName)
                .SetDescription(ThassilonianAbjurationDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianAbjurationSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianAbjurationSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianConjurationName = "2ThassilonianConjuration";
        public static readonly string ThassilonianConjurationGuid = "8bc9adfd-4e1e-4aee-bcb0-39bdcfac56c1";

        public static readonly string ThassilonianConjurationDisplayName = "ThassilonianConjuration.Name";
        public static readonly string ThassilonianConjurationDescription = "ThassilonianConjuration.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianConjuration()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianConjurationName, ThassilonianConjurationGuid)
                .SetDisplayName(ThassilonianConjurationDisplayName)
                .SetDescription(ThassilonianConjurationDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianConjurationSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianConjurationSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianEnchantmentName = "2ThassilonianEnchantment";
        public static readonly string ThassilonianEnchantmentGuid = "487af3ca-25e9-4a4f-acc1-82d542d0865e";

        public static readonly string ThassilonianEnchantmentDisplayName = "ThassilonianEnchantment.Name";
        public static readonly string ThassilonianEnchantmentDescription = "ThassilonianEnchantment.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianEnchantment()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianEnchantmentName, ThassilonianEnchantmentGuid)
                .SetDisplayName(ThassilonianEnchantmentDisplayName)
                .SetDescription(ThassilonianEnchantmentDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianEnchantmentSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianEnchantmentSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianEvocationName = "2ThassilonianEvocation";
        public static readonly string ThassilonianEvocationGuid = "2121dbbc-0cca-41d9-873a-295163b5e973";

        public static readonly string ThassilonianEvocationDisplayName = "ThassilonianEvocation.Name";
        public static readonly string ThassilonianEvocationDescription = "ThassilonianEvocation.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianEvocation()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianEvocationName, ThassilonianEvocationGuid)
                .SetDisplayName(ThassilonianEvocationDisplayName)
                .SetDescription(ThassilonianEvocationDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianEvocationSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianEvocationSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianIllusionName = "2ThassilonianIllusion";
        public static readonly string ThassilonianIllusionGuid = "e820f6c0-bd7f-4bb4-808a-337ceebdda58";

        public static readonly string ThassilonianIllusionDisplayName = "ThassilonianIllusion.Name";
        public static readonly string ThassilonianIllusionDescription = "ThassilonianIllusion.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianIllusion()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianIllusionName, ThassilonianIllusionGuid)
                .SetDisplayName(ThassilonianIllusionDisplayName)
                .SetDescription(ThassilonianIllusionDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianIllusionSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianIllusionSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianNecromancyName = "2ThassilonianNecromancy";
        public static readonly string ThassilonianNecromancyGuid = "5b377cef-b636-40bc-bb5a-16c24ab11cfe";

        public static readonly string ThassilonianNecromancyDisplayName = "ThassilonianNecromancy.Name";
        public static readonly string ThassilonianNecromancyDescription = "ThassilonianNecromancy.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianNecromancy()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianNecromancyName, ThassilonianNecromancyGuid)
                .SetDisplayName(ThassilonianNecromancyDisplayName)
                .SetDescription(ThassilonianNecromancyDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianNecromancySpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianNecromancySpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string ThassilonianTransmutationName = "2ThassilonianTransmutation";
        public static readonly string ThassilonianTransmutationGuid = "ddd3c4e8-928a-400c-9173-65626bd35239";

        public static readonly string ThassilonianTransmutationDisplayName = "ThassilonianTransmutation.Name";
        public static readonly string ThassilonianTransmutationDescription = "ThassilonianTransmutation.Description";

        public static BlueprintFeatureReplaceSpellbook ThassilonianTransmutation()
        {
            return FeatureReplaceSpellbookConfigurator.New(ThassilonianTransmutationName, ThassilonianTransmutationGuid)
                .SetDisplayName(ThassilonianTransmutationDisplayName)
                .SetDescription(ThassilonianTransmutationDescription)
                .SetSpellbook(SpellbookRefs.ThassilonianTransmutationSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.ThassilonianTransmutationSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string UnletteredArcanistName = "2UnletteredArcanist";
        public static readonly string UnletteredArcanistGuid = "0fa3c7ae-3f98-471a-919e-124260ba7d5c";

        public static readonly string UnletteredArcanistDisplayName = "UnletteredArcanist.Name";
        public static readonly string UnletteredArcanistDescription = "UnletteredArcanist.Description";

        public static BlueprintFeatureReplaceSpellbook UnletteredArcanist()
        {
            return FeatureReplaceSpellbookConfigurator.New(UnletteredArcanistName, UnletteredArcanistGuid)
                .SetDisplayName(UnletteredArcanistDisplayName)
                .SetDescription(UnletteredArcanistDescription)
                .SetSpellbook(SpellbookRefs.UnletteredArcanistSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.UnletteredArcanistSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string WarpriestName = "2Warpriest";
        public static readonly string WarpriestGuid = "568a5ec2-8d3f-4169-96af-0bfa38972b15";

        public static readonly string WarpriestDisplayName = "Warpriest.Name";
        public static readonly string WarpriestDescription = "Warpriest.Description";

        public static BlueprintFeatureReplaceSpellbook Warpriest()
        {
            return FeatureReplaceSpellbookConfigurator.New(WarpriestName, WarpriestGuid)
                .SetDisplayName(WarpriestDisplayName)
                .SetDescription(WarpriestDescription)
                .SetSpellbook(SpellbookRefs.WarpriestSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.WarpriestSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string WitchName = "2Witch";
        public static readonly string WitchGuid = "dce771a5-fbd4-454c-a340-12b9edf0e120";

        public static readonly string WitchDisplayName = "Witch.Name";
        public static readonly string WitchDescription = "Witch.Description";

        public static BlueprintFeatureReplaceSpellbook Witch()
        {
            return FeatureReplaceSpellbookConfigurator.New(WitchName, WitchGuid)
                .SetDisplayName(WitchDisplayName)
                .SetDescription(WitchDescription)
                .SetSpellbook(SpellbookRefs.WitchSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.WitchSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }

        public static readonly string WizardName = "2Wizard";
        public static readonly string WizardGuid = "1772a025-5160-43c2-992c-441ce929a5ff";

        public static readonly string WizardDisplayName = "Wizard.Name";
        public static readonly string WizardDescription = "Wizard.Description";

        public static BlueprintFeatureReplaceSpellbook Wizard()
        {
            return FeatureReplaceSpellbookConfigurator.New(WizardName, WizardGuid)
                .SetDisplayName(WizardDisplayName)
                .SetDescription(WizardDescription)
                .SetSpellbook(SpellbookRefs.WizardSpellbook.ToString()).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SpellbookRefs.WizardSpellbook.ToString(); })
                .SetRanks(20).Configure();
        }
        //expanded content
        public static readonly string DreadKnightSpellbook = "FDA83B75-D1B8-4858-9F3E-87A251560FF9";
        public static readonly string ClawOfTheFalseWyrmSpellbook = "4e0a9fa3-23c7-449f-af05-f4a20d5a20e1";
        public static readonly string SilverChampionSpellbook = "cf4f5875-5ead-4079-ab90-96cfe2238d38";
        public static readonly string RavenerHunterSpellbook = "e1604126-b492-4139-9add-2f9359f777e8";
        public static readonly string LivingScriptureSpellbook = "5ad84e29-93f9-44a9-8cb2-60343f08664d";
        public static readonly string SkulkingHunterSpellbook = "c59579dd-addf-4590-95a4-45eacb0a6d7e";
        public static readonly string SwornOfTheEldestSpellbook = "a241cf18-fca0-47a3-a98d-8a1befd781cf";
        //character option plus
        public static readonly string ArrowsongMinstrelSpellbook = "b9e9804e-5361-43e7-87c2-ffb7b1195be5";
        //micro content
        public static readonly string AntipaladinSpellbook = "c164072a-cbd2-4c6a-8c4e-271bbb7a6903";
        //ttt
        public static readonly string ChannelerOfTheUnknownSpellbook = "00aef73f-0e57-4d5b-a30e-c30e69165fee";
        public static readonly string MyrmidarchSpellbook = "df419202-e786-4382-adf1-f99b2a60e6c9";

        //mod support
        public static readonly string DreadKnightName = "2DreadKnight";
        public static readonly string DreadKnightGuid = "{898544D6-9C30-4E19-B0B8-90CB935EF68C}";

        public static readonly string DreadKnightDisplayName = "DreadKnight.Name";
        public static readonly string DreadKnightDescription = "DreadKnight.Description";

        public static BlueprintFeatureReplaceSpellbook DreadKnight()
        {
            return FeatureReplaceSpellbookConfigurator.New(DreadKnightName, DreadKnightGuid)
                .SetDisplayName(DreadKnightDisplayName)
                .SetDescription(DreadKnightDescription)
                .SetSpellbook(DreadKnightSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = DreadKnightSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string ClawOfTheFalseWyrmName = "2ClawOfTheFalseWyrm";
        public static readonly string ClawOfTheFalseWyrmGuid = "{B56D6A2F-500B-43C9-80CE-ADF57D55E8C0}";

        public static readonly string ClawOfTheFalseWyrmDisplayName = "ClawOfTheFalseWyrm.Name";
        public static readonly string ClawOfTheFalseWyrmDescription = "ClawOfTheFalseWyrm.Description";

        public static BlueprintFeatureReplaceSpellbook ClawOfTheFalseWyrm()
        {
            return FeatureReplaceSpellbookConfigurator.New(ClawOfTheFalseWyrmName, ClawOfTheFalseWyrmGuid)
                .SetDisplayName(ClawOfTheFalseWyrmDisplayName)
                .SetDescription(ClawOfTheFalseWyrmDescription)
                .SetSpellbook(ClawOfTheFalseWyrmSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = ClawOfTheFalseWyrmSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string SilverChampionName = "2SilverChampion";
        public static readonly string SilverChampionGuid = "{E6B79914-EAD0-4862-92F6-5BE40FE4508B}";

        public static readonly string SilverChampionDisplayName = "SilverChampion.Name";
        public static readonly string SilverChampionDescription = "SilverChampion.Description";

        public static BlueprintFeatureReplaceSpellbook SilverChampion()
        {
            return FeatureReplaceSpellbookConfigurator.New(SilverChampionName, SilverChampionGuid)
                .SetDisplayName(SilverChampionDisplayName)
                .SetDescription(SilverChampionDescription)
                .SetSpellbook(SilverChampionSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SilverChampionSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string RavenerHunterName = "2RavenerHunter";
        public static readonly string RavenerHunterGuid = "{6C51F7A5-0DC2-4815-9509-A77DCBFE7292}";

        public static readonly string RavenerHunterDisplayName = "RavenerHunter.Name";
        public static readonly string RavenerHunterDescription = "RavenerHunter.Description";

        public static BlueprintFeatureReplaceSpellbook RavenerHunter()
        {
            return FeatureReplaceSpellbookConfigurator.New(RavenerHunterName, RavenerHunterGuid)
                .SetDisplayName(RavenerHunterDisplayName)
                .SetDescription(RavenerHunterDescription)
                .SetSpellbook(RavenerHunterSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = RavenerHunterSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string LivingScriptureName = "2LivingScripture";
        public static readonly string LivingScriptureGuid = "{B0E300DC-413C-4943-9461-79E1C55505A6}";

        public static readonly string LivingScriptureDisplayName = "LivingScripture.Name";
        public static readonly string LivingScriptureDescription = "LivingScripture.Description";

        public static BlueprintFeatureReplaceSpellbook LivingScripture()
        {
            return FeatureReplaceSpellbookConfigurator.New(LivingScriptureName, LivingScriptureGuid)
                .SetDisplayName(LivingScriptureDisplayName)
                .SetDescription(LivingScriptureDescription)
                .SetSpellbook(LivingScriptureSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = LivingScriptureSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string SkulkingHunterName = "2SkulkingHunter";
        public static readonly string SkulkingHunterGuid = "{7A9EDBE1-8592-41E3-8EBC-8B4172462A28}";

        public static readonly string SkulkingHunterDisplayName = "SkulkingHunter.Name";
        public static readonly string SkulkingHunterDescription = "SkulkingHunter.Description";

        public static BlueprintFeatureReplaceSpellbook SkulkingHunter()
        {
            return FeatureReplaceSpellbookConfigurator.New(SkulkingHunterName, SkulkingHunterGuid)
                .SetDisplayName(SkulkingHunterDisplayName)
                .SetDescription(SkulkingHunterDescription)
                .SetSpellbook(SkulkingHunterSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SkulkingHunterSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string SwornOfTheEldestName = "2SwornOfTheEldest";
        public static readonly string SwornOfTheEldestGuid = "{E4282A13-BCDB-4F10-A756-9D57B94640F0}";

        public static readonly string SwornOfTheEldestDisplayName = "SwornOfTheEldest.Name";
        public static readonly string SwornOfTheEldestDescription = "SwornOfTheEldest.Description";

        public static BlueprintFeatureReplaceSpellbook SwornOfTheEldest()
        {
            return FeatureReplaceSpellbookConfigurator.New(SwornOfTheEldestName, SwornOfTheEldestGuid)
                .SetDisplayName(SwornOfTheEldestDisplayName)
                .SetDescription(SwornOfTheEldestDescription)
                .SetSpellbook(SwornOfTheEldestSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = SwornOfTheEldestSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string ArrowsongMinstrelName = "2ArrowsongMinstrel";
        public static readonly string ArrowsongMinstrelGuid = "{0271757F-DFBB-459E-A7D8-D80513362686}";

        public static readonly string ArrowsongMinstrelDisplayName = "ArrowsongMinstrel.Name";
        public static readonly string ArrowsongMinstrelDescription = "ArrowsongMinstrel.Description";

        public static BlueprintFeatureReplaceSpellbook ArrowsongMinstrel()
        {
            return FeatureReplaceSpellbookConfigurator.New(ArrowsongMinstrelName, ArrowsongMinstrelGuid)
                .SetDisplayName(ArrowsongMinstrelDisplayName)
                .SetDescription(ArrowsongMinstrelDescription)
                .SetSpellbook(ArrowsongMinstrelSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = ArrowsongMinstrelSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string AntipaladinName = "2Antipaladin";
        public static readonly string AntipaladinGuid = "{768BF21E-CD41-471B-BAE0-03CC88461CD3}";

        public static readonly string AntipaladinDisplayName = "Antipaladin.Name";
        public static readonly string AntipaladinDescription = "Antipaladin.Description";

        public static BlueprintFeatureReplaceSpellbook Antipaladin()
        {
            return FeatureReplaceSpellbookConfigurator.New(AntipaladinName, AntipaladinGuid)
                .SetDisplayName(AntipaladinDisplayName)
                .SetDescription(AntipaladinDescription)
                .SetSpellbook(AntipaladinSpellbook)
                .SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = AntipaladinSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string ChannelerOfTheUnknownName = "2ChannelerOfTheUnknown";
        public static readonly string ChannelerOfTheUnknownGuid = "{55313A0B-7038-4BB5-888A-FB648B338693}";

        public static readonly string ChannelerOfTheUnknownDisplayName = "ChannelerOfTheUnknown.Name";
        public static readonly string ChannelerOfTheUnknownDescription = "ChannelerOfTheUnknown.Description";

        public static BlueprintFeatureReplaceSpellbook ChannelerOfTheUnknown()
        {
            return FeatureReplaceSpellbookConfigurator.New(ChannelerOfTheUnknownName, ChannelerOfTheUnknownGuid)
                .SetDisplayName(ChannelerOfTheUnknownDisplayName)
                .SetDescription(ChannelerOfTheUnknownDescription)
                .SetSpellbook(ChannelerOfTheUnknownSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = ChannelerOfTheUnknownSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }

        public static readonly string MyrmidarchName = "2Myrmidarch";
        public static readonly string MyrmidarchGuid = "{973D6EB8-A5DD-462A-96EA-C42EF2C5CDC4}";

        public static readonly string MyrmidarchDisplayName = "Myrmidarch.Name";
        public static readonly string MyrmidarchDescription = "Myrmidarch.Description";

        public static BlueprintFeatureReplaceSpellbook Myrmidarch()
        {
            return FeatureReplaceSpellbookConfigurator.New(MyrmidarchName, MyrmidarchGuid)
                .SetDisplayName(MyrmidarchDisplayName)
                .SetDescription(MyrmidarchDescription)
                .SetSpellbook(MyrmidarchSpellbook).SetHideNotAvailibleInUI(true)
                .AddComponent<PrerequisiteSpellBook>(c => { c.Book = MyrmidarchSpellbook; })
                .SetRanks(20).Configure(delayed: true);
        }
    }
}
