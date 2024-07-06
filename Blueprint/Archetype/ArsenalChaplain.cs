using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrestigePlus.Mechanic;
using PrestigePlus.Modify;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Designers.Mechanics.Facts;

namespace PrestigePlus.Blueprint.Archetype
{
    internal class ArsenalChaplain
    {
        private const string ArchetypeName = "ArsenalChaplain";
        private static readonly string ArchetypeGuid = "{60734724-FA89-4C53-8E27-0A17E97DE20B}";
        internal const string ArchetypeDisplayName = "ArsenalChaplain.Name";
        private const string ArchetypeDescription = "ArsenalChaplain.Description";
        public static void Configure()
        {
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.WarpriestClass)
              .SetLocalizedName(ArchetypeDisplayName)
              .SetLocalizedDescription(ArchetypeDescription)
            .SetRemoveFeaturesEntry(1, FeatureSelectionRefs.BlessingSelection.ToString(), FeatureSelectionRefs.SecondBlessingSelection.ToString(), FeatureRefs.WarpriestSacredWeaponBaseDamageFeature.ToString())
            .SetRemoveFeaturesEntry(4, FeatureSelectionRefs.WarpriestChannelEnergySelection.ToString())
            .SetRemoveFeaturesEntry(7, FeatureRefs.SacredArmorFeature.ToString())
            .SetRemoveFeaturesEntry(10, FeatureRefs.SacredArmorEnchantPlus2.ToString())
            .SetRemoveFeaturesEntry(13, FeatureRefs.SacredArmorEnchantPlus3.ToString())
            .SetRemoveFeaturesEntry(16, FeatureRefs.SacredArmorEnchantPlus4.ToString())
            .SetRemoveFeaturesEntry(19, FeatureRefs.SacredArmorEnchantPlus5.ToString())
            .AddToAddFeatures(1, "2097edd687ff4cdeb33872c048599fc1")
            .AddToAddFeatures(7, WarB7Feat())
            .AddToAddFeatures(10, WarB10Feat())
            .AddToAddFeatures(13, WarB13Feat())
            .AddToAddFeatures(16, CreateWarB16())
            .AddToAddFeatures(19, CreateWarB19())
            .AddToAddFeatures(5, FeatureSelectionRefs.WeaponTrainingSelection.ToString())
            .AddToAddFeatures(9, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .AddToAddFeatures(13, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
            .AddToAddFeatures(17, FeatureSelectionRefs.WeaponTrainingSelection.ToString(), FeatureSelectionRefs.WeaponTrainingRankUpSelection.ToString())
              .Configure();

            ProgressionConfigurator.For(ProgressionRefs.WarpriestProgression)
                .AddToUIGroups([WarB7Guid, WarB10Guid, WarB13Guid, WarB16Guid, WarB19Guid, "2097edd687ff4cdeb33872c048599fc1"])
                .Configure();
        }

        private const string WarB7 = "ArsenalChaplain.WarB7";
        public static readonly string WarB7Guid = "{BEB253E3-48FE-4895-BF17-EB3B7ACB7CC6}";

        internal const string WarB7DisplayName = "ArsenalChaplainWarB7.Name";
        private const string WarB7Description = "ArsenalChaplainWarB7.Description";

        private const string WarB7Buff = "ArsenalChaplain.WarB7Buff";
        public static readonly string WarB7BuffGuid = "{3B12723D-3058-49E4-8C62-A0F7EEDF991D}";

        private const string WarB7Ability = "ArsenalChaplain.WarB7Ability";
        private static readonly string WarB7AbilityGuid = "{5BAFF80A-9F06-4E41-9225-E8283B41D55D}";
        public static BlueprintFeature WarB7Feat()
        {
            var icon = FeatureRefs.DisplayWeaponProwess.Reference.Get().Icon;

            var feat = BuffConfigurator.New(WarB7Buff, WarB7BuffGuid)
              .SetDisplayName(WarB7DisplayName)
              .SetDescription(WarB7Description)
              .SetIcon(icon)
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("13610da20c4840ed986568412207eba0"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("d6ab932ea5304c14ab3155647435b76c"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("85d935854e574b31a32ea6b031ba8f84"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("a15fa9c66b794f6986ee1d1d97db3419"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("b25af29679004b2085277bb8979b2912"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Move; })
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(WarB7Ability, WarB7AbilityGuid)
                .SetDisplayName(WarB7DisplayName)
                .SetDescription(WarB7Description)
                .SetIcon(icon)
                .SetBuff(feat)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(WarB7, WarB7Guid)
              .SetDisplayName(WarB7DisplayName)
              .SetDescription(WarB7Description)
              .SetIcon(icon)
              .AddComponent<ClassLevelsForPrerequisites>(c => {
                  c.m_ActualClass = CharacterClassRefs.WarpriestClass.Reference.Get().ToReference<BlueprintCharacterClassReference>();
                  c.m_FakeClass = CharacterClassRefs.FighterClass.Reference.Get().ToReference<BlueprintCharacterClassReference>();
                  c.m_ForSelection = FeatureSelectionRefs.WeaponTrainingSelection.Reference.Get().ToReference<BlueprintFeatureSelectionReference>();
                  c.Modifier = 1;
                  c.Summand = 0;
              })
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WarB16 = "ArsenalChaplain.WarB16";
        private static readonly string WarB16Guid = "{9253348A-9833-48D6-B20D-C18A8C161AB8}";

        internal const string WarB16DisplayName = "ArsenalChaplainWarB16.Name";
        private const string WarB16Description = "ArsenalChaplainWarB16.Description";
        private static BlueprintFeature CreateWarB16()
        {
            var icon = FeatureRefs.ShatterDefenses.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(WarB16, WarB16Guid)
              .SetDisplayName(WarB16DisplayName)
              .SetDescription(WarB16Description)
              .SetIcon(icon)
              .Configure();

            AddToResourceFact.Patch("13610da20c4840ed986568412207eba0", false, feat);
            AddToResourceFact.Patch("d6ab932ea5304c14ab3155647435b76c", false, feat);
            AddToResourceFact.Patch("85d935854e574b31a32ea6b031ba8f84", false, feat);
            AddToResourceFact.Patch("a15fa9c66b794f6986ee1d1d97db3419", false, feat);
            AddToResourceFact.Patch("b25af29679004b2085277bb8979b2912", false, feat);

            return feat;
        }

        private const string WarB19 = "ArsenalChaplain.WarB19";
        private static readonly string WarB19Guid = "{B4AB4585-B7B1-4F86-B048-8E7F4B0EF5A1}";

        internal const string WarB19DisplayName = "ArsenalChaplainWarB19.Name";
        private const string WarB19Description = "ArsenalChaplainWarB19.Description";
        private static BlueprintFeature CreateWarB19()
        {
            var icon = FeatureRefs.FighterTraining.Reference.Get().Icon;

            var feat = FeatureConfigurator.New(WarB19, WarB19Guid)
              .SetDisplayName(WarB19DisplayName)
              .SetDescription(WarB19Description)
              .SetIcon(icon)
              .Configure();

            AddToResourceFact.Patch("13610da20c4840ed986568412207eba0", false, feat);
            AddToResourceFact.Patch("d6ab932ea5304c14ab3155647435b76c", false, feat);
            AddToResourceFact.Patch("85d935854e574b31a32ea6b031ba8f84", false, feat);
            AddToResourceFact.Patch("a15fa9c66b794f6986ee1d1d97db3419", false, feat);
            AddToResourceFact.Patch("b25af29679004b2085277bb8979b2912", false, feat);

            return feat;
        }

        private const string WarB10 = "ArsenalChaplain.WarB10";
        public static readonly string WarB10Guid = "{79F0F0F2-C08B-4FA4-942A-7710E05C2D48}";

        internal const string WarB10DisplayName = "ArsenalChaplainWarB10.Name";
        private const string WarB10Description = "ArsenalChaplainWarB10.Description";

        private const string WarB10Buff = "ArsenalChaplain.WarB10Buff";
        public static readonly string WarB10BuffGuid = "{05A6EDA8-9DFA-42DF-850D-8B26554B4B91}";

        private const string WarB10Ability = "ArsenalChaplain.WarB10Ability";
        private static readonly string WarB10AbilityGuid = "{D07B4440-682F-49B4-9C1B-CF8DCCFE45B7}";
        public static BlueprintFeature WarB10Feat()
        {
            var icon = AbilityRefs.FeatherStep.Reference.Get().Icon;

            var feat = BuffConfigurator.New(WarB10Buff, WarB10BuffGuid)
              .SetDisplayName(WarB10DisplayName)
              .SetDescription(WarB10Description)
              .SetIcon(icon)
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("13610da20c4840ed986568412207eba0"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("d6ab932ea5304c14ab3155647435b76c"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("85d935854e574b31a32ea6b031ba8f84"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("a15fa9c66b794f6986ee1d1d97db3419"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free; })
              .AddComponent<ChangeActionSpell>(c => { c.Ability = BlueprintTool.GetRef<BlueprintAbilityReference>("b25af29679004b2085277bb8979b2912"); c.Type = Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free; })
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            AddToResourceFact.Patch("13610da20c4840ed986568412207eba0", true, feat);
            AddToResourceFact.Patch("d6ab932ea5304c14ab3155647435b76c", true, feat);
            AddToResourceFact.Patch("85d935854e574b31a32ea6b031ba8f84", true, feat);
            AddToResourceFact.Patch("a15fa9c66b794f6986ee1d1d97db3419", true, feat);
            AddToResourceFact.Patch("b25af29679004b2085277bb8979b2912", true, feat);

            var ability = ActivatableAbilityConfigurator.New(WarB10Ability, WarB10AbilityGuid)
                .SetDisplayName(WarB10DisplayName)
                .SetDescription(WarB10Description)
                .SetIcon(icon)
                .SetBuff(feat)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(WarB10, WarB10Guid)
              .SetDisplayName(WarB10DisplayName)
              .SetDescription(WarB10Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string WarB13 = "ArsenalChaplain.WarB13";
        public static readonly string WarB13Guid = "{7017C487-61C6-47E8-9E0E-0FD0F6E6B467}";

        internal const string WarB13DisplayName = "ArsenalChaplainWarB13.Name";
        private const string WarB13Description = "ArsenalChaplainWarB13.Description";

        private const string WarB13Buff = "ArsenalChaplain.WarB13Buff";
        private static readonly string WarB13BuffGuid = "{777598B9-F318-4983-B915-FED486CFC74B}";

        private const string WarB13Ability = "ArsenalChaplain.WarB13Ability";
        private static readonly string WarB13AbilityGuid = "{43F2928F-8A4B-4ED9-95BB-DDD5EF8A22F2}";
        public static BlueprintFeature WarB13Feat()
        {
            var icon = AbilityRefs.Bane.Reference.Get().Icon;

            var feat = BuffConfigurator.New(WarB13Buff, WarB13BuffGuid)
              .SetDisplayName(WarB13DisplayName)
              .SetDescription(WarB13Description)
              .SetIcon(icon)
              .AddAutoMetamagic(["13610da20c4840ed986568412207eba0",
                    "d6ab932ea5304c14ab3155647435b76c",
                    "85d935854e574b31a32ea6b031ba8f84",
                    "a15fa9c66b794f6986ee1d1d97db3419",
                    "b25af29679004b2085277bb8979b2912"],
              metamagic: Metamagic.Reach, allowedAbilities: Kingmaker.Designers.Mechanics.Facts.AutoMetamagic.AllowedType.Any, once: false)
              .AddToFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddToFlags(BlueprintBuff.Flags.StayOnDeath)
              .Configure();

            AddToResourceFact.Patch("13610da20c4840ed986568412207eba0", true, feat);
            AddToResourceFact.Patch("d6ab932ea5304c14ab3155647435b76c", true, feat);
            AddToResourceFact.Patch("85d935854e574b31a32ea6b031ba8f84", true, feat);
            AddToResourceFact.Patch("a15fa9c66b794f6986ee1d1d97db3419", true, feat);
            AddToResourceFact.Patch("b25af29679004b2085277bb8979b2912", true, feat);

            var ability = ActivatableAbilityConfigurator.New(WarB13Ability, WarB13AbilityGuid)
                .SetDisplayName(WarB13DisplayName)
                .SetDescription(WarB13Description)
                .SetIcon(icon)
                .SetBuff(feat)
                .SetDeactivateImmediately(true)
                .Configure();

            return FeatureConfigurator.New(WarB13, WarB13Guid)
              .SetDisplayName(WarB13DisplayName)
              .SetDescription(WarB13Description)
              .SetIcon(icon)
              .AddFacts(new() { ability })
              .Configure();
        }
    }
}
