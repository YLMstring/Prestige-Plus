using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Properties;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.UnitLogic.Alignments;
using PrestigePlus.Maneuvers;
using PrestigePlus.Blueprint.Feat;
using PrestigePlus.CustomComponent;
using BlueprintCore.Conditions.Builder.BasicEx;
using PrestigePlus.CustomComponent.PrestigeClass;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class GoldenLegionnaire
    {
        private const string ArchetypeName = "GoldenLegionnaire";
        public static readonly string ArchetypeGuid = "{8DFFB0E0-3A0E-4FC7-B118-7CC1DC6B832F}";
        internal const string ArchetypeDisplayName = "GoldenLegionnaire.Name";
        private const string ArchetypeDescription = "GoldenLegionnaire.Description";

        private const string ClassProgressName = "GoldenLegionnairePrestige";
        private static readonly string ClassProgressGuid = "{FC85D290-971B-4148-8F6F-F747E9C70399}";

        public static void Configure()
        {
            //"SwiftAidAnotherFeature": "8590fb52-921c-4365-832c-ca7635fd5a70",
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, DefyDangerFeature(), FeatureRefs.Mobility.ToString())
                .AddToLevelEntry(2, SeizetheOpportunity.FeatGuid, UnitedDefenseConfigure())
                .AddToLevelEntry(3, AlliedRetributionFeature())
                .AddToLevelEntry(4, BodyGuard.FeatGuid)
                .AddToLevelEntry(5, DefyDangerGuid)
                .AddToLevelEntry(6, BodyGuard.Feat2Guid, UnitedDefenseGuid)
                .AddToLevelEntry(7, AlliedRetributionGuid)
                .AddToLevelEntry(8, "8590fb52-921c-4365-832c-ca7635fd5a70")
                .AddToLevelEntry(9, RetaliateFeature(), DefyDangerGuid)
                .AddToLevelEntry(10, FeatureRefs.PerfectStrikeFeature.ToString(), UnitedDefenseGuid)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();
            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(2)
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(StatProgressionRefs.BABFull.ToString())
                .SetFortitudeSave(StatProgressionRefs.SavesPrestigeHigh.ToString())
                .SetReflexSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetWillSave(StatProgressionRefs.SavesPrestigeLow.ToString())
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreNature })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 5)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 5)
                .AddPrerequisiteStatValue(StatType.SkillPersuasion, 5)
                .AddPrerequisiteProficiency(armorProficiencies: new ArmorProficiencyGroup[] { ArmorProficiencyGroup.Heavy }, new WeaponCategory[] { })
                .AddPrerequisiteAlignment(AlignmentMaskType.Good, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.LawfulNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.TrueNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .AddPrerequisiteAlignment(AlignmentMaskType.ChaoticNeutral, group: Kingmaker.Blueprints.Classes.Prerequisites.Prerequisite.GroupType.Any)
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string DefyDanger = "GoldenLegionnaireDefyDanger";
        private static readonly string DefyDangerGuid = "{E891A2FF-14E6-4A00-9F0E-05EE87508A2F}";

        internal const string DefyDangerDisplayName = "GoldenLegionnaireDefyDanger.Name";
        private const string DefyDangerDescription = "GoldenLegionnaireDefyDanger.Description";
        public static BlueprintFeature DefyDangerFeature()
        {
            var icon = FeatureRefs.ArchaelogistDangerSense.Reference.Get().Icon;
            return FeatureConfigurator.New(DefyDanger, DefyDangerGuid)
              .SetDisplayName(DefyDangerDisplayName)
              .SetDescription(DefyDangerDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstFactMultiple(value: 2, facts: new () { FeatureRefs.DemonOfMagicFeature.ToString(), FeatureRefs.DemonOfSlaughterFeature.ToString(), FeatureRefs.DemonOfStrengthFeature.ToString() })
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(DefyDangerGuid))
              .SetRanks(10)
              .Configure();
        }

        private const string Retaliate = "GoldenLegionnaireRetaliate";
        public static readonly string RetaliateGuid = "{798F5C7F-3FEA-4AD8-ADAA-E57FA93D20F8}";

        internal const string RetaliateDisplayName = "GoldenLegionnaireRetaliate.Name";
        private const string RetaliateDescription = "GoldenLegionnaireRetaliate.Description";
        public static BlueprintFeature RetaliateFeature()
        {
            var icon = AbilityRefs.FighterRetaliationAbility.Reference.Get().Icon;
            return FeatureConfigurator.New(Retaliate, RetaliateGuid)
              .SetDisplayName(RetaliateDisplayName)
              .SetDescription(RetaliateDescription)
              .SetIcon(icon)
              .AddComponent<GoldenLegionnaireRetaliate>()
              .Configure();
        }

        private const string SilentSoul = "GoldenLegionnaireSilentSoul";
        private static readonly string SilentSoulGuid = "{8F05CF79-26E2-4741-96B4-AFE8FD2A57BA}";

        internal const string SilentSoulDisplayName = "GoldenLegionnaireSilentSoul.Name";
        private const string SilentSoulDescription = "GoldenLegionnaireSilentSoul.Description";
        public static BlueprintFeature SilentSoulFeature()
        {
            var icon = AbilityRefs.MindBlank.Reference.Get().Icon;
            return FeatureConfigurator.New(SilentSoul, SilentSoulGuid)
              .SetDisplayName(SilentSoulDisplayName)
              .SetDescription(SilentSoulDescription)
              .SetIcon(icon)
              .AddStatBonus(ModifierDescriptor.Circumstance, false, StatType.SkillStealth, 10)
              .AddSpellResistanceAgainstSpellDescriptor(spellDescriptor: SpellDescriptor.MindAffecting, value: 20)
              .Configure();
        }

        private static readonly string NarrowMissName = "GoldenLegionnaireNarrowMiss";
        public static readonly string NarrowMissGuid = "{6316C32A-BD50-4A18-BC96-CDEDF9BEDA10}";

        private static readonly string NarrowMissDisplayName = "GoldenLegionnaireNarrowMiss.Name";
        private static readonly string NarrowMissDescription = "GoldenLegionnaireNarrowMiss.Description";

        private const string NarrowMissAbility = "NarrowMiss.NarrowMissAbility";
        private static readonly string NarrowMissAbilityGuid = "{5A16E581-674C-4EB5-855D-FB91480023B3}";

        private const string NarrowMissBuff = "NarrowMiss.NarrowMissBuff";
        private static readonly string NarrowMissBuffGuid = "{82225953-51DA-4850-A1C4-2F8714EEB3CD}";

        private const string NarrowMissAblityRes = "GoldenLegionnaire.NarrowMissRes";
        public static readonly string NarrowMissAblityResGuid = "{92FD5B7B-9BA8-4D4A-B9BF-23182F894090}";
        public static BlueprintFeature NarrowMissConfigure()
        {
            var icon = AbilityRefs.Displacement.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(NarrowMissAblityRes, NarrowMissAblityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevelStartPlusDivStep(classes: new string[] { ArchetypeGuid }, otherClassLevelsMultiplier: 0, levelsPerStep: 2, bonusPerStep: 1))
                .SetUseMax()
                .SetMax(5)
                .Configure();

            var buff = BuffConfigurator.New(NarrowMissBuff, NarrowMissBuffGuid)
                .SetDisplayName(NarrowMissDisplayName)
                .SetDescription(NarrowMissDescription)
                .SetIcon(icon)
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.WindsOfVengenance)
                .Configure();

            var ability = AbilityConfigurator.New(NarrowMissAbility, NarrowMissAbilityGuid)
                .SetDisplayName(NarrowMissDisplayName)
                .SetDescription(NarrowMissDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .Configure();

            return FeatureConfigurator.New(NarrowMissName, NarrowMissGuid)
                    .SetDisplayName(NarrowMissDisplayName)
                    .SetDescription(NarrowMissDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: true)
                    .Configure();
        }

        private static readonly string UnitedDefenseName = "GoldenLegionnaireUnitedDefense";
        public static readonly string UnitedDefenseGuid = "{7C46838D-D2D2-4DDB-931C-25E21B33E6C4}";

        private static readonly string UnitedDefenseDisplayName = "GoldenLegionnaireUnitedDefense.Name";
        private static readonly string UnitedDefenseDescription = "GoldenLegionnaireUnitedDefense.Description";

        private const string UnitedDefenseAbility = "UnitedDefense.UnitedDefenseAbility";
        private static readonly string UnitedDefenseAbilityGuid = "{B31A6E9C-FFD0-47D2-AAC5-01F8DCA14B4D}";

        private const string UnitedDefenseBuff = "UnitedDefense.UnitedDefenseBuff";
        private static readonly string UnitedDefenseBuffGuid = "{7870DA78-C273-4A1B-B213-E0AEEB6A215F}";

        private const string UnitedDefenseSelfBuff = "UnitedDefense.UnitedDefenseSelfBuff";
        private static readonly string UnitedDefenseSelfBuffGuid = "{63ECE8A4-505B-4357-8C55-2957925EB7B4}";
        public static BlueprintFeature UnitedDefenseConfigure()
        {
            var icon = FeatureRefs.BackToBack.Reference.Get().Icon;
            var fx = AbilityRefs.DivineGuardianTrothAbility.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff2 = BuffConfigurator.New(UnitedDefenseSelfBuff, UnitedDefenseSelfBuffGuid)
                .SetDisplayName(UnitedDefenseDisplayName)
                .SetDescription(UnitedDefenseDescription)
                .SetIcon(icon)
                .AddContextStatBonus(StatType.AC, ContextValues.Rank(), descriptor: ModifierDescriptor.Penalty)
                .AddContextRankConfig(ContextRankConfigs.FeatureRank(UnitedDefenseGuid).WithMultiplyByModifierProgression(-1))
                .Configure();

            var buff = BuffConfigurator.New(UnitedDefenseBuff, UnitedDefenseBuffGuid)
                .SetDisplayName(UnitedDefenseDisplayName)
                .SetDescription(UnitedDefenseDescription)
                .SetIcon(icon)
                .AddContextStatBonus(StatType.AC, ContextValues.Rank(), descriptor: ModifierDescriptor.Dodge)
                .AddContextRankConfig(ContextRankConfigs.FeatureRank(UnitedDefenseGuid, true).WithBonusValueProgression(0, true))
                .Configure();

            var ability = AbilityConfigurator.New(UnitedDefenseAbility, UnitedDefenseAbilityGuid)
                .SetDisplayName(UnitedDefenseDisplayName)
                .SetDescription(UnitedDefenseDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .AllowTargeting(false, false, true, false)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .ApplyBuff(buff, ContextDuration.Fixed(1))
                    .ApplyBuff(buff2, ContextDuration.Fixed(1), toCaster: true)
                    .Build())
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Custom)
                .SetCustomRange(5)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(UnitedDefenseName, UnitedDefenseGuid)
                    .SetDisplayName(UnitedDefenseDisplayName)
                    .SetDescription(UnitedDefenseDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .SetRanks(10)
                    .Configure();
        }

        private const string AlliedRetribution = "GoldenLegionnaireAlliedRetribution";
        private static readonly string AlliedRetributionGuid = "{93F9ECCA-0D43-4688-9B3C-8BDBB1A067EE}";

        internal const string AlliedRetributionDisplayName = "GoldenLegionnaireAlliedRetribution.Name";
        private const string AlliedRetributionDescription = "GoldenLegionnaireAlliedRetribution.Description";

        private const string AlliedRetributionBuff = "AlliedRetribution.AlliedRetributionBuff";
        public static readonly string AlliedRetributionBuffGuid = "{CBCCE289-469B-4F75-942C-C99D628DA8E1}";
        public static BlueprintFeature AlliedRetributionFeature()
        {
            var icon = FeatureRefs.CavalierRetribution.Reference.Get().Icon;

            var buff = BuffConfigurator.New(AlliedRetributionBuff, AlliedRetributionBuffGuid)
                .SetDisplayName(AlliedRetributionDisplayName)
                .SetDescription(AlliedRetributionDescription)
                .SetIcon(icon)
                //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            return FeatureConfigurator.New(AlliedRetribution, AlliedRetributionGuid)
              .SetDisplayName(AlliedRetributionDisplayName)
              .SetDescription(AlliedRetributionDescription)
              .SetIcon(icon)
              .AddComponent<AlliedRetributionTrigger>()
              .AddAttackBonusAgainstFactOwner(1, 0, buff, ModifierDescriptor.Morale)
              .AddDamageBonusAgainstFactOwner(0, buff, 1, ModifierDescriptor.Morale)
              .AddContextRankConfig(ContextRankConfigs.FeatureRank(AlliedRetributionGuid))
              .SetRanks(10)
              .Configure();
        }
    }
}
