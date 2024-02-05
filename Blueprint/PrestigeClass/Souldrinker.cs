using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.FactLogic;
using PrestigePlus.Blueprint.RogueTalent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Blueprints.Classes.Prerequisites;
using PrestigePlus.Blueprint.Feat;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using PrestigePlus.CustomComponent.PrestigeClass;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using PrestigePlus.CustomAction.ClassRelated;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Conditions.Builder.ContextEx;
using PrestigePlus.Blueprint.GrappleFeat;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using PrestigePlus.Modify;
using PrestigePlus.CustomComponent.BasePrestigeEnhance;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class Souldrinker
    {
        private const string ArchetypeName = "Souldrinker";
        public static readonly string ArchetypeGuid = "{DD6B63C0-3EFE-4761-A845-39FC2174C00F}";
        internal const string ArchetypeDisplayName = "Souldrinker.Name";
        private const string ArchetypeDescription = "Souldrinker.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "SouldrinkerPrestige";
        private static readonly string ClassProgressGuid = "{50ABFB30-EAAB-449E-A0F5-F0815C3FCE50}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, Sentinel.BonusFeatGuid, SpellbookReplace.spellupgradeGuid, CreateProficiencies())
                .AddToLevelEntry(2, CreateEnergyDrain(), SoulPoolFeat())
                .AddToLevelEntry(3, Sentinel.DivineBoon1Guid)
                .AddToLevelEntry(4, LesserOblivionFeat())
                .AddToLevelEntry(5, EnergyDrainGuid)
                .AddToLevelEntry(6, Sentinel.DivineBoon2Guid)
                .AddToLevelEntry(7, OblivionFeat())
                .AddToLevelEntry(8, CacodaemonFeature())
                .AddToLevelEntry(9, Sentinel.DivineBoon3Guid)
                .AddToLevelEntry(10, GreaterOblivionFeat())
                .SetDisplayName("")
                .SetDescription(ArchetypeDescription)
                .Configure();

            var archetype =
              CharacterClassConfigurator.New(ArchetypeName, ArchetypeGuid)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .SetSkillPoints(2)
                .SetHitDie(DiceType.D6)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABLow)
                .SetFortitudeSave(SavesPrestigeLow)
                .SetReflexSave(SavesPrestigeLow)
                .SetWillSave(SavesPrestigeHigh)
                .SetProgression(progression)
                .AddSkipLevelsForSpellProgression(new int[] { 6 })
                .SetClassSkills(new StatType[] { StatType.SkillKnowledgeArcana, StatType.SkillKnowledgeWorld, StatType.SkillLoreReligion, StatType.SkillPerception, StatType.SkillPersuasion })
                .AddPrerequisiteFeature(DeificObedience.CharonGuid, group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(DeificObedience.SzurielGuid, group: Prerequisite.GroupType.Any)
                .AddPrerequisiteFeature(FeatureRefs.GreatFortitude.ToString())
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeArcana, 7)
                .AddPrerequisiteStatValue(StatType.SkillKnowledgeWorld, 7)
                .AddPrerequisiteStatValue(StatType.SkillLoreReligion, 7)
                .AddComponent<PrerequisiteSpellLevel>(c => { c.RequiredSpellLevel = 2; })
                .Configure();

            FakeAlignedClass.AddtoMenu(archetype);
        }

        private const string Proficiencies = "Souldrinker.Proficiencies";
        private static readonly string ProficienciesGuid = "{C722B77C-84F8-48D7-8F02-F8E44452F76A}";
        internal const string ProficienciesDisplayName = "SouldrinkerProficiencies.Name";
        private const string ProficienciesDescription = "SouldrinkerProficiencies.Description";

        private static BlueprintFeature CreateProficiencies()
        {
            return FeatureConfigurator.New(Proficiencies, ProficienciesGuid)
              .SetDisplayName(ProficienciesDisplayName)
              .SetDescription(ProficienciesDescription)
              .SetIsClassFeature(true)
              .AddComponent<AddDeityWeaponPro>()
              .Configure();
        }

        private const string Cacodaemon = "SouldrinkerCacodaemon";
        public static readonly string CacodaemonGuid = "{DDFD1C42-FEA4-44A4-8DD1-7937291CFC98}";

        internal const string CacodaemonDisplayName = "SouldrinkerCacodaemon.Name";
        private const string CacodaemonDescription = "SouldrinkerCacodaemon.Description";
        public static BlueprintFeature CacodaemonFeature()
        {
            var icon = AbilityRefs.SummonMonsterIVd3.Reference.Get().Icon;
            return FeatureConfigurator.New(Cacodaemon, CacodaemonGuid)
              .SetDisplayName(CacodaemonDisplayName)
              .SetDescription(CacodaemonDescription)
              .SetIcon(icon)
              .Configure();
        }

        private const string EnergyDrain = "Souldrinker.EnergyDrain";
        private static readonly string EnergyDrainGuid = "{F3F672DD-B795-4D29-8AED-9E3AEF3F9C4B}";
        internal const string EnergyDrainDisplayName = "SouldrinkerEnergyDrain.Name";
        private const string EnergyDrainDescription = "SouldrinkerEnergyDrain.Description";

        private const string EnergyDrainAblity = "Souldrinker.UseEnergyDrain";
        private static readonly string EnergyDrainAblityGuid = "{DF19BAFE-E415-4AB0-B541-53DA08AA7AD2}";

        private const string EnergyDrainBuff = "Souldrinker.EnergyDrainBuff";
        public static readonly string EnergyDrainBuffGuid = "{8DA300BB-107D-45B6-9EC9-D6C854110240}";

        private const string EnergyDrainBuff2 = "Souldrinker.EnergyDrainBuff2";
        public static readonly string EnergyDrainBuff2Guid = "{09C06EEA-288A-4E5D-BD44-E205D6FB3B36}";

        private const string EnergyDrainBuff3 = "Souldrinker.EnergyDrainBuff3";
        public static readonly string EnergyDrainBuff3Guid = "{6E867862-CDCF-4E10-943E-065AFAF23186}";

        private const string EnergyDrainBuff4 = "Souldrinker.EnergyDrainBuff4";
        public static readonly string EnergyDrainBuff4Guid = "{39C536E3-D824-4DAB-B775-66AC78E5E86C}";

        private static BlueprintFeature CreateEnergyDrain()
        {
            var icon = AbilityRefs.PerniciousPoison.Reference.Get().Icon;

            BuffConfigurator.New(EnergyDrainBuff, EnergyDrainBuffGuid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddComponent<EnergyDrainLife>(c => { c.value = 5; })
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff2, EnergyDrainBuff2Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddComponent<EnergyDrainLife>(c => { c.value = 10; })
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff3, EnergyDrainBuff3Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddComponent<EnergyDrainLife>(c => { c.value = 15; })
             .Configure();

            BuffConfigurator.New(EnergyDrainBuff4, EnergyDrainBuff4Guid)
             .SetDisplayName(EnergyDrainDisplayName)
             .SetDescription(EnergyDrainDescription)
             .SetIcon(icon)
             .AddComponent<EnergyDrainLife>(c => { c.value = 20; })
             .Configure();

            var ability = AbilityConfigurator.New(EnergyDrainAblity, EnergyDrainAblityGuid)
                .CopyFrom(
                AbilityRefs.WarpriestFervorNegativeAbility,
                typeof(AbilityDeliverTouch))
                .SetDisplayName(EnergyDrainDisplayName)
                .SetDescription(EnergyDrainDescription)
                .SetIcon(icon)
                .AllowTargeting(false, true, false, false)
                .AddAbilityTargetHasFact(new() { FeatureRefs.ConstructType.ToString(), FeatureRefs.UndeadType.ToString() }, inverted: true)
                .SetSpellDescriptor(SpellDescriptor.NegativeLevel)
                .SetType(AbilityType.Special)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                    .Add<EnergyDrainFalseLife>()
                    .Build())
                .Configure();

            return FeatureConfigurator.New(EnergyDrain, EnergyDrainGuid)
              .SetDisplayName(EnergyDrainDisplayName)
              .SetDescription(EnergyDrainDescription)
              .SetIcon(icon)
              .SetRanks(2)
              .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
        }

        private const string LesserOblivion = "Souldrinker.LesserOblivion";
        private static readonly string LesserOblivionGuid = "{A61D6340-B909-4EF8-865E-3F0ECCF95DF6}";

        internal const string SouldrinkerLesserOblivionDisplayName = "SouldrinkerLesserOblivion.Name";
        private const string SouldrinkerLesserOblivionDescription = "SouldrinkerLesserOblivion.Description";

        private const string LesserOblivionDeath = "Souldrinker.LesserOblivionDeath";
        private static readonly string LesserOblivionDeathGuid = "{5D2295DF-9270-473C-9DB4-90D4AC44A8C5}";

        private const string LesserOblivionWar = "Souldrinker.LesserOblivionWar";
        private static readonly string LesserOblivionWarGuid = "{607229A5-E67A-4AB3-8854-848404013B00}";
        public static BlueprintFeature LesserOblivionFeat()
        {
            var icon = FeatureRefs.BardLoreMaster.Reference.Get().Icon;

            var war = FeatureConfigurator.New(LesserOblivionWar, LesserOblivionWarGuid)
              .SetDisplayName(SouldrinkerLesserOblivionDisplayName)
              .SetDescription(SouldrinkerLesserOblivionDescription)
              .SetIcon(icon)
              .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Bleed)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Bleed)
              .AddImmunityToAbilityScoreDamage(true, statTypes: new StatType[] { StatType.Strength })
              .Configure();

            var death = FeatureConfigurator.New(LesserOblivionDeath, LesserOblivionDeathGuid)
              .SetDisplayName(SouldrinkerLesserOblivionDisplayName)
              .SetDescription(SouldrinkerLesserOblivionDescription)
              .SetIcon(icon)
              .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Petrified)
              .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.Petrified)
              .AddImmunityToAbilityScoreDamage(true, statTypes: new StatType[] { StatType.Constitution })
              .Configure();

            return FeatureConfigurator.New(LesserOblivion, LesserOblivionGuid)
              .SetDisplayName(SouldrinkerLesserOblivionDisplayName)
              .SetDescription(SouldrinkerLesserOblivionDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(DeificObedience.CharonGuid, death)
              .AddFeatureIfHasFact(DeificObedience.SzurielGuid, war)
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string Oblivion = "Souldrinker.Oblivion";
        private static readonly string OblivionGuid = "{7A9C7911-5407-4BE2-B0E8-70C967C10028}";

        internal const string SouldrinkerOblivionDisplayName = "SouldrinkerOblivion.Name";
        private const string SouldrinkerOblivionDescription = "SouldrinkerOblivion.Description";

        private const string OblivionDeath = "Souldrinker.OblivionDeath";
        private static readonly string OblivionDeathGuid = "{F0C20980-69A1-480A-A8CC-185B9F89C5DC}";

        private const string OblivionWar = "Souldrinker.OblivionWar";
        private static readonly string OblivionWarGuid = "{7939CDAC-6B30-44CE-96AF-1114A47D1637}";

        private const string OblivionDeathAbility = "Souldrinker.OblivionDeathAbility";
        private static readonly string OblivionDeathAbilityGuid = "{8B47FE08-A290-41D0-ABDE-80B4CD5CC971}";

        private const string OblivionWarAbility = "Souldrinker.OblivionWarAbility";
        private static readonly string OblivionWarAbilityGuid = "{B491C238-9AB0-4DCE-A00D-410B2DDC1729}";
        public static BlueprintFeature OblivionFeat()
        {
            var icon = AbilityRefs.VampiricTouchCast.Reference.Get().Icon;

            var abilitywar = AbilityConfigurator.New(OblivionWarAbility, OblivionWarAbilityGuid)
                .CopyFrom(
                AbilityRefs.Rage,
                typeof(AbilityEffectRunAction),
                typeof(AbilityTargetsAround),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SoulPoolAbilityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var war = FeatureConfigurator.New(OblivionWar, OblivionWarGuid)
              .SetDisplayName(SouldrinkerOblivionDisplayName)
              .SetDescription(SouldrinkerOblivionDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilitywar })
              .Configure();

            var abilitydeath = AbilityConfigurator.New(OblivionDeathAbility, OblivionDeathAbilityGuid)
                .CopyFrom(
                AbilityRefs.VampiricTouchCast,
                typeof(AbilityEffectRunAction),
                typeof(AbilityEffectStickyTouch),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 3)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SoulPoolAbilityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var death = FeatureConfigurator.New(OblivionDeath, OblivionDeathGuid)
              .SetDisplayName(SouldrinkerOblivionDisplayName)
              .SetDescription(SouldrinkerOblivionDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilitydeath })
              .Configure();

            return FeatureConfigurator.New(Oblivion, OblivionGuid)
              .SetDisplayName(SouldrinkerOblivionDisplayName)
              .SetDescription(SouldrinkerOblivionDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(DeificObedience.CharonGuid, death)
              .AddFeatureIfHasFact(DeificObedience.SzurielGuid, war)
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private const string GreaterOblivion = "Souldrinker.GreaterOblivion";
        private static readonly string GreaterOblivionGuid = "{495DABC8-34CE-430C-92B2-1ED97CF6FC92}";

        internal const string SouldrinkerGreaterOblivionDisplayName = "SouldrinkerGreaterOblivion.Name";
        private const string SouldrinkerGreaterOblivionDescription = "SouldrinkerGreaterOblivion.Description";

        private const string GreaterOblivionDeath = "Souldrinker.GreaterOblivionDeath";
        private static readonly string GreaterOblivionDeathGuid = "{D94C816E-0EA8-4098-BFD7-21F254685F01}";

        private const string GreaterOblivionWar = "Souldrinker.GreaterOblivionWar";
        private static readonly string GreaterOblivionWarGuid = "{D3E28EE1-ED74-489C-AB71-D6B12743A633}";

        private const string GreaterOblivionDeathAbility = "Souldrinker.GreaterOblivionDeathAbility";
        private static readonly string GreaterOblivionDeathAbilityGuid = "{3B26D251-179C-4E08-95FE-BC96CDFE5D50}";

        private const string GreaterOblivionWarAbility = "Souldrinker.GreaterOblivionWarAbility";
        private static readonly string GreaterOblivionWarAbilityGuid = "{78E35558-420C-4B39-AD9C-EAAA0E91C9E2}";
        public static BlueprintFeature GreaterOblivionFeat()
        {
            var icon = AbilityRefs.CircleOfDeath.Reference.Get().Icon;

            var abilitywar = AbilityConfigurator.New(GreaterOblivionWarAbility, GreaterOblivionWarAbilityGuid)
                .CopyFrom(
                AbilityRefs.BladeBarrier,
                typeof(AbilityEffectRunAction),
                typeof(ContextRankConfig),
                typeof(SpellComponent),
                typeof(SpellDescriptorComponent))
                .AddPretendSpellLevel(spellLevel: 6)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: SoulPoolAbilityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var war = FeatureConfigurator.New(GreaterOblivionWar, GreaterOblivionWarGuid)
              .SetDisplayName(SouldrinkerGreaterOblivionDisplayName)
              .SetDescription(SouldrinkerGreaterOblivionDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilitywar })
              .Configure();

            var abilitydeath = AbilityConfigurator.New(GreaterOblivionDeathAbility, GreaterOblivionDeathAbilityGuid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .ApplyBuff(BuffRefs.FastHealing10.ToString(), ContextDuration.Fixed(10))
                        .Build())
                .SetDisplayName(SouldrinkerGreaterOblivionDisplayName)
                .SetDescription(SouldrinkerGreaterOblivionDescription)
                .SetIcon(icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .AddPretendSpellLevel(spellLevel: 6)
                .AddAbilityResourceLogic(3, isSpendResource: true, requiredResource: SoulPoolAbilityResGuid)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var death = FeatureConfigurator.New(GreaterOblivionDeath, GreaterOblivionDeathGuid)
              .SetDisplayName(SouldrinkerGreaterOblivionDisplayName)
              .SetDescription(SouldrinkerGreaterOblivionDescription)
              .SetIcon(icon)
              .AddFacts(new() { abilitydeath })
              .Configure();

            return FeatureConfigurator.New(GreaterOblivion, GreaterOblivionGuid)
              .SetDisplayName(SouldrinkerGreaterOblivionDisplayName)
              .SetDescription(SouldrinkerGreaterOblivionDescription)
              .SetIcon(icon)
              .AddFeatureIfHasFact(DeificObedience.CharonGuid, death)
              .AddFeatureIfHasFact(DeificObedience.SzurielGuid, war)
              .SetHideInCharacterSheetAndLevelUp()
              .Configure();
        }

        private static readonly string SoulPoolFeatName = "SouldrinkerSoulPool";
        public static readonly string SoulPoolFeatGuid = "{5973C97A-D210-4499-A57D-C033B18245E4}";

        private static readonly string SoulPoolDisplayName = "SouldrinkerSoulPool.Name";
        private static readonly string SoulPoolDescription = "SouldrinkerSoulPool.Description";

        private static readonly string SoulPool2DisplayName = "SouldrinkerSoulPool2.Name";
        private static readonly string SoulPool2Description = "SouldrinkerSoulPool2.Description";

        private static readonly string SoulPool3DisplayName = "SouldrinkerSoulPool3.Name";
        private static readonly string SoulPool3Description = "SouldrinkerSoulPool3.Description";

        private static readonly string SoulPool4DisplayName = "SouldrinkerSoulPool4.Name";
        private static readonly string SoulPool4Description = "SouldrinkerSoulPool4.Description";

        private const string SoulPoolAbility2 = "Souldrinker.SoulPoolAbility2";
        private static readonly string SoulPoolAbility2Guid = "{DD8F513C-D7E8-46B6-953A-85546ED54579}";

        private const string SoulPoolAbility3 = "Souldrinker.SoulPoolAbility3";
        private static readonly string SoulPoolAbility3Guid = "{AEE3D3B6-B89A-46C4-917B-CDE9A738E665}";

        private const string SoulPoolAbility4 = "Souldrinker.SoulPoolAbility4";
        private static readonly string SoulPoolAbility4Guid = "{B65986E4-4A3F-4F05-8078-FC9DD62AF8D1}";

        private const string SoulPoolAbilityRes = "CrimsonTemplarStyle.SoulPoolAbilityRes";
        public static readonly string SoulPoolAbilityResGuid = "{DF8CAC1D-C121-41BD-AB7A-118DEE5FB340}";
        public static BlueprintFeature SoulPoolFeat()
        {
            var icon = AbilityRefs.HarmCast.Reference.Get().Icon;

            var abilityresourse = AbilityResourceConfigurator.New(SoulPoolAbilityRes, SoulPoolAbilityResGuid)
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0)
                        .IncreaseByLevel(classes: new string[] { ArchetypeGuid }))
                .SetUseMax()
                .SetMax(10)
                .Configure();

            var abilityTrick2 = AbilityConfigurator.New(SoulPoolAbility2, SoulPoolAbility2Guid)
                .AddAbilityEffectRunAction(ActionsBuilder.New()
                        .HealEnergyDrain(Kingmaker.RuleSystem.Rules.EnergyDrainHealType.One, Kingmaker.RuleSystem.Rules.EnergyDrainHealType.None)
                        .Build())
                .SetDisplayName(SoulPool3DisplayName)
                .SetDescription(SoulPool3Description)
                .SetIcon(icon)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: abilityresourse)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Free)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            var abilityTrick3 = AbilityConfigurator.New(SoulPoolAbility3, SoulPoolAbility3Guid)
                .AddComponent<AbilityRestoreSoulSpell>(c => { c.RequiredResource = abilityresourse; })
                .AddAbilityRestoreSpellSlot(true)
                .SetActionBarAutoFillIgnored(true)
                .SetDisplayName(SoulPool4DisplayName)
                .SetDescription(SoulPool4Description)
                .SetIcon(icon)
                .SetIsFullRoundAction(true)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            var abilityTrick4 = AbilityConfigurator.New(SoulPoolAbility4, SoulPoolAbility4Guid)
                .AddComponent<AbilityRestoreSoulSpell2>(c => { c.RequiredResource = abilityresourse; })
                .AddAbilityRestoreSpontaneousSpell(true)
                .SetActionBarAutoFillIgnored(true)
                .SetHidden(true)
                .SetDisplayName(SoulPool4DisplayName)
                .SetDescription(SoulPool4Description)
                .SetIcon(icon)
                .SetIsFullRoundAction(true)
                .SetRange(AbilityRange.Personal)
                .SetType(AbilityType.Supernatural)
                .Configure();

            return FeatureConfigurator.New(SoulPoolFeatName, SoulPoolFeatGuid)
                    .SetDisplayName(SoulPoolDisplayName)
                    .SetDescription(SoulPoolDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { abilityTrick2, abilityTrick3, abilityTrick4 })
                    .AddComponent<SoulPointStuff>(c => { c.Resource = abilityresourse; })
                    .AddAbilityResources(resource: abilityresourse, restoreAmount: false)
                    .Configure();
        }
    }
}
