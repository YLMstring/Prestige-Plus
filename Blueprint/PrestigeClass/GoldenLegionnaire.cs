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
                .AddToLevelEntry(1, DefyDangerFeature(), AuthoritativeCommandConfigure())
                .AddToLevelEntry(2, SeizetheOpportunity.FeatGuid, UnitedDefenseConfigure(), AuthoritativeCommand2Feature())
                .AddToLevelEntry(3, AlliedRetributionFeature(), AuthoritativeCommand3Feature())
                .AddToLevelEntry(4, BodyGuard.FeatGuid, AuthoritativeCommand4Feature())
                .AddToLevelEntry(5, DefyDangerGuid)
                .AddToLevelEntry(6, BodyGuard.Feat2Guid, UnitedDefenseGuid, AuthoritativeCommand5Feature())
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

        private const string AuthoritativeCommand2 = "GoldenLegionnaireAuthoritativeCommand2";
        public static readonly string AuthoritativeCommand2Guid = "{09666D53-7194-4B06-8CFB-303552B901C1}";

        internal const string AuthoritativeCommand2DisplayName = "GoldenLegionnaireAuthoritativeCommand2.Name";
        private const string AuthoritativeCommand2Description = "GoldenLegionnaireAuthoritativeCommand2.Description";
        public static BlueprintFeature AuthoritativeCommand2Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand2, AuthoritativeCommand2Guid)
              .SetDisplayName(AuthoritativeCommand2DisplayName)
              .SetDescription(AuthoritativeCommand2Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand3 = "GoldenLegionnaireAuthoritativeCommand3";
        public static readonly string AuthoritativeCommand3Guid = "{EFC5F4EC-81E4-4DBD-A341-79414EB71BFC}";

        internal const string AuthoritativeCommand3DisplayName = "GoldenLegionnaireAuthoritativeCommand3.Name";
        private const string AuthoritativeCommand3Description = "GoldenLegionnaireAuthoritativeCommand3.Description";
        public static BlueprintFeature AuthoritativeCommand3Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand3, AuthoritativeCommand3Guid)
              .SetDisplayName(AuthoritativeCommand3DisplayName)
              .SetDescription(AuthoritativeCommand3Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand5 = "GoldenLegionnaireAuthoritativeCommand5";
        public static readonly string AuthoritativeCommand5Guid = "{2E134743-07A7-405E-B3C1-B6243E6B9F57}";

        internal const string AuthoritativeCommand5DisplayName = "GoldenLegionnaireAuthoritativeCommand5.Name";
        private const string AuthoritativeCommand5Description = "GoldenLegionnaireAuthoritativeCommand5.Description";
        public static BlueprintFeature AuthoritativeCommand5Feature()
        {
            var icon = AbilityRefs.CommandGreater.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand5, AuthoritativeCommand5Guid)
              .SetDisplayName(AuthoritativeCommand5DisplayName)
              .SetDescription(AuthoritativeCommand5Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand4 = "GoldenLegionnaireAuthoritativeCommand4";
        public static readonly string AuthoritativeCommand4Guid = "{3429D9BD-E68D-4096-B2BE-3EB121825B2E}";

        internal const string AuthoritativeCommand4DisplayName = "GoldenLegionnaireAuthoritativeCommand4.Name";
        private const string AuthoritativeCommand4Description = "GoldenLegionnaireAuthoritativeCommand4.Description";
        public static BlueprintFeature AuthoritativeCommand4Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand4, AuthoritativeCommand4Guid)
              .SetDisplayName(AuthoritativeCommand4DisplayName)
              .SetDescription(AuthoritativeCommand4Description)
              .SetIcon(icon)
              .Configure();
        }

        private static readonly string AuthoritativeCommandName = "GoldenLegionnaireAuthoritativeCommand";
        public static readonly string AuthoritativeCommandGuid = "{E541F28C-42F6-4C5A-854B-F585318076A3}";

        private static readonly string AuthoritativeCommandDisplayName = "GoldenLegionnaireAuthoritativeCommand.Name";
        private static readonly string AuthoritativeCommandDescription = "GoldenLegionnaireAuthoritativeCommand.Description";

        private const string AuthoritativeCommandAbility = "AuthoritativeCommand.AuthoritativeCommandAbility";
        private static readonly string AuthoritativeCommandAbilityGuid = "{399B14AE-70FE-47DB-9206-B08056267AD4}";

        private const string AuthoritativeCommandBuff = "AuthoritativeCommand.AuthoritativeCommandBuff";
        private static readonly string AuthoritativeCommandBuffGuid = "{F5F99B01-0023-4A6B-A7B3-4F994DA903E2}";
        public static BlueprintFeature AuthoritativeCommandConfigure()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;

            var buff = BuffConfigurator.New(AuthoritativeCommandBuff, AuthoritativeCommandBuffGuid)
                .SetDisplayName(AuthoritativeCommandDisplayName)
                .SetDescription(AuthoritativeCommandDescription)
                .SetIcon(icon)
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.WindsOfVengenance)
                .Configure();

            var ability = AbilityConfigurator.New(AuthoritativeCommandAbility, AuthoritativeCommandAbilityGuid)
                .SetDisplayName(AuthoritativeCommandDisplayName)
                .SetDescription(AuthoritativeCommandDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .SetType(AbilityType.Supernatural)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .Configure();

            return FeatureConfigurator.New(AuthoritativeCommandName, AuthoritativeCommandGuid)
                    .SetDisplayName(AuthoritativeCommandDisplayName)
                    .SetDescription(AuthoritativeCommandDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
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

        private const string PreemptiveStrike = "GoldenLegionnairePreemptiveStrike";
        public static readonly string PreemptiveStrikeGuid = "{27C488F2-7613-4210-BFDF-56A4A49133B9}";

        internal const string PreemptiveStrikeDisplayName = "GoldenLegionnairePreemptiveStrike.Name";
        private const string PreemptiveStrikeDescription = "GoldenLegionnairePreemptiveStrike.Description";

        private const string PreemptiveStrikeBuff = "PreemptiveStrike.PreemptiveStrikeBuff";
        public static readonly string PreemptiveStrikeBuffGuid = "{830870C8-010D-4F44-9511-F1FF08B846D6}";

        private const string PreemptiveStrikeBuff2 = "PreemptiveStrike.PreemptiveStrikeBuff2";
        public static readonly string PreemptiveStrikeBuff2Guid = "{3F044DF1-C6DB-4392-97FF-9CE66B29C7E1}";
        public static BlueprintFeature PreemptiveStrikeFeature()
        {
            var icon = FeatureRefs.Opportunist.Reference.Get().Icon;

            BuffConfigurator.New(PreemptiveStrikeBuff, PreemptiveStrikeBuffGuid)
                .SetDisplayName(PreemptiveStrikeDisplayName)
                .SetDescription(PreemptiveStrikeDescription)
                .SetIcon(icon)
                //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            BuffConfigurator.New(PreemptiveStrikeBuff2, PreemptiveStrikeBuff2Guid)
                .SetDisplayName(PreemptiveStrikeDisplayName)
                .SetDescription(PreemptiveStrikeDescription)
                .SetIcon(icon)
                //.AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            return FeatureConfigurator.New(PreemptiveStrike, PreemptiveStrikeGuid)
              .SetDisplayName(PreemptiveStrikeDisplayName)
              .SetDescription(PreemptiveStrikeDescription)
              .SetIcon(icon)
              .Configure();
        }
    }
}
