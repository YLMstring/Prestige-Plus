using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.Maneuvers;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using TabletopTweaks.Core.NewComponents;
using PrestigePlus.Modify;
using PrestigePlus.CustomAction.OtherManeuver;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class HalflingOpportunist
    {
        private const string ArchetypeName = "HalflingOpportunist";
        private static readonly string ArchetypeGuid = "{AEA57FFB-36F0-43AB-9BA6-BFB3B0D9AFF9}";
        internal const string ArchetypeDisplayName = "HalflingOpportunist.Name";
        private const string ArchetypeDescription = "HalflingOpportunist.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "HalflingOpportunistPrestige";
        private static readonly string ClassProgressGuid = "{BB330A5A-1949-46D3-B34D-662876F43011}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, CreateExploitive(), SummonSmallSelection())
                .AddToLevelEntry(2, LuckyFeat(), FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(3, FeatureRefs.CannyObserver.ToString())
                .AddToLevelEntry(4, Lucky2Feat(), FeatureRefs.SneakAttack.ToString())
                .AddToLevelEntry(5, OpportunityFeat())
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(6)
                .SetHitDie(DiceType.D8)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillThievery, StatType.SkillStealth, StatType.SkillMobility, StatType.SkillPersuasion })
                .AddPrerequisiteFeature(RaceRefs.HalflingRace.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.DefensiveCombatTraining.ToString(), group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
                .AddPrerequisiteStatValue(StatType.SkillStealth, 5)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var HalflingOpportunistref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = HalflingOpportunistref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Exploitive = "HalflingOpportunist.Exploitive";
        private static readonly string ExploitiveGuid = "{1A5088E4-3A14-465F-98B5-408B7ACB6627}";

        private const string Exploitive2 = "HalflingOpportunist.Exploitive2";
        private static readonly string ExploitiveGuid2 = "{BCCD43B4-D897-4604-9957-5429ACDFB22C}";

        private const string ExploitiveBuff = "HalflingOpportunist.ExploitiveBuff";
        private static readonly string ExploitiveGuidBuff = "{1F58B574-C1D0-4D95-9888-2F765FBA7669}";

        private const string ExploitiveBuff2 = "HalflingOpportunist.ExploitiveBuff2";
        private static readonly string ExploitiveGuidBuff2 = "{7838922C-2131-42C5-824F-587AE3626D9D}";

        private const string ExploitiveBuff3 = "HalflingOpportunist.ExploitiveBuff3";
        private static readonly string ExploitiveGuidBuff3 = "{3952D4EF-8136-48ED-BA4A-8CFE13325B2D}";

        internal const string ExploitiveDisplayName = "HalflingOpportunistExploitive.Name";
        private const string ExploitiveDescription = "HalflingOpportunistExploitive.Description";

        internal const string ExploitiveDisplayName2 = "HalflingOpportunistExploitive2.Name";
        private const string ExploitiveDescription2 = "HalflingOpportunistExploitive2.Description";
        private static BlueprintFeature CreateExploitive()
        {
            var icon = FeatureRefs.CraneStyleFeat.Reference.Get().Icon;

            var selfbuff = BuffConfigurator.New(ExploitiveBuff, ExploitiveGuidBuff)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddCMBBonus(value: ContextValues.Rank(), descriptor: ModifierDescriptor.UntypedStackable)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((2, 3), (4, 4), (5, 5)))
              .AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff, toCaster: true).Build(), onOwner: true)
              .AddManeuverTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff, toCaster: true).Build(), onlySuccess: false)
              .Configure();

            var enemydebuff = BuffConfigurator.New(ExploitiveBuff2, ExploitiveGuidBuff2)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddAttackBonusConditional(bonus: ContextValues.Rank(), descriptor: ModifierDescriptor.Penalty)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((2, -3), (4, -4), (5, -5)))
              //.AddInitiatorAttackRollTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff2, toCaster: false).Build(), onOwner: true)
              .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveBuff(ExploitiveGuidBuff2, toCaster: false).Build(), actionsOnInitiator: true, onlyHit: false)
              .Configure();

            var cooldownbuff = BuffConfigurator.New(ExploitiveBuff3, ExploitiveGuidBuff3)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
              .Configure();

            var action = ActionsBuilder.New()
                .Add<ExploitiveManeuver>()
                .Build();

            var mythicfeat = FeatureConfigurator.New(Exploitive2, ExploitiveGuid2, FeatureGroup.MythicFeat)
              .SetDisplayName(ExploitiveDisplayName2)
              .SetDescription(ExploitiveDescription2)
              .SetIcon(icon)
              .AddPrerequisiteClassLevel(ArchetypeGuid, 5)
              .AddFacts(new() { SeizetheOpportunity.ManeuverGuid })
              //.AddPrerequisiteFeature(SeizetheOpportunity.FeatGuid)
              .AddToFeatureSelection("0d3a3619-9d99-47af-8e47-cb6cc4d26821") //ttt
              .Configure();

            return FeatureConfigurator.New(Exploitive, ExploitiveGuid)
              .SetDisplayName(ExploitiveDisplayName)
              .SetDescription(ExploitiveDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddTargetAttackWithWeaponTrigger(actionsOnAttacker: action, onlyMelee: true, triggerBeforeAttack: true)
              .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New().RemoveBuff(cooldownbuff, toCaster: true).Build())
              .Configure();
        }
        private const string SummonSmall0 = "HalflingOpportunist.SummonSmall0";
        private static readonly string SummonSmallGuid0 = "{A1EE2344-5261-481C-A4D5-D3CACB77782D}";

        private const string SummonSmall2 = "HalflingOpportunist.SummonSmall2";
        private static readonly string SummonSmallGuid2 = "{E7562A3D-553B-4E88-BDA3-1DB3D86BD322}";

        private const string SummonSmall = "HalflingOpportunist.SummonSmall";
        private static readonly string SummonSmallGuid = "{0E6FBDC4-17FA-44FA-96D5-351EE26814CF}";

        internal const string SummonSmallDisplayName = "HalflingOpportunistSummonSmall.Name";
        private const string SummonSmallDescription = "HalflingOpportunistSummonSmall.Description";

        public static BlueprintFeatureSelection SummonSmallSelection()
        {
            var icon = AbilityRefs.ReducePerson.Reference.Get().Icon;

            var feat0 = FeatureConfigurator.New(SummonSmall0, SummonSmallGuid0)
              .SetDisplayName(SummonSmallDisplayName)
              .SetDescription(SummonSmallDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteFeature(RaceRefs.HalflingRace.ToString())
              .Configure();

            var feat = FeatureConfigurator.New(SummonSmall, SummonSmallGuid)
              .SetDisplayName(SummonSmallDisplayName)
              .SetDescription(SummonSmallDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddPrerequisiteNoFeature(RaceRefs.HalflingRace.ToString())
              .AddComponent<ChangeUnitBaseSize>(c => { c.SizeDelta = -1; c.m_Type = TabletopTweaks.Core.NewUnitParts.UnitPartBaseSizeAdjustment.ChangeType.Delta; })
              .Configure();

            return FeatureSelectionConfigurator.New(SummonSmall2, SummonSmallGuid2)
              .SetDisplayName(SummonSmallDisplayName)
              .SetDescription(SummonSmallDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(true)
              .AddToAllFeatures(feat0)
              .AddToAllFeatures(feat)
              .Configure();
        }

        private const string Lucky = "HalflingOpportunist.Lucky";
        private static readonly string LuckyGuid = "{70CC63B0-C72A-4577-9D35-0CB3AD6E348F}";

        internal const string LuckyDisplayName = "HalflingOpportunistLucky.Name";
        private const string LuckyDescription = "HalflingOpportunistLucky.Description";

        public static BlueprintFeature LuckyFeat()
        {
            var icon = FeatureRefs.LuckDomainBaseFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Lucky, LuckyGuid)
              .SetDisplayName(LuckyDisplayName)
              .SetDescription(LuckyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Racial, 1)
              .Configure();
        }

        private const string Lucky2 = "HalflingOpportunist.Lucky2";
        private static readonly string Lucky2Guid = "{20AFD37E-6F88-4726-B18B-A07B9248C630}";

        public static BlueprintFeature Lucky2Feat()
        {
            var icon = FeatureRefs.LuckDomainBaseFeature.Reference.Get().Icon;

            return FeatureConfigurator.New(Lucky2, Lucky2Guid)
              .SetDisplayName(LuckyDisplayName)
              .SetDescription(LuckyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddBuffAllSavesBonus(descriptor: ModifierDescriptor.Racial, 1)
              .Configure();
        }

        private const string Opportunity = "HalflingOpportunist.Opportunity";
        private static readonly string OpportunityGuid = "{1D7C9736-1309-45F1-B5E0-B45E7F3755E0}";

        internal const string OpportunityDisplayName = "HalflingOpportunistOpportunity.Name";
        private const string OpportunityDescription = "HalflingOpportunistOpportunity.Description";

        public static BlueprintFeature OpportunityFeat()
        {
            var icon = FeatureRefs.Opportunist.Reference.Get().Icon;

            return FeatureConfigurator.New(Opportunity, OpportunityGuid)
              .SetDisplayName(OpportunityDisplayName)
              .SetDescription(OpportunityDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddComponent<ForceSneakDamage>()
              .Configure();
        }
    }
}
