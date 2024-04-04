using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.Utility;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomAction;
using PrestigePlus.CustomComponent.PrestigeClass;
using PrestigePlus.Maneuvers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Gunslinger;
using PrestigePlus.CustomComponent;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Blueprints.Classes.Spells;
using PrestigePlus.Blueprint.ManeuverFeat;
using PrestigePlus.Blueprint.SpecificManeuver;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class MortalUsher
    {
        private const string ArchetypeName = "MortalUsher";
        public static readonly string ArchetypeGuid = "{4CC7AD95-BCE1-4A37-A347-BE1804E94E72}";
        internal const string ArchetypeDisplayName = "MortalUsher.Name";
        private const string ArchetypeDescription = "MortalUsher.Description";

        private const string ClassProgressName = "MortalUsherPrestige";
        public static readonly string ClassProgressGuid = "{5E79E4AC-0249-461C-8BC0-B70771FD5D6A}";
        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, AlignSpamFeat(), ReapingStrikeFeature(), PsychicEsotericaFeat(), ProficiencyScythesFeature())
                .AddToLevelEntry(2)
                .AddToLevelEntry(3, ReapingStrikeGuid, PlagueMaskFeature())
                .AddToLevelEntry(4)
                .AddToLevelEntry(5, ReapingStrikeGuid, CatrinaFeature())
                .AddToLevelEntry(6, BetterEsotericaFeat())
                .AddToLevelEntry(7, ReapingStrikeGuid, MorrignaFeature())
                .AddToLevelEntry(8)
                .AddToLevelEntry(9, ReapingStrikeGuid, VanthWingsFeature())
                .AddToLevelEntry(10, FeatherCloakFeature())
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { PsychicEsotericaGuid, BetterEsotericaGuid }))
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(4)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABMedium.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion, StatType.SkillUseMagicDevice })
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 6)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 6)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.NeutralGood, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.NeutralEvil, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string PsychicEsoterica = "MortalUsher.PsychicEsoterica";
        public static readonly string PsychicEsotericaGuid = "{50B9881C-4C1E-4406-B7FD-45504ED63D7A}";

        private const string PsychicEsoterica0 = "MortalUsher.PsychicEsoterica0";
        private static readonly string PsychicEsoterica0Guid = "{13DFB19D-C3BC-484E-A51B-8955FEA7F0F1}";

        private const string PsychicEsoterica1 = "MortalUsher.PsychicEsoterica1";
        private static readonly string PsychicEsoterica1Guid = "{6CC68BC2-3DDC-48FF-8C21-0CEBCA443709}";

        internal const string PsychicEsotericaDisplayName = "MortalUsherPsychicEsoterica.Name";
        private const string PsychicEsotericaDescription = "MortalUsherPsychicEsoterica.Description";

        public static BlueprintFeatureSelection PsychicEsotericaFeat()
        {
            var icon = FeatureRefs.VitalStrikeFeature.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(PsychicEsoterica1, PsychicEsoterica1Guid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.VitalStrikeFeature.ToString())
              .AddFacts(new() { FeatureRefs.VitalStrikeFeature.ToString() })
              .Configure();

            var select = FeatureSelectionConfigurator.New(PsychicEsoterica0, PsychicEsoterica0Guid)
              .CopyFrom(FeatureSelectionRefs.BasicFeatSelection)
              .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeature.ToString())
              .Configure();

            return FeatureSelectionConfigurator.New(PsychicEsoterica, PsychicEsotericaGuid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .AddToAllFeatures(feat)
              .AddToAllFeatures(select)
              .Configure();
        }

        private const string BetterEsoterica = "MortalUsher.BetterEsoterica";
        private static readonly string BetterEsotericaGuid = "{4A00AECE-9359-40BE-B9BD-65BA811C95FF}";

        private const string BetterEsoterica0 = "MortalUsher.BetterEsoterica0";
        private static readonly string BetterEsoterica0Guid = "{C9C623CB-162B-44AF-A2B9-1A20627DB72D}";

        private const string BetterEsoterica1 = "MortalUsher.BetterEsoterica1";
        private static readonly string BetterEsoterica1Guid = "{6DE61991-5B9F-4E32-B8BB-F3E69756AE85}";
        public static BlueprintFeatureSelection BetterEsotericaFeat()
        {
            var icon = FeatureRefs.VitalStrikeFeatureImproved.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(BetterEsoterica1, BetterEsoterica1Guid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .AddPrerequisiteNoFeature(FeatureRefs.VitalStrikeFeatureImproved.ToString())
              .AddFacts(new() { FeatureRefs.VitalStrikeFeatureImproved.ToString() })
              .Configure();

            var select = FeatureSelectionConfigurator.New(BetterEsoterica0, BetterEsoterica0Guid)
              .CopyFrom(FeatureSelectionRefs.BasicFeatSelection)
              .AddPrerequisiteFeature(FeatureRefs.VitalStrikeFeatureImproved.ToString())
              .Configure();

            return FeatureSelectionConfigurator.New(BetterEsoterica, BetterEsotericaGuid)
              .SetDisplayName(PsychicEsotericaDisplayName)
              .SetDescription(PsychicEsotericaDescription)
              .SetIcon(icon)
              .AddToAllFeatures(feat)
              .AddToAllFeatures(select)
              .Configure();
        }

        private const string Morrigna = "MortalUsherMorrigna";
        private static readonly string MorrignaGuid = "{0AF76AB0-C337-44A1-AEE7-1C79B1D9669B}";

        internal const string MorrignaDisplayName = "MortalUsherMorrigna.Name";
        private const string MorrignaDescription = "MortalUsherMorrigna.Description";
        public static BlueprintFeature MorrignaFeature()
        {
            var icon = AbilityRefs.CloakOfAstoundingProwessAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(Morrigna, MorrignaGuid)
              .SetDisplayName(MorrignaDisplayName)
              .SetDescription(MorrignaDescription)
              .SetIcon(icon)
              .AddContextStatBonus(StatType.AC, ContextValues.Rank(), ModifierDescriptor.NaturalArmor)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithDiv2Progression())
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string Catrina = "MortalUsherCatrina";
        public static readonly string CatrinaGuid = "{DD210097-C947-4F7A-9567-A236E8652CC5}";

        private const string CatrinaFeat = "MortalUsherCatrinaFeat";
        public static readonly string CatrinaFeatGuid = "{0B9DB3B1-F357-40A5-A946-9D42997E4F53}";

        internal const string CatrinaDisplayName = "MortalUsherCatrina.Name";
        private const string CatrinaDescription = "MortalUsherCatrina.Description";
        public static BlueprintFeature CatrinaFeature()
        {
            var icon = AbilityRefs.EuphoricTranquilityCast.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(CatrinaFeat, CatrinaFeatGuid)
              .SetDisplayName(CatrinaDisplayName)
              .SetDescription(CatrinaDescription)
              .SetIcon(icon)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Disruption.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.PrimaryHand)
              .AddBuffEnchantAnyWeapon(WeaponEnchantmentRefs.Disruption.Reference.ToString(), Kingmaker.UI.GenericSlot.EquipSlotBase.SlotType.SecondaryHand)
              .Configure();

            return FeatureConfigurator.New(Catrina, CatrinaGuid)
              .SetDisplayName(CatrinaDisplayName)
              .SetDescription(CatrinaDescription)
              .SetIcon(icon)
              .AddComponent<HasWeaponFeatureUnlock>(c => { 
                  c.FilterByBlueprintWeaponTypes = true; 
                  c.m_BlueprintWeaponTypes = new[] { WeaponTypeRefs.Scythe.Reference.Get().ToReference<BlueprintWeaponTypeReference>() };
                  c.m_NewFact = feat.ToReference<BlueprintUnitFactReference>();
              })
              .Configure();
        }

        private const string FeatherCloak = "MortalUsherFeatherCloak";
        public static readonly string FeatherCloakGuid = "{D0FEBA1A-2727-4E3A-B1F5-818BD9587DD0}";

        internal const string FeatherCloakDisplayName = "MortalUsherFeatherCloak.Name";
        private const string FeatherCloakDescription = "MortalUsherFeatherCloak.Description";
        public static BlueprintFeature FeatherCloakFeature()
        {
            var icon = AbilityRefs.IceBody.Reference.Get().Icon;
            return FeatureConfigurator.New(FeatherCloak, FeatherCloakGuid)
              .SetDisplayName(FeatherCloakDisplayName)
              .SetDescription(FeatherCloakDescription)
              .SetIcon(icon)
              .AddComponent<AddEnergyDamageImmunityPP>(c => { c.EnergyType = DamageEnergyType.Electricity; c.HealOnDamage = true; c.m_HealRate = AddEnergyDamageImmunity.HealingRate.DamageDiv3; })
              .AddSpellImmunityToSpellDescriptor(null, SpellDescriptor.Death)
              .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Death)
              .Configure();
        }

        private const string VanthWings = "MortalUsherVanthWings";
        public static readonly string VanthWingsGuid = "{5585F03B-0268-48D7-93B9-8DCC269E9EDE}";

        internal const string VanthWingsDisplayName = "MortalUsherVanthWings.Name";
        private const string VanthWingsDescription = "MortalUsherVanthWings.Description";
        public static BlueprintFeature VanthWingsFeature()
        {
            var icon = FeatureRefs.FeatureWingsDemon.Reference.Get().Icon;
            return FeatureConfigurator.New(VanthWings, VanthWingsGuid)
              .SetDisplayName(VanthWingsDisplayName)
              .SetDescription(VanthWingsDescription)
              .SetIcon(icon)
              .AddConcentrationBonus(value: 5)
              .AddStatBonus(ModifierDescriptor.Competence, false, StatType.SkillMobility, 5)
              .AddDamageResistanceEnergy(healOnDamage: false, value: ContextValues.Rank(), type: DamageEnergyType.Cold)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithBonusValueProgression(10))
              .AddFacts(new() { FeatureRefs.FeatureWingsDemon.ToString() })
              .Configure();
        }

        private const string PlagueMask = "MortalUsherPlagueMask";
        public static readonly string PlagueMaskGuid = "{C0682921-CBBB-4531-8610-2EA514A908D0}";

        internal const string PlagueMaskDisplayName = "MortalUsherPlagueMask.Name";
        private const string PlagueMaskDescription = "MortalUsherPlagueMask.Description";
        public static BlueprintFeature PlagueMaskFeature()
        {
            var icon = AbilityRefs.FalseLife.Reference.Get().Icon;
            return FeatureConfigurator.New(PlagueMask, PlagueMaskGuid)
              .SetDisplayName(PlagueMaskDisplayName)
              .SetDescription(PlagueMaskDescription)
              .SetIcon(icon)
              .AddFacts(new() { AbilityRefs.Invisibility.ToString() })
              .Configure();
        }

        private const string ProficiencyScythes = "MortalUsherProficiencyScythes";
        public static readonly string ProficiencyScythesGuid = "{31A91ACF-DEA1-44AC-9A52-EDAB47B6DDFF}";

        internal const string ProficiencyScythesDisplayName = "MortalUsherProficiencyScythes.Name";
        private const string ProficiencyScythesDescription = "MortalUsherProficiencyScythes.Description";
        public static BlueprintFeature ProficiencyScythesFeature()
        {
            return FeatureConfigurator.New(ProficiencyScythes, ProficiencyScythesGuid)
              .SetDisplayName(ProficiencyScythesDisplayName)
              .SetDescription(ProficiencyScythesDescription)
              .AddProficiencies(weaponProficiencies: new WeaponCategory[] { WeaponCategory.Scythe })
              .Configure();
        }

        private const string ReapingStrike = "MortalUsherReapingStrike";
        public static readonly string ReapingStrikeGuid = "{D32E9839-D818-41CD-97BA-43E6AD842B82}";

        internal const string ReapingStrikeDisplayName = "MortalUsherReapingStrike.Name";
        private const string ReapingStrikeDescription = "MortalUsherReapingStrike.Description";
        public static BlueprintFeature ReapingStrikeFeature()
        {
            var icon = AbilityRefs.BrokenPhylacteryBodyScytheAbility.Reference.Get().Icon;

            return FeatureConfigurator.New(ReapingStrike, ReapingStrikeGuid)
              .SetDisplayName(ReapingStrikeDisplayName)
              .SetDescription(ReapingStrikeDescription)
              .SetIcon(icon)
              .AddComponent<ReapingStrikeComp>()
              .SetRanks(10)
              .Configure();
        }

        private const string AlignSpam = "MortalUsher.AlignSpam";
        public static readonly string AlignSpamGuid = "{7BD7F5AA-1A31-49D2-8414-6D4C6FA90504}";

        internal const string SanctifiedRogueDisplayName = "MortalUsherSanctifiedRogue.Name";
        private const string SanctifiedRogueDescription = "MortalUsherSanctifiedRogue.Description";

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
            string ExaltedEvangelistClass = ExaltedEvangelist.ArchetypeGuid;
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
            //string MortalUsherClass = MortalUsher.ArchetypeGuid;
            string ForestWardenClass = ForestWarden.ArchetypeGuid;

            var list = new List<BlueprintFeature>();

            var MortalAlchemistClasspro = ProgressionConfigurator.New(MortalAlchemistClass0Align, MortalAlchemistClass0AlignGuid)
            .SetDisplayName(MortalAlchemistClass0AlignDisplayName)
            .SetDescription(MortalAlchemistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AlchemistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalAlchemistClass2Align, MortalAlchemistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAlchemistClass4Align, MortalAlchemistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAlchemistClass6Align, MortalAlchemistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAlchemistClass8Align, MortalAlchemistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalAlchemistClass10Align, MortalAlchemistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalAlchemistClasspro.Configure());
            var MortalArcaneTricksterClasspro = ProgressionConfigurator.New(MortalArcaneTricksterClass0Align, MortalArcaneTricksterClass0AlignGuid)
            .SetDisplayName(MortalArcaneTricksterClass0AlignDisplayName)
            .SetDescription(MortalArcaneTricksterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcaneTricksterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalArcaneTricksterClass2Align, MortalArcaneTricksterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcaneTricksterClass4Align, MortalArcaneTricksterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcaneTricksterClass6Align, MortalArcaneTricksterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcaneTricksterClass8Align, MortalArcaneTricksterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalArcaneTricksterClass10Align, MortalArcaneTricksterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalArcaneTricksterClasspro.Configure());
            var MortalArcanistClasspro = ProgressionConfigurator.New(MortalArcanistClass0Align, MortalArcanistClass0AlignGuid)
            .SetDisplayName(MortalArcanistClass0AlignDisplayName)
            .SetDescription(MortalArcanistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcanistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalArcanistClass2Align, MortalArcanistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcanistClass4Align, MortalArcanistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcanistClass6Align, MortalArcanistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcanistClass8Align, MortalArcanistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalArcanistClass10Align, MortalArcanistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalArcanistClasspro.Configure());
            var MortalAssassinClasspro = ProgressionConfigurator.New(MortalAssassinClass0Align, MortalAssassinClass0AlignGuid)
            .SetDisplayName(MortalAssassinClass0AlignDisplayName)
            .SetDescription(MortalAssassinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AssassinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalAssassinClass2Align, MortalAssassinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAssassinClass4Align, MortalAssassinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAssassinClass6Align, MortalAssassinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAssassinClass8Align, MortalAssassinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalAssassinClass10Align, MortalAssassinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalAssassinClasspro.Configure());
            var MortalBarbarianClasspro = ProgressionConfigurator.New(MortalBarbarianClass0Align, MortalBarbarianClass0AlignGuid)
            .SetDisplayName(MortalBarbarianClass0AlignDisplayName)
            .SetDescription(MortalBarbarianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalBarbarianClass2Align, MortalBarbarianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBarbarianClass4Align, MortalBarbarianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBarbarianClass6Align, MortalBarbarianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBarbarianClass8Align, MortalBarbarianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalBarbarianClass10Align, MortalBarbarianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalBarbarianClasspro.Configure());
            var MortalBardClasspro = ProgressionConfigurator.New(MortalBardClass0Align, MortalBardClass0AlignGuid)
            .SetDisplayName(MortalBardClass0AlignDisplayName)
            .SetDescription(MortalBardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalBardClass2Align, MortalBardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBardClass4Align, MortalBardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBardClass6Align, MortalBardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBardClass8Align, MortalBardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalBardClass10Align, MortalBardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalBardClasspro.Configure());
            var MortalBloodragerClasspro = ProgressionConfigurator.New(MortalBloodragerClass0Align, MortalBloodragerClass0AlignGuid)
            .SetDisplayName(MortalBloodragerClass0AlignDisplayName)
            .SetDescription(MortalBloodragerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BloodragerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalBloodragerClass2Align, MortalBloodragerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBloodragerClass4Align, MortalBloodragerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBloodragerClass6Align, MortalBloodragerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBloodragerClass8Align, MortalBloodragerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalBloodragerClass10Align, MortalBloodragerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalBloodragerClasspro.Configure());
            var MortalCavalierClasspro = ProgressionConfigurator.New(MortalCavalierClass0Align, MortalCavalierClass0AlignGuid)
            .SetDisplayName(MortalCavalierClass0AlignDisplayName)
            .SetDescription(MortalCavalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.CavalierClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalCavalierClass2Align, MortalCavalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalCavalierClass4Align, MortalCavalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalCavalierClass6Align, MortalCavalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalCavalierClass8Align, MortalCavalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalCavalierClass10Align, MortalCavalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalCavalierClasspro.Configure());
            var MortalClericClasspro = ProgressionConfigurator.New(MortalClericClass0Align, MortalClericClass0AlignGuid)
            .SetDisplayName(MortalClericClass0AlignDisplayName)
            .SetDescription(MortalClericClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalClericClass2Align, MortalClericClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalClericClass4Align, MortalClericClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalClericClass6Align, MortalClericClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalClericClass8Align, MortalClericClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalClericClass10Align, MortalClericClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalClericClasspro.Configure());
            var MortalDragonDiscipleClasspro = ProgressionConfigurator.New(MortalDragonDiscipleClass0Align, MortalDragonDiscipleClass0AlignGuid)
            .SetDisplayName(MortalDragonDiscipleClass0AlignDisplayName)
            .SetDescription(MortalDragonDiscipleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DragonDiscipleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDragonDiscipleClass2Align, MortalDragonDiscipleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDragonDiscipleClass4Align, MortalDragonDiscipleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDragonDiscipleClass6Align, MortalDragonDiscipleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDragonDiscipleClass8Align, MortalDragonDiscipleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDragonDiscipleClass10Align, MortalDragonDiscipleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalDragonDiscipleClasspro.Configure());
            var MortalDruidClasspro = ProgressionConfigurator.New(MortalDruidClass0Align, MortalDruidClass0AlignGuid)
            .SetDisplayName(MortalDruidClass0AlignDisplayName)
            .SetDescription(MortalDruidClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DruidClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDruidClass2Align, MortalDruidClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDruidClass4Align, MortalDruidClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDruidClass6Align, MortalDruidClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDruidClass8Align, MortalDruidClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDruidClass10Align, MortalDruidClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalDruidClasspro.Configure());
            var MortalDuelistClasspro = ProgressionConfigurator.New(MortalDuelistClass0Align, MortalDuelistClass0AlignGuid)
            .SetDisplayName(MortalDuelistClass0AlignDisplayName)
            .SetDescription(MortalDuelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DuelistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDuelistClass2Align, MortalDuelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDuelistClass4Align, MortalDuelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDuelistClass6Align, MortalDuelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDuelistClass8Align, MortalDuelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDuelistClass10Align, MortalDuelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalDuelistClasspro.Configure());
            var MortalEldritchKnightClasspro = ProgressionConfigurator.New(MortalEldritchKnightClass0Align, MortalEldritchKnightClass0AlignGuid)
            .SetDisplayName(MortalEldritchKnightClass0AlignDisplayName)
            .SetDescription(MortalEldritchKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchKnightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalEldritchKnightClass2Align, MortalEldritchKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEldritchKnightClass4Align, MortalEldritchKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEldritchKnightClass6Align, MortalEldritchKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEldritchKnightClass8Align, MortalEldritchKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalEldritchKnightClass10Align, MortalEldritchKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalEldritchKnightClasspro.Configure());
            var MortalEldritchScionClasspro = ProgressionConfigurator.New(MortalEldritchScionClass0Align, MortalEldritchScionClass0AlignGuid)
            .SetDisplayName(MortalEldritchScionClass0AlignDisplayName)
            .SetDescription(MortalEldritchScionClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchScionClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalEldritchScionClass2Align, MortalEldritchScionClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEldritchScionClass4Align, MortalEldritchScionClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEldritchScionClass6Align, MortalEldritchScionClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEldritchScionClass8Align, MortalEldritchScionClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalEldritchScionClass10Align, MortalEldritchScionClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalEldritchScionClasspro.Configure());
            var MortalFighterClasspro = ProgressionConfigurator.New(MortalFighterClass0Align, MortalFighterClass0AlignGuid)
            .SetDisplayName(MortalFighterClass0AlignDisplayName)
            .SetDescription(MortalFighterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.FighterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalFighterClass2Align, MortalFighterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalFighterClass4Align, MortalFighterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalFighterClass6Align, MortalFighterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalFighterClass8Align, MortalFighterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalFighterClass10Align, MortalFighterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalFighterClasspro.Configure());
            var MortalHellknightClasspro = ProgressionConfigurator.New(MortalHellknightClass0Align, MortalHellknightClass0AlignGuid)
            .SetDisplayName(MortalHellknightClass0AlignDisplayName)
            .SetDescription(MortalHellknightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHellknightClass2Align, MortalHellknightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHellknightClass4Align, MortalHellknightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHellknightClass6Align, MortalHellknightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHellknightClass8Align, MortalHellknightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHellknightClass10Align, MortalHellknightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalHellknightClasspro.Configure());
            var MortalHellknightSigniferClasspro = ProgressionConfigurator.New(MortalHellknightSigniferClass0Align, MortalHellknightSigniferClass0AlignGuid)
            .SetDisplayName(MortalHellknightSigniferClass0AlignDisplayName)
            .SetDescription(MortalHellknightSigniferClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightSigniferClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHellknightSigniferClass2Align, MortalHellknightSigniferClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHellknightSigniferClass4Align, MortalHellknightSigniferClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHellknightSigniferClass6Align, MortalHellknightSigniferClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHellknightSigniferClass8Align, MortalHellknightSigniferClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHellknightSigniferClass10Align, MortalHellknightSigniferClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalHellknightSigniferClasspro.Configure());
            var MortalHunterClasspro = ProgressionConfigurator.New(MortalHunterClass0Align, MortalHunterClass0AlignGuid)
            .SetDisplayName(MortalHunterClass0AlignDisplayName)
            .SetDescription(MortalHunterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HunterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHunterClass2Align, MortalHunterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHunterClass4Align, MortalHunterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHunterClass6Align, MortalHunterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHunterClass8Align, MortalHunterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHunterClass10Align, MortalHunterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalHunterClasspro.Configure());
            var MortalInquisitorClasspro = ProgressionConfigurator.New(MortalInquisitorClass0Align, MortalInquisitorClass0AlignGuid)
            .SetDisplayName(MortalInquisitorClass0AlignDisplayName)
            .SetDescription(MortalInquisitorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.InquisitorClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalInquisitorClass2Align, MortalInquisitorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalInquisitorClass4Align, MortalInquisitorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalInquisitorClass6Align, MortalInquisitorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalInquisitorClass8Align, MortalInquisitorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalInquisitorClass10Align, MortalInquisitorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalInquisitorClasspro.Configure());
            var MortalKineticistClasspro = ProgressionConfigurator.New(MortalKineticistClass0Align, MortalKineticistClass0AlignGuid)
            .SetDisplayName(MortalKineticistClass0AlignDisplayName)
            .SetDescription(MortalKineticistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalKineticistClass2Align, MortalKineticistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalKineticistClass4Align, MortalKineticistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalKineticistClass6Align, MortalKineticistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalKineticistClass8Align, MortalKineticistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalKineticistClass10Align, MortalKineticistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalKineticistClasspro.Configure());
            var MortalLoremasterClasspro = ProgressionConfigurator.New(MortalLoremasterClass0Align, MortalLoremasterClass0AlignGuid)
            .SetDisplayName(MortalLoremasterClass0AlignDisplayName)
            .SetDescription(MortalLoremasterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.LoremasterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalLoremasterClass2Align, MortalLoremasterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalLoremasterClass4Align, MortalLoremasterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalLoremasterClass6Align, MortalLoremasterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalLoremasterClass8Align, MortalLoremasterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalLoremasterClass10Align, MortalLoremasterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalLoremasterClasspro.Configure());
            var MortalMagusClasspro = ProgressionConfigurator.New(MortalMagusClass0Align, MortalMagusClass0AlignGuid)
            .SetDisplayName(MortalMagusClass0AlignDisplayName)
            .SetDescription(MortalMagusClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalMagusClass2Align, MortalMagusClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMagusClass4Align, MortalMagusClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMagusClass6Align, MortalMagusClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMagusClass8Align, MortalMagusClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalMagusClass10Align, MortalMagusClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalMagusClasspro.Configure());
            var MortalMonkClasspro = ProgressionConfigurator.New(MortalMonkClass0Align, MortalMonkClass0AlignGuid)
            .SetDisplayName(MortalMonkClass0AlignDisplayName)
            .SetDescription(MortalMonkClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalMonkClass2Align, MortalMonkClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMonkClass4Align, MortalMonkClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMonkClass6Align, MortalMonkClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMonkClass8Align, MortalMonkClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalMonkClass10Align, MortalMonkClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalMonkClasspro.Configure());
            var MortalMysticTheurgeClasspro = ProgressionConfigurator.New(MortalMysticTheurgeClass0Align, MortalMysticTheurgeClass0AlignGuid)
            .SetDisplayName(MortalMysticTheurgeClass0AlignDisplayName)
            .SetDescription(MortalMysticTheurgeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MysticTheurgeClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalMysticTheurgeClass2Align, MortalMysticTheurgeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMysticTheurgeClass4Align, MortalMysticTheurgeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMysticTheurgeClass6Align, MortalMysticTheurgeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMysticTheurgeClass8Align, MortalMysticTheurgeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalMysticTheurgeClass10Align, MortalMysticTheurgeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalMysticTheurgeClasspro.Configure());
            var MortalOracleClasspro = ProgressionConfigurator.New(MortalOracleClass0Align, MortalOracleClass0AlignGuid)
            .SetDisplayName(MortalOracleClass0AlignDisplayName)
            .SetDescription(MortalOracleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.OracleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalOracleClass2Align, MortalOracleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalOracleClass4Align, MortalOracleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalOracleClass6Align, MortalOracleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalOracleClass8Align, MortalOracleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalOracleClass10Align, MortalOracleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalOracleClasspro.Configure());
            var MortalPaladinClasspro = ProgressionConfigurator.New(MortalPaladinClass0Align, MortalPaladinClass0AlignGuid)
            .SetDisplayName(MortalPaladinClass0AlignDisplayName)
            .SetDescription(MortalPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.PaladinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalPaladinClass2Align, MortalPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalPaladinClass4Align, MortalPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalPaladinClass6Align, MortalPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalPaladinClass8Align, MortalPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalPaladinClass10Align, MortalPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalPaladinClasspro.Configure());
            var MortalRangerClasspro = ProgressionConfigurator.New(MortalRangerClass0Align, MortalRangerClass0AlignGuid)
            .SetDisplayName(MortalRangerClass0AlignDisplayName)
            .SetDescription(MortalRangerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RangerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalRangerClass2Align, MortalRangerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalRangerClass4Align, MortalRangerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalRangerClass6Align, MortalRangerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalRangerClass8Align, MortalRangerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalRangerClass10Align, MortalRangerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalRangerClasspro.Configure());
            var MortalRogueClasspro = ProgressionConfigurator.New(MortalRogueClass0Align, MortalRogueClass0AlignGuid)
            .SetDisplayName(MortalRogueClass0AlignDisplayName)
            .SetDescription(MortalRogueClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RogueClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalRogueClass2Align, MortalRogueClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalRogueClass4Align, MortalRogueClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalRogueClass6Align, MortalRogueClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalRogueClass8Align, MortalRogueClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalRogueClass10Align, MortalRogueClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalRogueClasspro.Configure());
            var MortalShamanClasspro = ProgressionConfigurator.New(MortalShamanClass0Align, MortalShamanClass0AlignGuid)
            .SetDisplayName(MortalShamanClass0AlignDisplayName)
            .SetDescription(MortalShamanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShamanClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalShamanClass2Align, MortalShamanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShamanClass4Align, MortalShamanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShamanClass6Align, MortalShamanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShamanClass8Align, MortalShamanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalShamanClass10Align, MortalShamanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalShamanClasspro.Configure());
            var MortalShifterClasspro = ProgressionConfigurator.New(MortalShifterClass0Align, MortalShifterClass0AlignGuid)
            .SetDisplayName(MortalShifterClass0AlignDisplayName)
            .SetDescription(MortalShifterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShifterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalShifterClass2Align, MortalShifterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShifterClass4Align, MortalShifterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShifterClass6Align, MortalShifterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShifterClass8Align, MortalShifterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalShifterClass10Align, MortalShifterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalShifterClasspro.Configure());
            var MortalSkaldClasspro = ProgressionConfigurator.New(MortalSkaldClass0Align, MortalSkaldClass0AlignGuid)
            .SetDisplayName(MortalSkaldClass0AlignDisplayName)
            .SetDescription(MortalSkaldClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SkaldClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSkaldClass2Align, MortalSkaldClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSkaldClass4Align, MortalSkaldClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSkaldClass6Align, MortalSkaldClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSkaldClass8Align, MortalSkaldClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSkaldClass10Align, MortalSkaldClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalSkaldClasspro.Configure());
            var MortalSlayerClasspro = ProgressionConfigurator.New(MortalSlayerClass0Align, MortalSlayerClass0AlignGuid)
            .SetDisplayName(MortalSlayerClass0AlignDisplayName)
            .SetDescription(MortalSlayerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SlayerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSlayerClass2Align, MortalSlayerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSlayerClass4Align, MortalSlayerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSlayerClass6Align, MortalSlayerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSlayerClass8Align, MortalSlayerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSlayerClass10Align, MortalSlayerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalSlayerClasspro.Configure());
            var MortalSorcererClasspro = ProgressionConfigurator.New(MortalSorcererClass0Align, MortalSorcererClass0AlignGuid)
            .SetDisplayName(MortalSorcererClass0AlignDisplayName)
            .SetDescription(MortalSorcererClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SorcererClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSorcererClass2Align, MortalSorcererClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSorcererClass4Align, MortalSorcererClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSorcererClass6Align, MortalSorcererClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSorcererClass8Align, MortalSorcererClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSorcererClass10Align, MortalSorcererClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalSorcererClasspro.Configure());
            var MortalStalwartDefenderClasspro = ProgressionConfigurator.New(MortalStalwartDefenderClass0Align, MortalStalwartDefenderClass0AlignGuid)
            .SetDisplayName(MortalStalwartDefenderClass0AlignDisplayName)
            .SetDescription(MortalStalwartDefenderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StalwartDefenderClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalStalwartDefenderClass2Align, MortalStalwartDefenderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStalwartDefenderClass4Align, MortalStalwartDefenderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStalwartDefenderClass6Align, MortalStalwartDefenderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStalwartDefenderClass8Align, MortalStalwartDefenderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalStalwartDefenderClass10Align, MortalStalwartDefenderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalStalwartDefenderClasspro.Configure());
            var MortalStudentOfWarClasspro = ProgressionConfigurator.New(MortalStudentOfWarClass0Align, MortalStudentOfWarClass0AlignGuid)
            .SetDisplayName(MortalStudentOfWarClass0AlignDisplayName)
            .SetDescription(MortalStudentOfWarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StudentOfWarClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalStudentOfWarClass2Align, MortalStudentOfWarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStudentOfWarClass4Align, MortalStudentOfWarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStudentOfWarClass6Align, MortalStudentOfWarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStudentOfWarClass8Align, MortalStudentOfWarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalStudentOfWarClass10Align, MortalStudentOfWarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalStudentOfWarClasspro.Configure());
            var MortalSwordlordClasspro = ProgressionConfigurator.New(MortalSwordlordClass0Align, MortalSwordlordClass0AlignGuid)
            .SetDisplayName(MortalSwordlordClass0AlignDisplayName)
            .SetDescription(MortalSwordlordClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SwordlordClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSwordlordClass2Align, MortalSwordlordClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSwordlordClass4Align, MortalSwordlordClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSwordlordClass6Align, MortalSwordlordClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSwordlordClass8Align, MortalSwordlordClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSwordlordClass10Align, MortalSwordlordClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalSwordlordClasspro.Configure());
            var MortalWarpriestClasspro = ProgressionConfigurator.New(MortalWarpriestClass0Align, MortalWarpriestClass0AlignGuid)
            .SetDisplayName(MortalWarpriestClass0AlignDisplayName)
            .SetDescription(MortalWarpriestClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WarpriestClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalWarpriestClass2Align, MortalWarpriestClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWarpriestClass4Align, MortalWarpriestClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWarpriestClass6Align, MortalWarpriestClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWarpriestClass8Align, MortalWarpriestClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalWarpriestClass10Align, MortalWarpriestClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalWarpriestClasspro.Configure());
            var MortalWinterWitchClasspro = ProgressionConfigurator.New(MortalWinterWitchClass0Align, MortalWinterWitchClass0AlignGuid)
            .SetDisplayName(MortalWinterWitchClass0AlignDisplayName)
            .SetDescription(MortalWinterWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WinterWitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalWinterWitchClass2Align, MortalWinterWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWinterWitchClass4Align, MortalWinterWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWinterWitchClass6Align, MortalWinterWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWinterWitchClass8Align, MortalWinterWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalWinterWitchClass10Align, MortalWinterWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalWinterWitchClasspro.Configure());
            var MortalWitchClasspro = ProgressionConfigurator.New(MortalWitchClass0Align, MortalWitchClass0AlignGuid)
            .SetDisplayName(MortalWitchClass0AlignDisplayName)
            .SetDescription(MortalWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalWitchClass2Align, MortalWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWitchClass4Align, MortalWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWitchClass6Align, MortalWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWitchClass8Align, MortalWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalWitchClass10Align, MortalWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalWitchClasspro.Configure());
            var MortalWizardClasspro = ProgressionConfigurator.New(MortalWizardClass0Align, MortalWizardClass0AlignGuid)
            .SetDisplayName(MortalWizardClass0AlignDisplayName)
            .SetDescription(MortalWizardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WizardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalWizardClass2Align, MortalWizardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWizardClass4Align, MortalWizardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWizardClass6Align, MortalWizardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWizardClass8Align, MortalWizardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalWizardClass10Align, MortalWizardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(MortalWizardClasspro.Configure());
            var MortalGunslingerClasspro = ProgressionConfigurator.New(MortalGunslingerClass0Align, MortalGunslingerClass0AlignGuid)
            .SetDisplayName(MortalGunslingerClass0AlignDisplayName)
            .SetDescription(MortalGunslingerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GunslingerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalGunslingerClass2Align, MortalGunslingerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalGunslingerClass4Align, MortalGunslingerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalGunslingerClass6Align, MortalGunslingerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalGunslingerClass8Align, MortalGunslingerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalGunslingerClass10Align, MortalGunslingerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalGunslingerClasspro.Configure());
            var MortalAgentoftheGraveClasspro = ProgressionConfigurator.New(MortalAgentoftheGraveClass0Align, MortalAgentoftheGraveClass0AlignGuid)
            .SetDisplayName(MortalAgentoftheGraveClass0AlignDisplayName)
            .SetDescription(MortalAgentoftheGraveClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AgentoftheGraveClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalAgentoftheGraveClass2Align, MortalAgentoftheGraveClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAgentoftheGraveClass4Align, MortalAgentoftheGraveClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAgentoftheGraveClass6Align, MortalAgentoftheGraveClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAgentoftheGraveClass8Align, MortalAgentoftheGraveClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalAgentoftheGraveClass10Align, MortalAgentoftheGraveClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalAgentoftheGraveClasspro.Configure());
            var MortalAnchoriteofDawnClasspro = ProgressionConfigurator.New(MortalAnchoriteofDawnClass0Align, MortalAnchoriteofDawnClass0AlignGuid)
            .SetDisplayName(MortalAnchoriteofDawnClass0AlignDisplayName)
            .SetDescription(MortalAnchoriteofDawnClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AnchoriteofDawnClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass2Align, MortalAnchoriteofDawnClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass4Align, MortalAnchoriteofDawnClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass6Align, MortalAnchoriteofDawnClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass8Align, MortalAnchoriteofDawnClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass10Align, MortalAnchoriteofDawnClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalAnchoriteofDawnClasspro.Configure());
            var MortalArcaneAcherClasspro = ProgressionConfigurator.New(MortalArcaneAcherClass0Align, MortalArcaneAcherClass0AlignGuid)
            .SetDisplayName(MortalArcaneAcherClass0AlignDisplayName)
            .SetDescription(MortalArcaneAcherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ArcaneAcherClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalArcaneAcherClass2Align, MortalArcaneAcherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcaneAcherClass4Align, MortalArcaneAcherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcaneAcherClass6Align, MortalArcaneAcherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcaneAcherClass8Align, MortalArcaneAcherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalArcaneAcherClass10Align, MortalArcaneAcherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalArcaneAcherClasspro.Configure());
            var MortalAsavirClasspro = ProgressionConfigurator.New(MortalAsavirClass0Align, MortalAsavirClass0AlignGuid)
            .SetDisplayName(MortalAsavirClass0AlignDisplayName)
            .SetDescription(MortalAsavirClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AsavirClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalAsavirClass2Align, MortalAsavirClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAsavirClass4Align, MortalAsavirClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAsavirClass6Align, MortalAsavirClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAsavirClass8Align, MortalAsavirClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalAsavirClass10Align, MortalAsavirClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalAsavirClasspro.Configure());
            var MortalChevalierClasspro = ProgressionConfigurator.New(MortalChevalierClass0Align, MortalChevalierClass0AlignGuid)
            .SetDisplayName(MortalChevalierClass0AlignDisplayName)
            .SetDescription(MortalChevalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ChevalierClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalChevalierClass2Align, MortalChevalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalChevalierClass4Align, MortalChevalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalChevalierClass6Align, MortalChevalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalChevalierClass8Align, MortalChevalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalChevalierClass10Align, MortalChevalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalChevalierClasspro.Configure());
            var MortalCrimsonTemplarClasspro = ProgressionConfigurator.New(MortalCrimsonTemplarClass0Align, MortalCrimsonTemplarClass0AlignGuid)
            .SetDisplayName(MortalCrimsonTemplarClass0AlignDisplayName)
            .SetDescription(MortalCrimsonTemplarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CrimsonTemplarClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalCrimsonTemplarClass2Align, MortalCrimsonTemplarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalCrimsonTemplarClass4Align, MortalCrimsonTemplarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalCrimsonTemplarClass6Align, MortalCrimsonTemplarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalCrimsonTemplarClass8Align, MortalCrimsonTemplarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalCrimsonTemplarClass10Align, MortalCrimsonTemplarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalCrimsonTemplarClasspro.Configure());
            var MortalDeadeyeDevoteeClasspro = ProgressionConfigurator.New(MortalDeadeyeDevoteeClass0Align, MortalDeadeyeDevoteeClass0AlignGuid)
            .SetDisplayName(MortalDeadeyeDevoteeClass0AlignDisplayName)
            .SetDescription(MortalDeadeyeDevoteeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DeadeyeDevoteeClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass2Align, MortalDeadeyeDevoteeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass4Align, MortalDeadeyeDevoteeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass6Align, MortalDeadeyeDevoteeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass8Align, MortalDeadeyeDevoteeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass10Align, MortalDeadeyeDevoteeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalDeadeyeDevoteeClasspro.Configure());
            var MortalDragonFuryClasspro = ProgressionConfigurator.New(MortalDragonFuryClass0Align, MortalDragonFuryClass0AlignGuid)
            .SetDisplayName(MortalDragonFuryClass0AlignDisplayName)
            .SetDescription(MortalDragonFuryClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DragonFuryClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDragonFuryClass2Align, MortalDragonFuryClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDragonFuryClass4Align, MortalDragonFuryClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDragonFuryClass6Align, MortalDragonFuryClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDragonFuryClass8Align, MortalDragonFuryClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDragonFuryClass10Align, MortalDragonFuryClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalDragonFuryClasspro.Configure());
            var MortalEsotericKnightClasspro = ProgressionConfigurator.New(MortalEsotericKnightClass0Align, MortalEsotericKnightClass0AlignGuid)
            .SetDisplayName(MortalEsotericKnightClass0AlignDisplayName)
            .SetDescription(MortalEsotericKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EsotericKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalEsotericKnightClass2Align, MortalEsotericKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEsotericKnightClass4Align, MortalEsotericKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEsotericKnightClass6Align, MortalEsotericKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEsotericKnightClass8Align, MortalEsotericKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalEsotericKnightClass10Align, MortalEsotericKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalEsotericKnightClasspro.Configure());
            var MortalExaltedEvangelistClasspro = ProgressionConfigurator.New(MortalExaltedEvangelistClass0Align, MortalExaltedEvangelistClass0AlignGuid)
            .SetDisplayName(MortalExaltedEvangelistClass0AlignDisplayName)
            .SetDescription(MortalExaltedEvangelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ExaltedEvangelistClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalExaltedEvangelistClass2Align, MortalExaltedEvangelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalExaltedEvangelistClass4Align, MortalExaltedEvangelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalExaltedEvangelistClass6Align, MortalExaltedEvangelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalExaltedEvangelistClass8Align, MortalExaltedEvangelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalExaltedEvangelistClass10Align, MortalExaltedEvangelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalExaltedEvangelistClasspro.Configure());
            var MortalFuriousGuardianClasspro = ProgressionConfigurator.New(MortalFuriousGuardianClass0Align, MortalFuriousGuardianClass0AlignGuid)
            .SetDisplayName(MortalFuriousGuardianClass0AlignDisplayName)
            .SetDescription(MortalFuriousGuardianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(FuriousGuardianClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalFuriousGuardianClass2Align, MortalFuriousGuardianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalFuriousGuardianClass4Align, MortalFuriousGuardianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalFuriousGuardianClass6Align, MortalFuriousGuardianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalFuriousGuardianClass8Align, MortalFuriousGuardianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalFuriousGuardianClass10Align, MortalFuriousGuardianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalFuriousGuardianClasspro.Configure());
            var MortalHalflingOpportunistClasspro = ProgressionConfigurator.New(MortalHalflingOpportunistClass0Align, MortalHalflingOpportunistClass0AlignGuid)
            .SetDisplayName(MortalHalflingOpportunistClass0AlignDisplayName)
            .SetDescription(MortalHalflingOpportunistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HalflingOpportunistClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHalflingOpportunistClass2Align, MortalHalflingOpportunistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHalflingOpportunistClass4Align, MortalHalflingOpportunistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHalflingOpportunistClass6Align, MortalHalflingOpportunistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHalflingOpportunistClass8Align, MortalHalflingOpportunistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHalflingOpportunistClass10Align, MortalHalflingOpportunistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalHalflingOpportunistClasspro.Configure());
            var MortalHinterlanderClasspro = ProgressionConfigurator.New(MortalHinterlanderClass0Align, MortalHinterlanderClass0AlignGuid)
            .SetDisplayName(MortalHinterlanderClass0AlignDisplayName)
            .SetDescription(MortalHinterlanderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HinterlanderClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHinterlanderClass2Align, MortalHinterlanderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHinterlanderClass4Align, MortalHinterlanderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHinterlanderClass6Align, MortalHinterlanderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHinterlanderClass8Align, MortalHinterlanderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHinterlanderClass10Align, MortalHinterlanderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalHinterlanderClasspro.Configure());
            var MortalHorizonWalkerClasspro = ProgressionConfigurator.New(MortalHorizonWalkerClass0Align, MortalHorizonWalkerClass0AlignGuid)
            .SetDisplayName(MortalHorizonWalkerClass0AlignDisplayName)
            .SetDescription(MortalHorizonWalkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HorizonWalkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHorizonWalkerClass2Align, MortalHorizonWalkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHorizonWalkerClass4Align, MortalHorizonWalkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHorizonWalkerClass6Align, MortalHorizonWalkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHorizonWalkerClass8Align, MortalHorizonWalkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHorizonWalkerClass10Align, MortalHorizonWalkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalHorizonWalkerClasspro.Configure());
            var MortalInheritorCrusaderClasspro = ProgressionConfigurator.New(MortalInheritorCrusaderClass0Align, MortalInheritorCrusaderClass0AlignGuid)
            .SetDisplayName(MortalInheritorCrusaderClass0AlignDisplayName)
            .SetDescription(MortalInheritorCrusaderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(InheritorCrusaderClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalInheritorCrusaderClass2Align, MortalInheritorCrusaderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalInheritorCrusaderClass4Align, MortalInheritorCrusaderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalInheritorCrusaderClass6Align, MortalInheritorCrusaderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalInheritorCrusaderClass8Align, MortalInheritorCrusaderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalInheritorCrusaderClass10Align, MortalInheritorCrusaderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalInheritorCrusaderClasspro.Configure());
            var MortalMammothRiderClasspro = ProgressionConfigurator.New(MortalMammothRiderClass0Align, MortalMammothRiderClass0AlignGuid)
            .SetDisplayName(MortalMammothRiderClass0AlignDisplayName)
            .SetDescription(MortalMammothRiderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MammothRiderClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalMammothRiderClass2Align, MortalMammothRiderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMammothRiderClass4Align, MortalMammothRiderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMammothRiderClass6Align, MortalMammothRiderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMammothRiderClass8Align, MortalMammothRiderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalMammothRiderClass10Align, MortalMammothRiderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalMammothRiderClasspro.Configure());
            var MortalSanguineAngelClasspro = ProgressionConfigurator.New(MortalSanguineAngelClass0Align, MortalSanguineAngelClass0AlignGuid)
            .SetDisplayName(MortalSanguineAngelClass0AlignDisplayName)
            .SetDescription(MortalSanguineAngelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SanguineAngelClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSanguineAngelClass2Align, MortalSanguineAngelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSanguineAngelClass4Align, MortalSanguineAngelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSanguineAngelClass6Align, MortalSanguineAngelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSanguineAngelClass8Align, MortalSanguineAngelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSanguineAngelClass10Align, MortalSanguineAngelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalSanguineAngelClasspro.Configure());
            var MortalScarSeekerClasspro = ProgressionConfigurator.New(MortalScarSeekerClass0Align, MortalScarSeekerClass0AlignGuid)
            .SetDisplayName(MortalScarSeekerClass0AlignDisplayName)
            .SetDescription(MortalScarSeekerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ScarSeekerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalScarSeekerClass2Align, MortalScarSeekerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalScarSeekerClass4Align, MortalScarSeekerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalScarSeekerClass6Align, MortalScarSeekerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalScarSeekerClass8Align, MortalScarSeekerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalScarSeekerClass10Align, MortalScarSeekerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalScarSeekerClasspro.Configure());
            var MortalSentinelClasspro = ProgressionConfigurator.New(MortalSentinelClass0Align, MortalSentinelClass0AlignGuid)
            .SetDisplayName(MortalSentinelClass0AlignDisplayName)
            .SetDescription(MortalSentinelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SentinelClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSentinelClass2Align, MortalSentinelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSentinelClass4Align, MortalSentinelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSentinelClass6Align, MortalSentinelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSentinelClass8Align, MortalSentinelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSentinelClass10Align, MortalSentinelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalSentinelClasspro.Configure());
            var MortalShadowDancerClasspro = ProgressionConfigurator.New(MortalShadowDancerClass0Align, MortalShadowDancerClass0AlignGuid)
            .SetDisplayName(MortalShadowDancerClass0AlignDisplayName)
            .SetDescription(MortalShadowDancerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ShadowDancerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalShadowDancerClass2Align, MortalShadowDancerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShadowDancerClass4Align, MortalShadowDancerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShadowDancerClass6Align, MortalShadowDancerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShadowDancerClass8Align, MortalShadowDancerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalShadowDancerClass10Align, MortalShadowDancerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalShadowDancerClasspro.Configure());
            var MortalSouldrinkerClasspro = ProgressionConfigurator.New(MortalSouldrinkerClass0Align, MortalSouldrinkerClass0AlignGuid)
            .SetDisplayName(MortalSouldrinkerClass0AlignDisplayName)
            .SetDescription(MortalSouldrinkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SouldrinkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSouldrinkerClass2Align, MortalSouldrinkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSouldrinkerClass4Align, MortalSouldrinkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSouldrinkerClass6Align, MortalSouldrinkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSouldrinkerClass8Align, MortalSouldrinkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSouldrinkerClass10Align, MortalSouldrinkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalSouldrinkerClasspro.Configure());
            var MortalUmbralAgentClasspro = ProgressionConfigurator.New(MortalUmbralAgentClass0Align, MortalUmbralAgentClass0AlignGuid)
            .SetDisplayName(MortalUmbralAgentClass0AlignDisplayName)
            .SetDescription(MortalUmbralAgentClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(UmbralAgentClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalUmbralAgentClass2Align, MortalUmbralAgentClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalUmbralAgentClass4Align, MortalUmbralAgentClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalUmbralAgentClass6Align, MortalUmbralAgentClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalUmbralAgentClass8Align, MortalUmbralAgentClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalUmbralAgentClass10Align, MortalUmbralAgentClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalUmbralAgentClasspro.Configure());
            var MortalMicroAntiPaladinClasspro = ProgressionConfigurator.New(MortalMicroAntiPaladinClass0Align, MortalMicroAntiPaladinClass0AlignGuid)
            .SetDisplayName(MortalMicroAntiPaladinClass0AlignDisplayName)
            .SetDescription(MortalMicroAntiPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MicroAntiPaladinClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass2Align, MortalMicroAntiPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass4Align, MortalMicroAntiPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass6Align, MortalMicroAntiPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass8Align, MortalMicroAntiPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass10Align, MortalMicroAntiPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalMicroAntiPaladinClasspro.Configure());
            var MortalOathbreakerClasspro = ProgressionConfigurator.New(MortalOathbreakerClass0Align, MortalOathbreakerClass0AlignGuid)
            .SetDisplayName(MortalOathbreakerClass0AlignDisplayName)
            .SetDescription(MortalOathbreakerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(OathbreakerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalOathbreakerClass2Align, MortalOathbreakerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalOathbreakerClass4Align, MortalOathbreakerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalOathbreakerClass6Align, MortalOathbreakerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalOathbreakerClass8Align, MortalOathbreakerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalOathbreakerClass10Align, MortalOathbreakerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalOathbreakerClasspro.Configure());
            var MortalDreadKnightClasspro = ProgressionConfigurator.New(MortalDreadKnightClass0Align, MortalDreadKnightClass0AlignGuid)
            .SetDisplayName(MortalDreadKnightClass0AlignDisplayName)
            .SetDescription(MortalDreadKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DreadKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalDreadKnightClass2Align, MortalDreadKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDreadKnightClass4Align, MortalDreadKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDreadKnightClass6Align, MortalDreadKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDreadKnightClass8Align, MortalDreadKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalDreadKnightClass10Align, MortalDreadKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalDreadKnightClasspro.Configure());
            var MortalStargazerClasspro = ProgressionConfigurator.New(MortalStargazerClass0Align, MortalStargazerClass0AlignGuid)
            .SetDisplayName(MortalStargazerClass0AlignDisplayName)
            .SetDescription(MortalStargazerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(StargazerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalStargazerClass2Align, MortalStargazerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStargazerClass4Align, MortalStargazerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStargazerClass6Align, MortalStargazerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStargazerClass8Align, MortalStargazerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalStargazerClass10Align, MortalStargazerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalStargazerClasspro.Configure());
            var MortalSwashbucklerClasspro = ProgressionConfigurator.New(MortalSwashbucklerClass0Align, MortalSwashbucklerClass0AlignGuid)
            .SetDisplayName(MortalSwashbucklerClass0AlignDisplayName)
            .SetDescription(MortalSwashbucklerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SwashbucklerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSwashbucklerClass2Align, MortalSwashbucklerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSwashbucklerClass4Align, MortalSwashbucklerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSwashbucklerClass6Align, MortalSwashbucklerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSwashbucklerClass8Align, MortalSwashbucklerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSwashbucklerClass10Align, MortalSwashbucklerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalSwashbucklerClasspro.Configure());
            var MortalHolyVindicatorClasspro = ProgressionConfigurator.New(MortalHolyVindicatorClass0Align, MortalHolyVindicatorClass0AlignGuid)
            .SetDisplayName(MortalHolyVindicatorClass0AlignDisplayName)
            .SetDescription(MortalHolyVindicatorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HolyVindicatorClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHolyVindicatorClass2Align, MortalHolyVindicatorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHolyVindicatorClass4Align, MortalHolyVindicatorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHolyVindicatorClass6Align, MortalHolyVindicatorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHolyVindicatorClass8Align, MortalHolyVindicatorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHolyVindicatorClass10Align, MortalHolyVindicatorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalHolyVindicatorClasspro.Configure());
            var MortalSummonerClasspro = ProgressionConfigurator.New(MortalSummonerClass0Align, MortalSummonerClass0AlignGuid)
            .SetDisplayName(MortalSummonerClass0AlignDisplayName)
            .SetDescription(MortalSummonerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SummonerClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalSummonerClass2Align, MortalSummonerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSummonerClass4Align, MortalSummonerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSummonerClass6Align, MortalSummonerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSummonerClass8Align, MortalSummonerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalSummonerClass10Align, MortalSummonerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalSummonerClasspro.Configure());
            var MortalLionBladeClasspro = ProgressionConfigurator.New(MortalLionBladeClass0Align, MortalLionBladeClass0AlignGuid)
            .SetDisplayName(MortalLionBladeClass0AlignDisplayName)
            .SetDescription(MortalLionBladeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(LionBladeClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalLionBladeClass2Align, MortalLionBladeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalLionBladeClass4Align, MortalLionBladeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalLionBladeClass6Align, MortalLionBladeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalLionBladeClass8Align, MortalLionBladeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalLionBladeClass10Align, MortalLionBladeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalLionBladeClasspro.Configure());
            var MortalEnchantingCourtesanClasspro = ProgressionConfigurator.New(MortalEnchantingCourtesanClass0Align, MortalEnchantingCourtesanClass0AlignGuid)
            .SetDisplayName(MortalEnchantingCourtesanClass0AlignDisplayName)
            .SetDescription(MortalEnchantingCourtesanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EnchantingCourtesanClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass2Align, MortalEnchantingCourtesanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass4Align, MortalEnchantingCourtesanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass6Align, MortalEnchantingCourtesanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass8Align, MortalEnchantingCourtesanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass10Align, MortalEnchantingCourtesanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalEnchantingCourtesanClasspro.Configure());
            var MortalHeritorKnightClasspro = ProgressionConfigurator.New(MortalHeritorKnightClass0Align, MortalHeritorKnightClass0AlignGuid)
            .SetDisplayName(MortalHeritorKnightClass0AlignDisplayName)
            .SetDescription(MortalHeritorKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HeritorKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalHeritorKnightClass2Align, MortalHeritorKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHeritorKnightClass4Align, MortalHeritorKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHeritorKnightClass6Align, MortalHeritorKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHeritorKnightClass8Align, MortalHeritorKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalHeritorKnightClass10Align, MortalHeritorKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalHeritorKnightClasspro.Configure());
            var MortalGoldenLegionnaireClasspro = ProgressionConfigurator.New(MortalGoldenLegionnaireClass0Align, MortalGoldenLegionnaireClass0AlignGuid)
            .SetDisplayName(MortalGoldenLegionnaireClass0AlignDisplayName)
            .SetDescription(MortalGoldenLegionnaireClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GoldenLegionnaireClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass2Align, MortalGoldenLegionnaireClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass4Align, MortalGoldenLegionnaireClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass6Align, MortalGoldenLegionnaireClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass8Align, MortalGoldenLegionnaireClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass10Align, MortalGoldenLegionnaireClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalGoldenLegionnaireClasspro.Configure());
            var MortalBoltAceClasspro = ProgressionConfigurator.New(MortalBoltAceClass0Align, MortalBoltAceClass0AlignGuid)
            .SetDisplayName(MortalBoltAceClass0AlignDisplayName)
            .SetDescription(MortalBoltAceClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(BoltAceClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalBoltAceClass2Align, MortalBoltAceClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBoltAceClass4Align, MortalBoltAceClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBoltAceClass6Align, MortalBoltAceClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBoltAceClass8Align, MortalBoltAceClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalBoltAceClass10Align, MortalBoltAceClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalBoltAceClasspro.Configure());
            var MortalForestWardenClasspro = ProgressionConfigurator.New(MortalForestWardenClass0Align, MortalForestWardenClass0AlignGuid)
            .SetDisplayName(MortalForestWardenClass0AlignDisplayName)
            .SetDescription(MortalForestWardenClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ForestWardenClass, 1)
            .SetHideNotAvailibleInUI(true);
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(MortalForestWardenClass2Align, MortalForestWardenClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalForestWardenClass4Align, MortalForestWardenClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalForestWardenClass6Align, MortalForestWardenClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalForestWardenClass8Align, MortalForestWardenClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(MortalForestWardenClass10Align, MortalForestWardenClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            list.Add(MortalForestWardenClasspro.Configure());

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

        private const string MortalAlchemistClass0Align = "MortalAlchemistClass0Align";
        private static readonly string MortalAlchemistClass0AlignGuid = "82d246fd-fa03-4d38-9f45-85e15713a915";
        internal const string MortalAlchemistClass0AlignDisplayName = "EvangelistAlchemistClass0Align.Name";
        private const string MortalAlchemistClass0AlignDescription = "EvangelistAlchemistClass0Align.Description";
        private const string MortalAlchemistClass2Align = "MortalAlchemistClass2Align";
        private static readonly string MortalAlchemistClass2AlignGuid = "c9df9ddf-b5ae-4577-a7a9-864ec0e89137";
        private const string MortalAlchemistClass4Align = "MortalAlchemistClass4Align";
        private static readonly string MortalAlchemistClass4AlignGuid = "d5081ba0-d470-4d2e-a2e3-5d73526bce0e";
        private const string MortalAlchemistClass6Align = "MortalAlchemistClass6Align";
        private static readonly string MortalAlchemistClass6AlignGuid = "aa169ba0-e0ed-4058-86ed-2a977f1d8fbb";
        private const string MortalAlchemistClass8Align = "MortalAlchemistClass8Align";
        private static readonly string MortalAlchemistClass8AlignGuid = "1422a5db-236b-456f-b37d-67ca6ab45c47";
        private const string MortalAlchemistClass10Align = "MortalAlchemistClass10Align";
        private static readonly string MortalAlchemistClass10AlignGuid = "087756ab-d0a5-4a40-b769-92656e90a324";
        private const string MortalArcaneTricksterClass0Align = "MortalArcaneTricksterClass0Align";
        private static readonly string MortalArcaneTricksterClass0AlignGuid = "84485684-51d1-4698-a3ba-f9e568f6fc09";
        internal const string MortalArcaneTricksterClass0AlignDisplayName = "EvangelistArcaneTricksterClass0Align.Name";
        private const string MortalArcaneTricksterClass0AlignDescription = "EvangelistArcaneTricksterClass0Align.Description";
        private const string MortalArcaneTricksterClass2Align = "MortalArcaneTricksterClass2Align";
        private static readonly string MortalArcaneTricksterClass2AlignGuid = "dac9eb1f-914c-478a-95bf-8f50fff529ea";
        private const string MortalArcaneTricksterClass4Align = "MortalArcaneTricksterClass4Align";
        private static readonly string MortalArcaneTricksterClass4AlignGuid = "7f5aac2c-a6b8-4613-bd4e-ca23da3d9cdd";
        private const string MortalArcaneTricksterClass6Align = "MortalArcaneTricksterClass6Align";
        private static readonly string MortalArcaneTricksterClass6AlignGuid = "f52459f2-9158-4625-8e44-39da1d762edc";
        private const string MortalArcaneTricksterClass8Align = "MortalArcaneTricksterClass8Align";
        private static readonly string MortalArcaneTricksterClass8AlignGuid = "95c549da-2d33-490c-a616-1effaa97c39c";
        private const string MortalArcaneTricksterClass10Align = "MortalArcaneTricksterClass10Align";
        private static readonly string MortalArcaneTricksterClass10AlignGuid = "0e6e2e2f-00f7-49ab-ae83-ee36f765d390";
        private const string MortalArcanistClass0Align = "MortalArcanistClass0Align";
        private static readonly string MortalArcanistClass0AlignGuid = "40044583-1b67-4fbe-92b5-c3fd4a0d73f3";
        internal const string MortalArcanistClass0AlignDisplayName = "EvangelistArcanistClass0Align.Name";
        private const string MortalArcanistClass0AlignDescription = "EvangelistArcanistClass0Align.Description";
        private const string MortalArcanistClass2Align = "MortalArcanistClass2Align";
        private static readonly string MortalArcanistClass2AlignGuid = "aada0d76-bcd4-4d45-b312-4289f4e6af2d";
        private const string MortalArcanistClass4Align = "MortalArcanistClass4Align";
        private static readonly string MortalArcanistClass4AlignGuid = "2b62322d-22eb-4b0c-b2f6-6552c9959007";
        private const string MortalArcanistClass6Align = "MortalArcanistClass6Align";
        private static readonly string MortalArcanistClass6AlignGuid = "969adaec-5cc6-403d-881c-3a357c5378a4";
        private const string MortalArcanistClass8Align = "MortalArcanistClass8Align";
        private static readonly string MortalArcanistClass8AlignGuid = "31f94089-eb53-4d51-bec7-3f1ac324a701";
        private const string MortalArcanistClass10Align = "MortalArcanistClass10Align";
        private static readonly string MortalArcanistClass10AlignGuid = "8306e4f1-53ca-46b3-8c25-4f62d1dc75f4";
        private const string MortalAssassinClass0Align = "MortalAssassinClass0Align";
        private static readonly string MortalAssassinClass0AlignGuid = "08c01087-9076-479b-9801-47db1f528fba";
        internal const string MortalAssassinClass0AlignDisplayName = "EvangelistAssassinClass0Align.Name";
        private const string MortalAssassinClass0AlignDescription = "EvangelistAssassinClass0Align.Description";
        private const string MortalAssassinClass2Align = "MortalAssassinClass2Align";
        private static readonly string MortalAssassinClass2AlignGuid = "16088880-348e-4f13-a9dd-ea2c2ee3ff96";
        private const string MortalAssassinClass4Align = "MortalAssassinClass4Align";
        private static readonly string MortalAssassinClass4AlignGuid = "d379592d-476f-42cd-aaf6-b48ebce40bc6";
        private const string MortalAssassinClass6Align = "MortalAssassinClass6Align";
        private static readonly string MortalAssassinClass6AlignGuid = "9a5bea36-3140-45e1-9cea-34066cde5c5e";
        private const string MortalAssassinClass8Align = "MortalAssassinClass8Align";
        private static readonly string MortalAssassinClass8AlignGuid = "ebbbe007-ded6-4b39-86d7-897c4f55ef6b";
        private const string MortalAssassinClass10Align = "MortalAssassinClass10Align";
        private static readonly string MortalAssassinClass10AlignGuid = "9b7a4ddf-e1be-4324-b787-47c08fc27ec0";
        private const string MortalBarbarianClass0Align = "MortalBarbarianClass0Align";
        private static readonly string MortalBarbarianClass0AlignGuid = "7c796188-c4f0-4a15-bc03-4f038af8b84b";
        internal const string MortalBarbarianClass0AlignDisplayName = "EvangelistBarbarianClass0Align.Name";
        private const string MortalBarbarianClass0AlignDescription = "EvangelistBarbarianClass0Align.Description";
        private const string MortalBarbarianClass2Align = "MortalBarbarianClass2Align";
        private static readonly string MortalBarbarianClass2AlignGuid = "b145b753-7d7c-4093-9f99-cde83041bf4e";
        private const string MortalBarbarianClass4Align = "MortalBarbarianClass4Align";
        private static readonly string MortalBarbarianClass4AlignGuid = "3b9f6d10-d633-4638-b2b9-3bcbe4de8f84";
        private const string MortalBarbarianClass6Align = "MortalBarbarianClass6Align";
        private static readonly string MortalBarbarianClass6AlignGuid = "4cc6c283-2d99-4872-866f-a16878b46800";
        private const string MortalBarbarianClass8Align = "MortalBarbarianClass8Align";
        private static readonly string MortalBarbarianClass8AlignGuid = "b5cb8281-635d-4837-8a4f-699ef20e78db";
        private const string MortalBarbarianClass10Align = "MortalBarbarianClass10Align";
        private static readonly string MortalBarbarianClass10AlignGuid = "41b680b8-53f9-41fc-9d92-016dafc22744";
        private const string MortalBardClass0Align = "MortalBardClass0Align";
        private static readonly string MortalBardClass0AlignGuid = "60fb4da3-7714-43d1-9840-be9d4b6ddc07";
        internal const string MortalBardClass0AlignDisplayName = "EvangelistBardClass0Align.Name";
        private const string MortalBardClass0AlignDescription = "EvangelistBardClass0Align.Description";
        private const string MortalBardClass2Align = "MortalBardClass2Align";
        private static readonly string MortalBardClass2AlignGuid = "da6a2344-7803-4107-92ec-8c00c644f044";
        private const string MortalBardClass4Align = "MortalBardClass4Align";
        private static readonly string MortalBardClass4AlignGuid = "6190b639-5292-49fa-a34c-55b1e3fd4adf";
        private const string MortalBardClass6Align = "MortalBardClass6Align";
        private static readonly string MortalBardClass6AlignGuid = "a73df3aa-6f03-4727-b4bc-51e1b03f97c9";
        private const string MortalBardClass8Align = "MortalBardClass8Align";
        private static readonly string MortalBardClass8AlignGuid = "5676eea6-6e20-4389-9ea7-7f1c1d4efa71";
        private const string MortalBardClass10Align = "MortalBardClass10Align";
        private static readonly string MortalBardClass10AlignGuid = "c8cf2652-64a5-4f71-ab94-044845de3047";
        private const string MortalBloodragerClass0Align = "MortalBloodragerClass0Align";
        private static readonly string MortalBloodragerClass0AlignGuid = "aaf7a737-33f5-492c-afca-013c89f7c7e3";
        internal const string MortalBloodragerClass0AlignDisplayName = "EvangelistBloodragerClass0Align.Name";
        private const string MortalBloodragerClass0AlignDescription = "EvangelistBloodragerClass0Align.Description";
        private const string MortalBloodragerClass2Align = "MortalBloodragerClass2Align";
        private static readonly string MortalBloodragerClass2AlignGuid = "daf28968-b667-4d9e-b055-3b5c12a1a0d4";
        private const string MortalBloodragerClass4Align = "MortalBloodragerClass4Align";
        private static readonly string MortalBloodragerClass4AlignGuid = "0e1c8cde-7b25-4cc4-8749-e90dbbe73587";
        private const string MortalBloodragerClass6Align = "MortalBloodragerClass6Align";
        private static readonly string MortalBloodragerClass6AlignGuid = "af51714e-5610-4ccf-8be2-b75d1f834f4a";
        private const string MortalBloodragerClass8Align = "MortalBloodragerClass8Align";
        private static readonly string MortalBloodragerClass8AlignGuid = "0105eacd-a5b6-4f51-9b7b-045fcff4bab6";
        private const string MortalBloodragerClass10Align = "MortalBloodragerClass10Align";
        private static readonly string MortalBloodragerClass10AlignGuid = "b84c2ea5-41f1-4735-8443-2347b36ae8b4";
        private const string MortalCavalierClass0Align = "MortalCavalierClass0Align";
        private static readonly string MortalCavalierClass0AlignGuid = "e63061b4-aaf8-47a4-b5fd-8296a5a3e51c";
        internal const string MortalCavalierClass0AlignDisplayName = "EvangelistCavalierClass0Align.Name";
        private const string MortalCavalierClass0AlignDescription = "EvangelistCavalierClass0Align.Description";
        private const string MortalCavalierClass2Align = "MortalCavalierClass2Align";
        private static readonly string MortalCavalierClass2AlignGuid = "c1961348-678b-49b1-b762-37d7e8ba2f76";
        private const string MortalCavalierClass4Align = "MortalCavalierClass4Align";
        private static readonly string MortalCavalierClass4AlignGuid = "fbafdbe6-5ace-4fb4-a35c-579aee9be4d9";
        private const string MortalCavalierClass6Align = "MortalCavalierClass6Align";
        private static readonly string MortalCavalierClass6AlignGuid = "d38b0fa4-9950-4280-81c5-b3c68768ad03";
        private const string MortalCavalierClass8Align = "MortalCavalierClass8Align";
        private static readonly string MortalCavalierClass8AlignGuid = "16c12c1c-8129-4fd0-96fb-4e038faf5688";
        private const string MortalCavalierClass10Align = "MortalCavalierClass10Align";
        private static readonly string MortalCavalierClass10AlignGuid = "e25cee87-4b81-400d-aa04-61f8604181c0";
        private const string MortalClericClass0Align = "MortalClericClass0Align";
        private static readonly string MortalClericClass0AlignGuid = "6bfa4e44-38ce-40e1-8652-0d654473eb44";
        internal const string MortalClericClass0AlignDisplayName = "EvangelistClericClass0Align.Name";
        private const string MortalClericClass0AlignDescription = "EvangelistClericClass0Align.Description";
        private const string MortalClericClass2Align = "MortalClericClass2Align";
        private static readonly string MortalClericClass2AlignGuid = "cab2573a-6787-456b-b4b0-9b8301badfbe";
        private const string MortalClericClass4Align = "MortalClericClass4Align";
        private static readonly string MortalClericClass4AlignGuid = "471dd438-e995-422d-86e7-1456d9d60667";
        private const string MortalClericClass6Align = "MortalClericClass6Align";
        private static readonly string MortalClericClass6AlignGuid = "88d3a3af-c4eb-4f99-b464-09648e0a1a82";
        private const string MortalClericClass8Align = "MortalClericClass8Align";
        private static readonly string MortalClericClass8AlignGuid = "44efea0b-47f5-4e26-be12-5d81514092e2";
        private const string MortalClericClass10Align = "MortalClericClass10Align";
        private static readonly string MortalClericClass10AlignGuid = "73bbf458-9f88-42b1-ad75-15ac11afa6a9";
        private const string MortalDragonDiscipleClass0Align = "MortalDragonDiscipleClass0Align";
        private static readonly string MortalDragonDiscipleClass0AlignGuid = "6744dd25-60a7-4292-bece-3f085f07a7fe";
        internal const string MortalDragonDiscipleClass0AlignDisplayName = "EvangelistDragonDiscipleClass0Align.Name";
        private const string MortalDragonDiscipleClass0AlignDescription = "EvangelistDragonDiscipleClass0Align.Description";
        private const string MortalDragonDiscipleClass2Align = "MortalDragonDiscipleClass2Align";
        private static readonly string MortalDragonDiscipleClass2AlignGuid = "11d295cb-bca6-46e1-b863-e44e37a108b2";
        private const string MortalDragonDiscipleClass4Align = "MortalDragonDiscipleClass4Align";
        private static readonly string MortalDragonDiscipleClass4AlignGuid = "d6ac3d7e-75e0-4e48-857c-030b21d25494";
        private const string MortalDragonDiscipleClass6Align = "MortalDragonDiscipleClass6Align";
        private static readonly string MortalDragonDiscipleClass6AlignGuid = "142685b7-2e25-4c81-9c94-ae523cc176d7";
        private const string MortalDragonDiscipleClass8Align = "MortalDragonDiscipleClass8Align";
        private static readonly string MortalDragonDiscipleClass8AlignGuid = "8da4b56c-ebf8-43d2-acda-3f0708d00048";
        private const string MortalDragonDiscipleClass10Align = "MortalDragonDiscipleClass10Align";
        private static readonly string MortalDragonDiscipleClass10AlignGuid = "93b84c87-935c-4a90-a594-c50ae9be3887";
        private const string MortalDruidClass0Align = "MortalDruidClass0Align";
        private static readonly string MortalDruidClass0AlignGuid = "df0d1e2c-3959-4654-ba62-56a72c18a7c1";
        internal const string MortalDruidClass0AlignDisplayName = "EvangelistDruidClass0Align.Name";
        private const string MortalDruidClass0AlignDescription = "EvangelistDruidClass0Align.Description";
        private const string MortalDruidClass2Align = "MortalDruidClass2Align";
        private static readonly string MortalDruidClass2AlignGuid = "66c91b84-d853-48d4-a6f8-26bfd41b97f9";
        private const string MortalDruidClass4Align = "MortalDruidClass4Align";
        private static readonly string MortalDruidClass4AlignGuid = "00e84d6c-0864-4ec2-ab0c-219215d0288b";
        private const string MortalDruidClass6Align = "MortalDruidClass6Align";
        private static readonly string MortalDruidClass6AlignGuid = "f67cdf2e-0a1c-460b-832b-f18295aa9425";
        private const string MortalDruidClass8Align = "MortalDruidClass8Align";
        private static readonly string MortalDruidClass8AlignGuid = "2e7ac689-5c16-4a06-adde-72f1a3186984";
        private const string MortalDruidClass10Align = "MortalDruidClass10Align";
        private static readonly string MortalDruidClass10AlignGuid = "54ee288e-262e-4e59-95cd-fc7b64f5dff7";
        private const string MortalDuelistClass0Align = "MortalDuelistClass0Align";
        private static readonly string MortalDuelistClass0AlignGuid = "2be9e653-a49f-4442-b4da-cb4306c206cc";
        internal const string MortalDuelistClass0AlignDisplayName = "EvangelistDuelistClass0Align.Name";
        private const string MortalDuelistClass0AlignDescription = "EvangelistDuelistClass0Align.Description";
        private const string MortalDuelistClass2Align = "MortalDuelistClass2Align";
        private static readonly string MortalDuelistClass2AlignGuid = "4f4c7142-6f8c-4f10-96ba-cd46fd05ee17";
        private const string MortalDuelistClass4Align = "MortalDuelistClass4Align";
        private static readonly string MortalDuelistClass4AlignGuid = "0c31653c-363a-4e24-9122-eaeba883c2c9";
        private const string MortalDuelistClass6Align = "MortalDuelistClass6Align";
        private static readonly string MortalDuelistClass6AlignGuid = "9a705eb6-2f45-46d9-98ab-e35cf7043dc8";
        private const string MortalDuelistClass8Align = "MortalDuelistClass8Align";
        private static readonly string MortalDuelistClass8AlignGuid = "80302ff1-557e-4b69-800c-840ce4c99efe";
        private const string MortalDuelistClass10Align = "MortalDuelistClass10Align";
        private static readonly string MortalDuelistClass10AlignGuid = "af675c11-dde1-4b71-9b09-38be12ab5f9f";
        private const string MortalEldritchKnightClass0Align = "MortalEldritchKnightClass0Align";
        private static readonly string MortalEldritchKnightClass0AlignGuid = "4cc49c8c-8150-4752-97fa-9460399e814e";
        internal const string MortalEldritchKnightClass0AlignDisplayName = "EvangelistEldritchKnightClass0Align.Name";
        private const string MortalEldritchKnightClass0AlignDescription = "EvangelistEldritchKnightClass0Align.Description";
        private const string MortalEldritchKnightClass2Align = "MortalEldritchKnightClass2Align";
        private static readonly string MortalEldritchKnightClass2AlignGuid = "19f619c5-1bf5-45ba-badc-ebdab6b525b1";
        private const string MortalEldritchKnightClass4Align = "MortalEldritchKnightClass4Align";
        private static readonly string MortalEldritchKnightClass4AlignGuid = "7dbbda2a-43ef-43d3-90bb-8afc5b59d969";
        private const string MortalEldritchKnightClass6Align = "MortalEldritchKnightClass6Align";
        private static readonly string MortalEldritchKnightClass6AlignGuid = "94f25d2a-3326-4d92-9d58-e8c4bc3d2814";
        private const string MortalEldritchKnightClass8Align = "MortalEldritchKnightClass8Align";
        private static readonly string MortalEldritchKnightClass8AlignGuid = "7224923e-9a02-40a0-9988-2dc62540f732";
        private const string MortalEldritchKnightClass10Align = "MortalEldritchKnightClass10Align";
        private static readonly string MortalEldritchKnightClass10AlignGuid = "8b43bc7f-649a-4c44-af62-2d69f9445ae8";
        private const string MortalEldritchScionClass0Align = "MortalEldritchScionClass0Align";
        private static readonly string MortalEldritchScionClass0AlignGuid = "b73703d9-fa50-4014-9469-a926a7ff10e7";
        internal const string MortalEldritchScionClass0AlignDisplayName = "EvangelistEldritchScionClass0Align.Name";
        private const string MortalEldritchScionClass0AlignDescription = "EvangelistEldritchScionClass0Align.Description";
        private const string MortalEldritchScionClass2Align = "MortalEldritchScionClass2Align";
        private static readonly string MortalEldritchScionClass2AlignGuid = "6284469e-20cc-4f1b-8bd7-78d8e3f09441";
        private const string MortalEldritchScionClass4Align = "MortalEldritchScionClass4Align";
        private static readonly string MortalEldritchScionClass4AlignGuid = "d65a9764-f362-4bd3-880b-52889662ad02";
        private const string MortalEldritchScionClass6Align = "MortalEldritchScionClass6Align";
        private static readonly string MortalEldritchScionClass6AlignGuid = "cea83df7-87b7-493e-83d2-b99b27f36659";
        private const string MortalEldritchScionClass8Align = "MortalEldritchScionClass8Align";
        private static readonly string MortalEldritchScionClass8AlignGuid = "b3e92a20-0823-443d-9109-12443db7c34a";
        private const string MortalEldritchScionClass10Align = "MortalEldritchScionClass10Align";
        private static readonly string MortalEldritchScionClass10AlignGuid = "1cacbd4f-87cd-4262-883a-fd7470a05836";
        private const string MortalFighterClass0Align = "MortalFighterClass0Align";
        private static readonly string MortalFighterClass0AlignGuid = "0d778f9f-217c-4310-8f20-6b92b1599ac9";
        internal const string MortalFighterClass0AlignDisplayName = "EvangelistFighterClass0Align.Name";
        private const string MortalFighterClass0AlignDescription = "EvangelistFighterClass0Align.Description";
        private const string MortalFighterClass2Align = "MortalFighterClass2Align";
        private static readonly string MortalFighterClass2AlignGuid = "318e16dc-bcd9-4f97-9f5b-92ceacc81bed";
        private const string MortalFighterClass4Align = "MortalFighterClass4Align";
        private static readonly string MortalFighterClass4AlignGuid = "695ce4c0-6270-4698-a416-8c9a98400e7a";
        private const string MortalFighterClass6Align = "MortalFighterClass6Align";
        private static readonly string MortalFighterClass6AlignGuid = "f617a03f-1c14-44ff-a3ac-f138d338286d";
        private const string MortalFighterClass8Align = "MortalFighterClass8Align";
        private static readonly string MortalFighterClass8AlignGuid = "4011b917-78fe-41e4-8a98-82156d904313";
        private const string MortalFighterClass10Align = "MortalFighterClass10Align";
        private static readonly string MortalFighterClass10AlignGuid = "d77b82ff-b8dc-4c15-b53e-9aeb94f3504a";
        private const string MortalHellknightClass0Align = "MortalHellknightClass0Align";
        private static readonly string MortalHellknightClass0AlignGuid = "51cba8d1-a101-4d2b-bf25-3a67e92c63b0";
        internal const string MortalHellknightClass0AlignDisplayName = "EvangelistHellknightClass0Align.Name";
        private const string MortalHellknightClass0AlignDescription = "EvangelistHellknightClass0Align.Description";
        private const string MortalHellknightClass2Align = "MortalHellknightClass2Align";
        private static readonly string MortalHellknightClass2AlignGuid = "c8ee19a2-f29b-4c28-827f-45fae86185f6";
        private const string MortalHellknightClass4Align = "MortalHellknightClass4Align";
        private static readonly string MortalHellknightClass4AlignGuid = "57e34b3f-e49e-450a-be37-3e8240f88ced";
        private const string MortalHellknightClass6Align = "MortalHellknightClass6Align";
        private static readonly string MortalHellknightClass6AlignGuid = "7bbded0d-1077-4ae0-9143-0a490328c02c";
        private const string MortalHellknightClass8Align = "MortalHellknightClass8Align";
        private static readonly string MortalHellknightClass8AlignGuid = "7a6bdc56-23e3-437a-b6a6-993f19ad6639";
        private const string MortalHellknightClass10Align = "MortalHellknightClass10Align";
        private static readonly string MortalHellknightClass10AlignGuid = "9d0b9865-c1e6-4e9b-b632-8877227b0f01";
        private const string MortalHellknightSigniferClass0Align = "MortalHellknightSigniferClass0Align";
        private static readonly string MortalHellknightSigniferClass0AlignGuid = "579ad96e-cd95-48ca-b735-97241c66527e";
        internal const string MortalHellknightSigniferClass0AlignDisplayName = "EvangelistHellknightSigniferClass0Align.Name";
        private const string MortalHellknightSigniferClass0AlignDescription = "EvangelistHellknightSigniferClass0Align.Description";
        private const string MortalHellknightSigniferClass2Align = "MortalHellknightSigniferClass2Align";
        private static readonly string MortalHellknightSigniferClass2AlignGuid = "ca041650-d8ce-4210-9dc9-7029bc55b251";
        private const string MortalHellknightSigniferClass4Align = "MortalHellknightSigniferClass4Align";
        private static readonly string MortalHellknightSigniferClass4AlignGuid = "f544b2d6-6ad7-4dc8-a421-1aa9b0b2e8a0";
        private const string MortalHellknightSigniferClass6Align = "MortalHellknightSigniferClass6Align";
        private static readonly string MortalHellknightSigniferClass6AlignGuid = "eff5612e-9098-42b2-932b-99010ad47efc";
        private const string MortalHellknightSigniferClass8Align = "MortalHellknightSigniferClass8Align";
        private static readonly string MortalHellknightSigniferClass8AlignGuid = "e40edc6e-f806-40ed-a4ff-79517d9af225";
        private const string MortalHellknightSigniferClass10Align = "MortalHellknightSigniferClass10Align";
        private static readonly string MortalHellknightSigniferClass10AlignGuid = "2341ad08-0ee6-49e8-a6e3-3128a87886e2";
        private const string MortalHunterClass0Align = "MortalHunterClass0Align";
        private static readonly string MortalHunterClass0AlignGuid = "1038fc59-4de0-4fc1-8752-19715404eacd";
        internal const string MortalHunterClass0AlignDisplayName = "EvangelistHunterClass0Align.Name";
        private const string MortalHunterClass0AlignDescription = "EvangelistHunterClass0Align.Description";
        private const string MortalHunterClass2Align = "MortalHunterClass2Align";
        private static readonly string MortalHunterClass2AlignGuid = "4bd24382-0013-43d2-b2c3-77a2a71dc6ea";
        private const string MortalHunterClass4Align = "MortalHunterClass4Align";
        private static readonly string MortalHunterClass4AlignGuid = "30a68042-b11b-4e79-9244-3d948a2aabcc";
        private const string MortalHunterClass6Align = "MortalHunterClass6Align";
        private static readonly string MortalHunterClass6AlignGuid = "55d1f34d-7809-45db-b363-d07b3dbf019f";
        private const string MortalHunterClass8Align = "MortalHunterClass8Align";
        private static readonly string MortalHunterClass8AlignGuid = "be40b344-a156-4597-ae05-dc131315f140";
        private const string MortalHunterClass10Align = "MortalHunterClass10Align";
        private static readonly string MortalHunterClass10AlignGuid = "a442ee26-fd8e-419d-a23e-8a258105e3f5";
        private const string MortalInquisitorClass0Align = "MortalInquisitorClass0Align";
        private static readonly string MortalInquisitorClass0AlignGuid = "a75e0d27-5ec4-46f8-a4ef-f4f4766c2654";
        internal const string MortalInquisitorClass0AlignDisplayName = "EvangelistInquisitorClass0Align.Name";
        private const string MortalInquisitorClass0AlignDescription = "EvangelistInquisitorClass0Align.Description";
        private const string MortalInquisitorClass2Align = "MortalInquisitorClass2Align";
        private static readonly string MortalInquisitorClass2AlignGuid = "f193a5ea-00e2-4854-a805-5d700fec5c44";
        private const string MortalInquisitorClass4Align = "MortalInquisitorClass4Align";
        private static readonly string MortalInquisitorClass4AlignGuid = "f6b531fd-47d2-42f9-b817-55595b891570";
        private const string MortalInquisitorClass6Align = "MortalInquisitorClass6Align";
        private static readonly string MortalInquisitorClass6AlignGuid = "734eaab2-97f8-4e36-ae30-e08f165a62bf";
        private const string MortalInquisitorClass8Align = "MortalInquisitorClass8Align";
        private static readonly string MortalInquisitorClass8AlignGuid = "2043dbf1-fe90-4589-90d8-5c0e753888b4";
        private const string MortalInquisitorClass10Align = "MortalInquisitorClass10Align";
        private static readonly string MortalInquisitorClass10AlignGuid = "a6410328-8c71-4317-b287-6164a8bc7da8";
        private const string MortalKineticistClass0Align = "MortalKineticistClass0Align";
        private static readonly string MortalKineticistClass0AlignGuid = "af0718c1-2bfa-431e-bfe9-e835cd06e2ad";
        internal const string MortalKineticistClass0AlignDisplayName = "EvangelistKineticistClass0Align.Name";
        private const string MortalKineticistClass0AlignDescription = "EvangelistKineticistClass0Align.Description";
        private const string MortalKineticistClass2Align = "MortalKineticistClass2Align";
        private static readonly string MortalKineticistClass2AlignGuid = "a8cffe86-9f9a-48fd-9fa2-dca7366f72f8";
        private const string MortalKineticistClass4Align = "MortalKineticistClass4Align";
        private static readonly string MortalKineticistClass4AlignGuid = "ad2d160f-d13a-4117-a317-fdf667a91a8a";
        private const string MortalKineticistClass6Align = "MortalKineticistClass6Align";
        private static readonly string MortalKineticistClass6AlignGuid = "3134f0f3-d5f3-4f1c-af26-ee764fe6cb29";
        private const string MortalKineticistClass8Align = "MortalKineticistClass8Align";
        private static readonly string MortalKineticistClass8AlignGuid = "c39ba368-a1da-481b-873f-ea4411962b2d";
        private const string MortalKineticistClass10Align = "MortalKineticistClass10Align";
        private static readonly string MortalKineticistClass10AlignGuid = "64faff74-3981-4fdb-bc21-56e85ac58a4f";
        private const string MortalLoremasterClass0Align = "MortalLoremasterClass0Align";
        private static readonly string MortalLoremasterClass0AlignGuid = "6911b832-84da-40d8-adc0-c9a7b391e32b";
        internal const string MortalLoremasterClass0AlignDisplayName = "EvangelistLoremasterClass0Align.Name";
        private const string MortalLoremasterClass0AlignDescription = "EvangelistLoremasterClass0Align.Description";
        private const string MortalLoremasterClass2Align = "MortalLoremasterClass2Align";
        private static readonly string MortalLoremasterClass2AlignGuid = "48bd456f-6e0a-49a1-9ef7-50aadd6367ed";
        private const string MortalLoremasterClass4Align = "MortalLoremasterClass4Align";
        private static readonly string MortalLoremasterClass4AlignGuid = "2f217970-d528-4d6d-a29d-a62bc6b7fcf9";
        private const string MortalLoremasterClass6Align = "MortalLoremasterClass6Align";
        private static readonly string MortalLoremasterClass6AlignGuid = "d4189a09-d0f1-44bc-b829-106af86ba75c";
        private const string MortalLoremasterClass8Align = "MortalLoremasterClass8Align";
        private static readonly string MortalLoremasterClass8AlignGuid = "1065d4c0-f1c6-4fb2-9c6c-969b888e0844";
        private const string MortalLoremasterClass10Align = "MortalLoremasterClass10Align";
        private static readonly string MortalLoremasterClass10AlignGuid = "c9b729b4-ab25-4c7f-8e7a-88a1d6893e3d";
        private const string MortalMagusClass0Align = "MortalMagusClass0Align";
        private static readonly string MortalMagusClass0AlignGuid = "3ead24d4-054b-4e97-bf65-d196b411109a";
        internal const string MortalMagusClass0AlignDisplayName = "EvangelistMagusClass0Align.Name";
        private const string MortalMagusClass0AlignDescription = "EvangelistMagusClass0Align.Description";
        private const string MortalMagusClass2Align = "MortalMagusClass2Align";
        private static readonly string MortalMagusClass2AlignGuid = "5f8d2925-a623-4353-8a5a-6dd2ac80a165";
        private const string MortalMagusClass4Align = "MortalMagusClass4Align";
        private static readonly string MortalMagusClass4AlignGuid = "00fc4dae-3718-40f9-9333-d7a50af95f53";
        private const string MortalMagusClass6Align = "MortalMagusClass6Align";
        private static readonly string MortalMagusClass6AlignGuid = "eb67d790-5d6f-4324-a294-a56e161a2668";
        private const string MortalMagusClass8Align = "MortalMagusClass8Align";
        private static readonly string MortalMagusClass8AlignGuid = "d7f436ec-e39c-4023-b13c-c6bcfca7bcde";
        private const string MortalMagusClass10Align = "MortalMagusClass10Align";
        private static readonly string MortalMagusClass10AlignGuid = "a2c4f994-e838-4c4c-a77e-5f0d49a94b07";
        private const string MortalMonkClass0Align = "MortalMonkClass0Align";
        private static readonly string MortalMonkClass0AlignGuid = "8fa8a27e-7ced-4aa2-a448-c82f5350685c";
        internal const string MortalMonkClass0AlignDisplayName = "EvangelistMonkClass0Align.Name";
        private const string MortalMonkClass0AlignDescription = "EvangelistMonkClass0Align.Description";
        private const string MortalMonkClass2Align = "MortalMonkClass2Align";
        private static readonly string MortalMonkClass2AlignGuid = "50e6d77d-6bf8-480a-bd09-6551c51fd759";
        private const string MortalMonkClass4Align = "MortalMonkClass4Align";
        private static readonly string MortalMonkClass4AlignGuid = "835ae534-bf55-4947-b3b3-818761dfa171";
        private const string MortalMonkClass6Align = "MortalMonkClass6Align";
        private static readonly string MortalMonkClass6AlignGuid = "ca7395c5-2ce8-493d-bb34-969f6b66a12d";
        private const string MortalMonkClass8Align = "MortalMonkClass8Align";
        private static readonly string MortalMonkClass8AlignGuid = "7b115f8d-6a79-471a-baed-22cb6672352f";
        private const string MortalMonkClass10Align = "MortalMonkClass10Align";
        private static readonly string MortalMonkClass10AlignGuid = "77649039-67ab-49fd-94b2-58cded48b300";
        private const string MortalMysticTheurgeClass0Align = "MortalMysticTheurgeClass0Align";
        private static readonly string MortalMysticTheurgeClass0AlignGuid = "b51d98ea-1663-49b0-bbfa-5ad75255d126";
        internal const string MortalMysticTheurgeClass0AlignDisplayName = "EvangelistMysticTheurgeClass0Align.Name";
        private const string MortalMysticTheurgeClass0AlignDescription = "EvangelistMysticTheurgeClass0Align.Description";
        private const string MortalMysticTheurgeClass2Align = "MortalMysticTheurgeClass2Align";
        private static readonly string MortalMysticTheurgeClass2AlignGuid = "88c4579d-66ea-4c28-9fdb-efa8bfa31fcd";
        private const string MortalMysticTheurgeClass4Align = "MortalMysticTheurgeClass4Align";
        private static readonly string MortalMysticTheurgeClass4AlignGuid = "cc93a88c-3184-4d27-a455-b75eab020c8e";
        private const string MortalMysticTheurgeClass6Align = "MortalMysticTheurgeClass6Align";
        private static readonly string MortalMysticTheurgeClass6AlignGuid = "367a6e88-f810-49c1-9ae9-6ba809978a80";
        private const string MortalMysticTheurgeClass8Align = "MortalMysticTheurgeClass8Align";
        private static readonly string MortalMysticTheurgeClass8AlignGuid = "ebc10209-bdbd-45a2-9ab1-325cfd91be2e";
        private const string MortalMysticTheurgeClass10Align = "MortalMysticTheurgeClass10Align";
        private static readonly string MortalMysticTheurgeClass10AlignGuid = "f9e2d1b4-a2bf-434d-af84-5ea3ccbafbb8";
        private const string MortalOracleClass0Align = "MortalOracleClass0Align";
        private static readonly string MortalOracleClass0AlignGuid = "808fc1cd-fd68-42bc-aa60-91f628e76597";
        internal const string MortalOracleClass0AlignDisplayName = "EvangelistOracleClass0Align.Name";
        private const string MortalOracleClass0AlignDescription = "EvangelistOracleClass0Align.Description";
        private const string MortalOracleClass2Align = "MortalOracleClass2Align";
        private static readonly string MortalOracleClass2AlignGuid = "ca3253ff-393b-4eaa-b068-120ed7a8751f";
        private const string MortalOracleClass4Align = "MortalOracleClass4Align";
        private static readonly string MortalOracleClass4AlignGuid = "442a921f-0dbf-411f-b232-4441f682558b";
        private const string MortalOracleClass6Align = "MortalOracleClass6Align";
        private static readonly string MortalOracleClass6AlignGuid = "5fc796b1-bfe3-475d-b7a9-75709e672eea";
        private const string MortalOracleClass8Align = "MortalOracleClass8Align";
        private static readonly string MortalOracleClass8AlignGuid = "42bca8a5-0145-4ea7-a6ce-496128b53599";
        private const string MortalOracleClass10Align = "MortalOracleClass10Align";
        private static readonly string MortalOracleClass10AlignGuid = "584f1045-e0c1-4ca5-afd3-917de84effd0";
        private const string MortalPaladinClass0Align = "MortalPaladinClass0Align";
        private static readonly string MortalPaladinClass0AlignGuid = "75bc813c-28e4-4935-aa59-bb06dfeb373b";
        internal const string MortalPaladinClass0AlignDisplayName = "EvangelistPaladinClass0Align.Name";
        private const string MortalPaladinClass0AlignDescription = "EvangelistPaladinClass0Align.Description";
        private const string MortalPaladinClass2Align = "MortalPaladinClass2Align";
        private static readonly string MortalPaladinClass2AlignGuid = "5f9d2ca3-f996-4dac-8c34-ef2074489644";
        private const string MortalPaladinClass4Align = "MortalPaladinClass4Align";
        private static readonly string MortalPaladinClass4AlignGuid = "1bad1f00-4fc0-48a4-8d1a-79dad79e7990";
        private const string MortalPaladinClass6Align = "MortalPaladinClass6Align";
        private static readonly string MortalPaladinClass6AlignGuid = "9955d2df-9965-47e5-ade1-aaed74327e26";
        private const string MortalPaladinClass8Align = "MortalPaladinClass8Align";
        private static readonly string MortalPaladinClass8AlignGuid = "03f8d905-0383-4a4d-b977-af17effb598c";
        private const string MortalPaladinClass10Align = "MortalPaladinClass10Align";
        private static readonly string MortalPaladinClass10AlignGuid = "3a14f181-5faa-4320-b160-ae13d0c677ce";
        private const string MortalRangerClass0Align = "MortalRangerClass0Align";
        private static readonly string MortalRangerClass0AlignGuid = "7f4124ba-a98f-47f5-9540-c7ac8550aff8";
        internal const string MortalRangerClass0AlignDisplayName = "EvangelistRangerClass0Align.Name";
        private const string MortalRangerClass0AlignDescription = "EvangelistRangerClass0Align.Description";
        private const string MortalRangerClass2Align = "MortalRangerClass2Align";
        private static readonly string MortalRangerClass2AlignGuid = "ff817662-0fc4-4153-aaec-67403296e26b";
        private const string MortalRangerClass4Align = "MortalRangerClass4Align";
        private static readonly string MortalRangerClass4AlignGuid = "b60eef48-9540-4936-9c4b-82268671e87c";
        private const string MortalRangerClass6Align = "MortalRangerClass6Align";
        private static readonly string MortalRangerClass6AlignGuid = "a71d3cc8-672c-4e1d-95db-336915cb1e67";
        private const string MortalRangerClass8Align = "MortalRangerClass8Align";
        private static readonly string MortalRangerClass8AlignGuid = "e4331abc-1089-44a4-b247-297b14488513";
        private const string MortalRangerClass10Align = "MortalRangerClass10Align";
        private static readonly string MortalRangerClass10AlignGuid = "422ff21c-732b-4da1-a24c-b7067430bd29";
        private const string MortalRogueClass0Align = "MortalRogueClass0Align";
        private static readonly string MortalRogueClass0AlignGuid = "996e3bae-8288-4490-8c74-749693e6d173";
        internal const string MortalRogueClass0AlignDisplayName = "EvangelistRogueClass0Align.Name";
        private const string MortalRogueClass0AlignDescription = "EvangelistRogueClass0Align.Description";
        private const string MortalRogueClass2Align = "MortalRogueClass2Align";
        private static readonly string MortalRogueClass2AlignGuid = "b6105f79-7b1c-42af-ba42-231c658997c4";
        private const string MortalRogueClass4Align = "MortalRogueClass4Align";
        private static readonly string MortalRogueClass4AlignGuid = "3deb2f82-8f56-466e-a3f0-1928e7307f75";
        private const string MortalRogueClass6Align = "MortalRogueClass6Align";
        private static readonly string MortalRogueClass6AlignGuid = "2484b45e-5bab-4b4c-ac74-4b9f6e68fd19";
        private const string MortalRogueClass8Align = "MortalRogueClass8Align";
        private static readonly string MortalRogueClass8AlignGuid = "052bfd99-38d5-47e2-80a2-5f03e07d2028";
        private const string MortalRogueClass10Align = "MortalRogueClass10Align";
        private static readonly string MortalRogueClass10AlignGuid = "a547212c-8469-4116-8254-22e0b69a2792";
        private const string MortalShamanClass0Align = "MortalShamanClass0Align";
        private static readonly string MortalShamanClass0AlignGuid = "24d0ee34-24da-4158-96fe-b16e885f9ad3";
        internal const string MortalShamanClass0AlignDisplayName = "EvangelistShamanClass0Align.Name";
        private const string MortalShamanClass0AlignDescription = "EvangelistShamanClass0Align.Description";
        private const string MortalShamanClass2Align = "MortalShamanClass2Align";
        private static readonly string MortalShamanClass2AlignGuid = "54a97378-4646-49f9-8fe0-474d62a31eaa";
        private const string MortalShamanClass4Align = "MortalShamanClass4Align";
        private static readonly string MortalShamanClass4AlignGuid = "2fdbbf29-cd9a-4a87-a521-217df3cc8b3e";
        private const string MortalShamanClass6Align = "MortalShamanClass6Align";
        private static readonly string MortalShamanClass6AlignGuid = "ba56f849-ccc7-44e6-99be-3297c77a1985";
        private const string MortalShamanClass8Align = "MortalShamanClass8Align";
        private static readonly string MortalShamanClass8AlignGuid = "9d22194a-369c-4384-833c-f807ea1a8fdf";
        private const string MortalShamanClass10Align = "MortalShamanClass10Align";
        private static readonly string MortalShamanClass10AlignGuid = "13d8feed-0d4d-452e-8f65-4b93ebc9f23f";
        private const string MortalShifterClass0Align = "MortalShifterClass0Align";
        private static readonly string MortalShifterClass0AlignGuid = "57e113bc-2a21-4252-ab5b-1426f5af8c8e";
        internal const string MortalShifterClass0AlignDisplayName = "EvangelistShifterClass0Align.Name";
        private const string MortalShifterClass0AlignDescription = "EvangelistShifterClass0Align.Description";
        private const string MortalShifterClass2Align = "MortalShifterClass2Align";
        private static readonly string MortalShifterClass2AlignGuid = "cb874572-eb21-42bb-90b3-6ee068031426";
        private const string MortalShifterClass4Align = "MortalShifterClass4Align";
        private static readonly string MortalShifterClass4AlignGuid = "8187e851-907e-408d-8eb2-02d293b32611";
        private const string MortalShifterClass6Align = "MortalShifterClass6Align";
        private static readonly string MortalShifterClass6AlignGuid = "fafb40df-4e4e-4558-9758-17c4661bbb99";
        private const string MortalShifterClass8Align = "MortalShifterClass8Align";
        private static readonly string MortalShifterClass8AlignGuid = "4b543c1d-1a9a-471b-a928-1b7d9f6e7a5a";
        private const string MortalShifterClass10Align = "MortalShifterClass10Align";
        private static readonly string MortalShifterClass10AlignGuid = "d098b216-5330-4839-a3bc-f228e4e549de";
        private const string MortalSkaldClass0Align = "MortalSkaldClass0Align";
        private static readonly string MortalSkaldClass0AlignGuid = "2b64232b-6a79-4f4f-9ca9-7c4e67628040";
        internal const string MortalSkaldClass0AlignDisplayName = "EvangelistSkaldClass0Align.Name";
        private const string MortalSkaldClass0AlignDescription = "EvangelistSkaldClass0Align.Description";
        private const string MortalSkaldClass2Align = "MortalSkaldClass2Align";
        private static readonly string MortalSkaldClass2AlignGuid = "b957ab3a-6a0c-495d-8e3b-5eb92dc2e107";
        private const string MortalSkaldClass4Align = "MortalSkaldClass4Align";
        private static readonly string MortalSkaldClass4AlignGuid = "638ce52d-11f4-424f-871a-251bca754ab9";
        private const string MortalSkaldClass6Align = "MortalSkaldClass6Align";
        private static readonly string MortalSkaldClass6AlignGuid = "4a44e65c-c039-40a3-a46d-43263bd5c771";
        private const string MortalSkaldClass8Align = "MortalSkaldClass8Align";
        private static readonly string MortalSkaldClass8AlignGuid = "1cf2dca4-58e0-4afb-b693-47445c501b96";
        private const string MortalSkaldClass10Align = "MortalSkaldClass10Align";
        private static readonly string MortalSkaldClass10AlignGuid = "063ab208-1a5a-4c79-ad9c-32bbd4bcfb87";
        private const string MortalSlayerClass0Align = "MortalSlayerClass0Align";
        private static readonly string MortalSlayerClass0AlignGuid = "f93fa5b2-92d8-4412-87b0-26781e66922a";
        internal const string MortalSlayerClass0AlignDisplayName = "EvangelistSlayerClass0Align.Name";
        private const string MortalSlayerClass0AlignDescription = "EvangelistSlayerClass0Align.Description";
        private const string MortalSlayerClass2Align = "MortalSlayerClass2Align";
        private static readonly string MortalSlayerClass2AlignGuid = "f297e687-a5de-47ba-aa03-aa25fe1174bf";
        private const string MortalSlayerClass4Align = "MortalSlayerClass4Align";
        private static readonly string MortalSlayerClass4AlignGuid = "dfa86294-8daf-4ed7-bea7-d1e0125cabd3";
        private const string MortalSlayerClass6Align = "MortalSlayerClass6Align";
        private static readonly string MortalSlayerClass6AlignGuid = "43c5ec6e-3f8e-4ef4-b43b-a4b15abb0ee0";
        private const string MortalSlayerClass8Align = "MortalSlayerClass8Align";
        private static readonly string MortalSlayerClass8AlignGuid = "8154bca9-e2b1-4f68-9bee-3c491df3095e";
        private const string MortalSlayerClass10Align = "MortalSlayerClass10Align";
        private static readonly string MortalSlayerClass10AlignGuid = "af7fe2dc-1cb3-45bd-81aa-87695e92f77f";
        private const string MortalSorcererClass0Align = "MortalSorcererClass0Align";
        private static readonly string MortalSorcererClass0AlignGuid = "1b0bb073-2056-48f7-b880-b22341f10a80";
        internal const string MortalSorcererClass0AlignDisplayName = "EvangelistSorcererClass0Align.Name";
        private const string MortalSorcererClass0AlignDescription = "EvangelistSorcererClass0Align.Description";
        private const string MortalSorcererClass2Align = "MortalSorcererClass2Align";
        private static readonly string MortalSorcererClass2AlignGuid = "7c56eb56-4b13-4c05-892c-1f2d1b2d42b8";
        private const string MortalSorcererClass4Align = "MortalSorcererClass4Align";
        private static readonly string MortalSorcererClass4AlignGuid = "214f468d-7be3-4670-b480-da3bc2f604e1";
        private const string MortalSorcererClass6Align = "MortalSorcererClass6Align";
        private static readonly string MortalSorcererClass6AlignGuid = "c3e297f9-92e4-435c-af52-71f286de1032";
        private const string MortalSorcererClass8Align = "MortalSorcererClass8Align";
        private static readonly string MortalSorcererClass8AlignGuid = "ae65d876-d353-4055-a5db-a86ef8143953";
        private const string MortalSorcererClass10Align = "MortalSorcererClass10Align";
        private static readonly string MortalSorcererClass10AlignGuid = "c62df7c3-2b4c-4d7d-9550-4bab9e8f3e5e";
        private const string MortalStalwartDefenderClass0Align = "MortalStalwartDefenderClass0Align";
        private static readonly string MortalStalwartDefenderClass0AlignGuid = "f5bc9ec4-4506-4213-b162-c55d94d0dbb6";
        internal const string MortalStalwartDefenderClass0AlignDisplayName = "EvangelistStalwartDefenderClass0Align.Name";
        private const string MortalStalwartDefenderClass0AlignDescription = "EvangelistStalwartDefenderClass0Align.Description";
        private const string MortalStalwartDefenderClass2Align = "MortalStalwartDefenderClass2Align";
        private static readonly string MortalStalwartDefenderClass2AlignGuid = "19d42130-f079-4517-9880-7ca001dd927f";
        private const string MortalStalwartDefenderClass4Align = "MortalStalwartDefenderClass4Align";
        private static readonly string MortalStalwartDefenderClass4AlignGuid = "34f29a73-50e7-4877-9b04-9f10b17afb36";
        private const string MortalStalwartDefenderClass6Align = "MortalStalwartDefenderClass6Align";
        private static readonly string MortalStalwartDefenderClass6AlignGuid = "d8c9f8de-d77d-46a3-a147-3d12dc7761a2";
        private const string MortalStalwartDefenderClass8Align = "MortalStalwartDefenderClass8Align";
        private static readonly string MortalStalwartDefenderClass8AlignGuid = "76b73d82-7c10-4fd2-aa6e-00df73302082";
        private const string MortalStalwartDefenderClass10Align = "MortalStalwartDefenderClass10Align";
        private static readonly string MortalStalwartDefenderClass10AlignGuid = "5c1d4112-5e9e-4e2f-8cc3-1726a9e53ed8";
        private const string MortalStudentOfWarClass0Align = "MortalStudentOfWarClass0Align";
        private static readonly string MortalStudentOfWarClass0AlignGuid = "fd75a9dc-54ba-4bd7-b9bd-e67780bfa711";
        internal const string MortalStudentOfWarClass0AlignDisplayName = "EvangelistStudentOfWarClass0Align.Name";
        private const string MortalStudentOfWarClass0AlignDescription = "EvangelistStudentOfWarClass0Align.Description";
        private const string MortalStudentOfWarClass2Align = "MortalStudentOfWarClass2Align";
        private static readonly string MortalStudentOfWarClass2AlignGuid = "27bb7ae1-ff58-4411-b89e-aeda1797ff00";
        private const string MortalStudentOfWarClass4Align = "MortalStudentOfWarClass4Align";
        private static readonly string MortalStudentOfWarClass4AlignGuid = "98e4e044-78eb-466a-8398-7bbb93bdeaea";
        private const string MortalStudentOfWarClass6Align = "MortalStudentOfWarClass6Align";
        private static readonly string MortalStudentOfWarClass6AlignGuid = "0f0cec7d-447c-4de9-a1d2-8e254464d2d5";
        private const string MortalStudentOfWarClass8Align = "MortalStudentOfWarClass8Align";
        private static readonly string MortalStudentOfWarClass8AlignGuid = "fa0d4596-d4cf-443f-83ae-c4b5e8f6abf9";
        private const string MortalStudentOfWarClass10Align = "MortalStudentOfWarClass10Align";
        private static readonly string MortalStudentOfWarClass10AlignGuid = "a1e5e8d5-f67f-403c-9c03-500c2e63889b";
        private const string MortalSwordlordClass0Align = "MortalSwordlordClass0Align";
        private static readonly string MortalSwordlordClass0AlignGuid = "051b6820-a98d-44ad-9073-7c8334e76195";
        internal const string MortalSwordlordClass0AlignDisplayName = "EvangelistSwordlordClass0Align.Name";
        private const string MortalSwordlordClass0AlignDescription = "EvangelistSwordlordClass0Align.Description";
        private const string MortalSwordlordClass2Align = "MortalSwordlordClass2Align";
        private static readonly string MortalSwordlordClass2AlignGuid = "7f3dc0da-edb4-40c0-bcd5-9b5ce3b20742";
        private const string MortalSwordlordClass4Align = "MortalSwordlordClass4Align";
        private static readonly string MortalSwordlordClass4AlignGuid = "cdff73d9-1cdf-4a63-abc7-d806072942f5";
        private const string MortalSwordlordClass6Align = "MortalSwordlordClass6Align";
        private static readonly string MortalSwordlordClass6AlignGuid = "e3d4708f-f3d5-4c2c-b6bb-a02b78b8bbc9";
        private const string MortalSwordlordClass8Align = "MortalSwordlordClass8Align";
        private static readonly string MortalSwordlordClass8AlignGuid = "5790709c-5b5c-46e2-bae0-f4127a49862f";
        private const string MortalSwordlordClass10Align = "MortalSwordlordClass10Align";
        private static readonly string MortalSwordlordClass10AlignGuid = "296e8567-17bd-4e5b-833c-e8cbad148a29";
        private const string MortalWarpriestClass0Align = "MortalWarpriestClass0Align";
        private static readonly string MortalWarpriestClass0AlignGuid = "219e2f70-8f27-4b1b-b5fe-833ee0caa279";
        internal const string MortalWarpriestClass0AlignDisplayName = "EvangelistWarpriestClass0Align.Name";
        private const string MortalWarpriestClass0AlignDescription = "EvangelistWarpriestClass0Align.Description";
        private const string MortalWarpriestClass2Align = "MortalWarpriestClass2Align";
        private static readonly string MortalWarpriestClass2AlignGuid = "75fff39c-408d-45c7-96ca-45c0fdd97885";
        private const string MortalWarpriestClass4Align = "MortalWarpriestClass4Align";
        private static readonly string MortalWarpriestClass4AlignGuid = "d5ac2395-e922-4526-b1ac-73f67b0e4cc9";
        private const string MortalWarpriestClass6Align = "MortalWarpriestClass6Align";
        private static readonly string MortalWarpriestClass6AlignGuid = "f06c5689-6089-4663-81e5-6f0b56c6575e";
        private const string MortalWarpriestClass8Align = "MortalWarpriestClass8Align";
        private static readonly string MortalWarpriestClass8AlignGuid = "9ce3920e-2926-4658-814d-c1f3d467b1b3";
        private const string MortalWarpriestClass10Align = "MortalWarpriestClass10Align";
        private static readonly string MortalWarpriestClass10AlignGuid = "07daf629-30f8-4bd7-9666-51fb7ab790be";
        private const string MortalWinterWitchClass0Align = "MortalWinterWitchClass0Align";
        private static readonly string MortalWinterWitchClass0AlignGuid = "6765f3cf-9ef8-47a6-826b-f28f861f721b";
        internal const string MortalWinterWitchClass0AlignDisplayName = "EvangelistWinterWitchClass0Align.Name";
        private const string MortalWinterWitchClass0AlignDescription = "EvangelistWinterWitchClass0Align.Description";
        private const string MortalWinterWitchClass2Align = "MortalWinterWitchClass2Align";
        private static readonly string MortalWinterWitchClass2AlignGuid = "c553ad82-9238-44be-83ac-3ac602f73470";
        private const string MortalWinterWitchClass4Align = "MortalWinterWitchClass4Align";
        private static readonly string MortalWinterWitchClass4AlignGuid = "528c9912-e479-4dfb-88ac-d67fa0abe23a";
        private const string MortalWinterWitchClass6Align = "MortalWinterWitchClass6Align";
        private static readonly string MortalWinterWitchClass6AlignGuid = "68d6f404-f19b-4178-b168-72c91c782f23";
        private const string MortalWinterWitchClass8Align = "MortalWinterWitchClass8Align";
        private static readonly string MortalWinterWitchClass8AlignGuid = "dfeff67d-1131-41a8-ae80-d3771d42d4ef";
        private const string MortalWinterWitchClass10Align = "MortalWinterWitchClass10Align";
        private static readonly string MortalWinterWitchClass10AlignGuid = "bfe71ea4-9b89-4e0f-bdfe-37911e55c34c";
        private const string MortalWitchClass0Align = "MortalWitchClass0Align";
        private static readonly string MortalWitchClass0AlignGuid = "c59d0a52-3c33-4454-96d2-82ff4a17e07f";
        internal const string MortalWitchClass0AlignDisplayName = "EvangelistWitchClass0Align.Name";
        private const string MortalWitchClass0AlignDescription = "EvangelistWitchClass0Align.Description";
        private const string MortalWitchClass2Align = "MortalWitchClass2Align";
        private static readonly string MortalWitchClass2AlignGuid = "561bce85-4692-48af-b9a0-1b27d592ba53";
        private const string MortalWitchClass4Align = "MortalWitchClass4Align";
        private static readonly string MortalWitchClass4AlignGuid = "d005d30a-5417-4766-9d6d-908c4504f357";
        private const string MortalWitchClass6Align = "MortalWitchClass6Align";
        private static readonly string MortalWitchClass6AlignGuid = "0fd15dee-728d-4e9e-b08e-df24b2673c9c";
        private const string MortalWitchClass8Align = "MortalWitchClass8Align";
        private static readonly string MortalWitchClass8AlignGuid = "e452ff4e-595c-4062-8ae8-2004a0405917";
        private const string MortalWitchClass10Align = "MortalWitchClass10Align";
        private static readonly string MortalWitchClass10AlignGuid = "dc6c269d-8998-46ca-8a03-6b91b13b6a86";
        private const string MortalWizardClass0Align = "MortalWizardClass0Align";
        private static readonly string MortalWizardClass0AlignGuid = "87ef23be-9432-4d2b-9db3-c04c59d7c832";
        internal const string MortalWizardClass0AlignDisplayName = "EvangelistWizardClass0Align.Name";
        private const string MortalWizardClass0AlignDescription = "EvangelistWizardClass0Align.Description";
        private const string MortalWizardClass2Align = "MortalWizardClass2Align";
        private static readonly string MortalWizardClass2AlignGuid = "2db1c0ef-7ecf-43a6-be26-f71e4ef7233f";
        private const string MortalWizardClass4Align = "MortalWizardClass4Align";
        private static readonly string MortalWizardClass4AlignGuid = "3fd7a990-10a2-4ccc-9c43-8c8bae2b414b";
        private const string MortalWizardClass6Align = "MortalWizardClass6Align";
        private static readonly string MortalWizardClass6AlignGuid = "ee267a36-871d-4ea0-885a-6f761f9b64ad";
        private const string MortalWizardClass8Align = "MortalWizardClass8Align";
        private static readonly string MortalWizardClass8AlignGuid = "563e3b6e-809a-4b0b-8829-be43a079ad4d";
        private const string MortalWizardClass10Align = "MortalWizardClass10Align";
        private static readonly string MortalWizardClass10AlignGuid = "684407e5-f89e-4f15-af24-2917fb2254a7";
        private const string MortalGunslingerClass0Align = "MortalGunslingerClass0Align";
        private static readonly string MortalGunslingerClass0AlignGuid = "5ae94f2d-be40-494c-bdd3-cabce4a9bfa3";
        internal const string MortalGunslingerClass0AlignDisplayName = "EvangelistGunslingerClass0Align.Name";
        private const string MortalGunslingerClass0AlignDescription = "EvangelistGunslingerClass0Align.Description";
        private const string MortalGunslingerClass2Align = "MortalGunslingerClass2Align";
        private static readonly string MortalGunslingerClass2AlignGuid = "a0c0af75-932b-44ad-a44b-f1f76ad51923";
        private const string MortalGunslingerClass4Align = "MortalGunslingerClass4Align";
        private static readonly string MortalGunslingerClass4AlignGuid = "438883a2-865e-441b-b82f-c51c87b8d1f0";
        private const string MortalGunslingerClass6Align = "MortalGunslingerClass6Align";
        private static readonly string MortalGunslingerClass6AlignGuid = "d0273b70-93d5-4821-ae1e-b0fe1a222eb6";
        private const string MortalGunslingerClass8Align = "MortalGunslingerClass8Align";
        private static readonly string MortalGunslingerClass8AlignGuid = "a197e180-ea1f-4f59-8645-7de442994e8f";
        private const string MortalGunslingerClass10Align = "MortalGunslingerClass10Align";
        private static readonly string MortalGunslingerClass10AlignGuid = "05910b48-fc32-4a28-98f7-602ca5f74eb6";
        private const string MortalAgentoftheGraveClass0Align = "MortalAgentoftheGraveClass0Align";
        private static readonly string MortalAgentoftheGraveClass0AlignGuid = "f8683c67-e2bc-4f38-97b8-e5da41eb6cd8";
        internal const string MortalAgentoftheGraveClass0AlignDisplayName = "EvangelistAgentoftheGraveClass0Align.Name";
        private const string MortalAgentoftheGraveClass0AlignDescription = "EvangelistAgentoftheGraveClass0Align.Description";
        private const string MortalAgentoftheGraveClass2Align = "MortalAgentoftheGraveClass2Align";
        private static readonly string MortalAgentoftheGraveClass2AlignGuid = "affd89ff-7bff-49cc-8e99-87f168902b18";
        private const string MortalAgentoftheGraveClass4Align = "MortalAgentoftheGraveClass4Align";
        private static readonly string MortalAgentoftheGraveClass4AlignGuid = "81fc74da-fe34-46bb-a82f-a503b825956c";
        private const string MortalAgentoftheGraveClass6Align = "MortalAgentoftheGraveClass6Align";
        private static readonly string MortalAgentoftheGraveClass6AlignGuid = "a11f71b3-7278-4aca-935a-5fccb2790211";
        private const string MortalAgentoftheGraveClass8Align = "MortalAgentoftheGraveClass8Align";
        private static readonly string MortalAgentoftheGraveClass8AlignGuid = "1193fcf4-f2d9-4c5c-9983-36ff712f27f7";
        private const string MortalAgentoftheGraveClass10Align = "MortalAgentoftheGraveClass10Align";
        private static readonly string MortalAgentoftheGraveClass10AlignGuid = "b352f716-beef-4146-a350-2ddd8d2b294d";
        private const string MortalAnchoriteofDawnClass0Align = "MortalAnchoriteofDawnClass0Align";
        private static readonly string MortalAnchoriteofDawnClass0AlignGuid = "53bcc241-b1db-453c-a4a7-ab65fe247c31";
        internal const string MortalAnchoriteofDawnClass0AlignDisplayName = "EvangelistAnchoriteofDawnClass0Align.Name";
        private const string MortalAnchoriteofDawnClass0AlignDescription = "EvangelistAnchoriteofDawnClass0Align.Description";
        private const string MortalAnchoriteofDawnClass2Align = "MortalAnchoriteofDawnClass2Align";
        private static readonly string MortalAnchoriteofDawnClass2AlignGuid = "fddab6d3-6fb6-4d8c-9430-5810e0cff304";
        private const string MortalAnchoriteofDawnClass4Align = "MortalAnchoriteofDawnClass4Align";
        private static readonly string MortalAnchoriteofDawnClass4AlignGuid = "890c5843-b6d6-43e2-af0d-fb0d440c26d4";
        private const string MortalAnchoriteofDawnClass6Align = "MortalAnchoriteofDawnClass6Align";
        private static readonly string MortalAnchoriteofDawnClass6AlignGuid = "e0e8d43f-7287-4e48-98b9-3ae5679e9a8c";
        private const string MortalAnchoriteofDawnClass8Align = "MortalAnchoriteofDawnClass8Align";
        private static readonly string MortalAnchoriteofDawnClass8AlignGuid = "e09fcc75-e924-49ab-841c-5451068041f0";
        private const string MortalAnchoriteofDawnClass10Align = "MortalAnchoriteofDawnClass10Align";
        private static readonly string MortalAnchoriteofDawnClass10AlignGuid = "8fc00bc2-9c48-42cf-ae4a-5de106ca6768";
        private const string MortalArcaneAcherClass0Align = "MortalArcaneAcherClass0Align";
        private static readonly string MortalArcaneAcherClass0AlignGuid = "c42fd138-50f3-41a7-9122-ec900c6d91f0";
        internal const string MortalArcaneAcherClass0AlignDisplayName = "EvangelistArcaneAcherClass0Align.Name";
        private const string MortalArcaneAcherClass0AlignDescription = "EvangelistArcaneAcherClass0Align.Description";
        private const string MortalArcaneAcherClass2Align = "MortalArcaneAcherClass2Align";
        private static readonly string MortalArcaneAcherClass2AlignGuid = "9696ab53-bace-4d0b-8191-a7eeea734bf2";
        private const string MortalArcaneAcherClass4Align = "MortalArcaneAcherClass4Align";
        private static readonly string MortalArcaneAcherClass4AlignGuid = "f01a0c58-2361-4904-a99a-3e03ebdb5ae3";
        private const string MortalArcaneAcherClass6Align = "MortalArcaneAcherClass6Align";
        private static readonly string MortalArcaneAcherClass6AlignGuid = "4e4983f0-1d25-4fa8-aaf9-47cd936c4df1";
        private const string MortalArcaneAcherClass8Align = "MortalArcaneAcherClass8Align";
        private static readonly string MortalArcaneAcherClass8AlignGuid = "24a8c557-7c8c-462e-b537-0be9785e6c8c";
        private const string MortalArcaneAcherClass10Align = "MortalArcaneAcherClass10Align";
        private static readonly string MortalArcaneAcherClass10AlignGuid = "f4dabc60-a3cf-4e06-b6be-f28912229d54";
        private const string MortalAsavirClass0Align = "MortalAsavirClass0Align";
        private static readonly string MortalAsavirClass0AlignGuid = "a0237036-5767-46a6-9594-9a25b1f5375c";
        internal const string MortalAsavirClass0AlignDisplayName = "EvangelistAsavirClass0Align.Name";
        private const string MortalAsavirClass0AlignDescription = "EvangelistAsavirClass0Align.Description";
        private const string MortalAsavirClass2Align = "MortalAsavirClass2Align";
        private static readonly string MortalAsavirClass2AlignGuid = "766b2d34-0ce4-4ba7-adb2-8530e75957ac";
        private const string MortalAsavirClass4Align = "MortalAsavirClass4Align";
        private static readonly string MortalAsavirClass4AlignGuid = "3c4b4af1-3046-45c6-86b2-2ac7c6bc546d";
        private const string MortalAsavirClass6Align = "MortalAsavirClass6Align";
        private static readonly string MortalAsavirClass6AlignGuid = "3a6d3286-f141-4f04-ad44-94996c67774a";
        private const string MortalAsavirClass8Align = "MortalAsavirClass8Align";
        private static readonly string MortalAsavirClass8AlignGuid = "e910c658-a287-4a3d-8e03-ae4c06f98b71";
        private const string MortalAsavirClass10Align = "MortalAsavirClass10Align";
        private static readonly string MortalAsavirClass10AlignGuid = "5cf806f9-d480-410d-8b64-fa1b1b56160e";
        private const string MortalChevalierClass0Align = "MortalChevalierClass0Align";
        private static readonly string MortalChevalierClass0AlignGuid = "4075b311-c6c3-4d64-84c8-2737debfb29c";
        internal const string MortalChevalierClass0AlignDisplayName = "EvangelistChevalierClass0Align.Name";
        private const string MortalChevalierClass0AlignDescription = "EvangelistChevalierClass0Align.Description";
        private const string MortalChevalierClass2Align = "MortalChevalierClass2Align";
        private static readonly string MortalChevalierClass2AlignGuid = "ec4ab360-8391-4863-b368-55018ecdaf43";
        private const string MortalChevalierClass4Align = "MortalChevalierClass4Align";
        private static readonly string MortalChevalierClass4AlignGuid = "375c9b4d-b902-4f15-b11a-49327685eac0";
        private const string MortalChevalierClass6Align = "MortalChevalierClass6Align";
        private static readonly string MortalChevalierClass6AlignGuid = "e8559390-ef72-482d-9fac-06feddc247e8";
        private const string MortalChevalierClass8Align = "MortalChevalierClass8Align";
        private static readonly string MortalChevalierClass8AlignGuid = "f8be4264-f895-4be4-9ab7-ac10c4f020f0";
        private const string MortalChevalierClass10Align = "MortalChevalierClass10Align";
        private static readonly string MortalChevalierClass10AlignGuid = "56b26a04-b8da-4cbf-b099-64ed2a96a9f6";
        private const string MortalCrimsonTemplarClass0Align = "MortalCrimsonTemplarClass0Align";
        private static readonly string MortalCrimsonTemplarClass0AlignGuid = "6b61c308-e075-4473-9eab-4dede7b3c8c0";
        internal const string MortalCrimsonTemplarClass0AlignDisplayName = "EvangelistCrimsonTemplarClass0Align.Name";
        private const string MortalCrimsonTemplarClass0AlignDescription = "EvangelistCrimsonTemplarClass0Align.Description";
        private const string MortalCrimsonTemplarClass2Align = "MortalCrimsonTemplarClass2Align";
        private static readonly string MortalCrimsonTemplarClass2AlignGuid = "25df3d70-ea9f-4fae-8be9-08470706b2cf";
        private const string MortalCrimsonTemplarClass4Align = "MortalCrimsonTemplarClass4Align";
        private static readonly string MortalCrimsonTemplarClass4AlignGuid = "ebfa71f9-22c1-436a-a7e9-653552c0f378";
        private const string MortalCrimsonTemplarClass6Align = "MortalCrimsonTemplarClass6Align";
        private static readonly string MortalCrimsonTemplarClass6AlignGuid = "294b1c3e-97a5-46a7-8515-3986a0db90a9";
        private const string MortalCrimsonTemplarClass8Align = "MortalCrimsonTemplarClass8Align";
        private static readonly string MortalCrimsonTemplarClass8AlignGuid = "10fa0333-1568-4e54-90a0-0aea2a32f355";
        private const string MortalCrimsonTemplarClass10Align = "MortalCrimsonTemplarClass10Align";
        private static readonly string MortalCrimsonTemplarClass10AlignGuid = "1a610095-97fb-401a-b05e-fe3122a25847";
        private const string MortalDeadeyeDevoteeClass0Align = "MortalDeadeyeDevoteeClass0Align";
        private static readonly string MortalDeadeyeDevoteeClass0AlignGuid = "7a3a6a7b-8cc0-4f4f-912e-4e3c7ff1115d";
        internal const string MortalDeadeyeDevoteeClass0AlignDisplayName = "EvangelistDeadeyeDevoteeClass0Align.Name";
        private const string MortalDeadeyeDevoteeClass0AlignDescription = "EvangelistDeadeyeDevoteeClass0Align.Description";
        private const string MortalDeadeyeDevoteeClass2Align = "MortalDeadeyeDevoteeClass2Align";
        private static readonly string MortalDeadeyeDevoteeClass2AlignGuid = "6d64e8ab-528c-48df-900b-fb900b634d31";
        private const string MortalDeadeyeDevoteeClass4Align = "MortalDeadeyeDevoteeClass4Align";
        private static readonly string MortalDeadeyeDevoteeClass4AlignGuid = "4a7b2c92-b7a0-47dd-b1cb-022d4c6a8169";
        private const string MortalDeadeyeDevoteeClass6Align = "MortalDeadeyeDevoteeClass6Align";
        private static readonly string MortalDeadeyeDevoteeClass6AlignGuid = "64059f38-7515-4671-afe0-ce8b43402cb8";
        private const string MortalDeadeyeDevoteeClass8Align = "MortalDeadeyeDevoteeClass8Align";
        private static readonly string MortalDeadeyeDevoteeClass8AlignGuid = "a831adc7-1a1d-46ea-8ffa-cb7de7009424";
        private const string MortalDeadeyeDevoteeClass10Align = "MortalDeadeyeDevoteeClass10Align";
        private static readonly string MortalDeadeyeDevoteeClass10AlignGuid = "9ba0f02a-a2b3-4e66-a581-57867a794cbb";
        private const string MortalDragonFuryClass0Align = "MortalDragonFuryClass0Align";
        private static readonly string MortalDragonFuryClass0AlignGuid = "91126beb-386e-4ab6-907f-6025e2e8cd93";
        internal const string MortalDragonFuryClass0AlignDisplayName = "EvangelistDragonFuryClass0Align.Name";
        private const string MortalDragonFuryClass0AlignDescription = "EvangelistDragonFuryClass0Align.Description";
        private const string MortalDragonFuryClass2Align = "MortalDragonFuryClass2Align";
        private static readonly string MortalDragonFuryClass2AlignGuid = "9c626a49-3fe7-415c-93ca-912f5172285d";
        private const string MortalDragonFuryClass4Align = "MortalDragonFuryClass4Align";
        private static readonly string MortalDragonFuryClass4AlignGuid = "3e390ae3-4d7f-4583-be48-c515d8817e94";
        private const string MortalDragonFuryClass6Align = "MortalDragonFuryClass6Align";
        private static readonly string MortalDragonFuryClass6AlignGuid = "13a7fb20-3225-4c8c-8ef9-46beaa976d4a";
        private const string MortalDragonFuryClass8Align = "MortalDragonFuryClass8Align";
        private static readonly string MortalDragonFuryClass8AlignGuid = "fb7be945-8f18-4343-a3c3-ab6899e64547";
        private const string MortalDragonFuryClass10Align = "MortalDragonFuryClass10Align";
        private static readonly string MortalDragonFuryClass10AlignGuid = "087570b3-b07b-4910-9cb7-406316ad125c";
        private const string MortalEsotericKnightClass0Align = "MortalEsotericKnightClass0Align";
        private static readonly string MortalEsotericKnightClass0AlignGuid = "13a84cda-6e5c-4d08-933d-b5fa818579c0";
        internal const string MortalEsotericKnightClass0AlignDisplayName = "EvangelistEsotericKnightClass0Align.Name";
        private const string MortalEsotericKnightClass0AlignDescription = "EvangelistEsotericKnightClass0Align.Description";
        private const string MortalEsotericKnightClass2Align = "MortalEsotericKnightClass2Align";
        private static readonly string MortalEsotericKnightClass2AlignGuid = "9bf59809-b6c8-4425-a110-7d023ba5196c";
        private const string MortalEsotericKnightClass4Align = "MortalEsotericKnightClass4Align";
        private static readonly string MortalEsotericKnightClass4AlignGuid = "c257411f-2fea-49bf-b9f8-2aa4f3cd12a4";
        private const string MortalEsotericKnightClass6Align = "MortalEsotericKnightClass6Align";
        private static readonly string MortalEsotericKnightClass6AlignGuid = "4c9e1b6a-6c72-4338-9a5e-30f586937fbb";
        private const string MortalEsotericKnightClass8Align = "MortalEsotericKnightClass8Align";
        private static readonly string MortalEsotericKnightClass8AlignGuid = "50a1517f-7112-40ae-81d2-dc120a33eb5e";
        private const string MortalEsotericKnightClass10Align = "MortalEsotericKnightClass10Align";
        private static readonly string MortalEsotericKnightClass10AlignGuid = "41ac1ccd-f67b-4fa2-9140-d8137efcb2f7";
        private const string MortalExaltedEvangelistClass0Align = "MortalExaltedEvangelistClass0Align";
        private static readonly string MortalExaltedEvangelistClass0AlignGuid = "c2c740bf-4db1-456a-bdbc-fda9ff2c5b77";
        internal const string MortalExaltedEvangelistClass0AlignDisplayName = "EvangelistExaltedEvangelistClass0Align.Name";
        private const string MortalExaltedEvangelistClass0AlignDescription = "EvangelistExaltedEvangelistClass0Align.Description";
        private const string MortalExaltedEvangelistClass2Align = "MortalExaltedEvangelistClass2Align";
        private static readonly string MortalExaltedEvangelistClass2AlignGuid = "854c52bf-1c93-48cc-a05f-e0635607af33";
        private const string MortalExaltedEvangelistClass4Align = "MortalExaltedEvangelistClass4Align";
        private static readonly string MortalExaltedEvangelistClass4AlignGuid = "7141c29e-e6aa-4db4-90a6-382fee5a695c";
        private const string MortalExaltedEvangelistClass6Align = "MortalExaltedEvangelistClass6Align";
        private static readonly string MortalExaltedEvangelistClass6AlignGuid = "f54977b0-ee77-4794-a572-c1b2b7080941";
        private const string MortalExaltedEvangelistClass8Align = "MortalExaltedEvangelistClass8Align";
        private static readonly string MortalExaltedEvangelistClass8AlignGuid = "81c0334e-0cd0-4503-a580-b3c59a0ef8f4";
        private const string MortalExaltedEvangelistClass10Align = "MortalExaltedEvangelistClass10Align";
        private static readonly string MortalExaltedEvangelistClass10AlignGuid = "20dff362-2aeb-4045-afe4-cafddefe8604";
        private const string MortalFuriousGuardianClass0Align = "MortalFuriousGuardianClass0Align";
        private static readonly string MortalFuriousGuardianClass0AlignGuid = "04aeebed-9356-4c45-a7f9-1ebd3b7a0e08";
        internal const string MortalFuriousGuardianClass0AlignDisplayName = "EvangelistFuriousGuardianClass0Align.Name";
        private const string MortalFuriousGuardianClass0AlignDescription = "EvangelistFuriousGuardianClass0Align.Description";
        private const string MortalFuriousGuardianClass2Align = "MortalFuriousGuardianClass2Align";
        private static readonly string MortalFuriousGuardianClass2AlignGuid = "ca1402b6-0b26-47bd-a3e2-6d940eb1ce0d";
        private const string MortalFuriousGuardianClass4Align = "MortalFuriousGuardianClass4Align";
        private static readonly string MortalFuriousGuardianClass4AlignGuid = "8d6b514b-0f3f-44f6-b10d-006bab184629";
        private const string MortalFuriousGuardianClass6Align = "MortalFuriousGuardianClass6Align";
        private static readonly string MortalFuriousGuardianClass6AlignGuid = "d3778676-7a32-4239-9aa6-0661c6d99c91";
        private const string MortalFuriousGuardianClass8Align = "MortalFuriousGuardianClass8Align";
        private static readonly string MortalFuriousGuardianClass8AlignGuid = "4d97d452-7b21-4848-b615-d2de7e62d90a";
        private const string MortalFuriousGuardianClass10Align = "MortalFuriousGuardianClass10Align";
        private static readonly string MortalFuriousGuardianClass10AlignGuid = "9a66c954-bae2-4805-9be9-4ca65621e44a";
        private const string MortalHalflingOpportunistClass0Align = "MortalHalflingOpportunistClass0Align";
        private static readonly string MortalHalflingOpportunistClass0AlignGuid = "0596831b-e8e6-4108-b0c2-09e5f2f2ee5b";
        internal const string MortalHalflingOpportunistClass0AlignDisplayName = "EvangelistHalflingOpportunistClass0Align.Name";
        private const string MortalHalflingOpportunistClass0AlignDescription = "EvangelistHalflingOpportunistClass0Align.Description";
        private const string MortalHalflingOpportunistClass2Align = "MortalHalflingOpportunistClass2Align";
        private static readonly string MortalHalflingOpportunistClass2AlignGuid = "086b197a-9a35-4144-b24f-b1a072a6786b";
        private const string MortalHalflingOpportunistClass4Align = "MortalHalflingOpportunistClass4Align";
        private static readonly string MortalHalflingOpportunistClass4AlignGuid = "4997790f-90ea-49c7-a142-b07661d9b398";
        private const string MortalHalflingOpportunistClass6Align = "MortalHalflingOpportunistClass6Align";
        private static readonly string MortalHalflingOpportunistClass6AlignGuid = "61949428-8d7e-4518-b569-6bb46a2f04f2";
        private const string MortalHalflingOpportunistClass8Align = "MortalHalflingOpportunistClass8Align";
        private static readonly string MortalHalflingOpportunistClass8AlignGuid = "a351ddba-e6b2-4933-9eff-1d4be76aef73";
        private const string MortalHalflingOpportunistClass10Align = "MortalHalflingOpportunistClass10Align";
        private static readonly string MortalHalflingOpportunistClass10AlignGuid = "b23dc13f-3152-476a-9e2e-22e279a6b1fa";
        private const string MortalHinterlanderClass0Align = "MortalHinterlanderClass0Align";
        private static readonly string MortalHinterlanderClass0AlignGuid = "3f470b0c-2dac-487f-b6c9-2fe4a2b83a7e";
        internal const string MortalHinterlanderClass0AlignDisplayName = "EvangelistHinterlanderClass0Align.Name";
        private const string MortalHinterlanderClass0AlignDescription = "EvangelistHinterlanderClass0Align.Description";
        private const string MortalHinterlanderClass2Align = "MortalHinterlanderClass2Align";
        private static readonly string MortalHinterlanderClass2AlignGuid = "6df109f8-2b95-48c5-aea6-84d12950f8a5";
        private const string MortalHinterlanderClass4Align = "MortalHinterlanderClass4Align";
        private static readonly string MortalHinterlanderClass4AlignGuid = "a70c2b5b-5c70-48d1-8eb2-05d672e9802f";
        private const string MortalHinterlanderClass6Align = "MortalHinterlanderClass6Align";
        private static readonly string MortalHinterlanderClass6AlignGuid = "4b68cf2d-7acd-4cf3-a1fa-812e0b5d3924";
        private const string MortalHinterlanderClass8Align = "MortalHinterlanderClass8Align";
        private static readonly string MortalHinterlanderClass8AlignGuid = "1f32b8e6-cfd7-4449-a821-7ba233604755";
        private const string MortalHinterlanderClass10Align = "MortalHinterlanderClass10Align";
        private static readonly string MortalHinterlanderClass10AlignGuid = "7bbd1f08-1ac3-46bc-8d6a-37e4000b3124";
        private const string MortalHorizonWalkerClass0Align = "MortalHorizonWalkerClass0Align";
        private static readonly string MortalHorizonWalkerClass0AlignGuid = "e7899353-4955-4af8-9264-01810e6cd7a2";
        internal const string MortalHorizonWalkerClass0AlignDisplayName = "EvangelistHorizonWalkerClass0Align.Name";
        private const string MortalHorizonWalkerClass0AlignDescription = "EvangelistHorizonWalkerClass0Align.Description";
        private const string MortalHorizonWalkerClass2Align = "MortalHorizonWalkerClass2Align";
        private static readonly string MortalHorizonWalkerClass2AlignGuid = "344f5bd5-bd57-4d24-999c-db59df4ff939";
        private const string MortalHorizonWalkerClass4Align = "MortalHorizonWalkerClass4Align";
        private static readonly string MortalHorizonWalkerClass4AlignGuid = "c944575f-c809-4704-b1a8-5671e47accc3";
        private const string MortalHorizonWalkerClass6Align = "MortalHorizonWalkerClass6Align";
        private static readonly string MortalHorizonWalkerClass6AlignGuid = "2b3c6917-fd55-4e90-8994-dfc39d448b54";
        private const string MortalHorizonWalkerClass8Align = "MortalHorizonWalkerClass8Align";
        private static readonly string MortalHorizonWalkerClass8AlignGuid = "bb373f2a-e5a3-4332-8c9c-1251178a5fd4";
        private const string MortalHorizonWalkerClass10Align = "MortalHorizonWalkerClass10Align";
        private static readonly string MortalHorizonWalkerClass10AlignGuid = "c027519b-81d4-418c-adf1-3e02236b3c83";
        private const string MortalInheritorCrusaderClass0Align = "MortalInheritorCrusaderClass0Align";
        private static readonly string MortalInheritorCrusaderClass0AlignGuid = "c6a5c4d5-518a-4379-bbef-68734f50c6dc";
        internal const string MortalInheritorCrusaderClass0AlignDisplayName = "EvangelistInheritorCrusaderClass0Align.Name";
        private const string MortalInheritorCrusaderClass0AlignDescription = "EvangelistInheritorCrusaderClass0Align.Description";
        private const string MortalInheritorCrusaderClass2Align = "MortalInheritorCrusaderClass2Align";
        private static readonly string MortalInheritorCrusaderClass2AlignGuid = "d8bb6363-e27b-424c-a10d-d7993b64e1f5";
        private const string MortalInheritorCrusaderClass4Align = "MortalInheritorCrusaderClass4Align";
        private static readonly string MortalInheritorCrusaderClass4AlignGuid = "38ab415f-9d8b-495b-b27c-1538d9227b3d";
        private const string MortalInheritorCrusaderClass6Align = "MortalInheritorCrusaderClass6Align";
        private static readonly string MortalInheritorCrusaderClass6AlignGuid = "7d0bb1d2-eb86-446d-8dde-9ed7059e051e";
        private const string MortalInheritorCrusaderClass8Align = "MortalInheritorCrusaderClass8Align";
        private static readonly string MortalInheritorCrusaderClass8AlignGuid = "63d5d0b4-104c-486b-8ed7-3439cec059e3";
        private const string MortalInheritorCrusaderClass10Align = "MortalInheritorCrusaderClass10Align";
        private static readonly string MortalInheritorCrusaderClass10AlignGuid = "16c5cf7e-ccc9-486f-96e8-c0466b5812bd";
        private const string MortalMammothRiderClass0Align = "MortalMammothRiderClass0Align";
        private static readonly string MortalMammothRiderClass0AlignGuid = "dbb746ac-6de7-4817-a794-4070033157fd";
        internal const string MortalMammothRiderClass0AlignDisplayName = "EvangelistMammothRiderClass0Align.Name";
        private const string MortalMammothRiderClass0AlignDescription = "EvangelistMammothRiderClass0Align.Description";
        private const string MortalMammothRiderClass2Align = "MortalMammothRiderClass2Align";
        private static readonly string MortalMammothRiderClass2AlignGuid = "e11000e3-6c5b-4d58-9738-18bc36a6ca42";
        private const string MortalMammothRiderClass4Align = "MortalMammothRiderClass4Align";
        private static readonly string MortalMammothRiderClass4AlignGuid = "7441d12e-bdf1-4301-8904-ff7bd767f3c9";
        private const string MortalMammothRiderClass6Align = "MortalMammothRiderClass6Align";
        private static readonly string MortalMammothRiderClass6AlignGuid = "fa324430-8ea3-4373-8c69-30a2f37b3981";
        private const string MortalMammothRiderClass8Align = "MortalMammothRiderClass8Align";
        private static readonly string MortalMammothRiderClass8AlignGuid = "56a3a4c7-9efc-44b3-a307-7e0032aa8e9a";
        private const string MortalMammothRiderClass10Align = "MortalMammothRiderClass10Align";
        private static readonly string MortalMammothRiderClass10AlignGuid = "3356966e-27e5-4e0d-bfc0-c0a7bd70c318";
        private const string MortalSanguineAngelClass0Align = "MortalSanguineAngelClass0Align";
        private static readonly string MortalSanguineAngelClass0AlignGuid = "74a0937d-b849-43e2-89c3-4e4e04f978c8";
        internal const string MortalSanguineAngelClass0AlignDisplayName = "EvangelistSanguineAngelClass0Align.Name";
        private const string MortalSanguineAngelClass0AlignDescription = "EvangelistSanguineAngelClass0Align.Description";
        private const string MortalSanguineAngelClass2Align = "MortalSanguineAngelClass2Align";
        private static readonly string MortalSanguineAngelClass2AlignGuid = "0c092a4f-ea94-4552-9f24-b58f3a74634b";
        private const string MortalSanguineAngelClass4Align = "MortalSanguineAngelClass4Align";
        private static readonly string MortalSanguineAngelClass4AlignGuid = "7f6a654b-9488-42ad-a4fd-d3997af7c766";
        private const string MortalSanguineAngelClass6Align = "MortalSanguineAngelClass6Align";
        private static readonly string MortalSanguineAngelClass6AlignGuid = "6bf148f8-cda6-4ccd-99fe-5a7d21c483f6";
        private const string MortalSanguineAngelClass8Align = "MortalSanguineAngelClass8Align";
        private static readonly string MortalSanguineAngelClass8AlignGuid = "89a200c7-1702-43e9-9250-bfb601025e81";
        private const string MortalSanguineAngelClass10Align = "MortalSanguineAngelClass10Align";
        private static readonly string MortalSanguineAngelClass10AlignGuid = "fb48a29d-8f1b-4ba1-be17-a913590b4e56";
        private const string MortalScarSeekerClass0Align = "MortalScarSeekerClass0Align";
        private static readonly string MortalScarSeekerClass0AlignGuid = "235de586-2e2c-4d1b-9678-f0e5a9b46593";
        internal const string MortalScarSeekerClass0AlignDisplayName = "EvangelistScarSeekerClass0Align.Name";
        private const string MortalScarSeekerClass0AlignDescription = "EvangelistScarSeekerClass0Align.Description";
        private const string MortalScarSeekerClass2Align = "MortalScarSeekerClass2Align";
        private static readonly string MortalScarSeekerClass2AlignGuid = "952c739b-9216-41e2-950a-0e5867261caa";
        private const string MortalScarSeekerClass4Align = "MortalScarSeekerClass4Align";
        private static readonly string MortalScarSeekerClass4AlignGuid = "3c60822a-696e-4226-bf9e-32d55447ed2c";
        private const string MortalScarSeekerClass6Align = "MortalScarSeekerClass6Align";
        private static readonly string MortalScarSeekerClass6AlignGuid = "62e45edb-e8f2-4ee0-b1ae-0262ad8fb7d8";
        private const string MortalScarSeekerClass8Align = "MortalScarSeekerClass8Align";
        private static readonly string MortalScarSeekerClass8AlignGuid = "b3a99bc3-60b1-4164-8b50-d54c72f7aeff";
        private const string MortalScarSeekerClass10Align = "MortalScarSeekerClass10Align";
        private static readonly string MortalScarSeekerClass10AlignGuid = "6dc4b5ec-d534-45e7-956b-cb1ad8928e48";
        private const string MortalSentinelClass0Align = "MortalSentinelClass0Align";
        private static readonly string MortalSentinelClass0AlignGuid = "43a9887e-eb89-41a8-a8a2-bbcbdf427996";
        internal const string MortalSentinelClass0AlignDisplayName = "EvangelistSentinelClass0Align.Name";
        private const string MortalSentinelClass0AlignDescription = "EvangelistSentinelClass0Align.Description";
        private const string MortalSentinelClass2Align = "MortalSentinelClass2Align";
        private static readonly string MortalSentinelClass2AlignGuid = "1da256fc-6c6f-4375-ae05-8880ca324acc";
        private const string MortalSentinelClass4Align = "MortalSentinelClass4Align";
        private static readonly string MortalSentinelClass4AlignGuid = "3d2694d4-4ba6-4941-b7cd-b1f2dca03ece";
        private const string MortalSentinelClass6Align = "MortalSentinelClass6Align";
        private static readonly string MortalSentinelClass6AlignGuid = "1115aa12-dc04-4542-924d-44476d1bf5a1";
        private const string MortalSentinelClass8Align = "MortalSentinelClass8Align";
        private static readonly string MortalSentinelClass8AlignGuid = "e11c6bfa-2308-48a3-86be-1b1e22f111f0";
        private const string MortalSentinelClass10Align = "MortalSentinelClass10Align";
        private static readonly string MortalSentinelClass10AlignGuid = "f45b2d1d-965b-491c-bd54-0a484ad98ff9";
        private const string MortalShadowDancerClass0Align = "MortalShadowDancerClass0Align";
        private static readonly string MortalShadowDancerClass0AlignGuid = "98c0109c-fb4a-4ed1-8503-da4c292894f8";
        internal const string MortalShadowDancerClass0AlignDisplayName = "EvangelistShadowDancerClass0Align.Name";
        private const string MortalShadowDancerClass0AlignDescription = "EvangelistShadowDancerClass0Align.Description";
        private const string MortalShadowDancerClass2Align = "MortalShadowDancerClass2Align";
        private static readonly string MortalShadowDancerClass2AlignGuid = "100afe85-2337-4f47-8831-5de82d6800bb";
        private const string MortalShadowDancerClass4Align = "MortalShadowDancerClass4Align";
        private static readonly string MortalShadowDancerClass4AlignGuid = "ad1eaf9d-6a86-4b75-a5af-4bf64cb814ac";
        private const string MortalShadowDancerClass6Align = "MortalShadowDancerClass6Align";
        private static readonly string MortalShadowDancerClass6AlignGuid = "66632fb3-ad84-499b-a97d-a61fe7186840";
        private const string MortalShadowDancerClass8Align = "MortalShadowDancerClass8Align";
        private static readonly string MortalShadowDancerClass8AlignGuid = "e5b6f8ae-e671-4d22-a813-0ba87cd2ed99";
        private const string MortalShadowDancerClass10Align = "MortalShadowDancerClass10Align";
        private static readonly string MortalShadowDancerClass10AlignGuid = "9ac9eac0-3823-4075-871d-c05fe833cdd1";
        private const string MortalSouldrinkerClass0Align = "MortalSouldrinkerClass0Align";
        private static readonly string MortalSouldrinkerClass0AlignGuid = "3454f942-a9b3-4ef9-9291-4a5fa97fe0f2";
        internal const string MortalSouldrinkerClass0AlignDisplayName = "EvangelistSouldrinkerClass0Align.Name";
        private const string MortalSouldrinkerClass0AlignDescription = "EvangelistSouldrinkerClass0Align.Description";
        private const string MortalSouldrinkerClass2Align = "MortalSouldrinkerClass2Align";
        private static readonly string MortalSouldrinkerClass2AlignGuid = "dab695af-e053-43b3-9f94-8a14391f71ad";
        private const string MortalSouldrinkerClass4Align = "MortalSouldrinkerClass4Align";
        private static readonly string MortalSouldrinkerClass4AlignGuid = "6fe0fec2-37b7-456d-b3c4-ef1ebac8b0ec";
        private const string MortalSouldrinkerClass6Align = "MortalSouldrinkerClass6Align";
        private static readonly string MortalSouldrinkerClass6AlignGuid = "e449273a-3432-4f2f-95ef-d2789fa060af";
        private const string MortalSouldrinkerClass8Align = "MortalSouldrinkerClass8Align";
        private static readonly string MortalSouldrinkerClass8AlignGuid = "a65713a2-af7b-4eb1-b5db-c080debaabad";
        private const string MortalSouldrinkerClass10Align = "MortalSouldrinkerClass10Align";
        private static readonly string MortalSouldrinkerClass10AlignGuid = "3062fbce-8530-4795-a8fe-01a808520844";
        private const string MortalUmbralAgentClass0Align = "MortalUmbralAgentClass0Align";
        private static readonly string MortalUmbralAgentClass0AlignGuid = "929ba47e-3062-41a9-a73f-e5e81ee94011";
        internal const string MortalUmbralAgentClass0AlignDisplayName = "EvangelistUmbralAgentClass0Align.Name";
        private const string MortalUmbralAgentClass0AlignDescription = "EvangelistUmbralAgentClass0Align.Description";
        private const string MortalUmbralAgentClass2Align = "MortalUmbralAgentClass2Align";
        private static readonly string MortalUmbralAgentClass2AlignGuid = "f86b75a0-c103-4a93-be4d-2f7b1affa609";
        private const string MortalUmbralAgentClass4Align = "MortalUmbralAgentClass4Align";
        private static readonly string MortalUmbralAgentClass4AlignGuid = "1d0bc7fe-fd59-4ca2-8c0a-b17f64fefe3f";
        private const string MortalUmbralAgentClass6Align = "MortalUmbralAgentClass6Align";
        private static readonly string MortalUmbralAgentClass6AlignGuid = "5385b996-f6ba-486d-8663-beab0a1f5a17";
        private const string MortalUmbralAgentClass8Align = "MortalUmbralAgentClass8Align";
        private static readonly string MortalUmbralAgentClass8AlignGuid = "207ae530-77fe-4660-acdb-6492cb95ecd3";
        private const string MortalUmbralAgentClass10Align = "MortalUmbralAgentClass10Align";
        private static readonly string MortalUmbralAgentClass10AlignGuid = "2e408a67-8a89-473e-bc88-3e3a1d5cf2fd";
        private const string MortalMicroAntiPaladinClass0Align = "MortalMicroAntiPaladinClass0Align";
        private static readonly string MortalMicroAntiPaladinClass0AlignGuid = "32fabffc-e967-4336-9bad-1314083007ec";
        internal const string MortalMicroAntiPaladinClass0AlignDisplayName = "EvangelistMicroAntiPaladinClass0Align.Name";
        private const string MortalMicroAntiPaladinClass0AlignDescription = "EvangelistMicroAntiPaladinClass0Align.Description";
        private const string MortalMicroAntiPaladinClass2Align = "MortalMicroAntiPaladinClass2Align";
        private static readonly string MortalMicroAntiPaladinClass2AlignGuid = "67dbc538-9561-4d23-b780-84a3c468b296";
        private const string MortalMicroAntiPaladinClass4Align = "MortalMicroAntiPaladinClass4Align";
        private static readonly string MortalMicroAntiPaladinClass4AlignGuid = "00390db2-286e-46bc-a5e1-1e8fa1f624af";
        private const string MortalMicroAntiPaladinClass6Align = "MortalMicroAntiPaladinClass6Align";
        private static readonly string MortalMicroAntiPaladinClass6AlignGuid = "6647f39a-c43a-449c-a4ec-65323f1e275a";
        private const string MortalMicroAntiPaladinClass8Align = "MortalMicroAntiPaladinClass8Align";
        private static readonly string MortalMicroAntiPaladinClass8AlignGuid = "63c2d7bc-e683-4e54-af35-36b680fb184a";
        private const string MortalMicroAntiPaladinClass10Align = "MortalMicroAntiPaladinClass10Align";
        private static readonly string MortalMicroAntiPaladinClass10AlignGuid = "f28e8a80-935d-473f-964e-09dffb034089";
        private const string MortalOathbreakerClass0Align = "MortalOathbreakerClass0Align";
        private static readonly string MortalOathbreakerClass0AlignGuid = "2b260f5a-9e32-4f02-9ce7-5ea9dfffba5a";
        internal const string MortalOathbreakerClass0AlignDisplayName = "EvangelistOathbreakerClass0Align.Name";
        private const string MortalOathbreakerClass0AlignDescription = "EvangelistOathbreakerClass0Align.Description";
        private const string MortalOathbreakerClass2Align = "MortalOathbreakerClass2Align";
        private static readonly string MortalOathbreakerClass2AlignGuid = "ad9ac6e5-091c-4933-abbf-6e90a1f51577";
        private const string MortalOathbreakerClass4Align = "MortalOathbreakerClass4Align";
        private static readonly string MortalOathbreakerClass4AlignGuid = "ff12e922-b15f-4b12-8d7a-24e22bb945a2";
        private const string MortalOathbreakerClass6Align = "MortalOathbreakerClass6Align";
        private static readonly string MortalOathbreakerClass6AlignGuid = "aa8d4f8a-7212-45d7-b383-9a3ca19c7735";
        private const string MortalOathbreakerClass8Align = "MortalOathbreakerClass8Align";
        private static readonly string MortalOathbreakerClass8AlignGuid = "f3819201-4c68-4744-bc68-0a71307ffac7";
        private const string MortalOathbreakerClass10Align = "MortalOathbreakerClass10Align";
        private static readonly string MortalOathbreakerClass10AlignGuid = "990f8ded-e443-430a-ab8f-02f2db699242";
        private const string MortalDreadKnightClass0Align = "MortalDreadKnightClass0Align";
        private static readonly string MortalDreadKnightClass0AlignGuid = "28563f89-858f-4cfb-9c62-3584f25c4ca9";
        internal const string MortalDreadKnightClass0AlignDisplayName = "EvangelistDreadKnightClass0Align.Name";
        private const string MortalDreadKnightClass0AlignDescription = "EvangelistDreadKnightClass0Align.Description";
        private const string MortalDreadKnightClass2Align = "MortalDreadKnightClass2Align";
        private static readonly string MortalDreadKnightClass2AlignGuid = "592e37e1-7616-495e-95b1-6a7edd0fa1cb";
        private const string MortalDreadKnightClass4Align = "MortalDreadKnightClass4Align";
        private static readonly string MortalDreadKnightClass4AlignGuid = "b3422f1b-230e-4c18-99de-fe1781481c2e";
        private const string MortalDreadKnightClass6Align = "MortalDreadKnightClass6Align";
        private static readonly string MortalDreadKnightClass6AlignGuid = "13adda4e-4767-4ca5-b42f-1d99fd6da783";
        private const string MortalDreadKnightClass8Align = "MortalDreadKnightClass8Align";
        private static readonly string MortalDreadKnightClass8AlignGuid = "d28f184d-772b-4f4e-a15c-b1a435eabf45";
        private const string MortalDreadKnightClass10Align = "MortalDreadKnightClass10Align";
        private static readonly string MortalDreadKnightClass10AlignGuid = "e9c6826a-d489-4f5d-94a9-2a6590dcd32e";
        private const string MortalStargazerClass0Align = "MortalStargazerClass0Align";
        private static readonly string MortalStargazerClass0AlignGuid = "c9d8e70e-5aa6-48f1-9f59-8c8ca3ceb937";
        internal const string MortalStargazerClass0AlignDisplayName = "EvangelistStargazerClass0Align.Name";
        private const string MortalStargazerClass0AlignDescription = "EvangelistStargazerClass0Align.Description";
        private const string MortalStargazerClass2Align = "MortalStargazerClass2Align";
        private static readonly string MortalStargazerClass2AlignGuid = "971e0420-f9f0-42cc-b284-43b7aa9b386a";
        private const string MortalStargazerClass4Align = "MortalStargazerClass4Align";
        private static readonly string MortalStargazerClass4AlignGuid = "73fd1cff-b428-4bcd-97b0-8595ce22053d";
        private const string MortalStargazerClass6Align = "MortalStargazerClass6Align";
        private static readonly string MortalStargazerClass6AlignGuid = "46deae7b-e3c7-40d9-8fa9-4825ee62d887";
        private const string MortalStargazerClass8Align = "MortalStargazerClass8Align";
        private static readonly string MortalStargazerClass8AlignGuid = "d33831c9-31f4-4aed-a8fb-13e1ff7146bd";
        private const string MortalStargazerClass10Align = "MortalStargazerClass10Align";
        private static readonly string MortalStargazerClass10AlignGuid = "91b4a8d8-778e-47d7-8eb1-e74c42586e25";
        private const string MortalSwashbucklerClass0Align = "MortalSwashbucklerClass0Align";
        private static readonly string MortalSwashbucklerClass0AlignGuid = "56974121-89b8-4660-827f-151dd5d66252";
        internal const string MortalSwashbucklerClass0AlignDisplayName = "EvangelistSwashbucklerClass0Align.Name";
        private const string MortalSwashbucklerClass0AlignDescription = "EvangelistSwashbucklerClass0Align.Description";
        private const string MortalSwashbucklerClass2Align = "MortalSwashbucklerClass2Align";
        private static readonly string MortalSwashbucklerClass2AlignGuid = "6efe763a-dc0c-4c6f-b608-6ff85872ee42";
        private const string MortalSwashbucklerClass4Align = "MortalSwashbucklerClass4Align";
        private static readonly string MortalSwashbucklerClass4AlignGuid = "f1402422-ad2b-46e2-9726-60c145e4c370";
        private const string MortalSwashbucklerClass6Align = "MortalSwashbucklerClass6Align";
        private static readonly string MortalSwashbucklerClass6AlignGuid = "45fba4dd-062f-43cc-af1c-8f2deb152904";
        private const string MortalSwashbucklerClass8Align = "MortalSwashbucklerClass8Align";
        private static readonly string MortalSwashbucklerClass8AlignGuid = "e48281bb-cc62-4435-b703-21e7007e78b7";
        private const string MortalSwashbucklerClass10Align = "MortalSwashbucklerClass10Align";
        private static readonly string MortalSwashbucklerClass10AlignGuid = "a8f1674b-5f08-4712-8b15-a8a8889cd25f";
        private const string MortalHolyVindicatorClass0Align = "MortalHolyVindicatorClass0Align";
        private static readonly string MortalHolyVindicatorClass0AlignGuid = "3a5ba059-d664-40c7-a36f-41aa2da015d2";
        internal const string MortalHolyVindicatorClass0AlignDisplayName = "EvangelistHolyVindicatorClass0Align.Name";
        private const string MortalHolyVindicatorClass0AlignDescription = "EvangelistHolyVindicatorClass0Align.Description";
        private const string MortalHolyVindicatorClass2Align = "MortalHolyVindicatorClass2Align";
        private static readonly string MortalHolyVindicatorClass2AlignGuid = "0880b52d-a150-4259-819b-fbddbe113b72";
        private const string MortalHolyVindicatorClass4Align = "MortalHolyVindicatorClass4Align";
        private static readonly string MortalHolyVindicatorClass4AlignGuid = "f975b97a-5be6-4d72-bf3e-5fdda8ed77de";
        private const string MortalHolyVindicatorClass6Align = "MortalHolyVindicatorClass6Align";
        private static readonly string MortalHolyVindicatorClass6AlignGuid = "4914b907-7a21-4ef4-a53d-8d74e2cf3d73";
        private const string MortalHolyVindicatorClass8Align = "MortalHolyVindicatorClass8Align";
        private static readonly string MortalHolyVindicatorClass8AlignGuid = "dbd41621-9a2b-4bd2-aecc-05622e07680a";
        private const string MortalHolyVindicatorClass10Align = "MortalHolyVindicatorClass10Align";
        private static readonly string MortalHolyVindicatorClass10AlignGuid = "75ca119d-2ce6-400a-bd2e-38ea043420c5";
        private const string MortalSummonerClass0Align = "MortalSummonerClass0Align";
        private static readonly string MortalSummonerClass0AlignGuid = "6326a5a9-8672-4bc6-9ca5-fc7b1310e3d9";
        internal const string MortalSummonerClass0AlignDisplayName = "EvangelistSummonerClass0Align.Name";
        private const string MortalSummonerClass0AlignDescription = "EvangelistSummonerClass0Align.Description";
        private const string MortalSummonerClass2Align = "MortalSummonerClass2Align";
        private static readonly string MortalSummonerClass2AlignGuid = "c18ec01b-1ea4-4fa1-ad67-1ca6e1f579a1";
        private const string MortalSummonerClass4Align = "MortalSummonerClass4Align";
        private static readonly string MortalSummonerClass4AlignGuid = "c92288fe-f3e8-464e-bc52-4b7e86d88c76";
        private const string MortalSummonerClass6Align = "MortalSummonerClass6Align";
        private static readonly string MortalSummonerClass6AlignGuid = "89f50a24-1cb0-44e8-a423-2c57e48526b0";
        private const string MortalSummonerClass8Align = "MortalSummonerClass8Align";
        private static readonly string MortalSummonerClass8AlignGuid = "dab65f61-0d66-4f05-a168-f4eca4f5533a";
        private const string MortalSummonerClass10Align = "MortalSummonerClass10Align";
        private static readonly string MortalSummonerClass10AlignGuid = "5ba08a56-d088-4b9d-8a52-2c1673b7486c";
        private const string MortalLionBladeClass0Align = "MortalLionBladeClass0Align";
        private static readonly string MortalLionBladeClass0AlignGuid = "f5cdfb8d-dc8a-43e8-abe3-79916ba035b4";
        internal const string MortalLionBladeClass0AlignDisplayName = "EvangelistLionBladeClass0Align.Name";
        private const string MortalLionBladeClass0AlignDescription = "EvangelistLionBladeClass0Align.Description";
        private const string MortalLionBladeClass2Align = "MortalLionBladeClass2Align";
        private static readonly string MortalLionBladeClass2AlignGuid = "a6d102ab-3ee6-41c1-a501-e07d7f5a6573";
        private const string MortalLionBladeClass4Align = "MortalLionBladeClass4Align";
        private static readonly string MortalLionBladeClass4AlignGuid = "fd8a3f94-0e00-47ed-ad83-4f0fe702bf2d";
        private const string MortalLionBladeClass6Align = "MortalLionBladeClass6Align";
        private static readonly string MortalLionBladeClass6AlignGuid = "90743733-f981-4057-9320-0dd08d890329";
        private const string MortalLionBladeClass8Align = "MortalLionBladeClass8Align";
        private static readonly string MortalLionBladeClass8AlignGuid = "d492e3e3-e186-4db6-81fa-9f37eb7ceee0";
        private const string MortalLionBladeClass10Align = "MortalLionBladeClass10Align";
        private static readonly string MortalLionBladeClass10AlignGuid = "11cd6fbe-9862-4c97-b989-9adb24f1fb93";
        private const string MortalEnchantingCourtesanClass0Align = "MortalEnchantingCourtesanClass0Align";
        private static readonly string MortalEnchantingCourtesanClass0AlignGuid = "d06cabe1-3d8b-4804-9524-bce7c02f7841";
        internal const string MortalEnchantingCourtesanClass0AlignDisplayName = "EvangelistEnchantingCourtesanClass0Align.Name";
        private const string MortalEnchantingCourtesanClass0AlignDescription = "EvangelistEnchantingCourtesanClass0Align.Description";
        private const string MortalEnchantingCourtesanClass2Align = "MortalEnchantingCourtesanClass2Align";
        private static readonly string MortalEnchantingCourtesanClass2AlignGuid = "884dc53c-429c-4922-8d9a-dcee95fb846d";
        private const string MortalEnchantingCourtesanClass4Align = "MortalEnchantingCourtesanClass4Align";
        private static readonly string MortalEnchantingCourtesanClass4AlignGuid = "2ff8005e-9fb3-4e4a-b156-01aedeb3270e";
        private const string MortalEnchantingCourtesanClass6Align = "MortalEnchantingCourtesanClass6Align";
        private static readonly string MortalEnchantingCourtesanClass6AlignGuid = "e413cf80-f380-4399-9a00-753587eb7c7a";
        private const string MortalEnchantingCourtesanClass8Align = "MortalEnchantingCourtesanClass8Align";
        private static readonly string MortalEnchantingCourtesanClass8AlignGuid = "0c8df232-4916-4912-9fa1-63df9c70db4d";
        private const string MortalEnchantingCourtesanClass10Align = "MortalEnchantingCourtesanClass10Align";
        private static readonly string MortalEnchantingCourtesanClass10AlignGuid = "7d75f53d-88b4-4faa-9b49-eb618f9a386b";
        private const string MortalHeritorKnightClass0Align = "MortalHeritorKnightClass0Align";
        private static readonly string MortalHeritorKnightClass0AlignGuid = "ce5be197-9f1b-4f96-92db-da3ae31dba5e";
        internal const string MortalHeritorKnightClass0AlignDisplayName = "EvangelistHeritorKnightClass0Align.Name";
        private const string MortalHeritorKnightClass0AlignDescription = "EvangelistHeritorKnightClass0Align.Description";
        private const string MortalHeritorKnightClass2Align = "MortalHeritorKnightClass2Align";
        private static readonly string MortalHeritorKnightClass2AlignGuid = "d5820f4a-2bce-4d51-b688-5c385c350d27";
        private const string MortalHeritorKnightClass4Align = "MortalHeritorKnightClass4Align";
        private static readonly string MortalHeritorKnightClass4AlignGuid = "f8010778-7df8-414b-9a7f-f7aa679b58df";
        private const string MortalHeritorKnightClass6Align = "MortalHeritorKnightClass6Align";
        private static readonly string MortalHeritorKnightClass6AlignGuid = "1f4ce94e-769d-4282-af52-a4fcfe5be4b0";
        private const string MortalHeritorKnightClass8Align = "MortalHeritorKnightClass8Align";
        private static readonly string MortalHeritorKnightClass8AlignGuid = "60c847a9-28b8-4b36-bac8-22a872488df4";
        private const string MortalHeritorKnightClass10Align = "MortalHeritorKnightClass10Align";
        private static readonly string MortalHeritorKnightClass10AlignGuid = "7de9d912-f282-4ca9-a308-54b770e39bf7";
        private const string MortalGoldenLegionnaireClass0Align = "MortalGoldenLegionnaireClass0Align";
        private static readonly string MortalGoldenLegionnaireClass0AlignGuid = "3f351744-a672-4d6a-ab68-062a4e451832";
        internal const string MortalGoldenLegionnaireClass0AlignDisplayName = "EvangelistGoldenLegionnaireClass0Align.Name";
        private const string MortalGoldenLegionnaireClass0AlignDescription = "EvangelistGoldenLegionnaireClass0Align.Description";
        private const string MortalGoldenLegionnaireClass2Align = "MortalGoldenLegionnaireClass2Align";
        private static readonly string MortalGoldenLegionnaireClass2AlignGuid = "1275c092-2b9d-4ab8-8d66-36de2d2d497a";
        private const string MortalGoldenLegionnaireClass4Align = "MortalGoldenLegionnaireClass4Align";
        private static readonly string MortalGoldenLegionnaireClass4AlignGuid = "9cd41c40-b587-4788-89f1-8a95f8949df4";
        private const string MortalGoldenLegionnaireClass6Align = "MortalGoldenLegionnaireClass6Align";
        private static readonly string MortalGoldenLegionnaireClass6AlignGuid = "9c28d97c-ef98-43ee-a1aa-5490abefd7ae";
        private const string MortalGoldenLegionnaireClass8Align = "MortalGoldenLegionnaireClass8Align";
        private static readonly string MortalGoldenLegionnaireClass8AlignGuid = "cf16430a-10c4-41c8-b4e3-0d478dece67c";
        private const string MortalGoldenLegionnaireClass10Align = "MortalGoldenLegionnaireClass10Align";
        private static readonly string MortalGoldenLegionnaireClass10AlignGuid = "1138f217-7ee3-44d2-b101-a7c6229ad2cf";
        private const string MortalBoltAceClass0Align = "MortalBoltAceClass0Align";
        private static readonly string MortalBoltAceClass0AlignGuid = "ee945b9b-34d3-4526-8925-ce5240442442";
        internal const string MortalBoltAceClass0AlignDisplayName = "EvangelistBoltAceClass0Align.Name";
        private const string MortalBoltAceClass0AlignDescription = "EvangelistBoltAceClass0Align.Description";
        private const string MortalBoltAceClass2Align = "MortalBoltAceClass2Align";
        private static readonly string MortalBoltAceClass2AlignGuid = "96a17271-ce41-4ef8-9343-b94c4cbeb64b";
        private const string MortalBoltAceClass4Align = "MortalBoltAceClass4Align";
        private static readonly string MortalBoltAceClass4AlignGuid = "bd7d58aa-7ee0-4ae9-9bae-04d5b92ec887";
        private const string MortalBoltAceClass6Align = "MortalBoltAceClass6Align";
        private static readonly string MortalBoltAceClass6AlignGuid = "9655a6e8-a98c-4912-8a39-77713779e05d";
        private const string MortalBoltAceClass8Align = "MortalBoltAceClass8Align";
        private static readonly string MortalBoltAceClass8AlignGuid = "77d0412c-1572-4276-b70a-c020ccb60bd0";
        private const string MortalBoltAceClass10Align = "MortalBoltAceClass10Align";
        private static readonly string MortalBoltAceClass10AlignGuid = "2485b82c-efa7-4632-af88-3727b3d4261e";
        private const string MortalForestWardenClass0Align = "MortalForestWardenClass0Align";
        private static readonly string MortalForestWardenClass0AlignGuid = "c8258d5d-9e65-4db9-8a63-db39e40ff34d";
        internal const string MortalForestWardenClass0AlignDisplayName = "EvangelistForestWardenClass0Align.Name";
        private const string MortalForestWardenClass0AlignDescription = "EvangelistForestWardenClass0Align.Description";
        private const string MortalForestWardenClass2Align = "MortalForestWardenClass2Align";
        private static readonly string MortalForestWardenClass2AlignGuid = "13fbbcb5-ad59-4017-ada9-43d66c5ef5e0";
        private const string MortalForestWardenClass4Align = "MortalForestWardenClass4Align";
        private static readonly string MortalForestWardenClass4AlignGuid = "6ddc5f5e-054f-4a14-a1b5-2f27a406b1ec";
        private const string MortalForestWardenClass6Align = "MortalForestWardenClass6Align";
        private static readonly string MortalForestWardenClass6AlignGuid = "55856275-d46b-4357-8a67-e845d0320773";
        private const string MortalForestWardenClass8Align = "MortalForestWardenClass8Align";
        private static readonly string MortalForestWardenClass8AlignGuid = "6b6df64e-9324-4e83-892c-b6a56e9f2f81";
        private const string MortalForestWardenClass10Align = "MortalForestWardenClass10Align";
        private static readonly string MortalForestWardenClass10AlignGuid = "6279cb18-2cd9-4da0-ab5f-bdc15af26808";

    }
}
