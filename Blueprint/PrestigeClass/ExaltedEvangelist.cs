using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.CustomComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Blueprints.Configurators.Root;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using PrestigePlus.CustomComponent.Archetype;
using PrestigePlus.Blueprint.Archetype;
using PrestigePlus.Patch;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities;
using System.Drawing;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.Blueprint.Gunslinger;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class ExaltedEvangelist
    {
        private const string ArchetypeName = "ExaltedEvangelist";
        public static readonly string ArchetypeGuid = "{F22A5FE2-A6EF-4D1D-9A58-676A1AE62210}";
        internal const string ArchetypeDisplayName = "ExaltedEvangelist.Name";
        private const string ArchetypeDescription = "ExaltedEvangelist.Description";

        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "ExaltedEvangelistPrestige";
        private static readonly string ClassProgressGuid = "{BB7537D6-63C5-41AE-80B8-1B23826569F3}";

        public static void Configure()
        {
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1, ChoosePathFeat(), FeatureSelectionRefs.StudentOfWarAdditionalSKillSelection.ToString())
            .AddToLevelEntry(2, ProtectiveGraceFeat())
            .AddToLevelEntry(3, Sentinel.DivineBoon1Guid)
            .AddToLevelEntry(4)
            .AddToLevelEntry(5)
            .AddToLevelEntry(6, Sentinel.DivineBoon2Guid)
            .AddToLevelEntry(7, ProtectiveGraceGuid)
            .AddToLevelEntry(8)
            .AddToLevelEntry(9, Sentinel.DivineBoon3Guid)
            .AddToLevelEntry(10, SpiritualFormFeat())
            .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureSelectionRefs.SecondatyElementalFocusSelection.ToString(), FeatureRefs.Supercharge.ToString(), FeatureRefs.CompositeBlastSpecialisation.ToString() })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FeatureSelectionRefs.ThirdElementalFocusSelection.ToString(), FeatureSelectionRefs.InfusionSelection.ToString() })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { Sentinel.DivineBoon1Guid, Sentinel.DivineBoon2Guid, Sentinel.DivineBoon3Guid }))
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName("")
            .SetDescription(ArchetypeDescription)
            .Configure();

            BlueprintCharacterClass archetype =
          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName(ArchetypeDisplayName)
            .SetLocalizedDescription(ArchetypeDescription)
            .SetSkillPoints(6)
            .SetHitDie(DiceType.D8)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(BABMedium)
            .SetFortitudeSave(SavesPrestigeLow)
            .SetReflexSave(SavesPrestigeLow)
            .SetWillSave(SavesPrestigeHigh)
            .SetProgression(progression)
            .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillLoreNature, StatType.SkillPersuasion, StatType.SkillPerception })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5, group: Prerequisite.GroupType.Any)
            .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 5, group: Prerequisite.GroupType.Any)
            .AddComponent<PrerequisiteSpellLevel>(c => { c.RequiredSpellLevel = 3; c.Group = Prerequisite.GroupType.Any; })
            .AddPrerequisiteFeature(DeificObedience.DeificObedienceGuid)
            .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string BonusFeat = "ExaltedEvangelist.BonusFeat";
        public static readonly string BonusFeatGuid = "{7F5B0CBD-77E1-4269-9DC5-13255FF73567}";

        internal const string BonusFeatDisplayName = "ExaltedEvangelistBonusFeat.Name";
        private const string BonusFeatDescription = "ExaltedEvangelistBonusFeat.Description";

        public static BlueprintFeatureSelection BonusFeatFeat()
        {
            var icon = AbilityRefs.Prayer.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(BonusFeat, BonusFeatGuid)
              .SetDisplayName(BonusFeatDisplayName)
              .SetDescription(BonusFeatDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(DeificObedience.MahathallahExaltedGuid)
              .AddToAllFeatures(DeificObedience.LamashtuExaltedGuid)
              .AddToAllFeatures(DeificObedience.NiviExaltedGuid)
              .AddToAllFeatures(DeificObedience.KabririExaltedGuid)
              .AddToAllFeatures(DeificObedience.GozrehExaltedGuid)
              .AddToAllFeatures(DeificObedience.CalistriaExaltedGuid)
              .AddToAllFeatures(DeificObedience.PharasmaExaltedGuid)
              .AddToAllFeatures(DeificObedience.NethysExaltedGuid)
              .Configure();
        }

        private const string ProtectiveGrace = "ExaltedEvangelist.ProtectiveGrace";
        private static readonly string ProtectiveGraceGuid = "{EAB10976-F479-402D-B2CD-FDA197EB6B1A}";

        internal const string ExaltedEvangelistProtectiveGraceDisplayName = "ExaltedEvangelistProtectiveGrace.Name";
        private const string ExaltedEvangelistProtectiveGraceDescription = "ExaltedEvangelistProtectiveGrace.Description";
        public static BlueprintFeature ProtectiveGraceFeat()
        {
            var icon = FeatureRefs.Dodge.Reference.Get().Icon;
            return FeatureConfigurator.New(ProtectiveGrace, ProtectiveGraceGuid)
              .SetDisplayName(ExaltedEvangelistProtectiveGraceDisplayName)
              .SetDescription(ExaltedEvangelistProtectiveGraceDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.AC, value: ContextValues.Rank(), descriptor: ModifierDescriptor.Dodge)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(ProtectiveGraceGuid))
              .SetRanks(2)
              .Configure();
        }

        private const string ReligiousSpeaker = "ExaltedEvangelist.ReligiousSpeaker";
        private static readonly string ReligiousSpeakerGuid = "{31829A69-CCA4-4615-A033-C62838785B73}";

        internal const string ExaltedEvangelistReligiousSpeakerDisplayName = "ExaltedEvangelistReligiousSpeaker.Name";
        private const string ExaltedEvangelistReligiousSpeakerDescription = "ExaltedEvangelistReligiousSpeaker.Description";
        public static BlueprintFeature ReligiousSpeakerFeat()
        {
            var icon = FeatureRefs.Persuasive.Reference.Get().Icon;
            return FeatureConfigurator.New(ReligiousSpeaker, ReligiousSpeakerGuid)
              .SetDisplayName(ExaltedEvangelistReligiousSpeakerDisplayName)
              .SetDescription(ExaltedEvangelistReligiousSpeakerDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.CheckDiplomacy, value: 2, descriptor: ModifierDescriptor.Competence)
              .AddContextStatBonus(StatType.CheckBluff, value: 2, descriptor: ModifierDescriptor.Competence)
              .Configure();
        }

        private const string ChoosePath = "ExaltedEvangelist.ChoosePath";
        private static readonly string ChoosePathGuid = "{FAF8B16D-4C5F-4159-8DF6-6B322E09BD5D}";

        internal const string ChoosePathDisplayName = "ExaltedEvangelistChoosePath.Name";
        private const string ChoosePathDescription = "ExaltedEvangelistChoosePath.Description";

        public static BlueprintFeatureSelection ChoosePathFeat()
        {
            var icon = AbilityRefs.RemoveDisease.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChoosePath, ChoosePathGuid)
              .SetDisplayName(ChoosePathDisplayName)
              .SetDescription(ChoosePathDescription)
              .SetIcon(icon)
              .SetObligatory(true)
              .AddFacts(new() { FeatureRefs.LightArmorProficiency.ToString(), FeatureRefs.SimpleWeaponProficiency.ToString() })
              .AddComponent<AddDeityWeaponPro>()
              .AddToAllFeatures(AlignSpamFeat())
              .AddToAllFeatures(TrueExaltedFeat())
              .Configure();
        }

        private const string ChooseGoodEvil = "ExaltedEvangelist.ChooseGoodEvil";
        private static readonly string ChooseGoodEvilGuid = "{F116BAD1-6CDA-41EA-8317-1005C76C3791}";

        internal const string ChooseGoodEvilDisplayName = "ExaltedEvangelistChooseGoodEvil.Name";
        private const string ChooseGoodEvilDescription = "ExaltedEvangelistChooseGoodEvil.Description";

        public static BlueprintFeatureSelection ChooseGoodEvilFeat()
        {
            var icon = AbilityRefs.BreathOfLifeCast.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChooseGoodEvil, ChooseGoodEvilGuid)
              .SetDisplayName(ChooseGoodEvilDisplayName)
              .SetDescription(ChooseGoodEvilDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(VitalityGoodFeat())
              .AddToAllFeatures(VitalityEvilFeat())
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string VitalityGood = "ExaltedEvangelist.VitalityGood";
        private static readonly string VitalityGoodGuid = "{AB596A7C-F7C6-45C9-B920-2DAAEA75068C}";

        internal const string ExaltedEvangelistVitalityGoodDisplayName = "ExaltedEvangelistVitalityGood.Name";
        private const string ExaltedEvangelistVitalityGoodDescription = "ExaltedEvangelistVitalityGood.Description";
        public static BlueprintFeature VitalityGoodFeat()
        {
            var icon = AbilityRefs.BreathOfLifeCast.Reference.Get().Icon;
            return FeatureConfigurator.New(VitalityGood, VitalityGoodGuid)
              .SetDisplayName(ExaltedEvangelistVitalityGoodDisplayName)
              .SetDescription(ExaltedEvangelistVitalityGoodDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.EvilDomainAllowed.ToString())
              .AddStatBonus(ModifierDescriptor.Sacred, false, StatType.SaveFortitude, 2)
              .Configure();
        }

        private const string VitalityEvil = "ExaltedEvangelist.VitalityEvil";
        private static readonly string VitalityEvilGuid = "{6454B583-CFC1-4AEA-9287-649FD427DA06}";

        internal const string ExaltedEvangelistVitalityEvilDisplayName = "ExaltedEvangelistVitalityEvil.Name";
        private const string ExaltedEvangelistVitalityEvilDescription = "ExaltedEvangelistVitalityEvil.Description";
        public static BlueprintFeature VitalityEvilFeat()
        {
            var icon = FeatureRefs.BardLoreMaster.Reference.Get().Icon;
            return FeatureConfigurator.New(VitalityEvil, VitalityEvilGuid)
              .SetDisplayName(ExaltedEvangelistVitalityEvilDisplayName)
              .SetDescription(ExaltedEvangelistVitalityEvilDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.GoodDomainAllowed.ToString())
              .AddStatBonus(ModifierDescriptor.Profane, false, StatType.SaveFortitude, 2)
              .Configure();
        }

        private static readonly string SpiritualFormName = "ExaltedEvangelistSpiritualForm";
        public static readonly string SpiritualFormGuid = "{DDE79D72-241C-48AA-82A7-1F384F89B4D4}";

        private static readonly string SpiritualFormDisplayName = "ExaltedEvangelistSpiritualForm.Name";
        private static readonly string SpiritualFormDescription = "ExaltedEvangelistSpiritualForm.Description";

        private const string AuraBuff = "ExaltedEvangelist.SpiritualFormbuff";
        private static readonly string AuraBuffGuid = "{3EE49241-847D-4EAA-B443-07488F199866}";

        private const string SpiritualFormAbility = "ExaltedEvangelist.SpiritualFormAbility";
        private static readonly string SpiritualFormAbilityGuid = "{13ADBD31-88C2-444F-9E2D-C7DD99AE489F}";

        private const string SpiritualFormAbilityRes = "ExaltedEvangelist.SpiritualFormAbilityRes";
        private static readonly string SpiritualFormAbilityResGuid = "{ABF48D0A-312F-46A1-A671-CD8AE8923A36}";

        public static BlueprintFeature SpiritualFormFeat()
        {
            var icon = AbilityRefs.SacredNimbus.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(AuraBuff, AuraBuffGuid)
              .SetDisplayName(SpiritualFormDisplayName)
              .SetDescription(SpiritualFormDescription)
              .SetIcon(icon)
              .AddAdditionalLimb(ItemWeaponRefs.Tail1d6.ToString())
              .AddComponent<SpiritualFormBonus>()
              .Configure();

            var abilityresourse = AbilityResourceConfigurator.New(SpiritualFormAbilityRes, SpiritualFormAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0))
                .Configure();

            var ability = ActivatableAbilityConfigurator.New(SpiritualFormAbility, SpiritualFormAbilityGuid)
                .SetDisplayName(SpiritualFormDisplayName)
                .SetDescription(SpiritualFormDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.OncePerMinute)
                .Configure();

            return FeatureConfigurator.New(SpiritualFormName, SpiritualFormGuid)
                    .SetDisplayName(SpiritualFormDisplayName)
                    .SetDescription(SpiritualFormDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .AddIncreaseResourceAmountBySharedValue(false, abilityresourse, ContextValues.Property(UnitProperty.Level))
                    .Configure();
        }

        private const string TrueExalted = "ExaltedEvangelist.TrueExalted";
        private static readonly string TrueExaltedGuid = "{0DC8DB64-DA89-4092-A5A4-15923F6798A6}";

        internal const string TrueExaltedDisplayName = "ExaltedEvangelistTrueExalted.Name";
        private const string TrueExaltedDescription = "ExaltedEvangelistTrueExalted.Description";
        public static BlueprintProgression TrueExaltedFeat()
        {
            var icon = AbilityRefs.RemoveDisease.Reference.Get().Icon;
            return ProgressionConfigurator.New(TrueExalted, TrueExaltedGuid)
              .SetDisplayName(TrueExaltedDisplayName)
              .SetDescription(TrueExaltedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteCasterTypeSpellLevel(false, false, 3)
              .AddToLevelEntry(1, BonusFeatFeat(), "{05DC9561-0542-41BD-9E9F-404F59AB68C5}")
              .AddToLevelEntry(2, ChooseGoodEvilFeat())
              .AddToLevelEntry(4, ReligiousSpeakerFeat())
              .AddToLevelEntry(5, ExaltedDomainPlusFeat())
              .AddToLevelEntry(8, CreateSpellbook())
              .Configure();
        }

        private const string ExaltedDomainPlus = "Exalted.ExaltedDomainPlus";
        public static readonly string ExaltedDomainPlusGuid = "{18E8AE34-E61D-4149-989B-4E55560F03CA}";

        internal const string ExaltedDomainPlusDisplayName = "ExaltedExaltedDomainPlus.Name";
        private const string ExaltedDomainPlusDescription = "ExaltedExaltedDomainPlus.Description";

        public static BlueprintProgression ExaltedDomainPlusFeat()
        {
            var icon = AbilityRefs.Bless.Reference.Get().Icon;
            return ProgressionConfigurator.New(ExaltedDomainPlus, ExaltedDomainPlusGuid)
              .SetDisplayName(ExaltedDomainPlusDisplayName)
              .SetDescription(ExaltedDomainPlusDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetGiveFeaturesForPreviousLevels(true)
              .AddToClasses(ArchetypeGuid)
              .AddToLevelEntry(1, FeatureSelectionRefs.SecondDomainsSelection.ToString(), AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
              .AddToLevelEntry(2, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(3, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(4, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(5, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(6, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(7, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(8, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(9, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
                .AddToLevelEntry(10, AnchoriteofDawn.AnchoriteDomainPlusfeatGuid)
              .Configure();
        }

        private const string SpellBook = "ExaltedEvangelist.SpellBook";
        public static readonly string SpellBookGuid = "{2BEC7992-A06A-4691-A65E-71F6BE6E8D49}";

        private const string SpellBookBuff = "ExaltedEvangelist.SpellBookBuff";
        public static readonly string SpellBookBuffGuid = "{1E7CFC7C-6F5B-4F14-8D7B-9F9FF5C1D22E}";

        private const string SpellBookFeat = "ExaltedEvangelist.SpellBookFeat";
        private static readonly string SpellBookFeatGuid = "{337EF978-CEE6-4B71-9D64-7A32470C81B9}";

        internal const string SpellBookDisplayName = "ExaltedEvangelistSpellBook.Name";
        private const string SpellBookDescription = "ExaltedEvangelistSpellBook.Description";
        private static BlueprintFeature CreateSpellbook()
        {
            var icon = AbilityRefs.Foresight.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(SpellBookBuff, SpellBookBuffGuid)
              .SetDisplayName(SpellBookDisplayName)
              .SetDescription(SpellBookDescription)
              .SetIcon(icon)
              .AddForbidSpellbook(spellbook: SpellBookGuid)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .AddToFlags(BlueprintBuff.Flags.RemoveOnRest)
              .Configure();

            var spellbook = SpellbookConfigurator.New(SpellBook, SpellBookGuid)
              .SetName(ArchetypeDisplayName)
              .SetSpellsPerDay(GetSpellSlots())
              .SetAllSpellsKnown(true)
              .SetSpellList(GraveSpellList.spelllist2guid)
              .SetCharacterClass(ArchetypeGuid)
              .SetCastingAttribute(StatType.Charisma)
              .SetHasSpecialSpellList(true)
              .SetSpontaneous(false)
              .SetIsArcane(false)
              .SetCantripsType(CantripsType.Orisions)
              .Configure(delayed: true);

            return FeatureConfigurator.New(SpellBookFeat, SpellBookFeatGuid)
                    .SetDisplayName(SpellBookDisplayName)
                    .SetDescription(SpellBookDescription)
                    .SetIcon(icon)
                    .AddSpellbook(ContextValues.Property(UnitProperty.Level), spellbook: spellbook)
                    .AddComponent<MiracleSpellLevel>(c => { c.book = SpellBookGuid; c.level = 7; c.buff = SpellBookBuffGuid; })
                    .Configure();
        }

        private const string SpellTable = "ExaltedEvangelist.SpellTable";
        public static readonly string SpellTableGuid = "{8FD87C0A-831F-450C-8843-0F770B2F777D}";
        private static BlueprintSpellsTable GetSpellSlots()
        {
            var ClericSpellSlots = SpellsTableRefs.ClericSpellLevels.Reference.Get();
            var levelEntries =
              ClericSpellSlots.Levels.Select(
                l =>
                {
                    var count = new int[] { 0, 30, 30, 30, 30, 30, 30 };
                    return new SpellsLevelEntry { Count = count };
                });
            return SpellsTableConfigurator.New(SpellTable, SpellTableGuid)
              .SetLevels(levelEntries.ToArray())
              .Configure();
        }

        private const string AlignSpam = "ExaltedEvangelist.AlignSpam";
        public static readonly string AlignSpamGuid = "{69DFC912-DFE1-46A7-A6AC-4E701DCF0A27}";

        internal const string SanctifiedRogueDisplayName = "ExaltedEvangelistSanctifiedRogue.Name";
        private const string SanctifiedRogueDescription = "ExaltedEvangelistSanctifiedRogue.Description";

        public static BlueprintFeatureSelection AlignSpamFeat()
        {
            var icon = FeatureSelectionRefs.SlayerTalentSelection2.Reference.Get().Icon;

            string GunslingerClass = GunslingerMain.ArchetypeGuid;
            string AgentoftheGraveClass = AgentoftheGrave.ArchetypeGuid;
            string AnchoriteofDawnClass = AnchoriteofDawn.ArchetypeGuid;
            string ArcaneAcherClass = ArcaneArcher.ArchetypeGuid;
            string AsavirClass = Asavir.ArchetypeGuid;
            string ChevalierClass = Chevalier.ArchetypeGuid;
            string CrimsonTemplarClass = CrimsonTemplar.ArchetypeGuid;
            string DeadeyeDevoteeClass = DeadeyeDevotee.ArchetypeGuid;
            string DragonFuryClass = DragonFury.ArchetypeGuid;
            string EsotericKnightClass = EsotericKnight.ArchetypeGuid;
            //string ExaltedEvangelistClass = ExaltedEvangelist.ArchetypeGuid;
            string FuriousGuardianClass = FuriousGuardian.ArchetypeGuid;
            string HalflingOpportunistClass = HalflingOpportunist.ArchetypeGuid;
            string HinterlanderClass = Hinterlander.ArchetypeGuid;
            string HorizonWalkerClass = HorizonWalker.ArchetypeGuid;
            string InheritorCrusaderClass = InheritorCrusader.ArchetypeGuid;
            string MammothRiderClass = MammothRider.ArchetypeGuid;
            string SanguineAngelClass = SanguineAngel.ArchetypeGuid;
            string ScarSeekerClass = ScarSeeker.ArchetypeGuid;
            string SentinelClass = Sentinel.ArchetypeGuid;
            string ShadowDancerClass = ShadowDancer.ArchetypeGuid;
            string SouldrinkerClass = Souldrinker.ArchetypeGuid;
            string UmbralAgentClass = UmbralAgent.ArchetypeGuid;
            string MicroAntiPaladinClass = "8939eff2-5a0a-4b77-ad1a-b6be4c760a6c";
            string OathbreakerClass = "B35CE8EE-32C2-4BFD-8884-740F13AAEE12";
            string DreadKnightClass = "D0EB4CA4-4E11-417C-9B2F-0208491067A0";
            string StargazerClass = "7e3cde18-3dad-43ab-a7cc-e14b6ca51216";
            string SwashbucklerClass = "338ABF27-23C1-4C1A-B0F1-7CD7E3020444";
            string HolyVindicatorClass = "b5daf66532f5425aa22df5372c57d766";
            string SummonerClass = "c6a9c7f9-bdce-4c89-aedf-cde62620b2b7";
            string LionBladeClass = LionBlade.ArchetypeGuid;
            string EnchantingCourtesanClass = EnchantingCourtesan.ArchetypeGuid;
            string HeritorKnightClass = HeritorKnight.ArchetypeGuid;
            string GoldenLegionnaireClass = GoldenLegionnaire.ArchetypeGuid;
            string BoltAceClass = BoltAce.ArchetypeGuid;
            string MortalUsherClass = MortalUsher.ArchetypeGuid;
            string ForestWardenClass = ForestWarden.ArchetypeGuid;

            var list = new List<BlueprintFeature>();

            var EvangelistAlchemistClasspro = ProgressionConfigurator.New(EvangelistAlchemistClass0Align, EvangelistAlchemistClass0AlignGuid)
            .SetDisplayName(EvangelistAlchemistClass0AlignDisplayName)
            .SetDescription(EvangelistAlchemistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AlchemistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistAlchemistClass2Align, EvangelistAlchemistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistAlchemistClass3Align, EvangelistAlchemistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistAlchemistClass4Align, EvangelistAlchemistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistAlchemistClass5Align, EvangelistAlchemistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistAlchemistClass6Align, EvangelistAlchemistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistAlchemistClass7Align, EvangelistAlchemistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistAlchemistClass8Align, EvangelistAlchemistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistAlchemistClass9Align, EvangelistAlchemistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAlchemistClasspro = EvangelistAlchemistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistAlchemistClass10Align, EvangelistAlchemistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistAlchemistClasspro.Configure());
            var EvangelistArcaneTricksterClasspro = ProgressionConfigurator.New(EvangelistArcaneTricksterClass0Align, EvangelistArcaneTricksterClass0AlignGuid)
            .SetDisplayName(EvangelistArcaneTricksterClass0AlignDisplayName)
            .SetDescription(EvangelistArcaneTricksterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcaneTricksterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass2Align, EvangelistArcaneTricksterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass3Align, EvangelistArcaneTricksterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass4Align, EvangelistArcaneTricksterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass5Align, EvangelistArcaneTricksterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass6Align, EvangelistArcaneTricksterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass7Align, EvangelistArcaneTricksterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass8Align, EvangelistArcaneTricksterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass9Align, EvangelistArcaneTricksterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcaneTricksterClasspro = EvangelistArcaneTricksterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistArcaneTricksterClass10Align, EvangelistArcaneTricksterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistArcaneTricksterClasspro.Configure());
            var EvangelistArcanistClasspro = ProgressionConfigurator.New(EvangelistArcanistClass0Align, EvangelistArcanistClass0AlignGuid)
            .SetDisplayName(EvangelistArcanistClass0AlignDisplayName)
            .SetDescription(EvangelistArcanistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcanistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistArcanistClass2Align, EvangelistArcanistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistArcanistClass3Align, EvangelistArcanistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistArcanistClass4Align, EvangelistArcanistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistArcanistClass5Align, EvangelistArcanistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistArcanistClass6Align, EvangelistArcanistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistArcanistClass7Align, EvangelistArcanistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistArcanistClass8Align, EvangelistArcanistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistArcanistClass9Align, EvangelistArcanistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistArcanistClasspro = EvangelistArcanistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistArcanistClass10Align, EvangelistArcanistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistArcanistClasspro.Configure());
            var EvangelistAssassinClasspro = ProgressionConfigurator.New(EvangelistAssassinClass0Align, EvangelistAssassinClass0AlignGuid)
            .SetDisplayName(EvangelistAssassinClass0AlignDisplayName)
            .SetDescription(EvangelistAssassinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AssassinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistAssassinClass2Align, EvangelistAssassinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistAssassinClass3Align, EvangelistAssassinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistAssassinClass4Align, EvangelistAssassinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistAssassinClass5Align, EvangelistAssassinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistAssassinClass6Align, EvangelistAssassinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistAssassinClass7Align, EvangelistAssassinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistAssassinClass8Align, EvangelistAssassinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistAssassinClass9Align, EvangelistAssassinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistAssassinClasspro = EvangelistAssassinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistAssassinClass10Align, EvangelistAssassinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistAssassinClasspro.Configure());
            var EvangelistBarbarianClasspro = ProgressionConfigurator.New(EvangelistBarbarianClass0Align, EvangelistBarbarianClass0AlignGuid)
            .SetDisplayName(EvangelistBarbarianClass0AlignDisplayName)
            .SetDescription(EvangelistBarbarianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistBarbarianClass2Align, EvangelistBarbarianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistBarbarianClass3Align, EvangelistBarbarianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistBarbarianClass4Align, EvangelistBarbarianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistBarbarianClass5Align, EvangelistBarbarianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistBarbarianClass6Align, EvangelistBarbarianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistBarbarianClass7Align, EvangelistBarbarianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistBarbarianClass8Align, EvangelistBarbarianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistBarbarianClass9Align, EvangelistBarbarianClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBarbarianClasspro = EvangelistBarbarianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistBarbarianClass10Align, EvangelistBarbarianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistBarbarianClasspro.Configure());
            var EvangelistBardClasspro = ProgressionConfigurator.New(EvangelistBardClass0Align, EvangelistBardClass0AlignGuid)
            .SetDisplayName(EvangelistBardClass0AlignDisplayName)
            .SetDescription(EvangelistBardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistBardClass2Align, EvangelistBardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistBardClass3Align, EvangelistBardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistBardClass4Align, EvangelistBardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistBardClass5Align, EvangelistBardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistBardClass6Align, EvangelistBardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistBardClass7Align, EvangelistBardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistBardClass8Align, EvangelistBardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistBardClass9Align, EvangelistBardClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBardClasspro = EvangelistBardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistBardClass10Align, EvangelistBardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistBardClasspro.Configure());
            var EvangelistBloodragerClasspro = ProgressionConfigurator.New(EvangelistBloodragerClass0Align, EvangelistBloodragerClass0AlignGuid)
            .SetDisplayName(EvangelistBloodragerClass0AlignDisplayName)
            .SetDescription(EvangelistBloodragerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BloodragerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistBloodragerClass2Align, EvangelistBloodragerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistBloodragerClass3Align, EvangelistBloodragerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistBloodragerClass4Align, EvangelistBloodragerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistBloodragerClass5Align, EvangelistBloodragerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistBloodragerClass6Align, EvangelistBloodragerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistBloodragerClass7Align, EvangelistBloodragerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistBloodragerClass8Align, EvangelistBloodragerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistBloodragerClass9Align, EvangelistBloodragerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistBloodragerClasspro = EvangelistBloodragerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistBloodragerClass10Align, EvangelistBloodragerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistBloodragerClasspro.Configure());
            var EvangelistCavalierClasspro = ProgressionConfigurator.New(EvangelistCavalierClass0Align, EvangelistCavalierClass0AlignGuid)
            .SetDisplayName(EvangelistCavalierClass0AlignDisplayName)
            .SetDescription(EvangelistCavalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.CavalierClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistCavalierClass2Align, EvangelistCavalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistCavalierClass3Align, EvangelistCavalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistCavalierClass4Align, EvangelistCavalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistCavalierClass5Align, EvangelistCavalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistCavalierClass6Align, EvangelistCavalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistCavalierClass7Align, EvangelistCavalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistCavalierClass8Align, EvangelistCavalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistCavalierClass9Align, EvangelistCavalierClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistCavalierClasspro = EvangelistCavalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistCavalierClass10Align, EvangelistCavalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistCavalierClasspro.Configure());
            var EvangelistClericClasspro = ProgressionConfigurator.New(EvangelistClericClass0Align, EvangelistClericClass0AlignGuid)
            .SetDisplayName(EvangelistClericClass0AlignDisplayName)
            .SetDescription(EvangelistClericClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistClericClass2Align, EvangelistClericClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistClericClass3Align, EvangelistClericClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistClericClass4Align, EvangelistClericClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistClericClass5Align, EvangelistClericClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistClericClass6Align, EvangelistClericClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistClericClass7Align, EvangelistClericClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistClericClass8Align, EvangelistClericClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistClericClass9Align, EvangelistClericClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistClericClasspro = EvangelistClericClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistClericClass10Align, EvangelistClericClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistClericClasspro.Configure());
            var EvangelistDragonDiscipleClasspro = ProgressionConfigurator.New(EvangelistDragonDiscipleClass0Align, EvangelistDragonDiscipleClass0AlignGuid)
            .SetDisplayName(EvangelistDragonDiscipleClass0AlignDisplayName)
            .SetDescription(EvangelistDragonDiscipleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DragonDiscipleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass2Align, EvangelistDragonDiscipleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass3Align, EvangelistDragonDiscipleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass4Align, EvangelistDragonDiscipleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass5Align, EvangelistDragonDiscipleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass6Align, EvangelistDragonDiscipleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass7Align, EvangelistDragonDiscipleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass8Align, EvangelistDragonDiscipleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass9Align, EvangelistDragonDiscipleClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDragonDiscipleClasspro = EvangelistDragonDiscipleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDragonDiscipleClass10Align, EvangelistDragonDiscipleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDragonDiscipleClasspro.Configure());
            var EvangelistDruidClasspro = ProgressionConfigurator.New(EvangelistDruidClass0Align, EvangelistDruidClass0AlignGuid)
            .SetDisplayName(EvangelistDruidClass0AlignDisplayName)
            .SetDescription(EvangelistDruidClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DruidClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDruidClass2Align, EvangelistDruidClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDruidClass3Align, EvangelistDruidClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDruidClass4Align, EvangelistDruidClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDruidClass5Align, EvangelistDruidClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDruidClass6Align, EvangelistDruidClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDruidClass7Align, EvangelistDruidClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDruidClass8Align, EvangelistDruidClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDruidClass9Align, EvangelistDruidClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDruidClasspro = EvangelistDruidClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDruidClass10Align, EvangelistDruidClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDruidClasspro.Configure());
            var EvangelistDuelistClasspro = ProgressionConfigurator.New(EvangelistDuelistClass0Align, EvangelistDuelistClass0AlignGuid)
            .SetDisplayName(EvangelistDuelistClass0AlignDisplayName)
            .SetDescription(EvangelistDuelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DuelistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDuelistClass2Align, EvangelistDuelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDuelistClass3Align, EvangelistDuelistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDuelistClass4Align, EvangelistDuelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDuelistClass5Align, EvangelistDuelistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDuelistClass6Align, EvangelistDuelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDuelistClass7Align, EvangelistDuelistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDuelistClass8Align, EvangelistDuelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDuelistClass9Align, EvangelistDuelistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistDuelistClasspro = EvangelistDuelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDuelistClass10Align, EvangelistDuelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDuelistClasspro.Configure());
            var EvangelistEldritchKnightClasspro = ProgressionConfigurator.New(EvangelistEldritchKnightClass0Align, EvangelistEldritchKnightClass0AlignGuid)
            .SetDisplayName(EvangelistEldritchKnightClass0AlignDisplayName)
            .SetDescription(EvangelistEldritchKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchKnightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistEldritchKnightClass2Align, EvangelistEldritchKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistEldritchKnightClass3Align, EvangelistEldritchKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistEldritchKnightClass4Align, EvangelistEldritchKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistEldritchKnightClass5Align, EvangelistEldritchKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistEldritchKnightClass6Align, EvangelistEldritchKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistEldritchKnightClass7Align, EvangelistEldritchKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistEldritchKnightClass8Align, EvangelistEldritchKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistEldritchKnightClass9Align, EvangelistEldritchKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchKnightClasspro = EvangelistEldritchKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistEldritchKnightClass10Align, EvangelistEldritchKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistEldritchKnightClasspro.Configure());
            var EvangelistEldritchScionClasspro = ProgressionConfigurator.New(EvangelistEldritchScionClass0Align, EvangelistEldritchScionClass0AlignGuid)
            .SetDisplayName(EvangelistEldritchScionClass0AlignDisplayName)
            .SetDescription(EvangelistEldritchScionClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchScionClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistEldritchScionClass2Align, EvangelistEldritchScionClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistEldritchScionClass3Align, EvangelistEldritchScionClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistEldritchScionClass4Align, EvangelistEldritchScionClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistEldritchScionClass5Align, EvangelistEldritchScionClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistEldritchScionClass6Align, EvangelistEldritchScionClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistEldritchScionClass7Align, EvangelistEldritchScionClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistEldritchScionClass8Align, EvangelistEldritchScionClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistEldritchScionClass9Align, EvangelistEldritchScionClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistEldritchScionClasspro = EvangelistEldritchScionClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistEldritchScionClass10Align, EvangelistEldritchScionClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistEldritchScionClasspro.Configure());
            var EvangelistFighterClasspro = ProgressionConfigurator.New(EvangelistFighterClass0Align, EvangelistFighterClass0AlignGuid)
            .SetDisplayName(EvangelistFighterClass0AlignDisplayName)
            .SetDescription(EvangelistFighterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.FighterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistFighterClass2Align, EvangelistFighterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistFighterClass3Align, EvangelistFighterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistFighterClass4Align, EvangelistFighterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistFighterClass5Align, EvangelistFighterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistFighterClass6Align, EvangelistFighterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistFighterClass7Align, EvangelistFighterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistFighterClass8Align, EvangelistFighterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistFighterClass9Align, EvangelistFighterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistFighterClasspro = EvangelistFighterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistFighterClass10Align, EvangelistFighterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistFighterClasspro.Configure());
            var EvangelistHellknightClasspro = ProgressionConfigurator.New(EvangelistHellknightClass0Align, EvangelistHellknightClass0AlignGuid)
            .SetDisplayName(EvangelistHellknightClass0AlignDisplayName)
            .SetDescription(EvangelistHellknightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHellknightClass2Align, EvangelistHellknightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHellknightClass3Align, EvangelistHellknightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHellknightClass4Align, EvangelistHellknightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHellknightClass5Align, EvangelistHellknightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHellknightClass6Align, EvangelistHellknightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHellknightClass7Align, EvangelistHellknightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHellknightClass8Align, EvangelistHellknightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHellknightClass9Align, EvangelistHellknightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightClasspro = EvangelistHellknightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHellknightClass10Align, EvangelistHellknightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHellknightClasspro.Configure());
            var EvangelistHellknightSigniferClasspro = ProgressionConfigurator.New(EvangelistHellknightSigniferClass0Align, EvangelistHellknightSigniferClass0AlignGuid)
            .SetDisplayName(EvangelistHellknightSigniferClass0AlignDisplayName)
            .SetDescription(EvangelistHellknightSigniferClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightSigniferClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass2Align, EvangelistHellknightSigniferClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass3Align, EvangelistHellknightSigniferClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass4Align, EvangelistHellknightSigniferClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass5Align, EvangelistHellknightSigniferClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass6Align, EvangelistHellknightSigniferClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass7Align, EvangelistHellknightSigniferClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass8Align, EvangelistHellknightSigniferClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass9Align, EvangelistHellknightSigniferClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHellknightSigniferClasspro = EvangelistHellknightSigniferClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHellknightSigniferClass10Align, EvangelistHellknightSigniferClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHellknightSigniferClasspro.Configure());
            var EvangelistHunterClasspro = ProgressionConfigurator.New(EvangelistHunterClass0Align, EvangelistHunterClass0AlignGuid)
            .SetDisplayName(EvangelistHunterClass0AlignDisplayName)
            .SetDescription(EvangelistHunterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HunterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHunterClass2Align, EvangelistHunterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHunterClass3Align, EvangelistHunterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHunterClass4Align, EvangelistHunterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHunterClass5Align, EvangelistHunterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHunterClass6Align, EvangelistHunterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHunterClass7Align, EvangelistHunterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHunterClass8Align, EvangelistHunterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHunterClass9Align, EvangelistHunterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistHunterClasspro = EvangelistHunterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHunterClass10Align, EvangelistHunterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHunterClasspro.Configure());
            var EvangelistInquisitorClasspro = ProgressionConfigurator.New(EvangelistInquisitorClass0Align, EvangelistInquisitorClass0AlignGuid)
            .SetDisplayName(EvangelistInquisitorClass0AlignDisplayName)
            .SetDescription(EvangelistInquisitorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.InquisitorClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistInquisitorClass2Align, EvangelistInquisitorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistInquisitorClass3Align, EvangelistInquisitorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistInquisitorClass4Align, EvangelistInquisitorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistInquisitorClass5Align, EvangelistInquisitorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistInquisitorClass6Align, EvangelistInquisitorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistInquisitorClass7Align, EvangelistInquisitorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistInquisitorClass8Align, EvangelistInquisitorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistInquisitorClass9Align, EvangelistInquisitorClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistInquisitorClasspro = EvangelistInquisitorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistInquisitorClass10Align, EvangelistInquisitorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistInquisitorClasspro.Configure());
            var EvangelistKineticistClasspro = ProgressionConfigurator.New(EvangelistKineticistClass0Align, EvangelistKineticistClass0AlignGuid)
            .SetDisplayName(EvangelistKineticistClass0AlignDisplayName)
            .SetDescription(EvangelistKineticistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistKineticistClass2Align, EvangelistKineticistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistKineticistClass3Align, EvangelistKineticistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistKineticistClass4Align, EvangelistKineticistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistKineticistClass5Align, EvangelistKineticistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistKineticistClass6Align, EvangelistKineticistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistKineticistClass7Align, EvangelistKineticistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistKineticistClass8Align, EvangelistKineticistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistKineticistClass9Align, EvangelistKineticistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistKineticistClasspro = EvangelistKineticistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistKineticistClass10Align, EvangelistKineticistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistKineticistClasspro.Configure());
            var EvangelistLoremasterClasspro = ProgressionConfigurator.New(EvangelistLoremasterClass0Align, EvangelistLoremasterClass0AlignGuid)
            .SetDisplayName(EvangelistLoremasterClass0AlignDisplayName)
            .SetDescription(EvangelistLoremasterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.LoremasterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistLoremasterClass2Align, EvangelistLoremasterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistLoremasterClass3Align, EvangelistLoremasterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistLoremasterClass4Align, EvangelistLoremasterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistLoremasterClass5Align, EvangelistLoremasterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistLoremasterClass6Align, EvangelistLoremasterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistLoremasterClass7Align, EvangelistLoremasterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistLoremasterClass8Align, EvangelistLoremasterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistLoremasterClass9Align, EvangelistLoremasterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistLoremasterClasspro = EvangelistLoremasterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistLoremasterClass10Align, EvangelistLoremasterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistLoremasterClasspro.Configure());
            var EvangelistMagusClasspro = ProgressionConfigurator.New(EvangelistMagusClass0Align, EvangelistMagusClass0AlignGuid)
            .SetDisplayName(EvangelistMagusClass0AlignDisplayName)
            .SetDescription(EvangelistMagusClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMagusClass2Align, EvangelistMagusClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMagusClass3Align, EvangelistMagusClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMagusClass4Align, EvangelistMagusClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMagusClass5Align, EvangelistMagusClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMagusClass6Align, EvangelistMagusClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMagusClass7Align, EvangelistMagusClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMagusClass8Align, EvangelistMagusClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMagusClass9Align, EvangelistMagusClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMagusClasspro = EvangelistMagusClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMagusClass10Align, EvangelistMagusClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMagusClasspro.Configure());
            var EvangelistMonkClasspro = ProgressionConfigurator.New(EvangelistMonkClass0Align, EvangelistMonkClass0AlignGuid)
            .SetDisplayName(EvangelistMonkClass0AlignDisplayName)
            .SetDescription(EvangelistMonkClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMonkClass2Align, EvangelistMonkClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMonkClass3Align, EvangelistMonkClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMonkClass4Align, EvangelistMonkClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMonkClass5Align, EvangelistMonkClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMonkClass6Align, EvangelistMonkClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMonkClass7Align, EvangelistMonkClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMonkClass8Align, EvangelistMonkClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMonkClass9Align, EvangelistMonkClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMonkClasspro = EvangelistMonkClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMonkClass10Align, EvangelistMonkClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMonkClasspro.Configure());
            var EvangelistMysticTheurgeClasspro = ProgressionConfigurator.New(EvangelistMysticTheurgeClass0Align, EvangelistMysticTheurgeClass0AlignGuid)
            .SetDisplayName(EvangelistMysticTheurgeClass0AlignDisplayName)
            .SetDescription(EvangelistMysticTheurgeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MysticTheurgeClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass2Align, EvangelistMysticTheurgeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass3Align, EvangelistMysticTheurgeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass4Align, EvangelistMysticTheurgeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass5Align, EvangelistMysticTheurgeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass6Align, EvangelistMysticTheurgeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass7Align, EvangelistMysticTheurgeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass8Align, EvangelistMysticTheurgeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass9Align, EvangelistMysticTheurgeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistMysticTheurgeClasspro = EvangelistMysticTheurgeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMysticTheurgeClass10Align, EvangelistMysticTheurgeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMysticTheurgeClasspro.Configure());
            var EvangelistOracleClasspro = ProgressionConfigurator.New(EvangelistOracleClass0Align, EvangelistOracleClass0AlignGuid)
            .SetDisplayName(EvangelistOracleClass0AlignDisplayName)
            .SetDescription(EvangelistOracleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.OracleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistOracleClass2Align, EvangelistOracleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistOracleClass3Align, EvangelistOracleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistOracleClass4Align, EvangelistOracleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistOracleClass5Align, EvangelistOracleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistOracleClass6Align, EvangelistOracleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistOracleClass7Align, EvangelistOracleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistOracleClass8Align, EvangelistOracleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistOracleClass9Align, EvangelistOracleClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistOracleClasspro = EvangelistOracleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistOracleClass10Align, EvangelistOracleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistOracleClasspro.Configure());
            var EvangelistPaladinClasspro = ProgressionConfigurator.New(EvangelistPaladinClass0Align, EvangelistPaladinClass0AlignGuid)
            .SetDisplayName(EvangelistPaladinClass0AlignDisplayName)
            .SetDescription(EvangelistPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.PaladinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistPaladinClass2Align, EvangelistPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistPaladinClass3Align, EvangelistPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistPaladinClass4Align, EvangelistPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistPaladinClass5Align, EvangelistPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistPaladinClass6Align, EvangelistPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistPaladinClass7Align, EvangelistPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistPaladinClass8Align, EvangelistPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistPaladinClass9Align, EvangelistPaladinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistPaladinClasspro = EvangelistPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistPaladinClass10Align, EvangelistPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistPaladinClasspro.Configure());
            var EvangelistRangerClasspro = ProgressionConfigurator.New(EvangelistRangerClass0Align, EvangelistRangerClass0AlignGuid)
            .SetDisplayName(EvangelistRangerClass0AlignDisplayName)
            .SetDescription(EvangelistRangerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RangerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistRangerClass2Align, EvangelistRangerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistRangerClass3Align, EvangelistRangerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistRangerClass4Align, EvangelistRangerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistRangerClass5Align, EvangelistRangerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistRangerClass6Align, EvangelistRangerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistRangerClass7Align, EvangelistRangerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistRangerClass8Align, EvangelistRangerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistRangerClass9Align, EvangelistRangerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRangerClasspro = EvangelistRangerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistRangerClass10Align, EvangelistRangerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistRangerClasspro.Configure());
            var EvangelistRogueClasspro = ProgressionConfigurator.New(EvangelistRogueClass0Align, EvangelistRogueClass0AlignGuid)
            .SetDisplayName(EvangelistRogueClass0AlignDisplayName)
            .SetDescription(EvangelistRogueClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RogueClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistRogueClass2Align, EvangelistRogueClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistRogueClass3Align, EvangelistRogueClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistRogueClass4Align, EvangelistRogueClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistRogueClass5Align, EvangelistRogueClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistRogueClass6Align, EvangelistRogueClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistRogueClass7Align, EvangelistRogueClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistRogueClass8Align, EvangelistRogueClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistRogueClass9Align, EvangelistRogueClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistRogueClasspro = EvangelistRogueClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistRogueClass10Align, EvangelistRogueClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistRogueClasspro.Configure());
            var EvangelistShamanClasspro = ProgressionConfigurator.New(EvangelistShamanClass0Align, EvangelistShamanClass0AlignGuid)
            .SetDisplayName(EvangelistShamanClass0AlignDisplayName)
            .SetDescription(EvangelistShamanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShamanClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistShamanClass2Align, EvangelistShamanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistShamanClass3Align, EvangelistShamanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistShamanClass4Align, EvangelistShamanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistShamanClass5Align, EvangelistShamanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistShamanClass6Align, EvangelistShamanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistShamanClass7Align, EvangelistShamanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistShamanClass8Align, EvangelistShamanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistShamanClass9Align, EvangelistShamanClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShamanClasspro = EvangelistShamanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistShamanClass10Align, EvangelistShamanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistShamanClasspro.Configure());
            var EvangelistShifterClasspro = ProgressionConfigurator.New(EvangelistShifterClass0Align, EvangelistShifterClass0AlignGuid)
            .SetDisplayName(EvangelistShifterClass0AlignDisplayName)
            .SetDescription(EvangelistShifterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShifterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistShifterClass2Align, EvangelistShifterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistShifterClass3Align, EvangelistShifterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistShifterClass4Align, EvangelistShifterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistShifterClass5Align, EvangelistShifterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistShifterClass6Align, EvangelistShifterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistShifterClass7Align, EvangelistShifterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistShifterClass8Align, EvangelistShifterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistShifterClass9Align, EvangelistShifterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistShifterClasspro = EvangelistShifterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistShifterClass10Align, EvangelistShifterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistShifterClasspro.Configure());
            var EvangelistSkaldClasspro = ProgressionConfigurator.New(EvangelistSkaldClass0Align, EvangelistSkaldClass0AlignGuid)
            .SetDisplayName(EvangelistSkaldClass0AlignDisplayName)
            .SetDescription(EvangelistSkaldClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SkaldClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSkaldClass2Align, EvangelistSkaldClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSkaldClass3Align, EvangelistSkaldClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSkaldClass4Align, EvangelistSkaldClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSkaldClass5Align, EvangelistSkaldClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSkaldClass6Align, EvangelistSkaldClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSkaldClass7Align, EvangelistSkaldClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSkaldClass8Align, EvangelistSkaldClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSkaldClass9Align, EvangelistSkaldClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSkaldClasspro = EvangelistSkaldClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSkaldClass10Align, EvangelistSkaldClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSkaldClasspro.Configure());
            var EvangelistSlayerClasspro = ProgressionConfigurator.New(EvangelistSlayerClass0Align, EvangelistSlayerClass0AlignGuid)
            .SetDisplayName(EvangelistSlayerClass0AlignDisplayName)
            .SetDescription(EvangelistSlayerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SlayerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSlayerClass2Align, EvangelistSlayerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSlayerClass3Align, EvangelistSlayerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSlayerClass4Align, EvangelistSlayerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSlayerClass5Align, EvangelistSlayerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSlayerClass6Align, EvangelistSlayerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSlayerClass7Align, EvangelistSlayerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSlayerClass8Align, EvangelistSlayerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSlayerClass9Align, EvangelistSlayerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSlayerClasspro = EvangelistSlayerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSlayerClass10Align, EvangelistSlayerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSlayerClasspro.Configure());
            var EvangelistSorcererClasspro = ProgressionConfigurator.New(EvangelistSorcererClass0Align, EvangelistSorcererClass0AlignGuid)
            .SetDisplayName(EvangelistSorcererClass0AlignDisplayName)
            .SetDescription(EvangelistSorcererClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SorcererClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSorcererClass2Align, EvangelistSorcererClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSorcererClass3Align, EvangelistSorcererClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSorcererClass4Align, EvangelistSorcererClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSorcererClass5Align, EvangelistSorcererClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSorcererClass6Align, EvangelistSorcererClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSorcererClass7Align, EvangelistSorcererClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSorcererClass8Align, EvangelistSorcererClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSorcererClass9Align, EvangelistSorcererClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSorcererClasspro = EvangelistSorcererClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSorcererClass10Align, EvangelistSorcererClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSorcererClasspro.Configure());
            var EvangelistStalwartDefenderClasspro = ProgressionConfigurator.New(EvangelistStalwartDefenderClass0Align, EvangelistStalwartDefenderClass0AlignGuid)
            .SetDisplayName(EvangelistStalwartDefenderClass0AlignDisplayName)
            .SetDescription(EvangelistStalwartDefenderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StalwartDefenderClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass2Align, EvangelistStalwartDefenderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass3Align, EvangelistStalwartDefenderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass4Align, EvangelistStalwartDefenderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass5Align, EvangelistStalwartDefenderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass6Align, EvangelistStalwartDefenderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass7Align, EvangelistStalwartDefenderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass8Align, EvangelistStalwartDefenderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass9Align, EvangelistStalwartDefenderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStalwartDefenderClasspro = EvangelistStalwartDefenderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistStalwartDefenderClass10Align, EvangelistStalwartDefenderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistStalwartDefenderClasspro.Configure());
            var EvangelistStudentOfWarClasspro = ProgressionConfigurator.New(EvangelistStudentOfWarClass0Align, EvangelistStudentOfWarClass0AlignGuid)
            .SetDisplayName(EvangelistStudentOfWarClass0AlignDisplayName)
            .SetDescription(EvangelistStudentOfWarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StudentOfWarClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistStudentOfWarClass2Align, EvangelistStudentOfWarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistStudentOfWarClass3Align, EvangelistStudentOfWarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistStudentOfWarClass4Align, EvangelistStudentOfWarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistStudentOfWarClass5Align, EvangelistStudentOfWarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistStudentOfWarClass6Align, EvangelistStudentOfWarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistStudentOfWarClass7Align, EvangelistStudentOfWarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistStudentOfWarClass8Align, EvangelistStudentOfWarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistStudentOfWarClass9Align, EvangelistStudentOfWarClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistStudentOfWarClasspro = EvangelistStudentOfWarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistStudentOfWarClass10Align, EvangelistStudentOfWarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistStudentOfWarClasspro.Configure());
            var EvangelistSwordlordClasspro = ProgressionConfigurator.New(EvangelistSwordlordClass0Align, EvangelistSwordlordClass0AlignGuid)
            .SetDisplayName(EvangelistSwordlordClass0AlignDisplayName)
            .SetDescription(EvangelistSwordlordClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SwordlordClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSwordlordClass2Align, EvangelistSwordlordClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSwordlordClass3Align, EvangelistSwordlordClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSwordlordClass4Align, EvangelistSwordlordClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSwordlordClass5Align, EvangelistSwordlordClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSwordlordClass6Align, EvangelistSwordlordClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSwordlordClass7Align, EvangelistSwordlordClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSwordlordClass8Align, EvangelistSwordlordClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSwordlordClass9Align, EvangelistSwordlordClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistSwordlordClasspro = EvangelistSwordlordClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSwordlordClass10Align, EvangelistSwordlordClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSwordlordClasspro.Configure());
            var EvangelistWarpriestClasspro = ProgressionConfigurator.New(EvangelistWarpriestClass0Align, EvangelistWarpriestClass0AlignGuid)
            .SetDisplayName(EvangelistWarpriestClass0AlignDisplayName)
            .SetDescription(EvangelistWarpriestClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WarpriestClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistWarpriestClass2Align, EvangelistWarpriestClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistWarpriestClass3Align, EvangelistWarpriestClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistWarpriestClass4Align, EvangelistWarpriestClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistWarpriestClass5Align, EvangelistWarpriestClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistWarpriestClass6Align, EvangelistWarpriestClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistWarpriestClass7Align, EvangelistWarpriestClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistWarpriestClass8Align, EvangelistWarpriestClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistWarpriestClass9Align, EvangelistWarpriestClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWarpriestClasspro = EvangelistWarpriestClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistWarpriestClass10Align, EvangelistWarpriestClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistWarpriestClasspro.Configure());
            var EvangelistWinterWitchClasspro = ProgressionConfigurator.New(EvangelistWinterWitchClass0Align, EvangelistWinterWitchClass0AlignGuid)
            .SetDisplayName(EvangelistWinterWitchClass0AlignDisplayName)
            .SetDescription(EvangelistWinterWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WinterWitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistWinterWitchClass2Align, EvangelistWinterWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistWinterWitchClass3Align, EvangelistWinterWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistWinterWitchClass4Align, EvangelistWinterWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistWinterWitchClass5Align, EvangelistWinterWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistWinterWitchClass6Align, EvangelistWinterWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistWinterWitchClass7Align, EvangelistWinterWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistWinterWitchClass8Align, EvangelistWinterWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistWinterWitchClass9Align, EvangelistWinterWitchClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWinterWitchClasspro = EvangelistWinterWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistWinterWitchClass10Align, EvangelistWinterWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistWinterWitchClasspro.Configure());
            var EvangelistWitchClasspro = ProgressionConfigurator.New(EvangelistWitchClass0Align, EvangelistWitchClass0AlignGuid)
            .SetDisplayName(EvangelistWitchClass0AlignDisplayName)
            .SetDescription(EvangelistWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistWitchClass2Align, EvangelistWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistWitchClass3Align, EvangelistWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistWitchClass4Align, EvangelistWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistWitchClass5Align, EvangelistWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistWitchClass6Align, EvangelistWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistWitchClass7Align, EvangelistWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistWitchClass8Align, EvangelistWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistWitchClass9Align, EvangelistWitchClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWitchClasspro = EvangelistWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistWitchClass10Align, EvangelistWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistWitchClasspro.Configure());
            var EvangelistWizardClasspro = ProgressionConfigurator.New(EvangelistWizardClass0Align, EvangelistWizardClass0AlignGuid)
            .SetDisplayName(EvangelistWizardClass0AlignDisplayName)
            .SetDescription(EvangelistWizardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WizardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistWizardClass2Align, EvangelistWizardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistWizardClass3Align, EvangelistWizardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistWizardClass4Align, EvangelistWizardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistWizardClass5Align, EvangelistWizardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistWizardClass6Align, EvangelistWizardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistWizardClass7Align, EvangelistWizardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistWizardClass8Align, EvangelistWizardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistWizardClass9Align, EvangelistWizardClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            EvangelistWizardClasspro = EvangelistWizardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistWizardClass10Align, EvangelistWizardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistWizardClasspro.Configure());
            var EvangelistGunslingerClasspro = ProgressionConfigurator.New(EvangelistGunslingerClass0Align, EvangelistGunslingerClass0AlignGuid)
            .SetDisplayName(EvangelistGunslingerClass0AlignDisplayName)
            .SetDescription(EvangelistGunslingerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GunslingerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistGunslingerClass2Align, EvangelistGunslingerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistGunslingerClass3Align, EvangelistGunslingerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistGunslingerClass4Align, EvangelistGunslingerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistGunslingerClass5Align, EvangelistGunslingerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistGunslingerClass6Align, EvangelistGunslingerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistGunslingerClass7Align, EvangelistGunslingerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistGunslingerClass8Align, EvangelistGunslingerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistGunslingerClass9Align, EvangelistGunslingerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            EvangelistGunslingerClasspro = EvangelistGunslingerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistGunslingerClass10Align, EvangelistGunslingerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistGunslingerClasspro.Configure());
            var EvangelistAgentoftheGraveClasspro = ProgressionConfigurator.New(EvangelistAgentoftheGraveClass0Align, EvangelistAgentoftheGraveClass0AlignGuid)
            .SetDisplayName(EvangelistAgentoftheGraveClass0AlignDisplayName)
            .SetDescription(EvangelistAgentoftheGraveClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AgentoftheGraveClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass2Align, EvangelistAgentoftheGraveClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass3Align, EvangelistAgentoftheGraveClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass4Align, EvangelistAgentoftheGraveClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass5Align, EvangelistAgentoftheGraveClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass6Align, EvangelistAgentoftheGraveClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass7Align, EvangelistAgentoftheGraveClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass8Align, EvangelistAgentoftheGraveClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass9Align, EvangelistAgentoftheGraveClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            EvangelistAgentoftheGraveClasspro = EvangelistAgentoftheGraveClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistAgentoftheGraveClass10Align, EvangelistAgentoftheGraveClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistAgentoftheGraveClasspro.Configure());
            var EvangelistAnchoriteofDawnClasspro = ProgressionConfigurator.New(EvangelistAnchoriteofDawnClass0Align, EvangelistAnchoriteofDawnClass0AlignGuid)
            .SetDisplayName(EvangelistAnchoriteofDawnClass0AlignDisplayName)
            .SetDescription(EvangelistAnchoriteofDawnClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AnchoriteofDawnClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass2Align, EvangelistAnchoriteofDawnClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass3Align, EvangelistAnchoriteofDawnClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass4Align, EvangelistAnchoriteofDawnClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass5Align, EvangelistAnchoriteofDawnClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass6Align, EvangelistAnchoriteofDawnClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass7Align, EvangelistAnchoriteofDawnClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass8Align, EvangelistAnchoriteofDawnClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass9Align, EvangelistAnchoriteofDawnClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            EvangelistAnchoriteofDawnClasspro = EvangelistAnchoriteofDawnClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistAnchoriteofDawnClass10Align, EvangelistAnchoriteofDawnClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistAnchoriteofDawnClasspro.Configure());
            var EvangelistArcaneAcherClasspro = ProgressionConfigurator.New(EvangelistArcaneAcherClass0Align, EvangelistArcaneAcherClass0AlignGuid)
            .SetDisplayName(EvangelistArcaneAcherClass0AlignDisplayName)
            .SetDescription(EvangelistArcaneAcherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ArcaneAcherClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistArcaneAcherClass2Align, EvangelistArcaneAcherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistArcaneAcherClass3Align, EvangelistArcaneAcherClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistArcaneAcherClass4Align, EvangelistArcaneAcherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistArcaneAcherClass5Align, EvangelistArcaneAcherClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistArcaneAcherClass6Align, EvangelistArcaneAcherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistArcaneAcherClass7Align, EvangelistArcaneAcherClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistArcaneAcherClass8Align, EvangelistArcaneAcherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistArcaneAcherClass9Align, EvangelistArcaneAcherClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            EvangelistArcaneAcherClasspro = EvangelistArcaneAcherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistArcaneAcherClass10Align, EvangelistArcaneAcherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistArcaneAcherClasspro.Configure());
            var EvangelistAsavirClasspro = ProgressionConfigurator.New(EvangelistAsavirClass0Align, EvangelistAsavirClass0AlignGuid)
            .SetDisplayName(EvangelistAsavirClass0AlignDisplayName)
            .SetDescription(EvangelistAsavirClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AsavirClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistAsavirClass2Align, EvangelistAsavirClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistAsavirClass3Align, EvangelistAsavirClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistAsavirClass4Align, EvangelistAsavirClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistAsavirClass5Align, EvangelistAsavirClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistAsavirClass6Align, EvangelistAsavirClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistAsavirClass7Align, EvangelistAsavirClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistAsavirClass8Align, EvangelistAsavirClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistAsavirClass9Align, EvangelistAsavirClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            EvangelistAsavirClasspro = EvangelistAsavirClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistAsavirClass10Align, EvangelistAsavirClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistAsavirClasspro.Configure());
            var EvangelistChevalierClasspro = ProgressionConfigurator.New(EvangelistChevalierClass0Align, EvangelistChevalierClass0AlignGuid)
            .SetDisplayName(EvangelistChevalierClass0AlignDisplayName)
            .SetDescription(EvangelistChevalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ChevalierClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistChevalierClass2Align, EvangelistChevalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistChevalierClass3Align, EvangelistChevalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistChevalierClass4Align, EvangelistChevalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistChevalierClass5Align, EvangelistChevalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistChevalierClass6Align, EvangelistChevalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistChevalierClass7Align, EvangelistChevalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistChevalierClass8Align, EvangelistChevalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistChevalierClass9Align, EvangelistChevalierClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            EvangelistChevalierClasspro = EvangelistChevalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistChevalierClass10Align, EvangelistChevalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistChevalierClasspro.Configure());
            var EvangelistCrimsonTemplarClasspro = ProgressionConfigurator.New(EvangelistCrimsonTemplarClass0Align, EvangelistCrimsonTemplarClass0AlignGuid)
            .SetDisplayName(EvangelistCrimsonTemplarClass0AlignDisplayName)
            .SetDescription(EvangelistCrimsonTemplarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CrimsonTemplarClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass2Align, EvangelistCrimsonTemplarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass3Align, EvangelistCrimsonTemplarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass4Align, EvangelistCrimsonTemplarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass5Align, EvangelistCrimsonTemplarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass6Align, EvangelistCrimsonTemplarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass7Align, EvangelistCrimsonTemplarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass8Align, EvangelistCrimsonTemplarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass9Align, EvangelistCrimsonTemplarClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            EvangelistCrimsonTemplarClasspro = EvangelistCrimsonTemplarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistCrimsonTemplarClass10Align, EvangelistCrimsonTemplarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistCrimsonTemplarClasspro.Configure());
            var EvangelistDeadeyeDevoteeClasspro = ProgressionConfigurator.New(EvangelistDeadeyeDevoteeClass0Align, EvangelistDeadeyeDevoteeClass0AlignGuid)
            .SetDisplayName(EvangelistDeadeyeDevoteeClass0AlignDisplayName)
            .SetDescription(EvangelistDeadeyeDevoteeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DeadeyeDevoteeClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass2Align, EvangelistDeadeyeDevoteeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass3Align, EvangelistDeadeyeDevoteeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass4Align, EvangelistDeadeyeDevoteeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass5Align, EvangelistDeadeyeDevoteeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass6Align, EvangelistDeadeyeDevoteeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass7Align, EvangelistDeadeyeDevoteeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass8Align, EvangelistDeadeyeDevoteeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass9Align, EvangelistDeadeyeDevoteeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            EvangelistDeadeyeDevoteeClasspro = EvangelistDeadeyeDevoteeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDeadeyeDevoteeClass10Align, EvangelistDeadeyeDevoteeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDeadeyeDevoteeClasspro.Configure());
            var EvangelistDragonFuryClasspro = ProgressionConfigurator.New(EvangelistDragonFuryClass0Align, EvangelistDragonFuryClass0AlignGuid)
            .SetDisplayName(EvangelistDragonFuryClass0AlignDisplayName)
            .SetDescription(EvangelistDragonFuryClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DragonFuryClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDragonFuryClass2Align, EvangelistDragonFuryClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDragonFuryClass3Align, EvangelistDragonFuryClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDragonFuryClass4Align, EvangelistDragonFuryClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDragonFuryClass5Align, EvangelistDragonFuryClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDragonFuryClass6Align, EvangelistDragonFuryClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDragonFuryClass7Align, EvangelistDragonFuryClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDragonFuryClass8Align, EvangelistDragonFuryClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDragonFuryClass9Align, EvangelistDragonFuryClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            EvangelistDragonFuryClasspro = EvangelistDragonFuryClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDragonFuryClass10Align, EvangelistDragonFuryClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDragonFuryClasspro.Configure());
            var EvangelistEsotericKnightClasspro = ProgressionConfigurator.New(EvangelistEsotericKnightClass0Align, EvangelistEsotericKnightClass0AlignGuid)
            .SetDisplayName(EvangelistEsotericKnightClass0AlignDisplayName)
            .SetDescription(EvangelistEsotericKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EsotericKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistEsotericKnightClass2Align, EvangelistEsotericKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistEsotericKnightClass3Align, EvangelistEsotericKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistEsotericKnightClass4Align, EvangelistEsotericKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistEsotericKnightClass5Align, EvangelistEsotericKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistEsotericKnightClass6Align, EvangelistEsotericKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistEsotericKnightClass7Align, EvangelistEsotericKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistEsotericKnightClass8Align, EvangelistEsotericKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistEsotericKnightClass9Align, EvangelistEsotericKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistEsotericKnightClasspro = EvangelistEsotericKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistEsotericKnightClass10Align, EvangelistEsotericKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistEsotericKnightClasspro.Configure());
            var EvangelistFuriousGuardianClasspro = ProgressionConfigurator.New(EvangelistFuriousGuardianClass0Align, EvangelistFuriousGuardianClass0AlignGuid)
            .SetDisplayName(EvangelistFuriousGuardianClass0AlignDisplayName)
            .SetDescription(EvangelistFuriousGuardianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(FuriousGuardianClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass2Align, EvangelistFuriousGuardianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass3Align, EvangelistFuriousGuardianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass4Align, EvangelistFuriousGuardianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass5Align, EvangelistFuriousGuardianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass6Align, EvangelistFuriousGuardianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass7Align, EvangelistFuriousGuardianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass8Align, EvangelistFuriousGuardianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass9Align, EvangelistFuriousGuardianClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            EvangelistFuriousGuardianClasspro = EvangelistFuriousGuardianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistFuriousGuardianClass10Align, EvangelistFuriousGuardianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistFuriousGuardianClasspro.Configure());
            var EvangelistHalflingOpportunistClasspro = ProgressionConfigurator.New(EvangelistHalflingOpportunistClass0Align, EvangelistHalflingOpportunistClass0AlignGuid)
            .SetDisplayName(EvangelistHalflingOpportunistClass0AlignDisplayName)
            .SetDescription(EvangelistHalflingOpportunistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HalflingOpportunistClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass2Align, EvangelistHalflingOpportunistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass3Align, EvangelistHalflingOpportunistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass4Align, EvangelistHalflingOpportunistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass5Align, EvangelistHalflingOpportunistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass6Align, EvangelistHalflingOpportunistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass7Align, EvangelistHalflingOpportunistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass8Align, EvangelistHalflingOpportunistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass9Align, EvangelistHalflingOpportunistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            EvangelistHalflingOpportunistClasspro = EvangelistHalflingOpportunistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHalflingOpportunistClass10Align, EvangelistHalflingOpportunistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHalflingOpportunistClasspro.Configure());
            var EvangelistHinterlanderClasspro = ProgressionConfigurator.New(EvangelistHinterlanderClass0Align, EvangelistHinterlanderClass0AlignGuid)
            .SetDisplayName(EvangelistHinterlanderClass0AlignDisplayName)
            .SetDescription(EvangelistHinterlanderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HinterlanderClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHinterlanderClass2Align, EvangelistHinterlanderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHinterlanderClass3Align, EvangelistHinterlanderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHinterlanderClass4Align, EvangelistHinterlanderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHinterlanderClass5Align, EvangelistHinterlanderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHinterlanderClass6Align, EvangelistHinterlanderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHinterlanderClass7Align, EvangelistHinterlanderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHinterlanderClass8Align, EvangelistHinterlanderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHinterlanderClass9Align, EvangelistHinterlanderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            EvangelistHinterlanderClasspro = EvangelistHinterlanderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHinterlanderClass10Align, EvangelistHinterlanderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHinterlanderClasspro.Configure());
            var EvangelistHorizonWalkerClasspro = ProgressionConfigurator.New(EvangelistHorizonWalkerClass0Align, EvangelistHorizonWalkerClass0AlignGuid)
            .SetDisplayName(EvangelistHorizonWalkerClass0AlignDisplayName)
            .SetDescription(EvangelistHorizonWalkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HorizonWalkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass2Align, EvangelistHorizonWalkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass3Align, EvangelistHorizonWalkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass4Align, EvangelistHorizonWalkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass5Align, EvangelistHorizonWalkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass6Align, EvangelistHorizonWalkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass7Align, EvangelistHorizonWalkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass8Align, EvangelistHorizonWalkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass9Align, EvangelistHorizonWalkerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistHorizonWalkerClasspro = EvangelistHorizonWalkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHorizonWalkerClass10Align, EvangelistHorizonWalkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHorizonWalkerClasspro.Configure());
            var EvangelistInheritorCrusaderClasspro = ProgressionConfigurator.New(EvangelistInheritorCrusaderClass0Align, EvangelistInheritorCrusaderClass0AlignGuid)
            .SetDisplayName(EvangelistInheritorCrusaderClass0AlignDisplayName)
            .SetDescription(EvangelistInheritorCrusaderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(InheritorCrusaderClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass2Align, EvangelistInheritorCrusaderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass3Align, EvangelistInheritorCrusaderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass4Align, EvangelistInheritorCrusaderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass5Align, EvangelistInheritorCrusaderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass6Align, EvangelistInheritorCrusaderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass7Align, EvangelistInheritorCrusaderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass8Align, EvangelistInheritorCrusaderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass9Align, EvangelistInheritorCrusaderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            EvangelistInheritorCrusaderClasspro = EvangelistInheritorCrusaderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistInheritorCrusaderClass10Align, EvangelistInheritorCrusaderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistInheritorCrusaderClasspro.Configure());
            var EvangelistMammothRiderClasspro = ProgressionConfigurator.New(EvangelistMammothRiderClass0Align, EvangelistMammothRiderClass0AlignGuid)
            .SetDisplayName(EvangelistMammothRiderClass0AlignDisplayName)
            .SetDescription(EvangelistMammothRiderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MammothRiderClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMammothRiderClass2Align, EvangelistMammothRiderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMammothRiderClass3Align, EvangelistMammothRiderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMammothRiderClass4Align, EvangelistMammothRiderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMammothRiderClass5Align, EvangelistMammothRiderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMammothRiderClass6Align, EvangelistMammothRiderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMammothRiderClass7Align, EvangelistMammothRiderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMammothRiderClass8Align, EvangelistMammothRiderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMammothRiderClass9Align, EvangelistMammothRiderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            EvangelistMammothRiderClasspro = EvangelistMammothRiderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMammothRiderClass10Align, EvangelistMammothRiderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMammothRiderClasspro.Configure());
            var EvangelistSanguineAngelClasspro = ProgressionConfigurator.New(EvangelistSanguineAngelClass0Align, EvangelistSanguineAngelClass0AlignGuid)
            .SetDisplayName(EvangelistSanguineAngelClass0AlignDisplayName)
            .SetDescription(EvangelistSanguineAngelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SanguineAngelClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSanguineAngelClass2Align, EvangelistSanguineAngelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSanguineAngelClass3Align, EvangelistSanguineAngelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSanguineAngelClass4Align, EvangelistSanguineAngelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSanguineAngelClass5Align, EvangelistSanguineAngelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSanguineAngelClass6Align, EvangelistSanguineAngelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSanguineAngelClass7Align, EvangelistSanguineAngelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSanguineAngelClass8Align, EvangelistSanguineAngelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSanguineAngelClass9Align, EvangelistSanguineAngelClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSanguineAngelClasspro = EvangelistSanguineAngelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSanguineAngelClass10Align, EvangelistSanguineAngelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSanguineAngelClasspro.Configure());
            var EvangelistScarSeekerClasspro = ProgressionConfigurator.New(EvangelistScarSeekerClass0Align, EvangelistScarSeekerClass0AlignGuid)
            .SetDisplayName(EvangelistScarSeekerClass0AlignDisplayName)
            .SetDescription(EvangelistScarSeekerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ScarSeekerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistScarSeekerClass2Align, EvangelistScarSeekerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistScarSeekerClass3Align, EvangelistScarSeekerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistScarSeekerClass4Align, EvangelistScarSeekerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistScarSeekerClass5Align, EvangelistScarSeekerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistScarSeekerClass6Align, EvangelistScarSeekerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistScarSeekerClass7Align, EvangelistScarSeekerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistScarSeekerClass8Align, EvangelistScarSeekerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistScarSeekerClass9Align, EvangelistScarSeekerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            EvangelistScarSeekerClasspro = EvangelistScarSeekerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistScarSeekerClass10Align, EvangelistScarSeekerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistScarSeekerClasspro.Configure());
            var EvangelistSentinelClasspro = ProgressionConfigurator.New(EvangelistSentinelClass0Align, EvangelistSentinelClass0AlignGuid)
            .SetDisplayName(EvangelistSentinelClass0AlignDisplayName)
            .SetDescription(EvangelistSentinelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SentinelClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSentinelClass2Align, EvangelistSentinelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSentinelClass3Align, EvangelistSentinelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSentinelClass4Align, EvangelistSentinelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSentinelClass5Align, EvangelistSentinelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSentinelClass6Align, EvangelistSentinelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSentinelClass7Align, EvangelistSentinelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSentinelClass8Align, EvangelistSentinelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSentinelClass9Align, EvangelistSentinelClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            EvangelistSentinelClasspro = EvangelistSentinelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSentinelClass10Align, EvangelistSentinelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSentinelClasspro.Configure());
            var EvangelistShadowDancerClasspro = ProgressionConfigurator.New(EvangelistShadowDancerClass0Align, EvangelistShadowDancerClass0AlignGuid)
            .SetDisplayName(EvangelistShadowDancerClass0AlignDisplayName)
            .SetDescription(EvangelistShadowDancerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ShadowDancerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistShadowDancerClass2Align, EvangelistShadowDancerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistShadowDancerClass3Align, EvangelistShadowDancerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistShadowDancerClass4Align, EvangelistShadowDancerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistShadowDancerClass5Align, EvangelistShadowDancerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistShadowDancerClass6Align, EvangelistShadowDancerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistShadowDancerClass7Align, EvangelistShadowDancerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistShadowDancerClass8Align, EvangelistShadowDancerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistShadowDancerClass9Align, EvangelistShadowDancerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            EvangelistShadowDancerClasspro = EvangelistShadowDancerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistShadowDancerClass10Align, EvangelistShadowDancerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistShadowDancerClasspro.Configure());
            var EvangelistSouldrinkerClasspro = ProgressionConfigurator.New(EvangelistSouldrinkerClass0Align, EvangelistSouldrinkerClass0AlignGuid)
            .SetDisplayName(EvangelistSouldrinkerClass0AlignDisplayName)
            .SetDescription(EvangelistSouldrinkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SouldrinkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSouldrinkerClass2Align, EvangelistSouldrinkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSouldrinkerClass3Align, EvangelistSouldrinkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSouldrinkerClass4Align, EvangelistSouldrinkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSouldrinkerClass5Align, EvangelistSouldrinkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSouldrinkerClass6Align, EvangelistSouldrinkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSouldrinkerClass7Align, EvangelistSouldrinkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSouldrinkerClass8Align, EvangelistSouldrinkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSouldrinkerClass9Align, EvangelistSouldrinkerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSouldrinkerClasspro = EvangelistSouldrinkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSouldrinkerClass10Align, EvangelistSouldrinkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSouldrinkerClasspro.Configure());
            var EvangelistUmbralAgentClasspro = ProgressionConfigurator.New(EvangelistUmbralAgentClass0Align, EvangelistUmbralAgentClass0AlignGuid)
            .SetDisplayName(EvangelistUmbralAgentClass0AlignDisplayName)
            .SetDescription(EvangelistUmbralAgentClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(UmbralAgentClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistUmbralAgentClass2Align, EvangelistUmbralAgentClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistUmbralAgentClass3Align, EvangelistUmbralAgentClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistUmbralAgentClass4Align, EvangelistUmbralAgentClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistUmbralAgentClass5Align, EvangelistUmbralAgentClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistUmbralAgentClass6Align, EvangelistUmbralAgentClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistUmbralAgentClass7Align, EvangelistUmbralAgentClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistUmbralAgentClass8Align, EvangelistUmbralAgentClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistUmbralAgentClass9Align, EvangelistUmbralAgentClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            EvangelistUmbralAgentClasspro = EvangelistUmbralAgentClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistUmbralAgentClass10Align, EvangelistUmbralAgentClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistUmbralAgentClasspro.Configure());
            var EvangelistMicroAntiPaladinClasspro = ProgressionConfigurator.New(EvangelistMicroAntiPaladinClass0Align, EvangelistMicroAntiPaladinClass0AlignGuid)
            .SetDisplayName(EvangelistMicroAntiPaladinClass0AlignDisplayName)
            .SetDescription(EvangelistMicroAntiPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MicroAntiPaladinClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass2Align, EvangelistMicroAntiPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass3Align, EvangelistMicroAntiPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass4Align, EvangelistMicroAntiPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass5Align, EvangelistMicroAntiPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass6Align, EvangelistMicroAntiPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass7Align, EvangelistMicroAntiPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass8Align, EvangelistMicroAntiPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass9Align, EvangelistMicroAntiPaladinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            EvangelistMicroAntiPaladinClasspro = EvangelistMicroAntiPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMicroAntiPaladinClass10Align, EvangelistMicroAntiPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMicroAntiPaladinClasspro.Configure());
            var EvangelistOathbreakerClasspro = ProgressionConfigurator.New(EvangelistOathbreakerClass0Align, EvangelistOathbreakerClass0AlignGuid)
            .SetDisplayName(EvangelistOathbreakerClass0AlignDisplayName)
            .SetDescription(EvangelistOathbreakerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(OathbreakerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistOathbreakerClass2Align, EvangelistOathbreakerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistOathbreakerClass3Align, EvangelistOathbreakerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistOathbreakerClass4Align, EvangelistOathbreakerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistOathbreakerClass5Align, EvangelistOathbreakerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistOathbreakerClass6Align, EvangelistOathbreakerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistOathbreakerClass7Align, EvangelistOathbreakerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistOathbreakerClass8Align, EvangelistOathbreakerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistOathbreakerClass9Align, EvangelistOathbreakerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            EvangelistOathbreakerClasspro = EvangelistOathbreakerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistOathbreakerClass10Align, EvangelistOathbreakerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistOathbreakerClasspro.Configure());
            var EvangelistDreadKnightClasspro = ProgressionConfigurator.New(EvangelistDreadKnightClass0Align, EvangelistDreadKnightClass0AlignGuid)
            .SetDisplayName(EvangelistDreadKnightClass0AlignDisplayName)
            .SetDescription(EvangelistDreadKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DreadKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistDreadKnightClass2Align, EvangelistDreadKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistDreadKnightClass3Align, EvangelistDreadKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistDreadKnightClass4Align, EvangelistDreadKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistDreadKnightClass5Align, EvangelistDreadKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistDreadKnightClass6Align, EvangelistDreadKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistDreadKnightClass7Align, EvangelistDreadKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistDreadKnightClass8Align, EvangelistDreadKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistDreadKnightClass9Align, EvangelistDreadKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistDreadKnightClasspro = EvangelistDreadKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistDreadKnightClass10Align, EvangelistDreadKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistDreadKnightClasspro.Configure());
            var EvangelistStargazerClasspro = ProgressionConfigurator.New(EvangelistStargazerClass0Align, EvangelistStargazerClass0AlignGuid)
            .SetDisplayName(EvangelistStargazerClass0AlignDisplayName)
            .SetDescription(EvangelistStargazerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(StargazerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistStargazerClass2Align, EvangelistStargazerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistStargazerClass3Align, EvangelistStargazerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistStargazerClass4Align, EvangelistStargazerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistStargazerClass5Align, EvangelistStargazerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistStargazerClass6Align, EvangelistStargazerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistStargazerClass7Align, EvangelistStargazerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistStargazerClass8Align, EvangelistStargazerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistStargazerClass9Align, EvangelistStargazerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            EvangelistStargazerClasspro = EvangelistStargazerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistStargazerClass10Align, EvangelistStargazerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistStargazerClasspro.Configure());
            var EvangelistSwashbucklerClasspro = ProgressionConfigurator.New(EvangelistSwashbucklerClass0Align, EvangelistSwashbucklerClass0AlignGuid)
            .SetDisplayName(EvangelistSwashbucklerClass0AlignDisplayName)
            .SetDescription(EvangelistSwashbucklerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SwashbucklerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSwashbucklerClass2Align, EvangelistSwashbucklerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSwashbucklerClass3Align, EvangelistSwashbucklerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSwashbucklerClass4Align, EvangelistSwashbucklerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSwashbucklerClass5Align, EvangelistSwashbucklerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSwashbucklerClass6Align, EvangelistSwashbucklerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSwashbucklerClass7Align, EvangelistSwashbucklerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSwashbucklerClass8Align, EvangelistSwashbucklerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSwashbucklerClass9Align, EvangelistSwashbucklerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSwashbucklerClasspro = EvangelistSwashbucklerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSwashbucklerClass10Align, EvangelistSwashbucklerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSwashbucklerClasspro.Configure());
            var EvangelistHolyVindicatorClasspro = ProgressionConfigurator.New(EvangelistHolyVindicatorClass0Align, EvangelistHolyVindicatorClass0AlignGuid)
            .SetDisplayName(EvangelistHolyVindicatorClass0AlignDisplayName)
            .SetDescription(EvangelistHolyVindicatorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HolyVindicatorClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass2Align, EvangelistHolyVindicatorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass3Align, EvangelistHolyVindicatorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass4Align, EvangelistHolyVindicatorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass5Align, EvangelistHolyVindicatorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass6Align, EvangelistHolyVindicatorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass7Align, EvangelistHolyVindicatorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass8Align, EvangelistHolyVindicatorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass9Align, EvangelistHolyVindicatorClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            EvangelistHolyVindicatorClasspro = EvangelistHolyVindicatorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHolyVindicatorClass10Align, EvangelistHolyVindicatorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHolyVindicatorClasspro.Configure());
            var EvangelistSummonerClasspro = ProgressionConfigurator.New(EvangelistSummonerClass0Align, EvangelistSummonerClass0AlignGuid)
            .SetDisplayName(EvangelistSummonerClass0AlignDisplayName)
            .SetDescription(EvangelistSummonerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SummonerClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistSummonerClass2Align, EvangelistSummonerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistSummonerClass3Align, EvangelistSummonerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistSummonerClass4Align, EvangelistSummonerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistSummonerClass5Align, EvangelistSummonerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistSummonerClass6Align, EvangelistSummonerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistSummonerClass7Align, EvangelistSummonerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistSummonerClass8Align, EvangelistSummonerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistSummonerClass9Align, EvangelistSummonerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            EvangelistSummonerClasspro = EvangelistSummonerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistSummonerClass10Align, EvangelistSummonerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistSummonerClasspro.Configure());
            var EvangelistLionBladeClasspro = ProgressionConfigurator.New(EvangelistLionBladeClass0Align, EvangelistLionBladeClass0AlignGuid)
            .SetDisplayName(EvangelistLionBladeClass0AlignDisplayName)
            .SetDescription(EvangelistLionBladeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(LionBladeClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistLionBladeClass2Align, EvangelistLionBladeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistLionBladeClass3Align, EvangelistLionBladeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistLionBladeClass4Align, EvangelistLionBladeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistLionBladeClass5Align, EvangelistLionBladeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistLionBladeClass6Align, EvangelistLionBladeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistLionBladeClass7Align, EvangelistLionBladeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistLionBladeClass8Align, EvangelistLionBladeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistLionBladeClass9Align, EvangelistLionBladeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            EvangelistLionBladeClasspro = EvangelistLionBladeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistLionBladeClass10Align, EvangelistLionBladeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistLionBladeClasspro.Configure());
            var EvangelistEnchantingCourtesanClasspro = ProgressionConfigurator.New(EvangelistEnchantingCourtesanClass0Align, EvangelistEnchantingCourtesanClass0AlignGuid)
            .SetDisplayName(EvangelistEnchantingCourtesanClass0AlignDisplayName)
            .SetDescription(EvangelistEnchantingCourtesanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EnchantingCourtesanClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass2Align, EvangelistEnchantingCourtesanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass3Align, EvangelistEnchantingCourtesanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass4Align, EvangelistEnchantingCourtesanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass5Align, EvangelistEnchantingCourtesanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass6Align, EvangelistEnchantingCourtesanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass7Align, EvangelistEnchantingCourtesanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass8Align, EvangelistEnchantingCourtesanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass9Align, EvangelistEnchantingCourtesanClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            EvangelistEnchantingCourtesanClasspro = EvangelistEnchantingCourtesanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistEnchantingCourtesanClass10Align, EvangelistEnchantingCourtesanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistEnchantingCourtesanClasspro.Configure());
            var EvangelistHeritorKnightClasspro = ProgressionConfigurator.New(EvangelistHeritorKnightClass0Align, EvangelistHeritorKnightClass0AlignGuid)
            .SetDisplayName(EvangelistHeritorKnightClass0AlignDisplayName)
            .SetDescription(EvangelistHeritorKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HeritorKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistHeritorKnightClass2Align, EvangelistHeritorKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistHeritorKnightClass3Align, EvangelistHeritorKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistHeritorKnightClass4Align, EvangelistHeritorKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistHeritorKnightClass5Align, EvangelistHeritorKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistHeritorKnightClass6Align, EvangelistHeritorKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistHeritorKnightClass7Align, EvangelistHeritorKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistHeritorKnightClass8Align, EvangelistHeritorKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistHeritorKnightClass9Align, EvangelistHeritorKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            EvangelistHeritorKnightClasspro = EvangelistHeritorKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistHeritorKnightClass10Align, EvangelistHeritorKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistHeritorKnightClasspro.Configure());
            var EvangelistGoldenLegionnaireClasspro = ProgressionConfigurator.New(EvangelistGoldenLegionnaireClass0Align, EvangelistGoldenLegionnaireClass0AlignGuid)
            .SetDisplayName(EvangelistGoldenLegionnaireClass0AlignDisplayName)
            .SetDescription(EvangelistGoldenLegionnaireClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GoldenLegionnaireClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass2Align, EvangelistGoldenLegionnaireClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass3Align, EvangelistGoldenLegionnaireClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass4Align, EvangelistGoldenLegionnaireClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass5Align, EvangelistGoldenLegionnaireClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass6Align, EvangelistGoldenLegionnaireClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass7Align, EvangelistGoldenLegionnaireClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass8Align, EvangelistGoldenLegionnaireClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass9Align, EvangelistGoldenLegionnaireClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            EvangelistGoldenLegionnaireClasspro = EvangelistGoldenLegionnaireClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistGoldenLegionnaireClass10Align, EvangelistGoldenLegionnaireClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistGoldenLegionnaireClasspro.Configure());
            var EvangelistBoltAceClasspro = ProgressionConfigurator.New(EvangelistBoltAceClass0Align, EvangelistBoltAceClass0AlignGuid)
            .SetDisplayName(EvangelistBoltAceClass0AlignDisplayName)
            .SetDescription(EvangelistBoltAceClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(BoltAceClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistBoltAceClass2Align, EvangelistBoltAceClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistBoltAceClass3Align, EvangelistBoltAceClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistBoltAceClass4Align, EvangelistBoltAceClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistBoltAceClass5Align, EvangelistBoltAceClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistBoltAceClass6Align, EvangelistBoltAceClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistBoltAceClass7Align, EvangelistBoltAceClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistBoltAceClass8Align, EvangelistBoltAceClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistBoltAceClass9Align, EvangelistBoltAceClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            EvangelistBoltAceClasspro = EvangelistBoltAceClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistBoltAceClass10Align, EvangelistBoltAceClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistBoltAceClasspro.Configure());
            var EvangelistMortalUsherClasspro = ProgressionConfigurator.New(EvangelistMortalUsherClass0Align, EvangelistMortalUsherClass0AlignGuid)
            .SetDisplayName(EvangelistMortalUsherClass0AlignDisplayName)
            .SetDescription(EvangelistMortalUsherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MortalUsherClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistMortalUsherClass2Align, EvangelistMortalUsherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistMortalUsherClass3Align, EvangelistMortalUsherClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistMortalUsherClass4Align, EvangelistMortalUsherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistMortalUsherClass5Align, EvangelistMortalUsherClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistMortalUsherClass6Align, EvangelistMortalUsherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistMortalUsherClass7Align, EvangelistMortalUsherClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistMortalUsherClass8Align, EvangelistMortalUsherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistMortalUsherClass9Align, EvangelistMortalUsherClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            EvangelistMortalUsherClasspro = EvangelistMortalUsherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistMortalUsherClass10Align, EvangelistMortalUsherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistMortalUsherClasspro.Configure());
            var EvangelistForestWardenClasspro = ProgressionConfigurator.New(EvangelistForestWardenClass0Align, EvangelistForestWardenClass0AlignGuid)
            .SetDisplayName(EvangelistForestWardenClass0AlignDisplayName)
            .SetDescription(EvangelistForestWardenClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ForestWardenClass, 1)
            .SetHideNotAvailibleInUI(true);
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(EvangelistForestWardenClass2Align, EvangelistForestWardenClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(EvangelistForestWardenClass3Align, EvangelistForestWardenClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(EvangelistForestWardenClass4Align, EvangelistForestWardenClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(EvangelistForestWardenClass5Align, EvangelistForestWardenClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(EvangelistForestWardenClass6Align, EvangelistForestWardenClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(EvangelistForestWardenClass7Align, EvangelistForestWardenClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(EvangelistForestWardenClass8Align, EvangelistForestWardenClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(EvangelistForestWardenClass9Align, EvangelistForestWardenClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            EvangelistForestWardenClasspro = EvangelistForestWardenClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(EvangelistForestWardenClass10Align, EvangelistForestWardenClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            list.Add(EvangelistForestWardenClasspro.Configure());

            var select = FeatureSelectionConfigurator.New(AlignSpam, AlignSpamGuid)
              .SetDisplayName(SanctifiedRogueDisplayName)
              .SetDescription(SanctifiedRogueDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false);

            foreach (var feature in list)
            {
                select = select.AddToAllFeatures(feature);
            }

            return select.Configure();
        }
        private const string EvangelistAlchemistClass0Align = "EvangelistAlchemistClass0Align";
        private static readonly string EvangelistAlchemistClass0AlignGuid = "2ca09572-d794-4a82-afa5-b85d207b30ce";
        internal const string EvangelistAlchemistClass0AlignDisplayName = "EvangelistAlchemistClass0Align.Name";
        private const string EvangelistAlchemistClass0AlignDescription = "EvangelistAlchemistClass0Align.Description";
        private const string EvangelistAlchemistClass2Align = "EvangelistAlchemistClass2Align";
        private static readonly string EvangelistAlchemistClass2AlignGuid = "901b0ac1-0ca9-44b0-a9e7-a87e9fe922b6";
        private const string EvangelistAlchemistClass3Align = "EvangelistAlchemistClass3Align";
        private static readonly string EvangelistAlchemistClass3AlignGuid = "6c971348-4a8d-4d1c-8ffd-20b35dd4ebcb";
        private const string EvangelistAlchemistClass4Align = "EvangelistAlchemistClass4Align";
        private static readonly string EvangelistAlchemistClass4AlignGuid = "47862560-d996-46c4-8cb8-07f9c01e0111";
        private const string EvangelistAlchemistClass5Align = "EvangelistAlchemistClass5Align";
        private static readonly string EvangelistAlchemistClass5AlignGuid = "8ad6b91d-52be-467b-aeff-b490af03c303";
        private const string EvangelistAlchemistClass6Align = "EvangelistAlchemistClass6Align";
        private static readonly string EvangelistAlchemistClass6AlignGuid = "715b3fa4-f91d-4941-a8a4-611ed56b7b9e";
        private const string EvangelistAlchemistClass7Align = "EvangelistAlchemistClass7Align";
        private static readonly string EvangelistAlchemistClass7AlignGuid = "653aaae8-d9d8-477e-9ace-55974670165a";
        private const string EvangelistAlchemistClass8Align = "EvangelistAlchemistClass8Align";
        private static readonly string EvangelistAlchemistClass8AlignGuid = "93222514-f938-4a47-94f0-ea5a585fb1d2";
        private const string EvangelistAlchemistClass9Align = "EvangelistAlchemistClass9Align";
        private static readonly string EvangelistAlchemistClass9AlignGuid = "960863bb-7dea-44ce-b5a5-1d17e4514752";
        private const string EvangelistAlchemistClass10Align = "EvangelistAlchemistClass10Align";
        private static readonly string EvangelistAlchemistClass10AlignGuid = "88794d23-9a4f-4e43-8ae3-2b5823d404e3";
        private const string EvangelistArcaneTricksterClass0Align = "EvangelistArcaneTricksterClass0Align";
        private static readonly string EvangelistArcaneTricksterClass0AlignGuid = "6b2ea453-fa43-4162-936d-a38ee0dde5d1";
        internal const string EvangelistArcaneTricksterClass0AlignDisplayName = "EvangelistArcaneTricksterClass0Align.Name";
        private const string EvangelistArcaneTricksterClass0AlignDescription = "EvangelistArcaneTricksterClass0Align.Description";
        private const string EvangelistArcaneTricksterClass2Align = "EvangelistArcaneTricksterClass2Align";
        private static readonly string EvangelistArcaneTricksterClass2AlignGuid = "92e48221-1734-48f9-ac71-c7d3be3f74e6";
        private const string EvangelistArcaneTricksterClass3Align = "EvangelistArcaneTricksterClass3Align";
        private static readonly string EvangelistArcaneTricksterClass3AlignGuid = "01e79ae4-8366-4c9a-b227-007a6c8dc7be";
        private const string EvangelistArcaneTricksterClass4Align = "EvangelistArcaneTricksterClass4Align";
        private static readonly string EvangelistArcaneTricksterClass4AlignGuid = "0769f24d-e86d-4a53-b662-ab8625635d03";
        private const string EvangelistArcaneTricksterClass5Align = "EvangelistArcaneTricksterClass5Align";
        private static readonly string EvangelistArcaneTricksterClass5AlignGuid = "c8461131-c346-4e79-ab62-f880e0d5daa3";
        private const string EvangelistArcaneTricksterClass6Align = "EvangelistArcaneTricksterClass6Align";
        private static readonly string EvangelistArcaneTricksterClass6AlignGuid = "df58de13-4b1b-48b2-a8e6-224ecba131c5";
        private const string EvangelistArcaneTricksterClass7Align = "EvangelistArcaneTricksterClass7Align";
        private static readonly string EvangelistArcaneTricksterClass7AlignGuid = "a7ba19ab-dbdc-4c27-855c-e16967043cf4";
        private const string EvangelistArcaneTricksterClass8Align = "EvangelistArcaneTricksterClass8Align";
        private static readonly string EvangelistArcaneTricksterClass8AlignGuid = "dd5172a3-e5c8-4b1b-ba86-af0b75ec7750";
        private const string EvangelistArcaneTricksterClass9Align = "EvangelistArcaneTricksterClass9Align";
        private static readonly string EvangelistArcaneTricksterClass9AlignGuid = "356eadc7-a217-49e5-b14a-ea0af808d01f";
        private const string EvangelistArcaneTricksterClass10Align = "EvangelistArcaneTricksterClass10Align";
        private static readonly string EvangelistArcaneTricksterClass10AlignGuid = "4ac75100-377b-4367-b702-a7f45d6c5e22";
        private const string EvangelistArcanistClass0Align = "EvangelistArcanistClass0Align";
        private static readonly string EvangelistArcanistClass0AlignGuid = "22aded29-7122-42ab-84e9-a683876f023c";
        internal const string EvangelistArcanistClass0AlignDisplayName = "EvangelistArcanistClass0Align.Name";
        private const string EvangelistArcanistClass0AlignDescription = "EvangelistArcanistClass0Align.Description";
        private const string EvangelistArcanistClass2Align = "EvangelistArcanistClass2Align";
        private static readonly string EvangelistArcanistClass2AlignGuid = "1df7eb8e-8cd7-4009-a657-3c7a379bb7be";
        private const string EvangelistArcanistClass3Align = "EvangelistArcanistClass3Align";
        private static readonly string EvangelistArcanistClass3AlignGuid = "e4e8363a-2913-4b22-9e91-a5ef40112191";
        private const string EvangelistArcanistClass4Align = "EvangelistArcanistClass4Align";
        private static readonly string EvangelistArcanistClass4AlignGuid = "c801c434-2b53-4c69-a1a2-43206cc14bb3";
        private const string EvangelistArcanistClass5Align = "EvangelistArcanistClass5Align";
        private static readonly string EvangelistArcanistClass5AlignGuid = "1d7a7c73-71e1-453f-8357-425a15d8a057";
        private const string EvangelistArcanistClass6Align = "EvangelistArcanistClass6Align";
        private static readonly string EvangelistArcanistClass6AlignGuid = "ef44638b-b2ce-45eb-82e9-66f102691a26";
        private const string EvangelistArcanistClass7Align = "EvangelistArcanistClass7Align";
        private static readonly string EvangelistArcanistClass7AlignGuid = "965a27db-31ab-46ae-a043-e9a779a35d32";
        private const string EvangelistArcanistClass8Align = "EvangelistArcanistClass8Align";
        private static readonly string EvangelistArcanistClass8AlignGuid = "26ac9764-a080-40dd-8798-1b052793e5c3";
        private const string EvangelistArcanistClass9Align = "EvangelistArcanistClass9Align";
        private static readonly string EvangelistArcanistClass9AlignGuid = "0dd9ff56-fece-4fae-8b9e-7079879fe5b2";
        private const string EvangelistArcanistClass10Align = "EvangelistArcanistClass10Align";
        private static readonly string EvangelistArcanistClass10AlignGuid = "23ecb574-3782-4e39-8bfa-27610943db5d";
        private const string EvangelistAssassinClass0Align = "EvangelistAssassinClass0Align";
        private static readonly string EvangelistAssassinClass0AlignGuid = "d13219b3-13dd-4f0d-b691-986dbf8ee6f7";
        internal const string EvangelistAssassinClass0AlignDisplayName = "EvangelistAssassinClass0Align.Name";
        private const string EvangelistAssassinClass0AlignDescription = "EvangelistAssassinClass0Align.Description";
        private const string EvangelistAssassinClass2Align = "EvangelistAssassinClass2Align";
        private static readonly string EvangelistAssassinClass2AlignGuid = "a5ceebce-dd80-4f69-b8a5-19da673bbb02";
        private const string EvangelistAssassinClass3Align = "EvangelistAssassinClass3Align";
        private static readonly string EvangelistAssassinClass3AlignGuid = "29a469d4-6f3d-4dd4-ab16-9d749bd4245c";
        private const string EvangelistAssassinClass4Align = "EvangelistAssassinClass4Align";
        private static readonly string EvangelistAssassinClass4AlignGuid = "30123a74-6f91-40e7-931d-5784655f8810";
        private const string EvangelistAssassinClass5Align = "EvangelistAssassinClass5Align";
        private static readonly string EvangelistAssassinClass5AlignGuid = "9c7c5b5d-c611-402f-b31a-d84b889b02c0";
        private const string EvangelistAssassinClass6Align = "EvangelistAssassinClass6Align";
        private static readonly string EvangelistAssassinClass6AlignGuid = "cbf974e3-9f81-4c4a-8f72-919b74343a1a";
        private const string EvangelistAssassinClass7Align = "EvangelistAssassinClass7Align";
        private static readonly string EvangelistAssassinClass7AlignGuid = "a8e90f19-8d09-4623-9618-2449ae9ebdfb";
        private const string EvangelistAssassinClass8Align = "EvangelistAssassinClass8Align";
        private static readonly string EvangelistAssassinClass8AlignGuid = "db3149fd-ae42-4d8f-90a4-d55c8f14fb88";
        private const string EvangelistAssassinClass9Align = "EvangelistAssassinClass9Align";
        private static readonly string EvangelistAssassinClass9AlignGuid = "e7042874-9b83-4fd9-a3d4-3f3d4d417ccc";
        private const string EvangelistAssassinClass10Align = "EvangelistAssassinClass10Align";
        private static readonly string EvangelistAssassinClass10AlignGuid = "0a7732b6-3b0d-4104-9143-9178aa05397a";
        private const string EvangelistBarbarianClass0Align = "EvangelistBarbarianClass0Align";
        private static readonly string EvangelistBarbarianClass0AlignGuid = "efbbb1f5-ca14-4559-bb02-005d59c4931b";
        internal const string EvangelistBarbarianClass0AlignDisplayName = "EvangelistBarbarianClass0Align.Name";
        private const string EvangelistBarbarianClass0AlignDescription = "EvangelistBarbarianClass0Align.Description";
        private const string EvangelistBarbarianClass2Align = "EvangelistBarbarianClass2Align";
        private static readonly string EvangelistBarbarianClass2AlignGuid = "cdd66673-69fd-4624-b2e3-c06a3c7e1270";
        private const string EvangelistBarbarianClass3Align = "EvangelistBarbarianClass3Align";
        private static readonly string EvangelistBarbarianClass3AlignGuid = "48319b91-e6c1-406c-9cda-0b4203109d26";
        private const string EvangelistBarbarianClass4Align = "EvangelistBarbarianClass4Align";
        private static readonly string EvangelistBarbarianClass4AlignGuid = "410aade0-7158-4413-ab6d-38840d2fe93f";
        private const string EvangelistBarbarianClass5Align = "EvangelistBarbarianClass5Align";
        private static readonly string EvangelistBarbarianClass5AlignGuid = "d484bb55-17a2-470d-8a4a-408cc8712135";
        private const string EvangelistBarbarianClass6Align = "EvangelistBarbarianClass6Align";
        private static readonly string EvangelistBarbarianClass6AlignGuid = "b98fe83a-c311-4dbe-8da9-1c177e4a0313";
        private const string EvangelistBarbarianClass7Align = "EvangelistBarbarianClass7Align";
        private static readonly string EvangelistBarbarianClass7AlignGuid = "3e1189a2-6856-41a1-bd26-67aa30c24579";
        private const string EvangelistBarbarianClass8Align = "EvangelistBarbarianClass8Align";
        private static readonly string EvangelistBarbarianClass8AlignGuid = "baf8d59a-7fa6-4f21-9d02-59bec112d6df";
        private const string EvangelistBarbarianClass9Align = "EvangelistBarbarianClass9Align";
        private static readonly string EvangelistBarbarianClass9AlignGuid = "d9e6821b-455e-49d7-98bd-e7d2415dd4f9";
        private const string EvangelistBarbarianClass10Align = "EvangelistBarbarianClass10Align";
        private static readonly string EvangelistBarbarianClass10AlignGuid = "1e6902d6-4874-4d7c-a88c-6a6a5ead9ef1";
        private const string EvangelistBardClass0Align = "EvangelistBardClass0Align";
        private static readonly string EvangelistBardClass0AlignGuid = "1629717f-fd14-4e2d-bc78-96331effcffe";
        internal const string EvangelistBardClass0AlignDisplayName = "EvangelistBardClass0Align.Name";
        private const string EvangelistBardClass0AlignDescription = "EvangelistBardClass0Align.Description";
        private const string EvangelistBardClass2Align = "EvangelistBardClass2Align";
        private static readonly string EvangelistBardClass2AlignGuid = "fd9a9239-41db-4c1e-b51c-1f80d47198d2";
        private const string EvangelistBardClass3Align = "EvangelistBardClass3Align";
        private static readonly string EvangelistBardClass3AlignGuid = "4e03da24-da01-4e13-8c5c-9987e06b0e62";
        private const string EvangelistBardClass4Align = "EvangelistBardClass4Align";
        private static readonly string EvangelistBardClass4AlignGuid = "7e47329a-deac-4817-8f56-bdffc638af50";
        private const string EvangelistBardClass5Align = "EvangelistBardClass5Align";
        private static readonly string EvangelistBardClass5AlignGuid = "9d6a985f-d038-4f36-9b00-59c3a13fd484";
        private const string EvangelistBardClass6Align = "EvangelistBardClass6Align";
        private static readonly string EvangelistBardClass6AlignGuid = "a942d5de-6ddf-426b-917c-554779135a2a";
        private const string EvangelistBardClass7Align = "EvangelistBardClass7Align";
        private static readonly string EvangelistBardClass7AlignGuid = "a4fc0528-7ecc-4b47-889c-58e9df4d64d9";
        private const string EvangelistBardClass8Align = "EvangelistBardClass8Align";
        private static readonly string EvangelistBardClass8AlignGuid = "c3ad74e9-2252-4c9d-b93b-260cabee4e85";
        private const string EvangelistBardClass9Align = "EvangelistBardClass9Align";
        private static readonly string EvangelistBardClass9AlignGuid = "0187df12-ee70-47bd-af09-3eca9de0952c";
        private const string EvangelistBardClass10Align = "EvangelistBardClass10Align";
        private static readonly string EvangelistBardClass10AlignGuid = "5268b099-647c-4496-bd18-d34134b6e278";
        private const string EvangelistBloodragerClass0Align = "EvangelistBloodragerClass0Align";
        private static readonly string EvangelistBloodragerClass0AlignGuid = "fb688cc1-1b93-4ba6-b6f7-6999c038fdd3";
        internal const string EvangelistBloodragerClass0AlignDisplayName = "EvangelistBloodragerClass0Align.Name";
        private const string EvangelistBloodragerClass0AlignDescription = "EvangelistBloodragerClass0Align.Description";
        private const string EvangelistBloodragerClass2Align = "EvangelistBloodragerClass2Align";
        private static readonly string EvangelistBloodragerClass2AlignGuid = "3353e8e2-e63c-428b-bd95-fb512e04e22d";
        private const string EvangelistBloodragerClass3Align = "EvangelistBloodragerClass3Align";
        private static readonly string EvangelistBloodragerClass3AlignGuid = "5bdd1212-a28d-4f5d-88c7-fc8302d100e5";
        private const string EvangelistBloodragerClass4Align = "EvangelistBloodragerClass4Align";
        private static readonly string EvangelistBloodragerClass4AlignGuid = "31a7c1b3-fa99-4592-8d44-64bbbfbf3347";
        private const string EvangelistBloodragerClass5Align = "EvangelistBloodragerClass5Align";
        private static readonly string EvangelistBloodragerClass5AlignGuid = "f94986cf-bada-4ce1-93c0-c0da060cdda0";
        private const string EvangelistBloodragerClass6Align = "EvangelistBloodragerClass6Align";
        private static readonly string EvangelistBloodragerClass6AlignGuid = "c3e77145-d7ac-46e6-991c-9be8e56f3ce7";
        private const string EvangelistBloodragerClass7Align = "EvangelistBloodragerClass7Align";
        private static readonly string EvangelistBloodragerClass7AlignGuid = "08c84329-610c-46f2-8ea9-ec7f9fdb678d";
        private const string EvangelistBloodragerClass8Align = "EvangelistBloodragerClass8Align";
        private static readonly string EvangelistBloodragerClass8AlignGuid = "1912efc9-945d-4c75-af93-34464c5aebc7";
        private const string EvangelistBloodragerClass9Align = "EvangelistBloodragerClass9Align";
        private static readonly string EvangelistBloodragerClass9AlignGuid = "1f8fd4b9-d92c-49cc-9a9e-e15b984693c2";
        private const string EvangelistBloodragerClass10Align = "EvangelistBloodragerClass10Align";
        private static readonly string EvangelistBloodragerClass10AlignGuid = "4e533134-4016-43df-8d0c-cbede2cc0a5c";
        private const string EvangelistCavalierClass0Align = "EvangelistCavalierClass0Align";
        private static readonly string EvangelistCavalierClass0AlignGuid = "d861ef8c-cc48-4bb7-a7d2-5954ea5b70d6";
        internal const string EvangelistCavalierClass0AlignDisplayName = "EvangelistCavalierClass0Align.Name";
        private const string EvangelistCavalierClass0AlignDescription = "EvangelistCavalierClass0Align.Description";
        private const string EvangelistCavalierClass2Align = "EvangelistCavalierClass2Align";
        private static readonly string EvangelistCavalierClass2AlignGuid = "93070f9d-ca31-439c-8f71-e9168d0b92a3";
        private const string EvangelistCavalierClass3Align = "EvangelistCavalierClass3Align";
        private static readonly string EvangelistCavalierClass3AlignGuid = "3956b11b-59f1-4b57-87ff-7ff344d92473";
        private const string EvangelistCavalierClass4Align = "EvangelistCavalierClass4Align";
        private static readonly string EvangelistCavalierClass4AlignGuid = "679c7f0a-dd68-4fd3-8974-e2f0456c99bd";
        private const string EvangelistCavalierClass5Align = "EvangelistCavalierClass5Align";
        private static readonly string EvangelistCavalierClass5AlignGuid = "6392fcc1-d6ed-4646-9d99-1bc6ee429d09";
        private const string EvangelistCavalierClass6Align = "EvangelistCavalierClass6Align";
        private static readonly string EvangelistCavalierClass6AlignGuid = "01878bbe-3515-42cd-9dcf-e01cc1775656";
        private const string EvangelistCavalierClass7Align = "EvangelistCavalierClass7Align";
        private static readonly string EvangelistCavalierClass7AlignGuid = "62123386-2421-46bc-9e31-16fc90a06c7d";
        private const string EvangelistCavalierClass8Align = "EvangelistCavalierClass8Align";
        private static readonly string EvangelistCavalierClass8AlignGuid = "f1e614be-7d5b-462d-a8a6-7f55a524cddf";
        private const string EvangelistCavalierClass9Align = "EvangelistCavalierClass9Align";
        private static readonly string EvangelistCavalierClass9AlignGuid = "a18e2a9b-ecb6-470b-9ed5-5ee50737fe1f";
        private const string EvangelistCavalierClass10Align = "EvangelistCavalierClass10Align";
        private static readonly string EvangelistCavalierClass10AlignGuid = "492b3c05-2cb0-46e4-a862-0f66a2bdc5b5";
        private const string EvangelistClericClass0Align = "EvangelistClericClass0Align";
        private static readonly string EvangelistClericClass0AlignGuid = "93421f4a-8870-4af4-be46-63f522ce7b6e";
        internal const string EvangelistClericClass0AlignDisplayName = "EvangelistClericClass0Align.Name";
        private const string EvangelistClericClass0AlignDescription = "EvangelistClericClass0Align.Description";
        private const string EvangelistClericClass2Align = "EvangelistClericClass2Align";
        private static readonly string EvangelistClericClass2AlignGuid = "2fbd8961-70af-4cd1-999f-829ca50c4946";
        private const string EvangelistClericClass3Align = "EvangelistClericClass3Align";
        private static readonly string EvangelistClericClass3AlignGuid = "e9344d12-fb14-4a6a-b4a3-4ebc10a035d1";
        private const string EvangelistClericClass4Align = "EvangelistClericClass4Align";
        private static readonly string EvangelistClericClass4AlignGuid = "3ed3045e-e035-4642-be26-c4f873daab24";
        private const string EvangelistClericClass5Align = "EvangelistClericClass5Align";
        private static readonly string EvangelistClericClass5AlignGuid = "7bdb5048-d45c-40f7-b971-971953e7425a";
        private const string EvangelistClericClass6Align = "EvangelistClericClass6Align";
        private static readonly string EvangelistClericClass6AlignGuid = "0e4c05bc-66e9-4664-b92e-2aad7cbff1b0";
        private const string EvangelistClericClass7Align = "EvangelistClericClass7Align";
        private static readonly string EvangelistClericClass7AlignGuid = "6d151878-b90a-4505-8470-7d6498f63cd2";
        private const string EvangelistClericClass8Align = "EvangelistClericClass8Align";
        private static readonly string EvangelistClericClass8AlignGuid = "a07394ba-b490-40d2-ad21-097ad4ad4497";
        private const string EvangelistClericClass9Align = "EvangelistClericClass9Align";
        private static readonly string EvangelistClericClass9AlignGuid = "3afc4d2f-cb85-4000-b495-419348a92965";
        private const string EvangelistClericClass10Align = "EvangelistClericClass10Align";
        private static readonly string EvangelistClericClass10AlignGuid = "b0048ce8-1a5d-4b5d-917f-de1c4e5d410d";
        private const string EvangelistDragonDiscipleClass0Align = "EvangelistDragonDiscipleClass0Align";
        private static readonly string EvangelistDragonDiscipleClass0AlignGuid = "61f30ffd-1d56-4fc8-8afc-25ec6c63b235";
        internal const string EvangelistDragonDiscipleClass0AlignDisplayName = "EvangelistDragonDiscipleClass0Align.Name";
        private const string EvangelistDragonDiscipleClass0AlignDescription = "EvangelistDragonDiscipleClass0Align.Description";
        private const string EvangelistDragonDiscipleClass2Align = "EvangelistDragonDiscipleClass2Align";
        private static readonly string EvangelistDragonDiscipleClass2AlignGuid = "6b8fc180-a0d7-4c8a-b9a3-66b43d9f004c";
        private const string EvangelistDragonDiscipleClass3Align = "EvangelistDragonDiscipleClass3Align";
        private static readonly string EvangelistDragonDiscipleClass3AlignGuid = "2530f30c-c039-4134-9255-55050ed3e5af";
        private const string EvangelistDragonDiscipleClass4Align = "EvangelistDragonDiscipleClass4Align";
        private static readonly string EvangelistDragonDiscipleClass4AlignGuid = "71c47932-1cd0-409b-b6bd-432d6ace8648";
        private const string EvangelistDragonDiscipleClass5Align = "EvangelistDragonDiscipleClass5Align";
        private static readonly string EvangelistDragonDiscipleClass5AlignGuid = "8d8c6643-77ab-454d-993f-7799613bf4d9";
        private const string EvangelistDragonDiscipleClass6Align = "EvangelistDragonDiscipleClass6Align";
        private static readonly string EvangelistDragonDiscipleClass6AlignGuid = "ddea520c-46b6-4c63-a529-8cf3994b4617";
        private const string EvangelistDragonDiscipleClass7Align = "EvangelistDragonDiscipleClass7Align";
        private static readonly string EvangelistDragonDiscipleClass7AlignGuid = "871d53e6-306e-482f-809e-1cb6b331b0b2";
        private const string EvangelistDragonDiscipleClass8Align = "EvangelistDragonDiscipleClass8Align";
        private static readonly string EvangelistDragonDiscipleClass8AlignGuid = "755fe000-8183-4cf8-a479-598591308678";
        private const string EvangelistDragonDiscipleClass9Align = "EvangelistDragonDiscipleClass9Align";
        private static readonly string EvangelistDragonDiscipleClass9AlignGuid = "d906f705-10d6-4745-933a-327df485d81d";
        private const string EvangelistDragonDiscipleClass10Align = "EvangelistDragonDiscipleClass10Align";
        private static readonly string EvangelistDragonDiscipleClass10AlignGuid = "295ae8fa-47c0-481c-ab3f-c1516379fea4";
        private const string EvangelistDruidClass0Align = "EvangelistDruidClass0Align";
        private static readonly string EvangelistDruidClass0AlignGuid = "b22452f9-c1b2-4ee3-a895-7d2ddbc23867";
        internal const string EvangelistDruidClass0AlignDisplayName = "EvangelistDruidClass0Align.Name";
        private const string EvangelistDruidClass0AlignDescription = "EvangelistDruidClass0Align.Description";
        private const string EvangelistDruidClass2Align = "EvangelistDruidClass2Align";
        private static readonly string EvangelistDruidClass2AlignGuid = "5f3495c2-85d4-4a7e-92b4-b7120ea606a5";
        private const string EvangelistDruidClass3Align = "EvangelistDruidClass3Align";
        private static readonly string EvangelistDruidClass3AlignGuid = "78bd7c04-c015-48dc-a3f8-47df9996146a";
        private const string EvangelistDruidClass4Align = "EvangelistDruidClass4Align";
        private static readonly string EvangelistDruidClass4AlignGuid = "5d396599-b69f-4c40-ad9c-ad0776e4a1b8";
        private const string EvangelistDruidClass5Align = "EvangelistDruidClass5Align";
        private static readonly string EvangelistDruidClass5AlignGuid = "963e6f57-f492-458d-a67a-95c7a0d1466e";
        private const string EvangelistDruidClass6Align = "EvangelistDruidClass6Align";
        private static readonly string EvangelistDruidClass6AlignGuid = "49fb48f2-6dd3-41f9-a6b6-4358b4055754";
        private const string EvangelistDruidClass7Align = "EvangelistDruidClass7Align";
        private static readonly string EvangelistDruidClass7AlignGuid = "54e8ab74-2092-425f-ae7d-cddc484ca5fe";
        private const string EvangelistDruidClass8Align = "EvangelistDruidClass8Align";
        private static readonly string EvangelistDruidClass8AlignGuid = "02365632-1c23-4ebd-a9bb-e62071d9076d";
        private const string EvangelistDruidClass9Align = "EvangelistDruidClass9Align";
        private static readonly string EvangelistDruidClass9AlignGuid = "cfbc3b8c-f871-447c-96f8-e99e8cefaf5b";
        private const string EvangelistDruidClass10Align = "EvangelistDruidClass10Align";
        private static readonly string EvangelistDruidClass10AlignGuid = "e50e678f-8f42-4cc5-acc1-de1b430feee8";
        private const string EvangelistDuelistClass0Align = "EvangelistDuelistClass0Align";
        private static readonly string EvangelistDuelistClass0AlignGuid = "0f8baed1-4cbe-4f11-a560-3919409185df";
        internal const string EvangelistDuelistClass0AlignDisplayName = "EvangelistDuelistClass0Align.Name";
        private const string EvangelistDuelistClass0AlignDescription = "EvangelistDuelistClass0Align.Description";
        private const string EvangelistDuelistClass2Align = "EvangelistDuelistClass2Align";
        private static readonly string EvangelistDuelistClass2AlignGuid = "a1a2c22f-62a6-4be2-950f-009b2dfb005e";
        private const string EvangelistDuelistClass3Align = "EvangelistDuelistClass3Align";
        private static readonly string EvangelistDuelistClass3AlignGuid = "34feb6e4-a79c-43a3-9b96-f6670ec540fe";
        private const string EvangelistDuelistClass4Align = "EvangelistDuelistClass4Align";
        private static readonly string EvangelistDuelistClass4AlignGuid = "0342397e-0f8d-4c3f-bd40-3bfe8ff93960";
        private const string EvangelistDuelistClass5Align = "EvangelistDuelistClass5Align";
        private static readonly string EvangelistDuelistClass5AlignGuid = "e1587a51-a25d-4a20-9df5-93873177a210";
        private const string EvangelistDuelistClass6Align = "EvangelistDuelistClass6Align";
        private static readonly string EvangelistDuelistClass6AlignGuid = "1dc3a8d5-2236-4ddd-84cd-9f64da5f22f6";
        private const string EvangelistDuelistClass7Align = "EvangelistDuelistClass7Align";
        private static readonly string EvangelistDuelistClass7AlignGuid = "ef03555c-360f-4bb2-9af4-35a944e15b1f";
        private const string EvangelistDuelistClass8Align = "EvangelistDuelistClass8Align";
        private static readonly string EvangelistDuelistClass8AlignGuid = "90edbc8a-4e7e-42b3-b2e5-5f3cc5afdd30";
        private const string EvangelistDuelistClass9Align = "EvangelistDuelistClass9Align";
        private static readonly string EvangelistDuelistClass9AlignGuid = "df4527bb-2349-40a3-be3e-9414f4714f0b";
        private const string EvangelistDuelistClass10Align = "EvangelistDuelistClass10Align";
        private static readonly string EvangelistDuelistClass10AlignGuid = "a4cb5e41-59bd-421c-a044-b17d266605ce";
        private const string EvangelistEldritchKnightClass0Align = "EvangelistEldritchKnightClass0Align";
        private static readonly string EvangelistEldritchKnightClass0AlignGuid = "d0047936-cb3e-488d-80b8-e4fe0089c746";
        internal const string EvangelistEldritchKnightClass0AlignDisplayName = "EvangelistEldritchKnightClass0Align.Name";
        private const string EvangelistEldritchKnightClass0AlignDescription = "EvangelistEldritchKnightClass0Align.Description";
        private const string EvangelistEldritchKnightClass2Align = "EvangelistEldritchKnightClass2Align";
        private static readonly string EvangelistEldritchKnightClass2AlignGuid = "67e4586d-8dea-42d4-a0a9-4c55b2ce900b";
        private const string EvangelistEldritchKnightClass3Align = "EvangelistEldritchKnightClass3Align";
        private static readonly string EvangelistEldritchKnightClass3AlignGuid = "c81b2326-58ed-4d84-ba44-7d0ff1432f42";
        private const string EvangelistEldritchKnightClass4Align = "EvangelistEldritchKnightClass4Align";
        private static readonly string EvangelistEldritchKnightClass4AlignGuid = "ad8f7bb3-7135-4284-a888-0a6d3e6ea26c";
        private const string EvangelistEldritchKnightClass5Align = "EvangelistEldritchKnightClass5Align";
        private static readonly string EvangelistEldritchKnightClass5AlignGuid = "59f7c16f-6d85-4c00-8f13-17daec8bbd30";
        private const string EvangelistEldritchKnightClass6Align = "EvangelistEldritchKnightClass6Align";
        private static readonly string EvangelistEldritchKnightClass6AlignGuid = "9eda0233-2a9c-467e-a2f3-b550a94181cd";
        private const string EvangelistEldritchKnightClass7Align = "EvangelistEldritchKnightClass7Align";
        private static readonly string EvangelistEldritchKnightClass7AlignGuid = "e0aab5dc-8335-487a-b749-1666c9419efc";
        private const string EvangelistEldritchKnightClass8Align = "EvangelistEldritchKnightClass8Align";
        private static readonly string EvangelistEldritchKnightClass8AlignGuid = "cc49a7dc-ec79-4210-bbde-0de579e4efa1";
        private const string EvangelistEldritchKnightClass9Align = "EvangelistEldritchKnightClass9Align";
        private static readonly string EvangelistEldritchKnightClass9AlignGuid = "11886f31-8c8c-496c-b41c-25b21315b3eb";
        private const string EvangelistEldritchKnightClass10Align = "EvangelistEldritchKnightClass10Align";
        private static readonly string EvangelistEldritchKnightClass10AlignGuid = "b8e9fb7a-e1aa-49d1-bd57-69543b2201fe";
        private const string EvangelistEldritchScionClass0Align = "EvangelistEldritchScionClass0Align";
        private static readonly string EvangelistEldritchScionClass0AlignGuid = "b3b51d4f-5fa8-453b-b43a-b0b6bbb58943";
        internal const string EvangelistEldritchScionClass0AlignDisplayName = "EvangelistEldritchScionClass0Align.Name";
        private const string EvangelistEldritchScionClass0AlignDescription = "EvangelistEldritchScionClass0Align.Description";
        private const string EvangelistEldritchScionClass2Align = "EvangelistEldritchScionClass2Align";
        private static readonly string EvangelistEldritchScionClass2AlignGuid = "d4e304cb-6aa8-4971-8531-f3e2a2f4ebc6";
        private const string EvangelistEldritchScionClass3Align = "EvangelistEldritchScionClass3Align";
        private static readonly string EvangelistEldritchScionClass3AlignGuid = "19764b95-61f9-49f2-8d77-b0e9566e15fd";
        private const string EvangelistEldritchScionClass4Align = "EvangelistEldritchScionClass4Align";
        private static readonly string EvangelistEldritchScionClass4AlignGuid = "57c73282-5a48-458b-a1e6-45f245ce53b7";
        private const string EvangelistEldritchScionClass5Align = "EvangelistEldritchScionClass5Align";
        private static readonly string EvangelistEldritchScionClass5AlignGuid = "a4a53aab-b769-4a8c-b8db-d8faa68eac57";
        private const string EvangelistEldritchScionClass6Align = "EvangelistEldritchScionClass6Align";
        private static readonly string EvangelistEldritchScionClass6AlignGuid = "bfbd9e06-d49c-4a02-9a5c-5b57543b4777";
        private const string EvangelistEldritchScionClass7Align = "EvangelistEldritchScionClass7Align";
        private static readonly string EvangelistEldritchScionClass7AlignGuid = "f9caebdd-c0f9-4eb8-805e-3e01b002f452";
        private const string EvangelistEldritchScionClass8Align = "EvangelistEldritchScionClass8Align";
        private static readonly string EvangelistEldritchScionClass8AlignGuid = "aa680063-cadf-4118-981d-7d3b1ad0d7d9";
        private const string EvangelistEldritchScionClass9Align = "EvangelistEldritchScionClass9Align";
        private static readonly string EvangelistEldritchScionClass9AlignGuid = "bbdb37b1-35ea-46ef-b857-92338b4b50d7";
        private const string EvangelistEldritchScionClass10Align = "EvangelistEldritchScionClass10Align";
        private static readonly string EvangelistEldritchScionClass10AlignGuid = "aadf666a-7635-4699-84d3-7f8c7b5c3939";
        private const string EvangelistFighterClass0Align = "EvangelistFighterClass0Align";
        private static readonly string EvangelistFighterClass0AlignGuid = "28c53d6c-639e-4d6e-add7-71b9e3e90cba";
        internal const string EvangelistFighterClass0AlignDisplayName = "EvangelistFighterClass0Align.Name";
        private const string EvangelistFighterClass0AlignDescription = "EvangelistFighterClass0Align.Description";
        private const string EvangelistFighterClass2Align = "EvangelistFighterClass2Align";
        private static readonly string EvangelistFighterClass2AlignGuid = "365f7e3e-535e-4e76-804c-7e7326b837f3";
        private const string EvangelistFighterClass3Align = "EvangelistFighterClass3Align";
        private static readonly string EvangelistFighterClass3AlignGuid = "3c2e469b-8d28-4e5b-b671-262e4c4fa585";
        private const string EvangelistFighterClass4Align = "EvangelistFighterClass4Align";
        private static readonly string EvangelistFighterClass4AlignGuid = "f3355b80-55e7-4048-a3fb-ee00c674b794";
        private const string EvangelistFighterClass5Align = "EvangelistFighterClass5Align";
        private static readonly string EvangelistFighterClass5AlignGuid = "d219970b-2c81-46fc-8731-6b7a3e2f7587";
        private const string EvangelistFighterClass6Align = "EvangelistFighterClass6Align";
        private static readonly string EvangelistFighterClass6AlignGuid = "ba671b67-1029-4411-9f37-31bbfe975fb6";
        private const string EvangelistFighterClass7Align = "EvangelistFighterClass7Align";
        private static readonly string EvangelistFighterClass7AlignGuid = "4147cbc8-f4ae-4b15-9907-39e061e91964";
        private const string EvangelistFighterClass8Align = "EvangelistFighterClass8Align";
        private static readonly string EvangelistFighterClass8AlignGuid = "c0e87db3-fb90-499c-bd6c-166044e439be";
        private const string EvangelistFighterClass9Align = "EvangelistFighterClass9Align";
        private static readonly string EvangelistFighterClass9AlignGuid = "fc82a64b-c79f-467b-af98-41398a64a08f";
        private const string EvangelistFighterClass10Align = "EvangelistFighterClass10Align";
        private static readonly string EvangelistFighterClass10AlignGuid = "8815557e-055e-4131-a99b-b500dec1782f";
        private const string EvangelistHellknightClass0Align = "EvangelistHellknightClass0Align";
        private static readonly string EvangelistHellknightClass0AlignGuid = "4205d316-0f40-43b2-92bd-77d11ad29302";
        internal const string EvangelistHellknightClass0AlignDisplayName = "EvangelistHellknightClass0Align.Name";
        private const string EvangelistHellknightClass0AlignDescription = "EvangelistHellknightClass0Align.Description";
        private const string EvangelistHellknightClass2Align = "EvangelistHellknightClass2Align";
        private static readonly string EvangelistHellknightClass2AlignGuid = "22ee93c0-d82a-48a0-bbfb-bd729a9302f1";
        private const string EvangelistHellknightClass3Align = "EvangelistHellknightClass3Align";
        private static readonly string EvangelistHellknightClass3AlignGuid = "35759d5f-c21f-4094-825c-80e0ae44fa77";
        private const string EvangelistHellknightClass4Align = "EvangelistHellknightClass4Align";
        private static readonly string EvangelistHellknightClass4AlignGuid = "d2ae4a98-076e-4c55-98e3-331bc37bb363";
        private const string EvangelistHellknightClass5Align = "EvangelistHellknightClass5Align";
        private static readonly string EvangelistHellknightClass5AlignGuid = "e5af6f09-9b01-4065-bec0-c0935c4364c6";
        private const string EvangelistHellknightClass6Align = "EvangelistHellknightClass6Align";
        private static readonly string EvangelistHellknightClass6AlignGuid = "0e618445-8b57-42a3-b4c0-2681dc151f8a";
        private const string EvangelistHellknightClass7Align = "EvangelistHellknightClass7Align";
        private static readonly string EvangelistHellknightClass7AlignGuid = "30b06130-7ce5-45b3-83bf-c2a594fbd5cd";
        private const string EvangelistHellknightClass8Align = "EvangelistHellknightClass8Align";
        private static readonly string EvangelistHellknightClass8AlignGuid = "e6037489-9366-4ad4-98ff-d7c4a6ebc6ef";
        private const string EvangelistHellknightClass9Align = "EvangelistHellknightClass9Align";
        private static readonly string EvangelistHellknightClass9AlignGuid = "6d7fcd32-7005-4476-9336-418db862498a";
        private const string EvangelistHellknightClass10Align = "EvangelistHellknightClass10Align";
        private static readonly string EvangelistHellknightClass10AlignGuid = "318a45da-9327-44d0-97e9-6afc34547314";
        private const string EvangelistHellknightSigniferClass0Align = "EvangelistHellknightSigniferClass0Align";
        private static readonly string EvangelistHellknightSigniferClass0AlignGuid = "915c5826-a07c-45ba-a0a8-23be65f377fe";
        internal const string EvangelistHellknightSigniferClass0AlignDisplayName = "EvangelistHellknightSigniferClass0Align.Name";
        private const string EvangelistHellknightSigniferClass0AlignDescription = "EvangelistHellknightSigniferClass0Align.Description";
        private const string EvangelistHellknightSigniferClass2Align = "EvangelistHellknightSigniferClass2Align";
        private static readonly string EvangelistHellknightSigniferClass2AlignGuid = "c6f3b988-be52-47c3-9f9a-102c3c060c41";
        private const string EvangelistHellknightSigniferClass3Align = "EvangelistHellknightSigniferClass3Align";
        private static readonly string EvangelistHellknightSigniferClass3AlignGuid = "766f8dd4-6a1d-4064-bcb3-3cfeff8368be";
        private const string EvangelistHellknightSigniferClass4Align = "EvangelistHellknightSigniferClass4Align";
        private static readonly string EvangelistHellknightSigniferClass4AlignGuid = "9590c22d-3290-4e86-86db-771ccd84a52f";
        private const string EvangelistHellknightSigniferClass5Align = "EvangelistHellknightSigniferClass5Align";
        private static readonly string EvangelistHellknightSigniferClass5AlignGuid = "f1eda1bd-ebe8-4afd-9196-a91ee5a43ce5";
        private const string EvangelistHellknightSigniferClass6Align = "EvangelistHellknightSigniferClass6Align";
        private static readonly string EvangelistHellknightSigniferClass6AlignGuid = "2a92ef8a-9e86-4011-9c5d-93dd58691d63";
        private const string EvangelistHellknightSigniferClass7Align = "EvangelistHellknightSigniferClass7Align";
        private static readonly string EvangelistHellknightSigniferClass7AlignGuid = "edcdc39e-fe60-41e3-b955-e16d17d1e131";
        private const string EvangelistHellknightSigniferClass8Align = "EvangelistHellknightSigniferClass8Align";
        private static readonly string EvangelistHellknightSigniferClass8AlignGuid = "e444c85f-2579-49f0-a659-395c39ad0e98";
        private const string EvangelistHellknightSigniferClass9Align = "EvangelistHellknightSigniferClass9Align";
        private static readonly string EvangelistHellknightSigniferClass9AlignGuid = "e16600e8-fb3b-4f34-9a2f-25cb6b9b8a67";
        private const string EvangelistHellknightSigniferClass10Align = "EvangelistHellknightSigniferClass10Align";
        private static readonly string EvangelistHellknightSigniferClass10AlignGuid = "b1da5190-6b0c-47f2-a70b-d1146b284717";
        private const string EvangelistHunterClass0Align = "EvangelistHunterClass0Align";
        private static readonly string EvangelistHunterClass0AlignGuid = "f6000c5e-74b2-42bc-9094-aa41c6636f48";
        internal const string EvangelistHunterClass0AlignDisplayName = "EvangelistHunterClass0Align.Name";
        private const string EvangelistHunterClass0AlignDescription = "EvangelistHunterClass0Align.Description";
        private const string EvangelistHunterClass2Align = "EvangelistHunterClass2Align";
        private static readonly string EvangelistHunterClass2AlignGuid = "b9911926-1866-48fc-9bea-f653ea1f502a";
        private const string EvangelistHunterClass3Align = "EvangelistHunterClass3Align";
        private static readonly string EvangelistHunterClass3AlignGuid = "b15615de-5ce6-458e-9d03-5400612ac471";
        private const string EvangelistHunterClass4Align = "EvangelistHunterClass4Align";
        private static readonly string EvangelistHunterClass4AlignGuid = "fda38f79-f7e4-4cc7-bc83-e4c026231af0";
        private const string EvangelistHunterClass5Align = "EvangelistHunterClass5Align";
        private static readonly string EvangelistHunterClass5AlignGuid = "7d0ffdb0-dfd0-4989-9381-2180a726a96f";
        private const string EvangelistHunterClass6Align = "EvangelistHunterClass6Align";
        private static readonly string EvangelistHunterClass6AlignGuid = "7ddd3531-01b2-4d35-952c-1d8705e8989f";
        private const string EvangelistHunterClass7Align = "EvangelistHunterClass7Align";
        private static readonly string EvangelistHunterClass7AlignGuid = "d8fadc3d-9326-4e6b-849a-8233335f8b5a";
        private const string EvangelistHunterClass8Align = "EvangelistHunterClass8Align";
        private static readonly string EvangelistHunterClass8AlignGuid = "a7d3d830-9fd2-4e65-a2c3-b5c591467a3f";
        private const string EvangelistHunterClass9Align = "EvangelistHunterClass9Align";
        private static readonly string EvangelistHunterClass9AlignGuid = "ec4f79f0-c7e9-472e-9276-5f1c1fdd2966";
        private const string EvangelistHunterClass10Align = "EvangelistHunterClass10Align";
        private static readonly string EvangelistHunterClass10AlignGuid = "793e93e6-c1d4-4ef3-97e7-801dbd637107";
        private const string EvangelistInquisitorClass0Align = "EvangelistInquisitorClass0Align";
        private static readonly string EvangelistInquisitorClass0AlignGuid = "422e2b29-996a-4cce-a28a-ab2fcf5a2972";
        internal const string EvangelistInquisitorClass0AlignDisplayName = "EvangelistInquisitorClass0Align.Name";
        private const string EvangelistInquisitorClass0AlignDescription = "EvangelistInquisitorClass0Align.Description";
        private const string EvangelistInquisitorClass2Align = "EvangelistInquisitorClass2Align";
        private static readonly string EvangelistInquisitorClass2AlignGuid = "bf1e7993-e335-4773-b63e-f9503e7203c7";
        private const string EvangelistInquisitorClass3Align = "EvangelistInquisitorClass3Align";
        private static readonly string EvangelistInquisitorClass3AlignGuid = "c55d1a90-711d-46d8-9943-f7bf84279f62";
        private const string EvangelistInquisitorClass4Align = "EvangelistInquisitorClass4Align";
        private static readonly string EvangelistInquisitorClass4AlignGuid = "5f140c8d-9d8b-4a4e-8416-e2407874c26b";
        private const string EvangelistInquisitorClass5Align = "EvangelistInquisitorClass5Align";
        private static readonly string EvangelistInquisitorClass5AlignGuid = "fe5bd0b2-092a-4178-9727-dae6a42129a0";
        private const string EvangelistInquisitorClass6Align = "EvangelistInquisitorClass6Align";
        private static readonly string EvangelistInquisitorClass6AlignGuid = "38ba4226-f47b-479d-9a39-bd5e7318824c";
        private const string EvangelistInquisitorClass7Align = "EvangelistInquisitorClass7Align";
        private static readonly string EvangelistInquisitorClass7AlignGuid = "591b9e7d-c25b-4ada-b86b-ad529b039b6c";
        private const string EvangelistInquisitorClass8Align = "EvangelistInquisitorClass8Align";
        private static readonly string EvangelistInquisitorClass8AlignGuid = "eb67fb7c-3c89-4940-ba83-d538254f7b67";
        private const string EvangelistInquisitorClass9Align = "EvangelistInquisitorClass9Align";
        private static readonly string EvangelistInquisitorClass9AlignGuid = "04f363a4-6d89-4798-acd3-876c3636bf1b";
        private const string EvangelistInquisitorClass10Align = "EvangelistInquisitorClass10Align";
        private static readonly string EvangelistInquisitorClass10AlignGuid = "66f8bb79-3ea3-4380-b5d7-319c10a5db1a";
        private const string EvangelistKineticistClass0Align = "EvangelistKineticistClass0Align";
        private static readonly string EvangelistKineticistClass0AlignGuid = "303454c5-874d-43f7-953d-1eb6c91fbeb5";
        internal const string EvangelistKineticistClass0AlignDisplayName = "EvangelistKineticistClass0Align.Name";
        private const string EvangelistKineticistClass0AlignDescription = "EvangelistKineticistClass0Align.Description";
        private const string EvangelistKineticistClass2Align = "EvangelistKineticistClass2Align";
        private static readonly string EvangelistKineticistClass2AlignGuid = "d44cd437-bba7-4036-84d4-537bfed3ed47";
        private const string EvangelistKineticistClass3Align = "EvangelistKineticistClass3Align";
        private static readonly string EvangelistKineticistClass3AlignGuid = "8a3f7818-61ec-465e-93f0-40e451b1a095";
        private const string EvangelistKineticistClass4Align = "EvangelistKineticistClass4Align";
        private static readonly string EvangelistKineticistClass4AlignGuid = "bbe5daf7-c622-43be-95d6-61d198061ce6";
        private const string EvangelistKineticistClass5Align = "EvangelistKineticistClass5Align";
        private static readonly string EvangelistKineticistClass5AlignGuid = "78e55bdc-96d5-40cf-9c09-a6caaf3043a7";
        private const string EvangelistKineticistClass6Align = "EvangelistKineticistClass6Align";
        private static readonly string EvangelistKineticistClass6AlignGuid = "a0756d03-c195-4177-88ec-1286d53ade99";
        private const string EvangelistKineticistClass7Align = "EvangelistKineticistClass7Align";
        private static readonly string EvangelistKineticistClass7AlignGuid = "0c90ce36-1da3-4ebf-be0e-b7adc2e644bb";
        private const string EvangelistKineticistClass8Align = "EvangelistKineticistClass8Align";
        private static readonly string EvangelistKineticistClass8AlignGuid = "3685d9cf-e387-4524-97d9-21628cebb522";
        private const string EvangelistKineticistClass9Align = "EvangelistKineticistClass9Align";
        private static readonly string EvangelistKineticistClass9AlignGuid = "ec3832d8-b4c9-447a-913d-1a79d80d4f45";
        private const string EvangelistKineticistClass10Align = "EvangelistKineticistClass10Align";
        private static readonly string EvangelistKineticistClass10AlignGuid = "34146773-20ea-4b5a-b09e-32a6756a714b";
        private const string EvangelistLoremasterClass0Align = "EvangelistLoremasterClass0Align";
        private static readonly string EvangelistLoremasterClass0AlignGuid = "246363d1-2e77-4ef7-b3cd-cb29678aabcc";
        internal const string EvangelistLoremasterClass0AlignDisplayName = "EvangelistLoremasterClass0Align.Name";
        private const string EvangelistLoremasterClass0AlignDescription = "EvangelistLoremasterClass0Align.Description";
        private const string EvangelistLoremasterClass2Align = "EvangelistLoremasterClass2Align";
        private static readonly string EvangelistLoremasterClass2AlignGuid = "f0349713-79d8-4096-a349-e9d696130ae5";
        private const string EvangelistLoremasterClass3Align = "EvangelistLoremasterClass3Align";
        private static readonly string EvangelistLoremasterClass3AlignGuid = "b56cd9c0-df35-4914-b34f-a8058ee5bf1e";
        private const string EvangelistLoremasterClass4Align = "EvangelistLoremasterClass4Align";
        private static readonly string EvangelistLoremasterClass4AlignGuid = "bac41f31-e6d7-4fc0-a273-3c32f1a5f5b7";
        private const string EvangelistLoremasterClass5Align = "EvangelistLoremasterClass5Align";
        private static readonly string EvangelistLoremasterClass5AlignGuid = "afb9dd8c-6c56-4810-b123-9cda7ed9e7bf";
        private const string EvangelistLoremasterClass6Align = "EvangelistLoremasterClass6Align";
        private static readonly string EvangelistLoremasterClass6AlignGuid = "49ccdc9a-826c-4355-8850-668aee44c49d";
        private const string EvangelistLoremasterClass7Align = "EvangelistLoremasterClass7Align";
        private static readonly string EvangelistLoremasterClass7AlignGuid = "03e533db-3c6f-4717-b110-072760eed95c";
        private const string EvangelistLoremasterClass8Align = "EvangelistLoremasterClass8Align";
        private static readonly string EvangelistLoremasterClass8AlignGuid = "4a3dac9c-1aee-41a6-96a2-db710f9b298b";
        private const string EvangelistLoremasterClass9Align = "EvangelistLoremasterClass9Align";
        private static readonly string EvangelistLoremasterClass9AlignGuid = "7cdb007e-840b-426f-ad13-3833c0678058";
        private const string EvangelistLoremasterClass10Align = "EvangelistLoremasterClass10Align";
        private static readonly string EvangelistLoremasterClass10AlignGuid = "267dac53-66d7-4570-9b81-b43c26f6470e";
        private const string EvangelistMagusClass0Align = "EvangelistMagusClass0Align";
        private static readonly string EvangelistMagusClass0AlignGuid = "abefdeff-6a93-40da-afd7-31f1feda4850";
        internal const string EvangelistMagusClass0AlignDisplayName = "EvangelistMagusClass0Align.Name";
        private const string EvangelistMagusClass0AlignDescription = "EvangelistMagusClass0Align.Description";
        private const string EvangelistMagusClass2Align = "EvangelistMagusClass2Align";
        private static readonly string EvangelistMagusClass2AlignGuid = "65e1586c-5538-4c70-b7be-5ecc390aa064";
        private const string EvangelistMagusClass3Align = "EvangelistMagusClass3Align";
        private static readonly string EvangelistMagusClass3AlignGuid = "349adffd-06e6-4a82-9f13-eb7453f7bee7";
        private const string EvangelistMagusClass4Align = "EvangelistMagusClass4Align";
        private static readonly string EvangelistMagusClass4AlignGuid = "d181fc64-1a22-44ca-b053-816565ef7dbc";
        private const string EvangelistMagusClass5Align = "EvangelistMagusClass5Align";
        private static readonly string EvangelistMagusClass5AlignGuid = "b2565a41-3b2b-43c1-acbd-59606a508c5b";
        private const string EvangelistMagusClass6Align = "EvangelistMagusClass6Align";
        private static readonly string EvangelistMagusClass6AlignGuid = "5b2d267e-ab01-4831-88da-7ce359d72d68";
        private const string EvangelistMagusClass7Align = "EvangelistMagusClass7Align";
        private static readonly string EvangelistMagusClass7AlignGuid = "de58db76-c2e8-4bd0-a4e8-5767e1ad1c1b";
        private const string EvangelistMagusClass8Align = "EvangelistMagusClass8Align";
        private static readonly string EvangelistMagusClass8AlignGuid = "6b0bd0d7-a250-4b19-b7ca-399639aaffcc";
        private const string EvangelistMagusClass9Align = "EvangelistMagusClass9Align";
        private static readonly string EvangelistMagusClass9AlignGuid = "ae263f90-4a10-4845-96f9-b56687874a4a";
        private const string EvangelistMagusClass10Align = "EvangelistMagusClass10Align";
        private static readonly string EvangelistMagusClass10AlignGuid = "a3802b8f-b829-49d8-8437-6e0174b3491c";
        private const string EvangelistMonkClass0Align = "EvangelistMonkClass0Align";
        private static readonly string EvangelistMonkClass0AlignGuid = "2fadde24-ef62-400b-9d99-4c18264d31eb";
        internal const string EvangelistMonkClass0AlignDisplayName = "EvangelistMonkClass0Align.Name";
        private const string EvangelistMonkClass0AlignDescription = "EvangelistMonkClass0Align.Description";
        private const string EvangelistMonkClass2Align = "EvangelistMonkClass2Align";
        private static readonly string EvangelistMonkClass2AlignGuid = "92d70f7b-39d8-4a4d-8524-6e3817da3e6f";
        private const string EvangelistMonkClass3Align = "EvangelistMonkClass3Align";
        private static readonly string EvangelistMonkClass3AlignGuid = "885aa1e5-031c-4176-8d7d-00d1847bdc5c";
        private const string EvangelistMonkClass4Align = "EvangelistMonkClass4Align";
        private static readonly string EvangelistMonkClass4AlignGuid = "d41688a8-a397-456d-88c8-d9ade2641d1d";
        private const string EvangelistMonkClass5Align = "EvangelistMonkClass5Align";
        private static readonly string EvangelistMonkClass5AlignGuid = "10a19c50-9693-4609-9f7e-7b82690b7ef3";
        private const string EvangelistMonkClass6Align = "EvangelistMonkClass6Align";
        private static readonly string EvangelistMonkClass6AlignGuid = "d2a1c0ad-5e6b-4eb4-af72-fda7e61de37f";
        private const string EvangelistMonkClass7Align = "EvangelistMonkClass7Align";
        private static readonly string EvangelistMonkClass7AlignGuid = "24c41f14-d517-481c-ab31-5fbfa8c88430";
        private const string EvangelistMonkClass8Align = "EvangelistMonkClass8Align";
        private static readonly string EvangelistMonkClass8AlignGuid = "e478ad51-7a8d-49c9-9724-5256a824a978";
        private const string EvangelistMonkClass9Align = "EvangelistMonkClass9Align";
        private static readonly string EvangelistMonkClass9AlignGuid = "5d54c3a9-8142-4082-8030-df6b3f6fd93d";
        private const string EvangelistMonkClass10Align = "EvangelistMonkClass10Align";
        private static readonly string EvangelistMonkClass10AlignGuid = "fe5cfc2e-0b7c-4178-b550-cbe8061b40bf";
        private const string EvangelistMysticTheurgeClass0Align = "EvangelistMysticTheurgeClass0Align";
        private static readonly string EvangelistMysticTheurgeClass0AlignGuid = "d68a5ea8-5102-4d5a-ae1d-96935e71f206";
        internal const string EvangelistMysticTheurgeClass0AlignDisplayName = "EvangelistMysticTheurgeClass0Align.Name";
        private const string EvangelistMysticTheurgeClass0AlignDescription = "EvangelistMysticTheurgeClass0Align.Description";
        private const string EvangelistMysticTheurgeClass2Align = "EvangelistMysticTheurgeClass2Align";
        private static readonly string EvangelistMysticTheurgeClass2AlignGuid = "3037a91a-6fe5-4150-9465-4359ee6ed9f9";
        private const string EvangelistMysticTheurgeClass3Align = "EvangelistMysticTheurgeClass3Align";
        private static readonly string EvangelistMysticTheurgeClass3AlignGuid = "d706a57e-aede-4f54-99ea-4b670184d8b9";
        private const string EvangelistMysticTheurgeClass4Align = "EvangelistMysticTheurgeClass4Align";
        private static readonly string EvangelistMysticTheurgeClass4AlignGuid = "018deeed-c581-4816-9d9d-e3cae659c4f4";
        private const string EvangelistMysticTheurgeClass5Align = "EvangelistMysticTheurgeClass5Align";
        private static readonly string EvangelistMysticTheurgeClass5AlignGuid = "80bae8a9-5772-400b-b69d-4390d4cd66f9";
        private const string EvangelistMysticTheurgeClass6Align = "EvangelistMysticTheurgeClass6Align";
        private static readonly string EvangelistMysticTheurgeClass6AlignGuid = "21cd35fa-9c31-46d4-8ecd-61143e19e33e";
        private const string EvangelistMysticTheurgeClass7Align = "EvangelistMysticTheurgeClass7Align";
        private static readonly string EvangelistMysticTheurgeClass7AlignGuid = "366bc598-aeba-4210-8a8f-7de79844fc16";
        private const string EvangelistMysticTheurgeClass8Align = "EvangelistMysticTheurgeClass8Align";
        private static readonly string EvangelistMysticTheurgeClass8AlignGuid = "d21a883a-b583-4e6c-80b6-f06434ecac3b";
        private const string EvangelistMysticTheurgeClass9Align = "EvangelistMysticTheurgeClass9Align";
        private static readonly string EvangelistMysticTheurgeClass9AlignGuid = "770a6d4d-5e9b-4d2a-9b9a-c2e3bcddbf39";
        private const string EvangelistMysticTheurgeClass10Align = "EvangelistMysticTheurgeClass10Align";
        private static readonly string EvangelistMysticTheurgeClass10AlignGuid = "c9552dd5-7dfe-43c4-8357-f796529635a5";
        private const string EvangelistOracleClass0Align = "EvangelistOracleClass0Align";
        private static readonly string EvangelistOracleClass0AlignGuid = "b2224e46-4c61-4572-9754-4e8866ab7a7a";
        internal const string EvangelistOracleClass0AlignDisplayName = "EvangelistOracleClass0Align.Name";
        private const string EvangelistOracleClass0AlignDescription = "EvangelistOracleClass0Align.Description";
        private const string EvangelistOracleClass2Align = "EvangelistOracleClass2Align";
        private static readonly string EvangelistOracleClass2AlignGuid = "f8abd3cd-a44c-4c9f-8695-0d59117e9142";
        private const string EvangelistOracleClass3Align = "EvangelistOracleClass3Align";
        private static readonly string EvangelistOracleClass3AlignGuid = "4d102c63-295d-4d75-9e12-93401afc9353";
        private const string EvangelistOracleClass4Align = "EvangelistOracleClass4Align";
        private static readonly string EvangelistOracleClass4AlignGuid = "e38fde1d-0359-4264-9000-5cd5aa105422";
        private const string EvangelistOracleClass5Align = "EvangelistOracleClass5Align";
        private static readonly string EvangelistOracleClass5AlignGuid = "134dc6b5-a275-4f23-8920-e754a6cdda89";
        private const string EvangelistOracleClass6Align = "EvangelistOracleClass6Align";
        private static readonly string EvangelistOracleClass6AlignGuid = "077dc349-4cc5-4ad5-97a3-596fe92cb3f3";
        private const string EvangelistOracleClass7Align = "EvangelistOracleClass7Align";
        private static readonly string EvangelistOracleClass7AlignGuid = "4a886d79-f223-45b7-b99a-199c4c9aa6c1";
        private const string EvangelistOracleClass8Align = "EvangelistOracleClass8Align";
        private static readonly string EvangelistOracleClass8AlignGuid = "a4222ca5-f27e-46e1-b489-0f672119696e";
        private const string EvangelistOracleClass9Align = "EvangelistOracleClass9Align";
        private static readonly string EvangelistOracleClass9AlignGuid = "3fdc7e13-0cde-4873-add3-f1d52b2c06c5";
        private const string EvangelistOracleClass10Align = "EvangelistOracleClass10Align";
        private static readonly string EvangelistOracleClass10AlignGuid = "debf7edd-3751-40a9-b22e-a337732bcfc4";
        private const string EvangelistPaladinClass0Align = "EvangelistPaladinClass0Align";
        private static readonly string EvangelistPaladinClass0AlignGuid = "c856b35e-e63b-45d8-a26e-13d7dd315203";
        internal const string EvangelistPaladinClass0AlignDisplayName = "EvangelistPaladinClass0Align.Name";
        private const string EvangelistPaladinClass0AlignDescription = "EvangelistPaladinClass0Align.Description";
        private const string EvangelistPaladinClass2Align = "EvangelistPaladinClass2Align";
        private static readonly string EvangelistPaladinClass2AlignGuid = "165160b9-29e2-44f5-99da-751b9d1ef8b6";
        private const string EvangelistPaladinClass3Align = "EvangelistPaladinClass3Align";
        private static readonly string EvangelistPaladinClass3AlignGuid = "bb196208-1814-4904-b6d3-b59ab73b1559";
        private const string EvangelistPaladinClass4Align = "EvangelistPaladinClass4Align";
        private static readonly string EvangelistPaladinClass4AlignGuid = "cac703d3-7b1e-4198-a8a4-f4c9909e2019";
        private const string EvangelistPaladinClass5Align = "EvangelistPaladinClass5Align";
        private static readonly string EvangelistPaladinClass5AlignGuid = "9e3fdfee-ab3a-43be-ac63-6dc40542fb07";
        private const string EvangelistPaladinClass6Align = "EvangelistPaladinClass6Align";
        private static readonly string EvangelistPaladinClass6AlignGuid = "bc8988b6-1464-438c-8723-07a4c5f0cff5";
        private const string EvangelistPaladinClass7Align = "EvangelistPaladinClass7Align";
        private static readonly string EvangelistPaladinClass7AlignGuid = "2047665a-1313-4e7d-b5bb-4d58033cb468";
        private const string EvangelistPaladinClass8Align = "EvangelistPaladinClass8Align";
        private static readonly string EvangelistPaladinClass8AlignGuid = "acc71ade-2a93-4e0e-893a-00adaada1472";
        private const string EvangelistPaladinClass9Align = "EvangelistPaladinClass9Align";
        private static readonly string EvangelistPaladinClass9AlignGuid = "5e688b9f-d31e-47bd-b4f2-60acfa1e0dad";
        private const string EvangelistPaladinClass10Align = "EvangelistPaladinClass10Align";
        private static readonly string EvangelistPaladinClass10AlignGuid = "09f52921-808c-4981-97c8-366dbf1dd9a8";
        private const string EvangelistRangerClass0Align = "EvangelistRangerClass0Align";
        private static readonly string EvangelistRangerClass0AlignGuid = "4ea056af-41b2-47fb-94ed-01ebe4d3d246";
        internal const string EvangelistRangerClass0AlignDisplayName = "EvangelistRangerClass0Align.Name";
        private const string EvangelistRangerClass0AlignDescription = "EvangelistRangerClass0Align.Description";
        private const string EvangelistRangerClass2Align = "EvangelistRangerClass2Align";
        private static readonly string EvangelistRangerClass2AlignGuid = "a77ed909-cd1f-4d6c-9074-8fa3bb6cf4c4";
        private const string EvangelistRangerClass3Align = "EvangelistRangerClass3Align";
        private static readonly string EvangelistRangerClass3AlignGuid = "40596165-3a0c-45c9-aaf2-1b1e148ea7e6";
        private const string EvangelistRangerClass4Align = "EvangelistRangerClass4Align";
        private static readonly string EvangelistRangerClass4AlignGuid = "01e159e7-be02-48c6-9a42-0a6d45b315bd";
        private const string EvangelistRangerClass5Align = "EvangelistRangerClass5Align";
        private static readonly string EvangelistRangerClass5AlignGuid = "9c089cd1-6586-4a67-b51f-e344672efa10";
        private const string EvangelistRangerClass6Align = "EvangelistRangerClass6Align";
        private static readonly string EvangelistRangerClass6AlignGuid = "0125e90a-8311-48cb-8c54-96473c174f4b";
        private const string EvangelistRangerClass7Align = "EvangelistRangerClass7Align";
        private static readonly string EvangelistRangerClass7AlignGuid = "4628ddf1-b8c7-4618-97d7-9f67b5495974";
        private const string EvangelistRangerClass8Align = "EvangelistRangerClass8Align";
        private static readonly string EvangelistRangerClass8AlignGuid = "e8939a0d-4092-4d1d-859c-3d89ca9dd22f";
        private const string EvangelistRangerClass9Align = "EvangelistRangerClass9Align";
        private static readonly string EvangelistRangerClass9AlignGuid = "bf457eaa-d4f8-48f7-abae-4a5ae120ad9b";
        private const string EvangelistRangerClass10Align = "EvangelistRangerClass10Align";
        private static readonly string EvangelistRangerClass10AlignGuid = "93638a80-4b68-4f86-a21f-7fd59364edc7";
        private const string EvangelistRogueClass0Align = "EvangelistRogueClass0Align";
        private static readonly string EvangelistRogueClass0AlignGuid = "557ba039-6969-4c40-897c-dd23cd49373d";
        internal const string EvangelistRogueClass0AlignDisplayName = "EvangelistRogueClass0Align.Name";
        private const string EvangelistRogueClass0AlignDescription = "EvangelistRogueClass0Align.Description";
        private const string EvangelistRogueClass2Align = "EvangelistRogueClass2Align";
        private static readonly string EvangelistRogueClass2AlignGuid = "b9b26ef9-66d8-4c7b-b02f-6478a669afea";
        private const string EvangelistRogueClass3Align = "EvangelistRogueClass3Align";
        private static readonly string EvangelistRogueClass3AlignGuid = "09fd86e3-2647-4860-81d5-cfeeb70ee4d4";
        private const string EvangelistRogueClass4Align = "EvangelistRogueClass4Align";
        private static readonly string EvangelistRogueClass4AlignGuid = "a920a54b-a46c-4c85-be62-bb1f9ac5344f";
        private const string EvangelistRogueClass5Align = "EvangelistRogueClass5Align";
        private static readonly string EvangelistRogueClass5AlignGuid = "2c2604c8-3e9a-4932-bf7f-648dd73f85c4";
        private const string EvangelistRogueClass6Align = "EvangelistRogueClass6Align";
        private static readonly string EvangelistRogueClass6AlignGuid = "16ae6f3b-66cf-4dfe-a864-8175cd4e65de";
        private const string EvangelistRogueClass7Align = "EvangelistRogueClass7Align";
        private static readonly string EvangelistRogueClass7AlignGuid = "dbabe53b-92a4-47f4-bffb-30a6a748d859";
        private const string EvangelistRogueClass8Align = "EvangelistRogueClass8Align";
        private static readonly string EvangelistRogueClass8AlignGuid = "39b49761-224a-4457-9e57-30cfcb2ec3e0";
        private const string EvangelistRogueClass9Align = "EvangelistRogueClass9Align";
        private static readonly string EvangelistRogueClass9AlignGuid = "2e441aa2-b6cc-4d8e-b1a5-f147dc223eb8";
        private const string EvangelistRogueClass10Align = "EvangelistRogueClass10Align";
        private static readonly string EvangelistRogueClass10AlignGuid = "02a194ca-607c-49a2-947b-7b40e87d0803";
        private const string EvangelistShamanClass0Align = "EvangelistShamanClass0Align";
        private static readonly string EvangelistShamanClass0AlignGuid = "66a293d5-197d-435d-bd60-9ba20dd7c69f";
        internal const string EvangelistShamanClass0AlignDisplayName = "EvangelistShamanClass0Align.Name";
        private const string EvangelistShamanClass0AlignDescription = "EvangelistShamanClass0Align.Description";
        private const string EvangelistShamanClass2Align = "EvangelistShamanClass2Align";
        private static readonly string EvangelistShamanClass2AlignGuid = "382a8858-426f-4040-b7a0-3c1fe492fce2";
        private const string EvangelistShamanClass3Align = "EvangelistShamanClass3Align";
        private static readonly string EvangelistShamanClass3AlignGuid = "28d3026c-da97-4259-bfcb-0b410246edae";
        private const string EvangelistShamanClass4Align = "EvangelistShamanClass4Align";
        private static readonly string EvangelistShamanClass4AlignGuid = "e1883040-07d1-4698-9e6a-25edae380082";
        private const string EvangelistShamanClass5Align = "EvangelistShamanClass5Align";
        private static readonly string EvangelistShamanClass5AlignGuid = "19924440-f7af-4a9d-903b-21e8100bfe09";
        private const string EvangelistShamanClass6Align = "EvangelistShamanClass6Align";
        private static readonly string EvangelistShamanClass6AlignGuid = "6de7eeec-5fc5-48a6-a81a-7270ded74b5d";
        private const string EvangelistShamanClass7Align = "EvangelistShamanClass7Align";
        private static readonly string EvangelistShamanClass7AlignGuid = "739cb444-9606-4c4e-b1f4-668a59cbc86f";
        private const string EvangelistShamanClass8Align = "EvangelistShamanClass8Align";
        private static readonly string EvangelistShamanClass8AlignGuid = "34f03d81-470c-4b07-9a32-fb009e44f56e";
        private const string EvangelistShamanClass9Align = "EvangelistShamanClass9Align";
        private static readonly string EvangelistShamanClass9AlignGuid = "fd55fe90-50a5-4d1f-8cd0-407e25519198";
        private const string EvangelistShamanClass10Align = "EvangelistShamanClass10Align";
        private static readonly string EvangelistShamanClass10AlignGuid = "ccc6fd28-24e3-48e1-939f-533983efbd70";
        private const string EvangelistShifterClass0Align = "EvangelistShifterClass0Align";
        private static readonly string EvangelistShifterClass0AlignGuid = "c5c1cebb-7008-409a-8f4c-fb6431b9f192";
        internal const string EvangelistShifterClass0AlignDisplayName = "EvangelistShifterClass0Align.Name";
        private const string EvangelistShifterClass0AlignDescription = "EvangelistShifterClass0Align.Description";
        private const string EvangelistShifterClass2Align = "EvangelistShifterClass2Align";
        private static readonly string EvangelistShifterClass2AlignGuid = "e47a7b64-893a-4913-8de8-2fa66966c59d";
        private const string EvangelistShifterClass3Align = "EvangelistShifterClass3Align";
        private static readonly string EvangelistShifterClass3AlignGuid = "e2db44f6-78c5-435e-8855-e05279f1f0ed";
        private const string EvangelistShifterClass4Align = "EvangelistShifterClass4Align";
        private static readonly string EvangelistShifterClass4AlignGuid = "8ec8cb93-f10c-49f2-a538-c2b4363a2786";
        private const string EvangelistShifterClass5Align = "EvangelistShifterClass5Align";
        private static readonly string EvangelistShifterClass5AlignGuid = "2691054c-999f-4703-b0b8-79e538319335";
        private const string EvangelistShifterClass6Align = "EvangelistShifterClass6Align";
        private static readonly string EvangelistShifterClass6AlignGuid = "021fb575-a542-4b1c-9744-8965560dcf17";
        private const string EvangelistShifterClass7Align = "EvangelistShifterClass7Align";
        private static readonly string EvangelistShifterClass7AlignGuid = "71cbe6e9-9c5d-48ed-ae54-60ad4354f030";
        private const string EvangelistShifterClass8Align = "EvangelistShifterClass8Align";
        private static readonly string EvangelistShifterClass8AlignGuid = "cf3541b1-f745-4515-9e70-2ab550562fbb";
        private const string EvangelistShifterClass9Align = "EvangelistShifterClass9Align";
        private static readonly string EvangelistShifterClass9AlignGuid = "34f4bee6-d6c1-4e23-a34a-849b1c7833f0";
        private const string EvangelistShifterClass10Align = "EvangelistShifterClass10Align";
        private static readonly string EvangelistShifterClass10AlignGuid = "cfdda9c1-2a00-4055-95c8-1fe88a9a2c9a";
        private const string EvangelistSkaldClass0Align = "EvangelistSkaldClass0Align";
        private static readonly string EvangelistSkaldClass0AlignGuid = "f093f276-3541-4fe0-b425-76f0a4a33650";
        internal const string EvangelistSkaldClass0AlignDisplayName = "EvangelistSkaldClass0Align.Name";
        private const string EvangelistSkaldClass0AlignDescription = "EvangelistSkaldClass0Align.Description";
        private const string EvangelistSkaldClass2Align = "EvangelistSkaldClass2Align";
        private static readonly string EvangelistSkaldClass2AlignGuid = "d86a515d-92fd-4751-bb7a-9e388c02ee59";
        private const string EvangelistSkaldClass3Align = "EvangelistSkaldClass3Align";
        private static readonly string EvangelistSkaldClass3AlignGuid = "b783a672-6400-43a3-a4ef-735dc591d293";
        private const string EvangelistSkaldClass4Align = "EvangelistSkaldClass4Align";
        private static readonly string EvangelistSkaldClass4AlignGuid = "016eeae7-64c6-4929-be47-7a01ae38f38e";
        private const string EvangelistSkaldClass5Align = "EvangelistSkaldClass5Align";
        private static readonly string EvangelistSkaldClass5AlignGuid = "ce5d95c1-d1a6-4370-8ff7-ca8f7d2bfade";
        private const string EvangelistSkaldClass6Align = "EvangelistSkaldClass6Align";
        private static readonly string EvangelistSkaldClass6AlignGuid = "297b7f68-6438-4272-bf99-1e314c7de895";
        private const string EvangelistSkaldClass7Align = "EvangelistSkaldClass7Align";
        private static readonly string EvangelistSkaldClass7AlignGuid = "c345b494-2083-4d7a-9222-87f80dcdcf83";
        private const string EvangelistSkaldClass8Align = "EvangelistSkaldClass8Align";
        private static readonly string EvangelistSkaldClass8AlignGuid = "e879df40-c305-4f51-aadf-b1720310e97a";
        private const string EvangelistSkaldClass9Align = "EvangelistSkaldClass9Align";
        private static readonly string EvangelistSkaldClass9AlignGuid = "c56a048f-a094-40e4-951a-a02a2bf7aca2";
        private const string EvangelistSkaldClass10Align = "EvangelistSkaldClass10Align";
        private static readonly string EvangelistSkaldClass10AlignGuid = "578e4b23-8e41-424d-b5f3-d1c7beae49fd";
        private const string EvangelistSlayerClass0Align = "EvangelistSlayerClass0Align";
        private static readonly string EvangelistSlayerClass0AlignGuid = "87bdbca9-7aa8-4f8a-9e0f-d8c0b0dcdcaa";
        internal const string EvangelistSlayerClass0AlignDisplayName = "EvangelistSlayerClass0Align.Name";
        private const string EvangelistSlayerClass0AlignDescription = "EvangelistSlayerClass0Align.Description";
        private const string EvangelistSlayerClass2Align = "EvangelistSlayerClass2Align";
        private static readonly string EvangelistSlayerClass2AlignGuid = "c366c3b1-9578-444f-8c6f-9521a27c2142";
        private const string EvangelistSlayerClass3Align = "EvangelistSlayerClass3Align";
        private static readonly string EvangelistSlayerClass3AlignGuid = "b6b9c0ed-7b1c-4fb2-bf2d-25a5c183ed3f";
        private const string EvangelistSlayerClass4Align = "EvangelistSlayerClass4Align";
        private static readonly string EvangelistSlayerClass4AlignGuid = "422abdd6-f9bd-485c-9479-a872cb0a09eb";
        private const string EvangelistSlayerClass5Align = "EvangelistSlayerClass5Align";
        private static readonly string EvangelistSlayerClass5AlignGuid = "22bb71d0-a2f8-4903-9890-107f98da78e7";
        private const string EvangelistSlayerClass6Align = "EvangelistSlayerClass6Align";
        private static readonly string EvangelistSlayerClass6AlignGuid = "21542935-fd29-40c1-9474-932105188ec8";
        private const string EvangelistSlayerClass7Align = "EvangelistSlayerClass7Align";
        private static readonly string EvangelistSlayerClass7AlignGuid = "fcb6cc62-580b-42b6-b3de-737d59facf05";
        private const string EvangelistSlayerClass8Align = "EvangelistSlayerClass8Align";
        private static readonly string EvangelistSlayerClass8AlignGuid = "b8de95d8-5807-499a-86b6-6f0937b4c6a7";
        private const string EvangelistSlayerClass9Align = "EvangelistSlayerClass9Align";
        private static readonly string EvangelistSlayerClass9AlignGuid = "83d1397d-7630-449c-b197-bc9af65ae026";
        private const string EvangelistSlayerClass10Align = "EvangelistSlayerClass10Align";
        private static readonly string EvangelistSlayerClass10AlignGuid = "1d731015-b901-4968-90e8-daacb1b48f24";
        private const string EvangelistSorcererClass0Align = "EvangelistSorcererClass0Align";
        private static readonly string EvangelistSorcererClass0AlignGuid = "69e2a38c-f902-41b0-8568-c3b73a39f473";
        internal const string EvangelistSorcererClass0AlignDisplayName = "EvangelistSorcererClass0Align.Name";
        private const string EvangelistSorcererClass0AlignDescription = "EvangelistSorcererClass0Align.Description";
        private const string EvangelistSorcererClass2Align = "EvangelistSorcererClass2Align";
        private static readonly string EvangelistSorcererClass2AlignGuid = "5e102263-0ca6-4026-bc8f-580d525b270e";
        private const string EvangelistSorcererClass3Align = "EvangelistSorcererClass3Align";
        private static readonly string EvangelistSorcererClass3AlignGuid = "484ad2ae-3163-44f8-9f82-1a7fdd839851";
        private const string EvangelistSorcererClass4Align = "EvangelistSorcererClass4Align";
        private static readonly string EvangelistSorcererClass4AlignGuid = "6c4e59aa-cf98-438d-a228-bac8daf3b345";
        private const string EvangelistSorcererClass5Align = "EvangelistSorcererClass5Align";
        private static readonly string EvangelistSorcererClass5AlignGuid = "71e01be8-f4c3-48ef-92a8-90c1d1f1db9b";
        private const string EvangelistSorcererClass6Align = "EvangelistSorcererClass6Align";
        private static readonly string EvangelistSorcererClass6AlignGuid = "2c6d3c4c-77ce-4700-b39e-96ae8642b1b3";
        private const string EvangelistSorcererClass7Align = "EvangelistSorcererClass7Align";
        private static readonly string EvangelistSorcererClass7AlignGuid = "e88f8a7e-2e9b-4f9b-8e05-b193ed738f2a";
        private const string EvangelistSorcererClass8Align = "EvangelistSorcererClass8Align";
        private static readonly string EvangelistSorcererClass8AlignGuid = "70b9828c-ea1c-4ae8-aac7-d699ad417632";
        private const string EvangelistSorcererClass9Align = "EvangelistSorcererClass9Align";
        private static readonly string EvangelistSorcererClass9AlignGuid = "9aa203be-f87e-4275-bf14-24b725424f38";
        private const string EvangelistSorcererClass10Align = "EvangelistSorcererClass10Align";
        private static readonly string EvangelistSorcererClass10AlignGuid = "6f0f9477-7b89-4381-bf0e-3933c2c374c3";
        private const string EvangelistStalwartDefenderClass0Align = "EvangelistStalwartDefenderClass0Align";
        private static readonly string EvangelistStalwartDefenderClass0AlignGuid = "dd9c3847-d745-43c2-bb7b-0044252200f3";
        internal const string EvangelistStalwartDefenderClass0AlignDisplayName = "EvangelistStalwartDefenderClass0Align.Name";
        private const string EvangelistStalwartDefenderClass0AlignDescription = "EvangelistStalwartDefenderClass0Align.Description";
        private const string EvangelistStalwartDefenderClass2Align = "EvangelistStalwartDefenderClass2Align";
        private static readonly string EvangelistStalwartDefenderClass2AlignGuid = "c0690cbc-2de4-43f4-8f74-469a9cf4fc4a";
        private const string EvangelistStalwartDefenderClass3Align = "EvangelistStalwartDefenderClass3Align";
        private static readonly string EvangelistStalwartDefenderClass3AlignGuid = "22b1571b-fbe9-4883-adb6-66c36f0eff66";
        private const string EvangelistStalwartDefenderClass4Align = "EvangelistStalwartDefenderClass4Align";
        private static readonly string EvangelistStalwartDefenderClass4AlignGuid = "0be16a05-250e-45b2-958e-f284f613ea31";
        private const string EvangelistStalwartDefenderClass5Align = "EvangelistStalwartDefenderClass5Align";
        private static readonly string EvangelistStalwartDefenderClass5AlignGuid = "0d4b899b-8303-4c00-815d-6ee9f1810d3b";
        private const string EvangelistStalwartDefenderClass6Align = "EvangelistStalwartDefenderClass6Align";
        private static readonly string EvangelistStalwartDefenderClass6AlignGuid = "aea6b10c-b2c8-4c8e-b268-1aaf0c7c0463";
        private const string EvangelistStalwartDefenderClass7Align = "EvangelistStalwartDefenderClass7Align";
        private static readonly string EvangelistStalwartDefenderClass7AlignGuid = "bdfe2b26-5b6f-4d16-8e6b-dc454272486c";
        private const string EvangelistStalwartDefenderClass8Align = "EvangelistStalwartDefenderClass8Align";
        private static readonly string EvangelistStalwartDefenderClass8AlignGuid = "b17ecf68-30c4-4df2-879b-86ea083f7019";
        private const string EvangelistStalwartDefenderClass9Align = "EvangelistStalwartDefenderClass9Align";
        private static readonly string EvangelistStalwartDefenderClass9AlignGuid = "cf6e1785-32e4-4f91-bfa2-168fd08ca5ca";
        private const string EvangelistStalwartDefenderClass10Align = "EvangelistStalwartDefenderClass10Align";
        private static readonly string EvangelistStalwartDefenderClass10AlignGuid = "2da81ecd-e1e9-4acb-8fa9-57d3b8e82fc9";
        private const string EvangelistStudentOfWarClass0Align = "EvangelistStudentOfWarClass0Align";
        private static readonly string EvangelistStudentOfWarClass0AlignGuid = "c77490a7-6f2d-4d47-a8fc-2a2b6f0fd269";
        internal const string EvangelistStudentOfWarClass0AlignDisplayName = "EvangelistStudentOfWarClass0Align.Name";
        private const string EvangelistStudentOfWarClass0AlignDescription = "EvangelistStudentOfWarClass0Align.Description";
        private const string EvangelistStudentOfWarClass2Align = "EvangelistStudentOfWarClass2Align";
        private static readonly string EvangelistStudentOfWarClass2AlignGuid = "8897f7f4-5935-4321-ae31-1badbe64efa1";
        private const string EvangelistStudentOfWarClass3Align = "EvangelistStudentOfWarClass3Align";
        private static readonly string EvangelistStudentOfWarClass3AlignGuid = "43bde82c-fb66-4332-8675-ce642e2410b6";
        private const string EvangelistStudentOfWarClass4Align = "EvangelistStudentOfWarClass4Align";
        private static readonly string EvangelistStudentOfWarClass4AlignGuid = "7c232e08-a273-4e69-9053-3fb65bfa2e79";
        private const string EvangelistStudentOfWarClass5Align = "EvangelistStudentOfWarClass5Align";
        private static readonly string EvangelistStudentOfWarClass5AlignGuid = "5d79d1e1-f9c3-4f6a-92f5-ecf1ad212dc4";
        private const string EvangelistStudentOfWarClass6Align = "EvangelistStudentOfWarClass6Align";
        private static readonly string EvangelistStudentOfWarClass6AlignGuid = "bbad1bcd-8336-4950-a600-6a918bedca65";
        private const string EvangelistStudentOfWarClass7Align = "EvangelistStudentOfWarClass7Align";
        private static readonly string EvangelistStudentOfWarClass7AlignGuid = "4d1b1642-1600-4e50-94b6-def87158f9da";
        private const string EvangelistStudentOfWarClass8Align = "EvangelistStudentOfWarClass8Align";
        private static readonly string EvangelistStudentOfWarClass8AlignGuid = "04b148cd-4a41-4a95-8382-d86caf0d478f";
        private const string EvangelistStudentOfWarClass9Align = "EvangelistStudentOfWarClass9Align";
        private static readonly string EvangelistStudentOfWarClass9AlignGuid = "dd7d7541-8e15-4968-bd22-779252a4a2d4";
        private const string EvangelistStudentOfWarClass10Align = "EvangelistStudentOfWarClass10Align";
        private static readonly string EvangelistStudentOfWarClass10AlignGuid = "9b32d3e2-fac1-4752-b619-773ea0eb2275";
        private const string EvangelistSwordlordClass0Align = "EvangelistSwordlordClass0Align";
        private static readonly string EvangelistSwordlordClass0AlignGuid = "1ea92188-8f91-460e-987b-941d2c0a7cfb";
        internal const string EvangelistSwordlordClass0AlignDisplayName = "EvangelistSwordlordClass0Align.Name";
        private const string EvangelistSwordlordClass0AlignDescription = "EvangelistSwordlordClass0Align.Description";
        private const string EvangelistSwordlordClass2Align = "EvangelistSwordlordClass2Align";
        private static readonly string EvangelistSwordlordClass2AlignGuid = "335b09e1-d22d-4514-a32c-698031c104da";
        private const string EvangelistSwordlordClass3Align = "EvangelistSwordlordClass3Align";
        private static readonly string EvangelistSwordlordClass3AlignGuid = "184cf8c2-f843-4777-b81e-4fa0e171d1f1";
        private const string EvangelistSwordlordClass4Align = "EvangelistSwordlordClass4Align";
        private static readonly string EvangelistSwordlordClass4AlignGuid = "8ec100c1-e6ad-47ae-ac1e-11a9c9723a57";
        private const string EvangelistSwordlordClass5Align = "EvangelistSwordlordClass5Align";
        private static readonly string EvangelistSwordlordClass5AlignGuid = "6e15e380-2252-4e61-9f19-cd05335546f1";
        private const string EvangelistSwordlordClass6Align = "EvangelistSwordlordClass6Align";
        private static readonly string EvangelistSwordlordClass6AlignGuid = "47fde76d-95cd-453b-b04c-77ab516ba3e6";
        private const string EvangelistSwordlordClass7Align = "EvangelistSwordlordClass7Align";
        private static readonly string EvangelistSwordlordClass7AlignGuid = "498d8d42-f326-4e54-a09f-bfc3f69e081b";
        private const string EvangelistSwordlordClass8Align = "EvangelistSwordlordClass8Align";
        private static readonly string EvangelistSwordlordClass8AlignGuid = "882ea7ce-8107-4015-8438-6e802fb06528";
        private const string EvangelistSwordlordClass9Align = "EvangelistSwordlordClass9Align";
        private static readonly string EvangelistSwordlordClass9AlignGuid = "8bd02ed7-b543-4499-b59d-3d43395f2bbb";
        private const string EvangelistSwordlordClass10Align = "EvangelistSwordlordClass10Align";
        private static readonly string EvangelistSwordlordClass10AlignGuid = "9913597e-2dcb-4b7e-90ac-8975238240f0";
        private const string EvangelistWarpriestClass0Align = "EvangelistWarpriestClass0Align";
        private static readonly string EvangelistWarpriestClass0AlignGuid = "6de8857a-d5c6-4d55-9842-5ed2f5e59fd9";
        internal const string EvangelistWarpriestClass0AlignDisplayName = "EvangelistWarpriestClass0Align.Name";
        private const string EvangelistWarpriestClass0AlignDescription = "EvangelistWarpriestClass0Align.Description";
        private const string EvangelistWarpriestClass2Align = "EvangelistWarpriestClass2Align";
        private static readonly string EvangelistWarpriestClass2AlignGuid = "e0e70f4a-ec49-4079-8b01-c17c7000bb9a";
        private const string EvangelistWarpriestClass3Align = "EvangelistWarpriestClass3Align";
        private static readonly string EvangelistWarpriestClass3AlignGuid = "874f5e04-3ec2-4e8f-bf35-754d8ad136ae";
        private const string EvangelistWarpriestClass4Align = "EvangelistWarpriestClass4Align";
        private static readonly string EvangelistWarpriestClass4AlignGuid = "d19ae4a4-27a2-41d1-b666-b70d2e3036af";
        private const string EvangelistWarpriestClass5Align = "EvangelistWarpriestClass5Align";
        private static readonly string EvangelistWarpriestClass5AlignGuid = "26863af5-cd30-41eb-98c3-4cdf7d93e5c5";
        private const string EvangelistWarpriestClass6Align = "EvangelistWarpriestClass6Align";
        private static readonly string EvangelistWarpriestClass6AlignGuid = "10238139-14dd-48ef-824f-a040a73aaa4b";
        private const string EvangelistWarpriestClass7Align = "EvangelistWarpriestClass7Align";
        private static readonly string EvangelistWarpriestClass7AlignGuid = "4b3478a2-272c-43c4-86ac-83d787f16bce";
        private const string EvangelistWarpriestClass8Align = "EvangelistWarpriestClass8Align";
        private static readonly string EvangelistWarpriestClass8AlignGuid = "bbff22ee-91ea-4665-99d0-8d2ffe1c2bd4";
        private const string EvangelistWarpriestClass9Align = "EvangelistWarpriestClass9Align";
        private static readonly string EvangelistWarpriestClass9AlignGuid = "3ab5b3f2-cd22-44a4-8c9e-aa94f86668c1";
        private const string EvangelistWarpriestClass10Align = "EvangelistWarpriestClass10Align";
        private static readonly string EvangelistWarpriestClass10AlignGuid = "86d4319a-d169-4b95-b391-96574f73ed83";
        private const string EvangelistWinterWitchClass0Align = "EvangelistWinterWitchClass0Align";
        private static readonly string EvangelistWinterWitchClass0AlignGuid = "3c189a70-1a4c-42a4-9f37-9358ec31f2c5";
        internal const string EvangelistWinterWitchClass0AlignDisplayName = "EvangelistWinterWitchClass0Align.Name";
        private const string EvangelistWinterWitchClass0AlignDescription = "EvangelistWinterWitchClass0Align.Description";
        private const string EvangelistWinterWitchClass2Align = "EvangelistWinterWitchClass2Align";
        private static readonly string EvangelistWinterWitchClass2AlignGuid = "9b74bae9-bd2c-46b8-8de3-9dd1ab0ddb4e";
        private const string EvangelistWinterWitchClass3Align = "EvangelistWinterWitchClass3Align";
        private static readonly string EvangelistWinterWitchClass3AlignGuid = "61df8f13-4666-4b0a-b6ac-b0fb5d38f87c";
        private const string EvangelistWinterWitchClass4Align = "EvangelistWinterWitchClass4Align";
        private static readonly string EvangelistWinterWitchClass4AlignGuid = "12c017c7-8425-41a6-92cc-a716c986c697";
        private const string EvangelistWinterWitchClass5Align = "EvangelistWinterWitchClass5Align";
        private static readonly string EvangelistWinterWitchClass5AlignGuid = "a0a8a245-668a-48cc-a235-72278133a371";
        private const string EvangelistWinterWitchClass6Align = "EvangelistWinterWitchClass6Align";
        private static readonly string EvangelistWinterWitchClass6AlignGuid = "3492565e-749b-4c52-b105-29ca5e51bea0";
        private const string EvangelistWinterWitchClass7Align = "EvangelistWinterWitchClass7Align";
        private static readonly string EvangelistWinterWitchClass7AlignGuid = "5ef5c0cc-e52d-4cd6-8616-a5064a1d4a39";
        private const string EvangelistWinterWitchClass8Align = "EvangelistWinterWitchClass8Align";
        private static readonly string EvangelistWinterWitchClass8AlignGuid = "8ce152d7-ccd8-417f-823a-22b7840b997e";
        private const string EvangelistWinterWitchClass9Align = "EvangelistWinterWitchClass9Align";
        private static readonly string EvangelistWinterWitchClass9AlignGuid = "00b398b6-7647-4f20-8207-cbd27da458e6";
        private const string EvangelistWinterWitchClass10Align = "EvangelistWinterWitchClass10Align";
        private static readonly string EvangelistWinterWitchClass10AlignGuid = "81ee5272-4b94-4786-8e0e-b3d2d8fe5704";
        private const string EvangelistWitchClass0Align = "EvangelistWitchClass0Align";
        private static readonly string EvangelistWitchClass0AlignGuid = "40e9a6ea-2f7d-4ee9-b756-b97e70f01f64";
        internal const string EvangelistWitchClass0AlignDisplayName = "EvangelistWitchClass0Align.Name";
        private const string EvangelistWitchClass0AlignDescription = "EvangelistWitchClass0Align.Description";
        private const string EvangelistWitchClass2Align = "EvangelistWitchClass2Align";
        private static readonly string EvangelistWitchClass2AlignGuid = "7dc3a4cb-5fd6-4e2b-a079-8632aece5cef";
        private const string EvangelistWitchClass3Align = "EvangelistWitchClass3Align";
        private static readonly string EvangelistWitchClass3AlignGuid = "4c60f4de-3518-4a89-a8ec-a6d12ec75611";
        private const string EvangelistWitchClass4Align = "EvangelistWitchClass4Align";
        private static readonly string EvangelistWitchClass4AlignGuid = "8f4aa638-42c7-4433-b568-c5aebc2e827f";
        private const string EvangelistWitchClass5Align = "EvangelistWitchClass5Align";
        private static readonly string EvangelistWitchClass5AlignGuid = "a5aa5070-83d5-4dfa-8554-ed782e01465f";
        private const string EvangelistWitchClass6Align = "EvangelistWitchClass6Align";
        private static readonly string EvangelistWitchClass6AlignGuid = "c31ddacb-f5ab-43e9-9807-be86154de0f1";
        private const string EvangelistWitchClass7Align = "EvangelistWitchClass7Align";
        private static readonly string EvangelistWitchClass7AlignGuid = "34cf5690-2df8-4b40-a6b3-3e0634ce94ef";
        private const string EvangelistWitchClass8Align = "EvangelistWitchClass8Align";
        private static readonly string EvangelistWitchClass8AlignGuid = "c4f30ed7-bab4-47fa-bd79-f060cc63aa3e";
        private const string EvangelistWitchClass9Align = "EvangelistWitchClass9Align";
        private static readonly string EvangelistWitchClass9AlignGuid = "1bba8b98-4e99-4e1d-aa28-b48d299256ff";
        private const string EvangelistWitchClass10Align = "EvangelistWitchClass10Align";
        private static readonly string EvangelistWitchClass10AlignGuid = "73955d2b-ed47-4764-a3f1-16740bbc3a08";
        private const string EvangelistWizardClass0Align = "EvangelistWizardClass0Align";
        private static readonly string EvangelistWizardClass0AlignGuid = "908efe6f-a8b4-4275-90b0-bc9158438165";
        internal const string EvangelistWizardClass0AlignDisplayName = "EvangelistWizardClass0Align.Name";
        private const string EvangelistWizardClass0AlignDescription = "EvangelistWizardClass0Align.Description";
        private const string EvangelistWizardClass2Align = "EvangelistWizardClass2Align";
        private static readonly string EvangelistWizardClass2AlignGuid = "1f9ce743-71fe-4ce3-be60-a41d2e6e0b90";
        private const string EvangelistWizardClass3Align = "EvangelistWizardClass3Align";
        private static readonly string EvangelistWizardClass3AlignGuid = "e5dc8708-a2b0-440f-b126-257f17cb1446";
        private const string EvangelistWizardClass4Align = "EvangelistWizardClass4Align";
        private static readonly string EvangelistWizardClass4AlignGuid = "dc6ee426-aac0-4393-93a9-a8e0112d198d";
        private const string EvangelistWizardClass5Align = "EvangelistWizardClass5Align";
        private static readonly string EvangelistWizardClass5AlignGuid = "27c269a5-ee03-436d-9e01-65dadbbc29d8";
        private const string EvangelistWizardClass6Align = "EvangelistWizardClass6Align";
        private static readonly string EvangelistWizardClass6AlignGuid = "57b07662-7f77-4588-8937-3a74a6568af4";
        private const string EvangelistWizardClass7Align = "EvangelistWizardClass7Align";
        private static readonly string EvangelistWizardClass7AlignGuid = "1f098b53-9ff8-44ea-8847-bd6fe67d2dd7";
        private const string EvangelistWizardClass8Align = "EvangelistWizardClass8Align";
        private static readonly string EvangelistWizardClass8AlignGuid = "82e6d98d-f720-4601-8a16-6997b9d6b608";
        private const string EvangelistWizardClass9Align = "EvangelistWizardClass9Align";
        private static readonly string EvangelistWizardClass9AlignGuid = "b036a5cf-9e65-47cf-bc3f-8c0c33d46b88";
        private const string EvangelistWizardClass10Align = "EvangelistWizardClass10Align";
        private static readonly string EvangelistWizardClass10AlignGuid = "72ef66d3-8787-4a8e-94bc-ea09758a012a";

        private const string EvangelistGunslingerClass0Align = "EvangelistGunslingerClass0Align";
        private static readonly string EvangelistGunslingerClass0AlignGuid = "c95128e0-7f55-4534-8702-dcb3d33b8473";
        internal const string EvangelistGunslingerClass0AlignDisplayName = "EvangelistGunslingerClass0Align.Name";
        private const string EvangelistGunslingerClass0AlignDescription = "EvangelistGunslingerClass0Align.Description";
        private const string EvangelistGunslingerClass2Align = "EvangelistGunslingerClass2Align";
        private static readonly string EvangelistGunslingerClass2AlignGuid = "e33a0720-244e-440f-b874-3e1d53b5edc2";
        private const string EvangelistGunslingerClass3Align = "EvangelistGunslingerClass3Align";
        private static readonly string EvangelistGunslingerClass3AlignGuid = "1b43853d-e680-4e0b-bf55-b8d56f21d27d";
        private const string EvangelistGunslingerClass4Align = "EvangelistGunslingerClass4Align";
        private static readonly string EvangelistGunslingerClass4AlignGuid = "775f70fb-4043-428b-83cc-e9fa0ae30992";
        private const string EvangelistGunslingerClass5Align = "EvangelistGunslingerClass5Align";
        private static readonly string EvangelistGunslingerClass5AlignGuid = "68073c47-2c91-42ba-aae9-58c322747c0d";
        private const string EvangelistGunslingerClass6Align = "EvangelistGunslingerClass6Align";
        private static readonly string EvangelistGunslingerClass6AlignGuid = "2561eaa7-a89d-46f0-a2af-6c4b1d194936";
        private const string EvangelistGunslingerClass7Align = "EvangelistGunslingerClass7Align";
        private static readonly string EvangelistGunslingerClass7AlignGuid = "2ceb2fad-5bee-490a-a9ac-58cc13f0fd62";
        private const string EvangelistGunslingerClass8Align = "EvangelistGunslingerClass8Align";
        private static readonly string EvangelistGunslingerClass8AlignGuid = "7a37cdfa-3f4d-439b-a635-de0637d63e55";
        private const string EvangelistGunslingerClass9Align = "EvangelistGunslingerClass9Align";
        private static readonly string EvangelistGunslingerClass9AlignGuid = "bbfb1bf2-eea5-4d28-b082-abf13023eae5";
        private const string EvangelistGunslingerClass10Align = "EvangelistGunslingerClass10Align";
        private static readonly string EvangelistGunslingerClass10AlignGuid = "66b7b7fe-e209-4be2-809d-161288734258";
        private const string EvangelistAgentoftheGraveClass0Align = "EvangelistAgentoftheGraveClass0Align";
        private static readonly string EvangelistAgentoftheGraveClass0AlignGuid = "ad1f9914-4ca2-4800-98f7-1974a1764004";
        internal const string EvangelistAgentoftheGraveClass0AlignDisplayName = "EvangelistAgentoftheGraveClass0Align.Name";
        private const string EvangelistAgentoftheGraveClass0AlignDescription = "EvangelistAgentoftheGraveClass0Align.Description";
        private const string EvangelistAgentoftheGraveClass2Align = "EvangelistAgentoftheGraveClass2Align";
        private static readonly string EvangelistAgentoftheGraveClass2AlignGuid = "d154c67b-b34c-488b-bf33-00d5d3385317";
        private const string EvangelistAgentoftheGraveClass3Align = "EvangelistAgentoftheGraveClass3Align";
        private static readonly string EvangelistAgentoftheGraveClass3AlignGuid = "8c83af01-dc8c-4ca2-8892-812d54e1f8ce";
        private const string EvangelistAgentoftheGraveClass4Align = "EvangelistAgentoftheGraveClass4Align";
        private static readonly string EvangelistAgentoftheGraveClass4AlignGuid = "ea148794-c487-4650-9ddf-b7efb3a12f1f";
        private const string EvangelistAgentoftheGraveClass5Align = "EvangelistAgentoftheGraveClass5Align";
        private static readonly string EvangelistAgentoftheGraveClass5AlignGuid = "6dfb93f5-e796-4756-8e92-1df67136aa1c";
        private const string EvangelistAgentoftheGraveClass6Align = "EvangelistAgentoftheGraveClass6Align";
        private static readonly string EvangelistAgentoftheGraveClass6AlignGuid = "81b0bb81-62c3-4cfc-96be-444fb380ce5e";
        private const string EvangelistAgentoftheGraveClass7Align = "EvangelistAgentoftheGraveClass7Align";
        private static readonly string EvangelistAgentoftheGraveClass7AlignGuid = "1bbd96c3-1c91-4f6f-84fa-df6bd2936827";
        private const string EvangelistAgentoftheGraveClass8Align = "EvangelistAgentoftheGraveClass8Align";
        private static readonly string EvangelistAgentoftheGraveClass8AlignGuid = "6c16fb0c-a4d3-4451-b213-1a471c22b8bb";
        private const string EvangelistAgentoftheGraveClass9Align = "EvangelistAgentoftheGraveClass9Align";
        private static readonly string EvangelistAgentoftheGraveClass9AlignGuid = "839d6437-9273-4871-b960-17a3a9246fe8";
        private const string EvangelistAgentoftheGraveClass10Align = "EvangelistAgentoftheGraveClass10Align";
        private static readonly string EvangelistAgentoftheGraveClass10AlignGuid = "b4eff528-d1b2-49df-ba32-8f9db6b4142e";
        private const string EvangelistAnchoriteofDawnClass0Align = "EvangelistAnchoriteofDawnClass0Align";
        private static readonly string EvangelistAnchoriteofDawnClass0AlignGuid = "d5c643d2-6463-4b6e-aea8-7143df963f1d";
        internal const string EvangelistAnchoriteofDawnClass0AlignDisplayName = "EvangelistAnchoriteofDawnClass0Align.Name";
        private const string EvangelistAnchoriteofDawnClass0AlignDescription = "EvangelistAnchoriteofDawnClass0Align.Description";
        private const string EvangelistAnchoriteofDawnClass2Align = "EvangelistAnchoriteofDawnClass2Align";
        private static readonly string EvangelistAnchoriteofDawnClass2AlignGuid = "040adce5-a8c3-4e8c-b34c-f77929690b2e";
        private const string EvangelistAnchoriteofDawnClass3Align = "EvangelistAnchoriteofDawnClass3Align";
        private static readonly string EvangelistAnchoriteofDawnClass3AlignGuid = "8d290aec-c35f-40f3-965c-7fe083de4c3c";
        private const string EvangelistAnchoriteofDawnClass4Align = "EvangelistAnchoriteofDawnClass4Align";
        private static readonly string EvangelistAnchoriteofDawnClass4AlignGuid = "56d0ed01-04bf-4cc2-9fb2-42555a3f0dd5";
        private const string EvangelistAnchoriteofDawnClass5Align = "EvangelistAnchoriteofDawnClass5Align";
        private static readonly string EvangelistAnchoriteofDawnClass5AlignGuid = "4d49b68e-be0e-4cd4-939f-3b67704a480b";
        private const string EvangelistAnchoriteofDawnClass6Align = "EvangelistAnchoriteofDawnClass6Align";
        private static readonly string EvangelistAnchoriteofDawnClass6AlignGuid = "0e48d1a3-ab74-4897-90b3-96d2461c8387";
        private const string EvangelistAnchoriteofDawnClass7Align = "EvangelistAnchoriteofDawnClass7Align";
        private static readonly string EvangelistAnchoriteofDawnClass7AlignGuid = "734962f2-5b8e-4372-bfd6-7204bf181732";
        private const string EvangelistAnchoriteofDawnClass8Align = "EvangelistAnchoriteofDawnClass8Align";
        private static readonly string EvangelistAnchoriteofDawnClass8AlignGuid = "cdcfda44-1feb-448c-84d7-9ca999128328";
        private const string EvangelistAnchoriteofDawnClass9Align = "EvangelistAnchoriteofDawnClass9Align";
        private static readonly string EvangelistAnchoriteofDawnClass9AlignGuid = "38dcdfa3-0049-4123-9765-de812263fa5d";
        private const string EvangelistAnchoriteofDawnClass10Align = "EvangelistAnchoriteofDawnClass10Align";
        private static readonly string EvangelistAnchoriteofDawnClass10AlignGuid = "0e07fd77-c704-4fc4-9b30-0221a6dbbba5";
        private const string EvangelistArcaneAcherClass0Align = "EvangelistArcaneAcherClass0Align";
        private static readonly string EvangelistArcaneAcherClass0AlignGuid = "0e423f7a-5d44-41fc-acdb-2411afc7a4ca";
        internal const string EvangelistArcaneAcherClass0AlignDisplayName = "EvangelistArcaneAcherClass0Align.Name";
        private const string EvangelistArcaneAcherClass0AlignDescription = "EvangelistArcaneAcherClass0Align.Description";
        private const string EvangelistArcaneAcherClass2Align = "EvangelistArcaneAcherClass2Align";
        private static readonly string EvangelistArcaneAcherClass2AlignGuid = "440b549b-4276-44b8-8c71-5aa9de24111d";
        private const string EvangelistArcaneAcherClass3Align = "EvangelistArcaneAcherClass3Align";
        private static readonly string EvangelistArcaneAcherClass3AlignGuid = "d2db92fd-10f9-4a32-a844-75afeb979e68";
        private const string EvangelistArcaneAcherClass4Align = "EvangelistArcaneAcherClass4Align";
        private static readonly string EvangelistArcaneAcherClass4AlignGuid = "d603aeab-4e56-47dd-9aae-97f9be9e8b3f";
        private const string EvangelistArcaneAcherClass5Align = "EvangelistArcaneAcherClass5Align";
        private static readonly string EvangelistArcaneAcherClass5AlignGuid = "8639d1e5-0b24-4556-9fac-4a69ce1bd390";
        private const string EvangelistArcaneAcherClass6Align = "EvangelistArcaneAcherClass6Align";
        private static readonly string EvangelistArcaneAcherClass6AlignGuid = "b838c68e-c30d-45e7-b84d-aa9fb49eab88";
        private const string EvangelistArcaneAcherClass7Align = "EvangelistArcaneAcherClass7Align";
        private static readonly string EvangelistArcaneAcherClass7AlignGuid = "1c930689-9ec2-449d-9006-66a848aa946e";
        private const string EvangelistArcaneAcherClass8Align = "EvangelistArcaneAcherClass8Align";
        private static readonly string EvangelistArcaneAcherClass8AlignGuid = "1ea96546-d00a-4b48-abab-be782e312891";
        private const string EvangelistArcaneAcherClass9Align = "EvangelistArcaneAcherClass9Align";
        private static readonly string EvangelistArcaneAcherClass9AlignGuid = "7d71eb04-1d27-4984-92a1-592858931210";
        private const string EvangelistArcaneAcherClass10Align = "EvangelistArcaneAcherClass10Align";
        private static readonly string EvangelistArcaneAcherClass10AlignGuid = "c0349df3-bd73-47a5-a07b-35b02878f863";
        private const string EvangelistAsavirClass0Align = "EvangelistAsavirClass0Align";
        private static readonly string EvangelistAsavirClass0AlignGuid = "1aaf34f5-7107-4b58-8d0f-79518478d9cc";
        internal const string EvangelistAsavirClass0AlignDisplayName = "EvangelistAsavirClass0Align.Name";
        private const string EvangelistAsavirClass0AlignDescription = "EvangelistAsavirClass0Align.Description";
        private const string EvangelistAsavirClass2Align = "EvangelistAsavirClass2Align";
        private static readonly string EvangelistAsavirClass2AlignGuid = "a981eb6d-3066-460d-bbad-33fd105b3bfa";
        private const string EvangelistAsavirClass3Align = "EvangelistAsavirClass3Align";
        private static readonly string EvangelistAsavirClass3AlignGuid = "90b922fe-4697-47e9-9356-e25bdacfc277";
        private const string EvangelistAsavirClass4Align = "EvangelistAsavirClass4Align";
        private static readonly string EvangelistAsavirClass4AlignGuid = "c5083af7-6a99-49eb-990e-e885891a19b3";
        private const string EvangelistAsavirClass5Align = "EvangelistAsavirClass5Align";
        private static readonly string EvangelistAsavirClass5AlignGuid = "41301db8-de4d-4fff-ad70-afd739a626f5";
        private const string EvangelistAsavirClass6Align = "EvangelistAsavirClass6Align";
        private static readonly string EvangelistAsavirClass6AlignGuid = "118e455a-e130-425f-9d4d-fa9ed2b25437";
        private const string EvangelistAsavirClass7Align = "EvangelistAsavirClass7Align";
        private static readonly string EvangelistAsavirClass7AlignGuid = "a954c764-1e3d-47bf-962f-afb27e8f2310";
        private const string EvangelistAsavirClass8Align = "EvangelistAsavirClass8Align";
        private static readonly string EvangelistAsavirClass8AlignGuid = "63d9e9c3-e2cd-4691-a8c2-60d22ecee664";
        private const string EvangelistAsavirClass9Align = "EvangelistAsavirClass9Align";
        private static readonly string EvangelistAsavirClass9AlignGuid = "074a9d68-ad86-481b-bbbe-c305559cb630";
        private const string EvangelistAsavirClass10Align = "EvangelistAsavirClass10Align";
        private static readonly string EvangelistAsavirClass10AlignGuid = "f99f29e3-e380-4c51-afa3-ed54416e52e4";
        private const string EvangelistChevalierClass0Align = "EvangelistChevalierClass0Align";
        private static readonly string EvangelistChevalierClass0AlignGuid = "04d45317-0b56-4979-88a2-2c07a210a131";
        internal const string EvangelistChevalierClass0AlignDisplayName = "EvangelistChevalierClass0Align.Name";
        private const string EvangelistChevalierClass0AlignDescription = "EvangelistChevalierClass0Align.Description";
        private const string EvangelistChevalierClass2Align = "EvangelistChevalierClass2Align";
        private static readonly string EvangelistChevalierClass2AlignGuid = "ed2d499b-2a1a-49b5-8e8a-9fabf7c3fde3";
        private const string EvangelistChevalierClass3Align = "EvangelistChevalierClass3Align";
        private static readonly string EvangelistChevalierClass3AlignGuid = "3239d0b1-c9d8-451c-abb9-cf010b330f13";
        private const string EvangelistChevalierClass4Align = "EvangelistChevalierClass4Align";
        private static readonly string EvangelistChevalierClass4AlignGuid = "f02b0372-b85c-4f5e-a685-7b7a0d0826d9";
        private const string EvangelistChevalierClass5Align = "EvangelistChevalierClass5Align";
        private static readonly string EvangelistChevalierClass5AlignGuid = "884e30fc-ef23-416c-b34b-3ef0d41fd840";
        private const string EvangelistChevalierClass6Align = "EvangelistChevalierClass6Align";
        private static readonly string EvangelistChevalierClass6AlignGuid = "dcf42de2-f171-4225-97a4-65b396a94894";
        private const string EvangelistChevalierClass7Align = "EvangelistChevalierClass7Align";
        private static readonly string EvangelistChevalierClass7AlignGuid = "4ef9cb5e-eb04-4d35-87c3-ff6e45869d13";
        private const string EvangelistChevalierClass8Align = "EvangelistChevalierClass8Align";
        private static readonly string EvangelistChevalierClass8AlignGuid = "33abba56-8f05-4c75-8f3b-39684c0f0a85";
        private const string EvangelistChevalierClass9Align = "EvangelistChevalierClass9Align";
        private static readonly string EvangelistChevalierClass9AlignGuid = "22e571f8-771c-4408-8467-8ee3cb7e31c9";
        private const string EvangelistChevalierClass10Align = "EvangelistChevalierClass10Align";
        private static readonly string EvangelistChevalierClass10AlignGuid = "2c7b53e4-f75a-425c-8d87-19ce31f2ddfb";
        private const string EvangelistCrimsonTemplarClass0Align = "EvangelistCrimsonTemplarClass0Align";
        private static readonly string EvangelistCrimsonTemplarClass0AlignGuid = "d418eb2d-ab5a-4d2f-9818-49673955fd72";
        internal const string EvangelistCrimsonTemplarClass0AlignDisplayName = "EvangelistCrimsonTemplarClass0Align.Name";
        private const string EvangelistCrimsonTemplarClass0AlignDescription = "EvangelistCrimsonTemplarClass0Align.Description";
        private const string EvangelistCrimsonTemplarClass2Align = "EvangelistCrimsonTemplarClass2Align";
        private static readonly string EvangelistCrimsonTemplarClass2AlignGuid = "64b22429-4949-4a50-ac7d-4012ceb8a896";
        private const string EvangelistCrimsonTemplarClass3Align = "EvangelistCrimsonTemplarClass3Align";
        private static readonly string EvangelistCrimsonTemplarClass3AlignGuid = "ff09f530-e265-4297-9667-d67550d49991";
        private const string EvangelistCrimsonTemplarClass4Align = "EvangelistCrimsonTemplarClass4Align";
        private static readonly string EvangelistCrimsonTemplarClass4AlignGuid = "c48bd3ba-7d2e-409a-9706-20f91e520c5d";
        private const string EvangelistCrimsonTemplarClass5Align = "EvangelistCrimsonTemplarClass5Align";
        private static readonly string EvangelistCrimsonTemplarClass5AlignGuid = "5159ae34-9b6b-494d-9446-102d0da4049f";
        private const string EvangelistCrimsonTemplarClass6Align = "EvangelistCrimsonTemplarClass6Align";
        private static readonly string EvangelistCrimsonTemplarClass6AlignGuid = "69f7ceea-88e4-45eb-b0e0-7ea8785645a4";
        private const string EvangelistCrimsonTemplarClass7Align = "EvangelistCrimsonTemplarClass7Align";
        private static readonly string EvangelistCrimsonTemplarClass7AlignGuid = "e534bc26-88ca-44e4-958c-4aac2abf9770";
        private const string EvangelistCrimsonTemplarClass8Align = "EvangelistCrimsonTemplarClass8Align";
        private static readonly string EvangelistCrimsonTemplarClass8AlignGuid = "16d06b95-f032-41d4-a822-537c2f57df81";
        private const string EvangelistCrimsonTemplarClass9Align = "EvangelistCrimsonTemplarClass9Align";
        private static readonly string EvangelistCrimsonTemplarClass9AlignGuid = "1d77bf86-a3e2-4191-8e72-dc9295e62ece";
        private const string EvangelistCrimsonTemplarClass10Align = "EvangelistCrimsonTemplarClass10Align";
        private static readonly string EvangelistCrimsonTemplarClass10AlignGuid = "38410107-35ea-4506-a834-d6437906d21d";
        private const string EvangelistDeadeyeDevoteeClass0Align = "EvangelistDeadeyeDevoteeClass0Align";
        private static readonly string EvangelistDeadeyeDevoteeClass0AlignGuid = "1eb928b8-9a23-4676-9b89-ae68d9fe59d8";
        internal const string EvangelistDeadeyeDevoteeClass0AlignDisplayName = "EvangelistDeadeyeDevoteeClass0Align.Name";
        private const string EvangelistDeadeyeDevoteeClass0AlignDescription = "EvangelistDeadeyeDevoteeClass0Align.Description";
        private const string EvangelistDeadeyeDevoteeClass2Align = "EvangelistDeadeyeDevoteeClass2Align";
        private static readonly string EvangelistDeadeyeDevoteeClass2AlignGuid = "2d16fbc2-c3c8-4a71-9fcd-1c5379803c57";
        private const string EvangelistDeadeyeDevoteeClass3Align = "EvangelistDeadeyeDevoteeClass3Align";
        private static readonly string EvangelistDeadeyeDevoteeClass3AlignGuid = "8b0d5bcd-6b94-4c21-b081-98d013d3978f";
        private const string EvangelistDeadeyeDevoteeClass4Align = "EvangelistDeadeyeDevoteeClass4Align";
        private static readonly string EvangelistDeadeyeDevoteeClass4AlignGuid = "c145ad74-de1f-4f7d-906d-b71a07c7e2df";
        private const string EvangelistDeadeyeDevoteeClass5Align = "EvangelistDeadeyeDevoteeClass5Align";
        private static readonly string EvangelistDeadeyeDevoteeClass5AlignGuid = "e6b3e1a0-22b3-4c0e-a48d-e10823ead05b";
        private const string EvangelistDeadeyeDevoteeClass6Align = "EvangelistDeadeyeDevoteeClass6Align";
        private static readonly string EvangelistDeadeyeDevoteeClass6AlignGuid = "f5679e1e-fd45-41d6-90a7-ce0f1a267992";
        private const string EvangelistDeadeyeDevoteeClass7Align = "EvangelistDeadeyeDevoteeClass7Align";
        private static readonly string EvangelistDeadeyeDevoteeClass7AlignGuid = "a8c96f6d-943c-4cfe-aa00-caeb3dd44938";
        private const string EvangelistDeadeyeDevoteeClass8Align = "EvangelistDeadeyeDevoteeClass8Align";
        private static readonly string EvangelistDeadeyeDevoteeClass8AlignGuid = "a48996af-54bc-45bd-a5e2-e36c9ad4ceb6";
        private const string EvangelistDeadeyeDevoteeClass9Align = "EvangelistDeadeyeDevoteeClass9Align";
        private static readonly string EvangelistDeadeyeDevoteeClass9AlignGuid = "96314890-727d-4834-9b12-05cb7e469dc9";
        private const string EvangelistDeadeyeDevoteeClass10Align = "EvangelistDeadeyeDevoteeClass10Align";
        private static readonly string EvangelistDeadeyeDevoteeClass10AlignGuid = "e85dd71f-3f75-446e-9a32-85d8b977687d";
        private const string EvangelistDragonFuryClass0Align = "EvangelistDragonFuryClass0Align";
        private static readonly string EvangelistDragonFuryClass0AlignGuid = "f52fdc5d-5e1e-475e-9fc3-f47079d7642f";
        internal const string EvangelistDragonFuryClass0AlignDisplayName = "EvangelistDragonFuryClass0Align.Name";
        private const string EvangelistDragonFuryClass0AlignDescription = "EvangelistDragonFuryClass0Align.Description";
        private const string EvangelistDragonFuryClass2Align = "EvangelistDragonFuryClass2Align";
        private static readonly string EvangelistDragonFuryClass2AlignGuid = "4b18fb80-49c1-4a4d-b319-d277bed81cf7";
        private const string EvangelistDragonFuryClass3Align = "EvangelistDragonFuryClass3Align";
        private static readonly string EvangelistDragonFuryClass3AlignGuid = "84532743-a146-4e99-9778-6934b9cb4198";
        private const string EvangelistDragonFuryClass4Align = "EvangelistDragonFuryClass4Align";
        private static readonly string EvangelistDragonFuryClass4AlignGuid = "33678f93-edcc-4da5-9e0e-9eadd1cfa03c";
        private const string EvangelistDragonFuryClass5Align = "EvangelistDragonFuryClass5Align";
        private static readonly string EvangelistDragonFuryClass5AlignGuid = "e33e239b-f2f9-4fb5-bccc-d22cefe4c3bc";
        private const string EvangelistDragonFuryClass6Align = "EvangelistDragonFuryClass6Align";
        private static readonly string EvangelistDragonFuryClass6AlignGuid = "bbaec817-4e7a-45cf-b9f8-72f354bbda4b";
        private const string EvangelistDragonFuryClass7Align = "EvangelistDragonFuryClass7Align";
        private static readonly string EvangelistDragonFuryClass7AlignGuid = "b6c1fd55-a474-4ba4-8cb6-821a9428b831";
        private const string EvangelistDragonFuryClass8Align = "EvangelistDragonFuryClass8Align";
        private static readonly string EvangelistDragonFuryClass8AlignGuid = "fd446bf4-89a2-4ca1-83de-f533d0f3bc64";
        private const string EvangelistDragonFuryClass9Align = "EvangelistDragonFuryClass9Align";
        private static readonly string EvangelistDragonFuryClass9AlignGuid = "9cc5c34d-a1a0-44eb-b6ee-c18f293fe4b3";
        private const string EvangelistDragonFuryClass10Align = "EvangelistDragonFuryClass10Align";
        private static readonly string EvangelistDragonFuryClass10AlignGuid = "48d8cdb4-dded-4a1e-a079-11a44d466c0f";
        private const string EvangelistEsotericKnightClass0Align = "EvangelistEsotericKnightClass0Align";
        private static readonly string EvangelistEsotericKnightClass0AlignGuid = "eb353214-238b-4a34-b0dd-ee2c7e43537c";
        internal const string EvangelistEsotericKnightClass0AlignDisplayName = "EvangelistEsotericKnightClass0Align.Name";
        private const string EvangelistEsotericKnightClass0AlignDescription = "EvangelistEsotericKnightClass0Align.Description";
        private const string EvangelistEsotericKnightClass2Align = "EvangelistEsotericKnightClass2Align";
        private static readonly string EvangelistEsotericKnightClass2AlignGuid = "f15f59cd-ace4-4faf-84ed-7d10f2e1ad4c";
        private const string EvangelistEsotericKnightClass3Align = "EvangelistEsotericKnightClass3Align";
        private static readonly string EvangelistEsotericKnightClass3AlignGuid = "15bc9e0d-51b8-468d-8069-b66ff653110e";
        private const string EvangelistEsotericKnightClass4Align = "EvangelistEsotericKnightClass4Align";
        private static readonly string EvangelistEsotericKnightClass4AlignGuid = "0a80076c-164e-47f7-879c-4808b39d2aaf";
        private const string EvangelistEsotericKnightClass5Align = "EvangelistEsotericKnightClass5Align";
        private static readonly string EvangelistEsotericKnightClass5AlignGuid = "ab2cb9fa-c890-4863-981c-ba962797f6c7";
        private const string EvangelistEsotericKnightClass6Align = "EvangelistEsotericKnightClass6Align";
        private static readonly string EvangelistEsotericKnightClass6AlignGuid = "586b99d8-82b0-4d47-acad-01dc58215bbf";
        private const string EvangelistEsotericKnightClass7Align = "EvangelistEsotericKnightClass7Align";
        private static readonly string EvangelistEsotericKnightClass7AlignGuid = "7fbe4f77-fb91-45e9-aabf-00d0638502ab";
        private const string EvangelistEsotericKnightClass8Align = "EvangelistEsotericKnightClass8Align";
        private static readonly string EvangelistEsotericKnightClass8AlignGuid = "54e51291-8638-46c7-9e11-33d6b038a694";
        private const string EvangelistEsotericKnightClass9Align = "EvangelistEsotericKnightClass9Align";
        private static readonly string EvangelistEsotericKnightClass9AlignGuid = "3227db50-9f24-493c-bc81-11b489d65d86";
        private const string EvangelistEsotericKnightClass10Align = "EvangelistEsotericKnightClass10Align";
        private static readonly string EvangelistEsotericKnightClass10AlignGuid = "e33b25fb-069b-454c-9128-0788d08a647c";
        private const string EvangelistExaltedEvangelistClass0Align = "EvangelistExaltedEvangelistClass0Align";
        private static readonly string EvangelistExaltedEvangelistClass0AlignGuid = "aab2cd75-c997-4900-8f9c-bc9ca64e765f";
        internal const string EvangelistExaltedEvangelistClass0AlignDisplayName = "EvangelistExaltedEvangelistClass0Align.Name";
        private const string EvangelistExaltedEvangelistClass0AlignDescription = "EvangelistExaltedEvangelistClass0Align.Description";
        private const string EvangelistExaltedEvangelistClass2Align = "EvangelistExaltedEvangelistClass2Align";
        private static readonly string EvangelistExaltedEvangelistClass2AlignGuid = "9921fd1a-ccd6-4c28-a095-ddbd916fe934";
        private const string EvangelistExaltedEvangelistClass3Align = "EvangelistExaltedEvangelistClass3Align";
        private static readonly string EvangelistExaltedEvangelistClass3AlignGuid = "07155ba2-37bd-4efb-8010-e87559ecc16a";
        private const string EvangelistExaltedEvangelistClass4Align = "EvangelistExaltedEvangelistClass4Align";
        private static readonly string EvangelistExaltedEvangelistClass4AlignGuid = "186ada64-16c5-437e-8aff-68161fdfab10";
        private const string EvangelistExaltedEvangelistClass5Align = "EvangelistExaltedEvangelistClass5Align";
        private static readonly string EvangelistExaltedEvangelistClass5AlignGuid = "d5bf1ad1-e943-49d6-8d99-4c4884d2f584";
        private const string EvangelistExaltedEvangelistClass6Align = "EvangelistExaltedEvangelistClass6Align";
        private static readonly string EvangelistExaltedEvangelistClass6AlignGuid = "5c07f393-bf79-4069-94ff-410dc28ba63d";
        private const string EvangelistExaltedEvangelistClass7Align = "EvangelistExaltedEvangelistClass7Align";
        private static readonly string EvangelistExaltedEvangelistClass7AlignGuid = "e02805bc-0119-4b07-9d4b-72852a4f4e87";
        private const string EvangelistExaltedEvangelistClass8Align = "EvangelistExaltedEvangelistClass8Align";
        private static readonly string EvangelistExaltedEvangelistClass8AlignGuid = "abc706dd-abe4-4059-b67b-906549507c31";
        private const string EvangelistExaltedEvangelistClass9Align = "EvangelistExaltedEvangelistClass9Align";
        private static readonly string EvangelistExaltedEvangelistClass9AlignGuid = "00c965a5-7c32-47fd-ba60-11e6561455cd";
        private const string EvangelistExaltedEvangelistClass10Align = "EvangelistExaltedEvangelistClass10Align";
        private static readonly string EvangelistExaltedEvangelistClass10AlignGuid = "e67e55e5-b653-416e-966e-a772aa634bf6";
        private const string EvangelistFuriousGuardianClass0Align = "EvangelistFuriousGuardianClass0Align";
        private static readonly string EvangelistFuriousGuardianClass0AlignGuid = "cec0137f-9430-4277-8b47-378d0522a953";
        internal const string EvangelistFuriousGuardianClass0AlignDisplayName = "EvangelistFuriousGuardianClass0Align.Name";
        private const string EvangelistFuriousGuardianClass0AlignDescription = "EvangelistFuriousGuardianClass0Align.Description";
        private const string EvangelistFuriousGuardianClass2Align = "EvangelistFuriousGuardianClass2Align";
        private static readonly string EvangelistFuriousGuardianClass2AlignGuid = "4eb88a5a-f69c-4c0e-934c-52a432bf5eb0";
        private const string EvangelistFuriousGuardianClass3Align = "EvangelistFuriousGuardianClass3Align";
        private static readonly string EvangelistFuriousGuardianClass3AlignGuid = "27749fb2-d210-4fcd-90eb-cf6570f35556";
        private const string EvangelistFuriousGuardianClass4Align = "EvangelistFuriousGuardianClass4Align";
        private static readonly string EvangelistFuriousGuardianClass4AlignGuid = "04057605-cfb8-4772-86ae-1f163a2c7e96";
        private const string EvangelistFuriousGuardianClass5Align = "EvangelistFuriousGuardianClass5Align";
        private static readonly string EvangelistFuriousGuardianClass5AlignGuid = "1f31bd04-47f3-4a50-8596-654cc540c29c";
        private const string EvangelistFuriousGuardianClass6Align = "EvangelistFuriousGuardianClass6Align";
        private static readonly string EvangelistFuriousGuardianClass6AlignGuid = "72f09bd2-2e24-4291-91e2-7eb30bb46210";
        private const string EvangelistFuriousGuardianClass7Align = "EvangelistFuriousGuardianClass7Align";
        private static readonly string EvangelistFuriousGuardianClass7AlignGuid = "808d5d3e-6afb-47ff-9652-df3171a1c8ef";
        private const string EvangelistFuriousGuardianClass8Align = "EvangelistFuriousGuardianClass8Align";
        private static readonly string EvangelistFuriousGuardianClass8AlignGuid = "c0e43863-ac16-48c4-8a31-4a67e50d4376";
        private const string EvangelistFuriousGuardianClass9Align = "EvangelistFuriousGuardianClass9Align";
        private static readonly string EvangelistFuriousGuardianClass9AlignGuid = "eeb2999f-891c-4510-a653-c110a2705181";
        private const string EvangelistFuriousGuardianClass10Align = "EvangelistFuriousGuardianClass10Align";
        private static readonly string EvangelistFuriousGuardianClass10AlignGuid = "6bcb6da1-52af-43db-9fa2-b41dea6df785";
        private const string EvangelistHalflingOpportunistClass0Align = "EvangelistHalflingOpportunistClass0Align";
        private static readonly string EvangelistHalflingOpportunistClass0AlignGuid = "98328dc1-dd6e-4eca-b090-22df8b72892c";
        internal const string EvangelistHalflingOpportunistClass0AlignDisplayName = "EvangelistHalflingOpportunistClass0Align.Name";
        private const string EvangelistHalflingOpportunistClass0AlignDescription = "EvangelistHalflingOpportunistClass0Align.Description";
        private const string EvangelistHalflingOpportunistClass2Align = "EvangelistHalflingOpportunistClass2Align";
        private static readonly string EvangelistHalflingOpportunistClass2AlignGuid = "9c7acf29-9ed4-4a7a-b70a-8a7a15bf68f4";
        private const string EvangelistHalflingOpportunistClass3Align = "EvangelistHalflingOpportunistClass3Align";
        private static readonly string EvangelistHalflingOpportunistClass3AlignGuid = "bbf77caf-a2b7-4a25-876c-be78775dd599";
        private const string EvangelistHalflingOpportunistClass4Align = "EvangelistHalflingOpportunistClass4Align";
        private static readonly string EvangelistHalflingOpportunistClass4AlignGuid = "fd23a8dc-f74f-46c5-8788-b81d6e63ee48";
        private const string EvangelistHalflingOpportunistClass5Align = "EvangelistHalflingOpportunistClass5Align";
        private static readonly string EvangelistHalflingOpportunistClass5AlignGuid = "cd1e4e16-8266-401d-8b00-34032d444162";
        private const string EvangelistHalflingOpportunistClass6Align = "EvangelistHalflingOpportunistClass6Align";
        private static readonly string EvangelistHalflingOpportunistClass6AlignGuid = "eb56b671-7781-47db-b53b-e89d9593b742";
        private const string EvangelistHalflingOpportunistClass7Align = "EvangelistHalflingOpportunistClass7Align";
        private static readonly string EvangelistHalflingOpportunistClass7AlignGuid = "162ffbd6-a0ad-4ec2-ab6b-3044054e4aa8";
        private const string EvangelistHalflingOpportunistClass8Align = "EvangelistHalflingOpportunistClass8Align";
        private static readonly string EvangelistHalflingOpportunistClass8AlignGuid = "01e25f00-aec0-4639-948f-411b7c3210c6";
        private const string EvangelistHalflingOpportunistClass9Align = "EvangelistHalflingOpportunistClass9Align";
        private static readonly string EvangelistHalflingOpportunistClass9AlignGuid = "1172d5e5-ff53-464e-9e07-e99d2a908fcf";
        private const string EvangelistHalflingOpportunistClass10Align = "EvangelistHalflingOpportunistClass10Align";
        private static readonly string EvangelistHalflingOpportunistClass10AlignGuid = "7d4897d2-b929-47da-ac03-f312f2ac8df8";
        private const string EvangelistHinterlanderClass0Align = "EvangelistHinterlanderClass0Align";
        private static readonly string EvangelistHinterlanderClass0AlignGuid = "36458766-3abc-4b92-baac-e837b9a4f10a";
        internal const string EvangelistHinterlanderClass0AlignDisplayName = "EvangelistHinterlanderClass0Align.Name";
        private const string EvangelistHinterlanderClass0AlignDescription = "EvangelistHinterlanderClass0Align.Description";
        private const string EvangelistHinterlanderClass2Align = "EvangelistHinterlanderClass2Align";
        private static readonly string EvangelistHinterlanderClass2AlignGuid = "6fd7ab7b-5b08-4f9d-afb7-4077e90d0604";
        private const string EvangelistHinterlanderClass3Align = "EvangelistHinterlanderClass3Align";
        private static readonly string EvangelistHinterlanderClass3AlignGuid = "88ddfea0-66fa-41f4-8259-e244bca383aa";
        private const string EvangelistHinterlanderClass4Align = "EvangelistHinterlanderClass4Align";
        private static readonly string EvangelistHinterlanderClass4AlignGuid = "75f8ca68-5970-4336-b92a-1c4f04eecef0";
        private const string EvangelistHinterlanderClass5Align = "EvangelistHinterlanderClass5Align";
        private static readonly string EvangelistHinterlanderClass5AlignGuid = "41319d6d-c85e-4d4e-8f81-839da68c6230";
        private const string EvangelistHinterlanderClass6Align = "EvangelistHinterlanderClass6Align";
        private static readonly string EvangelistHinterlanderClass6AlignGuid = "1924398d-9207-4ce7-8aec-707571474d7f";
        private const string EvangelistHinterlanderClass7Align = "EvangelistHinterlanderClass7Align";
        private static readonly string EvangelistHinterlanderClass7AlignGuid = "3f7ebb47-231e-4c2f-96fe-583a35587094";
        private const string EvangelistHinterlanderClass8Align = "EvangelistHinterlanderClass8Align";
        private static readonly string EvangelistHinterlanderClass8AlignGuid = "97c97ced-c6dd-4d17-ab8f-708c8f9b86b8";
        private const string EvangelistHinterlanderClass9Align = "EvangelistHinterlanderClass9Align";
        private static readonly string EvangelistHinterlanderClass9AlignGuid = "ed575e0d-3e56-4fef-9006-8ea65160a982";
        private const string EvangelistHinterlanderClass10Align = "EvangelistHinterlanderClass10Align";
        private static readonly string EvangelistHinterlanderClass10AlignGuid = "1da670f8-70b1-403c-bb28-48620a1a1003";
        private const string EvangelistHorizonWalkerClass0Align = "EvangelistHorizonWalkerClass0Align";
        private static readonly string EvangelistHorizonWalkerClass0AlignGuid = "44900d43-c28b-4338-a511-74680cc2ee9a";
        internal const string EvangelistHorizonWalkerClass0AlignDisplayName = "EvangelistHorizonWalkerClass0Align.Name";
        private const string EvangelistHorizonWalkerClass0AlignDescription = "EvangelistHorizonWalkerClass0Align.Description";
        private const string EvangelistHorizonWalkerClass2Align = "EvangelistHorizonWalkerClass2Align";
        private static readonly string EvangelistHorizonWalkerClass2AlignGuid = "44b10cf3-daaf-4d50-8234-a412559b6762";
        private const string EvangelistHorizonWalkerClass3Align = "EvangelistHorizonWalkerClass3Align";
        private static readonly string EvangelistHorizonWalkerClass3AlignGuid = "d449e31f-c934-4bc8-ac64-27c59b4af958";
        private const string EvangelistHorizonWalkerClass4Align = "EvangelistHorizonWalkerClass4Align";
        private static readonly string EvangelistHorizonWalkerClass4AlignGuid = "2a1c9f00-f022-4cfe-87e9-dcd10407036b";
        private const string EvangelistHorizonWalkerClass5Align = "EvangelistHorizonWalkerClass5Align";
        private static readonly string EvangelistHorizonWalkerClass5AlignGuid = "0d92b6f0-b34d-4faf-b5c0-10aa2f3a2917";
        private const string EvangelistHorizonWalkerClass6Align = "EvangelistHorizonWalkerClass6Align";
        private static readonly string EvangelistHorizonWalkerClass6AlignGuid = "089fc18d-42ca-4b8f-bcab-fd2737e12f1f";
        private const string EvangelistHorizonWalkerClass7Align = "EvangelistHorizonWalkerClass7Align";
        private static readonly string EvangelistHorizonWalkerClass7AlignGuid = "aa7daab6-e2cd-43b1-806a-5d8bf6b414a8";
        private const string EvangelistHorizonWalkerClass8Align = "EvangelistHorizonWalkerClass8Align";
        private static readonly string EvangelistHorizonWalkerClass8AlignGuid = "c1af6d6f-c522-4f8b-8932-0f56b2c870dd";
        private const string EvangelistHorizonWalkerClass9Align = "EvangelistHorizonWalkerClass9Align";
        private static readonly string EvangelistHorizonWalkerClass9AlignGuid = "6aa1b0a3-fb01-486e-be06-bec45d2cded7";
        private const string EvangelistHorizonWalkerClass10Align = "EvangelistHorizonWalkerClass10Align";
        private static readonly string EvangelistHorizonWalkerClass10AlignGuid = "8eb8b316-e203-44b0-8c84-fa62d47a5c2f";
        private const string EvangelistInheritorCrusaderClass0Align = "EvangelistInheritorCrusaderClass0Align";
        private static readonly string EvangelistInheritorCrusaderClass0AlignGuid = "c58c3364-6570-4884-bf38-eeee1445b210";
        internal const string EvangelistInheritorCrusaderClass0AlignDisplayName = "EvangelistInheritorCrusaderClass0Align.Name";
        private const string EvangelistInheritorCrusaderClass0AlignDescription = "EvangelistInheritorCrusaderClass0Align.Description";
        private const string EvangelistInheritorCrusaderClass2Align = "EvangelistInheritorCrusaderClass2Align";
        private static readonly string EvangelistInheritorCrusaderClass2AlignGuid = "9edbcf6e-d8b5-4387-b8a7-cadafc8d0bb8";
        private const string EvangelistInheritorCrusaderClass3Align = "EvangelistInheritorCrusaderClass3Align";
        private static readonly string EvangelistInheritorCrusaderClass3AlignGuid = "c333eb32-186a-4bdd-8cb5-41de4d099a54";
        private const string EvangelistInheritorCrusaderClass4Align = "EvangelistInheritorCrusaderClass4Align";
        private static readonly string EvangelistInheritorCrusaderClass4AlignGuid = "f01c9b38-7214-4bd6-ad04-976a71e56b36";
        private const string EvangelistInheritorCrusaderClass5Align = "EvangelistInheritorCrusaderClass5Align";
        private static readonly string EvangelistInheritorCrusaderClass5AlignGuid = "d07468f3-9049-45af-ab80-6a6b7149cdf9";
        private const string EvangelistInheritorCrusaderClass6Align = "EvangelistInheritorCrusaderClass6Align";
        private static readonly string EvangelistInheritorCrusaderClass6AlignGuid = "79201f90-d607-4a81-bb6c-b48a106abdcf";
        private const string EvangelistInheritorCrusaderClass7Align = "EvangelistInheritorCrusaderClass7Align";
        private static readonly string EvangelistInheritorCrusaderClass7AlignGuid = "a6d0afad-412e-45a4-8caa-c71b95813f03";
        private const string EvangelistInheritorCrusaderClass8Align = "EvangelistInheritorCrusaderClass8Align";
        private static readonly string EvangelistInheritorCrusaderClass8AlignGuid = "aea4322d-95d5-4ebc-bfbe-cb394c1bb599";
        private const string EvangelistInheritorCrusaderClass9Align = "EvangelistInheritorCrusaderClass9Align";
        private static readonly string EvangelistInheritorCrusaderClass9AlignGuid = "45e942c5-0f04-4203-8e52-42330944768d";
        private const string EvangelistInheritorCrusaderClass10Align = "EvangelistInheritorCrusaderClass10Align";
        private static readonly string EvangelistInheritorCrusaderClass10AlignGuid = "e9f9834a-1f3b-4d0f-8539-4e212e7c72c4";
        private const string EvangelistMammothRiderClass0Align = "EvangelistMammothRiderClass0Align";
        private static readonly string EvangelistMammothRiderClass0AlignGuid = "ebb0dcce-7cae-41d8-8f0b-0fc75dfe6050";
        internal const string EvangelistMammothRiderClass0AlignDisplayName = "EvangelistMammothRiderClass0Align.Name";
        private const string EvangelistMammothRiderClass0AlignDescription = "EvangelistMammothRiderClass0Align.Description";
        private const string EvangelistMammothRiderClass2Align = "EvangelistMammothRiderClass2Align";
        private static readonly string EvangelistMammothRiderClass2AlignGuid = "a44980f9-b3f4-4909-8592-53f2bf3fd641";
        private const string EvangelistMammothRiderClass3Align = "EvangelistMammothRiderClass3Align";
        private static readonly string EvangelistMammothRiderClass3AlignGuid = "fd9088a7-3232-4f3b-a848-81c81ee0c1d1";
        private const string EvangelistMammothRiderClass4Align = "EvangelistMammothRiderClass4Align";
        private static readonly string EvangelistMammothRiderClass4AlignGuid = "ca693e7a-3b60-416b-b1a3-20ff7c1508f7";
        private const string EvangelistMammothRiderClass5Align = "EvangelistMammothRiderClass5Align";
        private static readonly string EvangelistMammothRiderClass5AlignGuid = "ea91c876-ffea-4982-9651-228c3245710e";
        private const string EvangelistMammothRiderClass6Align = "EvangelistMammothRiderClass6Align";
        private static readonly string EvangelistMammothRiderClass6AlignGuid = "11c1d7ee-da68-4ef3-b024-a9e3b1de289c";
        private const string EvangelistMammothRiderClass7Align = "EvangelistMammothRiderClass7Align";
        private static readonly string EvangelistMammothRiderClass7AlignGuid = "8f7bd2f7-4027-4242-9f32-f2e073d52a89";
        private const string EvangelistMammothRiderClass8Align = "EvangelistMammothRiderClass8Align";
        private static readonly string EvangelistMammothRiderClass8AlignGuid = "ab9e75ae-bfa5-4c36-b9da-bce19e06c524";
        private const string EvangelistMammothRiderClass9Align = "EvangelistMammothRiderClass9Align";
        private static readonly string EvangelistMammothRiderClass9AlignGuid = "f948fad2-590a-4118-8410-9cfda2b6b73a";
        private const string EvangelistMammothRiderClass10Align = "EvangelistMammothRiderClass10Align";
        private static readonly string EvangelistMammothRiderClass10AlignGuid = "acaf177b-3104-4fb1-816a-4fd7f90b02df";
        private const string EvangelistSanguineAngelClass0Align = "EvangelistSanguineAngelClass0Align";
        private static readonly string EvangelistSanguineAngelClass0AlignGuid = "3ac5aed9-88e8-4340-9f91-5c05f87d1d4b";
        internal const string EvangelistSanguineAngelClass0AlignDisplayName = "EvangelistSanguineAngelClass0Align.Name";
        private const string EvangelistSanguineAngelClass0AlignDescription = "EvangelistSanguineAngelClass0Align.Description";
        private const string EvangelistSanguineAngelClass2Align = "EvangelistSanguineAngelClass2Align";
        private static readonly string EvangelistSanguineAngelClass2AlignGuid = "59e7ee39-5cf5-4895-b971-1f41c6ed7b4d";
        private const string EvangelistSanguineAngelClass3Align = "EvangelistSanguineAngelClass3Align";
        private static readonly string EvangelistSanguineAngelClass3AlignGuid = "a6a98ed0-5c2a-4bdc-9774-4711bc230773";
        private const string EvangelistSanguineAngelClass4Align = "EvangelistSanguineAngelClass4Align";
        private static readonly string EvangelistSanguineAngelClass4AlignGuid = "e22c8d05-85a1-4146-b4ed-2018366fb9bb";
        private const string EvangelistSanguineAngelClass5Align = "EvangelistSanguineAngelClass5Align";
        private static readonly string EvangelistSanguineAngelClass5AlignGuid = "b2789989-2d06-45ff-b4ef-a7be77b9a7db";
        private const string EvangelistSanguineAngelClass6Align = "EvangelistSanguineAngelClass6Align";
        private static readonly string EvangelistSanguineAngelClass6AlignGuid = "223186f7-2ceb-40fe-b3ed-c334920d8df4";
        private const string EvangelistSanguineAngelClass7Align = "EvangelistSanguineAngelClass7Align";
        private static readonly string EvangelistSanguineAngelClass7AlignGuid = "0e0994fc-036a-4e03-b1a1-8febd930b858";
        private const string EvangelistSanguineAngelClass8Align = "EvangelistSanguineAngelClass8Align";
        private static readonly string EvangelistSanguineAngelClass8AlignGuid = "077a3301-db2c-412b-8d67-9e8bb02f7271";
        private const string EvangelistSanguineAngelClass9Align = "EvangelistSanguineAngelClass9Align";
        private static readonly string EvangelistSanguineAngelClass9AlignGuid = "c18a190c-040f-41a0-af3b-b178258231b0";
        private const string EvangelistSanguineAngelClass10Align = "EvangelistSanguineAngelClass10Align";
        private static readonly string EvangelistSanguineAngelClass10AlignGuid = "3820dbb5-c2af-4650-a8b1-563a9c056297";
        private const string EvangelistScarSeekerClass0Align = "EvangelistScarSeekerClass0Align";
        private static readonly string EvangelistScarSeekerClass0AlignGuid = "a41f93df-3864-4933-9c49-288fe3012e93";
        internal const string EvangelistScarSeekerClass0AlignDisplayName = "EvangelistScarSeekerClass0Align.Name";
        private const string EvangelistScarSeekerClass0AlignDescription = "EvangelistScarSeekerClass0Align.Description";
        private const string EvangelistScarSeekerClass2Align = "EvangelistScarSeekerClass2Align";
        private static readonly string EvangelistScarSeekerClass2AlignGuid = "5b73eeb5-3f01-4f5a-a1e6-18bb02a203f8";
        private const string EvangelistScarSeekerClass3Align = "EvangelistScarSeekerClass3Align";
        private static readonly string EvangelistScarSeekerClass3AlignGuid = "51552099-4266-46dd-b302-2a41f122d8ce";
        private const string EvangelistScarSeekerClass4Align = "EvangelistScarSeekerClass4Align";
        private static readonly string EvangelistScarSeekerClass4AlignGuid = "9e934d5a-ce43-405c-a193-83c226d4e91a";
        private const string EvangelistScarSeekerClass5Align = "EvangelistScarSeekerClass5Align";
        private static readonly string EvangelistScarSeekerClass5AlignGuid = "6315bfae-e16b-40ab-acc7-9c3de5056e30";
        private const string EvangelistScarSeekerClass6Align = "EvangelistScarSeekerClass6Align";
        private static readonly string EvangelistScarSeekerClass6AlignGuid = "838e62d6-de22-4ee7-a75c-ff59a0eccb1f";
        private const string EvangelistScarSeekerClass7Align = "EvangelistScarSeekerClass7Align";
        private static readonly string EvangelistScarSeekerClass7AlignGuid = "7714663f-df28-40fb-8bc6-a68a27fed564";
        private const string EvangelistScarSeekerClass8Align = "EvangelistScarSeekerClass8Align";
        private static readonly string EvangelistScarSeekerClass8AlignGuid = "fec74fe4-6d03-40a4-bfb3-05e971a3c6c2";
        private const string EvangelistScarSeekerClass9Align = "EvangelistScarSeekerClass9Align";
        private static readonly string EvangelistScarSeekerClass9AlignGuid = "796ed65b-21f5-457d-8d28-2593104c55b9";
        private const string EvangelistScarSeekerClass10Align = "EvangelistScarSeekerClass10Align";
        private static readonly string EvangelistScarSeekerClass10AlignGuid = "9336f443-347a-4525-8d2f-f17243ee8f63";
        private const string EvangelistSentinelClass0Align = "EvangelistSentinelClass0Align";
        private static readonly string EvangelistSentinelClass0AlignGuid = "ed167447-cc28-40b4-a589-59e8e965eb95";
        internal const string EvangelistSentinelClass0AlignDisplayName = "EvangelistSentinelClass0Align.Name";
        private const string EvangelistSentinelClass0AlignDescription = "EvangelistSentinelClass0Align.Description";
        private const string EvangelistSentinelClass2Align = "EvangelistSentinelClass2Align";
        private static readonly string EvangelistSentinelClass2AlignGuid = "138ba759-b45c-4b4f-a7fd-1e2aca54d8a7";
        private const string EvangelistSentinelClass3Align = "EvangelistSentinelClass3Align";
        private static readonly string EvangelistSentinelClass3AlignGuid = "15989777-be25-476a-84ba-23753dde23e9";
        private const string EvangelistSentinelClass4Align = "EvangelistSentinelClass4Align";
        private static readonly string EvangelistSentinelClass4AlignGuid = "19265a0f-629d-447b-9304-21e555a795c8";
        private const string EvangelistSentinelClass5Align = "EvangelistSentinelClass5Align";
        private static readonly string EvangelistSentinelClass5AlignGuid = "4ab11b9d-91b1-4921-8a48-4310cc5e7858";
        private const string EvangelistSentinelClass6Align = "EvangelistSentinelClass6Align";
        private static readonly string EvangelistSentinelClass6AlignGuid = "fcba8a95-3b59-409a-b42e-eba5d7bf7ac1";
        private const string EvangelistSentinelClass7Align = "EvangelistSentinelClass7Align";
        private static readonly string EvangelistSentinelClass7AlignGuid = "f0936c4b-42e3-4263-890d-c8235d1df6e7";
        private const string EvangelistSentinelClass8Align = "EvangelistSentinelClass8Align";
        private static readonly string EvangelistSentinelClass8AlignGuid = "d1798d6e-6041-418c-8074-12275596add9";
        private const string EvangelistSentinelClass9Align = "EvangelistSentinelClass9Align";
        private static readonly string EvangelistSentinelClass9AlignGuid = "215dc572-7a24-4b16-b06c-280b7a4d8630";
        private const string EvangelistSentinelClass10Align = "EvangelistSentinelClass10Align";
        private static readonly string EvangelistSentinelClass10AlignGuid = "e1e95251-e18c-429f-ac99-aea452ecc4cc";
        private const string EvangelistShadowDancerClass0Align = "EvangelistShadowDancerClass0Align";
        private static readonly string EvangelistShadowDancerClass0AlignGuid = "37498c3e-1565-44f7-9b75-e1ae0eb9e880";
        internal const string EvangelistShadowDancerClass0AlignDisplayName = "EvangelistShadowDancerClass0Align.Name";
        private const string EvangelistShadowDancerClass0AlignDescription = "EvangelistShadowDancerClass0Align.Description";
        private const string EvangelistShadowDancerClass2Align = "EvangelistShadowDancerClass2Align";
        private static readonly string EvangelistShadowDancerClass2AlignGuid = "66a9b8a0-d05f-43d2-8335-ba172c64f39d";
        private const string EvangelistShadowDancerClass3Align = "EvangelistShadowDancerClass3Align";
        private static readonly string EvangelistShadowDancerClass3AlignGuid = "33248687-6c8f-42c8-a52c-a60cbb2b9e8a";
        private const string EvangelistShadowDancerClass4Align = "EvangelistShadowDancerClass4Align";
        private static readonly string EvangelistShadowDancerClass4AlignGuid = "e7c6a6c1-f428-4aec-9f2b-f24ba2ba8fcc";
        private const string EvangelistShadowDancerClass5Align = "EvangelistShadowDancerClass5Align";
        private static readonly string EvangelistShadowDancerClass5AlignGuid = "e43cf9e0-3605-414f-ae20-4a364b9d838b";
        private const string EvangelistShadowDancerClass6Align = "EvangelistShadowDancerClass6Align";
        private static readonly string EvangelistShadowDancerClass6AlignGuid = "1e964e6d-b21e-4406-b907-9a105e4e0e0d";
        private const string EvangelistShadowDancerClass7Align = "EvangelistShadowDancerClass7Align";
        private static readonly string EvangelistShadowDancerClass7AlignGuid = "43d81b1b-bf70-498f-a63e-e12fe7fc4db2";
        private const string EvangelistShadowDancerClass8Align = "EvangelistShadowDancerClass8Align";
        private static readonly string EvangelistShadowDancerClass8AlignGuid = "ca8a57df-636a-4db7-8ccf-71a603d3b14a";
        private const string EvangelistShadowDancerClass9Align = "EvangelistShadowDancerClass9Align";
        private static readonly string EvangelistShadowDancerClass9AlignGuid = "ccabcdbb-a01a-49a5-92b9-c689dd671eb3";
        private const string EvangelistShadowDancerClass10Align = "EvangelistShadowDancerClass10Align";
        private static readonly string EvangelistShadowDancerClass10AlignGuid = "d28f7cff-49af-4c72-a1be-0bb34d8165b9";
        private const string EvangelistSouldrinkerClass0Align = "EvangelistSouldrinkerClass0Align";
        private static readonly string EvangelistSouldrinkerClass0AlignGuid = "5cfafad0-9366-4ef8-ba95-8cfd708410e9";
        internal const string EvangelistSouldrinkerClass0AlignDisplayName = "EvangelistSouldrinkerClass0Align.Name";
        private const string EvangelistSouldrinkerClass0AlignDescription = "EvangelistSouldrinkerClass0Align.Description";
        private const string EvangelistSouldrinkerClass2Align = "EvangelistSouldrinkerClass2Align";
        private static readonly string EvangelistSouldrinkerClass2AlignGuid = "e688d6d9-dc33-404f-9638-302fd96bb688";
        private const string EvangelistSouldrinkerClass3Align = "EvangelistSouldrinkerClass3Align";
        private static readonly string EvangelistSouldrinkerClass3AlignGuid = "879e3256-5745-4f33-a36f-86848b3bb538";
        private const string EvangelistSouldrinkerClass4Align = "EvangelistSouldrinkerClass4Align";
        private static readonly string EvangelistSouldrinkerClass4AlignGuid = "5ebfb32d-ab30-4a65-9223-3d71f3c121f5";
        private const string EvangelistSouldrinkerClass5Align = "EvangelistSouldrinkerClass5Align";
        private static readonly string EvangelistSouldrinkerClass5AlignGuid = "e80a1e9f-c482-4e3d-bfe9-7ac6c28909b7";
        private const string EvangelistSouldrinkerClass6Align = "EvangelistSouldrinkerClass6Align";
        private static readonly string EvangelistSouldrinkerClass6AlignGuid = "c099369e-ae5c-46e1-8b21-5f4d687e32ff";
        private const string EvangelistSouldrinkerClass7Align = "EvangelistSouldrinkerClass7Align";
        private static readonly string EvangelistSouldrinkerClass7AlignGuid = "001e2d25-1ac1-4305-a63c-d13e87a92ec7";
        private const string EvangelistSouldrinkerClass8Align = "EvangelistSouldrinkerClass8Align";
        private static readonly string EvangelistSouldrinkerClass8AlignGuid = "3b17b510-9b79-4ac2-85e7-d5db0063707e";
        private const string EvangelistSouldrinkerClass9Align = "EvangelistSouldrinkerClass9Align";
        private static readonly string EvangelistSouldrinkerClass9AlignGuid = "5d95cb28-f1d3-4b89-95c9-b775c34b84c3";
        private const string EvangelistSouldrinkerClass10Align = "EvangelistSouldrinkerClass10Align";
        private static readonly string EvangelistSouldrinkerClass10AlignGuid = "06680746-9d2d-40d6-9c4c-f4a4679beafe";
        private const string EvangelistUmbralAgentClass0Align = "EvangelistUmbralAgentClass0Align";
        private static readonly string EvangelistUmbralAgentClass0AlignGuid = "2ade17d1-12a8-4494-a089-92fd4fcf0bd8";
        internal const string EvangelistUmbralAgentClass0AlignDisplayName = "EvangelistUmbralAgentClass0Align.Name";
        private const string EvangelistUmbralAgentClass0AlignDescription = "EvangelistUmbralAgentClass0Align.Description";
        private const string EvangelistUmbralAgentClass2Align = "EvangelistUmbralAgentClass2Align";
        private static readonly string EvangelistUmbralAgentClass2AlignGuid = "b52e11da-c1f7-43b3-af1e-7d857b508c99";
        private const string EvangelistUmbralAgentClass3Align = "EvangelistUmbralAgentClass3Align";
        private static readonly string EvangelistUmbralAgentClass3AlignGuid = "cac03ee2-1f58-4624-ae95-556047b1129f";
        private const string EvangelistUmbralAgentClass4Align = "EvangelistUmbralAgentClass4Align";
        private static readonly string EvangelistUmbralAgentClass4AlignGuid = "3e80c33c-dcb2-46c6-a8ea-723cf70c7f32";
        private const string EvangelistUmbralAgentClass5Align = "EvangelistUmbralAgentClass5Align";
        private static readonly string EvangelistUmbralAgentClass5AlignGuid = "d18e55f5-6983-4d5f-ae68-8118377c28bc";
        private const string EvangelistUmbralAgentClass6Align = "EvangelistUmbralAgentClass6Align";
        private static readonly string EvangelistUmbralAgentClass6AlignGuid = "8ee20100-d689-4820-9e3d-d7e6cfc0642e";
        private const string EvangelistUmbralAgentClass7Align = "EvangelistUmbralAgentClass7Align";
        private static readonly string EvangelistUmbralAgentClass7AlignGuid = "66f5592c-28a5-4455-a006-69b80c649c81";
        private const string EvangelistUmbralAgentClass8Align = "EvangelistUmbralAgentClass8Align";
        private static readonly string EvangelistUmbralAgentClass8AlignGuid = "b4526ae0-09b8-4742-96f5-287336113154";
        private const string EvangelistUmbralAgentClass9Align = "EvangelistUmbralAgentClass9Align";
        private static readonly string EvangelistUmbralAgentClass9AlignGuid = "cae9eb03-3ea3-4f36-b5f2-9a7db1c282e1";
        private const string EvangelistUmbralAgentClass10Align = "EvangelistUmbralAgentClass10Align";
        private static readonly string EvangelistUmbralAgentClass10AlignGuid = "6a4e28c3-b5ec-49c3-84f1-bd73a06920c5";
        private const string EvangelistMicroAntiPaladinClass0Align = "EvangelistMicroAntiPaladinClass0Align";
        private static readonly string EvangelistMicroAntiPaladinClass0AlignGuid = "097c4fcb-d489-4072-ac1c-d20f97769547";
        internal const string EvangelistMicroAntiPaladinClass0AlignDisplayName = "EvangelistMicroAntiPaladinClass0Align.Name";
        private const string EvangelistMicroAntiPaladinClass0AlignDescription = "EvangelistMicroAntiPaladinClass0Align.Description";
        private const string EvangelistMicroAntiPaladinClass2Align = "EvangelistMicroAntiPaladinClass2Align";
        private static readonly string EvangelistMicroAntiPaladinClass2AlignGuid = "9c3118fd-4e2f-48db-a6a7-457dc996d002";
        private const string EvangelistMicroAntiPaladinClass3Align = "EvangelistMicroAntiPaladinClass3Align";
        private static readonly string EvangelistMicroAntiPaladinClass3AlignGuid = "9b2b6fe0-26ce-4c07-ad30-4ec6c9f55522";
        private const string EvangelistMicroAntiPaladinClass4Align = "EvangelistMicroAntiPaladinClass4Align";
        private static readonly string EvangelistMicroAntiPaladinClass4AlignGuid = "86d3af73-57d2-4b71-8f60-73c843aa5a3a";
        private const string EvangelistMicroAntiPaladinClass5Align = "EvangelistMicroAntiPaladinClass5Align";
        private static readonly string EvangelistMicroAntiPaladinClass5AlignGuid = "ef545261-97cd-42d5-ba69-b7b78da01e1f";
        private const string EvangelistMicroAntiPaladinClass6Align = "EvangelistMicroAntiPaladinClass6Align";
        private static readonly string EvangelistMicroAntiPaladinClass6AlignGuid = "caeab4c6-8e1e-4d6a-a782-9beee3aa7a3d";
        private const string EvangelistMicroAntiPaladinClass7Align = "EvangelistMicroAntiPaladinClass7Align";
        private static readonly string EvangelistMicroAntiPaladinClass7AlignGuid = "9b15c683-8224-4002-9290-f736eb808e75";
        private const string EvangelistMicroAntiPaladinClass8Align = "EvangelistMicroAntiPaladinClass8Align";
        private static readonly string EvangelistMicroAntiPaladinClass8AlignGuid = "bb431386-f2fd-4457-bc68-40231b9ac858";
        private const string EvangelistMicroAntiPaladinClass9Align = "EvangelistMicroAntiPaladinClass9Align";
        private static readonly string EvangelistMicroAntiPaladinClass9AlignGuid = "45a6773a-5baa-4869-86d1-3f4f8a93f1fa";
        private const string EvangelistMicroAntiPaladinClass10Align = "EvangelistMicroAntiPaladinClass10Align";
        private static readonly string EvangelistMicroAntiPaladinClass10AlignGuid = "e4aac75d-1877-4e47-8adf-3c4ca49d4e10";
        private const string EvangelistOathbreakerClass0Align = "EvangelistOathbreakerClass0Align";
        private static readonly string EvangelistOathbreakerClass0AlignGuid = "685b1671-f883-4044-ad6e-8edc30447141";
        internal const string EvangelistOathbreakerClass0AlignDisplayName = "EvangelistOathbreakerClass0Align.Name";
        private const string EvangelistOathbreakerClass0AlignDescription = "EvangelistOathbreakerClass0Align.Description";
        private const string EvangelistOathbreakerClass2Align = "EvangelistOathbreakerClass2Align";
        private static readonly string EvangelistOathbreakerClass2AlignGuid = "329e2e8d-9705-4500-9a4f-0c8717c400df";
        private const string EvangelistOathbreakerClass3Align = "EvangelistOathbreakerClass3Align";
        private static readonly string EvangelistOathbreakerClass3AlignGuid = "2810af81-77da-4d55-823c-61c907cb2d59";
        private const string EvangelistOathbreakerClass4Align = "EvangelistOathbreakerClass4Align";
        private static readonly string EvangelistOathbreakerClass4AlignGuid = "e8feb6ee-b70f-443c-a571-87504479b405";
        private const string EvangelistOathbreakerClass5Align = "EvangelistOathbreakerClass5Align";
        private static readonly string EvangelistOathbreakerClass5AlignGuid = "88a14c12-8f30-48a6-81d8-1d51d801e379";
        private const string EvangelistOathbreakerClass6Align = "EvangelistOathbreakerClass6Align";
        private static readonly string EvangelistOathbreakerClass6AlignGuid = "59204d38-4c10-4631-9c8c-401e0b62ade5";
        private const string EvangelistOathbreakerClass7Align = "EvangelistOathbreakerClass7Align";
        private static readonly string EvangelistOathbreakerClass7AlignGuid = "b6cf4031-e07a-46be-a750-c13a67d536bf";
        private const string EvangelistOathbreakerClass8Align = "EvangelistOathbreakerClass8Align";
        private static readonly string EvangelistOathbreakerClass8AlignGuid = "d1c27a14-3fd2-41c3-a8b6-33c944e06842";
        private const string EvangelistOathbreakerClass9Align = "EvangelistOathbreakerClass9Align";
        private static readonly string EvangelistOathbreakerClass9AlignGuid = "628b0475-6aaa-4832-b5f3-cd6d2f9146cb";
        private const string EvangelistOathbreakerClass10Align = "EvangelistOathbreakerClass10Align";
        private static readonly string EvangelistOathbreakerClass10AlignGuid = "bd344ca9-a5c2-4bc9-a44a-b86c0d2896ad";
        private const string EvangelistDreadKnightClass0Align = "EvangelistDreadKnightClass0Align";
        private static readonly string EvangelistDreadKnightClass0AlignGuid = "3e08f4fb-441a-4f3d-aa7d-0e26113fda06";
        internal const string EvangelistDreadKnightClass0AlignDisplayName = "EvangelistDreadKnightClass0Align.Name";
        private const string EvangelistDreadKnightClass0AlignDescription = "EvangelistDreadKnightClass0Align.Description";
        private const string EvangelistDreadKnightClass2Align = "EvangelistDreadKnightClass2Align";
        private static readonly string EvangelistDreadKnightClass2AlignGuid = "d749fc81-39cb-4ff1-8507-688f614cfcf7";
        private const string EvangelistDreadKnightClass3Align = "EvangelistDreadKnightClass3Align";
        private static readonly string EvangelistDreadKnightClass3AlignGuid = "c386b562-5bba-422e-bba5-35bdfd200b7c";
        private const string EvangelistDreadKnightClass4Align = "EvangelistDreadKnightClass4Align";
        private static readonly string EvangelistDreadKnightClass4AlignGuid = "76c1d541-d1d9-4120-aaaf-136d59892dbf";
        private const string EvangelistDreadKnightClass5Align = "EvangelistDreadKnightClass5Align";
        private static readonly string EvangelistDreadKnightClass5AlignGuid = "36629713-1467-41d2-96ce-9fe890a4624d";
        private const string EvangelistDreadKnightClass6Align = "EvangelistDreadKnightClass6Align";
        private static readonly string EvangelistDreadKnightClass6AlignGuid = "e9df4347-9878-40a6-914a-666dbb9e5e2d";
        private const string EvangelistDreadKnightClass7Align = "EvangelistDreadKnightClass7Align";
        private static readonly string EvangelistDreadKnightClass7AlignGuid = "21e0d68f-238c-4b9d-bb21-47c8024125b6";
        private const string EvangelistDreadKnightClass8Align = "EvangelistDreadKnightClass8Align";
        private static readonly string EvangelistDreadKnightClass8AlignGuid = "15b5abca-24be-414a-a857-ab84d4bdd6d4";
        private const string EvangelistDreadKnightClass9Align = "EvangelistDreadKnightClass9Align";
        private static readonly string EvangelistDreadKnightClass9AlignGuid = "1b3331f7-8ceb-4f49-b067-313164b4ac5b";
        private const string EvangelistDreadKnightClass10Align = "EvangelistDreadKnightClass10Align";
        private static readonly string EvangelistDreadKnightClass10AlignGuid = "d28389a0-ba9e-4541-9d54-25ffda1588f8";
        private const string EvangelistStargazerClass0Align = "EvangelistStargazerClass0Align";
        private static readonly string EvangelistStargazerClass0AlignGuid = "0dc4741c-60a6-4834-8a99-f45c943a52db";
        internal const string EvangelistStargazerClass0AlignDisplayName = "EvangelistStargazerClass0Align.Name";
        private const string EvangelistStargazerClass0AlignDescription = "EvangelistStargazerClass0Align.Description";
        private const string EvangelistStargazerClass2Align = "EvangelistStargazerClass2Align";
        private static readonly string EvangelistStargazerClass2AlignGuid = "b526f2d4-64c8-4fab-a952-e66e4601875b";
        private const string EvangelistStargazerClass3Align = "EvangelistStargazerClass3Align";
        private static readonly string EvangelistStargazerClass3AlignGuid = "9ef5a466-0818-46a7-bda0-6758903bcbea";
        private const string EvangelistStargazerClass4Align = "EvangelistStargazerClass4Align";
        private static readonly string EvangelistStargazerClass4AlignGuid = "409fce76-5a99-45f2-a5c0-eaebf2cf44ae";
        private const string EvangelistStargazerClass5Align = "EvangelistStargazerClass5Align";
        private static readonly string EvangelistStargazerClass5AlignGuid = "a0695cae-aa4b-4aaf-9d5f-0e077b7b53a5";
        private const string EvangelistStargazerClass6Align = "EvangelistStargazerClass6Align";
        private static readonly string EvangelistStargazerClass6AlignGuid = "b744415c-c435-4caa-86e9-c71bd9dcde3b";
        private const string EvangelistStargazerClass7Align = "EvangelistStargazerClass7Align";
        private static readonly string EvangelistStargazerClass7AlignGuid = "cdc5f7e7-c793-4ce7-8a94-bb022ed69c60";
        private const string EvangelistStargazerClass8Align = "EvangelistStargazerClass8Align";
        private static readonly string EvangelistStargazerClass8AlignGuid = "6977b462-4b15-4763-95b2-91f80e59b003";
        private const string EvangelistStargazerClass9Align = "EvangelistStargazerClass9Align";
        private static readonly string EvangelistStargazerClass9AlignGuid = "729b63a8-9f0f-4790-a78c-cbf013e70406";
        private const string EvangelistStargazerClass10Align = "EvangelistStargazerClass10Align";
        private static readonly string EvangelistStargazerClass10AlignGuid = "95f96dfd-0e8f-4384-a6f9-50ac0c7c56ae";
        private const string EvangelistSwashbucklerClass0Align = "EvangelistSwashbucklerClass0Align";
        private static readonly string EvangelistSwashbucklerClass0AlignGuid = "5399ff39-1418-4269-baf7-df3c28428412";
        internal const string EvangelistSwashbucklerClass0AlignDisplayName = "EvangelistSwashbucklerClass0Align.Name";
        private const string EvangelistSwashbucklerClass0AlignDescription = "EvangelistSwashbucklerClass0Align.Description";
        private const string EvangelistSwashbucklerClass2Align = "EvangelistSwashbucklerClass2Align";
        private static readonly string EvangelistSwashbucklerClass2AlignGuid = "fbea7428-c076-428f-b8ad-345f0f0a94c6";
        private const string EvangelistSwashbucklerClass3Align = "EvangelistSwashbucklerClass3Align";
        private static readonly string EvangelistSwashbucklerClass3AlignGuid = "f1ca425b-c5d5-44e7-8029-36b72047c149";
        private const string EvangelistSwashbucklerClass4Align = "EvangelistSwashbucklerClass4Align";
        private static readonly string EvangelistSwashbucklerClass4AlignGuid = "1ef602ba-0acc-4305-99b0-02e1ed6df7c7";
        private const string EvangelistSwashbucklerClass5Align = "EvangelistSwashbucklerClass5Align";
        private static readonly string EvangelistSwashbucklerClass5AlignGuid = "9319c23e-5508-4074-ba05-46dd56324ebc";
        private const string EvangelistSwashbucklerClass6Align = "EvangelistSwashbucklerClass6Align";
        private static readonly string EvangelistSwashbucklerClass6AlignGuid = "f1167d95-0ae7-468d-951c-8411571edcfa";
        private const string EvangelistSwashbucklerClass7Align = "EvangelistSwashbucklerClass7Align";
        private static readonly string EvangelistSwashbucklerClass7AlignGuid = "71476c8b-d9ae-4ed7-9717-bc75f15705c6";
        private const string EvangelistSwashbucklerClass8Align = "EvangelistSwashbucklerClass8Align";
        private static readonly string EvangelistSwashbucklerClass8AlignGuid = "254eac3c-be3e-4861-a2fc-2f04497436bc";
        private const string EvangelistSwashbucklerClass9Align = "EvangelistSwashbucklerClass9Align";
        private static readonly string EvangelistSwashbucklerClass9AlignGuid = "ca0bea7c-49bc-4367-913e-7b1198be5249";
        private const string EvangelistSwashbucklerClass10Align = "EvangelistSwashbucklerClass10Align";
        private static readonly string EvangelistSwashbucklerClass10AlignGuid = "c734779c-a37e-4fc8-bcf8-37745b2aad22";
        private const string EvangelistHolyVindicatorClass0Align = "EvangelistHolyVindicatorClass0Align";
        private static readonly string EvangelistHolyVindicatorClass0AlignGuid = "b2ae5c6f-952a-40cf-9152-711b6edc036d";
        internal const string EvangelistHolyVindicatorClass0AlignDisplayName = "EvangelistHolyVindicatorClass0Align.Name";
        private const string EvangelistHolyVindicatorClass0AlignDescription = "EvangelistHolyVindicatorClass0Align.Description";
        private const string EvangelistHolyVindicatorClass2Align = "EvangelistHolyVindicatorClass2Align";
        private static readonly string EvangelistHolyVindicatorClass2AlignGuid = "34633fbd-4e40-457e-a936-cab662f6140e";
        private const string EvangelistHolyVindicatorClass3Align = "EvangelistHolyVindicatorClass3Align";
        private static readonly string EvangelistHolyVindicatorClass3AlignGuid = "8fc46c6f-9618-4c4b-b81d-59cf75d36088";
        private const string EvangelistHolyVindicatorClass4Align = "EvangelistHolyVindicatorClass4Align";
        private static readonly string EvangelistHolyVindicatorClass4AlignGuid = "f2634797-ce09-4e69-8377-505c03342cc8";
        private const string EvangelistHolyVindicatorClass5Align = "EvangelistHolyVindicatorClass5Align";
        private static readonly string EvangelistHolyVindicatorClass5AlignGuid = "6e1b84f1-b198-4916-aebd-436694528ea6";
        private const string EvangelistHolyVindicatorClass6Align = "EvangelistHolyVindicatorClass6Align";
        private static readonly string EvangelistHolyVindicatorClass6AlignGuid = "e5f98b24-5d24-4b1b-b6cb-c177101faeea";
        private const string EvangelistHolyVindicatorClass7Align = "EvangelistHolyVindicatorClass7Align";
        private static readonly string EvangelistHolyVindicatorClass7AlignGuid = "7ac6e619-595e-4468-866a-5b5ada48b7dd";
        private const string EvangelistHolyVindicatorClass8Align = "EvangelistHolyVindicatorClass8Align";
        private static readonly string EvangelistHolyVindicatorClass8AlignGuid = "e34d389a-5054-4f3d-a68e-b5926ab51d6f";
        private const string EvangelistHolyVindicatorClass9Align = "EvangelistHolyVindicatorClass9Align";
        private static readonly string EvangelistHolyVindicatorClass9AlignGuid = "a82ce735-7f2b-4219-af76-654e3c237f00";
        private const string EvangelistHolyVindicatorClass10Align = "EvangelistHolyVindicatorClass10Align";
        private static readonly string EvangelistHolyVindicatorClass10AlignGuid = "1e88e2db-3a43-4963-b2f9-705432eabc3d";
        private const string EvangelistSummonerClass0Align = "EvangelistSummonerClass0Align";
        private static readonly string EvangelistSummonerClass0AlignGuid = "44242c61-690b-4de4-94d9-f1144046dc71";
        internal const string EvangelistSummonerClass0AlignDisplayName = "EvangelistSummonerClass0Align.Name";
        private const string EvangelistSummonerClass0AlignDescription = "EvangelistSummonerClass0Align.Description";
        private const string EvangelistSummonerClass2Align = "EvangelistSummonerClass2Align";
        private static readonly string EvangelistSummonerClass2AlignGuid = "3d0993bd-5e81-4c29-afbc-3455c2b9c6c2";
        private const string EvangelistSummonerClass3Align = "EvangelistSummonerClass3Align";
        private static readonly string EvangelistSummonerClass3AlignGuid = "2df491b3-41c5-4d0a-81dc-648c7082a2ea";
        private const string EvangelistSummonerClass4Align = "EvangelistSummonerClass4Align";
        private static readonly string EvangelistSummonerClass4AlignGuid = "65c69565-8e94-4975-a06a-f77fefccf191";
        private const string EvangelistSummonerClass5Align = "EvangelistSummonerClass5Align";
        private static readonly string EvangelistSummonerClass5AlignGuid = "1e2404a8-ad46-441c-856f-a2749e7fee9a";
        private const string EvangelistSummonerClass6Align = "EvangelistSummonerClass6Align";
        private static readonly string EvangelistSummonerClass6AlignGuid = "ced6ec9d-0ac8-43f1-be4d-9b7658908857";
        private const string EvangelistSummonerClass7Align = "EvangelistSummonerClass7Align";
        private static readonly string EvangelistSummonerClass7AlignGuid = "670fc310-f0f6-4dd9-8892-ad957ee6afb2";
        private const string EvangelistSummonerClass8Align = "EvangelistSummonerClass8Align";
        private static readonly string EvangelistSummonerClass8AlignGuid = "31f9c549-5e8d-40e0-9e0a-4924d2f6339a";
        private const string EvangelistSummonerClass9Align = "EvangelistSummonerClass9Align";
        private static readonly string EvangelistSummonerClass9AlignGuid = "affd5256-64ea-4c65-ae83-3a1cb87a4f04";
        private const string EvangelistSummonerClass10Align = "EvangelistSummonerClass10Align";
        private static readonly string EvangelistSummonerClass10AlignGuid = "1e65cefb-45ad-414e-bc7f-e9c1061ca565";
        private const string EvangelistLionBladeClass0Align = "EvangelistLionBladeClass0Align";
        private static readonly string EvangelistLionBladeClass0AlignGuid = "8030879c-affd-4f16-91f8-7f3069093a6b";
        internal const string EvangelistLionBladeClass0AlignDisplayName = "EvangelistLionBladeClass0Align.Name";
        private const string EvangelistLionBladeClass0AlignDescription = "EvangelistLionBladeClass0Align.Description";
        private const string EvangelistLionBladeClass2Align = "EvangelistLionBladeClass2Align";
        private static readonly string EvangelistLionBladeClass2AlignGuid = "a4d49101-e932-4b30-88f1-c47ce088048b";
        private const string EvangelistLionBladeClass3Align = "EvangelistLionBladeClass3Align";
        private static readonly string EvangelistLionBladeClass3AlignGuid = "aa3c08e6-0532-46e9-a5f6-c97198a0152d";
        private const string EvangelistLionBladeClass4Align = "EvangelistLionBladeClass4Align";
        private static readonly string EvangelistLionBladeClass4AlignGuid = "d625465e-7ab4-4d96-b74c-6773029522b3";
        private const string EvangelistLionBladeClass5Align = "EvangelistLionBladeClass5Align";
        private static readonly string EvangelistLionBladeClass5AlignGuid = "e66b287a-35aa-4637-b44d-987030dc95a5";
        private const string EvangelistLionBladeClass6Align = "EvangelistLionBladeClass6Align";
        private static readonly string EvangelistLionBladeClass6AlignGuid = "bdca1b8a-87da-4b8f-b0a5-9eed723e50f6";
        private const string EvangelistLionBladeClass7Align = "EvangelistLionBladeClass7Align";
        private static readonly string EvangelistLionBladeClass7AlignGuid = "8a968098-398a-47c4-9fa0-bef9cdb4ed32";
        private const string EvangelistLionBladeClass8Align = "EvangelistLionBladeClass8Align";
        private static readonly string EvangelistLionBladeClass8AlignGuid = "7487a6d5-5c04-4216-a99d-f9c54873132d";
        private const string EvangelistLionBladeClass9Align = "EvangelistLionBladeClass9Align";
        private static readonly string EvangelistLionBladeClass9AlignGuid = "feaa548a-9f16-4c77-8518-1237529b00d9";
        private const string EvangelistLionBladeClass10Align = "EvangelistLionBladeClass10Align";
        private static readonly string EvangelistLionBladeClass10AlignGuid = "b08024e1-8646-4479-84dc-af9ab6b1bc22";
        private const string EvangelistEnchantingCourtesanClass0Align = "EvangelistEnchantingCourtesanClass0Align";
        private static readonly string EvangelistEnchantingCourtesanClass0AlignGuid = "1d18efc5-bf96-44ad-9e08-61a1d163d4b7";
        internal const string EvangelistEnchantingCourtesanClass0AlignDisplayName = "EvangelistEnchantingCourtesanClass0Align.Name";
        private const string EvangelistEnchantingCourtesanClass0AlignDescription = "EvangelistEnchantingCourtesanClass0Align.Description";
        private const string EvangelistEnchantingCourtesanClass2Align = "EvangelistEnchantingCourtesanClass2Align";
        private static readonly string EvangelistEnchantingCourtesanClass2AlignGuid = "f462c9c3-06e1-4f31-adae-a0aa213d9b68";
        private const string EvangelistEnchantingCourtesanClass3Align = "EvangelistEnchantingCourtesanClass3Align";
        private static readonly string EvangelistEnchantingCourtesanClass3AlignGuid = "7c49b81d-8537-49e5-923f-a3fcdb128d2f";
        private const string EvangelistEnchantingCourtesanClass4Align = "EvangelistEnchantingCourtesanClass4Align";
        private static readonly string EvangelistEnchantingCourtesanClass4AlignGuid = "01949905-3d19-4e54-9f79-501fa1dc6cd0";
        private const string EvangelistEnchantingCourtesanClass5Align = "EvangelistEnchantingCourtesanClass5Align";
        private static readonly string EvangelistEnchantingCourtesanClass5AlignGuid = "6f9a023e-fe59-4214-992c-16960457bd24";
        private const string EvangelistEnchantingCourtesanClass6Align = "EvangelistEnchantingCourtesanClass6Align";
        private static readonly string EvangelistEnchantingCourtesanClass6AlignGuid = "74d5a9a0-8816-4eb9-96b7-dad598c14804";
        private const string EvangelistEnchantingCourtesanClass7Align = "EvangelistEnchantingCourtesanClass7Align";
        private static readonly string EvangelistEnchantingCourtesanClass7AlignGuid = "a8346a7b-14db-4f1e-a8b9-be48a109d553";
        private const string EvangelistEnchantingCourtesanClass8Align = "EvangelistEnchantingCourtesanClass8Align";
        private static readonly string EvangelistEnchantingCourtesanClass8AlignGuid = "334e072a-c768-46b3-8494-922f9a435052";
        private const string EvangelistEnchantingCourtesanClass9Align = "EvangelistEnchantingCourtesanClass9Align";
        private static readonly string EvangelistEnchantingCourtesanClass9AlignGuid = "a4e19211-27c1-488c-a908-4ee439dfd122";
        private const string EvangelistEnchantingCourtesanClass10Align = "EvangelistEnchantingCourtesanClass10Align";
        private static readonly string EvangelistEnchantingCourtesanClass10AlignGuid = "492a0bab-0663-481e-b045-fe2ce44f3125";
        private const string EvangelistHeritorKnightClass0Align = "EvangelistHeritorKnightClass0Align";
        private static readonly string EvangelistHeritorKnightClass0AlignGuid = "ac2002f0-5c63-4ea8-a3a2-aa6fd54a5faf";
        internal const string EvangelistHeritorKnightClass0AlignDisplayName = "EvangelistHeritorKnightClass0Align.Name";
        private const string EvangelistHeritorKnightClass0AlignDescription = "EvangelistHeritorKnightClass0Align.Description";
        private const string EvangelistHeritorKnightClass2Align = "EvangelistHeritorKnightClass2Align";
        private static readonly string EvangelistHeritorKnightClass2AlignGuid = "fa33917b-a739-4634-93a4-c5f43d104873";
        private const string EvangelistHeritorKnightClass3Align = "EvangelistHeritorKnightClass3Align";
        private static readonly string EvangelistHeritorKnightClass3AlignGuid = "16829840-e5ea-4bce-9edf-03e4c53f6257";
        private const string EvangelistHeritorKnightClass4Align = "EvangelistHeritorKnightClass4Align";
        private static readonly string EvangelistHeritorKnightClass4AlignGuid = "5857010e-c873-4614-b663-fcaf30343e06";
        private const string EvangelistHeritorKnightClass5Align = "EvangelistHeritorKnightClass5Align";
        private static readonly string EvangelistHeritorKnightClass5AlignGuid = "c5cc47be-63b9-441a-b185-b93a9c6a722f";
        private const string EvangelistHeritorKnightClass6Align = "EvangelistHeritorKnightClass6Align";
        private static readonly string EvangelistHeritorKnightClass6AlignGuid = "01f5bd2f-0209-4615-99d6-03130554435d";
        private const string EvangelistHeritorKnightClass7Align = "EvangelistHeritorKnightClass7Align";
        private static readonly string EvangelistHeritorKnightClass7AlignGuid = "3b1156c5-928d-47d4-ab42-640c2b36dc4d";
        private const string EvangelistHeritorKnightClass8Align = "EvangelistHeritorKnightClass8Align";
        private static readonly string EvangelistHeritorKnightClass8AlignGuid = "98992341-dfcc-472c-b12f-0279d8ee7c42";
        private const string EvangelistHeritorKnightClass9Align = "EvangelistHeritorKnightClass9Align";
        private static readonly string EvangelistHeritorKnightClass9AlignGuid = "57246171-5b36-4892-9233-631ece377899";
        private const string EvangelistHeritorKnightClass10Align = "EvangelistHeritorKnightClass10Align";
        private static readonly string EvangelistHeritorKnightClass10AlignGuid = "cd414dd0-c3fd-4f9e-bc4e-af7c3a8a6b27";
        private const string EvangelistGoldenLegionnaireClass0Align = "EvangelistGoldenLegionnaireClass0Align";
        private static readonly string EvangelistGoldenLegionnaireClass0AlignGuid = "01d6c0b3-d037-4fda-9892-7c2baa26b849";
        internal const string EvangelistGoldenLegionnaireClass0AlignDisplayName = "EvangelistGoldenLegionnaireClass0Align.Name";
        private const string EvangelistGoldenLegionnaireClass0AlignDescription = "EvangelistGoldenLegionnaireClass0Align.Description";
        private const string EvangelistGoldenLegionnaireClass2Align = "EvangelistGoldenLegionnaireClass2Align";
        private static readonly string EvangelistGoldenLegionnaireClass2AlignGuid = "85cb3c05-ee23-473c-9210-48072caca3fb";
        private const string EvangelistGoldenLegionnaireClass3Align = "EvangelistGoldenLegionnaireClass3Align";
        private static readonly string EvangelistGoldenLegionnaireClass3AlignGuid = "d02a87e7-0c35-407a-8dc2-ac1d4407ef9f";
        private const string EvangelistGoldenLegionnaireClass4Align = "EvangelistGoldenLegionnaireClass4Align";
        private static readonly string EvangelistGoldenLegionnaireClass4AlignGuid = "c774f3b7-44c8-4db7-a0a4-1fdcd026c4b6";
        private const string EvangelistGoldenLegionnaireClass5Align = "EvangelistGoldenLegionnaireClass5Align";
        private static readonly string EvangelistGoldenLegionnaireClass5AlignGuid = "2aa0858c-cdb3-48d1-9aa0-e7e2a7fae1b7";
        private const string EvangelistGoldenLegionnaireClass6Align = "EvangelistGoldenLegionnaireClass6Align";
        private static readonly string EvangelistGoldenLegionnaireClass6AlignGuid = "a0dd829c-cac3-4387-987d-af502d1866a6";
        private const string EvangelistGoldenLegionnaireClass7Align = "EvangelistGoldenLegionnaireClass7Align";
        private static readonly string EvangelistGoldenLegionnaireClass7AlignGuid = "6c98983d-f8c2-40bb-986b-90d03f8511c3";
        private const string EvangelistGoldenLegionnaireClass8Align = "EvangelistGoldenLegionnaireClass8Align";
        private static readonly string EvangelistGoldenLegionnaireClass8AlignGuid = "843e3e7a-5040-4996-8d4c-5c6f75186a13";
        private const string EvangelistGoldenLegionnaireClass9Align = "EvangelistGoldenLegionnaireClass9Align";
        private static readonly string EvangelistGoldenLegionnaireClass9AlignGuid = "fd7762a2-cf65-4f25-8ae2-8615d9c04a37";
        private const string EvangelistGoldenLegionnaireClass10Align = "EvangelistGoldenLegionnaireClass10Align";
        private static readonly string EvangelistGoldenLegionnaireClass10AlignGuid = "32da8509-fa7d-4407-a760-0024e629d273";
        private const string EvangelistBoltAceClass0Align = "EvangelistBoltAceClass0Align";
        private static readonly string EvangelistBoltAceClass0AlignGuid = "123d1403-4388-419a-a59e-68d5bc7a1fbd";
        internal const string EvangelistBoltAceClass0AlignDisplayName = "EvangelistBoltAceClass0Align.Name";
        private const string EvangelistBoltAceClass0AlignDescription = "EvangelistBoltAceClass0Align.Description";
        private const string EvangelistBoltAceClass2Align = "EvangelistBoltAceClass2Align";
        private static readonly string EvangelistBoltAceClass2AlignGuid = "98fe326e-c144-43fc-8265-27201906f7ee";
        private const string EvangelistBoltAceClass3Align = "EvangelistBoltAceClass3Align";
        private static readonly string EvangelistBoltAceClass3AlignGuid = "b3e315e8-283e-4584-bb0d-74b281e7a672";
        private const string EvangelistBoltAceClass4Align = "EvangelistBoltAceClass4Align";
        private static readonly string EvangelistBoltAceClass4AlignGuid = "e2f4546c-52c9-462c-be55-2520d1a69d16";
        private const string EvangelistBoltAceClass5Align = "EvangelistBoltAceClass5Align";
        private static readonly string EvangelistBoltAceClass5AlignGuid = "5be68d87-6b81-486d-bc7a-c35c833deb57";
        private const string EvangelistBoltAceClass6Align = "EvangelistBoltAceClass6Align";
        private static readonly string EvangelistBoltAceClass6AlignGuid = "cf526328-07c8-41ec-b9e5-a76f7c608d11";
        private const string EvangelistBoltAceClass7Align = "EvangelistBoltAceClass7Align";
        private static readonly string EvangelistBoltAceClass7AlignGuid = "49511465-f000-4665-aed3-4ce1ead2a514";
        private const string EvangelistBoltAceClass8Align = "EvangelistBoltAceClass8Align";
        private static readonly string EvangelistBoltAceClass8AlignGuid = "dd12832d-3a63-4baa-b0c0-c7bfdecd2d48";
        private const string EvangelistBoltAceClass9Align = "EvangelistBoltAceClass9Align";
        private static readonly string EvangelistBoltAceClass9AlignGuid = "b9f2ccef-0970-460b-a86f-dd617b226a30";
        private const string EvangelistBoltAceClass10Align = "EvangelistBoltAceClass10Align";
        private static readonly string EvangelistBoltAceClass10AlignGuid = "16d03c5f-078a-41af-bb47-708d6725bba6";
        private const string EvangelistMortalUsherClass0Align = "EvangelistMortalUsherClass0Align";
        private static readonly string EvangelistMortalUsherClass0AlignGuid = "13029daf-742b-4a05-87c5-36ae76da0dec";
        internal const string EvangelistMortalUsherClass0AlignDisplayName = "EvangelistMortalUsherClass0Align.Name";
        private const string EvangelistMortalUsherClass0AlignDescription = "EvangelistMortalUsherClass0Align.Description";
        private const string EvangelistMortalUsherClass2Align = "EvangelistMortalUsherClass2Align";
        private static readonly string EvangelistMortalUsherClass2AlignGuid = "dc46cf9e-b565-4f82-966a-44f425a17fcb";
        private const string EvangelistMortalUsherClass3Align = "EvangelistMortalUsherClass3Align";
        private static readonly string EvangelistMortalUsherClass3AlignGuid = "b33172c8-580b-47cb-8a83-5ff91f8a1385";
        private const string EvangelistMortalUsherClass4Align = "EvangelistMortalUsherClass4Align";
        private static readonly string EvangelistMortalUsherClass4AlignGuid = "0232ff79-9293-4b3f-acea-653df7b6b005";
        private const string EvangelistMortalUsherClass5Align = "EvangelistMortalUsherClass5Align";
        private static readonly string EvangelistMortalUsherClass5AlignGuid = "e5c913c0-7453-42fa-b0cc-9008aa194d57";
        private const string EvangelistMortalUsherClass6Align = "EvangelistMortalUsherClass6Align";
        private static readonly string EvangelistMortalUsherClass6AlignGuid = "b81d9ace-f336-42b2-af79-09c14f6a50ee";
        private const string EvangelistMortalUsherClass7Align = "EvangelistMortalUsherClass7Align";
        private static readonly string EvangelistMortalUsherClass7AlignGuid = "1352fc53-7df1-4984-bb3a-a99a2bf4d1d5";
        private const string EvangelistMortalUsherClass8Align = "EvangelistMortalUsherClass8Align";
        private static readonly string EvangelistMortalUsherClass8AlignGuid = "c2f3fe18-9dd5-4819-8d61-c3f1157dee5b";
        private const string EvangelistMortalUsherClass9Align = "EvangelistMortalUsherClass9Align";
        private static readonly string EvangelistMortalUsherClass9AlignGuid = "465bdb5c-36e6-4dee-96b4-8e2ab42b2cbd";
        private const string EvangelistMortalUsherClass10Align = "EvangelistMortalUsherClass10Align";
        private static readonly string EvangelistMortalUsherClass10AlignGuid = "22773a77-b59d-4920-b7e1-8f88ebaa576b";
        private const string EvangelistForestWardenClass0Align = "EvangelistForestWardenClass0Align";
        private static readonly string EvangelistForestWardenClass0AlignGuid = "6772d8ff-8f74-494f-9a3b-e2ed4afb2563";
        internal const string EvangelistForestWardenClass0AlignDisplayName = "EvangelistForestWardenClass0Align.Name";
        private const string EvangelistForestWardenClass0AlignDescription = "EvangelistForestWardenClass0Align.Description";
        private const string EvangelistForestWardenClass2Align = "EvangelistForestWardenClass2Align";
        private static readonly string EvangelistForestWardenClass2AlignGuid = "73319b5e-8791-4d34-aa7f-bc58c3c98fb7";
        private const string EvangelistForestWardenClass3Align = "EvangelistForestWardenClass3Align";
        private static readonly string EvangelistForestWardenClass3AlignGuid = "ad735f9e-1d45-49f6-a996-de1999e6a0b5";
        private const string EvangelistForestWardenClass4Align = "EvangelistForestWardenClass4Align";
        private static readonly string EvangelistForestWardenClass4AlignGuid = "572496cc-6dbd-4219-a3b3-7a9283022ab5";
        private const string EvangelistForestWardenClass5Align = "EvangelistForestWardenClass5Align";
        private static readonly string EvangelistForestWardenClass5AlignGuid = "28bd0123-09dd-4554-aa32-890a33a839ad";
        private const string EvangelistForestWardenClass6Align = "EvangelistForestWardenClass6Align";
        private static readonly string EvangelistForestWardenClass6AlignGuid = "eba7903e-3a15-4d66-ab65-6bbaad1d9d9b";
        private const string EvangelistForestWardenClass7Align = "EvangelistForestWardenClass7Align";
        private static readonly string EvangelistForestWardenClass7AlignGuid = "8ea16a7b-da1f-4581-bb34-5e28ec001508";
        private const string EvangelistForestWardenClass8Align = "EvangelistForestWardenClass8Align";
        private static readonly string EvangelistForestWardenClass8AlignGuid = "d4f55056-6faa-4c1a-a944-8af41761c8fc";
        private const string EvangelistForestWardenClass9Align = "EvangelistForestWardenClass9Align";
        private static readonly string EvangelistForestWardenClass9AlignGuid = "d90d8ad7-4372-443e-9f73-d0719265b012";
        private const string EvangelistForestWardenClass10Align = "EvangelistForestWardenClass10Align";
        private static readonly string EvangelistForestWardenClass10AlignGuid = "763fb196-4778-478a-a462-cd12e5145de0";

    }
}
