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
            .AddToLevelEntry(1, BonusFeatFeat(), ChoosePathFeat(), FeatureSelectionRefs.StudentOfWarAdditionalSKillSelection.ToString(), FeatureSelectionRefs.StudentOfWarAdditionalSKillSelection.ToString())
            .AddToLevelEntry(2, ProtectiveGraceFeat())
            .AddToLevelEntry(3, Sentinel.DivineBoon1Guid)
            .AddToLevelEntry(4)
            .AddToLevelEntry(5)
            .AddToLevelEntry(6, Sentinel.DivineBoon2Guid)
            .AddToLevelEntry(7, ProtectiveGraceGuid)
            .AddToLevelEntry(8, SpiritualFormFeat())
            .AddToLevelEntry(9, Sentinel.DivineBoon3Guid)
            .AddToLevelEntry(10)
            .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { FiendishStudiesGuid, FiendishStudies5Guid, FiendishStudies10Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { VitalityGood3Guid, VitalityGood6Guid, VitalityGood9Guid, VitalityGoodGuid })
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
            var icon = FeatureRefs.Dodge.Reference.Get().Icon;
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
            var icon = AbilityRefs.BreathOfLifeCast.Reference.Get().Icon;
            return FeatureSelectionConfigurator.New(ChoosePath, ChoosePathGuid)
              .SetDisplayName(ChoosePathDisplayName)
              .SetDescription(ChoosePathDescription)
              .SetIcon(icon)
              .SetObligatory(true)
              .AddFacts(new() { FeatureRefs.LightArmorProficiency.ToString(), FeatureRefs.SimpleWeaponProficiency.ToString() })
              .AddComponent<AddDeityWeaponPro>()
              .AddToAllFeatures(SanctifiedRogueFeat())
              .AddToAllFeatures(SanctifiedKineticistFeat())
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
              .AddPrerequisiteAlignment(AlignmentMaskType.Good, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Prerequisite.GroupType.Any)
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
              .AddPrerequisiteAlignment(AlignmentMaskType.Evil, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Prerequisite.GroupType.Any)
              .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Prerequisite.GroupType.Any)
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
            var icon = AbilityRefs.IceBody.Reference.Get().Icon;

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

        private const string SanctifiedRogue = "ExaltedEvangelist.SanctifiedRogue";
        private static readonly string SanctifiedRogueGuid = "{66B4B2A1-8C37-4301-9F16-59810027FF46}";

        internal const string SanctifiedRogueDisplayName = "ExaltedEvangelistSanctifiedRogue.Name";
        private const string SanctifiedRogueDescription = "ExaltedEvangelistSanctifiedRogue.Description";
        public static BlueprintProgression SanctifiedRogueFeat()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;
            return ProgressionConfigurator.New(SanctifiedRogue, SanctifiedRogueGuid)
              .SetDisplayName(SanctifiedRogueDisplayName)
              .SetDescription(SanctifiedRogueDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteFeature(FeatureRefs.SneakAttack.ToString())
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.SlayerClass.ToString(), modifier: 1, summand: 0)
              .AddToLevelEntry(2, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(3, FeatureSelectionRefs.RogueTalentSelection.ToString())
              .AddToLevelEntry(4, FeatureRefs.SneakAttack.ToString(), Kidnapper.CleanCaptureGuid)
              .AddToLevelEntry(5, FeatureSelectionRefs.RogueTalentSelection.ToString())
              .AddToLevelEntry(6, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(7, FeatureSelectionRefs.RogueTalentSelection.ToString(), FeatureRefs.AdvanceTalents.ToString())
              .AddToLevelEntry(8, FeatureRefs.SneakAttack.ToString())
              .AddToLevelEntry(9, FeatureSelectionRefs.RogueTalentSelection.ToString())
              .AddToLevelEntry(10, FeatureRefs.SneakAttack.ToString(), HalflingOpportunist.OpportunityGuid)
              .Configure();
        }

        private const string SanctifiedKineticist = "ExaltedEvangelist.SanctifiedKineticist";
        private static readonly string SanctifiedKineticistGuid = "{E80441D4-90BA-4880-85DC-1A66246F14D8}";

        internal const string SanctifiedKineticistDisplayName = "ExaltedEvangelistSanctifiedKineticist.Name";
        private const string SanctifiedKineticistDescription = "ExaltedEvangelistSanctifiedKineticist.Description";
        public static BlueprintProgression SanctifiedKineticistFeat()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;
            return ProgressionConfigurator.New(SanctifiedKineticist, SanctifiedKineticistGuid)
              .SetDisplayName(SanctifiedKineticistDisplayName)
              .SetDescription(SanctifiedKineticistDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddPrerequisiteFeature(FeatureRefs.KineticBlastFeature.ToString())
              .AddClassLevelsForPrerequisites(actualClass: ArchetypeGuid, fakeClass: CharacterClassRefs.KineticistClass.ToString(), modifier: 1, summand: 0)
              .AddToLevelEntry(2, FeatureSelectionRefs.InfusionSelection.ToString(), EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(3, FeatureSelectionRefs.WildTalentSelection.ToString(), EsotericKnight.BattleMindGuid, EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(4, FeatureSelectionRefs.InfusionSelection.ToString(), FeatureRefs.InfusionSpecialization.ToString(), EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(5, FeatureSelectionRefs.WildTalentSelection.ToString(), EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(6, FeatureSelectionRefs.InfusionSelection.ToString(), EsotericKnight.BattleMindGuid, EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(7, FeatureSelectionRefs.WildTalentSelection.ToString(), FeatureRefs.InfusionSpecialization.ToString(), EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(8, FeatureSelectionRefs.InfusionSelection.ToString(), EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(9, FeatureSelectionRefs.WildTalentSelection.ToString(), EsotericKnight.BattleMindGuid, EsotericKnight.FeatGuidPro2)
              .AddToLevelEntry(10, FeatureSelectionRefs.ThirdElementalFocusSelection.ToString(), FeatureRefs.InfusionSpecialization.ToString(), EsotericKnight.FeatGuidPro2)
              .Configure();
        }

        private const string TrueExalted = "ExaltedEvangelist.TrueExalted";
        private static readonly string TrueExaltedGuid = "{0DC8DB64-DA89-4092-A5A4-15923F6798A6}";

        internal const string TrueExaltedDisplayName = "ExaltedEvangelistTrueExalted.Name";
        private const string TrueExaltedDescription = "ExaltedEvangelistTrueExalted.Description";
        public static BlueprintProgression TrueExaltedFeat()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;
            return ProgressionConfigurator.New(TrueExalted, TrueExaltedGuid)
              .SetDisplayName(TrueExaltedDisplayName)
              .SetDescription(TrueExaltedDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetClasses(ArchetypeGuid)
              .AddToLevelEntry(1, "{05DC9561-0542-41BD-9E9F-404F59AB68C5}")
              .AddToLevelEntry(2, ChooseGoodEvilFeat())
              .AddToLevelEntry(4, ReligiousSpeakerFeat())
              .AddToLevelEntry(5, ExaltedDomainPlusFeat())
              .AddToLevelEntry(10, CreateSpellbook())
              .Configure();
        }

        private const string ExaltedDomainPlus = "Exalted.ExaltedDomainPlus";
        private static readonly string ExaltedDomainPlusGuid = "{18E8AE34-E61D-4149-989B-4E55560F03CA}";

        internal const string ExaltedDomainPlusDisplayName = "ExaltedExaltedDomainPlus.Name";
        private const string ExaltedDomainPlusDescription = "ExaltedExaltedDomainPlus.Description";

        public static BlueprintProgression ExaltedDomainPlusFeat()
        {
            return ProgressionConfigurator.New(ExaltedDomainPlus, ExaltedDomainPlusGuid)
              .SetDisplayName(ExaltedDomainPlusDisplayName)
              .SetDescription(ExaltedDomainPlusDescription)
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
        private static readonly string SpellBookGuid = "{2BEC7992-A06A-4691-A65E-71F6BE6E8D49}";

        private const string SpellBookFeat = "ExaltedEvangelist.SpellBookFeat";
        private static readonly string SpellBookFeatGuid = "{337EF978-CEE6-4B71-9D64-7A32470C81B9}";

        internal const string SpellBookDisplayName = "ExaltedEvangelistSpellBook.Name";
        private const string SpellBookDescription = "ExaltedEvangelistSpellBook.Description";
        private static BlueprintFeature CreateSpellbook()
        {
            var icon = FeatureSelectionRefs.CombatTrick.Reference.Get().Icon;

            var spellbook = SpellbookConfigurator.New(SpellBook, SpellBookGuid)
              .SetName(ArchetypeDisplayName)
              .SetSpellsPerDay(GetSpellSlots())
              .SetAllSpellsKnown(true)
              .SetSpellList(GraveSpellList.spelllist2guid)
              .SetCharacterClass(ArchetypeGuid)
              .SetCastingAttribute(StatType.Charisma)
              .SetSpontaneous(true)
              .SetIsArcane(false)
              .Configure(delayed: true);

            return FeatureConfigurator.New(SpellBookFeat, SpellBookFeatGuid)
                    .SetDisplayName(SpellBookDisplayName)
                    .SetDescription(SpellBookDescription)
                    .SetIcon(icon)
                    .AddSpellbook(ContextValues.Property(UnitProperty.Level), spellbook: spellbook)
                    .Configure();
        }

        private const string SpellTable = "ExaltedEvangelist.SpellTable";
        private static readonly string SpellTableGuid = "{8FD87C0A-831F-450C-8843-0F770B2F777D}";
        private static BlueprintSpellsTable GetSpellSlots()
        {
            var ClericSpellSlots = SpellsTableRefs.ClericSpellLevels.Reference.Get();
            var levelEntries =
              ClericSpellSlots.Levels.Select(
                l =>
                {
                    var count = new int[] { 0, 1, 1, 1, 1, 1, 1 };
                    return new SpellsLevelEntry { Count = count };
                });
            return SpellsTableConfigurator.New(SpellTable, SpellTableGuid)
              .SetLevels(levelEntries.ToArray())
              .Configure();
        }
    }
}
