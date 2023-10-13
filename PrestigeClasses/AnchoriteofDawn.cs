using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.AreaLogic;

namespace PrestigePlus.PrestigeClasses
{
    internal class AnchoriteofDawn
    {
        private const string ArchetypeName = "AnchoriteofDawn";
        public static readonly string ArchetypeGuid = "{18CA6378-AC25-4580-B684-31F929189459}";
        internal const string ArchetypeDisplayName = "AnchoriteofDawn.Name";
        private const string ArchetypeDescription = "AnchoriteofDawn.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "AnchoriteofDawnPrestige";
        private static readonly string ClassProgressGuid = "{4B23C85C-1859-4878-AEE0-83587526ABCA}";

        public static void Configure()
        {
            string spellupgradeGuid = "{05DC9561-0542-41BD-9E9F-404F59AB68C5}";
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, spellupgradeGuid, TenebrousGuid, SASolarInvocation())
                .AddToLevelEntry(2, FeatureRefs.BlindFight.ToString())
                .AddToLevelEntry(3)
                .AddToLevelEntry(4)
                .AddToLevelEntry(5)
                .AddToLevelEntry(6)
                .AddToLevelEntry(7)
                .AddToLevelEntry(8)
                .AddToLevelEntry(9)
                .AddToLevelEntry(10)
                .SetUIGroups(UIGroupBuilder.New()
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { spellupgradeGuid, ShadowChainsGuid, ShadowChains2Guid, ShadowChains3Guid })
                    //.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { TenebrousGuid, SolarGuid, Solar2Guid, Solar3Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] {  }))
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
                .SetBaseAttackBonus(BABMedium)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillLoreNature, StatType.SkillLoreReligion, StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.SkillLoreNature, 5)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 5)
                .AddPrerequisiteAlignment(AlignmentMaskType.Good, checkInProgression: true)
                .AddPrerequisiteCasterTypeSpellLevel(true, false, 2, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteCasterTypeSpellLevel(false, false, 2, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var AnchoriteofDawnref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = AnchoriteofDawnref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure();
        }

        private const string Tenebrous = "AnchoriteofDawn.Tenebrous";
        private static readonly string TenebrousGuid = "{3096368F-9B6A-4D06-9F47-4C2E17EF852B}";

        private const string TenebrousBuff = "AnchoriteofDawn.TenebrousBuff";
        private static readonly string TenebrousBuffGuid = "{D32CC7CB-8AE7-4698-850E-7F6FB7A0507F}";

        internal const string TenebrousDisplayName = "AnchoriteofDawnTenebrous.Name";
        private const string TenebrousDescription = "AnchoriteofDawnTenebrous.Description";

        public static void TenebrousFeat()
        {
            var icon = AbilityRefs.Thoughtsense.Reference.Get().Icon;

            var MasterBuff = BuffConfigurator.New(TenebrousBuff, TenebrousBuffGuid)
              .SetDisplayName(TenebrousDisplayName)
              .SetDescription(TenebrousDescription)
              .SetIcon(icon)
              .AddIncreaseCasterLevel(value: ContextValues.Constant(2))
              .AddIncreaseAllSpellsDC(value: ContextValues.Constant(2))
              .SetStacking(StackingType.Prolong)
              .Configure();

            var action = ActionsBuilder.New()
            .ApplyBuff(MasterBuff, durationValue: ContextDuration.VariableDice(DiceType.D4, 1))
            .Build();

            FeatureSelectionConfigurator.New(Tenebrous, TenebrousGuid)
              .SetDisplayName(TenebrousDisplayName)
              .SetDescription(TenebrousDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddAbilityUseTrigger(action: action, checkSpellSchool: true, isSpellSchool: SpellSchool.Illusion)
              .AddToAllFeatures(FeatureRefs.BolsteredSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.EmpowerSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.ExtendSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.HeightenSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.MaximizeSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.PersistentSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.QuickenSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.ReachSpellFeat.ToString())
              .AddToAllFeatures(FeatureRefs.SelectiveSpellFeat.ToString())
              .Configure();
        }

        private const string SolarInvocation = "AnchoriteofDawn.SolarInvocation";
        private static readonly string SolarInvocationGuid = "{AA35026D-1881-480F-A3D4-EA33B587A692}";

        private const string FreeSolarInvocation = "AnchoriteofDawn.FreeSolarInvocation";
        private static readonly string FreeSolarInvocationGuid = "{AA35026D-1881-480F-A3D4-EA33B587A692}";

        internal const string AnchoriteofDawnSolarInvocationDisplayName = "AnchoriteofDawnSolarInvocation.Name";
        private const string AnchoriteofDawnSolarInvocationDescription = "AnchoriteofDawnSolarInvocation.Description";

        private const string SolarAbility = "AnchoriteStyle.SolarAbility";
        private static readonly string SolarAbilityGuid = "{A915B1BF-FF14-4A82-B0A7-7BED9CF6D032}";

        private const string SolarAbilityRes = "AnchoriteStyle.SolarAbilityRes";
        private static readonly string SolarAbilityResGuid = "{BF4AE615-299C-45C8-9D92-A3278BC3CC4D}";

        private const string SolarBuff = "AnchoriteStyle.Gazebuff";
        private static readonly string SolarBuffGuid = "{04803783-BFC4-4C4D-B1F5-A76A2C2ACE90}";
        public static BlueprintFeature SASolarInvocation()
        {
            var icon = FeatureRefs.Persuasive.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SolarAbilityRes, SolarAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }, 2)
                        .IncreaseByStat(StatType.Charisma))
                .Configure();

            var Buff1 = BuffConfigurator.New(SolarBuff, SolarBuffGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddAttackBonusAgainstAlignment(AlignmentComponent.Evil, ContextValues.Rank(), descriptor: ModifierDescriptor.Competence)
              .AddDamageBonusAgainstAlignment(AlignmentComponent.Evil, ContextValues.Rank(), descriptor: ModifierDescriptor.Competence)
              .AddIncreaseAllSpellsDC(spellsOnly: true, value: 1)
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(classes: new string[] { ArchetypeGuid }).WithCustomProgression((4, 1), (8, 2), (10, 3)))
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(SolarAbility, SolarAbilityGuid)
                .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
                .SetDescription(AnchoriteofDawnSolarInvocationDescription)
                .SetIcon(icon)
                .SetBuff(Buff1)
                .SetDeactivateIfCombatEnded(true)
                .SetActivationType(AbilityActivationType.WithUnitCommand)
                .SetActivateWithUnitCommand(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AddActivatableAbilityResourceLogic(requiredResource: abilityresourse, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound, freeBlueprint: FreeSolarInvocationGuid)
                .SetDeactivateImmediately()
                .Configure();

            return FeatureConfigurator.New(SolarInvocation, SolarInvocationGuid)
              .SetDisplayName(AnchoriteofDawnSolarInvocationDisplayName)
              .SetDescription(AnchoriteofDawnSolarInvocationDescription)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
              .Configure();
        }
    }
}
