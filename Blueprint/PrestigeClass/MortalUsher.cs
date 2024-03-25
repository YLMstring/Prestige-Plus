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
            //"SwiftAidAnotherFeature": "8590fb52-921c-4365-832c-ca7635fd5a70",
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, DefyDangerFeature(), AuthoritativeCommandConfigure(), FeatureRefs.FighterProficiencies.ToString())
                .AddToLevelEntry(2, SeizetheOpportunity.FeatGuid, UnitedDefenseConfigure(), AuthoritativeCommand2Feature())
                .AddToLevelEntry(3, AlliedRetributionFeature(), AuthoritativeCommand3Feature())
                .AddToLevelEntry(4, BodyGuard.FeatGuid, AuthoritativeCommand4Feature())
                .AddToLevelEntry(5, DefyDangerGuid)
                .AddToLevelEntry(6, BodyGuard.Feat2Guid, UnitedDefenseGuid, AuthoritativeCommand5Feature())
                .AddToLevelEntry(7, AlliedRetributionGuid, PreemptiveStrikeFeature())
                .AddToLevelEntry(8, AuthoritativeCommandSwiftConfigure())
                .AddToLevelEntry(9, RetaliateFeature(), DefyDangerGuid)
                .AddToLevelEntry(10, FeatureRefs.PerfectStrikeFeature.ToString(), UnitedDefenseGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { AuthoritativeCommandGuid, AuthoritativeCommandSwiftGuid, AuthoritativeCommand2Guid, AuthoritativeCommand3Guid, AuthoritativeCommand4Guid, AuthoritativeCommand5Guid })
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeizetheOpportunity.FeatGuid, BodyGuard.FeatGuid, BodyGuard.Feat2Guid, "8590fb52-921c-4365-832c-ca7635fd5a70", FeatureRefs.PerfectStrikeFeature.ToString() }))
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

        private const string DefyDanger = "MortalUsherDefyDanger";
        private static readonly string DefyDangerGuid = "{E891A2FF-14E6-4A00-9F0E-05EE87508A2F}";

        internal const string DefyDangerDisplayName = "MortalUsherDefyDanger.Name";
        private const string DefyDangerDescription = "MortalUsherDefyDanger.Description";
        public static BlueprintFeature DefyDangerFeature()
        {
            var icon = FeatureRefs.ArchaelogistDangerSense.Reference.Get().Icon;
            return FeatureConfigurator.New(DefyDanger, DefyDangerGuid)
              .SetDisplayName(DefyDangerDisplayName)
              .SetDescription(DefyDangerDescription)
              .SetIcon(icon)
              .AddSavingThrowBonusAgainstFact(value: 2, checkedFact: FeatureRefs.DemonOfMagicFeature.ToString())
              .AddSavingThrowBonusAgainstFact(value: 2, checkedFact: FeatureRefs.DemonOfSlaughterFeature.ToString())
              .AddSavingThrowBonusAgainstFact(value: 2, checkedFact: FeatureRefs.DemonOfStrengthFeature.ToString())
              .SetRanks(10)
              .Configure();
        }

        private const string Retaliate = "MortalUsherRetaliate";
        public static readonly string RetaliateGuid = "{798F5C7F-3FEA-4AD8-ADAA-E57FA93D20F8}";

        internal const string RetaliateDisplayName = "MortalUsherRetaliate.Name";
        private const string RetaliateDescription = "MortalUsherRetaliate.Description";
        public static BlueprintFeature RetaliateFeature()
        {
            var icon = FeatureRefs.DevotedBladeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(Retaliate, RetaliateGuid)
              .SetDisplayName(RetaliateDisplayName)
              .SetDescription(RetaliateDescription)
              .SetIcon(icon)
              
              .Configure();
        }

        private const string AuthoritativeCommand2 = "MortalUsherAuthoritativeCommand2";
        public static readonly string AuthoritativeCommand2Guid = "{09666D53-7194-4B06-8CFB-303552B901C1}";

        internal const string AuthoritativeCommand2DisplayName = "MortalUsherAuthoritativeCommand2.Name";
        private const string AuthoritativeCommand2Description = "MortalUsherAuthoritativeCommand2.Description";
        public static BlueprintFeature AuthoritativeCommand2Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand2, AuthoritativeCommand2Guid)
              .SetDisplayName(AuthoritativeCommand2DisplayName)
              .SetDescription(AuthoritativeCommand2Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand3 = "MortalUsherAuthoritativeCommand3";
        public static readonly string AuthoritativeCommand3Guid = "{EFC5F4EC-81E4-4DBD-A341-79414EB71BFC}";

        internal const string AuthoritativeCommand3DisplayName = "MortalUsherAuthoritativeCommand3.Name";
        private const string AuthoritativeCommand3Description = "MortalUsherAuthoritativeCommand3.Description";
        public static BlueprintFeature AuthoritativeCommand3Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand3, AuthoritativeCommand3Guid)
              .SetDisplayName(AuthoritativeCommand3DisplayName)
              .SetDescription(AuthoritativeCommand3Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand5 = "MortalUsherAuthoritativeCommand5";
        public static readonly string AuthoritativeCommand5Guid = "{2E134743-07A7-405E-B3C1-B6243E6B9F57}";

        internal const string AuthoritativeCommand5DisplayName = "MortalUsherAuthoritativeCommand5.Name";
        private const string AuthoritativeCommand5Description = "MortalUsherAuthoritativeCommand5.Description";
        public static BlueprintFeature AuthoritativeCommand5Feature()
        {
            var icon = AbilityRefs.CommandGreater.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand5, AuthoritativeCommand5Guid)
              .SetDisplayName(AuthoritativeCommand5DisplayName)
              .SetDescription(AuthoritativeCommand5Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand4 = "MortalUsherAuthoritativeCommand4";
        public static readonly string AuthoritativeCommand4Guid = "{3429D9BD-E68D-4096-B2BE-3EB121825B2E}";

        internal const string AuthoritativeCommand4DisplayName = "MortalUsherAuthoritativeCommand4.Name";
        private const string AuthoritativeCommand4Description = "MortalUsherAuthoritativeCommand4.Description";
        public static BlueprintFeature AuthoritativeCommand4Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand4, AuthoritativeCommand4Guid)
              .SetDisplayName(AuthoritativeCommand4DisplayName)
              .SetDescription(AuthoritativeCommand4Description)
              .SetIcon(icon)
              .Configure();
        }

        private static readonly string AuthoritativeCommandName = "MortalUsherAuthoritativeCommand";
        public static readonly string AuthoritativeCommandGuid = "{E541F28C-42F6-4C5A-854B-F585318076A3}";

        private static readonly string AuthoritativeCommandDisplayName = "MortalUsherAuthoritativeCommand.Name";
        private static readonly string AuthoritativeCommandDescription = "MortalUsherAuthoritativeCommand.Description";

        private const string AuthoritativeCommandAbility = "AuthoritativeCommand.AuthoritativeCommandAbility";
        private static readonly string AuthoritativeCommandAbilityGuid = "{399B14AE-70FE-47DB-9206-B08056267AD4}";

        private const string AuthoritativeCommandBuff = "AuthoritativeCommand.AuthoritativeCommandBuff";
        private static readonly string AuthoritativeCommandBuffGuid = "{F5F99B01-0023-4A6B-A7B3-4F994DA903E2}";

        private const string CommandMoveAutoAbility = "MortalUsher.UseCommandMoveAuto";
        public static readonly string CommandMoveAutoAbilityGuid = "{2B9C109C-F0B6-4001-A6D0-8C4893615AE5}";

        private const string CommandMoveAutoBuff2 = "MortalUsher.CommandMoveAutoBuff2";
        public static readonly string CommandMoveAutoBuff2Guid = "{B53B1E0E-0999-4F5E-8B8A-FF808C088410}";
        public static BlueprintFeature AuthoritativeCommandConfigure()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            var fx = AbilityRefs.OverwhelmingPresence.Reference.Get().GetComponent<AbilitySpawnFx>();

            var buff = BuffConfigurator.New(AuthoritativeCommandBuff, AuthoritativeCommandBuffGuid)
                .SetDisplayName(AuthoritativeCommandDisplayName)
                .SetDescription(AuthoritativeCommandDescription)
                .SetIcon(icon)
                .AddComponent<AuthoritativeCommanComp>()
                .Configure();

            var ability = AbilityConfigurator.New(AuthoritativeCommandAbility, AuthoritativeCommandAbilityGuid)
                .SetDisplayName(AuthoritativeCommandDisplayName)
                .SetDescription(AuthoritativeCommandDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff, ContextDuration.Fixed(1)).Build())
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Immediate)
                .Configure();

            var Buff2 = BuffConfigurator.New(CommandMoveAutoBuff2, CommandMoveAutoBuff2Guid)
             .SetDisplayName(AuthoritativeCommandDisplayName)
             .SetDescription(AuthoritativeCommandDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New()
                .Add<SpendMoveAction>(c => {
                    c.OnHit = ActionsBuilder.New()
                        .CastSpell(ability)
                        .Build();
                })
                .Build())
             .Configure();

            var ability2 = ActivatableAbilityConfigurator.New(CommandMoveAutoAbility, CommandMoveAutoAbilityGuid)
                .SetDisplayName(AuthoritativeCommandDisplayName)
                .SetDescription(AuthoritativeCommandDescription)
                .SetIcon(icon)
                .SetBuff(Buff2)
                .SetDeactivateIfOwnerDisabled(true)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(AuthoritativeCommandName, AuthoritativeCommandGuid)
                    .SetDisplayName(AuthoritativeCommandDisplayName)
                    .SetDescription(AuthoritativeCommandDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();
        }

        private static readonly string AuthoritativeCommandSwiftName = "MortalUsherAuthoritativeCommandSwift";
        public static readonly string AuthoritativeCommandSwiftGuid = "{BA301623-4AF8-4898-A8DB-DA2F79F6573C}";

        private static readonly string AuthoritativeCommandSwiftDisplayName = "MortalUsherAuthoritativeCommandSwift.Name";
        private static readonly string AuthoritativeCommandSwiftDescription = "MortalUsherAuthoritativeCommandSwift.Description";

        private const string AuthoritativeCommandSwiftAbility = "AuthoritativeCommandSwift.AuthoritativeCommandSwiftAbility";
        private static readonly string AuthoritativeCommandSwiftAbilityGuid = "{A5697DB1-D8C7-438E-8E90-EE762CFF1176}";

        private const string CommandSwiftMoveAutoAbility = "MortalUsher.UseCommandSwiftMoveAuto";
        public static readonly string CommandSwiftMoveAutoAbilityGuid = "{7E7AA762-2638-40C2-ACC8-C05CD1F55FAE}";

        private const string CommandSwiftMoveAutoBuff2 = "MortalUsher.CommandSwiftMoveAutoBuff2";
        public static readonly string CommandSwiftMoveAutoBuff2Guid = "{1E426405-7AC0-4F53-A2DE-394B509D4609}";
        public static BlueprintFeature AuthoritativeCommandSwiftConfigure()
        {
            var icon = AbilityRefs.CommandGreater.Reference.Get().Icon;
            var fx = AbilityRefs.BlessingOfCourageAndLife.Reference.Get().GetComponent<AbilitySpawnFx>();

            var ability = AbilityConfigurator.New(AuthoritativeCommandSwiftAbility, AuthoritativeCommandSwiftAbilityGuid)
                .SetDisplayName(AuthoritativeCommandSwiftDisplayName)
                .SetDescription(AuthoritativeCommandSwiftDescription)
                .SetIcon(icon)
                .AddComponent(fx)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(AuthoritativeCommandBuffGuid, ContextDuration.Fixed(1)).Build())
                .AddAbilityTargetsAround(includeDead: false, targetType: TargetType.Ally, radius: 30.Feet(), spreadSpeed: 40.Feet())
                .SetType(AbilityType.Extraordinary)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            var Buff2 = BuffConfigurator.New(CommandSwiftMoveAutoBuff2, CommandSwiftMoveAutoBuff2Guid)
             .SetDisplayName(AuthoritativeCommandSwiftDisplayName)
             .SetDescription(AuthoritativeCommandSwiftDescription)
             .SetIcon(icon)
             .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
             .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New()
                .Add<SpendSwiftAction>(c => {
                    c.OnHit = ActionsBuilder.New()
                        .CastSpell(ability)
                        .Build();
                })
                .Build())
             .Configure();

            var ability2 = ActivatableAbilityConfigurator.New(CommandSwiftMoveAutoAbility, CommandSwiftMoveAutoAbilityGuid)
                .SetDisplayName(AuthoritativeCommandSwiftDisplayName)
                .SetDescription(AuthoritativeCommandSwiftDescription)
                .SetIcon(icon)
                .SetBuff(Buff2)
                .SetDeactivateIfOwnerDisabled(true)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(AuthoritativeCommandSwiftName, AuthoritativeCommandSwiftGuid)
                    .SetDisplayName(AuthoritativeCommandSwiftDisplayName)
                    .SetDescription(AuthoritativeCommandSwiftDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();
        }

        private static readonly string UnitedDefenseName = "MortalUsherUnitedDefense";
        public static readonly string UnitedDefenseGuid = "{7C46838D-D2D2-4DDB-931C-25E21B33E6C4}";

        private static readonly string UnitedDefenseDisplayName = "MortalUsherUnitedDefense.Name";
        private static readonly string UnitedDefenseDescription = "MortalUsherUnitedDefense.Description";

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
                .AddComponent<UnitedDefensePenalty>()
                .AddToFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi)
                .Configure();

            var buff = BuffConfigurator.New(UnitedDefenseBuff, UnitedDefenseBuffGuid)
                .SetDisplayName(UnitedDefenseDisplayName)
                .SetDescription(UnitedDefenseDescription)
                .SetIcon(icon)
                .AddContextStatBonus(StatType.AC, ContextValues.Rank(), descriptor: ModifierDescriptor.Dodge)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel(new string[] { ArchetypeGuid }).WithCustomProgression((5, 2), (9, 4), (10, 6)))
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

        private const string AlliedRetribution = "MortalUsherAlliedRetribution";
        private static readonly string AlliedRetributionGuid = "{93F9ECCA-0D43-4688-9B3C-8BDBB1A067EE}";

        internal const string AlliedRetributionDisplayName = "MortalUsherAlliedRetribution.Name";
        private const string AlliedRetributionDescription = "MortalUsherAlliedRetribution.Description";

        private const string AlliedRetributionBuff = "AlliedRetribution.AlliedRetributionBuff";
        public static readonly string AlliedRetributionBuffGuid = "{CBCCE289-469B-4F75-942C-C99D628DA8E1}";
        public static BlueprintFeature AlliedRetributionFeature()
        {
            var icon = FeatureRefs.PreciseStrike.Reference.Get().Icon;

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

        private const string PreemptiveStrike = "MortalUsherPreemptiveStrike";
        public static readonly string PreemptiveStrikeGuid = "{27C488F2-7613-4210-BFDF-56A4A49133B9}";

        internal const string PreemptiveStrikeDisplayName = "MortalUsherPreemptiveStrike.Name";
        private const string PreemptiveStrikeDescription = "MortalUsherPreemptiveStrike.Description";

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

        private const string AlignSpam = "MortalUsher.AlignSpam";
        public static readonly string AlignSpamGuid = "{69DFC912-DFE1-46A7-A6AC-4E701DCF0A27}";

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
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalAlchemistClass3Align, MortalAlchemistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAlchemistClass4Align, MortalAlchemistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalAlchemistClass5Align, MortalAlchemistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAlchemistClass6Align, MortalAlchemistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalAlchemistClass7Align, MortalAlchemistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAlchemistClass8Align, MortalAlchemistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAlchemistClasspro = MortalAlchemistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalAlchemistClass9Align, MortalAlchemistClass9AlignGuid)
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
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalArcaneTricksterClass3Align, MortalArcaneTricksterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcaneTricksterClass4Align, MortalArcaneTricksterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalArcaneTricksterClass5Align, MortalArcaneTricksterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcaneTricksterClass6Align, MortalArcaneTricksterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalArcaneTricksterClass7Align, MortalArcaneTricksterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcaneTricksterClass8Align, MortalArcaneTricksterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcaneTricksterClasspro = MortalArcaneTricksterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalArcaneTricksterClass9Align, MortalArcaneTricksterClass9AlignGuid)
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
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalArcanistClass3Align, MortalArcanistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcanistClass4Align, MortalArcanistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalArcanistClass5Align, MortalArcanistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcanistClass6Align, MortalArcanistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalArcanistClass7Align, MortalArcanistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcanistClass8Align, MortalArcanistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalArcanistClasspro = MortalArcanistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalArcanistClass9Align, MortalArcanistClass9AlignGuid)
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
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalAssassinClass3Align, MortalAssassinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAssassinClass4Align, MortalAssassinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalAssassinClass5Align, MortalAssassinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAssassinClass6Align, MortalAssassinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalAssassinClass7Align, MortalAssassinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAssassinClass8Align, MortalAssassinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalAssassinClasspro = MortalAssassinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalAssassinClass9Align, MortalAssassinClass9AlignGuid)
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
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalBarbarianClass3Align, MortalBarbarianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBarbarianClass4Align, MortalBarbarianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalBarbarianClass5Align, MortalBarbarianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBarbarianClass6Align, MortalBarbarianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalBarbarianClass7Align, MortalBarbarianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBarbarianClass8Align, MortalBarbarianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBarbarianClasspro = MortalBarbarianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalBarbarianClass9Align, MortalBarbarianClass9AlignGuid)
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
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalBardClass3Align, MortalBardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBardClass4Align, MortalBardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalBardClass5Align, MortalBardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBardClass6Align, MortalBardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalBardClass7Align, MortalBardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBardClass8Align, MortalBardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBardClasspro = MortalBardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalBardClass9Align, MortalBardClass9AlignGuid)
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
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalBloodragerClass3Align, MortalBloodragerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBloodragerClass4Align, MortalBloodragerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalBloodragerClass5Align, MortalBloodragerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBloodragerClass6Align, MortalBloodragerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalBloodragerClass7Align, MortalBloodragerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBloodragerClass8Align, MortalBloodragerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalBloodragerClasspro = MortalBloodragerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalBloodragerClass9Align, MortalBloodragerClass9AlignGuid)
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
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalCavalierClass3Align, MortalCavalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalCavalierClass4Align, MortalCavalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalCavalierClass5Align, MortalCavalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalCavalierClass6Align, MortalCavalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalCavalierClass7Align, MortalCavalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalCavalierClass8Align, MortalCavalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalCavalierClasspro = MortalCavalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalCavalierClass9Align, MortalCavalierClass9AlignGuid)
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
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalClericClass3Align, MortalClericClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalClericClass4Align, MortalClericClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalClericClass5Align, MortalClericClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalClericClass6Align, MortalClericClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalClericClass7Align, MortalClericClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalClericClass8Align, MortalClericClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalClericClasspro = MortalClericClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalClericClass9Align, MortalClericClass9AlignGuid)
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
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDragonDiscipleClass3Align, MortalDragonDiscipleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDragonDiscipleClass4Align, MortalDragonDiscipleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDragonDiscipleClass5Align, MortalDragonDiscipleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDragonDiscipleClass6Align, MortalDragonDiscipleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDragonDiscipleClass7Align, MortalDragonDiscipleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDragonDiscipleClass8Align, MortalDragonDiscipleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDragonDiscipleClasspro = MortalDragonDiscipleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDragonDiscipleClass9Align, MortalDragonDiscipleClass9AlignGuid)
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
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDruidClass3Align, MortalDruidClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDruidClass4Align, MortalDruidClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDruidClass5Align, MortalDruidClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDruidClass6Align, MortalDruidClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDruidClass7Align, MortalDruidClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDruidClass8Align, MortalDruidClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDruidClasspro = MortalDruidClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDruidClass9Align, MortalDruidClass9AlignGuid)
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
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDuelistClass3Align, MortalDuelistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDuelistClass4Align, MortalDuelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDuelistClass5Align, MortalDuelistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDuelistClass6Align, MortalDuelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDuelistClass7Align, MortalDuelistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDuelistClass8Align, MortalDuelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalDuelistClasspro = MortalDuelistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDuelistClass9Align, MortalDuelistClass9AlignGuid)
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
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalEldritchKnightClass3Align, MortalEldritchKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEldritchKnightClass4Align, MortalEldritchKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalEldritchKnightClass5Align, MortalEldritchKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEldritchKnightClass6Align, MortalEldritchKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalEldritchKnightClass7Align, MortalEldritchKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEldritchKnightClass8Align, MortalEldritchKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchKnightClasspro = MortalEldritchKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalEldritchKnightClass9Align, MortalEldritchKnightClass9AlignGuid)
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
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalEldritchScionClass3Align, MortalEldritchScionClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEldritchScionClass4Align, MortalEldritchScionClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalEldritchScionClass5Align, MortalEldritchScionClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEldritchScionClass6Align, MortalEldritchScionClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalEldritchScionClass7Align, MortalEldritchScionClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEldritchScionClass8Align, MortalEldritchScionClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalEldritchScionClasspro = MortalEldritchScionClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalEldritchScionClass9Align, MortalEldritchScionClass9AlignGuid)
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
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalFighterClass3Align, MortalFighterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalFighterClass4Align, MortalFighterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalFighterClass5Align, MortalFighterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalFighterClass6Align, MortalFighterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalFighterClass7Align, MortalFighterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalFighterClass8Align, MortalFighterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalFighterClasspro = MortalFighterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalFighterClass9Align, MortalFighterClass9AlignGuid)
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
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHellknightClass3Align, MortalHellknightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHellknightClass4Align, MortalHellknightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHellknightClass5Align, MortalHellknightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHellknightClass6Align, MortalHellknightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHellknightClass7Align, MortalHellknightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHellknightClass8Align, MortalHellknightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightClasspro = MortalHellknightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHellknightClass9Align, MortalHellknightClass9AlignGuid)
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
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHellknightSigniferClass3Align, MortalHellknightSigniferClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHellknightSigniferClass4Align, MortalHellknightSigniferClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHellknightSigniferClass5Align, MortalHellknightSigniferClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHellknightSigniferClass6Align, MortalHellknightSigniferClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHellknightSigniferClass7Align, MortalHellknightSigniferClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHellknightSigniferClass8Align, MortalHellknightSigniferClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHellknightSigniferClasspro = MortalHellknightSigniferClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHellknightSigniferClass9Align, MortalHellknightSigniferClass9AlignGuid)
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
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHunterClass3Align, MortalHunterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHunterClass4Align, MortalHunterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHunterClass5Align, MortalHunterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHunterClass6Align, MortalHunterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHunterClass7Align, MortalHunterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHunterClass8Align, MortalHunterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalHunterClasspro = MortalHunterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHunterClass9Align, MortalHunterClass9AlignGuid)
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
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalInquisitorClass3Align, MortalInquisitorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalInquisitorClass4Align, MortalInquisitorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalInquisitorClass5Align, MortalInquisitorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalInquisitorClass6Align, MortalInquisitorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalInquisitorClass7Align, MortalInquisitorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalInquisitorClass8Align, MortalInquisitorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalInquisitorClasspro = MortalInquisitorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalInquisitorClass9Align, MortalInquisitorClass9AlignGuid)
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
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalKineticistClass3Align, MortalKineticistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalKineticistClass4Align, MortalKineticistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalKineticistClass5Align, MortalKineticistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalKineticistClass6Align, MortalKineticistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalKineticistClass7Align, MortalKineticistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalKineticistClass8Align, MortalKineticistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalKineticistClasspro = MortalKineticistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalKineticistClass9Align, MortalKineticistClass9AlignGuid)
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
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalLoremasterClass3Align, MortalLoremasterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalLoremasterClass4Align, MortalLoremasterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalLoremasterClass5Align, MortalLoremasterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalLoremasterClass6Align, MortalLoremasterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalLoremasterClass7Align, MortalLoremasterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalLoremasterClass8Align, MortalLoremasterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalLoremasterClasspro = MortalLoremasterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalLoremasterClass9Align, MortalLoremasterClass9AlignGuid)
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
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalMagusClass3Align, MortalMagusClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMagusClass4Align, MortalMagusClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalMagusClass5Align, MortalMagusClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMagusClass6Align, MortalMagusClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalMagusClass7Align, MortalMagusClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMagusClass8Align, MortalMagusClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMagusClasspro = MortalMagusClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalMagusClass9Align, MortalMagusClass9AlignGuid)
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
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalMonkClass3Align, MortalMonkClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMonkClass4Align, MortalMonkClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalMonkClass5Align, MortalMonkClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMonkClass6Align, MortalMonkClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalMonkClass7Align, MortalMonkClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMonkClass8Align, MortalMonkClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMonkClasspro = MortalMonkClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalMonkClass9Align, MortalMonkClass9AlignGuid)
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
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalMysticTheurgeClass3Align, MortalMysticTheurgeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMysticTheurgeClass4Align, MortalMysticTheurgeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalMysticTheurgeClass5Align, MortalMysticTheurgeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMysticTheurgeClass6Align, MortalMysticTheurgeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalMysticTheurgeClass7Align, MortalMysticTheurgeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMysticTheurgeClass8Align, MortalMysticTheurgeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalMysticTheurgeClasspro = MortalMysticTheurgeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalMysticTheurgeClass9Align, MortalMysticTheurgeClass9AlignGuid)
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
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalOracleClass3Align, MortalOracleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalOracleClass4Align, MortalOracleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalOracleClass5Align, MortalOracleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalOracleClass6Align, MortalOracleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalOracleClass7Align, MortalOracleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalOracleClass8Align, MortalOracleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalOracleClasspro = MortalOracleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalOracleClass9Align, MortalOracleClass9AlignGuid)
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
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalPaladinClass3Align, MortalPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalPaladinClass4Align, MortalPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalPaladinClass5Align, MortalPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalPaladinClass6Align, MortalPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalPaladinClass7Align, MortalPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalPaladinClass8Align, MortalPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalPaladinClasspro = MortalPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalPaladinClass9Align, MortalPaladinClass9AlignGuid)
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
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalRangerClass3Align, MortalRangerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalRangerClass4Align, MortalRangerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalRangerClass5Align, MortalRangerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalRangerClass6Align, MortalRangerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalRangerClass7Align, MortalRangerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalRangerClass8Align, MortalRangerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRangerClasspro = MortalRangerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalRangerClass9Align, MortalRangerClass9AlignGuid)
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
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalRogueClass3Align, MortalRogueClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalRogueClass4Align, MortalRogueClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalRogueClass5Align, MortalRogueClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalRogueClass6Align, MortalRogueClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalRogueClass7Align, MortalRogueClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalRogueClass8Align, MortalRogueClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalRogueClasspro = MortalRogueClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalRogueClass9Align, MortalRogueClass9AlignGuid)
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
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalShamanClass3Align, MortalShamanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShamanClass4Align, MortalShamanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalShamanClass5Align, MortalShamanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShamanClass6Align, MortalShamanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalShamanClass7Align, MortalShamanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShamanClass8Align, MortalShamanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShamanClasspro = MortalShamanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalShamanClass9Align, MortalShamanClass9AlignGuid)
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
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalShifterClass3Align, MortalShifterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShifterClass4Align, MortalShifterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalShifterClass5Align, MortalShifterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShifterClass6Align, MortalShifterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalShifterClass7Align, MortalShifterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShifterClass8Align, MortalShifterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalShifterClasspro = MortalShifterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalShifterClass9Align, MortalShifterClass9AlignGuid)
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
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSkaldClass3Align, MortalSkaldClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSkaldClass4Align, MortalSkaldClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSkaldClass5Align, MortalSkaldClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSkaldClass6Align, MortalSkaldClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSkaldClass7Align, MortalSkaldClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSkaldClass8Align, MortalSkaldClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSkaldClasspro = MortalSkaldClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSkaldClass9Align, MortalSkaldClass9AlignGuid)
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
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSlayerClass3Align, MortalSlayerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSlayerClass4Align, MortalSlayerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSlayerClass5Align, MortalSlayerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSlayerClass6Align, MortalSlayerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSlayerClass7Align, MortalSlayerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSlayerClass8Align, MortalSlayerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSlayerClasspro = MortalSlayerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSlayerClass9Align, MortalSlayerClass9AlignGuid)
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
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSorcererClass3Align, MortalSorcererClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSorcererClass4Align, MortalSorcererClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSorcererClass5Align, MortalSorcererClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSorcererClass6Align, MortalSorcererClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSorcererClass7Align, MortalSorcererClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSorcererClass8Align, MortalSorcererClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSorcererClasspro = MortalSorcererClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSorcererClass9Align, MortalSorcererClass9AlignGuid)
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
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalStalwartDefenderClass3Align, MortalStalwartDefenderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStalwartDefenderClass4Align, MortalStalwartDefenderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalStalwartDefenderClass5Align, MortalStalwartDefenderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStalwartDefenderClass6Align, MortalStalwartDefenderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalStalwartDefenderClass7Align, MortalStalwartDefenderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStalwartDefenderClass8Align, MortalStalwartDefenderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStalwartDefenderClasspro = MortalStalwartDefenderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalStalwartDefenderClass9Align, MortalStalwartDefenderClass9AlignGuid)
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
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalStudentOfWarClass3Align, MortalStudentOfWarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStudentOfWarClass4Align, MortalStudentOfWarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalStudentOfWarClass5Align, MortalStudentOfWarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStudentOfWarClass6Align, MortalStudentOfWarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalStudentOfWarClass7Align, MortalStudentOfWarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStudentOfWarClass8Align, MortalStudentOfWarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalStudentOfWarClasspro = MortalStudentOfWarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalStudentOfWarClass9Align, MortalStudentOfWarClass9AlignGuid)
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
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSwordlordClass3Align, MortalSwordlordClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSwordlordClass4Align, MortalSwordlordClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSwordlordClass5Align, MortalSwordlordClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSwordlordClass6Align, MortalSwordlordClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSwordlordClass7Align, MortalSwordlordClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSwordlordClass8Align, MortalSwordlordClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalSwordlordClasspro = MortalSwordlordClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSwordlordClass9Align, MortalSwordlordClass9AlignGuid)
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
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalWarpriestClass3Align, MortalWarpriestClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWarpriestClass4Align, MortalWarpriestClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalWarpriestClass5Align, MortalWarpriestClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWarpriestClass6Align, MortalWarpriestClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalWarpriestClass7Align, MortalWarpriestClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWarpriestClass8Align, MortalWarpriestClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWarpriestClasspro = MortalWarpriestClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalWarpriestClass9Align, MortalWarpriestClass9AlignGuid)
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
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalWinterWitchClass3Align, MortalWinterWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWinterWitchClass4Align, MortalWinterWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalWinterWitchClass5Align, MortalWinterWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWinterWitchClass6Align, MortalWinterWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalWinterWitchClass7Align, MortalWinterWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWinterWitchClass8Align, MortalWinterWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWinterWitchClasspro = MortalWinterWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalWinterWitchClass9Align, MortalWinterWitchClass9AlignGuid)
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
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalWitchClass3Align, MortalWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWitchClass4Align, MortalWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalWitchClass5Align, MortalWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWitchClass6Align, MortalWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalWitchClass7Align, MortalWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWitchClass8Align, MortalWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWitchClasspro = MortalWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalWitchClass9Align, MortalWitchClass9AlignGuid)
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
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalWizardClass3Align, MortalWizardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalWizardClass4Align, MortalWizardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalWizardClass5Align, MortalWizardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalWizardClass6Align, MortalWizardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalWizardClass7Align, MortalWizardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalWizardClass8Align, MortalWizardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            MortalWizardClasspro = MortalWizardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalWizardClass9Align, MortalWizardClass9AlignGuid)
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
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalGunslingerClass3Align, MortalGunslingerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalGunslingerClass4Align, MortalGunslingerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalGunslingerClass5Align, MortalGunslingerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalGunslingerClass6Align, MortalGunslingerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalGunslingerClass7Align, MortalGunslingerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalGunslingerClass8Align, MortalGunslingerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            MortalGunslingerClasspro = MortalGunslingerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalGunslingerClass9Align, MortalGunslingerClass9AlignGuid)
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
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalAgentoftheGraveClass3Align, MortalAgentoftheGraveClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAgentoftheGraveClass4Align, MortalAgentoftheGraveClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalAgentoftheGraveClass5Align, MortalAgentoftheGraveClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAgentoftheGraveClass6Align, MortalAgentoftheGraveClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalAgentoftheGraveClass7Align, MortalAgentoftheGraveClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAgentoftheGraveClass8Align, MortalAgentoftheGraveClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            MortalAgentoftheGraveClasspro = MortalAgentoftheGraveClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalAgentoftheGraveClass9Align, MortalAgentoftheGraveClass9AlignGuid)
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
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass3Align, MortalAnchoriteofDawnClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass4Align, MortalAnchoriteofDawnClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass5Align, MortalAnchoriteofDawnClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass6Align, MortalAnchoriteofDawnClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass7Align, MortalAnchoriteofDawnClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass8Align, MortalAnchoriteofDawnClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            MortalAnchoriteofDawnClasspro = MortalAnchoriteofDawnClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalAnchoriteofDawnClass9Align, MortalAnchoriteofDawnClass9AlignGuid)
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
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalArcaneAcherClass3Align, MortalArcaneAcherClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalArcaneAcherClass4Align, MortalArcaneAcherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalArcaneAcherClass5Align, MortalArcaneAcherClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalArcaneAcherClass6Align, MortalArcaneAcherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalArcaneAcherClass7Align, MortalArcaneAcherClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalArcaneAcherClass8Align, MortalArcaneAcherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            MortalArcaneAcherClasspro = MortalArcaneAcherClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalArcaneAcherClass9Align, MortalArcaneAcherClass9AlignGuid)
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
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalAsavirClass3Align, MortalAsavirClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalAsavirClass4Align, MortalAsavirClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalAsavirClass5Align, MortalAsavirClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalAsavirClass6Align, MortalAsavirClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalAsavirClass7Align, MortalAsavirClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalAsavirClass8Align, MortalAsavirClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            MortalAsavirClasspro = MortalAsavirClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalAsavirClass9Align, MortalAsavirClass9AlignGuid)
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
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalChevalierClass3Align, MortalChevalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalChevalierClass4Align, MortalChevalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalChevalierClass5Align, MortalChevalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalChevalierClass6Align, MortalChevalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalChevalierClass7Align, MortalChevalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalChevalierClass8Align, MortalChevalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            MortalChevalierClasspro = MortalChevalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalChevalierClass9Align, MortalChevalierClass9AlignGuid)
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
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalCrimsonTemplarClass3Align, MortalCrimsonTemplarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalCrimsonTemplarClass4Align, MortalCrimsonTemplarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalCrimsonTemplarClass5Align, MortalCrimsonTemplarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalCrimsonTemplarClass6Align, MortalCrimsonTemplarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalCrimsonTemplarClass7Align, MortalCrimsonTemplarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalCrimsonTemplarClass8Align, MortalCrimsonTemplarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            MortalCrimsonTemplarClasspro = MortalCrimsonTemplarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalCrimsonTemplarClass9Align, MortalCrimsonTemplarClass9AlignGuid)
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
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass3Align, MortalDeadeyeDevoteeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass4Align, MortalDeadeyeDevoteeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass5Align, MortalDeadeyeDevoteeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass6Align, MortalDeadeyeDevoteeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass7Align, MortalDeadeyeDevoteeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass8Align, MortalDeadeyeDevoteeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            MortalDeadeyeDevoteeClasspro = MortalDeadeyeDevoteeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDeadeyeDevoteeClass9Align, MortalDeadeyeDevoteeClass9AlignGuid)
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
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDragonFuryClass3Align, MortalDragonFuryClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDragonFuryClass4Align, MortalDragonFuryClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDragonFuryClass5Align, MortalDragonFuryClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDragonFuryClass6Align, MortalDragonFuryClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDragonFuryClass7Align, MortalDragonFuryClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDragonFuryClass8Align, MortalDragonFuryClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            MortalDragonFuryClasspro = MortalDragonFuryClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDragonFuryClass9Align, MortalDragonFuryClass9AlignGuid)
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
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalEsotericKnightClass3Align, MortalEsotericKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEsotericKnightClass4Align, MortalEsotericKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalEsotericKnightClass5Align, MortalEsotericKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEsotericKnightClass6Align, MortalEsotericKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalEsotericKnightClass7Align, MortalEsotericKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEsotericKnightClass8Align, MortalEsotericKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            MortalEsotericKnightClasspro = MortalEsotericKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalEsotericKnightClass9Align, MortalEsotericKnightClass9AlignGuid)
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
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalExaltedEvangelistClass3Align, MortalExaltedEvangelistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalExaltedEvangelistClass4Align, MortalExaltedEvangelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalExaltedEvangelistClass5Align, MortalExaltedEvangelistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalExaltedEvangelistClass6Align, MortalExaltedEvangelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalExaltedEvangelistClass7Align, MortalExaltedEvangelistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalExaltedEvangelistClass8Align, MortalExaltedEvangelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            MortalExaltedEvangelistClasspro = MortalExaltedEvangelistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalExaltedEvangelistClass9Align, MortalExaltedEvangelistClass9AlignGuid)
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
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalFuriousGuardianClass3Align, MortalFuriousGuardianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalFuriousGuardianClass4Align, MortalFuriousGuardianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalFuriousGuardianClass5Align, MortalFuriousGuardianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalFuriousGuardianClass6Align, MortalFuriousGuardianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalFuriousGuardianClass7Align, MortalFuriousGuardianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalFuriousGuardianClass8Align, MortalFuriousGuardianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            MortalFuriousGuardianClasspro = MortalFuriousGuardianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalFuriousGuardianClass9Align, MortalFuriousGuardianClass9AlignGuid)
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
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHalflingOpportunistClass3Align, MortalHalflingOpportunistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHalflingOpportunistClass4Align, MortalHalflingOpportunistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHalflingOpportunistClass5Align, MortalHalflingOpportunistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHalflingOpportunistClass6Align, MortalHalflingOpportunistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHalflingOpportunistClass7Align, MortalHalflingOpportunistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHalflingOpportunistClass8Align, MortalHalflingOpportunistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            MortalHalflingOpportunistClasspro = MortalHalflingOpportunistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHalflingOpportunistClass9Align, MortalHalflingOpportunistClass9AlignGuid)
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
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHinterlanderClass3Align, MortalHinterlanderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHinterlanderClass4Align, MortalHinterlanderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHinterlanderClass5Align, MortalHinterlanderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHinterlanderClass6Align, MortalHinterlanderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHinterlanderClass7Align, MortalHinterlanderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHinterlanderClass8Align, MortalHinterlanderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            MortalHinterlanderClasspro = MortalHinterlanderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHinterlanderClass9Align, MortalHinterlanderClass9AlignGuid)
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
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHorizonWalkerClass3Align, MortalHorizonWalkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHorizonWalkerClass4Align, MortalHorizonWalkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHorizonWalkerClass5Align, MortalHorizonWalkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHorizonWalkerClass6Align, MortalHorizonWalkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHorizonWalkerClass7Align, MortalHorizonWalkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHorizonWalkerClass8Align, MortalHorizonWalkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            MortalHorizonWalkerClasspro = MortalHorizonWalkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHorizonWalkerClass9Align, MortalHorizonWalkerClass9AlignGuid)
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
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalInheritorCrusaderClass3Align, MortalInheritorCrusaderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalInheritorCrusaderClass4Align, MortalInheritorCrusaderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalInheritorCrusaderClass5Align, MortalInheritorCrusaderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalInheritorCrusaderClass6Align, MortalInheritorCrusaderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalInheritorCrusaderClass7Align, MortalInheritorCrusaderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalInheritorCrusaderClass8Align, MortalInheritorCrusaderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            MortalInheritorCrusaderClasspro = MortalInheritorCrusaderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalInheritorCrusaderClass9Align, MortalInheritorCrusaderClass9AlignGuid)
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
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalMammothRiderClass3Align, MortalMammothRiderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMammothRiderClass4Align, MortalMammothRiderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalMammothRiderClass5Align, MortalMammothRiderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMammothRiderClass6Align, MortalMammothRiderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalMammothRiderClass7Align, MortalMammothRiderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMammothRiderClass8Align, MortalMammothRiderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            MortalMammothRiderClasspro = MortalMammothRiderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalMammothRiderClass9Align, MortalMammothRiderClass9AlignGuid)
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
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSanguineAngelClass3Align, MortalSanguineAngelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSanguineAngelClass4Align, MortalSanguineAngelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSanguineAngelClass5Align, MortalSanguineAngelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSanguineAngelClass6Align, MortalSanguineAngelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSanguineAngelClass7Align, MortalSanguineAngelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSanguineAngelClass8Align, MortalSanguineAngelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            MortalSanguineAngelClasspro = MortalSanguineAngelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSanguineAngelClass9Align, MortalSanguineAngelClass9AlignGuid)
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
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalScarSeekerClass3Align, MortalScarSeekerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalScarSeekerClass4Align, MortalScarSeekerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalScarSeekerClass5Align, MortalScarSeekerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalScarSeekerClass6Align, MortalScarSeekerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalScarSeekerClass7Align, MortalScarSeekerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalScarSeekerClass8Align, MortalScarSeekerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            MortalScarSeekerClasspro = MortalScarSeekerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalScarSeekerClass9Align, MortalScarSeekerClass9AlignGuid)
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
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSentinelClass3Align, MortalSentinelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSentinelClass4Align, MortalSentinelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSentinelClass5Align, MortalSentinelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSentinelClass6Align, MortalSentinelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSentinelClass7Align, MortalSentinelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSentinelClass8Align, MortalSentinelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            MortalSentinelClasspro = MortalSentinelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSentinelClass9Align, MortalSentinelClass9AlignGuid)
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
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalShadowDancerClass3Align, MortalShadowDancerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalShadowDancerClass4Align, MortalShadowDancerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalShadowDancerClass5Align, MortalShadowDancerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalShadowDancerClass6Align, MortalShadowDancerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalShadowDancerClass7Align, MortalShadowDancerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalShadowDancerClass8Align, MortalShadowDancerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            MortalShadowDancerClasspro = MortalShadowDancerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalShadowDancerClass9Align, MortalShadowDancerClass9AlignGuid)
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
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSouldrinkerClass3Align, MortalSouldrinkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSouldrinkerClass4Align, MortalSouldrinkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSouldrinkerClass5Align, MortalSouldrinkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSouldrinkerClass6Align, MortalSouldrinkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSouldrinkerClass7Align, MortalSouldrinkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSouldrinkerClass8Align, MortalSouldrinkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            MortalSouldrinkerClasspro = MortalSouldrinkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSouldrinkerClass9Align, MortalSouldrinkerClass9AlignGuid)
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
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalUmbralAgentClass3Align, MortalUmbralAgentClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalUmbralAgentClass4Align, MortalUmbralAgentClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalUmbralAgentClass5Align, MortalUmbralAgentClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalUmbralAgentClass6Align, MortalUmbralAgentClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalUmbralAgentClass7Align, MortalUmbralAgentClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalUmbralAgentClass8Align, MortalUmbralAgentClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            MortalUmbralAgentClasspro = MortalUmbralAgentClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalUmbralAgentClass9Align, MortalUmbralAgentClass9AlignGuid)
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
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass3Align, MortalMicroAntiPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass4Align, MortalMicroAntiPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass5Align, MortalMicroAntiPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass6Align, MortalMicroAntiPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass7Align, MortalMicroAntiPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass8Align, MortalMicroAntiPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            MortalMicroAntiPaladinClasspro = MortalMicroAntiPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalMicroAntiPaladinClass9Align, MortalMicroAntiPaladinClass9AlignGuid)
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
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalOathbreakerClass3Align, MortalOathbreakerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalOathbreakerClass4Align, MortalOathbreakerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalOathbreakerClass5Align, MortalOathbreakerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalOathbreakerClass6Align, MortalOathbreakerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalOathbreakerClass7Align, MortalOathbreakerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalOathbreakerClass8Align, MortalOathbreakerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            MortalOathbreakerClasspro = MortalOathbreakerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalOathbreakerClass9Align, MortalOathbreakerClass9AlignGuid)
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
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalDreadKnightClass3Align, MortalDreadKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalDreadKnightClass4Align, MortalDreadKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalDreadKnightClass5Align, MortalDreadKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalDreadKnightClass6Align, MortalDreadKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalDreadKnightClass7Align, MortalDreadKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalDreadKnightClass8Align, MortalDreadKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            MortalDreadKnightClasspro = MortalDreadKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalDreadKnightClass9Align, MortalDreadKnightClass9AlignGuid)
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
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalStargazerClass3Align, MortalStargazerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalStargazerClass4Align, MortalStargazerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalStargazerClass5Align, MortalStargazerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalStargazerClass6Align, MortalStargazerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalStargazerClass7Align, MortalStargazerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalStargazerClass8Align, MortalStargazerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            MortalStargazerClasspro = MortalStargazerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalStargazerClass9Align, MortalStargazerClass9AlignGuid)
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
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSwashbucklerClass3Align, MortalSwashbucklerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSwashbucklerClass4Align, MortalSwashbucklerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSwashbucklerClass5Align, MortalSwashbucklerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSwashbucklerClass6Align, MortalSwashbucklerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSwashbucklerClass7Align, MortalSwashbucklerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSwashbucklerClass8Align, MortalSwashbucklerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            MortalSwashbucklerClasspro = MortalSwashbucklerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSwashbucklerClass9Align, MortalSwashbucklerClass9AlignGuid)
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
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHolyVindicatorClass3Align, MortalHolyVindicatorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHolyVindicatorClass4Align, MortalHolyVindicatorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHolyVindicatorClass5Align, MortalHolyVindicatorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHolyVindicatorClass6Align, MortalHolyVindicatorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHolyVindicatorClass7Align, MortalHolyVindicatorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHolyVindicatorClass8Align, MortalHolyVindicatorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            MortalHolyVindicatorClasspro = MortalHolyVindicatorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHolyVindicatorClass9Align, MortalHolyVindicatorClass9AlignGuid)
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
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalSummonerClass3Align, MortalSummonerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalSummonerClass4Align, MortalSummonerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalSummonerClass5Align, MortalSummonerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalSummonerClass6Align, MortalSummonerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalSummonerClass7Align, MortalSummonerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalSummonerClass8Align, MortalSummonerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            MortalSummonerClasspro = MortalSummonerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalSummonerClass9Align, MortalSummonerClass9AlignGuid)
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
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalLionBladeClass3Align, MortalLionBladeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalLionBladeClass4Align, MortalLionBladeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalLionBladeClass5Align, MortalLionBladeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalLionBladeClass6Align, MortalLionBladeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalLionBladeClass7Align, MortalLionBladeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalLionBladeClass8Align, MortalLionBladeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            MortalLionBladeClasspro = MortalLionBladeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalLionBladeClass9Align, MortalLionBladeClass9AlignGuid)
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
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass3Align, MortalEnchantingCourtesanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass4Align, MortalEnchantingCourtesanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass5Align, MortalEnchantingCourtesanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass6Align, MortalEnchantingCourtesanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass7Align, MortalEnchantingCourtesanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass8Align, MortalEnchantingCourtesanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            MortalEnchantingCourtesanClasspro = MortalEnchantingCourtesanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalEnchantingCourtesanClass9Align, MortalEnchantingCourtesanClass9AlignGuid)
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
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalHeritorKnightClass3Align, MortalHeritorKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalHeritorKnightClass4Align, MortalHeritorKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalHeritorKnightClass5Align, MortalHeritorKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalHeritorKnightClass6Align, MortalHeritorKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalHeritorKnightClass7Align, MortalHeritorKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalHeritorKnightClass8Align, MortalHeritorKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            MortalHeritorKnightClasspro = MortalHeritorKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalHeritorKnightClass9Align, MortalHeritorKnightClass9AlignGuid)
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
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass3Align, MortalGoldenLegionnaireClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass4Align, MortalGoldenLegionnaireClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass5Align, MortalGoldenLegionnaireClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass6Align, MortalGoldenLegionnaireClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass7Align, MortalGoldenLegionnaireClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass8Align, MortalGoldenLegionnaireClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            MortalGoldenLegionnaireClasspro = MortalGoldenLegionnaireClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalGoldenLegionnaireClass9Align, MortalGoldenLegionnaireClass9AlignGuid)
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
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalBoltAceClass3Align, MortalBoltAceClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalBoltAceClass4Align, MortalBoltAceClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalBoltAceClass5Align, MortalBoltAceClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalBoltAceClass6Align, MortalBoltAceClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalBoltAceClass7Align, MortalBoltAceClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalBoltAceClass8Align, MortalBoltAceClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            MortalBoltAceClasspro = MortalBoltAceClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalBoltAceClass9Align, MortalBoltAceClass9AlignGuid)
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
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(MortalForestWardenClass3Align, MortalForestWardenClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(MortalForestWardenClass4Align, MortalForestWardenClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(MortalForestWardenClass5Align, MortalForestWardenClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(MortalForestWardenClass6Align, MortalForestWardenClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(MortalForestWardenClass7Align, MortalForestWardenClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(MortalForestWardenClass8Align, MortalForestWardenClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ForestWardenClass; })
            .SetHideInUI(true).Configure());
            MortalForestWardenClasspro = MortalForestWardenClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(MortalForestWardenClass9Align, MortalForestWardenClass9AlignGuid)
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
        private static readonly string MortalAlchemistClass0AlignGuid = "9d4bdd72-c887-401c-8734-c04219a0bb20";
        internal const string MortalAlchemistClass0AlignDisplayName = "MortalAlchemistClass0Align.Name";
        private const string MortalAlchemistClass0AlignDescription = "MortalAlchemistClass0Align.Description";
        private const string MortalAlchemistClass2Align = "MortalAlchemistClass2Align";
        private static readonly string MortalAlchemistClass2AlignGuid = "3f953e14-221f-460c-aac9-ae99e70b39c1";
        private const string MortalAlchemistClass3Align = "MortalAlchemistClass3Align";
        private static readonly string MortalAlchemistClass3AlignGuid = "fb31fb52-38aa-4c1c-877c-dfb7a1f64731";
        private const string MortalAlchemistClass4Align = "MortalAlchemistClass4Align";
        private static readonly string MortalAlchemistClass4AlignGuid = "8d270a15-bd11-4a27-8e0b-3eb381e5a833";
        private const string MortalAlchemistClass5Align = "MortalAlchemistClass5Align";
        private static readonly string MortalAlchemistClass5AlignGuid = "19b3de8f-44ad-495a-9b20-e2c32dfc5b96";
        private const string MortalAlchemistClass6Align = "MortalAlchemistClass6Align";
        private static readonly string MortalAlchemistClass6AlignGuid = "7a64ac07-0a47-472c-9bbd-bb31cbf632be";
        private const string MortalAlchemistClass7Align = "MortalAlchemistClass7Align";
        private static readonly string MortalAlchemistClass7AlignGuid = "40937f37-d2a8-4d02-8c9b-162bde60f4e2";
        private const string MortalAlchemistClass8Align = "MortalAlchemistClass8Align";
        private static readonly string MortalAlchemistClass8AlignGuid = "91b973d0-b1ff-47c5-97fc-d287cb9b6f7b";
        private const string MortalAlchemistClass9Align = "MortalAlchemistClass9Align";
        private static readonly string MortalAlchemistClass9AlignGuid = "ee8c9a03-2cf9-44da-8fb5-eca7e68d2eac";
        private const string MortalAlchemistClass10Align = "MortalAlchemistClass10Align";
        private static readonly string MortalAlchemistClass10AlignGuid = "fab6bdc0-e1bd-46e4-a13d-a3ea49865189";
        private const string MortalArcaneTricksterClass0Align = "MortalArcaneTricksterClass0Align";
        private static readonly string MortalArcaneTricksterClass0AlignGuid = "22ad581b-3d42-4a3a-9d59-55f5ddc61868";
        internal const string MortalArcaneTricksterClass0AlignDisplayName = "MortalArcaneTricksterClass0Align.Name";
        private const string MortalArcaneTricksterClass0AlignDescription = "MortalArcaneTricksterClass0Align.Description";
        private const string MortalArcaneTricksterClass2Align = "MortalArcaneTricksterClass2Align";
        private static readonly string MortalArcaneTricksterClass2AlignGuid = "f29485d8-f817-4993-8acd-15d48c1763c0";
        private const string MortalArcaneTricksterClass3Align = "MortalArcaneTricksterClass3Align";
        private static readonly string MortalArcaneTricksterClass3AlignGuid = "afe56a2f-af34-4dc4-8599-fdb7c7d7de54";
        private const string MortalArcaneTricksterClass4Align = "MortalArcaneTricksterClass4Align";
        private static readonly string MortalArcaneTricksterClass4AlignGuid = "c6d298ce-29e2-4c38-8ef3-dc18dd04222d";
        private const string MortalArcaneTricksterClass5Align = "MortalArcaneTricksterClass5Align";
        private static readonly string MortalArcaneTricksterClass5AlignGuid = "31d5ebb4-616d-4a2e-bc9e-bfafddbd6062";
        private const string MortalArcaneTricksterClass6Align = "MortalArcaneTricksterClass6Align";
        private static readonly string MortalArcaneTricksterClass6AlignGuid = "4782c547-aaa9-4663-8f78-52285b40a25e";
        private const string MortalArcaneTricksterClass7Align = "MortalArcaneTricksterClass7Align";
        private static readonly string MortalArcaneTricksterClass7AlignGuid = "edd6f7fd-9bc9-407e-b470-da5bb77936c4";
        private const string MortalArcaneTricksterClass8Align = "MortalArcaneTricksterClass8Align";
        private static readonly string MortalArcaneTricksterClass8AlignGuid = "bb663f17-4acd-401f-a834-ba66c9c73455";
        private const string MortalArcaneTricksterClass9Align = "MortalArcaneTricksterClass9Align";
        private static readonly string MortalArcaneTricksterClass9AlignGuid = "348331d1-8766-44fc-946d-395090ccaa7f";
        private const string MortalArcaneTricksterClass10Align = "MortalArcaneTricksterClass10Align";
        private static readonly string MortalArcaneTricksterClass10AlignGuid = "463955dd-8e7c-46b2-be44-cb5d356fa8ae";
        private const string MortalArcanistClass0Align = "MortalArcanistClass0Align";
        private static readonly string MortalArcanistClass0AlignGuid = "353ab83f-ad55-4c8c-898c-6b4270ec9add";
        internal const string MortalArcanistClass0AlignDisplayName = "MortalArcanistClass0Align.Name";
        private const string MortalArcanistClass0AlignDescription = "MortalArcanistClass0Align.Description";
        private const string MortalArcanistClass2Align = "MortalArcanistClass2Align";
        private static readonly string MortalArcanistClass2AlignGuid = "dc3e2e2c-d703-432e-a291-519a3c3d74b1";
        private const string MortalArcanistClass3Align = "MortalArcanistClass3Align";
        private static readonly string MortalArcanistClass3AlignGuid = "30666cc1-a130-41b4-b1c9-1a369f595a0a";
        private const string MortalArcanistClass4Align = "MortalArcanistClass4Align";
        private static readonly string MortalArcanistClass4AlignGuid = "60ccb90c-ebd3-4b6d-8dff-71403e8c89f1";
        private const string MortalArcanistClass5Align = "MortalArcanistClass5Align";
        private static readonly string MortalArcanistClass5AlignGuid = "aaf15014-3183-4202-99a2-f801de56c4cc";
        private const string MortalArcanistClass6Align = "MortalArcanistClass6Align";
        private static readonly string MortalArcanistClass6AlignGuid = "29132f62-7905-4229-8cc1-a895e8270d53";
        private const string MortalArcanistClass7Align = "MortalArcanistClass7Align";
        private static readonly string MortalArcanistClass7AlignGuid = "ff2c5e6e-fbd1-44a5-8c48-425ce76649be";
        private const string MortalArcanistClass8Align = "MortalArcanistClass8Align";
        private static readonly string MortalArcanistClass8AlignGuid = "a5c2bbd8-6b09-4eab-bcef-24632290db08";
        private const string MortalArcanistClass9Align = "MortalArcanistClass9Align";
        private static readonly string MortalArcanistClass9AlignGuid = "7afeff2a-39ad-4a56-b133-52e00f20d037";
        private const string MortalArcanistClass10Align = "MortalArcanistClass10Align";
        private static readonly string MortalArcanistClass10AlignGuid = "587c92eb-1552-4329-b693-51b2705a2023";
        private const string MortalAssassinClass0Align = "MortalAssassinClass0Align";
        private static readonly string MortalAssassinClass0AlignGuid = "d9c71951-ddd1-4ff5-a557-57e588633cab";
        internal const string MortalAssassinClass0AlignDisplayName = "MortalAssassinClass0Align.Name";
        private const string MortalAssassinClass0AlignDescription = "MortalAssassinClass0Align.Description";
        private const string MortalAssassinClass2Align = "MortalAssassinClass2Align";
        private static readonly string MortalAssassinClass2AlignGuid = "5919e50a-24cd-4529-9ee2-711995b3304b";
        private const string MortalAssassinClass3Align = "MortalAssassinClass3Align";
        private static readonly string MortalAssassinClass3AlignGuid = "a400e9a9-8f35-4056-8cbe-655735981520";
        private const string MortalAssassinClass4Align = "MortalAssassinClass4Align";
        private static readonly string MortalAssassinClass4AlignGuid = "c6338c66-d074-4930-b85d-f9aea112c919";
        private const string MortalAssassinClass5Align = "MortalAssassinClass5Align";
        private static readonly string MortalAssassinClass5AlignGuid = "c0e1846b-feec-4413-9545-b32c4b2007d9";
        private const string MortalAssassinClass6Align = "MortalAssassinClass6Align";
        private static readonly string MortalAssassinClass6AlignGuid = "d198c4b2-e3bc-4742-81ca-b5e12aa5fed0";
        private const string MortalAssassinClass7Align = "MortalAssassinClass7Align";
        private static readonly string MortalAssassinClass7AlignGuid = "dbeee5c5-0fd0-40a0-a8ad-a14eabac8aad";
        private const string MortalAssassinClass8Align = "MortalAssassinClass8Align";
        private static readonly string MortalAssassinClass8AlignGuid = "c30e73da-0b1f-475b-9bc1-189e96cd54ab";
        private const string MortalAssassinClass9Align = "MortalAssassinClass9Align";
        private static readonly string MortalAssassinClass9AlignGuid = "5a1bf1f0-6730-4348-8f03-4a90e3ca67a0";
        private const string MortalAssassinClass10Align = "MortalAssassinClass10Align";
        private static readonly string MortalAssassinClass10AlignGuid = "c6270d90-af6c-4158-8d6c-ad8831565cc4";
        private const string MortalBarbarianClass0Align = "MortalBarbarianClass0Align";
        private static readonly string MortalBarbarianClass0AlignGuid = "0141e2a9-11aa-4e1f-a7fa-2b7b4eb8da80";
        internal const string MortalBarbarianClass0AlignDisplayName = "MortalBarbarianClass0Align.Name";
        private const string MortalBarbarianClass0AlignDescription = "MortalBarbarianClass0Align.Description";
        private const string MortalBarbarianClass2Align = "MortalBarbarianClass2Align";
        private static readonly string MortalBarbarianClass2AlignGuid = "b9cf0657-b861-4379-b5c0-c8363011e5a0";
        private const string MortalBarbarianClass3Align = "MortalBarbarianClass3Align";
        private static readonly string MortalBarbarianClass3AlignGuid = "b85206fc-2516-4984-99fb-5c609ed83b00";
        private const string MortalBarbarianClass4Align = "MortalBarbarianClass4Align";
        private static readonly string MortalBarbarianClass4AlignGuid = "09a83808-26b9-40d5-a840-3d2e62d6d013";
        private const string MortalBarbarianClass5Align = "MortalBarbarianClass5Align";
        private static readonly string MortalBarbarianClass5AlignGuid = "b4175776-b545-4f0c-949f-27a0512aa6f3";
        private const string MortalBarbarianClass6Align = "MortalBarbarianClass6Align";
        private static readonly string MortalBarbarianClass6AlignGuid = "18fd22d8-e248-4d61-a989-e31361091cb7";
        private const string MortalBarbarianClass7Align = "MortalBarbarianClass7Align";
        private static readonly string MortalBarbarianClass7AlignGuid = "b50b553f-de04-41bf-b76c-7e54118ad360";
        private const string MortalBarbarianClass8Align = "MortalBarbarianClass8Align";
        private static readonly string MortalBarbarianClass8AlignGuid = "27f1bafe-06a4-4102-a3f9-7ef9e690d514";
        private const string MortalBarbarianClass9Align = "MortalBarbarianClass9Align";
        private static readonly string MortalBarbarianClass9AlignGuid = "b438088b-f17d-4244-b5c6-367a3a05e1e7";
        private const string MortalBarbarianClass10Align = "MortalBarbarianClass10Align";
        private static readonly string MortalBarbarianClass10AlignGuid = "9e8ccf41-c6fa-42ac-9069-f71299175ed9";
        private const string MortalBardClass0Align = "MortalBardClass0Align";
        private static readonly string MortalBardClass0AlignGuid = "9ed6bd86-14df-464c-ba0b-3d9d2d7e5d91";
        internal const string MortalBardClass0AlignDisplayName = "MortalBardClass0Align.Name";
        private const string MortalBardClass0AlignDescription = "MortalBardClass0Align.Description";
        private const string MortalBardClass2Align = "MortalBardClass2Align";
        private static readonly string MortalBardClass2AlignGuid = "821717f1-b54e-45e2-943c-0cfaed7ba491";
        private const string MortalBardClass3Align = "MortalBardClass3Align";
        private static readonly string MortalBardClass3AlignGuid = "c0048767-d53c-47c0-a288-95852ccff78e";
        private const string MortalBardClass4Align = "MortalBardClass4Align";
        private static readonly string MortalBardClass4AlignGuid = "0b616ec4-10d5-408e-9a08-d0b26837ee80";
        private const string MortalBardClass5Align = "MortalBardClass5Align";
        private static readonly string MortalBardClass5AlignGuid = "098aa737-054e-4c3b-9d75-6543c717c603";
        private const string MortalBardClass6Align = "MortalBardClass6Align";
        private static readonly string MortalBardClass6AlignGuid = "2d78ecf8-1000-4574-8386-d3e9dff4456e";
        private const string MortalBardClass7Align = "MortalBardClass7Align";
        private static readonly string MortalBardClass7AlignGuid = "621c1bbf-58dd-4589-9a1c-9e1bb422a17c";
        private const string MortalBardClass8Align = "MortalBardClass8Align";
        private static readonly string MortalBardClass8AlignGuid = "fd2c1c39-a972-4664-a4f5-5d6b7a7dba04";
        private const string MortalBardClass9Align = "MortalBardClass9Align";
        private static readonly string MortalBardClass9AlignGuid = "d56e0424-becf-4ac8-ab04-c41f86638485";
        private const string MortalBardClass10Align = "MortalBardClass10Align";
        private static readonly string MortalBardClass10AlignGuid = "4514fc7d-6878-470b-b550-b50ac894cc58";
        private const string MortalBloodragerClass0Align = "MortalBloodragerClass0Align";
        private static readonly string MortalBloodragerClass0AlignGuid = "80fff047-baf7-498a-9d85-164fe8a8e449";
        internal const string MortalBloodragerClass0AlignDisplayName = "MortalBloodragerClass0Align.Name";
        private const string MortalBloodragerClass0AlignDescription = "MortalBloodragerClass0Align.Description";
        private const string MortalBloodragerClass2Align = "MortalBloodragerClass2Align";
        private static readonly string MortalBloodragerClass2AlignGuid = "9142bdb7-50c3-42b5-bc6d-493579db2db1";
        private const string MortalBloodragerClass3Align = "MortalBloodragerClass3Align";
        private static readonly string MortalBloodragerClass3AlignGuid = "1a5c0e67-3fa0-478e-a2c4-0592f4cbc139";
        private const string MortalBloodragerClass4Align = "MortalBloodragerClass4Align";
        private static readonly string MortalBloodragerClass4AlignGuid = "cd7e43a9-3148-4e85-b3a8-1310104d4128";
        private const string MortalBloodragerClass5Align = "MortalBloodragerClass5Align";
        private static readonly string MortalBloodragerClass5AlignGuid = "344595f4-7e44-47bd-8890-7e1ff1bcf4c4";
        private const string MortalBloodragerClass6Align = "MortalBloodragerClass6Align";
        private static readonly string MortalBloodragerClass6AlignGuid = "12ee07ce-991f-4aec-bff3-589f397b76cf";
        private const string MortalBloodragerClass7Align = "MortalBloodragerClass7Align";
        private static readonly string MortalBloodragerClass7AlignGuid = "2fd97400-f213-4705-bbbf-f94cf75daace";
        private const string MortalBloodragerClass8Align = "MortalBloodragerClass8Align";
        private static readonly string MortalBloodragerClass8AlignGuid = "76af1a0f-a106-47d9-aba8-83147f9f52b4";
        private const string MortalBloodragerClass9Align = "MortalBloodragerClass9Align";
        private static readonly string MortalBloodragerClass9AlignGuid = "4cfe09ec-4503-437f-b5ee-ef1a442161dc";
        private const string MortalBloodragerClass10Align = "MortalBloodragerClass10Align";
        private static readonly string MortalBloodragerClass10AlignGuid = "f6024b59-bc13-46e8-a378-d1cf19e2ca06";
        private const string MortalCavalierClass0Align = "MortalCavalierClass0Align";
        private static readonly string MortalCavalierClass0AlignGuid = "ec405dd4-2bee-4833-9eaf-d3255b65085e";
        internal const string MortalCavalierClass0AlignDisplayName = "MortalCavalierClass0Align.Name";
        private const string MortalCavalierClass0AlignDescription = "MortalCavalierClass0Align.Description";
        private const string MortalCavalierClass2Align = "MortalCavalierClass2Align";
        private static readonly string MortalCavalierClass2AlignGuid = "83f23699-24af-439c-ab70-649cdef3f56a";
        private const string MortalCavalierClass3Align = "MortalCavalierClass3Align";
        private static readonly string MortalCavalierClass3AlignGuid = "a56fdbe8-ae6a-44c1-a716-a30404b65300";
        private const string MortalCavalierClass4Align = "MortalCavalierClass4Align";
        private static readonly string MortalCavalierClass4AlignGuid = "5afde2df-3785-457a-a591-278fceead255";
        private const string MortalCavalierClass5Align = "MortalCavalierClass5Align";
        private static readonly string MortalCavalierClass5AlignGuid = "6ff18546-d4e7-4db5-bd3d-b8a797000b00";
        private const string MortalCavalierClass6Align = "MortalCavalierClass6Align";
        private static readonly string MortalCavalierClass6AlignGuid = "b1e4544f-186f-43fb-bfc4-9561fc82fbde";
        private const string MortalCavalierClass7Align = "MortalCavalierClass7Align";
        private static readonly string MortalCavalierClass7AlignGuid = "14da7cf5-5958-4df9-a536-a1a02a5506b9";
        private const string MortalCavalierClass8Align = "MortalCavalierClass8Align";
        private static readonly string MortalCavalierClass8AlignGuid = "81dd76db-56ed-483b-89e2-26ae3de30b30";
        private const string MortalCavalierClass9Align = "MortalCavalierClass9Align";
        private static readonly string MortalCavalierClass9AlignGuid = "f7066c3d-fe84-4a2d-bc4a-d64c2fbaa04d";
        private const string MortalCavalierClass10Align = "MortalCavalierClass10Align";
        private static readonly string MortalCavalierClass10AlignGuid = "fbe935b9-1506-402f-abb0-7573cd2424a0";
        private const string MortalClericClass0Align = "MortalClericClass0Align";
        private static readonly string MortalClericClass0AlignGuid = "8d964b9a-7b9b-4012-bf18-08e2b213df66";
        internal const string MortalClericClass0AlignDisplayName = "MortalClericClass0Align.Name";
        private const string MortalClericClass0AlignDescription = "MortalClericClass0Align.Description";
        private const string MortalClericClass2Align = "MortalClericClass2Align";
        private static readonly string MortalClericClass2AlignGuid = "f62f1cb6-3671-4e9f-a468-9bee928560fd";
        private const string MortalClericClass3Align = "MortalClericClass3Align";
        private static readonly string MortalClericClass3AlignGuid = "ba62588c-dfd5-4eaf-aa26-87f0d0514411";
        private const string MortalClericClass4Align = "MortalClericClass4Align";
        private static readonly string MortalClericClass4AlignGuid = "3285bab8-b67a-454b-8042-d14e115d94ba";
        private const string MortalClericClass5Align = "MortalClericClass5Align";
        private static readonly string MortalClericClass5AlignGuid = "e702e325-f789-491c-aa7a-42ef7b3ad044";
        private const string MortalClericClass6Align = "MortalClericClass6Align";
        private static readonly string MortalClericClass6AlignGuid = "3670e2e2-f118-4d6d-b9f2-9e9055d47cb1";
        private const string MortalClericClass7Align = "MortalClericClass7Align";
        private static readonly string MortalClericClass7AlignGuid = "09d9689d-e2b2-40ce-b3f9-75e6f2bc10bc";
        private const string MortalClericClass8Align = "MortalClericClass8Align";
        private static readonly string MortalClericClass8AlignGuid = "e7617c85-4530-4606-8d5b-d6d866e297a1";
        private const string MortalClericClass9Align = "MortalClericClass9Align";
        private static readonly string MortalClericClass9AlignGuid = "908cfdb4-0de2-4e32-94a9-962ccc23d7f7";
        private const string MortalClericClass10Align = "MortalClericClass10Align";
        private static readonly string MortalClericClass10AlignGuid = "b41590d5-b80d-4b26-b983-0e6738fbd783";
        private const string MortalDragonDiscipleClass0Align = "MortalDragonDiscipleClass0Align";
        private static readonly string MortalDragonDiscipleClass0AlignGuid = "0e429925-5d04-4401-b130-dcb780041295";
        internal const string MortalDragonDiscipleClass0AlignDisplayName = "MortalDragonDiscipleClass0Align.Name";
        private const string MortalDragonDiscipleClass0AlignDescription = "MortalDragonDiscipleClass0Align.Description";
        private const string MortalDragonDiscipleClass2Align = "MortalDragonDiscipleClass2Align";
        private static readonly string MortalDragonDiscipleClass2AlignGuid = "27495553-6455-4e76-a35d-b5f49ef3ce7a";
        private const string MortalDragonDiscipleClass3Align = "MortalDragonDiscipleClass3Align";
        private static readonly string MortalDragonDiscipleClass3AlignGuid = "6ae90e07-8d5c-4806-ab12-018814407114";
        private const string MortalDragonDiscipleClass4Align = "MortalDragonDiscipleClass4Align";
        private static readonly string MortalDragonDiscipleClass4AlignGuid = "cba84ddc-2c42-478d-b968-19e1c8461b5c";
        private const string MortalDragonDiscipleClass5Align = "MortalDragonDiscipleClass5Align";
        private static readonly string MortalDragonDiscipleClass5AlignGuid = "3fe5c03b-30ed-4ace-a8af-39b6b502d321";
        private const string MortalDragonDiscipleClass6Align = "MortalDragonDiscipleClass6Align";
        private static readonly string MortalDragonDiscipleClass6AlignGuid = "f3effe3b-ae01-4c37-9ad3-351d7abb220b";
        private const string MortalDragonDiscipleClass7Align = "MortalDragonDiscipleClass7Align";
        private static readonly string MortalDragonDiscipleClass7AlignGuid = "3c7a3a9c-4a53-4218-8c34-9ddc6f871c23";
        private const string MortalDragonDiscipleClass8Align = "MortalDragonDiscipleClass8Align";
        private static readonly string MortalDragonDiscipleClass8AlignGuid = "aab2f0d2-b28b-4902-900e-7d37e21d16eb";
        private const string MortalDragonDiscipleClass9Align = "MortalDragonDiscipleClass9Align";
        private static readonly string MortalDragonDiscipleClass9AlignGuid = "9b038b92-a339-4e79-85e6-78e6530d885d";
        private const string MortalDragonDiscipleClass10Align = "MortalDragonDiscipleClass10Align";
        private static readonly string MortalDragonDiscipleClass10AlignGuid = "e4cd8210-5ed4-4ea4-a848-e09a2c816ec4";
        private const string MortalDruidClass0Align = "MortalDruidClass0Align";
        private static readonly string MortalDruidClass0AlignGuid = "43318b3b-1792-4d03-9401-d48bf02d9332";
        internal const string MortalDruidClass0AlignDisplayName = "MortalDruidClass0Align.Name";
        private const string MortalDruidClass0AlignDescription = "MortalDruidClass0Align.Description";
        private const string MortalDruidClass2Align = "MortalDruidClass2Align";
        private static readonly string MortalDruidClass2AlignGuid = "c1241900-21d9-41f4-93a8-94596fd13b76";
        private const string MortalDruidClass3Align = "MortalDruidClass3Align";
        private static readonly string MortalDruidClass3AlignGuid = "47526108-f6c9-40a5-93ee-62377f96d2b2";
        private const string MortalDruidClass4Align = "MortalDruidClass4Align";
        private static readonly string MortalDruidClass4AlignGuid = "4abd720c-392d-4a77-b4dd-9ee839d4cad7";
        private const string MortalDruidClass5Align = "MortalDruidClass5Align";
        private static readonly string MortalDruidClass5AlignGuid = "ec251a76-1d8f-48cf-b4c2-38596989b069";
        private const string MortalDruidClass6Align = "MortalDruidClass6Align";
        private static readonly string MortalDruidClass6AlignGuid = "73204b81-6b23-4cd0-871d-2fd47ed7ca21";
        private const string MortalDruidClass7Align = "MortalDruidClass7Align";
        private static readonly string MortalDruidClass7AlignGuid = "0c3cb79b-e6f5-406d-b58a-84557f2be7b8";
        private const string MortalDruidClass8Align = "MortalDruidClass8Align";
        private static readonly string MortalDruidClass8AlignGuid = "af7fcf03-3960-4f28-88de-4106bc528ff2";
        private const string MortalDruidClass9Align = "MortalDruidClass9Align";
        private static readonly string MortalDruidClass9AlignGuid = "05349c30-7133-495c-a81f-8a98ac90db13";
        private const string MortalDruidClass10Align = "MortalDruidClass10Align";
        private static readonly string MortalDruidClass10AlignGuid = "1048beda-2326-48c9-b8b4-bff08fc5a316";
        private const string MortalDuelistClass0Align = "MortalDuelistClass0Align";
        private static readonly string MortalDuelistClass0AlignGuid = "3e7a9881-c558-495c-98f6-99df78b5f442";
        internal const string MortalDuelistClass0AlignDisplayName = "MortalDuelistClass0Align.Name";
        private const string MortalDuelistClass0AlignDescription = "MortalDuelistClass0Align.Description";
        private const string MortalDuelistClass2Align = "MortalDuelistClass2Align";
        private static readonly string MortalDuelistClass2AlignGuid = "51d104cf-5b5e-4717-b35b-8bacbb98aa1b";
        private const string MortalDuelistClass3Align = "MortalDuelistClass3Align";
        private static readonly string MortalDuelistClass3AlignGuid = "59d79724-c570-4756-945a-9173b9922fe5";
        private const string MortalDuelistClass4Align = "MortalDuelistClass4Align";
        private static readonly string MortalDuelistClass4AlignGuid = "9261a7cd-712b-4d45-a505-b5fab6227748";
        private const string MortalDuelistClass5Align = "MortalDuelistClass5Align";
        private static readonly string MortalDuelistClass5AlignGuid = "0f5510bc-15a2-4c21-9b5d-a023b0dd9c8d";
        private const string MortalDuelistClass6Align = "MortalDuelistClass6Align";
        private static readonly string MortalDuelistClass6AlignGuid = "6dc6ff16-15b4-43d8-aae3-9db310c7ea70";
        private const string MortalDuelistClass7Align = "MortalDuelistClass7Align";
        private static readonly string MortalDuelistClass7AlignGuid = "48464c5a-d515-447a-8367-abeecdf08e10";
        private const string MortalDuelistClass8Align = "MortalDuelistClass8Align";
        private static readonly string MortalDuelistClass8AlignGuid = "e074dcc7-a5f3-40ec-b6bc-3c2883766b24";
        private const string MortalDuelistClass9Align = "MortalDuelistClass9Align";
        private static readonly string MortalDuelistClass9AlignGuid = "5c0a1d6f-36fb-4fdb-b8ea-ffcf7924eac1";
        private const string MortalDuelistClass10Align = "MortalDuelistClass10Align";
        private static readonly string MortalDuelistClass10AlignGuid = "b5e76fab-a9d3-43f7-b353-26e687bdf6ca";
        private const string MortalEldritchKnightClass0Align = "MortalEldritchKnightClass0Align";
        private static readonly string MortalEldritchKnightClass0AlignGuid = "c081b392-7b25-4389-94ec-af58c439cdf3";
        internal const string MortalEldritchKnightClass0AlignDisplayName = "MortalEldritchKnightClass0Align.Name";
        private const string MortalEldritchKnightClass0AlignDescription = "MortalEldritchKnightClass0Align.Description";
        private const string MortalEldritchKnightClass2Align = "MortalEldritchKnightClass2Align";
        private static readonly string MortalEldritchKnightClass2AlignGuid = "77fc2b58-094a-497c-8285-d81274d59c61";
        private const string MortalEldritchKnightClass3Align = "MortalEldritchKnightClass3Align";
        private static readonly string MortalEldritchKnightClass3AlignGuid = "2b62ce33-7d70-47e6-98be-bb0fea157c04";
        private const string MortalEldritchKnightClass4Align = "MortalEldritchKnightClass4Align";
        private static readonly string MortalEldritchKnightClass4AlignGuid = "39a11bb7-f198-47c9-81eb-72a2e74af870";
        private const string MortalEldritchKnightClass5Align = "MortalEldritchKnightClass5Align";
        private static readonly string MortalEldritchKnightClass5AlignGuid = "6f3e6ae8-84a7-4186-b91c-29a98c7f550a";
        private const string MortalEldritchKnightClass6Align = "MortalEldritchKnightClass6Align";
        private static readonly string MortalEldritchKnightClass6AlignGuid = "cabb3953-1dc5-4da0-b5f8-2c9c8edb828f";
        private const string MortalEldritchKnightClass7Align = "MortalEldritchKnightClass7Align";
        private static readonly string MortalEldritchKnightClass7AlignGuid = "16ed8848-22b4-4032-b89e-c0c9cd3749bd";
        private const string MortalEldritchKnightClass8Align = "MortalEldritchKnightClass8Align";
        private static readonly string MortalEldritchKnightClass8AlignGuid = "dbba8a60-7c20-4a2d-9e39-52b47ba4988d";
        private const string MortalEldritchKnightClass9Align = "MortalEldritchKnightClass9Align";
        private static readonly string MortalEldritchKnightClass9AlignGuid = "13651353-a3e0-4588-ac94-c8b2e633096d";
        private const string MortalEldritchKnightClass10Align = "MortalEldritchKnightClass10Align";
        private static readonly string MortalEldritchKnightClass10AlignGuid = "3b101223-cf6b-40c2-810a-250c5f485b23";
        private const string MortalEldritchScionClass0Align = "MortalEldritchScionClass0Align";
        private static readonly string MortalEldritchScionClass0AlignGuid = "ffb88a45-75ea-4a73-a4f2-190348a586f5";
        internal const string MortalEldritchScionClass0AlignDisplayName = "MortalEldritchScionClass0Align.Name";
        private const string MortalEldritchScionClass0AlignDescription = "MortalEldritchScionClass0Align.Description";
        private const string MortalEldritchScionClass2Align = "MortalEldritchScionClass2Align";
        private static readonly string MortalEldritchScionClass2AlignGuid = "83b4cdb3-3dac-4dcd-9693-d28c0c9c164c";
        private const string MortalEldritchScionClass3Align = "MortalEldritchScionClass3Align";
        private static readonly string MortalEldritchScionClass3AlignGuid = "be5adbba-1dd3-4721-83fb-1805e6eddb49";
        private const string MortalEldritchScionClass4Align = "MortalEldritchScionClass4Align";
        private static readonly string MortalEldritchScionClass4AlignGuid = "16fef9ff-0ee6-4d54-a620-7e730d0d902b";
        private const string MortalEldritchScionClass5Align = "MortalEldritchScionClass5Align";
        private static readonly string MortalEldritchScionClass5AlignGuid = "86c6748f-9792-42b0-ac22-22400ef8db57";
        private const string MortalEldritchScionClass6Align = "MortalEldritchScionClass6Align";
        private static readonly string MortalEldritchScionClass6AlignGuid = "e003bea7-7697-427f-94ba-643e8a2aaa7d";
        private const string MortalEldritchScionClass7Align = "MortalEldritchScionClass7Align";
        private static readonly string MortalEldritchScionClass7AlignGuid = "d42a466c-d50b-4a34-af69-d744568d1a27";
        private const string MortalEldritchScionClass8Align = "MortalEldritchScionClass8Align";
        private static readonly string MortalEldritchScionClass8AlignGuid = "d415d4e2-06c9-40fe-8323-9e2a89d9924f";
        private const string MortalEldritchScionClass9Align = "MortalEldritchScionClass9Align";
        private static readonly string MortalEldritchScionClass9AlignGuid = "33e58a6f-046d-42f3-88de-a04bb7bd7c11";
        private const string MortalEldritchScionClass10Align = "MortalEldritchScionClass10Align";
        private static readonly string MortalEldritchScionClass10AlignGuid = "5f7dd7db-49d4-491c-9a40-709c1e4d84e5";
        private const string MortalFighterClass0Align = "MortalFighterClass0Align";
        private static readonly string MortalFighterClass0AlignGuid = "b0f03bdd-a0be-42e9-ba8b-eea108396472";
        internal const string MortalFighterClass0AlignDisplayName = "MortalFighterClass0Align.Name";
        private const string MortalFighterClass0AlignDescription = "MortalFighterClass0Align.Description";
        private const string MortalFighterClass2Align = "MortalFighterClass2Align";
        private static readonly string MortalFighterClass2AlignGuid = "f96c5b30-2fd4-4e87-9d24-819c9abcf580";
        private const string MortalFighterClass3Align = "MortalFighterClass3Align";
        private static readonly string MortalFighterClass3AlignGuid = "fe08277b-3e32-4f28-88bf-dcee40cb66b1";
        private const string MortalFighterClass4Align = "MortalFighterClass4Align";
        private static readonly string MortalFighterClass4AlignGuid = "8613f8d0-3804-4c89-8720-49ea158e8b08";
        private const string MortalFighterClass5Align = "MortalFighterClass5Align";
        private static readonly string MortalFighterClass5AlignGuid = "067a330b-ce40-4e9a-9a93-1333712d19e1";
        private const string MortalFighterClass6Align = "MortalFighterClass6Align";
        private static readonly string MortalFighterClass6AlignGuid = "389a3b14-e38a-48a6-b79f-cbc0225f4a14";
        private const string MortalFighterClass7Align = "MortalFighterClass7Align";
        private static readonly string MortalFighterClass7AlignGuid = "fd44033b-6416-4fcd-a22a-cdd3351ccb19";
        private const string MortalFighterClass8Align = "MortalFighterClass8Align";
        private static readonly string MortalFighterClass8AlignGuid = "0a64056c-d3a6-4341-95b0-f2678b4b0163";
        private const string MortalFighterClass9Align = "MortalFighterClass9Align";
        private static readonly string MortalFighterClass9AlignGuid = "203c35e5-7152-45fc-ad9e-ce31fa1f79cd";
        private const string MortalFighterClass10Align = "MortalFighterClass10Align";
        private static readonly string MortalFighterClass10AlignGuid = "1ca514cb-7260-474a-ab04-4f04c2196501";
        private const string MortalHellknightClass0Align = "MortalHellknightClass0Align";
        private static readonly string MortalHellknightClass0AlignGuid = "86aefb8d-8e96-4a9c-9bff-fabc7b4cf7a7";
        internal const string MortalHellknightClass0AlignDisplayName = "MortalHellknightClass0Align.Name";
        private const string MortalHellknightClass0AlignDescription = "MortalHellknightClass0Align.Description";
        private const string MortalHellknightClass2Align = "MortalHellknightClass2Align";
        private static readonly string MortalHellknightClass2AlignGuid = "d2e45d06-3d01-4ec4-afbc-6c6521747e54";
        private const string MortalHellknightClass3Align = "MortalHellknightClass3Align";
        private static readonly string MortalHellknightClass3AlignGuid = "83cbac74-3478-42b4-918d-ba8df195df67";
        private const string MortalHellknightClass4Align = "MortalHellknightClass4Align";
        private static readonly string MortalHellknightClass4AlignGuid = "2960660e-a3d3-4fb8-80df-5585480aa80c";
        private const string MortalHellknightClass5Align = "MortalHellknightClass5Align";
        private static readonly string MortalHellknightClass5AlignGuid = "3e363272-c5e9-4c81-bdf5-7192500fbff5";
        private const string MortalHellknightClass6Align = "MortalHellknightClass6Align";
        private static readonly string MortalHellknightClass6AlignGuid = "035da8e0-0083-4f2d-b43e-7956faee227f";
        private const string MortalHellknightClass7Align = "MortalHellknightClass7Align";
        private static readonly string MortalHellknightClass7AlignGuid = "cded44d7-dc98-4df7-9174-bbec25993fb6";
        private const string MortalHellknightClass8Align = "MortalHellknightClass8Align";
        private static readonly string MortalHellknightClass8AlignGuid = "19ed1255-25fb-4bc1-b8ca-098a11854bdb";
        private const string MortalHellknightClass9Align = "MortalHellknightClass9Align";
        private static readonly string MortalHellknightClass9AlignGuid = "d087a91f-11b8-4c4a-bfde-071171b5ef22";
        private const string MortalHellknightClass10Align = "MortalHellknightClass10Align";
        private static readonly string MortalHellknightClass10AlignGuid = "766d9bc1-ffc6-46fa-8e63-d9e3e858ae89";
        private const string MortalHellknightSigniferClass0Align = "MortalHellknightSigniferClass0Align";
        private static readonly string MortalHellknightSigniferClass0AlignGuid = "7cdd8db0-169e-4e1c-ba41-859cb7c0ef44";
        internal const string MortalHellknightSigniferClass0AlignDisplayName = "MortalHellknightSigniferClass0Align.Name";
        private const string MortalHellknightSigniferClass0AlignDescription = "MortalHellknightSigniferClass0Align.Description";
        private const string MortalHellknightSigniferClass2Align = "MortalHellknightSigniferClass2Align";
        private static readonly string MortalHellknightSigniferClass2AlignGuid = "87d6779f-d0a8-4941-8c09-d32883eb9786";
        private const string MortalHellknightSigniferClass3Align = "MortalHellknightSigniferClass3Align";
        private static readonly string MortalHellknightSigniferClass3AlignGuid = "aa7530e8-2942-4d30-999d-0ccf7d6036fb";
        private const string MortalHellknightSigniferClass4Align = "MortalHellknightSigniferClass4Align";
        private static readonly string MortalHellknightSigniferClass4AlignGuid = "5445e6c0-afce-4521-9552-c6a7819c5d7c";
        private const string MortalHellknightSigniferClass5Align = "MortalHellknightSigniferClass5Align";
        private static readonly string MortalHellknightSigniferClass5AlignGuid = "ad3962fb-a120-4256-8f60-cde36c43319e";
        private const string MortalHellknightSigniferClass6Align = "MortalHellknightSigniferClass6Align";
        private static readonly string MortalHellknightSigniferClass6AlignGuid = "280ef814-1380-4b32-9afb-7033f53b27bb";
        private const string MortalHellknightSigniferClass7Align = "MortalHellknightSigniferClass7Align";
        private static readonly string MortalHellknightSigniferClass7AlignGuid = "8b0549a0-f935-443e-afd3-663a4af5f04a";
        private const string MortalHellknightSigniferClass8Align = "MortalHellknightSigniferClass8Align";
        private static readonly string MortalHellknightSigniferClass8AlignGuid = "42c0c481-7c37-404e-b08b-e8e430275e18";
        private const string MortalHellknightSigniferClass9Align = "MortalHellknightSigniferClass9Align";
        private static readonly string MortalHellknightSigniferClass9AlignGuid = "133cd9fd-b918-41ef-acbf-ed10d763c047";
        private const string MortalHellknightSigniferClass10Align = "MortalHellknightSigniferClass10Align";
        private static readonly string MortalHellknightSigniferClass10AlignGuid = "51488df5-af83-452f-92d2-a0f8a849ca49";
        private const string MortalHunterClass0Align = "MortalHunterClass0Align";
        private static readonly string MortalHunterClass0AlignGuid = "8902db04-5ec3-4dd9-9faa-0698b1e4817c";
        internal const string MortalHunterClass0AlignDisplayName = "MortalHunterClass0Align.Name";
        private const string MortalHunterClass0AlignDescription = "MortalHunterClass0Align.Description";
        private const string MortalHunterClass2Align = "MortalHunterClass2Align";
        private static readonly string MortalHunterClass2AlignGuid = "89b45836-0a49-494e-8c0c-7d1559b57644";
        private const string MortalHunterClass3Align = "MortalHunterClass3Align";
        private static readonly string MortalHunterClass3AlignGuid = "a8fe771e-6eec-4488-90dc-b7396a26cff2";
        private const string MortalHunterClass4Align = "MortalHunterClass4Align";
        private static readonly string MortalHunterClass4AlignGuid = "138e8e16-88b9-4269-b78b-177c035209cf";
        private const string MortalHunterClass5Align = "MortalHunterClass5Align";
        private static readonly string MortalHunterClass5AlignGuid = "5f5d9759-a9d1-45fa-826f-a6f98698230c";
        private const string MortalHunterClass6Align = "MortalHunterClass6Align";
        private static readonly string MortalHunterClass6AlignGuid = "8f9ce674-9d90-45c6-ac51-04323fb62746";
        private const string MortalHunterClass7Align = "MortalHunterClass7Align";
        private static readonly string MortalHunterClass7AlignGuid = "8d8d77cb-1fb6-425c-bfe9-c42963954eb8";
        private const string MortalHunterClass8Align = "MortalHunterClass8Align";
        private static readonly string MortalHunterClass8AlignGuid = "9a849a77-4454-49f8-8b2e-db6ea2ac4baf";
        private const string MortalHunterClass9Align = "MortalHunterClass9Align";
        private static readonly string MortalHunterClass9AlignGuid = "1100c22e-4ce4-4799-9d63-18df4336b198";
        private const string MortalHunterClass10Align = "MortalHunterClass10Align";
        private static readonly string MortalHunterClass10AlignGuid = "8567e77c-5aee-41e1-8c58-7c93925a111f";
        private const string MortalInquisitorClass0Align = "MortalInquisitorClass0Align";
        private static readonly string MortalInquisitorClass0AlignGuid = "0f8281da-b11a-4a2d-905f-f93dc543d2a0";
        internal const string MortalInquisitorClass0AlignDisplayName = "MortalInquisitorClass0Align.Name";
        private const string MortalInquisitorClass0AlignDescription = "MortalInquisitorClass0Align.Description";
        private const string MortalInquisitorClass2Align = "MortalInquisitorClass2Align";
        private static readonly string MortalInquisitorClass2AlignGuid = "d049f89e-9c89-407d-acc7-c19e17135ed5";
        private const string MortalInquisitorClass3Align = "MortalInquisitorClass3Align";
        private static readonly string MortalInquisitorClass3AlignGuid = "65563fc4-fded-4f3a-8921-0073770454d8";
        private const string MortalInquisitorClass4Align = "MortalInquisitorClass4Align";
        private static readonly string MortalInquisitorClass4AlignGuid = "c067a092-d3a2-48ef-a63f-8538037f0897";
        private const string MortalInquisitorClass5Align = "MortalInquisitorClass5Align";
        private static readonly string MortalInquisitorClass5AlignGuid = "77d0cd5f-955d-4503-87cb-84b6694c3bd0";
        private const string MortalInquisitorClass6Align = "MortalInquisitorClass6Align";
        private static readonly string MortalInquisitorClass6AlignGuid = "f11aac95-386b-4dfb-9366-15338fd1e7a1";
        private const string MortalInquisitorClass7Align = "MortalInquisitorClass7Align";
        private static readonly string MortalInquisitorClass7AlignGuid = "db163acd-d755-4ed1-9a91-7e99d61f9449";
        private const string MortalInquisitorClass8Align = "MortalInquisitorClass8Align";
        private static readonly string MortalInquisitorClass8AlignGuid = "6625e26a-158c-481b-af6c-808da9ccfec4";
        private const string MortalInquisitorClass9Align = "MortalInquisitorClass9Align";
        private static readonly string MortalInquisitorClass9AlignGuid = "b4eb8b38-e3c5-4d40-85e1-a4a659e69372";
        private const string MortalInquisitorClass10Align = "MortalInquisitorClass10Align";
        private static readonly string MortalInquisitorClass10AlignGuid = "82801f4c-b807-4a85-ab04-ac82a51047d7";
        private const string MortalKineticistClass0Align = "MortalKineticistClass0Align";
        private static readonly string MortalKineticistClass0AlignGuid = "87532ffb-ca68-4da3-bf8c-65219ac36e06";
        internal const string MortalKineticistClass0AlignDisplayName = "MortalKineticistClass0Align.Name";
        private const string MortalKineticistClass0AlignDescription = "MortalKineticistClass0Align.Description";
        private const string MortalKineticistClass2Align = "MortalKineticistClass2Align";
        private static readonly string MortalKineticistClass2AlignGuid = "11e7cb40-3b3f-4956-84d7-60ed8dc5a326";
        private const string MortalKineticistClass3Align = "MortalKineticistClass3Align";
        private static readonly string MortalKineticistClass3AlignGuid = "7f2da1fd-7846-4840-9d2b-a4ea8cfb5641";
        private const string MortalKineticistClass4Align = "MortalKineticistClass4Align";
        private static readonly string MortalKineticistClass4AlignGuid = "7ceaf067-e3fa-4d12-85d3-98b5c1ca47c0";
        private const string MortalKineticistClass5Align = "MortalKineticistClass5Align";
        private static readonly string MortalKineticistClass5AlignGuid = "b6cba1c9-df69-489d-a10b-c92089f2354b";
        private const string MortalKineticistClass6Align = "MortalKineticistClass6Align";
        private static readonly string MortalKineticistClass6AlignGuid = "84c1d21f-130a-4651-b4a0-790ea2afdf2e";
        private const string MortalKineticistClass7Align = "MortalKineticistClass7Align";
        private static readonly string MortalKineticistClass7AlignGuid = "09fd3108-ece3-4a85-83e3-b42dde1eac83";
        private const string MortalKineticistClass8Align = "MortalKineticistClass8Align";
        private static readonly string MortalKineticistClass8AlignGuid = "0c3fd927-4f6e-40f3-9529-e67d0498926b";
        private const string MortalKineticistClass9Align = "MortalKineticistClass9Align";
        private static readonly string MortalKineticistClass9AlignGuid = "0f1b81e4-f033-421c-b9ab-e911f8bbc13d";
        private const string MortalKineticistClass10Align = "MortalKineticistClass10Align";
        private static readonly string MortalKineticistClass10AlignGuid = "6e0e2a70-c5de-49b0-a9ae-6c453293ac4e";
        private const string MortalLoremasterClass0Align = "MortalLoremasterClass0Align";
        private static readonly string MortalLoremasterClass0AlignGuid = "1a8cffad-b003-4b49-ae91-a5557966ef84";
        internal const string MortalLoremasterClass0AlignDisplayName = "MortalLoremasterClass0Align.Name";
        private const string MortalLoremasterClass0AlignDescription = "MortalLoremasterClass0Align.Description";
        private const string MortalLoremasterClass2Align = "MortalLoremasterClass2Align";
        private static readonly string MortalLoremasterClass2AlignGuid = "54983782-78ef-4999-bf4d-9d08a5ce38cf";
        private const string MortalLoremasterClass3Align = "MortalLoremasterClass3Align";
        private static readonly string MortalLoremasterClass3AlignGuid = "080c8501-d23a-425e-9065-f9ae32d578f4";
        private const string MortalLoremasterClass4Align = "MortalLoremasterClass4Align";
        private static readonly string MortalLoremasterClass4AlignGuid = "a2804810-b696-4c8a-9b76-a4758ac91a62";
        private const string MortalLoremasterClass5Align = "MortalLoremasterClass5Align";
        private static readonly string MortalLoremasterClass5AlignGuid = "953505bb-12ae-4a9c-ae08-bca6d8536192";
        private const string MortalLoremasterClass6Align = "MortalLoremasterClass6Align";
        private static readonly string MortalLoremasterClass6AlignGuid = "6e811ba4-435a-4318-b6d3-f4eb5d818b74";
        private const string MortalLoremasterClass7Align = "MortalLoremasterClass7Align";
        private static readonly string MortalLoremasterClass7AlignGuid = "73c3851b-4750-4a3d-aade-3b78dd13d973";
        private const string MortalLoremasterClass8Align = "MortalLoremasterClass8Align";
        private static readonly string MortalLoremasterClass8AlignGuid = "e7e3acc4-cef5-4b37-899f-2bf2af158e4c";
        private const string MortalLoremasterClass9Align = "MortalLoremasterClass9Align";
        private static readonly string MortalLoremasterClass9AlignGuid = "44cf56c3-7203-4b99-a25c-ec33da418987";
        private const string MortalLoremasterClass10Align = "MortalLoremasterClass10Align";
        private static readonly string MortalLoremasterClass10AlignGuid = "a3f0da9e-d593-45b7-a68d-be17ced6eb52";
        private const string MortalMagusClass0Align = "MortalMagusClass0Align";
        private static readonly string MortalMagusClass0AlignGuid = "ceb142c1-eac5-42c2-b763-930a1680a377";
        internal const string MortalMagusClass0AlignDisplayName = "MortalMagusClass0Align.Name";
        private const string MortalMagusClass0AlignDescription = "MortalMagusClass0Align.Description";
        private const string MortalMagusClass2Align = "MortalMagusClass2Align";
        private static readonly string MortalMagusClass2AlignGuid = "f54a2729-0f24-4359-8671-d694cef20fa7";
        private const string MortalMagusClass3Align = "MortalMagusClass3Align";
        private static readonly string MortalMagusClass3AlignGuid = "9b67173e-6a82-4df4-98aa-06644e25961c";
        private const string MortalMagusClass4Align = "MortalMagusClass4Align";
        private static readonly string MortalMagusClass4AlignGuid = "1b2754ce-2944-4428-b19f-47ac5ac5860e";
        private const string MortalMagusClass5Align = "MortalMagusClass5Align";
        private static readonly string MortalMagusClass5AlignGuid = "bf3d78ad-ab8c-40bc-badc-372a17233b91";
        private const string MortalMagusClass6Align = "MortalMagusClass6Align";
        private static readonly string MortalMagusClass6AlignGuid = "dac786e0-f5d8-42ef-85f9-486cfc14baa5";
        private const string MortalMagusClass7Align = "MortalMagusClass7Align";
        private static readonly string MortalMagusClass7AlignGuid = "b6a9d5c4-ab15-40d4-877b-769eca11144f";
        private const string MortalMagusClass8Align = "MortalMagusClass8Align";
        private static readonly string MortalMagusClass8AlignGuid = "a0eedad8-1655-4087-b74e-05a34a12671a";
        private const string MortalMagusClass9Align = "MortalMagusClass9Align";
        private static readonly string MortalMagusClass9AlignGuid = "2a57aa6a-ceb6-4c8c-80ba-1bc1fc821b98";
        private const string MortalMagusClass10Align = "MortalMagusClass10Align";
        private static readonly string MortalMagusClass10AlignGuid = "cc5483ae-29b0-41e5-9d5b-4e8c27fc0aeb";
        private const string MortalMonkClass0Align = "MortalMonkClass0Align";
        private static readonly string MortalMonkClass0AlignGuid = "51ccbfc8-dd8f-49d4-9de2-9ae5b44898da";
        internal const string MortalMonkClass0AlignDisplayName = "MortalMonkClass0Align.Name";
        private const string MortalMonkClass0AlignDescription = "MortalMonkClass0Align.Description";
        private const string MortalMonkClass2Align = "MortalMonkClass2Align";
        private static readonly string MortalMonkClass2AlignGuid = "f3332a2f-446b-485c-bab6-fe1f9d81cc6e";
        private const string MortalMonkClass3Align = "MortalMonkClass3Align";
        private static readonly string MortalMonkClass3AlignGuid = "89cf1621-8eed-4ced-93ff-df33c3485e9a";
        private const string MortalMonkClass4Align = "MortalMonkClass4Align";
        private static readonly string MortalMonkClass4AlignGuid = "2cd55ed3-d983-4f95-b342-131d8da7fbbb";
        private const string MortalMonkClass5Align = "MortalMonkClass5Align";
        private static readonly string MortalMonkClass5AlignGuid = "0dd64166-b6a4-4d55-ad39-8f1f63832833";
        private const string MortalMonkClass6Align = "MortalMonkClass6Align";
        private static readonly string MortalMonkClass6AlignGuid = "58586a6c-122c-4048-80bd-aa6fb6c64a96";
        private const string MortalMonkClass7Align = "MortalMonkClass7Align";
        private static readonly string MortalMonkClass7AlignGuid = "d71eb429-3cdd-444e-ab31-ff60f433e7fb";
        private const string MortalMonkClass8Align = "MortalMonkClass8Align";
        private static readonly string MortalMonkClass8AlignGuid = "07468b2c-04e5-422a-beae-debc1bb834f6";
        private const string MortalMonkClass9Align = "MortalMonkClass9Align";
        private static readonly string MortalMonkClass9AlignGuid = "3edb9d8a-8e3e-4495-afec-09f4f573646a";
        private const string MortalMonkClass10Align = "MortalMonkClass10Align";
        private static readonly string MortalMonkClass10AlignGuid = "41d6c10a-da77-46af-b04d-d6ba99673b09";
        private const string MortalMysticTheurgeClass0Align = "MortalMysticTheurgeClass0Align";
        private static readonly string MortalMysticTheurgeClass0AlignGuid = "10d384a9-7b91-4a35-a421-61da1efe0004";
        internal const string MortalMysticTheurgeClass0AlignDisplayName = "MortalMysticTheurgeClass0Align.Name";
        private const string MortalMysticTheurgeClass0AlignDescription = "MortalMysticTheurgeClass0Align.Description";
        private const string MortalMysticTheurgeClass2Align = "MortalMysticTheurgeClass2Align";
        private static readonly string MortalMysticTheurgeClass2AlignGuid = "29f0ffda-f618-45d3-8ac6-3a0ddb0deba1";
        private const string MortalMysticTheurgeClass3Align = "MortalMysticTheurgeClass3Align";
        private static readonly string MortalMysticTheurgeClass3AlignGuid = "8aa9e6c2-20ee-4452-8e27-f1340bfbfe4f";
        private const string MortalMysticTheurgeClass4Align = "MortalMysticTheurgeClass4Align";
        private static readonly string MortalMysticTheurgeClass4AlignGuid = "63f015fe-66da-4635-be20-2d355ae1ef9f";
        private const string MortalMysticTheurgeClass5Align = "MortalMysticTheurgeClass5Align";
        private static readonly string MortalMysticTheurgeClass5AlignGuid = "fab1dbf3-1171-4627-8a96-f119d2e5879c";
        private const string MortalMysticTheurgeClass6Align = "MortalMysticTheurgeClass6Align";
        private static readonly string MortalMysticTheurgeClass6AlignGuid = "ce844d45-6b0b-4bfb-b59c-8666079114d3";
        private const string MortalMysticTheurgeClass7Align = "MortalMysticTheurgeClass7Align";
        private static readonly string MortalMysticTheurgeClass7AlignGuid = "5f3e1ebc-3487-4e6f-973e-ff99b366d3fb";
        private const string MortalMysticTheurgeClass8Align = "MortalMysticTheurgeClass8Align";
        private static readonly string MortalMysticTheurgeClass8AlignGuid = "3431ad1b-3340-49a3-8e2c-3ad0a593de89";
        private const string MortalMysticTheurgeClass9Align = "MortalMysticTheurgeClass9Align";
        private static readonly string MortalMysticTheurgeClass9AlignGuid = "479a5a7b-24ed-4c1f-8005-e3de90d4b882";
        private const string MortalMysticTheurgeClass10Align = "MortalMysticTheurgeClass10Align";
        private static readonly string MortalMysticTheurgeClass10AlignGuid = "c3507f15-9954-4556-a261-01333d79d293";
        private const string MortalOracleClass0Align = "MortalOracleClass0Align";
        private static readonly string MortalOracleClass0AlignGuid = "67845977-eb45-4721-96a0-0fc0046b9964";
        internal const string MortalOracleClass0AlignDisplayName = "MortalOracleClass0Align.Name";
        private const string MortalOracleClass0AlignDescription = "MortalOracleClass0Align.Description";
        private const string MortalOracleClass2Align = "MortalOracleClass2Align";
        private static readonly string MortalOracleClass2AlignGuid = "99e864ab-8399-4c84-b5ef-3ce1d191aa95";
        private const string MortalOracleClass3Align = "MortalOracleClass3Align";
        private static readonly string MortalOracleClass3AlignGuid = "0e8b6707-b763-43e6-a995-a8c642d1f5b1";
        private const string MortalOracleClass4Align = "MortalOracleClass4Align";
        private static readonly string MortalOracleClass4AlignGuid = "30dff1bf-40f1-4877-a4ad-ad902c913705";
        private const string MortalOracleClass5Align = "MortalOracleClass5Align";
        private static readonly string MortalOracleClass5AlignGuid = "5dbffea6-b7c8-4c29-bcb6-3bbbbc7372f0";
        private const string MortalOracleClass6Align = "MortalOracleClass6Align";
        private static readonly string MortalOracleClass6AlignGuid = "e4209378-57a1-4031-a40d-5d04d6a57fc6";
        private const string MortalOracleClass7Align = "MortalOracleClass7Align";
        private static readonly string MortalOracleClass7AlignGuid = "b64bfbd7-877b-42c6-9538-06e14816ddaa";
        private const string MortalOracleClass8Align = "MortalOracleClass8Align";
        private static readonly string MortalOracleClass8AlignGuid = "30ef4ffa-0d7c-4c45-ac9e-0e5141341444";
        private const string MortalOracleClass9Align = "MortalOracleClass9Align";
        private static readonly string MortalOracleClass9AlignGuid = "cc8349a0-62df-4639-96fb-6db62dcb862f";
        private const string MortalOracleClass10Align = "MortalOracleClass10Align";
        private static readonly string MortalOracleClass10AlignGuid = "90442186-4635-4942-b69d-232e1664d6ad";
        private const string MortalPaladinClass0Align = "MortalPaladinClass0Align";
        private static readonly string MortalPaladinClass0AlignGuid = "e0be64d8-aa71-4689-9344-665369b8ff2b";
        internal const string MortalPaladinClass0AlignDisplayName = "MortalPaladinClass0Align.Name";
        private const string MortalPaladinClass0AlignDescription = "MortalPaladinClass0Align.Description";
        private const string MortalPaladinClass2Align = "MortalPaladinClass2Align";
        private static readonly string MortalPaladinClass2AlignGuid = "d2a7fe98-0ee1-45b9-941a-9a0cd529f7b8";
        private const string MortalPaladinClass3Align = "MortalPaladinClass3Align";
        private static readonly string MortalPaladinClass3AlignGuid = "9a1abd34-c436-4cfc-abb7-3a3ad641932c";
        private const string MortalPaladinClass4Align = "MortalPaladinClass4Align";
        private static readonly string MortalPaladinClass4AlignGuid = "64bd1b27-8337-40b8-a91f-72623ed999ef";
        private const string MortalPaladinClass5Align = "MortalPaladinClass5Align";
        private static readonly string MortalPaladinClass5AlignGuid = "bbe98ddf-bda6-43ac-84e1-487dd2cb584d";
        private const string MortalPaladinClass6Align = "MortalPaladinClass6Align";
        private static readonly string MortalPaladinClass6AlignGuid = "20877a37-3380-4cac-a7a4-4178d8fc815f";
        private const string MortalPaladinClass7Align = "MortalPaladinClass7Align";
        private static readonly string MortalPaladinClass7AlignGuid = "0f236bde-4f8f-423f-8022-855227de0db8";
        private const string MortalPaladinClass8Align = "MortalPaladinClass8Align";
        private static readonly string MortalPaladinClass8AlignGuid = "51ab0535-b98f-4733-b9be-b8fefca1c70d";
        private const string MortalPaladinClass9Align = "MortalPaladinClass9Align";
        private static readonly string MortalPaladinClass9AlignGuid = "31ca728f-a042-4c4d-ae7e-b2b1292ab3a0";
        private const string MortalPaladinClass10Align = "MortalPaladinClass10Align";
        private static readonly string MortalPaladinClass10AlignGuid = "cb999378-d614-4f7c-ab4d-d393c959b77e";
        private const string MortalRangerClass0Align = "MortalRangerClass0Align";
        private static readonly string MortalRangerClass0AlignGuid = "a9d36a62-fd19-4ff9-81ab-a38b2b9f9006";
        internal const string MortalRangerClass0AlignDisplayName = "MortalRangerClass0Align.Name";
        private const string MortalRangerClass0AlignDescription = "MortalRangerClass0Align.Description";
        private const string MortalRangerClass2Align = "MortalRangerClass2Align";
        private static readonly string MortalRangerClass2AlignGuid = "89006c62-344f-4f94-83bb-5137977df038";
        private const string MortalRangerClass3Align = "MortalRangerClass3Align";
        private static readonly string MortalRangerClass3AlignGuid = "d15907df-a5a3-4ca5-b962-9010d4897905";
        private const string MortalRangerClass4Align = "MortalRangerClass4Align";
        private static readonly string MortalRangerClass4AlignGuid = "f2e0287e-baf5-4eff-9fd4-78a0542f0860";
        private const string MortalRangerClass5Align = "MortalRangerClass5Align";
        private static readonly string MortalRangerClass5AlignGuid = "c7e58f01-084a-4ef0-87fa-02db0e967887";
        private const string MortalRangerClass6Align = "MortalRangerClass6Align";
        private static readonly string MortalRangerClass6AlignGuid = "f825d99c-609b-4cf8-9f30-507bf72e804b";
        private const string MortalRangerClass7Align = "MortalRangerClass7Align";
        private static readonly string MortalRangerClass7AlignGuid = "b6e260fe-0ad8-4294-bbcb-c61b12e56c23";
        private const string MortalRangerClass8Align = "MortalRangerClass8Align";
        private static readonly string MortalRangerClass8AlignGuid = "cd52e5c1-86a5-4221-a596-4f7ab8047e57";
        private const string MortalRangerClass9Align = "MortalRangerClass9Align";
        private static readonly string MortalRangerClass9AlignGuid = "1cc4ed83-bd99-4831-9df7-621d438184fb";
        private const string MortalRangerClass10Align = "MortalRangerClass10Align";
        private static readonly string MortalRangerClass10AlignGuid = "e9ea2e42-d905-45d8-bfcc-7d19be48428b";
        private const string MortalRogueClass0Align = "MortalRogueClass0Align";
        private static readonly string MortalRogueClass0AlignGuid = "1bf22f6c-83e6-4280-8c48-ff3cde62ff38";
        internal const string MortalRogueClass0AlignDisplayName = "MortalRogueClass0Align.Name";
        private const string MortalRogueClass0AlignDescription = "MortalRogueClass0Align.Description";
        private const string MortalRogueClass2Align = "MortalRogueClass2Align";
        private static readonly string MortalRogueClass2AlignGuid = "1fcaa123-ac43-452f-a9f6-a2583f9190ef";
        private const string MortalRogueClass3Align = "MortalRogueClass3Align";
        private static readonly string MortalRogueClass3AlignGuid = "8b4a34f9-89fa-40c7-9b2b-63c814ba41b9";
        private const string MortalRogueClass4Align = "MortalRogueClass4Align";
        private static readonly string MortalRogueClass4AlignGuid = "d0fc65db-ca43-4b40-80d6-0feb816b2c1a";
        private const string MortalRogueClass5Align = "MortalRogueClass5Align";
        private static readonly string MortalRogueClass5AlignGuid = "082963fc-8059-4b8d-a005-1404262070d6";
        private const string MortalRogueClass6Align = "MortalRogueClass6Align";
        private static readonly string MortalRogueClass6AlignGuid = "0f158f0e-1eaa-446e-af09-113b1203f995";
        private const string MortalRogueClass7Align = "MortalRogueClass7Align";
        private static readonly string MortalRogueClass7AlignGuid = "a8931fd8-83fb-4b48-bd47-009d5d578794";
        private const string MortalRogueClass8Align = "MortalRogueClass8Align";
        private static readonly string MortalRogueClass8AlignGuid = "daeb8a4e-222b-49e0-a249-319c01ad320f";
        private const string MortalRogueClass9Align = "MortalRogueClass9Align";
        private static readonly string MortalRogueClass9AlignGuid = "68d0a7fa-1811-44e3-96a6-d929bcea11df";
        private const string MortalRogueClass10Align = "MortalRogueClass10Align";
        private static readonly string MortalRogueClass10AlignGuid = "3fdc5216-7fe1-4ce7-a656-e7dfbe842fde";
        private const string MortalShamanClass0Align = "MortalShamanClass0Align";
        private static readonly string MortalShamanClass0AlignGuid = "a96eea15-c259-49c3-b92f-2c62724fa28a";
        internal const string MortalShamanClass0AlignDisplayName = "MortalShamanClass0Align.Name";
        private const string MortalShamanClass0AlignDescription = "MortalShamanClass0Align.Description";
        private const string MortalShamanClass2Align = "MortalShamanClass2Align";
        private static readonly string MortalShamanClass2AlignGuid = "8c340e97-19d8-4f39-8508-39d987e72549";
        private const string MortalShamanClass3Align = "MortalShamanClass3Align";
        private static readonly string MortalShamanClass3AlignGuid = "d2e38cb5-8bb4-403c-b60c-7f18dc941d18";
        private const string MortalShamanClass4Align = "MortalShamanClass4Align";
        private static readonly string MortalShamanClass4AlignGuid = "9a3297a5-167c-44d9-b817-24d08c6837d9";
        private const string MortalShamanClass5Align = "MortalShamanClass5Align";
        private static readonly string MortalShamanClass5AlignGuid = "a1ec3f74-57f4-45ee-8b75-4e916908082d";
        private const string MortalShamanClass6Align = "MortalShamanClass6Align";
        private static readonly string MortalShamanClass6AlignGuid = "e201b122-64ff-4646-a68e-9f1c1532da5f";
        private const string MortalShamanClass7Align = "MortalShamanClass7Align";
        private static readonly string MortalShamanClass7AlignGuid = "118524ca-8613-42e6-aea9-0375a9abd2a4";
        private const string MortalShamanClass8Align = "MortalShamanClass8Align";
        private static readonly string MortalShamanClass8AlignGuid = "1723dc05-ac18-438d-9ee2-d97e50352ec2";
        private const string MortalShamanClass9Align = "MortalShamanClass9Align";
        private static readonly string MortalShamanClass9AlignGuid = "6f55f2c3-d8f0-4d57-ba23-107e24a0bda5";
        private const string MortalShamanClass10Align = "MortalShamanClass10Align";
        private static readonly string MortalShamanClass10AlignGuid = "7e8f3bfd-9ed3-4c98-bbab-4c59f4ff56ce";
        private const string MortalShifterClass0Align = "MortalShifterClass0Align";
        private static readonly string MortalShifterClass0AlignGuid = "6bc1312a-67dd-4baf-a47b-65304f242797";
        internal const string MortalShifterClass0AlignDisplayName = "MortalShifterClass0Align.Name";
        private const string MortalShifterClass0AlignDescription = "MortalShifterClass0Align.Description";
        private const string MortalShifterClass2Align = "MortalShifterClass2Align";
        private static readonly string MortalShifterClass2AlignGuid = "bb50e59f-14cf-4469-9b45-5d968e51f7dc";
        private const string MortalShifterClass3Align = "MortalShifterClass3Align";
        private static readonly string MortalShifterClass3AlignGuid = "c5ca630a-64d2-4e20-b1a8-b24c5b6c7af1";
        private const string MortalShifterClass4Align = "MortalShifterClass4Align";
        private static readonly string MortalShifterClass4AlignGuid = "e5dc25b2-9f79-4014-b3a8-2096cae8d5f7";
        private const string MortalShifterClass5Align = "MortalShifterClass5Align";
        private static readonly string MortalShifterClass5AlignGuid = "9fc9954c-ff01-47b9-a690-44bb5cfa9387";
        private const string MortalShifterClass6Align = "MortalShifterClass6Align";
        private static readonly string MortalShifterClass6AlignGuid = "586e7e34-9c9c-42da-b0cc-da467afdb1c9";
        private const string MortalShifterClass7Align = "MortalShifterClass7Align";
        private static readonly string MortalShifterClass7AlignGuid = "893672e7-a4ea-4806-b9fe-46a648315d89";
        private const string MortalShifterClass8Align = "MortalShifterClass8Align";
        private static readonly string MortalShifterClass8AlignGuid = "76b87ffc-6498-4e4c-8c31-4f7a3efcdf02";
        private const string MortalShifterClass9Align = "MortalShifterClass9Align";
        private static readonly string MortalShifterClass9AlignGuid = "2dac3f1a-5d5d-4c53-b248-e2b9bbd4ec32";
        private const string MortalShifterClass10Align = "MortalShifterClass10Align";
        private static readonly string MortalShifterClass10AlignGuid = "1ca01714-c2d5-4180-b45d-460279e9299d";
        private const string MortalSkaldClass0Align = "MortalSkaldClass0Align";
        private static readonly string MortalSkaldClass0AlignGuid = "0d5e81ca-c92e-45cb-8cd0-a38ede465a5f";
        internal const string MortalSkaldClass0AlignDisplayName = "MortalSkaldClass0Align.Name";
        private const string MortalSkaldClass0AlignDescription = "MortalSkaldClass0Align.Description";
        private const string MortalSkaldClass2Align = "MortalSkaldClass2Align";
        private static readonly string MortalSkaldClass2AlignGuid = "0a6124ce-7716-44ad-ac5c-b25a5ae05774";
        private const string MortalSkaldClass3Align = "MortalSkaldClass3Align";
        private static readonly string MortalSkaldClass3AlignGuid = "20c68765-7b0e-42a6-9f37-ac154cce4216";
        private const string MortalSkaldClass4Align = "MortalSkaldClass4Align";
        private static readonly string MortalSkaldClass4AlignGuid = "e7b7db05-fb1e-48f5-a2e6-c741d67baee6";
        private const string MortalSkaldClass5Align = "MortalSkaldClass5Align";
        private static readonly string MortalSkaldClass5AlignGuid = "9471686f-49ad-468b-9477-30f2429432e2";
        private const string MortalSkaldClass6Align = "MortalSkaldClass6Align";
        private static readonly string MortalSkaldClass6AlignGuid = "719c2c5c-97f9-4e42-a562-e6b291d69ce1";
        private const string MortalSkaldClass7Align = "MortalSkaldClass7Align";
        private static readonly string MortalSkaldClass7AlignGuid = "b530aced-ed4a-4215-baeb-5820abb39260";
        private const string MortalSkaldClass8Align = "MortalSkaldClass8Align";
        private static readonly string MortalSkaldClass8AlignGuid = "78d8bee1-146c-4d96-a89b-dd95d2402819";
        private const string MortalSkaldClass9Align = "MortalSkaldClass9Align";
        private static readonly string MortalSkaldClass9AlignGuid = "c1627eef-f5de-401b-97e5-ff98392dd39f";
        private const string MortalSkaldClass10Align = "MortalSkaldClass10Align";
        private static readonly string MortalSkaldClass10AlignGuid = "92d0becd-e564-4b69-af22-cf360f397a19";
        private const string MortalSlayerClass0Align = "MortalSlayerClass0Align";
        private static readonly string MortalSlayerClass0AlignGuid = "f7675ba1-4ec6-4dab-8d36-8306917c2e8a";
        internal const string MortalSlayerClass0AlignDisplayName = "MortalSlayerClass0Align.Name";
        private const string MortalSlayerClass0AlignDescription = "MortalSlayerClass0Align.Description";
        private const string MortalSlayerClass2Align = "MortalSlayerClass2Align";
        private static readonly string MortalSlayerClass2AlignGuid = "df9a62a2-37d8-4015-98f8-2cedfbad667a";
        private const string MortalSlayerClass3Align = "MortalSlayerClass3Align";
        private static readonly string MortalSlayerClass3AlignGuid = "e2a09a5f-3d99-48c3-945a-45932530cdb3";
        private const string MortalSlayerClass4Align = "MortalSlayerClass4Align";
        private static readonly string MortalSlayerClass4AlignGuid = "b7e87e76-2e34-45b9-8d23-9a985529285e";
        private const string MortalSlayerClass5Align = "MortalSlayerClass5Align";
        private static readonly string MortalSlayerClass5AlignGuid = "2057e545-b68c-44ce-ad14-b0974675cda4";
        private const string MortalSlayerClass6Align = "MortalSlayerClass6Align";
        private static readonly string MortalSlayerClass6AlignGuid = "7d88517e-ad65-4b42-9d9a-b44157bab429";
        private const string MortalSlayerClass7Align = "MortalSlayerClass7Align";
        private static readonly string MortalSlayerClass7AlignGuid = "cb8ec610-1a57-4eec-bc08-4be2ce7b2515";
        private const string MortalSlayerClass8Align = "MortalSlayerClass8Align";
        private static readonly string MortalSlayerClass8AlignGuid = "19b0ce84-4aac-4bdf-8a8c-79480c423287";
        private const string MortalSlayerClass9Align = "MortalSlayerClass9Align";
        private static readonly string MortalSlayerClass9AlignGuid = "d74a35f8-5531-43f5-804e-38c697e92347";
        private const string MortalSlayerClass10Align = "MortalSlayerClass10Align";
        private static readonly string MortalSlayerClass10AlignGuid = "241e1c13-0397-4aaa-b07f-49707595eb9d";
        private const string MortalSorcererClass0Align = "MortalSorcererClass0Align";
        private static readonly string MortalSorcererClass0AlignGuid = "3c088c2d-0abe-44cb-8890-37d600eeded0";
        internal const string MortalSorcererClass0AlignDisplayName = "MortalSorcererClass0Align.Name";
        private const string MortalSorcererClass0AlignDescription = "MortalSorcererClass0Align.Description";
        private const string MortalSorcererClass2Align = "MortalSorcererClass2Align";
        private static readonly string MortalSorcererClass2AlignGuid = "6c54bfaf-9c29-485c-9660-bb6cb2b50270";
        private const string MortalSorcererClass3Align = "MortalSorcererClass3Align";
        private static readonly string MortalSorcererClass3AlignGuid = "02bd6ddd-46c8-469d-b5ae-b50e5cf6a2f5";
        private const string MortalSorcererClass4Align = "MortalSorcererClass4Align";
        private static readonly string MortalSorcererClass4AlignGuid = "26b514c3-cfad-484d-94c1-aac08c3cf901";
        private const string MortalSorcererClass5Align = "MortalSorcererClass5Align";
        private static readonly string MortalSorcererClass5AlignGuid = "9ea9c1c7-2383-464f-9c7e-9c01b37da3bd";
        private const string MortalSorcererClass6Align = "MortalSorcererClass6Align";
        private static readonly string MortalSorcererClass6AlignGuid = "bc776f5e-1cda-41ef-997a-92f3ecd66c5e";
        private const string MortalSorcererClass7Align = "MortalSorcererClass7Align";
        private static readonly string MortalSorcererClass7AlignGuid = "c3cff14c-dc12-4a45-b8dc-64dee1a70ec8";
        private const string MortalSorcererClass8Align = "MortalSorcererClass8Align";
        private static readonly string MortalSorcererClass8AlignGuid = "468e77d8-8d63-4014-b4c3-12f10bc1da30";
        private const string MortalSorcererClass9Align = "MortalSorcererClass9Align";
        private static readonly string MortalSorcererClass9AlignGuid = "232504c8-15c1-4dec-8184-82d2f609fb71";
        private const string MortalSorcererClass10Align = "MortalSorcererClass10Align";
        private static readonly string MortalSorcererClass10AlignGuid = "f1094632-d4be-4a44-95f3-50b9c658a832";
        private const string MortalStalwartDefenderClass0Align = "MortalStalwartDefenderClass0Align";
        private static readonly string MortalStalwartDefenderClass0AlignGuid = "cbff0931-1b1e-4339-b9f6-2c5ecf8e76c2";
        internal const string MortalStalwartDefenderClass0AlignDisplayName = "MortalStalwartDefenderClass0Align.Name";
        private const string MortalStalwartDefenderClass0AlignDescription = "MortalStalwartDefenderClass0Align.Description";
        private const string MortalStalwartDefenderClass2Align = "MortalStalwartDefenderClass2Align";
        private static readonly string MortalStalwartDefenderClass2AlignGuid = "a71bc15c-603a-4e4d-bb3d-989b336f1480";
        private const string MortalStalwartDefenderClass3Align = "MortalStalwartDefenderClass3Align";
        private static readonly string MortalStalwartDefenderClass3AlignGuid = "d5066523-9ac7-4554-af4d-a98de92edfaa";
        private const string MortalStalwartDefenderClass4Align = "MortalStalwartDefenderClass4Align";
        private static readonly string MortalStalwartDefenderClass4AlignGuid = "d6a42cde-3a1f-4c3a-9cda-1b2345a9ced7";
        private const string MortalStalwartDefenderClass5Align = "MortalStalwartDefenderClass5Align";
        private static readonly string MortalStalwartDefenderClass5AlignGuid = "17e1e391-076b-47d0-94f7-08da8e3388f7";
        private const string MortalStalwartDefenderClass6Align = "MortalStalwartDefenderClass6Align";
        private static readonly string MortalStalwartDefenderClass6AlignGuid = "4f4ed973-81dc-472c-a8e4-8e5851751569";
        private const string MortalStalwartDefenderClass7Align = "MortalStalwartDefenderClass7Align";
        private static readonly string MortalStalwartDefenderClass7AlignGuid = "695c5089-b867-428f-87bd-9eba52d3076b";
        private const string MortalStalwartDefenderClass8Align = "MortalStalwartDefenderClass8Align";
        private static readonly string MortalStalwartDefenderClass8AlignGuid = "7ba818b1-ee24-42d0-abe6-90879e74e54e";
        private const string MortalStalwartDefenderClass9Align = "MortalStalwartDefenderClass9Align";
        private static readonly string MortalStalwartDefenderClass9AlignGuid = "8eeab5fc-1122-47ad-b7d9-271ea9ac9cb7";
        private const string MortalStalwartDefenderClass10Align = "MortalStalwartDefenderClass10Align";
        private static readonly string MortalStalwartDefenderClass10AlignGuid = "3b467445-1681-44a9-b7f6-f5f6b0dd4824";
        private const string MortalStudentOfWarClass0Align = "MortalStudentOfWarClass0Align";
        private static readonly string MortalStudentOfWarClass0AlignGuid = "8fcc6a8d-066e-4b03-84b9-f0e46d492fbe";
        internal const string MortalStudentOfWarClass0AlignDisplayName = "MortalStudentOfWarClass0Align.Name";
        private const string MortalStudentOfWarClass0AlignDescription = "MortalStudentOfWarClass0Align.Description";
        private const string MortalStudentOfWarClass2Align = "MortalStudentOfWarClass2Align";
        private static readonly string MortalStudentOfWarClass2AlignGuid = "17011516-b609-44f3-8495-2b84dc7bbc18";
        private const string MortalStudentOfWarClass3Align = "MortalStudentOfWarClass3Align";
        private static readonly string MortalStudentOfWarClass3AlignGuid = "e15b23c4-8231-421b-992c-bc18e69030c5";
        private const string MortalStudentOfWarClass4Align = "MortalStudentOfWarClass4Align";
        private static readonly string MortalStudentOfWarClass4AlignGuid = "4601bf5c-6b59-418c-815d-a8db820b0f39";
        private const string MortalStudentOfWarClass5Align = "MortalStudentOfWarClass5Align";
        private static readonly string MortalStudentOfWarClass5AlignGuid = "a7e3f7e1-0533-4e61-a3d5-48a8e29fc90d";
        private const string MortalStudentOfWarClass6Align = "MortalStudentOfWarClass6Align";
        private static readonly string MortalStudentOfWarClass6AlignGuid = "70b932ee-b2d3-4274-bca7-9c8867fdb25c";
        private const string MortalStudentOfWarClass7Align = "MortalStudentOfWarClass7Align";
        private static readonly string MortalStudentOfWarClass7AlignGuid = "fda371e1-bdf9-428c-a91d-39f29c2f5c75";
        private const string MortalStudentOfWarClass8Align = "MortalStudentOfWarClass8Align";
        private static readonly string MortalStudentOfWarClass8AlignGuid = "ce95d03c-9953-4445-914a-8f26e0835aa2";
        private const string MortalStudentOfWarClass9Align = "MortalStudentOfWarClass9Align";
        private static readonly string MortalStudentOfWarClass9AlignGuid = "929ebf05-c4d9-4e69-82e5-b66e60f981cd";
        private const string MortalStudentOfWarClass10Align = "MortalStudentOfWarClass10Align";
        private static readonly string MortalStudentOfWarClass10AlignGuid = "72b9830e-89d0-4571-9d3c-a69e64857957";
        private const string MortalSwordlordClass0Align = "MortalSwordlordClass0Align";
        private static readonly string MortalSwordlordClass0AlignGuid = "8675c00f-4719-4424-bd89-746e088801fb";
        internal const string MortalSwordlordClass0AlignDisplayName = "MortalSwordlordClass0Align.Name";
        private const string MortalSwordlordClass0AlignDescription = "MortalSwordlordClass0Align.Description";
        private const string MortalSwordlordClass2Align = "MortalSwordlordClass2Align";
        private static readonly string MortalSwordlordClass2AlignGuid = "d7a0e4e7-94ab-420c-b75d-5d95e314bc2f";
        private const string MortalSwordlordClass3Align = "MortalSwordlordClass3Align";
        private static readonly string MortalSwordlordClass3AlignGuid = "e5f385a9-e183-4e1f-8018-35afbd40a011";
        private const string MortalSwordlordClass4Align = "MortalSwordlordClass4Align";
        private static readonly string MortalSwordlordClass4AlignGuid = "e68ab9ae-2c1c-40a2-873e-4e481e69a2da";
        private const string MortalSwordlordClass5Align = "MortalSwordlordClass5Align";
        private static readonly string MortalSwordlordClass5AlignGuid = "b2eb1a90-6dd6-4001-9502-2a7bc7623c61";
        private const string MortalSwordlordClass6Align = "MortalSwordlordClass6Align";
        private static readonly string MortalSwordlordClass6AlignGuid = "ca914508-6053-4017-adde-ff10b2763d1d";
        private const string MortalSwordlordClass7Align = "MortalSwordlordClass7Align";
        private static readonly string MortalSwordlordClass7AlignGuid = "8d45030b-8165-4539-a2a2-b469ce784bcf";
        private const string MortalSwordlordClass8Align = "MortalSwordlordClass8Align";
        private static readonly string MortalSwordlordClass8AlignGuid = "4b06f028-0591-444c-ade5-a20c65232a61";
        private const string MortalSwordlordClass9Align = "MortalSwordlordClass9Align";
        private static readonly string MortalSwordlordClass9AlignGuid = "34ce4d48-b073-4983-8fa2-ba5399fd7683";
        private const string MortalSwordlordClass10Align = "MortalSwordlordClass10Align";
        private static readonly string MortalSwordlordClass10AlignGuid = "48141b4c-c7e3-4a4b-88d7-bf0949d5be55";
        private const string MortalWarpriestClass0Align = "MortalWarpriestClass0Align";
        private static readonly string MortalWarpriestClass0AlignGuid = "2e3c67de-5498-4bb5-a6dd-3177e3897132";
        internal const string MortalWarpriestClass0AlignDisplayName = "MortalWarpriestClass0Align.Name";
        private const string MortalWarpriestClass0AlignDescription = "MortalWarpriestClass0Align.Description";
        private const string MortalWarpriestClass2Align = "MortalWarpriestClass2Align";
        private static readonly string MortalWarpriestClass2AlignGuid = "167553b7-9252-442e-872a-63d9e3256bcd";
        private const string MortalWarpriestClass3Align = "MortalWarpriestClass3Align";
        private static readonly string MortalWarpriestClass3AlignGuid = "5b6e36f1-0c53-403f-8e64-b728d09f7900";
        private const string MortalWarpriestClass4Align = "MortalWarpriestClass4Align";
        private static readonly string MortalWarpriestClass4AlignGuid = "24e37034-c8b4-462e-bdff-bf48a543bc47";
        private const string MortalWarpriestClass5Align = "MortalWarpriestClass5Align";
        private static readonly string MortalWarpriestClass5AlignGuid = "a11bad51-d645-4ba7-a820-0aabe3e4987c";
        private const string MortalWarpriestClass6Align = "MortalWarpriestClass6Align";
        private static readonly string MortalWarpriestClass6AlignGuid = "3444891b-828b-42b4-ac9c-016a77223ab1";
        private const string MortalWarpriestClass7Align = "MortalWarpriestClass7Align";
        private static readonly string MortalWarpriestClass7AlignGuid = "00963c71-6ea7-40d0-9cd9-cf70192e750d";
        private const string MortalWarpriestClass8Align = "MortalWarpriestClass8Align";
        private static readonly string MortalWarpriestClass8AlignGuid = "35cf8796-e437-4c77-8b6a-93e46565add0";
        private const string MortalWarpriestClass9Align = "MortalWarpriestClass9Align";
        private static readonly string MortalWarpriestClass9AlignGuid = "bdab0dec-5864-4e14-a651-5ec4b324d2a4";
        private const string MortalWarpriestClass10Align = "MortalWarpriestClass10Align";
        private static readonly string MortalWarpriestClass10AlignGuid = "60bcb39d-ab9e-4c67-9d69-2e3eb43a537e";
        private const string MortalWinterWitchClass0Align = "MortalWinterWitchClass0Align";
        private static readonly string MortalWinterWitchClass0AlignGuid = "711c23c9-3a94-4dd2-bb77-feb926d31934";
        internal const string MortalWinterWitchClass0AlignDisplayName = "MortalWinterWitchClass0Align.Name";
        private const string MortalWinterWitchClass0AlignDescription = "MortalWinterWitchClass0Align.Description";
        private const string MortalWinterWitchClass2Align = "MortalWinterWitchClass2Align";
        private static readonly string MortalWinterWitchClass2AlignGuid = "7cd241c2-098b-4963-9730-6f33e8c510b4";
        private const string MortalWinterWitchClass3Align = "MortalWinterWitchClass3Align";
        private static readonly string MortalWinterWitchClass3AlignGuid = "cbb52fb5-3af7-4f96-b332-b97892821915";
        private const string MortalWinterWitchClass4Align = "MortalWinterWitchClass4Align";
        private static readonly string MortalWinterWitchClass4AlignGuid = "d8028da9-f056-4d33-ae56-78275cd69782";
        private const string MortalWinterWitchClass5Align = "MortalWinterWitchClass5Align";
        private static readonly string MortalWinterWitchClass5AlignGuid = "e5bf763e-619f-468c-abd0-72fa90ec0bed";
        private const string MortalWinterWitchClass6Align = "MortalWinterWitchClass6Align";
        private static readonly string MortalWinterWitchClass6AlignGuid = "09db05bb-b1d2-4135-96ec-382dd573a82c";
        private const string MortalWinterWitchClass7Align = "MortalWinterWitchClass7Align";
        private static readonly string MortalWinterWitchClass7AlignGuid = "197fb7b6-27dd-4610-a384-f1fd4df9aed6";
        private const string MortalWinterWitchClass8Align = "MortalWinterWitchClass8Align";
        private static readonly string MortalWinterWitchClass8AlignGuid = "b3cfcb65-291c-41db-a6c7-135d3bbef895";
        private const string MortalWinterWitchClass9Align = "MortalWinterWitchClass9Align";
        private static readonly string MortalWinterWitchClass9AlignGuid = "76dc504e-582d-48b3-bb02-6db29d0a92fb";
        private const string MortalWinterWitchClass10Align = "MortalWinterWitchClass10Align";
        private static readonly string MortalWinterWitchClass10AlignGuid = "27f66947-f837-4c77-9d02-6e6986dea46c";
        private const string MortalWitchClass0Align = "MortalWitchClass0Align";
        private static readonly string MortalWitchClass0AlignGuid = "02dac0de-c146-4758-8f93-873f27bb7530";
        internal const string MortalWitchClass0AlignDisplayName = "MortalWitchClass0Align.Name";
        private const string MortalWitchClass0AlignDescription = "MortalWitchClass0Align.Description";
        private const string MortalWitchClass2Align = "MortalWitchClass2Align";
        private static readonly string MortalWitchClass2AlignGuid = "02d4e991-efe0-4497-b50e-f548a86e0e31";
        private const string MortalWitchClass3Align = "MortalWitchClass3Align";
        private static readonly string MortalWitchClass3AlignGuid = "9f86d058-34e7-4585-b6f1-f08465b02e81";
        private const string MortalWitchClass4Align = "MortalWitchClass4Align";
        private static readonly string MortalWitchClass4AlignGuid = "9ca98237-f89f-4137-8b56-dff778609fb8";
        private const string MortalWitchClass5Align = "MortalWitchClass5Align";
        private static readonly string MortalWitchClass5AlignGuid = "f91eda10-8107-4a37-93d0-2f3b50a91ab8";
        private const string MortalWitchClass6Align = "MortalWitchClass6Align";
        private static readonly string MortalWitchClass6AlignGuid = "b4f023a1-30ff-4732-949b-e3ce36b8c08b";
        private const string MortalWitchClass7Align = "MortalWitchClass7Align";
        private static readonly string MortalWitchClass7AlignGuid = "62011537-3ecb-44a4-953e-bd1daa5e5270";
        private const string MortalWitchClass8Align = "MortalWitchClass8Align";
        private static readonly string MortalWitchClass8AlignGuid = "dd90c21c-748f-49b9-900b-ad36bee5d120";
        private const string MortalWitchClass9Align = "MortalWitchClass9Align";
        private static readonly string MortalWitchClass9AlignGuid = "2857ebfb-5120-40ef-bb74-ea1072ae4756";
        private const string MortalWitchClass10Align = "MortalWitchClass10Align";
        private static readonly string MortalWitchClass10AlignGuid = "2a320a2e-002b-49f7-b5ee-89a9b408f79c";
        private const string MortalWizardClass0Align = "MortalWizardClass0Align";
        private static readonly string MortalWizardClass0AlignGuid = "e7dfb602-3612-412a-856c-72f759fd6dcb";
        internal const string MortalWizardClass0AlignDisplayName = "MortalWizardClass0Align.Name";
        private const string MortalWizardClass0AlignDescription = "MortalWizardClass0Align.Description";
        private const string MortalWizardClass2Align = "MortalWizardClass2Align";
        private static readonly string MortalWizardClass2AlignGuid = "c700d373-68b2-41fc-89c0-993e68cf4f98";
        private const string MortalWizardClass3Align = "MortalWizardClass3Align";
        private static readonly string MortalWizardClass3AlignGuid = "a2bdce82-e418-41cb-8150-ad780e350d19";
        private const string MortalWizardClass4Align = "MortalWizardClass4Align";
        private static readonly string MortalWizardClass4AlignGuid = "aa9eedc1-9fc1-4a9c-94bb-d1cdd3b721f3";
        private const string MortalWizardClass5Align = "MortalWizardClass5Align";
        private static readonly string MortalWizardClass5AlignGuid = "5b8cffba-3192-4268-8563-f264c758751d";
        private const string MortalWizardClass6Align = "MortalWizardClass6Align";
        private static readonly string MortalWizardClass6AlignGuid = "c3259f1d-aa38-4d1e-a2ef-da92f9e94c05";
        private const string MortalWizardClass7Align = "MortalWizardClass7Align";
        private static readonly string MortalWizardClass7AlignGuid = "31c63b16-4715-4980-a78c-a0118c0573e6";
        private const string MortalWizardClass8Align = "MortalWizardClass8Align";
        private static readonly string MortalWizardClass8AlignGuid = "9d350e2c-f4a8-429e-b317-a6948e9edf6f";
        private const string MortalWizardClass9Align = "MortalWizardClass9Align";
        private static readonly string MortalWizardClass9AlignGuid = "d4822718-ccc9-496d-8e3a-41f28ad65d78";
        private const string MortalWizardClass10Align = "MortalWizardClass10Align";
        private static readonly string MortalWizardClass10AlignGuid = "f10f3cc1-b14f-4c9f-adf2-e4df5c68dbc7";
        private const string MortalGunslingerClass0Align = "MortalGunslingerClass0Align";
        private static readonly string MortalGunslingerClass0AlignGuid = "03ab3751-b85b-41c6-b71c-8f4baf110453";
        internal const string MortalGunslingerClass0AlignDisplayName = "MortalGunslingerClass0Align.Name";
        private const string MortalGunslingerClass0AlignDescription = "MortalGunslingerClass0Align.Description";
        private const string MortalGunslingerClass2Align = "MortalGunslingerClass2Align";
        private static readonly string MortalGunslingerClass2AlignGuid = "82abefb0-29d0-4af9-86ad-b626ba8a9afa";
        private const string MortalGunslingerClass3Align = "MortalGunslingerClass3Align";
        private static readonly string MortalGunslingerClass3AlignGuid = "4b786cf3-f8cc-4ecc-8e0f-30f271511776";
        private const string MortalGunslingerClass4Align = "MortalGunslingerClass4Align";
        private static readonly string MortalGunslingerClass4AlignGuid = "36cd5e7f-e924-4ee8-954a-f13b18d57bad";
        private const string MortalGunslingerClass5Align = "MortalGunslingerClass5Align";
        private static readonly string MortalGunslingerClass5AlignGuid = "62075f02-20c3-4b41-8684-3e0970e7aa84";
        private const string MortalGunslingerClass6Align = "MortalGunslingerClass6Align";
        private static readonly string MortalGunslingerClass6AlignGuid = "0c83afd7-f589-4175-97cf-19c6739e6344";
        private const string MortalGunslingerClass7Align = "MortalGunslingerClass7Align";
        private static readonly string MortalGunslingerClass7AlignGuid = "b33fb813-e0ed-4eef-974c-1e5dd908510e";
        private const string MortalGunslingerClass8Align = "MortalGunslingerClass8Align";
        private static readonly string MortalGunslingerClass8AlignGuid = "e8f232ee-e0c2-45ed-afcb-0ca0621f7b87";
        private const string MortalGunslingerClass9Align = "MortalGunslingerClass9Align";
        private static readonly string MortalGunslingerClass9AlignGuid = "20b5de28-e52a-4a55-ba1b-e79294f148df";
        private const string MortalGunslingerClass10Align = "MortalGunslingerClass10Align";
        private static readonly string MortalGunslingerClass10AlignGuid = "2f2631fd-831f-45af-b279-cab8bf42c7cc";
        private const string MortalAgentoftheGraveClass0Align = "MortalAgentoftheGraveClass0Align";
        private static readonly string MortalAgentoftheGraveClass0AlignGuid = "d6731685-bac3-470d-85b1-e39fe65a9e15";
        internal const string MortalAgentoftheGraveClass0AlignDisplayName = "MortalAgentoftheGraveClass0Align.Name";
        private const string MortalAgentoftheGraveClass0AlignDescription = "MortalAgentoftheGraveClass0Align.Description";
        private const string MortalAgentoftheGraveClass2Align = "MortalAgentoftheGraveClass2Align";
        private static readonly string MortalAgentoftheGraveClass2AlignGuid = "d2005ec0-cd5f-4bfb-baa9-7136b148ff91";
        private const string MortalAgentoftheGraveClass3Align = "MortalAgentoftheGraveClass3Align";
        private static readonly string MortalAgentoftheGraveClass3AlignGuid = "93b41cd5-ca22-46e9-a3cc-6b94825b4abe";
        private const string MortalAgentoftheGraveClass4Align = "MortalAgentoftheGraveClass4Align";
        private static readonly string MortalAgentoftheGraveClass4AlignGuid = "81a85d87-1646-48a6-b6bd-c9dc68986ac3";
        private const string MortalAgentoftheGraveClass5Align = "MortalAgentoftheGraveClass5Align";
        private static readonly string MortalAgentoftheGraveClass5AlignGuid = "06ff8024-423a-429b-b0b6-9e7801d526a5";
        private const string MortalAgentoftheGraveClass6Align = "MortalAgentoftheGraveClass6Align";
        private static readonly string MortalAgentoftheGraveClass6AlignGuid = "fd69686d-ba0d-43a0-8cb8-c2429df1dae8";
        private const string MortalAgentoftheGraveClass7Align = "MortalAgentoftheGraveClass7Align";
        private static readonly string MortalAgentoftheGraveClass7AlignGuid = "e371ef2d-85e7-4ebd-bd02-32d249b451df";
        private const string MortalAgentoftheGraveClass8Align = "MortalAgentoftheGraveClass8Align";
        private static readonly string MortalAgentoftheGraveClass8AlignGuid = "f07b2e72-68ff-47f5-9251-555e1959c2cd";
        private const string MortalAgentoftheGraveClass9Align = "MortalAgentoftheGraveClass9Align";
        private static readonly string MortalAgentoftheGraveClass9AlignGuid = "7ac6701c-69f9-43f7-9875-30f665e84ef5";
        private const string MortalAgentoftheGraveClass10Align = "MortalAgentoftheGraveClass10Align";
        private static readonly string MortalAgentoftheGraveClass10AlignGuid = "a5e37170-a65f-4f71-bb29-a1114e8d8e1e";
        private const string MortalAnchoriteofDawnClass0Align = "MortalAnchoriteofDawnClass0Align";
        private static readonly string MortalAnchoriteofDawnClass0AlignGuid = "04300870-7b33-4e19-92de-a2ed19c4bc80";
        internal const string MortalAnchoriteofDawnClass0AlignDisplayName = "MortalAnchoriteofDawnClass0Align.Name";
        private const string MortalAnchoriteofDawnClass0AlignDescription = "MortalAnchoriteofDawnClass0Align.Description";
        private const string MortalAnchoriteofDawnClass2Align = "MortalAnchoriteofDawnClass2Align";
        private static readonly string MortalAnchoriteofDawnClass2AlignGuid = "cbe96876-20bb-4f6e-b30d-306739b0a6b3";
        private const string MortalAnchoriteofDawnClass3Align = "MortalAnchoriteofDawnClass3Align";
        private static readonly string MortalAnchoriteofDawnClass3AlignGuid = "2ef7f50d-4812-498b-bdf1-cd89d7c9c2a2";
        private const string MortalAnchoriteofDawnClass4Align = "MortalAnchoriteofDawnClass4Align";
        private static readonly string MortalAnchoriteofDawnClass4AlignGuid = "4e3db579-796f-477b-a39e-1a14dfa1ccd6";
        private const string MortalAnchoriteofDawnClass5Align = "MortalAnchoriteofDawnClass5Align";
        private static readonly string MortalAnchoriteofDawnClass5AlignGuid = "411088d1-a8b6-4c98-8446-eb606a9c5ee7";
        private const string MortalAnchoriteofDawnClass6Align = "MortalAnchoriteofDawnClass6Align";
        private static readonly string MortalAnchoriteofDawnClass6AlignGuid = "d1974e27-f6e5-4e27-8d68-129a37f153a8";
        private const string MortalAnchoriteofDawnClass7Align = "MortalAnchoriteofDawnClass7Align";
        private static readonly string MortalAnchoriteofDawnClass7AlignGuid = "cb17883b-91ac-4856-a10f-5014cf713283";
        private const string MortalAnchoriteofDawnClass8Align = "MortalAnchoriteofDawnClass8Align";
        private static readonly string MortalAnchoriteofDawnClass8AlignGuid = "71b7a74e-70ef-4f8f-8364-85c5623b1cd4";
        private const string MortalAnchoriteofDawnClass9Align = "MortalAnchoriteofDawnClass9Align";
        private static readonly string MortalAnchoriteofDawnClass9AlignGuid = "561d0339-21ca-461d-a32c-49c0add8e359";
        private const string MortalAnchoriteofDawnClass10Align = "MortalAnchoriteofDawnClass10Align";
        private static readonly string MortalAnchoriteofDawnClass10AlignGuid = "f59ea27f-251c-48bf-b1b0-a28b245fa849";
        private const string MortalArcaneAcherClass0Align = "MortalArcaneAcherClass0Align";
        private static readonly string MortalArcaneAcherClass0AlignGuid = "9e90d066-ff49-498e-83e4-2ac07f89870d";
        internal const string MortalArcaneAcherClass0AlignDisplayName = "MortalArcaneAcherClass0Align.Name";
        private const string MortalArcaneAcherClass0AlignDescription = "MortalArcaneAcherClass0Align.Description";
        private const string MortalArcaneAcherClass2Align = "MortalArcaneAcherClass2Align";
        private static readonly string MortalArcaneAcherClass2AlignGuid = "f411d28d-fbda-4e6a-9b60-3c61ca9cfc98";
        private const string MortalArcaneAcherClass3Align = "MortalArcaneAcherClass3Align";
        private static readonly string MortalArcaneAcherClass3AlignGuid = "2d4d61bc-cc2c-411c-9b26-703941b9693b";
        private const string MortalArcaneAcherClass4Align = "MortalArcaneAcherClass4Align";
        private static readonly string MortalArcaneAcherClass4AlignGuid = "afa62b24-a7dc-498a-bd6a-d612843a314e";
        private const string MortalArcaneAcherClass5Align = "MortalArcaneAcherClass5Align";
        private static readonly string MortalArcaneAcherClass5AlignGuid = "264dc525-7008-4cbd-aae4-28c7a7c84bca";
        private const string MortalArcaneAcherClass6Align = "MortalArcaneAcherClass6Align";
        private static readonly string MortalArcaneAcherClass6AlignGuid = "f018718d-d0a6-476b-a74d-0e4359b89274";
        private const string MortalArcaneAcherClass7Align = "MortalArcaneAcherClass7Align";
        private static readonly string MortalArcaneAcherClass7AlignGuid = "760fc961-59af-4ba5-a4fa-ee65707f4a36";
        private const string MortalArcaneAcherClass8Align = "MortalArcaneAcherClass8Align";
        private static readonly string MortalArcaneAcherClass8AlignGuid = "90a1fd52-ab1f-4584-b66a-44b068018d6c";
        private const string MortalArcaneAcherClass9Align = "MortalArcaneAcherClass9Align";
        private static readonly string MortalArcaneAcherClass9AlignGuid = "14eda80b-6e2e-4b78-8de5-f488c4788782";
        private const string MortalArcaneAcherClass10Align = "MortalArcaneAcherClass10Align";
        private static readonly string MortalArcaneAcherClass10AlignGuid = "9f5b57c3-6fcb-4ac5-ac9a-0dcc10369547";
        private const string MortalAsavirClass0Align = "MortalAsavirClass0Align";
        private static readonly string MortalAsavirClass0AlignGuid = "23c3944a-53aa-4231-9dca-075c79c7ed7d";
        internal const string MortalAsavirClass0AlignDisplayName = "MortalAsavirClass0Align.Name";
        private const string MortalAsavirClass0AlignDescription = "MortalAsavirClass0Align.Description";
        private const string MortalAsavirClass2Align = "MortalAsavirClass2Align";
        private static readonly string MortalAsavirClass2AlignGuid = "904b2446-3fa4-4c1e-93a9-6475febd6fc5";
        private const string MortalAsavirClass3Align = "MortalAsavirClass3Align";
        private static readonly string MortalAsavirClass3AlignGuid = "d003642d-4228-4933-9ee1-77604b5416c1";
        private const string MortalAsavirClass4Align = "MortalAsavirClass4Align";
        private static readonly string MortalAsavirClass4AlignGuid = "d653fa44-5ae7-4d86-bb00-56f37b774458";
        private const string MortalAsavirClass5Align = "MortalAsavirClass5Align";
        private static readonly string MortalAsavirClass5AlignGuid = "29bfddde-bf43-4fe9-add3-a6b0916c5db6";
        private const string MortalAsavirClass6Align = "MortalAsavirClass6Align";
        private static readonly string MortalAsavirClass6AlignGuid = "5f9222c1-36f6-4094-a387-ce4742622844";
        private const string MortalAsavirClass7Align = "MortalAsavirClass7Align";
        private static readonly string MortalAsavirClass7AlignGuid = "664151e6-d88a-4b8e-a6d9-cbcc51b4d0e6";
        private const string MortalAsavirClass8Align = "MortalAsavirClass8Align";
        private static readonly string MortalAsavirClass8AlignGuid = "4e5440d3-94f0-4c84-bdc9-deead05889b4";
        private const string MortalAsavirClass9Align = "MortalAsavirClass9Align";
        private static readonly string MortalAsavirClass9AlignGuid = "efcfc86f-bee4-4b58-9537-06afe71b7f3f";
        private const string MortalAsavirClass10Align = "MortalAsavirClass10Align";
        private static readonly string MortalAsavirClass10AlignGuid = "461bf69c-bec1-481a-b174-c49c73fe261d";
        private const string MortalChevalierClass0Align = "MortalChevalierClass0Align";
        private static readonly string MortalChevalierClass0AlignGuid = "b4c6478d-cb28-4014-b66a-d65f240bae40";
        internal const string MortalChevalierClass0AlignDisplayName = "MortalChevalierClass0Align.Name";
        private const string MortalChevalierClass0AlignDescription = "MortalChevalierClass0Align.Description";
        private const string MortalChevalierClass2Align = "MortalChevalierClass2Align";
        private static readonly string MortalChevalierClass2AlignGuid = "f86bbcfe-199c-4d20-ab1c-68a2d6e5961b";
        private const string MortalChevalierClass3Align = "MortalChevalierClass3Align";
        private static readonly string MortalChevalierClass3AlignGuid = "2a0186e0-f0e4-4263-a383-34401fb6b5c6";
        private const string MortalChevalierClass4Align = "MortalChevalierClass4Align";
        private static readonly string MortalChevalierClass4AlignGuid = "20188a12-6d00-4e88-8252-b6c4c84893c5";
        private const string MortalChevalierClass5Align = "MortalChevalierClass5Align";
        private static readonly string MortalChevalierClass5AlignGuid = "1ad4b0c1-5189-47ca-860f-160596c8a2cb";
        private const string MortalChevalierClass6Align = "MortalChevalierClass6Align";
        private static readonly string MortalChevalierClass6AlignGuid = "3080ade1-b888-4dfd-9678-7909f540149d";
        private const string MortalChevalierClass7Align = "MortalChevalierClass7Align";
        private static readonly string MortalChevalierClass7AlignGuid = "44ed088d-91b7-4372-9038-e93c5057ef05";
        private const string MortalChevalierClass8Align = "MortalChevalierClass8Align";
        private static readonly string MortalChevalierClass8AlignGuid = "860148b2-c5dc-4083-8930-b7e928267bd3";
        private const string MortalChevalierClass9Align = "MortalChevalierClass9Align";
        private static readonly string MortalChevalierClass9AlignGuid = "b4108ee5-6b20-4704-9f9b-168d26c789e5";
        private const string MortalChevalierClass10Align = "MortalChevalierClass10Align";
        private static readonly string MortalChevalierClass10AlignGuid = "a3fe0f9a-f76c-483a-b126-c1d3fac352d8";
        private const string MortalCrimsonTemplarClass0Align = "MortalCrimsonTemplarClass0Align";
        private static readonly string MortalCrimsonTemplarClass0AlignGuid = "1895f11d-cb6d-4c06-9996-82a92e604e6a";
        internal const string MortalCrimsonTemplarClass0AlignDisplayName = "MortalCrimsonTemplarClass0Align.Name";
        private const string MortalCrimsonTemplarClass0AlignDescription = "MortalCrimsonTemplarClass0Align.Description";
        private const string MortalCrimsonTemplarClass2Align = "MortalCrimsonTemplarClass2Align";
        private static readonly string MortalCrimsonTemplarClass2AlignGuid = "f641a7fb-4d8f-4347-9f7a-3edcb486e07c";
        private const string MortalCrimsonTemplarClass3Align = "MortalCrimsonTemplarClass3Align";
        private static readonly string MortalCrimsonTemplarClass3AlignGuid = "5722050a-cb40-4632-a3f9-92ca293fb65e";
        private const string MortalCrimsonTemplarClass4Align = "MortalCrimsonTemplarClass4Align";
        private static readonly string MortalCrimsonTemplarClass4AlignGuid = "3173feb4-bb13-428b-855e-df0fe08b6123";
        private const string MortalCrimsonTemplarClass5Align = "MortalCrimsonTemplarClass5Align";
        private static readonly string MortalCrimsonTemplarClass5AlignGuid = "f2f422b4-b968-483e-820c-b9925bbe5cc4";
        private const string MortalCrimsonTemplarClass6Align = "MortalCrimsonTemplarClass6Align";
        private static readonly string MortalCrimsonTemplarClass6AlignGuid = "f1560204-fbc8-4303-96bb-5a1ef391ec61";
        private const string MortalCrimsonTemplarClass7Align = "MortalCrimsonTemplarClass7Align";
        private static readonly string MortalCrimsonTemplarClass7AlignGuid = "e13ff7bf-962c-4ebd-afb6-1fdbe10604d8";
        private const string MortalCrimsonTemplarClass8Align = "MortalCrimsonTemplarClass8Align";
        private static readonly string MortalCrimsonTemplarClass8AlignGuid = "a899c88a-b10d-47da-bfad-2447cc7a7cb7";
        private const string MortalCrimsonTemplarClass9Align = "MortalCrimsonTemplarClass9Align";
        private static readonly string MortalCrimsonTemplarClass9AlignGuid = "374816e9-ede4-4eb8-af06-515710b47226";
        private const string MortalCrimsonTemplarClass10Align = "MortalCrimsonTemplarClass10Align";
        private static readonly string MortalCrimsonTemplarClass10AlignGuid = "d65eb835-7c97-4593-8509-6ce6052c3194";
        private const string MortalDeadeyeDevoteeClass0Align = "MortalDeadeyeDevoteeClass0Align";
        private static readonly string MortalDeadeyeDevoteeClass0AlignGuid = "f8bade08-b473-41a5-a8a6-77a7cd43a05e";
        internal const string MortalDeadeyeDevoteeClass0AlignDisplayName = "MortalDeadeyeDevoteeClass0Align.Name";
        private const string MortalDeadeyeDevoteeClass0AlignDescription = "MortalDeadeyeDevoteeClass0Align.Description";
        private const string MortalDeadeyeDevoteeClass2Align = "MortalDeadeyeDevoteeClass2Align";
        private static readonly string MortalDeadeyeDevoteeClass2AlignGuid = "96a37654-3458-4f80-9c3d-5580ad3a5a6f";
        private const string MortalDeadeyeDevoteeClass3Align = "MortalDeadeyeDevoteeClass3Align";
        private static readonly string MortalDeadeyeDevoteeClass3AlignGuid = "dfd71e5b-56c5-4d11-9897-b2910eea49c6";
        private const string MortalDeadeyeDevoteeClass4Align = "MortalDeadeyeDevoteeClass4Align";
        private static readonly string MortalDeadeyeDevoteeClass4AlignGuid = "e1038536-7cc0-4ea3-9af6-27ca277d4a66";
        private const string MortalDeadeyeDevoteeClass5Align = "MortalDeadeyeDevoteeClass5Align";
        private static readonly string MortalDeadeyeDevoteeClass5AlignGuid = "306740c4-3330-449d-9f56-d61282d3ea34";
        private const string MortalDeadeyeDevoteeClass6Align = "MortalDeadeyeDevoteeClass6Align";
        private static readonly string MortalDeadeyeDevoteeClass6AlignGuid = "ddef2e79-dcbd-4480-a310-08e75985c563";
        private const string MortalDeadeyeDevoteeClass7Align = "MortalDeadeyeDevoteeClass7Align";
        private static readonly string MortalDeadeyeDevoteeClass7AlignGuid = "4adacc80-16bb-4e30-b0fe-ee218221a27b";
        private const string MortalDeadeyeDevoteeClass8Align = "MortalDeadeyeDevoteeClass8Align";
        private static readonly string MortalDeadeyeDevoteeClass8AlignGuid = "02fafd94-52f7-4465-922b-56aef7e40882";
        private const string MortalDeadeyeDevoteeClass9Align = "MortalDeadeyeDevoteeClass9Align";
        private static readonly string MortalDeadeyeDevoteeClass9AlignGuid = "b8dca911-da8c-43ec-b580-51861f5c1a84";
        private const string MortalDeadeyeDevoteeClass10Align = "MortalDeadeyeDevoteeClass10Align";
        private static readonly string MortalDeadeyeDevoteeClass10AlignGuid = "60b59e99-d72c-463b-9c82-725647d19e59";
        private const string MortalDragonFuryClass0Align = "MortalDragonFuryClass0Align";
        private static readonly string MortalDragonFuryClass0AlignGuid = "6eb79f06-59f6-4fb7-bcd1-bb0f5a208cb2";
        internal const string MortalDragonFuryClass0AlignDisplayName = "MortalDragonFuryClass0Align.Name";
        private const string MortalDragonFuryClass0AlignDescription = "MortalDragonFuryClass0Align.Description";
        private const string MortalDragonFuryClass2Align = "MortalDragonFuryClass2Align";
        private static readonly string MortalDragonFuryClass2AlignGuid = "83b5859d-9c44-4af5-add5-af18c01f3b77";
        private const string MortalDragonFuryClass3Align = "MortalDragonFuryClass3Align";
        private static readonly string MortalDragonFuryClass3AlignGuid = "4456350a-99dd-4e0e-b5e8-e34cab8fd83c";
        private const string MortalDragonFuryClass4Align = "MortalDragonFuryClass4Align";
        private static readonly string MortalDragonFuryClass4AlignGuid = "7d655188-fbc9-4137-a506-90155a55d641";
        private const string MortalDragonFuryClass5Align = "MortalDragonFuryClass5Align";
        private static readonly string MortalDragonFuryClass5AlignGuid = "44127166-25ff-4aa7-ab84-9a350ef3bb73";
        private const string MortalDragonFuryClass6Align = "MortalDragonFuryClass6Align";
        private static readonly string MortalDragonFuryClass6AlignGuid = "53763f3d-6055-4e36-bbd9-c649ad6d1bcb";
        private const string MortalDragonFuryClass7Align = "MortalDragonFuryClass7Align";
        private static readonly string MortalDragonFuryClass7AlignGuid = "9a77a0ea-3ccd-4d00-b7e0-e8118288145b";
        private const string MortalDragonFuryClass8Align = "MortalDragonFuryClass8Align";
        private static readonly string MortalDragonFuryClass8AlignGuid = "4777a18f-4f44-4e33-b21d-f7283e952ac4";
        private const string MortalDragonFuryClass9Align = "MortalDragonFuryClass9Align";
        private static readonly string MortalDragonFuryClass9AlignGuid = "1256d0c2-834f-4fab-a4b6-65e5c83ce949";
        private const string MortalDragonFuryClass10Align = "MortalDragonFuryClass10Align";
        private static readonly string MortalDragonFuryClass10AlignGuid = "f17e462e-7edd-41b9-80a0-6d6204e6cc49";
        private const string MortalEsotericKnightClass0Align = "MortalEsotericKnightClass0Align";
        private static readonly string MortalEsotericKnightClass0AlignGuid = "bf921833-da21-44fa-80ee-f8c3a6da6713";
        internal const string MortalEsotericKnightClass0AlignDisplayName = "MortalEsotericKnightClass0Align.Name";
        private const string MortalEsotericKnightClass0AlignDescription = "MortalEsotericKnightClass0Align.Description";
        private const string MortalEsotericKnightClass2Align = "MortalEsotericKnightClass2Align";
        private static readonly string MortalEsotericKnightClass2AlignGuid = "95a946f4-b4db-402a-9e1d-e3bef28f0bbc";
        private const string MortalEsotericKnightClass3Align = "MortalEsotericKnightClass3Align";
        private static readonly string MortalEsotericKnightClass3AlignGuid = "63330008-90e6-4228-8983-81044f37546e";
        private const string MortalEsotericKnightClass4Align = "MortalEsotericKnightClass4Align";
        private static readonly string MortalEsotericKnightClass4AlignGuid = "adb698d5-65af-420f-a270-c32aa463c13c";
        private const string MortalEsotericKnightClass5Align = "MortalEsotericKnightClass5Align";
        private static readonly string MortalEsotericKnightClass5AlignGuid = "3c3e4e8f-54e2-46f0-b5c9-530cee461feb";
        private const string MortalEsotericKnightClass6Align = "MortalEsotericKnightClass6Align";
        private static readonly string MortalEsotericKnightClass6AlignGuid = "e182de11-1c16-4ed4-9a4b-a11390ce47a8";
        private const string MortalEsotericKnightClass7Align = "MortalEsotericKnightClass7Align";
        private static readonly string MortalEsotericKnightClass7AlignGuid = "0ebc17f6-ba41-4602-aac7-1e7c7d1a3470";
        private const string MortalEsotericKnightClass8Align = "MortalEsotericKnightClass8Align";
        private static readonly string MortalEsotericKnightClass8AlignGuid = "f4ae0bde-db4c-4cbb-81c0-da8ede827521";
        private const string MortalEsotericKnightClass9Align = "MortalEsotericKnightClass9Align";
        private static readonly string MortalEsotericKnightClass9AlignGuid = "ea4d3659-5ebb-4ee2-bd48-f30f5a8fa1ab";
        private const string MortalEsotericKnightClass10Align = "MortalEsotericKnightClass10Align";
        private static readonly string MortalEsotericKnightClass10AlignGuid = "825e364c-bbf6-472a-98e9-0bad010467f4";
        private const string MortalExaltedEvangelistClass0Align = "MortalExaltedEvangelistClass0Align";
        private static readonly string MortalExaltedEvangelistClass0AlignGuid = "892cdb6e-e877-4f39-aeea-f98233c82479";
        internal const string MortalExaltedEvangelistClass0AlignDisplayName = "MortalExaltedEvangelistClass0Align.Name";
        private const string MortalExaltedEvangelistClass0AlignDescription = "MortalExaltedEvangelistClass0Align.Description";
        private const string MortalExaltedEvangelistClass2Align = "MortalExaltedEvangelistClass2Align";
        private static readonly string MortalExaltedEvangelistClass2AlignGuid = "9a9c0719-d2ad-4ea2-8f9f-32b4fbdaed17";
        private const string MortalExaltedEvangelistClass3Align = "MortalExaltedEvangelistClass3Align";
        private static readonly string MortalExaltedEvangelistClass3AlignGuid = "755f016b-d8fc-4323-bf55-1fcf66220890";
        private const string MortalExaltedEvangelistClass4Align = "MortalExaltedEvangelistClass4Align";
        private static readonly string MortalExaltedEvangelistClass4AlignGuid = "8001db5c-4e79-4517-bf6b-97cb1b5c92a3";
        private const string MortalExaltedEvangelistClass5Align = "MortalExaltedEvangelistClass5Align";
        private static readonly string MortalExaltedEvangelistClass5AlignGuid = "1dca84b2-3eda-4cae-98d4-5ea9306dcbc7";
        private const string MortalExaltedEvangelistClass6Align = "MortalExaltedEvangelistClass6Align";
        private static readonly string MortalExaltedEvangelistClass6AlignGuid = "d45faff9-d005-459f-b4dd-c258b0f8413f";
        private const string MortalExaltedEvangelistClass7Align = "MortalExaltedEvangelistClass7Align";
        private static readonly string MortalExaltedEvangelistClass7AlignGuid = "bd2171ab-4ba4-408c-bfcd-4757bf3394d0";
        private const string MortalExaltedEvangelistClass8Align = "MortalExaltedEvangelistClass8Align";
        private static readonly string MortalExaltedEvangelistClass8AlignGuid = "16fc76c5-8bfd-430c-8c0b-83be8f99419e";
        private const string MortalExaltedEvangelistClass9Align = "MortalExaltedEvangelistClass9Align";
        private static readonly string MortalExaltedEvangelistClass9AlignGuid = "c2f13099-45ba-42db-9a22-ba122ff9c8a3";
        private const string MortalExaltedEvangelistClass10Align = "MortalExaltedEvangelistClass10Align";
        private static readonly string MortalExaltedEvangelistClass10AlignGuid = "e4b2b820-70d5-4e6c-ab0b-0d2532ba9c37";
        private const string MortalFuriousGuardianClass0Align = "MortalFuriousGuardianClass0Align";
        private static readonly string MortalFuriousGuardianClass0AlignGuid = "cf8576bf-2e0a-429e-a804-c1900c8ddf29";
        internal const string MortalFuriousGuardianClass0AlignDisplayName = "MortalFuriousGuardianClass0Align.Name";
        private const string MortalFuriousGuardianClass0AlignDescription = "MortalFuriousGuardianClass0Align.Description";
        private const string MortalFuriousGuardianClass2Align = "MortalFuriousGuardianClass2Align";
        private static readonly string MortalFuriousGuardianClass2AlignGuid = "554d15b9-f7fe-4528-a859-6997be270349";
        private const string MortalFuriousGuardianClass3Align = "MortalFuriousGuardianClass3Align";
        private static readonly string MortalFuriousGuardianClass3AlignGuid = "453e6168-3305-4fbd-b643-42fdc6f8849d";
        private const string MortalFuriousGuardianClass4Align = "MortalFuriousGuardianClass4Align";
        private static readonly string MortalFuriousGuardianClass4AlignGuid = "dafa0fbf-1d5e-485e-9129-0cc2714fd5d1";
        private const string MortalFuriousGuardianClass5Align = "MortalFuriousGuardianClass5Align";
        private static readonly string MortalFuriousGuardianClass5AlignGuid = "5a7df621-d225-48d8-8fdd-1cd1db159a55";
        private const string MortalFuriousGuardianClass6Align = "MortalFuriousGuardianClass6Align";
        private static readonly string MortalFuriousGuardianClass6AlignGuid = "c6948992-e826-426d-a0bc-56a1d4710a3f";
        private const string MortalFuriousGuardianClass7Align = "MortalFuriousGuardianClass7Align";
        private static readonly string MortalFuriousGuardianClass7AlignGuid = "92f6c3a1-b55c-40db-84de-04346994c695";
        private const string MortalFuriousGuardianClass8Align = "MortalFuriousGuardianClass8Align";
        private static readonly string MortalFuriousGuardianClass8AlignGuid = "ddeb3f41-2f23-495d-a4b2-68ab07ffc616";
        private const string MortalFuriousGuardianClass9Align = "MortalFuriousGuardianClass9Align";
        private static readonly string MortalFuriousGuardianClass9AlignGuid = "79bfffc5-0f25-4ba6-9d03-e7fabfda3180";
        private const string MortalFuriousGuardianClass10Align = "MortalFuriousGuardianClass10Align";
        private static readonly string MortalFuriousGuardianClass10AlignGuid = "79d4a58d-a5ad-4541-9848-c99bf782fcb9";
        private const string MortalHalflingOpportunistClass0Align = "MortalHalflingOpportunistClass0Align";
        private static readonly string MortalHalflingOpportunistClass0AlignGuid = "ac3b8a23-e865-408f-956a-6a06cc303764";
        internal const string MortalHalflingOpportunistClass0AlignDisplayName = "MortalHalflingOpportunistClass0Align.Name";
        private const string MortalHalflingOpportunistClass0AlignDescription = "MortalHalflingOpportunistClass0Align.Description";
        private const string MortalHalflingOpportunistClass2Align = "MortalHalflingOpportunistClass2Align";
        private static readonly string MortalHalflingOpportunistClass2AlignGuid = "0962ad1f-19c2-4630-bb5d-2e9758e3c53d";
        private const string MortalHalflingOpportunistClass3Align = "MortalHalflingOpportunistClass3Align";
        private static readonly string MortalHalflingOpportunistClass3AlignGuid = "12f5ff60-cdcc-453b-9b32-958efd94489a";
        private const string MortalHalflingOpportunistClass4Align = "MortalHalflingOpportunistClass4Align";
        private static readonly string MortalHalflingOpportunistClass4AlignGuid = "65b6edcc-91c4-415d-917f-dac9fab00be7";
        private const string MortalHalflingOpportunistClass5Align = "MortalHalflingOpportunistClass5Align";
        private static readonly string MortalHalflingOpportunistClass5AlignGuid = "935565a6-1817-4fc9-94e6-9616349b16e5";
        private const string MortalHalflingOpportunistClass6Align = "MortalHalflingOpportunistClass6Align";
        private static readonly string MortalHalflingOpportunistClass6AlignGuid = "e89c9ac2-0d38-4359-a889-72f0fa096106";
        private const string MortalHalflingOpportunistClass7Align = "MortalHalflingOpportunistClass7Align";
        private static readonly string MortalHalflingOpportunistClass7AlignGuid = "c01c06c0-b929-4370-9a8d-2e3db600a484";
        private const string MortalHalflingOpportunistClass8Align = "MortalHalflingOpportunistClass8Align";
        private static readonly string MortalHalflingOpportunistClass8AlignGuid = "c317b9bd-8f14-4ba0-91b7-2cc08c57ef69";
        private const string MortalHalflingOpportunistClass9Align = "MortalHalflingOpportunistClass9Align";
        private static readonly string MortalHalflingOpportunistClass9AlignGuid = "f9f3f5de-8af4-486e-9f60-d983b39b3602";
        private const string MortalHalflingOpportunistClass10Align = "MortalHalflingOpportunistClass10Align";
        private static readonly string MortalHalflingOpportunistClass10AlignGuid = "16c957f4-5cb6-41c3-9436-d4c5667b97df";
        private const string MortalHinterlanderClass0Align = "MortalHinterlanderClass0Align";
        private static readonly string MortalHinterlanderClass0AlignGuid = "13880609-066b-4544-9da6-67bc915791df";
        internal const string MortalHinterlanderClass0AlignDisplayName = "MortalHinterlanderClass0Align.Name";
        private const string MortalHinterlanderClass0AlignDescription = "MortalHinterlanderClass0Align.Description";
        private const string MortalHinterlanderClass2Align = "MortalHinterlanderClass2Align";
        private static readonly string MortalHinterlanderClass2AlignGuid = "a5524ad0-e708-458e-b5cf-f9f3f36428ff";
        private const string MortalHinterlanderClass3Align = "MortalHinterlanderClass3Align";
        private static readonly string MortalHinterlanderClass3AlignGuid = "8d40a957-2de2-4c85-a707-6bbcc8b013d4";
        private const string MortalHinterlanderClass4Align = "MortalHinterlanderClass4Align";
        private static readonly string MortalHinterlanderClass4AlignGuid = "96253c73-970e-4188-b5de-19ce9d75f62c";
        private const string MortalHinterlanderClass5Align = "MortalHinterlanderClass5Align";
        private static readonly string MortalHinterlanderClass5AlignGuid = "99116fd7-7ee7-4ddb-aa08-d9f760ccf5d5";
        private const string MortalHinterlanderClass6Align = "MortalHinterlanderClass6Align";
        private static readonly string MortalHinterlanderClass6AlignGuid = "9d56763a-e47d-4308-aaf2-311310404ab1";
        private const string MortalHinterlanderClass7Align = "MortalHinterlanderClass7Align";
        private static readonly string MortalHinterlanderClass7AlignGuid = "8be84abc-bf7f-4c61-8273-c6b96b589473";
        private const string MortalHinterlanderClass8Align = "MortalHinterlanderClass8Align";
        private static readonly string MortalHinterlanderClass8AlignGuid = "13f83441-8137-4f71-a762-e43078f168f0";
        private const string MortalHinterlanderClass9Align = "MortalHinterlanderClass9Align";
        private static readonly string MortalHinterlanderClass9AlignGuid = "e9b86a9f-4e2d-4ab6-97cd-27eae2b68f2c";
        private const string MortalHinterlanderClass10Align = "MortalHinterlanderClass10Align";
        private static readonly string MortalHinterlanderClass10AlignGuid = "3ffdd4e2-a13b-49a4-8acb-c1eb3281cef4";
        private const string MortalHorizonWalkerClass0Align = "MortalHorizonWalkerClass0Align";
        private static readonly string MortalHorizonWalkerClass0AlignGuid = "6fb18773-cee9-4c27-b795-db619db67a4c";
        internal const string MortalHorizonWalkerClass0AlignDisplayName = "MortalHorizonWalkerClass0Align.Name";
        private const string MortalHorizonWalkerClass0AlignDescription = "MortalHorizonWalkerClass0Align.Description";
        private const string MortalHorizonWalkerClass2Align = "MortalHorizonWalkerClass2Align";
        private static readonly string MortalHorizonWalkerClass2AlignGuid = "083e921e-8147-493b-b5b8-7ab584bd8ab3";
        private const string MortalHorizonWalkerClass3Align = "MortalHorizonWalkerClass3Align";
        private static readonly string MortalHorizonWalkerClass3AlignGuid = "dd21c543-17fd-4112-8c88-51166903dc5d";
        private const string MortalHorizonWalkerClass4Align = "MortalHorizonWalkerClass4Align";
        private static readonly string MortalHorizonWalkerClass4AlignGuid = "42b08d63-65d3-4d44-a46a-d613f6afc01b";
        private const string MortalHorizonWalkerClass5Align = "MortalHorizonWalkerClass5Align";
        private static readonly string MortalHorizonWalkerClass5AlignGuid = "95576634-92c5-4f37-bec9-ef0b74be695b";
        private const string MortalHorizonWalkerClass6Align = "MortalHorizonWalkerClass6Align";
        private static readonly string MortalHorizonWalkerClass6AlignGuid = "ae2489dc-3cb8-4c49-931b-6cdc4c1839f7";
        private const string MortalHorizonWalkerClass7Align = "MortalHorizonWalkerClass7Align";
        private static readonly string MortalHorizonWalkerClass7AlignGuid = "484cf28c-048f-4710-9d45-ee6060336165";
        private const string MortalHorizonWalkerClass8Align = "MortalHorizonWalkerClass8Align";
        private static readonly string MortalHorizonWalkerClass8AlignGuid = "1fed6f41-21d9-4e48-ae43-d2bc94b0ae38";
        private const string MortalHorizonWalkerClass9Align = "MortalHorizonWalkerClass9Align";
        private static readonly string MortalHorizonWalkerClass9AlignGuid = "ad63b296-62f4-4574-b55f-c32da0d271fe";
        private const string MortalHorizonWalkerClass10Align = "MortalHorizonWalkerClass10Align";
        private static readonly string MortalHorizonWalkerClass10AlignGuid = "80946e81-2fa3-4d86-b1a7-cee5fb48ebb4";
        private const string MortalInheritorCrusaderClass0Align = "MortalInheritorCrusaderClass0Align";
        private static readonly string MortalInheritorCrusaderClass0AlignGuid = "c5e6a66a-74e7-4c79-bda0-b48ddf9d26db";
        internal const string MortalInheritorCrusaderClass0AlignDisplayName = "MortalInheritorCrusaderClass0Align.Name";
        private const string MortalInheritorCrusaderClass0AlignDescription = "MortalInheritorCrusaderClass0Align.Description";
        private const string MortalInheritorCrusaderClass2Align = "MortalInheritorCrusaderClass2Align";
        private static readonly string MortalInheritorCrusaderClass2AlignGuid = "9d1a6304-b3a9-437a-a4ab-3fe969963264";
        private const string MortalInheritorCrusaderClass3Align = "MortalInheritorCrusaderClass3Align";
        private static readonly string MortalInheritorCrusaderClass3AlignGuid = "b810659a-9f8a-426b-9ab7-25cd1b33fdcb";
        private const string MortalInheritorCrusaderClass4Align = "MortalInheritorCrusaderClass4Align";
        private static readonly string MortalInheritorCrusaderClass4AlignGuid = "a134e049-cd2e-425f-ace5-c9fcb6d81ee2";
        private const string MortalInheritorCrusaderClass5Align = "MortalInheritorCrusaderClass5Align";
        private static readonly string MortalInheritorCrusaderClass5AlignGuid = "d55c33fb-0267-411c-b559-aaf724ce0172";
        private const string MortalInheritorCrusaderClass6Align = "MortalInheritorCrusaderClass6Align";
        private static readonly string MortalInheritorCrusaderClass6AlignGuid = "6280d162-3db7-4434-bdf8-06f631786b53";
        private const string MortalInheritorCrusaderClass7Align = "MortalInheritorCrusaderClass7Align";
        private static readonly string MortalInheritorCrusaderClass7AlignGuid = "e95206aa-b369-434b-958a-d675a22248f1";
        private const string MortalInheritorCrusaderClass8Align = "MortalInheritorCrusaderClass8Align";
        private static readonly string MortalInheritorCrusaderClass8AlignGuid = "b11e6af7-eeef-4bd3-adf2-29e371dd48ac";
        private const string MortalInheritorCrusaderClass9Align = "MortalInheritorCrusaderClass9Align";
        private static readonly string MortalInheritorCrusaderClass9AlignGuid = "6090d52b-ee88-4505-82c8-9a6a001b92b7";
        private const string MortalInheritorCrusaderClass10Align = "MortalInheritorCrusaderClass10Align";
        private static readonly string MortalInheritorCrusaderClass10AlignGuid = "2c0be43d-c804-4f5c-8ca9-4690b6e73ff4";
        private const string MortalMammothRiderClass0Align = "MortalMammothRiderClass0Align";
        private static readonly string MortalMammothRiderClass0AlignGuid = "1fbaef6e-a51d-49b9-88ac-9271336491a9";
        internal const string MortalMammothRiderClass0AlignDisplayName = "MortalMammothRiderClass0Align.Name";
        private const string MortalMammothRiderClass0AlignDescription = "MortalMammothRiderClass0Align.Description";
        private const string MortalMammothRiderClass2Align = "MortalMammothRiderClass2Align";
        private static readonly string MortalMammothRiderClass2AlignGuid = "33e7aaae-3dec-4e7c-a0ae-58ebed4cb77f";
        private const string MortalMammothRiderClass3Align = "MortalMammothRiderClass3Align";
        private static readonly string MortalMammothRiderClass3AlignGuid = "8a86f314-14b1-4d63-ae56-b4e52c093e3c";
        private const string MortalMammothRiderClass4Align = "MortalMammothRiderClass4Align";
        private static readonly string MortalMammothRiderClass4AlignGuid = "a61c668a-4554-471d-a7da-a56962bf6a5a";
        private const string MortalMammothRiderClass5Align = "MortalMammothRiderClass5Align";
        private static readonly string MortalMammothRiderClass5AlignGuid = "c8a79e83-cd28-4545-98d3-6a40bf21a90b";
        private const string MortalMammothRiderClass6Align = "MortalMammothRiderClass6Align";
        private static readonly string MortalMammothRiderClass6AlignGuid = "a4910615-9ad0-43de-8a16-010d9c01f651";
        private const string MortalMammothRiderClass7Align = "MortalMammothRiderClass7Align";
        private static readonly string MortalMammothRiderClass7AlignGuid = "e27a921b-dd5d-4f72-b42d-8fa8bb84d9ef";
        private const string MortalMammothRiderClass8Align = "MortalMammothRiderClass8Align";
        private static readonly string MortalMammothRiderClass8AlignGuid = "fcc44770-3eb5-4c86-a293-4e8338bee396";
        private const string MortalMammothRiderClass9Align = "MortalMammothRiderClass9Align";
        private static readonly string MortalMammothRiderClass9AlignGuid = "d6b6cfc9-8030-41d7-8afe-1e1667eff3b7";
        private const string MortalMammothRiderClass10Align = "MortalMammothRiderClass10Align";
        private static readonly string MortalMammothRiderClass10AlignGuid = "8257c1ca-6f68-449a-844e-ddab90578268";
        private const string MortalSanguineAngelClass0Align = "MortalSanguineAngelClass0Align";
        private static readonly string MortalSanguineAngelClass0AlignGuid = "66e180b0-cfc2-470d-8703-39e0e215dda1";
        internal const string MortalSanguineAngelClass0AlignDisplayName = "MortalSanguineAngelClass0Align.Name";
        private const string MortalSanguineAngelClass0AlignDescription = "MortalSanguineAngelClass0Align.Description";
        private const string MortalSanguineAngelClass2Align = "MortalSanguineAngelClass2Align";
        private static readonly string MortalSanguineAngelClass2AlignGuid = "65d07c71-9f86-4404-8d40-07de6d629fd3";
        private const string MortalSanguineAngelClass3Align = "MortalSanguineAngelClass3Align";
        private static readonly string MortalSanguineAngelClass3AlignGuid = "e02e4ddb-a842-44ab-b5b7-94aa672119ef";
        private const string MortalSanguineAngelClass4Align = "MortalSanguineAngelClass4Align";
        private static readonly string MortalSanguineAngelClass4AlignGuid = "d10c98ae-4e5a-47b0-8575-21e131a33ae8";
        private const string MortalSanguineAngelClass5Align = "MortalSanguineAngelClass5Align";
        private static readonly string MortalSanguineAngelClass5AlignGuid = "0ad76861-0bb4-4acb-a48b-426a46e7052a";
        private const string MortalSanguineAngelClass6Align = "MortalSanguineAngelClass6Align";
        private static readonly string MortalSanguineAngelClass6AlignGuid = "4576fed4-da5d-4bbf-beb9-5a0b22d3194e";
        private const string MortalSanguineAngelClass7Align = "MortalSanguineAngelClass7Align";
        private static readonly string MortalSanguineAngelClass7AlignGuid = "9a830cf1-3ebd-4a28-9825-77e07c4c4673";
        private const string MortalSanguineAngelClass8Align = "MortalSanguineAngelClass8Align";
        private static readonly string MortalSanguineAngelClass8AlignGuid = "34176f0f-ef1e-4dc3-a931-37569b1ce37f";
        private const string MortalSanguineAngelClass9Align = "MortalSanguineAngelClass9Align";
        private static readonly string MortalSanguineAngelClass9AlignGuid = "5c43864d-a2d3-44d7-bc56-a2ebe0c66f9d";
        private const string MortalSanguineAngelClass10Align = "MortalSanguineAngelClass10Align";
        private static readonly string MortalSanguineAngelClass10AlignGuid = "4b93b652-e36e-45f7-879b-136601019781";
        private const string MortalScarSeekerClass0Align = "MortalScarSeekerClass0Align";
        private static readonly string MortalScarSeekerClass0AlignGuid = "f6d2cbee-4106-4e46-8a93-3dd23567b01f";
        internal const string MortalScarSeekerClass0AlignDisplayName = "MortalScarSeekerClass0Align.Name";
        private const string MortalScarSeekerClass0AlignDescription = "MortalScarSeekerClass0Align.Description";
        private const string MortalScarSeekerClass2Align = "MortalScarSeekerClass2Align";
        private static readonly string MortalScarSeekerClass2AlignGuid = "c964ddd5-cfd2-41eb-bd66-bc4305245176";
        private const string MortalScarSeekerClass3Align = "MortalScarSeekerClass3Align";
        private static readonly string MortalScarSeekerClass3AlignGuid = "49c9c10e-9faa-460b-a598-d8c8322d0aef";
        private const string MortalScarSeekerClass4Align = "MortalScarSeekerClass4Align";
        private static readonly string MortalScarSeekerClass4AlignGuid = "80ec8a4b-240a-4066-8590-50422d90d7f8";
        private const string MortalScarSeekerClass5Align = "MortalScarSeekerClass5Align";
        private static readonly string MortalScarSeekerClass5AlignGuid = "445295b6-7fa7-48c3-8bf2-939edb183ea6";
        private const string MortalScarSeekerClass6Align = "MortalScarSeekerClass6Align";
        private static readonly string MortalScarSeekerClass6AlignGuid = "c01a1a87-7505-4de8-8c99-a1cf66b2868b";
        private const string MortalScarSeekerClass7Align = "MortalScarSeekerClass7Align";
        private static readonly string MortalScarSeekerClass7AlignGuid = "459a862d-96ef-4279-9234-fff581e7948d";
        private const string MortalScarSeekerClass8Align = "MortalScarSeekerClass8Align";
        private static readonly string MortalScarSeekerClass8AlignGuid = "1c5b4f9b-b2ea-4877-b65f-0ec2fa80a90b";
        private const string MortalScarSeekerClass9Align = "MortalScarSeekerClass9Align";
        private static readonly string MortalScarSeekerClass9AlignGuid = "94c61f85-71f3-4229-97fb-91362d8326a4";
        private const string MortalScarSeekerClass10Align = "MortalScarSeekerClass10Align";
        private static readonly string MortalScarSeekerClass10AlignGuid = "c38d671c-fc1d-410e-9a22-6d8721114fca";
        private const string MortalSentinelClass0Align = "MortalSentinelClass0Align";
        private static readonly string MortalSentinelClass0AlignGuid = "94da1de6-027c-4449-adf8-a6aed7ed640e";
        internal const string MortalSentinelClass0AlignDisplayName = "MortalSentinelClass0Align.Name";
        private const string MortalSentinelClass0AlignDescription = "MortalSentinelClass0Align.Description";
        private const string MortalSentinelClass2Align = "MortalSentinelClass2Align";
        private static readonly string MortalSentinelClass2AlignGuid = "09ad67e7-c781-4d79-9a11-53e8da8cd8ed";
        private const string MortalSentinelClass3Align = "MortalSentinelClass3Align";
        private static readonly string MortalSentinelClass3AlignGuid = "08e07993-266e-45f2-8f39-d4e8d5aeae29";
        private const string MortalSentinelClass4Align = "MortalSentinelClass4Align";
        private static readonly string MortalSentinelClass4AlignGuid = "278bca62-43e3-4ad4-b634-4f1fc7609e43";
        private const string MortalSentinelClass5Align = "MortalSentinelClass5Align";
        private static readonly string MortalSentinelClass5AlignGuid = "e038d51a-5250-42d4-9add-02a3a95ea1f0";
        private const string MortalSentinelClass6Align = "MortalSentinelClass6Align";
        private static readonly string MortalSentinelClass6AlignGuid = "d755e31a-5e0e-4466-aa7e-01758df6ce8d";
        private const string MortalSentinelClass7Align = "MortalSentinelClass7Align";
        private static readonly string MortalSentinelClass7AlignGuid = "f2dffd31-91dd-4841-9cf4-8e6bd580a19f";
        private const string MortalSentinelClass8Align = "MortalSentinelClass8Align";
        private static readonly string MortalSentinelClass8AlignGuid = "57ad69e1-51f6-46ff-a8b9-b7e99c57f1a3";
        private const string MortalSentinelClass9Align = "MortalSentinelClass9Align";
        private static readonly string MortalSentinelClass9AlignGuid = "cc3446d5-6998-4596-bab6-2780192e6846";
        private const string MortalSentinelClass10Align = "MortalSentinelClass10Align";
        private static readonly string MortalSentinelClass10AlignGuid = "00f18060-61c9-460a-9fe1-302deebdcc96";
        private const string MortalShadowDancerClass0Align = "MortalShadowDancerClass0Align";
        private static readonly string MortalShadowDancerClass0AlignGuid = "db0adefc-9ad1-4af1-ae49-945058c13141";
        internal const string MortalShadowDancerClass0AlignDisplayName = "MortalShadowDancerClass0Align.Name";
        private const string MortalShadowDancerClass0AlignDescription = "MortalShadowDancerClass0Align.Description";
        private const string MortalShadowDancerClass2Align = "MortalShadowDancerClass2Align";
        private static readonly string MortalShadowDancerClass2AlignGuid = "46feed28-1476-4a9b-9d3a-250d5e994a65";
        private const string MortalShadowDancerClass3Align = "MortalShadowDancerClass3Align";
        private static readonly string MortalShadowDancerClass3AlignGuid = "f3ebb527-5b84-4acb-8733-2f3361fdbe38";
        private const string MortalShadowDancerClass4Align = "MortalShadowDancerClass4Align";
        private static readonly string MortalShadowDancerClass4AlignGuid = "3128981b-b0ad-4e1d-aa2f-692041752962";
        private const string MortalShadowDancerClass5Align = "MortalShadowDancerClass5Align";
        private static readonly string MortalShadowDancerClass5AlignGuid = "88c9f154-f00a-46e9-a376-86fc3516c7c8";
        private const string MortalShadowDancerClass6Align = "MortalShadowDancerClass6Align";
        private static readonly string MortalShadowDancerClass6AlignGuid = "d70c996c-0468-4935-b25f-7e707e827b96";
        private const string MortalShadowDancerClass7Align = "MortalShadowDancerClass7Align";
        private static readonly string MortalShadowDancerClass7AlignGuid = "3dcc4b9a-8dc5-4237-ada6-96f4f6e0b3f6";
        private const string MortalShadowDancerClass8Align = "MortalShadowDancerClass8Align";
        private static readonly string MortalShadowDancerClass8AlignGuid = "f79bf803-1972-49a4-9f2a-6e07e12a2781";
        private const string MortalShadowDancerClass9Align = "MortalShadowDancerClass9Align";
        private static readonly string MortalShadowDancerClass9AlignGuid = "7fbc1205-8aa5-4946-bea8-9724dfc55846";
        private const string MortalShadowDancerClass10Align = "MortalShadowDancerClass10Align";
        private static readonly string MortalShadowDancerClass10AlignGuid = "af249a9d-071a-414a-8d57-6d2c10ae0c1c";
        private const string MortalSouldrinkerClass0Align = "MortalSouldrinkerClass0Align";
        private static readonly string MortalSouldrinkerClass0AlignGuid = "bafea140-bc43-4864-badb-0010bb54e182";
        internal const string MortalSouldrinkerClass0AlignDisplayName = "MortalSouldrinkerClass0Align.Name";
        private const string MortalSouldrinkerClass0AlignDescription = "MortalSouldrinkerClass0Align.Description";
        private const string MortalSouldrinkerClass2Align = "MortalSouldrinkerClass2Align";
        private static readonly string MortalSouldrinkerClass2AlignGuid = "86afd5eb-997f-4a01-9b0c-6ab6b8fe4142";
        private const string MortalSouldrinkerClass3Align = "MortalSouldrinkerClass3Align";
        private static readonly string MortalSouldrinkerClass3AlignGuid = "bf0d4d08-7cfe-422b-92c6-a2e5c9a59c16";
        private const string MortalSouldrinkerClass4Align = "MortalSouldrinkerClass4Align";
        private static readonly string MortalSouldrinkerClass4AlignGuid = "8e35b6a5-bd42-4e9e-aa6d-71fc448dce77";
        private const string MortalSouldrinkerClass5Align = "MortalSouldrinkerClass5Align";
        private static readonly string MortalSouldrinkerClass5AlignGuid = "d913623c-40ec-4aa7-b46b-f36a5c90e47c";
        private const string MortalSouldrinkerClass6Align = "MortalSouldrinkerClass6Align";
        private static readonly string MortalSouldrinkerClass6AlignGuid = "76441469-537a-4878-9971-4dd834b535a3";
        private const string MortalSouldrinkerClass7Align = "MortalSouldrinkerClass7Align";
        private static readonly string MortalSouldrinkerClass7AlignGuid = "eca65eaf-4d0f-4317-94e3-4be79a928d5b";
        private const string MortalSouldrinkerClass8Align = "MortalSouldrinkerClass8Align";
        private static readonly string MortalSouldrinkerClass8AlignGuid = "ee3a446a-7bf3-450b-841b-c640acb0c487";
        private const string MortalSouldrinkerClass9Align = "MortalSouldrinkerClass9Align";
        private static readonly string MortalSouldrinkerClass9AlignGuid = "5a3984e2-bc27-4432-8223-66492778eea9";
        private const string MortalSouldrinkerClass10Align = "MortalSouldrinkerClass10Align";
        private static readonly string MortalSouldrinkerClass10AlignGuid = "10eaa9c6-eab9-44c4-a904-96a397b2972e";
        private const string MortalUmbralAgentClass0Align = "MortalUmbralAgentClass0Align";
        private static readonly string MortalUmbralAgentClass0AlignGuid = "42de2d57-fb41-40a4-8276-f816bf2e67d9";
        internal const string MortalUmbralAgentClass0AlignDisplayName = "MortalUmbralAgentClass0Align.Name";
        private const string MortalUmbralAgentClass0AlignDescription = "MortalUmbralAgentClass0Align.Description";
        private const string MortalUmbralAgentClass2Align = "MortalUmbralAgentClass2Align";
        private static readonly string MortalUmbralAgentClass2AlignGuid = "595910eb-d8c1-4018-a7c5-0d1708b7e46d";
        private const string MortalUmbralAgentClass3Align = "MortalUmbralAgentClass3Align";
        private static readonly string MortalUmbralAgentClass3AlignGuid = "e5c7ef43-8dff-419c-9241-de1fd590ecc5";
        private const string MortalUmbralAgentClass4Align = "MortalUmbralAgentClass4Align";
        private static readonly string MortalUmbralAgentClass4AlignGuid = "f8cca343-0377-4f95-8e80-935077efa2af";
        private const string MortalUmbralAgentClass5Align = "MortalUmbralAgentClass5Align";
        private static readonly string MortalUmbralAgentClass5AlignGuid = "bce5ab30-5d29-4a4d-a972-3039b55eccaf";
        private const string MortalUmbralAgentClass6Align = "MortalUmbralAgentClass6Align";
        private static readonly string MortalUmbralAgentClass6AlignGuid = "8deee227-bb71-4c0f-88f6-eeb17557a146";
        private const string MortalUmbralAgentClass7Align = "MortalUmbralAgentClass7Align";
        private static readonly string MortalUmbralAgentClass7AlignGuid = "36fe8638-b7c5-4f84-be2e-440df2148ce2";
        private const string MortalUmbralAgentClass8Align = "MortalUmbralAgentClass8Align";
        private static readonly string MortalUmbralAgentClass8AlignGuid = "fdea2b53-ae7e-46c7-8381-bc96b73a6719";
        private const string MortalUmbralAgentClass9Align = "MortalUmbralAgentClass9Align";
        private static readonly string MortalUmbralAgentClass9AlignGuid = "53d890d8-c032-4704-9b0e-c3b784a3d2c9";
        private const string MortalUmbralAgentClass10Align = "MortalUmbralAgentClass10Align";
        private static readonly string MortalUmbralAgentClass10AlignGuid = "e435eefb-9962-4071-bf2f-2841b8f7be8d";
        private const string MortalMicroAntiPaladinClass0Align = "MortalMicroAntiPaladinClass0Align";
        private static readonly string MortalMicroAntiPaladinClass0AlignGuid = "787838ce-eadb-4a41-b5cf-199a7d3cb3eb";
        internal const string MortalMicroAntiPaladinClass0AlignDisplayName = "MortalMicroAntiPaladinClass0Align.Name";
        private const string MortalMicroAntiPaladinClass0AlignDescription = "MortalMicroAntiPaladinClass0Align.Description";
        private const string MortalMicroAntiPaladinClass2Align = "MortalMicroAntiPaladinClass2Align";
        private static readonly string MortalMicroAntiPaladinClass2AlignGuid = "f220f500-2d86-4489-a597-a376e0a55578";
        private const string MortalMicroAntiPaladinClass3Align = "MortalMicroAntiPaladinClass3Align";
        private static readonly string MortalMicroAntiPaladinClass3AlignGuid = "55a1deca-fc4d-4cfd-adaa-ae508e74f9b1";
        private const string MortalMicroAntiPaladinClass4Align = "MortalMicroAntiPaladinClass4Align";
        private static readonly string MortalMicroAntiPaladinClass4AlignGuid = "bf2d45f9-4953-41f1-9b91-174d29aa3fd0";
        private const string MortalMicroAntiPaladinClass5Align = "MortalMicroAntiPaladinClass5Align";
        private static readonly string MortalMicroAntiPaladinClass5AlignGuid = "d1c6d94f-65c6-4e7d-850f-5827467180ef";
        private const string MortalMicroAntiPaladinClass6Align = "MortalMicroAntiPaladinClass6Align";
        private static readonly string MortalMicroAntiPaladinClass6AlignGuid = "d80e9900-7a81-4f00-bd51-dac0a105816f";
        private const string MortalMicroAntiPaladinClass7Align = "MortalMicroAntiPaladinClass7Align";
        private static readonly string MortalMicroAntiPaladinClass7AlignGuid = "4a8dbcd6-07c4-4a58-999a-af92b8edd777";
        private const string MortalMicroAntiPaladinClass8Align = "MortalMicroAntiPaladinClass8Align";
        private static readonly string MortalMicroAntiPaladinClass8AlignGuid = "27460880-4b80-406b-8786-446d365adbcc";
        private const string MortalMicroAntiPaladinClass9Align = "MortalMicroAntiPaladinClass9Align";
        private static readonly string MortalMicroAntiPaladinClass9AlignGuid = "039adf74-7708-4e00-bf2c-afff397ac26f";
        private const string MortalMicroAntiPaladinClass10Align = "MortalMicroAntiPaladinClass10Align";
        private static readonly string MortalMicroAntiPaladinClass10AlignGuid = "591a1124-4d3c-4868-aa53-29ba157b2f71";
        private const string MortalOathbreakerClass0Align = "MortalOathbreakerClass0Align";
        private static readonly string MortalOathbreakerClass0AlignGuid = "02010286-794c-4f8a-8a3a-a97bc2b6e4eb";
        internal const string MortalOathbreakerClass0AlignDisplayName = "MortalOathbreakerClass0Align.Name";
        private const string MortalOathbreakerClass0AlignDescription = "MortalOathbreakerClass0Align.Description";
        private const string MortalOathbreakerClass2Align = "MortalOathbreakerClass2Align";
        private static readonly string MortalOathbreakerClass2AlignGuid = "af81d8f6-6879-432e-8869-7277f2fb5337";
        private const string MortalOathbreakerClass3Align = "MortalOathbreakerClass3Align";
        private static readonly string MortalOathbreakerClass3AlignGuid = "4d951247-0339-4727-9d4a-c98bc884883c";
        private const string MortalOathbreakerClass4Align = "MortalOathbreakerClass4Align";
        private static readonly string MortalOathbreakerClass4AlignGuid = "51398bb6-a2bb-4445-b326-539cc0e96365";
        private const string MortalOathbreakerClass5Align = "MortalOathbreakerClass5Align";
        private static readonly string MortalOathbreakerClass5AlignGuid = "c8b8efcf-6412-4356-8d6e-f5af7c39f862";
        private const string MortalOathbreakerClass6Align = "MortalOathbreakerClass6Align";
        private static readonly string MortalOathbreakerClass6AlignGuid = "502443a5-58c0-4cf1-a75d-e96914e658a2";
        private const string MortalOathbreakerClass7Align = "MortalOathbreakerClass7Align";
        private static readonly string MortalOathbreakerClass7AlignGuid = "81c21e97-05c1-46df-9ee2-1a536f5a4495";
        private const string MortalOathbreakerClass8Align = "MortalOathbreakerClass8Align";
        private static readonly string MortalOathbreakerClass8AlignGuid = "2efb3eff-ab18-482a-ac4e-ce276458363d";
        private const string MortalOathbreakerClass9Align = "MortalOathbreakerClass9Align";
        private static readonly string MortalOathbreakerClass9AlignGuid = "ea6ad36a-5a11-43ca-8bd8-ce1f870da73c";
        private const string MortalOathbreakerClass10Align = "MortalOathbreakerClass10Align";
        private static readonly string MortalOathbreakerClass10AlignGuid = "428798a1-8ba4-4a40-b476-94ed59fd13c9";
        private const string MortalDreadKnightClass0Align = "MortalDreadKnightClass0Align";
        private static readonly string MortalDreadKnightClass0AlignGuid = "315e0df6-4b66-4bd3-9b72-89ba1e329a54";
        internal const string MortalDreadKnightClass0AlignDisplayName = "MortalDreadKnightClass0Align.Name";
        private const string MortalDreadKnightClass0AlignDescription = "MortalDreadKnightClass0Align.Description";
        private const string MortalDreadKnightClass2Align = "MortalDreadKnightClass2Align";
        private static readonly string MortalDreadKnightClass2AlignGuid = "3a525602-1b49-435b-bbfe-89b513dac0e4";
        private const string MortalDreadKnightClass3Align = "MortalDreadKnightClass3Align";
        private static readonly string MortalDreadKnightClass3AlignGuid = "e3eabb66-b172-432c-9401-3793f847b428";
        private const string MortalDreadKnightClass4Align = "MortalDreadKnightClass4Align";
        private static readonly string MortalDreadKnightClass4AlignGuid = "b2fcfddb-7bda-4e2d-8a3f-7eaa2961ebd0";
        private const string MortalDreadKnightClass5Align = "MortalDreadKnightClass5Align";
        private static readonly string MortalDreadKnightClass5AlignGuid = "44f4cd1c-0d9b-43e0-9ffa-d6cf996f6977";
        private const string MortalDreadKnightClass6Align = "MortalDreadKnightClass6Align";
        private static readonly string MortalDreadKnightClass6AlignGuid = "307083d2-1628-47f8-aaa2-79ac081c7525";
        private const string MortalDreadKnightClass7Align = "MortalDreadKnightClass7Align";
        private static readonly string MortalDreadKnightClass7AlignGuid = "f45ff691-11eb-4e56-853a-5df64631c913";
        private const string MortalDreadKnightClass8Align = "MortalDreadKnightClass8Align";
        private static readonly string MortalDreadKnightClass8AlignGuid = "6857d655-6c44-43d7-a3a5-d99c57a93fcc";
        private const string MortalDreadKnightClass9Align = "MortalDreadKnightClass9Align";
        private static readonly string MortalDreadKnightClass9AlignGuid = "56e48f56-1a77-4395-aaea-4c23596eca1d";
        private const string MortalDreadKnightClass10Align = "MortalDreadKnightClass10Align";
        private static readonly string MortalDreadKnightClass10AlignGuid = "3f2af56d-5513-4fa9-836c-a7f79515ae11";
        private const string MortalStargazerClass0Align = "MortalStargazerClass0Align";
        private static readonly string MortalStargazerClass0AlignGuid = "6553d078-9c51-4108-aca7-c6d5bb18c868";
        internal const string MortalStargazerClass0AlignDisplayName = "MortalStargazerClass0Align.Name";
        private const string MortalStargazerClass0AlignDescription = "MortalStargazerClass0Align.Description";
        private const string MortalStargazerClass2Align = "MortalStargazerClass2Align";
        private static readonly string MortalStargazerClass2AlignGuid = "d159bd68-4a6e-45f4-8017-38da434b3286";
        private const string MortalStargazerClass3Align = "MortalStargazerClass3Align";
        private static readonly string MortalStargazerClass3AlignGuid = "54f1290a-26a7-4adb-abef-1498cb276d82";
        private const string MortalStargazerClass4Align = "MortalStargazerClass4Align";
        private static readonly string MortalStargazerClass4AlignGuid = "1c192e26-92a2-4877-9160-7ef892516bb0";
        private const string MortalStargazerClass5Align = "MortalStargazerClass5Align";
        private static readonly string MortalStargazerClass5AlignGuid = "352f895d-6480-4b5c-999b-150efebb1ec9";
        private const string MortalStargazerClass6Align = "MortalStargazerClass6Align";
        private static readonly string MortalStargazerClass6AlignGuid = "466c44af-170c-4857-8aac-99dffd2962de";
        private const string MortalStargazerClass7Align = "MortalStargazerClass7Align";
        private static readonly string MortalStargazerClass7AlignGuid = "a6b046e4-a405-4540-945b-7118d347027b";
        private const string MortalStargazerClass8Align = "MortalStargazerClass8Align";
        private static readonly string MortalStargazerClass8AlignGuid = "a2713801-4fe2-465d-9e27-6a956292ad5e";
        private const string MortalStargazerClass9Align = "MortalStargazerClass9Align";
        private static readonly string MortalStargazerClass9AlignGuid = "02244adc-bdb7-4ed4-a6f2-0d8855939079";
        private const string MortalStargazerClass10Align = "MortalStargazerClass10Align";
        private static readonly string MortalStargazerClass10AlignGuid = "5799c2bd-6565-49c8-878b-df98119cfec5";
        private const string MortalSwashbucklerClass0Align = "MortalSwashbucklerClass0Align";
        private static readonly string MortalSwashbucklerClass0AlignGuid = "72417a90-19e9-426b-b62c-66014f16deb0";
        internal const string MortalSwashbucklerClass0AlignDisplayName = "MortalSwashbucklerClass0Align.Name";
        private const string MortalSwashbucklerClass0AlignDescription = "MortalSwashbucklerClass0Align.Description";
        private const string MortalSwashbucklerClass2Align = "MortalSwashbucklerClass2Align";
        private static readonly string MortalSwashbucklerClass2AlignGuid = "67c051aa-2ec7-4b37-862b-532f192b1ead";
        private const string MortalSwashbucklerClass3Align = "MortalSwashbucklerClass3Align";
        private static readonly string MortalSwashbucklerClass3AlignGuid = "73598501-83ba-4ff1-8605-bcb95207be7f";
        private const string MortalSwashbucklerClass4Align = "MortalSwashbucklerClass4Align";
        private static readonly string MortalSwashbucklerClass4AlignGuid = "e081d0b0-7a35-4227-9f6d-77818e3a0dff";
        private const string MortalSwashbucklerClass5Align = "MortalSwashbucklerClass5Align";
        private static readonly string MortalSwashbucklerClass5AlignGuid = "8cb85c2e-4345-4b5d-bb38-817c8bd8303c";
        private const string MortalSwashbucklerClass6Align = "MortalSwashbucklerClass6Align";
        private static readonly string MortalSwashbucklerClass6AlignGuid = "fd037815-5817-4035-a369-1f9f4a75dbe6";
        private const string MortalSwashbucklerClass7Align = "MortalSwashbucklerClass7Align";
        private static readonly string MortalSwashbucklerClass7AlignGuid = "ed777ebd-8f6b-4d9f-bf5c-a1997019dcc5";
        private const string MortalSwashbucklerClass8Align = "MortalSwashbucklerClass8Align";
        private static readonly string MortalSwashbucklerClass8AlignGuid = "9e37b507-9e99-4662-8b9e-70535112dc7e";
        private const string MortalSwashbucklerClass9Align = "MortalSwashbucklerClass9Align";
        private static readonly string MortalSwashbucklerClass9AlignGuid = "4129031b-7ff6-4044-a096-9d863b4a867b";
        private const string MortalSwashbucklerClass10Align = "MortalSwashbucklerClass10Align";
        private static readonly string MortalSwashbucklerClass10AlignGuid = "a052133e-cf24-4316-8fcb-c33d14256529";
        private const string MortalHolyVindicatorClass0Align = "MortalHolyVindicatorClass0Align";
        private static readonly string MortalHolyVindicatorClass0AlignGuid = "f3b2432a-b827-49a0-8389-9198d82b962c";
        internal const string MortalHolyVindicatorClass0AlignDisplayName = "MortalHolyVindicatorClass0Align.Name";
        private const string MortalHolyVindicatorClass0AlignDescription = "MortalHolyVindicatorClass0Align.Description";
        private const string MortalHolyVindicatorClass2Align = "MortalHolyVindicatorClass2Align";
        private static readonly string MortalHolyVindicatorClass2AlignGuid = "14bf2a27-54f8-4585-aa96-e398f9d75cec";
        private const string MortalHolyVindicatorClass3Align = "MortalHolyVindicatorClass3Align";
        private static readonly string MortalHolyVindicatorClass3AlignGuid = "5d672a35-7fca-4c12-8958-8987f09df207";
        private const string MortalHolyVindicatorClass4Align = "MortalHolyVindicatorClass4Align";
        private static readonly string MortalHolyVindicatorClass4AlignGuid = "70e61e84-0e3a-4362-9798-3a10045edc0f";
        private const string MortalHolyVindicatorClass5Align = "MortalHolyVindicatorClass5Align";
        private static readonly string MortalHolyVindicatorClass5AlignGuid = "77dd39dc-e5c5-4a45-a9b2-53a8e5619d48";
        private const string MortalHolyVindicatorClass6Align = "MortalHolyVindicatorClass6Align";
        private static readonly string MortalHolyVindicatorClass6AlignGuid = "202f4899-5af7-43f6-8d6e-87480e3b1311";
        private const string MortalHolyVindicatorClass7Align = "MortalHolyVindicatorClass7Align";
        private static readonly string MortalHolyVindicatorClass7AlignGuid = "2aacad4e-51ac-47ed-ad2a-d02a26c81558";
        private const string MortalHolyVindicatorClass8Align = "MortalHolyVindicatorClass8Align";
        private static readonly string MortalHolyVindicatorClass8AlignGuid = "ad3875b8-9b2c-4f48-a62e-037f83bf416a";
        private const string MortalHolyVindicatorClass9Align = "MortalHolyVindicatorClass9Align";
        private static readonly string MortalHolyVindicatorClass9AlignGuid = "94221516-9ca2-4569-ae85-fa5db798eb21";
        private const string MortalHolyVindicatorClass10Align = "MortalHolyVindicatorClass10Align";
        private static readonly string MortalHolyVindicatorClass10AlignGuid = "1f3330d5-ba13-4e44-bc0f-2abe0b0db1e6";
        private const string MortalSummonerClass0Align = "MortalSummonerClass0Align";
        private static readonly string MortalSummonerClass0AlignGuid = "809e9761-6e3e-4856-bda7-c255d4078a7a";
        internal const string MortalSummonerClass0AlignDisplayName = "MortalSummonerClass0Align.Name";
        private const string MortalSummonerClass0AlignDescription = "MortalSummonerClass0Align.Description";
        private const string MortalSummonerClass2Align = "MortalSummonerClass2Align";
        private static readonly string MortalSummonerClass2AlignGuid = "12f7e5e8-59fc-4210-9259-1026dded3738";
        private const string MortalSummonerClass3Align = "MortalSummonerClass3Align";
        private static readonly string MortalSummonerClass3AlignGuid = "60edb43c-7e2d-4c36-8c4f-9cb684dc79b6";
        private const string MortalSummonerClass4Align = "MortalSummonerClass4Align";
        private static readonly string MortalSummonerClass4AlignGuid = "bb4c219c-bffb-40d0-837e-18e0cfc156a9";
        private const string MortalSummonerClass5Align = "MortalSummonerClass5Align";
        private static readonly string MortalSummonerClass5AlignGuid = "0b113234-f3e2-4215-9afa-c92707000811";
        private const string MortalSummonerClass6Align = "MortalSummonerClass6Align";
        private static readonly string MortalSummonerClass6AlignGuid = "dadbcc39-f60e-4a8b-b876-7484954854b9";
        private const string MortalSummonerClass7Align = "MortalSummonerClass7Align";
        private static readonly string MortalSummonerClass7AlignGuid = "5dd21bcd-94c6-444e-9316-81b95f62e7c0";
        private const string MortalSummonerClass8Align = "MortalSummonerClass8Align";
        private static readonly string MortalSummonerClass8AlignGuid = "78598cc1-5a1d-4842-96dd-5f2836491b2a";
        private const string MortalSummonerClass9Align = "MortalSummonerClass9Align";
        private static readonly string MortalSummonerClass9AlignGuid = "309182fe-7035-4d57-8ff7-a9ca13046bb2";
        private const string MortalSummonerClass10Align = "MortalSummonerClass10Align";
        private static readonly string MortalSummonerClass10AlignGuid = "a0d30a6a-1e8c-4019-86e3-20abe5cab839";
        private const string MortalLionBladeClass0Align = "MortalLionBladeClass0Align";
        private static readonly string MortalLionBladeClass0AlignGuid = "86f50410-9806-4409-97a1-9b788437d6e4";
        internal const string MortalLionBladeClass0AlignDisplayName = "MortalLionBladeClass0Align.Name";
        private const string MortalLionBladeClass0AlignDescription = "MortalLionBladeClass0Align.Description";
        private const string MortalLionBladeClass2Align = "MortalLionBladeClass2Align";
        private static readonly string MortalLionBladeClass2AlignGuid = "a55350ab-6e99-44a0-91da-0c5c496494b3";
        private const string MortalLionBladeClass3Align = "MortalLionBladeClass3Align";
        private static readonly string MortalLionBladeClass3AlignGuid = "75379e46-c6cf-4518-ae55-f213834e4906";
        private const string MortalLionBladeClass4Align = "MortalLionBladeClass4Align";
        private static readonly string MortalLionBladeClass4AlignGuid = "4f6cd854-a57a-4a2f-a5db-2bd33ca5989c";
        private const string MortalLionBladeClass5Align = "MortalLionBladeClass5Align";
        private static readonly string MortalLionBladeClass5AlignGuid = "f66bb1e9-9b45-4f55-b4dc-096f7d5244da";
        private const string MortalLionBladeClass6Align = "MortalLionBladeClass6Align";
        private static readonly string MortalLionBladeClass6AlignGuid = "91da0390-2f61-4fe3-98e3-ea7c94052d99";
        private const string MortalLionBladeClass7Align = "MortalLionBladeClass7Align";
        private static readonly string MortalLionBladeClass7AlignGuid = "c0ea3efd-570d-4928-8032-7e97fc19eb32";
        private const string MortalLionBladeClass8Align = "MortalLionBladeClass8Align";
        private static readonly string MortalLionBladeClass8AlignGuid = "cdfb306b-5750-4a6b-ac51-6cbc396fd5f5";
        private const string MortalLionBladeClass9Align = "MortalLionBladeClass9Align";
        private static readonly string MortalLionBladeClass9AlignGuid = "191ea4a4-e778-4470-94bc-dbd1aa80e7f7";
        private const string MortalLionBladeClass10Align = "MortalLionBladeClass10Align";
        private static readonly string MortalLionBladeClass10AlignGuid = "ff2a551d-94df-4220-8c62-64f90d2be635";
        private const string MortalEnchantingCourtesanClass0Align = "MortalEnchantingCourtesanClass0Align";
        private static readonly string MortalEnchantingCourtesanClass0AlignGuid = "8885b682-2429-46a8-aefd-0acc0049375d";
        internal const string MortalEnchantingCourtesanClass0AlignDisplayName = "MortalEnchantingCourtesanClass0Align.Name";
        private const string MortalEnchantingCourtesanClass0AlignDescription = "MortalEnchantingCourtesanClass0Align.Description";
        private const string MortalEnchantingCourtesanClass2Align = "MortalEnchantingCourtesanClass2Align";
        private static readonly string MortalEnchantingCourtesanClass2AlignGuid = "160dd1d6-9af8-4066-bd36-392686fee7e4";
        private const string MortalEnchantingCourtesanClass3Align = "MortalEnchantingCourtesanClass3Align";
        private static readonly string MortalEnchantingCourtesanClass3AlignGuid = "18f21ca4-0bb0-4cdc-9b65-a96f9d05335f";
        private const string MortalEnchantingCourtesanClass4Align = "MortalEnchantingCourtesanClass4Align";
        private static readonly string MortalEnchantingCourtesanClass4AlignGuid = "87042299-2659-4705-ad41-c1b9bf52e4de";
        private const string MortalEnchantingCourtesanClass5Align = "MortalEnchantingCourtesanClass5Align";
        private static readonly string MortalEnchantingCourtesanClass5AlignGuid = "0d6068c9-559e-4f03-bc9b-255a355130ce";
        private const string MortalEnchantingCourtesanClass6Align = "MortalEnchantingCourtesanClass6Align";
        private static readonly string MortalEnchantingCourtesanClass6AlignGuid = "69d3a795-aee2-43a2-9957-38369c193fdf";
        private const string MortalEnchantingCourtesanClass7Align = "MortalEnchantingCourtesanClass7Align";
        private static readonly string MortalEnchantingCourtesanClass7AlignGuid = "32130b96-173c-4276-b20a-dd86399594a2";
        private const string MortalEnchantingCourtesanClass8Align = "MortalEnchantingCourtesanClass8Align";
        private static readonly string MortalEnchantingCourtesanClass8AlignGuid = "72227931-c5db-46e3-99ad-3b9bc9947f51";
        private const string MortalEnchantingCourtesanClass9Align = "MortalEnchantingCourtesanClass9Align";
        private static readonly string MortalEnchantingCourtesanClass9AlignGuid = "d8c8c008-6d98-4cd5-913d-648e23349144";
        private const string MortalEnchantingCourtesanClass10Align = "MortalEnchantingCourtesanClass10Align";
        private static readonly string MortalEnchantingCourtesanClass10AlignGuid = "25fe8e7a-c2b5-405c-85d7-9e2d34fac7cb";
        private const string MortalHeritorKnightClass0Align = "MortalHeritorKnightClass0Align";
        private static readonly string MortalHeritorKnightClass0AlignGuid = "e23c832d-2bf9-4014-aac2-1695a39ccf7b";
        internal const string MortalHeritorKnightClass0AlignDisplayName = "MortalHeritorKnightClass0Align.Name";
        private const string MortalHeritorKnightClass0AlignDescription = "MortalHeritorKnightClass0Align.Description";
        private const string MortalHeritorKnightClass2Align = "MortalHeritorKnightClass2Align";
        private static readonly string MortalHeritorKnightClass2AlignGuid = "07b1f26e-e121-40e5-ac65-1829c3eb3a38";
        private const string MortalHeritorKnightClass3Align = "MortalHeritorKnightClass3Align";
        private static readonly string MortalHeritorKnightClass3AlignGuid = "d6e1fe91-1526-4324-b238-7a81880db98e";
        private const string MortalHeritorKnightClass4Align = "MortalHeritorKnightClass4Align";
        private static readonly string MortalHeritorKnightClass4AlignGuid = "8adabd58-d60c-409b-9db4-25edae4a5de7";
        private const string MortalHeritorKnightClass5Align = "MortalHeritorKnightClass5Align";
        private static readonly string MortalHeritorKnightClass5AlignGuid = "6886dce7-e8a4-4cf6-9833-416a0c17842a";
        private const string MortalHeritorKnightClass6Align = "MortalHeritorKnightClass6Align";
        private static readonly string MortalHeritorKnightClass6AlignGuid = "90229200-515d-49da-94c3-2d1a88de4198";
        private const string MortalHeritorKnightClass7Align = "MortalHeritorKnightClass7Align";
        private static readonly string MortalHeritorKnightClass7AlignGuid = "8f2c07e4-7399-4e4c-bdfe-e301f7108c99";
        private const string MortalHeritorKnightClass8Align = "MortalHeritorKnightClass8Align";
        private static readonly string MortalHeritorKnightClass8AlignGuid = "d1cd042a-4262-44d4-a68b-a3fafe2980de";
        private const string MortalHeritorKnightClass9Align = "MortalHeritorKnightClass9Align";
        private static readonly string MortalHeritorKnightClass9AlignGuid = "96dc1079-d588-4ba1-96ea-65c0706af4ed";
        private const string MortalHeritorKnightClass10Align = "MortalHeritorKnightClass10Align";
        private static readonly string MortalHeritorKnightClass10AlignGuid = "201602f3-0294-44c4-be77-5e85782bbe7b";
        private const string MortalGoldenLegionnaireClass0Align = "MortalGoldenLegionnaireClass0Align";
        private static readonly string MortalGoldenLegionnaireClass0AlignGuid = "2bb84d41-c2f9-49d5-92a8-c5f2e103bcc4";
        internal const string MortalGoldenLegionnaireClass0AlignDisplayName = "MortalGoldenLegionnaireClass0Align.Name";
        private const string MortalGoldenLegionnaireClass0AlignDescription = "MortalGoldenLegionnaireClass0Align.Description";
        private const string MortalGoldenLegionnaireClass2Align = "MortalGoldenLegionnaireClass2Align";
        private static readonly string MortalGoldenLegionnaireClass2AlignGuid = "009355aa-a142-457c-84fc-b658cac95bc0";
        private const string MortalGoldenLegionnaireClass3Align = "MortalGoldenLegionnaireClass3Align";
        private static readonly string MortalGoldenLegionnaireClass3AlignGuid = "2988f038-3239-49f8-83ed-294ea55afa10";
        private const string MortalGoldenLegionnaireClass4Align = "MortalGoldenLegionnaireClass4Align";
        private static readonly string MortalGoldenLegionnaireClass4AlignGuid = "d05b9f26-e5b5-4582-a8dd-7828e0504de5";
        private const string MortalGoldenLegionnaireClass5Align = "MortalGoldenLegionnaireClass5Align";
        private static readonly string MortalGoldenLegionnaireClass5AlignGuid = "e2f37d58-6e8c-46a1-916c-b6d6f255e6c9";
        private const string MortalGoldenLegionnaireClass6Align = "MortalGoldenLegionnaireClass6Align";
        private static readonly string MortalGoldenLegionnaireClass6AlignGuid = "d2ec9032-e2e5-4aea-a80e-a154bcaf6f14";
        private const string MortalGoldenLegionnaireClass7Align = "MortalGoldenLegionnaireClass7Align";
        private static readonly string MortalGoldenLegionnaireClass7AlignGuid = "2fdae07b-8232-47c1-8d63-3e33ad0e3434";
        private const string MortalGoldenLegionnaireClass8Align = "MortalGoldenLegionnaireClass8Align";
        private static readonly string MortalGoldenLegionnaireClass8AlignGuid = "9718bb76-5aea-4c4f-8015-070f23dc6505";
        private const string MortalGoldenLegionnaireClass9Align = "MortalGoldenLegionnaireClass9Align";
        private static readonly string MortalGoldenLegionnaireClass9AlignGuid = "9c4fe8a6-b03d-417d-bde5-17a6a6f664c3";
        private const string MortalGoldenLegionnaireClass10Align = "MortalGoldenLegionnaireClass10Align";
        private static readonly string MortalGoldenLegionnaireClass10AlignGuid = "0e8a02ba-76de-49d0-916d-4bf45a325b51";
        private const string MortalBoltAceClass0Align = "MortalBoltAceClass0Align";
        private static readonly string MortalBoltAceClass0AlignGuid = "11d8fe4e-75ab-4047-a8a1-1ca26bda42c3";
        internal const string MortalBoltAceClass0AlignDisplayName = "MortalBoltAceClass0Align.Name";
        private const string MortalBoltAceClass0AlignDescription = "MortalBoltAceClass0Align.Description";
        private const string MortalBoltAceClass2Align = "MortalBoltAceClass2Align";
        private static readonly string MortalBoltAceClass2AlignGuid = "e91aa19e-861a-400b-8d4c-33d0bad0e8af";
        private const string MortalBoltAceClass3Align = "MortalBoltAceClass3Align";
        private static readonly string MortalBoltAceClass3AlignGuid = "ef14b17e-31c2-41e0-8a50-9e5c5382b792";
        private const string MortalBoltAceClass4Align = "MortalBoltAceClass4Align";
        private static readonly string MortalBoltAceClass4AlignGuid = "358c352e-fda8-4716-ad1d-89d98177cd04";
        private const string MortalBoltAceClass5Align = "MortalBoltAceClass5Align";
        private static readonly string MortalBoltAceClass5AlignGuid = "ac7b643e-043c-4651-a3c7-bf2a06bc1e34";
        private const string MortalBoltAceClass6Align = "MortalBoltAceClass6Align";
        private static readonly string MortalBoltAceClass6AlignGuid = "a1437e7e-d276-4a58-ba6a-84c4f355e91d";
        private const string MortalBoltAceClass7Align = "MortalBoltAceClass7Align";
        private static readonly string MortalBoltAceClass7AlignGuid = "f79981c3-7650-4126-bbf2-6b9b35dcf109";
        private const string MortalBoltAceClass8Align = "MortalBoltAceClass8Align";
        private static readonly string MortalBoltAceClass8AlignGuid = "8a551a87-9621-4f3d-b0dd-da0b05980cac";
        private const string MortalBoltAceClass9Align = "MortalBoltAceClass9Align";
        private static readonly string MortalBoltAceClass9AlignGuid = "768f9c70-25d2-4cfc-b1a7-8a12c5dd1993";
        private const string MortalBoltAceClass10Align = "MortalBoltAceClass10Align";
        private static readonly string MortalBoltAceClass10AlignGuid = "d868e79a-215e-4f20-aa00-c2c2e4a54c08";
        private const string MortalForestWardenClass0Align = "MortalForestWardenClass0Align";
        private static readonly string MortalForestWardenClass0AlignGuid = "99d86ff7-b9aa-4c8f-b83f-b9dd9a99b74d";
        internal const string MortalForestWardenClass0AlignDisplayName = "MortalForestWardenClass0Align.Name";
        private const string MortalForestWardenClass0AlignDescription = "MortalForestWardenClass0Align.Description";
        private const string MortalForestWardenClass2Align = "MortalForestWardenClass2Align";
        private static readonly string MortalForestWardenClass2AlignGuid = "fe543e86-3290-4247-8d8a-4dd5bea9d1e1";
        private const string MortalForestWardenClass3Align = "MortalForestWardenClass3Align";
        private static readonly string MortalForestWardenClass3AlignGuid = "5dff5b5e-1d02-4e69-9cc9-5436d3f892af";
        private const string MortalForestWardenClass4Align = "MortalForestWardenClass4Align";
        private static readonly string MortalForestWardenClass4AlignGuid = "9b062968-34b1-4f4a-929a-ca8836870a6a";
        private const string MortalForestWardenClass5Align = "MortalForestWardenClass5Align";
        private static readonly string MortalForestWardenClass5AlignGuid = "07c9e617-2374-4926-bdde-8a7f4a635caf";
        private const string MortalForestWardenClass6Align = "MortalForestWardenClass6Align";
        private static readonly string MortalForestWardenClass6AlignGuid = "9119f12a-7df1-4677-9af0-7ef04a1ee186";
        private const string MortalForestWardenClass7Align = "MortalForestWardenClass7Align";
        private static readonly string MortalForestWardenClass7AlignGuid = "2893185d-bfed-4149-b159-ee4991c6ce7a";
        private const string MortalForestWardenClass8Align = "MortalForestWardenClass8Align";
        private static readonly string MortalForestWardenClass8AlignGuid = "b581fa31-7e4b-4bfc-b7f1-6d67650bec8f";
        private const string MortalForestWardenClass9Align = "MortalForestWardenClass9Align";
        private static readonly string MortalForestWardenClass9AlignGuid = "c268d75f-4a95-46ce-9f89-8d666372fcf3";
        private const string MortalForestWardenClass10Align = "MortalForestWardenClass10Align";
        private static readonly string MortalForestWardenClass10AlignGuid = "da40b072-065d-4af4-a6d0-8dc7f99da94e";

    }
}
