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
using PrestigePlus.Feats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.Root;
using Kingmaker.EntitySystem.Stats;
using BlueprintCore.Utils.Types;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using PrestigePlus.Grapple;
using PrestigePlus.Maneuvers;
using BlueprintCore.Actions.Builder.ContextEx;
using PrestigePlus.Modify;
using TabletopTweaks.Core.NewComponents;
using PrestigePlus.CustomComponent.PrestigeClass;

namespace PrestigePlus.Blueprint.PrestigeClass
{
    internal class DragonFury
    {
        private const string ArchetypeName = "DragonFury";
        private static readonly string ArchetypeGuid = "{B216ABB1-3EF6-4E03-9D58-69CE7CF94B4F}";
        internal const string ArchetypeDisplayName = "DragonFury.Name";
        private const string ArchetypeDescription = "DragonFury.Description";

        private static readonly string BABFull = "b3057560ffff3514299e8b93e7648a9d";
        private static readonly string BABLow = "0538081888b2d8c41893d25d098dee99";
        private static readonly string BABMedium = "4c936de4249b61e419a3fb775b9f2581";

        private static readonly string SavesPrestigeLow = "dc5257e1100ad0d48b8f3b9798421c72";
        private static readonly string SavesPrestigeHigh = "1f309006cd2855e4e91a6c3707f3f700";

        private const string ClassProgressName = "DragonFuryPrestige";
        private static readonly string ClassProgressGuid = "{DC26C8F9-4164-4A34-8518-9B0A39B678F9}";

        public static void Configure()
        {
            var progression =
                ProgressionConfigurator.New(ClassProgressName, ClassProgressGuid)
                .SetClasses(ArchetypeGuid)
                .AddToLevelEntry(1, SharpFangFeature(), DualFangFocusFeature(), FuryTrainingFeature())
                .AddToLevelEntry(2, OuterSphereStanceConfigure())
                .AddToLevelEntry(3, FuryDefenseFeature())
                .AddToLevelEntry(4, DualFangFocusGuid)
                .AddToLevelEntry(5, ViciousFangFeature())
                .AddToLevelEntry(6, FuryDefenseGuid)
                .AddToLevelEntry(7, DualFangFocusGuid)
                .AddToLevelEntry(8, BendwithWindConfigure())
                .AddToLevelEntry(9, FuryDefenseGuid)
                .AddToLevelEntry(10, DragonWarDanceConfigure(), DualFangFocusGuid)
                .SetUIGroups(UIGroupBuilder.New()
                    .AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SharpFangGuid }))
                ///.AddGroup(new Blueprint<BlueprintFeatureBaseReference>[] { SeekerArrowGuid, PhaseArrowGuid, HailArrowGuid, DeathArrowGuid }))
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
                .SetHitDie(DiceType.D10)
                .SetPrestigeClass(true)
                .SetBaseAttackBonus(BABFull)
                .SetFortitudeSave(SavesPrestigeHigh)
                .SetReflexSave(SavesPrestigeHigh)
                .SetWillSave(SavesPrestigeLow)
                .SetProgression(progression)
                .SetClassSkills(new StatType[] { StatType.SkillAthletics, StatType.SkillMobility, StatType.SkillLoreNature, StatType.SkillPerception })
                .AddPrerequisiteStatValue(StatType.BaseAttackBonus, 4)
                .AddPrerequisiteStatValue(StatType.SkillAthletics, 6)
                .AddPrerequisiteStatValue(StatType.SkillMobility, 6)
                .AddPrerequisiteFeature(ParametrizedFeatureRefs.WeaponFocus.ToString())
                .AddPrerequisiteFeature(FeatureRefs.CombatExpertiseFeature.ToString())
                .AddPrerequisiteFeature(FeatureRefs.TwoWeaponFighting.ToString())
                .Configure();

            Action<ProgressionRoot> act = delegate (ProgressionRoot i)
            {
                BlueprintCharacterClassReference[] result = new BlueprintCharacterClassReference[i.m_CharacterClasses.Length + 1];
                for (int a = 0; a < i.m_CharacterClasses.Length; a++)
                {
                    result[a] = i.m_CharacterClasses[a];
                }
                var DragonFuryref = archetype.ToReference<BlueprintCharacterClassReference>();
                result[i.m_CharacterClasses.Length] = DragonFuryref;
                i.m_CharacterClasses = result;
            };

            RootConfigurator.For(RootRefs.BlueprintRoot)
                .ModifyProgression(act)
                .Configure(delayed: true);
        }

        private const string FuryTraining = "FuryTraining";
        private static readonly string FuryTrainingGuid = "{F5E6DA2E-6C88-46D8-8493-379097D80EA2}";

        internal const string FuryTrainingDisplayName = "FuryTraining.Name";
        private const string FuryTrainingDescription = "FuryTraining.Description";
        public static BlueprintFeature FuryTrainingFeature()
        {
            var icon = FeatureRefs.NatureSense.Reference.Get().Icon;
            return FeatureConfigurator.New(FuryTraining, FuryTrainingGuid)
              .SetDisplayName(FuryTrainingDisplayName)
              .SetDescription(FuryTrainingDescription)
              .SetIcon(icon)
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.KiPowerResource.ToString(), ContextValues.Rank(AbilityRankType.DamageDice))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.RageResourse.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BloodragerRageResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.FocusedRageResourse.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.DemonRageResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddIncreaseResourceAmountBySharedValue(false, AbilityResourceRefs.BardicPerformanceResource.ToString(), ContextValues.Rank(AbilityRankType.ProjectilesCount))
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.DamageDice).WithOnePlusDiv2Progression())
              .AddContextRankConfig(ContextRankConfigs.ClassLevel(new[] { ArchetypeGuid }, type: AbilityRankType.ProjectilesCount).WithBonusValueProgression(0, true))
              .Configure();
        }

        private const string DualFangFocus = "DualFangFocus";
        private static readonly string DualFangFocusGuid = "{F757BBA9-EF7D-4DEA-BCDD-BF071A584D0A}";

        internal const string DualFangFocusDisplayName = "DualFangFocus.Name";
        private const string DualFangFocusDescription = "DualFangFocus.Description";
        public static BlueprintFeature DualFangFocusFeature()
        {
            var icon = FeatureRefs.ShifterClawsFeatureAddLevel1.Reference.Get().Icon;
            return FeatureConfigurator.New(DualFangFocus, DualFangFocusGuid)
              .SetDisplayName(DualFangFocusDisplayName)
              .SetDescription(DualFangFocusDescription)
              .SetIcon(icon)
              .AddComponent<DualFangFocus>()
              .SetRanks(10)
              .Configure();
        }

        private const string SharpFang = "SharpFang";
        public static readonly string SharpFangGuid = "{68504AA7-C18E-4EB5-A6F0-BBC26EEA5AB7}";

        internal const string SharpFangDisplayName = "SharpFang.Name";
        private const string SharpFangDescription = "SharpFang.Description";
        public static BlueprintFeature SharpFangFeature()
        {
            var icon = FeatureRefs.ShiftersEdgeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(SharpFang, SharpFangGuid)
              .SetDisplayName(SharpFangDisplayName)
              .SetDescription(SharpFangDescription)
              .SetIcon(icon)
              .AddFacts(new() { FeatureRefs.PowerAttackFeature.ToString() })
              .Configure();
        }

        private const string FuryDefense = "FuryDefense";
        private static readonly string FuryDefenseGuid = "{DA5D8199-BDA1-4CB6-8D4D-ED6B25DB6254}";

        internal const string FuryDefenseDisplayName = "FuryDefense.Name";
        private const string FuryDefenseDescription = "FuryDefense.Description";
        public static BlueprintFeature FuryDefenseFeature()
        {
            var icon = FeatureRefs.ShifterACBonusUnlock.Reference.Get().Icon;
            return FeatureConfigurator.New(FuryDefense, FuryDefenseGuid)
              .SetDisplayName(FuryDefenseDisplayName)
              .SetDescription(FuryDefenseDescription)
              .SetIcon(icon)
              .AddComponent<DragonFuryDefense>()
              .SetRanks(10)
              .Configure();
        }

        private const string ViciousFang = "ViciousFang";
        public static readonly string ViciousFangGuid = "{7737F897-1EDD-4631-9101-26AE64974743}";

        internal const string ViciousFangDisplayName = "ViciousFang.Name";
        private const string ViciousFangDescription = "ViciousFang.Description";
        public static BlueprintFeature ViciousFangFeature()
        {
            var icon = FeatureRefs.ShiftersEdgeFeature.Reference.Get().Icon;
            return FeatureConfigurator.New(ViciousFang, ViciousFangGuid)
              .SetDisplayName(ViciousFangDisplayName)
              .SetDescription(ViciousFangDescription)
              .SetIcon(icon)
              .Configure();
        }

        private static readonly string DragonWarDanceName = "DragonWarDance";
        public static readonly string DragonWarDanceGuid = "{A2466A39-0FF5-4198-BBD2-42C9EAEC2E0B}";

        private static readonly string DragonWarDanceDisplayName = "DragonWarDance.Name";
        private static readonly string DragonWarDanceDescription = "DragonWarDance.Description";

        private const string WarDanceAbility = "DragonWarDance.WarDanceAbility";
        private static readonly string WarDanceAbilityGuid = "{D1A0ECB5-E753-4AB2-9543-9BDCF395B5D4}";

        private const string WarDanceBuff = "DragonWarDance.WarDanceBuff";
        private static readonly string WarDanceBuffGuid = "{899AAA9A-0E9E-4331-86DB-97C6E41C9104}";
        public static BlueprintFeature DragonWarDanceConfigure()
        {
            var icon = FeatureRefs.ShifterDragonFormFeature.Reference.Get().Icon;

            var buff = BuffConfigurator.New(WarDanceBuff, WarDanceBuffGuid)
                .SetDisplayName(DragonWarDanceDisplayName)
                .SetDescription(DragonWarDanceDescription)
                .SetIcon(icon)
                .AddPartialDRIgnore(checkWeaponCategories: false, reductionReduction: 1000, useContextValue: false)
                .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().ApplyBuffPermanent(BuffRefs.Bleed2d6Buff.ToString()).Build(), false, onlyHit: true)
                .Configure();

            var ability = AbilityConfigurator.New(WarDanceAbility, WarDanceAbilityGuid)
                .SetDisplayName(DragonWarDanceDisplayName)
                .SetDescription(DragonWarDanceDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(OuterStanceBuffGuid).RemoveBuff(BendWindBuffGuid).ApplyBuffPermanent(buff).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(DragonWarDanceName, DragonWarDanceGuid)
                    .SetDisplayName(DragonWarDanceDisplayName)
                    .SetDescription(DragonWarDanceDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string BendwithWindName = "BendwithWind";
        public static readonly string BendwithWindGuid = "{05E46EA6-F494-4133-902C-3031E3235A93}";

        private static readonly string BendwithWindDisplayName = "BendwithWind.Name";
        private static readonly string BendwithWindDescription = "BendwithWind.Description";

        private const string BendWindAbility = "BendwithWind.BendWindAbility";
        private static readonly string BendWindAbilityGuid = "{D3A13A05-D86E-4801-BDDE-4E473B5B7DAF}";

        private const string BendWindBuff = "BendwithWind.BendWindBuff";
        private static readonly string BendWindBuffGuid = "{77915AA7-1B48-4E2E-A4B0-9806B80AB807}";
        public static BlueprintFeature BendwithWindConfigure()
        {
            var icon = AbilityRefs.FeatherStep.Reference.Get().Icon;

            var buff = BuffConfigurator.New(BendWindBuff, BendWindBuffGuid)
                .SetDisplayName(BendwithWindDisplayName)
                .SetDescription(BendwithWindDescription)
                .SetIcon(icon)
                .AddComponent<DFbindwind>()
                .Configure();

            var ability = AbilityConfigurator.New(BendWindAbility, BendWindAbilityGuid)
                .SetDisplayName(BendwithWindDisplayName)
                .SetDescription(BendwithWindDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(OuterStanceBuffGuid).RemoveBuff(WarDanceBuffGuid).ApplyBuffPermanent(buff).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(BendwithWindName, BendwithWindGuid)
                    .SetDisplayName(BendwithWindDisplayName)
                    .SetDescription(BendwithWindDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability })
                    .Configure();
        }

        private static readonly string OuterSphereStanceName = "OuterSphereStance";
        public static readonly string OuterSphereStanceGuid = "{E7D3DD28-1714-4083-A7C4-43A7427C9D55}";

        private static readonly string OuterSphereStanceDisplayName = "OuterSphereStance.Name";
        private static readonly string OuterSphereStanceDescription = "OuterSphereStance.Description";

        private const string OuterStanceAbility = "OuterSphereStance.OuterStanceAbility";
        private static readonly string OuterStanceAbilityGuid = "{01B9D8F0-563E-4431-AF98-6F84EA9C7B92}";

        private const string OuterStanceBuff = "OuterSphereStance.OuterStanceBuff";
        private static readonly string OuterStanceBuffGuid = "{F1182AFE-66DC-4FA5-83AD-018D958DFC92}";

        private static readonly string InnerSphereStanceDisplayName = "InnerSphereStance.Name";
        private static readonly string InnerSphereStanceDescription = "InnerSphereStance.Description";

        private const string InnerStanceAbility = "InnerSphereStance.InnerStanceAbility";
        private static readonly string InnerStanceAbilityGuid = "{38C39F01-EB3E-4FF9-A318-5D76E5D2BB29}";
        public static BlueprintFeature OuterSphereStanceConfigure()
        {
            var icon = FeatureSelectionRefs.SlayerTalentSelection2.Reference.Get().Icon;
            var icon2 = AbilityRefs.WildShapeTurnBackAbility.Reference.Get().Icon;

            var buff = BuffConfigurator.New(OuterStanceBuff, OuterStanceBuffGuid)
                .SetDisplayName(OuterSphereStanceDisplayName)
                .SetDescription(OuterSphereStanceDescription)
                .SetIcon(icon)
                .AddStatBonus(ModifierDescriptor.Penalty, false, StatType.AC, -2)
                .AddComponent<ScarVicious>(c => { c.num = 1; c.checkBuff = false; })
                .Configure();

            var ability = AbilityConfigurator.New(OuterStanceAbility, OuterStanceAbilityGuid)
                .SetDisplayName(OuterSphereStanceDisplayName)
                .SetDescription(OuterSphereStanceDescription)
                .SetIcon(icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(BendWindBuffGuid).RemoveBuff(WarDanceBuffGuid).ApplyBuffPermanent(buff).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            var ability2 = AbilityConfigurator.New(InnerStanceAbility, InnerStanceAbilityGuid)
                .SetDisplayName(InnerSphereStanceDisplayName)
                .SetDescription(InnerSphereStanceDescription)
                .SetIcon(icon2)
                .AddAbilityEffectRunAction(ActionsBuilder.New().RemoveBuff(OuterStanceBuffGuid).RemoveBuff(BendWindBuffGuid).RemoveBuff(WarDanceBuffGuid).Build())
                .SetType(AbilityType.Physical)
                .SetRange(AbilityRange.Personal)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .Configure();

            return FeatureConfigurator.New(OuterSphereStanceName, OuterSphereStanceGuid)
                    .SetDisplayName(OuterSphereStanceDisplayName)
                    .SetDescription(OuterSphereStanceDescription)
                    .SetIcon(icon)
                    .AddFacts(new() { ability, ability2 })
                    .Configure();
        }
    }
}
