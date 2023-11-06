using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.CustomAction.ClassRelated;
using PrestigePlus.CustomComponent.PrestigeClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class FuriousGuardian
    {
        private const string ArchetypeName = "FuriousGuardian";
        public static readonly string ArchetypeGuid = "{D7381124-3BD3-4D63-BBB8-AF778DAA372D}";
        internal const string ArchetypeDisplayName = "FuriousGuardian.Name";
        private const string ArchetypeDescription = "FuriousGuardian.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "FuriousGuardianPrestige";
        private static readonly string ClassProgressGuid = "{B803064F-15FA-4450-BD88-48A82076246F}";

        public static void Configure()
        {
            DedicationsFeat(); 
            BlueprintProgression progression =
            ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
            .SetClasses(ArchetypeGuid)
            .AddToLevelEntry(1)
            .AddToLevelEntry(2)
            .AddToLevelEntry(3, GuardianRageGuid)
            .AddToLevelEntry(4)
            .AddToLevelEntry(6)
            .AddToLevelEntry(8)
            .AddToLevelEntry(9, GuardianRageGuid)
            .AddToLevelEntry(10)
            .SetRanks(1)
            .SetIsClassFeature(true)
            .SetDisplayName("")
            .SetDescription(ArchetypeDescription)
            .Configure();

            BlueprintCharacterClass archetype =
          CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
            .SetLocalizedName(ArchetypeDisplayName)
            .SetLocalizedDescription(ArchetypeDescription)
            .SetSkillPoints(4)
            .SetHitDie(DiceType.D10)
            .SetPrestigeClass(true)
            .SetBaseAttackBonus(BABFull)
            .SetFortitudeSave(SavesPrestigeHigh)
            .SetReflexSave(SavesPrestigeLow)
            .SetWillSave(SavesPrestigeHigh)
            .SetProgression(progression)
            .SetClassSkills(new StatType[] { StatType.SkillKnowledgeWorld, StatType.SkillPerception, StatType.SkillPersuasion })
            .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
            .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 2)
            .AddPrerequisiteStatValue(StatType.SkillPerception, 5)
            .AddPrerequisiteStatValue(StatType.SkillPersuasion, 3)
            .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var FuriousGuardianref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = FuriousGuardianref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string Dedications = "FuriousGuardian.Dedications";
        private static readonly string DedicationsGuid = "{553B034F-59BF-4FB9-8981-039574453903}";

        internal const string DedicationsDisplayName = "GuardianDedications.Name";
        private const string DedicationsDescription = "GuardianDedications.Description";

        public static void DedicationsFeat()
        {
            var icon = AbilityRefs.LifeBubble.Reference.Get().Icon;

            FeatureSelectionConfigurator.New(Dedications, DedicationsGuid)
              .SetDisplayName(DedicationsDisplayName)
              .SetDescription(DedicationsDescription)
              .SetIcon(icon)
              .SetIgnorePrerequisites(false)
              .SetObligatory(false)
              .AddToAllFeatures(FeatureRefs.CombatCasting.ToString())
              .AddToAllFeatures(FeatureRefs.ExtraRage.ToString())
              .AddToAllFeatures(FeatureRefs.ExtraRageInstinctualWarrior.ToString())
              .AddToAllFeatures(FeatureRefs.PointBlankShot.ToString())
              .AddToAllFeatures(FeatureRefs.PreciseShot.ToString())
              .AddToAllFeatures(ParametrizedFeatureRefs.SpellFocus.ToString())
              .AddToAllFeatures(FeatureRefs.SpellPenetration.ToString())
              .Configure();
        }

        private const string GuardianRage = "FuriousGuardian.GuardianRage";
        private static readonly string GuardianRageGuid = "{39473A62-E2D6-4E6F-8DB2-3D3CC4F92EA8}";

        internal const string FuriousGuardianGuardianRageDisplayName = "FuriousGuardianGuardianRage.Name";
        private const string FuriousGuardianGuardianRageDescription = "FuriousGuardianGuardianRage.Description";
        public static BlueprintFeature SAGuardianRage()
        {
            var icon = FeatureRefs.IntimidatingProwess.Reference.Get().Icon;
            return FeatureConfigurator.New(GuardianRage, GuardianRageGuid)
              .SetDisplayName(FuriousGuardianGuardianRageDisplayName)
              .SetDescription(FuriousGuardianGuardianRageDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.RageFeature.ToString() })
              .AddIncreaseResourceAmountBySharedValue(decrease: false, resource: AbilityResourceRefs.RageResourse.ToString(), value: ContextValues.Rank())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }).WithBonusValueProgression(0, true))
              .SetReapplyOnLevelUp(true)
              .Configure();
        }

        private const string ChosenAlly = "FuriousGuardian.ChosenAlly";
        public static readonly string ChosenAllyGuid = "{B1D76188-4010-4B24-90FD-23B9E698EBDF}";

        private const string ChosenAllyAblity2 = "FuriousGuardian.UseChosenAlly2";
        public static readonly string ChosenAllyAblity2Guid = "{FD19A84E-64E6-416C-A037-101406E49073}";

        private const string ChosenAllyBuff = "FuriousGuardian.ChosenAllyBuff";
        public static readonly string ChosenAllyBuffGuid = "{B762ABF7-C98E-4A70-AB4C-BBDFA0571CC2}";

        internal const string ChosenAllyDisplayName = "FuriousGuardianChosenAlly.Name";
        private const string ChosenAllyDescription = "FuriousGuardianChosenAlly.Description";

        public static BlueprintFeature ChosenAllyFeat()
        {
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            var Buff1 = BuffConfigurator.New(ChosenAllyBuff, ChosenAllyBuffGuid)
             .SetDisplayName(ChosenAllyDisplayName)
             .SetDescription(ChosenAllyDescription)
             .SetIcon(icon)
             .AddUniqueBuff()
             .AddComponent<ChosenAllyTarget>()
             .Configure();

            var ability2 = AbilityConfigurator.New(ChosenAllyAblity2, ChosenAllyAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifFalse: ActionsBuilder.New()
                        .ApplyBuff(Buff1, ContextDuration.Fixed(1))
                        .Build())
                    .Build())
                .SetDisplayName(ChosenAllyDisplayName)
                .SetDescription(ChosenAllyDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(ChosenAlly, ChosenAllyGuid)
              .SetDisplayName(ChosenAllyDisplayName)
              .SetDescription(ChosenAllyDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .AddComponent<ChosenAllyCaster>()
              .Configure();
        }

        private const string UnbreakableDefender = "FuriousGuardian.UnbreakableDefender";
        private static readonly string UnbreakableDefenderGuid = "{7FF8AAB3-E55C-4B51-8DA0-A0AD7C921180}";

        internal const string FuriousGuardianUnbreakableDefenderDisplayName = "FuriousGuardianUnbreakableDefender.Name";
        private const string FuriousGuardianUnbreakableDefenderDescription = "FuriousGuardianUnbreakableDefender.Description";
        public static BlueprintFeature SAUnbreakableDefender()
        {
            var icon = FeatureRefs.IntimidatingProwess.Reference.Get().Icon;
            return FeatureConfigurator.New(UnbreakableDefender, UnbreakableDefenderGuid)
              .SetDisplayName(FuriousGuardianUnbreakableDefenderDisplayName)
              .SetDescription(FuriousGuardianUnbreakableDefenderDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.Diehard.ToString() })
              .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Fatigued)
              .Configure();
        }

        private const string GuardedThoughts = "FuriousGuardian.GuardedThoughts";
        private static readonly string GuardedThoughtsGuid = "{8C298FEE-312C-4C7E-B222-0CFCBC0A1FFB}";

        internal const string FuriousGuardianGuardedThoughtsDisplayName = "FuriousGuardianGuardedThoughts.Name";
        private const string FuriousGuardianGuardedThoughtsDescription = "FuriousGuardianGuardedThoughts.Description";
        public static BlueprintFeature SAGuardedThoughts()
        {
            var icon = FeatureRefs.IntimidatingProwess.Reference.Get().Icon;
            return FeatureConfigurator.New(GuardedThoughts, GuardedThoughtsGuid)
              .SetDisplayName(FuriousGuardianGuardedThoughtsDisplayName)
              .SetDescription(FuriousGuardianGuardedThoughtsDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstDescriptor(5, spellDescriptor: SpellDescriptor.Charm, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Circumstance)
              .AddSavingThrowBonusAgainstDescriptor(5, spellDescriptor: SpellDescriptor.Compulsion, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.Circumstance)
              .Configure();
        }

        private const string AdaptableGuardian4 = "FuriousGuardian.AdaptableGuardian4";
        public static readonly string AdaptableGuardian4Guid = "{02C47D70-7DE6-44F9-A7A9-5E1DAFCF73D9}";

        private const string AdaptableGuardian4Ablity2 = "FuriousGuardian.UseAdaptableGuardian42";
        public static readonly string AdaptableGuardian4Ablity2Guid = "{61044A2A-415B-479E-B9A2-B1FAD2637920}";

        internal const string AdaptableGuardian4DisplayName = "FuriousGuardianAdaptableGuardian4.Name";
        private const string AdaptableGuardian4Description = "FuriousGuardianAdaptableGuardian4.Description";

        public static BlueprintFeature AdaptableGuardian4Feat()
        {
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(AdaptableGuardian4Ablity2, AdaptableGuardian4Ablity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuff(ChosenAllyBuffGuid, ContextDuration.Fixed(1))
                        .Build())
                    .Build())
                .SetDisplayName(AdaptableGuardian4DisplayName)
                .SetDescription(AdaptableGuardian4Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(AdaptableGuardian4, AdaptableGuardian4Guid)
              .SetDisplayName(AdaptableGuardian4DisplayName)
              .SetDescription(AdaptableGuardian4Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string AdaptableGuardian8 = "FuriousGuardian.AdaptableGuardian8";
        public static readonly string AdaptableGuardian8Guid = "{0921CC42-6186-417A-95CB-04D93DC895F7}";

        private const string AdaptableGuardian8Ablity2 = "FuriousGuardian.UseAdaptableGuardian82";
        public static readonly string AdaptableGuardian8Ablity2Guid = "{002069DA-F667-4F41-9ECA-9CAADB70D4D8}";

        internal const string AdaptableGuardian8DisplayName = "FuriousGuardianAdaptableGuardian8.Name";
        private const string AdaptableGuardian8Description = "FuriousGuardianAdaptableGuardian8.Description";

        public static BlueprintFeature AdaptableGuardian8Feat()
        {
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(AdaptableGuardian8Ablity2, AdaptableGuardian8Ablity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Conditional(ConditionsBuilder.New().CasterHasFact(BuffRefs.StandartRageBuff.ToString()),
                        ifTrue: ActionsBuilder.New()
                        .ApplyBuff(ChosenAllyBuffGuid, ContextDuration.Fixed(1))
                        .Build())
                    .Build())
                .SetDisplayName(AdaptableGuardian8DisplayName)
                .SetDescription(AdaptableGuardian8Description)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetRange(AbilityRange.Close)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(true)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(true)
                .Configure();

            return FeatureConfigurator.New(AdaptableGuardian8, AdaptableGuardian8Guid)
              .SetDisplayName(AdaptableGuardian8DisplayName)
              .SetDescription(AdaptableGuardian8Description)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }

        private const string TightFollower = "FuriousGuardian.TightFollower";
        public static readonly string TightFollowerGuid = "{EC077714-9D6D-433B-92E0-DB43AD354F78}";

        private const string TightFollowerAblity2 = "FuriousGuardian.UseTightFollower2";
        public static readonly string TightFollowerAblity2Guid = "{62A97B04-59B7-43D6-A8D6-AA2F7449DB95}";

        internal const string TightFollowerDisplayName = "FuriousGuardianTightFollower.Name";
        private const string TightFollowerDescription = "FuriousGuardianTightFollower.Description";

        public static BlueprintFeature TightFollowerFeat()
        {
            var icon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            var ability2 = AbilityConfigurator.New(TightFollowerAblity2, TightFollowerAblity2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<TightFollowerAction>()
                    .Build())
                .SetDisplayName(TightFollowerDisplayName)
                .SetDescription(TightFollowerDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.DoubleMove)
                .SetType(AbilityType.Special)
                .SetCanTargetSelf(false)
                .SetCanTargetEnemies(false)
                .SetCanTargetFriends(false)
                .SetCanTargetPoint(true)
                .Configure();

            return FeatureConfigurator.New(TightFollower, TightFollowerGuid)
              .SetDisplayName(TightFollowerDisplayName)
              .SetDescription(TightFollowerDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability2 })
              .Configure();
        }
    }
}
