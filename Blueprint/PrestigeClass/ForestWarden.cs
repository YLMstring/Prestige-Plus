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
using Kingmaker.Blueprints.Classes.Selection;
using PrestigePlus.Blueprint.Gunslinger;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using PrestigePlus.CustomComponent;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class ForestWarden
    {
        private const string ArchetypeName = "ForestWarden";
        public static readonly string ArchetypeGuid = "{89FFF2B7-0C77-4973-8631-5A4C94A3C1EB}";
        internal const string ArchetypeDisplayName = "ForestWarden.Name";
        private const string ArchetypeDescription = "ForestWarden.Description";

        private const string ClassProgressName = "ForestWardenPrestige";
        public static readonly string ClassProgressGuid = "{FAE0E13F-A118-440A-B313-95E68D1B33EA}";

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

        private const string DefyDanger = "ForestWardenDefyDanger";
        private static readonly string DefyDangerGuid = "{E891A2FF-14E6-4A00-9F0E-05EE87508A2F}";

        internal const string DefyDangerDisplayName = "ForestWardenDefyDanger.Name";
        private const string DefyDangerDescription = "ForestWardenDefyDanger.Description";
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

        private const string Retaliate = "ForestWardenRetaliate";
        public static readonly string RetaliateGuid = "{798F5C7F-3FEA-4AD8-ADAA-E57FA93D20F8}";

        internal const string RetaliateDisplayName = "ForestWardenRetaliate.Name";
        private const string RetaliateDescription = "ForestWardenRetaliate.Description";
        public static BlueprintFeature RetaliateFeature()
        {
            var icon = FeatureRefs.DevotedBladeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(Retaliate, RetaliateGuid)
              .SetDisplayName(RetaliateDisplayName)
              .SetDescription(RetaliateDescription)
              .SetIcon(icon)

              .Configure();
        }

        private const string AuthoritativeCommand2 = "ForestWardenAuthoritativeCommand2";
        public static readonly string AuthoritativeCommand2Guid = "{09666D53-7194-4B06-8CFB-303552B901C1}";

        internal const string AuthoritativeCommand2DisplayName = "ForestWardenAuthoritativeCommand2.Name";
        private const string AuthoritativeCommand2Description = "ForestWardenAuthoritativeCommand2.Description";
        public static BlueprintFeature AuthoritativeCommand2Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand2, AuthoritativeCommand2Guid)
              .SetDisplayName(AuthoritativeCommand2DisplayName)
              .SetDescription(AuthoritativeCommand2Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand3 = "ForestWardenAuthoritativeCommand3";
        public static readonly string AuthoritativeCommand3Guid = "{EFC5F4EC-81E4-4DBD-A341-79414EB71BFC}";

        internal const string AuthoritativeCommand3DisplayName = "ForestWardenAuthoritativeCommand3.Name";
        private const string AuthoritativeCommand3Description = "ForestWardenAuthoritativeCommand3.Description";
        public static BlueprintFeature AuthoritativeCommand3Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand3, AuthoritativeCommand3Guid)
              .SetDisplayName(AuthoritativeCommand3DisplayName)
              .SetDescription(AuthoritativeCommand3Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand5 = "ForestWardenAuthoritativeCommand5";
        public static readonly string AuthoritativeCommand5Guid = "{2E134743-07A7-405E-B3C1-B6243E6B9F57}";

        internal const string AuthoritativeCommand5DisplayName = "ForestWardenAuthoritativeCommand5.Name";
        private const string AuthoritativeCommand5Description = "ForestWardenAuthoritativeCommand5.Description";
        public static BlueprintFeature AuthoritativeCommand5Feature()
        {
            var icon = AbilityRefs.CommandGreater.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand5, AuthoritativeCommand5Guid)
              .SetDisplayName(AuthoritativeCommand5DisplayName)
              .SetDescription(AuthoritativeCommand5Description)
              .SetIcon(icon)
              .Configure();
        }

        private const string AuthoritativeCommand4 = "ForestWardenAuthoritativeCommand4";
        public static readonly string AuthoritativeCommand4Guid = "{3429D9BD-E68D-4096-B2BE-3EB121825B2E}";

        internal const string AuthoritativeCommand4DisplayName = "ForestWardenAuthoritativeCommand4.Name";
        private const string AuthoritativeCommand4Description = "ForestWardenAuthoritativeCommand4.Description";
        public static BlueprintFeature AuthoritativeCommand4Feature()
        {
            var icon = AbilityRefs.Command.Reference.Get().Icon;
            return FeatureConfigurator.New(AuthoritativeCommand4, AuthoritativeCommand4Guid)
              .SetDisplayName(AuthoritativeCommand4DisplayName)
              .SetDescription(AuthoritativeCommand4Description)
              .SetIcon(icon)
              .Configure();
        }

        private static readonly string AuthoritativeCommandName = "ForestWardenAuthoritativeCommand";
        public static readonly string AuthoritativeCommandGuid = "{E541F28C-42F6-4C5A-854B-F585318076A3}";

        private static readonly string AuthoritativeCommandDisplayName = "ForestWardenAuthoritativeCommand.Name";
        private static readonly string AuthoritativeCommandDescription = "ForestWardenAuthoritativeCommand.Description";

        private const string AuthoritativeCommandAbility = "AuthoritativeCommand.AuthoritativeCommandAbility";
        private static readonly string AuthoritativeCommandAbilityGuid = "{399B14AE-70FE-47DB-9206-B08056267AD4}";

        private const string AuthoritativeCommandBuff = "AuthoritativeCommand.AuthoritativeCommandBuff";
        private static readonly string AuthoritativeCommandBuffGuid = "{F5F99B01-0023-4A6B-A7B3-4F994DA903E2}";

        private const string CommandMoveAutoAbility = "ForestWarden.UseCommandMoveAuto";
        public static readonly string CommandMoveAutoAbilityGuid = "{2B9C109C-F0B6-4001-A6D0-8C4893615AE5}";

        private const string CommandMoveAutoBuff2 = "ForestWarden.CommandMoveAutoBuff2";
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
                .Add<SpendMoveAction>(c =>
                {
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

        private static readonly string AuthoritativeCommandSwiftName = "ForestWardenAuthoritativeCommandSwift";
        public static readonly string AuthoritativeCommandSwiftGuid = "{BA301623-4AF8-4898-A8DB-DA2F79F6573C}";

        private static readonly string AuthoritativeCommandSwiftDisplayName = "ForestWardenAuthoritativeCommandSwift.Name";
        private static readonly string AuthoritativeCommandSwiftDescription = "ForestWardenAuthoritativeCommandSwift.Description";

        private const string AuthoritativeCommandSwiftAbility = "AuthoritativeCommandSwift.AuthoritativeCommandSwiftAbility";
        private static readonly string AuthoritativeCommandSwiftAbilityGuid = "{A5697DB1-D8C7-438E-8E90-EE762CFF1176}";

        private const string CommandSwiftMoveAutoAbility = "ForestWarden.UseCommandSwiftMoveAuto";
        public static readonly string CommandSwiftMoveAutoAbilityGuid = "{7E7AA762-2638-40C2-ACC8-C05CD1F55FAE}";

        private const string CommandSwiftMoveAutoBuff2 = "ForestWarden.CommandSwiftMoveAutoBuff2";
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
                .Add<SpendSwiftAction>(c =>
                {
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

        private static readonly string UnitedDefenseName = "ForestWardenUnitedDefense";
        public static readonly string UnitedDefenseGuid = "{7C46838D-D2D2-4DDB-931C-25E21B33E6C4}";

        private static readonly string UnitedDefenseDisplayName = "ForestWardenUnitedDefense.Name";
        private static readonly string UnitedDefenseDescription = "ForestWardenUnitedDefense.Description";

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

        private const string AlliedRetribution = "ForestWardenAlliedRetribution";
        private static readonly string AlliedRetributionGuid = "{93F9ECCA-0D43-4688-9B3C-8BDBB1A067EE}";

        internal const string AlliedRetributionDisplayName = "ForestWardenAlliedRetribution.Name";
        private const string AlliedRetributionDescription = "ForestWardenAlliedRetribution.Description";

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

        private const string PreemptiveStrike = "ForestWardenPreemptiveStrike";
        public static readonly string PreemptiveStrikeGuid = "{27C488F2-7613-4210-BFDF-56A4A49133B9}";

        internal const string PreemptiveStrikeDisplayName = "ForestWardenPreemptiveStrike.Name";
        private const string PreemptiveStrikeDescription = "ForestWardenPreemptiveStrike.Description";

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

        private const string AlignSpam = "ForestWarden.AlignSpam";
        public static readonly string AlignSpamGuid = "{69DFC912-DFE1-46A7-A6AC-4E701DCF0A27}";

        internal const string SanctifiedRogueDisplayName = "ForestWardenSanctifiedRogue.Name";
        private const string SanctifiedRogueDescription = "ForestWardenSanctifiedRogue.Description";

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
            string MortalUsherClass = MortalUsher.ArchetypeGuid;
            //string ForestWardenClass = ForestWarden.ArchetypeGuid;

            var list = new List<BlueprintFeature>();

            var ForestAlchemistClasspro = ProgressionConfigurator.New(ForestAlchemistClass0Align, ForestAlchemistClass0AlignGuid)
            .SetDisplayName(ForestAlchemistClass0AlignDisplayName)
            .SetDescription(ForestAlchemistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AlchemistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAlchemistClass2Align, ForestAlchemistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestAlchemistClass3Align, ForestAlchemistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAlchemistClass4Align, ForestAlchemistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestAlchemistClass5Align, ForestAlchemistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAlchemistClass6Align, ForestAlchemistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestAlchemistClass7Align, ForestAlchemistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAlchemistClass8Align, ForestAlchemistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestAlchemistClass9Align, ForestAlchemistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAlchemistClasspro = ForestAlchemistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAlchemistClass10Align, ForestAlchemistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AlchemistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestAlchemistClasspro.Configure());
            var ForestArcaneTricksterClasspro = ProgressionConfigurator.New(ForestArcaneTricksterClass0Align, ForestArcaneTricksterClass0AlignGuid)
            .SetDisplayName(ForestArcaneTricksterClass0AlignDisplayName)
            .SetDescription(ForestArcaneTricksterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcaneTricksterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcaneTricksterClass2Align, ForestArcaneTricksterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestArcaneTricksterClass3Align, ForestArcaneTricksterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcaneTricksterClass4Align, ForestArcaneTricksterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestArcaneTricksterClass5Align, ForestArcaneTricksterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcaneTricksterClass6Align, ForestArcaneTricksterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestArcaneTricksterClass7Align, ForestArcaneTricksterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcaneTricksterClass8Align, ForestArcaneTricksterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestArcaneTricksterClass9Align, ForestArcaneTricksterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcaneTricksterClasspro = ForestArcaneTricksterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcaneTricksterClass10Align, ForestArcaneTricksterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcaneTricksterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcaneTricksterClasspro.Configure());
            var ForestArcanistClasspro = ProgressionConfigurator.New(ForestArcanistClass0Align, ForestArcanistClass0AlignGuid)
            .SetDisplayName(ForestArcanistClass0AlignDisplayName)
            .SetDescription(ForestArcanistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ArcanistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcanistClass2Align, ForestArcanistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestArcanistClass3Align, ForestArcanistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcanistClass4Align, ForestArcanistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestArcanistClass5Align, ForestArcanistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcanistClass6Align, ForestArcanistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestArcanistClass7Align, ForestArcanistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcanistClass8Align, ForestArcanistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestArcanistClass9Align, ForestArcanistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestArcanistClasspro = ForestArcanistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcanistClass10Align, ForestArcanistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ArcanistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcanistClasspro.Configure());
            var ForestAssassinClasspro = ProgressionConfigurator.New(ForestAssassinClass0Align, ForestAssassinClass0AlignGuid)
            .SetDisplayName(ForestAssassinClass0AlignDisplayName)
            .SetDescription(ForestAssassinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.AssassinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAssassinClass2Align, ForestAssassinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestAssassinClass3Align, ForestAssassinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAssassinClass4Align, ForestAssassinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestAssassinClass5Align, ForestAssassinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAssassinClass6Align, ForestAssassinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestAssassinClass7Align, ForestAssassinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAssassinClass8Align, ForestAssassinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestAssassinClass9Align, ForestAssassinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestAssassinClasspro = ForestAssassinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAssassinClass10Align, ForestAssassinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.AssassinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestAssassinClasspro.Configure());
            var ForestBarbarianClasspro = ProgressionConfigurator.New(ForestBarbarianClass0Align, ForestBarbarianClass0AlignGuid)
            .SetDisplayName(ForestBarbarianClass0AlignDisplayName)
            .SetDescription(ForestBarbarianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BarbarianClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBarbarianClass2Align, ForestBarbarianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestBarbarianClass3Align, ForestBarbarianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBarbarianClass4Align, ForestBarbarianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestBarbarianClass5Align, ForestBarbarianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBarbarianClass6Align, ForestBarbarianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestBarbarianClass7Align, ForestBarbarianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBarbarianClass8Align, ForestBarbarianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestBarbarianClass9Align, ForestBarbarianClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBarbarianClasspro = ForestBarbarianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBarbarianClass10Align, ForestBarbarianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BarbarianClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBarbarianClasspro.Configure());
            var ForestBardClasspro = ProgressionConfigurator.New(ForestBardClass0Align, ForestBardClass0AlignGuid)
            .SetDisplayName(ForestBardClass0AlignDisplayName)
            .SetDescription(ForestBardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBardClass2Align, ForestBardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestBardClass3Align, ForestBardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBardClass4Align, ForestBardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestBardClass5Align, ForestBardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBardClass6Align, ForestBardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestBardClass7Align, ForestBardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBardClass8Align, ForestBardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestBardClass9Align, ForestBardClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBardClasspro = ForestBardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBardClass10Align, ForestBardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBardClasspro.Configure());
            var ForestBloodragerClasspro = ProgressionConfigurator.New(ForestBloodragerClass0Align, ForestBloodragerClass0AlignGuid)
            .SetDisplayName(ForestBloodragerClass0AlignDisplayName)
            .SetDescription(ForestBloodragerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.BloodragerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBloodragerClass2Align, ForestBloodragerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestBloodragerClass3Align, ForestBloodragerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBloodragerClass4Align, ForestBloodragerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestBloodragerClass5Align, ForestBloodragerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBloodragerClass6Align, ForestBloodragerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestBloodragerClass7Align, ForestBloodragerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBloodragerClass8Align, ForestBloodragerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestBloodragerClass9Align, ForestBloodragerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestBloodragerClasspro = ForestBloodragerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBloodragerClass10Align, ForestBloodragerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.BloodragerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestBloodragerClasspro.Configure());
            var ForestCavalierClasspro = ProgressionConfigurator.New(ForestCavalierClass0Align, ForestCavalierClass0AlignGuid)
            .SetDisplayName(ForestCavalierClass0AlignDisplayName)
            .SetDescription(ForestCavalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.CavalierClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestCavalierClass2Align, ForestCavalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestCavalierClass3Align, ForestCavalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestCavalierClass4Align, ForestCavalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestCavalierClass5Align, ForestCavalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestCavalierClass6Align, ForestCavalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestCavalierClass7Align, ForestCavalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestCavalierClass8Align, ForestCavalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestCavalierClass9Align, ForestCavalierClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestCavalierClasspro = ForestCavalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestCavalierClass10Align, ForestCavalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.CavalierClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestCavalierClasspro.Configure());
            var ForestClericClasspro = ProgressionConfigurator.New(ForestClericClass0Align, ForestClericClass0AlignGuid)
            .SetDisplayName(ForestClericClass0AlignDisplayName)
            .SetDescription(ForestClericClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ClericClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestClericClass2Align, ForestClericClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestClericClass3Align, ForestClericClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestClericClass4Align, ForestClericClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestClericClass5Align, ForestClericClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestClericClass6Align, ForestClericClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestClericClass7Align, ForestClericClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestClericClass8Align, ForestClericClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestClericClass9Align, ForestClericClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestClericClasspro = ForestClericClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestClericClass10Align, ForestClericClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ClericClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestClericClasspro.Configure());
            var ForestDragonDiscipleClasspro = ProgressionConfigurator.New(ForestDragonDiscipleClass0Align, ForestDragonDiscipleClass0AlignGuid)
            .SetDisplayName(ForestDragonDiscipleClass0AlignDisplayName)
            .SetDescription(ForestDragonDiscipleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DragonDiscipleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDragonDiscipleClass2Align, ForestDragonDiscipleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDragonDiscipleClass3Align, ForestDragonDiscipleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDragonDiscipleClass4Align, ForestDragonDiscipleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDragonDiscipleClass5Align, ForestDragonDiscipleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDragonDiscipleClass6Align, ForestDragonDiscipleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDragonDiscipleClass7Align, ForestDragonDiscipleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDragonDiscipleClass8Align, ForestDragonDiscipleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDragonDiscipleClass9Align, ForestDragonDiscipleClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDragonDiscipleClasspro = ForestDragonDiscipleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDragonDiscipleClass10Align, ForestDragonDiscipleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DragonDiscipleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDragonDiscipleClasspro.Configure());
            var ForestDruidClasspro = ProgressionConfigurator.New(ForestDruidClass0Align, ForestDruidClass0AlignGuid)
            .SetDisplayName(ForestDruidClass0AlignDisplayName)
            .SetDescription(ForestDruidClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DruidClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDruidClass2Align, ForestDruidClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDruidClass3Align, ForestDruidClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDruidClass4Align, ForestDruidClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDruidClass5Align, ForestDruidClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDruidClass6Align, ForestDruidClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDruidClass7Align, ForestDruidClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDruidClass8Align, ForestDruidClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDruidClass9Align, ForestDruidClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDruidClasspro = ForestDruidClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDruidClass10Align, ForestDruidClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DruidClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDruidClasspro.Configure());
            var ForestDuelistClasspro = ProgressionConfigurator.New(ForestDuelistClass0Align, ForestDuelistClass0AlignGuid)
            .SetDisplayName(ForestDuelistClass0AlignDisplayName)
            .SetDescription(ForestDuelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.DuelistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDuelistClass2Align, ForestDuelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDuelistClass3Align, ForestDuelistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDuelistClass4Align, ForestDuelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDuelistClass5Align, ForestDuelistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDuelistClass6Align, ForestDuelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDuelistClass7Align, ForestDuelistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDuelistClass8Align, ForestDuelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDuelistClass9Align, ForestDuelistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestDuelistClasspro = ForestDuelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDuelistClass10Align, ForestDuelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.DuelistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestDuelistClasspro.Configure());
            var ForestEldritchKnightClasspro = ProgressionConfigurator.New(ForestEldritchKnightClass0Align, ForestEldritchKnightClass0AlignGuid)
            .SetDisplayName(ForestEldritchKnightClass0AlignDisplayName)
            .SetDescription(ForestEldritchKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchKnightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEldritchKnightClass2Align, ForestEldritchKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestEldritchKnightClass3Align, ForestEldritchKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEldritchKnightClass4Align, ForestEldritchKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestEldritchKnightClass5Align, ForestEldritchKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEldritchKnightClass6Align, ForestEldritchKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestEldritchKnightClass7Align, ForestEldritchKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEldritchKnightClass8Align, ForestEldritchKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestEldritchKnightClass9Align, ForestEldritchKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchKnightClasspro = ForestEldritchKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEldritchKnightClass10Align, ForestEldritchKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchKnightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestEldritchKnightClasspro.Configure());
            var ForestEldritchScionClasspro = ProgressionConfigurator.New(ForestEldritchScionClass0Align, ForestEldritchScionClass0AlignGuid)
            .SetDisplayName(ForestEldritchScionClass0AlignDisplayName)
            .SetDescription(ForestEldritchScionClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.EldritchScionClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEldritchScionClass2Align, ForestEldritchScionClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestEldritchScionClass3Align, ForestEldritchScionClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEldritchScionClass4Align, ForestEldritchScionClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestEldritchScionClass5Align, ForestEldritchScionClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEldritchScionClass6Align, ForestEldritchScionClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestEldritchScionClass7Align, ForestEldritchScionClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEldritchScionClass8Align, ForestEldritchScionClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestEldritchScionClass9Align, ForestEldritchScionClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestEldritchScionClasspro = ForestEldritchScionClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEldritchScionClass10Align, ForestEldritchScionClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.EldritchScionClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestEldritchScionClasspro.Configure());
            var ForestFighterClasspro = ProgressionConfigurator.New(ForestFighterClass0Align, ForestFighterClass0AlignGuid)
            .SetDisplayName(ForestFighterClass0AlignDisplayName)
            .SetDescription(ForestFighterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.FighterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestFighterClass2Align, ForestFighterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestFighterClass3Align, ForestFighterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestFighterClass4Align, ForestFighterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestFighterClass5Align, ForestFighterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestFighterClass6Align, ForestFighterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestFighterClass7Align, ForestFighterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestFighterClass8Align, ForestFighterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestFighterClass9Align, ForestFighterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestFighterClasspro = ForestFighterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestFighterClass10Align, ForestFighterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.FighterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestFighterClasspro.Configure());
            var ForestHellknightClasspro = ProgressionConfigurator.New(ForestHellknightClass0Align, ForestHellknightClass0AlignGuid)
            .SetDisplayName(ForestHellknightClass0AlignDisplayName)
            .SetDescription(ForestHellknightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHellknightClass2Align, ForestHellknightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHellknightClass3Align, ForestHellknightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHellknightClass4Align, ForestHellknightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHellknightClass5Align, ForestHellknightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHellknightClass6Align, ForestHellknightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHellknightClass7Align, ForestHellknightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHellknightClass8Align, ForestHellknightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHellknightClass9Align, ForestHellknightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightClasspro = ForestHellknightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHellknightClass10Align, ForestHellknightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHellknightClasspro.Configure());
            var ForestHellknightSigniferClasspro = ProgressionConfigurator.New(ForestHellknightSigniferClass0Align, ForestHellknightSigniferClass0AlignGuid)
            .SetDisplayName(ForestHellknightSigniferClass0AlignDisplayName)
            .SetDescription(ForestHellknightSigniferClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HellknightSigniferClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHellknightSigniferClass2Align, ForestHellknightSigniferClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHellknightSigniferClass3Align, ForestHellknightSigniferClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHellknightSigniferClass4Align, ForestHellknightSigniferClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHellknightSigniferClass5Align, ForestHellknightSigniferClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHellknightSigniferClass6Align, ForestHellknightSigniferClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHellknightSigniferClass7Align, ForestHellknightSigniferClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHellknightSigniferClass8Align, ForestHellknightSigniferClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHellknightSigniferClass9Align, ForestHellknightSigniferClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHellknightSigniferClasspro = ForestHellknightSigniferClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHellknightSigniferClass10Align, ForestHellknightSigniferClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HellknightSigniferClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHellknightSigniferClasspro.Configure());
            var ForestHunterClasspro = ProgressionConfigurator.New(ForestHunterClass0Align, ForestHunterClass0AlignGuid)
            .SetDisplayName(ForestHunterClass0AlignDisplayName)
            .SetDescription(ForestHunterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.HunterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHunterClass2Align, ForestHunterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHunterClass3Align, ForestHunterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHunterClass4Align, ForestHunterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHunterClass5Align, ForestHunterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHunterClass6Align, ForestHunterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHunterClass7Align, ForestHunterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHunterClass8Align, ForestHunterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHunterClass9Align, ForestHunterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestHunterClasspro = ForestHunterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHunterClass10Align, ForestHunterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.HunterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestHunterClasspro.Configure());
            var ForestInquisitorClasspro = ProgressionConfigurator.New(ForestInquisitorClass0Align, ForestInquisitorClass0AlignGuid)
            .SetDisplayName(ForestInquisitorClass0AlignDisplayName)
            .SetDescription(ForestInquisitorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.InquisitorClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestInquisitorClass2Align, ForestInquisitorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestInquisitorClass3Align, ForestInquisitorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestInquisitorClass4Align, ForestInquisitorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestInquisitorClass5Align, ForestInquisitorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestInquisitorClass6Align, ForestInquisitorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestInquisitorClass7Align, ForestInquisitorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestInquisitorClass8Align, ForestInquisitorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestInquisitorClass9Align, ForestInquisitorClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestInquisitorClasspro = ForestInquisitorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestInquisitorClass10Align, ForestInquisitorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.InquisitorClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestInquisitorClasspro.Configure());
            var ForestKineticistClasspro = ProgressionConfigurator.New(ForestKineticistClass0Align, ForestKineticistClass0AlignGuid)
            .SetDisplayName(ForestKineticistClass0AlignDisplayName)
            .SetDescription(ForestKineticistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestKineticistClass2Align, ForestKineticistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestKineticistClass3Align, ForestKineticistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestKineticistClass4Align, ForestKineticistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestKineticistClass5Align, ForestKineticistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestKineticistClass6Align, ForestKineticistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestKineticistClass7Align, ForestKineticistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestKineticistClass8Align, ForestKineticistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestKineticistClass9Align, ForestKineticistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestKineticistClasspro = ForestKineticistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestKineticistClass10Align, ForestKineticistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.KineticistClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestKineticistClasspro.Configure());
            var ForestLoremasterClasspro = ProgressionConfigurator.New(ForestLoremasterClass0Align, ForestLoremasterClass0AlignGuid)
            .SetDisplayName(ForestLoremasterClass0AlignDisplayName)
            .SetDescription(ForestLoremasterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.LoremasterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestLoremasterClass2Align, ForestLoremasterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestLoremasterClass3Align, ForestLoremasterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestLoremasterClass4Align, ForestLoremasterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestLoremasterClass5Align, ForestLoremasterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestLoremasterClass6Align, ForestLoremasterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestLoremasterClass7Align, ForestLoremasterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestLoremasterClass8Align, ForestLoremasterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestLoremasterClass9Align, ForestLoremasterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestLoremasterClasspro = ForestLoremasterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestLoremasterClass10Align, ForestLoremasterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.LoremasterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestLoremasterClasspro.Configure());
            var ForestMagusClasspro = ProgressionConfigurator.New(ForestMagusClass0Align, ForestMagusClass0AlignGuid)
            .SetDisplayName(ForestMagusClass0AlignDisplayName)
            .SetDescription(ForestMagusClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMagusClass2Align, ForestMagusClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMagusClass3Align, ForestMagusClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMagusClass4Align, ForestMagusClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMagusClass5Align, ForestMagusClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMagusClass6Align, ForestMagusClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMagusClass7Align, ForestMagusClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMagusClass8Align, ForestMagusClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMagusClass9Align, ForestMagusClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMagusClasspro = ForestMagusClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMagusClass10Align, ForestMagusClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MagusClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMagusClasspro.Configure());
            var ForestMonkClasspro = ProgressionConfigurator.New(ForestMonkClass0Align, ForestMonkClass0AlignGuid)
            .SetDisplayName(ForestMonkClass0AlignDisplayName)
            .SetDescription(ForestMonkClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MonkClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMonkClass2Align, ForestMonkClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMonkClass3Align, ForestMonkClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMonkClass4Align, ForestMonkClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMonkClass5Align, ForestMonkClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMonkClass6Align, ForestMonkClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMonkClass7Align, ForestMonkClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMonkClass8Align, ForestMonkClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMonkClass9Align, ForestMonkClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMonkClasspro = ForestMonkClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMonkClass10Align, ForestMonkClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MonkClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMonkClasspro.Configure());
            var ForestMysticTheurgeClasspro = ProgressionConfigurator.New(ForestMysticTheurgeClass0Align, ForestMysticTheurgeClass0AlignGuid)
            .SetDisplayName(ForestMysticTheurgeClass0AlignDisplayName)
            .SetDescription(ForestMysticTheurgeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.MysticTheurgeClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMysticTheurgeClass2Align, ForestMysticTheurgeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMysticTheurgeClass3Align, ForestMysticTheurgeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMysticTheurgeClass4Align, ForestMysticTheurgeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMysticTheurgeClass5Align, ForestMysticTheurgeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMysticTheurgeClass6Align, ForestMysticTheurgeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMysticTheurgeClass7Align, ForestMysticTheurgeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMysticTheurgeClass8Align, ForestMysticTheurgeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMysticTheurgeClass9Align, ForestMysticTheurgeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestMysticTheurgeClasspro = ForestMysticTheurgeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMysticTheurgeClass10Align, ForestMysticTheurgeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.MysticTheurgeClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestMysticTheurgeClasspro.Configure());
            var ForestOracleClasspro = ProgressionConfigurator.New(ForestOracleClass0Align, ForestOracleClass0AlignGuid)
            .SetDisplayName(ForestOracleClass0AlignDisplayName)
            .SetDescription(ForestOracleClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.OracleClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestOracleClass2Align, ForestOracleClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestOracleClass3Align, ForestOracleClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestOracleClass4Align, ForestOracleClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestOracleClass5Align, ForestOracleClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestOracleClass6Align, ForestOracleClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestOracleClass7Align, ForestOracleClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestOracleClass8Align, ForestOracleClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestOracleClass9Align, ForestOracleClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestOracleClasspro = ForestOracleClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestOracleClass10Align, ForestOracleClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.OracleClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestOracleClasspro.Configure());
            var ForestPaladinClasspro = ProgressionConfigurator.New(ForestPaladinClass0Align, ForestPaladinClass0AlignGuid)
            .SetDisplayName(ForestPaladinClass0AlignDisplayName)
            .SetDescription(ForestPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.PaladinClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestPaladinClass2Align, ForestPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestPaladinClass3Align, ForestPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestPaladinClass4Align, ForestPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestPaladinClass5Align, ForestPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestPaladinClass6Align, ForestPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestPaladinClass7Align, ForestPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestPaladinClass8Align, ForestPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestPaladinClass9Align, ForestPaladinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestPaladinClasspro = ForestPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestPaladinClass10Align, ForestPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.PaladinClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestPaladinClasspro.Configure());
            var ForestRangerClasspro = ProgressionConfigurator.New(ForestRangerClass0Align, ForestRangerClass0AlignGuid)
            .SetDisplayName(ForestRangerClass0AlignDisplayName)
            .SetDescription(ForestRangerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RangerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestRangerClass2Align, ForestRangerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestRangerClass3Align, ForestRangerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestRangerClass4Align, ForestRangerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestRangerClass5Align, ForestRangerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestRangerClass6Align, ForestRangerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestRangerClass7Align, ForestRangerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestRangerClass8Align, ForestRangerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestRangerClass9Align, ForestRangerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRangerClasspro = ForestRangerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestRangerClass10Align, ForestRangerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RangerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestRangerClasspro.Configure());
            var ForestRogueClasspro = ProgressionConfigurator.New(ForestRogueClass0Align, ForestRogueClass0AlignGuid)
            .SetDisplayName(ForestRogueClass0AlignDisplayName)
            .SetDescription(ForestRogueClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.RogueClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestRogueClass2Align, ForestRogueClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestRogueClass3Align, ForestRogueClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestRogueClass4Align, ForestRogueClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestRogueClass5Align, ForestRogueClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestRogueClass6Align, ForestRogueClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestRogueClass7Align, ForestRogueClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestRogueClass8Align, ForestRogueClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestRogueClass9Align, ForestRogueClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestRogueClasspro = ForestRogueClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestRogueClass10Align, ForestRogueClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.RogueClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestRogueClasspro.Configure());
            var ForestShamanClasspro = ProgressionConfigurator.New(ForestShamanClass0Align, ForestShamanClass0AlignGuid)
            .SetDisplayName(ForestShamanClass0AlignDisplayName)
            .SetDescription(ForestShamanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShamanClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShamanClass2Align, ForestShamanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestShamanClass3Align, ForestShamanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShamanClass4Align, ForestShamanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestShamanClass5Align, ForestShamanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShamanClass6Align, ForestShamanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestShamanClass7Align, ForestShamanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShamanClass8Align, ForestShamanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestShamanClass9Align, ForestShamanClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShamanClasspro = ForestShamanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShamanClass10Align, ForestShamanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShamanClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestShamanClasspro.Configure());
            var ForestShifterClasspro = ProgressionConfigurator.New(ForestShifterClass0Align, ForestShifterClass0AlignGuid)
            .SetDisplayName(ForestShifterClass0AlignDisplayName)
            .SetDescription(ForestShifterClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.ShifterClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShifterClass2Align, ForestShifterClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestShifterClass3Align, ForestShifterClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShifterClass4Align, ForestShifterClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestShifterClass5Align, ForestShifterClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShifterClass6Align, ForestShifterClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestShifterClass7Align, ForestShifterClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShifterClass8Align, ForestShifterClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestShifterClass9Align, ForestShifterClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestShifterClasspro = ForestShifterClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShifterClass10Align, ForestShifterClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.ShifterClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestShifterClasspro.Configure());
            var ForestSkaldClasspro = ProgressionConfigurator.New(ForestSkaldClass0Align, ForestSkaldClass0AlignGuid)
            .SetDisplayName(ForestSkaldClass0AlignDisplayName)
            .SetDescription(ForestSkaldClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SkaldClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSkaldClass2Align, ForestSkaldClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSkaldClass3Align, ForestSkaldClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSkaldClass4Align, ForestSkaldClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSkaldClass5Align, ForestSkaldClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSkaldClass6Align, ForestSkaldClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSkaldClass7Align, ForestSkaldClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSkaldClass8Align, ForestSkaldClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSkaldClass9Align, ForestSkaldClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSkaldClasspro = ForestSkaldClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSkaldClass10Align, ForestSkaldClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SkaldClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSkaldClasspro.Configure());
            var ForestSlayerClasspro = ProgressionConfigurator.New(ForestSlayerClass0Align, ForestSlayerClass0AlignGuid)
            .SetDisplayName(ForestSlayerClass0AlignDisplayName)
            .SetDescription(ForestSlayerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SlayerClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSlayerClass2Align, ForestSlayerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSlayerClass3Align, ForestSlayerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSlayerClass4Align, ForestSlayerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSlayerClass5Align, ForestSlayerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSlayerClass6Align, ForestSlayerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSlayerClass7Align, ForestSlayerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSlayerClass8Align, ForestSlayerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSlayerClass9Align, ForestSlayerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSlayerClasspro = ForestSlayerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSlayerClass10Align, ForestSlayerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SlayerClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSlayerClasspro.Configure());
            var ForestSorcererClasspro = ProgressionConfigurator.New(ForestSorcererClass0Align, ForestSorcererClass0AlignGuid)
            .SetDisplayName(ForestSorcererClass0AlignDisplayName)
            .SetDescription(ForestSorcererClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SorcererClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSorcererClass2Align, ForestSorcererClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSorcererClass3Align, ForestSorcererClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSorcererClass4Align, ForestSorcererClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSorcererClass5Align, ForestSorcererClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSorcererClass6Align, ForestSorcererClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSorcererClass7Align, ForestSorcererClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSorcererClass8Align, ForestSorcererClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSorcererClass9Align, ForestSorcererClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSorcererClasspro = ForestSorcererClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSorcererClass10Align, ForestSorcererClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SorcererClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSorcererClasspro.Configure());
            var ForestStalwartDefenderClasspro = ProgressionConfigurator.New(ForestStalwartDefenderClass0Align, ForestStalwartDefenderClass0AlignGuid)
            .SetDisplayName(ForestStalwartDefenderClass0AlignDisplayName)
            .SetDescription(ForestStalwartDefenderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StalwartDefenderClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStalwartDefenderClass2Align, ForestStalwartDefenderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestStalwartDefenderClass3Align, ForestStalwartDefenderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStalwartDefenderClass4Align, ForestStalwartDefenderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestStalwartDefenderClass5Align, ForestStalwartDefenderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStalwartDefenderClass6Align, ForestStalwartDefenderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestStalwartDefenderClass7Align, ForestStalwartDefenderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStalwartDefenderClass8Align, ForestStalwartDefenderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestStalwartDefenderClass9Align, ForestStalwartDefenderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStalwartDefenderClasspro = ForestStalwartDefenderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStalwartDefenderClass10Align, ForestStalwartDefenderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StalwartDefenderClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestStalwartDefenderClasspro.Configure());
            var ForestStudentOfWarClasspro = ProgressionConfigurator.New(ForestStudentOfWarClass0Align, ForestStudentOfWarClass0AlignGuid)
            .SetDisplayName(ForestStudentOfWarClass0AlignDisplayName)
            .SetDescription(ForestStudentOfWarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.StudentOfWarClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStudentOfWarClass2Align, ForestStudentOfWarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestStudentOfWarClass3Align, ForestStudentOfWarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStudentOfWarClass4Align, ForestStudentOfWarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestStudentOfWarClass5Align, ForestStudentOfWarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStudentOfWarClass6Align, ForestStudentOfWarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestStudentOfWarClass7Align, ForestStudentOfWarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStudentOfWarClass8Align, ForestStudentOfWarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestStudentOfWarClass9Align, ForestStudentOfWarClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestStudentOfWarClasspro = ForestStudentOfWarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStudentOfWarClass10Align, ForestStudentOfWarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.StudentOfWarClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestStudentOfWarClasspro.Configure());
            var ForestSwordlordClasspro = ProgressionConfigurator.New(ForestSwordlordClass0Align, ForestSwordlordClass0AlignGuid)
            .SetDisplayName(ForestSwordlordClass0AlignDisplayName)
            .SetDescription(ForestSwordlordClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.SwordlordClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSwordlordClass2Align, ForestSwordlordClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSwordlordClass3Align, ForestSwordlordClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSwordlordClass4Align, ForestSwordlordClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSwordlordClass5Align, ForestSwordlordClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSwordlordClass6Align, ForestSwordlordClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSwordlordClass7Align, ForestSwordlordClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSwordlordClass8Align, ForestSwordlordClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSwordlordClass9Align, ForestSwordlordClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestSwordlordClasspro = ForestSwordlordClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSwordlordClass10Align, ForestSwordlordClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.SwordlordClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestSwordlordClasspro.Configure());
            var ForestWarpriestClasspro = ProgressionConfigurator.New(ForestWarpriestClass0Align, ForestWarpriestClass0AlignGuid)
            .SetDisplayName(ForestWarpriestClass0AlignDisplayName)
            .SetDescription(ForestWarpriestClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WarpriestClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWarpriestClass2Align, ForestWarpriestClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestWarpriestClass3Align, ForestWarpriestClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWarpriestClass4Align, ForestWarpriestClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestWarpriestClass5Align, ForestWarpriestClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWarpriestClass6Align, ForestWarpriestClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestWarpriestClass7Align, ForestWarpriestClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWarpriestClass8Align, ForestWarpriestClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestWarpriestClass9Align, ForestWarpriestClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWarpriestClasspro = ForestWarpriestClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWarpriestClass10Align, ForestWarpriestClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WarpriestClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWarpriestClasspro.Configure());
            var ForestWinterWitchClasspro = ProgressionConfigurator.New(ForestWinterWitchClass0Align, ForestWinterWitchClass0AlignGuid)
            .SetDisplayName(ForestWinterWitchClass0AlignDisplayName)
            .SetDescription(ForestWinterWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WinterWitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWinterWitchClass2Align, ForestWinterWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestWinterWitchClass3Align, ForestWinterWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWinterWitchClass4Align, ForestWinterWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestWinterWitchClass5Align, ForestWinterWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWinterWitchClass6Align, ForestWinterWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestWinterWitchClass7Align, ForestWinterWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWinterWitchClass8Align, ForestWinterWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestWinterWitchClass9Align, ForestWinterWitchClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWinterWitchClasspro = ForestWinterWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWinterWitchClass10Align, ForestWinterWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WinterWitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWinterWitchClasspro.Configure());
            var ForestWitchClasspro = ProgressionConfigurator.New(ForestWitchClass0Align, ForestWitchClass0AlignGuid)
            .SetDisplayName(ForestWitchClass0AlignDisplayName)
            .SetDescription(ForestWitchClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WitchClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWitchClass2Align, ForestWitchClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestWitchClass3Align, ForestWitchClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWitchClass4Align, ForestWitchClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestWitchClass5Align, ForestWitchClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWitchClass6Align, ForestWitchClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestWitchClass7Align, ForestWitchClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWitchClass8Align, ForestWitchClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestWitchClass9Align, ForestWitchClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWitchClasspro = ForestWitchClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWitchClass10Align, ForestWitchClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WitchClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWitchClasspro.Configure());
            var ForestWizardClasspro = ProgressionConfigurator.New(ForestWizardClass0Align, ForestWizardClass0AlignGuid)
            .SetDisplayName(ForestWizardClass0AlignDisplayName)
            .SetDescription(ForestWizardClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CharacterClassRefs.WizardClass.ToString(), 1)
            .SetHideNotAvailibleInUI(true);
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestWizardClass2Align, ForestWizardClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestWizardClass3Align, ForestWizardClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestWizardClass4Align, ForestWizardClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestWizardClass5Align, ForestWizardClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestWizardClass6Align, ForestWizardClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestWizardClass7Align, ForestWizardClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestWizardClass8Align, ForestWizardClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestWizardClass9Align, ForestWizardClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            ForestWizardClasspro = ForestWizardClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestWizardClass10Align, ForestWizardClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CharacterClassRefs.WizardClass.ToString(); })
            .SetHideInUI(true).Configure());
            list.Add(ForestWizardClasspro.Configure());
            var ForestGunslingerClasspro = ProgressionConfigurator.New(ForestGunslingerClass0Align, ForestGunslingerClass0AlignGuid)
            .SetDisplayName(ForestGunslingerClass0AlignDisplayName)
            .SetDescription(ForestGunslingerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GunslingerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestGunslingerClass2Align, ForestGunslingerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestGunslingerClass3Align, ForestGunslingerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestGunslingerClass4Align, ForestGunslingerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestGunslingerClass5Align, ForestGunslingerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestGunslingerClass6Align, ForestGunslingerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestGunslingerClass7Align, ForestGunslingerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestGunslingerClass8Align, ForestGunslingerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestGunslingerClass9Align, ForestGunslingerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            ForestGunslingerClasspro = ForestGunslingerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestGunslingerClass10Align, ForestGunslingerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GunslingerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestGunslingerClasspro.Configure());
            var ForestAgentoftheGraveClasspro = ProgressionConfigurator.New(ForestAgentoftheGraveClass0Align, ForestAgentoftheGraveClass0AlignGuid)
            .SetDisplayName(ForestAgentoftheGraveClass0AlignDisplayName)
            .SetDescription(ForestAgentoftheGraveClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AgentoftheGraveClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAgentoftheGraveClass2Align, ForestAgentoftheGraveClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestAgentoftheGraveClass3Align, ForestAgentoftheGraveClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAgentoftheGraveClass4Align, ForestAgentoftheGraveClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestAgentoftheGraveClass5Align, ForestAgentoftheGraveClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAgentoftheGraveClass6Align, ForestAgentoftheGraveClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestAgentoftheGraveClass7Align, ForestAgentoftheGraveClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAgentoftheGraveClass8Align, ForestAgentoftheGraveClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestAgentoftheGraveClass9Align, ForestAgentoftheGraveClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            ForestAgentoftheGraveClasspro = ForestAgentoftheGraveClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAgentoftheGraveClass10Align, ForestAgentoftheGraveClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AgentoftheGraveClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAgentoftheGraveClasspro.Configure());
            var ForestAnchoriteofDawnClasspro = ProgressionConfigurator.New(ForestAnchoriteofDawnClass0Align, ForestAnchoriteofDawnClass0AlignGuid)
            .SetDisplayName(ForestAnchoriteofDawnClass0AlignDisplayName)
            .SetDescription(ForestAnchoriteofDawnClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AnchoriteofDawnClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass2Align, ForestAnchoriteofDawnClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass3Align, ForestAnchoriteofDawnClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass4Align, ForestAnchoriteofDawnClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass5Align, ForestAnchoriteofDawnClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass6Align, ForestAnchoriteofDawnClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass7Align, ForestAnchoriteofDawnClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass8Align, ForestAnchoriteofDawnClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass9Align, ForestAnchoriteofDawnClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            ForestAnchoriteofDawnClasspro = ForestAnchoriteofDawnClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAnchoriteofDawnClass10Align, ForestAnchoriteofDawnClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AnchoriteofDawnClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAnchoriteofDawnClasspro.Configure());
            var ForestArcaneAcherClasspro = ProgressionConfigurator.New(ForestArcaneAcherClass0Align, ForestArcaneAcherClass0AlignGuid)
            .SetDisplayName(ForestArcaneAcherClass0AlignDisplayName)
            .SetDescription(ForestArcaneAcherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ArcaneAcherClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestArcaneAcherClass2Align, ForestArcaneAcherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestArcaneAcherClass3Align, ForestArcaneAcherClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestArcaneAcherClass4Align, ForestArcaneAcherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestArcaneAcherClass5Align, ForestArcaneAcherClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestArcaneAcherClass6Align, ForestArcaneAcherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestArcaneAcherClass7Align, ForestArcaneAcherClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestArcaneAcherClass8Align, ForestArcaneAcherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestArcaneAcherClass9Align, ForestArcaneAcherClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            ForestArcaneAcherClasspro = ForestArcaneAcherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestArcaneAcherClass10Align, ForestArcaneAcherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ArcaneAcherClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestArcaneAcherClasspro.Configure());
            var ForestAsavirClasspro = ProgressionConfigurator.New(ForestAsavirClass0Align, ForestAsavirClass0AlignGuid)
            .SetDisplayName(ForestAsavirClass0AlignDisplayName)
            .SetDescription(ForestAsavirClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(AsavirClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestAsavirClass2Align, ForestAsavirClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestAsavirClass3Align, ForestAsavirClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestAsavirClass4Align, ForestAsavirClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestAsavirClass5Align, ForestAsavirClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestAsavirClass6Align, ForestAsavirClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestAsavirClass7Align, ForestAsavirClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestAsavirClass8Align, ForestAsavirClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestAsavirClass9Align, ForestAsavirClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            ForestAsavirClasspro = ForestAsavirClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestAsavirClass10Align, ForestAsavirClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = AsavirClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestAsavirClasspro.Configure());
            var ForestChevalierClasspro = ProgressionConfigurator.New(ForestChevalierClass0Align, ForestChevalierClass0AlignGuid)
            .SetDisplayName(ForestChevalierClass0AlignDisplayName)
            .SetDescription(ForestChevalierClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ChevalierClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestChevalierClass2Align, ForestChevalierClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestChevalierClass3Align, ForestChevalierClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestChevalierClass4Align, ForestChevalierClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestChevalierClass5Align, ForestChevalierClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestChevalierClass6Align, ForestChevalierClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestChevalierClass7Align, ForestChevalierClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestChevalierClass8Align, ForestChevalierClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestChevalierClass9Align, ForestChevalierClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            ForestChevalierClasspro = ForestChevalierClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestChevalierClass10Align, ForestChevalierClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ChevalierClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestChevalierClasspro.Configure());
            var ForestCrimsonTemplarClasspro = ProgressionConfigurator.New(ForestCrimsonTemplarClass0Align, ForestCrimsonTemplarClass0AlignGuid)
            .SetDisplayName(ForestCrimsonTemplarClass0AlignDisplayName)
            .SetDescription(ForestCrimsonTemplarClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(CrimsonTemplarClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestCrimsonTemplarClass2Align, ForestCrimsonTemplarClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestCrimsonTemplarClass3Align, ForestCrimsonTemplarClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestCrimsonTemplarClass4Align, ForestCrimsonTemplarClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestCrimsonTemplarClass5Align, ForestCrimsonTemplarClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestCrimsonTemplarClass6Align, ForestCrimsonTemplarClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestCrimsonTemplarClass7Align, ForestCrimsonTemplarClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestCrimsonTemplarClass8Align, ForestCrimsonTemplarClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestCrimsonTemplarClass9Align, ForestCrimsonTemplarClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            ForestCrimsonTemplarClasspro = ForestCrimsonTemplarClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestCrimsonTemplarClass10Align, ForestCrimsonTemplarClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = CrimsonTemplarClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestCrimsonTemplarClasspro.Configure());
            var ForestDeadeyeDevoteeClasspro = ProgressionConfigurator.New(ForestDeadeyeDevoteeClass0Align, ForestDeadeyeDevoteeClass0AlignGuid)
            .SetDisplayName(ForestDeadeyeDevoteeClass0AlignDisplayName)
            .SetDescription(ForestDeadeyeDevoteeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DeadeyeDevoteeClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass2Align, ForestDeadeyeDevoteeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass3Align, ForestDeadeyeDevoteeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass4Align, ForestDeadeyeDevoteeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass5Align, ForestDeadeyeDevoteeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass6Align, ForestDeadeyeDevoteeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass7Align, ForestDeadeyeDevoteeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass8Align, ForestDeadeyeDevoteeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass9Align, ForestDeadeyeDevoteeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            ForestDeadeyeDevoteeClasspro = ForestDeadeyeDevoteeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDeadeyeDevoteeClass10Align, ForestDeadeyeDevoteeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DeadeyeDevoteeClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDeadeyeDevoteeClasspro.Configure());
            var ForestDragonFuryClasspro = ProgressionConfigurator.New(ForestDragonFuryClass0Align, ForestDragonFuryClass0AlignGuid)
            .SetDisplayName(ForestDragonFuryClass0AlignDisplayName)
            .SetDescription(ForestDragonFuryClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DragonFuryClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDragonFuryClass2Align, ForestDragonFuryClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDragonFuryClass3Align, ForestDragonFuryClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDragonFuryClass4Align, ForestDragonFuryClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDragonFuryClass5Align, ForestDragonFuryClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDragonFuryClass6Align, ForestDragonFuryClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDragonFuryClass7Align, ForestDragonFuryClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDragonFuryClass8Align, ForestDragonFuryClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDragonFuryClass9Align, ForestDragonFuryClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            ForestDragonFuryClasspro = ForestDragonFuryClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDragonFuryClass10Align, ForestDragonFuryClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DragonFuryClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDragonFuryClasspro.Configure());
            var ForestEsotericKnightClasspro = ProgressionConfigurator.New(ForestEsotericKnightClass0Align, ForestEsotericKnightClass0AlignGuid)
            .SetDisplayName(ForestEsotericKnightClass0AlignDisplayName)
            .SetDescription(ForestEsotericKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EsotericKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEsotericKnightClass2Align, ForestEsotericKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestEsotericKnightClass3Align, ForestEsotericKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEsotericKnightClass4Align, ForestEsotericKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestEsotericKnightClass5Align, ForestEsotericKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEsotericKnightClass6Align, ForestEsotericKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestEsotericKnightClass7Align, ForestEsotericKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEsotericKnightClass8Align, ForestEsotericKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestEsotericKnightClass9Align, ForestEsotericKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            ForestEsotericKnightClasspro = ForestEsotericKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEsotericKnightClass10Align, ForestEsotericKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EsotericKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestEsotericKnightClasspro.Configure());
            var ForestExaltedEvangelistClasspro = ProgressionConfigurator.New(ForestExaltedEvangelistClass0Align, ForestExaltedEvangelistClass0AlignGuid)
            .SetDisplayName(ForestExaltedEvangelistClass0AlignDisplayName)
            .SetDescription(ForestExaltedEvangelistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ExaltedEvangelistClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestExaltedEvangelistClass2Align, ForestExaltedEvangelistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestExaltedEvangelistClass3Align, ForestExaltedEvangelistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestExaltedEvangelistClass4Align, ForestExaltedEvangelistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestExaltedEvangelistClass5Align, ForestExaltedEvangelistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestExaltedEvangelistClass6Align, ForestExaltedEvangelistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestExaltedEvangelistClass7Align, ForestExaltedEvangelistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestExaltedEvangelistClass8Align, ForestExaltedEvangelistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestExaltedEvangelistClass9Align, ForestExaltedEvangelistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            ForestExaltedEvangelistClasspro = ForestExaltedEvangelistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestExaltedEvangelistClass10Align, ForestExaltedEvangelistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ExaltedEvangelistClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestExaltedEvangelistClasspro.Configure());
            var ForestFuriousGuardianClasspro = ProgressionConfigurator.New(ForestFuriousGuardianClass0Align, ForestFuriousGuardianClass0AlignGuid)
            .SetDisplayName(ForestFuriousGuardianClass0AlignDisplayName)
            .SetDescription(ForestFuriousGuardianClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(FuriousGuardianClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestFuriousGuardianClass2Align, ForestFuriousGuardianClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestFuriousGuardianClass3Align, ForestFuriousGuardianClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestFuriousGuardianClass4Align, ForestFuriousGuardianClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestFuriousGuardianClass5Align, ForestFuriousGuardianClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestFuriousGuardianClass6Align, ForestFuriousGuardianClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestFuriousGuardianClass7Align, ForestFuriousGuardianClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestFuriousGuardianClass8Align, ForestFuriousGuardianClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestFuriousGuardianClass9Align, ForestFuriousGuardianClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            ForestFuriousGuardianClasspro = ForestFuriousGuardianClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestFuriousGuardianClass10Align, ForestFuriousGuardianClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = FuriousGuardianClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestFuriousGuardianClasspro.Configure());
            var ForestHalflingOpportunistClasspro = ProgressionConfigurator.New(ForestHalflingOpportunistClass0Align, ForestHalflingOpportunistClass0AlignGuid)
            .SetDisplayName(ForestHalflingOpportunistClass0AlignDisplayName)
            .SetDescription(ForestHalflingOpportunistClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HalflingOpportunistClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHalflingOpportunistClass2Align, ForestHalflingOpportunistClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHalflingOpportunistClass3Align, ForestHalflingOpportunistClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHalflingOpportunistClass4Align, ForestHalflingOpportunistClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHalflingOpportunistClass5Align, ForestHalflingOpportunistClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHalflingOpportunistClass6Align, ForestHalflingOpportunistClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHalflingOpportunistClass7Align, ForestHalflingOpportunistClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHalflingOpportunistClass8Align, ForestHalflingOpportunistClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHalflingOpportunistClass9Align, ForestHalflingOpportunistClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            ForestHalflingOpportunistClasspro = ForestHalflingOpportunistClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHalflingOpportunistClass10Align, ForestHalflingOpportunistClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HalflingOpportunistClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHalflingOpportunistClasspro.Configure());
            var ForestHinterlanderClasspro = ProgressionConfigurator.New(ForestHinterlanderClass0Align, ForestHinterlanderClass0AlignGuid)
            .SetDisplayName(ForestHinterlanderClass0AlignDisplayName)
            .SetDescription(ForestHinterlanderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HinterlanderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHinterlanderClass2Align, ForestHinterlanderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHinterlanderClass3Align, ForestHinterlanderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHinterlanderClass4Align, ForestHinterlanderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHinterlanderClass5Align, ForestHinterlanderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHinterlanderClass6Align, ForestHinterlanderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHinterlanderClass7Align, ForestHinterlanderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHinterlanderClass8Align, ForestHinterlanderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHinterlanderClass9Align, ForestHinterlanderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            ForestHinterlanderClasspro = ForestHinterlanderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHinterlanderClass10Align, ForestHinterlanderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HinterlanderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHinterlanderClasspro.Configure());
            var ForestHorizonWalkerClasspro = ProgressionConfigurator.New(ForestHorizonWalkerClass0Align, ForestHorizonWalkerClass0AlignGuid)
            .SetDisplayName(ForestHorizonWalkerClass0AlignDisplayName)
            .SetDescription(ForestHorizonWalkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HorizonWalkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHorizonWalkerClass2Align, ForestHorizonWalkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHorizonWalkerClass3Align, ForestHorizonWalkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHorizonWalkerClass4Align, ForestHorizonWalkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHorizonWalkerClass5Align, ForestHorizonWalkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHorizonWalkerClass6Align, ForestHorizonWalkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHorizonWalkerClass7Align, ForestHorizonWalkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHorizonWalkerClass8Align, ForestHorizonWalkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHorizonWalkerClass9Align, ForestHorizonWalkerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            ForestHorizonWalkerClasspro = ForestHorizonWalkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHorizonWalkerClass10Align, ForestHorizonWalkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HorizonWalkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHorizonWalkerClasspro.Configure());
            var ForestInheritorCrusaderClasspro = ProgressionConfigurator.New(ForestInheritorCrusaderClass0Align, ForestInheritorCrusaderClass0AlignGuid)
            .SetDisplayName(ForestInheritorCrusaderClass0AlignDisplayName)
            .SetDescription(ForestInheritorCrusaderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(InheritorCrusaderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestInheritorCrusaderClass2Align, ForestInheritorCrusaderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestInheritorCrusaderClass3Align, ForestInheritorCrusaderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestInheritorCrusaderClass4Align, ForestInheritorCrusaderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestInheritorCrusaderClass5Align, ForestInheritorCrusaderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestInheritorCrusaderClass6Align, ForestInheritorCrusaderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestInheritorCrusaderClass7Align, ForestInheritorCrusaderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestInheritorCrusaderClass8Align, ForestInheritorCrusaderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestInheritorCrusaderClass9Align, ForestInheritorCrusaderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            ForestInheritorCrusaderClasspro = ForestInheritorCrusaderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestInheritorCrusaderClass10Align, ForestInheritorCrusaderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = InheritorCrusaderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestInheritorCrusaderClasspro.Configure());
            var ForestMammothRiderClasspro = ProgressionConfigurator.New(ForestMammothRiderClass0Align, ForestMammothRiderClass0AlignGuid)
            .SetDisplayName(ForestMammothRiderClass0AlignDisplayName)
            .SetDescription(ForestMammothRiderClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MammothRiderClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMammothRiderClass2Align, ForestMammothRiderClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMammothRiderClass3Align, ForestMammothRiderClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMammothRiderClass4Align, ForestMammothRiderClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMammothRiderClass5Align, ForestMammothRiderClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMammothRiderClass6Align, ForestMammothRiderClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMammothRiderClass7Align, ForestMammothRiderClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMammothRiderClass8Align, ForestMammothRiderClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMammothRiderClass9Align, ForestMammothRiderClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            ForestMammothRiderClasspro = ForestMammothRiderClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMammothRiderClass10Align, ForestMammothRiderClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MammothRiderClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMammothRiderClasspro.Configure());
            var ForestSanguineAngelClasspro = ProgressionConfigurator.New(ForestSanguineAngelClass0Align, ForestSanguineAngelClass0AlignGuid)
            .SetDisplayName(ForestSanguineAngelClass0AlignDisplayName)
            .SetDescription(ForestSanguineAngelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SanguineAngelClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSanguineAngelClass2Align, ForestSanguineAngelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSanguineAngelClass3Align, ForestSanguineAngelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSanguineAngelClass4Align, ForestSanguineAngelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSanguineAngelClass5Align, ForestSanguineAngelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSanguineAngelClass6Align, ForestSanguineAngelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSanguineAngelClass7Align, ForestSanguineAngelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSanguineAngelClass8Align, ForestSanguineAngelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSanguineAngelClass9Align, ForestSanguineAngelClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            ForestSanguineAngelClasspro = ForestSanguineAngelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSanguineAngelClass10Align, ForestSanguineAngelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SanguineAngelClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSanguineAngelClasspro.Configure());
            var ForestScarSeekerClasspro = ProgressionConfigurator.New(ForestScarSeekerClass0Align, ForestScarSeekerClass0AlignGuid)
            .SetDisplayName(ForestScarSeekerClass0AlignDisplayName)
            .SetDescription(ForestScarSeekerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ScarSeekerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestScarSeekerClass2Align, ForestScarSeekerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestScarSeekerClass3Align, ForestScarSeekerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestScarSeekerClass4Align, ForestScarSeekerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestScarSeekerClass5Align, ForestScarSeekerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestScarSeekerClass6Align, ForestScarSeekerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestScarSeekerClass7Align, ForestScarSeekerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestScarSeekerClass8Align, ForestScarSeekerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestScarSeekerClass9Align, ForestScarSeekerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            ForestScarSeekerClasspro = ForestScarSeekerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestScarSeekerClass10Align, ForestScarSeekerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ScarSeekerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestScarSeekerClasspro.Configure());
            var ForestSentinelClasspro = ProgressionConfigurator.New(ForestSentinelClass0Align, ForestSentinelClass0AlignGuid)
            .SetDisplayName(ForestSentinelClass0AlignDisplayName)
            .SetDescription(ForestSentinelClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SentinelClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSentinelClass2Align, ForestSentinelClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSentinelClass3Align, ForestSentinelClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSentinelClass4Align, ForestSentinelClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSentinelClass5Align, ForestSentinelClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSentinelClass6Align, ForestSentinelClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSentinelClass7Align, ForestSentinelClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSentinelClass8Align, ForestSentinelClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSentinelClass9Align, ForestSentinelClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            ForestSentinelClasspro = ForestSentinelClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSentinelClass10Align, ForestSentinelClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SentinelClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSentinelClasspro.Configure());
            var ForestShadowDancerClasspro = ProgressionConfigurator.New(ForestShadowDancerClass0Align, ForestShadowDancerClass0AlignGuid)
            .SetDisplayName(ForestShadowDancerClass0AlignDisplayName)
            .SetDescription(ForestShadowDancerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(ShadowDancerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestShadowDancerClass2Align, ForestShadowDancerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestShadowDancerClass3Align, ForestShadowDancerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestShadowDancerClass4Align, ForestShadowDancerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestShadowDancerClass5Align, ForestShadowDancerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestShadowDancerClass6Align, ForestShadowDancerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestShadowDancerClass7Align, ForestShadowDancerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestShadowDancerClass8Align, ForestShadowDancerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestShadowDancerClass9Align, ForestShadowDancerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            ForestShadowDancerClasspro = ForestShadowDancerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestShadowDancerClass10Align, ForestShadowDancerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = ShadowDancerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestShadowDancerClasspro.Configure());
            var ForestSouldrinkerClasspro = ProgressionConfigurator.New(ForestSouldrinkerClass0Align, ForestSouldrinkerClass0AlignGuid)
            .SetDisplayName(ForestSouldrinkerClass0AlignDisplayName)
            .SetDescription(ForestSouldrinkerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SouldrinkerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSouldrinkerClass2Align, ForestSouldrinkerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSouldrinkerClass3Align, ForestSouldrinkerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSouldrinkerClass4Align, ForestSouldrinkerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSouldrinkerClass5Align, ForestSouldrinkerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSouldrinkerClass6Align, ForestSouldrinkerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSouldrinkerClass7Align, ForestSouldrinkerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSouldrinkerClass8Align, ForestSouldrinkerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSouldrinkerClass9Align, ForestSouldrinkerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            ForestSouldrinkerClasspro = ForestSouldrinkerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSouldrinkerClass10Align, ForestSouldrinkerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SouldrinkerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSouldrinkerClasspro.Configure());
            var ForestUmbralAgentClasspro = ProgressionConfigurator.New(ForestUmbralAgentClass0Align, ForestUmbralAgentClass0AlignGuid)
            .SetDisplayName(ForestUmbralAgentClass0AlignDisplayName)
            .SetDescription(ForestUmbralAgentClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(UmbralAgentClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestUmbralAgentClass2Align, ForestUmbralAgentClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestUmbralAgentClass3Align, ForestUmbralAgentClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestUmbralAgentClass4Align, ForestUmbralAgentClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestUmbralAgentClass5Align, ForestUmbralAgentClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestUmbralAgentClass6Align, ForestUmbralAgentClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestUmbralAgentClass7Align, ForestUmbralAgentClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestUmbralAgentClass8Align, ForestUmbralAgentClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestUmbralAgentClass9Align, ForestUmbralAgentClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            ForestUmbralAgentClasspro = ForestUmbralAgentClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestUmbralAgentClass10Align, ForestUmbralAgentClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = UmbralAgentClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestUmbralAgentClasspro.Configure());
            var ForestMicroAntiPaladinClasspro = ProgressionConfigurator.New(ForestMicroAntiPaladinClass0Align, ForestMicroAntiPaladinClass0AlignGuid)
            .SetDisplayName(ForestMicroAntiPaladinClass0AlignDisplayName)
            .SetDescription(ForestMicroAntiPaladinClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MicroAntiPaladinClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass2Align, ForestMicroAntiPaladinClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass3Align, ForestMicroAntiPaladinClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass4Align, ForestMicroAntiPaladinClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass5Align, ForestMicroAntiPaladinClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass6Align, ForestMicroAntiPaladinClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass7Align, ForestMicroAntiPaladinClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass8Align, ForestMicroAntiPaladinClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass9Align, ForestMicroAntiPaladinClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            ForestMicroAntiPaladinClasspro = ForestMicroAntiPaladinClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMicroAntiPaladinClass10Align, ForestMicroAntiPaladinClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MicroAntiPaladinClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMicroAntiPaladinClasspro.Configure());
            var ForestOathbreakerClasspro = ProgressionConfigurator.New(ForestOathbreakerClass0Align, ForestOathbreakerClass0AlignGuid)
            .SetDisplayName(ForestOathbreakerClass0AlignDisplayName)
            .SetDescription(ForestOathbreakerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(OathbreakerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestOathbreakerClass2Align, ForestOathbreakerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestOathbreakerClass3Align, ForestOathbreakerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestOathbreakerClass4Align, ForestOathbreakerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestOathbreakerClass5Align, ForestOathbreakerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestOathbreakerClass6Align, ForestOathbreakerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestOathbreakerClass7Align, ForestOathbreakerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestOathbreakerClass8Align, ForestOathbreakerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestOathbreakerClass9Align, ForestOathbreakerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            ForestOathbreakerClasspro = ForestOathbreakerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestOathbreakerClass10Align, ForestOathbreakerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = OathbreakerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestOathbreakerClasspro.Configure());
            var ForestDreadKnightClasspro = ProgressionConfigurator.New(ForestDreadKnightClass0Align, ForestDreadKnightClass0AlignGuid)
            .SetDisplayName(ForestDreadKnightClass0AlignDisplayName)
            .SetDescription(ForestDreadKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(DreadKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestDreadKnightClass2Align, ForestDreadKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestDreadKnightClass3Align, ForestDreadKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestDreadKnightClass4Align, ForestDreadKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestDreadKnightClass5Align, ForestDreadKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestDreadKnightClass6Align, ForestDreadKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestDreadKnightClass7Align, ForestDreadKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestDreadKnightClass8Align, ForestDreadKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestDreadKnightClass9Align, ForestDreadKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            ForestDreadKnightClasspro = ForestDreadKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestDreadKnightClass10Align, ForestDreadKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = DreadKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestDreadKnightClasspro.Configure());
            var ForestStargazerClasspro = ProgressionConfigurator.New(ForestStargazerClass0Align, ForestStargazerClass0AlignGuid)
            .SetDisplayName(ForestStargazerClass0AlignDisplayName)
            .SetDescription(ForestStargazerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(StargazerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestStargazerClass2Align, ForestStargazerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestStargazerClass3Align, ForestStargazerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestStargazerClass4Align, ForestStargazerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestStargazerClass5Align, ForestStargazerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestStargazerClass6Align, ForestStargazerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestStargazerClass7Align, ForestStargazerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestStargazerClass8Align, ForestStargazerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestStargazerClass9Align, ForestStargazerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            ForestStargazerClasspro = ForestStargazerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestStargazerClass10Align, ForestStargazerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = StargazerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestStargazerClasspro.Configure());
            var ForestSwashbucklerClasspro = ProgressionConfigurator.New(ForestSwashbucklerClass0Align, ForestSwashbucklerClass0AlignGuid)
            .SetDisplayName(ForestSwashbucklerClass0AlignDisplayName)
            .SetDescription(ForestSwashbucklerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SwashbucklerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSwashbucklerClass2Align, ForestSwashbucklerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSwashbucklerClass3Align, ForestSwashbucklerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSwashbucklerClass4Align, ForestSwashbucklerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSwashbucklerClass5Align, ForestSwashbucklerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSwashbucklerClass6Align, ForestSwashbucklerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSwashbucklerClass7Align, ForestSwashbucklerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSwashbucklerClass8Align, ForestSwashbucklerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSwashbucklerClass9Align, ForestSwashbucklerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            ForestSwashbucklerClasspro = ForestSwashbucklerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSwashbucklerClass10Align, ForestSwashbucklerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SwashbucklerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSwashbucklerClasspro.Configure());
            var ForestHolyVindicatorClasspro = ProgressionConfigurator.New(ForestHolyVindicatorClass0Align, ForestHolyVindicatorClass0AlignGuid)
            .SetDisplayName(ForestHolyVindicatorClass0AlignDisplayName)
            .SetDescription(ForestHolyVindicatorClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HolyVindicatorClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHolyVindicatorClass2Align, ForestHolyVindicatorClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHolyVindicatorClass3Align, ForestHolyVindicatorClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHolyVindicatorClass4Align, ForestHolyVindicatorClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHolyVindicatorClass5Align, ForestHolyVindicatorClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHolyVindicatorClass6Align, ForestHolyVindicatorClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHolyVindicatorClass7Align, ForestHolyVindicatorClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHolyVindicatorClass8Align, ForestHolyVindicatorClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHolyVindicatorClass9Align, ForestHolyVindicatorClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            ForestHolyVindicatorClasspro = ForestHolyVindicatorClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHolyVindicatorClass10Align, ForestHolyVindicatorClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HolyVindicatorClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHolyVindicatorClasspro.Configure());
            var ForestSummonerClasspro = ProgressionConfigurator.New(ForestSummonerClass0Align, ForestSummonerClass0AlignGuid)
            .SetDisplayName(ForestSummonerClass0AlignDisplayName)
            .SetDescription(ForestSummonerClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(SummonerClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestSummonerClass2Align, ForestSummonerClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestSummonerClass3Align, ForestSummonerClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestSummonerClass4Align, ForestSummonerClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestSummonerClass5Align, ForestSummonerClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestSummonerClass6Align, ForestSummonerClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestSummonerClass7Align, ForestSummonerClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestSummonerClass8Align, ForestSummonerClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestSummonerClass9Align, ForestSummonerClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            ForestSummonerClasspro = ForestSummonerClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestSummonerClass10Align, ForestSummonerClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = SummonerClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestSummonerClasspro.Configure());
            var ForestLionBladeClasspro = ProgressionConfigurator.New(ForestLionBladeClass0Align, ForestLionBladeClass0AlignGuid)
            .SetDisplayName(ForestLionBladeClass0AlignDisplayName)
            .SetDescription(ForestLionBladeClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(LionBladeClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestLionBladeClass2Align, ForestLionBladeClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestLionBladeClass3Align, ForestLionBladeClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestLionBladeClass4Align, ForestLionBladeClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestLionBladeClass5Align, ForestLionBladeClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestLionBladeClass6Align, ForestLionBladeClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestLionBladeClass7Align, ForestLionBladeClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestLionBladeClass8Align, ForestLionBladeClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestLionBladeClass9Align, ForestLionBladeClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            ForestLionBladeClasspro = ForestLionBladeClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestLionBladeClass10Align, ForestLionBladeClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = LionBladeClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestLionBladeClasspro.Configure());
            var ForestEnchantingCourtesanClasspro = ProgressionConfigurator.New(ForestEnchantingCourtesanClass0Align, ForestEnchantingCourtesanClass0AlignGuid)
            .SetDisplayName(ForestEnchantingCourtesanClass0AlignDisplayName)
            .SetDescription(ForestEnchantingCourtesanClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(EnchantingCourtesanClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass2Align, ForestEnchantingCourtesanClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass3Align, ForestEnchantingCourtesanClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass4Align, ForestEnchantingCourtesanClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass5Align, ForestEnchantingCourtesanClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass6Align, ForestEnchantingCourtesanClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass7Align, ForestEnchantingCourtesanClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass8Align, ForestEnchantingCourtesanClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass9Align, ForestEnchantingCourtesanClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            ForestEnchantingCourtesanClasspro = ForestEnchantingCourtesanClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestEnchantingCourtesanClass10Align, ForestEnchantingCourtesanClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = EnchantingCourtesanClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestEnchantingCourtesanClasspro.Configure());
            var ForestHeritorKnightClasspro = ProgressionConfigurator.New(ForestHeritorKnightClass0Align, ForestHeritorKnightClass0AlignGuid)
            .SetDisplayName(ForestHeritorKnightClass0AlignDisplayName)
            .SetDescription(ForestHeritorKnightClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(HeritorKnightClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestHeritorKnightClass2Align, ForestHeritorKnightClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestHeritorKnightClass3Align, ForestHeritorKnightClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestHeritorKnightClass4Align, ForestHeritorKnightClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestHeritorKnightClass5Align, ForestHeritorKnightClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestHeritorKnightClass6Align, ForestHeritorKnightClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestHeritorKnightClass7Align, ForestHeritorKnightClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestHeritorKnightClass8Align, ForestHeritorKnightClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestHeritorKnightClass9Align, ForestHeritorKnightClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            ForestHeritorKnightClasspro = ForestHeritorKnightClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestHeritorKnightClass10Align, ForestHeritorKnightClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = HeritorKnightClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestHeritorKnightClasspro.Configure());
            var ForestGoldenLegionnaireClasspro = ProgressionConfigurator.New(ForestGoldenLegionnaireClass0Align, ForestGoldenLegionnaireClass0AlignGuid)
            .SetDisplayName(ForestGoldenLegionnaireClass0AlignDisplayName)
            .SetDescription(ForestGoldenLegionnaireClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(GoldenLegionnaireClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass2Align, ForestGoldenLegionnaireClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass3Align, ForestGoldenLegionnaireClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass4Align, ForestGoldenLegionnaireClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass5Align, ForestGoldenLegionnaireClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass6Align, ForestGoldenLegionnaireClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass7Align, ForestGoldenLegionnaireClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass8Align, ForestGoldenLegionnaireClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass9Align, ForestGoldenLegionnaireClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            ForestGoldenLegionnaireClasspro = ForestGoldenLegionnaireClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestGoldenLegionnaireClass10Align, ForestGoldenLegionnaireClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = GoldenLegionnaireClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestGoldenLegionnaireClasspro.Configure());
            var ForestBoltAceClasspro = ProgressionConfigurator.New(ForestBoltAceClass0Align, ForestBoltAceClass0AlignGuid)
            .SetDisplayName(ForestBoltAceClass0AlignDisplayName)
            .SetDescription(ForestBoltAceClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(BoltAceClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestBoltAceClass2Align, ForestBoltAceClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestBoltAceClass3Align, ForestBoltAceClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestBoltAceClass4Align, ForestBoltAceClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestBoltAceClass5Align, ForestBoltAceClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestBoltAceClass6Align, ForestBoltAceClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestBoltAceClass7Align, ForestBoltAceClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestBoltAceClass8Align, ForestBoltAceClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestBoltAceClass9Align, ForestBoltAceClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            ForestBoltAceClasspro = ForestBoltAceClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestBoltAceClass10Align, ForestBoltAceClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = BoltAceClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestBoltAceClasspro.Configure());
            var ForestMortalUsherClasspro = ProgressionConfigurator.New(ForestMortalUsherClass0Align, ForestMortalUsherClass0AlignGuid)
            .SetDisplayName(ForestMortalUsherClass0AlignDisplayName)
            .SetDescription(ForestMortalUsherClass0AlignDescription)
            .SetClasses(ArchetypeGuid)
            .AddPrerequisiteClassLevel(MortalUsherClass, 1)
            .SetHideNotAvailibleInUI(true);
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(2,
            FeatureConfigurator.New(ForestMortalUsherClass2Align, ForestMortalUsherClass2AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(3,
            FeatureConfigurator.New(ForestMortalUsherClass3Align, ForestMortalUsherClass3AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(4,
            FeatureConfigurator.New(ForestMortalUsherClass4Align, ForestMortalUsherClass4AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(5,
            FeatureConfigurator.New(ForestMortalUsherClass5Align, ForestMortalUsherClass5AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(6,
            FeatureConfigurator.New(ForestMortalUsherClass6Align, ForestMortalUsherClass6AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(7,
            FeatureConfigurator.New(ForestMortalUsherClass7Align, ForestMortalUsherClass7AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(8,
            FeatureConfigurator.New(ForestMortalUsherClass8Align, ForestMortalUsherClass8AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(9,
            FeatureConfigurator.New(ForestMortalUsherClass9Align, ForestMortalUsherClass9AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            ForestMortalUsherClasspro = ForestMortalUsherClasspro.AddToLevelEntry(10,
            FeatureConfigurator.New(ForestMortalUsherClass10Align, ForestMortalUsherClass10AlignGuid)
            .AddComponent<FakeLevelUpClass>(c => { c.clazz = MortalUsherClass; })
            .SetHideInUI(true).Configure());
            list.Add(ForestMortalUsherClasspro.Configure());
            
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

        private const string ForestAlchemistClass0Align = "ForestAlchemistClass0Align";
        private static readonly string ForestAlchemistClass0AlignGuid = "ee46be1f-32fc-4a43-979f-18d9be0e3a01";
        internal const string ForestAlchemistClass0AlignDisplayName = "ForestAlchemistClass0Align.Name";
        private const string ForestAlchemistClass0AlignDescription = "ForestAlchemistClass0Align.Description";
        private const string ForestAlchemistClass2Align = "ForestAlchemistClass2Align";
        private static readonly string ForestAlchemistClass2AlignGuid = "9a936efb-382f-4a3c-9376-9eb38ab12b50";
        private const string ForestAlchemistClass3Align = "ForestAlchemistClass3Align";
        private static readonly string ForestAlchemistClass3AlignGuid = "654a0cfe-030c-4bc8-a282-71d751bd62c4";
        private const string ForestAlchemistClass4Align = "ForestAlchemistClass4Align";
        private static readonly string ForestAlchemistClass4AlignGuid = "bd0eedc6-806b-4719-b5bf-96e73ee4b270";
        private const string ForestAlchemistClass5Align = "ForestAlchemistClass5Align";
        private static readonly string ForestAlchemistClass5AlignGuid = "2463cde6-daf4-46ef-b9d1-e9fea86c3a4b";
        private const string ForestAlchemistClass6Align = "ForestAlchemistClass6Align";
        private static readonly string ForestAlchemistClass6AlignGuid = "4796adfc-3124-48ce-9744-b753dd879585";
        private const string ForestAlchemistClass7Align = "ForestAlchemistClass7Align";
        private static readonly string ForestAlchemistClass7AlignGuid = "44d03fbc-ba4a-4290-be91-ad77b3172797";
        private const string ForestAlchemistClass8Align = "ForestAlchemistClass8Align";
        private static readonly string ForestAlchemistClass8AlignGuid = "d804f332-8da5-49e0-a51e-b2c1be2e69a9";
        private const string ForestAlchemistClass9Align = "ForestAlchemistClass9Align";
        private static readonly string ForestAlchemistClass9AlignGuid = "11b61517-9e5b-46f8-8880-f579fbd59f8c";
        private const string ForestAlchemistClass10Align = "ForestAlchemistClass10Align";
        private static readonly string ForestAlchemistClass10AlignGuid = "4f891384-c692-4e74-8d54-180bd9d3a58c";
        private const string ForestArcaneTricksterClass0Align = "ForestArcaneTricksterClass0Align";
        private static readonly string ForestArcaneTricksterClass0AlignGuid = "9799fe48-daa1-4f99-b6f0-8aa0b656c08e";
        internal const string ForestArcaneTricksterClass0AlignDisplayName = "ForestArcaneTricksterClass0Align.Name";
        private const string ForestArcaneTricksterClass0AlignDescription = "ForestArcaneTricksterClass0Align.Description";
        private const string ForestArcaneTricksterClass2Align = "ForestArcaneTricksterClass2Align";
        private static readonly string ForestArcaneTricksterClass2AlignGuid = "3596cfd8-868e-4d8d-989a-c4a6b40ca1ac";
        private const string ForestArcaneTricksterClass3Align = "ForestArcaneTricksterClass3Align";
        private static readonly string ForestArcaneTricksterClass3AlignGuid = "05a173b5-70db-46fc-a5ad-06802a76520a";
        private const string ForestArcaneTricksterClass4Align = "ForestArcaneTricksterClass4Align";
        private static readonly string ForestArcaneTricksterClass4AlignGuid = "99da6a84-7d4c-4b61-beea-19c93a17a48e";
        private const string ForestArcaneTricksterClass5Align = "ForestArcaneTricksterClass5Align";
        private static readonly string ForestArcaneTricksterClass5AlignGuid = "4a9b22ef-b639-4ebe-b8f1-7994e6166300";
        private const string ForestArcaneTricksterClass6Align = "ForestArcaneTricksterClass6Align";
        private static readonly string ForestArcaneTricksterClass6AlignGuid = "98a24973-8de8-42cb-bb19-4f89b99f74b0";
        private const string ForestArcaneTricksterClass7Align = "ForestArcaneTricksterClass7Align";
        private static readonly string ForestArcaneTricksterClass7AlignGuid = "0641c344-212f-434a-9bc6-4e66caf291a9";
        private const string ForestArcaneTricksterClass8Align = "ForestArcaneTricksterClass8Align";
        private static readonly string ForestArcaneTricksterClass8AlignGuid = "9614c7fa-0149-42aa-9de6-368697abff95";
        private const string ForestArcaneTricksterClass9Align = "ForestArcaneTricksterClass9Align";
        private static readonly string ForestArcaneTricksterClass9AlignGuid = "31733eca-3d76-479a-9779-89c6e8131f04";
        private const string ForestArcaneTricksterClass10Align = "ForestArcaneTricksterClass10Align";
        private static readonly string ForestArcaneTricksterClass10AlignGuid = "46a123f3-19b2-48f0-b58a-081ec2d67803";
        private const string ForestArcanistClass0Align = "ForestArcanistClass0Align";
        private static readonly string ForestArcanistClass0AlignGuid = "113afe94-454c-409b-a474-109f70f595af";
        internal const string ForestArcanistClass0AlignDisplayName = "ForestArcanistClass0Align.Name";
        private const string ForestArcanistClass0AlignDescription = "ForestArcanistClass0Align.Description";
        private const string ForestArcanistClass2Align = "ForestArcanistClass2Align";
        private static readonly string ForestArcanistClass2AlignGuid = "da8fddbf-0be1-48b9-81e8-81636df4c5e6";
        private const string ForestArcanistClass3Align = "ForestArcanistClass3Align";
        private static readonly string ForestArcanistClass3AlignGuid = "c6e4af03-3d99-44b0-b978-4eb60ea7365e";
        private const string ForestArcanistClass4Align = "ForestArcanistClass4Align";
        private static readonly string ForestArcanistClass4AlignGuid = "7e7c8a91-8476-44cb-8f71-7758de316d3b";
        private const string ForestArcanistClass5Align = "ForestArcanistClass5Align";
        private static readonly string ForestArcanistClass5AlignGuid = "c679f85b-9de2-451f-946f-4041bc2172f1";
        private const string ForestArcanistClass6Align = "ForestArcanistClass6Align";
        private static readonly string ForestArcanistClass6AlignGuid = "564e8677-ec95-4964-9695-aabdad2235f4";
        private const string ForestArcanistClass7Align = "ForestArcanistClass7Align";
        private static readonly string ForestArcanistClass7AlignGuid = "dffee33f-cc60-4980-9dd6-cb273873da33";
        private const string ForestArcanistClass8Align = "ForestArcanistClass8Align";
        private static readonly string ForestArcanistClass8AlignGuid = "aa505582-d502-41ed-a7ed-97f438aaefb1";
        private const string ForestArcanistClass9Align = "ForestArcanistClass9Align";
        private static readonly string ForestArcanistClass9AlignGuid = "aa728b4c-5395-4ef5-b7ee-75d9cb99b6b1";
        private const string ForestArcanistClass10Align = "ForestArcanistClass10Align";
        private static readonly string ForestArcanistClass10AlignGuid = "38a3a7b9-b36e-4d83-8684-ab5c9ad1cb12";
        private const string ForestAssassinClass0Align = "ForestAssassinClass0Align";
        private static readonly string ForestAssassinClass0AlignGuid = "06519c2f-4c5c-4e78-8936-44589390452b";
        internal const string ForestAssassinClass0AlignDisplayName = "ForestAssassinClass0Align.Name";
        private const string ForestAssassinClass0AlignDescription = "ForestAssassinClass0Align.Description";
        private const string ForestAssassinClass2Align = "ForestAssassinClass2Align";
        private static readonly string ForestAssassinClass2AlignGuid = "5d6475bc-724d-4a4b-b351-c9b0d3b14d05";
        private const string ForestAssassinClass3Align = "ForestAssassinClass3Align";
        private static readonly string ForestAssassinClass3AlignGuid = "644741fc-7912-4ba4-b440-cb6d15123ecb";
        private const string ForestAssassinClass4Align = "ForestAssassinClass4Align";
        private static readonly string ForestAssassinClass4AlignGuid = "d4f76c12-3871-4ee0-9c42-74db19d36dc3";
        private const string ForestAssassinClass5Align = "ForestAssassinClass5Align";
        private static readonly string ForestAssassinClass5AlignGuid = "6dc2bc00-b60d-4f2d-bc5f-e0e7b3b8300e";
        private const string ForestAssassinClass6Align = "ForestAssassinClass6Align";
        private static readonly string ForestAssassinClass6AlignGuid = "85f9474b-86e1-4699-ad05-a77a5972d2a1";
        private const string ForestAssassinClass7Align = "ForestAssassinClass7Align";
        private static readonly string ForestAssassinClass7AlignGuid = "97a09fac-a79b-49d7-bc03-1bc8e0150543";
        private const string ForestAssassinClass8Align = "ForestAssassinClass8Align";
        private static readonly string ForestAssassinClass8AlignGuid = "b4db6e83-b378-44b3-b98c-9286cf13e388";
        private const string ForestAssassinClass9Align = "ForestAssassinClass9Align";
        private static readonly string ForestAssassinClass9AlignGuid = "75b0503a-71f3-484b-9ecd-3f56569a0803";
        private const string ForestAssassinClass10Align = "ForestAssassinClass10Align";
        private static readonly string ForestAssassinClass10AlignGuid = "dbb2a2a4-7f61-427f-b380-4cce0db8c136";
        private const string ForestBarbarianClass0Align = "ForestBarbarianClass0Align";
        private static readonly string ForestBarbarianClass0AlignGuid = "bb98b11b-163a-4668-a81f-d3251840285d";
        internal const string ForestBarbarianClass0AlignDisplayName = "ForestBarbarianClass0Align.Name";
        private const string ForestBarbarianClass0AlignDescription = "ForestBarbarianClass0Align.Description";
        private const string ForestBarbarianClass2Align = "ForestBarbarianClass2Align";
        private static readonly string ForestBarbarianClass2AlignGuid = "28027e06-d9cf-4d39-96ee-cef6cb1ba800";
        private const string ForestBarbarianClass3Align = "ForestBarbarianClass3Align";
        private static readonly string ForestBarbarianClass3AlignGuid = "214bd86e-57f4-4ae6-9e9f-7561a7e5ea19";
        private const string ForestBarbarianClass4Align = "ForestBarbarianClass4Align";
        private static readonly string ForestBarbarianClass4AlignGuid = "eaae12e9-0c38-4e90-a1f6-1671d856be62";
        private const string ForestBarbarianClass5Align = "ForestBarbarianClass5Align";
        private static readonly string ForestBarbarianClass5AlignGuid = "74c396c0-8916-4480-86ba-872f4f3f67bb";
        private const string ForestBarbarianClass6Align = "ForestBarbarianClass6Align";
        private static readonly string ForestBarbarianClass6AlignGuid = "0fe5d9a5-bef9-4321-b588-2da897642a9c";
        private const string ForestBarbarianClass7Align = "ForestBarbarianClass7Align";
        private static readonly string ForestBarbarianClass7AlignGuid = "ba23356a-b4e6-4da8-8c64-38c4266a05ce";
        private const string ForestBarbarianClass8Align = "ForestBarbarianClass8Align";
        private static readonly string ForestBarbarianClass8AlignGuid = "e377910b-379d-435f-b4c9-166d58aca150";
        private const string ForestBarbarianClass9Align = "ForestBarbarianClass9Align";
        private static readonly string ForestBarbarianClass9AlignGuid = "b1e5da6b-927d-4587-9c68-129c4e405b7b";
        private const string ForestBarbarianClass10Align = "ForestBarbarianClass10Align";
        private static readonly string ForestBarbarianClass10AlignGuid = "c48dc64b-eb2e-46eb-8ce3-f3fdbb9b2aa5";
        private const string ForestBardClass0Align = "ForestBardClass0Align";
        private static readonly string ForestBardClass0AlignGuid = "4349a668-6d20-40cc-82c3-e53ab47b29f9";
        internal const string ForestBardClass0AlignDisplayName = "ForestBardClass0Align.Name";
        private const string ForestBardClass0AlignDescription = "ForestBardClass0Align.Description";
        private const string ForestBardClass2Align = "ForestBardClass2Align";
        private static readonly string ForestBardClass2AlignGuid = "0b42baea-ba37-4044-9ee0-9355c6fbe344";
        private const string ForestBardClass3Align = "ForestBardClass3Align";
        private static readonly string ForestBardClass3AlignGuid = "3e1fc76e-0bf4-4b80-812e-ffce7bead9a1";
        private const string ForestBardClass4Align = "ForestBardClass4Align";
        private static readonly string ForestBardClass4AlignGuid = "6d575938-47fd-4346-810c-b913c2218b6f";
        private const string ForestBardClass5Align = "ForestBardClass5Align";
        private static readonly string ForestBardClass5AlignGuid = "763f1bc8-185e-423d-8e22-9298d7cad08a";
        private const string ForestBardClass6Align = "ForestBardClass6Align";
        private static readonly string ForestBardClass6AlignGuid = "6a2dac3a-74d7-4d23-8585-dadb8a2de344";
        private const string ForestBardClass7Align = "ForestBardClass7Align";
        private static readonly string ForestBardClass7AlignGuid = "45155fd7-3da2-4c59-8558-8bd3142b41cf";
        private const string ForestBardClass8Align = "ForestBardClass8Align";
        private static readonly string ForestBardClass8AlignGuid = "42609e8c-b432-4685-a49a-66cffdb18eac";
        private const string ForestBardClass9Align = "ForestBardClass9Align";
        private static readonly string ForestBardClass9AlignGuid = "d5844287-5552-4f29-8c36-bdd2b61c12cb";
        private const string ForestBardClass10Align = "ForestBardClass10Align";
        private static readonly string ForestBardClass10AlignGuid = "a5e6fb43-16ee-4302-8526-9e2f2c7e5ca8";
        private const string ForestBloodragerClass0Align = "ForestBloodragerClass0Align";
        private static readonly string ForestBloodragerClass0AlignGuid = "067e1753-100a-40c8-928b-e026d09a24dd";
        internal const string ForestBloodragerClass0AlignDisplayName = "ForestBloodragerClass0Align.Name";
        private const string ForestBloodragerClass0AlignDescription = "ForestBloodragerClass0Align.Description";
        private const string ForestBloodragerClass2Align = "ForestBloodragerClass2Align";
        private static readonly string ForestBloodragerClass2AlignGuid = "50fe943c-7b69-4c41-9c50-ee9dc3d6892b";
        private const string ForestBloodragerClass3Align = "ForestBloodragerClass3Align";
        private static readonly string ForestBloodragerClass3AlignGuid = "107364c7-a496-48db-b387-c77774d12383";
        private const string ForestBloodragerClass4Align = "ForestBloodragerClass4Align";
        private static readonly string ForestBloodragerClass4AlignGuid = "ff5346a1-508b-4c25-b18e-f0f3c60adec1";
        private const string ForestBloodragerClass5Align = "ForestBloodragerClass5Align";
        private static readonly string ForestBloodragerClass5AlignGuid = "3d39a8c4-0137-4757-8f3c-fce684a3fa15";
        private const string ForestBloodragerClass6Align = "ForestBloodragerClass6Align";
        private static readonly string ForestBloodragerClass6AlignGuid = "1916ca41-ad7d-4b0d-891e-cff33e29661b";
        private const string ForestBloodragerClass7Align = "ForestBloodragerClass7Align";
        private static readonly string ForestBloodragerClass7AlignGuid = "c6ec1488-a46f-4c96-971c-5a582a9c3682";
        private const string ForestBloodragerClass8Align = "ForestBloodragerClass8Align";
        private static readonly string ForestBloodragerClass8AlignGuid = "cc2282d2-17ef-4501-89e4-992fd21bfdf7";
        private const string ForestBloodragerClass9Align = "ForestBloodragerClass9Align";
        private static readonly string ForestBloodragerClass9AlignGuid = "eaac3a56-05f2-4347-bbc6-a6c5856747bd";
        private const string ForestBloodragerClass10Align = "ForestBloodragerClass10Align";
        private static readonly string ForestBloodragerClass10AlignGuid = "9c7b678d-b615-4cf6-a3ca-855ec8e1ee17";
        private const string ForestCavalierClass0Align = "ForestCavalierClass0Align";
        private static readonly string ForestCavalierClass0AlignGuid = "32f520e8-a193-4240-8a03-f81ea961b4c4";
        internal const string ForestCavalierClass0AlignDisplayName = "ForestCavalierClass0Align.Name";
        private const string ForestCavalierClass0AlignDescription = "ForestCavalierClass0Align.Description";
        private const string ForestCavalierClass2Align = "ForestCavalierClass2Align";
        private static readonly string ForestCavalierClass2AlignGuid = "57afdaa3-e4e8-4e68-b1a7-bf303aab1cf9";
        private const string ForestCavalierClass3Align = "ForestCavalierClass3Align";
        private static readonly string ForestCavalierClass3AlignGuid = "c6d8fdd2-f88c-46c7-a69b-037a0f0930a7";
        private const string ForestCavalierClass4Align = "ForestCavalierClass4Align";
        private static readonly string ForestCavalierClass4AlignGuid = "63ea978b-94af-4f96-871a-ef9d5ac0852a";
        private const string ForestCavalierClass5Align = "ForestCavalierClass5Align";
        private static readonly string ForestCavalierClass5AlignGuid = "72f6970e-de18-429c-8a92-c9c30cfc152e";
        private const string ForestCavalierClass6Align = "ForestCavalierClass6Align";
        private static readonly string ForestCavalierClass6AlignGuid = "19ffc3a0-0878-4cec-b365-746437c92d13";
        private const string ForestCavalierClass7Align = "ForestCavalierClass7Align";
        private static readonly string ForestCavalierClass7AlignGuid = "9d3f6129-1f10-48a1-8368-71e7e09f49b4";
        private const string ForestCavalierClass8Align = "ForestCavalierClass8Align";
        private static readonly string ForestCavalierClass8AlignGuid = "e9ee0885-4cb5-483b-b128-4e49e7402649";
        private const string ForestCavalierClass9Align = "ForestCavalierClass9Align";
        private static readonly string ForestCavalierClass9AlignGuid = "f54ef02f-9866-48d1-8e4f-f57b5c389ab5";
        private const string ForestCavalierClass10Align = "ForestCavalierClass10Align";
        private static readonly string ForestCavalierClass10AlignGuid = "a1753ba6-337b-4d45-be83-e17fc735892e";
        private const string ForestClericClass0Align = "ForestClericClass0Align";
        private static readonly string ForestClericClass0AlignGuid = "764853f7-aab1-4b92-b8dc-0f367af90b44";
        internal const string ForestClericClass0AlignDisplayName = "ForestClericClass0Align.Name";
        private const string ForestClericClass0AlignDescription = "ForestClericClass0Align.Description";
        private const string ForestClericClass2Align = "ForestClericClass2Align";
        private static readonly string ForestClericClass2AlignGuid = "ece0171a-6c83-44ba-9b9e-b05d89ff6c49";
        private const string ForestClericClass3Align = "ForestClericClass3Align";
        private static readonly string ForestClericClass3AlignGuid = "a8e9cf12-d9f4-4436-8ca7-68743640df6d";
        private const string ForestClericClass4Align = "ForestClericClass4Align";
        private static readonly string ForestClericClass4AlignGuid = "1f1e1136-f353-4bc8-a73f-80e8e7a3fb22";
        private const string ForestClericClass5Align = "ForestClericClass5Align";
        private static readonly string ForestClericClass5AlignGuid = "a75400c7-2d4b-43bb-b5c9-cbc8ae488e91";
        private const string ForestClericClass6Align = "ForestClericClass6Align";
        private static readonly string ForestClericClass6AlignGuid = "17cee097-be4a-4e84-82c3-a7be42c41c7d";
        private const string ForestClericClass7Align = "ForestClericClass7Align";
        private static readonly string ForestClericClass7AlignGuid = "226122ae-ce4e-4ffb-804f-2ac78fbaef9f";
        private const string ForestClericClass8Align = "ForestClericClass8Align";
        private static readonly string ForestClericClass8AlignGuid = "3ee78fac-d41e-4c01-8f22-289a299aeab1";
        private const string ForestClericClass9Align = "ForestClericClass9Align";
        private static readonly string ForestClericClass9AlignGuid = "a71ba245-3a2a-4cc9-b892-97987b29e9ba";
        private const string ForestClericClass10Align = "ForestClericClass10Align";
        private static readonly string ForestClericClass10AlignGuid = "dcdd2e11-b783-4846-a99b-12571d62b5e8";
        private const string ForestDragonDiscipleClass0Align = "ForestDragonDiscipleClass0Align";
        private static readonly string ForestDragonDiscipleClass0AlignGuid = "980cac17-fe63-4a3b-b631-0f9ad074885c";
        internal const string ForestDragonDiscipleClass0AlignDisplayName = "ForestDragonDiscipleClass0Align.Name";
        private const string ForestDragonDiscipleClass0AlignDescription = "ForestDragonDiscipleClass0Align.Description";
        private const string ForestDragonDiscipleClass2Align = "ForestDragonDiscipleClass2Align";
        private static readonly string ForestDragonDiscipleClass2AlignGuid = "1523d4ae-f269-4620-abc2-f92ca2bcb95d";
        private const string ForestDragonDiscipleClass3Align = "ForestDragonDiscipleClass3Align";
        private static readonly string ForestDragonDiscipleClass3AlignGuid = "cb670306-608a-44b1-92b8-e9d9890d83e7";
        private const string ForestDragonDiscipleClass4Align = "ForestDragonDiscipleClass4Align";
        private static readonly string ForestDragonDiscipleClass4AlignGuid = "18d2b607-151a-49ef-b78b-eacfbfa360ff";
        private const string ForestDragonDiscipleClass5Align = "ForestDragonDiscipleClass5Align";
        private static readonly string ForestDragonDiscipleClass5AlignGuid = "520a9459-ade6-4b4e-8de5-1737358e90c7";
        private const string ForestDragonDiscipleClass6Align = "ForestDragonDiscipleClass6Align";
        private static readonly string ForestDragonDiscipleClass6AlignGuid = "024382ec-a157-4bea-b256-dd93bed07ce0";
        private const string ForestDragonDiscipleClass7Align = "ForestDragonDiscipleClass7Align";
        private static readonly string ForestDragonDiscipleClass7AlignGuid = "d5e89db9-4c76-4fe4-a968-1136927af9bd";
        private const string ForestDragonDiscipleClass8Align = "ForestDragonDiscipleClass8Align";
        private static readonly string ForestDragonDiscipleClass8AlignGuid = "8a1cbb90-6306-4234-8cf7-9fe6b106f1a6";
        private const string ForestDragonDiscipleClass9Align = "ForestDragonDiscipleClass9Align";
        private static readonly string ForestDragonDiscipleClass9AlignGuid = "9d6c2c0b-ff6f-4a11-9bd9-5dcf9b25dc83";
        private const string ForestDragonDiscipleClass10Align = "ForestDragonDiscipleClass10Align";
        private static readonly string ForestDragonDiscipleClass10AlignGuid = "5e7b70d9-6b35-4169-a826-24ddde0b1dde";
        private const string ForestDruidClass0Align = "ForestDruidClass0Align";
        private static readonly string ForestDruidClass0AlignGuid = "23b80de0-9ccd-4ba2-b8f3-b6c96694df0f";
        internal const string ForestDruidClass0AlignDisplayName = "ForestDruidClass0Align.Name";
        private const string ForestDruidClass0AlignDescription = "ForestDruidClass0Align.Description";
        private const string ForestDruidClass2Align = "ForestDruidClass2Align";
        private static readonly string ForestDruidClass2AlignGuid = "cc4dd6ff-10de-46cf-a27a-407d0568de5e";
        private const string ForestDruidClass3Align = "ForestDruidClass3Align";
        private static readonly string ForestDruidClass3AlignGuid = "378b38da-7cbc-4a82-852b-bbda9dad73fd";
        private const string ForestDruidClass4Align = "ForestDruidClass4Align";
        private static readonly string ForestDruidClass4AlignGuid = "8d142bb8-a7e8-4ce1-87cd-04750310108b";
        private const string ForestDruidClass5Align = "ForestDruidClass5Align";
        private static readonly string ForestDruidClass5AlignGuid = "f47bcb72-3b6b-434c-8ebe-e151281f358e";
        private const string ForestDruidClass6Align = "ForestDruidClass6Align";
        private static readonly string ForestDruidClass6AlignGuid = "5166ec79-9e6c-4aa2-a08f-fb0578fcbeae";
        private const string ForestDruidClass7Align = "ForestDruidClass7Align";
        private static readonly string ForestDruidClass7AlignGuid = "5683db64-b640-4bb7-bc96-d5483a3b629b";
        private const string ForestDruidClass8Align = "ForestDruidClass8Align";
        private static readonly string ForestDruidClass8AlignGuid = "4104b912-f05c-4c6c-bd9e-5c6769a7bfd2";
        private const string ForestDruidClass9Align = "ForestDruidClass9Align";
        private static readonly string ForestDruidClass9AlignGuid = "a80abb42-600f-415b-9ddf-2de0eb7f7224";
        private const string ForestDruidClass10Align = "ForestDruidClass10Align";
        private static readonly string ForestDruidClass10AlignGuid = "fee153a1-68bc-467a-b14f-292cf5f7fc55";
        private const string ForestDuelistClass0Align = "ForestDuelistClass0Align";
        private static readonly string ForestDuelistClass0AlignGuid = "d93ccb38-5610-432c-85ef-40bc2526f96b";
        internal const string ForestDuelistClass0AlignDisplayName = "ForestDuelistClass0Align.Name";
        private const string ForestDuelistClass0AlignDescription = "ForestDuelistClass0Align.Description";
        private const string ForestDuelistClass2Align = "ForestDuelistClass2Align";
        private static readonly string ForestDuelistClass2AlignGuid = "17c49878-6140-459d-988d-656a09a43b33";
        private const string ForestDuelistClass3Align = "ForestDuelistClass3Align";
        private static readonly string ForestDuelistClass3AlignGuid = "02a16c59-5208-48cc-a0f4-244b864609b9";
        private const string ForestDuelistClass4Align = "ForestDuelistClass4Align";
        private static readonly string ForestDuelistClass4AlignGuid = "639731ba-782b-4c8b-928f-d74f3ede17dd";
        private const string ForestDuelistClass5Align = "ForestDuelistClass5Align";
        private static readonly string ForestDuelistClass5AlignGuid = "e6111467-eab2-4786-b92c-6263ec6a3ceb";
        private const string ForestDuelistClass6Align = "ForestDuelistClass6Align";
        private static readonly string ForestDuelistClass6AlignGuid = "ddfbc542-cb31-43dd-b99f-f1d4df0d2ceb";
        private const string ForestDuelistClass7Align = "ForestDuelistClass7Align";
        private static readonly string ForestDuelistClass7AlignGuid = "864aed25-d1f8-485c-8df2-af06e40a657e";
        private const string ForestDuelistClass8Align = "ForestDuelistClass8Align";
        private static readonly string ForestDuelistClass8AlignGuid = "f7be11f9-0f9d-46a6-bee3-348360538d57";
        private const string ForestDuelistClass9Align = "ForestDuelistClass9Align";
        private static readonly string ForestDuelistClass9AlignGuid = "53914de5-f96c-4d60-9075-34dd544022eb";
        private const string ForestDuelistClass10Align = "ForestDuelistClass10Align";
        private static readonly string ForestDuelistClass10AlignGuid = "8bec9b8c-c60b-4194-9d66-9219aae6b867";
        private const string ForestEldritchKnightClass0Align = "ForestEldritchKnightClass0Align";
        private static readonly string ForestEldritchKnightClass0AlignGuid = "01e74e27-03aa-494d-9c3c-820321efa5cb";
        internal const string ForestEldritchKnightClass0AlignDisplayName = "ForestEldritchKnightClass0Align.Name";
        private const string ForestEldritchKnightClass0AlignDescription = "ForestEldritchKnightClass0Align.Description";
        private const string ForestEldritchKnightClass2Align = "ForestEldritchKnightClass2Align";
        private static readonly string ForestEldritchKnightClass2AlignGuid = "7f8cfc33-d10e-4430-aadd-70969b58752e";
        private const string ForestEldritchKnightClass3Align = "ForestEldritchKnightClass3Align";
        private static readonly string ForestEldritchKnightClass3AlignGuid = "527fc3a0-52ce-435a-b975-090e342f6a36";
        private const string ForestEldritchKnightClass4Align = "ForestEldritchKnightClass4Align";
        private static readonly string ForestEldritchKnightClass4AlignGuid = "aaa8c85f-a5ec-4765-bc25-13ead94fd7dd";
        private const string ForestEldritchKnightClass5Align = "ForestEldritchKnightClass5Align";
        private static readonly string ForestEldritchKnightClass5AlignGuid = "bb4eb1a1-4418-408f-8ff3-c398f663e2d7";
        private const string ForestEldritchKnightClass6Align = "ForestEldritchKnightClass6Align";
        private static readonly string ForestEldritchKnightClass6AlignGuid = "ce06024a-fe63-4eae-9a51-ec7f8309e641";
        private const string ForestEldritchKnightClass7Align = "ForestEldritchKnightClass7Align";
        private static readonly string ForestEldritchKnightClass7AlignGuid = "bd5c2209-619e-4a48-8da7-55f0fa4c75b7";
        private const string ForestEldritchKnightClass8Align = "ForestEldritchKnightClass8Align";
        private static readonly string ForestEldritchKnightClass8AlignGuid = "2d2201f4-c01b-4d92-a8fc-ab49666acfa9";
        private const string ForestEldritchKnightClass9Align = "ForestEldritchKnightClass9Align";
        private static readonly string ForestEldritchKnightClass9AlignGuid = "0af2055d-4142-4bcd-b897-e3f20e2b01c4";
        private const string ForestEldritchKnightClass10Align = "ForestEldritchKnightClass10Align";
        private static readonly string ForestEldritchKnightClass10AlignGuid = "680d38fe-654f-43e5-abc3-68adf00b2caf";
        private const string ForestEldritchScionClass0Align = "ForestEldritchScionClass0Align";
        private static readonly string ForestEldritchScionClass0AlignGuid = "52c9f239-5c5f-448e-ac97-a9a7bc2f5094";
        internal const string ForestEldritchScionClass0AlignDisplayName = "ForestEldritchScionClass0Align.Name";
        private const string ForestEldritchScionClass0AlignDescription = "ForestEldritchScionClass0Align.Description";
        private const string ForestEldritchScionClass2Align = "ForestEldritchScionClass2Align";
        private static readonly string ForestEldritchScionClass2AlignGuid = "2c98e4d8-184c-4e8f-9d11-de674450b8fb";
        private const string ForestEldritchScionClass3Align = "ForestEldritchScionClass3Align";
        private static readonly string ForestEldritchScionClass3AlignGuid = "1f004c85-587e-475d-9e1f-c02b757e5eb1";
        private const string ForestEldritchScionClass4Align = "ForestEldritchScionClass4Align";
        private static readonly string ForestEldritchScionClass4AlignGuid = "6d1cb39f-816f-4d8b-bdc9-6c04f9b2657a";
        private const string ForestEldritchScionClass5Align = "ForestEldritchScionClass5Align";
        private static readonly string ForestEldritchScionClass5AlignGuid = "ab78fd76-7114-4a90-9540-c4f51d5cc501";
        private const string ForestEldritchScionClass6Align = "ForestEldritchScionClass6Align";
        private static readonly string ForestEldritchScionClass6AlignGuid = "77e3b49b-946d-4dd4-ac2f-36b00d5e5d4e";
        private const string ForestEldritchScionClass7Align = "ForestEldritchScionClass7Align";
        private static readonly string ForestEldritchScionClass7AlignGuid = "69924811-ae19-4dd7-a359-0c230247406c";
        private const string ForestEldritchScionClass8Align = "ForestEldritchScionClass8Align";
        private static readonly string ForestEldritchScionClass8AlignGuid = "4a248230-8351-4787-82ba-5bb98c26361b";
        private const string ForestEldritchScionClass9Align = "ForestEldritchScionClass9Align";
        private static readonly string ForestEldritchScionClass9AlignGuid = "c1ed0380-ae0d-4526-b1e1-165e300962de";
        private const string ForestEldritchScionClass10Align = "ForestEldritchScionClass10Align";
        private static readonly string ForestEldritchScionClass10AlignGuid = "5cc99730-de4a-4caa-96e1-19d293c610e9";
        private const string ForestFighterClass0Align = "ForestFighterClass0Align";
        private static readonly string ForestFighterClass0AlignGuid = "08a0f894-d168-4db8-b794-d24614c3beba";
        internal const string ForestFighterClass0AlignDisplayName = "ForestFighterClass0Align.Name";
        private const string ForestFighterClass0AlignDescription = "ForestFighterClass0Align.Description";
        private const string ForestFighterClass2Align = "ForestFighterClass2Align";
        private static readonly string ForestFighterClass2AlignGuid = "ecd31d36-0462-4d62-85f8-70b19837b3e2";
        private const string ForestFighterClass3Align = "ForestFighterClass3Align";
        private static readonly string ForestFighterClass3AlignGuid = "67d858f7-292c-42c5-9e79-e927a94d1473";
        private const string ForestFighterClass4Align = "ForestFighterClass4Align";
        private static readonly string ForestFighterClass4AlignGuid = "2d32dd07-93b1-4083-8e10-944ecaae19ee";
        private const string ForestFighterClass5Align = "ForestFighterClass5Align";
        private static readonly string ForestFighterClass5AlignGuid = "4643bef3-bae8-4d5e-a45f-2e3081911a1f";
        private const string ForestFighterClass6Align = "ForestFighterClass6Align";
        private static readonly string ForestFighterClass6AlignGuid = "8ab30f35-613c-4a92-891e-85702ffee77b";
        private const string ForestFighterClass7Align = "ForestFighterClass7Align";
        private static readonly string ForestFighterClass7AlignGuid = "adba6c47-8734-4db4-93d5-f5cb15af17ff";
        private const string ForestFighterClass8Align = "ForestFighterClass8Align";
        private static readonly string ForestFighterClass8AlignGuid = "628c2c99-ba91-4341-8f51-b0b5e3dc2066";
        private const string ForestFighterClass9Align = "ForestFighterClass9Align";
        private static readonly string ForestFighterClass9AlignGuid = "4a807127-d151-4809-a120-53ee1616f844";
        private const string ForestFighterClass10Align = "ForestFighterClass10Align";
        private static readonly string ForestFighterClass10AlignGuid = "3e1561d6-a0c2-4d16-914c-7466b7a3a7af";
        private const string ForestHellknightClass0Align = "ForestHellknightClass0Align";
        private static readonly string ForestHellknightClass0AlignGuid = "33cddabd-40d1-4f79-910b-d974632338c2";
        internal const string ForestHellknightClass0AlignDisplayName = "ForestHellknightClass0Align.Name";
        private const string ForestHellknightClass0AlignDescription = "ForestHellknightClass0Align.Description";
        private const string ForestHellknightClass2Align = "ForestHellknightClass2Align";
        private static readonly string ForestHellknightClass2AlignGuid = "e03bcb71-ab69-4c31-9489-42f0c0de89f6";
        private const string ForestHellknightClass3Align = "ForestHellknightClass3Align";
        private static readonly string ForestHellknightClass3AlignGuid = "df682945-9a50-47f5-b814-beacb2fda6d6";
        private const string ForestHellknightClass4Align = "ForestHellknightClass4Align";
        private static readonly string ForestHellknightClass4AlignGuid = "4294d8b0-62e5-4b08-8532-661ed1958733";
        private const string ForestHellknightClass5Align = "ForestHellknightClass5Align";
        private static readonly string ForestHellknightClass5AlignGuid = "bdcb71bd-80a4-4396-93a0-a750448fd2a1";
        private const string ForestHellknightClass6Align = "ForestHellknightClass6Align";
        private static readonly string ForestHellknightClass6AlignGuid = "b378bdfb-40fa-4300-9949-b9ce7f38f0fd";
        private const string ForestHellknightClass7Align = "ForestHellknightClass7Align";
        private static readonly string ForestHellknightClass7AlignGuid = "27b87282-2546-4274-952e-5ad7f8d65705";
        private const string ForestHellknightClass8Align = "ForestHellknightClass8Align";
        private static readonly string ForestHellknightClass8AlignGuid = "937cccc8-2fd9-4680-ba85-4d40917776c0";
        private const string ForestHellknightClass9Align = "ForestHellknightClass9Align";
        private static readonly string ForestHellknightClass9AlignGuid = "c14be3b0-702a-47ea-86eb-81c13e0310d9";
        private const string ForestHellknightClass10Align = "ForestHellknightClass10Align";
        private static readonly string ForestHellknightClass10AlignGuid = "f431ac79-de9d-4ec3-990c-cd43c08cbda4";
        private const string ForestHellknightSigniferClass0Align = "ForestHellknightSigniferClass0Align";
        private static readonly string ForestHellknightSigniferClass0AlignGuid = "37e6a361-6df6-4529-8cd0-74561c9cd6e3";
        internal const string ForestHellknightSigniferClass0AlignDisplayName = "ForestHellknightSigniferClass0Align.Name";
        private const string ForestHellknightSigniferClass0AlignDescription = "ForestHellknightSigniferClass0Align.Description";
        private const string ForestHellknightSigniferClass2Align = "ForestHellknightSigniferClass2Align";
        private static readonly string ForestHellknightSigniferClass2AlignGuid = "98f547fc-319e-41a8-add1-38b5b4cefc5f";
        private const string ForestHellknightSigniferClass3Align = "ForestHellknightSigniferClass3Align";
        private static readonly string ForestHellknightSigniferClass3AlignGuid = "5b6f93eb-f003-44b1-9d67-cd555626b039";
        private const string ForestHellknightSigniferClass4Align = "ForestHellknightSigniferClass4Align";
        private static readonly string ForestHellknightSigniferClass4AlignGuid = "f19549db-45d9-4063-a4c4-0fa066e724a3";
        private const string ForestHellknightSigniferClass5Align = "ForestHellknightSigniferClass5Align";
        private static readonly string ForestHellknightSigniferClass5AlignGuid = "dabeb0e9-1d16-4424-a276-f65db4953373";
        private const string ForestHellknightSigniferClass6Align = "ForestHellknightSigniferClass6Align";
        private static readonly string ForestHellknightSigniferClass6AlignGuid = "f8b7bd01-9c8d-4df7-acb1-a7ee884664c2";
        private const string ForestHellknightSigniferClass7Align = "ForestHellknightSigniferClass7Align";
        private static readonly string ForestHellknightSigniferClass7AlignGuid = "0b808b9f-6310-4ee3-bd05-c726dac4db24";
        private const string ForestHellknightSigniferClass8Align = "ForestHellknightSigniferClass8Align";
        private static readonly string ForestHellknightSigniferClass8AlignGuid = "34c74e7a-9cac-4729-9254-7efdb1f198a4";
        private const string ForestHellknightSigniferClass9Align = "ForestHellknightSigniferClass9Align";
        private static readonly string ForestHellknightSigniferClass9AlignGuid = "c736745a-c610-41a0-901a-c3521764b301";
        private const string ForestHellknightSigniferClass10Align = "ForestHellknightSigniferClass10Align";
        private static readonly string ForestHellknightSigniferClass10AlignGuid = "19b9abd4-6562-4668-9aa7-059a68b4dca4";
        private const string ForestHunterClass0Align = "ForestHunterClass0Align";
        private static readonly string ForestHunterClass0AlignGuid = "7562ff13-1be0-401a-8c90-867dd8803545";
        internal const string ForestHunterClass0AlignDisplayName = "ForestHunterClass0Align.Name";
        private const string ForestHunterClass0AlignDescription = "ForestHunterClass0Align.Description";
        private const string ForestHunterClass2Align = "ForestHunterClass2Align";
        private static readonly string ForestHunterClass2AlignGuid = "c6682488-7904-4780-9458-887c596b33f6";
        private const string ForestHunterClass3Align = "ForestHunterClass3Align";
        private static readonly string ForestHunterClass3AlignGuid = "449837a3-52b0-472e-ba99-9b0e9cc7a217";
        private const string ForestHunterClass4Align = "ForestHunterClass4Align";
        private static readonly string ForestHunterClass4AlignGuid = "f8bb460f-3ce1-4cc5-88b8-14389ae43af9";
        private const string ForestHunterClass5Align = "ForestHunterClass5Align";
        private static readonly string ForestHunterClass5AlignGuid = "ad1da439-8352-45a7-b626-dba812d704a1";
        private const string ForestHunterClass6Align = "ForestHunterClass6Align";
        private static readonly string ForestHunterClass6AlignGuid = "595b1e7d-4c89-4fe6-b714-c1242bda65d2";
        private const string ForestHunterClass7Align = "ForestHunterClass7Align";
        private static readonly string ForestHunterClass7AlignGuid = "bd8c04d7-3fce-4b0b-8356-43b4957f2e23";
        private const string ForestHunterClass8Align = "ForestHunterClass8Align";
        private static readonly string ForestHunterClass8AlignGuid = "89d3b822-1003-4f4b-bce3-e96a198d3df4";
        private const string ForestHunterClass9Align = "ForestHunterClass9Align";
        private static readonly string ForestHunterClass9AlignGuid = "64205e85-b9cc-4a34-9efc-b324d7c033a9";
        private const string ForestHunterClass10Align = "ForestHunterClass10Align";
        private static readonly string ForestHunterClass10AlignGuid = "0ed74854-ab89-42d3-a4ca-00a08e90a491";
        private const string ForestInquisitorClass0Align = "ForestInquisitorClass0Align";
        private static readonly string ForestInquisitorClass0AlignGuid = "e264e625-2ea7-4bd1-b85d-1625157fbf43";
        internal const string ForestInquisitorClass0AlignDisplayName = "ForestInquisitorClass0Align.Name";
        private const string ForestInquisitorClass0AlignDescription = "ForestInquisitorClass0Align.Description";
        private const string ForestInquisitorClass2Align = "ForestInquisitorClass2Align";
        private static readonly string ForestInquisitorClass2AlignGuid = "442f094f-fe69-4501-baea-e1a898c5d691";
        private const string ForestInquisitorClass3Align = "ForestInquisitorClass3Align";
        private static readonly string ForestInquisitorClass3AlignGuid = "66f44376-1555-4573-ae39-b715002b0d6a";
        private const string ForestInquisitorClass4Align = "ForestInquisitorClass4Align";
        private static readonly string ForestInquisitorClass4AlignGuid = "f33ccfba-e503-46f5-84f2-7b7f3bf53855";
        private const string ForestInquisitorClass5Align = "ForestInquisitorClass5Align";
        private static readonly string ForestInquisitorClass5AlignGuid = "e0cf919d-aed1-4ebd-9bb3-ad7eba9293c9";
        private const string ForestInquisitorClass6Align = "ForestInquisitorClass6Align";
        private static readonly string ForestInquisitorClass6AlignGuid = "ba9440d5-a145-4036-b9d0-30727f11bf70";
        private const string ForestInquisitorClass7Align = "ForestInquisitorClass7Align";
        private static readonly string ForestInquisitorClass7AlignGuid = "79e05ef9-5bf9-4a6c-9d69-5c57817f6adc";
        private const string ForestInquisitorClass8Align = "ForestInquisitorClass8Align";
        private static readonly string ForestInquisitorClass8AlignGuid = "e830531e-e4fa-4c7e-a5d8-91f498fff041";
        private const string ForestInquisitorClass9Align = "ForestInquisitorClass9Align";
        private static readonly string ForestInquisitorClass9AlignGuid = "a81418aa-838b-4405-be7a-4f27800e5f19";
        private const string ForestInquisitorClass10Align = "ForestInquisitorClass10Align";
        private static readonly string ForestInquisitorClass10AlignGuid = "88d62851-05f1-494c-9920-4eef00d141ad";
        private const string ForestKineticistClass0Align = "ForestKineticistClass0Align";
        private static readonly string ForestKineticistClass0AlignGuid = "e1a33100-0a2b-463b-8816-719e20e5914d";
        internal const string ForestKineticistClass0AlignDisplayName = "ForestKineticistClass0Align.Name";
        private const string ForestKineticistClass0AlignDescription = "ForestKineticistClass0Align.Description";
        private const string ForestKineticistClass2Align = "ForestKineticistClass2Align";
        private static readonly string ForestKineticistClass2AlignGuid = "5ef0ba09-1c83-481d-af29-6ad475e3d82c";
        private const string ForestKineticistClass3Align = "ForestKineticistClass3Align";
        private static readonly string ForestKineticistClass3AlignGuid = "670063b8-fd7a-40ed-9c28-33dad77eaab7";
        private const string ForestKineticistClass4Align = "ForestKineticistClass4Align";
        private static readonly string ForestKineticistClass4AlignGuid = "66c2fb62-de38-43cb-8e17-203c3225e9bd";
        private const string ForestKineticistClass5Align = "ForestKineticistClass5Align";
        private static readonly string ForestKineticistClass5AlignGuid = "4fa33732-0080-448f-83d9-35ef7c6003ad";
        private const string ForestKineticistClass6Align = "ForestKineticistClass6Align";
        private static readonly string ForestKineticistClass6AlignGuid = "65f0b199-5ca9-4fb1-a0f7-194f331d663c";
        private const string ForestKineticistClass7Align = "ForestKineticistClass7Align";
        private static readonly string ForestKineticistClass7AlignGuid = "2e259ddf-1e58-47e9-b090-3fa41502c1b4";
        private const string ForestKineticistClass8Align = "ForestKineticistClass8Align";
        private static readonly string ForestKineticistClass8AlignGuid = "c3174cd8-5b91-4a96-a76c-36b7cfea6961";
        private const string ForestKineticistClass9Align = "ForestKineticistClass9Align";
        private static readonly string ForestKineticistClass9AlignGuid = "8dfdc9b1-be8c-4a98-bd07-1acb2556c8f2";
        private const string ForestKineticistClass10Align = "ForestKineticistClass10Align";
        private static readonly string ForestKineticistClass10AlignGuid = "de5d621f-aa9c-47db-a08c-9001b4238fd3";
        private const string ForestLoremasterClass0Align = "ForestLoremasterClass0Align";
        private static readonly string ForestLoremasterClass0AlignGuid = "97070430-03cc-488e-aef9-4b82ba3cdc25";
        internal const string ForestLoremasterClass0AlignDisplayName = "ForestLoremasterClass0Align.Name";
        private const string ForestLoremasterClass0AlignDescription = "ForestLoremasterClass0Align.Description";
        private const string ForestLoremasterClass2Align = "ForestLoremasterClass2Align";
        private static readonly string ForestLoremasterClass2AlignGuid = "22d1219c-a99a-4fcb-9c21-6ec57de1364c";
        private const string ForestLoremasterClass3Align = "ForestLoremasterClass3Align";
        private static readonly string ForestLoremasterClass3AlignGuid = "f7cc982e-5a9f-464f-8b5a-3258d1d09f74";
        private const string ForestLoremasterClass4Align = "ForestLoremasterClass4Align";
        private static readonly string ForestLoremasterClass4AlignGuid = "4e727873-d1f3-4e27-acc0-457b35b11d3d";
        private const string ForestLoremasterClass5Align = "ForestLoremasterClass5Align";
        private static readonly string ForestLoremasterClass5AlignGuid = "47a60d03-c359-4560-90be-6f4494f23f11";
        private const string ForestLoremasterClass6Align = "ForestLoremasterClass6Align";
        private static readonly string ForestLoremasterClass6AlignGuid = "d346c673-c4d1-48c1-b1df-37f6fd2b8d0e";
        private const string ForestLoremasterClass7Align = "ForestLoremasterClass7Align";
        private static readonly string ForestLoremasterClass7AlignGuid = "c3c3de21-844c-4985-8b27-475727cc2905";
        private const string ForestLoremasterClass8Align = "ForestLoremasterClass8Align";
        private static readonly string ForestLoremasterClass8AlignGuid = "092d6911-a95d-4cc7-8071-de75e1c88928";
        private const string ForestLoremasterClass9Align = "ForestLoremasterClass9Align";
        private static readonly string ForestLoremasterClass9AlignGuid = "0d78617b-2478-4316-8e4a-faf31895bbd6";
        private const string ForestLoremasterClass10Align = "ForestLoremasterClass10Align";
        private static readonly string ForestLoremasterClass10AlignGuid = "2d99042b-0d02-4c18-a4f8-1a9c1fd3c6fe";
        private const string ForestMagusClass0Align = "ForestMagusClass0Align";
        private static readonly string ForestMagusClass0AlignGuid = "6c894f1d-1e84-4cc3-a127-f8687e1ba836";
        internal const string ForestMagusClass0AlignDisplayName = "ForestMagusClass0Align.Name";
        private const string ForestMagusClass0AlignDescription = "ForestMagusClass0Align.Description";
        private const string ForestMagusClass2Align = "ForestMagusClass2Align";
        private static readonly string ForestMagusClass2AlignGuid = "52e698f7-03f1-4045-955e-456894582dd9";
        private const string ForestMagusClass3Align = "ForestMagusClass3Align";
        private static readonly string ForestMagusClass3AlignGuid = "f15560d5-620f-4a43-a4a1-549e3e4e34de";
        private const string ForestMagusClass4Align = "ForestMagusClass4Align";
        private static readonly string ForestMagusClass4AlignGuid = "9326cfb7-cdfe-4895-9a06-ed6481c777e7";
        private const string ForestMagusClass5Align = "ForestMagusClass5Align";
        private static readonly string ForestMagusClass5AlignGuid = "376c2c5d-6a5a-40ac-8841-1d18a66d3000";
        private const string ForestMagusClass6Align = "ForestMagusClass6Align";
        private static readonly string ForestMagusClass6AlignGuid = "7e71cf74-fb39-459f-911d-d91f2d89169b";
        private const string ForestMagusClass7Align = "ForestMagusClass7Align";
        private static readonly string ForestMagusClass7AlignGuid = "cc874cab-cd9b-49af-8fa9-6d71798fcb38";
        private const string ForestMagusClass8Align = "ForestMagusClass8Align";
        private static readonly string ForestMagusClass8AlignGuid = "3b9d1c58-3dc1-4940-874e-78b32499a6f1";
        private const string ForestMagusClass9Align = "ForestMagusClass9Align";
        private static readonly string ForestMagusClass9AlignGuid = "b39a00e2-3f9d-45c8-8434-3ae69387f4e0";
        private const string ForestMagusClass10Align = "ForestMagusClass10Align";
        private static readonly string ForestMagusClass10AlignGuid = "4d4d535a-ef0e-4083-88f7-8d0c9ded145b";
        private const string ForestMonkClass0Align = "ForestMonkClass0Align";
        private static readonly string ForestMonkClass0AlignGuid = "b5c6c9d0-1cc3-4c5e-b23e-bdaff804e865";
        internal const string ForestMonkClass0AlignDisplayName = "ForestMonkClass0Align.Name";
        private const string ForestMonkClass0AlignDescription = "ForestMonkClass0Align.Description";
        private const string ForestMonkClass2Align = "ForestMonkClass2Align";
        private static readonly string ForestMonkClass2AlignGuid = "1f7e5694-be91-43f5-b22e-9a9189568306";
        private const string ForestMonkClass3Align = "ForestMonkClass3Align";
        private static readonly string ForestMonkClass3AlignGuid = "e7d52a0b-2db3-47d4-b524-e1cc1174ae37";
        private const string ForestMonkClass4Align = "ForestMonkClass4Align";
        private static readonly string ForestMonkClass4AlignGuid = "a7d3ca14-95ac-47fe-8513-a4d9dce7e0b0";
        private const string ForestMonkClass5Align = "ForestMonkClass5Align";
        private static readonly string ForestMonkClass5AlignGuid = "2bd57d53-c3e8-46ac-8908-3413a1488475";
        private const string ForestMonkClass6Align = "ForestMonkClass6Align";
        private static readonly string ForestMonkClass6AlignGuid = "7fa4dd34-4a75-4c09-8384-6a51745b22d7";
        private const string ForestMonkClass7Align = "ForestMonkClass7Align";
        private static readonly string ForestMonkClass7AlignGuid = "db9af207-0080-4a77-ac35-ae2f4e4568ed";
        private const string ForestMonkClass8Align = "ForestMonkClass8Align";
        private static readonly string ForestMonkClass8AlignGuid = "683b41c2-f988-4663-a2a9-551e60824bdd";
        private const string ForestMonkClass9Align = "ForestMonkClass9Align";
        private static readonly string ForestMonkClass9AlignGuid = "58ee0531-d03e-4d09-aaac-aef3838d2429";
        private const string ForestMonkClass10Align = "ForestMonkClass10Align";
        private static readonly string ForestMonkClass10AlignGuid = "ea06bdbe-8e36-488a-be8a-09f8543c2608";
        private const string ForestMysticTheurgeClass0Align = "ForestMysticTheurgeClass0Align";
        private static readonly string ForestMysticTheurgeClass0AlignGuid = "378dac29-6702-4cc4-8b53-868788280714";
        internal const string ForestMysticTheurgeClass0AlignDisplayName = "ForestMysticTheurgeClass0Align.Name";
        private const string ForestMysticTheurgeClass0AlignDescription = "ForestMysticTheurgeClass0Align.Description";
        private const string ForestMysticTheurgeClass2Align = "ForestMysticTheurgeClass2Align";
        private static readonly string ForestMysticTheurgeClass2AlignGuid = "5be79d14-a1be-4b80-89d5-160e1513fdab";
        private const string ForestMysticTheurgeClass3Align = "ForestMysticTheurgeClass3Align";
        private static readonly string ForestMysticTheurgeClass3AlignGuid = "2b229ed2-faff-45d9-a05c-ef400c2a1ac5";
        private const string ForestMysticTheurgeClass4Align = "ForestMysticTheurgeClass4Align";
        private static readonly string ForestMysticTheurgeClass4AlignGuid = "5ccdbba5-1f24-4f9a-a87c-b69e10f46789";
        private const string ForestMysticTheurgeClass5Align = "ForestMysticTheurgeClass5Align";
        private static readonly string ForestMysticTheurgeClass5AlignGuid = "73e5bc44-eba1-4dac-8e45-3257a6f92810";
        private const string ForestMysticTheurgeClass6Align = "ForestMysticTheurgeClass6Align";
        private static readonly string ForestMysticTheurgeClass6AlignGuid = "c4fa9f58-30d4-4c11-87e3-538e2bb936fa";
        private const string ForestMysticTheurgeClass7Align = "ForestMysticTheurgeClass7Align";
        private static readonly string ForestMysticTheurgeClass7AlignGuid = "fa1181a0-14cc-48ad-85c0-43eb13097efd";
        private const string ForestMysticTheurgeClass8Align = "ForestMysticTheurgeClass8Align";
        private static readonly string ForestMysticTheurgeClass8AlignGuid = "9f7af8d2-b0aa-40f7-81d3-204c88c72da8";
        private const string ForestMysticTheurgeClass9Align = "ForestMysticTheurgeClass9Align";
        private static readonly string ForestMysticTheurgeClass9AlignGuid = "c014ddb9-8703-48f1-b441-47e5cf150f74";
        private const string ForestMysticTheurgeClass10Align = "ForestMysticTheurgeClass10Align";
        private static readonly string ForestMysticTheurgeClass10AlignGuid = "211208a9-5361-4a83-8165-35073e8e995b";
        private const string ForestOracleClass0Align = "ForestOracleClass0Align";
        private static readonly string ForestOracleClass0AlignGuid = "29392cdb-8f0d-4c55-9324-f34c74809f18";
        internal const string ForestOracleClass0AlignDisplayName = "ForestOracleClass0Align.Name";
        private const string ForestOracleClass0AlignDescription = "ForestOracleClass0Align.Description";
        private const string ForestOracleClass2Align = "ForestOracleClass2Align";
        private static readonly string ForestOracleClass2AlignGuid = "d2f65201-981a-4b0e-bda4-a24066faf2eb";
        private const string ForestOracleClass3Align = "ForestOracleClass3Align";
        private static readonly string ForestOracleClass3AlignGuid = "c304a13a-f0e1-4f01-8f35-d1bfd7df6700";
        private const string ForestOracleClass4Align = "ForestOracleClass4Align";
        private static readonly string ForestOracleClass4AlignGuid = "a82c8337-6ef8-47a7-a2b0-6b817d92d18c";
        private const string ForestOracleClass5Align = "ForestOracleClass5Align";
        private static readonly string ForestOracleClass5AlignGuid = "fe5356ef-74d7-4edb-b8b7-1941f1731785";
        private const string ForestOracleClass6Align = "ForestOracleClass6Align";
        private static readonly string ForestOracleClass6AlignGuid = "a35e8dc5-e02c-4ddd-9569-a0f8786cf13f";
        private const string ForestOracleClass7Align = "ForestOracleClass7Align";
        private static readonly string ForestOracleClass7AlignGuid = "4335657d-1fb1-410f-b478-7bea715160e3";
        private const string ForestOracleClass8Align = "ForestOracleClass8Align";
        private static readonly string ForestOracleClass8AlignGuid = "c3097e9c-e2be-4379-a300-66bff2749905";
        private const string ForestOracleClass9Align = "ForestOracleClass9Align";
        private static readonly string ForestOracleClass9AlignGuid = "5936cf65-9a33-47d4-b49e-701916701ef6";
        private const string ForestOracleClass10Align = "ForestOracleClass10Align";
        private static readonly string ForestOracleClass10AlignGuid = "e7247cbb-b78b-42d0-80bc-6d54625f7b33";
        private const string ForestPaladinClass0Align = "ForestPaladinClass0Align";
        private static readonly string ForestPaladinClass0AlignGuid = "d6adfadf-5b67-4ff9-b3d8-f98467e3be98";
        internal const string ForestPaladinClass0AlignDisplayName = "ForestPaladinClass0Align.Name";
        private const string ForestPaladinClass0AlignDescription = "ForestPaladinClass0Align.Description";
        private const string ForestPaladinClass2Align = "ForestPaladinClass2Align";
        private static readonly string ForestPaladinClass2AlignGuid = "4eee46d5-ccc5-4535-bf57-576072215500";
        private const string ForestPaladinClass3Align = "ForestPaladinClass3Align";
        private static readonly string ForestPaladinClass3AlignGuid = "84ae6cf1-acff-4b81-9a44-61655b76c91e";
        private const string ForestPaladinClass4Align = "ForestPaladinClass4Align";
        private static readonly string ForestPaladinClass4AlignGuid = "353a1a73-bd6b-4c96-97d1-9b4dc458d045";
        private const string ForestPaladinClass5Align = "ForestPaladinClass5Align";
        private static readonly string ForestPaladinClass5AlignGuid = "bc328e40-b42d-4c5d-a287-86b6bfc7eddd";
        private const string ForestPaladinClass6Align = "ForestPaladinClass6Align";
        private static readonly string ForestPaladinClass6AlignGuid = "5c2ba78d-ce6e-4757-b009-577aff60816d";
        private const string ForestPaladinClass7Align = "ForestPaladinClass7Align";
        private static readonly string ForestPaladinClass7AlignGuid = "3a70e05f-a39e-4386-92af-847226a8497a";
        private const string ForestPaladinClass8Align = "ForestPaladinClass8Align";
        private static readonly string ForestPaladinClass8AlignGuid = "2c894769-e3d7-4fb7-8a6f-e09503e526e1";
        private const string ForestPaladinClass9Align = "ForestPaladinClass9Align";
        private static readonly string ForestPaladinClass9AlignGuid = "2792d386-5f99-4ca6-b861-6f0bf1f56222";
        private const string ForestPaladinClass10Align = "ForestPaladinClass10Align";
        private static readonly string ForestPaladinClass10AlignGuid = "04265f64-72c9-4cb1-a91d-f2f911ca39f2";
        private const string ForestRangerClass0Align = "ForestRangerClass0Align";
        private static readonly string ForestRangerClass0AlignGuid = "5b9c5f2d-09c8-458a-abfc-3e0f673f81bb";
        internal const string ForestRangerClass0AlignDisplayName = "ForestRangerClass0Align.Name";
        private const string ForestRangerClass0AlignDescription = "ForestRangerClass0Align.Description";
        private const string ForestRangerClass2Align = "ForestRangerClass2Align";
        private static readonly string ForestRangerClass2AlignGuid = "a110cdba-a5a4-4aee-885c-e0eea5f5dc65";
        private const string ForestRangerClass3Align = "ForestRangerClass3Align";
        private static readonly string ForestRangerClass3AlignGuid = "43ad3c6f-1975-465b-8dc5-9e5b5d3634d2";
        private const string ForestRangerClass4Align = "ForestRangerClass4Align";
        private static readonly string ForestRangerClass4AlignGuid = "02fa6776-d0ae-4d91-bd08-03167c7dbd76";
        private const string ForestRangerClass5Align = "ForestRangerClass5Align";
        private static readonly string ForestRangerClass5AlignGuid = "96035b55-45b2-470f-8633-c76b7236487d";
        private const string ForestRangerClass6Align = "ForestRangerClass6Align";
        private static readonly string ForestRangerClass6AlignGuid = "825ba8ec-3901-4e50-964b-78fa655d7dcf";
        private const string ForestRangerClass7Align = "ForestRangerClass7Align";
        private static readonly string ForestRangerClass7AlignGuid = "618342e4-5c45-4ab4-9df6-0cabb1349547";
        private const string ForestRangerClass8Align = "ForestRangerClass8Align";
        private static readonly string ForestRangerClass8AlignGuid = "cee0ffd1-4df0-42c3-a071-4aa6d1e403e2";
        private const string ForestRangerClass9Align = "ForestRangerClass9Align";
        private static readonly string ForestRangerClass9AlignGuid = "0bd6997f-f787-40a8-b8b6-4b9bf9993b23";
        private const string ForestRangerClass10Align = "ForestRangerClass10Align";
        private static readonly string ForestRangerClass10AlignGuid = "e2837398-8264-422b-b805-ad80ca2ff01b";
        private const string ForestRogueClass0Align = "ForestRogueClass0Align";
        private static readonly string ForestRogueClass0AlignGuid = "4a4e1ef1-3b1f-4def-bfda-54d3d662c37e";
        internal const string ForestRogueClass0AlignDisplayName = "ForestRogueClass0Align.Name";
        private const string ForestRogueClass0AlignDescription = "ForestRogueClass0Align.Description";
        private const string ForestRogueClass2Align = "ForestRogueClass2Align";
        private static readonly string ForestRogueClass2AlignGuid = "546706d3-9eaa-4a8d-8b57-e4524bee456b";
        private const string ForestRogueClass3Align = "ForestRogueClass3Align";
        private static readonly string ForestRogueClass3AlignGuid = "182b577c-ed07-4424-b23c-26c5341c7df2";
        private const string ForestRogueClass4Align = "ForestRogueClass4Align";
        private static readonly string ForestRogueClass4AlignGuid = "44404a84-d271-431e-a70c-9ed420fec7bd";
        private const string ForestRogueClass5Align = "ForestRogueClass5Align";
        private static readonly string ForestRogueClass5AlignGuid = "ef015f0f-531e-484e-83c8-f0a4c0ad1f76";
        private const string ForestRogueClass6Align = "ForestRogueClass6Align";
        private static readonly string ForestRogueClass6AlignGuid = "028dd161-fee0-4309-8b9d-252992ff6eda";
        private const string ForestRogueClass7Align = "ForestRogueClass7Align";
        private static readonly string ForestRogueClass7AlignGuid = "aeb343a7-f4c3-45cd-b763-ff0ccec56ac8";
        private const string ForestRogueClass8Align = "ForestRogueClass8Align";
        private static readonly string ForestRogueClass8AlignGuid = "34497966-63b0-44d8-ba22-d29ef2c3f4c4";
        private const string ForestRogueClass9Align = "ForestRogueClass9Align";
        private static readonly string ForestRogueClass9AlignGuid = "92729670-1625-432c-bb5c-5ba473aa58ed";
        private const string ForestRogueClass10Align = "ForestRogueClass10Align";
        private static readonly string ForestRogueClass10AlignGuid = "f0719b14-6ee2-4aa0-a005-1758cabe39be";
        private const string ForestShamanClass0Align = "ForestShamanClass0Align";
        private static readonly string ForestShamanClass0AlignGuid = "252d7657-99a3-40af-9774-34805d168c13";
        internal const string ForestShamanClass0AlignDisplayName = "ForestShamanClass0Align.Name";
        private const string ForestShamanClass0AlignDescription = "ForestShamanClass0Align.Description";
        private const string ForestShamanClass2Align = "ForestShamanClass2Align";
        private static readonly string ForestShamanClass2AlignGuid = "c46f4996-a17d-489e-9180-eaf2acf232a1";
        private const string ForestShamanClass3Align = "ForestShamanClass3Align";
        private static readonly string ForestShamanClass3AlignGuid = "785b064b-66c3-4e6d-98d2-af38ca31ecd8";
        private const string ForestShamanClass4Align = "ForestShamanClass4Align";
        private static readonly string ForestShamanClass4AlignGuid = "7895ee4f-850f-4300-902c-03bcb00cba94";
        private const string ForestShamanClass5Align = "ForestShamanClass5Align";
        private static readonly string ForestShamanClass5AlignGuid = "9580fdd1-70b2-4ce3-88e4-d7c5b4f8367a";
        private const string ForestShamanClass6Align = "ForestShamanClass6Align";
        private static readonly string ForestShamanClass6AlignGuid = "b7f61fd5-7cba-42d9-80ad-f76c0cd5d871";
        private const string ForestShamanClass7Align = "ForestShamanClass7Align";
        private static readonly string ForestShamanClass7AlignGuid = "f77fc7cd-4c85-4982-a6d2-fad00d2b436b";
        private const string ForestShamanClass8Align = "ForestShamanClass8Align";
        private static readonly string ForestShamanClass8AlignGuid = "65b7bffd-3d12-4ef7-ad09-a46206650ee6";
        private const string ForestShamanClass9Align = "ForestShamanClass9Align";
        private static readonly string ForestShamanClass9AlignGuid = "69c34f85-8564-4c62-be9b-b6f962bcddc8";
        private const string ForestShamanClass10Align = "ForestShamanClass10Align";
        private static readonly string ForestShamanClass10AlignGuid = "51947e52-5a20-4f58-98dc-ed5818479ecc";
        private const string ForestShifterClass0Align = "ForestShifterClass0Align";
        private static readonly string ForestShifterClass0AlignGuid = "d9bf6cf5-ddab-4027-8a8b-08e1f6e0b803";
        internal const string ForestShifterClass0AlignDisplayName = "ForestShifterClass0Align.Name";
        private const string ForestShifterClass0AlignDescription = "ForestShifterClass0Align.Description";
        private const string ForestShifterClass2Align = "ForestShifterClass2Align";
        private static readonly string ForestShifterClass2AlignGuid = "4dbe93b4-9739-4d10-be8c-640c3e31d970";
        private const string ForestShifterClass3Align = "ForestShifterClass3Align";
        private static readonly string ForestShifterClass3AlignGuid = "d44fc6ff-90ba-451d-bcf0-7cf63e59aba9";
        private const string ForestShifterClass4Align = "ForestShifterClass4Align";
        private static readonly string ForestShifterClass4AlignGuid = "ed5556a4-1eee-4ac3-8507-ac5549b73187";
        private const string ForestShifterClass5Align = "ForestShifterClass5Align";
        private static readonly string ForestShifterClass5AlignGuid = "48af70d1-9d40-4090-a8eb-b184a65d195f";
        private const string ForestShifterClass6Align = "ForestShifterClass6Align";
        private static readonly string ForestShifterClass6AlignGuid = "ce3e889d-0169-4c64-9902-54edee14804f";
        private const string ForestShifterClass7Align = "ForestShifterClass7Align";
        private static readonly string ForestShifterClass7AlignGuid = "14b2701b-ecc9-4294-b289-f8617928a668";
        private const string ForestShifterClass8Align = "ForestShifterClass8Align";
        private static readonly string ForestShifterClass8AlignGuid = "2d826cdb-e126-4bc9-833d-d1e4ca545bbc";
        private const string ForestShifterClass9Align = "ForestShifterClass9Align";
        private static readonly string ForestShifterClass9AlignGuid = "212b6ac8-2772-4ed7-89aa-b73ed624ccbe";
        private const string ForestShifterClass10Align = "ForestShifterClass10Align";
        private static readonly string ForestShifterClass10AlignGuid = "0dec3c59-882f-4457-a85e-c9ef845e1c6d";
        private const string ForestSkaldClass0Align = "ForestSkaldClass0Align";
        private static readonly string ForestSkaldClass0AlignGuid = "88d73e7c-aae9-479e-93c3-edce9401c312";
        internal const string ForestSkaldClass0AlignDisplayName = "ForestSkaldClass0Align.Name";
        private const string ForestSkaldClass0AlignDescription = "ForestSkaldClass0Align.Description";
        private const string ForestSkaldClass2Align = "ForestSkaldClass2Align";
        private static readonly string ForestSkaldClass2AlignGuid = "2530a6d6-d2e4-4351-8e44-3e3cef70c245";
        private const string ForestSkaldClass3Align = "ForestSkaldClass3Align";
        private static readonly string ForestSkaldClass3AlignGuid = "032ae803-327c-4a8b-821d-709ebac9a552";
        private const string ForestSkaldClass4Align = "ForestSkaldClass4Align";
        private static readonly string ForestSkaldClass4AlignGuid = "2087ea56-21ea-46d0-9940-b096c0de0621";
        private const string ForestSkaldClass5Align = "ForestSkaldClass5Align";
        private static readonly string ForestSkaldClass5AlignGuid = "b8629427-89c3-4655-881e-bbac50d37253";
        private const string ForestSkaldClass6Align = "ForestSkaldClass6Align";
        private static readonly string ForestSkaldClass6AlignGuid = "dd3ba7b0-3df8-43ad-b048-fedb7c131919";
        private const string ForestSkaldClass7Align = "ForestSkaldClass7Align";
        private static readonly string ForestSkaldClass7AlignGuid = "be99650b-285c-48b3-b0e7-9b8741823d93";
        private const string ForestSkaldClass8Align = "ForestSkaldClass8Align";
        private static readonly string ForestSkaldClass8AlignGuid = "edaab55b-deb3-4ed7-bbc7-ec3aab7b334b";
        private const string ForestSkaldClass9Align = "ForestSkaldClass9Align";
        private static readonly string ForestSkaldClass9AlignGuid = "e70124c9-7478-4248-ac32-611cf138982e";
        private const string ForestSkaldClass10Align = "ForestSkaldClass10Align";
        private static readonly string ForestSkaldClass10AlignGuid = "e1915be9-c244-464e-81fc-e552b627e222";
        private const string ForestSlayerClass0Align = "ForestSlayerClass0Align";
        private static readonly string ForestSlayerClass0AlignGuid = "4b06a93b-4797-4ffb-a029-9bb885f779b7";
        internal const string ForestSlayerClass0AlignDisplayName = "ForestSlayerClass0Align.Name";
        private const string ForestSlayerClass0AlignDescription = "ForestSlayerClass0Align.Description";
        private const string ForestSlayerClass2Align = "ForestSlayerClass2Align";
        private static readonly string ForestSlayerClass2AlignGuid = "6eb7dc74-7bc6-4df9-8667-a1375e4d3c69";
        private const string ForestSlayerClass3Align = "ForestSlayerClass3Align";
        private static readonly string ForestSlayerClass3AlignGuid = "5f6f6428-2b76-4cb7-af25-a1f94f978c4b";
        private const string ForestSlayerClass4Align = "ForestSlayerClass4Align";
        private static readonly string ForestSlayerClass4AlignGuid = "99e4ab5d-1627-4b7a-94ef-d7e60a20afab";
        private const string ForestSlayerClass5Align = "ForestSlayerClass5Align";
        private static readonly string ForestSlayerClass5AlignGuid = "c2623487-01ab-445f-861a-062b13e9a18e";
        private const string ForestSlayerClass6Align = "ForestSlayerClass6Align";
        private static readonly string ForestSlayerClass6AlignGuid = "22444318-dd4d-4fbf-8b74-0b17f234a05b";
        private const string ForestSlayerClass7Align = "ForestSlayerClass7Align";
        private static readonly string ForestSlayerClass7AlignGuid = "0fc8b027-a517-4cbd-a65a-6492ccacc162";
        private const string ForestSlayerClass8Align = "ForestSlayerClass8Align";
        private static readonly string ForestSlayerClass8AlignGuid = "65e7356e-5773-4131-b222-1e87103492f1";
        private const string ForestSlayerClass9Align = "ForestSlayerClass9Align";
        private static readonly string ForestSlayerClass9AlignGuid = "525450ef-8726-4844-80f6-37a998b1e885";
        private const string ForestSlayerClass10Align = "ForestSlayerClass10Align";
        private static readonly string ForestSlayerClass10AlignGuid = "fc922ab1-2a6d-4c9c-ba0a-17482df7599d";
        private const string ForestSorcererClass0Align = "ForestSorcererClass0Align";
        private static readonly string ForestSorcererClass0AlignGuid = "6fc1954f-4e13-4bcb-be97-d008faaac2fb";
        internal const string ForestSorcererClass0AlignDisplayName = "ForestSorcererClass0Align.Name";
        private const string ForestSorcererClass0AlignDescription = "ForestSorcererClass0Align.Description";
        private const string ForestSorcererClass2Align = "ForestSorcererClass2Align";
        private static readonly string ForestSorcererClass2AlignGuid = "f45cf13c-8d66-4ef3-880f-bb63c88f1aa2";
        private const string ForestSorcererClass3Align = "ForestSorcererClass3Align";
        private static readonly string ForestSorcererClass3AlignGuid = "b60419a4-6776-4b6b-a19d-f4c7fa11c382";
        private const string ForestSorcererClass4Align = "ForestSorcererClass4Align";
        private static readonly string ForestSorcererClass4AlignGuid = "6a75abd1-89e0-4e59-a3ae-9e310031e268";
        private const string ForestSorcererClass5Align = "ForestSorcererClass5Align";
        private static readonly string ForestSorcererClass5AlignGuid = "039f834e-75a6-4066-8522-d96d162b9cb6";
        private const string ForestSorcererClass6Align = "ForestSorcererClass6Align";
        private static readonly string ForestSorcererClass6AlignGuid = "a8b54ee4-c081-43c2-a6d2-5c08d2d74723";
        private const string ForestSorcererClass7Align = "ForestSorcererClass7Align";
        private static readonly string ForestSorcererClass7AlignGuid = "7f6e3d66-862e-4205-a44f-a91614a63af2";
        private const string ForestSorcererClass8Align = "ForestSorcererClass8Align";
        private static readonly string ForestSorcererClass8AlignGuid = "7321893c-1f17-4287-9224-f4b5c29563ed";
        private const string ForestSorcererClass9Align = "ForestSorcererClass9Align";
        private static readonly string ForestSorcererClass9AlignGuid = "82422b74-58a8-472b-94aa-ea8c8e7d37c1";
        private const string ForestSorcererClass10Align = "ForestSorcererClass10Align";
        private static readonly string ForestSorcererClass10AlignGuid = "e0060fd3-d6cc-4196-acc6-a9f47fb61299";
        private const string ForestStalwartDefenderClass0Align = "ForestStalwartDefenderClass0Align";
        private static readonly string ForestStalwartDefenderClass0AlignGuid = "71d11ce0-a32f-4183-84e4-59ee67e0ca7f";
        internal const string ForestStalwartDefenderClass0AlignDisplayName = "ForestStalwartDefenderClass0Align.Name";
        private const string ForestStalwartDefenderClass0AlignDescription = "ForestStalwartDefenderClass0Align.Description";
        private const string ForestStalwartDefenderClass2Align = "ForestStalwartDefenderClass2Align";
        private static readonly string ForestStalwartDefenderClass2AlignGuid = "f49251bd-5c04-4ff0-b6d7-16cdae5cb2c4";
        private const string ForestStalwartDefenderClass3Align = "ForestStalwartDefenderClass3Align";
        private static readonly string ForestStalwartDefenderClass3AlignGuid = "f3cd5049-ccf5-4fbe-bf2b-3469ffb0dc97";
        private const string ForestStalwartDefenderClass4Align = "ForestStalwartDefenderClass4Align";
        private static readonly string ForestStalwartDefenderClass4AlignGuid = "6d847ea4-69ec-4130-8659-c91ebc21a49a";
        private const string ForestStalwartDefenderClass5Align = "ForestStalwartDefenderClass5Align";
        private static readonly string ForestStalwartDefenderClass5AlignGuid = "f821e127-52dd-432e-a8b0-405d7aedb5a4";
        private const string ForestStalwartDefenderClass6Align = "ForestStalwartDefenderClass6Align";
        private static readonly string ForestStalwartDefenderClass6AlignGuid = "8c4d5798-a8c8-4732-aaed-93dbf4d10440";
        private const string ForestStalwartDefenderClass7Align = "ForestStalwartDefenderClass7Align";
        private static readonly string ForestStalwartDefenderClass7AlignGuid = "4f646df0-d98f-4470-9f51-115049fdac04";
        private const string ForestStalwartDefenderClass8Align = "ForestStalwartDefenderClass8Align";
        private static readonly string ForestStalwartDefenderClass8AlignGuid = "b2dfb2fe-20b7-4148-a488-498e9f96d887";
        private const string ForestStalwartDefenderClass9Align = "ForestStalwartDefenderClass9Align";
        private static readonly string ForestStalwartDefenderClass9AlignGuid = "3868e593-3986-4f3b-83b8-e4f5b5354858";
        private const string ForestStalwartDefenderClass10Align = "ForestStalwartDefenderClass10Align";
        private static readonly string ForestStalwartDefenderClass10AlignGuid = "6146fc70-bdcd-48a4-8b83-97e596150fbc";
        private const string ForestStudentOfWarClass0Align = "ForestStudentOfWarClass0Align";
        private static readonly string ForestStudentOfWarClass0AlignGuid = "d59fdbc7-9c13-4dcd-b075-1f2e7dea6e1d";
        internal const string ForestStudentOfWarClass0AlignDisplayName = "ForestStudentOfWarClass0Align.Name";
        private const string ForestStudentOfWarClass0AlignDescription = "ForestStudentOfWarClass0Align.Description";
        private const string ForestStudentOfWarClass2Align = "ForestStudentOfWarClass2Align";
        private static readonly string ForestStudentOfWarClass2AlignGuid = "265ad9a5-5481-4c1c-b466-441ad34693e1";
        private const string ForestStudentOfWarClass3Align = "ForestStudentOfWarClass3Align";
        private static readonly string ForestStudentOfWarClass3AlignGuid = "b8912bb3-1230-4d2a-9742-ff16b04b559f";
        private const string ForestStudentOfWarClass4Align = "ForestStudentOfWarClass4Align";
        private static readonly string ForestStudentOfWarClass4AlignGuid = "aeb37a73-ef40-47b9-aac4-796e15989119";
        private const string ForestStudentOfWarClass5Align = "ForestStudentOfWarClass5Align";
        private static readonly string ForestStudentOfWarClass5AlignGuid = "8cf1530d-a1ec-4589-a71d-604e336fd8ae";
        private const string ForestStudentOfWarClass6Align = "ForestStudentOfWarClass6Align";
        private static readonly string ForestStudentOfWarClass6AlignGuid = "0a7d41cb-b951-47e6-98c9-d98d7a082144";
        private const string ForestStudentOfWarClass7Align = "ForestStudentOfWarClass7Align";
        private static readonly string ForestStudentOfWarClass7AlignGuid = "6b776966-03e5-47de-8528-1aad11f9b2e4";
        private const string ForestStudentOfWarClass8Align = "ForestStudentOfWarClass8Align";
        private static readonly string ForestStudentOfWarClass8AlignGuid = "49ed3618-06dd-4c47-b6e8-c658746dcc5a";
        private const string ForestStudentOfWarClass9Align = "ForestStudentOfWarClass9Align";
        private static readonly string ForestStudentOfWarClass9AlignGuid = "f999f069-5d61-4aa7-b39e-b8da61dcb520";
        private const string ForestStudentOfWarClass10Align = "ForestStudentOfWarClass10Align";
        private static readonly string ForestStudentOfWarClass10AlignGuid = "909700d7-ee4e-4b7d-81a5-72cf82d61b7d";
        private const string ForestSwordlordClass0Align = "ForestSwordlordClass0Align";
        private static readonly string ForestSwordlordClass0AlignGuid = "ebeb3013-4ace-45a3-b902-644a54bc5bc6";
        internal const string ForestSwordlordClass0AlignDisplayName = "ForestSwordlordClass0Align.Name";
        private const string ForestSwordlordClass0AlignDescription = "ForestSwordlordClass0Align.Description";
        private const string ForestSwordlordClass2Align = "ForestSwordlordClass2Align";
        private static readonly string ForestSwordlordClass2AlignGuid = "c271367b-dc4e-4071-9bd2-6a98655539e9";
        private const string ForestSwordlordClass3Align = "ForestSwordlordClass3Align";
        private static readonly string ForestSwordlordClass3AlignGuid = "19c39703-6e1b-4a2a-ae02-6093d2fc633b";
        private const string ForestSwordlordClass4Align = "ForestSwordlordClass4Align";
        private static readonly string ForestSwordlordClass4AlignGuid = "3ea30225-abbe-47bf-bf13-5051d42dd8eb";
        private const string ForestSwordlordClass5Align = "ForestSwordlordClass5Align";
        private static readonly string ForestSwordlordClass5AlignGuid = "ff9b521b-564c-4617-aff2-089753171611";
        private const string ForestSwordlordClass6Align = "ForestSwordlordClass6Align";
        private static readonly string ForestSwordlordClass6AlignGuid = "0d6d9de0-8c0d-4f99-90bc-f16f265c3df9";
        private const string ForestSwordlordClass7Align = "ForestSwordlordClass7Align";
        private static readonly string ForestSwordlordClass7AlignGuid = "11d9761e-3d1c-4b1b-a43a-6c30b199fd8e";
        private const string ForestSwordlordClass8Align = "ForestSwordlordClass8Align";
        private static readonly string ForestSwordlordClass8AlignGuid = "8b6bb9f0-bde9-4381-a71e-9ac74040d2d4";
        private const string ForestSwordlordClass9Align = "ForestSwordlordClass9Align";
        private static readonly string ForestSwordlordClass9AlignGuid = "b1de34d8-dc66-4cb6-92ab-a942d29305c4";
        private const string ForestSwordlordClass10Align = "ForestSwordlordClass10Align";
        private static readonly string ForestSwordlordClass10AlignGuid = "71f1814b-8c0e-47dc-ae4a-019151f85641";
        private const string ForestWarpriestClass0Align = "ForestWarpriestClass0Align";
        private static readonly string ForestWarpriestClass0AlignGuid = "ae422421-156c-4a84-965d-ecfb694fdd78";
        internal const string ForestWarpriestClass0AlignDisplayName = "ForestWarpriestClass0Align.Name";
        private const string ForestWarpriestClass0AlignDescription = "ForestWarpriestClass0Align.Description";
        private const string ForestWarpriestClass2Align = "ForestWarpriestClass2Align";
        private static readonly string ForestWarpriestClass2AlignGuid = "9a83a991-8ad9-43ab-b3b9-9c582a9b0d89";
        private const string ForestWarpriestClass3Align = "ForestWarpriestClass3Align";
        private static readonly string ForestWarpriestClass3AlignGuid = "a2e4eb45-bcbc-4b38-8f41-627832bac8e7";
        private const string ForestWarpriestClass4Align = "ForestWarpriestClass4Align";
        private static readonly string ForestWarpriestClass4AlignGuid = "92602d8d-506c-4bc1-807a-80b7a1815cec";
        private const string ForestWarpriestClass5Align = "ForestWarpriestClass5Align";
        private static readonly string ForestWarpriestClass5AlignGuid = "79a7805a-e067-4343-b187-606aa1137b79";
        private const string ForestWarpriestClass6Align = "ForestWarpriestClass6Align";
        private static readonly string ForestWarpriestClass6AlignGuid = "1f07bd2a-d45b-4779-a84a-f5059a47009a";
        private const string ForestWarpriestClass7Align = "ForestWarpriestClass7Align";
        private static readonly string ForestWarpriestClass7AlignGuid = "8cb9c31a-88f9-49b6-91d2-964cb665c65b";
        private const string ForestWarpriestClass8Align = "ForestWarpriestClass8Align";
        private static readonly string ForestWarpriestClass8AlignGuid = "50c629a5-3bc4-4a1c-ac1d-a85c87b4cff2";
        private const string ForestWarpriestClass9Align = "ForestWarpriestClass9Align";
        private static readonly string ForestWarpriestClass9AlignGuid = "b0a90fc0-2987-4deb-a38f-7a70e466eeb9";
        private const string ForestWarpriestClass10Align = "ForestWarpriestClass10Align";
        private static readonly string ForestWarpriestClass10AlignGuid = "b53df5cf-5195-48df-b568-52f33d112cec";
        private const string ForestWinterWitchClass0Align = "ForestWinterWitchClass0Align";
        private static readonly string ForestWinterWitchClass0AlignGuid = "c035f3df-e6a2-40e5-9afb-9de86055e27f";
        internal const string ForestWinterWitchClass0AlignDisplayName = "ForestWinterWitchClass0Align.Name";
        private const string ForestWinterWitchClass0AlignDescription = "ForestWinterWitchClass0Align.Description";
        private const string ForestWinterWitchClass2Align = "ForestWinterWitchClass2Align";
        private static readonly string ForestWinterWitchClass2AlignGuid = "d53e181b-8c46-423b-807d-bb2b517f257f";
        private const string ForestWinterWitchClass3Align = "ForestWinterWitchClass3Align";
        private static readonly string ForestWinterWitchClass3AlignGuid = "7b9cf762-5620-49fb-9a09-3f94e030daa1";
        private const string ForestWinterWitchClass4Align = "ForestWinterWitchClass4Align";
        private static readonly string ForestWinterWitchClass4AlignGuid = "d44c6a79-a8a1-4600-b966-d690136cff9a";
        private const string ForestWinterWitchClass5Align = "ForestWinterWitchClass5Align";
        private static readonly string ForestWinterWitchClass5AlignGuid = "019c8ec8-50d1-4822-a4f8-7a6e4146e306";
        private const string ForestWinterWitchClass6Align = "ForestWinterWitchClass6Align";
        private static readonly string ForestWinterWitchClass6AlignGuid = "3c5ebe22-922d-4b1a-a66a-1fd747ca648e";
        private const string ForestWinterWitchClass7Align = "ForestWinterWitchClass7Align";
        private static readonly string ForestWinterWitchClass7AlignGuid = "c2e237fb-0818-4886-8dea-6b65eb856e18";
        private const string ForestWinterWitchClass8Align = "ForestWinterWitchClass8Align";
        private static readonly string ForestWinterWitchClass8AlignGuid = "a729e214-891b-4a12-b682-65064794f78c";
        private const string ForestWinterWitchClass9Align = "ForestWinterWitchClass9Align";
        private static readonly string ForestWinterWitchClass9AlignGuid = "aa15fd6e-27e8-4478-8225-77750f58d2eb";
        private const string ForestWinterWitchClass10Align = "ForestWinterWitchClass10Align";
        private static readonly string ForestWinterWitchClass10AlignGuid = "fc0ab24e-1955-4b04-adc6-3b18c8062de7";
        private const string ForestWitchClass0Align = "ForestWitchClass0Align";
        private static readonly string ForestWitchClass0AlignGuid = "ac04914e-b0ef-4959-bf9b-e2bef4ac827f";
        internal const string ForestWitchClass0AlignDisplayName = "ForestWitchClass0Align.Name";
        private const string ForestWitchClass0AlignDescription = "ForestWitchClass0Align.Description";
        private const string ForestWitchClass2Align = "ForestWitchClass2Align";
        private static readonly string ForestWitchClass2AlignGuid = "8e42fa8f-b7be-4870-baae-6d1a660a6f08";
        private const string ForestWitchClass3Align = "ForestWitchClass3Align";
        private static readonly string ForestWitchClass3AlignGuid = "57a63b2d-2a64-487f-81cd-baf745d54a7e";
        private const string ForestWitchClass4Align = "ForestWitchClass4Align";
        private static readonly string ForestWitchClass4AlignGuid = "e147767f-a7b4-4625-afa7-a163993d7a66";
        private const string ForestWitchClass5Align = "ForestWitchClass5Align";
        private static readonly string ForestWitchClass5AlignGuid = "24c9254c-9b4c-4900-ba9f-ff6676a0daa1";
        private const string ForestWitchClass6Align = "ForestWitchClass6Align";
        private static readonly string ForestWitchClass6AlignGuid = "4dad84db-1a9b-4ddd-8608-5954cf22760f";
        private const string ForestWitchClass7Align = "ForestWitchClass7Align";
        private static readonly string ForestWitchClass7AlignGuid = "9165227e-5baf-425a-931c-c684e557e6ec";
        private const string ForestWitchClass8Align = "ForestWitchClass8Align";
        private static readonly string ForestWitchClass8AlignGuid = "88d20bfd-7500-48fe-a57d-c7892176e28f";
        private const string ForestWitchClass9Align = "ForestWitchClass9Align";
        private static readonly string ForestWitchClass9AlignGuid = "8d7611f5-4fcd-40e4-9814-eab4c6ea5235";
        private const string ForestWitchClass10Align = "ForestWitchClass10Align";
        private static readonly string ForestWitchClass10AlignGuid = "08e52d41-8823-4791-acd9-aa58263cc4c0";
        private const string ForestWizardClass0Align = "ForestWizardClass0Align";
        private static readonly string ForestWizardClass0AlignGuid = "ef9b2779-7c8f-4aa9-bd80-5c35a9fe2b57";
        internal const string ForestWizardClass0AlignDisplayName = "ForestWizardClass0Align.Name";
        private const string ForestWizardClass0AlignDescription = "ForestWizardClass0Align.Description";
        private const string ForestWizardClass2Align = "ForestWizardClass2Align";
        private static readonly string ForestWizardClass2AlignGuid = "a3775a86-5409-47d5-817e-558df474f8cc";
        private const string ForestWizardClass3Align = "ForestWizardClass3Align";
        private static readonly string ForestWizardClass3AlignGuid = "eea83e85-963d-4348-a82a-be243e94a5c5";
        private const string ForestWizardClass4Align = "ForestWizardClass4Align";
        private static readonly string ForestWizardClass4AlignGuid = "eecbbbd5-ab9e-49e7-918a-1f9c0ab4eda7";
        private const string ForestWizardClass5Align = "ForestWizardClass5Align";
        private static readonly string ForestWizardClass5AlignGuid = "553a8735-600d-4afd-bd6e-40fa98842301";
        private const string ForestWizardClass6Align = "ForestWizardClass6Align";
        private static readonly string ForestWizardClass6AlignGuid = "9b98023a-1a46-4e88-b338-26f8382f0bec";
        private const string ForestWizardClass7Align = "ForestWizardClass7Align";
        private static readonly string ForestWizardClass7AlignGuid = "70c3b4fa-2c56-4a33-9195-2a9610bf991d";
        private const string ForestWizardClass8Align = "ForestWizardClass8Align";
        private static readonly string ForestWizardClass8AlignGuid = "61218d09-c447-42b0-be4f-0dd428ad9698";
        private const string ForestWizardClass9Align = "ForestWizardClass9Align";
        private static readonly string ForestWizardClass9AlignGuid = "84448e04-f911-45f8-8cd0-bb97621ae23e";
        private const string ForestWizardClass10Align = "ForestWizardClass10Align";
        private static readonly string ForestWizardClass10AlignGuid = "df54a4ce-0180-428a-a623-df7ecc4e9b84";
        private const string ForestGunslingerClass0Align = "ForestGunslingerClass0Align";
        private static readonly string ForestGunslingerClass0AlignGuid = "5034f2c8-9d33-47b8-bc76-8a7f6212e474";
        internal const string ForestGunslingerClass0AlignDisplayName = "ForestGunslingerClass0Align.Name";
        private const string ForestGunslingerClass0AlignDescription = "ForestGunslingerClass0Align.Description";
        private const string ForestGunslingerClass2Align = "ForestGunslingerClass2Align";
        private static readonly string ForestGunslingerClass2AlignGuid = "09e445ce-faa9-44f2-805c-b6e3988ddaeb";
        private const string ForestGunslingerClass3Align = "ForestGunslingerClass3Align";
        private static readonly string ForestGunslingerClass3AlignGuid = "c5ff4933-01ca-45a7-8653-45f7c869da5d";
        private const string ForestGunslingerClass4Align = "ForestGunslingerClass4Align";
        private static readonly string ForestGunslingerClass4AlignGuid = "838f45db-d9cc-4f00-9ef3-f2f18f500d35";
        private const string ForestGunslingerClass5Align = "ForestGunslingerClass5Align";
        private static readonly string ForestGunslingerClass5AlignGuid = "7a158648-e101-4868-92bc-ab6c8258a733";
        private const string ForestGunslingerClass6Align = "ForestGunslingerClass6Align";
        private static readonly string ForestGunslingerClass6AlignGuid = "2fd588a0-5a5d-4174-a050-d576de824522";
        private const string ForestGunslingerClass7Align = "ForestGunslingerClass7Align";
        private static readonly string ForestGunslingerClass7AlignGuid = "c7843824-6411-4b3f-9395-54acf1c2507f";
        private const string ForestGunslingerClass8Align = "ForestGunslingerClass8Align";
        private static readonly string ForestGunslingerClass8AlignGuid = "7a852624-8922-42e3-83b6-e5744ceefa37";
        private const string ForestGunslingerClass9Align = "ForestGunslingerClass9Align";
        private static readonly string ForestGunslingerClass9AlignGuid = "b7a19a0f-10cc-4d94-8490-59659d5cc16c";
        private const string ForestGunslingerClass10Align = "ForestGunslingerClass10Align";
        private static readonly string ForestGunslingerClass10AlignGuid = "d6665900-0ac5-4ff4-a68f-a5d7f9bd5a16";
        private const string ForestAgentoftheGraveClass0Align = "ForestAgentoftheGraveClass0Align";
        private static readonly string ForestAgentoftheGraveClass0AlignGuid = "4907cd4f-7516-4d03-952d-9f5c54111d31";
        internal const string ForestAgentoftheGraveClass0AlignDisplayName = "ForestAgentoftheGraveClass0Align.Name";
        private const string ForestAgentoftheGraveClass0AlignDescription = "ForestAgentoftheGraveClass0Align.Description";
        private const string ForestAgentoftheGraveClass2Align = "ForestAgentoftheGraveClass2Align";
        private static readonly string ForestAgentoftheGraveClass2AlignGuid = "e19bf7e7-4c5b-4dca-bab1-98eb1dc14c8c";
        private const string ForestAgentoftheGraveClass3Align = "ForestAgentoftheGraveClass3Align";
        private static readonly string ForestAgentoftheGraveClass3AlignGuid = "78abe020-262b-4a3e-af2e-fee7c42cb1d0";
        private const string ForestAgentoftheGraveClass4Align = "ForestAgentoftheGraveClass4Align";
        private static readonly string ForestAgentoftheGraveClass4AlignGuid = "6f2f68f1-3ba4-46da-9ffe-55142e0ccf60";
        private const string ForestAgentoftheGraveClass5Align = "ForestAgentoftheGraveClass5Align";
        private static readonly string ForestAgentoftheGraveClass5AlignGuid = "9dd3accb-cd21-4e75-b011-5639b4435eec";
        private const string ForestAgentoftheGraveClass6Align = "ForestAgentoftheGraveClass6Align";
        private static readonly string ForestAgentoftheGraveClass6AlignGuid = "8441a48e-47c4-47f4-80b4-9de9357464b2";
        private const string ForestAgentoftheGraveClass7Align = "ForestAgentoftheGraveClass7Align";
        private static readonly string ForestAgentoftheGraveClass7AlignGuid = "2c50c45e-f821-4d5d-9eb0-d5be34bca90b";
        private const string ForestAgentoftheGraveClass8Align = "ForestAgentoftheGraveClass8Align";
        private static readonly string ForestAgentoftheGraveClass8AlignGuid = "233b6128-5e6b-455a-bb0b-680477004f09";
        private const string ForestAgentoftheGraveClass9Align = "ForestAgentoftheGraveClass9Align";
        private static readonly string ForestAgentoftheGraveClass9AlignGuid = "b211ab27-a3dc-41bb-b88d-48171b7de91d";
        private const string ForestAgentoftheGraveClass10Align = "ForestAgentoftheGraveClass10Align";
        private static readonly string ForestAgentoftheGraveClass10AlignGuid = "aca061b5-b1df-439b-b4e7-78a2202f8127";
        private const string ForestAnchoriteofDawnClass0Align = "ForestAnchoriteofDawnClass0Align";
        private static readonly string ForestAnchoriteofDawnClass0AlignGuid = "0e3c2da0-0abb-41af-af15-992b7779c386";
        internal const string ForestAnchoriteofDawnClass0AlignDisplayName = "ForestAnchoriteofDawnClass0Align.Name";
        private const string ForestAnchoriteofDawnClass0AlignDescription = "ForestAnchoriteofDawnClass0Align.Description";
        private const string ForestAnchoriteofDawnClass2Align = "ForestAnchoriteofDawnClass2Align";
        private static readonly string ForestAnchoriteofDawnClass2AlignGuid = "eb2d152e-5096-4990-9a77-14220309b745";
        private const string ForestAnchoriteofDawnClass3Align = "ForestAnchoriteofDawnClass3Align";
        private static readonly string ForestAnchoriteofDawnClass3AlignGuid = "dccf6509-e539-46a0-9a8c-6a3595d53865";
        private const string ForestAnchoriteofDawnClass4Align = "ForestAnchoriteofDawnClass4Align";
        private static readonly string ForestAnchoriteofDawnClass4AlignGuid = "c54ecc35-f849-46e6-8f98-7c8b199d510b";
        private const string ForestAnchoriteofDawnClass5Align = "ForestAnchoriteofDawnClass5Align";
        private static readonly string ForestAnchoriteofDawnClass5AlignGuid = "17922e2c-b691-4b93-9d8f-49d5a5875837";
        private const string ForestAnchoriteofDawnClass6Align = "ForestAnchoriteofDawnClass6Align";
        private static readonly string ForestAnchoriteofDawnClass6AlignGuid = "2b3940b8-5f34-4ce2-a6e9-659a5da7723f";
        private const string ForestAnchoriteofDawnClass7Align = "ForestAnchoriteofDawnClass7Align";
        private static readonly string ForestAnchoriteofDawnClass7AlignGuid = "9c5d5efd-5ad5-4d57-97a1-47add4875a06";
        private const string ForestAnchoriteofDawnClass8Align = "ForestAnchoriteofDawnClass8Align";
        private static readonly string ForestAnchoriteofDawnClass8AlignGuid = "bc59de02-48e7-4846-a39a-aaf2ff693eb1";
        private const string ForestAnchoriteofDawnClass9Align = "ForestAnchoriteofDawnClass9Align";
        private static readonly string ForestAnchoriteofDawnClass9AlignGuid = "7b73af1b-cde4-496f-919b-eb3ee4e93f63";
        private const string ForestAnchoriteofDawnClass10Align = "ForestAnchoriteofDawnClass10Align";
        private static readonly string ForestAnchoriteofDawnClass10AlignGuid = "b8567311-1c9a-4d50-9794-2e15baf88cf0";
        private const string ForestArcaneAcherClass0Align = "ForestArcaneAcherClass0Align";
        private static readonly string ForestArcaneAcherClass0AlignGuid = "d740d8b7-96ac-4f7e-8ea6-aa300d86e20b";
        internal const string ForestArcaneAcherClass0AlignDisplayName = "ForestArcaneAcherClass0Align.Name";
        private const string ForestArcaneAcherClass0AlignDescription = "ForestArcaneAcherClass0Align.Description";
        private const string ForestArcaneAcherClass2Align = "ForestArcaneAcherClass2Align";
        private static readonly string ForestArcaneAcherClass2AlignGuid = "b713858f-4a3f-4b52-a967-6cad68f74121";
        private const string ForestArcaneAcherClass3Align = "ForestArcaneAcherClass3Align";
        private static readonly string ForestArcaneAcherClass3AlignGuid = "b706b23a-e233-4380-9140-1307cd3e1dfb";
        private const string ForestArcaneAcherClass4Align = "ForestArcaneAcherClass4Align";
        private static readonly string ForestArcaneAcherClass4AlignGuid = "4643a785-36ea-40bc-844b-748631418d19";
        private const string ForestArcaneAcherClass5Align = "ForestArcaneAcherClass5Align";
        private static readonly string ForestArcaneAcherClass5AlignGuid = "c968f14f-99cb-43ae-a21d-6c4d3c24c583";
        private const string ForestArcaneAcherClass6Align = "ForestArcaneAcherClass6Align";
        private static readonly string ForestArcaneAcherClass6AlignGuid = "3e65d0ff-ddde-4dd4-b3bf-a3d212e15ced";
        private const string ForestArcaneAcherClass7Align = "ForestArcaneAcherClass7Align";
        private static readonly string ForestArcaneAcherClass7AlignGuid = "a18a7f7c-2a93-4787-93ab-cced92e81367";
        private const string ForestArcaneAcherClass8Align = "ForestArcaneAcherClass8Align";
        private static readonly string ForestArcaneAcherClass8AlignGuid = "cb6313d8-0e19-4ed1-b067-65e4f83c1e99";
        private const string ForestArcaneAcherClass9Align = "ForestArcaneAcherClass9Align";
        private static readonly string ForestArcaneAcherClass9AlignGuid = "329dbe10-0b66-4451-af49-f151fc7f836f";
        private const string ForestArcaneAcherClass10Align = "ForestArcaneAcherClass10Align";
        private static readonly string ForestArcaneAcherClass10AlignGuid = "32e3701d-3795-434e-b431-c0f48dff18a1";
        private const string ForestAsavirClass0Align = "ForestAsavirClass0Align";
        private static readonly string ForestAsavirClass0AlignGuid = "8acc5d11-3f77-4741-9eb3-74509a8c68c3";
        internal const string ForestAsavirClass0AlignDisplayName = "ForestAsavirClass0Align.Name";
        private const string ForestAsavirClass0AlignDescription = "ForestAsavirClass0Align.Description";
        private const string ForestAsavirClass2Align = "ForestAsavirClass2Align";
        private static readonly string ForestAsavirClass2AlignGuid = "1ca83204-aad0-4971-9775-0268fe87dc75";
        private const string ForestAsavirClass3Align = "ForestAsavirClass3Align";
        private static readonly string ForestAsavirClass3AlignGuid = "76488b9d-79fd-47c5-9d32-34ce133f56aa";
        private const string ForestAsavirClass4Align = "ForestAsavirClass4Align";
        private static readonly string ForestAsavirClass4AlignGuid = "5fb1f4ab-9f1e-41eb-8767-29654f4c6468";
        private const string ForestAsavirClass5Align = "ForestAsavirClass5Align";
        private static readonly string ForestAsavirClass5AlignGuid = "d1fb01b3-cb21-4d5a-928b-b4c93a06e113";
        private const string ForestAsavirClass6Align = "ForestAsavirClass6Align";
        private static readonly string ForestAsavirClass6AlignGuid = "b1ff4a2d-6060-4b28-89c9-6065d2f0d5e5";
        private const string ForestAsavirClass7Align = "ForestAsavirClass7Align";
        private static readonly string ForestAsavirClass7AlignGuid = "bde71a17-7dda-4424-9686-c0ca55f8f6ea";
        private const string ForestAsavirClass8Align = "ForestAsavirClass8Align";
        private static readonly string ForestAsavirClass8AlignGuid = "5820b464-1aea-41cf-9a3a-01d75c8f1b19";
        private const string ForestAsavirClass9Align = "ForestAsavirClass9Align";
        private static readonly string ForestAsavirClass9AlignGuid = "0f1bd555-d05b-49b1-9a77-307c88288d40";
        private const string ForestAsavirClass10Align = "ForestAsavirClass10Align";
        private static readonly string ForestAsavirClass10AlignGuid = "b895dce3-323f-457d-8660-36996c4d9e0f";
        private const string ForestChevalierClass0Align = "ForestChevalierClass0Align";
        private static readonly string ForestChevalierClass0AlignGuid = "38c73f86-8e3e-4973-9f61-a4e97dcdf4af";
        internal const string ForestChevalierClass0AlignDisplayName = "ForestChevalierClass0Align.Name";
        private const string ForestChevalierClass0AlignDescription = "ForestChevalierClass0Align.Description";
        private const string ForestChevalierClass2Align = "ForestChevalierClass2Align";
        private static readonly string ForestChevalierClass2AlignGuid = "a4ef7a98-f49b-4bf0-80d4-ac1b9959e319";
        private const string ForestChevalierClass3Align = "ForestChevalierClass3Align";
        private static readonly string ForestChevalierClass3AlignGuid = "99c84838-02b6-46bf-b84c-f5994f406cac";
        private const string ForestChevalierClass4Align = "ForestChevalierClass4Align";
        private static readonly string ForestChevalierClass4AlignGuid = "9488f31c-c062-4aba-9ba1-5ec01628de8c";
        private const string ForestChevalierClass5Align = "ForestChevalierClass5Align";
        private static readonly string ForestChevalierClass5AlignGuid = "bcc58d35-4722-4e89-935d-5a01f5113fdc";
        private const string ForestChevalierClass6Align = "ForestChevalierClass6Align";
        private static readonly string ForestChevalierClass6AlignGuid = "b3f2e189-fb80-4301-89ea-ac9e03eb29c3";
        private const string ForestChevalierClass7Align = "ForestChevalierClass7Align";
        private static readonly string ForestChevalierClass7AlignGuid = "c50fb1d2-2009-42f1-8416-daf2e2a63325";
        private const string ForestChevalierClass8Align = "ForestChevalierClass8Align";
        private static readonly string ForestChevalierClass8AlignGuid = "7cb6343b-4fb0-480e-98fb-39222de0f6ba";
        private const string ForestChevalierClass9Align = "ForestChevalierClass9Align";
        private static readonly string ForestChevalierClass9AlignGuid = "18c7d617-51cf-447b-bc53-7941cfdfe9a1";
        private const string ForestChevalierClass10Align = "ForestChevalierClass10Align";
        private static readonly string ForestChevalierClass10AlignGuid = "216cd0ea-8de6-4b74-8e3e-37c181d0e166";
        private const string ForestCrimsonTemplarClass0Align = "ForestCrimsonTemplarClass0Align";
        private static readonly string ForestCrimsonTemplarClass0AlignGuid = "f97efb58-77c5-43ff-8fda-6d3360b4ff0b";
        internal const string ForestCrimsonTemplarClass0AlignDisplayName = "ForestCrimsonTemplarClass0Align.Name";
        private const string ForestCrimsonTemplarClass0AlignDescription = "ForestCrimsonTemplarClass0Align.Description";
        private const string ForestCrimsonTemplarClass2Align = "ForestCrimsonTemplarClass2Align";
        private static readonly string ForestCrimsonTemplarClass2AlignGuid = "05009016-997b-44da-a9b6-4690bd2ebc35";
        private const string ForestCrimsonTemplarClass3Align = "ForestCrimsonTemplarClass3Align";
        private static readonly string ForestCrimsonTemplarClass3AlignGuid = "a2c4abde-21ff-4e5b-a80a-cbe57baee06a";
        private const string ForestCrimsonTemplarClass4Align = "ForestCrimsonTemplarClass4Align";
        private static readonly string ForestCrimsonTemplarClass4AlignGuid = "d3b0877d-69e3-4503-886a-3dfc4b65ae3b";
        private const string ForestCrimsonTemplarClass5Align = "ForestCrimsonTemplarClass5Align";
        private static readonly string ForestCrimsonTemplarClass5AlignGuid = "a670a260-f1e7-431a-88a9-4e7d9f53666a";
        private const string ForestCrimsonTemplarClass6Align = "ForestCrimsonTemplarClass6Align";
        private static readonly string ForestCrimsonTemplarClass6AlignGuid = "41807469-3f3b-4984-929a-d18379f2494d";
        private const string ForestCrimsonTemplarClass7Align = "ForestCrimsonTemplarClass7Align";
        private static readonly string ForestCrimsonTemplarClass7AlignGuid = "c6367aa7-4e4e-44c3-ad56-ca46bc500a08";
        private const string ForestCrimsonTemplarClass8Align = "ForestCrimsonTemplarClass8Align";
        private static readonly string ForestCrimsonTemplarClass8AlignGuid = "9d48aac2-af7a-4dfa-b25d-bfb96770851a";
        private const string ForestCrimsonTemplarClass9Align = "ForestCrimsonTemplarClass9Align";
        private static readonly string ForestCrimsonTemplarClass9AlignGuid = "98db8888-983e-49b2-b6d6-d88c9b5b4f43";
        private const string ForestCrimsonTemplarClass10Align = "ForestCrimsonTemplarClass10Align";
        private static readonly string ForestCrimsonTemplarClass10AlignGuid = "4184c40c-fea5-412f-8462-79dbba67a46b";
        private const string ForestDeadeyeDevoteeClass0Align = "ForestDeadeyeDevoteeClass0Align";
        private static readonly string ForestDeadeyeDevoteeClass0AlignGuid = "4a8949cb-305e-4b22-a6e3-554a12b194cb";
        internal const string ForestDeadeyeDevoteeClass0AlignDisplayName = "ForestDeadeyeDevoteeClass0Align.Name";
        private const string ForestDeadeyeDevoteeClass0AlignDescription = "ForestDeadeyeDevoteeClass0Align.Description";
        private const string ForestDeadeyeDevoteeClass2Align = "ForestDeadeyeDevoteeClass2Align";
        private static readonly string ForestDeadeyeDevoteeClass2AlignGuid = "7109204a-b704-4794-8207-dceb698be849";
        private const string ForestDeadeyeDevoteeClass3Align = "ForestDeadeyeDevoteeClass3Align";
        private static readonly string ForestDeadeyeDevoteeClass3AlignGuid = "bae9bb7f-e9f6-46c9-8779-58981969225e";
        private const string ForestDeadeyeDevoteeClass4Align = "ForestDeadeyeDevoteeClass4Align";
        private static readonly string ForestDeadeyeDevoteeClass4AlignGuid = "1bb2290f-bfc0-4817-96de-90b6ef8856f9";
        private const string ForestDeadeyeDevoteeClass5Align = "ForestDeadeyeDevoteeClass5Align";
        private static readonly string ForestDeadeyeDevoteeClass5AlignGuid = "6f6871fd-3ac9-4041-9239-38a1393e6d42";
        private const string ForestDeadeyeDevoteeClass6Align = "ForestDeadeyeDevoteeClass6Align";
        private static readonly string ForestDeadeyeDevoteeClass6AlignGuid = "9351f964-d22b-4b84-9872-23b6405c13c1";
        private const string ForestDeadeyeDevoteeClass7Align = "ForestDeadeyeDevoteeClass7Align";
        private static readonly string ForestDeadeyeDevoteeClass7AlignGuid = "60bef236-308d-42a5-be8f-f72c85aaf9c5";
        private const string ForestDeadeyeDevoteeClass8Align = "ForestDeadeyeDevoteeClass8Align";
        private static readonly string ForestDeadeyeDevoteeClass8AlignGuid = "c6bc8cc0-18f9-4f31-9d17-1bbbc6a1aad7";
        private const string ForestDeadeyeDevoteeClass9Align = "ForestDeadeyeDevoteeClass9Align";
        private static readonly string ForestDeadeyeDevoteeClass9AlignGuid = "1638eb18-29d1-4aa2-b61c-99a659d6db8b";
        private const string ForestDeadeyeDevoteeClass10Align = "ForestDeadeyeDevoteeClass10Align";
        private static readonly string ForestDeadeyeDevoteeClass10AlignGuid = "ee69115f-fb6b-434b-b84b-17dbe08a6894";
        private const string ForestDragonFuryClass0Align = "ForestDragonFuryClass0Align";
        private static readonly string ForestDragonFuryClass0AlignGuid = "b9ff4c8a-825e-4216-8a37-fd92fbb72fc3";
        internal const string ForestDragonFuryClass0AlignDisplayName = "ForestDragonFuryClass0Align.Name";
        private const string ForestDragonFuryClass0AlignDescription = "ForestDragonFuryClass0Align.Description";
        private const string ForestDragonFuryClass2Align = "ForestDragonFuryClass2Align";
        private static readonly string ForestDragonFuryClass2AlignGuid = "ae9e3f81-e60b-484b-a9f0-156fdf6d169d";
        private const string ForestDragonFuryClass3Align = "ForestDragonFuryClass3Align";
        private static readonly string ForestDragonFuryClass3AlignGuid = "4b64e41d-241d-4852-9a77-f813d6973a6a";
        private const string ForestDragonFuryClass4Align = "ForestDragonFuryClass4Align";
        private static readonly string ForestDragonFuryClass4AlignGuid = "2b40738c-f9e3-4490-86fe-3284aec71740";
        private const string ForestDragonFuryClass5Align = "ForestDragonFuryClass5Align";
        private static readonly string ForestDragonFuryClass5AlignGuid = "aa329259-ea63-43fa-9a95-cf47f89f6df6";
        private const string ForestDragonFuryClass6Align = "ForestDragonFuryClass6Align";
        private static readonly string ForestDragonFuryClass6AlignGuid = "b0bd8e3d-c84b-48d6-8193-2e8154557f99";
        private const string ForestDragonFuryClass7Align = "ForestDragonFuryClass7Align";
        private static readonly string ForestDragonFuryClass7AlignGuid = "fc42b238-7240-4f84-8926-77d60af835e1";
        private const string ForestDragonFuryClass8Align = "ForestDragonFuryClass8Align";
        private static readonly string ForestDragonFuryClass8AlignGuid = "299b9f3e-a4b3-4f0b-874d-aa9b37d81881";
        private const string ForestDragonFuryClass9Align = "ForestDragonFuryClass9Align";
        private static readonly string ForestDragonFuryClass9AlignGuid = "edbd73f5-12b0-40fe-93af-dde309199a46";
        private const string ForestDragonFuryClass10Align = "ForestDragonFuryClass10Align";
        private static readonly string ForestDragonFuryClass10AlignGuid = "3a1f9152-44e7-476d-8094-4d6c98a4e3c0";
        private const string ForestEsotericKnightClass0Align = "ForestEsotericKnightClass0Align";
        private static readonly string ForestEsotericKnightClass0AlignGuid = "2b77bd2a-a276-4477-9380-7c63301b60a6";
        internal const string ForestEsotericKnightClass0AlignDisplayName = "ForestEsotericKnightClass0Align.Name";
        private const string ForestEsotericKnightClass0AlignDescription = "ForestEsotericKnightClass0Align.Description";
        private const string ForestEsotericKnightClass2Align = "ForestEsotericKnightClass2Align";
        private static readonly string ForestEsotericKnightClass2AlignGuid = "2f3964be-2628-41a9-9a3b-1ccfbf0b2a1f";
        private const string ForestEsotericKnightClass3Align = "ForestEsotericKnightClass3Align";
        private static readonly string ForestEsotericKnightClass3AlignGuid = "33beae91-591f-402a-bc64-7c1d28eab5df";
        private const string ForestEsotericKnightClass4Align = "ForestEsotericKnightClass4Align";
        private static readonly string ForestEsotericKnightClass4AlignGuid = "55757978-3db0-477c-9a49-de51e904bf97";
        private const string ForestEsotericKnightClass5Align = "ForestEsotericKnightClass5Align";
        private static readonly string ForestEsotericKnightClass5AlignGuid = "1fad7d2f-272d-4a67-ac3c-ce9777d0013f";
        private const string ForestEsotericKnightClass6Align = "ForestEsotericKnightClass6Align";
        private static readonly string ForestEsotericKnightClass6AlignGuid = "dced7f23-e11f-40bc-bd8e-cadf301b0e4b";
        private const string ForestEsotericKnightClass7Align = "ForestEsotericKnightClass7Align";
        private static readonly string ForestEsotericKnightClass7AlignGuid = "7b109adb-3504-4c3a-b86a-40236f04f861";
        private const string ForestEsotericKnightClass8Align = "ForestEsotericKnightClass8Align";
        private static readonly string ForestEsotericKnightClass8AlignGuid = "b534c060-9bb9-4ef7-bcc0-653fcb7e49c6";
        private const string ForestEsotericKnightClass9Align = "ForestEsotericKnightClass9Align";
        private static readonly string ForestEsotericKnightClass9AlignGuid = "6b8f72fd-a5d0-4e6c-950c-f32ba639e870";
        private const string ForestEsotericKnightClass10Align = "ForestEsotericKnightClass10Align";
        private static readonly string ForestEsotericKnightClass10AlignGuid = "7c07f24e-8a93-4058-843d-cd096a12e606";
        private const string ForestExaltedEvangelistClass0Align = "ForestExaltedEvangelistClass0Align";
        private static readonly string ForestExaltedEvangelistClass0AlignGuid = "2fd566d6-2677-4ea9-9e88-2cd0df9200c8";
        internal const string ForestExaltedEvangelistClass0AlignDisplayName = "ForestExaltedEvangelistClass0Align.Name";
        private const string ForestExaltedEvangelistClass0AlignDescription = "ForestExaltedEvangelistClass0Align.Description";
        private const string ForestExaltedEvangelistClass2Align = "ForestExaltedEvangelistClass2Align";
        private static readonly string ForestExaltedEvangelistClass2AlignGuid = "488d51fd-e9b0-408b-9246-b2b77dce4377";
        private const string ForestExaltedEvangelistClass3Align = "ForestExaltedEvangelistClass3Align";
        private static readonly string ForestExaltedEvangelistClass3AlignGuid = "c9ed4ce9-4322-4b85-8ba0-336746211394";
        private const string ForestExaltedEvangelistClass4Align = "ForestExaltedEvangelistClass4Align";
        private static readonly string ForestExaltedEvangelistClass4AlignGuid = "7db65364-8961-46cc-97eb-fe9943eb5d52";
        private const string ForestExaltedEvangelistClass5Align = "ForestExaltedEvangelistClass5Align";
        private static readonly string ForestExaltedEvangelistClass5AlignGuid = "5eb8b16e-acec-4134-b4b9-e3539e51d4c8";
        private const string ForestExaltedEvangelistClass6Align = "ForestExaltedEvangelistClass6Align";
        private static readonly string ForestExaltedEvangelistClass6AlignGuid = "46a8f260-8914-4808-a102-9b85f8a66bb0";
        private const string ForestExaltedEvangelistClass7Align = "ForestExaltedEvangelistClass7Align";
        private static readonly string ForestExaltedEvangelistClass7AlignGuid = "bed863dd-38f6-4eb9-b5fc-ae2662d1d551";
        private const string ForestExaltedEvangelistClass8Align = "ForestExaltedEvangelistClass8Align";
        private static readonly string ForestExaltedEvangelistClass8AlignGuid = "f9482260-0ad9-4bee-a890-4fbea522e96f";
        private const string ForestExaltedEvangelistClass9Align = "ForestExaltedEvangelistClass9Align";
        private static readonly string ForestExaltedEvangelistClass9AlignGuid = "cdb4949d-6fd4-4993-81d8-0e54cb566ebc";
        private const string ForestExaltedEvangelistClass10Align = "ForestExaltedEvangelistClass10Align";
        private static readonly string ForestExaltedEvangelistClass10AlignGuid = "71758cf4-0246-42fc-b7d1-760899de1b98";
        private const string ForestFuriousGuardianClass0Align = "ForestFuriousGuardianClass0Align";
        private static readonly string ForestFuriousGuardianClass0AlignGuid = "76e65621-8954-4f4a-9fe2-f03c9cb45061";
        internal const string ForestFuriousGuardianClass0AlignDisplayName = "ForestFuriousGuardianClass0Align.Name";
        private const string ForestFuriousGuardianClass0AlignDescription = "ForestFuriousGuardianClass0Align.Description";
        private const string ForestFuriousGuardianClass2Align = "ForestFuriousGuardianClass2Align";
        private static readonly string ForestFuriousGuardianClass2AlignGuid = "fef79b03-ee1b-4a15-9998-2139842075ec";
        private const string ForestFuriousGuardianClass3Align = "ForestFuriousGuardianClass3Align";
        private static readonly string ForestFuriousGuardianClass3AlignGuid = "bacc7580-fe65-45c3-944e-6ad75f1bbed2";
        private const string ForestFuriousGuardianClass4Align = "ForestFuriousGuardianClass4Align";
        private static readonly string ForestFuriousGuardianClass4AlignGuid = "95e2427e-dfa4-4c7a-9a56-0cad2aeadf50";
        private const string ForestFuriousGuardianClass5Align = "ForestFuriousGuardianClass5Align";
        private static readonly string ForestFuriousGuardianClass5AlignGuid = "d687dec1-fb68-4553-9fc5-b42f9d961b26";
        private const string ForestFuriousGuardianClass6Align = "ForestFuriousGuardianClass6Align";
        private static readonly string ForestFuriousGuardianClass6AlignGuid = "ef2a6c67-484d-4bc4-ac0e-309b91108551";
        private const string ForestFuriousGuardianClass7Align = "ForestFuriousGuardianClass7Align";
        private static readonly string ForestFuriousGuardianClass7AlignGuid = "052d57aa-4601-4755-9886-9da0df5e37a0";
        private const string ForestFuriousGuardianClass8Align = "ForestFuriousGuardianClass8Align";
        private static readonly string ForestFuriousGuardianClass8AlignGuid = "6e7e936d-8782-4305-b019-bf664584a25f";
        private const string ForestFuriousGuardianClass9Align = "ForestFuriousGuardianClass9Align";
        private static readonly string ForestFuriousGuardianClass9AlignGuid = "f1e41900-7a05-44bf-9941-e36cdfd502f5";
        private const string ForestFuriousGuardianClass10Align = "ForestFuriousGuardianClass10Align";
        private static readonly string ForestFuriousGuardianClass10AlignGuid = "76f220f0-65fe-40f4-9a36-768e5dd3c974";
        private const string ForestHalflingOpportunistClass0Align = "ForestHalflingOpportunistClass0Align";
        private static readonly string ForestHalflingOpportunistClass0AlignGuid = "0201aaf4-f3da-4550-a6d9-9938ccc5400a";
        internal const string ForestHalflingOpportunistClass0AlignDisplayName = "ForestHalflingOpportunistClass0Align.Name";
        private const string ForestHalflingOpportunistClass0AlignDescription = "ForestHalflingOpportunistClass0Align.Description";
        private const string ForestHalflingOpportunistClass2Align = "ForestHalflingOpportunistClass2Align";
        private static readonly string ForestHalflingOpportunistClass2AlignGuid = "2d4525b2-92d8-40e3-8e88-2552a22768f0";
        private const string ForestHalflingOpportunistClass3Align = "ForestHalflingOpportunistClass3Align";
        private static readonly string ForestHalflingOpportunistClass3AlignGuid = "9310a338-b8eb-4ea1-bdd1-61d49e6926b0";
        private const string ForestHalflingOpportunistClass4Align = "ForestHalflingOpportunistClass4Align";
        private static readonly string ForestHalflingOpportunistClass4AlignGuid = "fc7e3fc1-8279-44d7-9ed6-e54426a2a8a5";
        private const string ForestHalflingOpportunistClass5Align = "ForestHalflingOpportunistClass5Align";
        private static readonly string ForestHalflingOpportunistClass5AlignGuid = "e4a6588e-1d53-439d-a062-4ddf03b00d10";
        private const string ForestHalflingOpportunistClass6Align = "ForestHalflingOpportunistClass6Align";
        private static readonly string ForestHalflingOpportunistClass6AlignGuid = "a2b1925e-c0ca-4ba8-b453-4cc9f9bf2d1b";
        private const string ForestHalflingOpportunistClass7Align = "ForestHalflingOpportunistClass7Align";
        private static readonly string ForestHalflingOpportunistClass7AlignGuid = "0e97b9c4-2ae6-4d70-b3cf-ff5e8ccedae4";
        private const string ForestHalflingOpportunistClass8Align = "ForestHalflingOpportunistClass8Align";
        private static readonly string ForestHalflingOpportunistClass8AlignGuid = "1828b1ac-c4fd-49ec-b53a-95b99af4d91d";
        private const string ForestHalflingOpportunistClass9Align = "ForestHalflingOpportunistClass9Align";
        private static readonly string ForestHalflingOpportunistClass9AlignGuid = "106d3ead-4849-4220-b480-19b004e599ad";
        private const string ForestHalflingOpportunistClass10Align = "ForestHalflingOpportunistClass10Align";
        private static readonly string ForestHalflingOpportunistClass10AlignGuid = "cd83c2c3-d6b9-4752-9b12-e4d392f6feb6";
        private const string ForestHinterlanderClass0Align = "ForestHinterlanderClass0Align";
        private static readonly string ForestHinterlanderClass0AlignGuid = "e2aaae0f-de0b-4f63-a0b7-a320523fd5c8";
        internal const string ForestHinterlanderClass0AlignDisplayName = "ForestHinterlanderClass0Align.Name";
        private const string ForestHinterlanderClass0AlignDescription = "ForestHinterlanderClass0Align.Description";
        private const string ForestHinterlanderClass2Align = "ForestHinterlanderClass2Align";
        private static readonly string ForestHinterlanderClass2AlignGuid = "e2b59f0e-6c1d-42fc-a84f-db8d6498f84d";
        private const string ForestHinterlanderClass3Align = "ForestHinterlanderClass3Align";
        private static readonly string ForestHinterlanderClass3AlignGuid = "a1d3aa7e-8c7c-4600-b20c-5f0a739bfe24";
        private const string ForestHinterlanderClass4Align = "ForestHinterlanderClass4Align";
        private static readonly string ForestHinterlanderClass4AlignGuid = "34d2acd8-3117-401a-8fac-d43754b182e5";
        private const string ForestHinterlanderClass5Align = "ForestHinterlanderClass5Align";
        private static readonly string ForestHinterlanderClass5AlignGuid = "bff9becb-4457-47c1-bd73-a7f53a577599";
        private const string ForestHinterlanderClass6Align = "ForestHinterlanderClass6Align";
        private static readonly string ForestHinterlanderClass6AlignGuid = "a9a778ed-2b2f-4eba-b5b1-47746262ee86";
        private const string ForestHinterlanderClass7Align = "ForestHinterlanderClass7Align";
        private static readonly string ForestHinterlanderClass7AlignGuid = "d2ae170b-5510-4537-81fa-5553756a5333";
        private const string ForestHinterlanderClass8Align = "ForestHinterlanderClass8Align";
        private static readonly string ForestHinterlanderClass8AlignGuid = "67bea1c5-904b-4c40-9b95-ad0dd43edd12";
        private const string ForestHinterlanderClass9Align = "ForestHinterlanderClass9Align";
        private static readonly string ForestHinterlanderClass9AlignGuid = "3513aae1-4982-491e-9436-7d0c54b8ac5b";
        private const string ForestHinterlanderClass10Align = "ForestHinterlanderClass10Align";
        private static readonly string ForestHinterlanderClass10AlignGuid = "0052e4c6-4951-4abe-b7e7-3f2c7719252a";
        private const string ForestHorizonWalkerClass0Align = "ForestHorizonWalkerClass0Align";
        private static readonly string ForestHorizonWalkerClass0AlignGuid = "b7ee5df8-b098-4ae0-9164-fb66c4462559";
        internal const string ForestHorizonWalkerClass0AlignDisplayName = "ForestHorizonWalkerClass0Align.Name";
        private const string ForestHorizonWalkerClass0AlignDescription = "ForestHorizonWalkerClass0Align.Description";
        private const string ForestHorizonWalkerClass2Align = "ForestHorizonWalkerClass2Align";
        private static readonly string ForestHorizonWalkerClass2AlignGuid = "77363bf1-8dc6-403f-a6f2-4842d6b15eb4";
        private const string ForestHorizonWalkerClass3Align = "ForestHorizonWalkerClass3Align";
        private static readonly string ForestHorizonWalkerClass3AlignGuid = "3d52af27-8313-45af-9183-ddb8bb946db7";
        private const string ForestHorizonWalkerClass4Align = "ForestHorizonWalkerClass4Align";
        private static readonly string ForestHorizonWalkerClass4AlignGuid = "1bbe8da5-7d9c-4a9d-8645-7b3f04c8b111";
        private const string ForestHorizonWalkerClass5Align = "ForestHorizonWalkerClass5Align";
        private static readonly string ForestHorizonWalkerClass5AlignGuid = "1adb0d5c-5d53-421e-a1d5-396db178fa11";
        private const string ForestHorizonWalkerClass6Align = "ForestHorizonWalkerClass6Align";
        private static readonly string ForestHorizonWalkerClass6AlignGuid = "8d0b7dc0-5bda-4c88-a694-180cf0f34a06";
        private const string ForestHorizonWalkerClass7Align = "ForestHorizonWalkerClass7Align";
        private static readonly string ForestHorizonWalkerClass7AlignGuid = "43900999-e797-47aa-b3c8-b3edb6bc8c40";
        private const string ForestHorizonWalkerClass8Align = "ForestHorizonWalkerClass8Align";
        private static readonly string ForestHorizonWalkerClass8AlignGuid = "7860cbb8-7874-4f39-976e-6f8e8a86db5b";
        private const string ForestHorizonWalkerClass9Align = "ForestHorizonWalkerClass9Align";
        private static readonly string ForestHorizonWalkerClass9AlignGuid = "b73d9653-243c-4123-adf4-0cf2c73ea76d";
        private const string ForestHorizonWalkerClass10Align = "ForestHorizonWalkerClass10Align";
        private static readonly string ForestHorizonWalkerClass10AlignGuid = "4e0b52b1-e7c2-4102-90f6-61fdf7d15ba9";
        private const string ForestInheritorCrusaderClass0Align = "ForestInheritorCrusaderClass0Align";
        private static readonly string ForestInheritorCrusaderClass0AlignGuid = "a16fbdfe-c6f9-46ff-9f81-462b1156a7b9";
        internal const string ForestInheritorCrusaderClass0AlignDisplayName = "ForestInheritorCrusaderClass0Align.Name";
        private const string ForestInheritorCrusaderClass0AlignDescription = "ForestInheritorCrusaderClass0Align.Description";
        private const string ForestInheritorCrusaderClass2Align = "ForestInheritorCrusaderClass2Align";
        private static readonly string ForestInheritorCrusaderClass2AlignGuid = "7aba7245-e2d4-4b33-a0a5-6dec40108808";
        private const string ForestInheritorCrusaderClass3Align = "ForestInheritorCrusaderClass3Align";
        private static readonly string ForestInheritorCrusaderClass3AlignGuid = "da014797-1b0f-4669-ad4c-fe9fb8faf253";
        private const string ForestInheritorCrusaderClass4Align = "ForestInheritorCrusaderClass4Align";
        private static readonly string ForestInheritorCrusaderClass4AlignGuid = "59426d92-045b-472d-ac5c-5cff8c21d6e5";
        private const string ForestInheritorCrusaderClass5Align = "ForestInheritorCrusaderClass5Align";
        private static readonly string ForestInheritorCrusaderClass5AlignGuid = "d9ba8c69-8679-4eb6-acc9-759e46d6c6b6";
        private const string ForestInheritorCrusaderClass6Align = "ForestInheritorCrusaderClass6Align";
        private static readonly string ForestInheritorCrusaderClass6AlignGuid = "07b65b9e-3817-4c85-9f09-19939338f5ed";
        private const string ForestInheritorCrusaderClass7Align = "ForestInheritorCrusaderClass7Align";
        private static readonly string ForestInheritorCrusaderClass7AlignGuid = "cb85734e-7897-4ba4-86b7-ca7d5837fff8";
        private const string ForestInheritorCrusaderClass8Align = "ForestInheritorCrusaderClass8Align";
        private static readonly string ForestInheritorCrusaderClass8AlignGuid = "0dcef072-f984-45c9-b891-e564cc5665d1";
        private const string ForestInheritorCrusaderClass9Align = "ForestInheritorCrusaderClass9Align";
        private static readonly string ForestInheritorCrusaderClass9AlignGuid = "638df52b-a626-444a-845b-0a857ba88e5c";
        private const string ForestInheritorCrusaderClass10Align = "ForestInheritorCrusaderClass10Align";
        private static readonly string ForestInheritorCrusaderClass10AlignGuid = "1a3de8b3-54a2-4bfc-a517-1f1068fa179f";
        private const string ForestMammothRiderClass0Align = "ForestMammothRiderClass0Align";
        private static readonly string ForestMammothRiderClass0AlignGuid = "15e44283-f0e6-4050-8ea4-274ca90f81f3";
        internal const string ForestMammothRiderClass0AlignDisplayName = "ForestMammothRiderClass0Align.Name";
        private const string ForestMammothRiderClass0AlignDescription = "ForestMammothRiderClass0Align.Description";
        private const string ForestMammothRiderClass2Align = "ForestMammothRiderClass2Align";
        private static readonly string ForestMammothRiderClass2AlignGuid = "4bf636b9-b37b-453b-b92b-0f9c6b88c8fb";
        private const string ForestMammothRiderClass3Align = "ForestMammothRiderClass3Align";
        private static readonly string ForestMammothRiderClass3AlignGuid = "a229f8d8-36e5-40fd-a280-2f67ec12fd1b";
        private const string ForestMammothRiderClass4Align = "ForestMammothRiderClass4Align";
        private static readonly string ForestMammothRiderClass4AlignGuid = "a757a3ad-3607-4f21-90ab-31f3a1f65f9a";
        private const string ForestMammothRiderClass5Align = "ForestMammothRiderClass5Align";
        private static readonly string ForestMammothRiderClass5AlignGuid = "aa7532c4-20fa-427f-826b-8ed90f70eb70";
        private const string ForestMammothRiderClass6Align = "ForestMammothRiderClass6Align";
        private static readonly string ForestMammothRiderClass6AlignGuid = "2c91ccdb-6f20-436e-a5a5-2c291744a0d4";
        private const string ForestMammothRiderClass7Align = "ForestMammothRiderClass7Align";
        private static readonly string ForestMammothRiderClass7AlignGuid = "a42cd5ea-1a61-4e4d-a207-08fc947c7b74";
        private const string ForestMammothRiderClass8Align = "ForestMammothRiderClass8Align";
        private static readonly string ForestMammothRiderClass8AlignGuid = "2c2cd9d2-1a88-44d4-a852-a7ceb0853e4e";
        private const string ForestMammothRiderClass9Align = "ForestMammothRiderClass9Align";
        private static readonly string ForestMammothRiderClass9AlignGuid = "6d09c846-62cf-4400-afa4-c89c3e0ed816";
        private const string ForestMammothRiderClass10Align = "ForestMammothRiderClass10Align";
        private static readonly string ForestMammothRiderClass10AlignGuid = "aac39c53-485b-4101-ae48-1ddcb0d6aebf";
        private const string ForestSanguineAngelClass0Align = "ForestSanguineAngelClass0Align";
        private static readonly string ForestSanguineAngelClass0AlignGuid = "77dfa87f-efe3-4a46-b39e-f1572764da2a";
        internal const string ForestSanguineAngelClass0AlignDisplayName = "ForestSanguineAngelClass0Align.Name";
        private const string ForestSanguineAngelClass0AlignDescription = "ForestSanguineAngelClass0Align.Description";
        private const string ForestSanguineAngelClass2Align = "ForestSanguineAngelClass2Align";
        private static readonly string ForestSanguineAngelClass2AlignGuid = "1c153c9e-7d2e-481c-9dd1-f9da0b9743f1";
        private const string ForestSanguineAngelClass3Align = "ForestSanguineAngelClass3Align";
        private static readonly string ForestSanguineAngelClass3AlignGuid = "2a069c12-760c-4b46-abc9-54fa8f55a06b";
        private const string ForestSanguineAngelClass4Align = "ForestSanguineAngelClass4Align";
        private static readonly string ForestSanguineAngelClass4AlignGuid = "e533be28-a3c1-43e1-8c9e-26f64f836f1e";
        private const string ForestSanguineAngelClass5Align = "ForestSanguineAngelClass5Align";
        private static readonly string ForestSanguineAngelClass5AlignGuid = "df00234b-610f-4916-b125-ff72940bb753";
        private const string ForestSanguineAngelClass6Align = "ForestSanguineAngelClass6Align";
        private static readonly string ForestSanguineAngelClass6AlignGuid = "1cc9742a-da97-4eaa-ad46-17de4b890fdc";
        private const string ForestSanguineAngelClass7Align = "ForestSanguineAngelClass7Align";
        private static readonly string ForestSanguineAngelClass7AlignGuid = "aeb38506-3a6c-441a-8104-c1dde266a6c3";
        private const string ForestSanguineAngelClass8Align = "ForestSanguineAngelClass8Align";
        private static readonly string ForestSanguineAngelClass8AlignGuid = "a56a74d6-a2ca-430b-9587-8ae85f1d0eae";
        private const string ForestSanguineAngelClass9Align = "ForestSanguineAngelClass9Align";
        private static readonly string ForestSanguineAngelClass9AlignGuid = "164d6536-f9b6-4c4b-b3b3-24245925c577";
        private const string ForestSanguineAngelClass10Align = "ForestSanguineAngelClass10Align";
        private static readonly string ForestSanguineAngelClass10AlignGuid = "fdca5496-de55-4f84-88e9-526af1ca08bf";
        private const string ForestScarSeekerClass0Align = "ForestScarSeekerClass0Align";
        private static readonly string ForestScarSeekerClass0AlignGuid = "a2d16a8d-cf80-44c5-bc27-b1940a86cdec";
        internal const string ForestScarSeekerClass0AlignDisplayName = "ForestScarSeekerClass0Align.Name";
        private const string ForestScarSeekerClass0AlignDescription = "ForestScarSeekerClass0Align.Description";
        private const string ForestScarSeekerClass2Align = "ForestScarSeekerClass2Align";
        private static readonly string ForestScarSeekerClass2AlignGuid = "e25e1f80-3918-4a33-bf88-2c3f4ea1130e";
        private const string ForestScarSeekerClass3Align = "ForestScarSeekerClass3Align";
        private static readonly string ForestScarSeekerClass3AlignGuid = "f34bc2ea-c07d-4ca6-ac4d-f1b5db9c018e";
        private const string ForestScarSeekerClass4Align = "ForestScarSeekerClass4Align";
        private static readonly string ForestScarSeekerClass4AlignGuid = "55b24692-791e-4853-9b27-18d7ff036cae";
        private const string ForestScarSeekerClass5Align = "ForestScarSeekerClass5Align";
        private static readonly string ForestScarSeekerClass5AlignGuid = "e4288be3-7b0c-46e0-a15e-94423c200509";
        private const string ForestScarSeekerClass6Align = "ForestScarSeekerClass6Align";
        private static readonly string ForestScarSeekerClass6AlignGuid = "93167122-459b-4a2e-a9f6-7326a3f2cacf";
        private const string ForestScarSeekerClass7Align = "ForestScarSeekerClass7Align";
        private static readonly string ForestScarSeekerClass7AlignGuid = "c68ebaf4-c433-413e-b1a3-773efb1952a5";
        private const string ForestScarSeekerClass8Align = "ForestScarSeekerClass8Align";
        private static readonly string ForestScarSeekerClass8AlignGuid = "55d10a99-c5f0-4664-abcb-e502534731cb";
        private const string ForestScarSeekerClass9Align = "ForestScarSeekerClass9Align";
        private static readonly string ForestScarSeekerClass9AlignGuid = "dee988f5-22a0-46ce-9d16-5ed3bc0d220a";
        private const string ForestScarSeekerClass10Align = "ForestScarSeekerClass10Align";
        private static readonly string ForestScarSeekerClass10AlignGuid = "23e8d4b1-09c8-4413-bdd1-4dec7c2b02f7";
        private const string ForestSentinelClass0Align = "ForestSentinelClass0Align";
        private static readonly string ForestSentinelClass0AlignGuid = "0c229f60-5e94-40c6-bc5a-57d23b27abc8";
        internal const string ForestSentinelClass0AlignDisplayName = "ForestSentinelClass0Align.Name";
        private const string ForestSentinelClass0AlignDescription = "ForestSentinelClass0Align.Description";
        private const string ForestSentinelClass2Align = "ForestSentinelClass2Align";
        private static readonly string ForestSentinelClass2AlignGuid = "b107cd9f-9fc5-49a1-bcd4-01cf2a76d511";
        private const string ForestSentinelClass3Align = "ForestSentinelClass3Align";
        private static readonly string ForestSentinelClass3AlignGuid = "85900fd1-b6e1-47bf-8284-7fcb598978e0";
        private const string ForestSentinelClass4Align = "ForestSentinelClass4Align";
        private static readonly string ForestSentinelClass4AlignGuid = "e072d22c-2337-4b20-a61a-59a4119a7d87";
        private const string ForestSentinelClass5Align = "ForestSentinelClass5Align";
        private static readonly string ForestSentinelClass5AlignGuid = "59f6bb2c-5b3b-4b67-a733-572008a5bca4";
        private const string ForestSentinelClass6Align = "ForestSentinelClass6Align";
        private static readonly string ForestSentinelClass6AlignGuid = "2ca86fef-8e63-46ce-b62a-6a8aad0771ae";
        private const string ForestSentinelClass7Align = "ForestSentinelClass7Align";
        private static readonly string ForestSentinelClass7AlignGuid = "4194d004-d2e1-44c7-82e6-03caa465a53a";
        private const string ForestSentinelClass8Align = "ForestSentinelClass8Align";
        private static readonly string ForestSentinelClass8AlignGuid = "a0d04e86-2012-4f6b-9f2a-27d9190c6f76";
        private const string ForestSentinelClass9Align = "ForestSentinelClass9Align";
        private static readonly string ForestSentinelClass9AlignGuid = "9f649217-11ed-4777-9901-78506e93405e";
        private const string ForestSentinelClass10Align = "ForestSentinelClass10Align";
        private static readonly string ForestSentinelClass10AlignGuid = "c56f4f40-81b2-4690-b02c-de948af71571";
        private const string ForestShadowDancerClass0Align = "ForestShadowDancerClass0Align";
        private static readonly string ForestShadowDancerClass0AlignGuid = "7a9dff52-45ba-4e23-b29d-4410c6c8e0eb";
        internal const string ForestShadowDancerClass0AlignDisplayName = "ForestShadowDancerClass0Align.Name";
        private const string ForestShadowDancerClass0AlignDescription = "ForestShadowDancerClass0Align.Description";
        private const string ForestShadowDancerClass2Align = "ForestShadowDancerClass2Align";
        private static readonly string ForestShadowDancerClass2AlignGuid = "525a0979-8815-460b-8a2d-53573927bb03";
        private const string ForestShadowDancerClass3Align = "ForestShadowDancerClass3Align";
        private static readonly string ForestShadowDancerClass3AlignGuid = "88d09ace-88f8-4723-9b95-ceb594d6b219";
        private const string ForestShadowDancerClass4Align = "ForestShadowDancerClass4Align";
        private static readonly string ForestShadowDancerClass4AlignGuid = "20ae0555-dce2-447d-950d-56919a11e9d4";
        private const string ForestShadowDancerClass5Align = "ForestShadowDancerClass5Align";
        private static readonly string ForestShadowDancerClass5AlignGuid = "0a9602f2-bd63-49bf-b53e-eee81d33d30f";
        private const string ForestShadowDancerClass6Align = "ForestShadowDancerClass6Align";
        private static readonly string ForestShadowDancerClass6AlignGuid = "b46bb948-58b6-444f-a999-7623426af221";
        private const string ForestShadowDancerClass7Align = "ForestShadowDancerClass7Align";
        private static readonly string ForestShadowDancerClass7AlignGuid = "509e510c-6e15-40fa-911b-0239e11dcc26";
        private const string ForestShadowDancerClass8Align = "ForestShadowDancerClass8Align";
        private static readonly string ForestShadowDancerClass8AlignGuid = "9f620b61-5b42-441e-913b-4b9cd1203881";
        private const string ForestShadowDancerClass9Align = "ForestShadowDancerClass9Align";
        private static readonly string ForestShadowDancerClass9AlignGuid = "5ea28b87-7969-4966-b69d-e19db9f029a9";
        private const string ForestShadowDancerClass10Align = "ForestShadowDancerClass10Align";
        private static readonly string ForestShadowDancerClass10AlignGuid = "b9638ce2-c1b1-4241-8acf-a3a1aea70f66";
        private const string ForestSouldrinkerClass0Align = "ForestSouldrinkerClass0Align";
        private static readonly string ForestSouldrinkerClass0AlignGuid = "54a0bc4b-a10c-409b-ad4c-2f1f3c25cb24";
        internal const string ForestSouldrinkerClass0AlignDisplayName = "ForestSouldrinkerClass0Align.Name";
        private const string ForestSouldrinkerClass0AlignDescription = "ForestSouldrinkerClass0Align.Description";
        private const string ForestSouldrinkerClass2Align = "ForestSouldrinkerClass2Align";
        private static readonly string ForestSouldrinkerClass2AlignGuid = "991fde12-ec74-42c4-af91-81674d47017b";
        private const string ForestSouldrinkerClass3Align = "ForestSouldrinkerClass3Align";
        private static readonly string ForestSouldrinkerClass3AlignGuid = "40f91120-53a4-4261-996f-0903e2266652";
        private const string ForestSouldrinkerClass4Align = "ForestSouldrinkerClass4Align";
        private static readonly string ForestSouldrinkerClass4AlignGuid = "403e3250-dea3-46dc-afe9-e21094e930f5";
        private const string ForestSouldrinkerClass5Align = "ForestSouldrinkerClass5Align";
        private static readonly string ForestSouldrinkerClass5AlignGuid = "02fec89c-cefa-4038-b967-e05e8d43c6ee";
        private const string ForestSouldrinkerClass6Align = "ForestSouldrinkerClass6Align";
        private static readonly string ForestSouldrinkerClass6AlignGuid = "69c84892-68fc-4a0f-8691-47f8c882c08a";
        private const string ForestSouldrinkerClass7Align = "ForestSouldrinkerClass7Align";
        private static readonly string ForestSouldrinkerClass7AlignGuid = "c70ef049-e741-4e2e-bf45-70ea0f6f5217";
        private const string ForestSouldrinkerClass8Align = "ForestSouldrinkerClass8Align";
        private static readonly string ForestSouldrinkerClass8AlignGuid = "002722c2-4774-455a-85b2-7da56abfd178";
        private const string ForestSouldrinkerClass9Align = "ForestSouldrinkerClass9Align";
        private static readonly string ForestSouldrinkerClass9AlignGuid = "be382465-4c03-425f-b385-17e3a26383cf";
        private const string ForestSouldrinkerClass10Align = "ForestSouldrinkerClass10Align";
        private static readonly string ForestSouldrinkerClass10AlignGuid = "f5371c1b-d69c-49a6-acf4-9952d0c62e29";
        private const string ForestUmbralAgentClass0Align = "ForestUmbralAgentClass0Align";
        private static readonly string ForestUmbralAgentClass0AlignGuid = "783a79bc-112b-4853-b37d-996a1b33fa5f";
        internal const string ForestUmbralAgentClass0AlignDisplayName = "ForestUmbralAgentClass0Align.Name";
        private const string ForestUmbralAgentClass0AlignDescription = "ForestUmbralAgentClass0Align.Description";
        private const string ForestUmbralAgentClass2Align = "ForestUmbralAgentClass2Align";
        private static readonly string ForestUmbralAgentClass2AlignGuid = "55cf7369-4e98-4743-955c-81980d830d4a";
        private const string ForestUmbralAgentClass3Align = "ForestUmbralAgentClass3Align";
        private static readonly string ForestUmbralAgentClass3AlignGuid = "a9317ef7-f073-468c-bdce-fc3cfabadf65";
        private const string ForestUmbralAgentClass4Align = "ForestUmbralAgentClass4Align";
        private static readonly string ForestUmbralAgentClass4AlignGuid = "e583f099-ddc0-44ff-9ef6-e0c5f0b3b158";
        private const string ForestUmbralAgentClass5Align = "ForestUmbralAgentClass5Align";
        private static readonly string ForestUmbralAgentClass5AlignGuid = "49a9e29e-2523-40da-bd72-df518deb00e6";
        private const string ForestUmbralAgentClass6Align = "ForestUmbralAgentClass6Align";
        private static readonly string ForestUmbralAgentClass6AlignGuid = "f33ff7e0-27b4-4e9b-9ad1-372820806e5b";
        private const string ForestUmbralAgentClass7Align = "ForestUmbralAgentClass7Align";
        private static readonly string ForestUmbralAgentClass7AlignGuid = "614a45f6-fe74-4016-af79-5f74c8878e34";
        private const string ForestUmbralAgentClass8Align = "ForestUmbralAgentClass8Align";
        private static readonly string ForestUmbralAgentClass8AlignGuid = "6a10cf24-d857-4c11-a664-dafcef7fd492";
        private const string ForestUmbralAgentClass9Align = "ForestUmbralAgentClass9Align";
        private static readonly string ForestUmbralAgentClass9AlignGuid = "7ba0d95c-4a2d-446d-b71a-82d7e0fc41e9";
        private const string ForestUmbralAgentClass10Align = "ForestUmbralAgentClass10Align";
        private static readonly string ForestUmbralAgentClass10AlignGuid = "d6ba1aea-dee3-4251-95e3-02b44b3de1ff";
        private const string ForestMicroAntiPaladinClass0Align = "ForestMicroAntiPaladinClass0Align";
        private static readonly string ForestMicroAntiPaladinClass0AlignGuid = "c28333d1-1333-41b1-b041-a8c3a0d19d5c";
        internal const string ForestMicroAntiPaladinClass0AlignDisplayName = "ForestMicroAntiPaladinClass0Align.Name";
        private const string ForestMicroAntiPaladinClass0AlignDescription = "ForestMicroAntiPaladinClass0Align.Description";
        private const string ForestMicroAntiPaladinClass2Align = "ForestMicroAntiPaladinClass2Align";
        private static readonly string ForestMicroAntiPaladinClass2AlignGuid = "53a7f6bd-e5ad-417e-9337-9957558c2476";
        private const string ForestMicroAntiPaladinClass3Align = "ForestMicroAntiPaladinClass3Align";
        private static readonly string ForestMicroAntiPaladinClass3AlignGuid = "d304ae4c-b7d0-4fa4-8c39-360703286da1";
        private const string ForestMicroAntiPaladinClass4Align = "ForestMicroAntiPaladinClass4Align";
        private static readonly string ForestMicroAntiPaladinClass4AlignGuid = "8c57091d-b557-42e6-a683-ab07baae4391";
        private const string ForestMicroAntiPaladinClass5Align = "ForestMicroAntiPaladinClass5Align";
        private static readonly string ForestMicroAntiPaladinClass5AlignGuid = "dc0672b8-9aa8-488e-a1a4-609b6ecf7067";
        private const string ForestMicroAntiPaladinClass6Align = "ForestMicroAntiPaladinClass6Align";
        private static readonly string ForestMicroAntiPaladinClass6AlignGuid = "7320aa44-7447-454f-84cf-b1003ffd9d61";
        private const string ForestMicroAntiPaladinClass7Align = "ForestMicroAntiPaladinClass7Align";
        private static readonly string ForestMicroAntiPaladinClass7AlignGuid = "409ce05d-deb2-4d92-8412-65709f1525b3";
        private const string ForestMicroAntiPaladinClass8Align = "ForestMicroAntiPaladinClass8Align";
        private static readonly string ForestMicroAntiPaladinClass8AlignGuid = "6da6f20e-5cc6-4825-8416-221042545c63";
        private const string ForestMicroAntiPaladinClass9Align = "ForestMicroAntiPaladinClass9Align";
        private static readonly string ForestMicroAntiPaladinClass9AlignGuid = "8a77e39a-69af-4031-b6ed-beb82d46dc38";
        private const string ForestMicroAntiPaladinClass10Align = "ForestMicroAntiPaladinClass10Align";
        private static readonly string ForestMicroAntiPaladinClass10AlignGuid = "7e3f2281-cdc3-4ef6-9802-1e39f36baf8c";
        private const string ForestOathbreakerClass0Align = "ForestOathbreakerClass0Align";
        private static readonly string ForestOathbreakerClass0AlignGuid = "894f933a-96c2-47f8-9b18-68efbaee9803";
        internal const string ForestOathbreakerClass0AlignDisplayName = "ForestOathbreakerClass0Align.Name";
        private const string ForestOathbreakerClass0AlignDescription = "ForestOathbreakerClass0Align.Description";
        private const string ForestOathbreakerClass2Align = "ForestOathbreakerClass2Align";
        private static readonly string ForestOathbreakerClass2AlignGuid = "023af9fa-35cf-4ff6-ae1c-50f68b9ecb1d";
        private const string ForestOathbreakerClass3Align = "ForestOathbreakerClass3Align";
        private static readonly string ForestOathbreakerClass3AlignGuid = "ba2b2b2b-4852-4611-8d56-a9783ab017e7";
        private const string ForestOathbreakerClass4Align = "ForestOathbreakerClass4Align";
        private static readonly string ForestOathbreakerClass4AlignGuid = "40135c35-8a57-48ed-80db-812bf5f57d4c";
        private const string ForestOathbreakerClass5Align = "ForestOathbreakerClass5Align";
        private static readonly string ForestOathbreakerClass5AlignGuid = "3e628e72-f5f0-4638-a72d-ebec9c8e15f9";
        private const string ForestOathbreakerClass6Align = "ForestOathbreakerClass6Align";
        private static readonly string ForestOathbreakerClass6AlignGuid = "88ee811a-45bc-4fb3-8ce5-d00b4c37373f";
        private const string ForestOathbreakerClass7Align = "ForestOathbreakerClass7Align";
        private static readonly string ForestOathbreakerClass7AlignGuid = "f21d9e40-c1be-420d-85bd-e7aa935322b1";
        private const string ForestOathbreakerClass8Align = "ForestOathbreakerClass8Align";
        private static readonly string ForestOathbreakerClass8AlignGuid = "efa3d233-78d4-4a6c-bfc5-85dd5583013d";
        private const string ForestOathbreakerClass9Align = "ForestOathbreakerClass9Align";
        private static readonly string ForestOathbreakerClass9AlignGuid = "f852bf2e-cccf-4948-a752-38589e505d22";
        private const string ForestOathbreakerClass10Align = "ForestOathbreakerClass10Align";
        private static readonly string ForestOathbreakerClass10AlignGuid = "04112809-605a-48c1-8a99-630ea85f0efb";
        private const string ForestDreadKnightClass0Align = "ForestDreadKnightClass0Align";
        private static readonly string ForestDreadKnightClass0AlignGuid = "7e63e266-d3af-4ad0-aab1-eb6bc1ae9b01";
        internal const string ForestDreadKnightClass0AlignDisplayName = "ForestDreadKnightClass0Align.Name";
        private const string ForestDreadKnightClass0AlignDescription = "ForestDreadKnightClass0Align.Description";
        private const string ForestDreadKnightClass2Align = "ForestDreadKnightClass2Align";
        private static readonly string ForestDreadKnightClass2AlignGuid = "63160a58-ec8f-4eeb-aff3-a7153802ddcf";
        private const string ForestDreadKnightClass3Align = "ForestDreadKnightClass3Align";
        private static readonly string ForestDreadKnightClass3AlignGuid = "394fd486-f346-4e11-9d22-2e6a0b3d35f8";
        private const string ForestDreadKnightClass4Align = "ForestDreadKnightClass4Align";
        private static readonly string ForestDreadKnightClass4AlignGuid = "50a362d4-bafd-435f-a68b-e5882852b4c3";
        private const string ForestDreadKnightClass5Align = "ForestDreadKnightClass5Align";
        private static readonly string ForestDreadKnightClass5AlignGuid = "c099982f-af73-4c76-a47f-3f44b4950f91";
        private const string ForestDreadKnightClass6Align = "ForestDreadKnightClass6Align";
        private static readonly string ForestDreadKnightClass6AlignGuid = "7f4e9a72-509a-41e3-a174-f394aefdacc0";
        private const string ForestDreadKnightClass7Align = "ForestDreadKnightClass7Align";
        private static readonly string ForestDreadKnightClass7AlignGuid = "e10b86b9-0616-468d-80bb-558a15eb4c42";
        private const string ForestDreadKnightClass8Align = "ForestDreadKnightClass8Align";
        private static readonly string ForestDreadKnightClass8AlignGuid = "dc60b39a-4321-4328-bac5-d4de7b9806d6";
        private const string ForestDreadKnightClass9Align = "ForestDreadKnightClass9Align";
        private static readonly string ForestDreadKnightClass9AlignGuid = "28399728-0c3e-47d8-9aeb-46d9e154111e";
        private const string ForestDreadKnightClass10Align = "ForestDreadKnightClass10Align";
        private static readonly string ForestDreadKnightClass10AlignGuid = "6a04f88c-cee0-40a9-82a8-6a83c8361b62";
        private const string ForestStargazerClass0Align = "ForestStargazerClass0Align";
        private static readonly string ForestStargazerClass0AlignGuid = "e2e31aa7-d9fa-45b9-8c45-c051c09f9807";
        internal const string ForestStargazerClass0AlignDisplayName = "ForestStargazerClass0Align.Name";
        private const string ForestStargazerClass0AlignDescription = "ForestStargazerClass0Align.Description";
        private const string ForestStargazerClass2Align = "ForestStargazerClass2Align";
        private static readonly string ForestStargazerClass2AlignGuid = "83353865-bc85-4405-ad86-342a3efeff11";
        private const string ForestStargazerClass3Align = "ForestStargazerClass3Align";
        private static readonly string ForestStargazerClass3AlignGuid = "862ffedc-108e-4c22-99c5-de084b8fcadb";
        private const string ForestStargazerClass4Align = "ForestStargazerClass4Align";
        private static readonly string ForestStargazerClass4AlignGuid = "dce790a3-553b-4b4c-b537-a6193b397cf5";
        private const string ForestStargazerClass5Align = "ForestStargazerClass5Align";
        private static readonly string ForestStargazerClass5AlignGuid = "d09751cc-7f3e-4079-8e21-1984429c1117";
        private const string ForestStargazerClass6Align = "ForestStargazerClass6Align";
        private static readonly string ForestStargazerClass6AlignGuid = "46766168-a361-4b1f-a0fc-13a3193535ae";
        private const string ForestStargazerClass7Align = "ForestStargazerClass7Align";
        private static readonly string ForestStargazerClass7AlignGuid = "10b4bc73-2a68-48b3-89f1-4bf7e635b5af";
        private const string ForestStargazerClass8Align = "ForestStargazerClass8Align";
        private static readonly string ForestStargazerClass8AlignGuid = "85865f22-3981-408b-bb87-870b4c32f400";
        private const string ForestStargazerClass9Align = "ForestStargazerClass9Align";
        private static readonly string ForestStargazerClass9AlignGuid = "a551aae9-72a3-4d52-9667-f671c80ceab0";
        private const string ForestStargazerClass10Align = "ForestStargazerClass10Align";
        private static readonly string ForestStargazerClass10AlignGuid = "a169793d-512f-4f9e-bcaf-deb09467f6a4";
        private const string ForestSwashbucklerClass0Align = "ForestSwashbucklerClass0Align";
        private static readonly string ForestSwashbucklerClass0AlignGuid = "eca10ba1-71c7-4a12-b468-bb63401f272a";
        internal const string ForestSwashbucklerClass0AlignDisplayName = "ForestSwashbucklerClass0Align.Name";
        private const string ForestSwashbucklerClass0AlignDescription = "ForestSwashbucklerClass0Align.Description";
        private const string ForestSwashbucklerClass2Align = "ForestSwashbucklerClass2Align";
        private static readonly string ForestSwashbucklerClass2AlignGuid = "dc2ec429-af85-4cc4-bf4c-f21838a2f1a0";
        private const string ForestSwashbucklerClass3Align = "ForestSwashbucklerClass3Align";
        private static readonly string ForestSwashbucklerClass3AlignGuid = "b773b490-c5fc-427f-8ab3-513ba20c0c1e";
        private const string ForestSwashbucklerClass4Align = "ForestSwashbucklerClass4Align";
        private static readonly string ForestSwashbucklerClass4AlignGuid = "9d77e951-9557-4c50-b186-62d2b62df5d5";
        private const string ForestSwashbucklerClass5Align = "ForestSwashbucklerClass5Align";
        private static readonly string ForestSwashbucklerClass5AlignGuid = "0d2abc07-ded0-4bb7-bad4-3d4c702d70eb";
        private const string ForestSwashbucklerClass6Align = "ForestSwashbucklerClass6Align";
        private static readonly string ForestSwashbucklerClass6AlignGuid = "c71ce1cd-1a69-4131-b581-f01b8cbd6a74";
        private const string ForestSwashbucklerClass7Align = "ForestSwashbucklerClass7Align";
        private static readonly string ForestSwashbucklerClass7AlignGuid = "45e1e1ae-a0ae-4c0e-be67-66764b789a34";
        private const string ForestSwashbucklerClass8Align = "ForestSwashbucklerClass8Align";
        private static readonly string ForestSwashbucklerClass8AlignGuid = "a9b2800d-5358-4d7e-b91d-d122401a2309";
        private const string ForestSwashbucklerClass9Align = "ForestSwashbucklerClass9Align";
        private static readonly string ForestSwashbucklerClass9AlignGuid = "523e365a-9ba6-400c-9f4f-0f31bd3c29e7";
        private const string ForestSwashbucklerClass10Align = "ForestSwashbucklerClass10Align";
        private static readonly string ForestSwashbucklerClass10AlignGuid = "21bcda68-d121-4465-8a09-9cdc746bc658";
        private const string ForestHolyVindicatorClass0Align = "ForestHolyVindicatorClass0Align";
        private static readonly string ForestHolyVindicatorClass0AlignGuid = "0d1a70a0-a860-4b9f-b037-9d36cf9f398f";
        internal const string ForestHolyVindicatorClass0AlignDisplayName = "ForestHolyVindicatorClass0Align.Name";
        private const string ForestHolyVindicatorClass0AlignDescription = "ForestHolyVindicatorClass0Align.Description";
        private const string ForestHolyVindicatorClass2Align = "ForestHolyVindicatorClass2Align";
        private static readonly string ForestHolyVindicatorClass2AlignGuid = "2f29eac6-e522-45ec-ac81-c3edebcf66b9";
        private const string ForestHolyVindicatorClass3Align = "ForestHolyVindicatorClass3Align";
        private static readonly string ForestHolyVindicatorClass3AlignGuid = "9d828117-61aa-4bf6-8983-73012eb5b0cb";
        private const string ForestHolyVindicatorClass4Align = "ForestHolyVindicatorClass4Align";
        private static readonly string ForestHolyVindicatorClass4AlignGuid = "c958c0a4-1775-497c-a65d-250901518aec";
        private const string ForestHolyVindicatorClass5Align = "ForestHolyVindicatorClass5Align";
        private static readonly string ForestHolyVindicatorClass5AlignGuid = "e821c3b3-a24a-4596-9eee-6072ebbffb1d";
        private const string ForestHolyVindicatorClass6Align = "ForestHolyVindicatorClass6Align";
        private static readonly string ForestHolyVindicatorClass6AlignGuid = "40fe501d-ffd9-49c0-aa5e-309b3c2b5642";
        private const string ForestHolyVindicatorClass7Align = "ForestHolyVindicatorClass7Align";
        private static readonly string ForestHolyVindicatorClass7AlignGuid = "ba0837f8-55c6-43e0-b6fd-d5bfa10cba49";
        private const string ForestHolyVindicatorClass8Align = "ForestHolyVindicatorClass8Align";
        private static readonly string ForestHolyVindicatorClass8AlignGuid = "1b7de33d-72c4-4b35-94d0-11410ab337fa";
        private const string ForestHolyVindicatorClass9Align = "ForestHolyVindicatorClass9Align";
        private static readonly string ForestHolyVindicatorClass9AlignGuid = "e267500a-e417-4af0-818d-c50f276d0b38";
        private const string ForestHolyVindicatorClass10Align = "ForestHolyVindicatorClass10Align";
        private static readonly string ForestHolyVindicatorClass10AlignGuid = "687626b1-0149-4da2-aec6-855e05a27b96";
        private const string ForestSummonerClass0Align = "ForestSummonerClass0Align";
        private static readonly string ForestSummonerClass0AlignGuid = "4ff07bbe-1316-4430-a528-a6d0d1659a5e";
        internal const string ForestSummonerClass0AlignDisplayName = "ForestSummonerClass0Align.Name";
        private const string ForestSummonerClass0AlignDescription = "ForestSummonerClass0Align.Description";
        private const string ForestSummonerClass2Align = "ForestSummonerClass2Align";
        private static readonly string ForestSummonerClass2AlignGuid = "5803843e-ca1f-4dd4-b7dc-856d1ff75d95";
        private const string ForestSummonerClass3Align = "ForestSummonerClass3Align";
        private static readonly string ForestSummonerClass3AlignGuid = "df066134-69a1-4755-84db-ac9f7d437f41";
        private const string ForestSummonerClass4Align = "ForestSummonerClass4Align";
        private static readonly string ForestSummonerClass4AlignGuid = "8635e3d5-e25f-4506-a3e4-20d42997a6b8";
        private const string ForestSummonerClass5Align = "ForestSummonerClass5Align";
        private static readonly string ForestSummonerClass5AlignGuid = "f16d87b6-7256-411b-9fa3-b374b666912c";
        private const string ForestSummonerClass6Align = "ForestSummonerClass6Align";
        private static readonly string ForestSummonerClass6AlignGuid = "381ab12a-c97e-448a-b21c-4998f8516b74";
        private const string ForestSummonerClass7Align = "ForestSummonerClass7Align";
        private static readonly string ForestSummonerClass7AlignGuid = "03e87fb2-42de-4ce9-bd45-e6a3d1326e8d";
        private const string ForestSummonerClass8Align = "ForestSummonerClass8Align";
        private static readonly string ForestSummonerClass8AlignGuid = "45238fe1-da08-4741-b079-be4937ee2a98";
        private const string ForestSummonerClass9Align = "ForestSummonerClass9Align";
        private static readonly string ForestSummonerClass9AlignGuid = "e4f629c7-6f42-4f18-b8d5-b47e6977093d";
        private const string ForestSummonerClass10Align = "ForestSummonerClass10Align";
        private static readonly string ForestSummonerClass10AlignGuid = "c0eb3dff-cb2b-43a8-9b5a-e04bb40e1109";
        private const string ForestLionBladeClass0Align = "ForestLionBladeClass0Align";
        private static readonly string ForestLionBladeClass0AlignGuid = "5f0cd3c7-d978-452c-91ea-7c76061f0ad5";
        internal const string ForestLionBladeClass0AlignDisplayName = "ForestLionBladeClass0Align.Name";
        private const string ForestLionBladeClass0AlignDescription = "ForestLionBladeClass0Align.Description";
        private const string ForestLionBladeClass2Align = "ForestLionBladeClass2Align";
        private static readonly string ForestLionBladeClass2AlignGuid = "21c4f365-21e0-400a-bb70-a36f6ae80635";
        private const string ForestLionBladeClass3Align = "ForestLionBladeClass3Align";
        private static readonly string ForestLionBladeClass3AlignGuid = "42648dd3-7a97-44a7-a77f-5a79da8a81d3";
        private const string ForestLionBladeClass4Align = "ForestLionBladeClass4Align";
        private static readonly string ForestLionBladeClass4AlignGuid = "42a780a4-10b1-4f04-9c37-475c24df05fb";
        private const string ForestLionBladeClass5Align = "ForestLionBladeClass5Align";
        private static readonly string ForestLionBladeClass5AlignGuid = "dc74b008-e141-4bf4-b255-10a4b8f07127";
        private const string ForestLionBladeClass6Align = "ForestLionBladeClass6Align";
        private static readonly string ForestLionBladeClass6AlignGuid = "12639daa-e96d-4999-89eb-08957e7942fc";
        private const string ForestLionBladeClass7Align = "ForestLionBladeClass7Align";
        private static readonly string ForestLionBladeClass7AlignGuid = "072afcac-d557-42d8-8268-e8bfc6737c6e";
        private const string ForestLionBladeClass8Align = "ForestLionBladeClass8Align";
        private static readonly string ForestLionBladeClass8AlignGuid = "0c90c6ec-5762-47a6-b56f-5f5c0b3a45a3";
        private const string ForestLionBladeClass9Align = "ForestLionBladeClass9Align";
        private static readonly string ForestLionBladeClass9AlignGuid = "754f1651-4062-4144-8a45-1d29ae34b5a4";
        private const string ForestLionBladeClass10Align = "ForestLionBladeClass10Align";
        private static readonly string ForestLionBladeClass10AlignGuid = "a8514445-1d88-4d37-8e9f-a9acdccf00c5";
        private const string ForestEnchantingCourtesanClass0Align = "ForestEnchantingCourtesanClass0Align";
        private static readonly string ForestEnchantingCourtesanClass0AlignGuid = "72ee224f-7826-4f81-b36c-08567edd25c0";
        internal const string ForestEnchantingCourtesanClass0AlignDisplayName = "ForestEnchantingCourtesanClass0Align.Name";
        private const string ForestEnchantingCourtesanClass0AlignDescription = "ForestEnchantingCourtesanClass0Align.Description";
        private const string ForestEnchantingCourtesanClass2Align = "ForestEnchantingCourtesanClass2Align";
        private static readonly string ForestEnchantingCourtesanClass2AlignGuid = "54f16540-324d-4cea-bf59-607d7d899141";
        private const string ForestEnchantingCourtesanClass3Align = "ForestEnchantingCourtesanClass3Align";
        private static readonly string ForestEnchantingCourtesanClass3AlignGuid = "2ac2cd04-9a61-44ef-a8ca-979b5cc790ce";
        private const string ForestEnchantingCourtesanClass4Align = "ForestEnchantingCourtesanClass4Align";
        private static readonly string ForestEnchantingCourtesanClass4AlignGuid = "92a5b6f9-e1a2-43b1-be1a-56586d288e11";
        private const string ForestEnchantingCourtesanClass5Align = "ForestEnchantingCourtesanClass5Align";
        private static readonly string ForestEnchantingCourtesanClass5AlignGuid = "62e20c5b-1308-4791-8a5a-614a2141ea5d";
        private const string ForestEnchantingCourtesanClass6Align = "ForestEnchantingCourtesanClass6Align";
        private static readonly string ForestEnchantingCourtesanClass6AlignGuid = "6a3d6249-36bb-4264-b083-1a99f1a920c8";
        private const string ForestEnchantingCourtesanClass7Align = "ForestEnchantingCourtesanClass7Align";
        private static readonly string ForestEnchantingCourtesanClass7AlignGuid = "ffcefdbd-0ea8-4461-9e1e-74d779604a49";
        private const string ForestEnchantingCourtesanClass8Align = "ForestEnchantingCourtesanClass8Align";
        private static readonly string ForestEnchantingCourtesanClass8AlignGuid = "a90df578-605f-4dda-b02c-b61f9778dd7a";
        private const string ForestEnchantingCourtesanClass9Align = "ForestEnchantingCourtesanClass9Align";
        private static readonly string ForestEnchantingCourtesanClass9AlignGuid = "c42435b7-af8f-4e4e-bcc4-3be2e3daefa5";
        private const string ForestEnchantingCourtesanClass10Align = "ForestEnchantingCourtesanClass10Align";
        private static readonly string ForestEnchantingCourtesanClass10AlignGuid = "8b3f9f8c-9014-46c6-b4e1-8d2e491bd003";
        private const string ForestHeritorKnightClass0Align = "ForestHeritorKnightClass0Align";
        private static readonly string ForestHeritorKnightClass0AlignGuid = "deb18acb-8478-4bf2-b813-b3a34caaef05";
        internal const string ForestHeritorKnightClass0AlignDisplayName = "ForestHeritorKnightClass0Align.Name";
        private const string ForestHeritorKnightClass0AlignDescription = "ForestHeritorKnightClass0Align.Description";
        private const string ForestHeritorKnightClass2Align = "ForestHeritorKnightClass2Align";
        private static readonly string ForestHeritorKnightClass2AlignGuid = "d72153e6-ec16-4c37-85b8-69cec5d27223";
        private const string ForestHeritorKnightClass3Align = "ForestHeritorKnightClass3Align";
        private static readonly string ForestHeritorKnightClass3AlignGuid = "c75ff1e5-bd04-48f3-af8b-4964e8846c59";
        private const string ForestHeritorKnightClass4Align = "ForestHeritorKnightClass4Align";
        private static readonly string ForestHeritorKnightClass4AlignGuid = "b2773d34-7c38-40d3-ab20-d06c3a667d83";
        private const string ForestHeritorKnightClass5Align = "ForestHeritorKnightClass5Align";
        private static readonly string ForestHeritorKnightClass5AlignGuid = "a8fca69e-9437-4ad0-9fe8-7cee87ca0c42";
        private const string ForestHeritorKnightClass6Align = "ForestHeritorKnightClass6Align";
        private static readonly string ForestHeritorKnightClass6AlignGuid = "4554d4e1-365e-4e4e-bfe1-ab6194d90cfc";
        private const string ForestHeritorKnightClass7Align = "ForestHeritorKnightClass7Align";
        private static readonly string ForestHeritorKnightClass7AlignGuid = "ea761231-00f0-49b8-9160-3dfeca88f7b7";
        private const string ForestHeritorKnightClass8Align = "ForestHeritorKnightClass8Align";
        private static readonly string ForestHeritorKnightClass8AlignGuid = "18e051db-f91b-4805-86b0-b2bc15229b1f";
        private const string ForestHeritorKnightClass9Align = "ForestHeritorKnightClass9Align";
        private static readonly string ForestHeritorKnightClass9AlignGuid = "0cc09345-bc93-461d-b5fa-79a90a4633a8";
        private const string ForestHeritorKnightClass10Align = "ForestHeritorKnightClass10Align";
        private static readonly string ForestHeritorKnightClass10AlignGuid = "0fb6e705-85d9-47bf-b6a7-18063fee2000";
        private const string ForestGoldenLegionnaireClass0Align = "ForestGoldenLegionnaireClass0Align";
        private static readonly string ForestGoldenLegionnaireClass0AlignGuid = "2720c029-36a1-4c9a-bd7a-5807496c3e37";
        internal const string ForestGoldenLegionnaireClass0AlignDisplayName = "ForestGoldenLegionnaireClass0Align.Name";
        private const string ForestGoldenLegionnaireClass0AlignDescription = "ForestGoldenLegionnaireClass0Align.Description";
        private const string ForestGoldenLegionnaireClass2Align = "ForestGoldenLegionnaireClass2Align";
        private static readonly string ForestGoldenLegionnaireClass2AlignGuid = "4f20d02a-795f-4533-97af-0e09b3991c33";
        private const string ForestGoldenLegionnaireClass3Align = "ForestGoldenLegionnaireClass3Align";
        private static readonly string ForestGoldenLegionnaireClass3AlignGuid = "275452ef-176e-467f-95ae-421b2d30d573";
        private const string ForestGoldenLegionnaireClass4Align = "ForestGoldenLegionnaireClass4Align";
        private static readonly string ForestGoldenLegionnaireClass4AlignGuid = "3d7a4ee0-dc25-4c54-84b0-7bc494ecdd44";
        private const string ForestGoldenLegionnaireClass5Align = "ForestGoldenLegionnaireClass5Align";
        private static readonly string ForestGoldenLegionnaireClass5AlignGuid = "c6e551f4-b2d0-4641-a08f-d42fad122aed";
        private const string ForestGoldenLegionnaireClass6Align = "ForestGoldenLegionnaireClass6Align";
        private static readonly string ForestGoldenLegionnaireClass6AlignGuid = "707e36c1-8abf-4907-965e-1d12d44bcd87";
        private const string ForestGoldenLegionnaireClass7Align = "ForestGoldenLegionnaireClass7Align";
        private static readonly string ForestGoldenLegionnaireClass7AlignGuid = "870f5d2d-8bda-4534-afb9-9042dbc18e32";
        private const string ForestGoldenLegionnaireClass8Align = "ForestGoldenLegionnaireClass8Align";
        private static readonly string ForestGoldenLegionnaireClass8AlignGuid = "1c5c65c7-eeb6-41b2-a9d9-01a588d01795";
        private const string ForestGoldenLegionnaireClass9Align = "ForestGoldenLegionnaireClass9Align";
        private static readonly string ForestGoldenLegionnaireClass9AlignGuid = "140566a9-b977-4cc4-8cf7-527caf28d7f9";
        private const string ForestGoldenLegionnaireClass10Align = "ForestGoldenLegionnaireClass10Align";
        private static readonly string ForestGoldenLegionnaireClass10AlignGuid = "f5e9028e-cebe-4141-b8fa-f7f37f619550";
        private const string ForestBoltAceClass0Align = "ForestBoltAceClass0Align";
        private static readonly string ForestBoltAceClass0AlignGuid = "9511884a-4bf4-4964-889f-d119cedb4ece";
        internal const string ForestBoltAceClass0AlignDisplayName = "ForestBoltAceClass0Align.Name";
        private const string ForestBoltAceClass0AlignDescription = "ForestBoltAceClass0Align.Description";
        private const string ForestBoltAceClass2Align = "ForestBoltAceClass2Align";
        private static readonly string ForestBoltAceClass2AlignGuid = "c7b34bf8-762b-4724-8f3a-cfe695c7fcd9";
        private const string ForestBoltAceClass3Align = "ForestBoltAceClass3Align";
        private static readonly string ForestBoltAceClass3AlignGuid = "87a0fd90-9ffc-41c4-ae14-5ad288af55a0";
        private const string ForestBoltAceClass4Align = "ForestBoltAceClass4Align";
        private static readonly string ForestBoltAceClass4AlignGuid = "07bbac4a-631e-45b0-b06c-17d637ec33e2";
        private const string ForestBoltAceClass5Align = "ForestBoltAceClass5Align";
        private static readonly string ForestBoltAceClass5AlignGuid = "249b8183-6d35-4afc-9a00-40a3f6f4ed20";
        private const string ForestBoltAceClass6Align = "ForestBoltAceClass6Align";
        private static readonly string ForestBoltAceClass6AlignGuid = "aeaf148d-f2f6-48aa-8d71-8d98f1099cb0";
        private const string ForestBoltAceClass7Align = "ForestBoltAceClass7Align";
        private static readonly string ForestBoltAceClass7AlignGuid = "71b744cd-6e37-46ce-acd0-0b7aec96a405";
        private const string ForestBoltAceClass8Align = "ForestBoltAceClass8Align";
        private static readonly string ForestBoltAceClass8AlignGuid = "5bd88ca3-0ada-4c0d-8e9e-dd88e4ad8d30";
        private const string ForestBoltAceClass9Align = "ForestBoltAceClass9Align";
        private static readonly string ForestBoltAceClass9AlignGuid = "b37d63b0-3ce4-4239-88e6-6bc1701d2086";
        private const string ForestBoltAceClass10Align = "ForestBoltAceClass10Align";
        private static readonly string ForestBoltAceClass10AlignGuid = "ed354e0d-49d8-4f6e-8a4d-8db6e6a4a62f";
        private const string ForestMortalUsherClass0Align = "ForestMortalUsherClass0Align";
        private static readonly string ForestMortalUsherClass0AlignGuid = "aa207989-58e9-48aa-bb93-50300a18714c";
        internal const string ForestMortalUsherClass0AlignDisplayName = "ForestMortalUsherClass0Align.Name";
        private const string ForestMortalUsherClass0AlignDescription = "ForestMortalUsherClass0Align.Description";
        private const string ForestMortalUsherClass2Align = "ForestMortalUsherClass2Align";
        private static readonly string ForestMortalUsherClass2AlignGuid = "75439fe2-43fd-4445-920b-fd95d88fa8b6";
        private const string ForestMortalUsherClass3Align = "ForestMortalUsherClass3Align";
        private static readonly string ForestMortalUsherClass3AlignGuid = "478396d7-3ebd-4b23-aded-f59c4a31aeaf";
        private const string ForestMortalUsherClass4Align = "ForestMortalUsherClass4Align";
        private static readonly string ForestMortalUsherClass4AlignGuid = "309f7d9a-1b03-4860-81d1-4f4372f40cf2";
        private const string ForestMortalUsherClass5Align = "ForestMortalUsherClass5Align";
        private static readonly string ForestMortalUsherClass5AlignGuid = "040fdee6-5bcf-4c5b-9c50-88b31f68189e";
        private const string ForestMortalUsherClass6Align = "ForestMortalUsherClass6Align";
        private static readonly string ForestMortalUsherClass6AlignGuid = "535c0b05-2e1c-40e2-907a-4d2f68d9ddcf";
        private const string ForestMortalUsherClass7Align = "ForestMortalUsherClass7Align";
        private static readonly string ForestMortalUsherClass7AlignGuid = "3f96fdd8-5567-43a2-a0a6-b1296b1e1516";
        private const string ForestMortalUsherClass8Align = "ForestMortalUsherClass8Align";
        private static readonly string ForestMortalUsherClass8AlignGuid = "75c78455-5b59-4bc3-b72b-64399eb49290";
        private const string ForestMortalUsherClass9Align = "ForestMortalUsherClass9Align";
        private static readonly string ForestMortalUsherClass9AlignGuid = "41ef49fd-eeb1-4b5c-8457-7afa2c64747d";
        private const string ForestMortalUsherClass10Align = "ForestMortalUsherClass10Align";
        private static readonly string ForestMortalUsherClass10AlignGuid = "4877fe28-3b32-4389-bc39-6c3feb27ed95";
    }
}
