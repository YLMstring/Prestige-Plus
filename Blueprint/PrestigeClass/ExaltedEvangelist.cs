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
            .AddComponent<PrerequisiteCasterLevel>(c => { c.RequiredCasterLevel = 3; c.Group = Prerequisite.GroupType.Any; })
            .AddPrerequisiteFeature(DeificObedience.DeificObedienceGuid)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var ExaltedEvangelistref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = ExaltedEvangelistref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
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
              .AddToAllFeatures(SanctifiedRogueFeat())
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
                .SetDeactivateImmediately(true)
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
        private static readonly string ExaltedDomainPlusGuid = "{18E8AE34-E61D-4149-989B-4E55560F03CA}";

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

            

            var select = FeatureSelectionConfigurator.New(AlignSpam, AlignSpamGuid)
              .SetDisplayName(SanctifiedRogueDisplayName)
              .SetDescription(SanctifiedRogueDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(DeificObedience.MahathallahExaltedGuid)
              .AddToAllFeatures(DeificObedience.LamashtuExaltedGuid)
              .AddToAllFeatures(DeificObedience.NiviExaltedGuid)
              .AddToAllFeatures(DeificObedience.KabririExaltedGuid)
              .Configure();

            return select;
        }
        private const string EvangelistAlchemistClass0Align = "EvangelistAlchemistClass0Align";
        private static readonly string EvangelistAlchemistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistAlchemistClass0AlignDisplayName = "EvangelistAlchemistClass0Align.Name";
        private const string EvangelistAlchemistClass0AlignDescription = "EvangelistAlchemistClass0Align.Description";
        private const string EvangelistAlchemistClass2Align = "EvangelistAlchemistClass2Align";
        private static readonly string EvangelistAlchemistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass3Align = "EvangelistAlchemistClass3Align";
        private static readonly string EvangelistAlchemistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass4Align = "EvangelistAlchemistClass4Align";
        private static readonly string EvangelistAlchemistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass5Align = "EvangelistAlchemistClass5Align";
        private static readonly string EvangelistAlchemistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass6Align = "EvangelistAlchemistClass6Align";
        private static readonly string EvangelistAlchemistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass7Align = "EvangelistAlchemistClass7Align";
        private static readonly string EvangelistAlchemistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass8Align = "EvangelistAlchemistClass8Align";
        private static readonly string EvangelistAlchemistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass9Align = "EvangelistAlchemistClass9Align";
        private static readonly string EvangelistAlchemistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistAlchemistClass10Align = "EvangelistAlchemistClass10Align";
        private static readonly string EvangelistAlchemistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass0Align = "EvangelistArcaneTricksterClass0Align";
        private static readonly string EvangelistArcaneTricksterClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistArcaneTricksterClass0AlignDisplayName = "EvangelistArcaneTricksterClass0Align.Name";
        private const string EvangelistArcaneTricksterClass0AlignDescription = "EvangelistArcaneTricksterClass0Align.Description";
        private const string EvangelistArcaneTricksterClass2Align = "EvangelistArcaneTricksterClass2Align";
        private static readonly string EvangelistArcaneTricksterClass2AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass3Align = "EvangelistArcaneTricksterClass3Align";
        private static readonly string EvangelistArcaneTricksterClass3AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass4Align = "EvangelistArcaneTricksterClass4Align";
        private static readonly string EvangelistArcaneTricksterClass4AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass5Align = "EvangelistArcaneTricksterClass5Align";
        private static readonly string EvangelistArcaneTricksterClass5AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass6Align = "EvangelistArcaneTricksterClass6Align";
        private static readonly string EvangelistArcaneTricksterClass6AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass7Align = "EvangelistArcaneTricksterClass7Align";
        private static readonly string EvangelistArcaneTricksterClass7AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass8Align = "EvangelistArcaneTricksterClass8Align";
        private static readonly string EvangelistArcaneTricksterClass8AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass9Align = "EvangelistArcaneTricksterClass9Align";
        private static readonly string EvangelistArcaneTricksterClass9AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneTricksterClass10Align = "EvangelistArcaneTricksterClass10Align";
        private static readonly string EvangelistArcaneTricksterClass10AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass0Align = "EvangelistArcanistClass0Align";
        private static readonly string EvangelistArcanistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistArcanistClass0AlignDisplayName = "EvangelistArcanistClass0Align.Name";
        private const string EvangelistArcanistClass0AlignDescription = "EvangelistArcanistClass0Align.Description";
        private const string EvangelistArcanistClass2Align = "EvangelistArcanistClass2Align";
        private static readonly string EvangelistArcanistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass3Align = "EvangelistArcanistClass3Align";
        private static readonly string EvangelistArcanistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass4Align = "EvangelistArcanistClass4Align";
        private static readonly string EvangelistArcanistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass5Align = "EvangelistArcanistClass5Align";
        private static readonly string EvangelistArcanistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass6Align = "EvangelistArcanistClass6Align";
        private static readonly string EvangelistArcanistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass7Align = "EvangelistArcanistClass7Align";
        private static readonly string EvangelistArcanistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass8Align = "EvangelistArcanistClass8Align";
        private static readonly string EvangelistArcanistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass9Align = "EvangelistArcanistClass9Align";
        private static readonly string EvangelistArcanistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistArcanistClass10Align = "EvangelistArcanistClass10Align";
        private static readonly string EvangelistArcanistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass0Align = "EvangelistAssassinClass0Align";
        private static readonly string EvangelistAssassinClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistAssassinClass0AlignDisplayName = "EvangelistAssassinClass0Align.Name";
        private const string EvangelistAssassinClass0AlignDescription = "EvangelistAssassinClass0Align.Description";
        private const string EvangelistAssassinClass2Align = "EvangelistAssassinClass2Align";
        private static readonly string EvangelistAssassinClass2AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass3Align = "EvangelistAssassinClass3Align";
        private static readonly string EvangelistAssassinClass3AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass4Align = "EvangelistAssassinClass4Align";
        private static readonly string EvangelistAssassinClass4AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass5Align = "EvangelistAssassinClass5Align";
        private static readonly string EvangelistAssassinClass5AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass6Align = "EvangelistAssassinClass6Align";
        private static readonly string EvangelistAssassinClass6AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass7Align = "EvangelistAssassinClass7Align";
        private static readonly string EvangelistAssassinClass7AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass8Align = "EvangelistAssassinClass8Align";
        private static readonly string EvangelistAssassinClass8AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass9Align = "EvangelistAssassinClass9Align";
        private static readonly string EvangelistAssassinClass9AlignGuid = "replaceguidhere";
        private const string EvangelistAssassinClass10Align = "EvangelistAssassinClass10Align";
        private static readonly string EvangelistAssassinClass10AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass0Align = "EvangelistBarbarianClass0Align";
        private static readonly string EvangelistBarbarianClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistBarbarianClass0AlignDisplayName = "EvangelistBarbarianClass0Align.Name";
        private const string EvangelistBarbarianClass0AlignDescription = "EvangelistBarbarianClass0Align.Description";
        private const string EvangelistBarbarianClass2Align = "EvangelistBarbarianClass2Align";
        private static readonly string EvangelistBarbarianClass2AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass3Align = "EvangelistBarbarianClass3Align";
        private static readonly string EvangelistBarbarianClass3AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass4Align = "EvangelistBarbarianClass4Align";
        private static readonly string EvangelistBarbarianClass4AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass5Align = "EvangelistBarbarianClass5Align";
        private static readonly string EvangelistBarbarianClass5AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass6Align = "EvangelistBarbarianClass6Align";
        private static readonly string EvangelistBarbarianClass6AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass7Align = "EvangelistBarbarianClass7Align";
        private static readonly string EvangelistBarbarianClass7AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass8Align = "EvangelistBarbarianClass8Align";
        private static readonly string EvangelistBarbarianClass8AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass9Align = "EvangelistBarbarianClass9Align";
        private static readonly string EvangelistBarbarianClass9AlignGuid = "replaceguidhere";
        private const string EvangelistBarbarianClass10Align = "EvangelistBarbarianClass10Align";
        private static readonly string EvangelistBarbarianClass10AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass0Align = "EvangelistBardClass0Align";
        private static readonly string EvangelistBardClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistBardClass0AlignDisplayName = "EvangelistBardClass0Align.Name";
        private const string EvangelistBardClass0AlignDescription = "EvangelistBardClass0Align.Description";
        private const string EvangelistBardClass2Align = "EvangelistBardClass2Align";
        private static readonly string EvangelistBardClass2AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass3Align = "EvangelistBardClass3Align";
        private static readonly string EvangelistBardClass3AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass4Align = "EvangelistBardClass4Align";
        private static readonly string EvangelistBardClass4AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass5Align = "EvangelistBardClass5Align";
        private static readonly string EvangelistBardClass5AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass6Align = "EvangelistBardClass6Align";
        private static readonly string EvangelistBardClass6AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass7Align = "EvangelistBardClass7Align";
        private static readonly string EvangelistBardClass7AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass8Align = "EvangelistBardClass8Align";
        private static readonly string EvangelistBardClass8AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass9Align = "EvangelistBardClass9Align";
        private static readonly string EvangelistBardClass9AlignGuid = "replaceguidhere";
        private const string EvangelistBardClass10Align = "EvangelistBardClass10Align";
        private static readonly string EvangelistBardClass10AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass0Align = "EvangelistBloodragerClass0Align";
        private static readonly string EvangelistBloodragerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistBloodragerClass0AlignDisplayName = "EvangelistBloodragerClass0Align.Name";
        private const string EvangelistBloodragerClass0AlignDescription = "EvangelistBloodragerClass0Align.Description";
        private const string EvangelistBloodragerClass2Align = "EvangelistBloodragerClass2Align";
        private static readonly string EvangelistBloodragerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass3Align = "EvangelistBloodragerClass3Align";
        private static readonly string EvangelistBloodragerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass4Align = "EvangelistBloodragerClass4Align";
        private static readonly string EvangelistBloodragerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass5Align = "EvangelistBloodragerClass5Align";
        private static readonly string EvangelistBloodragerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass6Align = "EvangelistBloodragerClass6Align";
        private static readonly string EvangelistBloodragerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass7Align = "EvangelistBloodragerClass7Align";
        private static readonly string EvangelistBloodragerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass8Align = "EvangelistBloodragerClass8Align";
        private static readonly string EvangelistBloodragerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass9Align = "EvangelistBloodragerClass9Align";
        private static readonly string EvangelistBloodragerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistBloodragerClass10Align = "EvangelistBloodragerClass10Align";
        private static readonly string EvangelistBloodragerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass0Align = "EvangelistCavalierClass0Align";
        private static readonly string EvangelistCavalierClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistCavalierClass0AlignDisplayName = "EvangelistCavalierClass0Align.Name";
        private const string EvangelistCavalierClass0AlignDescription = "EvangelistCavalierClass0Align.Description";
        private const string EvangelistCavalierClass2Align = "EvangelistCavalierClass2Align";
        private static readonly string EvangelistCavalierClass2AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass3Align = "EvangelistCavalierClass3Align";
        private static readonly string EvangelistCavalierClass3AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass4Align = "EvangelistCavalierClass4Align";
        private static readonly string EvangelistCavalierClass4AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass5Align = "EvangelistCavalierClass5Align";
        private static readonly string EvangelistCavalierClass5AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass6Align = "EvangelistCavalierClass6Align";
        private static readonly string EvangelistCavalierClass6AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass7Align = "EvangelistCavalierClass7Align";
        private static readonly string EvangelistCavalierClass7AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass8Align = "EvangelistCavalierClass8Align";
        private static readonly string EvangelistCavalierClass8AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass9Align = "EvangelistCavalierClass9Align";
        private static readonly string EvangelistCavalierClass9AlignGuid = "replaceguidhere";
        private const string EvangelistCavalierClass10Align = "EvangelistCavalierClass10Align";
        private static readonly string EvangelistCavalierClass10AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass0Align = "EvangelistClericClass0Align";
        private static readonly string EvangelistClericClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistClericClass0AlignDisplayName = "EvangelistClericClass0Align.Name";
        private const string EvangelistClericClass0AlignDescription = "EvangelistClericClass0Align.Description";
        private const string EvangelistClericClass2Align = "EvangelistClericClass2Align";
        private static readonly string EvangelistClericClass2AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass3Align = "EvangelistClericClass3Align";
        private static readonly string EvangelistClericClass3AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass4Align = "EvangelistClericClass4Align";
        private static readonly string EvangelistClericClass4AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass5Align = "EvangelistClericClass5Align";
        private static readonly string EvangelistClericClass5AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass6Align = "EvangelistClericClass6Align";
        private static readonly string EvangelistClericClass6AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass7Align = "EvangelistClericClass7Align";
        private static readonly string EvangelistClericClass7AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass8Align = "EvangelistClericClass8Align";
        private static readonly string EvangelistClericClass8AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass9Align = "EvangelistClericClass9Align";
        private static readonly string EvangelistClericClass9AlignGuid = "replaceguidhere";
        private const string EvangelistClericClass10Align = "EvangelistClericClass10Align";
        private static readonly string EvangelistClericClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass0Align = "EvangelistDragonDiscipleClass0Align";
        private static readonly string EvangelistDragonDiscipleClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDragonDiscipleClass0AlignDisplayName = "EvangelistDragonDiscipleClass0Align.Name";
        private const string EvangelistDragonDiscipleClass0AlignDescription = "EvangelistDragonDiscipleClass0Align.Description";
        private const string EvangelistDragonDiscipleClass2Align = "EvangelistDragonDiscipleClass2Align";
        private static readonly string EvangelistDragonDiscipleClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass3Align = "EvangelistDragonDiscipleClass3Align";
        private static readonly string EvangelistDragonDiscipleClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass4Align = "EvangelistDragonDiscipleClass4Align";
        private static readonly string EvangelistDragonDiscipleClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass5Align = "EvangelistDragonDiscipleClass5Align";
        private static readonly string EvangelistDragonDiscipleClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass6Align = "EvangelistDragonDiscipleClass6Align";
        private static readonly string EvangelistDragonDiscipleClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass7Align = "EvangelistDragonDiscipleClass7Align";
        private static readonly string EvangelistDragonDiscipleClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass8Align = "EvangelistDragonDiscipleClass8Align";
        private static readonly string EvangelistDragonDiscipleClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass9Align = "EvangelistDragonDiscipleClass9Align";
        private static readonly string EvangelistDragonDiscipleClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDragonDiscipleClass10Align = "EvangelistDragonDiscipleClass10Align";
        private static readonly string EvangelistDragonDiscipleClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass0Align = "EvangelistDruidClass0Align";
        private static readonly string EvangelistDruidClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDruidClass0AlignDisplayName = "EvangelistDruidClass0Align.Name";
        private const string EvangelistDruidClass0AlignDescription = "EvangelistDruidClass0Align.Description";
        private const string EvangelistDruidClass2Align = "EvangelistDruidClass2Align";
        private static readonly string EvangelistDruidClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass3Align = "EvangelistDruidClass3Align";
        private static readonly string EvangelistDruidClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass4Align = "EvangelistDruidClass4Align";
        private static readonly string EvangelistDruidClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass5Align = "EvangelistDruidClass5Align";
        private static readonly string EvangelistDruidClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass6Align = "EvangelistDruidClass6Align";
        private static readonly string EvangelistDruidClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass7Align = "EvangelistDruidClass7Align";
        private static readonly string EvangelistDruidClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass8Align = "EvangelistDruidClass8Align";
        private static readonly string EvangelistDruidClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass9Align = "EvangelistDruidClass9Align";
        private static readonly string EvangelistDruidClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDruidClass10Align = "EvangelistDruidClass10Align";
        private static readonly string EvangelistDruidClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass0Align = "EvangelistDuelistClass0Align";
        private static readonly string EvangelistDuelistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDuelistClass0AlignDisplayName = "EvangelistDuelistClass0Align.Name";
        private const string EvangelistDuelistClass0AlignDescription = "EvangelistDuelistClass0Align.Description";
        private const string EvangelistDuelistClass2Align = "EvangelistDuelistClass2Align";
        private static readonly string EvangelistDuelistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass3Align = "EvangelistDuelistClass3Align";
        private static readonly string EvangelistDuelistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass4Align = "EvangelistDuelistClass4Align";
        private static readonly string EvangelistDuelistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass5Align = "EvangelistDuelistClass5Align";
        private static readonly string EvangelistDuelistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass6Align = "EvangelistDuelistClass6Align";
        private static readonly string EvangelistDuelistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass7Align = "EvangelistDuelistClass7Align";
        private static readonly string EvangelistDuelistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass8Align = "EvangelistDuelistClass8Align";
        private static readonly string EvangelistDuelistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass9Align = "EvangelistDuelistClass9Align";
        private static readonly string EvangelistDuelistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDuelistClass10Align = "EvangelistDuelistClass10Align";
        private static readonly string EvangelistDuelistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass0Align = "EvangelistEldritchKnightClass0Align";
        private static readonly string EvangelistEldritchKnightClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistEldritchKnightClass0AlignDisplayName = "EvangelistEldritchKnightClass0Align.Name";
        private const string EvangelistEldritchKnightClass0AlignDescription = "EvangelistEldritchKnightClass0Align.Description";
        private const string EvangelistEldritchKnightClass2Align = "EvangelistEldritchKnightClass2Align";
        private static readonly string EvangelistEldritchKnightClass2AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass3Align = "EvangelistEldritchKnightClass3Align";
        private static readonly string EvangelistEldritchKnightClass3AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass4Align = "EvangelistEldritchKnightClass4Align";
        private static readonly string EvangelistEldritchKnightClass4AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass5Align = "EvangelistEldritchKnightClass5Align";
        private static readonly string EvangelistEldritchKnightClass5AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass6Align = "EvangelistEldritchKnightClass6Align";
        private static readonly string EvangelistEldritchKnightClass6AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass7Align = "EvangelistEldritchKnightClass7Align";
        private static readonly string EvangelistEldritchKnightClass7AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass8Align = "EvangelistEldritchKnightClass8Align";
        private static readonly string EvangelistEldritchKnightClass8AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass9Align = "EvangelistEldritchKnightClass9Align";
        private static readonly string EvangelistEldritchKnightClass9AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchKnightClass10Align = "EvangelistEldritchKnightClass10Align";
        private static readonly string EvangelistEldritchKnightClass10AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass0Align = "EvangelistEldritchScionClass0Align";
        private static readonly string EvangelistEldritchScionClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistEldritchScionClass0AlignDisplayName = "EvangelistEldritchScionClass0Align.Name";
        private const string EvangelistEldritchScionClass0AlignDescription = "EvangelistEldritchScionClass0Align.Description";
        private const string EvangelistEldritchScionClass2Align = "EvangelistEldritchScionClass2Align";
        private static readonly string EvangelistEldritchScionClass2AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass3Align = "EvangelistEldritchScionClass3Align";
        private static readonly string EvangelistEldritchScionClass3AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass4Align = "EvangelistEldritchScionClass4Align";
        private static readonly string EvangelistEldritchScionClass4AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass5Align = "EvangelistEldritchScionClass5Align";
        private static readonly string EvangelistEldritchScionClass5AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass6Align = "EvangelistEldritchScionClass6Align";
        private static readonly string EvangelistEldritchScionClass6AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass7Align = "EvangelistEldritchScionClass7Align";
        private static readonly string EvangelistEldritchScionClass7AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass8Align = "EvangelistEldritchScionClass8Align";
        private static readonly string EvangelistEldritchScionClass8AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass9Align = "EvangelistEldritchScionClass9Align";
        private static readonly string EvangelistEldritchScionClass9AlignGuid = "replaceguidhere";
        private const string EvangelistEldritchScionClass10Align = "EvangelistEldritchScionClass10Align";
        private static readonly string EvangelistEldritchScionClass10AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass0Align = "EvangelistFighterClass0Align";
        private static readonly string EvangelistFighterClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistFighterClass0AlignDisplayName = "EvangelistFighterClass0Align.Name";
        private const string EvangelistFighterClass0AlignDescription = "EvangelistFighterClass0Align.Description";
        private const string EvangelistFighterClass2Align = "EvangelistFighterClass2Align";
        private static readonly string EvangelistFighterClass2AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass3Align = "EvangelistFighterClass3Align";
        private static readonly string EvangelistFighterClass3AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass4Align = "EvangelistFighterClass4Align";
        private static readonly string EvangelistFighterClass4AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass5Align = "EvangelistFighterClass5Align";
        private static readonly string EvangelistFighterClass5AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass6Align = "EvangelistFighterClass6Align";
        private static readonly string EvangelistFighterClass6AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass7Align = "EvangelistFighterClass7Align";
        private static readonly string EvangelistFighterClass7AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass8Align = "EvangelistFighterClass8Align";
        private static readonly string EvangelistFighterClass8AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass9Align = "EvangelistFighterClass9Align";
        private static readonly string EvangelistFighterClass9AlignGuid = "replaceguidhere";
        private const string EvangelistFighterClass10Align = "EvangelistFighterClass10Align";
        private static readonly string EvangelistFighterClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass0Align = "EvangelistHellknightClass0Align";
        private static readonly string EvangelistHellknightClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHellknightClass0AlignDisplayName = "EvangelistHellknightClass0Align.Name";
        private const string EvangelistHellknightClass0AlignDescription = "EvangelistHellknightClass0Align.Description";
        private const string EvangelistHellknightClass2Align = "EvangelistHellknightClass2Align";
        private static readonly string EvangelistHellknightClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass3Align = "EvangelistHellknightClass3Align";
        private static readonly string EvangelistHellknightClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass4Align = "EvangelistHellknightClass4Align";
        private static readonly string EvangelistHellknightClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass5Align = "EvangelistHellknightClass5Align";
        private static readonly string EvangelistHellknightClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass6Align = "EvangelistHellknightClass6Align";
        private static readonly string EvangelistHellknightClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass7Align = "EvangelistHellknightClass7Align";
        private static readonly string EvangelistHellknightClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass8Align = "EvangelistHellknightClass8Align";
        private static readonly string EvangelistHellknightClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass9Align = "EvangelistHellknightClass9Align";
        private static readonly string EvangelistHellknightClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightClass10Align = "EvangelistHellknightClass10Align";
        private static readonly string EvangelistHellknightClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass0Align = "EvangelistHellknightSigniferClass0Align";
        private static readonly string EvangelistHellknightSigniferClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHellknightSigniferClass0AlignDisplayName = "EvangelistHellknightSigniferClass0Align.Name";
        private const string EvangelistHellknightSigniferClass0AlignDescription = "EvangelistHellknightSigniferClass0Align.Description";
        private const string EvangelistHellknightSigniferClass2Align = "EvangelistHellknightSigniferClass2Align";
        private static readonly string EvangelistHellknightSigniferClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass3Align = "EvangelistHellknightSigniferClass3Align";
        private static readonly string EvangelistHellknightSigniferClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass4Align = "EvangelistHellknightSigniferClass4Align";
        private static readonly string EvangelistHellknightSigniferClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass5Align = "EvangelistHellknightSigniferClass5Align";
        private static readonly string EvangelistHellknightSigniferClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass6Align = "EvangelistHellknightSigniferClass6Align";
        private static readonly string EvangelistHellknightSigniferClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass7Align = "EvangelistHellknightSigniferClass7Align";
        private static readonly string EvangelistHellknightSigniferClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass8Align = "EvangelistHellknightSigniferClass8Align";
        private static readonly string EvangelistHellknightSigniferClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass9Align = "EvangelistHellknightSigniferClass9Align";
        private static readonly string EvangelistHellknightSigniferClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHellknightSigniferClass10Align = "EvangelistHellknightSigniferClass10Align";
        private static readonly string EvangelistHellknightSigniferClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass0Align = "EvangelistHunterClass0Align";
        private static readonly string EvangelistHunterClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHunterClass0AlignDisplayName = "EvangelistHunterClass0Align.Name";
        private const string EvangelistHunterClass0AlignDescription = "EvangelistHunterClass0Align.Description";
        private const string EvangelistHunterClass2Align = "EvangelistHunterClass2Align";
        private static readonly string EvangelistHunterClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass3Align = "EvangelistHunterClass3Align";
        private static readonly string EvangelistHunterClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass4Align = "EvangelistHunterClass4Align";
        private static readonly string EvangelistHunterClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass5Align = "EvangelistHunterClass5Align";
        private static readonly string EvangelistHunterClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass6Align = "EvangelistHunterClass6Align";
        private static readonly string EvangelistHunterClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass7Align = "EvangelistHunterClass7Align";
        private static readonly string EvangelistHunterClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass8Align = "EvangelistHunterClass8Align";
        private static readonly string EvangelistHunterClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass9Align = "EvangelistHunterClass9Align";
        private static readonly string EvangelistHunterClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHunterClass10Align = "EvangelistHunterClass10Align";
        private static readonly string EvangelistHunterClass10AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass0Align = "EvangelistInquisitorClass0Align";
        private static readonly string EvangelistInquisitorClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistInquisitorClass0AlignDisplayName = "EvangelistInquisitorClass0Align.Name";
        private const string EvangelistInquisitorClass0AlignDescription = "EvangelistInquisitorClass0Align.Description";
        private const string EvangelistInquisitorClass2Align = "EvangelistInquisitorClass2Align";
        private static readonly string EvangelistInquisitorClass2AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass3Align = "EvangelistInquisitorClass3Align";
        private static readonly string EvangelistInquisitorClass3AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass4Align = "EvangelistInquisitorClass4Align";
        private static readonly string EvangelistInquisitorClass4AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass5Align = "EvangelistInquisitorClass5Align";
        private static readonly string EvangelistInquisitorClass5AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass6Align = "EvangelistInquisitorClass6Align";
        private static readonly string EvangelistInquisitorClass6AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass7Align = "EvangelistInquisitorClass7Align";
        private static readonly string EvangelistInquisitorClass7AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass8Align = "EvangelistInquisitorClass8Align";
        private static readonly string EvangelistInquisitorClass8AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass9Align = "EvangelistInquisitorClass9Align";
        private static readonly string EvangelistInquisitorClass9AlignGuid = "replaceguidhere";
        private const string EvangelistInquisitorClass10Align = "EvangelistInquisitorClass10Align";
        private static readonly string EvangelistInquisitorClass10AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass0Align = "EvangelistKineticistClass0Align";
        private static readonly string EvangelistKineticistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistKineticistClass0AlignDisplayName = "EvangelistKineticistClass0Align.Name";
        private const string EvangelistKineticistClass0AlignDescription = "EvangelistKineticistClass0Align.Description";
        private const string EvangelistKineticistClass2Align = "EvangelistKineticistClass2Align";
        private static readonly string EvangelistKineticistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass3Align = "EvangelistKineticistClass3Align";
        private static readonly string EvangelistKineticistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass4Align = "EvangelistKineticistClass4Align";
        private static readonly string EvangelistKineticistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass5Align = "EvangelistKineticistClass5Align";
        private static readonly string EvangelistKineticistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass6Align = "EvangelistKineticistClass6Align";
        private static readonly string EvangelistKineticistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass7Align = "EvangelistKineticistClass7Align";
        private static readonly string EvangelistKineticistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass8Align = "EvangelistKineticistClass8Align";
        private static readonly string EvangelistKineticistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass9Align = "EvangelistKineticistClass9Align";
        private static readonly string EvangelistKineticistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistKineticistClass10Align = "EvangelistKineticistClass10Align";
        private static readonly string EvangelistKineticistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass0Align = "EvangelistLoremasterClass0Align";
        private static readonly string EvangelistLoremasterClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistLoremasterClass0AlignDisplayName = "EvangelistLoremasterClass0Align.Name";
        private const string EvangelistLoremasterClass0AlignDescription = "EvangelistLoremasterClass0Align.Description";
        private const string EvangelistLoremasterClass2Align = "EvangelistLoremasterClass2Align";
        private static readonly string EvangelistLoremasterClass2AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass3Align = "EvangelistLoremasterClass3Align";
        private static readonly string EvangelistLoremasterClass3AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass4Align = "EvangelistLoremasterClass4Align";
        private static readonly string EvangelistLoremasterClass4AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass5Align = "EvangelistLoremasterClass5Align";
        private static readonly string EvangelistLoremasterClass5AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass6Align = "EvangelistLoremasterClass6Align";
        private static readonly string EvangelistLoremasterClass6AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass7Align = "EvangelistLoremasterClass7Align";
        private static readonly string EvangelistLoremasterClass7AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass8Align = "EvangelistLoremasterClass8Align";
        private static readonly string EvangelistLoremasterClass8AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass9Align = "EvangelistLoremasterClass9Align";
        private static readonly string EvangelistLoremasterClass9AlignGuid = "replaceguidhere";
        private const string EvangelistLoremasterClass10Align = "EvangelistLoremasterClass10Align";
        private static readonly string EvangelistLoremasterClass10AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass0Align = "EvangelistMagusClass0Align";
        private static readonly string EvangelistMagusClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistMagusClass0AlignDisplayName = "EvangelistMagusClass0Align.Name";
        private const string EvangelistMagusClass0AlignDescription = "EvangelistMagusClass0Align.Description";
        private const string EvangelistMagusClass2Align = "EvangelistMagusClass2Align";
        private static readonly string EvangelistMagusClass2AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass3Align = "EvangelistMagusClass3Align";
        private static readonly string EvangelistMagusClass3AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass4Align = "EvangelistMagusClass4Align";
        private static readonly string EvangelistMagusClass4AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass5Align = "EvangelistMagusClass5Align";
        private static readonly string EvangelistMagusClass5AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass6Align = "EvangelistMagusClass6Align";
        private static readonly string EvangelistMagusClass6AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass7Align = "EvangelistMagusClass7Align";
        private static readonly string EvangelistMagusClass7AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass8Align = "EvangelistMagusClass8Align";
        private static readonly string EvangelistMagusClass8AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass9Align = "EvangelistMagusClass9Align";
        private static readonly string EvangelistMagusClass9AlignGuid = "replaceguidhere";
        private const string EvangelistMagusClass10Align = "EvangelistMagusClass10Align";
        private static readonly string EvangelistMagusClass10AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass0Align = "EvangelistMonkClass0Align";
        private static readonly string EvangelistMonkClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistMonkClass0AlignDisplayName = "EvangelistMonkClass0Align.Name";
        private const string EvangelistMonkClass0AlignDescription = "EvangelistMonkClass0Align.Description";
        private const string EvangelistMonkClass2Align = "EvangelistMonkClass2Align";
        private static readonly string EvangelistMonkClass2AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass3Align = "EvangelistMonkClass3Align";
        private static readonly string EvangelistMonkClass3AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass4Align = "EvangelistMonkClass4Align";
        private static readonly string EvangelistMonkClass4AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass5Align = "EvangelistMonkClass5Align";
        private static readonly string EvangelistMonkClass5AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass6Align = "EvangelistMonkClass6Align";
        private static readonly string EvangelistMonkClass6AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass7Align = "EvangelistMonkClass7Align";
        private static readonly string EvangelistMonkClass7AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass8Align = "EvangelistMonkClass8Align";
        private static readonly string EvangelistMonkClass8AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass9Align = "EvangelistMonkClass9Align";
        private static readonly string EvangelistMonkClass9AlignGuid = "replaceguidhere";
        private const string EvangelistMonkClass10Align = "EvangelistMonkClass10Align";
        private static readonly string EvangelistMonkClass10AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass0Align = "EvangelistMysticTheurgeClass0Align";
        private static readonly string EvangelistMysticTheurgeClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistMysticTheurgeClass0AlignDisplayName = "EvangelistMysticTheurgeClass0Align.Name";
        private const string EvangelistMysticTheurgeClass0AlignDescription = "EvangelistMysticTheurgeClass0Align.Description";
        private const string EvangelistMysticTheurgeClass2Align = "EvangelistMysticTheurgeClass2Align";
        private static readonly string EvangelistMysticTheurgeClass2AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass3Align = "EvangelistMysticTheurgeClass3Align";
        private static readonly string EvangelistMysticTheurgeClass3AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass4Align = "EvangelistMysticTheurgeClass4Align";
        private static readonly string EvangelistMysticTheurgeClass4AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass5Align = "EvangelistMysticTheurgeClass5Align";
        private static readonly string EvangelistMysticTheurgeClass5AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass6Align = "EvangelistMysticTheurgeClass6Align";
        private static readonly string EvangelistMysticTheurgeClass6AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass7Align = "EvangelistMysticTheurgeClass7Align";
        private static readonly string EvangelistMysticTheurgeClass7AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass8Align = "EvangelistMysticTheurgeClass8Align";
        private static readonly string EvangelistMysticTheurgeClass8AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass9Align = "EvangelistMysticTheurgeClass9Align";
        private static readonly string EvangelistMysticTheurgeClass9AlignGuid = "replaceguidhere";
        private const string EvangelistMysticTheurgeClass10Align = "EvangelistMysticTheurgeClass10Align";
        private static readonly string EvangelistMysticTheurgeClass10AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass0Align = "EvangelistOracleClass0Align";
        private static readonly string EvangelistOracleClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistOracleClass0AlignDisplayName = "EvangelistOracleClass0Align.Name";
        private const string EvangelistOracleClass0AlignDescription = "EvangelistOracleClass0Align.Description";
        private const string EvangelistOracleClass2Align = "EvangelistOracleClass2Align";
        private static readonly string EvangelistOracleClass2AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass3Align = "EvangelistOracleClass3Align";
        private static readonly string EvangelistOracleClass3AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass4Align = "EvangelistOracleClass4Align";
        private static readonly string EvangelistOracleClass4AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass5Align = "EvangelistOracleClass5Align";
        private static readonly string EvangelistOracleClass5AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass6Align = "EvangelistOracleClass6Align";
        private static readonly string EvangelistOracleClass6AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass7Align = "EvangelistOracleClass7Align";
        private static readonly string EvangelistOracleClass7AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass8Align = "EvangelistOracleClass8Align";
        private static readonly string EvangelistOracleClass8AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass9Align = "EvangelistOracleClass9Align";
        private static readonly string EvangelistOracleClass9AlignGuid = "replaceguidhere";
        private const string EvangelistOracleClass10Align = "EvangelistOracleClass10Align";
        private static readonly string EvangelistOracleClass10AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass0Align = "EvangelistPaladinClass0Align";
        private static readonly string EvangelistPaladinClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistPaladinClass0AlignDisplayName = "EvangelistPaladinClass0Align.Name";
        private const string EvangelistPaladinClass0AlignDescription = "EvangelistPaladinClass0Align.Description";
        private const string EvangelistPaladinClass2Align = "EvangelistPaladinClass2Align";
        private static readonly string EvangelistPaladinClass2AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass3Align = "EvangelistPaladinClass3Align";
        private static readonly string EvangelistPaladinClass3AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass4Align = "EvangelistPaladinClass4Align";
        private static readonly string EvangelistPaladinClass4AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass5Align = "EvangelistPaladinClass5Align";
        private static readonly string EvangelistPaladinClass5AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass6Align = "EvangelistPaladinClass6Align";
        private static readonly string EvangelistPaladinClass6AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass7Align = "EvangelistPaladinClass7Align";
        private static readonly string EvangelistPaladinClass7AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass8Align = "EvangelistPaladinClass8Align";
        private static readonly string EvangelistPaladinClass8AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass9Align = "EvangelistPaladinClass9Align";
        private static readonly string EvangelistPaladinClass9AlignGuid = "replaceguidhere";
        private const string EvangelistPaladinClass10Align = "EvangelistPaladinClass10Align";
        private static readonly string EvangelistPaladinClass10AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass0Align = "EvangelistRangerClass0Align";
        private static readonly string EvangelistRangerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistRangerClass0AlignDisplayName = "EvangelistRangerClass0Align.Name";
        private const string EvangelistRangerClass0AlignDescription = "EvangelistRangerClass0Align.Description";
        private const string EvangelistRangerClass2Align = "EvangelistRangerClass2Align";
        private static readonly string EvangelistRangerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass3Align = "EvangelistRangerClass3Align";
        private static readonly string EvangelistRangerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass4Align = "EvangelistRangerClass4Align";
        private static readonly string EvangelistRangerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass5Align = "EvangelistRangerClass5Align";
        private static readonly string EvangelistRangerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass6Align = "EvangelistRangerClass6Align";
        private static readonly string EvangelistRangerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass7Align = "EvangelistRangerClass7Align";
        private static readonly string EvangelistRangerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass8Align = "EvangelistRangerClass8Align";
        private static readonly string EvangelistRangerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass9Align = "EvangelistRangerClass9Align";
        private static readonly string EvangelistRangerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistRangerClass10Align = "EvangelistRangerClass10Align";
        private static readonly string EvangelistRangerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass0Align = "EvangelistRogueClass0Align";
        private static readonly string EvangelistRogueClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistRogueClass0AlignDisplayName = "EvangelistRogueClass0Align.Name";
        private const string EvangelistRogueClass0AlignDescription = "EvangelistRogueClass0Align.Description";
        private const string EvangelistRogueClass2Align = "EvangelistRogueClass2Align";
        private static readonly string EvangelistRogueClass2AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass3Align = "EvangelistRogueClass3Align";
        private static readonly string EvangelistRogueClass3AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass4Align = "EvangelistRogueClass4Align";
        private static readonly string EvangelistRogueClass4AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass5Align = "EvangelistRogueClass5Align";
        private static readonly string EvangelistRogueClass5AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass6Align = "EvangelistRogueClass6Align";
        private static readonly string EvangelistRogueClass6AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass7Align = "EvangelistRogueClass7Align";
        private static readonly string EvangelistRogueClass7AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass8Align = "EvangelistRogueClass8Align";
        private static readonly string EvangelistRogueClass8AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass9Align = "EvangelistRogueClass9Align";
        private static readonly string EvangelistRogueClass9AlignGuid = "replaceguidhere";
        private const string EvangelistRogueClass10Align = "EvangelistRogueClass10Align";
        private static readonly string EvangelistRogueClass10AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass0Align = "EvangelistShamanClass0Align";
        private static readonly string EvangelistShamanClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistShamanClass0AlignDisplayName = "EvangelistShamanClass0Align.Name";
        private const string EvangelistShamanClass0AlignDescription = "EvangelistShamanClass0Align.Description";
        private const string EvangelistShamanClass2Align = "EvangelistShamanClass2Align";
        private static readonly string EvangelistShamanClass2AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass3Align = "EvangelistShamanClass3Align";
        private static readonly string EvangelistShamanClass3AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass4Align = "EvangelistShamanClass4Align";
        private static readonly string EvangelistShamanClass4AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass5Align = "EvangelistShamanClass5Align";
        private static readonly string EvangelistShamanClass5AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass6Align = "EvangelistShamanClass6Align";
        private static readonly string EvangelistShamanClass6AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass7Align = "EvangelistShamanClass7Align";
        private static readonly string EvangelistShamanClass7AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass8Align = "EvangelistShamanClass8Align";
        private static readonly string EvangelistShamanClass8AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass9Align = "EvangelistShamanClass9Align";
        private static readonly string EvangelistShamanClass9AlignGuid = "replaceguidhere";
        private const string EvangelistShamanClass10Align = "EvangelistShamanClass10Align";
        private static readonly string EvangelistShamanClass10AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass0Align = "EvangelistShifterClass0Align";
        private static readonly string EvangelistShifterClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistShifterClass0AlignDisplayName = "EvangelistShifterClass0Align.Name";
        private const string EvangelistShifterClass0AlignDescription = "EvangelistShifterClass0Align.Description";
        private const string EvangelistShifterClass2Align = "EvangelistShifterClass2Align";
        private static readonly string EvangelistShifterClass2AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass3Align = "EvangelistShifterClass3Align";
        private static readonly string EvangelistShifterClass3AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass4Align = "EvangelistShifterClass4Align";
        private static readonly string EvangelistShifterClass4AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass5Align = "EvangelistShifterClass5Align";
        private static readonly string EvangelistShifterClass5AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass6Align = "EvangelistShifterClass6Align";
        private static readonly string EvangelistShifterClass6AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass7Align = "EvangelistShifterClass7Align";
        private static readonly string EvangelistShifterClass7AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass8Align = "EvangelistShifterClass8Align";
        private static readonly string EvangelistShifterClass8AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass9Align = "EvangelistShifterClass9Align";
        private static readonly string EvangelistShifterClass9AlignGuid = "replaceguidhere";
        private const string EvangelistShifterClass10Align = "EvangelistShifterClass10Align";
        private static readonly string EvangelistShifterClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass0Align = "EvangelistSkaldClass0Align";
        private static readonly string EvangelistSkaldClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSkaldClass0AlignDisplayName = "EvangelistSkaldClass0Align.Name";
        private const string EvangelistSkaldClass0AlignDescription = "EvangelistSkaldClass0Align.Description";
        private const string EvangelistSkaldClass2Align = "EvangelistSkaldClass2Align";
        private static readonly string EvangelistSkaldClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass3Align = "EvangelistSkaldClass3Align";
        private static readonly string EvangelistSkaldClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass4Align = "EvangelistSkaldClass4Align";
        private static readonly string EvangelistSkaldClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass5Align = "EvangelistSkaldClass5Align";
        private static readonly string EvangelistSkaldClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass6Align = "EvangelistSkaldClass6Align";
        private static readonly string EvangelistSkaldClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass7Align = "EvangelistSkaldClass7Align";
        private static readonly string EvangelistSkaldClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass8Align = "EvangelistSkaldClass8Align";
        private static readonly string EvangelistSkaldClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass9Align = "EvangelistSkaldClass9Align";
        private static readonly string EvangelistSkaldClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSkaldClass10Align = "EvangelistSkaldClass10Align";
        private static readonly string EvangelistSkaldClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass0Align = "EvangelistSlayerClass0Align";
        private static readonly string EvangelistSlayerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSlayerClass0AlignDisplayName = "EvangelistSlayerClass0Align.Name";
        private const string EvangelistSlayerClass0AlignDescription = "EvangelistSlayerClass0Align.Description";
        private const string EvangelistSlayerClass2Align = "EvangelistSlayerClass2Align";
        private static readonly string EvangelistSlayerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass3Align = "EvangelistSlayerClass3Align";
        private static readonly string EvangelistSlayerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass4Align = "EvangelistSlayerClass4Align";
        private static readonly string EvangelistSlayerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass5Align = "EvangelistSlayerClass5Align";
        private static readonly string EvangelistSlayerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass6Align = "EvangelistSlayerClass6Align";
        private static readonly string EvangelistSlayerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass7Align = "EvangelistSlayerClass7Align";
        private static readonly string EvangelistSlayerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass8Align = "EvangelistSlayerClass8Align";
        private static readonly string EvangelistSlayerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass9Align = "EvangelistSlayerClass9Align";
        private static readonly string EvangelistSlayerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSlayerClass10Align = "EvangelistSlayerClass10Align";
        private static readonly string EvangelistSlayerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass0Align = "EvangelistSorcererClass0Align";
        private static readonly string EvangelistSorcererClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSorcererClass0AlignDisplayName = "EvangelistSorcererClass0Align.Name";
        private const string EvangelistSorcererClass0AlignDescription = "EvangelistSorcererClass0Align.Description";
        private const string EvangelistSorcererClass2Align = "EvangelistSorcererClass2Align";
        private static readonly string EvangelistSorcererClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass3Align = "EvangelistSorcererClass3Align";
        private static readonly string EvangelistSorcererClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass4Align = "EvangelistSorcererClass4Align";
        private static readonly string EvangelistSorcererClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass5Align = "EvangelistSorcererClass5Align";
        private static readonly string EvangelistSorcererClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass6Align = "EvangelistSorcererClass6Align";
        private static readonly string EvangelistSorcererClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass7Align = "EvangelistSorcererClass7Align";
        private static readonly string EvangelistSorcererClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass8Align = "EvangelistSorcererClass8Align";
        private static readonly string EvangelistSorcererClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass9Align = "EvangelistSorcererClass9Align";
        private static readonly string EvangelistSorcererClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSorcererClass10Align = "EvangelistSorcererClass10Align";
        private static readonly string EvangelistSorcererClass10AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass0Align = "EvangelistStalwartDefenderClass0Align";
        private static readonly string EvangelistStalwartDefenderClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistStalwartDefenderClass0AlignDisplayName = "EvangelistStalwartDefenderClass0Align.Name";
        private const string EvangelistStalwartDefenderClass0AlignDescription = "EvangelistStalwartDefenderClass0Align.Description";
        private const string EvangelistStalwartDefenderClass2Align = "EvangelistStalwartDefenderClass2Align";
        private static readonly string EvangelistStalwartDefenderClass2AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass3Align = "EvangelistStalwartDefenderClass3Align";
        private static readonly string EvangelistStalwartDefenderClass3AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass4Align = "EvangelistStalwartDefenderClass4Align";
        private static readonly string EvangelistStalwartDefenderClass4AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass5Align = "EvangelistStalwartDefenderClass5Align";
        private static readonly string EvangelistStalwartDefenderClass5AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass6Align = "EvangelistStalwartDefenderClass6Align";
        private static readonly string EvangelistStalwartDefenderClass6AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass7Align = "EvangelistStalwartDefenderClass7Align";
        private static readonly string EvangelistStalwartDefenderClass7AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass8Align = "EvangelistStalwartDefenderClass8Align";
        private static readonly string EvangelistStalwartDefenderClass8AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass9Align = "EvangelistStalwartDefenderClass9Align";
        private static readonly string EvangelistStalwartDefenderClass9AlignGuid = "replaceguidhere";
        private const string EvangelistStalwartDefenderClass10Align = "EvangelistStalwartDefenderClass10Align";
        private static readonly string EvangelistStalwartDefenderClass10AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass0Align = "EvangelistStudentOfWarClass0Align";
        private static readonly string EvangelistStudentOfWarClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistStudentOfWarClass0AlignDisplayName = "EvangelistStudentOfWarClass0Align.Name";
        private const string EvangelistStudentOfWarClass0AlignDescription = "EvangelistStudentOfWarClass0Align.Description";
        private const string EvangelistStudentOfWarClass2Align = "EvangelistStudentOfWarClass2Align";
        private static readonly string EvangelistStudentOfWarClass2AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass3Align = "EvangelistStudentOfWarClass3Align";
        private static readonly string EvangelistStudentOfWarClass3AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass4Align = "EvangelistStudentOfWarClass4Align";
        private static readonly string EvangelistStudentOfWarClass4AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass5Align = "EvangelistStudentOfWarClass5Align";
        private static readonly string EvangelistStudentOfWarClass5AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass6Align = "EvangelistStudentOfWarClass6Align";
        private static readonly string EvangelistStudentOfWarClass6AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass7Align = "EvangelistStudentOfWarClass7Align";
        private static readonly string EvangelistStudentOfWarClass7AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass8Align = "EvangelistStudentOfWarClass8Align";
        private static readonly string EvangelistStudentOfWarClass8AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass9Align = "EvangelistStudentOfWarClass9Align";
        private static readonly string EvangelistStudentOfWarClass9AlignGuid = "replaceguidhere";
        private const string EvangelistStudentOfWarClass10Align = "EvangelistStudentOfWarClass10Align";
        private static readonly string EvangelistStudentOfWarClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass0Align = "EvangelistSwordlordClass0Align";
        private static readonly string EvangelistSwordlordClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSwordlordClass0AlignDisplayName = "EvangelistSwordlordClass0Align.Name";
        private const string EvangelistSwordlordClass0AlignDescription = "EvangelistSwordlordClass0Align.Description";
        private const string EvangelistSwordlordClass2Align = "EvangelistSwordlordClass2Align";
        private static readonly string EvangelistSwordlordClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass3Align = "EvangelistSwordlordClass3Align";
        private static readonly string EvangelistSwordlordClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass4Align = "EvangelistSwordlordClass4Align";
        private static readonly string EvangelistSwordlordClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass5Align = "EvangelistSwordlordClass5Align";
        private static readonly string EvangelistSwordlordClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass6Align = "EvangelistSwordlordClass6Align";
        private static readonly string EvangelistSwordlordClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass7Align = "EvangelistSwordlordClass7Align";
        private static readonly string EvangelistSwordlordClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass8Align = "EvangelistSwordlordClass8Align";
        private static readonly string EvangelistSwordlordClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass9Align = "EvangelistSwordlordClass9Align";
        private static readonly string EvangelistSwordlordClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSwordlordClass10Align = "EvangelistSwordlordClass10Align";
        private static readonly string EvangelistSwordlordClass10AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass0Align = "EvangelistWarpriestClass0Align";
        private static readonly string EvangelistWarpriestClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistWarpriestClass0AlignDisplayName = "EvangelistWarpriestClass0Align.Name";
        private const string EvangelistWarpriestClass0AlignDescription = "EvangelistWarpriestClass0Align.Description";
        private const string EvangelistWarpriestClass2Align = "EvangelistWarpriestClass2Align";
        private static readonly string EvangelistWarpriestClass2AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass3Align = "EvangelistWarpriestClass3Align";
        private static readonly string EvangelistWarpriestClass3AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass4Align = "EvangelistWarpriestClass4Align";
        private static readonly string EvangelistWarpriestClass4AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass5Align = "EvangelistWarpriestClass5Align";
        private static readonly string EvangelistWarpriestClass5AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass6Align = "EvangelistWarpriestClass6Align";
        private static readonly string EvangelistWarpriestClass6AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass7Align = "EvangelistWarpriestClass7Align";
        private static readonly string EvangelistWarpriestClass7AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass8Align = "EvangelistWarpriestClass8Align";
        private static readonly string EvangelistWarpriestClass8AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass9Align = "EvangelistWarpriestClass9Align";
        private static readonly string EvangelistWarpriestClass9AlignGuid = "replaceguidhere";
        private const string EvangelistWarpriestClass10Align = "EvangelistWarpriestClass10Align";
        private static readonly string EvangelistWarpriestClass10AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass0Align = "EvangelistWinterWitchClass0Align";
        private static readonly string EvangelistWinterWitchClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistWinterWitchClass0AlignDisplayName = "EvangelistWinterWitchClass0Align.Name";
        private const string EvangelistWinterWitchClass0AlignDescription = "EvangelistWinterWitchClass0Align.Description";
        private const string EvangelistWinterWitchClass2Align = "EvangelistWinterWitchClass2Align";
        private static readonly string EvangelistWinterWitchClass2AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass3Align = "EvangelistWinterWitchClass3Align";
        private static readonly string EvangelistWinterWitchClass3AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass4Align = "EvangelistWinterWitchClass4Align";
        private static readonly string EvangelistWinterWitchClass4AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass5Align = "EvangelistWinterWitchClass5Align";
        private static readonly string EvangelistWinterWitchClass5AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass6Align = "EvangelistWinterWitchClass6Align";
        private static readonly string EvangelistWinterWitchClass6AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass7Align = "EvangelistWinterWitchClass7Align";
        private static readonly string EvangelistWinterWitchClass7AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass8Align = "EvangelistWinterWitchClass8Align";
        private static readonly string EvangelistWinterWitchClass8AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass9Align = "EvangelistWinterWitchClass9Align";
        private static readonly string EvangelistWinterWitchClass9AlignGuid = "replaceguidhere";
        private const string EvangelistWinterWitchClass10Align = "EvangelistWinterWitchClass10Align";
        private static readonly string EvangelistWinterWitchClass10AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass0Align = "EvangelistWitchClass0Align";
        private static readonly string EvangelistWitchClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistWitchClass0AlignDisplayName = "EvangelistWitchClass0Align.Name";
        private const string EvangelistWitchClass0AlignDescription = "EvangelistWitchClass0Align.Description";
        private const string EvangelistWitchClass2Align = "EvangelistWitchClass2Align";
        private static readonly string EvangelistWitchClass2AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass3Align = "EvangelistWitchClass3Align";
        private static readonly string EvangelistWitchClass3AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass4Align = "EvangelistWitchClass4Align";
        private static readonly string EvangelistWitchClass4AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass5Align = "EvangelistWitchClass5Align";
        private static readonly string EvangelistWitchClass5AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass6Align = "EvangelistWitchClass6Align";
        private static readonly string EvangelistWitchClass6AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass7Align = "EvangelistWitchClass7Align";
        private static readonly string EvangelistWitchClass7AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass8Align = "EvangelistWitchClass8Align";
        private static readonly string EvangelistWitchClass8AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass9Align = "EvangelistWitchClass9Align";
        private static readonly string EvangelistWitchClass9AlignGuid = "replaceguidhere";
        private const string EvangelistWitchClass10Align = "EvangelistWitchClass10Align";
        private static readonly string EvangelistWitchClass10AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass0Align = "EvangelistWizardClass0Align";
        private static readonly string EvangelistWizardClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistWizardClass0AlignDisplayName = "EvangelistWizardClass0Align.Name";
        private const string EvangelistWizardClass0AlignDescription = "EvangelistWizardClass0Align.Description";
        private const string EvangelistWizardClass2Align = "EvangelistWizardClass2Align";
        private static readonly string EvangelistWizardClass2AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass3Align = "EvangelistWizardClass3Align";
        private static readonly string EvangelistWizardClass3AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass4Align = "EvangelistWizardClass4Align";
        private static readonly string EvangelistWizardClass4AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass5Align = "EvangelistWizardClass5Align";
        private static readonly string EvangelistWizardClass5AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass6Align = "EvangelistWizardClass6Align";
        private static readonly string EvangelistWizardClass6AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass7Align = "EvangelistWizardClass7Align";
        private static readonly string EvangelistWizardClass7AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass8Align = "EvangelistWizardClass8Align";
        private static readonly string EvangelistWizardClass8AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass9Align = "EvangelistWizardClass9Align";
        private static readonly string EvangelistWizardClass9AlignGuid = "replaceguidhere";
        private const string EvangelistWizardClass10Align = "EvangelistWizardClass10Align";
        private static readonly string EvangelistWizardClass10AlignGuid = "replaceguidhere";

        private const string EvangelistGunslingerClass0Align = "EvangelistGunslingerClass0Align";
        private static readonly string EvangelistGunslingerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistGunslingerClass0AlignDisplayName = "EvangelistGunslingerClass0Align.Name";
        private const string EvangelistGunslingerClass0AlignDescription = "EvangelistGunslingerClass0Align.Description";
        private const string EvangelistGunslingerClass2Align = "EvangelistGunslingerClass2Align";
        private static readonly string EvangelistGunslingerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass3Align = "EvangelistGunslingerClass3Align";
        private static readonly string EvangelistGunslingerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass4Align = "EvangelistGunslingerClass4Align";
        private static readonly string EvangelistGunslingerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass5Align = "EvangelistGunslingerClass5Align";
        private static readonly string EvangelistGunslingerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass6Align = "EvangelistGunslingerClass6Align";
        private static readonly string EvangelistGunslingerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass7Align = "EvangelistGunslingerClass7Align";
        private static readonly string EvangelistGunslingerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass8Align = "EvangelistGunslingerClass8Align";
        private static readonly string EvangelistGunslingerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass9Align = "EvangelistGunslingerClass9Align";
        private static readonly string EvangelistGunslingerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistGunslingerClass10Align = "EvangelistGunslingerClass10Align";
        private static readonly string EvangelistGunslingerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass0Align = "EvangelistAgentoftheGraveClass0Align";
        private static readonly string EvangelistAgentoftheGraveClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistAgentoftheGraveClass0AlignDisplayName = "EvangelistAgentoftheGraveClass0Align.Name";
        private const string EvangelistAgentoftheGraveClass0AlignDescription = "EvangelistAgentoftheGraveClass0Align.Description";
        private const string EvangelistAgentoftheGraveClass2Align = "EvangelistAgentoftheGraveClass2Align";
        private static readonly string EvangelistAgentoftheGraveClass2AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass3Align = "EvangelistAgentoftheGraveClass3Align";
        private static readonly string EvangelistAgentoftheGraveClass3AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass4Align = "EvangelistAgentoftheGraveClass4Align";
        private static readonly string EvangelistAgentoftheGraveClass4AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass5Align = "EvangelistAgentoftheGraveClass5Align";
        private static readonly string EvangelistAgentoftheGraveClass5AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass6Align = "EvangelistAgentoftheGraveClass6Align";
        private static readonly string EvangelistAgentoftheGraveClass6AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass7Align = "EvangelistAgentoftheGraveClass7Align";
        private static readonly string EvangelistAgentoftheGraveClass7AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass8Align = "EvangelistAgentoftheGraveClass8Align";
        private static readonly string EvangelistAgentoftheGraveClass8AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass9Align = "EvangelistAgentoftheGraveClass9Align";
        private static readonly string EvangelistAgentoftheGraveClass9AlignGuid = "replaceguidhere";
        private const string EvangelistAgentoftheGraveClass10Align = "EvangelistAgentoftheGraveClass10Align";
        private static readonly string EvangelistAgentoftheGraveClass10AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass0Align = "EvangelistAnchoriteofDawnClass0Align";
        private static readonly string EvangelistAnchoriteofDawnClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistAnchoriteofDawnClass0AlignDisplayName = "EvangelistAnchoriteofDawnClass0Align.Name";
        private const string EvangelistAnchoriteofDawnClass0AlignDescription = "EvangelistAnchoriteofDawnClass0Align.Description";
        private const string EvangelistAnchoriteofDawnClass2Align = "EvangelistAnchoriteofDawnClass2Align";
        private static readonly string EvangelistAnchoriteofDawnClass2AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass3Align = "EvangelistAnchoriteofDawnClass3Align";
        private static readonly string EvangelistAnchoriteofDawnClass3AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass4Align = "EvangelistAnchoriteofDawnClass4Align";
        private static readonly string EvangelistAnchoriteofDawnClass4AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass5Align = "EvangelistAnchoriteofDawnClass5Align";
        private static readonly string EvangelistAnchoriteofDawnClass5AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass6Align = "EvangelistAnchoriteofDawnClass6Align";
        private static readonly string EvangelistAnchoriteofDawnClass6AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass7Align = "EvangelistAnchoriteofDawnClass7Align";
        private static readonly string EvangelistAnchoriteofDawnClass7AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass8Align = "EvangelistAnchoriteofDawnClass8Align";
        private static readonly string EvangelistAnchoriteofDawnClass8AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass9Align = "EvangelistAnchoriteofDawnClass9Align";
        private static readonly string EvangelistAnchoriteofDawnClass9AlignGuid = "replaceguidhere";
        private const string EvangelistAnchoriteofDawnClass10Align = "EvangelistAnchoriteofDawnClass10Align";
        private static readonly string EvangelistAnchoriteofDawnClass10AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass0Align = "EvangelistArcaneAcherClass0Align";
        private static readonly string EvangelistArcaneAcherClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistArcaneAcherClass0AlignDisplayName = "EvangelistArcaneAcherClass0Align.Name";
        private const string EvangelistArcaneAcherClass0AlignDescription = "EvangelistArcaneAcherClass0Align.Description";
        private const string EvangelistArcaneAcherClass2Align = "EvangelistArcaneAcherClass2Align";
        private static readonly string EvangelistArcaneAcherClass2AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass3Align = "EvangelistArcaneAcherClass3Align";
        private static readonly string EvangelistArcaneAcherClass3AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass4Align = "EvangelistArcaneAcherClass4Align";
        private static readonly string EvangelistArcaneAcherClass4AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass5Align = "EvangelistArcaneAcherClass5Align";
        private static readonly string EvangelistArcaneAcherClass5AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass6Align = "EvangelistArcaneAcherClass6Align";
        private static readonly string EvangelistArcaneAcherClass6AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass7Align = "EvangelistArcaneAcherClass7Align";
        private static readonly string EvangelistArcaneAcherClass7AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass8Align = "EvangelistArcaneAcherClass8Align";
        private static readonly string EvangelistArcaneAcherClass8AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass9Align = "EvangelistArcaneAcherClass9Align";
        private static readonly string EvangelistArcaneAcherClass9AlignGuid = "replaceguidhere";
        private const string EvangelistArcaneAcherClass10Align = "EvangelistArcaneAcherClass10Align";
        private static readonly string EvangelistArcaneAcherClass10AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass0Align = "EvangelistAsavirClass0Align";
        private static readonly string EvangelistAsavirClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistAsavirClass0AlignDisplayName = "EvangelistAsavirClass0Align.Name";
        private const string EvangelistAsavirClass0AlignDescription = "EvangelistAsavirClass0Align.Description";
        private const string EvangelistAsavirClass2Align = "EvangelistAsavirClass2Align";
        private static readonly string EvangelistAsavirClass2AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass3Align = "EvangelistAsavirClass3Align";
        private static readonly string EvangelistAsavirClass3AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass4Align = "EvangelistAsavirClass4Align";
        private static readonly string EvangelistAsavirClass4AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass5Align = "EvangelistAsavirClass5Align";
        private static readonly string EvangelistAsavirClass5AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass6Align = "EvangelistAsavirClass6Align";
        private static readonly string EvangelistAsavirClass6AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass7Align = "EvangelistAsavirClass7Align";
        private static readonly string EvangelistAsavirClass7AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass8Align = "EvangelistAsavirClass8Align";
        private static readonly string EvangelistAsavirClass8AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass9Align = "EvangelistAsavirClass9Align";
        private static readonly string EvangelistAsavirClass9AlignGuid = "replaceguidhere";
        private const string EvangelistAsavirClass10Align = "EvangelistAsavirClass10Align";
        private static readonly string EvangelistAsavirClass10AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass0Align = "EvangelistChevalierClass0Align";
        private static readonly string EvangelistChevalierClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistChevalierClass0AlignDisplayName = "EvangelistChevalierClass0Align.Name";
        private const string EvangelistChevalierClass0AlignDescription = "EvangelistChevalierClass0Align.Description";
        private const string EvangelistChevalierClass2Align = "EvangelistChevalierClass2Align";
        private static readonly string EvangelistChevalierClass2AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass3Align = "EvangelistChevalierClass3Align";
        private static readonly string EvangelistChevalierClass3AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass4Align = "EvangelistChevalierClass4Align";
        private static readonly string EvangelistChevalierClass4AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass5Align = "EvangelistChevalierClass5Align";
        private static readonly string EvangelistChevalierClass5AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass6Align = "EvangelistChevalierClass6Align";
        private static readonly string EvangelistChevalierClass6AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass7Align = "EvangelistChevalierClass7Align";
        private static readonly string EvangelistChevalierClass7AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass8Align = "EvangelistChevalierClass8Align";
        private static readonly string EvangelistChevalierClass8AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass9Align = "EvangelistChevalierClass9Align";
        private static readonly string EvangelistChevalierClass9AlignGuid = "replaceguidhere";
        private const string EvangelistChevalierClass10Align = "EvangelistChevalierClass10Align";
        private static readonly string EvangelistChevalierClass10AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass0Align = "EvangelistCrimsonTemplarClass0Align";
        private static readonly string EvangelistCrimsonTemplarClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistCrimsonTemplarClass0AlignDisplayName = "EvangelistCrimsonTemplarClass0Align.Name";
        private const string EvangelistCrimsonTemplarClass0AlignDescription = "EvangelistCrimsonTemplarClass0Align.Description";
        private const string EvangelistCrimsonTemplarClass2Align = "EvangelistCrimsonTemplarClass2Align";
        private static readonly string EvangelistCrimsonTemplarClass2AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass3Align = "EvangelistCrimsonTemplarClass3Align";
        private static readonly string EvangelistCrimsonTemplarClass3AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass4Align = "EvangelistCrimsonTemplarClass4Align";
        private static readonly string EvangelistCrimsonTemplarClass4AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass5Align = "EvangelistCrimsonTemplarClass5Align";
        private static readonly string EvangelistCrimsonTemplarClass5AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass6Align = "EvangelistCrimsonTemplarClass6Align";
        private static readonly string EvangelistCrimsonTemplarClass6AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass7Align = "EvangelistCrimsonTemplarClass7Align";
        private static readonly string EvangelistCrimsonTemplarClass7AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass8Align = "EvangelistCrimsonTemplarClass8Align";
        private static readonly string EvangelistCrimsonTemplarClass8AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass9Align = "EvangelistCrimsonTemplarClass9Align";
        private static readonly string EvangelistCrimsonTemplarClass9AlignGuid = "replaceguidhere";
        private const string EvangelistCrimsonTemplarClass10Align = "EvangelistCrimsonTemplarClass10Align";
        private static readonly string EvangelistCrimsonTemplarClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass0Align = "EvangelistDeadeyeDevoteeClass0Align";
        private static readonly string EvangelistDeadeyeDevoteeClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDeadeyeDevoteeClass0AlignDisplayName = "EvangelistDeadeyeDevoteeClass0Align.Name";
        private const string EvangelistDeadeyeDevoteeClass0AlignDescription = "EvangelistDeadeyeDevoteeClass0Align.Description";
        private const string EvangelistDeadeyeDevoteeClass2Align = "EvangelistDeadeyeDevoteeClass2Align";
        private static readonly string EvangelistDeadeyeDevoteeClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass3Align = "EvangelistDeadeyeDevoteeClass3Align";
        private static readonly string EvangelistDeadeyeDevoteeClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass4Align = "EvangelistDeadeyeDevoteeClass4Align";
        private static readonly string EvangelistDeadeyeDevoteeClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass5Align = "EvangelistDeadeyeDevoteeClass5Align";
        private static readonly string EvangelistDeadeyeDevoteeClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass6Align = "EvangelistDeadeyeDevoteeClass6Align";
        private static readonly string EvangelistDeadeyeDevoteeClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass7Align = "EvangelistDeadeyeDevoteeClass7Align";
        private static readonly string EvangelistDeadeyeDevoteeClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass8Align = "EvangelistDeadeyeDevoteeClass8Align";
        private static readonly string EvangelistDeadeyeDevoteeClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass9Align = "EvangelistDeadeyeDevoteeClass9Align";
        private static readonly string EvangelistDeadeyeDevoteeClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDeadeyeDevoteeClass10Align = "EvangelistDeadeyeDevoteeClass10Align";
        private static readonly string EvangelistDeadeyeDevoteeClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass0Align = "EvangelistDragonFuryClass0Align";
        private static readonly string EvangelistDragonFuryClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDragonFuryClass0AlignDisplayName = "EvangelistDragonFuryClass0Align.Name";
        private const string EvangelistDragonFuryClass0AlignDescription = "EvangelistDragonFuryClass0Align.Description";
        private const string EvangelistDragonFuryClass2Align = "EvangelistDragonFuryClass2Align";
        private static readonly string EvangelistDragonFuryClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass3Align = "EvangelistDragonFuryClass3Align";
        private static readonly string EvangelistDragonFuryClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass4Align = "EvangelistDragonFuryClass4Align";
        private static readonly string EvangelistDragonFuryClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass5Align = "EvangelistDragonFuryClass5Align";
        private static readonly string EvangelistDragonFuryClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass6Align = "EvangelistDragonFuryClass6Align";
        private static readonly string EvangelistDragonFuryClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass7Align = "EvangelistDragonFuryClass7Align";
        private static readonly string EvangelistDragonFuryClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass8Align = "EvangelistDragonFuryClass8Align";
        private static readonly string EvangelistDragonFuryClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass9Align = "EvangelistDragonFuryClass9Align";
        private static readonly string EvangelistDragonFuryClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDragonFuryClass10Align = "EvangelistDragonFuryClass10Align";
        private static readonly string EvangelistDragonFuryClass10AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass0Align = "EvangelistEsotericKnightClass0Align";
        private static readonly string EvangelistEsotericKnightClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistEsotericKnightClass0AlignDisplayName = "EvangelistEsotericKnightClass0Align.Name";
        private const string EvangelistEsotericKnightClass0AlignDescription = "EvangelistEsotericKnightClass0Align.Description";
        private const string EvangelistEsotericKnightClass2Align = "EvangelistEsotericKnightClass2Align";
        private static readonly string EvangelistEsotericKnightClass2AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass3Align = "EvangelistEsotericKnightClass3Align";
        private static readonly string EvangelistEsotericKnightClass3AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass4Align = "EvangelistEsotericKnightClass4Align";
        private static readonly string EvangelistEsotericKnightClass4AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass5Align = "EvangelistEsotericKnightClass5Align";
        private static readonly string EvangelistEsotericKnightClass5AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass6Align = "EvangelistEsotericKnightClass6Align";
        private static readonly string EvangelistEsotericKnightClass6AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass7Align = "EvangelistEsotericKnightClass7Align";
        private static readonly string EvangelistEsotericKnightClass7AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass8Align = "EvangelistEsotericKnightClass8Align";
        private static readonly string EvangelistEsotericKnightClass8AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass9Align = "EvangelistEsotericKnightClass9Align";
        private static readonly string EvangelistEsotericKnightClass9AlignGuid = "replaceguidhere";
        private const string EvangelistEsotericKnightClass10Align = "EvangelistEsotericKnightClass10Align";
        private static readonly string EvangelistEsotericKnightClass10AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass0Align = "EvangelistExaltedEvangelistClass0Align";
        private static readonly string EvangelistExaltedEvangelistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistExaltedEvangelistClass0AlignDisplayName = "EvangelistExaltedEvangelistClass0Align.Name";
        private const string EvangelistExaltedEvangelistClass0AlignDescription = "EvangelistExaltedEvangelistClass0Align.Description";
        private const string EvangelistExaltedEvangelistClass2Align = "EvangelistExaltedEvangelistClass2Align";
        private static readonly string EvangelistExaltedEvangelistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass3Align = "EvangelistExaltedEvangelistClass3Align";
        private static readonly string EvangelistExaltedEvangelistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass4Align = "EvangelistExaltedEvangelistClass4Align";
        private static readonly string EvangelistExaltedEvangelistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass5Align = "EvangelistExaltedEvangelistClass5Align";
        private static readonly string EvangelistExaltedEvangelistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass6Align = "EvangelistExaltedEvangelistClass6Align";
        private static readonly string EvangelistExaltedEvangelistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass7Align = "EvangelistExaltedEvangelistClass7Align";
        private static readonly string EvangelistExaltedEvangelistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass8Align = "EvangelistExaltedEvangelistClass8Align";
        private static readonly string EvangelistExaltedEvangelistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass9Align = "EvangelistExaltedEvangelistClass9Align";
        private static readonly string EvangelistExaltedEvangelistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistExaltedEvangelistClass10Align = "EvangelistExaltedEvangelistClass10Align";
        private static readonly string EvangelistExaltedEvangelistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass0Align = "EvangelistFuriousGuardianClass0Align";
        private static readonly string EvangelistFuriousGuardianClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistFuriousGuardianClass0AlignDisplayName = "EvangelistFuriousGuardianClass0Align.Name";
        private const string EvangelistFuriousGuardianClass0AlignDescription = "EvangelistFuriousGuardianClass0Align.Description";
        private const string EvangelistFuriousGuardianClass2Align = "EvangelistFuriousGuardianClass2Align";
        private static readonly string EvangelistFuriousGuardianClass2AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass3Align = "EvangelistFuriousGuardianClass3Align";
        private static readonly string EvangelistFuriousGuardianClass3AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass4Align = "EvangelistFuriousGuardianClass4Align";
        private static readonly string EvangelistFuriousGuardianClass4AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass5Align = "EvangelistFuriousGuardianClass5Align";
        private static readonly string EvangelistFuriousGuardianClass5AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass6Align = "EvangelistFuriousGuardianClass6Align";
        private static readonly string EvangelistFuriousGuardianClass6AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass7Align = "EvangelistFuriousGuardianClass7Align";
        private static readonly string EvangelistFuriousGuardianClass7AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass8Align = "EvangelistFuriousGuardianClass8Align";
        private static readonly string EvangelistFuriousGuardianClass8AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass9Align = "EvangelistFuriousGuardianClass9Align";
        private static readonly string EvangelistFuriousGuardianClass9AlignGuid = "replaceguidhere";
        private const string EvangelistFuriousGuardianClass10Align = "EvangelistFuriousGuardianClass10Align";
        private static readonly string EvangelistFuriousGuardianClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass0Align = "EvangelistHalflingOpportunistClass0Align";
        private static readonly string EvangelistHalflingOpportunistClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHalflingOpportunistClass0AlignDisplayName = "EvangelistHalflingOpportunistClass0Align.Name";
        private const string EvangelistHalflingOpportunistClass0AlignDescription = "EvangelistHalflingOpportunistClass0Align.Description";
        private const string EvangelistHalflingOpportunistClass2Align = "EvangelistHalflingOpportunistClass2Align";
        private static readonly string EvangelistHalflingOpportunistClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass3Align = "EvangelistHalflingOpportunistClass3Align";
        private static readonly string EvangelistHalflingOpportunistClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass4Align = "EvangelistHalflingOpportunistClass4Align";
        private static readonly string EvangelistHalflingOpportunistClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass5Align = "EvangelistHalflingOpportunistClass5Align";
        private static readonly string EvangelistHalflingOpportunistClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass6Align = "EvangelistHalflingOpportunistClass6Align";
        private static readonly string EvangelistHalflingOpportunistClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass7Align = "EvangelistHalflingOpportunistClass7Align";
        private static readonly string EvangelistHalflingOpportunistClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass8Align = "EvangelistHalflingOpportunistClass8Align";
        private static readonly string EvangelistHalflingOpportunistClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass9Align = "EvangelistHalflingOpportunistClass9Align";
        private static readonly string EvangelistHalflingOpportunistClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHalflingOpportunistClass10Align = "EvangelistHalflingOpportunistClass10Align";
        private static readonly string EvangelistHalflingOpportunistClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass0Align = "EvangelistHinterlanderClass0Align";
        private static readonly string EvangelistHinterlanderClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHinterlanderClass0AlignDisplayName = "EvangelistHinterlanderClass0Align.Name";
        private const string EvangelistHinterlanderClass0AlignDescription = "EvangelistHinterlanderClass0Align.Description";
        private const string EvangelistHinterlanderClass2Align = "EvangelistHinterlanderClass2Align";
        private static readonly string EvangelistHinterlanderClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass3Align = "EvangelistHinterlanderClass3Align";
        private static readonly string EvangelistHinterlanderClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass4Align = "EvangelistHinterlanderClass4Align";
        private static readonly string EvangelistHinterlanderClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass5Align = "EvangelistHinterlanderClass5Align";
        private static readonly string EvangelistHinterlanderClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass6Align = "EvangelistHinterlanderClass6Align";
        private static readonly string EvangelistHinterlanderClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass7Align = "EvangelistHinterlanderClass7Align";
        private static readonly string EvangelistHinterlanderClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass8Align = "EvangelistHinterlanderClass8Align";
        private static readonly string EvangelistHinterlanderClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass9Align = "EvangelistHinterlanderClass9Align";
        private static readonly string EvangelistHinterlanderClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHinterlanderClass10Align = "EvangelistHinterlanderClass10Align";
        private static readonly string EvangelistHinterlanderClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass0Align = "EvangelistHorizonWalkerClass0Align";
        private static readonly string EvangelistHorizonWalkerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHorizonWalkerClass0AlignDisplayName = "EvangelistHorizonWalkerClass0Align.Name";
        private const string EvangelistHorizonWalkerClass0AlignDescription = "EvangelistHorizonWalkerClass0Align.Description";
        private const string EvangelistHorizonWalkerClass2Align = "EvangelistHorizonWalkerClass2Align";
        private static readonly string EvangelistHorizonWalkerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass3Align = "EvangelistHorizonWalkerClass3Align";
        private static readonly string EvangelistHorizonWalkerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass4Align = "EvangelistHorizonWalkerClass4Align";
        private static readonly string EvangelistHorizonWalkerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass5Align = "EvangelistHorizonWalkerClass5Align";
        private static readonly string EvangelistHorizonWalkerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass6Align = "EvangelistHorizonWalkerClass6Align";
        private static readonly string EvangelistHorizonWalkerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass7Align = "EvangelistHorizonWalkerClass7Align";
        private static readonly string EvangelistHorizonWalkerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass8Align = "EvangelistHorizonWalkerClass8Align";
        private static readonly string EvangelistHorizonWalkerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass9Align = "EvangelistHorizonWalkerClass9Align";
        private static readonly string EvangelistHorizonWalkerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHorizonWalkerClass10Align = "EvangelistHorizonWalkerClass10Align";
        private static readonly string EvangelistHorizonWalkerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass0Align = "EvangelistInheritorCrusaderClass0Align";
        private static readonly string EvangelistInheritorCrusaderClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistInheritorCrusaderClass0AlignDisplayName = "EvangelistInheritorCrusaderClass0Align.Name";
        private const string EvangelistInheritorCrusaderClass0AlignDescription = "EvangelistInheritorCrusaderClass0Align.Description";
        private const string EvangelistInheritorCrusaderClass2Align = "EvangelistInheritorCrusaderClass2Align";
        private static readonly string EvangelistInheritorCrusaderClass2AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass3Align = "EvangelistInheritorCrusaderClass3Align";
        private static readonly string EvangelistInheritorCrusaderClass3AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass4Align = "EvangelistInheritorCrusaderClass4Align";
        private static readonly string EvangelistInheritorCrusaderClass4AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass5Align = "EvangelistInheritorCrusaderClass5Align";
        private static readonly string EvangelistInheritorCrusaderClass5AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass6Align = "EvangelistInheritorCrusaderClass6Align";
        private static readonly string EvangelistInheritorCrusaderClass6AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass7Align = "EvangelistInheritorCrusaderClass7Align";
        private static readonly string EvangelistInheritorCrusaderClass7AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass8Align = "EvangelistInheritorCrusaderClass8Align";
        private static readonly string EvangelistInheritorCrusaderClass8AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass9Align = "EvangelistInheritorCrusaderClass9Align";
        private static readonly string EvangelistInheritorCrusaderClass9AlignGuid = "replaceguidhere";
        private const string EvangelistInheritorCrusaderClass10Align = "EvangelistInheritorCrusaderClass10Align";
        private static readonly string EvangelistInheritorCrusaderClass10AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass0Align = "EvangelistMammothRiderClass0Align";
        private static readonly string EvangelistMammothRiderClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistMammothRiderClass0AlignDisplayName = "EvangelistMammothRiderClass0Align.Name";
        private const string EvangelistMammothRiderClass0AlignDescription = "EvangelistMammothRiderClass0Align.Description";
        private const string EvangelistMammothRiderClass2Align = "EvangelistMammothRiderClass2Align";
        private static readonly string EvangelistMammothRiderClass2AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass3Align = "EvangelistMammothRiderClass3Align";
        private static readonly string EvangelistMammothRiderClass3AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass4Align = "EvangelistMammothRiderClass4Align";
        private static readonly string EvangelistMammothRiderClass4AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass5Align = "EvangelistMammothRiderClass5Align";
        private static readonly string EvangelistMammothRiderClass5AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass6Align = "EvangelistMammothRiderClass6Align";
        private static readonly string EvangelistMammothRiderClass6AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass7Align = "EvangelistMammothRiderClass7Align";
        private static readonly string EvangelistMammothRiderClass7AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass8Align = "EvangelistMammothRiderClass8Align";
        private static readonly string EvangelistMammothRiderClass8AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass9Align = "EvangelistMammothRiderClass9Align";
        private static readonly string EvangelistMammothRiderClass9AlignGuid = "replaceguidhere";
        private const string EvangelistMammothRiderClass10Align = "EvangelistMammothRiderClass10Align";
        private static readonly string EvangelistMammothRiderClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass0Align = "EvangelistSanguineAngelClass0Align";
        private static readonly string EvangelistSanguineAngelClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSanguineAngelClass0AlignDisplayName = "EvangelistSanguineAngelClass0Align.Name";
        private const string EvangelistSanguineAngelClass0AlignDescription = "EvangelistSanguineAngelClass0Align.Description";
        private const string EvangelistSanguineAngelClass2Align = "EvangelistSanguineAngelClass2Align";
        private static readonly string EvangelistSanguineAngelClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass3Align = "EvangelistSanguineAngelClass3Align";
        private static readonly string EvangelistSanguineAngelClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass4Align = "EvangelistSanguineAngelClass4Align";
        private static readonly string EvangelistSanguineAngelClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass5Align = "EvangelistSanguineAngelClass5Align";
        private static readonly string EvangelistSanguineAngelClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass6Align = "EvangelistSanguineAngelClass6Align";
        private static readonly string EvangelistSanguineAngelClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass7Align = "EvangelistSanguineAngelClass7Align";
        private static readonly string EvangelistSanguineAngelClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass8Align = "EvangelistSanguineAngelClass8Align";
        private static readonly string EvangelistSanguineAngelClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass9Align = "EvangelistSanguineAngelClass9Align";
        private static readonly string EvangelistSanguineAngelClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSanguineAngelClass10Align = "EvangelistSanguineAngelClass10Align";
        private static readonly string EvangelistSanguineAngelClass10AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass0Align = "EvangelistScarSeekerClass0Align";
        private static readonly string EvangelistScarSeekerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistScarSeekerClass0AlignDisplayName = "EvangelistScarSeekerClass0Align.Name";
        private const string EvangelistScarSeekerClass0AlignDescription = "EvangelistScarSeekerClass0Align.Description";
        private const string EvangelistScarSeekerClass2Align = "EvangelistScarSeekerClass2Align";
        private static readonly string EvangelistScarSeekerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass3Align = "EvangelistScarSeekerClass3Align";
        private static readonly string EvangelistScarSeekerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass4Align = "EvangelistScarSeekerClass4Align";
        private static readonly string EvangelistScarSeekerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass5Align = "EvangelistScarSeekerClass5Align";
        private static readonly string EvangelistScarSeekerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass6Align = "EvangelistScarSeekerClass6Align";
        private static readonly string EvangelistScarSeekerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass7Align = "EvangelistScarSeekerClass7Align";
        private static readonly string EvangelistScarSeekerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass8Align = "EvangelistScarSeekerClass8Align";
        private static readonly string EvangelistScarSeekerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass9Align = "EvangelistScarSeekerClass9Align";
        private static readonly string EvangelistScarSeekerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistScarSeekerClass10Align = "EvangelistScarSeekerClass10Align";
        private static readonly string EvangelistScarSeekerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass0Align = "EvangelistSentinelClass0Align";
        private static readonly string EvangelistSentinelClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSentinelClass0AlignDisplayName = "EvangelistSentinelClass0Align.Name";
        private const string EvangelistSentinelClass0AlignDescription = "EvangelistSentinelClass0Align.Description";
        private const string EvangelistSentinelClass2Align = "EvangelistSentinelClass2Align";
        private static readonly string EvangelistSentinelClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass3Align = "EvangelistSentinelClass3Align";
        private static readonly string EvangelistSentinelClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass4Align = "EvangelistSentinelClass4Align";
        private static readonly string EvangelistSentinelClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass5Align = "EvangelistSentinelClass5Align";
        private static readonly string EvangelistSentinelClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass6Align = "EvangelistSentinelClass6Align";
        private static readonly string EvangelistSentinelClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass7Align = "EvangelistSentinelClass7Align";
        private static readonly string EvangelistSentinelClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass8Align = "EvangelistSentinelClass8Align";
        private static readonly string EvangelistSentinelClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass9Align = "EvangelistSentinelClass9Align";
        private static readonly string EvangelistSentinelClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSentinelClass10Align = "EvangelistSentinelClass10Align";
        private static readonly string EvangelistSentinelClass10AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass0Align = "EvangelistShadowDancerClass0Align";
        private static readonly string EvangelistShadowDancerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistShadowDancerClass0AlignDisplayName = "EvangelistShadowDancerClass0Align.Name";
        private const string EvangelistShadowDancerClass0AlignDescription = "EvangelistShadowDancerClass0Align.Description";
        private const string EvangelistShadowDancerClass2Align = "EvangelistShadowDancerClass2Align";
        private static readonly string EvangelistShadowDancerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass3Align = "EvangelistShadowDancerClass3Align";
        private static readonly string EvangelistShadowDancerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass4Align = "EvangelistShadowDancerClass4Align";
        private static readonly string EvangelistShadowDancerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass5Align = "EvangelistShadowDancerClass5Align";
        private static readonly string EvangelistShadowDancerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass6Align = "EvangelistShadowDancerClass6Align";
        private static readonly string EvangelistShadowDancerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass7Align = "EvangelistShadowDancerClass7Align";
        private static readonly string EvangelistShadowDancerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass8Align = "EvangelistShadowDancerClass8Align";
        private static readonly string EvangelistShadowDancerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass9Align = "EvangelistShadowDancerClass9Align";
        private static readonly string EvangelistShadowDancerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistShadowDancerClass10Align = "EvangelistShadowDancerClass10Align";
        private static readonly string EvangelistShadowDancerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass0Align = "EvangelistSouldrinkerClass0Align";
        private static readonly string EvangelistSouldrinkerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSouldrinkerClass0AlignDisplayName = "EvangelistSouldrinkerClass0Align.Name";
        private const string EvangelistSouldrinkerClass0AlignDescription = "EvangelistSouldrinkerClass0Align.Description";
        private const string EvangelistSouldrinkerClass2Align = "EvangelistSouldrinkerClass2Align";
        private static readonly string EvangelistSouldrinkerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass3Align = "EvangelistSouldrinkerClass3Align";
        private static readonly string EvangelistSouldrinkerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass4Align = "EvangelistSouldrinkerClass4Align";
        private static readonly string EvangelistSouldrinkerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass5Align = "EvangelistSouldrinkerClass5Align";
        private static readonly string EvangelistSouldrinkerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass6Align = "EvangelistSouldrinkerClass6Align";
        private static readonly string EvangelistSouldrinkerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass7Align = "EvangelistSouldrinkerClass7Align";
        private static readonly string EvangelistSouldrinkerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass8Align = "EvangelistSouldrinkerClass8Align";
        private static readonly string EvangelistSouldrinkerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass9Align = "EvangelistSouldrinkerClass9Align";
        private static readonly string EvangelistSouldrinkerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSouldrinkerClass10Align = "EvangelistSouldrinkerClass10Align";
        private static readonly string EvangelistSouldrinkerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass0Align = "EvangelistUmbralAgentClass0Align";
        private static readonly string EvangelistUmbralAgentClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistUmbralAgentClass0AlignDisplayName = "EvangelistUmbralAgentClass0Align.Name";
        private const string EvangelistUmbralAgentClass0AlignDescription = "EvangelistUmbralAgentClass0Align.Description";
        private const string EvangelistUmbralAgentClass2Align = "EvangelistUmbralAgentClass2Align";
        private static readonly string EvangelistUmbralAgentClass2AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass3Align = "EvangelistUmbralAgentClass3Align";
        private static readonly string EvangelistUmbralAgentClass3AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass4Align = "EvangelistUmbralAgentClass4Align";
        private static readonly string EvangelistUmbralAgentClass4AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass5Align = "EvangelistUmbralAgentClass5Align";
        private static readonly string EvangelistUmbralAgentClass5AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass6Align = "EvangelistUmbralAgentClass6Align";
        private static readonly string EvangelistUmbralAgentClass6AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass7Align = "EvangelistUmbralAgentClass7Align";
        private static readonly string EvangelistUmbralAgentClass7AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass8Align = "EvangelistUmbralAgentClass8Align";
        private static readonly string EvangelistUmbralAgentClass8AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass9Align = "EvangelistUmbralAgentClass9Align";
        private static readonly string EvangelistUmbralAgentClass9AlignGuid = "replaceguidhere";
        private const string EvangelistUmbralAgentClass10Align = "EvangelistUmbralAgentClass10Align";
        private static readonly string EvangelistUmbralAgentClass10AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass0Align = "EvangelistMicroAntiPaladinClass0Align";
        private static readonly string EvangelistMicroAntiPaladinClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistMicroAntiPaladinClass0AlignDisplayName = "EvangelistMicroAntiPaladinClass0Align.Name";
        private const string EvangelistMicroAntiPaladinClass0AlignDescription = "EvangelistMicroAntiPaladinClass0Align.Description";
        private const string EvangelistMicroAntiPaladinClass2Align = "EvangelistMicroAntiPaladinClass2Align";
        private static readonly string EvangelistMicroAntiPaladinClass2AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass3Align = "EvangelistMicroAntiPaladinClass3Align";
        private static readonly string EvangelistMicroAntiPaladinClass3AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass4Align = "EvangelistMicroAntiPaladinClass4Align";
        private static readonly string EvangelistMicroAntiPaladinClass4AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass5Align = "EvangelistMicroAntiPaladinClass5Align";
        private static readonly string EvangelistMicroAntiPaladinClass5AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass6Align = "EvangelistMicroAntiPaladinClass6Align";
        private static readonly string EvangelistMicroAntiPaladinClass6AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass7Align = "EvangelistMicroAntiPaladinClass7Align";
        private static readonly string EvangelistMicroAntiPaladinClass7AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass8Align = "EvangelistMicroAntiPaladinClass8Align";
        private static readonly string EvangelistMicroAntiPaladinClass8AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass9Align = "EvangelistMicroAntiPaladinClass9Align";
        private static readonly string EvangelistMicroAntiPaladinClass9AlignGuid = "replaceguidhere";
        private const string EvangelistMicroAntiPaladinClass10Align = "EvangelistMicroAntiPaladinClass10Align";
        private static readonly string EvangelistMicroAntiPaladinClass10AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass0Align = "EvangelistOathbreakerClass0Align";
        private static readonly string EvangelistOathbreakerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistOathbreakerClass0AlignDisplayName = "EvangelistOathbreakerClass0Align.Name";
        private const string EvangelistOathbreakerClass0AlignDescription = "EvangelistOathbreakerClass0Align.Description";
        private const string EvangelistOathbreakerClass2Align = "EvangelistOathbreakerClass2Align";
        private static readonly string EvangelistOathbreakerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass3Align = "EvangelistOathbreakerClass3Align";
        private static readonly string EvangelistOathbreakerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass4Align = "EvangelistOathbreakerClass4Align";
        private static readonly string EvangelistOathbreakerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass5Align = "EvangelistOathbreakerClass5Align";
        private static readonly string EvangelistOathbreakerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass6Align = "EvangelistOathbreakerClass6Align";
        private static readonly string EvangelistOathbreakerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass7Align = "EvangelistOathbreakerClass7Align";
        private static readonly string EvangelistOathbreakerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass8Align = "EvangelistOathbreakerClass8Align";
        private static readonly string EvangelistOathbreakerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass9Align = "EvangelistOathbreakerClass9Align";
        private static readonly string EvangelistOathbreakerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistOathbreakerClass10Align = "EvangelistOathbreakerClass10Align";
        private static readonly string EvangelistOathbreakerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass0Align = "EvangelistDreadKnightClass0Align";
        private static readonly string EvangelistDreadKnightClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistDreadKnightClass0AlignDisplayName = "EvangelistDreadKnightClass0Align.Name";
        private const string EvangelistDreadKnightClass0AlignDescription = "EvangelistDreadKnightClass0Align.Description";
        private const string EvangelistDreadKnightClass2Align = "EvangelistDreadKnightClass2Align";
        private static readonly string EvangelistDreadKnightClass2AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass3Align = "EvangelistDreadKnightClass3Align";
        private static readonly string EvangelistDreadKnightClass3AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass4Align = "EvangelistDreadKnightClass4Align";
        private static readonly string EvangelistDreadKnightClass4AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass5Align = "EvangelistDreadKnightClass5Align";
        private static readonly string EvangelistDreadKnightClass5AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass6Align = "EvangelistDreadKnightClass6Align";
        private static readonly string EvangelistDreadKnightClass6AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass7Align = "EvangelistDreadKnightClass7Align";
        private static readonly string EvangelistDreadKnightClass7AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass8Align = "EvangelistDreadKnightClass8Align";
        private static readonly string EvangelistDreadKnightClass8AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass9Align = "EvangelistDreadKnightClass9Align";
        private static readonly string EvangelistDreadKnightClass9AlignGuid = "replaceguidhere";
        private const string EvangelistDreadKnightClass10Align = "EvangelistDreadKnightClass10Align";
        private static readonly string EvangelistDreadKnightClass10AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass0Align = "EvangelistStargazerClass0Align";
        private static readonly string EvangelistStargazerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistStargazerClass0AlignDisplayName = "EvangelistStargazerClass0Align.Name";
        private const string EvangelistStargazerClass0AlignDescription = "EvangelistStargazerClass0Align.Description";
        private const string EvangelistStargazerClass2Align = "EvangelistStargazerClass2Align";
        private static readonly string EvangelistStargazerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass3Align = "EvangelistStargazerClass3Align";
        private static readonly string EvangelistStargazerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass4Align = "EvangelistStargazerClass4Align";
        private static readonly string EvangelistStargazerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass5Align = "EvangelistStargazerClass5Align";
        private static readonly string EvangelistStargazerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass6Align = "EvangelistStargazerClass6Align";
        private static readonly string EvangelistStargazerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass7Align = "EvangelistStargazerClass7Align";
        private static readonly string EvangelistStargazerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass8Align = "EvangelistStargazerClass8Align";
        private static readonly string EvangelistStargazerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass9Align = "EvangelistStargazerClass9Align";
        private static readonly string EvangelistStargazerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistStargazerClass10Align = "EvangelistStargazerClass10Align";
        private static readonly string EvangelistStargazerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass0Align = "EvangelistSwashbucklerClass0Align";
        private static readonly string EvangelistSwashbucklerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSwashbucklerClass0AlignDisplayName = "EvangelistSwashbucklerClass0Align.Name";
        private const string EvangelistSwashbucklerClass0AlignDescription = "EvangelistSwashbucklerClass0Align.Description";
        private const string EvangelistSwashbucklerClass2Align = "EvangelistSwashbucklerClass2Align";
        private static readonly string EvangelistSwashbucklerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass3Align = "EvangelistSwashbucklerClass3Align";
        private static readonly string EvangelistSwashbucklerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass4Align = "EvangelistSwashbucklerClass4Align";
        private static readonly string EvangelistSwashbucklerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass5Align = "EvangelistSwashbucklerClass5Align";
        private static readonly string EvangelistSwashbucklerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass6Align = "EvangelistSwashbucklerClass6Align";
        private static readonly string EvangelistSwashbucklerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass7Align = "EvangelistSwashbucklerClass7Align";
        private static readonly string EvangelistSwashbucklerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass8Align = "EvangelistSwashbucklerClass8Align";
        private static readonly string EvangelistSwashbucklerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass9Align = "EvangelistSwashbucklerClass9Align";
        private static readonly string EvangelistSwashbucklerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSwashbucklerClass10Align = "EvangelistSwashbucklerClass10Align";
        private static readonly string EvangelistSwashbucklerClass10AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass0Align = "EvangelistHolyVindicatorClass0Align";
        private static readonly string EvangelistHolyVindicatorClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistHolyVindicatorClass0AlignDisplayName = "EvangelistHolyVindicatorClass0Align.Name";
        private const string EvangelistHolyVindicatorClass0AlignDescription = "EvangelistHolyVindicatorClass0Align.Description";
        private const string EvangelistHolyVindicatorClass2Align = "EvangelistHolyVindicatorClass2Align";
        private static readonly string EvangelistHolyVindicatorClass2AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass3Align = "EvangelistHolyVindicatorClass3Align";
        private static readonly string EvangelistHolyVindicatorClass3AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass4Align = "EvangelistHolyVindicatorClass4Align";
        private static readonly string EvangelistHolyVindicatorClass4AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass5Align = "EvangelistHolyVindicatorClass5Align";
        private static readonly string EvangelistHolyVindicatorClass5AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass6Align = "EvangelistHolyVindicatorClass6Align";
        private static readonly string EvangelistHolyVindicatorClass6AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass7Align = "EvangelistHolyVindicatorClass7Align";
        private static readonly string EvangelistHolyVindicatorClass7AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass8Align = "EvangelistHolyVindicatorClass8Align";
        private static readonly string EvangelistHolyVindicatorClass8AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass9Align = "EvangelistHolyVindicatorClass9Align";
        private static readonly string EvangelistHolyVindicatorClass9AlignGuid = "replaceguidhere";
        private const string EvangelistHolyVindicatorClass10Align = "EvangelistHolyVindicatorClass10Align";
        private static readonly string EvangelistHolyVindicatorClass10AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass0Align = "EvangelistSummonerClass0Align";
        private static readonly string EvangelistSummonerClass0AlignGuid = "replaceguidhere";
        internal const string EvangelistSummonerClass0AlignDisplayName = "EvangelistSummonerClass0Align.Name";
        private const string EvangelistSummonerClass0AlignDescription = "EvangelistSummonerClass0Align.Description";
        private const string EvangelistSummonerClass2Align = "EvangelistSummonerClass2Align";
        private static readonly string EvangelistSummonerClass2AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass3Align = "EvangelistSummonerClass3Align";
        private static readonly string EvangelistSummonerClass3AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass4Align = "EvangelistSummonerClass4Align";
        private static readonly string EvangelistSummonerClass4AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass5Align = "EvangelistSummonerClass5Align";
        private static readonly string EvangelistSummonerClass5AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass6Align = "EvangelistSummonerClass6Align";
        private static readonly string EvangelistSummonerClass6AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass7Align = "EvangelistSummonerClass7Align";
        private static readonly string EvangelistSummonerClass7AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass8Align = "EvangelistSummonerClass8Align";
        private static readonly string EvangelistSummonerClass8AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass9Align = "EvangelistSummonerClass9Align";
        private static readonly string EvangelistSummonerClass9AlignGuid = "replaceguidhere";
        private const string EvangelistSummonerClass10Align = "EvangelistSummonerClass10Align";
        private static readonly string EvangelistSummonerClass10AlignGuid = "replaceguidhere";

    }
}
